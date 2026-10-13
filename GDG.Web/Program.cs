using GDG.Application.Interfaces;
using GDG.Web.Components;
using GDG.Web.Services;
using GDG.Infrastructure.Contexts;
using GDG.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Antiforgery;
using System.Threading.RateLimiting;
using GDG.Application.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Ensure static web assets (app.css, GDG.Web.styles.css, bootstrap, etc.) are always loaded
builder.WebHost.UseStaticWebAssets();

// Connection String Kontrolü:
// Null, boş veya sadece boşluk karakterlerinden oluşan connection string kesin olarak reddedilir.
var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(rawConnectionString))
{
    throw new InvalidOperationException(
        "Veritabanı bağlantı dizesi ('ConnectionStrings:DefaultConnection') tanımlanmamış veya boş. " +
        "Geliştirme ortamında 'dotnet user-secrets', canlı ortamda ise 'ConnectionStrings__DefaultConnection' " +
        "ortam değişkeni ile geçerli bir Supabase PostgreSQL bağlantı dizesi tanımlayınız.");
}

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(rawConnectionString));

// ASP.NET Core Identity Altyapısı
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Güçlendirilmiş parola politikası
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 4;

    // Hesap kilitleme: 5 başarısız denemede 15 dakika kilitle
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.LoginPath = "/admin/login";
    options.AccessDeniedPath = "/admin/login?error=unauthorized";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAntiforgery();

// Rate limits operate at the HTTP endpoint level. Contact submissions use a tighter
// per-client limit than admin login attempts.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (rejection, cancellationToken) =>
    {
        var httpContext = rejection.HttpContext;
        if (string.Equals(httpContext.Request.Path.Value, "/api/contact", StringComparison.OrdinalIgnoreCase))
        {
            httpContext.Response.Redirect("/contact?status=rate_limited");
            return;
        }

        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await httpContext.Response.WriteAsync("Çok fazla istek gönderildi. Lütfen daha sonra tekrar deneyin.", cancellationToken);
    };

    options.AddPolicy("auth", httpContext => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: GetClientRateLimitKey(httpContext),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(5),
            QueueLimit = 0,
            AutoReplenishment = true
        }));

    options.AddPolicy("contact", httpContext => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: GetClientRateLimitKey(httpContext),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(10),
            QueueLimit = 0,
            AutoReplenishment = true
        }));
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Uygulama & Altyapı Servisleri (PostgreSQL / EF Core)
builder.Services.AddScoped<ITechnologyService, TechnologyService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISiteSettingsService, SiteSettingsService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddHttpClient<SupabaseStorageService>();

var forwardedHeadersEnabled =
    builder.Configuration.GetValue<bool>("ForwardedHeaders:Enabled");

if (forwardedHeadersEnabled)
{
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor |
            ForwardedHeaders.XForwardedProto;

        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
        options.ForwardLimit = 1;
    });
}

var app = builder.Build();

// Otomatik Güvenli Admin Bootstrap Kontrolü
// Yalnızca yapılandırmada AdminBootstrap__Email ve AdminBootstrap__Password mevcutsa çalışır.
try
{
    await AdminBootstrapService.EnsureAdminBootstrapAsync(app.Services, app.Configuration);
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Admin bootstrap yapılandırılmış ancak tamamlanamadı. Uygulama güvenli bir yönetici hesabı oluşturulmadan başlatılmayacak.");
    throw;
}
if (forwardedHeadersEnabled)
{
    app.UseForwardedHeaders();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// ======================== AUTH HTTP ENDPOINTS ========================
// These endpoints validate antiforgery tokens explicitly because they read form values
// manually rather than binding an antiforgery-enabled [FromForm] model.
app.MapPost("/api/auth/login", LoginAsync)
    .WithMetadata(new Microsoft.AspNetCore.Mvc.RequestSizeLimitAttribute(16 * 1024))
    .RequireRateLimiting("auth");
app.MapPost("/api/auth/logout", LogoutAsync)
    .WithMetadata(new Microsoft.AspNetCore.Mvc.RequestSizeLimitAttribute(16 * 1024));
app.MapPost("/api/contact", ContactSubmitAsync)
    .WithMetadata(new Microsoft.AspNetCore.Mvc.RequestSizeLimitAttribute(16 * 1024))
    .RequireRateLimiting("contact");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static string GetClientRateLimitKey(HttpContext context) =>
    context.Connection.RemoteIpAddress?.ToString() ?? "unknown-client";

static async Task<IResult> LoginAsync(
    HttpContext context,
    IAntiforgery antiforgery,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager)
{
    try
    {
        await antiforgery.ValidateRequestAsync(context);
    }
    catch (AntiforgeryValidationException)
    {
        return Results.BadRequest("İstek doğrulanamadı. Sayfayı yenileyip tekrar deneyin.");
    }

    if (!context.Request.HasFormContentType)
        return Results.BadRequest("Geçersiz istek türü.");

    var form = await context.Request.ReadFormAsync();
    var email = form["email"].ToString();
    var password = form["password"].ToString();
    var returnUrl = form["returnUrl"].ToString();

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return Results.Redirect("/admin/login?error=empty");

    var user = await userManager.FindByEmailAsync(email.Trim());
    if (user == null || await userManager.IsLockedOutAsync(user))
    {
        // Keep login errors generic so account existence and lockout state are not disclosed.
        return Results.Redirect("/admin/login?error=invalid_credentials");
    }

    var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
    if (!isAdmin)
        return Results.Redirect("/admin/login?error=invalid_credentials");

    var result = await signInManager.PasswordSignInAsync(
        user, password, isPersistent: false, lockoutOnFailure: true);

    if (!result.Succeeded)
    {
        return Results.Redirect("/admin/login?error=invalid_credentials");
    }

    return Results.Redirect(UrlSafety.IsLocalUrl(returnUrl) ? returnUrl : "/admin");
}

static async Task<IResult> LogoutAsync(HttpContext context, IAntiforgery antiforgery,
    SignInManager<IdentityUser> signInManager)
{
    try
    {
        await antiforgery.ValidateRequestAsync(context);
    }
    catch (AntiforgeryValidationException)
    {
        return Results.BadRequest("İstek doğrulanamadı. Sayfayı yenileyip tekrar deneyin.");
    }

    await signInManager.SignOutAsync();
    return Results.Redirect("/admin/login");
}

static async Task<IResult> ContactSubmitAsync(HttpContext context, IAntiforgery antiforgery,
    IContactService contactService)
{
    try
    {
        await antiforgery.ValidateRequestAsync(context);
    }
    catch (AntiforgeryValidationException)
    {
        return Results.BadRequest("Form doğrulanamadı. Lütfen sayfayı yenileyip tekrar deneyin.");
    }

    if (!context.Request.HasFormContentType)
        return Results.BadRequest("Geçersiz istek türü.");

    var form = await context.Request.ReadFormAsync();

    // Honeypot field: quietly discard likely automated submissions.
    if (!string.IsNullOrWhiteSpace(form["companyWebsite"].ToString()))
        return Results.Redirect("/contact?status=success");

    var dto = new CreateContactMessageDto
    {
        FullName = form["FullName"].ToString(),
        Email = form["Email"].ToString(),
        Subject = form["Subject"].ToString(),
        Message = form["Message"].ToString()
    };

    if (!ContactMessageValidator.TryValidate(dto, out _))
        return Results.Redirect("/contact?status=invalid");

    var saved = await contactService.SubmitMessageAsync(dto);
    return Results.Redirect(saved ? "/contact?status=success" : "/contact?status=error");
}

using GDG.Application.Interfaces;
using GDG.Web.Components;
using GDG.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Ensure static web assets (app.css, GDG.Web.styles.css, bootstrap, etc.) are always loaded
builder.WebHost.UseStaticWebAssets();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Core Admin Panel Services
builder.Services.AddScoped<ITechnologyService, TechnologyService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IEventService, EventService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

using GDG.Infrastructure.Contexts;
using GDG.Web.Components;
using GDG.Application.Interfaces;
using GDG.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Core Panel Services (Project, Technology, Member, Team, Event)
builder.Services.AddSingleton<ITechnologyService, TechnologyService>();
builder.Services.AddSingleton<IProjectService, ProjectService>();
builder.Services.AddSingleton<IMemberService, MemberService>();
builder.Services.AddSingleton<ITeamService, TeamService>();
builder.Services.AddSingleton<IEventService, EventService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

using Blazored.LocalStorage;
using McDermott.Application.Extentions;
using McDermott.Persistence.Extensions;
using McDermott.Web.Authentication;
using McDermott.Web.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationLayer();
DevExpress.Blazor.CompatibilitySettings.AddSpaceAroundFormLayoutContent = true;
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);
builder.Services.AddBlazoredLocalStorage();
builder.Services.Configure<IdentityOptions>(x =>
{
    //x.Lockout.
});

builder.Services.AddPersistenceLayer(builder.Configuration);

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
using McDermott.Persistence.Context;
using McDermott.Persistence.Extensions;
using McDermott.Web.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddPersistenceLayer(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromSeconds(10);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/access-denied";
        options.Cookie.HttpOnly = true; // Pastikan ini diset ke false
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;

        // Event untuk memvalidasi sesi dan logout jika token kadaluarsa
        options.Events.OnValidatePrincipal = async context =>
        {
            // Dapatkan claim `TokenExpiration`
            var expirationClaim = context.Principal?.FindFirst("TokenExpiration")?.Value;

            if (expirationClaim != null && DateTime.TryParse(expirationClaim, out var expirationTime))
            {
                // Cek apakah token sudah kadaluarsa
                if (DateTime.UtcNow > expirationTime)
                {
                    // Jika kadaluarsa, tolak sesi dan logout pengguna
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        };
    });

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Migrate the database automatically on startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

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
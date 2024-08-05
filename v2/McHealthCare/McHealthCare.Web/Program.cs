using Blazored.Toast;
using McHealthCare.Application.Extentions;
using McHealthCare.Application.Services;
using McHealthCare.Context;
using McHealthCare.Domain.Entities;
using McHealthCare.Persistence.Extentions;
using McHealthCare.Web.Components;
using McHealthCare.Web.Components.Account;
using McHealthCare.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;

DevExpress.Blazor.CompatibilitySettings.AddSpaceAroundFormLayoutContent = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

builder.Services.AddPersistenceLayer(builder.Configuration);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.PayloadSerializerOptions.WriteIndented = true; // optional
    });

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IFileExportService, FileExportService>();
builder.Services.AddApplicationLayer();

builder.Services.AddBlazoredToast();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

//builder.Services.AddScoped<SignInManager<IdentityUser>>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
//    options.SignIn.RequireConfirmedAccount = false;
//})
//      .AddEntityFrameworkStores<ApplicationDbContext>()
//      .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddCors();

var app = builder.Build();

//app.UseMiddleware<SessionExpirationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();
app.UseStaticFiles();

// Konfigurasi middleware lainnya
app.UseRouting();

app.UseAuthentication(); // Tambahkan jika menggunakan autentikasi
app.UseAuthorization();

app.UseMiddleware<PageTrackingMiddleware>();


app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<NotificationHub>("notificationHub");

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Console.WriteLine("=== ===");
        Console.WriteLine("=== Starting Migrate the database. ===");
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error occurred while migrating the database.");
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.InnerException?.Message ?? "");
        Console.WriteLine(ex.Source ?? "");
        Console.WriteLine(ex.StackTrace ?? "");
    }
    finally
    {
        Console.WriteLine("=== ===");
    }
}

app.Run();


public class PageTrackingMiddleware(RequestDelegate _next, IServiceProvider _serviceProvider)
{
    public async Task InvokeAsync(HttpContext context)
    { 
        if (context.Request.Path.StartsWithSegments("/forbidden"))
        {
            await _next(context);
            return;
        }

        using (var scope = _serviceProvider.CreateScope())
        {
            //var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //var id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id) ?? new();
            //var groupMenus = await dbContext.GroupMenus.Include(x => x.Menu).Where(x => x.GroupId == user.GroupId).ToListAsync();  
            //var aa = context.Request.Path.ToString().TrimStart('/');
            //var bb = groupMenus.FirstOrDefault(x => x.Menu.Url.Contains(aa)) ;
            //if (bb is null && !aa.Equals("/"))
            //{
            //    context.Response.Redirect("/forbidden");
            //       return;
            //}
            //if (!isValidUser)
            //{
            //    context.Response.Redirect("/forbidden");  
            //    return;
            //}
        }
         
        await _next(context);
    }
}
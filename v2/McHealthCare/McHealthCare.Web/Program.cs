using Blazored.Toast;
using McHealthCare.Application.Extentions;
using McHealthCare.Application.Services;
using McHealthCare.Context;
using McHealthCare.Domain.Entities;
using McHealthCare.Domain.Entities.Configuration;
using McHealthCare.Domain.Entities.Medical;
using McHealthCare.Persistence.Extentions;
using McHealthCare.Web.Components;
using McHealthCare.Web.Components.Account;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using McHealthCare.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.ResponseCompression;

DevExpress.Blazor.CompatibilitySettings.AddSpaceAroundFormLayoutContent = true;

var builder = WebApplication.CreateBuilder(args);

//TypeAdapterConfig<ApplicationUser, ApplicationUserDto>
//    .NewConfig()
//    .PreserveReference(true) // Menghindari pemetaan rekursif
//    .Ignore(dest => dest.Employee.ApplicationUser) // Mengabaikan properti yang berpotensi rekursif
//    .Ignore(dest => dest.Doctor)
//    .Ignore(dest => dest.Patient.ApplicationUser);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // Aktifkan kompresi untuk HTTPS
    options.Providers.Add<GzipCompressionProvider>(); // Tambahkan provider kompresi Gzip
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" }); // Tambahkan tipe MIME tambahan jika perlu
});

// Konfigurasi tingkat kompresi (opsional)
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.SmallestSize; // Atur tingkat kompresi
});

builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

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

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>() // Add role support
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager<SignInManager<ApplicationUser>>()
.AddRoleManager<RoleManager<IdentityRole>>() // Register RoleManager
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

//app.UsePathBase("/v2");
app.UseResponseCompression();

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

//app.UseMiddleware<PageTrackingMiddleware>();

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
        Console.WriteLine("=== Success Migrated ===");

        Console.WriteLine("=== Starting Seeding the data. ===");
        await new SeedData().Initialize(services);
        Console.WriteLine("=== Success Seeding ===");
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

public class SeedData
{
    public async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
        // Periksa apakah ada pengguna admin
        if (!context.Users.Any())
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Buat peran Admin jika belum ada
            if (!await roleManager.RoleExistsAsync(EnumRole.Admin.GetDisplayName()))
            {
                await roleManager.CreateAsync(new IdentityRole(EnumRole.Admin.GetDisplayName()));
            }
            if (!await roleManager.RoleExistsAsync(EnumRole.Practitioner.GetDisplayName()))
            {
                await roleManager.CreateAsync(new IdentityRole(EnumRole.Practitioner.GetDisplayName()));
            }

            if (!await roleManager.RoleExistsAsync(EnumRole.Patient.GetDisplayName()))
            {
                await roleManager.CreateAsync(new IdentityRole(EnumRole.Patient.GetDisplayName()));
            }

            if (!await roleManager.RoleExistsAsync(EnumRole.Pharmacy.GetDisplayName()))
            {
                await roleManager.CreateAsync(new IdentityRole(EnumRole.Pharmacy.GetDisplayName()));
            }

            if (!await roleManager.RoleExistsAsync(EnumRole.MCU.GetDisplayName()))
            {
                await roleManager.CreateAsync(new IdentityRole(EnumRole.MCU.GetDisplayName()));
            }

            if (!await roleManager.RoleExistsAsync(EnumRole.HR.GetDisplayName()))
            {
                await roleManager.CreateAsync(new IdentityRole(EnumRole.HR.GetDisplayName()));
            }

            if (!await roleManager.RoleExistsAsync(EnumRole.Employee.GetDisplayName()))
            {
                await roleManager.CreateAsync(new IdentityRole(EnumRole.Employee.GetDisplayName()));
            }

            if (!await roleManager.RoleExistsAsync(EnumRole.User.GetDisplayName()))
            {
                await roleManager.CreateAsync(new IdentityRole(EnumRole.User.GetDisplayName()));
            }

            if (!await context.Groups.AnyAsync(x => x.Name == EnumRole.Admin.GetDisplayName()))
            {
                await context.Groups.AddAsync(new Group
                {
                    Name = EnumRole.Admin.GetDisplayName(),
                    IsDefaultData = true
                });

                await context.SaveChangesAsync();
            }

            Guid groupId = Guid.Empty;
            Guid userMenuId = Guid.Empty;
            Guid groupGroupId = Guid.Empty;
            Guid groupMenuId = Guid.Empty;

            if (!await context.Menus.AnyAsync(x => x.Name == "Configuration"))
            {
                var config = await context.Menus.AddAsync(new Menu
                {
                    Name = "Configuration",
                    Sequence = 10,
                    IsDefaultData = true
                });

                await context.SaveChangesAsync();

                groupId = (await context.Groups.FirstOrDefaultAsync(x => x.Name == "Admin")!)!.Id;

                var user = await context.Menus.AddAsync(new Menu
                {
                    Name = "Users",
                    ParentId = config.Entity.Id,
                    Sequence = 1,
                    Url = "configuration/users",
                    IsDefaultData = true
                });

                var group = await context.Menus.AddAsync(new Menu
                {
                    Name = "Groups",
                    ParentId = config.Entity.Id,
                    Sequence = 2,
                    Url = "configuration/groups",
                    IsDefaultData = true
                });

                var menu = await context.Menus.AddAsync(new Menu
                {
                    Name = "Menus",
                    ParentId = config.Entity.Id,
                    Sequence = 3,
                    Url = "configuration/menus",
                    IsDefaultData = true
                });

                await context.SaveChangesAsync();

                userMenuId = user.Entity.Id;
                groupGroupId = group.Entity.Id;
                groupMenuId = menu.Entity.Id;
            }

            if (userMenuId != Guid.Empty && !await context.GroupMenus.AnyAsync(x => x.GroupId == groupId && x.MenuId == userMenuId))
            {
                await context.GroupMenus.AddAsync(new GroupMenu
                {
                    GroupId = groupId,
                    MenuId = userMenuId,
                    IsRead = true,
                    IsCreate = true,
                    IsUpdate = true,
                    IsDelete = true,
                    IsImport = true,
                    IsDefaultData = true
                });
            }

            if (groupGroupId != Guid.Empty && !await context.GroupMenus.AnyAsync(x => x.GroupId == groupId && x.MenuId == groupGroupId))
            {
                await context.GroupMenus.AddAsync(new GroupMenu
                {
                    GroupId = groupId,
                    MenuId = groupGroupId,
                    IsRead = true,
                    IsCreate = true,
                    IsUpdate = true,
                    IsDelete = true,
                    IsImport = true,
                    IsDefaultData = true
                });
            }

            if (groupMenuId != Guid.Empty && !await context.GroupMenus.AnyAsync(x => x.GroupId == groupId && x.MenuId == groupMenuId))
            {
                await context.GroupMenus.AddAsync(new GroupMenu
                {
                    GroupId = groupId,
                    MenuId = groupMenuId,
                    IsRead = true,
                    IsCreate = true,
                    IsUpdate = true,
                    IsDelete = true,
                    IsImport = true,
                    IsDefaultData = true
                });
            }

            await context.SaveChangesAsync();

            if (groupId == Guid.Empty)
                groupId = (await context.Groups.FirstOrDefaultAsync(x => x.Name == "Admin")!)!.Id;

            var adminUser = new ApplicationUser
            {
                Name = "Administrator",
                NoId = "3671052024",
                UserName = "admin@example.com",
                GroupId = groupId,
                Email = "admin@example.com",
                EmailConfirmed = true,
                IsDefaultData = true
            };

            var result = await userManager.CreateAsync(adminUser, "P@ssw0rd1123");
            if (result.Succeeded)
            {
                var roles = Enum.GetValues(typeof(EnumRole)).Cast<EnumRole>();
                foreach (var role in roles)
                {
                    try
                    {
                        await userManager.AddToRoleAsync(adminUser, role.GetDisplayName());
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
    }
}
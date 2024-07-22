
using DinkToPdf.Contracts;
using DinkToPdf;
using McDermott.Application.Extentions;
using McDermott.Application.Features.Services;
using McDermott.Application.Interfaces.Repositories;
using McDermott.Persistence.Context;
using McDermott.Persistence.Extensions;
using McDermott.Web.Components;
using McDermott.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Serilog;

DevExpress.Blazor.CompatibilitySettings.AddSpaceAroundFormLayoutContent = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.UseUrls("http://*:5001");
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationLayer();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPCareService, PCareService>();
builder.Services.AddScoped<IDocumentProvider, DocumentProvider>();
builder.Services.AddScoped<IFileExportService, FileExportService>(); 
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddMemoryCache(); // Menambahkan layanan memory cache
builder.Services.AddDistributedMemoryCache(); // Menambahkan layanan distributed memory cache (opsional, digunakan jika Anda memerlukan cache di beberapa instance server)
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();

builder.Services.AddScoped<UserInfoService>();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = "CustomAuth";
//    options.DefaultScheme = "CustomAuth";
//    // Hapus baris berikut karena Anda tidak menggunakan skema tantangan
//    // options.DefaultChallengeScheme = "CustomAuth";
//}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie("CustomAuth", options =>
//{
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//    options.SlidingExpiration = true;
//    options.AccessDeniedPath = "/Home/Forbidden";
//    options.LoginPath = "/Identity/Account/Login";
//});
//builder.Services.AddAuthorization();

builder.Services.AddPersistenceLayer(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<AuthorizationMiddleware>();

app.UseSerilogRequestLogging();
app.UseRouting();
//app.UseAuthentication(); // Gunakan autentikasi
//app.UseAuthorization();  // Gunakan otorisasi
//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapHub<RealTimeHub>("/realTimeHub");

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Log.Information("=== ===");
        Log.Information("=== Starting Migrate the database. ===");
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error("Error occurred while migrating the database.");
        Log.Error(ex.Message);
        Log.Error(ex.InnerException?.Message ?? "");
        Log.Error(ex.Source ?? "");
        Log.Error(ex.StackTrace ?? "");
    }
    finally
    {
        Log.Information("=== ===");
    }
}


app.Run();
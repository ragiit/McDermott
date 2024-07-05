using McDermott.Application.Extentions;
using McDermott.Persistence.Extensions;
using McDermott.Web.Components;
using Serilog;
using McDermott.Web.Hubs;
using McDermott.Application.Interfaces.Repositories;
using McDermott.Application.Features.Queries;
using Microsoft.AspNetCore.Authorization;
using McDermott.Persistence.Context;

DevExpress.Blazor.CompatibilitySettings.AddSpaceAroundFormLayoutContent = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationLayer();
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
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapHub<RealTimeHub>("/realTimeHub");

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

//// Tambahkan migrasi otomatis di sini
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        var context = services.GetRequiredService<ApplicationDbContext>(); // Ganti dengan nama DbContext Anda
//        context.Database.Migrate();
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while migrating the database.");
//    }
//}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>(); // Ganti dengan nama DbContext Anda
        //context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}


app.Run();
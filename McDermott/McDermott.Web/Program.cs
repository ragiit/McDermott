
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
using Microsoft.AspNetCore.ResponseCompression;

DevExpress.Blazor.CompatibilitySettings.AddSpaceAroundFormLayoutContent = true;

var builder = WebApplication.CreateBuilder(args);
// Tambahkan layanan kompresi respons
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // Aktifkan kompresi untuk HTTPS
    options.Providers.Add<GzipCompressionProvider>(); // Tambahkan provider kompresi Gzip
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]); // Tambahkan tipe MIME tambahan jika perlu
});

// Konfigurasi tingkat kompresi (opsional)
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.SmallestSize; // Atur tingkat kompresi
});
//builder.WebHost.ConfigureKestrel((context, options) =>
//{
//    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
//    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
//    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
//});
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers(); // Menambahkan layanan Controllers
builder.Services.AddControllersWithViews();
// Configure HttpClient with BaseAddress
//builder.Services.AddHttpClient("ServerAPI", client =>
//{
//    // Replace "https://localhost:5001/" with your actual base URL
//    client.BaseAddress = new Uri("https://localhost:5001/");
//    // Or you can use a configuration setting
//    // client.BaseAddress = new Uri(builder.Configuration["ServerAPI:BaseUrl"]);
//});
builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServerAPI:BaseUrl"] ?? "http://localhost:5001/");
}); 
// Add services to the container.
//builder.WebHost.UseUrls("http://*:5001");
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationLayer();
builder.Services.AddRazorPages();
builder.Services.AddAntiforgery();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always; 
});

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
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

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

// Gunakan middleware kompresi respons
app.UseResponseCompression();

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

//app.UsePathBase("/McDermott");
//app.UseMiddleware<AuthorizationMiddleware>();

app.UseSerilogRequestLogging();
app.UseRouting();
//app.UseAuthentication(); // Gunakan autentikasi
//app.UseAuthorization();  // Gunakan otorisasi
//app.UseHttpsRedirection();

app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapHub<RealTimeHub>("/realTimeHub");
//app.UseMiddleware<RateLimitMiddleware>();

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

//app.UseMiddleware<RequestTimeoutMiddleware>();
//app.Use(async (context, next) =>
//{
//    context.RequestAborted.WaitHandle.WaitOne(TimeSpan.FromMinutes(2)); // Set timeout
//    await next.Invoke();
//});

app.Run();

public class RateLimitMiddleware
{
    private static Dictionary<string, DateTime> requestTimes = new Dictionary<string, DateTime>();
    private static TimeSpan limitPeriod = TimeSpan.FromSeconds(5); // Time period to check requests

    private readonly RequestDelegate _next;

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress.ToString();

        if (requestTimes.ContainsKey(ipAddress))
        {
            var lastRequestTime = requestTimes[ipAddress];
            if (DateTime.UtcNow < lastRequestTime.Add(limitPeriod))
            {
                context.Response.StatusCode = 429; // Too Many Requests
                return;
            }
        }

        requestTimes[ipAddress] = DateTime.UtcNow;
        await _next(context);
    }
}

public class RequestTimeoutMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TimeSpan _timeout;

    public RequestTimeoutMiddleware(RequestDelegate next, TimeSpan timeout)
    {
        _next = next;
        _timeout = timeout;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var cts = new CancellationTokenSource(_timeout))
        {
            context.RequestAborted.Register(() => cts.Cancel());

            try
            {
                await _next(context);
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested)
            {
                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                await context.Response.WriteAsync("Request timed out.");
            }
        }
    }
}

public static class RequestTimeoutMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTimeout(this IApplicationBuilder builder, TimeSpan timeout)
    {
        return builder.UseMiddleware<RequestTimeoutMiddleware>(timeout);
    }
}

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
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Antiforgery;
using McDermott.Web;

DevExpress.Blazor.CompatibilitySettings.AddSpaceAroundFormLayoutContent = true;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization()
    .AddQueryableCursorPagingProvider()
    .AddQueryableOffsetPagingProvider();

builder.Services.AddAntiforgery();

// Tambahkan layanan kompresi respons
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // Aktifkan kompresi untuk HTTPS
    options.Providers.Add<GzipCompressionProvider>(); // Tambahkan provider kompresi Gzip
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]); // Tambahkan tipe MIME tambahan jika perlu
});

builder.Services.AddScoped<GoogleMeetService>();
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
//builder.Services.AddInMemoryRateLimiting();
builder.Services.AddOptions();
// Add rate limiting services
//builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
//builder.Services.Configure<RateLimitOptions>(builder.Configuration.GetSection("RateLimiting"));
//builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
// Add rate limiting processing strategy
//builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddHttpClient("GraphQLClient", client =>
{
    var a = new Uri(builder.Configuration["GraphQLServer"]);
    client.BaseAddress = a;
});

builder.Services.AddWebOptimizer(pipeline =>
{
    // Bundle and minify CSS
    pipeline.AddCssBundle("/css/site.min.css",
        "css/switcher-resources/themes/lumen/bootstrap.min.css",
        "css/my-style.css",
        "AdminLTE/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
        "AdminLTE/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
        "AdminLTE/plugins/jqvmap/jqvmap.min.css",
        "AdminLTE/dist/css/adminlte.min.css",
        "AdminLTE/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
        "AdminLTE/plugins/daterangepicker/daterangepicker.css",
        "AdminLTE/plugins/summernote/summernote-bs4.min.css",
        "fontawesome/css/all.css",
        "_content/DevExpress.Blazor.Themes/bootstrap-external.bs5.min.css");

    //"https://unpkg.com/jspdf@latest/dist/jspdf.umd.min.js",

    pipeline.AddJavaScriptBundle("/js/site.min.js",
              "_content/Blazored.TextEditor/quill-blot-formatter.min.js",
              "_content/Blazored.TextEditor/Blazored-BlazorQuill.js",
              "AdminLTE/plugins/jquery/jquery.min.js",
              "AdminLTE/plugins/jquery-ui/jquery-ui.min.js",
              "AdminLTE/plugins/bootstrap/js/bootstrap.bundle.min.js",
              "AdminLTE/plugins/chart.js/Chart.min.js",
              "AdminLTE/plugins/sparklines/sparkline.js",
              "AdminLTE/plugins/jqvmap/jquery.vmap.min.js",
              "AdminLTE/plugins/jqvmap/maps/jquery.vmap.usa.js",
              "AdminLTE/plugins/jquery-knob/jquery.knob.min.js",
              "AdminLTE/plugins/moment/moment.min.js",
              "AdminLTE/plugins/daterangepicker/daterangepicker.js",
              "AdminLTE/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
              "AdminLTE/plugins/summernote/summernote-bs4.min.js",
              "AdminLTE/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
              "AdminLTE/dist/js/adminlte.js",
              //"canvasScript.js",
              "js/jspdf.umd.min.js",
              "js/quill.js",
              "js/my-js.js",
              "CetakEtiket.js");
});
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // Mengaktifkan kompresi untuk HTTPS
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/javascript", "text/css", "application/json" });
});

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
//builder.Services.AddAntiforgery();
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
builder.Services.AddScoped<ITestDataVillageService, TestDataVillageService>();
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
app.UseAntiforgery();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});
//app.UseIpRateLimiting();
//app.UseAuthentication(); // Gunakan autentikasi
//app.UseAuthorization();  // Gunakan otorisasi
//app.UseHttpsRedirection();

app.MapControllers();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Set the cache headers
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
        ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddYears(1).ToString("R"));
    }
});
//app.UseMiddleware<CsrfTokenCOokieMiddleware>();
app.UseResponseCompression();
app.UseWebOptimizer();
// Tambahkan middleware logging untuk rate limiting
//app.UseMiddleware<RateLimitLoggingMiddleware>();
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

public class RateLimitLoggingMiddleware(RequestDelegate next, ILogger<RateLimitLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var rateLimitExceeded = context.Response.StatusCode == StatusCodes.Status429TooManyRequests;
        if (rateLimitExceeded)
        {
            logger.LogWarning("Rate limit exceeded for {RequestPath}", context.Request.Path);
        }

        await next(context);
    }
}

public class RateLimitMiddleware(RequestDelegate next)
{
    private static Dictionary<string, DateTime> requestTimes = new Dictionary<string, DateTime>();
    private static TimeSpan limitPeriod = TimeSpan.FromSeconds(5); // Time period to check requests

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
        await next(context);
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

internal class CsrfTokenCOokieMiddleware(IAntiforgery antiforgery, RequestDelegate next)
{
    private readonly IAntiforgery _antiforgery = antiforgery;
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Cookies["CSRF-TOKEN"] == null)
        {
            var token = _antiforgery.GetAndStoreTokens(context);
            context.Response.Cookies.Append("CSRF-TOKEN", token.RequestToken, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = false });
        }
        await _next(context);
    }
}
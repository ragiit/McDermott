using App.Metrics;
using App.Metrics.Timer;
using AspNetCoreRateLimit;
using McDermott.Application.Extentions;
using McDermott.Domain.Entities;
using McDermott.Persistence.Extensions;
using McDermott.TelemedicianApi.Middleware;
using McDermott.TelemedicianApi.ViewModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Identity.Client;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using McDermott.TelemedicianApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMediatR(options =>
//{
//    options.RegisterServicesFromAssemblies(typeof(Program).Assembly);
//});

//builder.Services.AddControllers()
//        .AddOData(options =>
//            options.Select().Filter().OrderBy().Expand().Count()
//            .AddRouteComponents("odata", GetEdmModel()));
builder.Services.AddControllers(options =>
{
    //options.Filters.AddService<KeyValidationActionFilter>();
    //options.Filters.AddService<NullActionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; // Semua URL menjadi lowercase
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddOptions();
// Add rate limiting services
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<RateLimitOptions>(builder.Configuration.GetSection("RateLimiting"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
//builder.Services.AddScoped<KeyValidationActionFilter>();
//builder.Services.AddScoped<NullActionFilter>(); // Menambahkan filter kosong ke container DI
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddSingleton<EmailService>();

// Add rate limiting processing strategy
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);

var metrics = new MetricsBuilder()
           .Report.ToTextFile("metrics.txt")
           .Build();

builder.Services.AddMetrics(metrics);
builder.Services.AddMetricsReportingHostedService();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Daftarkan middleware sebelum UseRouting
//app.UseMiddleware<ApiHeaderMiddleware>();
//app.UseMiddleware<SignatureValidationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseStaticFiles();
// Tambahkan middleware logging untuk rate limiting
app.UseMiddleware<RateLimitLoggingMiddleware>();
app.UseMiddleware<MetricsMiddleware>();
//app.UseMiddleware<KeyValidationMiddleware>();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Gunakan pengaturan routing
});
app.UseIpRateLimiting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseResponseCaching();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    //builder.EntitySet<User>("Users");
    return builder.GetEdmModel();
}

public class MetricsMiddleware(RequestDelegate next, IMetrics metrics)
{
    private readonly IMetrics _metrics = metrics;

    public async Task InvokeAsync(HttpContext context)
    {
        var timer = _metrics.Measure.Timer.Time(new TimerOptions
        {
            Name = "Request Duration",
            MeasurementUnit = App.Metrics.Unit.Requests
        });

        await next(context);

        timer.Dispose();
    }
}

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

public class KeyValidationMiddleware
{
    private readonly RequestDelegate _next;

    public KeyValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userKey = context.Request.Headers["user_key"].FirstOrDefault();
        var secretKey = context.Request.Headers["secret_key"].FirstOrDefault();

        // Cek apakah kedua header ada
        if (!string.IsNullOrEmpty(userKey) && !string.IsNullOrEmpty(secretKey))
        {
            if (!LzStringHelper.VerifyKeysFromHeader(userKey, secretKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new ApiResponse<object>(
                    statusCode: StatusCodes.Status401Unauthorized,
                    message: "Invalid user_key or secret_key."));
                return;
            }
        }

        // Lanjutkan request ke middleware berikutnya jika valid atau tidak ada header
        await _next(context);
    }
}

public class KeyValidationActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Ambil nilai header user_key dan secret_key
        var userKey = context.HttpContext.Request.Headers["user_key"].ToString();
        var secretKey = context.HttpContext.Request.Headers["secret_key"].ToString();

        // Verifikasi kunci dengan LzStringHelper
        if (!LzStringHelper.VerifyKeysFromHeader(userKey, secretKey))
        {
            // Jika tidak valid, batalkan eksekusi action dan kembalikan Unauthorized
            context.Result = new UnauthorizedObjectResult(new ApiResponse<object>
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Invalid user_key or secret_key."
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Tidak ada aksi setelah action dijalankan
    }
}

public class NullActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Tidak melakukan apa-apa
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Tidak melakukan apa-apa
    }
}
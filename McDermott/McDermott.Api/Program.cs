using App.Metrics;
using App.Metrics.Timer;
using AspNetCoreRateLimit;
using McDermott.Application.Extentions;
using McDermott.Domain.Entities;
using McDermott.Persistence.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddMediatR(options =>
//{
//    options.RegisterServicesFromAssemblies(typeof(Program).Assembly);
//});

builder.Services.AddControllers()
        .AddOData(options =>
            options.Select().Filter().OrderBy().Expand().Count()
            .AddRouteComponents("odata", GetEdmModel()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Telemedicine API",
        Version = "v1",
        Description = "API untuk layanan Telemedicine yang menyediakan fitur konsultasi jarak jauh."
    });
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

// Add rate limiting processing strategy
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);

var metrics = new MetricsBuilder()
           .Report.ToTextFile("metrics.txt")
           .Build();

builder.Services.AddMetrics(metrics);
builder.Services.AddMetricsReportingHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Telemedicine API v1");
    options.RoutePrefix = string.Empty; // Swagger UI di root
});

app.UseStaticFiles();
// Tambahkan middleware logging untuk rate limiting
app.UseMiddleware<RateLimitLoggingMiddleware>();
app.UseMiddleware<MetricsMiddleware>();
app.UseRouting();
app.UseIpRateLimiting();
app.UseResponseCaching();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<User>("Users");
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
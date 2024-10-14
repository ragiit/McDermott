using McDermott.Telemedic.Components;
using McDermott.Telemedic.Service;
using static TelemedicService;

var builder = WebApplication.CreateBuilder(args);
//inject ServiceApiUrl
// Baca konfigurasi dari appsettings.json
var apiConfig = builder.Configuration.GetSection("ServerAPI").Get<APIConfig>();

// Registrasi HttpClient dan IApiService untuk dependency injection
builder.Services.AddHttpClient<ITelemedicService, TelemedicService>(client =>
{
    client.BaseAddress = new Uri(apiConfig.BaseUrl); // Gunakan BaseUrl dari appsettings.json
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

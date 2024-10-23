using McDermott.Telemedic.Model;
using McDermott.Telemedic.Service;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using System.Text.Json;

public class TelemedicService : ITelemedicService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TelemedicService> _logger;
      

    public TelemedicService(HttpClient httpClient, IConfiguration configuration, ILogger<TelemedicService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<TelemedicResult> SearchTelemedicAsync(string number, int serviceId)
    {
        try
        {
            var serviceName = _configuration["ServerAPI:serviceName"];
            var fullUrl = $"{_httpClient.BaseAddress}{serviceName}{number}/{serviceId}";
            _logger.LogInformation($"Sending request to: {fullUrl}");

            var response = await _httpClient.GetAsync(fullUrl);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Received response: {content}");

            var result = JsonSerializer.Deserialize<TelemedicResult>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? throw new InvalidOperationException("Failed to deserialize the response");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP Request failed");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize the response");
            throw new InvalidOperationException("Failed to process the response from the server", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred");
            throw;
        }
    }

    // Metode tambahan untuk mengambil nama pengguna
    public string GetUserName(TelemedicResult result)
    {
        if (result?.Data?.User != null && result.Data.User.Any())
        {
            return result.Data.User[0].Name;
        }
        throw new InvalidOperationException("No user data found in the result");
    }
    public class APIConfig
    {
        public string BaseUrl { get; set; }
        public string ServiceName { get; set; }
        public string ConsId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string KdAplikasi { get; set; }
        public string UserKey { get; set; }
        public string SecretKey { get; set; }
    }
   
}
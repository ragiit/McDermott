namespace McDermott.Application.Features.Services
{
    public interface IPCareService
    {
        Task<(dynamic, int)> SendPCareService(string baseURL, string requestURL, HttpMethod method, object? requestBody = null);
    }
}
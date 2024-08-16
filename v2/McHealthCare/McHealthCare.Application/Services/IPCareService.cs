namespace McHealthCare.Application.Services
{
    public interface IPCareService
    {
        Task<(dynamic, int)> SendPCareService(string requestURL, HttpMethod method, object? requestBody = null);
    }
}

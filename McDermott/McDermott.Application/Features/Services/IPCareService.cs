namespace McDermott.Application.Features.Services
{
    public interface IPCareService
    {
        Task<(string, int)> SendPCareService(string requestURL, HttpMethod method);
    }
}

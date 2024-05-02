namespace McDermott.Application.Features.Services
{
    public interface IPCareService
    {
        Task<(dynamic, int)> SendPCareService(string requestURL, HttpMethod method);
    }
}

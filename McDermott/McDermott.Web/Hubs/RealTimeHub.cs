using Microsoft.AspNetCore.SignalR;

namespace McDermott.Web.Hubs
{
    public class RealTimeHub : Hub
    {
        public async Task SendQueue(int CounterId, int ServerKId, int NoQueue)
        {
            await Clients.All.SendAsync("ReceivedQueue", CounterId, ServerKId, NoQueue);
        }
    }
}

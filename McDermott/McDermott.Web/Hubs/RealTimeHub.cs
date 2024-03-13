using McDermott.Application.Dtos.Queue;
using Microsoft.AspNetCore.SignalR;

namespace McDermott.Web.Hubs
{
    public class RealTimeHub : Hub
    {
        public async Task CallPatient(long Id, long NumberQueue)
        {
            await Clients.All.SendAsync("CallPatient", Id, NumberQueue);
        }

        public async Task Presents(KioskQueueDto queue)
        {
            await Clients.All.SendAsync("Presents", queue);
        }
    }
}

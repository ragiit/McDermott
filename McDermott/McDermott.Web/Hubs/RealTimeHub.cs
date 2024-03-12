using McDermott.Application.Dtos.Queue;
using Microsoft.AspNetCore.SignalR;

namespace McDermott.Web.Hubs
{
    public class RealTimeHub : Hub
    {
        public async Task SendQueue(List<KioskQueueDto> queue)
        {
            await Clients.All.SendAsync("SendQueue", queue);
        }
        public async Task ReceivedQueue(List<KioskQueueDto> queue)
        {
            await Clients.All.SendAsync("ReceivedQueue", queue);
        }

        public async Task SenCountry(CountryDto country)
        {
            await Clients.All.SendAsync("ReceivedCountry", country);
        }
    }
}

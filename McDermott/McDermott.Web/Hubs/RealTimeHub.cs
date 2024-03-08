using McDermott.Application.Dtos.Queue;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace McDermott.Web.Hubs
{
    public class RealTimeHub : Hub
    {
        public async Task SendQueue(KioskQueueDto queue)
        {
            await Clients.All.SendAsync("ReceivedQueue", queue);
        }

        public async Task SenCountry(CountryDto country)
        {
            await Clients.All.SendAsync("ReceivedCountry", country);
        }
    }
}

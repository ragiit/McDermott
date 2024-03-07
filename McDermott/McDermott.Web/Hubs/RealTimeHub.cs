using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace McDermott.Web.Hubs
{
    public class RealTimeHub : Hub
    {
        public async Task SendQueue(int CounterId, int ServerKId, int NoQueue)
        {
            await Clients.All.SendAsync("ReceivedQueue", CounterId, ServerKId, NoQueue);
        }

        public async Task SenCountry(CountryDto country)
        {
            await Clients.All.SendAsync("ReceivedCountry", country);
        }
    }
}

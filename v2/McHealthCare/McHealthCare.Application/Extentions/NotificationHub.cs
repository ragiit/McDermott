using McHealthCare.Domain.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Extentions.EnumHelper;

namespace McHealthCare.Application.Extentions
{
    public class NotificationHub : Hub<INotificationClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification(new());
            await base.OnConnectedAsync();
        }
        //public async Task NotifyCreateAsync(object data)
        //{
        //    await Clients.All.ReceiveNotification(new { Type = "Create", Data = data });
        //}

        //public async Task NotifyUpdateAsync(object data)
        //{
        //    await Clients.All.ReceiveNotification(new { Type = "Update", Data = data });
        //}

        //public async Task NotifyDeleteAsync(object data)
        //{
        //    await Clients.All.ReceiveNotification(new { Type = "Delete", Data = data });
        //}
    }


    public interface INotificationClient
    {
        Task ReceiveNotification(ReceiveDataDto m);
    }

    public class ReceiveDataDto
    {
        public EnumTypeReceiveData Type { get; set; }
        public object Data { get; set; }
    }
 }

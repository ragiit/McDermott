using McDermott.Application.Dtos.Queue;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;
using static McDermott.Application.Features.Commands.Transaction.CounterCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class ViewCounterPage
    {
        #region Relation Data

        private List<CounterDto> counters = new();
        private List<KioskQueueDto> KiosksQueue = new();
        private List<KioskDto> Kiosks = new();
        private List<ServiceDto> Services = [];
        private List<UserDto> Physicians = [];
        public List<UserDto> Phys = new();
        private List<ServiceDto> ServiceK = new();
        private List<ServiceDto> ServiceP = [];

        #endregion Relation Data

        #region Grid properties

        public IGrid Grid { get; set; }

        #endregion Grid properties

        #region variabel data

        private string NameCounter { get; set; } = string.Empty;
        private string NameServices { get; set; } = string.Empty;
        private string NameServicesK { get; set; } = string.Empty;
        private string Phy { get; set; } = string.Empty;
        private int? sId { get; set; }
        private int? PhysicianId { get; set; }
        private string? userBy;
        private User? User = new();

        [Parameter]
        public int id { get; set; } = 14;

        #endregion variabel data

        #region Async Data

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await base.OnAfterRenderAsync(firstRender);

        //    if (firstRender)
        //    {
        //        try
        //        {
        //            await LoadUser();
        //        }
        //        catch { }
        //    }
        //}

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadData();
            }
            catch { }
        }

        //private async Task LoadUser()
        //{
        //    User = await oLocal.GetUserInfo();
        //    userBy = User.Name;
        //}

        private async Task LoadData()
        {
            User = await oLocal.GetUserInfo();
            userBy = User.Name;

            var general = await Mediator.Send(new GetCounterByIdQuery(id));
            Services = await Mediator.Send(new GetServiceQuery());
            var physician = await Mediator.Send(new GetUserQuery());
            var b = await Mediator.Send(new GetKioskQueueQuery());

            //var cekServicesK = service

            KiosksQueue = [.. b.Where(x => x.ServiceId == general.ServiceId && x.CreatedDate.Value.Date == DateTime.Now.Date)];

            NameCounter = $"Queue Listing Counter {general.Name}";
            sId = general.ServiceId;
            PhysicianId = general.PhysicianId;

            var skId = general.ServiceKId;
            NameServicesK = Services.FirstOrDefault(x => x.Id == skId)?.Name;

            NameServices = sId != null ? Services.FirstOrDefault(x => x.Id == sId)?.Name : "-";
            Phy = PhysicianId != null ? physician.FirstOrDefault(x => x.Id == PhysicianId && x.IsPhysicion)?.Name : null;
        }

        #endregion Async Data

        #region Grid Confiduration

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            else if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
        }

        public MarkupString GetIssuePriorityIconHtml(string status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case "call":
                    priorityClass = "info";
                    title = "Call";
                    break;

                case "hadir":
                    priorityClass = "success";
                    title = "Hadir";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row justify-content-center'><div class='col-sm-5 pl-0'>" +
                          $"<span class='badge bg-{priorityClass} py-1 px-4' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #endregion Grid Confiduration

        #region Function Button

        private void CloseDetail()
        {
        }

        #endregion Function Button
    }
}
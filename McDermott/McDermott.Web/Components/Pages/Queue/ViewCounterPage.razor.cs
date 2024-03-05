using McDermott.Application.Dtos.Queue;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class ViewCounterPage
    {
        #region Relation Data

        private List<CounterDto> counters = new();
        private List<KioskQueueDto> KiosksQueue = new();
        private List<KioskQueueDto> DataKiosksQueue = new();
        private KioskQueueDto FormKiosksQueue = new();
        private List<KioskDto> Kiosks = new();
        private List<ServiceDto> Services = [];
        private List<UserDto> Physicians = [];
        public List<UserDto> Phys = new();
        private List<ServiceDto> ServiceK = new();
        private List<ServiceDto> ServiceP = [];
        private User? User = new();
        private KioskQueueDto DataPatient = new();
        private KioskQueueDto FormCounters = new();

        #endregion Relation Data

        #region Grid properties

        public IGrid Grid { get; set; }

        #endregion Grid properties

        #region variabel data

        private string NameCounter { get; set; } = string.Empty;
        private string NameServices { get; set; } = string.Empty;
        private string NameServicesK { get; set; } = string.Empty;
        private string Phy { get; set; } = string.Empty;
        private string? userBy;
        private int? sId { get; set; }
        private int? PhysicianId { get; set; }
        private bool ShowPresent { get; set; }

        [Parameter]
        public int id { get; set; }

        #endregion variabel data

        #region Async Data

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadData();
            }
            catch { }
        }

        private async Task LoadData()
        {
            User = await oLocal.GetUserInfo();
            userBy = User.Name;

            var general = await Mediator.Send(new GetCounterByIdQuery(id));
            Services = await Mediator.Send(new GetServiceQuery());
            var physician = await Mediator.Send(new GetUserQuery());
            DataKiosksQueue = await Mediator.Send(new GetKioskQueueQuery());
            ServiceK = Services.Where(x => x.IsKiosk == true).ToList();

            //var cekServicesK = service
            if (general.ServiceId == null & general.PhysicianId == null)
            {
                KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date)];
            }
            else if (general.ServiceId != null & general.PhysicianId == null)
            {
                KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.ServiceId == general.ServiceId && x.CreatedDate.Value.Date == DateTime.Now.Date)];
            }
            else if (general.ServiceId == null & general.PhysicianId != null)
            {
                KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.Kiosk.PhysicianId == general.PhysicianId && x.CreatedDate.Value.Date == DateTime.Now.Date)];
            }
            else if (general.ServiceId != null & general.PhysicianId != null)
            {
                KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date && x.Kiosk.PhysicianId == general.PhysicianId && x.ServiceId == general.ServiceId)];
            }
            NameCounter = $"Queue Listing Counter {general.Name}";
            sId = general.ServiceId;
            PhysicianId = general.PhysicianId;

            var skId = general.ServiceKId;
            NameServicesK = Services.FirstOrDefault(x => x.Id == skId)?.Name;

            NameServices = sId != null ? Services.FirstOrDefault(x => x.Id == sId)?.Name : "-";
            Phy = PhysicianId != null ? physician.FirstOrDefault(x => x.Id == PhysicianId && x.IsPhysicion)?.Name : "-";
        }

        #endregion Async Data

        #region Grid Configuration

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

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #endregion Grid Configuration

        #region Function Button

        private async Task Click_Call(int id)
        {
            try
            {
                var data = DataKiosksQueue.Where(x => x.Id == id).FirstOrDefault();
                FormKiosksQueue.Id = data.Id;
                FormKiosksQueue.KioskId = data.KioskId;
                FormKiosksQueue.ServiceId = data.ServiceId;
                FormKiosksQueue.NoQueue = data.NoQueue;
                FormKiosksQueue.ServiceKId = data.ServiceKId;
                FormKiosksQueue.Status = "call";

                if (FormKiosksQueue.Id != null)
                {
                    await Mediator.Send(new UpdateKioskQueueRequest(FormKiosksQueue));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task Click_Next()
        {
            try
            {
                var data = await Mediator.Send(new GetKioskQueueQuery());

                if (FormCounters.Id != 0)
                {
                    //Mendapatkan data berdasarkan Counter, service dan tanggal hari ini
                    var TodayQueu = data.Where(x => x.ServiceId == FormCounters.ServiceId && x.ServiceKId == FormCounters.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date).ToList();

                    if (TodayQueu.Count == 0)
                    {
                        FormCounters.NoQueue = 1;
                    }
                    else
                    {
                        var GetNoQueue = TodayQueu.OrderByDescending(x => x.NoQueue).FirstOrDefault();
                        FormCounters.NoQueue = (int)GetNoQueue.NoQueue + 1;
                    }
                    FormCounters.Status = null;
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                    var datas = FormCounters;
                }
                ShowPresent = false;
                ToastService.ShowInfo("Patient Successfully Diverted");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Click_Present(int Id)
        {
            try
            {
                ShowPresent = true;
                FormCounters = DataKiosksQueue.Where(x => x.Id == Id).FirstOrDefault();
                if (FormCounters.Id != 0)
                {
                    FormCounters.Status = "present";
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                }
                ToastService.ShowSuccess("Data Success!");
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task Click_Absent(int Id)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private void CloseDetail()
        {
            NavigationManager.NavigateTo("/queue/queue-counter");
        }

        #endregion Function Button
    }
}
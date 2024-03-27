using McDermott.Application.Dtos.Queue;

using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;

using static McDermott.Application.Features.Commands.Queue.DetailQueueDisplayCommand;

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

        private HubConnection hubConnection;
        private string NameCounter { get; set; } = string.Empty;
        private string NameClasses { get; set; }
        private string NameServices { get; set; } = string.Empty;
        private string NameServicesK { get; set; } = string.Empty;
        private string Phy { get; set; } = string.Empty;
        private string? userBy;
        private long? sId { get; set; }
        private long? PhysicianId { get; set; }
        private bool ShowPresent { get; set; }
        private bool PanelVisible { get; set; } = true;

        [Parameter]
        public long CounterId { get; set; }

        #endregion variabel data

        #region Async Data

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadData();
                //hubConnection = new HubConnectionBuilder()
                //    .WithUrl("http://localhost:5000/realTimeHub")
                //    .Build();

                //await hubConnection.StartAsync();

                await InvokeAsync(StateHasChanged);
            }
            catch { }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {
                    await oLocal.GetCookieUserLogin();
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task LoadData()
        {
            try
            {
                var us = await JsRuntime.GetCookieUserLogin();

                userBy = us.Name;

                ShowPresent = false;
                PanelVisible = true;
                var general = await Mediator.Send(new GetCounterByIdQuery(CounterId));
                Services = await Mediator.Send(new GetServiceQuery());
                var physician = await Mediator.Send(new GetUserQuery());
                DataKiosksQueue = await Mediator.Send(new GetKioskQueueQuery());
                ServiceK = Services.Where(x => x.IsKiosk == true).ToList();
                var GetClass = await Mediator.Send(new GetClassTypeQuery());

                //var cekServicesK = service
                if (general.ServiceId == null && general.PhysicianId == null)
                {
                    KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date && (x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present"))];
                }
                else if (general.ServiceId != null && general.PhysicianId == null)
                {
                    KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.ServiceId == general.ServiceId && x.CreatedDate.Value.Date == DateTime.Now.Date && (x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present"))];
                }
                else if (general.ServiceId == null && general.PhysicianId != null)
                {
                    KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.Kiosk.PhysicianId == general.PhysicianId && x.CreatedDate.Value.Date == DateTime.Now.Date && x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present")];
                }
                else if (general.ServiceId != null && general.PhysicianId != null )
                {
                    KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date &&( x.Kiosk.PhysicianId == general.PhysicianId || x.Kiosk.PhysicianId == null) && x.ServiceId == general.ServiceId && x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present")];
                }
                foreach (var cl in KiosksQueue)
                {
                    var ClassId = KiosksQueue.Where(x => x.ClassTypeId == cl.ClassTypeId).Select(x => x.ClassTypeId).FirstOrDefault();
                    var classList = GetClass.Where(x => x.Id == ClassId).FirstOrDefault();
                    if (classList == null)
                    {
                        cl.NameClass = "-";
                    }
                    else
                    {
                        cl.NameClass = classList.Name;
                    }
                }
                NameCounter = $"Queue Counter {general.Name}";
                sId = general.ServiceId;
                PhysicianId = general.PhysicianId;

                var skId = general.ServiceKId;
                NameServicesK = Services.FirstOrDefault(x => x.Id == skId)?.Name;

                NameServices = sId != null ? Services.FirstOrDefault(x => x.Id == sId)?.Name : "-";
                Phy = PhysicianId != null ? physician.FirstOrDefault(x => x.Id == PhysicianId && x.IsPhysicion)?.Name : "-";
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                  "\n" +
                  "==================== START ERROR ====================" + "\n" +
                  "Message: " + ex.Message + "\n" +
                  "Inner Message: " + ex.InnerException?.Message + "\n" +
                  "Stack Trace: " + ex.StackTrace + "\n" +
                  "==================== END ERROR ====================" + "\n");
            }
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

        private async Task Refresh_Click()
        {

            await LoadData();

        }

        public MarkupString GetIssueStageIconHtml(string status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case "call":
                    priorityClass = "info";
                    title = "Call";
                    break;

                case "present":
                    priorityClass = "success";
                    title = "Present";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        public MarkupString GetClassTypeIconHtml(string? classType)
        {
            string priorityClass;
            string title;

            switch (classType)
            {
                case "VVIP":
                    priorityClass = "danger";
                    title = "VVIP";
                    break;

                case "VIP":
                    priorityClass = "danger";
                    title = "VIP";
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

        private async Task Click_Call(KioskQueueDto context)
        {
            try
            {
                var z = context;

                context.QueueStage = "call";
                context.QueueStatus = "calling";

                if (context.Id != 0)
                {
                    var dataQueue = await Mediator.Send(new UpdateKioskQueueRequest(context));

                    DetailQueueDisplayDto displayDet = new();
                    displayDet.KioskQueueId = context.Id;
                    displayDet.ServiceId = context.ServiceId;
                    displayDet.ServicekId = context.ServiceKId;
                    displayDet.NumberQueue = context.QueueNumber;
                    await Mediator.Send(new CreateDetailQueueDisplayRequest(displayDet));

                    //await hubConnection.SendAsync("CallPatient", CounterId, context.Id);
                }
                var cek = CounterId;
                //DataKiosksQueue = FormKiosksQueue;

                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task Click_Finish()
        {
            try
            {
                if (FormCounters.Id != 0)
                {
                    FormCounters.QueueStage = "finish";
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                }
                ToastService.ShowSuccess("Data Success!");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Click_Next()
        {
            try
            {
                var data = await Mediator.Send(new GetKioskQueueQuery());

                if (FormCounters.Id != 0)
                {
                    if (FormCounters.ClassTypeId != null)
                    {
                        var TodayQueu = data.Where(x => x.ServiceId == FormCounters.ServiceId && x.ServiceKId == FormCounters.ServiceKId && x.ClassTypeId == FormCounters.ClassTypeId && x.CreatedDate.Value.Date == DateTime.Now.Date).ToList();
                        if (TodayQueu.Count == 0)
                        {
                            FormCounters.QueueNumber = 1;
                        }
                        else
                        {
                            var GetNoQueue = TodayQueu.OrderByDescending(x => x.QueueNumber).FirstOrDefault();
                            if (GetNoQueue.QueueNumber < 9)
                            {
                                FormCounters.QueueNumber = 1 * (long)GetNoQueue.QueueNumber + 2;
                            }
                            else
                            {
                                ToastService.ShowError("Penuh Eyy!!");
                            }
                        }
                    }
                    else
                    {
                        //Mendapatkan data berdasarkan Counter, service dan tanggal hari ini
                        var TodayQueu = data.Where(x => x.ServiceId == FormCounters.ServiceId && x.ServiceKId == FormCounters.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date).ToList();

                        if (TodayQueu.Count == 0)
                        {
                            FormCounters.QueueNumber = 2;
                        }
                        else
                        {
                            var GetNoQueue = TodayQueu.OrderByDescending(x => x.QueueNumber).FirstOrDefault();
                            if (GetNoQueue.QueueNumber < 10)
                            {
                                FormCounters.QueueNumber = 1 * ((long)GetNoQueue.QueueNumber + 2);
                            }
                            else
                            {
                                FormCounters.QueueNumber = 1 * ((long)GetNoQueue.QueueNumber + 1);
                            }
                        }
                    }
                    FormCounters.QueueStage = null;
                    FormCounters.QueueStatus = "waiting";
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

        private async Task Click_Present(long Id)
        {
            try
            {
                ShowPresent = true;
                FormCounters = DataKiosksQueue.Where(x => x.Id == Id).FirstOrDefault();
                if (FormCounters.Id != 0)
                {
                    FormCounters.QueueStage = "present";
                    FormCounters.QueueStatus = "on process";
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                }
                ToastService.ShowSuccess("Data Success!");
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task Click_Absent(long Id)
        {
            try
            {
                FormCounters = DataKiosksQueue.Where(x => x.Id == Id).FirstOrDefault();
                if (FormCounters.Id != 0)
                {
                    FormCounters.QueueStage = "absent";
                    FormCounters.QueueStatus = "cancel";
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                }
                ToastService.ShowError("Patient Absent!!");
                await LoadData();
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
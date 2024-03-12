using McDermott.Application.Dtos.Queue;
using McDermott.Domain.Entities;
using System;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.DetailQueueDisplayCommand;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;
using static McDermott.Application.Features.Commands.Queue.QueueDisplayCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class ViewDisplayPage
    {
        #region Data Relation

        private List<CompanyDto> companies = new();
        private List<KioskQueueDto> kioskQueues = [];
        private List<KioskQueueDto> DataQueue = new();
        private List<DetailQueueDisplayDto> DetQueues = new();
        private CounterDto getCounId = new();

        #endregion Data Relation

        #region static Variable

        [Parameter]
        public long DisplayId { get; set; }

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private HubConnection hubConnection;
        private List<long> CounterCount = new List<long>();
        private string currentTime;
        private long? Cids { get; set; }
        private long ServiceKId { get; set; }
        private KioskQueueDto? Queuek { get; set; }

        #endregion static Variable

        private CultureInfo indonesianCulture = new CultureInfo("id-ID");

        private async Task UpdateTime()
        {
            while (true)
            {
                currentTime = DateTime.Now.ToString("HH:mm:ss");
                StateHasChanged();
                await Task.Delay(100); // Update every 1 second
            }
        }

        #region Async Data

        private void ReloadPage()
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await UpdateTime();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            DisplayId = $"{NavigationManager.Uri.Replace(NavigationManager.BaseUri + "queue/viewdisplay/", "")}".ToInt32();
            await LoadData();
            hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/realTimeHub")
                    .Build();
            hubConnection.On<List<KioskQueueDto>>("ReceivedQueue", (queue) =>
            {
                kioskQueues = [.. queue.Where(q =>
                  q.ServiceKId == getCounId.ServiceKId &&
                    (q.Status == null || q.Status == "call"))];
                Cids = kioskQueues.Select(x => x.NoQueue).FirstOrDefault(); ;
                InvokeAsync(StateHasChanged);
            });
            await hubConnection.StartAsync();

        }

        private List<CounterDto> listcounter = [];

        private async Task LoadData()
        {
            PanelVisible = true;
            // Mengambil detail antrian berdasarkan ID tampilan
            var queues = await Mediator.Send(new GetDetailQueueDisplayQuery());
            var dispId = queues.FirstOrDefault(q => q.Id == DisplayId);
            DetQueues = queues.Where(q => q.QueueDisplayId == dispId.QueueDisplayId).ToList();

            // Mengambil detail counter berdasarkan ID counter dari antrian terpilih
            var counters = await Mediator.Send(new GetCounterQuery());
            getCounId = counters.FirstOrDefault(c => c.Id == dispId.CounterId);

            // Mengambil antrian kiosk berdasarkan layanan counter dan status "call" atau null
            //var kioskQueues = await Mediator.Send(new GetKioskQueueQuery());
            //var dataQueue = kioskQueues.Where(q =>
            //    q.ServiceKId == getCounId.ServiceKId &&
            //    (q.Status == null || q.Status == "call")).ToList();
            Cids = DataQueue.Select(x => x.NoQueue).FirstOrDefault();
            PanelVisible = false;
        }

        #endregion Async Data

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
    }
}
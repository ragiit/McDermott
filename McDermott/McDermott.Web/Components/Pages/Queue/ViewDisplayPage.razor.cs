using McDermott.Application.Dtos.Queue;
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
        private List<KioskQueueDto> kioskQueues = new();
        private List<DetailQueueDisplayDto> DetQueues = new();
        private CounterDto cqueues = new();

        #endregion Data Relation

        #region static Variable

        [Parameter]
        public long DisplayId { get; set; }

        public IGrid Grid { get; set; }
        private HubConnection hubConnection;
        private List<long> CounterCount = new List<long>();
        private string currentTime;
        private long Cids { get; set; }
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

        #region

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await UpdateTime();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/realTimeHub")
                    .Build();
            hubConnection.On<long, long, long>("ReceivedQueue", (CounterId, ServiceKId, NoQueue) =>
            {
                Cids = NoQueue;
                StateHasChanged();
            });
            await hubConnection.StartAsync();

            await LoadData();
        }

        private List<CounterDto> listcounter = [];

        private async Task LoadData()
        {
            var queues = await Mediator.Send(new GetDetailQueueDisplayQuery());
            var DispId = queues.FirstOrDefault(x => x.Id == DisplayId);
            DetQueues = queues.Where(x => x.QueueDisplayId == DispId.QueueDisplayId).ToList();
            var Counters = await Mediator.Send(new GetCounterQuery());
            var getCounId = Counters.FirstOrDefault(x => x.Id == DispId.CounterId);
            var kioskQueue = await Mediator.Send(new GetKioskQueueQuery());
            kioskQueues = kioskQueue.Where(x => x.ServiceKId == getCounId.ServiceKId && (x.Status == null || x.Status == "call")).ToList();

        }

        #endregion

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
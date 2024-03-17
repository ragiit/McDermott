using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.DetailQueueDisplayCommand;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;

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
        private long? IdQueue { get; set; }
        private long ServiceKId { get; set; }
        private KioskQueueDto? Queuek { get; set; }
        private Timer timer;
        private int _counter;

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
            hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/realTimeHub")
                    .Build();
            hubConnection.On<long, long>("CallPatient", (Id, numberQueue) =>
            {
                IdQueue = Id;
                Cids = numberQueue;
                InvokeAsync(StateHasChanged);
            });
            await hubConnection.StartAsync();
            await LoadData();

            timer = new Timer(async (_) => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private async Task LoadData()
        {
            _counter++;
            await InvokeAsync(() =>
            {
                PanelVisible = true;
            });

            try
            {
                // Mengambil detail antrian berdasarkan ID tampilan
                var queues = await Mediator.Send(new GetDetailQueueDisplayQuery());
                var dispId = queues.FirstOrDefault(q => q.Id == DisplayId);
                DetQueues = queues.Where(q => q.QueueDisplayId == dispId?.QueueDisplayId).ToList();

                // Mengambil detail counter berdasarkan ID counter dari antrian terpilih
                var counters = await Mediator.Send(new GetCounterQuery());
                getCounId = counters.FirstOrDefault(c => c.Id == dispId?.CounterId);

                // Mengambil antrian kiosk berdasarkan layanan counter dan status "call" atau null
                var dataQueue = await Mediator.Send(new GetKioskQueueQuery());
                kioskQueues = dataQueue.Where(q => q.ServiceKId == getCounId?.ServiceKId &&
                                                    (q.QueueStage == null || q.QueueStage == "call")).ToList();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            await InvokeAsync(() =>
            {
                PanelVisible = false; // Jika diperlukan, panel disembunyikan di sini
                StateHasChanged(); // Memastikan bahwa perubahan UI diterapkan
            });
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
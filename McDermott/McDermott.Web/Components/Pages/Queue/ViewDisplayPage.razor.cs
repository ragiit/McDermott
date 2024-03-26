using McDermott.Application.Dtos.Queue;
using McDermott.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private QueueDisplayDto DetQueues = new();
        private List<CounterDto> getCount = new();
        private List<DetailQueueDisplayDto> QueueNumber = new();

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
        private long? cId { get; set; }
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
            //hubConnection = new HubConnectionBuilder()
            //        .WithUrl("http://localhost:5000/realTimeHub")
            //        .Build();
            //hubConnection.On<long, long>("CallPatient", (Id, numberQueue) =>
            //{
            //    IdQueue = Id;
            //    Cids = numberQueue;
            //    //foreach (var ic in getCount)
            //    //{
            //    //    if (ic.Id == Id)
            //    //    {
            //    //        Cids = numberQueue;
            //    //        break;
            //    //    }
            //    //}
            //    InvokeAsync(StateHasChanged);
            //});
            //await hubConnection.StartAsync();
            var queues = await Mediator.Send(new GetQueueDisplayByIdQuery(DisplayId));
            foreach (var i in queues.CounterIds)
            {
                var DataCounter = await Mediator.Send(new GetCounterByIdQuery(i));
                var card = new CounterDto
                {
                    Id = i,
                    Name = DataCounter.Name,
                    ServiceKId = DataCounter.ServiceKId,
                    ServiceId = DataCounter.ServiceId

                };
                getCount.Add(card);
                var sa = getCount;
                cId = DataCounter.Id;
            }

            await LoadData();

            timer = new Timer(async (_) => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void Dispose()
        {
            timer.Dispose();
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

                var datakioskQueue = await Mediator.Send(new GetKioskQueueQuery());
                QueueNumber = await Mediator.Send(new GetDetailQueueDisplay());

                
                kioskQueues = [.. datakioskQueue.Where(q => q.CreatedDate.Value.Date == DateTime.Now.Date)];


            }
            catch { }

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
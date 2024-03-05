using DevExpress.Blazor.Internal;
using McDermott.Application.Dtos.Queue;
using System;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.QueueDisplayCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class ViewDisplayPage
    {
        #region Data Relation

        private List<CompanyDto> companies = new();
        private List<QueueDisplayDto> queues = new();
        private CounterDto cqueues = new();

        #endregion Data Relation

        #region static Variable

        [Parameter]
        public int id { get; set; }

        private List<int> CounterCount = new List<int>();
        private string currentTime;
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
            //await UpdateTime();
            await LoadData();
        }

        private List<CounterDto> listcounter = [];

        private async Task LoadData()
        {
            queues = await Mediator.Send(new GetQueueDisplayQuery());
            var CounterCout = queues.Where(x => x.Id == id).ToList();
            var Counters = await Mediator.Send(new GetCounterQuery());

            foreach (var i in CounterCout[0].CounterId)
            {
                //var a = queues.ForEach(x => x.listIdCounter =int.Join(",", Counters.Where(z => x.CounterId != null && x.CounterId.Contains(z.Id)).Select(x => x.Id).ToList()));

                listcounter.Add(Counters.Where(x => x.Id == i).FirstOrDefault());
            }
        }

        #endregion
    }
}
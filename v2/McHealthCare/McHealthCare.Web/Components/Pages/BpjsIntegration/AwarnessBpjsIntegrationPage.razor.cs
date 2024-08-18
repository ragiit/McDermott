using McHealthCare.Application.Dtos.BpjsIntegration;
using Newtonsoft.Json;
using static McHealthCare.Application.Features.Commands.BpjsIntegration.AwarenessCommand;

namespace McHealthCare.Web.Components.Pages.BpjsIntegration
{
    public partial class AwarnessBpjsIntegrationPage
    {
        private List<AwarenessDto> _awarnessDto { get; set; } = new();
        private int parameter1 = 1; // Row data awal yang akan ditampilkan
        private int parameter2 = 10; // Limit jumlah data yang akan ditampilkan
        private IGrid Grid { get; set; }
        private bool IsLoading { get; set; } = false;
        private bool PanelVisible { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            _awarnessDto = await Mediator.Send(new GetAwarenessQuery());

            if (_awarnessDto.Count == 0)
                await RefreshToDb();

            PanelVisible = false;
        }

        private async Task RefreshToDb()
        {
            PanelVisible = true;

            Console.WriteLine("Getting API awarness");

            var response = await PcareService.SendPCareService($"kesadaran", HttpMethod.Get);

            if (response.Item2 != 200)
            {
                PanelVisible = false;
                ToastService.ClearErrorToasts();
                ToastService.ShowError(Convert.ToString(response.Item1));
                return;
            }

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Item1);

            var dynamicList = (IEnumerable<dynamic>)data.list;

            var AwarnessList = dynamicList.Select(item => new AwarenessDto
            {
                KdSadar = item.kdSadar,
                NmSadar = item.nmSadar
            }).ToList();

            Console.WriteLine("Success Getting API awarness");

            _awarnessDto = await Mediator.Send(new UpdateToDbAwarenessRequest(AwarnessList));

            PanelVisible = false;
        }

        private int CalculateParameter1(int pageIndex, int pageSize)
        {
            return (pageIndex - 1) * pageSize + 1;
        }

        private async Task HandlePageIndexChanged(int newIndex)
        {
            parameter1 = newIndex++;
            await LoadData();
        }

        private async Task HandlePageSizeChanged(int newSize)
        {
            parameter2 = newSize;
            await LoadData();
        }
    }
}
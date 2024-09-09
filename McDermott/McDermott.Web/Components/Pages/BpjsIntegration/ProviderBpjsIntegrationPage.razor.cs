using System.Text.Json.Serialization;

namespace McDermott.Web.Components.Pages.BpjsIntegration
{
    public partial class ProviderBpjsIntegrationPage
    {
        public class ProviderBPJSIntegrationTemp
        {
            [JsonPropertyName("kdProvider")]
            public string KdProvider { get; set; } = string.Empty;

            [JsonPropertyName("nmProvider")]
            public string NmProvider { get; set; } = string.Empty;
        }

        private List<ProviderBPJSIntegrationTemp> _ProviderBPJSIntegrationTemp { get; set; } = new();
        private int parameter1 = 1; // Row data awal yang akan ditampilkan
        private int parameter2 = 10; // Limit jumlah data yang akan ditampilkan
        private int _pageIndex = 0;

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

            var response = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"provider/{parameter1}/{parameter2}", HttpMethod.Get);

            if (response.Item2 != 200)
            {
                PanelVisible = false;

                if (response.Item2 == 404)
                {
                    ToastService.ClearErrorToasts();
                    ToastService.ShowError(Convert.ToString(response.Item1));
                }

                _ProviderBPJSIntegrationTemp.Clear();

                return;
            }

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Item1);

            var dynamicList = (IEnumerable<dynamic>)data.list;

            var ProviderList = dynamicList.Select(item => new ProviderBPJSIntegrationTemp
            {
                KdProvider = item.kdProvider,
                NmProvider = item.nmProvider,
            }).ToList();

            _ProviderBPJSIntegrationTemp = ProviderList;
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

        private async Task HandlePageShowAllSizeChanged(bool isAll)
        {
            parameter2 = int.MaxValue;
            await LoadData();
        }
    }
}
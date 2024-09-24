using System.Text.Json.Serialization;

namespace McDermott.Web.Components.Pages.BpjsIntegration
{
    public partial class ObatBpjsIntegrationPage
    {
        private List<ProductDto> Products = [];

        public class ObatBPJSIntegrationTemp
        {
            [JsonPropertyName("kdObat")]
            public string KdObat { get; set; } = string.Empty;

            [JsonPropertyName("nmObat")]
            public string NmObat { get; set; } = string.Empty;

            [JsonPropertyName("sedia")]
            public int Sedia { get; set; }
        }

        public class KdTkp
        {
            public int Code { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private ProductDto SelectedProduct { get; set; } = new();

        private List<ObatBPJSIntegrationTemp> _ObatBPJSIntegrationTemp { get; set; } = new();
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
            //Products = await Mediator.Send(new GetProductQuery());
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

        private async Task SelectChangeProduct(ProductDto req)
        {
            SelectedProduct = req;
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            var response = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"obat/dpho/{SelectedProduct.Name}/{parameter1}/{parameter2}", HttpMethod.Get);

            if (response.Item2 != 200)
            {
                PanelVisible = false;

                if (response.Item2 == 404)
                {
                    ToastService.ClearErrorToasts();
                    ToastService.ShowError(Convert.ToString(response.Item1));
                }

                _ObatBPJSIntegrationTemp.Clear();

                return;
            }

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Item1);

            var dynamicList = (IEnumerable<dynamic>)data.list;

            var ObatList = dynamicList.Select(item => new ObatBPJSIntegrationTemp
            {
                KdObat = item.kdObat,
                NmObat = item.nmObat,
                Sedia = item.sedia,
            }).ToList();

            _ObatBPJSIntegrationTemp = ObatList;

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
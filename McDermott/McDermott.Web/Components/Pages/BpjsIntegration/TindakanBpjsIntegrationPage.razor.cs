using System.Text.Json.Serialization;

namespace McDermott.Web.Components.Pages.BpjsIntegration
{
    public partial class TindakanBpjsIntegrationPage
    {
        public class TindakanBPJSIntegrationTemp
        {
            [JsonPropertyName("kdTindakan")]
            public string KdTindakan { get; set; } = string.Empty;

            [JsonPropertyName("nmTindakan")]
            public string NmTindakan { get; set; } = string.Empty;

            [JsonPropertyName("maxTarif")]
            public int MaxTarif { get; set; }

            [JsonPropertyName("withValue")]
            public bool WithValue { get; set; }
        }

        public class KdTkp
        {
            public int Code { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private int SelectedCodeKdTkp { get; set; } = 10;

        private List<KdTkp> _KdTkpList = [
                new()
                {
                    Code = 10,
                    Name = "kdTkp"
                },
                new()
                {
                    Code = 20,
                    Name = "RJTP"
                },
                new()
                {
                    Code = 50,
                    Name = "Promotif"
                },
            ];

        private List<TindakanBPJSIntegrationTemp> _tindakanBPJSIntegrationTemp { get; set; } = new();
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

        private async Task SelectChangeKdTkp(KdTkp req)
        {
            SelectedCodeKdTkp = req.Code;
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            var response = await PcareService.SendPCareService($"tindakan/kdTkp/{SelectedCodeKdTkp}/{parameter1}/{parameter2}", HttpMethod.Get);

            if (response.Item2 != 200)
            {
                PanelVisible = false;

                if (response.Item2 == 404)
                {
                    ToastService.ClearErrorToasts();
                    ToastService.ShowError(Convert.ToString(response.Item1));
                }

                _tindakanBPJSIntegrationTemp.Clear();

                return;
            }

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Item1);

            var dynamicList = (IEnumerable<dynamic>)data.list;

            var TindakanList = dynamicList.Select(item => new TindakanBPJSIntegrationTemp
            {
                KdTindakan = item.kdTindakan,
                NmTindakan = item.nmTindakan,
                MaxTarif = item.maxTarif,
                WithValue = item.withValue
            }).ToList();

            _tindakanBPJSIntegrationTemp = TindakanList;

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
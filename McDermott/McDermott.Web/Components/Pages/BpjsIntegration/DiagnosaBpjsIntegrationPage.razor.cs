namespace McDermott.Web.Components.Pages.BpjsIntegration
{
    public partial class DiagnosaBpjsIntegrationPage
    {
        private List<DiagnosisDto> Diagnoses = [];

        public class KdTkp
        {
            public int Code { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        //private int SelectedDiagnosis { get; set; } = 10;
        private DiagnosisDto SelectedDiagnosis { get; set; } = new();

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

        private List<DiagnosisBPJSIntegrationTemp> _diagnosisBPJSIntegrationTemp { get; set; } = new();
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
            var Diagnoses = (await Mediator.Send(new GetDiagnosisQuery())).Item1;
            this.Diagnoses = Diagnoses;
            IsLoading = false;
        }

        public string Diagnosa { get; set; } = string.Empty;

        private async Task OnSearchDiagnosis()
        {
            var a = refDiagnosa;
            var ba = Diagnosa;

            await LoadData();
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

        private async Task SelectChangeDiagnoses(DiagnosisDto req)
        {
            SelectedDiagnosis = req;
            await LoadData();
        }

        private DxMaskedInput<string> refDiagnosa { get; set; }

        private async Task LoadData()
        {
            PanelVisible = true;

            var response = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"diagnosa/{Diagnosa}/{parameter1}/{parameter2}", HttpMethod.Get);

            if (response.Item2 != 200)
            {
                PanelVisible = false;

                if (response.Item2 == 404)
                {
                    ToastService.ClearErrorToasts();
                    ToastService.ShowError(Convert.ToString(response.Item1));
                }

                _diagnosisBPJSIntegrationTemp.Clear();

                return;
            }

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Item1);

            var dynamicList = (IEnumerable<dynamic>)data.list;

            var DiagnosisList = dynamicList.Select(item => new DiagnosisBPJSIntegrationTemp
            {
                KdDiag = item.kdDiag,
                NmDiag = item.nmDiag,
                NonSpesialis = item.nonSpesialis,
            }).ToList();

            _diagnosisBPJSIntegrationTemp = DiagnosisList;

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
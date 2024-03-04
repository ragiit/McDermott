using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;

namespace McDermott.Web.Components.Pages.Patient
{
    public partial class InsurancePolicyPage
    {
        private List<CountryDto> Countries = [];
        private List<ProvinceDto> Provinces = [];

        [Parameter]
        public UserDto User { get; set; } = new()
        {
            Name = "-"
        };

        private List<UserDto> Users = [];
        private List<InsuranceDto> Insurances = [];
        private List<InsurancePolicyDto> InsurancePolicies = [];
        private InsurancePolicyDto InsurancePoliciyForm = new();

        #region Data

        private bool IsBPJS = false;
        private int? _InsuranceId = 0;

        private int? InsuranceId
        {
            get => _InsuranceId;
            set
            {
                InsurancePoliciyForm.InsuranceId = (int)value;
                _InsuranceId = value;

                IsBPJS = Insurances.Any(x => x.IsBPJS == true && x.Id == value) ? true : false;
            }
        }

        #endregion Data

        #region Grid Properties

        private GroupMenuDto UserAccessCRUID = new();

        private bool ShowForm { get; set; } = false;
        private bool IsAccess = false;
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        private bool FormValidationState = true;

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        #endregion Grid Properties

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

            Insurances = await Mediator.Send(new GetInsuranceQuery());

            await LoadData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    var result = await NavigationManager.CheckAccessUser(oLocal);
                    IsAccess = result.Item1;
                    UserAccessCRUID = result.Item2;
                }
                catch { }
            }
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            Countries = await Mediator.Send(new GetCountryQuery());

            InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery());

            if (User != null && User.Id != 0)
            {
                InsurancePolicies = InsurancePolicies.Where(x => x.UserId == User.Id).ToList();
            }

            PanelVisible = false;
        }

        #endregion LoadData

        #region Grid Function

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #region SaveDelete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteInsurancePolicyRequest(((InsuranceDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteInsurancePolicyRequest(ids: SelectedDataItems.Adapt<List<InsuranceDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch { }
        }

        private async Task OnSave()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(User.Name))
                {
                    ToastService.ShowInfo("Please select the Patient first.");
                    return;
                }

                if (InsurancePoliciyForm.Id == 0)
                    await Mediator.Send(new CreateInsurancePolicyRequest(InsurancePoliciyForm));
                else
                    await Mediator.Send(new UpdateInsurancePolicyRequest(InsurancePoliciyForm));

                await LoadData();
            }
            catch { }
        }

        #endregion SaveDelete

        #region ToolBar Button

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Name", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString().Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match the grid.");
                        return;
                    }

                    var countries = new List<CountryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new CountryDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim()
                        };

                        if (!Countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()) && !countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()))
                            countries.Add(country);
                    }

                    await Mediator.Send(new CreateListCountryRequest(countries));

                    await LoadData();
                }
                catch { }
            }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void NewItem_Click()
        {
            InsurancePoliciyForm = new();
            ShowForm = true;
        }

        private void EditItem_Click()
        {
            try
            {
                InsurancePoliciyForm = SelectedDataItems[0].Adapt<InsurancePolicyDto>();
                ShowForm = true;
            }
            catch { }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
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

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile");
        }

        #endregion ToolBar Button

        #endregion Grid Function

        #region Form

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            await OnSave();
        }

        private void HandleInvalidSubmit()
        {
            FormValidationState = false;
        }

        private void OnCancel()
        {
            InsurancePoliciyForm = new();
            ShowForm = false;
        }

        #endregion Form
    }
}
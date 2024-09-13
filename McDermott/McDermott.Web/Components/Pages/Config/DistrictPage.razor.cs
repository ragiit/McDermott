using Google.Apis.Http;
using McDermott.Web.Components.Layout;
using OfficeOpenXml.Style;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class DistrictPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    await GetUserInfo();
                }
                catch { }

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                    }
                }
                catch { }
            }
        }

        private async Task GetUserInfo()
        {
            try
            {
                var user = await UserInfoService.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        public IGrid Grid { get; set; }
        private List<DistrictDto> Districts = new();
        private List<ProvinceDto> Provinces = new();
        private List<CityDto> Cities = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private dynamic dd;
        private int Value { get; set; } = 0;
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private bool PanelVisible { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataProvince();
            await LoadDataCity();
            PanelVisible = false;
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "district_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Province",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "City",
                    Notes = "Mandatory"
                }
            ]);
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
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

                    var headerNames = new List<string>() { "Name", "Province", "City" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => !headerNames[i - 1].Trim().Equals(ws.Cells[1, i].Value?.ToString()?.Trim().ToLower(), StringComparison.CurrentCultureIgnoreCase)))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<DistrictDto>();

                    var provinceNames = new HashSet<string>();
                    var cityNames = new HashSet<string>();

                    var list1 = new List<ProvinceDto>();
                    var list2 = new List<CityDto>();

                    // First loop: Collect unique province, city, and district names
                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var prov = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var city = ws.Cells[row, 3].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(prov))
                            provinceNames.Add(prov.ToLower());

                        if (!string.IsNullOrEmpty(city))
                            cityNames.Add(city.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetProvinceQuery(x => provinceNames.Contains(x.Name), 0, 0))).Item1;
                    list2 = (await Mediator.Send(new GetCityQuery(x => cityNames.Contains(x.Name), 0, 0))).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        long? provinceId = null;
                        var province = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var city = ws.Cells[row, 3].Value?.ToString()?.Trim();

                        // Cari parent di cache atau database
                        if (!string.IsNullOrEmpty(province))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(province, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                var parentMenu = (await Mediator.Send(new GetProvinceQuery(x =>
                                      x.Name == province,
                                    searchTerm: province, pageSize: 1, pageIndex: 0))).Item1.FirstOrDefault();

                                if (parentMenu is null)
                                {
                                    isValid = false;
                                    ToastService.ShowErrorImport(row, 1, province ?? string.Empty);
                                }
                                else
                                {
                                    provinceId = parentMenu.Id;
                                    list1.Add(parentMenu);
                                }
                            }
                            else
                            {
                                provinceId = cachedParent.Id;
                            }
                        }

                        long? cityId = null;
                        if (!string.IsNullOrEmpty(city))
                        {
                            var cachedParent = list2.FirstOrDefault(x => x.Name.Equals(city, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                var parentMenu = (await Mediator.Send(new GetCityQuery(x =>
                                      x.Name == city,
                                    searchTerm: city, pageSize: 1, pageIndex: 0))).Item1.FirstOrDefault();

                                if (parentMenu is null)
                                {
                                    isValid = false;
                                    ToastService.ShowErrorImport(row, 2, city ?? string.Empty);
                                }
                                else
                                {
                                    cityId = parentMenu.Id;
                                    list2.Add(parentMenu);
                                }
                            }
                            else
                            {
                                cityId = cachedParent.Id;
                            }
                        }

                        if (!isValid)
                            continue;

                        var newMenu = new DistrictDto
                        {
                            ProvinceId = provinceId,
                            CityId = cityId,
                            Name = ws.Cells[row, 3].Value?.ToString()?.Trim(),
                        };

                        bool exists = await Mediator.Send(new ValidateDistrictQuery(x =>
                            x.Name == newMenu.Name &&
                            x.ProvinceId == newMenu.ProvinceId &&
                            x.CityId == newMenu.CityId));

                        if (!exists)
                            list.Add(newMenu);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.ProvinceId, x.Name, x.CityId }).ToList();

                        // Panggil BulkValidateVillageQuery untuk validasi bulk
                        var existingVillages = await Mediator.Send(new BulkValidateDistrictQuery(list));

                        // Filter village baru yang tidak ada di database
                        list = list.Where(village =>
                            !existingVillages.Any(ev =>
                                ev.Name == village.Name &&
                                ev.ProvinceId == village.ProvinceId &&
                                ev.CityId == village.CityId
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListDistrictRequest(list));
                        await LoadData(0, pageSize);
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadData(newPageIndex, pageSize);
        }

        #endregion Searching

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetDistrictQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            Districts = result.Item1;
            totalCount = result.pageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        #region ComboboxProvince

        private DxComboBox<ProvinceDto, long?> refProvinceComboBox { get; set; }
        private int ProvinceComboBoxIndex { get; set; } = 0;
        private int totalCountProvince = 0;

        private async Task OnSearchProvince()
        {
            await LoadDataProvince(0, 10);
        }

        private async Task OnSearchProvinceIndexIncrement()
        {
            if (ProvinceComboBoxIndex < (totalCountProvince - 1))
            {
                ProvinceComboBoxIndex++;
                await LoadDataProvince(ProvinceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProvincendexDecrement()
        {
            if (ProvinceComboBoxIndex > 0)
            {
                ProvinceComboBoxIndex--;
                await LoadDataProvince(ProvinceComboBoxIndex, 10);
            }
        }

        private async Task OnInputProvinceChanged(string e)
        {
            ProvinceComboBoxIndex = 0;
            await LoadDataProvince(0, 10);
        }

        private async Task LoadDataProvince(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProvinceQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refProvinceComboBox?.Text ?? ""));
            Provinces = result.Item1;
            totalCountProvince = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxProvince

        #region ComboboxCity

        private DxComboBox<CityDto, long?> refCityComboBox { get; set; }
        private int CityComboBoxIndex { get; set; } = 0;
        private int totalCountCity = 0;

        private async Task OnSearchCity()
        {
            await LoadDataCity(0, 10);
        }

        private async Task OnSearchCityIndexIncrement()
        {
            if (CityComboBoxIndex < (totalCountCity - 1))
            {
                CityComboBoxIndex++;
                await LoadDataCity(CityComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCityndexDecrement()
        {
            if (CityComboBoxIndex > 0)
            {
                CityComboBoxIndex--;
                await LoadDataCity(CityComboBoxIndex, 10);
            }
        }

        private async Task OnInputCityChanged(string e)
        {
            CityComboBoxIndex = 0;
            await LoadDataCity(0, 10);
        }

        private async Task LoadDataCity(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetCityQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refCityComboBox?.Text ?? ""));
            Cities = result.Item1;
            totalCountCity = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCity

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
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

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var aq = SelectedDataItems.Count;
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteDistrictRequest(((DistrictDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DistrictDto>>();
                    await Mediator.Send(new DeleteDistrictRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                await JsRuntime.InvokeVoidAsync("alert", ee.InnerException.Message); // Alert
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (DistrictDto)e.EditModel;

            bool validate = await Mediator.Send(new ValidateDistrictQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.ProvinceId == editModel.ProvinceId && x.CityId == editModel.CityId));

            if (validate)
            {
                ToastService.ShowInfo($"District with name '{editModel.Name}', province '{refProvinceComboBox.Text}' and city '{refCityComboBox.Text}' is already exists");
                e.Cancel = true;
                return;
            }

            if (editModel.Id == 0)
                await Mediator.Send(new CreateDistrictRequest(editModel));
            else
                await Mediator.Send(new UpdateDistrictRequest(editModel));

            await LoadData();
        }
    }
}
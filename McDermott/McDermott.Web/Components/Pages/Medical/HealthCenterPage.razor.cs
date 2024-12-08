namespace McDermott.Web.Components.Pages.Medical
{
    public partial class HealthCenterPage
    {
        private List<HealthCenterDto> HealthCenters = [];
        public List<CountryDto> Countries = [];
        public List<CityDto> Cities = [];
        public List<ProvinceDto> Provinces = [];

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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

        private List<string> Types = new List<string>
        {
            "Clinic"
        };

        #region Default Grid

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private Timer _timer;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

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

        #region Load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataCountry();
            PanelVisible = false;

            return;

            try
            {
                _timer = new Timer(async (_) => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await GetUserInfo();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.QueryGetHelper<HealthCenter, HealthCenterDto>(pageIndex, pageSize, searchTerm);
            HealthCenters = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion Load Data

        #region ComboBox

        #region ComboBox Sampel TypeCountry

        private DxComboBox<CountryDto, long?> refCountryComboBox { get; set; }
        private int CountryComboBoxIndex { get; set; } = 0;
        private int totalCountCountry = 0;

        private async Task OnSearchCountry()
        {
            await LoadDataCountry(0, 10);
        }

        private async Task OnSearchCountryIndexIncrement()
        {
            if (CountryComboBoxIndex < (totalCountCountry - 1))
            {
                CountryComboBoxIndex++;
                await LoadDataCountry(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCountryIndexDecrement()
        {
            if (CountryComboBoxIndex > 0)
            {
                CountryComboBoxIndex--;
                await LoadDataCountry(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnInputCountryChanged(string e)
        {
            CountryComboBoxIndex = 0;
            await LoadDataCountry(0, 10);
        }

        private async Task LoadDataCountry(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetCountryQuery
            {
                SearchTerm = refCountryComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            Countries = result.Item1;
            totalCountCountry = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Sampel TypeCountry

        #region ComboBox Sampel Province

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

        private async Task OnSearchProvinceIndexDecrement()
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
            var c = refCountryComboBox?.Value;
            var result = await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.CountryId == c,
                SearchTerm = refProvinceComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            Provinces = result.Item1;
            totalCountProvince = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Sampel Province

        #region ComboBox City

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

        private async Task OnSearchCityIndexDecrement()
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
            var c = refProvinceComboBox?.Value;
            var result = await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.ProvinceId == c,
                SearchTerm = refCityComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            Cities = result.Item1;
            totalCountCity = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox City

        #endregion ComboBox

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteHealthCenterRequest(((HealthCenterDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<HealthCenterDto>>();
                    await Mediator.Send(new DeleteHealthCenterRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (HealthCenterDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateHealthCenterRequest(editModel));
            else
                await Mediator.Send(new UpdateHealthCenterRequest(editModel));

            await LoadData();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            EditItemsEnabled = true;
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
            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as HealthCenterDto ?? new());

            await LoadComboboxEdit(a);
        }

        private async Task LoadComboboxEdit(HealthCenterDto a)
        {
            Provinces = (await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.Id == a.ProvinceId,
            })).Item1;

            Cities = (await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.Id == a.CityId,
            })).Item1;

            Countries = (await Mediator.Send(new GetCountryQuery
            {
                Predicate = x => x.Id == a.CountryId,
            })).Item1;
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
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

                    var headerNames = new List<string>() { "Name", "Type" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<HealthCenterDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new HealthCenterDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Type = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        if (!HealthCenters.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.Type.Trim().ToLower() == c?.Type?.Trim().ToLower()))
                            list.Add(c);
                    }
                }
                catch (Exception ee)
                {
                    ee.HandleException(ToastService);
                }
            }
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "NursingDiagnoses_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Type"
                },
            ]);
        }

        #endregion Default Grid
    }
}
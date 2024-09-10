

using Google.Apis.Http;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class ProvincePage
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
                    StateHasChanged();
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
            var result = await MyQuery.GetProvinces(HttpClientFactory, pageIndex, pageSize, searchTerm ?? "");
            Provinces = result.Item1;
            totalCount = result.Item2;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        #region ComboboxCountry

        private DxComboBox<CountryDto, long?> refCountryComboBox { get; set; }
        private int CountryComboBoxIndex { get; set; } = 0;
        private int totalCountCountry = 0;

        private async Task OnSearchCountry()
        {
            await LoadDataCountries(0, 10);
        }

        private async Task OnSearchCountryIndexIncrement()
        {
            if (CountryComboBoxIndex < (totalCountCountry - 1))
            {
                CountryComboBoxIndex++;
                await LoadDataCountries(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCountryndexDecrement()
        {
            if (CountryComboBoxIndex > 0)
            {
                CountryComboBoxIndex--;
                await LoadDataCountries(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnInputCountryChanged(string e)
        {
            CountryComboBoxIndex = 0;
            await LoadDataCountries(0, 10);
        }

        private async Task LoadDataCountries(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await MyQuery.GetCountries(HttpClientFactory, pageIndex, pageSize, refCountryComboBox?.Text ?? "");
            Countries = result.Item1;
            totalCountCountry = result.Item2;
            PanelVisible = false;
        }

        #endregion ComboboxCountry

        public IGrid Grid { get; set; }

        private List<CountryDto> Countries { get; set; } = [];
        private List<ProvinceDto> Provinces { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int Value { get; set; } = 0;
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private bool PanelVisible { get; set; } = false;

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var aq = SelectedDataItems.Count;
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteProvinceRequest(((ProvinceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<ProvinceDto>>();
                    await Mediator.Send(new DeleteDistrictRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            await LoadDataCountries();
            PanelVisible = false;
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "province_template.xlsx",
            [
                new()
                {
                    Column = "Country",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Code",
                    Notes = "Max Lenght 5 char",
                },
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

                    var headerNames = new List<string>() { "Country", "Name", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<ProvinceDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = Countries.FirstOrDefault(x => x.Name == ws.Cells[row, 1].Value?.ToString()?.Trim());

                        if (a is null)
                        {
                            PanelVisible = false;
                            ToastService.ShowInfo($"Country with name \"{ws.Cells[row, 1].Value?.ToString()?.Trim()}\" is not found");
                            return;
                        }

                        var c = new ProvinceDto
                        {
                            CountryId = a.Id,
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 3].Value?.ToString()?.Trim(),
                        };

                        if (!Provinces.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.CountryId == c.CountryId))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListProvinceRequest(list));

                    await LoadData();
                    SelectedDataItems = [];

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        //private async Task LoadData()
        //{
        //    PanelVisible = true;
        //    SelectedDataItems = [];
        //    Provinces = await Mediator.Send(new GetProvinceQuery());
        //    PanelVisible = false;
        //}

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

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        //protected async Task SelectedFilesChangedAsync(IEnumerable<UploadFileInfo> files)
        //{
        //    UploadVisible = files.ToList().Count == 0;
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error reading Excel file: {ex.Message}");
        //    }
        //}
        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (ProvinceDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateProvinceRequest(editModel));
            else
                await Mediator.Send(new UpdateProvinceRequest(editModel));

            await LoadData();
        }
    }
}
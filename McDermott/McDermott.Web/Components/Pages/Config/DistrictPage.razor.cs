
using Google.Apis.Http;
using McDermott.Domain.Entities;
using McDermott.Web.Components.Layout;
using OfficeOpenXml.Style;
using System.Linq.Expressions;
using System.Reactive.Subjects;

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
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<DistrictDto>();

                    var provinceNames = new HashSet<string>();
                    var cityNames = new HashSet<string>();

                    var list1 = new List<ProvinceDto>();
                    var list2 = new List<CityDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var prov = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var city = ws.Cells[row, 3].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(prov))
                            provinceNames.Add(prov.ToLower());

                        if (!string.IsNullOrEmpty(city))
                            cityNames.Add(city.ToLower());
                    }
                    list1 = (await Mediator.Send(new GetProvinceQuery
                    {
                        Predicate = x => provinceNames.Contains(x.Name.ToLower()),
                        IsGetAll = true,
                        Select = x => new Province
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    list2 = (await Mediator.Send(new GetCityQuery
                    {
                        Predicate = x => cityNames.Contains(x.Name.ToLower()),
                        IsGetAll = true,
                        Select = x => new City
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        var name = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var province = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var city = ws.Cells[row, 3].Value?.ToString()?.Trim();

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            ToastService.ShowErrorImport(row, 1, name ?? string.Empty);
                            isValid = false;
                        }

                        long? provinceId = null;
                        if (!string.IsNullOrEmpty(province))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(province, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 2, province ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                provinceId = cachedParent.Id;
                            }
                        }
                        else
                        {
                            ToastService.ShowErrorImport(row, 2, province ?? string.Empty);
                            isValid = false;
                        }

                        long? cityId = null;
                        if (!string.IsNullOrEmpty(city))
                        {
                            var cachedParent = list2.FirstOrDefault(x => x.Name.Equals(city, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                isValid = false;
                                ToastService.ShowErrorImport(row, 3, city ?? string.Empty);
                            }
                            else
                            {
                                cityId = cachedParent.Id;
                            }
                        }
                        else
                        {
                            ToastService.ShowErrorImport(row, 3, city ?? string.Empty);
                            isValid = false;
                        }

                        if (!isValid)
                            continue;

                        var newMenu = new DistrictDto
                        {
                            ProvinceId = provinceId,
                            CityId = cityId,
                            Name = name,
                        };

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
                finally { PanelVisible = false; }
            }
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
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetDistrictQuery
                {
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                Districts = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            await LoadProvince();
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            try
            {
                PanelVisible = true;

                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
                var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DistrictDto ?? new());
                await LoadProvince(predicate: x => x.Id == a.ProvinceId);
                await LoadCity(predicate: x => x.Id == a.CityId && x.ProvinceId == a.ProvinceId);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
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
                await LoadData(0, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (DistrictDto)e.EditModel;

                bool validate = await Mediator.Send(new ValidateDistrictQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.ProvinceId == editModel.ProvinceId && x.CityId == editModel.CityId));

                if (validate)
                {
                    ToastService.ShowInfo($"District with name '{editModel.Name}', province  and city   is already exists");
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateDistrictRequest(editModel));
                else
                    await Mediator.Send(new UpdateDistrictRequest(editModel));

                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #region ComboBox Province
        private ProvinceDto SelectedProvince { get; set; } = new();
        async Task SelectedItemChanged(ProvinceDto e)
        {
            if (e is null)
                SelectedProvince = new();
            else
            {
                SelectedProvince = e;
                await LoadCity();
            }
        }

        private CancellationTokenSource? _ctsProvince;
        private async Task OnInputProvince(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _ctsProvince?.Cancel();
                _ctsProvince?.Dispose();
                _ctsProvince = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsProvince.Token);

                await LoadProvince(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _ctsProvince?.Dispose();
                _ctsProvince = null;
            }
        }

        private async Task LoadProvince(string? e = "", Expression<Func<Province, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                Provinces = await Mediator.QueryGetComboBox<Province, ProvinceDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion

        #region ComboBox City
        private CityDto SelectedCity { get; set; } = new();
        async Task SelectedItemCityChanged(CityDto e)
        {
            if (e is null)
                SelectedCity = new();
            else
            {
                SelectedCity = e;
                await LoadCity();
            }
        }
        private CancellationTokenSource? _ctsCity;
        private async Task OnInputCity(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _ctsCity?.Cancel();
                _ctsCity?.Dispose();
                _ctsCity = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsCity.Token);

                await LoadCity(e.Value?.ToString() ?? "", x => x.ProvinceId == SelectedProvince.Id);
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _ctsCity?.Dispose();
                _ctsCity = null;
            }
        }

        private async Task LoadCity(string? e = "", Expression<Func<City, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                Cities = await Mediator.QueryGetComboBox<City, CityDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion

       
    }
}
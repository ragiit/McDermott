using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Domain.Entities;
using McDermott.Web.Components.Layout;
using Microsoft.AspNetCore.HttpLogging;
using System.Linq.Expressions;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class VillagePage
    {
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
                var result = await Mediator.QueryGetHelper<Village, VillageDto>(pageIndex, pageSize, searchTerm);
                Villages = result.Item1;
                totalCount = result.pageCount;
                activePageIndex = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

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
                await GetUserInfo();
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
        private List<ProvinceDto> Provinces = [];
        private List<DistrictDto> Districts = [];
        private List<CountryDto> Countrys = [];
        private List<CityDto> Cities = [];
        private List<VillageDto> Villages = [];

        //private object Data { get; set; }
        private IEnumerable<VillageDto> Data { get; set; } = [];

        private bool PanelVisible { get; set; } = true;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadProvince();
            PanelVisible = false;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
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
            try
            {
                PanelVisible = true;
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
                var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());
                await LoadProvince(predicate: x => x.Id == a.ProvinceId);
                await LoadCity(predicate: x => x.Id == a.CityId && x.ProvinceId == a.ProvinceId);
                await LoadDistrict(predicate: x => x.Id == a.DistrictId && x.CityId == a.CityId && x.ProvinceId == a.ProvinceId);
                PanelVisible = false;
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
                var aq = SelectedDataItems.Count;
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteVillageRequest(((VillageDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<VillageDto>>();
                    await Mediator.Send(new DeleteDistrictRequest(ids: a.Select(x => x.Id).ToList()));
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
            var editModel = (VillageDto)e.EditModel;

            bool validate = await Mediator.Send(new ValidateVillageQuery(x => x.Id != editModel.Id && x.DistrictId == editModel.DistrictId && x.PostalCode == editModel.PostalCode && x.Name == editModel.Name && x.ProvinceId == editModel.ProvinceId && x.CityId == editModel.CityId));

            if (validate)
            {
                ToastService.ShowInfo($"Village with name '{editModel.Name}', postal code '{editModel.PostalCode}', province, city and district is already exists");
                e.Cancel = true;
                return;
            }

            if (editModel.Id == 0)
                await Mediator.Send(new CreateVillageRequest(editModel));
            else
                await Mediator.Send(new UpdateVillageRequest(editModel));

            await LoadData();
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            PanelVisible = true;

            long maxAllowedSize = 3 * 1024 * 1024; // 2MB
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream(maxAllowedSize).CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Name", "Postal Code", "Province", "City", "District" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var provinceNames = new HashSet<string>();
                    var cityNames = new HashSet<string>();
                    var districtNames = new HashSet<string>();

                    var list = new List<VillageDto>();
                    var list1 = new List<ProvinceDto>();
                    var list2 = new List<CityDto>();
                    var list3 = new List<DistrictDto>();

                    // First loop: Collect unique province, city, and district names
                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var prov = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var city = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        var district = ws.Cells[row, 5].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(prov))
                            provinceNames.Add(prov.ToLower());

                        if (!string.IsNullOrEmpty(city))
                            cityNames.Add(city.ToLower());

                        if (!string.IsNullOrEmpty(district))
                            districtNames.Add(district.ToLower());
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

                    list3 = (await Mediator.Send(new GetDistrictQuery
                    {
                        Predicate = x => districtNames.Contains(x.Name.ToLower()),
                        IsGetAll = true,
                        Select = x => new District
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var name = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var code = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var prov = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var city = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        var district = ws.Cells[row, 5].Value?.ToString()?.Trim();

                        long? provinceId = null;
                        long? cityId = null;
                        long? districtId = null;
                        bool isValid = true;

                        if (!string.IsNullOrEmpty(prov))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(prov, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 3, prov ?? string.Empty);
                            }
                            else
                            {
                                provinceId = cachedParent.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(city))
                        {
                            var cachedParent = list2.FirstOrDefault(x => x.Name.Equals(city, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 4, city ?? string.Empty);
                            }
                            else
                            {
                                cityId = cachedParent.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(district))
                        {
                            var cachedParent = list3.FirstOrDefault(x => x.Name.Equals(district, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 5, district ?? string.Empty);
                            }
                            else
                            {
                                districtId = cachedParent.Id;
                            }
                        }

                        if (!isValid)
                            continue;

                        var newMenu = new VillageDto
                        {
                            ProvinceId = provinceId,
                            CityId = cityId,
                            DistrictId = districtId,
                            Name = name,
                            PostalCode = code,
                        };

                        list.Add(newMenu);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.ProvinceId, x.Name, x.CityId, x.DistrictId, x.PostalCode }).ToList();

                        // Panggil BulkValidateVillageQuery untuk validasi bulk
                        var existingVillages = await Mediator.Send(new BulkValidateVillageQuery(list));

                        // Filter village baru yang tidak ada di database
                        list = list.Where(village =>
                            !existingVillages.Any(ev =>
                                ev.Name == village.Name &&
                                ev.PostalCode == village.PostalCode &&
                                ev.ProvinceId == village.ProvinceId &&
                                ev.CityId == village.CityId &&
                                ev.DistrictId == village.DistrictId
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListVillageRequest(list));
                        await LoadData(0, pageSize);
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
                finally
                {
                    PanelVisible = false;
                }
            }
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "village_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Postal Code"
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
                },
                new()
                {
                    Column = "District",
                    Notes = "Mandatory"
                },
            ]);
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        #region ComboBox Province

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

        #region ComboBox District

        private CancellationTokenSource? _ctsDistrict;
        private async Task OnInputDistrict(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _ctsDistrict?.Cancel();
                _ctsDistrict?.Dispose();
                _ctsDistrict = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsDistrict.Token);

                await LoadDistrict(e.Value?.ToString() ?? "", x => x.CityId == SelectedCity.Id && x.ProvinceId == x.ProvinceId);
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _ctsDistrict?.Dispose();
                _ctsDistrict = null;
            }
        }

        private async Task LoadDistrict(string? e = "", Expression<Func<District, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                Districts = await Mediator.QueryGetComboBox<District, DistrictDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion

        private ProvinceDto SelectedProvince { get; set; } = new();
        void SelectedItemChanged(ProvinceDto e)
        {
            if (e is null)
                SelectedProvince = new();
            else
                SelectedProvince = e;
        }

        private CityDto SelectedCity { get; set; } = new();
        void SelectedItemCityChanged(CityDto e)
        {
            if (e is null)
                SelectedCity = new();
            else
                SelectedCity = e;
        }
    }
}
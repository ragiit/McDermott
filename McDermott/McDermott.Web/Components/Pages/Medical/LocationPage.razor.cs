using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using static McDermott.Application.Features.Commands.Medical.LocationCommand;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class LocationPage
    {
        public List<LocationDto> Locations = [];
        public List<LocationDto> ParentLocations = [];
        public List<CompanyDto> Companies = [];

        private List<string> Types = new()
        {
            "Internal Location"
        };

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

        #region Default Grid

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private Timer _timer;
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

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

        #region Load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataParentLocations();
            await LoadDataCompanies();
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
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetLocationQuery
                {
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                Locations = result.Item1;
                activePageIndex = pageIndex;
                totalCount = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Load Data

        #region Load ComboBox

        private DxComboBox<LocationDto, long?> refParentLocationsComboBox { get; set; }
        private int ParentLocationsComboBoxIndex { get; set; } = 0;
        private int totalCountParentLocations = 0;

        private async Task OnSearchParentLocations()
        {
            await LoadDataParentLocations(0, 10);
        }

        private async Task OnSearchParentLocationsIndexIncrement()
        {
            if (ParentLocationsComboBoxIndex < (totalCountParentLocations - 1))
            {
                ParentLocationsComboBoxIndex++;
                await LoadDataParentLocations(ParentLocationsComboBoxIndex, 10);
            }
        }

        private async Task OnSearchParentLocationsndexDecrement()
        {
            if (ParentLocationsComboBoxIndex > 0)
            {
                ParentLocationsComboBoxIndex--;
                await LoadDataParentLocations(ParentLocationsComboBoxIndex, 10);
            }
        }

        private async Task OnInputParentLocationsChanged(string e)
        {
            ParentLocationsComboBoxIndex = 0;
            await LoadDataParentLocations(0, 10);
        }

        private async Task LoadDataParentLocations(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetLocationQuery
                {
                    Predicate = x => x.ParentLocationId != null,
                    SearchTerm = refParentLocationsComboBox?.Text ?? "",
                    PageSize = pageSize,
                    PageIndex = pageIndex
                });

                if (result.Item1 != null)
                {
                    ParentLocations = [.. result.Item1.Where(x => x.ParentLocationId is not null).OrderBy(x => x.Name)];
                    totalCountParentLocations = result.PageCount;
                }
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private DxComboBox<CompanyDto, long?> refCompaniesComboBox { get; set; }
        private int CompaniesComboBoxIndex { get; set; } = 0;
        private int totalCountCompanies = 0;

        private async Task OnSearchCompanies()
        {
            await LoadDataCompanies(0, 10);
        }

        private async Task OnSearchCompaniesIndexIncrement()
        {
            if (CompaniesComboBoxIndex < (totalCountCompanies - 1))
            {
                CompaniesComboBoxIndex++;
                await LoadDataCompanies(CompaniesComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCompaniesndexDecrement()
        {
            if (CompaniesComboBoxIndex > 0)
            {
                CompaniesComboBoxIndex--;
                await LoadDataCompanies(CompaniesComboBoxIndex, 10);
            }
        }

        private async Task OnInputCompaniesChanged(string e)
        {
            CompaniesComboBoxIndex = 0;
            await LoadDataCompanies(0, 10);
        }

        private async Task LoadDataCompanies(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetCompanyQuery
                {
                    SearchTerm = refCompaniesComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                Companies = result.Item1;
                totalCountCompanies = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Load ComboBox

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems is not null && SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteLocationRequest(((LocationDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<LocationDto>>();
                    await Mediator.Send(new DeleteLocationRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
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
                var editModel = (LocationDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateLocationRequest(editModel));
                else
                    await Mediator.Send(new UpdateLocationRequest(editModel));

                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
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
            try
            {
                PanelVisible = true;
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
                var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as LocationDto ?? new());
                ParentLocations = (await Mediator.QueryGetHelper<Locations, LocationDto>(predicate: x => x.Id == a.ParentLocationId)).Item1;

                ParentLocations = (await Mediator.Send(new GetLocationQuery
                {
                    Predicate = x => x.Id == a.ParentLocationId
                })).Item1;

                Companies = (await Mediator.QueryGetHelper<Company, CompanyDto>(predicate: x => x.Id == a.ParentLocationId)).Item1;
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

        #endregion Default Grid

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

                    //if (Enumerable.Range(1, ws.Dimension.End.Column)
                    //    .Any(i => ExportTemp.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    //{
                    //    PanelVisible = false;
                    //    ToastService.ShowInfo("The header must match with the template.");
                    //    return;
                    //}
                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                      .Any(i => ExportTemp.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<LocationDto>();

                    var a = new HashSet<string>();
                    var b = new HashSet<string>();

                    var list1 = new List<LocationDto>();
                    var list2 = new List<CompanyDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var aa = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var bb = ws.Cells[row, 4].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(aa))
                            a.Add(aa.ToLower());

                        if (!string.IsNullOrEmpty(bb))
                            b.Add(bb.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetLocationQuery
                    {
                        Predicate = x => a.Contains(x.Name.ToLower()),
                        IsGetAll = true,
                    })).Item1;

                    list2 = (await Mediator.Send(new GetCompanyQuery
                    {
                        Predicate = x => b.Contains(x.Name.ToLower()),
                        IsGetAll = true,
                        Select = x => new Company
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var par = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var type = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var com = ws.Cells[row, 4].Value?.ToString()?.Trim();

                        bool isValid = true;

                        long? parId = null;
                        if (!string.IsNullOrEmpty(par))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(par, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                isValid = false;
                                ToastService.ShowErrorImport(row, 2, par ?? string.Empty);
                            }
                            else
                            {
                                parId = cachedParent.Id;
                            }
                        }

                        long? compId = null;
                        if (!string.IsNullOrEmpty(com))
                        {
                            var cachedParent = list2.FirstOrDefault(x => x.Name.Equals(com, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                isValid = false;
                                ToastService.ShowErrorImport(row, 4, com ?? string.Empty);
                            }
                            else
                            {
                                compId = cachedParent.Id;
                            }
                        }

                        if (!isValid)
                            continue;

                        var c = new LocationDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            ParentLocationId = parId,
                            Type = type,
                            CompanyId = compId,
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.ParentLocationId, x.Type, x.CompanyId }).ToList();

                        // Panggil BulkValidateLocationQuery untuk validasi bulk
                        var existingLocations = await Mediator.Send(new BulkValidateLocationsQuery(list));

                        // Filter Location baru yang tidak ada di database
                        list = list.Where(Location =>
                            !existingLocations.Any(ev =>
                                ev.Name == Location.Name &&
                                ev.ParentLocationId == Location.ParentLocationId &&
                                ev.Type == Location.Type &&
                                ev.CompanyId == Location.CompanyId
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListLocationRequest(list));
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

        private List<ExportFileData> ExportTemp =
        [
            new() { Column = "Name", Notes = "Mandatory" },
            new() { Column = "Parent"},
            new() { Column = "Type", Notes = "Internal Location"},
            new() { Column = "Company"},
        ];

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "location_template.xlsx", ExportTemp);
        }
    }
}
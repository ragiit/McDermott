using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Web.Components.Layout;
using System.Linq.Expressions;
using System.Reactive.Subjects;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class ProvincePage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess { get; set; } = false;

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
                catch (Exception)
                {
                    // Handle exception if necessary
                }
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
            catch (Exception)
            {
                // Handle exception if necessary
            }
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
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetProvinceQuery
                {
                    OrderByList =
                    [
                        (x => x.Name, true)
                    ],
                    SearchTerm = searchTerm,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                });
                Provinces = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }


        #region ComboBox Country

        private CountryDto SelectedCountry { get; set; } = new();
        async Task SelectedItemChanged(CountryDto e)
        {
            if (e is null)
            {
                SelectedCountry = new();
                await LoadCounty();
            }
            else
                SelectedCountry = e;
        }

        private CancellationTokenSource? _cts;
        private async Task OnInputCountry(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadCounty(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadCounty(string? e = "", Expression<Func<Country, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                Countries = await Mediator.QueryGetComboBox<Country, CountryDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion

        public IGrid Grid { get; set; }

        private List<CountryDto> Countries { get; set; } = new();
        private List<ProvinceDto> Provinces { get; set; } = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = Array.Empty<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private bool PanelVisible { get; set; } = false;

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems.Count == 0)
                {
                    await Mediator.Send(new DeleteProvinceRequest(((ProvinceDto)e.DataItem).Id));
                }
                else
                {
                    var selectedProvinces = SelectedDataItems.Adapt<List<ProvinceDto>>();
                    await Mediator.Send(new DeleteProvinceRequest(ids: selectedProvinces.Select(x => x.Id).ToList()));
                }

                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            PanelVisible = false;
        }

        private async Task NewItem_Click()
        {
            await LoadCounty();
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
                var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new());
                await LoadCounty(predicate: x => x.Id == a.CountryId);
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

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (ProvinceDto)e.EditModel;

                bool exists = await Mediator.Send(new ValidateProvinceQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.CountryId == editModel.CountryId));
                if (exists)
                {
                    ToastService.ShowInfo($"Province with name '{editModel.Name}' and country is already exists.");
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                {
                    await Mediator.Send(new CreateProvinceRequest(editModel));
                }
                else
                {
                    await Mediator.Send(new UpdateProvinceRequest(editModel));
                }

                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "province_template.xlsx",
            [
                new() { Column = "Country", Notes = "Mandatory" },
                new() { Column = "Name", Notes = "Mandatory" }
            ]);
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

                    var headerNames = new List<string>() { "Country", "Name" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<ProvinceDto>();

                    var list1 = new List<CountryDto>();
                    var countryNames = new HashSet<string>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var prov = ws.Cells[row, 1].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(prov))
                            countryNames.Add(prov.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetCountryQuery
                    {
                        IsGetAll = true,
                        Predicate = x => countryNames.Contains(x.Name.ToLower()),
                        Select = x => new Country
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var countryName = ws.Cells[row, 1].Value?.ToString()?.Trim();

                        long? parentId = null;
                        if (!string.IsNullOrEmpty(countryName))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name == countryName);
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 1, countryName ?? string.Empty);
                            }
                            else
                            {
                                parentId = cachedParent.Id;
                            }
                        }

                        var newMenu = new ProvinceDto
                        {
                            CountryId = parentId,
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        list.Add(newMenu);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.CountryId, x.Name }).ToList();

                        // Panggil BulkValidateVillageQuery untuk validasi bulk
                        var existingVillages = await Mediator.Send(new BulkValidateProvinceQuery(list));

                        // Filter village baru yang tidak ada di database
                        list = list.Where(village =>
                            !existingVillages.Any(ev =>
                                ev.Name == village.Name &&
                                ev.CountryId == village.CountryId
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListProvinceRequest(list));
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
            PanelVisible = false;
        }
    }
}
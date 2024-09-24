using McDermott.Application.Features.Services;
using McDermott.Web.Components.Layout;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class CountryPage : IDisposable
    {
        private List<CountryDto> Countries = new();
        private Timer _timer;

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

        #region Grid

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            await GetUserInfo();
            PanelVisible = false;
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var countries = await Mediator.Send(new GetCountryQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            Countries = countries.Item1;
            totalCount = countries.Item4;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion Grid

        #region ImportExport

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "country_template.xlsx", new List<ExportFileData>
        {
            new() { Column = "Name", Notes = "Mandatory" },
            new() { Column = "Code"},
        });
        }

        public async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jsRuntime, IFileExportService file, string fileName, DotNetStreamReference streamReference, List<ExportFileData> data, string name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jsRuntime.InvokeVoidAsync("saveFileExcellExporrt", fileName, streamRef);
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

                    var headerNames = new List<string>() { "Name", "Code", };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<CountryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new CountryDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        list.Add(country);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Code, }).ToList();

                        // Panggil BulkValidateCountryQuery untuk validasi bulk
                        var existingCountrys = await Mediator.Send(new BulkValidateCountryQuery(list));

                        // Filter Country baru yang tidak ada di database
                        list = list.Where(Country =>
                            !existingCountrys.Any(ev =>
                                ev.Name == Country.Name &&
                                ev.Code == Country.Code
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListCountryRequest(list));
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

        #endregion ImportExport

        #region Grid Events

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteCountryRequest(((CountryDto)e.DataItem).Id));
                }
                else
                {
                    var countriesToDelete = SelectedDataItems.Adapt<List<CountryDto>>();
                    await Mediator.Send(new DeleteCountryRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                }

                SelectedDataItems = [];
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
                var editModel = (CountryDto)e.EditModel;

                bool validate = await Mediator.Send(new ValidateCountryQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.Code == editModel.Code));

                if (validate)
                {
                    ToastService.ShowInfo($"Country with name '{editModel.Name}' and code '{editModel.Code}' is already exists");
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCountryRequest(editModel));
                else
                    await Mediator.Send(new UpdateCountryRequest(editModel));

                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Grid Events
    }
}
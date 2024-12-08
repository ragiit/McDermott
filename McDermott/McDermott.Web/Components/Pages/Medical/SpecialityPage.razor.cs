using McDermott.Application.Features.Services;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class SpecialityPage
    {
        private List<string> extentions = new() { ".xlsx", ".xls" };
        private const string ExportFileName = "ExportResult";
        private IEnumerable<GridEditMode> GridEditModes { get; } = Enum.GetValues<GridEditMode>();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private dynamic dd;
        private Timer _timer;
        private long Value { get; set; } = 0;
        private bool PanelVisible { get; set; } = true;
        private string textPopUp = "";
        private int FocusedRowVisibleIndex { get; set; }

        public IGrid Grid { get; set; }
        private List<SpecialityDto> Specialitys = new();

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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            await GetUserInfo();
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
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetSpecialityQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
                Specialitys = result.Item1;
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

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "speciality_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Code"
                },
            ]);
        }

        public async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jSRuntime, IFileExportService file, string fileName, DotNetStreamReference streamReference, List<ExportFileData> data, string? name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteSpecialityRequest(((SpecialityDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<SpecialityDto>>();
                    await Mediator.Send(new DeleteSpecialityRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private bool EditItemsEnabled { get; set; }

        private async Task NewItem_Click()
        {
            textPopUp = "Add Data Speciality";
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private bool UploadVisible { get; set; } = false;

        protected async Task SelectedFilesChangedAsync(IEnumerable<UploadFileInfo> files)
        {
            UploadVisible = files.ToList().Count == 0;
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading Excel file: {ex.Message}");
            }
        }

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task EditItem_Click()
        {
            textPopUp = "Edit Data Speciality";
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
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

                    var headerNames = new List<string>() { "Name", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<SpecialityDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new SpecialityDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Code, }).ToList();

                        // Panggil BulkValidateSpecialityQuery untuk validasi bulk
                        var existingSpecialitys = await Mediator.Send(new BulkValidateSpecialityQuery(list));

                        // Filter Speciality baru yang tidak ada di database
                        list = list.Where(Speciality =>
                            !existingSpecialitys.Any(ev =>
                                ev.Name == Speciality.Name &&
                                ev.Code == Speciality.Code
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListSpecialityRequest(list));
                        await LoadData(0, pageSize);
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
                finally { PanelVisible = false; }
            }
            PanelVisible = false;
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (SpecialityDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateSpecialityRequest(editModel));
                else
                    await Mediator.Send(new UpdateSpecialityRequest(editModel));

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }
    }
}
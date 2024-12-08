using System.ComponentModel.DataAnnotations;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class CronisCategoryPage
    {
        private bool PanelVisible { get; set; } = true;
        private List<string> extentions = new() { ".xlsx", ".xls" };
        private const string ExportFileName = "ExportResult";
        private IEnumerable<GridEditMode> GridEditModes { get; } = Enum.GetValues<GridEditMode>();
        private List<CronisCategoryDto> Chronises = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private dynamic dd;
        private long Value { get; set; } = 0;
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        public IGrid Grid { get; set; }

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

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
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

                    var headerNames = new List<string>() { "Name", "Description" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<CronisCategoryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new CronisCategoryDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Description = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Description, }).ToList();

                        // Panggil BulkValidateCronisCategoryQuery untuk validasi bulk
                        var existingCronisCategorys = await Mediator.Send(new BulkValidateCronisCategoryQuery(list));

                        // Filter CronisCategory baru yang tidak ada di database
                        list = list.Where(CronisCategory =>
                            !existingCronisCategorys.Any(ev =>
                                ev.Name == CronisCategory.Name &&
                                ev.Description == CronisCategory.Description
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListCronisCategoryRequest(list));
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

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "cronis_category_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Description"
                },
            ]);
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
            var result = await Mediator.Send(new GetCronisCategoryQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = searchTerm ?? ""
            });
            Chronises = result.Item1;
            totalCount = result.PageCount;
            activePageIndex = pageIndex;
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

        private void OnFileUploadStarted(FileUploadEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public enum EducationType
        {
            [Display(Name = "Not Stated")]
            NoInfo = 0,

            [Display(Name = "High school")]
            School = 1,

            [Display(Name = "College")]
            College = 2,

            [Display(Name = "University Degree")]
            UniversityDegree = 3,

            [Display(Name = "PhD")]
            PhD = 4
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteCronisCategoryRequest(((CronisCategoryDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<CronisCategoryDto>>();
                    await Mediator.Send(new DeleteCronisCategoryRequest(ids: a.Select(x => x.Id).ToList()));
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
            var editModel = (CronisCategoryDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateCronisCategoryRequest(editModel));
            else
                await Mediator.Send(new UpdateCronisCategoryRequest(editModel));

            await LoadData();
        }
    }
}
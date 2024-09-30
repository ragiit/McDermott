namespace McDermott.Web.Components.Pages.Employee
{
    public partial class JobPositionPage
    {
        private List<JobPositionDto> JobPositions = [];
        public List<DepartmentDto> Departments = [];

        #region Default Grid

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

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteJobPositionRequest(((JobPositionDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<JobPositionDto>>();
                    await Mediator.Send(new DeleteJobPositionRequest(ids: a.Select(x => x.Id).ToList()));
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
                var editModel = (JobPositionDto)e.EditModel;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateJobPositionRequest(editModel));
                else
                    await Mediator.Send(new UpdateJobPositionRequest(editModel));

                await LoadData();
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

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var a = await Mediator.Send(new GetJobPositionQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
                JobPositions = a.Item1;
                totalCount = a.pageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

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

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => ExportTemp.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<JobPositionDto>();

                    var aa = new HashSet<string>();

                    var list1 = new List<DepartmentDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 2].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            aa.Add(a.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetDepartmentQuery(x => aa.Contains(x.Name.ToLower()), 0, 0))).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        var parent = ws.Cells[row, 2].Value?.ToString()?.Trim();

                        long? depId = null;

                        if (!string.IsNullOrWhiteSpace(parent))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(parent, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 2, parent ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                depId = cachedParent.Id;
                            }
                        }

                        if (!isValid)
                            continue;

                        var c = new JobPositionDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            DepartmentId = depId
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.DepartmentId }).ToList();

                        var existingDepartments = await Mediator.Send(new BulkValidateJobPositionQuery(list));

                        // Filter Department baru yang tidak ada di database
                        list = list.Where(Department =>
                            !existingDepartments.Any(ev =>
                                ev.Name == Department.Name &&
                                ev.DepartmentId == Department.DepartmentId)
                        ).ToList();

                        await Mediator.Send(new CreateListJobPositionRequest(list));
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
            new() { Column = "Department"}
        ];

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "job_position_template.xlsx", ExportTemp);
        }

        #region ComboboxDepartment

        private DxComboBox<DepartmentDto, long?> refDepartmentComboBox { get; set; }
        private int DepartmentComboBoxIndex { get; set; } = 0;
        private int totalCountDepartment = 0;

        private async Task OnSearchDepartment()
        {
            await LoadDataDepartment();
        }

        private async Task OnSearchDepartmentIndexIncrement()
        {
            if (DepartmentComboBoxIndex < (totalCountDepartment - 1))
            {
                DepartmentComboBoxIndex++;
                await LoadDataDepartment(DepartmentComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDepartmentndexDecrement()
        {
            if (DepartmentComboBoxIndex > 0)
            {
                DepartmentComboBoxIndex--;
                await LoadDataDepartment(DepartmentComboBoxIndex, 10);
            }
        }

        private async Task OnInputDepartmentChanged(string e)
        {
            DepartmentComboBoxIndex = 0;
            await LoadDataDepartment();
        }

        private async Task LoadDataDepartment(int pageIndex = 0, int pageSize = 10, long? DepartmentId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetDepartmentQuery(DepartmentId == null ? null : x => x.Id == DepartmentId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDepartmentComboBox?.Text ?? ""));
            Departments = result.Item1;
            totalCountDepartment = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDepartment
    }
}
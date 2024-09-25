using DocumentFormat.OpenXml.Drawing.Charts;
using McDermott.Domain.Entities;

namespace McDermott.Web.Components.Pages.Employee
{
    public partial class DepartmentPage
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

        public List<DepartmentDto> Departments = [];
        public List<DepartmentDto> AllParentDepartments = [];
        public List<DepartmentDto> ParentDepartments = [];
        public List<CompanyDto> Companies = [];
        private List<UserDto> AllUsers = [];
        private List<UserDto> Users = [];

        private long UpdateUserId { get; set; }
        private long? SelectedUserId { get; set; }

        private List<string> DepartmentCategories = new()
        {
            "Department",
            "Unit",
        };

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
            await LoadCombobox();
        }

        private async Task LoadCombobox()
        {
            await LoadDataParentDepartment();
            await LoadDataCompany();
            await LoadDataUser();
        }

        #region ComboboxUser

        private DxComboBox<UserDto, long?> refUserComboBox { get; set; }
        private int UserComboBoxIndex { get; set; } = 0;
        private int totalCountUser = 0;

        private async Task OnSearchUser()
        {
            await LoadDataUser();
        }

        private async Task OnSearchUserIndexIncrement()
        {
            if (UserComboBoxIndex < (totalCountUser - 1))
            {
                UserComboBoxIndex++;
                await LoadDataUser(UserComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUserndexDecrement()
        {
            if (UserComboBoxIndex > 0)
            {
                UserComboBoxIndex--;
                await LoadDataUser(UserComboBoxIndex, 10);
            }
        }

        private async Task OnInputUserChanged(string e)
        {
            UserComboBoxIndex = 0;
            await LoadDataUser();
        }

        private async Task LoadDataUser(int pageIndex = 0, int pageSize = 10, long? UserId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetUserQuery2(z => z.IsEmployee == true, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refUserComboBox?.Text ?? ""));
            Users = result.Item1;
            totalCountUser = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxUser

        #region ComboboxCompany

        private DxComboBox<CompanyDto, long?> refCompanyComboBox { get; set; }
        private int CompanyComboBoxIndex { get; set; } = 0;
        private int totalCountCompany = 0;

        private async Task OnSearchCompany()
        {
            await LoadDataCompany();
        }

        private async Task OnSearchCompanyIndexIncrement()
        {
            if (CompanyComboBoxIndex < (totalCountCompany - 1))
            {
                CompanyComboBoxIndex++;
                await LoadDataCompany(CompanyComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCompanyndexDecrement()
        {
            if (CompanyComboBoxIndex > 0)
            {
                CompanyComboBoxIndex--;
                await LoadDataCompany(CompanyComboBoxIndex, 10);
            }
        }

        private async Task OnInputCompanyChanged(string e)
        {
            CompanyComboBoxIndex = 0;
            await LoadDataCompany();
        }

        private async Task LoadDataCompany(int pageIndex = 0, int pageSize = 10, long? CompanyId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetCompanyQuery(CompanyId == null ? null : x => x.Id == CompanyId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refCompanyComboBox?.Text ?? ""));
            Companies = result.Item1;
            totalCountCompany = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCompany

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
                var a = await Mediator.Send(new GetDepartmentQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
                Departments = a.Item1;
                totalCount = a.pageCount;
                activePageIndex = pageIndex;
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

                    var list = new List<DepartmentDto>();

                    var parentNames = new HashSet<string>();
                    var companyNames = new HashSet<string>();
                    var managerNames = new HashSet<string>();

                    var list1 = new List<DepartmentDto>();
                    var list2 = new List<CompanyDto>();
                    var list3 = new List<UserDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var b = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var c = ws.Cells[row, 4].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            parentNames.Add(a.ToLower());

                        if (!string.IsNullOrEmpty(b))
                            companyNames.Add(b.ToLower());

                        if (!string.IsNullOrEmpty(c))
                            managerNames.Add(c.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetDepartmentQuery(x => parentNames.Contains(x.Name.ToLower()), 0, 0))).Item1;
                    list2 = (await Mediator.Send(new GetCompanyQuery(x => companyNames.Contains(x.Name.ToLower()), 0, 0))).Item1;
                    list3 = (await Mediator.Send(new GetUserQuery2(x => managerNames.Contains(x.Name.ToLower()), 0, 0))).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;
                        var parent = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var comp = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var manag = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        var categ = ws.Cells[row, 5].Value?.ToString()?.Trim();

                        long? parentId = null;
                        long? compId = null;
                        long? managId = null;

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
                                parentId = cachedParent.Id;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(comp))
                        {
                            var cachedParent = list2.FirstOrDefault(x => x.Name.Equals(comp, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 3, comp ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                compId = cachedParent.Id;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(manag))
                        {
                            var cachedParent = list3.FirstOrDefault(x => x.Name.Equals(manag, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 4, manag ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                managId = cachedParent.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(categ) && !DepartmentCategories.Contains(categ))
                        {
                            isValid = false;
                            ToastService.ShowErrorImport(row, 5, categ ?? string.Empty);
                        }

                        if (!isValid)
                            continue;

                        var c = new DepartmentDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            ParentDepartmentId = parentId,
                            CompanyId = compId,
                            ManagerId = managId,
                            DepartmentCategory = categ
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.ParentDepartmentId, x.CompanyId, x.ManagerId, x.DepartmentCategory }).ToList();

                        var existingDepartments = await Mediator.Send(new BulkValidateDepartmentQuery(list));

                        // Filter Department baru yang tidak ada di database
                        list = list.Where(Department =>
                            !existingDepartments.Any(ev =>
                                ev.Name == Department.Name &&
                                ev.ParentDepartmentId == Department.ParentDepartmentId &&
                                ev.ManagerId == Department.ManagerId &&
                                ev.DepartmentCategory == Department.DepartmentCategory
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListDepartmentRequest(list));
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
            new() { Column = "Company"},
            new() { Column = "Manager"},
            new() { Column = "Category", Notes = "Select one: Department/Unit"},
        ];

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "department_template.xlsx", ExportTemp);
        }

        #region Default Grid

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
                    await Mediator.Send(new DeleteDepartmentRequest(((DepartmentDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DepartmentDto>>();
                    await Mediator.Send(new DeleteDepartmentRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData(0, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (DepartmentDto)e.EditModel;

                if (editModel.Id == 0)
                {
                    var result = await Mediator.Send(new CreateDepartmentRequest(editModel));
                    editModel.Id = result.Id;
                }
                else
                {
                    var user = await Mediator.Send(new GetUserQuery(x => x.Id == UpdateUserId));

                    if (user.Count > 0)
                    {
                        user[0].DepartmentId = null;
                        await Mediator.Send(new UpdateUserRequest(user[0]));
                    }

                    await Mediator.Send(new UpdateDepartmentRequest(editModel));
                }
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

            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DepartmentDto ?? new());

            await LoadComboboxEdit(a);
        }

        private async Task LoadComboboxEdit(DepartmentDto a)
        {
            ParentDepartments = (await Mediator.Send(new GetDepartmentQuery(x => x.Id == a.ParentDepartmentId))).Item1;
            Companies = (await Mediator.Send(new GetCompanyQuery(x => x.Id == a.CompanyId))).Item1;
            Users = (await Mediator.Send(new GetUserQuery2(x => x.Id == a.ManagerId))).Item1;
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #endregion Default Grid

        #region ComboboxParentDepartment

        private DxComboBox<DepartmentDto, long?> refParentDepartmentComboBox { get; set; }
        private int ParentDepartmentComboBoxIndex { get; set; } = 0;
        private int totalCountParentDepartment = 0;

        private async Task OnSearchParentDepartment()
        {
            await LoadDataParentDepartment();
        }

        private async Task OnSearchParentDepartmentIndexIncrement()
        {
            if (ParentDepartmentComboBoxIndex < (totalCountParentDepartment - 1))
            {
                ParentDepartmentComboBoxIndex++;
                await LoadDataParentDepartment(ParentDepartmentComboBoxIndex, 10);
            }
        }

        private async Task OnSearchParentDepartmentndexDecrement()
        {
            if (ParentDepartmentComboBoxIndex > 0)
            {
                ParentDepartmentComboBoxIndex--;
                await LoadDataParentDepartment(ParentDepartmentComboBoxIndex, 10);
            }
        }

        private async Task OnInputParentDepartmentChanged(string e)
        {
            ParentDepartmentComboBoxIndex = 0;
            await LoadDataParentDepartment();
        }

        private async Task LoadDataParentDepartment(int pageIndex = 0, int pageSize = 10, long? ParentDepartmentId = null)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetDepartmentQuery(x => x.ParentDepartmentId == null, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refParentDepartmentComboBox?.Text ?? ""));
                ParentDepartments = result.Item1;
                totalCountParentDepartment = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxParentDepartment
    }
}
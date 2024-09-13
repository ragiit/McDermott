using McDermott.Web.Components.Layout;
using System.Security.Policy;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class MenuPage
    {
        #region Fields and Properties

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private DxComboBox<MenuDto, long?> refParentMenuComboBox { get; set; }
        private int ParentMenuComboBoxIndex { get; set; } = 0;
        private int totalCountParentMenu = 0;

        [Inject] public UserInfoService UserInfoService { get; set; }
        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        public IGrid Grid { get; set; }
        private List<MenuDto> Menus = new();
        private List<MenuDto> ParentMenuDto = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private bool PanelVisible { get; set; }

        private List<ExportFileData> ExportFiles = new()
    {
        new() { Column = "Name", Notes = "Mandatory" },
        new() { Column = "Parent" },
        new() { Column = "Parent Icon" },
        new() { Column = "Sequence", Notes = "Mandatory" },
        new() { Column = "URL" }
    };

        #endregion Fields and Properties

        #region Lifecycle Methods

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataParentMenu();
            PanelVisible = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (firstRender)
            //{
            //    await GetUserInfo();
            //    await LoadData();
            //}
            //await base.OnAfterRenderAsync(firstRender);
        }

        #endregion Lifecycle Methods

        #region User and Access Management

        private async Task GetUserInfo()
        {
            try
            {
                var user = await UserInfoService.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch
            {
                // Handle user information retrieval failure.
            }
        }

        #endregion User and Access Management

        #region Data Loading and Searching

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
                var result = await Mediator.Send(new GetMenuQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
                Menus = result.Item1;
                totalCount = result.pageCount;
                activePageIndex = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                PanelVisible = false;
                ex.HandleException(ToastService);
            }
        }

        #endregion Data Loading and Searching

        #region Parent Menu Combobox

        private async Task OnSearchParentMenu()
        {
            await LoadDataParentMenu(0, 10);
        }

        private async Task OnSearchParentMenuIndexIncrement()
        {
            if (ParentMenuComboBoxIndex < (totalCountParentMenu - 1))
            {
                ParentMenuComboBoxIndex++;
                await LoadDataParentMenu(ParentMenuComboBoxIndex, 10);
            }
        }

        private async Task OnSearchParentMenundexDecrement()
        {
            if (ParentMenuComboBoxIndex > 0)
            {
                ParentMenuComboBoxIndex--;
                await LoadDataParentMenu(ParentMenuComboBoxIndex, 10);
            }
        }

        private async Task OnInputParentMenuChanged(string e)
        {
            ParentMenuComboBoxIndex = 0;
            await LoadDataParentMenu(0, 10);
        }

        private async Task LoadDataParentMenu(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetMenuQuery(x => x.Parent != null, searchTerm: refParentMenuComboBox?.Text ?? "", pageSize: pageSize, pageIndex: pageIndex));
            ParentMenuDto = [.. result.Item1.OrderBy(x => x.Name).ThenBy(x => x.Sequence)];
            totalCountParentMenu = result.pageCount;
            PanelVisible = false;
        }

        #endregion Parent Menu Combobox

        #region CRUD Operations

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteMenuRequest(((MenuDto)e.DataItem).Id));
                }
                else
                {
                    var selectedMenus = SelectedDataItems.Adapt<List<MenuDto>>();
                    await Mediator.Send(new DeleteMenuRequest(ids: selectedMenus.Select(x => x.Id).ToList()));
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
            var editModel = (MenuDto)e.EditModel;

            bool exists = await Mediator.Send(new ValidateMenuQuery(x =>
                x.Name == editModel.Name &&
                x.Id != editModel.Id &&
                x.ParentId == editModel.ParentId &&
                x.Sequence == editModel.Sequence &&
                x.Url == editModel.Url));

            if (exists)
            {
                ToastService.ShowErrorConflictValidation(nameof(Menu));
                e.Cancel = true;
                return;
            }

            if (editModel.Id == 0)
                await Mediator.Send(new CreateMenuRequest(editModel));
            else
                await Mediator.Send(new UpdateMenuRequest(editModel));

            await LoadData();
        }

        #endregion CRUD Operations

        #region Grid Event Handlers

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #endregion Grid Event Handlers

        #region Import and Export

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "menu_template.xlsx", ExportFiles);
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
                    using MemoryStream memoryStream = new();
                    await file.OpenReadStream().CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage excelPackage = new(memoryStream);
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

                    var expectedHeaders = ExportFiles.Select(x => x.Column);

                    // Validasi header
                    if (!expectedHeaders.Select((header, index) => header.Trim().ToLower() == worksheet.Cells[1, index + 1].Value?.ToString()?.Trim().ToLower()).All(valid => valid))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var importedMenus = new List<MenuDto>();
                    var parentCache = new List<MenuDto>();

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        string name = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        string parentName = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                        string icon = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                        int? sequence = worksheet.Cells[row, 4].Value?.ToInt32();
                        string url = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                        bool isValid = true;

                        // Validasi URL
                        if (!Helper.URLS.Contains(url) && !string.IsNullOrEmpty(parentName))
                        {
                            isValid = false;
                            ToastService.ShowErrorImport(row, 5, url ?? string.Empty);
                        }

                        // Validasi Name
                        if (string.IsNullOrEmpty(name))
                        {
                            isValid = false;
                            ToastService.ShowErrorImport(row, 1, name ?? string.Empty);
                        }

                        long? parentId = null;
                        if (!string.IsNullOrEmpty(parentName))
                        {
                            var cachedParent = parentCache.FirstOrDefault(x => x.Name == parentName);
                            if (cachedParent is null)
                            {
                                var parentMenu = (await Mediator.Send(new GetMenuQuery(
                                    x => x.Parent == null && x.Name == parentName,
                                    searchTerm: parentName, pageSize: 1, pageIndex: 0))).Item1.FirstOrDefault();

                                if (parentMenu is null)
                                {
                                    isValid = false;
                                    ToastService.ShowErrorImport(row, 2, parentName ?? string.Empty);
                                }
                                else
                                {
                                    parentId = parentMenu.Id;
                                    parentCache.Add(parentMenu);
                                }
                            }
                            else
                            {
                                parentId = cachedParent.Id;
                            }
                        }

                        // Lewati baris jika tidak valid
                        if (!isValid)
                            continue;

                        // Validasi dan tambahkan menu baru
                        var newMenu = new MenuDto
                        {
                            Name = name,
                            ParentId = parentId,
                            Icon = icon,
                            Sequence = sequence,
                            Url = url
                        };

                        bool exists = await Mediator.Send(new ValidateMenuQuery(x =>
                            x.Name == newMenu.Name &&
                            x.ParentId == newMenu.ParentId &&
                            x.Sequence == newMenu.Sequence &&
                            x.Url == newMenu.Url));

                        if (!exists)
                            importedMenus.Add(newMenu);
                    }

                    if (importedMenus.Count != 0)
                    {
                        await Mediator.Send(new CreateListMenuRequest(importedMenus));
                        await LoadData();
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(importedMenus.Count);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }

            PanelVisible = false;
        }

        #endregion Import and Export
    }
}
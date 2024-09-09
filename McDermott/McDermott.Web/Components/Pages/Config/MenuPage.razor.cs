namespace McDermott.Web.Components.Pages.Config
{
    public partial class MenuPage
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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await MyQuery.GetMenus(HttpClientFactory, pageIndex, pageSize, searchTerm ?? "");
            Menus = result.Item1;
            totalCount = result.Item2;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        #endregion Searching

        #region ComboboxParentMenu

        private DxComboBox<MenuDto, long?> refParentMenuComboBox { get; set; }
        private int ParentMenuComboBoxIndex { get; set; } = 0;
        private int totalCountParentMenu = 0;

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
            var result = await MyQuery.GetMenus(HttpClientFactory, pageIndex, pageSize, refParentMenuComboBox?.Text ?? "");
            ParentMenuDto = result.Item1.Where(x => x.Parent != null).OrderBy(x => x.Sequence).ToList();
            totalCountParentMenu = result.Item2;
            PanelVisible = false;
        }

        #endregion ComboboxParentMenu

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
            //    try
            //    {
            //        await GetUserInfo();
            //        StateHasChanged();
            //    }
            //    catch { }

            //    await LoadData();
            //    StateHasChanged();

            //    ParentMenuDto = (await Mediator.Send(new GetMenuQuery())).Where(x => x.ParentMenu == null || x.ParentMenu == string.Empty).OrderBy(x => x.Sequence).ToList();
            //    StateHasChanged();
            //}
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
        private List<MenuDto> Menus = new();
        private List<MenuDto> ParentMenuDto = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private bool PanelVisible { get; set; }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteMenuRequest(((MenuDto)e.DataItem).Id));

            NavigationManager.NavigateTo("config/menu", true);

            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataParentMenu();
            PanelVisible = false;
        }

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

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (MenuDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            //if (!string.IsNullOrWhiteSpace(editModel.ParentMenu))
            //{
            //    var splits = editModel.Name.ToLower().Split(" ");
            //    editModel.Url = $"{string.Join("-", editModel.ParentMenu.ToLower().Trim().Split())}/{editModel.Url}";

            //    if (editModel.ParentMenu.Contains("Configuration"))
            //    {
            //        editModel.Url = $"config/{string.Join("-", splits)}";
            //    }
            //}

            if (editModel.Id == 0)
                await Mediator.Send(new CreateMenuRequest(editModel));
            else
            {
                await Mediator.Send(new UpdateMenuRequest(editModel));

                if (editModel.Parent != null && string.IsNullOrWhiteSpace(editModel.Parent.Name))
                {
                    //var relatedMenus = await Mediator.Send(new GetMenuQuery(x => x.ParentMenu == SelectedDataItems[0].Adapt<MenuDto>().Name));

                    //var splits = editModel.Name.ToLower().Split(" ");

                    //if (editModel.Name.Contains("Configuration"))
                    //{
                    //    editModel.Url = $"config/{string.Join("-", splits)}";
                    //}
                    //else
                    //{
                    //    editModel.Url = $"{string.Join("-", splits)}/";
                    //}

                    //relatedMenus.ForEach(x =>
                    //{
                    //    x.ParentMenu = editModel.Name;
                    //    x.Url = editModel.Url + string.Join("-", x.Name.ToLower().Split(" "));
                    //});

                    //await Mediator.Send(new UpdateMenuRequest(editModel));

                    //await Mediator.Send(new UpdateListMenuRequest(relatedMenus));
                }
            }

            //await Mediator.Send(new GetGroupMenuQuery(removeCache: true));

            NavigationManager.NavigateTo("config/menu", true);

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

                    var headerNames = new List<string>() { "Name", "Parent", "Parent Icon", "Sequence", "Url" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<MenuDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool IsValid = true;
                        var a = this.Menus.FirstOrDefault(x => x.Name == ws.Cells[row, 2].Value?.ToString()?.Trim())?.Id ?? 0;

                        if (ws.Cells[row, 2].Value?.ToString()?.Trim() is not null)
                        {
                            if (a == 0)
                            {
                                ToastService.ShowErrorImport(row, 1, ws.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty);
                                IsValid = false;
                            }
                        }

                        if (!IsValid)
                            continue;

                        var c = new MenuDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            ParentId = a == 0 ? null : a,
                            Icon = ws.Cells[row, 3].Value?.ToString()?.Trim(),
                            Sequence = ws.Cells[row, 4].Value?.ToInt32(),
                            Url = ws.Cells[row, 5].Value?.ToString().Trim()
                        };

                        if (!Menus.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.ParentId == c?.ParentId && x.Sequence == c?.Sequence && x.Url?.Trim().ToLower() == c?.Url?.Trim().ToLower()))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListMenuRequest(list));

                    await LoadData();
                    SelectedDataItems = [];

                    ToastService.ShowSuccess("Successfully Imported.");
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
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "menu_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Parent"
                },
                new()
                {
                    Column = "Parent Icon"
                },
                new()
                {
                    Column = "Sequence",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "URL"
                }
            ]);
        }
    }
}
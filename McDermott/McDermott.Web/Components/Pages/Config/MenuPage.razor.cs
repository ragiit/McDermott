namespace McDermott.Web.Components.Pages.Config
{
    public partial class MenuPage
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
            var q = await Mediator.Send(new GetMenuQuery());
            ParentMenuDto = [.. q.Where(x => x.ParentMenu == null || x.ParentMenu == string.Empty).OrderBy(x => x.Sequence)];

            await GetUserInfo();
            await LoadData();

            PanelVisible = false;
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = [];
            Menus = await Mediator.Send(new GetMenuQuery());
            Menus = [.. Menus.OrderBy(x => x.ParentMenu == null).ThenBy(x => x.Sequence!.ToInt32())];
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

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
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

                if (string.IsNullOrWhiteSpace(editModel.ParentMenu))
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

                    var headerNames = new List<string>() { "Parent Menu", "Name", "Sequence", "Url" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<MenuDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new MenuDto
                        {
                            ParentMenu = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                            Sequence = ws.Cells[row, 3].Value?.ToInt32(),
                            Url = ws.Cells[row, 4].Value?.ToString().Trim()
                        };

                        if (!Menus.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.ParentMenu?.Trim().ToLower() == c?.ParentMenu?.Trim().ToLower() && x.Sequence == c?.Sequence && x.Url?.Trim().ToLower() == c?.Url?.Trim().ToLower()))
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
                    Column = "Parent Menu",
                },
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Sequence",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Url"
                },
            ]);
        }
    }
}
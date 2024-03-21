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
                var user = await UserInfoService.GetUserInfo();
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

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteMenuRequest(((MenuDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            var q = await Mediator.Send(new GetMenuQuery());
            ParentMenuDto = [.. q.Where(x => x.ParentMenu == null).OrderBy(x => x.Sequence)];

            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            SelectedDataItems = new ObservableRangeCollection<object>();
            Menus = await Mediator.Send(new GetMenuQuery());
            Menus = [.. Menus.OrderBy(x => x.ParentMenu == null).ThenBy(x => x.Sequence!.ToInt32())];
        }

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
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

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (MenuDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (!string.IsNullOrWhiteSpace(editModel.ParentMenu))
            {
                var splits = editModel.Name.ToLower().Split(" ");
                editModel.Url = $"{string.Join("-", editModel.ParentMenu.ToLower().Trim().Split())}/{string.Join("-", splits)}";

                if (editModel.ParentMenu.Contains("Configuration"))
                {
                    editModel.Url = $"config/{string.Join("-", splits)}";
                }
            }

            if (editModel.Id == 0)
                await Mediator.Send(new CreateMenuRequest(editModel));
            else
            {
                await Mediator.Send(new UpdateMenuRequest(editModel));

                if (string.IsNullOrWhiteSpace(editModel.ParentMenu))
                {
                    var relatedMenus = await Mediator.Send(new GetMenuQuery(x => x.ParentMenu == SelectedDataItems[0].Adapt<MenuDto>().Name));

                    var splits = editModel.Name.ToLower().Split(" ");

                    if (editModel.Name.Contains("Configuration"))
                    {
                        editModel.Url = $"config/{string.Join("-", splits)}";
                    }
                    else
                    {
                        editModel.Url = $"{string.Join("-", splits)}/";
                    }

                    relatedMenus.ForEach(x =>
                    {
                        x.ParentMenu = editModel.Name;
                        x.Url = editModel.Url + string.Join("-", x.Name.ToLower().Split(" "));
                    });

                    await Mediator.Send(new UpdateListMenuRequest(relatedMenus));
                }
            }

            await Mediator.Send(new GetGroupMenuQuery(removeCache: true));

            NavigationManager.NavigateTo("config/menu", true);

            await LoadData();
        }
    }
}
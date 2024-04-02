using DevExpress.Blazor.Internal;
using System.ComponentModel.DataAnnotations;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class GroupPage
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

        private bool PanelVisible { get; set; } = true;
        private long Id { get; set; }
        public IGrid Grid { get; set; }
        public IGrid GridGropMenu { get; set; }
        private bool IsAddMenu { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        private bool EditItemsEnabled { get; set; }
        private bool EditItemsGroupEnabled { get; set; } = false;
        private string GroupName { get; set; }
        private GroupDto Group { get; set; } = new();

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataItemsGroupMenu { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexGroupMenu { get; set; }
        private List<GroupDto> Groups = new();
        private List<GroupMenuDto> GroupMenus = [];
        private List<GroupMenuDto> DeletedGroupMenus = [];
        private List<MenuDto> Menus = [];

        [Required]
        private IEnumerable<MenuDto> SelectedGroupMenus = [];

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGroupRequest(SelectedDataItems[0].Adapt<GroupDto>().Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GroupDto>>();
                    await Mediator.Send(new DeleteGroupMenuRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                await JsRuntime.InvokeVoidAsync("alert", ee.InnerException.Message); // Alert
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Menus = await Mediator.Send(new GetMenuQuery());
            Menus.Insert(0, new MenuDto
            {
                Id = 0,
                Name = "All",
            });

            await GetUserInfo();
            await LoadData();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
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

        private void NewItem_Click()
        {
            ShowForm = true;
            GroupMenus = [];
            Group = new();
        }

        private async Task EditItem_Click()
        {
            try
            {
                Group = SelectedDataItems[0].Adapt<GroupDto>();
                ShowForm = true;

                if (Group != null)
                {
                    DeletedGroupMenus = await Mediator.Send(new GetGroupMenuQuery(x => x.GroupId == Group.Id));
                    GroupMenus = DeletedGroupMenus.Select(x => x).ToList();
                }
            }
            catch (Exception e)
            {
                var zz = e;
            }
            //await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsGroupEnabled = enabled;
        }

        private void UpdateEditItemsGroupEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private async Task NewItemGroup_Click()
        {
            GroupMenu = new();
            IsAddMenu = true;
            await GridGropMenu.StartEditNewRowAsync();
        }

        private async Task EditItemGroup_Click(IGrid context)
        {
            var aa = context;
            GroupMenu = (GroupMenuDto)context.SelectedDataItem;
            // Buat salinan objek yang akan diedit menggunakan Mapster
            var editedGroupMenu = GroupMenu.Adapt<GroupMenuDto>(); // GroupMenu adalah objek yang sedang diedit

            IsAddMenu = false;
            await GridGropMenu.StartEditRowAsync(FocusedRowVisibleIndexGroupMenu);

            var groupMenu = GroupMenus.FirstOrDefault(x => x.Id == editedGroupMenu.Id);

            if (groupMenu is not null)
                // Gunakan salinan objek yang diedit
                this.GroupMenu = editedGroupMenu;
        }

        private void DeleteItemGrid_Click()
        {
            GridGropMenu.ShowRowDeleteConfirmation(FocusedRowVisibleIndexGroupMenu);
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            var state = GroupMenus.Count > 0 ? true : false;
            UpdateEditItemsEnabled(state);
        }

        private void GridGroupMenu_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexGroupMenu = args.VisibleIndex;
            var state = GroupMenus.Count > 0 ? true : false;
            UpdateEditItemsEnabled(state);
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemsGroupMenu = [];
                GroupMenu = new();
                SelectedDataItems = [];
                Group = new();
                ShowForm = false;
                GroupMenus = new();
                Groups = await Mediator.Send(new GetGroupQuery());
                PanelVisible = false;
            }
            catch (Exception) { }
        }

        private void ColumnChooserButton_Click()
        {
            GridGropMenu.ShowColumnChooser();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            }); ;
        }

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
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

        private void OnDeleteGroupMenu()
        {
            StateHasChanged();
            var aaa = SelectedDataItemsGroupMenu.Adapt<List<GroupMenuDto>>();
            GroupMenus.RemoveAll(x => aaa.Select(z => z.MenuId).Contains(x.MenuId));
            SelectedDataItemsGroupMenu = new ObservableRangeCollection<object>();
        }

        private bool FormValidationState = false;

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            await SaveItemGroupMenuGrid_Click();
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        private GroupMenuDto GroupMenu { get; set; } = new();

        private async Task OnSaveGroupMenu()
        {
            var groupMenu = GroupMenu;

            GroupMenuDto updateMenu = new();

            if (IsAddMenu)
            {
                if (GroupMenus.Where(x => x.MenuId == groupMenu.MenuId).Any())
                    return;

                updateMenu = GroupMenus.FirstOrDefault(x => x.MenuId == groupMenu.MenuId)!;
                groupMenu.Menu = Menus.FirstOrDefault(x => x.Id == groupMenu.MenuId);
            }
            else
            {
                var q = SelectedDataItemsGroupMenu[0].Adapt<GroupMenuDto>();

                updateMenu = GroupMenus.FirstOrDefault(x => x.MenuId == q.MenuId)!;
                groupMenu.Menu = Menus.FirstOrDefault(x => x.Id == groupMenu.MenuId);
            }

            if (IsAddMenu)
            {
                GroupMenus.Add(groupMenu);
            }
            else
            {
                var index = GroupMenus.IndexOf(updateMenu!);
                GroupMenus[index] = groupMenu;
            }

            SelectedDataItemsGroupMenu = [];
            GroupMenu = new();
        }

        private void CancelItemGroupMenuGrid_Click()
        {
            GroupMenus = new();
            SelectedDataItemsGroupMenu = new ObservableRangeCollection<object>();
            ShowForm = false;
        }

        private async Task SaveItemGroupMenuGrid_Click()
        {
            if (Group.Id == 0)
            {
                var existingName = await Mediator.Send(new GetGroupQuery(x => x.Name == GroupName));

                if (existingName.Count > 0) return;

                var result = await Mediator.Send(new CreateGroupRequest(Group));

                var request = new List<GroupMenuDto>();

                var group = await Mediator.Send(new GetGroupQuery(x => x.Name == Group.Name));

                if (GroupMenus.Any(x => x.Menu?.Name is "All"))
                {
                    Menus.ForEach(z =>
                    {
                        if (z.Id != 0 && z.Name is not "All")
                        {
                            var all = GroupMenus.FirstOrDefault(x => x.Menu?.Name is "All");
                            request.Add(new GroupMenuDto
                            {
                                Id = 0,
                                MenuId = z.Id,
                                GroupId = group[0].Id,
                                Create = all.Create,
                                Read = all.Read,
                                Update = all.Update,
                                Delete = all.Delete,
                                Import = all.Import,
                            });
                        }
                    });

                    await Mediator.Send(new CreateListGroupMenuRequest(request));

                    ShowForm = false;

                    await LoadData();

                    return;
                }

                GroupMenus.ForEach(x =>
                {
                    x.Id = 0;
                    x.GroupId = group[0].Id;
                });

                for (int i = 0; i < GroupMenus.Count; i++)
                {
                    var check = Menus.FirstOrDefault(x => x.Id == GroupMenus[i].MenuId);
                    var cekP = Menus.FirstOrDefault(x => x.Name == check!.ParentMenu);
                    if (cekP is not null)
                    {
                        var cekLagi = GroupMenus.FirstOrDefault(x => x.MenuId == cekP.Id);
                        if (cekLagi is null)
                        {
                            GroupMenus.Add(new GroupMenuDto
                            {
                                Id = 0,
                                GroupId = group[0].Id,
                                MenuId = cekP.Id,
                                Menu = cekP
                            });
                        }
                    }
                }

                await Mediator.Send(new CreateListGroupMenuRequest(GroupMenus));
            }
            else
            {
                var result = await Mediator.Send(new UpdateGroupRequest(Group));

                var group = await Mediator.Send(new GetGroupQuery(x => x.Name == Group.Name));

                await Mediator.Send(new DeleteGroupMenuRequest(ids: DeletedGroupMenus.Select(x => x.Id).ToList()));

                var request = new List<GroupMenuDto>();

                if (GroupMenus.Any(x => x.Menu?.Name is "All"))
                {
                    Menus.ForEach(z =>
                    {
                        if (z.Id != 0 && z.Name is not "All")
                        {
                            var all = GroupMenus.FirstOrDefault(x => x.Menu?.Name is "All");
                            request.Add(new GroupMenuDto
                            {
                                Id = 0,
                                MenuId = z.Id,
                                GroupId = group[0].Id,
                                Create = all.Create,
                                Read = all.Read,
                                Update = all.Update,
                                Delete = all.Delete,
                                Import = all.Import,
                            });
                        }
                    });

                    await Mediator.Send(new CreateListGroupMenuRequest(request));

                    ShowForm = false;

                    NavigationManager.NavigateTo("config/group", true);

                    await LoadData();

                    return;
                }

                GroupMenus.ForEach(x =>
                {
                    x.Id = 0;
                    x.GroupId = group[0].Id;
                });

                for (int i = 0; i < GroupMenus.Count; i++)
                {
                    var check = Menus.FirstOrDefault(x => x.Id == GroupMenus[i].MenuId);
                    var cekP = Menus.FirstOrDefault(x => x.Name == check!.ParentMenu);
                    if (cekP is not null)
                    {
                        var cekLagi = GroupMenus.FirstOrDefault(x => x.MenuId == cekP.Id);
                        if (cekLagi is null)
                        {
                            GroupMenus.Add(new GroupMenuDto
                            {
                                Id = 0,
                                GroupId = group[0].Id,
                                MenuId = cekP.Id,
                                Menu = cekP
                            });
                        }
                    }
                }

                await Mediator.Send(new CreateListGroupMenuRequest(GroupMenus));

                NavigationManager.NavigateTo("config/group", true);
            }

            ShowForm = false;

            await LoadData();
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            //var Group = (GroupDto)e.EditModel;
            //var menus = await JSRuntime.InvokeAsync<IEnumerable<SelectedDropdown>>("getSelectedOptions", selectedMenus);

            //if (string.IsNullOrWhiteSpace(Group!.Name))
            //    return;

            //if (Group.Id == 0)
            //{
            //    var existingName = await Mediator.Send(new GetGroupByNameQuery(Group!.Name));

            //    if (existingName is not null)
            //        return;

            //    var result = await Mediator.Send(new CreateGroupRequest(Group));

            //    var request = new List<GroupMenuDto>();

            //    var group = await Mediator.Send(new GetGroupByNameQuery(Group.Name));

            //    if (menus.Where(x => x.Value == "0").Any())
            //    {
            //        Menus.ForEach(z =>
            //        {
            //            {
            //                request.Add(new GroupMenuDto
            //                {
            //                    MenuId = z.Id,
            //                    GroupId = group.Id
            //                });

            //        });

            //        await Mediator.Send(new CreateGroupMenuRequest(request));

            //        NavigationManager.NavigateTo("groups", true);
            //        return;
            //    }

            //    foreach (var item in menus)
            //    {
            //        request.Add(new GroupMenuDto
            //        {
            //            GroupId = group.Id,
            //            MenuId = Convert.ToInt32(item.Value)
            //        });
            //    }

            //    for (long i = 0; i < request.Count; i++)
            //    {
            //        var check = Menus.FirstOrDefault(x => x.Id == request[i].MenuId);
            //        var cekP = Menus.FirstOrDefault(x => x.Name == check!.ParentMenu);
            //        if (cekP is not null)
            //        {
            //            var cekLagi = request.FirstOrDefault(x => x.MenuId == cekP.Id);
            //            if (cekLagi is null)
            //            {
            //                request.Add(new GroupMenuDto
            //                {
            //                    GroupId = group.Id,
            //                    MenuId = cekP.Id
            //                });
            //            }
            //        }
            //    }
            //    await Mediator.Send(new CreateGroupMenuRequest(request));

            //    NavigationManager.NavigateTo("groups", true);
            //}
            //else
            //{
            //    var result = await Mediator.Send(new UpdateGroupRequest(Group));

            //    var request = new List<GroupMenuDto>();

            //    var group = await Mediator.Send(new GetGroupByNameQuery(Group.Name));

            //    if (menus.Where(x => x.Value == "0").Any())
            //    {
            //        Menus.ForEach(z =>
            //        {
            //            if (z.Id != 0)
            //            {
            //                request.Add(new GroupMenuDto
            //                {
            //                    MenuId = z.Id,
            //                    GroupId = group.Id
            //                });
            //            }
            //        });

            //        await Mediator.Send(new UpdateGroupMenuRequest(request, SelectedIds));
            //        NavigationManager.NavigateTo("groups", true);
            //        return;
            //    }

            //    foreach (var item in menus)
            //    {
            //        request.Add(new GroupMenuDto
            //        {
            //            GroupId = group.Id,
            //            MenuId = Convert.ToInt32(item.Value)
            //        });
            //    }

            //    for (long i = 0; i < request.Count; i++)
            //    {
            //        var check = Menus.FirstOrDefault(x => x.Id == request[i].MenuId);
            //        var cekP = Menus.FirstOrDefault(x => x.Name == check!.ParentMenu);
            //        if (cekP is not null)
            //        {
            //            var cekLagi = request.FirstOrDefault(x => x.MenuId == cekP.Id);
            //            if (cekLagi is null)
            //            {
            //                request.Add(new GroupMenuDto
            //                {
            //                    GroupId = group.Id,
            //                    MenuId = cekP.Id
            //                });
            //            }
            //        }
            //    }

            //    await Mediator.Send(new UpdateGroupMenuRequest(request, SelectedIds));
            //    NavigationManager.NavigateTo("groups", true);
            //}

            //if (Id is null)
            //{
            //    var result = await Mediator.Send(new CreateMenuRequest(Menu));
            //    if (result is not null)
            //        NavigationManager.NavigateTo("menus");
            //}
            //else
            //{
            //    var result = await Mediator.Send(new UpdateMenuRequest(Menu));
            //    if (result)
            //        NavigationManager.NavigateTo("menus");
            //}
        }
    }
}
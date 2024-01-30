using DevExpress.Blazor.Internal;
using DevExpress.Data.XtraReports.Native;
using McDermott.Domain.Entities;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using static McDermott.Application.Features.Commands.CountryCommand;
using static McDermott.Application.Features.Commands.GroupCommand;
using static McDermott.Application.Features.Commands.MenuCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class GroupPage
    {
        private GroupMenuDto UserAccessCRUID = new();
        private bool PanelVisible { get; set; } = true;
        private int Id { get; set; }
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
                    await Mediator.Send(new DeleteListGroupMenuRequest(a.Select(x => x.Id).ToList()));
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
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

            Menus = await Mediator.Send(new GetMenuQuery());
            Menus.Insert(0, new MenuDto
            {
                Id = 0,
                Name = "All",
            });
            await LoadData();
        }

        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    var result = await NavigationManager.CheckAccessUser(oLocal);
                    IsAccess = result.Item1;
                    UserAccessCRUID = result.Item2;
                }
                catch { }
            }
        }

        private async Task NewItem_Click()
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
                    DeletedGroupMenus = await Mediator.Send(new GetGroupMenuByGroupIdRequest(Group.Id));
                    GroupMenus = await Mediator.Send(new GetGroupMenuByGroupIdRequest(Group.Id));
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
            IsAddMenu = true;
            await GridGropMenu.StartEditNewRowAsync();
        }

        private async Task EditItemGroup_Click()
        {
            IsAddMenu = false;
            await GridGropMenu.StartEditRowAsync(FocusedRowVisibleIndexGroupMenu);
        }

        private void DeleteItemGrid_Click()
        {
            GridGropMenu.ShowRowDeleteConfirmation(FocusedRowVisibleIndexGroupMenu);
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexGroupMenu = args.VisibleIndex;
            var state = GroupMenus.Count > 0 ? true : false;
            UpdateEditItemsEnabled(state);
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void GridGroup_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsGroupEnabled(true);
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            Group = new();
            GroupMenus = new();
            Groups = await Mediator.Send(new GetGroupQuery());
            PanelVisible = false;
        }

        private void LoadGroupMenu()
        {
            GroupMenus = new();
        }

        //private async Task OnSave(GridEditModelSavingEventArgs e)
        //{
        //    var editModel = (GroupDto)e.EditModel;

        //    if (string.IsNullOrWhiteSpace(editModel.Name))
        //        return;

        //    if (editModel.Id == 0)
        //        await Mediator.Send(new CreateGroupRequest(editModel));
        //    else
        //        await Mediator.Send(new UpdateGroupRequest(editModel));

        //    await LoadData();
        //}

        private void test(GridCommandColumnCellDisplayTemplateContext context)
        {
            Id = ((GroupDto)context.DataItem).Id;
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

        private async Task OnSaveGroupMenu(GridEditModelSavingEventArgs e)
        {
            var groupMenu = (GroupMenuDto)e.EditModel;

            if (!GroupMenus.Where(x => x.MenuId == groupMenu.MenuId).Any())
                return;

            GroupMenuDto updateMenu = new();

            if (IsAddMenu)
            {
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

            SelectedDataItemsGroupMenu = new ObservableRangeCollection<object>();
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
                var existingName = await Mediator.Send(new GetGroupByNameQuery(GroupName));

                if (existingName is not null) return;

                var result = await Mediator.Send(new CreateGroupRequest(Group));

                var request = new List<GroupMenuDto>();

                var group = await Mediator.Send(new GetGroupByNameQuery(Group.Name));

                if (GroupMenus.Where(x => x.Menu.Name is "All").Any())
                {
                    Menus.ForEach(z =>
                    {
                        var all = GroupMenus.FirstOrDefault(x => x.Menu.Name is "All");
                        request.Add(new GroupMenuDto
                        {
                            MenuId = z.Id,
                            GroupId = group.Id,
                            Create = all.Create,
                            Read = all.Read,
                            Update = all.Update,
                            Delete = all.Delete,
                            Import = all.Import,
                        });
                    });

                    await Mediator.Send(new CreateGroupMenuRequest(request));

                    ShowForm = false;

                    await LoadData();

                    return;
                }

                GroupMenus.ForEach(x =>
                {
                    x.GroupId = group.Id;
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
                                GroupId = group.Id,
                                MenuId = cekP.Id,
                                Menu = cekP
                            });
                        }
                    }
                }

                await Mediator.Send(new CreateGroupMenuRequest(GroupMenus));
            }
            else
            {
                var result = await Mediator.Send(new UpdateGroupRequest(Group));

                var group = await Mediator.Send(new GetGroupByNameQuery(Group.Name));

                await Mediator.Send(new DeleteGroupMenuByIdRequest(DeletedGroupMenus.Select(x => x.Id).ToList()));

                var request = new List<GroupMenuDto>();

                if (GroupMenus.Where(x => x.Menu.Name is "All").Any())
                {
                    Menus.ForEach(z =>
                    {
                        var all = GroupMenus.FirstOrDefault(x => x.Menu.Name is "All");
                        request.Add(new GroupMenuDto
                        {
                            MenuId = z.Id,
                            GroupId = group.Id,
                            Create = all.Create,
                            Read = all.Read,
                            Update = all.Update,
                            Delete = all.Delete,
                            Import = all.Import,
                        });
                    });

                    await Mediator.Send(new CreateGroupMenuRequest(request));

                    ShowForm = false;

                    NavigationManager.NavigateTo("config/group", true);

                    await LoadData();

                    return;
                }

                GroupMenus.ForEach(x =>
                {
                    x.GroupId = group.Id;
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
                                GroupId = group.Id,
                                MenuId = cekP.Id,
                                Menu = cekP
                            });
                        }
                    }
                }

                await Mediator.Send(new CreateGroupMenuRequest(GroupMenus));

                await oLocal.SetItemAsync("Menu", string.Join(",", GroupMenus.Select(x => x.Menu?.Name)));

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

            //    for (int i = 0; i < request.Count; i++)
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

            //    for (int i = 0; i < request.Count; i++)
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
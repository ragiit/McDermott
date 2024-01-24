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
        private int Id { get; set; }
        public IGrid Grid { get; set; }
        public IGrid GridGropMenu { get; set; }
        private bool ShowForm { get; set; } = false;
        private bool EditItemsEnabled { get; set; }
        private bool EditItemsGroupEnabled { get; set; }
        private string GroupName { get; set; }
        private GroupDto Group { get; set; } = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; }
        private IReadOnlyList<object> SelectedDataItemsGroupMenu { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexGroupMenu { get; set; }
        private List<GroupDto> Groups = new();
        private List<GroupMenuDto> GroupMenus = [];
        private List<MenuDto> Menus = [];

        [Required]
        private IEnumerable<MenuDto> SelectedGroupMenus = [];

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteGroupRequest(((GroupDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            Menus = await Mediator.Send(new GetMenuQuery());
            Menus.Insert(0, new MenuDto
            {
                Id = 0,
                Name = "All",
            });
            await LoadData();
        }

        private async Task NewItem_Click()
        {
            ShowForm = true;
        }

        private async Task EditItem_Click()
        {
            try
            {
                Group = Groups[FocusedRowVisibleIndex];
                ShowForm = true;
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

        private async Task NewItemGroup_Click()
        {
            await GridGropMenu.StartEditNewRowAsync();
        }

        private async Task EditItemGroup_Click()
        {
            await GridGropMenu.StartEditRowAsync(FocusedRowVisibleIndexGroupMenu);
        }

        //private async Task OnSave(GridEditModelSavingEventArgs e)
        //{
        //    var editModel = (CountryDto)e.EditModel;

        //    if (string.IsNullOrWhiteSpace(editModel.Name))
        //        return;

        //    if (editModel.Id == 0)
        //        await Mediator.Send(new CreateCountryRequest(editModel));
        //    else
        //        await Mediator.Send(new UpdateCountryRequest(editModel));

        //    await LoadData();
        //}

        private void DeleteItemGrid_Click()
        {
            GridGropMenu.ShowRowDeleteConfirmation(FocusedRowVisibleIndexGroupMenu);
            GroupMenus.RemoveAt(FocusedRowVisibleIndexGroupMenu);
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexGroupMenu = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        private async Task LoadData()
        {
            Group = new();
            GroupMenus = new();
            Groups = await Mediator.Send(new GetGroupQuery());
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
            Grid.ShowColumnChooser();
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

        private async Task OnSaveGroupMenu(GridEditModelSavingEventArgs e)
        {
            var groupMenu = (GroupMenuDto)e.EditModel;
            var update = GroupMenus.FirstOrDefault(x => x.MenuId == groupMenu.MenuId);
            groupMenu.Menu = Menus.FirstOrDefault(x => x.Id == groupMenu.MenuId);
            if (update == null)
            {
                GroupMenus.Add(groupMenu);
            }
            else
            {
                update = groupMenu;
            }
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

                await Mediator.Send(new CreateGroupMenuRequest(GroupMenus));

                ShowForm = false;

                await LoadData();
            }
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
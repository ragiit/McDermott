using DevExpress.Blazor.Internal;
using McDermott.Application.Features.Services;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

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
            //await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
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

        private bool PanelVisible { get; set; } = true;

        [SupplyParameterFromQuery]
        private long Id { get; set; }

        public IGrid Grid { get; set; }
        public IGrid GridGropMenu { get; set; }
        private bool IsAddMenu { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        private bool EditItemsEnabled { get; set; }
        private bool EditItemsGroupEnabled { get; set; } = false;
        private string GroupName { get; set; }

        [SupplyParameterFromForm]
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
                    await Mediator.Send(new DeleteGroupRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadComboBox();
            PanelVisible = false;
        }

        #region ComboboxMenu

        private DxComboBox<MenuDto, long?> refMenuComboBox { get; set; }
        private int MenuComboBoxIndex { get; set; } = 0;
        private int totalCountMenu = 0;

        private async Task OnSearchMenu()
        {
            await LoadComboBox(0, 10);
        }

        private async Task OnSearchMenuIndexIncrement()
        {
            if (MenuComboBoxIndex < (totalCountMenu - 1))
            {
                MenuComboBoxIndex++;
                await LoadComboBox(MenuComboBoxIndex, 10);
            }
        }

        private async Task OnSearchMenundexDecrement()
        {
            if (MenuComboBoxIndex > 0)
            {
                MenuComboBoxIndex--;
                await LoadComboBox(MenuComboBoxIndex, 10);
            }
        }

        private async Task OnInputMenuChanged(string e)
        {
            MenuComboBoxIndex = 0;
            await LoadComboBox(0, 10);
        }

        private async Task LoadComboBox(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await MyQuery.GetMenus(HttpClientFactory, pageIndex, pageSize, refMenuComboBox?.Text ?? "");
            Menus = result.Item1;
            Menus.Insert(0, new MenuDto
            {
                Id = 0,
                Name = "All",
            });
            Menus = Menus.Where(x => x.ParentId != null || x.Name.Equals("All")).ToList();
            totalCountMenu = result.Item2;
            PanelVisible = false;
        }

        #endregion ComboboxMenu

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void NewItem_Click()
        {
            NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Create.GetDisplayName()}");
            return;
            ShowForm = true;
            GroupMenus = [];
            Group = new();
        }

        private bool IsLoading { get; set; } = false;

        private async Task EditItem_Click()
        {
            try
            {
                Group = SelectedDataItems[0].Adapt<GroupDto>();
                NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}");
                return;
                IsLoading = true;
                ShowForm = true;

                if (Group != null)
                {
                    await LoadGroupMenus();
                }
            }
            catch (Exception e)
            {
                var zz = e;
            }
            //await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            IsLoading = false;
        }

        private async Task LoadGroupMenus()
        {
            DeletedGroupMenus = await Mediator.Send(new GetGroupMenuQuery(x => x.GroupId == Group.Id));
            GroupMenus = DeletedGroupMenus.Select(x => x).ToList();
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
            SelectedDataItemsGroupMenu = [];
            GroupMenu = new();
            SelectedDataItems = [];
            Group = new();
            ShowForm = false;
            GroupMenus = [];
            var result = await MyQuery.GetGroups(HttpClientFactory, pageIndex, pageSize, searchTerm ?? "");
            Groups = result.Item1;
            totalCount = result.Item2;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        //private async Task LoadData()
        //{
        //    try
        //    {
        //        PanelVisible = true;
        //        SelectedDataItemsGroupMenu = [];
        //        GroupMenu = new();
        //        SelectedDataItems = [];
        //        Group = new();
        //        ShowForm = false;
        //        GroupMenus = [];
        //        Groups = await Mediator.Send(new GetGroupQuery());
        //        PanelVisible = false;
        //    }
        //    catch (Exception ex) { ex.HandleException(ToastService); }
        //}

        private void OnDeleteGroupMenu()
        {
            StateHasChanged();
            var aaa = SelectedDataItemsGroupMenu.Adapt<List<GroupMenuDto>>();
            GroupMenus.RemoveAll(x => aaa.Select(z => z.MenuId).Contains(x.MenuId));
            SelectedDataItemsGroupMenu = new ObservableRangeCollection<object>();
        }

        private bool FormValidationState = true;

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await SaveItemGroupMenuGrid_Click();
            else
                FormValidationState = true;
        }

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        [SupplyParameterFromForm]
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
            GroupMenus = [];
            Group = new();
            SelectedDataItems = [];
            SelectedDataItemsGroupMenu = [];
            ShowForm = false;
        }

        private async Task SaveItemGroupMenuGrid_Click()
        {
            if (!FormValidationState)
                return;

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
                                IsCreate = all.IsCreate,
                                IsRead = all.IsRead,
                                IsUpdate = all.IsUpdate,
                                IsDelete = all.IsDelete,
                                IsImport = all.IsImport,
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
                    var cekP = Menus.FirstOrDefault(x => check!.Parent != null && x.Name == check!.Parent.Name);
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

                NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={result.Id}");
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
                                IsCreate = all.IsCreate,
                                IsRead = all.IsRead,
                                IsUpdate = all.IsUpdate,
                                IsDelete = all.IsDelete,
                                IsImport = all.IsImport,
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
                    var cekP = Menus.FirstOrDefault(x => check!.Parent != null && x.Name == check!.Parent.Name);
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

                NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={result.Id}", true);
            }

            ShowForm = false;

            await LoadData();
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "group_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
            ]);
        }

        private List<ExportFileData> ExportFileDatasGroupMenus =
      [
          new()
            {
                Column = "Menu",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Parent Menu",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Is Create",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Is Read",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Is Update",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Is Delete",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Is Import",
                Notes = "Select one: Yes/No"
            }
      ];

        private async Task ExportToExcel2()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "group_menu_template.xlsx", ExportFileDatasGroupMenus.ToList());
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ImportFile2()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput2");
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

                    var headerNames = new List<string>() { "Name" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var countries = new List<GroupDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new GroupDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                        };

                        if (!Groups.Any(x => x.Name.Trim().ToLower() == country?.Name?.Trim().ToLower()))
                            countries.Add(country);
                    }

                    await Mediator.Send(new CreateListGroupRequest(countries));

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

        public async Task ImportExcelFile2(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = ExportFileDatasGroupMenus.Select(x => x.Column).ToList();

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var gg = new List<GroupMenuDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool IsValid = true;

                        var ab = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var parentId = Menus.FirstOrDefault(x => x.ParentId == null && x.Name == ab)?.Id ?? 0;
                        if (parentId == 0)
                        {
                            ToastService.ShowErrorImport(row, 2, ws.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty);
                            IsValid = false;
                        }

                        var aa = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var menuId = Menus.FirstOrDefault(x => x.ParentId == parentId && x.Name == aa)?.Id ?? 0;

                        if (menuId == 0)
                        {
                            ToastService.ShowErrorImport(row, 1, ws.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty);
                            IsValid = false;
                        }

                        if (!IsValid)
                            continue;

                        var g = new GroupMenuDto
                        {
                            GroupId = Group.Id,
                            MenuId = menuId,
                            IsCreate = ws.Cells[row, 3].Value?.ToString()?.Trim() == "Yes" ? true : false,
                            IsRead = ws.Cells[row, 4].Value?.ToString()?.Trim() == "Yes" ? true : false,
                            IsUpdate = ws.Cells[row, 5].Value?.ToString()?.Trim() == "Yes" ? true : false,
                            IsDelete = ws.Cells[row, 6].Value?.ToString()?.Trim() == "Yes" ? true : false,
                            IsImport = ws.Cells[row, 7].Value?.ToString()?.Trim() == "Yes" ? true : false,
                        };

                        if (!GroupMenus.Any(x => x.GroupId == g.GroupId && x.MenuId == g.MenuId && x.IsCreate == g.IsCreate
                         && x.IsRead == g.IsRead && x.IsUpdate == g.IsUpdate && x.IsDelete == g.IsDelete && x.IsImport == g.IsImport))
                            gg.Add(g);
                    }

                    await Mediator.Send(new CreateListGroupMenuRequest(gg));

                    NavigationManager.NavigateTo("configuration/group", true);

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
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
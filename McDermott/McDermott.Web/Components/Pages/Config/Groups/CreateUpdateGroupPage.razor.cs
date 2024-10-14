using DevExpress.Data.Access;
using DocumentFormat.OpenXml.Spreadsheet;
using GreenDonut;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using Group = McDermott.Domain.Entities.Group;

namespace McDermott.Web.Components.Pages.Config.Groups
{
    public partial class CreateUpdateGroupPage
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
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

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
        private int FocusedRowVisibleIndexGroupMenu { get; set; }
        private int FocusedRowVisibleIndexGroupMenuGroupMenu { get; set; }
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

        private async Task LoadGroupMenus(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.QueryGetHelper<GroupMenu, GroupMenuDto>(pageIndex, pageSize, searchTerm, predicate: x => x.GroupId == Group.Id);
            GroupMenus = result.Item1;
            totalCount = result.pageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadComboBox();
            await LoadData();
            PanelVisible = false;
            return;
            await GetUserInfo();
            await LoadData();
            await LoadComboBox();
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
            var result = await Mediator.QueryGetHelper<Menu, MenuDto>(pageIndex, pageSize, refMenuComboBox?.Text ?? "");
            Menus = result.Item1;
            Menus = Menus.Where(x => x.ParentId != null || x.Name.Equals("All")).ToList();
            totalCountMenu = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxMenu

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void NewItem_Click()
        {
            ShowForm = true;
            GroupMenus = [];
            Group = new();
        }

        private bool IsLoading { get; set; } = false;

        private async Task EditItem_Click()
        {
            try
            {
                IsLoading = true;
                Group = SelectedDataItems[0].Adapt<GroupDto>();
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
            //await GridGropMenu.StartEditRowAsync(FocusedRowVisibleIndexGroupMenu);
            IsLoading = false;
        }

        private void DeleteItem_Click()
        {
            GridGropMenu.ShowRowDeleteConfirmation(FocusedRowVisibleIndexGroupMenu);
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
            await GridGropMenu.StartEditRowAsync(FocusedRowVisibleIndexGroupMenuGroupMenu);

            var a = (GridGropMenu.GetDataItem(FocusedRowVisibleIndexGroupMenuGroupMenu) as GroupMenuDto ?? new());

            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetMenuQuery(x => x.Id == a.MenuId));
            Menus = result.Item1;
            Menus = Menus.Where(x => x.ParentId != null || x.Name.Equals("All")).ToList();
            totalCountMenu = result.pageCount;
            PanelVisible = false;

            return;
            var aa = context;
            GroupMenu = (GroupMenuDto)context.SelectedDataItem;
            // Buat salinan objek yang akan diedit menggunakan Mapster
            var editedGroupMenu = GroupMenu.Adapt<GroupMenuDto>(); // GroupMenu adalah objek yang sedang diedit

            IsAddMenu = false;

            var groupMenu = GroupMenus.FirstOrDefault(x => x.Id == editedGroupMenu.Id);

            if (groupMenu is not null)
                // Gunakan salinan objek yang diedit
                this.GroupMenu = editedGroupMenu;
        }

        private void DeleteItemGridGropMenu_Click()
        {
            GridGropMenu.ShowRowDeleteConfirmation(FocusedRowVisibleIndexGroupMenu);
        }

        //private void GridGropMenu_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        //{
        //    FocusedRowVisibleIndexGroupMenu = args.VisibleIndex;
        //    var state = GroupMenus.Count > 0 ? true : false;
        //    UpdateEditItemsEnabled(state);
        //}

        private void GridGropMenu_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexGroupMenuGroupMenu = args.VisibleIndex;
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
            await LoadGroupMenus(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadGroupMenus(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadGroupMenus(newPageIndex, pageSize);
        }

        #endregion Searching

        private async Task LoadData()
        {
            //var result = await MyQuery.GetGroups(HttpClientFactory, 0, 1, Id.HasValue ? Id.ToString() : "");

            //Id = McDermott.Extentions.SecureHelper.DecryptIdFromBase64(Ids);
            var result = await Mediator.QueryGetHelper<Group, GroupDto>(0, 0, predicate: x => x.Id == Id);

            Group = new();
            GroupMenus.Clear();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("configuration/groups");
                    return;
                }

                Group = result.Item1.FirstOrDefault() ?? new();
                await LoadGroupMenus();
            }
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

        private async Task OnDeleteGroupMenu(GridDataItemDeletingEventArgs e)
        {
            //StateHasChanged();
            //var aaa = SelectedDataItemsGroupMenu.Adapt<List<GroupMenuDto>>();
            //GroupMenus.RemoveAll(x => aaa.Select(z => z.MenuId).Contains(x.MenuId));
            //SelectedDataItemsGroupMenu = new ObservableRangeCollection<object>();
            try
            {
                PanelVisible = true;
                if (SelectedDataItemsGroupMenu is null)
                {
                    await Mediator.Send(new DeleteGroupMenuRequest(((GroupMenuDto)e.DataItem).Id));
                }
                else
                {
                    var selectedMenus = SelectedDataItemsGroupMenu.Adapt<List<GroupMenuDto>>();
                    await Mediator.Send(new DeleteGroupMenuRequest(ids: selectedMenus.Select(x => x.Id).ToList()));
                }
                await LoadGroupMenus(0, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private bool FormValidationState = true;

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await SaveItemGroupMenuGridGropMenu_Click();
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

        private async Task OnSaveGroupMenu(GridEditModelSavingEventArgs e)
        {
            var editModel = (GroupMenuDto)e.EditModel;

            editModel.GroupId = Group.Id;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateGroupMenuRequest(editModel));
            else
                await Mediator.Send(new UpdateGroupMenuRequest(editModel));

            NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}", true);

            await LoadGroupMenus();

            //var groupMenu = GroupMenu;

            //GroupMenuDto updateMenu = new();

            //if (IsAddMenu)
            //{
            //    if (GroupMenus.Where(x => x.MenuId == groupMenu.MenuId).Any())
            //        return;

            //    updateMenu = GroupMenus.FirstOrDefault(x => x.MenuId == groupMenu.MenuId)!;
            //    groupMenu.Menu = Menus.FirstOrDefault(x => x.Id == groupMenu.MenuId);
            //}
            //else
            //{
            //    var q = SelectedDataItemsGroupMenu[0].Adapt<GroupMenuDto>();

            //    updateMenu = GroupMenus.FirstOrDefault(x => x.MenuId == q.MenuId)!;
            //    groupMenu.Menu = Menus.FirstOrDefault(x => x.Id == groupMenu.MenuId);
            //}

            //if (IsAddMenu)
            //{
            //    GroupMenus.Add(groupMenu);
            //}
            //else
            //{
            //    var index = GroupMenus.IndexOf(updateMenu!);
            //    GroupMenus[index] = groupMenu;
            //}

            //SelectedDataItemsGroupMenu = [];
            //GroupMenu = new();
        }

        private void CancelItemGroupMenuGridGropMenu_Click()
        {
            //GroupMenus = [];
            //Group = new();
            //SelectedDataItems = [];
            //SelectedDataItemsGroupMenu = [];
            //ShowForm = false;

            NavigationManager.NavigateTo("configuration/groups");
        }

        private async Task SaveItemGroupMenuGridGropMenu_Click()
        {
            if (!FormValidationState)
                return;

            if (string.IsNullOrWhiteSpace(Group.Name))
            {
                ToastService.ShowInfo("Please insert the Group name");
                return;
            }

            var existingName = await Mediator.Send(new ValidateGroupQuery(x => x.Name == Group.Name && x.Id != Group.Id));

            if (existingName)
            {
                ToastService.ShowInfo("Group name already exist");
                return;
            }

            if (Group.Id == 0)
            {
                var result = await Mediator.Send(new CreateGroupRequest(Group));

                //var group = await Mediator.Send(new GetGroupQuery(x => x.Name == Group.Name));

                //if (GroupMenus.Any(x => x.Menu?.Name is "All"))
                //{
                //    Menus.ForEach(z =>
                //    {
                //        if (z.Id != 0 && z.Name is not "All")
                //        {
                //            var all = GroupMenus.FirstOrDefault(x => x.Menu?.Name is "All");
                //            request.Add(new GroupMenuDto
                //            {
                //                Id = 0,
                //                MenuId = z.Id,
                //                GroupId = group[0].Id,
                //                IsCreate = all.IsCreate,
                //                IsRead = all.IsRead,
                //                IsUpdate = all.IsUpdate,
                //                IsDelete = all.IsDelete,
                //                IsImport = all.IsImport,
                //            });
                //        }
                //    });

                //    await Mediator.Send(new CreateListGroupMenuRequest(request));

                //    ShowForm = false;

                //    await LoadData();

                //    return;
                //}

                GroupMenus.ForEach(x =>
                {
                    x.Id = 0;
                    x.GroupId = result.Id;
                });

                //for (int i = 0; i < GroupMenus.Count; i++)
                //{
                //    var check = Menus.FirstOrDefault(x => x.Id == GroupMenus[i].MenuId);
                //    var cekP = Menus.FirstOrDefault(x => check!.Parent != null && x.Name == check!.Parent.Name);
                //    if (cekP is not null)
                //    {
                //        var cekLagi = GroupMenus.FirstOrDefault(x => x.MenuId == cekP.Id);
                //        if (cekLagi is null)
                //        {
                //            GroupMenus.Add(new GroupMenuDto
                //            {
                //                Id = 0,
                //                GroupId = group[0].Id,
                //                MenuId = cekP.Id,
                //                Menu = cekP
                //            });
                //        }
                //    }
                //}

                await Mediator.Send(new CreateListGroupMenuRequest(GroupMenus));
                NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}", true);
            }
            else
            {
                var result = await Mediator.Send(new UpdateGroupRequest(Group));
                NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}", true);

                return;
                //var group = await Mediator.Send(new GetGroupQuery(x => x.Name == Group.Name));

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
                                GroupId = Group.Id,
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
                    NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}", true);

                    await LoadData();

                    return;
                }

                GroupMenus.ForEach(x =>
                {
                    x.Id = 0;
                    x.GroupId = result.Id;
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
                                GroupId = result.Id,
                                MenuId = cekP.Id,
                                Menu = cekP
                            });
                        }
                    }
                }

                await Mediator.Send(new CreateListGroupMenuRequest(GroupMenus));

                NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}", true);
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

                    var menus = new HashSet<string>();
                    var parentMenus = new HashSet<string>();

                    var list1 = new List<MenuDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var b = ws.Cells[row, 2].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            menus.Add(a.ToLower());

                        if (!string.IsNullOrEmpty(b))
                            parentMenus.Add(b.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetMenuQuery(x => parentMenus.Contains(x.Parent.Name) && menus.Contains(x.Name.ToLower()), 0, 0,
                        includes:
                        [
                            x => x.Parent
                        ],
                        select: x => new Menu
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Parent = new Menu
                            {
                                Name = x.Parent.Name
                            }
                        }))).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        string menu = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        string parentName = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        string isCreate = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        string isRead = ws.Cells[row, 4].Value?.ToString().Trim();
                        string isUpdate = ws.Cells[row, 5].Value?.ToString()?.Trim();
                        string isDelete = ws.Cells[row, 6].Value?.ToString()?.Trim();
                        string isImport = ws.Cells[row, 7].Value?.ToString()?.Trim();

                        bool isValid = true;

                        long? menuId = null;
                        if (!string.IsNullOrEmpty(menu) && !string.IsNullOrEmpty(parentName))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Parent.Name.Equals(parentName, StringComparison.CurrentCultureIgnoreCase) && x.Name.Equals(menu, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 2, $"Menu {menu ?? string.Empty} and Parent Menu {parentName ?? string.Empty}");
                                isValid = false;
                            }
                            else
                            {
                                menuId = cachedParent.Id;
                            }
                        }

                        if (!isValid)
                            continue;

                        var g = new GroupMenuDto
                        {
                            GroupId = Group.Id,
                            MenuId = menuId,
                            IsCreate = isCreate == "Yes",
                            IsRead = isRead == "Yes",
                            IsUpdate = isUpdate == "Yes",
                            IsDelete = isDelete == "Yes",
                            IsImport = isImport == "Yes",
                        };
                        gg.Add(g);
                    }
                    if (gg.Count > 0)
                    {
                        gg = gg.DistinctBy(x => new { x.GroupId, x.MenuId, x.IsCreate, x.IsRead, x.IsUpdate, x.IsDelete, x.IsImport }).ToList();

                        // Panggil BulkValidateVillageQuery untuk validasi bulk
                        var existingVillages = await Mediator.Send(new BulkValidateGroupMenuQuery(gg));

                        // Filter village baru yang tidak ada di database
                        gg = gg.Where(village =>
                            !existingVillages.Any(ev =>
                                ev.GroupId == village.GroupId &&
                                ev.MenuId == village.MenuId &&
                                ev.IsCreate == village.IsCreate &&
                                ev.IsRead == village.IsRead &&
                                ev.IsUpdate == village.IsUpdate &&
                                ev.IsDelete == village.IsDelete &&
                                ev.IsImport == village.IsImport
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListGroupMenuRequest(gg));
                        NavigationManager.NavigateTo($"configuration/groups/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}", true);
                    }

                    ToastService.ShowSuccessCountImported(gg.Count);
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
                finally
                {
                    PanelVisible = false;
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
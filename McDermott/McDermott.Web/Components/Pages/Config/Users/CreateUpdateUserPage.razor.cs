using DevExpress.Data.Access;
using DocumentFormat.OpenXml.Spreadsheet;
using GreenDonut;
using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Web.Components.Pages.Config.Users
{
    public partial class CreateUpdateUserPage
    {
        private List<UserDto> Users = new();
        private UserDto UserForm = new();
        public List<CountryDto> Countries = [];
        public List<ProvinceDto> Provinces = [];
        public List<CityDto> Cities = [];
        public List<DistrictDto> Districts = [];
        public List<VillageDto> Villages = [];
        private List<OccupationalDto> Occupationals = [];
        public List<GroupDto> Groups = [];
        public List<ReligionDto> Religions = [];
        public List<GenderDto> Genders = [];
        public List<DepartmentDto> Departments = [];
        public List<JobPositionDto> JobPositions = [];

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

        private char Placeholder { get; set; } = '_';

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataItemsGroupMenu { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndexGroupMenu { get; set; }
        private int FocusedRowVisibleIndexGroupMenuGroupMenu { get; set; }
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

        private List<SpecialityDto> Specialities = [];

        private async Task LoadGroupMenus(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            //var result = await MyQuery.GetGroupMenus(HttpClientFactory, pageIndex, pageSize, searchTerm ?? "", groupId: Group.Id == 0 ? null : Group.Id);
            var result = await Mediator.Send(new GetGroupMenuQuery(x => x.GroupId == Group.Id, pageIndex, pageSize, searchTerm));
            GroupMenus = result.Item1;
            totalCount = result.Item4;
            var aa = GroupMenus.Where(x => x.MenuId == 66).ToList();
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
            var aa = context;
            GroupMenu = (GroupMenuDto)context.SelectedDataItem;
            // Buat salinan objek yang akan diedit menggunakan Mapster
            var editedGroupMenu = GroupMenu.Adapt<GroupMenuDto>(); // GroupMenu adalah objek yang sedang diedit

            IsAddMenu = false;
            await GridGropMenu.StartEditRowAsync(FocusedRowVisibleIndexGroupMenuGroupMenu);

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

        private IEnumerable<ServiceDto> Services { get; set; } = [];
        private IEnumerable<ServiceDto> SelectedServices { get; set; } = [];

        private async Task LoadData()
        {
            //var result = await MyQuery.GetGroups(HttpClientFactory, 0, 1, Id.HasValue ? Id.ToString() : "");

            var result = await Mediator.Send(new GetUserQuery2(x => x.Id == Id, 0, 1));
            UserForm = new();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("configuration/users");
                    return;
                }

                UserForm = result.Item1.FirstOrDefault() ?? new();
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
        }

        private bool FormValidationState = true;

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await SaveItemGroupMenuGridGropMenu_Click();
            else
                FormValidationState = true;
        }

        private bool showPassword = false;
        private string showPasswordIcon = "fa-solid fa-eye-slash";

        private void TogglePasswordVisibility()
        {
            showPassword = !showPassword;

            if (showPassword == true)
            {
                showPasswordIcon = "fa-solid fa-eye";
            }
            else
            {
                showPasswordIcon = "fa-solid fa-eye-slash";
            }
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

            NavigationManager.NavigateTo("configuration/users");
        }

        [Inject]
        public CustomAuthenticationStateProvider CustomAuth { get; set; }

        private IBrowserFile BrowserFile;

        private async Task SaveItemGroupMenuGridGropMenu_Click()
        {
            if (!FormValidationState)
                return;

            var a = await Mediator.Send(new ValidateUserQuery(x => x.Id != UserForm.Id && x.NoId == UserForm.NoId));
            if (a)
            {
                ToastService.ShowInfo("The Identity Number already exist");
                return;
            }

            if (Convert.ToBoolean(UserForm.IsPatient))
            {
                var date = DateTime.Now;
                var lastId = Users.Where(x => x.IsPatient == true).ToList().LastOrDefault();

                UserForm.NoRm = lastId is null
                         ? $"{date:dd-MM-yyyy}-0001"
                         : $"{date:dd-MM-yyyy}-{(long.Parse(lastId!.NoRm!.Substring(lastId.NoRm.Length - 4)) + 1):0000}";
            }

            if (UserForm.IsSameDomicileAddress)
            {
                UserForm.DomicileAddress1 = UserForm.IdCardAddress1;
                UserForm.DomicileAddress2 = UserForm.IdCardAddress2;
                UserForm.DomicileRtRw = UserForm.IdCardRtRw;
                UserForm.DomicileProvinceId = UserForm.IdCardProvinceId;
                UserForm.DomicileCityId = UserForm.IdCardCityId;
                UserForm.DomicileDistrictId = UserForm.IdCardDistrictId;
                UserForm.DomicileVillageId = UserForm.IdCardVillageId;
                UserForm.DomicileCountryId = UserForm.IdCardCountryId;
            }

            if (!string.IsNullOrWhiteSpace(UserForm.Password))
                UserForm.Password = Helper.HashMD5(UserForm.Password);

            if (UserForm.Id == 0)
            {
                await FileUploadService.UploadFileAsync(BrowserFile);
                UserForm = await Mediator.Send(new CreateUserRequest(UserForm));
            }
            else
            {
                //var userDtoSipFile = SelectedDataItems[0].Adapt<UserDto>().SipFile;

                //if (UserForm.SipFile != userDtoSipFile)
                //{
                //    if (UserForm.SipFile != null)
                //        Helper.DeleteFile(UserForm.SipFile);

                //    if (userDtoSipFile != null)
                //        Helper.DeleteFile(userDtoSipFile);
                //}

                var result = await Mediator.Send(new UpdateUserRequest(UserForm));

                //if (UserForm.SipFile != userDtoSipFile)
                //{
                //    if (UserForm.SipFile != null)
                //        await FileUploadService.UploadFileAsync(BrowserFile);
                //}

                if (UserLogin.Id == result.Id)
                {
                    await JsRuntime.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);

                    var aa = (CustomAuthenticationStateProvider)CustomAuth;
                    await aa.UpdateAuthState(string.Empty);

                    await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_INFO, Helper.Encrypt(JsonConvert.SerializeObject(result)), 2);
                }
            }

            NavigationManager.NavigateTo($"configuration/users/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}", true);
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
                    var parentCache = new List<MenuDto>();

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

                        if (menu.Contains("Chronic Diagnoses"))
                        {
                            var a = "a";
                        }

                        long? menuId = null;
                        if (!string.IsNullOrEmpty(parentName))
                        {
                            var cachedParent = parentCache.FirstOrDefault(x => x.Name == parentName);
                            if (cachedParent is null)
                            {
                                var parentMenu = (await Mediator.Send(new GetMenuQuery(
                                    x => x.Parent != null && x.Parent.Name == parentName,
                                    searchTerm: menu, pageSize: 1, pageIndex: 0))).Item1.FirstOrDefault();

                                if (parentMenu is null)
                                {
                                    isValid = false;
                                    ToastService.ShowErrorImport(row, 2, $"Menu {menu ?? string.Empty} and Parent Menu {parentName ?? string.Empty}");
                                }
                                else
                                {
                                    menuId = parentMenu.Id;
                                    parentCache.Add(parentMenu);
                                }
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

                        bool exists = await Mediator.Send(new ValidateGroupMenuQuery(x =>
                                x.GroupId == g.GroupId &&
                                x.MenuId == g.MenuId));

                        if (!exists)
                            gg.Add(g);
                    }

                    if (gg.Count > 0)
                    {
                        SelectedDataItemsGroupMenu = [];
                        gg = gg.DistinctBy(x => x.MenuId).ToList();
                        await Mediator.Send(new CreateListGroupMenuRequest(gg));
                        await LoadGroupMenus(0, pageSize);
                    }

                    ToastService.ShowSuccess($"{gg.Count} items were successfully imported.");

                    //NavigationManager.NavigateTo($"configuration/users/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
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

        private async Task LoadComboBox(int pageIndex = 0, int pageSize = 10)
        {
            #region KTP Address

            await LoadDataCountry();
            await LoadDataProvince();
            await LoadDataCity();
            await LoadDataDistrict();
            await LoadDataVillage();

            #endregion KTP Address

            await LoadDataGroup();
            await LoadDataOccupational();
            await LoadDataSpeciality();
            await LoadDataService();
            await LoadDataSupervisor();
            await LoadDataJobPosition();
            await LoadDataDepartment();

            Departments = (await Mediator.Send(new GetDepartmentQuery())).Item1;
            JobPositions = (await Mediator.Send(new GetJobPositionQuery())).Item1;
            Religions = await Mediator.Send(new GetReligionQuery());
        }

        #region ComboboxVillage

        private DxComboBox<VillageDto, long?> refVillageComboBox { get; set; }
        private int VillageComboBoxIndex { get; set; } = 0;
        private int totalCountVillage = 0;

        private async Task OnSearchVillage()
        {
            await LoadDataVillage(0, 10);
        }

        private async Task OnSearchVillageIndexIncrement()
        {
            if (VillageComboBoxIndex < (totalCountVillage - 1))
            {
                VillageComboBoxIndex++;
                await LoadDataVillage(VillageComboBoxIndex, 10);
            }
        }

        private async Task OnSearchVillageIndexDecrement()
        {
            if (VillageComboBoxIndex > 0)
            {
                VillageComboBoxIndex--;
                await LoadDataVillage(VillageComboBoxIndex, 10);
            }
        }

        private async Task OnInputVillageChanged(string e)
        {
            VillageComboBoxIndex = 0;
            await LoadDataVillage(0, 10);
        }

        private async Task LoadDataVillage(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var districtId = refDistrictComboBox?.Value.GetValueOrDefault();
            var id = refVillageComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetVillageQuery(x => x.DistrictId == districtId && (id == null || x.Id == id), pageIndex: pageIndex, pageSize: pageSize, searchTerm: refVillageComboBox?.Text ?? ""));
            Villages = result.Item1;
            totalCountVillage = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxVillage

        #region ComboboxCountry

        private DxComboBox<CountryDto, long?> refCountryComboBox { get; set; }
        private int CountryComboBoxIndex { get; set; } = 0;
        private int totalCountCountry = 0;

        private async Task OnSearchCountry()
        {
            await LoadDataCountry(0, 10);
        }

        private async Task OnSearchCountryIndexIncrement()
        {
            if (CountryComboBoxIndex < (totalCountCountry - 1))
            {
                CountryComboBoxIndex++;
                await LoadDataCountry(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCountryIndexDecrement()
        {
            if (CountryComboBoxIndex > 0)
            {
                CountryComboBoxIndex--;
                await LoadDataCountry(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnInputCountryChanged(string e)
        {
            CountryComboBoxIndex = 0;
            await LoadDataCountry(0, 10);
        }

        private async Task LoadDataCountry(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            Provinces.Clear();
            Cities.Clear();
            Districts.Clear();
            Villages.Clear();
            UserForm.IdCardProvinceId = null;
            UserForm.IdCardCityId = null;
            UserForm.IdCardDistrictId = null;
            UserForm.IdCardVillageId = null;
            var id = refCountryComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetCountryQuery(x => (id == null || x.Id == id), pageIndex: pageIndex, pageSize: pageSize, searchTerm: refCountryComboBox?.Text ?? ""));
            Countries = result.Item1;
            totalCountCountry = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCountry

        #region ComboboxCity

        private DxComboBox<CityDto, long?> refCityComboBox { get; set; }
        private int CityComboBoxIndex { get; set; } = 0;
        private int totalCountCity = 0;

        private async Task OnSearchCity()
        {
            await LoadDataCity(0, 10);
        }

        private async Task OnSearchCityIndexIncrement()
        {
            if (CityComboBoxIndex < (totalCountCity - 1))
            {
                CityComboBoxIndex++;
                await LoadDataCity(CityComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCityIndexDecrement()
        {
            if (CityComboBoxIndex > 0)
            {
                CityComboBoxIndex--;
                await LoadDataCity(CityComboBoxIndex, 10);
            }
        }

        private async Task OnInputCityChanged(string e)
        {
            CityComboBoxIndex = 0;
            await LoadDataCity(0, 10);
        }

        private async Task LoadDataCity(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var provinceId = refProvinceComboBox?.Value.GetValueOrDefault();
            UserForm.IdCardDistrictId = null;
            UserForm.IdCardVillageId = null;
            var id = refCityComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetCityQuery(x => x.ProvinceId == provinceId && (id == null || x.Id == id), pageIndex: pageIndex, pageSize: pageSize, searchTerm: refCityComboBox?.Text ?? ""));
            Cities = result.Item1;
            totalCountCity = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCity

        #region ComboboxProvince

        private DxComboBox<ProvinceDto, long?> refProvinceComboBox { get; set; }
        private int ProvinceComboBoxIndex { get; set; } = 0;
        private int totalCountProvince = 0;

        private async Task OnSearchProvince()
        {
            await LoadDataProvince(0, 10);
        }

        private async Task OnSearchProvinceIndexIncrement()
        {
            if (ProvinceComboBoxIndex < (totalCountProvince - 1))
            {
                ProvinceComboBoxIndex++;
                await LoadDataProvince(ProvinceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProvinceIndexDecrement()
        {
            if (ProvinceComboBoxIndex > 0)
            {
                ProvinceComboBoxIndex--;
                await LoadDataProvince(ProvinceComboBoxIndex, 10);
            }
        }

        private async Task OnInputProvinceChanged(string e)
        {
            ProvinceComboBoxIndex = 0;
            await LoadDataProvince(0, 10);
        }

        private async Task LoadDataProvince(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var countryId = refCountryComboBox?.Value.GetValueOrDefault();
            UserForm.IdCardCityId = null;
            UserForm.IdCardDistrictId = null;
            UserForm.IdCardVillageId = null;

            var id = refProvinceComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetProvinceQuery(x => x.CountryId == countryId && (id == null || x.Id == id), pageIndex: pageIndex, pageSize: pageSize, searchTerm: refProvinceComboBox?.Text ?? ""));
            Provinces = result.Item1;
            totalCountProvince = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxProvince

        #region ComboboxDistrict

        private DxComboBox<DistrictDto, long?> refDistrictComboBox { get; set; }
        private int DistrictComboBoxIndex { get; set; } = 0;
        private int totalCountDistrict = 0;

        private async Task OnSearchDistrict()
        {
            await LoadDataDistrict(0, 10);
        }

        private async Task OnSearchDistrictIndexIncrement()
        {
            if (DistrictComboBoxIndex < (totalCountDistrict - 1))
            {
                DistrictComboBoxIndex++;
                await LoadDataDistrict(DistrictComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDistrictndexDecrement()
        {
            if (DistrictComboBoxIndex > 0)
            {
                DistrictComboBoxIndex--;
                await LoadDataDistrict(DistrictComboBoxIndex, 10);
            }
        }

        private async Task OnInputDistrictChanged(string e)
        {
            DistrictComboBoxIndex = 0;
            await LoadDataDistrict(0, 10);
        }

        private async Task LoadDataDistrict(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var cityId = refCityComboBox?.Value.GetValueOrDefault();
            UserForm.IdCardVillageId = null;
            var id = refDistrictComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetDistrictQuery(x => x.CityId == cityId && (id == null || x.Id == id), pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDistrictComboBox?.Text ?? ""));
            Districts = result.Item1;
            totalCountDistrict = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDistrict

        #region ComboboxGroup

        private DxComboBox<GroupDto, long?> refGroupComboBox { get; set; }
        private int GroupComboBoxIndex { get; set; } = 0;
        private int totalCountGroup = 0;

        private async Task OnSearchGroup()
        {
            await LoadDataGroup(0, 10);
        }

        private async Task OnSearchGroupIndexIncrement()
        {
            if (GroupComboBoxIndex < (totalCountGroup - 1))
            {
                GroupComboBoxIndex++;
                await LoadDataGroup(GroupComboBoxIndex, 10);
            }
        }

        private async Task OnSearchGroupndexDecrement()
        {
            if (GroupComboBoxIndex > 0)
            {
                GroupComboBoxIndex--;
                await LoadDataGroup(GroupComboBoxIndex, 10);
            }
        }

        private async Task OnInputGroupChanged(string e)
        {
            GroupComboBoxIndex = 0;
            await LoadDataGroup(0, 10);
        }

        private async Task LoadDataGroup(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetGroupQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refGroupComboBox?.Text ?? ""));
            Groups = result.Item1;
            totalCountGroup = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxGroup

        #region ComboboxOccupational

        private DxComboBox<OccupationalDto, long?> refOccupationalComboBox { get; set; }
        private int OccupationalComboBoxIndex { get; set; } = 0;
        private int totalCountOccupational = 0;

        private async Task OnSearchOccupational()
        {
            await LoadDataOccupational(0, 10);
        }

        private async Task OnSearchOccupationalIndexIncrement()
        {
            if (OccupationalComboBoxIndex < (totalCountOccupational - 1))
            {
                OccupationalComboBoxIndex++;
                await LoadDataOccupational(OccupationalComboBoxIndex, 10);
            }
        }

        private async Task OnSearchOccupationalndexDecrement()
        {
            if (OccupationalComboBoxIndex > 0)
            {
                OccupationalComboBoxIndex--;
                await LoadDataOccupational(OccupationalComboBoxIndex, 10);
            }
        }

        private async Task OnInputOccupationalChanged(string e)
        {
            OccupationalComboBoxIndex = 0;
            await LoadDataOccupational(0, 10);
        }

        private async Task LoadDataOccupational(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetOccupationalQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refOccupationalComboBox?.Text ?? ""));
            Occupationals = result.Item1;
            totalCountOccupational = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxOccupational

        #region ComboboxSpeciality

        private DxComboBox<SpecialityDto, long?> refSpecialityComboBox { get; set; }
        private int SpecialityComboBoxIndex { get; set; } = 0;
        private int totalCountSpeciality = 0;

        private async Task OnSearchSpeciality()
        {
            await LoadDataSpeciality(0, 10);
        }

        private async Task OnSearchSpecialityIndexIncrement()
        {
            if (SpecialityComboBoxIndex < (totalCountSpeciality - 1))
            {
                SpecialityComboBoxIndex++;
                await LoadDataSpeciality(SpecialityComboBoxIndex, 10);
            }
        }

        private async Task OnSearchSpecialityndexDecrement()
        {
            if (SpecialityComboBoxIndex > 0)
            {
                SpecialityComboBoxIndex--;
                await LoadDataSpeciality(SpecialityComboBoxIndex, 10);
            }
        }

        private async Task OnInputSpecialityChanged(string e)
        {
            SpecialityComboBoxIndex = 0;
            await LoadDataSpeciality(0, 10);
        }

        private async Task LoadDataSpeciality(int pageIndex = 0, int pageSize = 10, long? SpecialityId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetSpecialityQuery(SpecialityId == null ? null : x => x.Id == SpecialityId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refSpecialityComboBox?.Text ?? ""));
            Specialities = result.Item1;
            totalCountSpeciality = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxSpeciality

        #region ComboboxService

        private DxComboBox<ServiceDto?, long?> refServiceComboBox { get; set; }
        private int ServiceComboBoxIndex { get; set; } = 0;
        private int totalCountService = 0;

        private async Task OnSearchService()
        {
            await LoadDataService();
        }

        private async Task OnSearchServiceIndexIncrement()
        {
            if (ServiceComboBoxIndex < (totalCountService - 1))
            {
                ServiceComboBoxIndex++;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchServicendexDecrement()
        {
            if (ServiceComboBoxIndex > 0)
            {
                ServiceComboBoxIndex--;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnInputServiceChanged(string e)
        {
            ServiceComboBoxIndex = 0;
            await LoadDataService();
        }

        private async Task LoadDataService(int pageIndex = 0, int pageSize = 10, long? ServiceId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetServiceQuery(ServiceId == null ? null : x => x.Id == ServiceId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refServiceComboBox?.Text ?? ""));
            Services = result.Item1;
            totalCountService = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxService

        #region ComboboxSupervisor

        private DxComboBox<UserDto, long?> refSupervisorComboBox { get; set; }
        private int SupervisorComboBoxIndex { get; set; } = 0;
        private int totalCountSupervisor = 0;

        private async Task OnSearchSupervisor()
        {
            await LoadDataSupervisor();
        }

        private async Task OnSearchSupervisorIndexIncrement()
        {
            if (SupervisorComboBoxIndex < (totalCountSupervisor - 1))
            {
                SupervisorComboBoxIndex++;
                await LoadDataSupervisor(SupervisorComboBoxIndex, 10);
            }
        }

        private async Task OnSearchSupervisorndexDecrement()
        {
            if (SupervisorComboBoxIndex > 0)
            {
                SupervisorComboBoxIndex--;
                await LoadDataSupervisor(SupervisorComboBoxIndex, 10);
            }
        }

        private async Task OnInputSupervisorChanged(string e)
        {
            SupervisorComboBoxIndex = 0;
            await LoadDataSupervisor();
        }

        private List<UserDto> Supervisors = [];

        private async Task LoadDataSupervisor(int pageIndex = 0, int pageSize = 10, long? SupervisorId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetUserQuery2(SupervisorId == null ? null : x => x.Id == SupervisorId && x.IsEmployee == true, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refSupervisorComboBox?.Text ?? ""));
            Supervisors = result.Item1;
            totalCountSupervisor = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxSupervisor

        #region ComboboxJobPosition

        private DxComboBox<JobPositionDto, long?> refJobPositionComboBox { get; set; }
        private int JobPositionComboBoxIndex { get; set; } = 0;
        private int totalCountJobPosition = 0;

        private async Task OnSearchJobPosition()
        {
            await LoadDataJobPosition();
        }

        private async Task OnSearchJobPositionIndexIncrement()
        {
            if (JobPositionComboBoxIndex < (totalCountJobPosition - 1))
            {
                JobPositionComboBoxIndex++;
                await LoadDataJobPosition(JobPositionComboBoxIndex, 10);
            }
        }

        private async Task OnSearchJobPositionndexDecrement()
        {
            if (JobPositionComboBoxIndex > 0)
            {
                JobPositionComboBoxIndex--;
                await LoadDataJobPosition(JobPositionComboBoxIndex, 10);
            }
        }

        private async Task OnInputJobPositionChanged(string e)
        {
            JobPositionComboBoxIndex = 0;
            await LoadDataJobPosition();
        }

        private async Task LoadDataJobPosition(int pageIndex = 0, int pageSize = 10, long? JobPositionId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetJobPositionQuery(JobPositionId == null ? null : x => x.Id == JobPositionId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refJobPositionComboBox?.Text ?? ""));
            JobPositions = result.Item1;
            totalCountJobPosition = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxJobPosition

        #region ComboboxDepartment

        private DxComboBox<DepartmentDto, long?> refDepartmentComboBox { get; set; }
        private int DepartmentComboBoxIndex { get; set; } = 0;
        private int totalCountDepartment = 0;

        private async Task OnSearchDepartment()
        {
            await LoadDataDepartment();
        }

        private async Task OnSearchDepartmentIndexIncrement()
        {
            if (DepartmentComboBoxIndex < (totalCountDepartment - 1))
            {
                DepartmentComboBoxIndex++;
                await LoadDataDepartment(DepartmentComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDepartmentndexDecrement()
        {
            if (DepartmentComboBoxIndex > 0)
            {
                DepartmentComboBoxIndex--;
                await LoadDataDepartment(DepartmentComboBoxIndex, 10);
            }
        }

        private async Task OnInputDepartmentChanged(string e)
        {
            DepartmentComboBoxIndex = 0;
            await LoadDataDepartment();
        }

        private async Task LoadDataDepartment(int pageIndex = 0, int pageSize = 10, long? DepartmentId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetDepartmentQuery(DepartmentId == null ? null : x => x.Id == DepartmentId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDepartmentComboBox?.Text ?? ""));
            Departments = result.Item1;
            totalCountDepartment = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDepartment
    }
}
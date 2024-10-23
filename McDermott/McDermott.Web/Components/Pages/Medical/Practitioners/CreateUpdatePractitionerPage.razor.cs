using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Web.Components.Pages.Medical.Practitioners
{
    public partial class CreateUpdatePractitionerPage
    {
        private List<UserDto> Users = new();
        private UserDto UserForm = new();

        #region KTP Address

        public List<CountryDto> Countries = [];
        public List<ProvinceDto> Provinces = [];
        public List<CityDto> Cities = [];
        public List<DistrictDto> Districts = [];
        public List<VillageDto> Villages = [];

        #endregion KTP Address

        #region Residence  Address

        public List<CountryDto> CountriesResidence = [];
        public List<ProvinceDto> ProvincesResidence = [];
        public List<CityDto> CitiesResidence = [];
        public List<DistrictDto> DistrictsResidence = [];
        public List<VillageDto> VillagesResidence = [];

        #endregion Residence  Address

        private List<OccupationalDto> Occupationals = [];
        public List<GroupDto> Groups = [];
        public List<ReligionDto> Religions = [];

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

            Religions = await Mediator.Send(new GetReligionQuery());

            await GetUserInfo();
            await LoadData();

            if (Id.HasValue)
                await LoadComboBoxEdit();
            PanelVisible = false;
            return;
        }

        private async Task LoadComboBoxEdit()
        {
            PanelVisible = true;

            #region KTP Address

            Countries = (await Mediator.Send(new GetCountryQuery
            {
                Predicate = x => x.Id == UserForm.IdCardCountryId,
            })).Item1;
            Provinces = (await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.Id == UserForm.IdCardProvinceId,
            })).Item1;
            Cities = (await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.Id == UserForm.IdCardCityId,
            })).Item1;
            Districts = (await Mediator.QueryGetHelper<District, DistrictDto>(predicate: x => x.Id == UserForm.IdCardDistrictId)).Item1;
            Villages = (await Mediator.QueryGetHelper<Village, VillageDto>(predicate: x => x.Id == UserForm.IdCardVillageId)).Item1;

            #endregion KTP Address

            #region Residence  Address

            CountriesResidence = (await Mediator.Send(new GetCountryQuery
            {
                Predicate = x => x.Id == UserForm.DomicileCountryId,
            })).Item1;
            ProvincesResidence = (await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.Id == UserForm.DomicileProvinceId,
            })).Item1;
            CitiesResidence = (await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.Id == UserForm.DomicileCityId,
            })).Item1;
            DistrictsResidence = (await Mediator.QueryGetHelper<District, DistrictDto>(predicate: x => x.Id == UserForm.DomicileDistrictId)).Item1;
            VillagesResidence = (await Mediator.QueryGetHelper<Village, VillageDto>(predicate: x => x.Id == UserForm.DomicileVillageId)).Item1;

            #endregion Residence  Address

            Specialities = (await Mediator.QueryGetHelper<Speciality, SpecialityDto>(predicate: x => x.Id == UserForm.SpecialityId)).Item1;
            Services = (await Mediator.QueryGetHelper<Service, ServiceDto>(predicate: x => UserForm.DoctorServiceIds != null && UserForm.DoctorServiceIds.Contains(x.Id), includes: [],
                    select: x => new Service
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Quota = x.Quota
                    })).Item1;

            SelectedServices = (await Mediator.Send(new GetServiceQuery
            {
                Predicate = x => UserForm.DoctorServiceIds != null && UserForm.DoctorServiceIds.Contains(x.Id),
                Select = x => new Service
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Quota = x.Quota
                }
            })).Item1;
            PanelVisible = false;
        }

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

            var result = await Mediator.Send(new GetUserQuery2(x => x.Id == Id && x.IsDoctor == true, 0, 1, includes: []));
            UserForm = new();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("medical/practitioners");
                    return;
                }

                UserForm = result.Item1.FirstOrDefault() ?? new();
                UserForm.Password = Helper.HashMD5(UserForm.Password);
            }

            UserForm.IsDoctor = true;
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

            NavigationManager.NavigateTo("medical/practitioners");
        }

        [Inject]
        public CustomAuthenticationStateProvider CustomAuth { get; set; }

        private IBrowserFile BrowserFile;

        private async Task SaveItemGroupMenuGridGropMenu_Click()
        {
            if (!FormValidationState)
                return;

            UserForm.IsDoctor = true;
            bool isValid = true;
            if (!string.IsNullOrWhiteSpace(UserForm.NoId))
            {
                var a = await Mediator.Send(new ValidateUserQuery(x => x.Id != UserForm.Id && x.NoId == UserForm.NoId));
                if (a)
                {
                    ToastService.ShowInfo("The Identity Number is already exist");
                    isValid = false;
                }
            }

            var chekcEmail = await Mediator.Send(new ValidateUserQuery(x => x.Id != UserForm.Id && x.Email == UserForm.Email));
            if (chekcEmail)
            {
                ToastService.ShowInfo("The Email is already exist");
                isValid = false;
            }

            if (UserForm.IsPhysicion)
            {
                var b = await Mediator.Send(new ValidateUserQuery(x => x.Id != UserForm.Id && x.PhysicanCode == UserForm.PhysicanCode));
                if (b)
                {
                    ToastService.ShowInfo("The Physician Code is already exist");
                    isValid = false;
                }
            }

            if (!isValid)
                return;

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

            var ax = SelectedServices.Select(x => x.Id).ToList();
            UserForm.DoctorServiceIds?.AddRange(ax);

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

            NavigationManager.NavigateTo($"medical/practitioners/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}", true);
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

                    //NavigationManager.NavigateTo($"medical/practitioners/{EnumPageMode.Update.GetDisplayName()}?Id={Group.Id}");
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
            var result = await Mediator.QueryGetHelper<Village, VillageDto>(pageIndex, pageSize, refVillageComboBox?.Text ?? "", x => x.DistrictId == districtId);
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
            var result = await Mediator.Send(new GetCountryQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCountryComboBox?.Text ?? ""
            });
            Countries = result.Item1;
            totalCount = result.PageCount;
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
            var result = await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.ProvinceId == provinceId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCityComboBox?.Text ?? ""
            });
            Cities = result.Item1;
            totalCountCity = result.PageCount;
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

            var result = await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.CountryId == countryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refProvinceComboBox?.Text ?? ""
            });
            Provinces = result.Item1;
            totalCountProvince = result.PageCount;

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
            var result = await Mediator.QueryGetHelper<District, DistrictDto>(pageIndex, pageSize, refDistrictComboBox?.Text ?? "", x => x.CityId == cityId);
            Districts = result.Item1;
            totalCountDistrict = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDistrict

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

        private async Task LoadDataSpeciality(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.QueryGetHelper<Speciality, SpecialityDto>(pageIndex, pageSize, searchTerm);
                Specialities = result.Item1;
                totalCountSpeciality = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxSpeciality

        #region ComboboxService

        private DxComboBox<ServiceDto?, long?> refServiceComboBox { get; set; }
        private int ServiceComboBoxIndex { get; set; } = 0;
        private int totalCountService = 0;

        public string SearchTextService { get; set; }

        private async Task OnSearchService(string e)
        {
            SearchTextService = e;
            await LoadDataService();
        }

        private async Task OnSearchServiceClick()
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

        private async Task LoadDataService(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetServiceQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = refServiceComboBox?.Text ?? ""
                });
                Services = result.Item1;
                totalCountService = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxService

        #region Residence Address

        #region ComboboxVillageResidence

        private DxComboBox<VillageDto, long?> refVillageResidenceComboBox { get; set; }
        private int VillageResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountVillageResidence = 0;

        private async Task OnSearchVillageResidence()
        {
            await LoadDataVillageResidence(0, 10);
        }

        private async Task OnSearchVillageResidenceIndexIncrement()
        {
            if (VillageResidenceComboBoxIndex < (totalCountVillageResidence - 1))
            {
                VillageResidenceComboBoxIndex++;
                await LoadDataVillageResidence(VillageResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchVillageResidenceIndexDecrement()
        {
            if (VillageResidenceComboBoxIndex > 0)
            {
                VillageResidenceComboBoxIndex--;
                await LoadDataVillageResidence(VillageResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputVillageResidenceChanged(string e)
        {
            VillageResidenceComboBoxIndex = 0;
            await LoadDataVillageResidence(0, 10);
        }

        private async Task LoadDataVillageResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var DistrictResidenceId = refDistrictResidenceComboBox?.Value.GetValueOrDefault();
            var result = await Mediator.QueryGetHelper<Village, VillageDto>(pageIndex, pageSize, refVillageResidenceComboBox?.Text ?? "", x => x.DistrictId == DistrictResidenceId);
            VillagesResidence = result.Item1;
            totalCountVillageResidence = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxVillageResidence

        #region ComboboxCountryResidence

        private DxComboBox<CountryDto, long?> refCountryResidenceComboBox { get; set; }
        private int CountryResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountCountryResidence = 0;

        private async Task OnSearchCountryResidence()
        {
            await LoadDataCountryResidence(0, 10);
        }

        private async Task OnSearchCountryResidenceIndexIncrement()
        {
            if (CountryResidenceComboBoxIndex < (totalCountCountryResidence - 1))
            {
                CountryResidenceComboBoxIndex++;
                await LoadDataCountryResidence(CountryResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCountryResidenceIndexDecrement()
        {
            if (CountryResidenceComboBoxIndex > 0)
            {
                CountryResidenceComboBoxIndex--;
                await LoadDataCountryResidence(CountryResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputCountryResidenceChanged(string e)
        {
            CountryResidenceComboBoxIndex = 0;
            await LoadDataCountryResidence(0, 10);
        }

        private async Task LoadDataCountryResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetCountryQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCountryResidenceComboBox?.Text ?? ""
            });
            CountriesResidence = result.Item1;
            totalCountCountryResidence = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCountryResidence

        #region ComboboxCityResidence

        private DxComboBox<CityDto, long?> refCityResidenceComboBox { get; set; }
        private int CityResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountCityResidence = 0;

        private async Task OnSearchCityResidence()
        {
            await LoadDataCityResidence(0, 10);
        }

        private async Task OnSearchCityResidenceIndexIncrement()
        {
            if (CityResidenceComboBoxIndex < (totalCountCityResidence - 1))
            {
                CityResidenceComboBoxIndex++;
                await LoadDataCityResidence(CityResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCityResidenceIndexDecrement()
        {
            if (CityResidenceComboBoxIndex > 0)
            {
                CityResidenceComboBoxIndex--;
                await LoadDataCityResidence(CityResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputCityResidenceChanged(string e)
        {
            CityResidenceComboBoxIndex = 0;
            await LoadDataCityResidence(0, 10);
        }

        private async Task LoadDataCityResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var ProvinceResidenceId = refProvinceResidenceComboBox?.Value.GetValueOrDefault();
            var result = await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.ProvinceId == ProvinceResidenceId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCityResidenceComboBox?.Text ?? ""
            });
            CitiesResidence = result.Item1;
            totalCountCityResidence = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCityResidence

        #region ComboboxProvinceResidence

        private DxComboBox<ProvinceDto, long?> refProvinceResidenceComboBox { get; set; }
        private int ProvinceResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountProvinceResidence = 0;

        private async Task OnSearchProvinceResidence()
        {
            await LoadDataProvinceResidence(0, 10);
        }

        private async Task OnSearchProvinceResidenceIndexIncrement()
        {
            if (ProvinceResidenceComboBoxIndex < (totalCountProvinceResidence - 1))
            {
                ProvinceResidenceComboBoxIndex++;
                await LoadDataProvinceResidence(ProvinceResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProvinceResidenceIndexDecrement()
        {
            if (ProvinceResidenceComboBoxIndex > 0)
            {
                ProvinceResidenceComboBoxIndex--;
                await LoadDataProvinceResidence(ProvinceResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputProvinceResidenceChanged(string e)
        {
            ProvinceResidenceComboBoxIndex = 0;
            await LoadDataProvinceResidence(0, 10);
        }

        private async Task LoadDataProvinceResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var CountryResidenceId = refCountryResidenceComboBox?.Value.GetValueOrDefault();
            var id = refProvinceResidenceComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.CountryId == CountryResidenceId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refProvinceResidenceComboBox?.Text ?? ""
            });
            ProvincesResidence = result.Item1;
            totalCountProvinceResidence = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxProvinceResidence

        #region ComboboxDistrictResidence

        private DxComboBox<DistrictDto, long?> refDistrictResidenceComboBox { get; set; }
        private int DistrictResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountDistrictResidence = 0;

        private async Task OnSearchDistrictResidence()
        {
            await LoadDataDistrictResidence(0, 10);
        }

        private async Task OnSearchDistrictResidenceIndexIncrement()
        {
            if (DistrictResidenceComboBoxIndex < (totalCountDistrictResidence - 1))
            {
                DistrictResidenceComboBoxIndex++;
                await LoadDataDistrictResidence(DistrictResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDistrictResidencendexDecrement()
        {
            if (DistrictResidenceComboBoxIndex > 0)
            {
                DistrictResidenceComboBoxIndex--;
                await LoadDataDistrictResidence(DistrictResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputDistrictResidenceChanged(string e)
        {
            DistrictResidenceComboBoxIndex = 0;
            await LoadDataDistrictResidence(0, 10);
        }

        private async Task LoadDataDistrictResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var CityResidenceId = refCityResidenceComboBox?.Value.GetValueOrDefault();
            var result = await Mediator.QueryGetHelper<District, DistrictDto>(pageIndex, pageSize, refDistrictResidenceComboBox?.Text ?? "", x => x.CityId == CityResidenceId);
            DistrictsResidence = result.Item1;
            totalCountDistrictResidence = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDistrictResidence

        #endregion Residence Address
    }
}
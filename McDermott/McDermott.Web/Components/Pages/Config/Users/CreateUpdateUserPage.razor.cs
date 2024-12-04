using DevExpress.Data.Access;
using DevExpress.Office.Services.Implementation;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GreenDonut;
using HotChocolate.Utilities;
using Humanizer;
using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;
using Department = McDermott.Domain.Entities.Department;
using Group = McDermott.Domain.Entities.Group;

namespace McDermott.Web.Components.Pages.Config.Users
{
    public partial class CreateUpdateUserPage
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
            var result = await Mediator.Send(new GetGroupMenuQuery
            {
                SearchTerm = searchTerm ?? "",
                Predicate = x => x.GroupId == Group.Id,
                PageIndex = pageIndex,
                PageSize = pageSize
            });
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
            else
            {
                await LoadCountry();
                await LoadOccupational();
                await LoadUser();
                await LoadJobPosition();
                await LoadDepartment();
            }
            await LoadService();
            PanelVisible = false;
            return;
        }

        private async Task LoadComboBoxEdit()
        {
            PanelVisible = true;

            #region KTP Address

            await LoadCountry(predicate: UserForm.IdCardCountryId is null ? null : x => x.Id == UserForm.IdCardCountryId);
            await LoadProvince(predicate: UserForm.IdCardProvinceId is null ? null : x => x.Id == UserForm.IdCardProvinceId);
            await LoadCity(predicate: UserForm.IdCardCityId is null ? null : x => x.Id == UserForm.IdCardCityId);
            await LoadDistrict(predicate: UserForm.IdCardDistrictId is null ? null : x => x.Id == UserForm.IdCardDistrictId);
            await LoadVillage(predicate: UserForm.IdCardVillageId is null ? null : x => x.Id == UserForm.IdCardVillageId);

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
            DistrictsResidence = (await Mediator.Send(new GetDistrictQuery
            {
                Predicate = x => x.Id == UserForm.DomicileDistrictId,
            })).Item1;
            VillagesResidence = (await Mediator.QueryGetHelper<Village, VillageDto>(predicate: x => x.Id == UserForm.DomicileVillageId)).Item1;

            #endregion Residence  Address

            Groups = (await Mediator.Send(new GetGroupQuery
            {
                Predicate = x => x.Id == UserForm.GroupId
            })).Item1;

            await LoadUser(predicate: UserForm.SupervisorId is null ? null : x => x.Id == UserForm.SupervisorId);
            await LoadJobPosition(predicate: UserForm.SupervisorId is null ? null : x => x.Id == UserForm.JobPositionId);
            await LoadDepartment(predicate: UserForm.SupervisorId is null ? null : x => x.Id == UserForm.DepartmentId);
            await LoadOccupational(predicate: UserForm.OccupationalId is null ? null : x => x.Id == UserForm.OccupationalId);

            Specialities = (await Mediator.QueryGetHelper<Speciality, SpecialityDto>(predicate: x => x.Id == UserForm.SpecialityId)).Item1;

            Services = (await Mediator.Send(new GetServiceQuery
            {
                Predicate = x => UserForm.DoctorServiceIds != null && UserForm.DoctorServiceIds.Contains(x.Id),
            })).Item1;

            SelectedServices = Services.Where(x => UserForm.DoctorServiceIds is not null && UserForm.DoctorServiceIds.Contains(x.Id)).AsEnumerable();

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

        private string tempPassword = "";

        private async Task LoadData()
        {
            //var result = await MyQuery.GetGroups(HttpClientFactory, 0, 1, Id.HasValue ? Id.ToString() : "");

            var result = await Mediator.Send(new GetSingleUserQuery
            {
                Predicate = x => x.Id == Id
            });
            UserForm = new();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("configuration/users");
                    return;
                }

                UserForm = result ?? new();
                tempPassword = UserForm.Password;

                //UserForm.Password = Helper.HashMD5(UserForm.Password);
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
                await SaveUser();
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

        private async Task<bool> CheckExistingNumber(Expression<Func<User, bool>> predicate, string numberType)
        {
            var users = await Mediator.Send(new ValidateUserQuery(predicate));
            var isOkOk = false;
            if (users)
            {
                ToastService.ShowInfo($"{numberType} Number already exists");
                isOkOk = true;
            }
            return isOkOk;
        }

        public async Task<bool> CheckUserFormAsync()
        {
            var checks = new List<Func<Task<bool>>>
            {
                () => CheckExistingNumber(x => UserForm.Id != x.Id && UserForm.NIP != null && x.IsEmployee == true && x.NIP != null && x.NIP.ToLower().Trim().Equals(UserForm.NIP.ToLower().Trim()), "NIP"),
                () => CheckExistingNumber(x => UserForm.Id != x.Id && UserForm.Oracle != null && x.IsEmployee == true && x.Oracle != null && x.Oracle.ToLower().Trim().Equals(UserForm.Oracle.ToLower().Trim()), "Oracle"),
                () => CheckExistingNumber(x => UserForm.Id != x.Id && UserForm.Legacy != null && x.IsEmployee == true && x.Legacy != null && x.Legacy.ToLower().Trim().Equals(UserForm.Legacy.ToLower().Trim()), "Legacy"),
                () => CheckExistingNumber(x => UserForm.Id != x.Id && UserForm.SAP != null && x.IsEmployee == true && x.SAP != null && x.SAP.ToLower().Trim().Equals(UserForm.SAP.ToLower().Trim()), "SAP")
            };

            var isOkOk = true;

            foreach (var check in checks)
            {
                if (await check())
                {
                    isOkOk = false;
                }
            }

            return isOkOk;
        }

        private async Task SaveUser()
        {
            try
            {
                IsLoading = true;
                if (!FormValidationState)
                    return;

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

                if (Convert.ToBoolean(UserForm.IsEmployee))
                {
                    var isOk = await CheckUserFormAsync();
                    if (!isOk)
                        isValid = false;
                }

                if (!isValid)
                    return;

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

                UserForm.DoctorServiceIds ??= [];

                var ax = SelectedServices.Select(x => x.Id).ToList();
                UserForm.DoctorServiceIds?.AddRange(ax);
                UserForm.DoctorServiceIds?.Distinct();

                UserForm.Password = tempPassword;

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
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
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
                                //var parentMenu = (await Mediator.Send(new GetMenuQuery(
                                //    x => x.Parent != null && x.Parent.Name == parentName,
                                //    searchTerm: menu, pageSize: 1, pageIndex: 0))).Item1.FirstOrDefault();

                                //if (parentMenu is null)
                                //{
                                //    isValid = false;
                                //    ToastService.ShowErrorImport(row, 2, $"Menu {menu ?? string.Empty} and Parent Menu {parentName ?? string.Empty}");
                                //}
                                //else
                                //{
                                //    menuId = parentMenu.Id;
                                //    parentCache.Add(parentMenu);
                                //}
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
            var result = (await Mediator.Send(new GetGroupQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refGroupComboBox?.Text ?? ""
            }));
            //var result = await Mediator.Send(new GetGroupQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refGroupComboBox?.Text ?? ""));
            Groups = result.Item1;
            totalCountGroup = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxGroup

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
                Services = result.Item1.AsEnumerable();
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

        #region Ktp Address

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

        #endregion Ktp Address

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
            var result = await Mediator.Send(new GetDistrictQuery
            {
                Predicate = x => x.CityId == CityResidenceId,
                SearchTerm = refDistrictResidenceComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            DistrictsResidence = result.Item1;
            totalCountDistrictResidence = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDistrictResidence

        #endregion Residence Address

        #region ComboBox

        #region KTP Address

        #region ComboBox Country

        private async Task SelectedItemChanged(CountryDto e)
        {
            if (e is null)
            {
                UserForm.IdCardCountryId = null;
                Provinces = [];
                Cities = [];
                Districts = [];
                Villages = [];
                await LoadCountry();
            }
            else
            {
                UserForm.IdCardCountryId = e.Id;
                await LoadProvince();
            }
        }

        private CancellationTokenSource? _ctsCountry;

        private async Task OnInputCountry(ChangeEventArgs e)
        {
            try
            {
                _ctsCountry?.Cancel();
                _ctsCountry?.Dispose();
                _ctsCountry = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsCountry.Token);

                await LoadCountry(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsCountry?.Dispose();
                _ctsCountry = null;
            }
        }

        private async Task LoadCountry(string? e = "", Expression<Func<Country, bool>>? predicate = null)
        {
            try
            {
                Countries = await Mediator.QueryGetComboBox<Country, CountryDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Country

        #region ComboBox Province

        private async Task SelectedItemChanged(ProvinceDto e)
        {
            if (e is null)
            {
                UserForm.IdCardProvinceId = new();
                Cities = [];
                Districts = [];
                Villages = [];
                await LoadProvince();
            }
            else
            {
                UserForm.IdCardProvinceId = e.Id;
                await LoadCity();
            }
        }

        private CancellationTokenSource? _cts;

        private async Task OnInputProvince(ChangeEventArgs e)
        {
            try
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadProvince(e.Value?.ToString() ?? "", x => x.CountryId == UserForm.IdCardCountryId);
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadProvince(string? e = "", Expression<Func<Province, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.CountryId == UserForm.IdCardCountryId;

                Provinces = await Mediator.QueryGetComboBox<Province, ProvinceDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Province

        #region ComboBox City

        private async Task SelectedItemChanged(CityDto e)
        {
            if (e is null)
            {
                UserForm.IdCardCityId = new();
                Districts = [];
                Villages = [];
                await LoadCity();
            }
            else
            {
                UserForm.IdCardCityId = e.Id;
                await LoadDistrict();
            }
        }

        private CancellationTokenSource? _ctsCity;

        private async Task OnInputCity(ChangeEventArgs e)
        {
            try
            {
                _ctsCity?.Cancel();
                _ctsCity?.Dispose();
                _ctsCity = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsCity.Token);

                await LoadCity(e.Value?.ToString() ?? "", x => x.ProvinceId == UserForm.IdCardProvinceId);
            }
            finally
            {
                _ctsCity?.Dispose();
                _ctsCity = null;
            }
        }

        private async Task LoadCity(string? e = "", Expression<Func<City, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.ProvinceId == UserForm.IdCardProvinceId;

                Cities = await Mediator.QueryGetComboBox<City, CityDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox City

        #region ComboBox District

        private async Task SelectedItemChanged(DistrictDto e)
        {
            if (e is null)
            {
                UserForm.IdCardDistrictId = new();
                Villages = [];
                await LoadDistrict();
            }
            else
            {
                UserForm.IdCardDistrictId = e.Id;
                await LoadVillage();
            }
        }

        private CancellationTokenSource? _ctsDistrict;

        private async Task OnInputDistrict(ChangeEventArgs e)
        {
            try
            {
                _ctsDistrict?.Cancel();
                _ctsDistrict?.Dispose();
                _ctsDistrict = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsDistrict.Token);

                await LoadDistrict(e.Value?.ToString() ?? "", x => x.CityId == UserForm.IdCardCityId);
            }
            finally
            {
                _ctsDistrict?.Dispose();
                _ctsDistrict = null;
            }
        }

        private async Task LoadDistrict(string? e = "", Expression<Func<District, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.CityId == UserForm.IdCardCityId;
                Districts = await Mediator.QueryGetComboBox<District, DistrictDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox District

        #region ComboBox Village

        private VillageDto SelectedVillage { get; set; } = new();

        private async Task SelectedItemChanged(VillageDto e)
        {
            if (e is null)
            {
                SelectedVillage = new();
                await LoadVillage();
            }
            else
                SelectedVillage = e;
        }

        private CancellationTokenSource? _ctsVillage;

        private async Task OnInputVillage(ChangeEventArgs e)
        {
            try
            {
                _ctsVillage?.Cancel();
                _ctsVillage?.Dispose();
                _ctsVillage = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsVillage.Token);

                await LoadVillage(e.Value?.ToString() ?? "", x => x.DistrictId == UserForm.IdCardDistrictId);
            }
            finally
            {
                _ctsVillage?.Dispose();
                _ctsVillage = null;
            }
        }

        private async Task LoadVillage(string? e = "", Expression<Func<Village, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.DistrictId == UserForm.IdCardDistrictId;

                Villages = await Mediator.QueryGetComboBox<Village, VillageDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Village

        #endregion KTP Address

        #region ComboBox User

        private async Task SelectedItemChanged(UserDto e)
        {
            if (e is null)
            {
                await LoadUser();
            }
        }

        private CancellationTokenSource? _ctsUser;

        private async Task OnInputUser(ChangeEventArgs e)
        {
            try
            {
                _ctsUser?.Cancel();
                _ctsUser?.Dispose();
                _ctsUser = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsUser.Token);

                await LoadUser(e.Value?.ToString() ?? "", x => x.IsEmployee == true);
            }
            finally
            {
                _ctsUser?.Dispose();
                _ctsUser = null;
            }
        }

        private async Task LoadUser(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.IsEmployee == true;
                Users = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox User

        #region ComboBox JobPosition

        private async Task SelectedItemChanged(JobPositionDto e)
        {
            if (e is null)
            {
                await LoadJobPosition();
            }
        }

        private CancellationTokenSource? _ctsJobPosition;

        private async Task OnInputJobPosition(ChangeEventArgs e)
        {
            try
            {
                _ctsJobPosition?.Cancel();
                _ctsJobPosition?.Dispose();
                _ctsJobPosition = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsJobPosition.Token);

                await LoadJobPosition(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsJobPosition?.Dispose();
                _ctsJobPosition = null;
            }
        }

        private async Task LoadJobPosition(string? e = "", Expression<Func<JobPosition, bool>>? predicate = null)
        {
            try
            {
                JobPositions = await Mediator.QueryGetComboBox<JobPosition, JobPositionDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox JobPosition

        #region ComboBox Department

        private async Task SelectedItemChanged(DepartmentDto e)
        {
            if (e is null)
            {
                await LoadDepartment();
            }
        }

        private CancellationTokenSource? _ctsDepartment;

        private async Task OnInputDepartment(ChangeEventArgs e)
        {
            try
            {
                _ctsDepartment?.Cancel();
                _ctsDepartment?.Dispose();
                _ctsDepartment = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsDepartment.Token);

                await LoadDepartment(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsDepartment?.Dispose();
                _ctsDepartment = null;
            }
        }

        private async Task LoadDepartment(string? e = "", Expression<Func<Department, bool>>? predicate = null)
        {
            try
            {
                Departments = await Mediator.QueryGetComboBox<Department, DepartmentDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Department

        #region ComboBox Occupational

        private async Task SelectedItemChanged(OccupationalDto e)
        {
            if (e is null)
            {
                await LoadOccupational();
            }
        }

        private CancellationTokenSource? _ctsOccupational;

        private async Task OnInputOccupational(ChangeEventArgs e)
        {
            try
            {
                _ctsOccupational?.Cancel();
                _ctsOccupational?.Dispose();
                _ctsOccupational = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsOccupational.Token);

                await LoadOccupational(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsOccupational?.Dispose();
                _ctsOccupational = null;
            }
        }

        private async Task LoadOccupational(string? e = "", Expression<Func<Occupational, bool>>? predicate = null)
        {
            try
            {
                Occupationals = await Mediator.QueryGetComboBox<Occupational, OccupationalDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Occupational

        #endregion ComboBox

        #region ComboBox Service

        private async Task SelectedItemChanged(ServiceDto e)
        {
            if (e is null)
            {
                await LoadService();
            }
        }

        private CancellationTokenSource? _ctsService;

        private async Task OnInputService(ChangeEventArgs e)
        {
            try
            {
                _ctsService?.Cancel();
                _ctsService?.Dispose();
                _ctsService = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsService.Token);

                await LoadService(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsService?.Dispose();
                _ctsService = null;
            }
        }

        private async Task LoadService(string? e = "", Expression<Func<Service, bool>>? predicate = null)
        {
            try
            {
                Services = await Mediator.QueryGetComboBox<Service, ServiceDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Service
    }
}
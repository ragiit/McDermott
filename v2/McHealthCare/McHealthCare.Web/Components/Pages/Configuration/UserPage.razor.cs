using DevExpress.Blazor.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class UserPage
    {
        #region Default Variables & Forms

        private IGrid Grid { get; set; }
        private IGrid GridGroupMenu { get; set; }

        [Inject]
        private UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        private RoleManager<IdentityRole> RoleManager { get; set; }

        [Inject]
        private IUserStore<ApplicationUser> UserStore { get; set; }

        private bool PanelVisible { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; } = -1;
        private int FocusedRowVisibleIndex2 { get; set; } = -1;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new List<object>();
        private IReadOnlyList<object> SelectedDataItemsGroupMenu { get; set; } = new List<object>();
        private string Url => Helper.URLS.FirstOrDefault(x => x == "configuration/users") ?? string.Empty;
        public bool IsLoading { get; set; } = false;
        private string PageName => new Uri(NavigationManager.Uri).PathAndQuery.Replace(NavigationManager.BaseUri, "/");

        [Parameter]
        public string? PageMode { get; set; }

        public bool IsDeleted { get; set; } = false;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();

        #endregion Default Variables & Forms

        #region Variables

        private List<ApplicationUserDto> Users = [];
        private List<CountryDto> Countries = [];
        private List<GroupDto> Groups = [];
        private List<CityDto> Cities = [];
        private List<ServiceDto> Services = [];
        private List<SpecialityDto> Specialists = [];
        private List<DistrictDto> Districts = [];
        private List<ReligionDto> Religions = [];
        private List<VillageDto> Villages = [];
        private List<ProvinceDto> Provinces = [];
        private List<OccupationalDto> Occupationals = [];
        private UserRoleDto UserRole { get; set; } = new();

        //private List<JobPo> Provinces = [];
        //private List<DepartmentDto> Provinces = [];
        private ApplicationUserDto User { get; set; } = new();

        private DoctorDto Doctor { get; set; } = new();
        private EmployeeDto Employee { get; set; } = new();

        private IEnumerable<ServiceDto> SelectedServices { get; set; } = [];

        #endregion Variables

        private void CanDeleteSelectedItems(GridFocusedRowChangedEventArgs e)
        {
            FocusedRowVisibleIndex = e.VisibleIndex;

            if (e.DataItem is not null)
                IsDeleted = e.DataItem.Adapt<GroupDto>().IsDefaultData;
        }

        private async Task BackButtonAsync()
        {
            try
            {
                NavigationManager.NavigateToUrl(Url);
                PanelVisible = true;
                Users = await UserService.GetAllUsers();
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

        [Parameter] public string? Id { get; set; }

        private async Task LoadComboBox()
        {
            Groups = await Mediator.Send(new GetGroupQuery());
            Countries = await Mediator.Send(new GetCountryQuery());
            Cities = await Mediator.Send(new GetCityQuery());
            Districts = await Mediator.Send(new GetDistrictQuery());
            Religions = await Mediator.Send(new GetReligionQuery());
            Villages = await Mediator.Send(new GetVillageQuery());
            Provinces = await Mediator.Send(new GetProvinceQuery());
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            try
            {
                var a = await RoleManager.Roles.ToListAsync();
                UserAccess = await UserService.GetUserInfo(ToastService);

                await LoadDataAsync();
                await LoadComboBox();

                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!UserManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)UserStore;
        }

        protected override async Task OnParametersSetAsync()
        {
            // Periksa apakah PageMode diatur
            if (!string.IsNullOrWhiteSpace(PageMode))
            {
                // Cek apakah PageMode adalah Create
                if (PageMode == EnumPageMode.Create.GetDisplayName())
                {
                    // Periksa apakah URL saat ini tidak diakhiri dengan mode Create
                    if (!PageName.EndsWith($"/{EnumPageMode.Create.GetDisplayName()}", StringComparison.OrdinalIgnoreCase))
                    {
                        // Redirect ke URL dengan mode Create
                        NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Create.GetDisplayName()}", true);
                        return; // Pastikan kode berikutnya tidak dieksekusi
                    }
                    else
                    {
                        InitializeNew(true);
                    }
                }
                // Cek apakah PageMode adalah Update
                else if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    // Logika untuk update
                    if (!string.IsNullOrWhiteSpace(Id))
                    {
                        await LoadDataByIdAsync(Id);
                    }
                    else
                    {
                        NavigationManager.NavigateTo($"{Url}", true);
                        return; // Pastikan kode berikutnya tidak dieksekusi
                    }
                }
            }
        }

        private async Task HandleValidSubmitAsync()
        {
            try
            {
                //User = User.Id == Guid.Empty.ToString()
                //    ? await Mediator.Send(new CreateGroupRequest(Group))
                //    : await Mediator.Send(new UpdateGroupRequest(Group));

                if (PageMode == EnumPageMode.Create.GetDisplayName())
                {
                    User.Id = Guid.NewGuid().ToString();
                    var result = await UserManager.CreateAsync(User.Adapt<ApplicationUser>(), User?.Password ?? "P@ssRandom!123");

                    if (result.Succeeded)
                    {
                        await UserService.RemoveUserFromCache();

                        var s = await UserManager.FindByIdAsync(User.Id);
                        await UserService.UpdateUserRolesAsync(UserManager, s, UserRole);

                        Doctor.ApplicationUserId = User.Id;
                        Employee.ApplicationUserId = User.Id;

                        if (!UserRole.IsPractitioner)
                            await UserService.RemoveUserDoctor(User);
                        else
                            await UserService.UpdateDoctorAsync(Doctor);

                        if (!UserRole.IsEmployee)
                            await UserService.RemoveEmployeeDoctor(User);
                        else
                            await UserService.UpdateEmployeeAsync(Employee);

                        await LoadDataByIdAsync(User?.Id ?? string.Empty);
                        ToastService.ShowSuccessSaved("User");
                        NavigationManager.NavigateToUrl($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{User?.Id}");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ToastService.ShowError(error.Description);
                        }
                    }
                }
                else
                {
                    var s = await UserManager.FindByIdAsync(User.Id);
                    User.Adapt(s);
                    var result = await UserManager.UpdateAsync(s);
                    if (result.Succeeded)
                    {
                        await UserService.UpdateUserRolesAsync(UserManager, s, UserRole);

                        Doctor.ApplicationUserId = User.Id;
                        Employee.ApplicationUserId = User.Id;

                        if (!UserRole.IsPractitioner)
                            await UserService.RemoveUserDoctor(User);
                        else
                            await UserService.UpdateDoctorAsync(Doctor);

                        if (!UserRole.IsEmployee)
                            await UserService.RemoveEmployeeDoctor(User);
                        else
                            await UserService.UpdateEmployeeAsync(Employee);

                        await LoadDataByIdAsync(User?.Id ?? string.Empty);
                        ToastService.ShowSuccessUpdated("User");
                        NavigationManager.NavigateToUrl($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{User?.Id}");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ToastService.ShowError(error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private void HandleInvalidSubmitAsync()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                PanelVisible = true;

                Users = await UserService.GetAllUsers();
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

        private async Task LoadDataByIdAsync(string id)
        {
            IsLoading = true;
            try
            {
                //User = await UserService.GetUserId(id);

                var userTask = UserService.GetUserId(id, true);
                var doctorTask = UserService.GetDoctorByIdAsync(id, true);
                var employeeTask = UserService.GetEmployeeByIdAsync(id, true);

                //// Await tasks
                //User = await userTask;
                //Doctor = await doctorTask;
                //Employee = await employeeTask;

                //User = (await UserService.GetUserId(id, true)) ?? new();
                //Doctor = (await UserService.GetDoctorByIdAsync(id, true)) ?? new();
                //Employee = (await UserService.GetEmployeeByIdAsync(id, true)) ?? new();

                User = await userTask ?? new();
                Doctor = await doctorTask ?? new();
                Employee = await employeeTask ?? new();

                UserRole = await UserService.GetUserRolesAsync(UserManager, User.Adapt<ApplicationUser>());

                if (User is null || string.IsNullOrWhiteSpace(User.Id))
                {
                    NavigationManager.NavigateToUrl(Url);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task InitializeEditAsync()
        {
            if (SelectedDataItems.Count > 0)
            {
                var id = SelectedDataItems[0].Adapt<ApplicationUserDto>().Id;
                UserRole = new();
                Employee = new();
                User = new();
                Doctor = new();
                NavigationManager.NavigateToUrl($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{id}");
                await LoadDataByIdAsync(id);
            }
        }

        private async Task CancelItem_Click()
        {
            await BackButtonAsync();
        }

        private void InitializeNew(bool isParam = false)
        {
            User = new();

            if (!isParam)
                NavigationManager.NavigateToUrl($"{Url}/{EnumPageMode.Create.GetDisplayName()}");
        }

        private async Task OnDeleteAsync(GridDataItemDeletingEventArgs e)
        {
            try
            {
                IsLoading = true;
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    if (((ApplicationUser)e.DataItem).IsDefaultData)
                        return;

                    var u = await UserManager.FindByIdAsync(((ApplicationUser)e.DataItem).Id);

                    if (u != null)
                    {
                        await UserManager.DeleteAsync(u);
                        await UserService.RemoveUserFromCache();
                    }
                }
                else
                {
                    var ids = SelectedDataItems.Adapt<List<ApplicationUser>>().Where(x => x.IsDefaultData == false).Select(x => x.Id).ToList();
                    foreach (var id in ids)
                    {
                        var u = await UserManager.FindByIdAsync(id);

                        if (u != null)
                        {
                            await UserManager.DeleteAsync(u);
                            await UserService.RemoveUserFromCache();
                        }
                    }
                }
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
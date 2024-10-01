using MailKit.Search;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Web.Components.Pages.Employee.Employees
{
    public partial class EmployeePage
    {
        private List<UserDto> Users = [];
        private List<OccupationalDto> Occupationals = [];
        public List<CityDto> Cities = [];
        public List<CountryDto> Countries = [];
        public List<ProvinceDto> Provinces = [];
        public List<DistrictDto> Districts = [];
        public List<VillageDto> Villages = [];
        public List<DepartmentDto> Departments = [];
        public List<JobPositionDto> JobPositions = [];
        public List<ReligionDto> Religions = [];

        private UserDto UserForm = new();

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
                var user = await UserInfoService.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        private bool PanelVisible = false;
        private bool FormValidationState = true;
        private bool ShowForm { get; set; } = false;
        private bool IsDeleted { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        private char Placeholder { get; set; } = '_';
        private string EmailMask { get; set; } = @"(\w|[.-])+@(\w|-)+\.(\w|-){2,4}";
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private List<string> IdentityTypes = new()
        {
            "KTP",
            "Paspor",
            "SIM",
            "VISA",
        };

        private List<string> MartialStatuss = new()
        {
            "Single",
            "Married",
            "Divorced",
            "Widowed",
            "Separated",
            "Unmarried"
        };

        private List<string> EmployeeTypes = new()
        {
            "Employee",
            "Pre Employee",
            "Nurse",
            "Doctor",
        };

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            await GetUserInfo();
            await LoadData();
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
            SelectedDataItems = [];
            //var result = await Mediator.Send(new GetUserQuery2(x => x.IsEmployee == true, searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            var result = await Mediator.Send(new GetUserQuery2(
                x => x.IsEmployee == true,
                searchTerm: searchTerm,
                pageSize: pageSize,
                pageIndex:
                pageIndex,
                includes:
                [
                    user => user.Supervisor,
                    user => user.JobPosition,
                    user => user.Department,
                    user => user.Occupational,
                ],
                select: x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth,
                    Supervisor = x.Supervisor != null ? new Domain.Entities.User
                    {
                        Name = x.Supervisor.Name
                    } : null,
                    JobPosition = x.JobPosition != null ? new Domain.Entities.JobPosition
                    {
                        Name = x.JobPosition.Name
                    } : null,
                    Department = x.Department != null ? new Domain.Entities.Department
                    {
                        Name = x.Department.Name
                    } : null,
                    Occupational = x.Occupational != null ? new Domain.Entities.Occupational
                    {
                        Name = x.Occupational.Name
                    } : null,
                    EmployeeType = x.EmployeeType,
                    JoinDate = x.JoinDate,
                    NoBpjsKs = x.NoBpjsKs,
                    NoBpjsTk = x.NoBpjsTk,
                    Legacy = x.Legacy,
                    SAP = x.SAP,
                    NIP = x.NIP,
                    Oracle = x.Oracle,
                }

            ));
            Users = result.Item1;
            totalCount = result.pageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        #region Grid

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            await OnSave();
        }

        private void HandleInvalidSubmit()
        {
            FormValidationState = false;
        }

        private void OnCancel()
        {
            UserForm = new();
            ShowForm = false;
        }

        private async Task<bool> CheckExistingNumber(Expression<Func<User, bool>> predicate, string numberType)
        {
            var users = await Mediator.Send(new GetUserQuery(predicate));
            var isOkOk = false;
            if (users.Count > 0 && (UserForm.Id == 0 || users[0].Id != UserForm.Id))
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
                () => CheckExistingNumber(x => UserForm.NIP != null && x.IsEmployee == true && x.NIP != null && x.NIP.ToLower().Trim().Contains(UserForm.NIP.ToLower().Trim()), "NIP"),
                () => CheckExistingNumber(x => UserForm.Oracle != null && x.IsEmployee == true && x.Oracle != null && x.Oracle.ToLower().Trim().Contains(UserForm.Oracle.ToLower().Trim()), "Oracle"),
                () => CheckExistingNumber(x => UserForm.Legacy != null && x.IsEmployee == true && x.Legacy != null && x.Legacy.ToLower().Trim().Contains(UserForm.Legacy.ToLower().Trim()), "Legacy"),
                () => CheckExistingNumber(x => UserForm.SAP != null && x.IsEmployee == true && x.SAP != null && x.SAP.ToLower().Trim().Contains(UserForm.SAP.ToLower().Trim()), "SAP")
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

        private async Task OnSave()
        {
            if (!FormValidationState)
                return;

            UserForm.IsEmployee = true;

            ToastService.ClearInfoToasts();

            var isOk = await CheckUserFormAsync();
            if (!isOk)
                return;

            //var checkNipNumber = await Mediator.Send(new GetUserQuery(x => UserForm.NIP != null && x.IsEmployee == true && x.NIP != null && x.NIP.ToLower().Trim().Contains(UserForm.NIP.ToLower().Trim())));
            //var checkOracleumber = await Mediator.Send(new GetUserQuery(x => UserForm.Oracle != null && x.IsEmployee == true && x.Oracle != null && x.Oracle.ToLower().Trim().Contains(UserForm.Oracle.ToLower().Trim())));
            //var checkLegacyNumber = await Mediator.Send(new GetUserQuery(x => UserForm.Legacy != null && x.IsEmployee == true && x.Legacy != null && x.Legacy.ToLower().Trim().Contains(UserForm.Legacy.ToLower().Trim())));
            //var checkSapNumber = await Mediator.Send(new GetUserQuery(x => UserForm.SAP != null && x.IsEmployee == true && x.SAP != null && x.SAP.ToLower().Trim().Contains(UserForm.SAP.ToLower().Trim())));

            //if (UserForm.Id == 0)
            //{
            //    bool b = false;
            //    if (checkNipNumber.Count > 0)
            //    {
            //        b = true;
            //        ToastService.ShowInfo("NIP Number already exist");
            //    }
            //    if (checkOracleumber.Count > 0)
            //    {
            //        b = true;
            //        ToastService.ShowInfo("Oracle Number already exist");
            //    }
            //    if (checkLegacyNumber.Count > 0)
            //    {
            //        b = true;
            //        ToastService.ShowInfo("Legacy Number already exist");
            //    }
            //    if (checkSapNumber.Count > 0)
            //    {
            //        b = true;
            //        ToastService.ShowInfo("SAP Number already exist");
            //    }

            //    if (b)
            //        return;
            //}
            //else
            //{
            //    bool b = false;
            //    if (checkNipNumber.Count > 0 && checkNipNumber[0].Id != UserForm.Id)
            //    {
            //        b = true;
            //        ToastService.ShowInfo("NIP Number already exist");
            //    }
            //    if (checkOracleumber.Count > 0 && checkOracleumber[0].Id != UserForm.Id)
            //    {
            //        b = true;
            //        ToastService.ShowInfo("Oracle Number already exist");
            //    }
            //    if (checkLegacyNumber.Count > 0 && checkLegacyNumber[0].Id != UserForm.Id)
            //    {
            //        b = true;
            //        ToastService.ShowInfo("Legacy Number already exist");
            //    }
            //    if (checkSapNumber.Count > 0 && checkSapNumber[0].Id != UserForm.Id)
            //    {
            //        b = true;
            //        ToastService.ShowInfo("SAP Number already exist");
            //    }

            //    if (b)
            //        return;
            //}

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
                await Mediator.Send(new CreateUserRequest(UserForm));
            else
                await Mediator.Send(new UpdateUserRequest(UserForm));

            await LoadData();
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            if (args.DataItem is not null)
            {
                IsDeleted = UserLogin.Id == ((UserDto)args.DataItem).Id;
            }

            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            if (SelectedDataItems is null)
            {
                await Mediator.Send(new DeleteUserRequest(((UserDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<UserDto>>();

                await Mediator.Send(new DeleteUserRequest(ids: a.Where(x => x.Id != UserLogin.Id).Select(x => x.Id).ToList()));
            }
            await LoadData();
        }

        private async Task NewItem_Click()
        {
            NavigationManager.NavigateTo($"employee/employees/{EnumPageMode.Create.GetDisplayName()}");
        }

        private void OnRowDoubleClick()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                NavigationManager.NavigateTo($"employee/employees/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}");
                //ShowForm = true;
            }
            catch { }
        }

        private void EditItem_Click()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                NavigationManager.NavigateTo($"employee/employees/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}");
                return;

                ShowForm = true;
            }
            catch (Exception)
            {
            }
            //await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
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

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #endregion Grid
    }
}
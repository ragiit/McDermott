﻿using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.GetDataCommand;

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
            PanelVisible = false;
        }

        private object Data { get; set; }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var dataSource = new GridDevExtremeDataSource<User>(await Mediator.Send(new GetQueryUser
                {
                    Predicate = x => x.IsEmployee == true,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        Supervisor = new Domain.Entities.User
                        {
                            Name = x.Supervisor.Name
                        },
                        JobPosition = new Domain.Entities.JobPosition
                        {
                            Name = x.JobPosition.Name
                        },
                        Department = new Domain.Entities.Department
                        {
                            Name = x.Department.Name
                        },
                        Occupational = new Domain.Entities.Occupational
                        {
                            Name = x.Occupational.Name
                        },
                        EmployeeType = x.EmployeeType,
                        JoinDate = x.JoinDate,
                        NoBpjsKs = x.NoBpjsKs,
                        NoBpjsTk = x.NoBpjsTk,
                        Legacy = x.Legacy,
                        SAP = x.SAP,
                        NIP = x.NIP,
                        Oracle = x.Oracle,
                    }
                }))
                {
                    CustomizeLoadOptions = (loadOptions) =>
                    {
                        loadOptions.PrimaryKey = ["Id"];
                        loadOptions.PaginateViaPrimaryKey = true;
                    }
                };
                Data = dataSource;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
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
                IsDeleted = UserLogin.Id == ((User)args.DataItem).Id;
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

        private List<ExportFileData> ExportTemp =
        [
            new() { Column = "Name", Notes = "Mandatory" },
            new() { Column = "Email"},
            new() { Column = "Identity Number"},
            new() { Column = "Religion"},
            new() { Column = "Date of Birth"},
            new() { Column = "Gender"},
            new() { Column = "Marital Status"},
            new() { Column = "Mobile"},
            new() { Column = "Current Mobile"},
            new() { Column = "Phone"},
            new() { Column = "NPWP"},
            new() { Column = "Emergency Name"},
            new() { Column = "Emergency Email"},
            new() { Column = "Emergency Phone"},

            new() { Column = "BPJS KS Number"},
            new() { Column = "BPJS TK Number"},
            new() { Column = "Legacy Number"},
            new() { Column = "SAP Number"},
            new() { Column = "NIP Number"},
            new() { Column = "Oracle Number"},
            new() { Column = "Employee Type", Notes = "Select one: Employee/Pre Employee/Nurse/Doctor"},
            new() { Column = "Join Date"},
        ];

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

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => ExportTemp.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<UserDto>();

                    var religionNames = new HashSet<string>();

                    var groups = new List<GroupDto>();
                    var religions = new List<ReligionDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var b = ws.Cells[row, 4].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(b))
                            religionNames.Add(b.ToLower());
                    }

                    religions = (await Mediator.Send(new GetReligionQuery(x => religionNames.Contains(x.Name.ToLower()))));

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var name = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var email = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var identityNum = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var religion = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        var dateOfBirth = ws.Cells[row, 5].Value?.ToString()?.Trim();
                        var gender = ws.Cells[row, 6].Value?.ToString()?.Trim();
                        var marital = ws.Cells[row, 7].Value?.ToString()?.Trim();
                        var mobile = ws.Cells[row, 8].Value?.ToString()?.Trim();
                        var currentMobile = ws.Cells[row, 9].Value?.ToString()?.Trim();
                        var phone = ws.Cells[row, 10].Value?.ToString()?.Trim();
                        var npwp = ws.Cells[row, 11].Value?.ToString()?.Trim();
                        var emergencyName = ws.Cells[row, 12].Value?.ToString()?.Trim();
                        var emergencyEmail = ws.Cells[row, 13].Value?.ToString()?.Trim();
                        var emergencyPhone = ws.Cells[row, 14].Value?.ToString()?.Trim();
                        var bpjsks = ws.Cells[row, 15].Value?.ToString()?.Trim();
                        var bpjstk = ws.Cells[row, 16].Value?.ToString()?.Trim();
                        var legacy = ws.Cells[row, 17].Value?.ToString()?.Trim();
                        var sap = ws.Cells[row, 18].Value?.ToString()?.Trim();
                        var nip = ws.Cells[row, 19].Value?.ToString()?.Trim();
                        var oracle = ws.Cells[row, 20].Value?.ToString()?.Trim();
                        var empType = ws.Cells[row, 21].Value?.ToString()?.Trim();
                        var joinDate = ws.Cells[row, 22].Value?.ToString()?.Trim();

                        bool isValid = true;

                        long? religionId = null;
                        if (!string.IsNullOrEmpty(religion))
                        {
                            var cachedParent = religions.FirstOrDefault(x => x.Name.Equals(religion, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 4, religion ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                religionId = cachedParent.Id;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(gender))
                        {
                            if (!Helper.Genders.Contains(gender) && !string.IsNullOrEmpty(gender))
                            {
                                ToastService.ShowErrorImport(row, 6, gender ?? string.Empty);
                                isValid = false;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(empType))
                        {
                            if (!Helper.EmployeeTypes.Contains(empType) && !string.IsNullOrEmpty(empType))
                            {
                                ToastService.ShowErrorImport(row, 21, empType ?? string.Empty);
                                isValid = false;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(marital))
                        {
                            if (!Helper.MartialStatuss.Contains(marital) && !string.IsNullOrEmpty(marital))
                            {
                                ToastService.ShowErrorImport(row, 7, marital ?? string.Empty);
                                isValid = false;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            ToastService.ShowErrorImport(row, 1, name ?? string.Empty);
                            isValid = false;
                        }

                        if (string.IsNullOrWhiteSpace(email))
                        {
                            ToastService.ShowErrorImport(row, 2, email ?? string.Empty);
                            isValid = false;
                        }

                        if (!string.IsNullOrWhiteSpace(identityNum))
                        {
                            var a = await Mediator.Send(new ValidateUserQuery(x => x.NoId == identityNum));
                            if (a)
                            {
                                ToastService.ShowErrorImport(row, 3, identityNum ?? string.Empty);
                                isValid = false;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            var chekcEmail = await Mediator.Send(new ValidateUserQuery(x => x.Email == email));
                            if (chekcEmail)
                            {
                                ToastService.ShowErrorImport(row, 2, email ?? string.Empty);
                                isValid = false;
                            }
                        }

                        var isOk = await CheckUserFormAsync();
                        if (!isOk)
                            isValid = false;

                        if (!isValid)
                            continue;

                        var c = new UserDto
                        {
                            Name = name,
                            Email = email,
                            NoId = identityNum,
                            ReligionId = religionId,
                            DateOfBirth = Convert.ToDateTime(dateOfBirth),
                            Gender = gender,
                            MartialStatus = marital,
                            MobilePhone = mobile,
                            CurrentMobile = currentMobile,
                            HomePhoneNumber = phone,
                            Npwp = npwp,
                            EmergencyName = emergencyName,
                            EmergencyEmail = emergencyEmail,
                            EmergencyPhone = emergencyPhone,
                            IsEmployee = true,

                            NoBpjsKs = bpjsks,
                            NoBpjsTk = bpjstk,
                            Legacy = legacy,
                            SAP = sap,
                            NIP = nip,
                            Oracle = oracle,
                            EmployeeType = empType,
                            JoinDate = Convert.ToDateTime(joinDate)
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new
                        {
                            x.Name,
                            x.Email,
                            x.NoId,
                            x.ReligionId,
                            x.DateOfBirth,
                            x.Gender,
                            x.MartialStatus,
                            x.MobilePhone,
                            x.CurrentMobile,
                            x.HomePhoneNumber,
                            x.Npwp,
                            x.EmergencyName,
                            x.EmergencyEmail,
                            x.EmergencyPhone,
                            x.IsEmployee,
                            x.NoBpjsKs,
                            x.NoBpjsTk,
                            x.Legacy,
                            x.SAP,
                            x.NIP,
                            x.Oracle,
                            x.EmployeeType,
                            x.JoinDate
                        }).ToList();

                        // Panggil BulkValidateProjectQuery untuk validasi bulk
                        var existingProjects = await Mediator.Send(new BulkValidateEmployeeQuery(list));

                        // Filter Project baru yang tidak ada di database
                        list = list.Where(x =>
                            !existingProjects.Any(ev =>
                                ev.Name == x.Name &&
                                ev.Email == x.Email && // Include Email
                                ev.Password == x.Password && // Include Password
                                ev.GroupId == x.GroupId && // Include GroupId
                                ev.NoId == x.NoId && // Include NoId
                                ev.ReligionId == x.ReligionId && // Include ReligionId
                                ev.DateOfBirth == x.DateOfBirth && // Include DateOfBirth
                                ev.Gender == x.Gender && // Include Gender
                                ev.MartialStatus == x.MartialStatus && // Include MartialStatus
                                ev.MobilePhone == x.MobilePhone && // Include MobilePhone
                                ev.CurrentMobile == x.CurrentMobile && // Include CurrentMobile
                                ev.HomePhoneNumber == x.HomePhoneNumber && // Include HomePhoneNumber
                                ev.Npwp == x.Npwp && // Include Npwp
                                ev.EmergencyName == x.EmergencyName && // Include EmergencyName
                                ev.EmergencyEmail == x.EmergencyEmail && // Include EmergencyEmail
                                ev.EmergencyPhone == x.EmergencyPhone && // Include EmergencyPhone

                                ev.IsEmployee == x.IsEmployee &&
                                ev.NoBpjsKs == x.NoBpjsKs &&
                                ev.NoBpjsTk == x.NoBpjsTk &&
                                ev.Legacy == x.Legacy &&
                                ev.SAP == x.SAP &&
                                ev.NIP == x.NIP &&
                                ev.Oracle == x.Oracle &&
                                ev.EmployeeType == x.EmployeeType &&
                                ev.JoinDate == x.JoinDate
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListUserRequest(list));
                        await LoadData();
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "employees_template.xlsx", ExportTemp);
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
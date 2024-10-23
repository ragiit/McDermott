using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;

namespace McDermott.Web.Components.Pages.Medical.Practitioners
{
    public partial class PractitionerPage
    {
        #region Import

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
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

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => ExportTemp.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<UserDto>();

                    var religionNames = new HashSet<string>();

                    var religions = new List<ReligionDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 4].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            religionNames.Add(a.ToLower());
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
                        var isPhysicion = ws.Cells[row, 15].Value?.ToString()?.Trim();
                        var isNurse = ws.Cells[row, 16].Value?.ToString()?.Trim();
                        var sip = ws.Cells[row, 17].Value?.ToString()?.Trim();
                        var sipExpired = ws.Cells[row, 18].Value?.ToString()?.Trim();
                        var str = ws.Cells[row, 19].Value?.ToString()?.Trim();
                        var strExpired = ws.Cells[row, 20].Value?.ToString()?.Trim();
                        bool isValid = true;

                        long? groupId = null;

                        long? religionId = null;
                        if (!string.IsNullOrEmpty(religion))
                        {
                            var cachedParent = religions.FirstOrDefault(x => x.Name.Equals(religion, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 6, religion ?? string.Empty);
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
                                ToastService.ShowErrorImport(row, 8, gender ?? string.Empty);
                                isValid = false;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(marital))
                        {
                            if (!Helper.MartialStatuss.Contains(marital) && !string.IsNullOrEmpty(marital))
                            {
                                ToastService.ShowErrorImport(row, 9, marital ?? string.Empty);
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
                                ToastService.ShowInfo($"Identity Number with number '{identityNum}' in row {row} and column {3} is already exists");
                                isValid = false;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            var chekcEmail = await Mediator.Send(new ValidateUserQuery(x => x.Email == email));
                            if (chekcEmail)
                            {
                                ToastService.ShowInfo($"Email with name '{email}' in row {row} and column {2} is already exists");
                                isValid = false;
                            }
                        }

                        var isP = isPhysicion != null && isPhysicion == "True";
                        var isN = isNurse != null && isNurse == "True";

                        if (isP && isN)
                        {
                            ToastService.ShowInfo("Both Physician and Nurse cannot be selected at the same time. Please choose only one.");
                            isValid = false;
                        }

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
                            IsDoctor = true,
                            IsPhysicion = isPhysicion != null && isPhysicion == "True",
                            IsNurse = isNurse != null && isNurse == "True",
                            SipNo = sip,
                            SipExp = Convert.ToDateTime(sipExpired),
                            StrNo = str,
                            StrExp = Convert.ToDateTime(strExpired)
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
                            x.IsPhysicion,
                            x.IsNurse,
                            x.SipNo,
                            x.SipExp,
                            x.StrNo,
                            x.StrExp,
                        }).ToList();

                        // Panggil BulkValidateProjectQuery untuk validasi bulk
                        var existingProjects = await Mediator.Send(new BulkValidateUserQuery(list));

                        // Filter Project baru yang tidak ada di database
                        list = list.Where(x =>
                            !existingProjects.Any(ev =>
                                ev.Name == x.Name &&
                                ev.Email == x.Email && // Include Email
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
                                ev.EmergencyPhone == x.EmergencyPhone &&
                                ev.IsNurse == x.IsNurse &&
                                ev.IsPhysicion == x.IsPhysicion &&
                                ev.SipNo == x.SipNo &&
                                ev.SipExp == x.SipExp &&
                                ev.StrNo == x.StrNo &&
                                ev.StrExp == x.StrExp
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListUserRequest(list));
                        await LoadData(0, pageSize);
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

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "practitioners_template.xlsx", ExportTemp);
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
            new() { Column = "Physicion", Notes = "True is Physicion, otherwise False"},
            new() { Column = "Nurse", Notes = "True is Nurse, otherwise False"},
            new() { Column = "SIP Number"},
            new() { Column = "SIP Expired"},
            new() { Column = "STR Number"},
            new() { Column = "STR Expired"},
        ];

        #endregion Import

        private List<UserDto> Users = [];
        private List<SpecialityDto> Specialities = [];
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
        private Timer _timer;
        private IBrowserFile BrowserFile { get; set; }

        private bool ShowForm { get; set; } = false;
        private bool IsDeleted { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        private string EmailMask { get; set; } = @"(\w|[.-])+@(\w|-)+\.(\w|-){2,4}";
        private char Placeholder { get; set; } = '_';
        private IEnumerable<ServiceDto> Services { get; set; } = [];
        private IEnumerable<ServiceDto> SelectedServices { get; set; } = [];

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

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;

            return;

            try
            {
                _timer = new Timer(async (_) => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await GetUserInfo();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetUserQuery2(
                x => x.IsDoctor == true,
                searchTerm: searchTerm,
                pageSize: pageSize,
                pageIndex:
                pageIndex,
                includes: [],
                select: x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth,
                    IsPhysicion = x.IsPhysicion,
                    IsNurse = x.IsNurse,
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

        private async Task OnSave()
        {
            if (!FormValidationState)
                return;

            if (UserForm.IsPhysicion && string.IsNullOrWhiteSpace(UserForm.PhysicanCode))
            {
                ToastService.ShowInfoSubmittingForm();
                return;
            }

            UserForm.IsDoctor = true;

            if (UserForm.Id == 0)
            {
                await FileUploadService.UploadFileAsync(BrowserFile);
                var a = SelectedServices.Select(x => x.Id).ToList();
                UserForm.DoctorServiceIds?.AddRange(a);
                await Mediator.Send(new CreateUserRequest(UserForm));
            }
            else
            {
                //if (UserForm.SipFile != null && SelectedDataItems[0].Adapt<UserDto>().SipFile != null && UserForm.SipFile != SelectedDataItems[0].Adapt<UserDto>().SipFile)
                //    Helper.DeleteFile(SelectedDataItems[0].Adapt<UserDto>().SipFile!);

                //if (UserForm.SipFile == null)
                //    Helper.DeleteFile(SelectedDataItems[0].Adapt<UserDto>().SipFile!);

                //UserForm.DoctorServiceIds = SelectedServices.Select(x => x.Id).ToList();
                //await Mediator.Send(new UpdateUserRequest(UserForm));

                //if (UserForm.SipFile is not null && UserForm.SipFile != SelectedDataItems[0].Adapt<UserDto>().SipFile)
                //    await FileUploadService.UploadFileAsync(BrowserFile);

                var userDtoSipFile = SelectedDataItems[0].Adapt<UserDto>().SipFile;

                if (UserForm.SipFile != userDtoSipFile)
                {
                    if (UserForm.SipFile != null)
                        Helper.DeleteFile(UserForm.SipFile);

                    if (userDtoSipFile != null)
                        Helper.DeleteFile(userDtoSipFile);
                }

                UserForm.DoctorServiceIds = SelectedServices.Select(x => x.Id).ToList();
                _ = await Mediator.Send(new UpdateUserRequest(UserForm));

                if (UserForm.SipFile != userDtoSipFile)
                {
                    if (UserForm.SipFile != null)
                        _ = await FileUploadService.UploadFileAsync(BrowserFile);
                }
            }

            SelectedServices = [];

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
                IsDeleted = ((UserDto)args.DataItem).Id == UserLogin.Id;
            }

            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            if (SelectedDataItems is null)
            {
                _ = await Mediator.Send(new DeleteUserRequest(((UserDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<UserDto>>();

                _ = await Mediator.Send(new DeleteUserRequest(ids: a.Where(x => x.Id != UserLogin.Id).Select(x => x.Id).ToList()));
            }
            await LoadData();
        }

        private void NewItem_Click()
        {
            NavigationManager.NavigateTo($"medical/practitioners/{EnumPageMode.Create.GetDisplayName()}");
        }

        private void OnRowDoubleClick()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                NavigationManager.NavigateTo($"medical/practitioners/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}");
                //ShowForm = true;
            }
            catch { }
        }

        private void EditItem_Click()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                NavigationManager.NavigateTo($"medical/practitioners/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}");
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
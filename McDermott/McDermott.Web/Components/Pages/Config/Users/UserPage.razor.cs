using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;
using Group = McDermott.Domain.Entities.Group;

namespace McDermott.Web.Components.Pages.Config.Users
{
    public partial class UserPage
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
            //    IsLoading = true;
            //    try
            //    {
            //        await GetUserInfo();
            //    }
            //    catch { }

            //    await LoadData();
            //    StateHasChanged();

            //    await LoadComboBox();
            //    IsLoading = false;
            //    StateHasChanged();

            //    try
            //    {
            //        Grid?.SelectRow(0, true);
            //    }
            //    catch { }
            //}
        }

        private List<ExportFileData> ExportTemp =
        [
            new() { Column = "Name", Notes = "Mandatory" },
            new() { Column = "Email"},
            new() { Column = "Password"},
            new() { Column = "Group"},
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

                    var groupNames = new HashSet<string>();
                    var religionNames = new HashSet<string>();

                    var groups = new List<GroupDto>();
                    var religions = new List<ReligionDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        var b = ws.Cells[row, 6].Value?.ToString()?.Trim();
                        var c = ws.Cells[row, 9].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            groupNames.Add(a.ToLower());

                        if (!string.IsNullOrEmpty(b))
                            religionNames.Add(b.ToLower());
                    }

                    groups = (await Mediator.Send(new GetGroupQuery(x => groupNames.Contains(x.Name.ToLower()), 0, 0,
                        select: x => new Group
                        {
                            Id = x.Id,
                            Name = x.Name
                        }))).Item1;

                    religions = (await Mediator.Send(new GetReligionQuery(x => religionNames.Contains(x.Name.ToLower()))));

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var name = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var email = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var password = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var group = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        var identityNum = ws.Cells[row, 5].Value?.ToString()?.Trim();
                        var religion = ws.Cells[row, 6].Value?.ToString()?.Trim();
                        var dateOfBirth = ws.Cells[row, 7].Value?.ToString()?.Trim();
                        var gender = ws.Cells[row, 8].Value?.ToString()?.Trim();
                        var marital = ws.Cells[row, 9].Value?.ToString()?.Trim();
                        var mobile = ws.Cells[row, 10].Value?.ToString()?.Trim();
                        var currentMobile = ws.Cells[row, 11].Value?.ToString()?.Trim();
                        var phone = ws.Cells[row, 12].Value?.ToString()?.Trim();
                        var npwp = ws.Cells[row, 13].Value?.ToString()?.Trim();
                        var emergencyName = ws.Cells[row, 14].Value?.ToString()?.Trim();
                        var emergencyEmail = ws.Cells[row, 15].Value?.ToString()?.Trim();
                        var emergencyPhone = ws.Cells[row, 16].Value?.ToString()?.Trim();

                        bool isValid = true;

                        long? groupId = null;
                        if (!string.IsNullOrEmpty(group))
                        {
                            var cachedParent = groups.FirstOrDefault(x => x.Name.Equals(group, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 4, group ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                groupId = cachedParent.Id;
                            }
                        }

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

                        if (string.IsNullOrWhiteSpace(password))
                        {
                            ToastService.ShowErrorImport(row, 3, password ?? string.Empty);
                            isValid = false;
                        }

                        if (!string.IsNullOrWhiteSpace(identityNum))
                        {
                            var a = await Mediator.Send(new ValidateUserQuery(x => x.NoId == identityNum));
                            if (a)
                            {
                                ToastService.ShowErrorImport(row, 5, identityNum ?? string.Empty);
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

                        if (!isValid)
                            continue;

                        var c = new UserDto
                        {
                            Name = name,
                            Email = email,
                            Password = Helper.HashMD5(password),
                            GroupId = groupId,
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
                            IsUser = true,
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new
                        {
                            x.Name,
                            x.Email,
                            x.Password,
                            x.GroupId,
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
                            x.EmergencyPhone
                        }).ToList();

                        // Panggil BulkValidateProjectQuery untuk validasi bulk
                        var existingProjects = await Mediator.Send(new BulkValidateUserQuery(list));

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
                                ev.EmergencyPhone == x.EmergencyPhone // Include EmergencyPhone
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

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "users_template.xlsx", ExportTemp);
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

        [Inject]
        public CustomAuthenticationStateProvider CustomAuth { get; set; }

        private bool Loading = true;
        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private List<UserDto> Users = new();
        private UserDto UserForm = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private bool EditItemsEnabled { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool OnVacation { get; set; } = true;
        private bool IsDeleted { get; set; } = true;
        private bool ShowForm { get; set; } = false;
        private bool VisibleExpiredId { get; set; } = false;
        private char Placeholder { get; set; } = '_';
        private string EmailMask { get; set; } = @"(\w|[.-])+@(\w|-)+\.(\w|-){2,4}";
        private string imageUrl;

        public List<CountryDto> Countries = [];
        public List<ProvinceDto> Provinces = [];
        public List<CityDto> Cities = [];
        public List<DistrictDto> Districts = [];
        public List<VillageDto> Villages = [];
        private List<OccupationalDto> Occupationals = [];
        public List<GroupDto> Groups = [];
        public List<ReligionDto> Religions = [];

        public List<DepartmentDto> Departments = [];
        public List<JobPositionDto> JobPositions = [];

        private readonly List<string> IdentityTypes =
        [
            "KTP",
            "Paspor",
            "SIM",
            "VISA",
        ];

        private readonly List<string> MartialStatuss =
        [
            "Single",
            "Married",
            "Divorced",
            "Widowed",
            "Separated",
            "Unmarried"
        ];

        private string fileKey = Guid.NewGuid().ToString();

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            //var imageFile = e.File;

            //var buffer = new byte[imageFile.Size];
            //await imageFile.OpenReadStream().ReadAsync(buffer);

            //// Ubah buffer gambar menjadi format base64
            //imageUrl = $"data:{imageFile.ContentType};base64,{Convert.ToBase64String(buffer)}";

            try
            {
                var imageFile = e.File;

                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);

                // Ubah buffer gambar menjadi format base64
                imageUrl = $"data:{imageFile.ContentType};base64,{Convert.ToBase64String(buffer)}";

                fileKey = Guid.NewGuid().ToString();
            }
            catch { }
        }

        private async Task PickImage()
        {
            try
            {
                await JsRuntime.InvokeVoidAsync("clickInputFile");
            }
            catch { }
        }

        private void SelectedUserFormChanged(string ee)
        {
            UserForm.TypeId = ee;

            if (ee.Contains("VISA"))
                VisibleExpiredId = true;
            else
                VisibleExpiredId = false;
        }

        private bool FormValidationState = true;

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            await OnSave();
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            //await LoadComboBox();
            PanelVisible = false;
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
            var result = await Mediator.Send(new GetUserQuery2(
                searchTerm: searchTerm,
                pageSize: pageSize,
                pageIndex:
                pageIndex,
                includes:
                [
                    user => user.Group
                ],
                select: x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth,
                    Group = new Domain.Entities.Group
                    {
                        Name = x.Group.Name
                    },
                    IsEmployee = x.IsEmployee,
                    IsPatient = x.IsPatient,
                    IsUser = x.IsUser,
                    IsPhysicion = x.IsPhysicion,
                    IsNurse = x.IsNurse,
                    IsPharmacy = x.IsPharmacy,
                    IsMcu = x.IsMcu,
                    IsHr = x.IsHr,
                }
            ));
            Users = result.Item1;
            totalCount = result.pageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        private bool IsLoading { get; set; } = false;

        private async Task LoadComboBox()
        {
            await LoadDataVillage(0, 10);
            await LoadDataCountry(0, 10);
            await LoadDataProvince(0, 10);
            await LoadDataCity(0, 10);
            await LoadDataGroup(0, 10);
            await LoadDataDistrict(0, 10);
            await LoadDataOccupational(0, 10);
            Departments = (await Mediator.Send(new GetDepartmentQuery())).Item1;
            JobPositions = (await Mediator.Send(new GetJobPositionQuery())).Item1;

            Religions = await Mediator.Send(new GetReligionQuery());
        }

        private async Task OnSave()
        {
            if (!FormValidationState)
                return;

            var a = Users.FirstOrDefault(x => x.NoId == UserForm.NoId && x.Id != UserForm.Id);
            if (a != null)
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
                await Mediator.Send(new CreateUserRequest(UserForm));
            }
            else
            {
                var userDtoSipFile = SelectedDataItems[0].Adapt<UserDto>().SipFile;

                if (UserForm.SipFile != userDtoSipFile)
                {
                    if (UserForm.SipFile != null)
                        Helper.DeleteFile(UserForm.SipFile);

                    if (userDtoSipFile != null)
                        Helper.DeleteFile(userDtoSipFile);
                }

                var result = await Mediator.Send(new UpdateUserRequest(UserForm));

                if (UserForm.SipFile != userDtoSipFile)
                {
                    if (UserForm.SipFile != null)
                        await FileUploadService.UploadFileAsync(BrowserFile);
                }

                if (UserLogin.Id == result.Id)
                {
                    await JsRuntime.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);

                    var aa = (CustomAuthenticationStateProvider)CustomAuth;
                    await aa.UpdateAuthState(string.Empty);

                    await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_INFO, Helper.Encrypt(JsonConvert.SerializeObject(result)), 2);
                }
            }

            await LoadData();
        }

        private void OnCancel()
        {
            UserForm = new();
            ShowForm = false;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                // Check if the single item being deleted has isDeleting = true
                var item = (UserDto)e.DataItem;
                if (item.IsDefaultData)
                {
                    ToastService.ShowWarning("This item is currently being deleted and cannot be deleted again.");
                    return;
                }

                // If there are selected items
                if (SelectedDataItems != null)
                {
                    // Adapt the selected data items to a list
                    var selectedItems = SelectedDataItems.Adapt<List<UserDto>>();

                    // Filter out items with isDeleting = true or UserLogin.Id
                    var validItemsToDelete = selectedItems
                        .Where(x => !x.IsDefaultData && x.Id != UserLogin.Id)
                        .Select(x => x.Id)
                        .ToList();

                    // If no valid items left, show a warning and exit
                    if (validItemsToDelete.Count == 0)
                    {
                        ToastService.ShowWarning("No valid items to delete.");
                        return;
                    }

                    // Send the delete request for the valid items
                    await Mediator.Send(new DeleteUserRequest(ids: validItemsToDelete));
                }
                else
                {
                    // No selected items, deleting a single item, check if isDeleting
                    await Mediator.Send(new DeleteUserRequest(item.Id));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        //private async Task OnDelete(GridDataItemDeletingEventArgs e)
        //{
        //    try
        //    {
        //        if (SelectedDataItems is null)
        //        {
        //            await Mediator.Send(new DeleteUserRequest(((UserDto)e.DataItem).Id));
        //        }
        //        else
        //        {
        //            var a = SelectedDataItems.Adapt<List<UserDto>>();

        //            await Mediator.Send(new DeleteUserRequest(ids: a.Where(x => x.Id != UserLogin.Id).Select(x => x.Id).ToList()));
        //        }
        //        await LoadData();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.HandleException(ToastService);
        //    }
        //}

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

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

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            if (args.DataItem is not null)
            {
                IsDeleted = ((UserDto)args.DataItem).Id == UserLogin.Id;
            }

            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        private IBrowserFile BrowserFile;

        private async Task DownloadFile()
        {
            if (UserForm.Id != 0 && !string.IsNullOrWhiteSpace(UserForm.SipFile))
            {
                await Helper.DownloadFile(UserForm.SipFile, HttpContextAccessor, HttpClient, JsRuntime);
            }
        }

        private async void SelectFiles(InputFileChangeEventArgs e)
        {
            BrowserFile = e.File;
            UserForm.SipFile = e.File.Name;
        }

        private async Task SelectFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "sipFile");
        }

        private void RemoveSelectedFile()
        {
            UserForm.SipFile = null;
        }

        private void OnItemUpdating(string fieldName, object newValue)
        {
            if (fieldName == nameof(UserForm.Name))
            {
                //UserForm = newValue.ToString();
            }
        }

        private async Task NewItem_Click()
        {
            NavigationManager.NavigateTo($"configuration/users/{EnumPageMode.Create.GetDisplayName()}");
            return;
            UserForm = new();
            ShowForm = true;
        }

        private void OnRowDoubleClick()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                NavigationManager.NavigateTo($"configuration/users/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}");
                //ShowForm = true;
            }
            catch { }
        }

        private void EditItem_Click()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                NavigationManager.NavigateTo($"configuration/users/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}");
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

        private async Task OnSearchVillagendexDecrement()
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
            var result = await Mediator.Send(new GetVillageQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refVillageComboBox?.Text ?? ""));
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

        private async Task OnSearchCountryndexDecrement()
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

        private async Task OnSearchCityndexDecrement()
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
            //var result = await Mediator.Send(new GetCityQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refCityComboBox?.Text ?? ""));
            //Cities = result.Item1;
            //totalCountCity = result.pageCount;
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

        private async Task OnSearchProvincendexDecrement()
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
            var result = await Mediator.Send(new GetProvinceQuery
            {
                SearchTerm = refProvinceComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
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
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetDistrictQuery
                {
                    SearchTerm = refDistrictComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                Districts = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = pageIndex;
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
    }
}
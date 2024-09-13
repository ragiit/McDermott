using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Web.Components.Pages.Config
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
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                IsLoading = true;
                try
                {
                    await GetUserInfo();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                await LoadComboBox();
                IsLoading = false;
                StateHasChanged();

                try
                {
                    Grid?.SelectRow(0, true);
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
        public List<GenderDto> Genders = [];
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

        private async Task LoadData()
        {
            ShowForm = false;
            PanelVisible = true;
            SelectedDataItems = [];
            Users = await Mediator.Send(new GetUserQuery());
            Loading = false;
            PanelVisible = false;
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
        }

        private bool IsLoading { get; set; } = false;

        private async Task LoadComboBox()
        {
            var countries = await Mediator.Send(new GetCountryQuery());
            Countries = countries.Item1;
            var result = await Mediator.Send(new GetProvinceQuery());
            Provinces = result.Item1;
            var results = await Mediator.Send(new GetCityQuery());
            Cities = results.Item1;
            var resultDistrict = await Mediator.Send(new GetDistrictQuery());
            Districts = resultDistrict.Item1;
            //Villages = await Mediator.Send(new GetVillageQuery());
            var result2 = await Mediator.Send(new GetGroupQuery(pageIndex: 0, pageSize: short.MaxValue));
            Groups = result2.Item1;
            Religions = await Mediator.Send(new GetReligionQuery());
            Genders = await Mediator.Send(new GetGenderQuery());
            Departments = await Mediator.Send(new GetDepartmentQuery());
            JobPositions = await Mediator.Send(new GetJobPositionQuery());
            var resultOccupationals = await Mediator.Send(new GetOccupationalQuery());
            Occupationals = resultOccupationals.Item1;
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
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

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
            UserForm = new();
            ShowForm = true;
        }

        private void OnRowDoubleClick()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                ShowForm = true;
            }
            catch { }
        }

        private void EditItem_Click()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
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
    }
}
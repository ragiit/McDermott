using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class PractitionerPage
    {
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
            await LoadData();
            await GetUserInfo();
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

            ShowForm = false;
            UserForm = new UserDto();
            SelectedDataItems = [];
            //Specialities = await Mediator.Send(new GetSpecialityQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            //Services = await Mediator.Send(new GetServiceQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            var countries = await Mediator.Send(new GetCountryQuery());
            Countries = countries.Item1;
            var result = await Mediator.Send(new GetProvinceQuery());
            Provinces = result.Item1;
            //Cities = await Mediator.Send(new GetCityQuery());
            var resultDistrict = await Mediator.Send(new GetDistrictQuery());
            Districts = resultDistrict.Item1;
            //Villages = await Mediator.Send(new GetVillageQuery());
            Religions = await Mediator.Send(new GetReligionQuery());
            
            Departments = (await Mediator.Send(new GetDepartmentQuery())).Item1;
            JobPositions = (await Mediator.Send(new GetJobPositionQuery())).Item1;
            Users = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true));

            PanelVisible = false;
        }

        private void RemoveSelectedFile()
        {
            UserForm.SipFile = null;
        }

        private void SelectFiles(InputFileChangeEventArgs e)
        {
            BrowserFile = e.File;
            UserForm.SipFile = e.File.Name;
        }

        private async Task SelectFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "sipFile");
        }

        private async Task DownloadFile()
        {
            if (UserForm.Id != 0 && !string.IsNullOrWhiteSpace(UserForm.SipFile))
            {
                await Helper.DownloadFile(UserForm.SipFile, HttpContextAccessor, HttpClient, JsRuntime);
            }
        }

        #region Grid

        private void OnRowDoubleClick(GridRowClickEventArgs e)
        {
            UserForm = SelectedDataItems[0].Adapt<UserDto>();
            SelectedServices = Services.Where(x => UserForm.DoctorServiceIds is not null && UserForm.DoctorServiceIds.Contains(x.Id)).ToList();
            ShowForm = true;
        }

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
            UserForm = new();
            ShowForm = true;
            UserForm.IsPhysicion = true;
        }

        private void EditItem_Click()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                SelectedServices = Services.Where(x => UserForm.DoctorServiceIds is not null && UserForm.DoctorServiceIds.Contains(x.Id)).ToList();
                ShowForm = true;
            }
            catch { }
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
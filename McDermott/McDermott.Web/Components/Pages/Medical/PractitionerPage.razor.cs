using DevExpress.Data.XtraReports.Native;
using DevExpress.Utils.Design;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;
using System.Security.Claims;

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
        public List<GenderDto> Genders = [];

        private UserDto UserForm = new();
        private GroupMenuDto UserAccessCRUID = new();

        private bool PanelVisible = false;
        private bool FormValidationState = true;
        private bool IsAccess = false;

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
            "Married"
        };

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

            Specialities = await Mediator.Send(new GetSpecialityQuery());
            Services = await Mediator.Send(new GetServiceQuery());
            Countries = await Mediator.Send(new GetCountryQuery());
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Cities = await Mediator.Send(new GetCityQuery());
            Districts = await Mediator.Send(new GetDistrictQuery());
            Villages = await Mediator.Send(new GetVillageQuery());
            Religions = await Mediator.Send(new GetReligionQuery());
            Genders = await Mediator.Send(new GetGenderQuery());
            Departments = await Mediator.Send(new GetDepartmentQuery());
            JobPositions = await Mediator.Send(new GetJobPositionQuery());

            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            ShowForm = false;

            Users = await Mediator.Send(new GetUserPractitionerQuery());

            PanelVisible = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    var result = await NavigationManager.CheckAccessUser(oLocal);
                    IsAccess = result.Item1;
                    UserAccessCRUID = result.Item2;
                }
                catch { }
            }
        }

        private void RemoveSelectedFile()
        {
            UserForm.SipFile = null;
        }

        private async void SelectFiles(InputFileChangeEventArgs e)
        {
            int maxFileSize = 1 * 1024 * 1024;
            var allowedExtenstions = new string[] { ".png", ".jpg", ".jpeg", ".gif" };

            UserForm.SipFile = e.File.Name;

            await FileUploadService.UploadFileAsync(e.File, maxFileSize, []);
        }

        private async Task SelectFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "sipFile");
        }

        private async Task HandleFormSubmit()
        {
        }

        #region Grid

        private void OnRowDoubleClick(GridRowClickEventArgs e)
        {
            UserForm = SelectedDataItems[0].Adapt<UserDto>();
            SelectedServices = Services.Where(x => UserForm.DoctorServiceIds.Contains(x.Id)).ToList();
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

            UserForm.IsEmployee = false;
            UserForm.IsPatient = false;
            UserForm.IsUser = false;
            UserForm.IsDoctor = true;

            if (UserForm.Id == 0)
            {
                var a = SelectedServices.Select(x => x.Id).ToList();
                UserForm.DoctorServiceIds?.AddRange(a);
                await Mediator.Send(new CreateUserRequest(UserForm));
            }
            else
            {
                UserForm.DoctorServiceIds = SelectedServices.Select(x => x.Id).ToList();
                await Mediator.Send(new UpdateUserRequest(UserForm));
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
                IsDeleted = (bool)_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(((UserDto)args.DataItem).Id.ToString())!;
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

                int userActive = (int)_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!.ToInt32()!;

                await Mediator.Send(new DeleteListUserRequest(a.Where(x => x.Id != userActive).Select(x => x.Id).ToList()));
            }
            await LoadData();
        }

        private void NewItem_Click()
        {
            UserForm = new();
            ShowForm = true;
        }

        private void EditItem_Click()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                SelectedServices = Services.Where(x => UserForm.DoctorServiceIds.Contains(x.Id)).ToList();
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
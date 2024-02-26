using DevExpress.Data.XtraReports.Native;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class UserPage
    {
        private GroupMenuDto UserAccessCRUID = new();

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
        public List<GroupDto> Groups = [];
        public List<ReligionDto> Religions = [];
        public List<GenderDto> Genders = [];
        public List<DepartmentDto> Departments = [];
        public List<JobPositionDto> JobPositions = [];

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
            SelectedDataItems = new ObservableRangeCollection<object>();
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

        private async void HandleInvalidSubmit()
        {
            FormValidationState = false;
        }

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

            Countries = await Mediator.Send(new GetCountryQuery());
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Cities = await Mediator.Send(new GetCityQuery());
            Districts = await Mediator.Send(new GetDistrictQuery());
            Villages = await Mediator.Send(new GetVillageQuery());
            Groups = await Mediator.Send(new GetGroupQuery());
            Religions = await Mediator.Send(new GetReligionQuery());
            Genders = await Mediator.Send(new GetGenderQuery());
            Departments = await Mediator.Send(new GetDepartmentQuery());
            JobPositions = await Mediator.Send(new GetJobPositionQuery());

            await LoadData();
        }

        private bool IsAccess = false;

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

        private async Task OnSave()
        {
            if (!FormValidationState)
                return;

            if (Convert.ToBoolean(UserForm.IsPatient))
            {
                var date = DateTime.Now;
                var lastId = Users.Where(x => x.IsPatient == true).ToList().LastOrDefault();

                UserForm.NoRm = lastId is null
                         ? $"{date:dd-MM-yyyy}-0001"
                         : $"{date:dd-MM-yyyy}-{(int.Parse(lastId!.NoRm!.Substring(lastId.NoRm.Length - 4)) + 1):0000}";
            }

            if (UserForm.Id == 0)
                await Mediator.Send(new CreateUserRequest(UserForm));
            else
                await Mediator.Send(new UpdateUserRequest(UserForm));

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
            if (SelectedDataItems is null)
            {
                await Mediator.Send(new DeleteUserRequest(((UserDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<UserDto>>();

                int userActive = (int)HttpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!.ToInt32()!;

                await Mediator.Send(new DeleteListUserRequest(a.Where(x => x.Id != userActive).Select(x => x.Id).ToList()));
            }
            await LoadData();
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            if (args.DataItem is not null)
            {
                IsDeleted = (bool)HttpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(((UserDto)args.DataItem).Id.ToString())!;
            }

            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
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
            catch (Exception e)
            {
                var zz = e;
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
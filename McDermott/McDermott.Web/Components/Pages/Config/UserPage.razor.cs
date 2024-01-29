using DevExpress.Data.XtraReports.Native;
using McDermott.Web.Components.Layout;
using static McDermott.Application.Features.Commands.CityCommand;
using static McDermott.Application.Features.Commands.CountryCommand;
using static McDermott.Application.Features.Commands.DepartmentCommand;
using static McDermott.Application.Features.Commands.DistrictCommand;
using static McDermott.Application.Features.Commands.GenderCommand;
using static McDermott.Application.Features.Commands.GroupCommand;
using static McDermott.Application.Features.Commands.JobPositionCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;
using static McDermott.Application.Features.Commands.ReligionCommand;
using static McDermott.Application.Features.Commands.UserCommand;
using static McDermott.Application.Features.Commands.VillageCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class UserPage
    {
        private BaseAuthorizationLayout AuthorizationLayout = new();

        private bool Loading = true;
        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private List<UserDto> Users = new();
        private UserDto UserForm = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private bool EditItemsEnabled { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool OnVacation { get; set; } = true;
        private bool ShowForm { get; set; } = false;
        private bool VisibleExpiredId { get; set; } = false;
        public List<CountryDto> Countries { get; private set; }
        public List<ProvinceDto> Provinces { get; private set; }
        public List<CityDto> Cities { get; private set; }

        public List<DistrictDto> Districts { get; private set; }
        public List<VillageDto> Villages { get; private set; }
        public List<GroupDto> Groups { get; private set; }
        public List<ReligionDto> Religions { get; private set; }
        public List<GenderDto> Genders { get; private set; }
        public List<DepartmentDto> Departments { get; private set; }
        public List<JobPositionDto> JobPositions { get; private set; }

        private List<string> IdentityTypes = new List<string>
        {
            "KTP",
            "Paspor",
            "SIM",
            "VISA",
        };

        private List<string> MartialStatuss = new List<string>
        {
            "Single",
            "Married"
        };

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

        private async Task OnSave()
        {
            if (!FormValidationState)
                return;

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
                await Mediator.Send(new DeleteListUserRequest(a.Select(x => x.Id).ToList()));
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

        private async Task EditItem_Click()
        {
            try
            {
                var user = SelectedDataItems[0].Adapt<UserDto>();
                UserForm = user;
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
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.CityCommand;
using static McDermott.Application.Features.Commands.CountryCommand;
using static McDermott.Application.Features.Commands.DistrictCommand;
using static McDermott.Application.Features.Commands.GenderCommand;
using static McDermott.Application.Features.Commands.GroupCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;
using static McDermott.Application.Features.Commands.ReligionCommand;
using static McDermott.Application.Features.Commands.UserCommand;
using static McDermott.Application.Features.Commands.VillageCommand;

namespace McDermott.Web.Components.Pages
{
    public partial class UserPage
    {
        public IGrid Grid { get; set; }
        private List<UserDto> Users = new();
        private UserDto UserForm = new();
        private IReadOnlyList<object>? SelectedDataItems { get; set; }
        private bool EditItemsEnabled { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool OnVacation { get; set; } = true;
        public List<CountryDto> Countries { get; private set; }
        public List<ProvinceDto> Provinces { get; private set; }
        public List<CityDto> Cities { get; private set; }
        public List<DistrictDto> Districts { get; private set; }
        public List<VillageDto> Villages { get; private set; }
        public List<GroupDto> Groups { get; private set; }
        public List<ReligionDto> Religions { get; private set; }
        public List<GenderDto> Genders { get; private set; }

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
            Users = await Mediator.Send(new GetUserQuery());
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

            await LoadData();
        }

        private async Task OnSave()
        {
            var a = UserForm;
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
                UserForm.Name = newValue.ToString();
            } 
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
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
            }); ;
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
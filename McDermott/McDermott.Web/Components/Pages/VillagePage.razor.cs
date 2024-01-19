using static McDermott.Application.Features.Commands.CityCommand;

using static McDermott.Application.Features.Commands.DistrictCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;

using static McDermott.Application.Features.Commands.VillageCommand;

namespace McDermott.Web.Components.Pages
{
    public partial class VillagePage
    {
        public IGrid Grid { get; set; }
        private List<VillageDto> Villages = new();
        private List<ProvinceDto> Provinces = new();
        private List<DistrictDto> Districts = new();
        private List<CityDto> Cities = new();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteVillageRequest(((VillageDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Districts = await Mediator.Send(new GetDistrictQuery());
            Cities = await Mediator.Send(new GetCityQuery());
            await LoadData();
        }

        private async Task LoadData()
        {
            Villages = await Mediator.Send(new GetVillageQuery());
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (VillageDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateVillageRequest(editModel));
            else
                await Mediator.Send(new UpdateVillageRequest(editModel));

            await LoadData();
        }
    }
}
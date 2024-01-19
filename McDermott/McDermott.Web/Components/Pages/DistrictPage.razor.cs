using static McDermott.Application.Features.Commands.CityCommand;

using static McDermott.Application.Features.Commands.DistrictCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;

namespace McDermott.Web.Components.Pages
{
    public partial class DistrictPage
    {
        public IGrid Grid { get; set; }
        private List<DistrictDto> Districts = new();
        private List<ProvinceDto> Provinces = new();
        private List<CityDto> Cities = new();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteDistrictRequest(((DistrictDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Cities = await Mediator.Send(new GetCityQuery());
            await LoadData();
        }

        private async Task LoadData()
        {
            Districts = await Mediator.Send(new GetDistrictQuery());
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (DistrictDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateDistrictRequest(editModel));
            else
                await Mediator.Send(new UpdateDistrictRequest(editModel));

            await LoadData();
        }
    }
}
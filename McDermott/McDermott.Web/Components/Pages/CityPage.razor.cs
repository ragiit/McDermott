using static McDermott.Application.Features.Commands.CityCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;

namespace McDermott.Web.Components.Pages
{
    public partial class CityPage
    {
        public IGrid Grid { get; set; }
        private List<CityDto> Cities = new();
        private List<ProvinceDto> Provinces = new();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteCityRequest(((CityDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            Provinces = await Mediator.Send(new GetProvinceQuery());
            await LoadData();
        }

        private async Task LoadData()
        {
            Cities = await Mediator.Send(new GetCityQuery());
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (CityDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateCityRequest(editModel));
            else
                await Mediator.Send(new UpdateCityRequest(editModel));

            await LoadData();
        }
    }
}
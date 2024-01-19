using static McDermott.Application.Features.Commands.CountryCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;

namespace McDermott.Web.Components.Pages
{
    public partial class ProvincePage
    {
        public IGrid Grid { get; set; }

        private List<CountryDto> Countries = new();
        private List<ProvinceDto> Provinces = new();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteProvinceRequest(((ProvinceDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            Countries = await Mediator.Send(new GetCountryQuery());
            await LoadData();
        }

        private async Task LoadData()
        {
            Provinces = await Mediator.Send(new GetProvinceQuery());
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (ProvinceDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateProvinceRequest(editModel));
            else
                await Mediator.Send(new UpdateProvinceRequest(editModel));

            await LoadData();
        }
    }
}
using static McDermott.Application.Features.Commands.ReligionCommand;

namespace McDermott.Web.Components.Pages
{
    public partial class ReligionPage
    {
        public IGrid Grid { get; set; }
        private List<ReligionDto> Religions = new();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteReligionRequest(((ReligionDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            Religions = await Mediator.Send(new GetReligionQuery());
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (ReligionDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateReligionRequest(editModel));
            else
                await Mediator.Send(new UpdateReligionRequest(editModel));

            await LoadData();
        }
    }
}
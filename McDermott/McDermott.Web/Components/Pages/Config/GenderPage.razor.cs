using McDermott.Web.Extentions;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class GenderPage
    {
        private GroupMenuDto UserAccessCRUID = new();
        public IGrid Grid { get; set; }
        private List<GenderDto> Genders = new();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteGenderRequest(((GenderDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

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

        private async Task LoadData()
        {
            Genders = await Mediator.Send(new GetGenderQuery());
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (GenderDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateGenderRequest(editModel));
            else
                await Mediator.Send(new UpdateGenderRequest(editModel));

            await LoadData();
        }
    }
}
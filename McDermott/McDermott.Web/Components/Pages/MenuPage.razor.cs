using McDermott.Application.Dtos;
using static McDermott.Application.Features.Commands.MenuCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;

namespace McDermott.Web.Components.Pages
{
    public partial class MenuPage
    {
        public IGrid Grid { get; set; }
        private List<MenuDto> Menus = new();
        private List<MenuDto> ParentMenuDto = new();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteMenuRequest(((MenuDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            var q = await Mediator.Send(new GetMenuQuery());
            ParentMenuDto = [.. q.Where(x => x.ParentMenu == null).OrderBy(x => x.Sequence)];

            await LoadData();
        }

        private async Task LoadData()
        {
            Menus = await Mediator.Send(new GetMenuQuery());
            Menus = [.. Menus.OrderBy(x => x.ParentMenu == null).ThenBy(x => x.Sequence!.ToInt32())];
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (MenuDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateMenuRequest(editModel));
            else
                await Mediator.Send(new UpdateMenuRequest(editModel));

            await LoadData();
        }
    }
}
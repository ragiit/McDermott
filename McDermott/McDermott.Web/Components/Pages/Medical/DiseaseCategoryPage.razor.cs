using static McDermott.Application.Features.Commands.DiseasCategoryCommand;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class DiseaseCategoryPage
    {
        public IGrid Grid { get; set; }
        private List<DiseasCategoryDto> DiseaseCategorys = new();
        private List<DiseasCategoryDto> ParentCategoryDto = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            await Mediator.Send(new DeleteDiseaseCategoryRequest(((DiseasCategoryDto)e.DataItem).Id));
            await LoadData();
        }

        protected override async Task OnInitializedAsync()
        {
            var q = await Mediator.Send(new GetDiseaseCategoryQuery());
            ParentCategoryDto = [.. q.Where(x => x.ParentCategory == null)];

            await LoadData();
        }

        private async Task LoadData()
        {
            DiseaseCategorys = await Mediator.Send(new GetDiseaseCategoryQuery());
            DiseaseCategorys = [.. DiseaseCategorys.OrderBy(x => x.ParentCategory == null)];
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
        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
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
            });
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
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
        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (DiseasCategoryDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateDiseaseCategoryRequest(editModel));
            else
                await Mediator.Send(new UpdateDiseaseCategoryRequest(editModel));

            await LoadData();
        }
    }
}


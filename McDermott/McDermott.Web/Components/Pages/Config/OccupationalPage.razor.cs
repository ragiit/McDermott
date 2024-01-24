using DevExpress.Data.XtraReports.Native;
using static McDermott.Application.Features.Commands.OccupationalCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class OccupationalPage
    {
        private List<string> extentions = new() { ".xlsx", ".xls" };
        private const string ExportFileName = "ExportResult";
        private IEnumerable<GridEditMode> GridEditModes { get; } = Enum.GetValues<GridEditMode>();
        private IReadOnlyList<object>? SelectedDataItems { get; set; }

        //private IEnumerable<GridSelect> a { get; set; }
        private dynamic dd;

        private int Value { get; set; } = 0;

        private int FocusedRowVisibleIndex { get; set; }

        public IGrid Grid { get; set; }
        private List<OccupationalDto> Occupationals = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            if (SelectedDataItems is null)
            {
                await Mediator.Send(new DeleteOccupationalRequest(((OccupationalDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<OccupationalDto>>();
                await Mediator.Send(new DeleteListOccupationalRequest(a.Select(x => x.Id).ToList()));
            }
            await LoadData();
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private bool EditItemsEnabled { get; set; }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            }); ;
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        // private void ImportItem_Click()
        // {
        //     /// .....
        // }

        // private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        // {
        //     if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
        //     {
        //         e.CssClass = "alt-item";
        //     }
        //     if (e.ElementType == GridElementType.HeaderCell)
        //     {
        //         e.Style = "background-color: rgba(0, 0, 0, 0.08)";
        //         e.CssClass = "header-bold";
        //     }
        // }

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private bool UploadVisible { get; set; } = false;

        protected async Task SelectedFilesChangedAsync(IEnumerable<UploadFileInfo> files)
        {
            UploadVisible = files.ToList().Count == 0;
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading Excel file: {ex.Message}");
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

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task LoadData()
        {
            SelectedDataItems = new ObservableRangeCollection<object>();
            Occupationals = await Mediator.Send(new GetOccupationalQuery());
            Occupationals.ForEach(x =>
            {
                if (string.IsNullOrWhiteSpace(x.Description))
                    x.Description = "-";
            });
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

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (OccupationalDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateOccupationalRequest(editModel));
            else
                await Mediator.Send(new UpdateOccupationalRequest(editModel));

            await LoadData();
        }
    }
}
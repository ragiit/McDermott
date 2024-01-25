using DevExpress.Data.XtraReports.Native;
using static McDermott.Application.Features.Commands.SpecialityCommand;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class SpecialityPage
    {
        private List<string> extentions = new() { ".xlsx", ".xls" };
        private const string ExportFileName = "ExportResult";
        private IEnumerable<GridEditMode> GridEditModes { get; } = Enum.GetValues<GridEditMode>();
        private IReadOnlyList<object>? SelectedDataItems { get; set; }
        private dynamic dd;
        private int Value { get; set; } = 0;

        private int FocusedRowVisibleIndex { get; set; }

        public IGrid Grid { get; set; }
        private List<SpecialityDto> Specialitys = new();

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
                await Mediator.Send(new DeleteSpecialityRequest(((SpecialityDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<SpecialityDto>>();
                await Mediator.Send(new DeleteListSpecialityRequest(a.Select(x => x.Id).ToList()));
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
            Specialitys = await Mediator.Send(new GetSpecialityQuery());
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
            var editModel = (SpecialityDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateSpecialityRequest(editModel));
            else
                await Mediator.Send(new UpdateSpecialityRequest(editModel));

            await LoadData();
        }
    }

    
}
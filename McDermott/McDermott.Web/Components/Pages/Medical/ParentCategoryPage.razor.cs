using Microsoft.JSInterop;
using DevExpress.Data.XtraReports.Native;
using static McDermott.Application.Features.Commands.ParentCategoryCommand;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class ParentCategoryPage
    {
        private List<string> extentions = new() { ".xlsx", ".xls" };
        private const string ExportFileName = "ExportResult";
        private IEnumerable<GridEditMode> GridEditModes { get; } = Enum.GetValues<GridEditMode>();
        private List<ParentCategoryDto> Parents = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; }
        private dynamic dd;
        private int Value { get; set; } = 0;
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        public IGrid Grid { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }
        private async Task LoadData()
        {
            SelectedDataItems = new ObservableRangeCollection<object>();
            Parents = await Mediator.Send(new GetParentCategoryQuery());
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
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

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
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

        private void OnFileUploadStarted(FileUploadEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        //public enum EducationType
        //{
        //    [Display(Name = "Not Stated")]
        //    NoInfo = 0,

        //    [Display(Name = "High school")]
        //    School = 1,

        //    [Display(Name = "College")]
        //    College = 2,

        //    [Display(Name = "University Degree")]
        //    UniversityDegree = 3,

        //    [Display(Name = "PhD")]
        //    PhD = 4
        //}
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var aq = SelectedDataItems.Count;
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteParentCategoryRequest(((ParentCategoryDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<ParentCategoryDto>>();
                    await Mediator.Send(new DeleteListParentCategoryRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                await JsRuntime.InvokeVoidAsync("alert", ee.InnerException.Message); // Alert
            }
        }
        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (ParentCategoryDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateParentCategoryRequest(editModel));
            else
                await Mediator.Send(new UpdateParentCategoryRequest(editModel));

            await LoadData();
        }
    }
}


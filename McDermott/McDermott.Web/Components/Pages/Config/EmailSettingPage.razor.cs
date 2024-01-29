using DevExpress.Data.XtraReports.Native;
using static McDermott.Application.Features.Commands.EmailSettingCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class EmailSettingPage
    {
        private bool PanelVisible { get; set; } = true;
        private string textPopUp = "";

        public IGrid Grid { get; set; }
        private List<EmailSettingDto> EmailSettings = new();

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }

        private List<string> Stts_Ecrypt = new List<string>
        {
            "none",
            "TLS (STARTTLS)",
            "SSL/TLS"
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }
        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            EmailSettings = await Mediator.Send(new GetEmailSettingQuery());
            PanelVisible = false;
        }
        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
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
        private async Task NewItem_Click()
        {
            textPopUp = "Tambah Data";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            textPopUp = "Edit Data";
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
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            if (SelectedDataItems is null)
            {
                await Mediator.Send(new DeleteEmailSettingRequest(((EmailSettingDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<EmailSettingDto>>();
                await Mediator.Send(new DeleteListEmailSettingRequest(a.Select(x => x.Id).ToList()));
            }
            await LoadData();
        }
        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (EmailSettingDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Description))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateEmailSettingRequest(editModel));
            else
                await Mediator.Send(new UpdateEmailSettingRequest(editModel));

            await LoadData();
        }
    }
}

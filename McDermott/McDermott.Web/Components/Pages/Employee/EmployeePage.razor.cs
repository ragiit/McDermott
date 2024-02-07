using DevExpress.Data.XtraReports.Native;
using McDermott.Application.Dtos.Config;
using System.Security.Claims;

namespace McDermott.Web.Components.Pages.Employee
{
    public partial class EmployeePage
    {
        private GroupMenuDto UserAccessCRUID = new();
        private bool PanelVisible = false;
        private bool IsAccess = false;
        private bool ShowForm { get; set; } = false;
        public IGrid Grid { get; set; }
        private List<UserDto> Users = new();
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private bool IsDeleted { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }
        }

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

        #region Grid

        private async Task OnSave()
        {
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            if (args.DataItem is not null)
            {
                IsDeleted = (bool)_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(((UserDto)args.DataItem).Id.ToString())!;
            }

            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void OnDelete()
        {
        }

        private async Task NewItem_Click()
        {
        }

        private async Task EditItem_Click()
        {
            try
            {
                var user = SelectedDataItems[0].Adapt<UserDto>();
                ShowForm = true;
            }
            catch (Exception e)
            {
                var zz = e;
            }
            //await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
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

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
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

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #endregion Grid
    }
}
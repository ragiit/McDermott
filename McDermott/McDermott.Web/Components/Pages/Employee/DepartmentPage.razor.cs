using DevExpress.Data.XtraReports.Native;
using Microsoft.JSInterop;

namespace McDermott.Web.Components.Pages.Employee
{
    public partial class DepartmentPage
    {
        public List<DepartmentDto> Departments = [];
        public List<DepartmentDto> ParentDepartments = [];
        public List<CompanyDto> Companies = [];
        private List<UserDto> Users = [];

        private List<string> DepartmentCategories = new()
        {
            "Department",
            "Unit",
        };

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

            PanelVisible = true;

            SelectedDataItems = new ObservableRangeCollection<object>();
            Companies = await Mediator.Send(new GetCompanyQuery());

            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            Users = await Mediator.Send(new GetUserEmployeeQuery());
            Departments = await Mediator.Send(new GetDepartmentQuery());
            ParentDepartments = await Mediator.Send(new GetDepartmentQuery());

            PanelVisible = false;
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

        #region Default Grid

        private GroupMenuDto UserAccessCRUID = new();
        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteDepartmentRequest(((DepartmentDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DepartmentDto>>();
                    await Mediator.Send(new DeleteListDepartmentRequest(a.Select(x => x.Id).ToList()));
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
            var editModel = (DepartmentDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.ParentId is not null)
                editModel.ParentName = Departments.FirstOrDefault(x => x.Id == editModel.ParentId).Name;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateDepartmentRequest(editModel));
            else
                await Mediator.Send(new UpdateDepartmentRequest(editModel));

            await LoadData();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            EditItemsEnabled = true;
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

        #endregion Default Grid
    }
}
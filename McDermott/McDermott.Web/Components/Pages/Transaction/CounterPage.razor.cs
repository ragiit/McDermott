using DevExpress.Data.XtraReports.Native;
using static McDermott.Application.Features.Commands.Transaction.CounterCommand;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class CounterPage
    {
        #region Relation Data

        private List<CounterDto> counters = new();
        private CounterDto counterForm = new();

        #endregion Relation Data

        #region setings Grid

        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;
        private string textPopUp = "";
        private int CountCard { get; set; } = 0;
        private List<string> NameCard = new();
        public IGrid Grid { get; set; }
        private int ActiveTabIndex { get; set; } = 1;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private GroupMenuDto UserAccessCRUID = new();

        #endregion setings Grid

        #region Async Data And Auth

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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }
            //var by =

            await LoadData();
        }

        private async Task LoadData()
        {
            StateHasChanged();
            PopUpVisible = false;
            PanelVisible = true;
            counters.Clear();
            var c = await Mediator.Send(new GetCounterQuery());
            counters = [.. c.Where(x => x.IsActive == true)];
            //CountCard = [.. counters.Select(x => x.Name);
            PanelVisible = false;
        }

        #endregion Async Data And Auth

        #region Config Grid

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

        #endregion Config Grid

        #region Button Function

        private async Task NewItem_Click()
        {
            counterForm = new();
            PopUpVisible = true;
            textPopUp = "Add Data Counter";
        }

        private async Task EditItem_Click(int Id)
        {
            try
            {
                var General = await Mediator.Send(new GetCounterByIdQuery(Id));
                counterForm = General;
                PopUpVisible = true;
                textPopUp = "Edit Data Counter";
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            }
            catch { }
        }

        private void DeleteItem_Click(int Id)
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #region Button Export, Import And Colmn Chooser

        private async Task InActive_Click(int Id, string Name)
        {
            try
            {
                if (Id != 0)
                {
                    counterForm.Id = Id;
                    counterForm.IsActive = false;
                    counterForm.Name = Name;
                    await Mediator.Send(new UpdateCounterRequest(counterForm));
                    //NavigationManager.NavigateTo("transaction/counter", true);
                    await LoadData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        #endregion Button Export, Import And Colmn Chooser

        private void OnCancel()
        {
            counterForm = new();
            PopUpVisible = false;
        }

        #endregion Button Function

        #region Methode Delete

        private async Task OnDelete(int Id)
        {
            try
            {
                if (Id != null)
                {
                    await Mediator.Send(new DeleteCounterRequest(Id));
                }

                await LoadData();
            }
            catch { }
        }

        #endregion Methode Delete

        #region Methode Save And Update

        private async Task OnSave()
        {
            try
            {
                counterForm.IsActive = true;
                var edit = counterForm;
                if (counterForm.Id == 0)
                    await Mediator.Send(new CreateCounterRequest(counterForm));
                else
                    await Mediator.Send(new UpdateCounterRequest(counterForm));

                counterForm = new();
                await LoadData();
            }
            catch { }
        }

        #endregion Methode Save And Update
    }
}
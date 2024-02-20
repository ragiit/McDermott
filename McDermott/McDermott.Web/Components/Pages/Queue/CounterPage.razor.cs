using DevExpress.Data.XtraReports.Native;
using Microsoft.AspNetCore.Components;
using static McDermott.Application.Features.Commands.Transaction.CounterCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class CounterPage
    {
        #region Relation Data

        private List<CounterDto> counters = new();
        private List<CounterDto> countersActive = new();
        private List<CounterDto> countersInActive = new();
        private CounterDto counterForm = new();
        private List<KioskDto> kiosks = new();

        #endregion Relation Data

        #region setings Grid

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private BaseAuthorizationLayout AuthorizationLayout = new();
        private GroupMenuDto UserAccessCRUID = new();
        public IGrid Grid { get; set; }
        private List<string> NameCard = new();
        private string textPopUp = "";
        private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;
        private bool ArchiveCard { get; set; } = false;
        private bool EditItemsEnabled { get; set; }
        private bool GirdDetail { get; set; } = false;
        private int CountCard { get; set; } = 0;

        private int ActiveTabIndex { get; set; } = 1;

        private int FocusedRowVisibleIndex { get; set; }

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
            countersActive.Clear();
            counters = await Mediator.Send(new GetCounterQuery());
            countersActive = [.. counters.Where(x => x.IsActive == true)];
            countersInActive = [.. counters.Where(x => x.IsActive == false)];
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

        private void NewItem_Click()
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

        #region Card

        private async Task InActive_Click(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var a = counters.Where(x => x.Id == Id).FirstOrDefault();
                    if (a.IsActive == true)
                    {
                        counterForm.Id = Id;
                        counterForm.IsActive = false;
                        counterForm.Name = a.Name;
                        await Mediator.Send(new UpdateCounterRequest(counterForm));
                    }
                    else
                    {
                        counterForm.Id = Id;
                        counterForm.IsActive = true;
                        counterForm.Name = a.Name;
                        await Mediator.Send(new UpdateCounterRequest(counterForm));
                    }
                    //NavigationManager.NavigateTo("transaction/counter", true);
                    await LoadData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task DetailList(int Id)
        {
            try
            {
                GirdDetail = true;
                var a = await Mediator.Send(new GetKioskQuery());
                kiosks = [.. a.Where(x => x.CounterId == Id)];
            }
            catch
            {
            }
        }

        private async Task Archive_Click()
        {
            try
            {
                ArchiveCard = true;
                await LoadData();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void onBack()
        {
            try
            {
                ArchiveCard = false;
                kiosks.Clear();
            }
            catch { }
        }

        #endregion Card

        #region Button Export, Import And Colmn Chooser

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
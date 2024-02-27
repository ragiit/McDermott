using DevExpress.Data.XtraReports.Native;
using McDermott.Application.Dtos.Queue;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;
using static McDermott.Application.Features.Commands.Transaction.CounterCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class CounterPage
    {
        #region Relation Data

        private List<CounterDto> counters = new();
        private List<CounterDto> countersActive = new();
        private List<CounterDto> countersInActive = new();
        private List<ServiceDto> Services = [];
        private List<UserDto> Physicians = [];
        public List<UserDto> Phys = new();
        private List<ServiceDto> ServiceK = new();
        private List<ServiceDto> ServiceP = [];
        private CounterDto counterForm = new();
        private List<KioskQueueDto> KiosksQueue = new();
        private List<KioskDto> Kiosks = new();

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
        private bool showFormProcess { get; set; } = false;
        private bool GirdDetail { get; set; } = false;
        private int CountCard { get; set; } = 0;
        private int _ServiceId { get; set; }
        private int idServiceK { get; set; }
        private int ActiveTabIndex { get; set; } = 1;
        private string NameCounter { get; set; } = string.Empty;
        private string NameServices { get; set; } = string.Empty;
        private string NameServicesK { get; set; } = string.Empty;
        private string? userBy;
        private User? User = new();

        private int FocusedRowVisibleIndex { get; set; }

        #endregion setings Grid

        #region Async Data And Auth

        private int Serviced
        {
            get => _ServiceId;
            set
            {
                _ServiceId = value;
                counterForm.ServiceId = value;
                List<ServiceDto> service = new();
                service = Services.Where(x => x.Id == value).ToList();

                foreach (var i in service)
                {
                    if (i != null)
                    {
                        ServiceK = Services.Where(x => x.Id == i.ServicedId).ToList();
                        Phys = Physicians.Where(x => x.DoctorServiceIds.Contains(value)).ToList();
                    }
                }

                //var schedules = await Mediator.Send(new GetDoctorScheduleQuery());
            }
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
            counterForm = new();
            counters = await Mediator.Send(new GetCounterQuery());
            Services = await Mediator.Send(new GetServiceQuery());
            ServiceP = [.. Services.Where(x => x.IsPatient == true)];
            var Physician = await Mediator.Send(new GetUserQuery());
            Physicians = [.. Physician.Where(x => x.IsPhysicion == true)];
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
            }
            catch { }
        }

        private void DeleteItem_Click(int Id)
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #region Card

        private async Task ShowOnProcess(int id)
        {
            try
            {
                StateHasChanged();
                PanelVisible = true;
                counterForm = new();
                var General = await Mediator.Send(new GetCounterByIdQuery(id));
                counterForm.Id = General.Id;
                counterForm.Name = General.Name;
                //PopUpVisible = true;
                textPopUp = "Configurtion Counter " + counterForm.Name;
                showFormProcess = true;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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

        private void CloseDetail()
        {
            GirdDetail = false;
        }

        private async Task DetailList(int Id)
        {
            try
            {
                GirdDetail = true;
                var General = await Mediator.Send(new GetCounterByIdQuery(Id));
                var a = await Mediator.Send(new GetKioskQuery());
                var b = await Mediator.Send(new GetKioskQueueQuery());
                KiosksQueue = [.. b.Where(x => x.ServiceId == General.ServiceId)];
                NameCounter = "Queue Listing Counter " + General.Name;
                var q = KiosksQueue.Select(x => x.NoQueue).ToList();
                var s = General.ServiceId;
                var sk = General.ServiceKId;
                NameServices = Services.Where(x => x.Id == s).Select(x => x.Name).FirstOrDefault();
                if (sk != null)
                {
                    NameServicesK = Services.Where(x => x.Id == sk).Select(x => x.Name).FirstOrDefault();
                }
                else
                {
                    NameServicesK = "-";
                }
                User = await oLocal.GetUserInfo();
                userBy = User.Name;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                Kiosks.Clear();
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
                counterForm.Status = "open";
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

        private async Task OnProcess()
        {
            //counterForm.Name = General.Name;
            counterForm.IsActive = true;
            counterForm.Status = "on process";
            var edit = counterForm;
            if (counterForm.Id != 0)
            {
                await Mediator.Send(new UpdateCounterRequest(counterForm));
            }
            showFormProcess = false;
            await LoadData();
        }

        #endregion Methode Save And Update
    }
}
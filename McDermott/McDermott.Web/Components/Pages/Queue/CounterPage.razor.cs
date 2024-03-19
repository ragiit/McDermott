using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;

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
        public IGrid Grid { get; set; }
        private List<string> NameCard = new();
        private string textPopUp = "";
        private bool PanelVisible { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;
        private bool ArchiveCard { get; set; } = false;
        private bool EditItemsEnabled { get; set; }
        private bool showFormProcess { get; set; } = false;
        private bool GirdDetail { get; set; } = false;
        private long CountCard { get; set; } = 0;
        private long? _ServiceId { get; set; }
        private long? _physicionId { get; set; }
        private long idServiceK { get; set; }
        private long ActiveTabIndex { get; set; } = 1;
        private string NameCounter { get; set; } = string.Empty;
        private string NameServices { get; set; } = string.Empty;
        private string NameServicesK { get; set; } = string.Empty;
        private long? PhysicianId { get; set; }
        private string? userBy;
        private User? User = new();

        private int FocusedRowVisibleIndex { get; set; }

        #endregion setings Grid

        #region Async Data And Auth

        private long? SelectServiced
        {
            get => _ServiceId;
            set
            {
                _ServiceId = value;

                if (value is null)
                    return;

                counterForm.ServiceKId = value;
                List<ServiceDto> service = new();
                service = Services.Where(x => x.Id == value).ToList();

                //get Service Flag P
                ServiceP = Services.Where(x => x.ServicedId == value && x.IsPatient == true).ToList();

            }
        }

        private long? SelectPhysicion
        {
            get => _physicionId;
            set
            {
                _physicionId = value;

                if (value is null)
                    return;

                counterForm.ServiceId = value;
                List<ServiceDto> service = new();
                service = Services.Where(x => x.Id == value).ToList();

                //get Physicion
                foreach (var i in service)
                {
                    if (i != null)
                    {
                        Phys = Physicians.Where(x => x.DoctorServiceIds.Contains(value.GetValueOrDefault())).ToList();
                    }
                }

            }
        }

        public MarkupString GetIssuePriorityIconHtml(string status)
        {
            string priorytyClass = "info";
            string title = "Call";
            if (status == "call")
            {
                priorytyClass = "info";
                title = " Call ";

                string html = string.Format("<div class='row justify-content-center'><div class='col-sm-5 pl-0'><span class='badge bg-{0} py-1 px-4' title='{1}'>{1}</span></div></div>", priorytyClass, title);
                return new MarkupString(html);
            }
            else if (status == "hadir")
            {
                priorytyClass = "success";
                title = " Hadir ";

                string html = string.Format("<div class='row justify-content-center'><div class='col-sm-5 pl-0'><span class='badge bg-{0} py-1 px-4' title='{1}'>{1}</span></div></div>", priorytyClass, title);
                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    await GetUserInfo();
                }
                catch { }
            }
        }

        private async Task GetUserInfo()
        {
            try
            {
                var user = await UserInfoService.GetUserInfo();
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
                
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
            ServiceK = [.. Services.Where(x => x.IsKiosk == true)];
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

        private async Task EditItem_Click(long Id)
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

        private void DeleteItem_Click(long Id)
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #region Card

        private async Task ShowOnProcess(long id)
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
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task InActive_Click(long Id)
        {
            try
            {
                if (Id != 0)
                {
                    var a = counters.Where(x => x.Id == Id).FirstOrDefault();

                    counterForm.Id = Id;
                    counterForm.Name = a.Name;
                    counterForm.Status = "open";
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

        private void CloseDetail()
        {
            GirdDetail = false;
        }

        private async Task DetailList(long Id)
        {
            try
            {
                //    GirdDetail = true;
                //    var General = await Mediator.Send(new GetCounterByIdQuery(Id));
                //    if (General.PhysicianId != null)
                //    {
                //        PhysicianId = General.PhysicianId;
                //    }
                //    var a = await Mediator.Send(new GetKioskQuery());
                //    var b = await Mediator.Send(new GetKioskQueueQuery());
                //    KiosksQueue = [.. b.Where(x => x.ServiceId == General.ServiceId && x.CreatedDate.Value.Date == DateTime.Now.Date)];
                //    NameCounter = "Queue Listing Counter " + General.Name;
                //    var q = KiosksQueue.Select(x => x.NoQueue).ToList();
                //    var s = General.ServiceId;
                //    var sk = General.ServiceKId;

                //    NameServices = Services.Where(x => x.Id == s).Select(x => x.Name).FirstOrDefault();
                //    if (sk != null)
                //    {
                //        NameServicesK = Services.Where(x => x.Id == sk).Select(x => x.Name).FirstOrDefault();
                //    }
                //    else
                //    {
                //        NameServicesK = "-";
                //    }

                //    User = await oLocal.GetUserInfo();
                //    userBy = User.Name;
                NavigationManager.NavigateTo($"/queue/counter-view/{Id}");
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
            showFormProcess = false;
        }

        #endregion Button Function

        #region Methode Delete

        private async Task OnDelete(long Id)
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

        private async Task StopProcess(long id)
        {
            try
            {
                var a = counters.Where(x => x.Id == id).FirstOrDefault();
                counterForm.Id = a.Id;
                counterForm.Name = a.Name;
                counterForm.ServiceId = a.ServiceId;
                counterForm.ServiceKId = a.ServiceKId;
                counterForm.IsActive = true;
                counterForm.Status = "stop";

                if (a != null)
                {
                    await Mediator.Send(new UpdateCounterRequest(counterForm));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task ResumeProcess(long id)
        {
            try
            {
                var a = counters.Where(x => x.Id == id).FirstOrDefault();
                counterForm.Id = a.Id;
                counterForm.Name = a.Name;
                counterForm.ServiceId = a.ServiceId;
                counterForm.ServiceKId = a.ServiceKId;
                counterForm.IsActive = true;
                counterForm.Status = "on process";

                if (a != null)
                {
                    await Mediator.Send(new UpdateCounterRequest(counterForm));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task OnProcess()
        {
            try
            {
                counterForm.IsActive = true;
                counterForm.Status = "on process";
                var edit = counterForm;
                if (counterForm.Id != 0)
                {
                    await Mediator.Send(new UpdateCounterRequest(counterForm));
                }
                NavigationManager.NavigateTo($"/queue/counter-view/{counterForm.Id}");
                showFormProcess = false;
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        #endregion Methode Save And Update
    }
}
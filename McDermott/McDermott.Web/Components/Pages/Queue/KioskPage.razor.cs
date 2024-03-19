using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.KioskConfigCommand;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class KioskPage
    {
        #region Relation Data

        public List<KioskDto> Kiosks = new();
        public List<KioskQueueDto> KioskQueues = new();
        public KioskQueueDto ViewQueue = new();
        public List<KioskConfigDto> KioskConf = new();
        public List<ServiceDto> Services = new();
        public List<ServiceDto> serv = new List<ServiceDto>();
        public List<UserDto> Phys = new();
        public ServiceDto servs = new();
        public List<UserDto> Patients = new();
        public List<UserDto> Physician = new();

        #endregion Relation Data

        #region setings Grid

        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool PanelVisible { get; set; } = true;

        private bool showForm { get; set; } = false;
        private bool showPhysician { get; set; } = false;
        private string textPopUp = "";
        private string HeaderName { get; set; } = string.Empty;
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        //private GroupMenuDto UserAccessCRUID = new();

        #endregion setings Grid

        #region Data Static And Variable Additional

        [Parameter]
        public long Id { get; set; }

        private List<string> type = new List<string>
        {
            "NIP",
            "Oracle",
            "SAP",
            "Legacy",
            "NIK"
        };
        private IEnumerable<ServiceDto> SelectedServices = [];
        private KioskDto FormKios = new();
        private KioskQueueDto FormQueue = new();
        private Group? group;
        private GeneralConsultanServiceDto FormGeneral= new();
        private string? NamePatient { get; set; } = string.Empty;
        private string? statBPJS { get; set; } = string.Empty;
        private string Bpjs { get; set; } = string.Empty;
        private long? CountServiceId { get; set; }
        private long _ServiceId { get; set; }
        private bool showQueue { get; set; } = false;
       
        #endregion Data Static And Variable Additional

        #region Async Data And Auth

        private long ServiceId
        {
            get => _ServiceId;
            set
            {
                _ServiceId = value;
                LoadPhysicians(value);               
                showPhysician = true;               
            }
        }

        private async Task LoadPhysicians(long serviceId)
        {
            // Dapatkan daftar dokter
            var doctors = await Mediator.Send(new GetUserQuery());

            // Dapatkan ID dokter yang terkait dengan layanan (service) yang dipilih
            var doctorIds = doctors
                .Where(u => u.DoctorServiceIds?.Contains(serviceId) ?? false) // Filter dokter berdasarkan serviceId
                .Select(u => u.Id)
                .ToList();

            // Dapatkan dokter yang sesuai dengan daftar ID dokter yang ditemukan
            var filteredDoctors = doctors
                .Where(u => doctorIds.Contains(u.Id))
                .ToList();

            // Tampilkan daftar dokter
            Phys = filteredDoctors;
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
            Id = $"{NavigationManager.Uri.Replace(NavigationManager.BaseUri + "queue/kiosk/", "")}".ToInt32();
            try
            {
                await GetUserInfo();

            }
            catch { }
            //var by =

            await LoadData();
        }

        private void ReloadPage()
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        var result = await NavigationManager.CheckAccessUser(oLocal);
        //        IsAccess = result.Item1;
        //        UserAccessCRUID = result.Item2;
        //    }
        //    catch { }
        //    Kiosks = await Mediator.Send(new GetKioskQuery());

        //    Id = $"{NavigationManager.Uri.Replace(NavigationManager.BaseUri + "queue/kiosk/", "")}".ToInt32();
        //    await LoadData();
        //    foreach (var i in KioskConf)
        //    {
        //        HeaderName = i.Name;
        //        break;
        //    }
        //}

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            var ad = Id;
            Id = Id;
        }

        private async Task LoadData()
        {
            var cs = UserAccessCRUID.Group.Name;
            PanelVisible = true;
            StateHasChanged();
            showForm = false;
            Physician = await Mediator.Send(new GetUserQuery());
            KioskQueues = await Mediator.Send(new GetKioskQueueQuery());
            ViewQueue = KioskQueues.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            SelectedDataItems = new ObservableRangeCollection<object>();
            Kiosks = await Mediator.Send(new GetKioskQuery());
            Services = await Mediator.Send(new GetServiceQuery());
            var kconfig = await Mediator.Send(new GetKioskConfigQuery());
            KioskConf = kconfig.Where(x => x.Id == Id).ToList();
            //var bse = UserAccessCRUID.GroupId;
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

        //private void UpdateEditItemsEnabled(bool enabled)
        //{
        //    EditItemsEnabled = enabled;
        //}

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            //UpdateEditItemsEnabled(true);
        }

        #endregion Config Grid

        #region Button Function

        private async Task NewItem_Click()
        {
            showForm = true;
            textPopUp = "Add Data Kiosk";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            try
            {
                var General = SelectedDataItems[0].Adapt<KioskDto>();
                FormKios = General;
                showForm = true;
                textPopUp = "Edit Data Kiosk";
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            }
            catch { }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #region Button Export, Import And Colmn Chooser

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

        #endregion Button Export, Import And Colmn Chooser

        private async Task OnCanceled(long id)
        {
            try
            {
                if (id != 0)
                {
                    await Mediator.Send(new DeleteKioskQueueRequest(id));
                }
                showQueue = false;
                FormKios = new();
                ToastService.ShowError("Antrian Dibatalkan!!");
                ReloadPage();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void OnCancel()
        {
            showQueue = false;
            FormKios = new();
            showForm = false;
        }

        private async Task OnSearch()
        {
            var types = FormKios.Type;
            var InputSearch = FormKios.NumberType;
            Patients = await Mediator.Send(new GetDataUserForKioskQuery(types, InputSearch));

            if (Patients != null)
            {
                showForm = true;
                NamePatient = Patients.Select(x => x.Name).FirstOrDefault();
                FormKios.PatientId = Patients.Select(x => x.Id).FirstOrDefault();
                FormKios.BPJS = Patients.Select(x => x.NoBpjsKs).FirstOrDefault();
                if (FormKios.BPJS != null)
                {
                    FormKios.StageBpjs = true;
                    statBPJS = "Active";
                }
                else
                {
                    FormKios.StageBpjs = false;
                    statBPJS = "InActive";
                }

                foreach (var kiosk in KioskConf)
                {
                    var serviceIds = kiosk.ServiceIds;
                    CountServiceId = KioskConf.SelectMany(k => k.ServiceIds).Count();

                    if (CountServiceId > 1)
                    {
                        serv.AddRange(Services.Where(service => serviceIds.Contains(service.Id)));
                    }
                    else
                    {
                        FormKios.ServiceId = Services.FirstOrDefault(service => serviceIds.Contains(service.Id))?.Id;
                        var serviceId = FormKios.ServiceId;
                        if (serviceId.HasValue)
                        {
                            LoadPhysicians(serviceId.Value);
                        }

                        //var matchingPhysicians = Physician.Where(phy => phy.DoctorServiceIds.Contains(serviceId.Value));

                        //if (matchingPhysicians.Any())
                        //{
                        showPhysician = true;
                        //    Phys.AddRange(matchingPhysicians.Where(phy => phy.IsDoctor == true));
                        //}
                    }
                }
            }
            else
            {
                showForm = false;
            }
        }

        #endregion Button Function

        #region Methode Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteKioskRequest(((KioskDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<KioskDto>>();
                    await Mediator.Send(new DeleteListKioskRequest(a.Select(x => x.Id).ToList()));
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
                var edit = FormKios;

                if (FormKios.Id == 0)
                {
                    // Mendapatkan ID dari hasil CreateKioskRequest
                    var checkId = await Mediator.Send(new CreateKioskRequest(FormKios));

                    // Mendapatkan antrian kiosk
                    var todayQueues = KioskQueues
                        .Where(x => x.ServiceId == checkId.ServiceId && x.CreatedDate.Value.Date == DateTime.Now.Date).ToList();

                    // Menentukan nomor antrian
                    if (todayQueues.Count == 0)
                    {
                        FormQueue.QueueNumber = 1;
                    }
                    else
                    {
                        var GetNoQueue = todayQueues.OrderByDescending(x => x.QueueNumber).FirstOrDefault();
                        FormQueue.QueueNumber = (long)GetNoQueue.QueueNumber + 1;
                    }

                    // mendapatkan service counter Id
                    var skId = Services.Where(x => x.Id == checkId.ServiceId).Select(x => x.ServicedId).FirstOrDefault();
                    // Mengisi informasi antrian
                    FormQueue.KioskId = checkId.Id;
                    FormQueue.ServiceId = checkId.ServiceId;
                    FormQueue.ServiceKId = skId;
                    FormQueue.QueueStatus = "waiting";

                    // Membuat KioskQueue baru
                    var QueueKioskId = await Mediator.Send(new CreateKioskQueueRequest(FormQueue));
                    showQueue = true;
                    ToastService.ShowSuccess("Number Queue is Generated Succces!!");

                    FormGeneral.PatientId = FormKios.PatientId;
                    FormGeneral.ServiceId = FormKios.ServiceId;
                    FormGeneral.KioskQueueId = QueueKioskId.Id;
                    FormGeneral.RegistrationDate = DateTime.Now;
                    if(checkId.PhysicianId != null)
                    {
                        FormGeneral.PratitionerId = FormKios.PhysicianId;

                    }
                    await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormGeneral));
                }
                else
                {
                    await Mediator.Send(new UpdateKioskRequest(FormKios));
                }

                FormKios = new();
                await LoadData();
            }
            catch { }
        }

        #endregion Methode Save And Update
    }
}
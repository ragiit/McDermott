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
        public List<ClassTypeDto> classTypes = new();
        public List<GroupDto> groups = new();
        public List<InsurancePolicyDto> InsurancePolices = [];
        public InsurancePolicyDto BPJS = new();

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
            "Family Relation"
        };

        private bool NumberType { get; set; } = true;
        private IEnumerable<ServiceDto> SelectedServices = [];
        private KioskDto FormKios = new();
        private KioskQueueDto FormQueue = new();
        private Group? group;
        private GeneralConsultanServiceDto FormGeneral = new();
        private string? NamePatient { get; set; } = string.Empty;
        private string? statBPJS { get; set; } = string.Empty;

        private string captions { get; set; } = "Number";
        private long? CountServiceId { get; set; }
        private long _ServiceId { get; set; }
        private bool showQueue { get; set; } = false;
        private bool showClass { get; set; } = false;
        private bool ActiveBack { get; set; } = false;

        #endregion Data Static And Variable Additional

        #region Async Data And Auth

        private long ServiceId
        {
            get => _ServiceId;
            set
            {
                _ServiceId = value;
                LoadPhysicians(value);
                FormKios.ServiceId = value;
                showPhysician = true;
            }
        }

        private void UserSelected(UserDto user)
        {
            BPJS = InsurancePolices.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (BPJS is not null)
            {
                FormKios.BPJS = BPJS.PolicyNumber;
                if (BPJS.Active)
                {
                    FormKios.StageBpjs = true;
                    statBPJS = "Active";
                }
                else
                {
                    FormKios.StageBpjs = false;
                    statBPJS = "InActive";
                }
            }
            else
            {
                statBPJS = "no BPJS number";
            }
        }

        public void SelectedItemChanged(string kiosk)
        {
            if (kiosk == "Family Relation")
            {
                captions = "Name Family";
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

            var NameGroup = groups.FirstOrDefault(x => x.Id == UserAccessCRUID.GroupId);
            if (NameGroup == null)
            {
                ActiveBack = false;
            }
            else if (NameGroup.Name == "Nurse" || NameGroup.Name == "Perawat" || NameGroup.Name == "Nursing")
            {
                ActiveBack = true;
            }
            await LoadData();
            foreach (var i in KioskConf)
            {
                HeaderName = i.Name;
                break;
            }
        }

        private void ReloadPage()
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            var ad = Id;
            Id = Id;
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            StateHasChanged();
            showForm = false;
            FormKios = new();
            Physician = await Mediator.Send(new GetUserQuery());
            KioskQueues = await Mediator.Send(new GetKioskQueueQuery());
            ViewQueue = KioskQueues.OrderByDescending(x => x.CreatedDate).FirstOrDefault() ?? new();
            InsurancePolices = await Mediator.Send(new GetInsurancePolicyQuery());
            SelectedDataItems = new ObservableRangeCollection<object>();
            Kiosks = await Mediator.Send(new GetKioskQuery());
            Services = await Mediator.Send(new GetServiceQuery());
            var kconfig = await Mediator.Send(new GetKioskConfigQuery());
            KioskConf = kconfig.Where(x => x.Id == Id).ToList();
            classTypes = await Mediator.Send(new GetClassTypeQuery());
            groups = await Mediator.Send(new GetGroupQuery());

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
                    var cekId = await Mediator.Send(new GetGeneralConsultanServiceQuery()); //get Data General Consultation
                    var GId = cekId.Where(x => x.KioskQueueId == id).Select(x => x.Id).FirstOrDefault(); //Get Id in General Consultation where KioskQueueId = id
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(GId)); //Delete Data in General Consultation where id

                    await Mediator.Send(new DeleteKioskQueueRequest(id)); // delete data kioskQueue in id
                }
                showQueue = false;
                FormKios = new();
                ToastService.ShowError("Canceled Queue!!");
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
            var group = await Mediator.Send(new GetGroupQuery());
            var NameGroup = group.FirstOrDefault(x => x.Id == UserAccessCRUID.GroupId) ?? new();
            var types = FormKios.Type;
            var InputSearch = FormKios.NumberType ?? string.Empty;
            Patients = await Mediator.Send(new GetDataUserForKioskQuery(InputSearch));

            if (Patients != null)
            {
                showForm = true;
                NamePatient = Patients.Select(x => x.Name).FirstOrDefault();
                FormKios.PatientId = Patients.Select(x => x.Id).FirstOrDefault();
                BPJS = InsurancePolices.Where(x => x.UserId == FormKios.PatientId).FirstOrDefault();
                if (BPJS is not null)
                {
                    FormKios.BPJS = BPJS.PolicyNumber;
                    if (BPJS.Active)
                    {
                        FormKios.StageBpjs = true;
                        statBPJS = "Active";
                    }
                    else
                    {
                        FormKios.StageBpjs = false;
                        statBPJS = "InActive";
                    }
                }
                else
                {
                    statBPJS = "no BPJS number";
                }
                if (NameGroup.Name == "Nurse" || NameGroup.Name == "Perawat" || NameGroup.Name == "Nursing")
                {
                    showClass = true;
                }

                foreach (var kiosk in KioskConf)
                {
                    var serviceIds = kiosk.ServiceIds ?? [];
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
                            await LoadPhysicians(serviceId.Value);
                        }

                        showPhysician = true;
                    }
                }
            }
            else
            {
                showForm = false;
            }
        }

        private async Task onPrint()
        {
            var aas = ViewQueue.QueueNumber.ToString() ?? string.Empty;
            string queueNumber = aas.PadLeft(3, '0');

            // HTML untuk dokumen cetak
            string contentToPrint = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Cetak Antrian</title>
                <style>
                   @page {{size: 8cm 8cm; /* Ukuran kertas 8x8 cm */
                        margin: 0; /* Hilangkan margin default */
                    }}
                    body {{font - family: Arial, sans-serif;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                        margin: 0;
                        padding: 0;
                    }}
                    .print-container {{width: 8cm;
                        height: 8cm;
                        padding: 20px;
                        box-sizing: border-box;
                        flex-direction: column;
                        justify-content: center;
                        align-items: center;
                        border: 1px solid black;
                    }}
                    .company{{align - items: center;
                      justify-content: center;

                        text-align:center;
                    }}
                    .company-logo img {{width: 50px;
                        height: auto;
                        margin-bottom: 10px;
                    }}
                    .company-name {{text - align: center;
                        font-size: 18px;
                        font-weight: bold;
                        margin-bottom: 10px;
                    }}
                    .queue-label {{font - size: 14px;
                        margin-bottom: 5px;
                    }}
                    .queue-number {{text - align: center;
                        font-size: 48px;
                        font-weight: bold;
                    }}
                </style>
            </head>
            <body>
                <page>
                    <div class='print-container'>
                        <div class=""company"">
                          <div class='company-logo'><img src='https://th.bing.com/th/id/R.11c17a8cffdce4bc047102db49a94a51?rik=YMKOe232a1ee3w&riu=http%3a%2f%2flogos-download.com%2fwp-content%2fuploads%2f2016%2f02%2fBMW_logo_big_transparent_png.png&ehk=AhLghiJcc6OJgtYYdNOiiM061S%2fa11BCNRbBYQtUBjI%3d&risl=&pid=ImgRaw&r=0' alt='Logo Perusahaan'></div>
                        </div>
                        <div class='company-name'>McHealthCare</div>
                        <div class='queue-label'>Your Number :</div>
                        <div class='queue-number'>{queueNumber}</div>
                    </div>
                </page>
            </body>
            </html>";

            // Panggil JavaScript Interop untuk memicu pencetakan
            await JsRuntime.InvokeVoidAsync("printJS", contentToPrint);
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

        private bool IsLoading { get; set; } = false;

        private async Task OnSave()
        {
            try
            {
                var edit = FormKios;
                showQueue = true;
                IsLoading = true;

                FormGeneral.Method = null;
                FormGeneral.InsurancePolicyId = null;

                // Save BPJS Insurance Policy
                var bpjs = await Mediator.Send(new GetBPJSIntegrationQuery(x => FormKios != null && FormKios.NumberType != null && x.NoKartu != null && x.NoKartu.ToLower().Trim().Equals(FormKios.NumberType.ToLower().Trim())));
                if (bpjs is not null && bpjs.Count > 0)
                {
                    FormGeneral.Method = "BPJS";
                    FormGeneral.InsurancePolicyId = bpjs[0].InsurancePolicyId;
                }

                if (FormKios.Id == 0)
                {
                    // Mendapatkan ID dari hasil CreateKioskRequest
                    var checkId = await Mediator.Send(new CreateKioskRequest(FormKios));

                    if (FormKios.ClassTypeId != null)
                    {
                        var TodayQueu = KioskQueues.Where(x => x.ServiceId == checkId.ServiceId && x.ClassTypeId == FormKios.ClassTypeId && x.CreatedDate.Value.Date == DateTime.Now.Date).ToList();
                        if (TodayQueu.Count == 0)
                        {
                            FormQueue.QueueNumber = 1;
                        }
                        else
                        {
                            var GetNoQueue = TodayQueu.OrderByDescending(x => x.QueueNumber).FirstOrDefault() ?? new();
                            if (GetNoQueue.QueueNumber < 9)
                            {
                                FormQueue.QueueNumber = 1 * (long)GetNoQueue.QueueNumber + 2;
                            }
                            else
                            {
                                IsLoading = false;
                                ToastService.ShowError("Full!!");
                                return;
                            }
                        }
                    }
                    else
                    {
                        //Mendapatkan data berdasarkan Counter, service dan tanggal hari ini
                        var TodayQueu = KioskQueues.Where(x => x.ServiceId == checkId.ServiceId && x.CreatedDate.GetValueOrDefault().Date == DateTime.Now.Date).ToList();

                        if (TodayQueu.Count == 0)
                        {
                            FormQueue.QueueNumber = 2;
                        }
                        else
                        {
                            var GetNoQueue = TodayQueu.OrderByDescending(x => x.QueueNumber).FirstOrDefault() ?? new();
                            if (GetNoQueue.QueueNumber < 10)
                            {
                                FormQueue.QueueNumber = 1 * ((long)GetNoQueue.QueueNumber + 2);
                            }
                            else
                            {
                                FormQueue.QueueNumber = 1 * ((long)GetNoQueue.QueueNumber.GetValueOrDefault() + 1);
                            }
                        }
                    }

                    // mendapatkan service counter Id
                    var skId = Services.Where(x => x.Id == checkId.ServiceId).Select(x => x.ServicedId).FirstOrDefault();
                    // Mengisi informasi antrian
                    FormQueue.KioskId = checkId.Id;
                    FormQueue.ServiceId = checkId.ServiceId;
                    FormQueue.ServiceKId = skId;
                    FormQueue.ClassTypeId = FormKios.ClassTypeId;
                    FormQueue.QueueStatus = "waiting";

                    // Membuat KioskQueue baru
                    var QueueKioskId = await Mediator.Send(new CreateKioskQueueRequest(FormQueue));

                    FormGeneral.PatientId = FormKios.PatientId;
                    FormGeneral.ServiceId = FormKios.ServiceId;
                    FormGeneral.KioskQueueId = QueueKioskId.Id;
                    FormGeneral.ClassTypeId = FormKios.ClassTypeId;
                    FormGeneral.RegistrationDate = DateTime.Now;
                    if (checkId.PhysicianId != null)
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
                ToastService.ShowSuccess("Number Queue is Generated Succces!!");
                await LoadData();
            }
            catch { }
            IsLoading = false;
        }

        #endregion Methode Save And Update
    }
}
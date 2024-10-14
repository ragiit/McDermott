using DocumentFormat.OpenXml.Office2010.Drawing.ChartDrawing;
using McDermott.Application.Dtos.Queue;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static McDermott.Application.Features.Commands.Queue.KioskConfigCommand;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class KioskPage
    {
        public class AntreanRequestBPJS
        {
            [JsonProperty("nomorkartu")]
            public string Nomorkartu { get; set; }

            [JsonProperty("nik")]
            public string Nik { get; set; }

            [JsonProperty("nohp")]
            public string Nohp { get; set; }

            [JsonProperty("kodepoli")]
            public string Kodepoli { get; set; }

            [JsonProperty("namapoli")]
            public string Namapoli { get; set; }

            [JsonProperty("norm")]
            public string Norm { get; set; }

            [JsonProperty("tanggalperiksa")]
            public string Tanggalperiksa { get; set; }

            [JsonProperty("kodedokter")]
            public string Kodedokter { get; set; }

            [JsonProperty("namadokter")]
            public string Namadokter { get; set; }

            [JsonProperty("jampraktek")]
            public string Jampraktek { get; set; }

            [JsonProperty("nomorantrean")]
            public string Nomorantrean { get; set; }

            [JsonProperty("angkaantrean")]
            public int Angkaantrean { get; set; }

            [JsonProperty("keterangan")]
            public string Keterangan { get; set; }
        }

        public class UpdateStatusPanggilAntreanRequestPCare
        {
            [JsonProperty("tanggalperiksa")]
            public string Tanggalperiksa { get; set; }

            [JsonProperty("kodepoli")]
            public string Kodepoli { get; set; }

            [JsonProperty("nomorkartu")]
            public string Nomorkartu { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("waktu")]
            public long Waktu { get; set; }
        }

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
        private DoctorScheduleSlotDto? SelectedScheduleSlot { get; set; }
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

        //private long ServiceId
        //{
        //    get => _ServiceId;
        //    set
        //    {
        //        _ServiceId = value;
        //        LoadPhysicians(value);
        //        FormKios.ServiceId = value;
        //        showPhysician = true;
        //    }
        //}

        private long ServiceId { get; set; }

        private async Task SelectedItemServiceChanged(ServiceDto e)
        {
            if (e is null)
                return;

            ServiceId = e.Id;

            FormKios.ServiceId = e.Id;

            await LoadPhysicians(e.Id);

            var telemedicineIds = Services.Where(x => x.IsTelemedicine && x.IsKiosk).Select(x => x.Id).ToList();

            IsTelemedicine = telemedicineIds.Any(x => x == e.Id);

            showPhysician = true;
        }

        private async Task UserSelected(UserDto user)
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

        private List<DoctorScheduleSlotDto> DoctorScheduleSlots = [];

        private async Task ChangePhysician(UserDto r)
        {
            FormKios.PhysicianId = null;
            if (r is null)
                return;

            FormKios.PhysicianId = r.Id;

            return;
            await SelectScheduleSlots();
        }

        private async Task SelectScheduleSlots()
        {
            var schedules = await Mediator.Send(new GetDoctorScheduleQuery(x => x.ServiceId == FormKios.ServiceId && x.PhysicionIds != null && x.PhysicionIds.Contains(FormKios.PhysicianId.GetValueOrDefault())));
            if (schedules.Count > 0)
            {
                DoctorScheduleSlots = (await Mediator.Send(new GetDoctorScheduleSlotQuery(x => x.DoctorScheduleId == schedules[0].Id && x.PhysicianId == FormKios.PhysicianId))).Item1;
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
                var user = await UserInfoService.GetUserInfo(ToastService);
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

        private bool IsTelemedicine = false;

        private async Task LoadData()
        {
            PanelVisible = true;
            StateHasChanged();
            showForm = false;
            FormKios = new();
            Physician = await Mediator.Send(new GetUserQuery());
            KioskQueues = await Mediator.Send(new GetKioskQueueQuery());
            ViewQueue = KioskQueues.OrderByDescending(x => x.CreatedDate).FirstOrDefault() ?? new();
            //InsurancePolices = await Mediator.Send(new GetInsurancePolicyQuery());
            SelectedDataItems = new ObservableRangeCollection<object>();
            Kiosks = await Mediator.Send(new GetKioskQuery());
            //Services = await Mediator.Send(new GetServiceQuery());
            var kconfig = await Mediator.Send(new GetKioskConfigQuery());
            KioskConf = kconfig.Where(x => x.Id == Id).ToList();
            classTypes = await Mediator.Send(new GetClassTypeQuery());
            var result2 = await Mediator.Send(new GetGroupQuery(pageIndex: 0, pageSize: short.MaxValue));
            groups = result2.Item1;

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
                    if (BPJSIntegration is not null)
                    {
                        // Cancel Antrean
                        var isSuccess = await SendPCareRequestBatalAntrean();
                        if (!isSuccess)
                        {
                            showQueue = true;
                            return;
                        }
                    }

                    var cekId = (await Mediator.Send(new GetGeneralConsultanServiceQuery())).Item1; //get Data General Consultation
                    var GId = cekId.Where(x => x.KioskQueueId == id).Select(x => x.Id).FirstOrDefault(); //Get Id in General Consultation where KioskQueueId = id
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(GId)); //Delete Data in General Consultation where id

                    await Mediator.Send(new DeleteKioskQueueRequest(id)); // delete data kioskQueue in id
                }
                showQueue = false;
                BPJSIntegration = new();
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

        private async Task<bool> OnClickGetBPJS(string number)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(number))
                {
                    ToastService.ShowInfo("Please insert the Policy Number!");
                    return false;
                }

                var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"peserta/{number}", HttpMethod.Get);
                if (result.Item2 != 200 || result.Item1 == null)
                {
                    return false;
                }

                var response = System.Text.Json.JsonSerializer.Deserialize<ResponseAPIBPJSIntegrationGetPeserta>(result.Item1) as ResponseAPIBPJSIntegrationGetPeserta;

                if (response is null)
                    return false;

                return response.Aktif;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
                return false;
            }
        }

        private async Task OnSearch()
        {
            var result2 = await Mediator.Send(new GetGroupQuery(pageIndex: 0, pageSize: short.MaxValue));
            var group = result2.Item1;
            var NameGroup = group.FirstOrDefault(x => x.Id == UserAccessCRUID.GroupId) ?? new();
            var InputSearch = FormKios.NumberType ?? string.Empty;
            Patients = await Mediator.Send(new GetDataUserForKioskQuery(InputSearch));

            if (Patients == null)
            {
                showForm = false;
                return;
            }

            showForm = true;

            NamePatient = Patients.Select(x => x.Name).FirstOrDefault();
            FormKios.PatientId = Patients.Select(x => x.Id).FirstOrDefault();
            //BPJS = InsurancePolices.FirstOrDefault(x => x.UserId == FormKios.PatientId);
            BPJS = (await Mediator.Send(new GetSingleInsurancePolicyQuery
            {
                Predicate = x => x.UserId == FormKios.PatientId
            }));
            if (BPJS != null)
            {
                var isActive = await OnClickGetBPJS(BPJS.PolicyNumber);
                if (!isActive)
                {
                    FormKios.BPJS = BPJS.PolicyNumber;
                    FormKios.StageBpjs = false;
                    statBPJS = "InActive";
                }
                else
                {
                    FormKios.BPJS = BPJS.PolicyNumber;
                    FormKios.StageBpjs = true;
                    statBPJS = "Active";

                    //var bpjs = (await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == BPJS.Id))).FirstOrDefault();
                    //if (bpjs != null)
                    //{
                    //    FormKios.BPJS = bpjs.NoKartu;
                    //    FormKios.StageBpjs = bpjs.Aktif;
                    //    statBPJS = bpjs.Aktif ? "Active" : "InActive";
                    //}
                    //else
                    //{
                    //    statBPJS = "no BPJS number";
                    //}
                }
            }
            else
            {
                statBPJS = "no BPJS number";
            }

            if (new[] { "Nurse", "Perawat", "Nursing" }.Contains(NameGroup.Name))
            {
                showClass = true;
            }

            foreach (var kiosk in KioskConf)
            {
                var serviceIds = kiosk.ServiceIds ?? [];
                CountServiceId = KioskConf.Where(x => x.ServiceIds != null).SelectMany(k => k.ServiceIds).Count();

                if (CountServiceId > 1)
                {
                    serv.AddRange(Services.Where(service => serviceIds.Contains(service.Id)));
                }
                else
                {
                    FormKios.ServiceId = Services.FirstOrDefault(service => serviceIds.Contains(service.Id))?.Id;
                    if (FormKios.ServiceId.HasValue)
                    {
                        await LoadPhysicians(FormKios.ServiceId.Value);
                    }

                    showPhysician = true;
                }
            }

            if (FormKios.ServiceId is not null)
            {
                var telemedicineIds = Services.Where(x => x.IsTelemedicine && x.IsKiosk).Select(x => x.Id).ToList();
                IsTelemedicine = telemedicineIds.Any(x => x == FormKios.ServiceId);
                await LoadPhysicians(FormKios.ServiceId.Value);
                showPhysician = true;
            }

            //var telemedicineIds = Services.Where(x => x.IsTelemedicine && x.IsKiosk).Select(x => x.Id).ToList();
            //if (KioskConf.Any() && KioskConf.FirstOrDefault()?.ServiceIds is not null)
            //    IsTelemedicine = telemedicineIds.Any(x => KioskConf.FirstOrDefault().ServiceIds.Contains(x));

            await SelectScheduleSlots();
        }

        //private async Task OnSearch()
        //{
        //    var group = await Mediator.Send(new GetGroupQuery());
        //    var NameGroup = group.FirstOrDefault(x => x.Id == UserAccessCRUID.GroupId) ?? new();
        //    var types = FormKios.Type;
        //    var InputSearch = FormKios.NumberType ?? string.Empty;
        //    Patients = await Mediator.Send(new GetDataUserForKioskQuery(InputSearch));

        //    if (Patients != null)
        //    {
        //        showForm = true;
        //        NamePatient = Patients.Select(x => x.Name).FirstOrDefault();
        //        FormKios.PatientId = Patients.Select(x => x.Id).FirstOrDefault();
        //        BPJS = InsurancePolices.Where(x => x.UserId == FormKios.PatientId).FirstOrDefault()!;
        //        if (BPJS is not null)
        //        {
        //            var isActive = await OnClickGetBPJS(BPJS.PolicyNumber);
        //            if (!isActive)
        //            {
        //                FormKios.BPJS = BPJS.PolicyNumber;
        //                FormKios.StageBpjs = false;
        //                statBPJS = "InActive";
        //            }
        //            else
        //            {
        //                var bpjs = (await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == BPJS.Id))).FirstOrDefault();
        //                if (bpjs is not null)
        //                {
        //                    if (bpjs.Aktif)
        //                    {
        //                        FormKios.BPJS = bpjs.NoKartu;
        //                        FormKios.StageBpjs = true;
        //                        statBPJS = "Active";
        //                    }
        //                    else
        //                    {
        //                        FormKios.StageBpjs = false;
        //                        statBPJS = "InActive";
        //                    }
        //                }
        //                else
        //                {
        //                    statBPJS = "no BPJS number";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            statBPJS = "no BPJS number";
        //        }
        //        if (NameGroup.Name == "Nurse" || NameGroup.Name == "Perawat" || NameGroup.Name == "Nursing")
        //        {
        //            showClass = true;
        //        }

        //        foreach (var kiosk in KioskConf)
        //        {
        //            var serviceIds = kiosk.ServiceIds ?? [];
        //            CountServiceId = KioskConf.SelectMany(k => k.ServiceIds).Count();

        //            if (CountServiceId > 1)
        //            {
        //                serv.AddRange(Services.Where(service => serviceIds.Contains(service.Id)));
        //            }
        //            else
        //            {
        //                FormKios.ServiceId = Services.FirstOrDefault(service => serviceIds.Contains(service.Id))?.Id;
        //                var serviceId = FormKios.ServiceId;
        //                if (serviceId.HasValue)
        //                {
        //                    await LoadPhysicians(serviceId.Value);
        //                }

        //                showPhysician = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        showForm = false;
        //    }

        //    await SelectScheduleSlots();
        //}

        private async Task onPrint()
        {
            var noBPJS = "";
            var nameBPJS = "";
            var aas = ViewQueue.QueueNumber.ToString() ?? string.Empty;
            var NamePatient = ViewQueue?.Kiosk?.Patient?.Name;
            var ServiceKiosk = ViewQueue?.Service?.Name;
            var TypeNumber = Patients.Select(x => x.TypeNumber).FirstOrDefault();
            var numbers = Patients.Select(x => x.Numbers).FirstOrDefault();
            if (ViewQueue?.Kiosk?.StageBpjs == true)
            {
                nameBPJS = "BPJS Kesehatan";
                noBPJS = BPJS.PolicyNumber;
            }
            else
            {
                nameBPJS = "-";
                noBPJS = "-";
            }
            string queueNumber = aas.PadLeft(3, '0');

            // HTML untuk dokumen cetak
            string contentToPrint = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Cetak Antrian</title>
                <link re='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css'>
                <style>
                   @page {{size: 80mm 80mm;
    margin: 0;
}}

.ticket {{width: 80mm;
    height: 10px;
    padding: 0;
    box-sizing: border-box;
    font-family: Arial, sans-serif;
}}

.header {{text-align: center;
}}

.logo {{width: 50px;
    height: auto;
}}

.ticket-number {{text-align: center;
    font-size: 40pt;
    margin: 10px 0;
}}

.details p {{margin: 1px 0;
    font-size: 9pt;
}}

.details p strong {{font-weight: bold;
}}
                </style>
            </head>
            <body>
                <div class='ticket'>
    <div class='header'>
        <img src='/image/logo.png' alt='Logo' class='logo' />
        <h1>Klinik McDermott</h1>
    </div>
    <h2 class='ticket-number'>{queueNumber}</h2>
    <div class='details'>
        <p><strong>Tanggal Daftar:</strong> {DateTime.Now}</p>
        <p><strong>Nama:</strong> {NamePatient}</p>
        <p><strong>Poliklinik:</strong> {ServiceKiosk}</p>
        <p><strong>Jaminan:</strong> {nameBPJS}</p>
        <p><strong>Nomor Jaminan:</strong> {noBPJS}</p>
        <p><strong>{TypeNumber}:</strong> {numbers}</p>
    </div>
</div>
<script src='https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js' integrity='sha384-kenU1KFdBIe4zVF0s0G1M5b4hcpxyD9F7jL+jjXkk+Q2h455rYXK/7HAuoJl+0I4' crossorigin='anonymous'></script>
            </body>
            </html>";

            // Panggil JavaScript Interop untuk memicu pencetakan
            await JsRuntime.InvokeVoidAsync("printJS", contentToPrint);
        }

        private void PrintTicket()
        {
            NavigationManager.NavigateTo("/print-ticket", true);
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

        [Inject]
        public GoogleMeetService GoogleMeetService { get; set; }

        public InsurancePolicyDto BPJSIntegration { get; set; } = new();

        private async Task OnSave()
        {
            try
            {
                var edit = FormKios;
                IsLoading = true;

                FormGeneral.Method = null;
                FormGeneral.InsurancePolicyId = null;

                // Save BPJS Insurance Policy
                //var bpjs = await Mediator.Send(new GetBPJSIntegrationQuery(x => FormKios != null && FormKios.BPJS != null && x.NoKartu != null && x.NoKartu.ToLower().Trim().Equals(FormKios.BPJS.ToLower().Trim())));
                var bpjs = await Mediator.Send(new GetSingleInsurancePolicyQuery
                {
                    Predicate = x => FormKios != null && FormKios.BPJS != null && x.NoKartu != null && x.NoKartu.ToLower().Trim().Equals(FormKios.BPJS.ToLower().Trim())
                });
                if (bpjs is not null)
                {
                    FormGeneral.Method = "BPJS";
                    BPJSIntegration = bpjs;
                    FormGeneral.InsurancePolicyId = bpjs.Id;
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

                    if (bpjs is not null && !string.IsNullOrWhiteSpace(FormKios.BPJS))
                    {
                        var isSuccess = await SendPCareRequestAntrean(bpjs ?? new());
                        if (!isSuccess)
                        {
                            BPJSIntegration = new();
                            showQueue = false;
                            IsLoading = false;
                        }
                    }

                    // Membuat KioskQueue baru
                    var QueueKioskId = await Mediator.Send(new CreateKioskQueueRequest(FormQueue));

                    FormGeneral.PatientId = FormKios.PatientId;
                    FormGeneral.ServiceId = FormKios.ServiceId;
                    FormGeneral.KioskQueueId = QueueKioskId.Id;
                    FormGeneral.RegistrationDate = DateTime.Now;
                    if (checkId.PhysicianId != null)
                    {
                        FormGeneral.PratitionerId = FormKios.PhysicianId;
                    }

                    if (IsTelemedicine)
                    {
                        FormGeneral.RegistrationDate = FormKios.DateForTelemedicine.GetValueOrDefault();
                        FormGeneral.PratitionerId = FormKios.PhysicianId;
                        FormGeneral.ServiceId = FormKios.ServiceId;
                        FormGeneral.Status = EnumStatusGeneralConsultantService.Planned;
                        try
                        {
                            FormGeneral.LinkMeet = await GoogleMeetService.CreateMeetingAsync(null);
                        }
                        catch (Exception ex)
                        {
                            ToastService.ShowError(ex.Message);
                        }
                        FormGeneral.TypeRegistration = "Telemedicine";
                    }

                    await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormGeneral));
                }
                else
                {
                    await Mediator.Send(new UpdateKioskRequest(FormKios));
                }

                showQueue = true;
                FormKios = new();
                ToastService.ShowSuccess("Number Queue is Generated Succces!!");
                await LoadData();
            }
            catch { }
            IsLoading = false;
        }

        public class BatalAntreanBPJS
        {
            [JsonProperty("tanggalperiksa")]
            public string Tanggalperiksa { get; set; }

            [JsonProperty("kodepoli")]
            public string Kodepoli { get; set; }

            [JsonProperty("nomorkartu")]
            public string Nomorkartu { get; set; }

            [JsonProperty("alasan")]
            public string Alasan { get; set; }
        }

        private async Task<bool> SendPCareRequestBatalAntrean()
        {
            try
            {
                var service = Services.FirstOrDefault(x => x.Id == FormKios.ServiceId);
                var physician = Physician.FirstOrDefault(x => x.Id == FormKios.PhysicianId);

                var antreanRequest = new BatalAntreanBPJS
                {
                    Tanggalperiksa = DateTime.Now.ToString("yyyy-MM-dd"),
                    Kodepoli = service!.Code ?? string.Empty,
                    Nomorkartu = BPJSIntegration.NoKartu ?? string.Empty,
                    Alasan = string.Empty
                };

                Console.WriteLine("Sending antrean/batal...");
                var responseApi = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"antrean/batal", HttpMethod.Post, antreanRequest);

                if (responseApi.Item2 != 200)
                {
                    ToastService.ShowError($"{responseApi.Item1}, Code: {responseApi.Item2}");
                    Console.WriteLine(JsonConvert.SerializeObject(antreanRequest, Formatting.Indented));
                    Console.WriteLine("ResponseAPI Antrean/Batal " + Convert.ToString(responseApi.Item1));
                    IsLoading = false;
                    return false;
                }
                else
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(responseApi.Item1);
                    Console.WriteLine(Convert.ToString(data));
                }

                return true;
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
                return false;
            }
        }

        private async Task<bool> SendPCareRequestAntrean(InsurancePolicyDto bpjs)
        {
            try
            {
                var service = Services.FirstOrDefault(x => x.Id == FormKios.ServiceId);
                var physician = Physician.FirstOrDefault(x => x.Id == FormKios.PhysicianId);

                //if (physician is null)
                //{
                //    ToastService.ShowInfo("Please select the Physician!");
                //    return false;
                //}

                if (service is null)
                {
                    ToastService.ShowInfo("Please select the Service!");
                    return false;
                }

                var antreanRequest = new AntreanRequestBPJS
                {
                    Nomorkartu = bpjs.NoKartu ?? string.Empty,
                    Nik = bpjs.NoKTP ?? string.Empty,
                    Nohp = bpjs.NoHP ?? string.Empty,
                    Kodepoli = service?.Code ?? string.Empty,
                    Namapoli = service?.Name ?? string.Empty,
                    Norm = Patients.FirstOrDefault(x => x.Id == FormKios.PatientId)!.NoRm ?? string.Empty,
                    Tanggalperiksa = DateTime.Now.ToString("yyyy-MM-dd"),
                    Kodedokter = physician?.PhysicanCode ?? null,
                    Namadokter = physician.Name ?? null,
                    Jampraktek = SelectedScheduleSlot?.ResultWorkFormatStringKiosk ?? "00:00:00",
                    Nomorantrean = ViewQueue!.QueueNumber!.ToString()! ?? "",
                    Angkaantrean = ViewQueue.QueueNumber.ToInt32(),
                    Keterangan = ""
                };

                Console.WriteLine("Sending antrean/add...");
                var responseApi = await PcareService.SendPCareService(nameof(SystemParameter.AntreanFKTPBaseURL), $"antrean/add", HttpMethod.Post, antreanRequest);

                if (responseApi.Item2 != 200)
                {
                    ToastService.ShowError($"{responseApi.Item1}, Code: {responseApi.Item2}");
                    Console.WriteLine(JsonConvert.SerializeObject(antreanRequest, Formatting.Indented));
                    Console.WriteLine("ResponseAPI Antrean/Add: " + Convert.ToString(responseApi.Item1));
                    IsLoading = false;
                    return false;
                }
                else
                {
                    if (responseApi.Item1 is not null)
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(responseApi.Item1);
                        Console.WriteLine(Convert.ToString(data));
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
                return false;
            }
        }

        #endregion Methode Save And Update
    }
}
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.JSInterop;
using System.Globalization;
using System.Reflection;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;
using static McDermott.Application.Features.Commands.Medical.DiagnosisCommand;
using static McDermott.Application.Features.Commands.Medical.DiseaseCategoryCommand;
using static McDermott.Application.Features.Commands.Medical.NursingDiagnosesCommand;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanCPPTCommand;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanMedicalSupportCommand;
using static McDermott.Application.Features.Queries.Transaction.GeneralConsultanServiceQueryHandler;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class GeneralConsultanServicePage
    {
        #region Relation Data

        private GeneralConsultantClinicalAssesmentDto GeneralConsultantClinical = new();
        private List<GeneralConsultanServiceDto> GeneralConsultanServices = new();
        private List<UserDto> IsPatient = new();
        private List<UserDto> patients = new List<UserDto>();
        private int PatientIds = new();
        private IEnumerable<GeneralConsultanServiceDto> SelectPatient = [];

        private List<UserDto> IsPratition = new();
        private List<UserDto> AllDoctors = new();
        private List<InsuranceDto> Insurances = [];
        private List<InsuranceDto> AllInsurances = [];
        private List<InsurancePolicyDto> InsurancePolicies = [];
        private List<ServiceDto> Services = [];
        private List<PatientAllergyDto> PatientAllergies = [];

        #region Form Regis

        public MarkupString GetIssuePriorityIconHtml(bool priority)
        {
            string priorytyClass = "warning";
            string title = "Medium";
            if (priority == true)
            {
                priorytyClass = "danger";
                title = " Priority ";

                string html = string.Format("<span class='badge bg-{0} py-1 px-2' title='{1} Priority'>{1}</span>", priorytyClass, title);
                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        private GeneralConsultanServiceDto FormRegis = new();

        #endregion Form Regis

        #region UserInfo

        private User UserLogin = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadUser();
                StateHasChanged();

                try
                {
                    var result = await NavigationManager.CheckAccessUser(oLocal);
                    IsAccess = result.Item1;
                    UserAccessCRUID = result.Item2;
                }
                catch { }
            }
        }

        private async Task LoadUser()
        {
            try
            {
                UserLogin = await oLocal.GetUserInfo();
            }
            catch { }
        }

        #endregion UserInfo

        private IEnumerable<DoctorScheduleDto> SelectedSchedules = [];
        private IEnumerable<string> SelectedNames { get; set; } = new List<string>();
        private List<string> Names { get; set; } = new();

        #endregion Relation Data

        #region Data Statis

        private string PaymentMethod
        {
            get => _PaymentMethod;
            set
            {
                FormRegis.Payment = value;
                _PaymentMethod = value;

                Insurances.Clear();

                if (PaymentMethod.Equals("BPJS"))
                {
                    var all = InsurancePolicies.Where(x => x.UserId == PatientsId && x.Insurance.IsBPJS == true).ToList();
                    Temps = all.Select(x => new InsuranceTemp
                    {
                        InsurancePolicyId = x.Id,
                        InsuranceId = x.InsuranceId,
                        InsuranceName = x.Insurance.Name,
                        PolicyNumber = x.PolicyNumber
                    }).ToList();
                }
                else
                {
                    var all = InsurancePolicies.Where(x => x.UserId == PatientsId && x.Insurance.IsBPJS != true).ToList();
                    Temps = all.Select(x => new InsuranceTemp
                    {
                        InsurancePolicyId = x.Id,
                        InsuranceId = x.InsuranceId,
                        InsuranceName = x.Insurance.Name,
                        PolicyNumber = x.PolicyNumber
                    }).ToList();
                }

                //GetInsurancePhysician(Value);
            }
        }

        private int Value
        {
            get => PatientsId;
            set
            {
                int PatientsId = value;
                this.PatientsId = value;

                var item = patients.FirstOrDefault(x => x.Id == PatientsId);

                if (item.DateOfBirth != null)
                {
                    DateTime currentDate = DateTime.UtcNow;
                    Birthdate = item.DateOfBirth;
                    Age = currentDate.Year - Birthdate!.Value.Year;
                }

                try
                {
                    FormRegis.Age = Age;
                    FormRegis.NoRM = item.NoRm;
                    FormRegis.IdentityNumber = item.NoId.ToString();
                    FormRegis.PatientId = item.Id;
                }
                catch { }

                var patientAlergy = PatientAllergies.FirstOrDefault(x => x.UserId == item.Id);
                if (patientAlergy is not null)
                {
                    FormRegis.IsWeather = !string.IsNullOrWhiteSpace(patientAlergy.Weather);
                    FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(patientAlergy.Farmacology);
                    FormRegis.IsFood = !string.IsNullOrWhiteSpace(patientAlergy.Food);
                }
                else
                {
                    FormRegis.IsWeather = false;
                    FormRegis.IsPharmacology = false;
                    FormRegis.IsFood = false;
                }
            }
        }

        private string Code { get; set; } = "";
        private int _NursingDiagnosis { get; set; }

        private int NursingDiagnosis
        {
            get => _NursingDiagnosis;
            set
            {
                FormInputCPPTGeneralConsultan.NursingDiagnosisId = value;
                _NursingDiagnosis = value;

                FormInputCPPTGeneralConsultan.Code = NursingDiagnoses.FirstOrDefault(x => x.Id == FormInputCPPTGeneralConsultan.NursingDiagnosisId)!.Code!;
            }
        }

        public class NurseStation
        {
            public int Id { get; set; }
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public IEnumerable<NurseStation> NurseStations { get; set; } = new List<NurseStation>
        {
            new() { Id = 1, Status = "Planned", Count = 10 },
            new() { Id = 2, Status = "Confirmed", Count = 5 },
            new() { Id = 3, Status = "Waiting", Count = 2 },
            new() { Id = 4, Status = "Physician", Count = 1 },
            new() { Id = 5, Status = "Finished", Count = 0 },
        };

        private List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        private List<string> RegisType = new List<string>
        {
            "General Consultation",
            "Emergency",
            "MCU"
        };

        private List<string> Method = new List<string>
        {
            "MCU",
            "Gas And Oil"
        };

        private int PatientsId = 0;

        private int PractitionerId = 0;

        private int Age = 0;
        private DateTime? Birthdate { get; set; }
        private string IdentityNum { get; set; }
        private string _PaymentMethod { get; set; }

        private string MedicalTypee { get; set; }
        private List<InsuranceTemp> Temps = [];

        private class InsuranceTemp
        {
            public int InsurancePolicyId { get; set; }
            public int InsuranceId { get; set; }
            public string InsuranceName { get; set; }
            public string PolicyNumber { get; set; }

            public string ConcatInsurancePolicy
            { get { return PolicyNumber + " - " + InsuranceName; } }
        }

        private List<string> Stagings = new List<string>
        {
            "Planned",
            "Confirmed",
            "Nurse Station",
            "Waiting",
            "Physician",
            "Finished"
        }; private int _InsurancePolicyId { get; set; }

        private int InsuranceId { get; set; }

        private int InsurancePolicyId
        {
            get => _InsurancePolicyId;
            set
            {
                _InsurancePolicyId = value;
            }
        }

        private int _ServiceId { get; set; }

        private int _DoctorId { get; set; }

        private int DoctorId
        {
            get => _DoctorId;
            set
            {
                FormRegis.PratitionerId = value;
                _DoctorId = value;
                SetTimeSchedule(value, RegistrationDate);
            }
        }

        private DateTime _RegistrationDate { get; set; } = DateTime.Now;

        private DateTime RegistrationDate
        {
            get => _RegistrationDate;
            set
            {
                FormRegis.RegistrationDate = value;
                _RegistrationDate = value;
                SetTimeSchedule(DoctorId, value);
            }
        }

        private List<string> Times = [];

        #endregion Data Statis

        #region Grid Setting

        private int _DiseasesId { get; set; }

        private int DiseasesId
        {
            get => _DiseasesId;
            set
            {
                _DiseasesId = value;
                FormInputCPPTGeneralConsultan.DiseasesId = value;
                var parent = Diagnoses.FirstOrDefault(z => z.Id == value).Name;
                DiseaseCategories = AllDiseaseCategories.Where(x => x.ParentCategory == parent).ToList();

                //var schedules = await Mediator.Send(new GetDoctorScheduleQuery());
            }
        }

        private int ServiceId
        {
            get => _ServiceId;
            set
            {
                _ServiceId = value;
                FormRegis.ServiceId = value;
                IsPratition = AllDoctors.Where(x => x.DoctorServiceIds.Contains(value)).ToList();

                //var schedules = await Mediator.Send(new GetDoctorScheduleQuery());
            }
        }

        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool showForm { get; set; } = false;
        private string textPopUp = "";
        private string Timeee = "";
        private string DisplayFormat { get; } = string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? "HH:mm" : "h:mm tt";
        public IGrid Grid { get; set; }
        private int ActiveTabIndex { get; set; } = 0;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataItems2 { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private GroupMenuDto UserAccessCRUID = new();

        private List<Temppp> Temppps { get; set; } = new List<Temppp>
        {
            new Temppp
            {
                Title = "Test",
                Body = "Test Body"
            }
        };

        private class Temppp
        {
            public string Title { get; set; }
            public string Body { get; set; }
        }

        #endregion Grid Setting

        #region File Upload Attachment

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

        //private void RemoveSelectedFile()
        //{
        //    UserForm.SipFile = null;
        //}

        //private async void SelectFiles(InputFileChangeEventArgs e)
        //{
        //    int maxFileSize = 1 * 1024 * 1024;
        //    var allowedExtenstions = new string[] { ".png", ".jpg", ".jpeg", ".gif" };

        //    UserForm.SipFile = e.File.Name;

        //    await FileUploadService.UploadFileAsync(e.File, maxFileSize, []);
        //}

        #endregion File Upload Attachment

        #region Tab CPPT

        private IGrid GridTabCPPT { get; set; }
        private int FocusedGridTabCPPTRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItemsCPPT { get; set; } = new ObservableRangeCollection<object>();

        private InputCPPTGeneralConsultanCPPT FormInputCPPTGeneralConsultan = new();
        private GeneralConsultanCPPTDto GeneralConsultanCPPT = new();

        private List<NursingDiagnosesDto> NursingDiagnoses = [];
        private List<DiagnosisDto> Diagnoses = [];
        private List<DiseaseCategoryDto> DiseaseCategories = [];
        private List<DiseaseCategoryDto> AllDiseaseCategories = [];
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs = [];

        private class InputCPPTGeneralConsultanCPPT
        {
            public string? Subjective { get; set; }
            public string? Objective { get; set; }
            public int NursingDiagnosisId { get; set; }
            public string? Code { get; set; }
            public int? DiseasesId { get; set; }
            public int? DiseasesCategoryId { get; set; }
            public string? Plan { get; set; }
            public DateTime Date { get; set; } = DateTime.Now;
        }

        private async Task OnSaveTabCPPTConfirm()
        {
        }

        private void OnDeleteTabCPPTConfirm(GridDataItemDeletingEventArgs e)
        {
            GeneralConsultanCPPTs.Remove((GeneralConsultanCPPTDto)e.DataItem);
            GridTabCPPT.Reload();
        }

        private void OnClickConfirmCPPT()
        {
            var temps = new List<GeneralConsultanCPPTDto>();
            temps.Add(new GeneralConsultanCPPTDto
            {
                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                Title = UserLogin.Name,
            });
            temps.Add(new GeneralConsultanCPPTDto
            {
                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                Title = "Date and Time",
                Body = DateTime.Now.ToString("dd-MMM-yyy HH:mm:tt")
            });

            foreach (var key in GetPropertyNames(new InputCPPTGeneralConsultanCPPT()))
            {
                if (key == "Date")
                    continue;

                PropertyInfo property = typeof(InputCPPTGeneralConsultanCPPT).GetProperty(key);
                object? value = property?.GetValue(FormInputCPPTGeneralConsultan);

                if (value != null)
                {
                    temps.Add(new GeneralConsultanCPPTDto
                    {
                        Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                        Title = key,
                        Body = value is null ? "" : value.ToString(), // Ubah ke string sesuai kebutuhan
                    });
                }
            }

            GeneralConsultanCPPTs.AddRange(temps);
            GridTabCPPT.Reload();
            OnClickCancelConfirmCPPT();
        }

        private void GridTabCPPT_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;
        }

        public static List<string> GetPropertyNames<T>(T obj)
        {
            List<string> propertyNames = new List<string>();
            Type type = typeof(T);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                propertyNames.Add(prop.Name);
            }

            return propertyNames;
        }

        private void OnClickCancelConfirmCPPT()
        {
            //NursingDiagnosis = 0;
            FormInputCPPTGeneralConsultan = new InputCPPTGeneralConsultanCPPT();
        }

        #endregion Tab CPPT

        #region Tab Medical Support

        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport = new();
        private List<IBrowserFile> BrowserFiles = [];

        #region FileAttachmentLab

        private void RemoveSelectedFileLab()
        {
            GeneralConsultanMedicalSupport.LabEximinationAttachment = null;
        }

        private async void SelectFilesLab(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanMedicalSupport.LabEximinationAttachment = e.File.Name;

            await FileUploadService.UploadFileAsync(e.File, 1 * 1024 * 1024, []);
        }

        private async Task SelectFileLab()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "labFile");
        }

        private async Task DownloadSIPFileLab()
        {
            var a = await FileUploadService.DownloadFile(GeneralConsultanMedicalSupport.LabEximinationAttachment);

            NavigationManager.NavigateTo(a);

            return;

            //using var s = new DotNetStreamReference(stream: a);

            //await JsRuntime.InvokeVoidAsync("downloadFileFromStream", GeneralConsultanMedicalSupport.LabEximinationAttachment, s);
        }

        #endregion FileAttachmentLab

        #region FileAttachmentRadiology

        private void RemoveSelectedFileRadiology()
        {
            GeneralConsultanMedicalSupport.RadiologyEximinationAttachment = null;
        }

        private async void SelectFilesRadiology(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanMedicalSupport.RadiologyEximinationAttachment = e.File.Name;

            //await FileUploadService.UploadFileAsync(e.File, 1 * 1024 * 1024, []);
        }

        private async Task SelectFileRadiology()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "radiologyFile");
        }

        private async Task DownloadSIPFileRadiology()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "radiologyFile");
        }

        #endregion FileAttachmentRadiology

        #region FileAttachmentAlcohol

        private void RemoveSelectedFileAlcohol()
        {
            GeneralConsultanMedicalSupport.AlcoholEximinationAttachment = null;
        }

        private async void SelectFilesAlcohol(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanMedicalSupport.AlcoholEximinationAttachment = e.File.Name;

            //await FileUploadService.UploadFileAsync(e.File, 1 * 1024 * 1024, []);
        }

        private async Task SelectFileAlcohol()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "alcoholFile");
        }

        private async Task DownloadSIPFileAlcohol()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "alcoholFile");
        }

        #endregion FileAttachmentAlcohol

        #region FileAttachmentDrug

        private void RemoveSelectedFileDrug()
        {
            GeneralConsultanMedicalSupport.DrugEximinationAttachment = null;
        }

        private async void SelectFilesDrug(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanMedicalSupport.DrugEximinationAttachment = e.File.Name;

            ToastService.ShowInfo(BrowserFiles.Count().ToString());

            //await FileUploadService.UploadFileAsync(e.File, 1 * 1024 * 1024, []);
        }

        private async Task SelectFileDrug()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "drugFile");
        }

        private async Task DownloadSIPFileDrug()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "drugFile");
        }

        #endregion FileAttachmentDrug

        #endregion Tab Medical Support

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

            GeneralConsultanServices = await Mediator.Send(new GetGeneralConsultanServiceQuery());
            InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery());
            NursingDiagnoses = await Mediator.Send(new GetNursingDiagnosesQuery());
            Diagnoses = await Mediator.Send(new GetDiagnosisQuery());
            AllDiseaseCategories = await Mediator.Send(new GetDiseaseCategoryQuery());
            PatientAllergies = await Mediator.Send(new GetPatientAllergyQuery());
            await LoadData();
        }

        private void GetInsurancePhysician(int value)
        {
            if (string.IsNullOrWhiteSpace(PaymentMethod))
                return;

            InsurancePolicies.Clear();

            if (PaymentMethod.Equals("BPJS"))
            {
                Insurances = AllInsurances.Where(x => InsurancePolicies.Select(z => z.InsuranceId).Contains(x.Id) && x.IsBPJS == true).ToList();
            }
            else
            {
                Insurances = AllInsurances.Where(x => InsurancePolicies.Select(z => z.InsuranceId).Contains(x.Id) && x.IsBPJS == false).ToList();
            }
        }

        private async Task GetScheduleTimesUser(int value)
        {
            var slots = await Mediator.Send(new GetDoctorScheduleSlotQuery(x => x.PhysicianId == value));
        }

        private async Task SetTimeSchedule(int patientId, DateTime date)
        {
            var slots = await Mediator.Send(new GetDoctorScheduleSlotQuery(x => x.PhysicianId == patientId && x.StartDate.Date == date.Date && x.DoctorSchedule.ServiceId == ServiceId));

            Times.Clear();

            Times.AddRange(slots.Select(x => $"{x.WorkFromFormatString} - {x.WorkToFormatString}"));
        }

        private async Task OnClickConfirm()
        {
            var index = Stagings.FindIndex(x => x == FormRegis.StagingStatus);
            if (FormRegis.StagingStatus != "Finished")
            {
                FormRegis.StagingStatus = Stagings[index + 1];
            }
            await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));
        }

        private void OnCancel2()
        {
            ToastService.ShowInfo("TESTTTTTTTTTTTTTTTTTTTTTTTTTTTTtt");
        }

        private void SelectedService(DoctorScheduleDto docter)
        {
            //var selectedServices = DoctorScheduleDto
            //    .Where(service => PhysicionIds.Any(physicionId =>
            //        physicions.Any(physicion => physicion.Id == physicionId && service.PhysicionIds.Contains(physicionId.Id))))
            //    .ToList();
        }

        private async Task SelectData()
        {
            var user = await Mediator.Send(new GetUserQuery());

            //patient
            patients = [.. user.Where(x => x.IsPatient == true).ToList()];
            PatientIds = IsPatient.Select(x => x.Id).FirstOrDefault();

            //IsDocter
            IsPratition = [.. user.Where(x => x.IsDoctor == true).ToList()];
            AllDoctors = [.. user.Where(x => x.IsDoctor == true).ToList()];

            //Insurance
            AllInsurances = await Mediator.Send(new GetInsuranceQuery());

            //Medical Type
            Services = await Mediator.Send(new GetServiceQuery());
        }

        private bool FormValidationState = true;

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            await OnSave();
        }

        private async Task OnSave()
        {
            try
            {
                if (!FormValidationState)
                    return;

                BrowserFiles.Distinct();

                foreach (var item in BrowserFiles)
                {
                    await FileUploadService.UploadFileAsync(item, 1 * 1024 * 1024, []);
                }

                if (FormRegis.Id == 0)
                {
                    var result = await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormRegis));
                    GeneralConsultantClinical.GeneralConsultanServiceId = result.Id;
                    GeneralConsultanMedicalSupport.GeneralConsultanServiceId = result.Id;
                    GeneralConsultanCPPTs.ForEach(x => { x.GeneralConsultanServiceId = result.Id; x.Id = 0; });
                    await Mediator.Send(new CreateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinical));
                    //  await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                    //  await Mediator.Send(new CreateListGeneralConsultanCPPTRequest(GeneralConsultanCPPTs));
                }
                else
                {
                    await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));
                    GeneralConsultantClinical.GeneralConsultanServiceId = FormRegis.Id;
                    await Mediator.Send(new UpdateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinical));
                    // await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                    await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(ids: GeneralConsultanCPPTs.Select(x => x.Id).ToList()));
                    //   await Mediator.Send(new CreateListGeneralConsultanCPPTRequest(GeneralConsultanCPPTs));
                }

                FormRegis = new GeneralConsultanServiceDto();
                GeneralConsultantClinical = new();
                showForm = false;

                await LoadData();

                ToastService.ShowInfo("Successfully");
            }
            catch (Exception exx)
            {
                ToastService.ShowError(exx.Message);
            }
        }

        private async void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        private async Task LoadData()
        {
            showForm = false;
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            GeneralConsultanServices = await Mediator.Send(new GetGeneralConsultanServiceQuery());
            await SelectData();
            PanelVisible = false;
        }

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
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        private async Task NewItem_Click()
        {
            await SelectData();
            showForm = true;
            textPopUp = "Add Data Registration";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await EditItemVoid();
        }

        private async Task EditItemVoid()
        {
            try
            {
                showForm = true;

                FormRegis = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();
                if (FormRegis.StagingStatus != "Finished")
                {
                    var index = Stagings.FindIndex(x => x == FormRegis.StagingStatus);

                    FormRegis.StagingStatus = Stagings[index + 1];
                }

                Value = FormRegis.PatientId.ToInt32();
                ServiceId = FormRegis.ServiceId.ToInt32();
                DoctorId = FormRegis.PratitionerId.ToInt32();
                PaymentMethod = FormRegis.Payment;

                var clinical = await Mediator.Send(new GetGeneralConsultantClinicalAssesmentQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                var support = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));

                GeneralConsultanMedicalSupport = support[0];
                GeneralConsultantClinical = clinical[0];
            }
            catch (Exception exx)
            {
                ToastService.ShowError(exx.Message);
            }
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            await EditItemVoid();
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void OnCancel()
        {
            FormRegis = new();
            GeneralConsultantClinical = new GeneralConsultantClinicalAssesmentDto();
            showForm = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(((GeneralConsultanServiceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>();
                    await Mediator.Send(new DeleteListGeneralConsultanServiceRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }
    }
}
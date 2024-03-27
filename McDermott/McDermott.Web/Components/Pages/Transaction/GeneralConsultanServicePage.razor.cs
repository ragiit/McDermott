using QuestPDF.Fluent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanClinicalAssesmentCommand;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class GeneralConsultanServicePage
    {
        #region Grid Lab Test

        private IGrid GridLabTest { get; set; }
        private IReadOnlyList<object> SelectedLabTestDataItems { get; set; } = [];
        private List<LabResultDetailDto> LabResultDetails = [];
        private LabResultDetailDto LabResultDetail = new();
        private LabTestDto LabTest = new();

        private List<long> DeletedLabTestIds = [];
        private int FocusedRowLabTestVisibleIndex { get; set; }
        private LabUomDto LabUom = new();
        private bool IsAddOrUpdateOrDeleteLabResult = false;

        private List<string> ResultValueTypes =
          [
              "Low",
                "Normal",
                "High",
                "Positive",
                "Negative",
          ];

        private async Task OnSaveLabTest(GridEditModelSavingEventArgs e)
        {
            IsAddOrUpdateOrDeleteLabResult = true;
            var editModel = LabResultDetail;

            editModel.LabTest = LabTests.FirstOrDefault(l => l.Id == selectedLabTestId);

            if (editModel.Id == 0)
            {
                long newId;
                do
                {
                    newId = Helper.RandomNumber;
                } while (LabResultDetails.Any(pfr => pfr.Id == newId));

                editModel.Id = newId;
                LabResultDetails.Add(editModel);
            }
            else
                LabResultDetails[FocusedRowLabTestVisibleIndex] = editModel;

            LabResultDetail = new();
            LabTest = new();
        }

        private async Task AddNewLabResult()
        {
            LabResultDetail = new();
            LabTest = new();
            LabUom = new();
            await GridLabTest.StartEditNewRowAsync();
        }

        private void GridLabTest_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowLabTestVisibleIndex = args.VisibleIndex;
            if (args.DataItem is not null)
                LabResultDetail = args.DataItem as LabResultDetailDto;
            else
                LabResultDetail = new();
        }

        private async Task OnDeleteLabTest()
        {
            IsAddOrUpdateOrDeleteLabResult = true;
            //var aaa = SelectedLabTestDataItems.Adapt<List<LabResultDetailDto>>();

            LabResultDetails.Remove(LabResultDetails.FirstOrDefault(x => x.Id == SelectedLabTestDataItems[0].Adapt<LabResultDetailDto>().Id));

            SelectedLabTestDataItems = [];
        }

        private long selectedLabTestId { get; set; }

        private void SelectedItemParameter(LabTestDto e)
        {
            if (e is null)
                return;

            selectedLabTestId = e.Id;

            var labTest = e;

            if (FormRegis.Patient is not null && FormRegis.Patient.Gender is not null)
            {
                if (FormRegis.Patient.Gender.Equals("Male"))
                    labTest.NormalRangeByGender = labTest.NormalRangeMale;
                else
                    labTest.NormalRangeByGender = labTest.NormalRangeFemale;
            }

            labTest.LabUom ??= new LabUomDto();

            LabTest = labTest;
            LabUom = labTest.LabUom;
        }

        #endregion Grid Lab Test

        private async Task OnPrint()
        {
            if (FormRegis.Id == 0)
                return;

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var image = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\mcdermott_logo.png");
            var file = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files\Slip_Registration.pdf");
            QuestPDF.Fluent.Document
                .Create(x =>
                {
                    x.Page(page =>
                    {
                        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);

                        page.Header().Row(row =>
                        {
                            row.ConstantItem(150).Image(File.ReadAllBytes(image));
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Slip Registration").FontSize(36).SemiBold();
                                c.Item().Text($"MedRec: {FormRegis.Patient?.NoRm}");
                                c.Item().Text($"Patient: {FormRegis.Patient?.Name}");
                                c.Item().Text($"Identity Number: {FormRegis.Patient?.NoId}");
                                c.Item().Text($"Reg Type: {FormRegis.TypeRegistration}");
                                c.Item().Text($"Service: {FormRegis.Service?.Name}");
                                c.Item().Text($"Physicion: {FormRegis.Pratitioner?.Name}");
                                c.Item().Text($"Payment: {FormRegis.Payment}");
                                c.Item().Text($"Registration Date: {FormRegis.RegistrationDate}");
                                c.Item().Text($"Schedule Time: {FormRegis.ScheduleTime}");
                            });
                        });
                        //page.Header().Text("Slip Registration").SemiBold().FontSize(30);
                    });
                })
            .GeneratePdf(file);

            await Helper.DownloadFile("Slip_Registration.pdf", HttpContextAccessor, HttpClient, JsRuntime);

            //NavigationManager.NavigateTo(Path.Combine(Directory.GetCurrentDirectory(), @"Slip_Registration.pdf"), forceLoad: true);
        }

        #region Relation Data

        private PatientAllergyDto PatientAllergy = new();
        private GeneralConsultantClinicalAssesmentDto GeneralConsultantClinical = new();
        private List<GeneralConsultanServiceDto> GeneralConsultanServices = [];
        private List<UserDto> IsPatient = [];
        private List<UserDto> patients = [];

        private List<LabTestDto> LabTests = [];
        private List<UserDto> IsPratition = [];
        private List<UserDto> AllDoctors = [];
        private List<InsuranceDto> Insurances = [];
        private List<InsuranceDto> AllInsurances = [];
        private List<InsurancePolicyDto> InsurancePolicies = [];
        private List<ServiceDto> Services = [];
        private List<PatientAllergyDto> PatientAllergies = [];

        #region Form Regis

        public MarkupString GetIssuePriorityIconHtml(GeneralConsultanServiceDto priority)
        {
            if (priority is not null)
            {
                if (!priority.IsAlertInformationSpecialCase && priority.ClassType is null)
                    return new MarkupString("");

                string priorytyClass = "danger";
                string title = string.Empty;

                if (priority.IsAlertInformationSpecialCase && priority.ClassType is not null)
                    title = $" Priority, {priority.ClassType.Name}";
                else
                {
                    if (priority.ClassType is not null)
                        title = $"{priority.ClassType.Name}";
                    if (priority.IsAlertInformationSpecialCase)
                        title = $" Priority ";
                }

                string html = string.Format("<span class='badge bg-{0} py-1 px-2' title='{1} Priority'>{1}</span>", priorytyClass, title);

                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        private GeneralConsultanServiceDto FormRegis = new();

        #endregion Form Regis

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

        private IEnumerable<DoctorScheduleDto> SelectedSchedules = [];
        private IEnumerable<string> SelectedNames { get; set; } = [];
        private List<string> Names { get; set; } = [];

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

        private long Value
        {
            get => PatientsId;
            set
            {
                long PatientsId = value;
                this.PatientsId = value;

                var item = patients.FirstOrDefault(x => x.Id == PatientsId);

                try
                {
                    if (item.DateOfBirth != null)
                    {
                        DateTime currentDate = DateTime.UtcNow;
                        Birthdate = item.DateOfBirth;
                        Age = currentDate.Year - Birthdate!.Value.Year;
                    }

                    FormRegis.NoRM = item.NoRm;
                    FormRegis.IdentityNumber = item.NoId ?? "";
                    FormRegis.PatientId = item.Id;
                }
                catch { }

                var patientAlergy = PatientAllergies.Where(x => x.UserId == item!.Id).FirstOrDefault();

                if (patientAlergy is not null)
                {
                    //FormRegis.IsWeather = patientAlergy.Any(x => !string.IsNullOrWhiteSpace(x.Weather));
                    //FormRegis.IsPharmacology = patientAlergy.Any(x => !string.IsNullOrWhiteSpace(x.Farmacology));
                    //FormRegis.IsFood = patientAlergy.Any(x => !string.IsNullOrWhiteSpace(x.Food));
                    PatientAllergy = patientAlergy;
                    PatientAllergy.Food = patientAlergy.Food;
                    PatientAllergy.Weather = patientAlergy.Weather;
                    PatientAllergy.Farmacology = patientAlergy.Farmacology;
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
        private long _NursingDiagnosis { get; set; }

        private long NursingDiagnosis
        {
            get => _NursingDiagnosis;
            set
            {
                //FormInputCPPTGeneralConsultan.NursingDiagnosisId = value;
                //_NursingDiagnosis = value;

                //FormInputCPPTGeneralConsultan.Code = NursingDiagnoses.FirstOrDefault(x => x.Id == FormInputCPPTGeneralConsultan.NursingDiagnosisId)!.Code!;
            }
        }

        public class NurseStation
        {
            public long Id { get; set; }
            public string Status { get; set; }
            public long Count { get; set; }
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

        private List<ClassTypeDto> ClassTypes = [];

        private List<string> Method = new List<string>
        {
            "MCU",
            "Gas And Oil"
        };

        private long PatientsId = 0;

        private long PractitionerId = 0;

        private long Age = 0;
        private DateTime? Birthdate { get; set; }
        private string IdentityNum { get; set; }
        private string _PaymentMethod { get; set; }

        private string MedicalTypee { get; set; }
        private List<InsuranceTemp> Temps = [];

        private class InsuranceTemp
        {
            public long InsurancePolicyId { get; set; }
            public long InsuranceId { get; set; }
            public string InsuranceName { get; set; }
            public string PolicyNumber { get; set; }

            public string ConcatInsurancePolicy
            { get { return PolicyNumber + " - " + InsuranceName; } }
        }

        private string StagingText = "Confirmed";

        private List<string> Stagings = new List<string>
        {
            "Planned",
            "Confirmed",
            "Nurse Station",
            "Waiting",
            "In Consultation",
            "Consultation Completed"
        };

        private List<string> Times = [];

        #endregion Data Statis

        #region Grid Setting

        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool PanelVisible { get; set; } = true;
        private bool showForm { get; set; } = false;
        private string textPopUp = "";
        private string Timeee = "";
        private string DisplayFormat { get; } = string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? "HH:mm" : "h:mm tt";
        public IGrid Grid { get; set; }
        private int ActiveTabIndex { get; set; } = 0;

        private void OnTabClick(TabClickEventArgs e)
        {
        }

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataItems2 { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }

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
        //    long maxFileSize = 1 * 1024 * 1024;
        //    var allowedExtenstions = new string[] { ".png", ".jpg", ".jpeg", ".gif" };

        //    UserForm.SipFile = e.File.Name;

        //    await FileUploadService.UploadFileAsync(e.File, maxFileSize, []);
        //}

        #endregion File Upload Attachment

        #region Tab CPPT

        private IGrid GridTabCPPT { get; set; }
        private long FocusedGridTabCPPTRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItemsCPPT { get; set; } = new ObservableRangeCollection<object>();

        private InputCPPTGeneralConsultanCPPT FormInputCPPTGeneralConsultan = new();
        private GeneralConsultanCPPTDto GeneralConsultanCPPT = new();

        private List<NursingDiagnosesDto> NursingDiagnoses = [];
        private List<NursingDiagnosesTemp> NursingDiagnosesTemps = [];
        private List<DiagnosisDto> Diagnoses = [];
        private List<DiagnosesTemp> DiagnosesTemps = [];
        private List<DiseaseCategoryDto> DiseaseCategories = [];
        private List<DiseaseCategoryDto> AllDiseaseCategories = [];
        private List<GeneralConsultanCPPTDto> AllGeneralConsultanCPPTs = [];
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs = [];

        private class NursingDiagnosesTemp
        {
            public long Id { get; set; }
            public string Problem { get; set; } = string.Empty;
        }

        private class DiagnosesTemp
        {
            public long Id { get; set; }

            public string NameCode { get; set; }
        }

        private class InputCPPTGeneralConsultanCPPT
        {
            public string? Subjective { get; set; }
            public string? Objective { get; set; }

            [DisplayName("Planning")]
            public string? Plan { get; set; }

            public string? Diagnosis { get; set; }

            [Required]
            [DisplayName("Nurse Diagnosis")]
            public string? NursingDiagnosis { get; set; }

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
                    string title = key;

                    if (title.Equals("Plan"))
                        title = "Planning";
                    else if (title.Equals("NursingDiagnosis"))
                        title = "Nurse Diagnosis";

                    temps.Add(new GeneralConsultanCPPTDto
                    {
                        Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                        Title = title,
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

        private void OnClickGetObjectives()
        {
            FormInputCPPTGeneralConsultan.Objective = $"Weight: {GeneralConsultantClinical.Weight}, Height: {GeneralConsultantClinical.Height}, RR: {GeneralConsultantClinical.RR}, SpO2: {GeneralConsultantClinical.SpO2}, BMIIndex: {Math.Round(GeneralConsultantClinical.BMIIndex, 2).ToString()}, BMIState: {GeneralConsultantClinical.BMIState}, Temp: {GeneralConsultantClinical.Temp}, HR: {GeneralConsultantClinical.HR}, Systolic: {GeneralConsultantClinical.Systolic}, DiastolicBP: {GeneralConsultantClinical.DiastolicBP}, RBS: {GeneralConsultantClinical.RBS}, E: {GeneralConsultantClinical.E}, V: {GeneralConsultantClinical.V}, M: {GeneralConsultantClinical.M}";
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

        private async Task DownloadFile(string fileName)
        {
            if (GeneralConsultanMedicalSupport.Id != 0 && !string.IsNullOrWhiteSpace(fileName))
            {
                await Helper.DownloadFile(fileName, HttpContextAccessor, HttpClient, JsRuntime);
            }
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
            PanelVisible = true;

            ClassTypes = await Mediator.Send(new GetClassTypeQuery());

            InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery());
            NursingDiagnoses = await Mediator.Send(new GetNursingDiagnosesQuery());
            LabTests = await Mediator.Send(new GetLabTestQuery());

            var nursingDiagnosesTemps = NursingDiagnoses.Select(x => new NursingDiagnosesTemp
            {
                Id = x.Id,
                Problem = $"{x.Problem} - {x.Code}" // Menggunakan interpolasi string untuk menggabungkan Problem dan Code
            }).ToList();
            NursingDiagnosesTemps.AddRange(nursingDiagnosesTemps);

            Diagnoses = await Mediator.Send(new GetDiagnosisQuery());
            var diagnosesTemps = Diagnoses.Select(x => new DiagnosesTemp
            {
                Id = x.Id,
                NameCode = $"{x.Name} - {x.Code}" // Menggunakan interpolasi string untuk menggabungkan Problem dan Code
            }).ToList();
            DiagnosesTemps.AddRange(diagnosesTemps);

            AllDiseaseCategories = await Mediator.Send(new GetDiseaseCategoryQuery());

            await GetUserInfo();
            await LoadData();
        }

        private void GetInsurancePhysician(long value)
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

        private async Task GetScheduleTimesUser(long value)
        {
            var slots = await Mediator.Send(new GetDoctorScheduleSlotQuery(x => x.PhysicianId == value));
        }

        private async Task SetTimeSchedule()
        {
            try
            {
                var slots = await Mediator.Send(new GetDoctorScheduleSlotQuery(x => x.PhysicianId == FormRegis.PratitionerId && x.StartDate.Date == FormRegis.RegistrationDate.Date && x.DoctorSchedule.ServiceId == FormRegis.ServiceId));

                Times.Clear();

                Times.AddRange(slots.Select(x => $"{x.WorkFromFormatString} - {x.WorkToFormatString}"));

                if (slots.Count <= 0)
                    FormRegis.ScheduleTime = null;

                StateHasChanged();

                //if (Times.Count <= 0)
                //    FormRegis.ScheduleTime = null;
            }
            catch { }
        }

        private bool IsLoading { get; set; } = false;

        private async Task Send()
        {
            IsLoading = true;
            ToastService.ShowInfo("Start");
            await Task.Delay(5000);
            IsLoading = false;
        }

        private async Task OnClickConfirm()
        {
            try
            {
                IsLoading = true;
                if (FormRegis.PatientId == null || FormRegis.TypeRegistration == null || FormRegis.ServiceId is null || FormRegis.PratitionerId is null || FormRegis.ScheduleTime is null || (!FormRegis.Payment!.Equals("Personal") && (FormRegis.InsurancePolicyId == 0 || FormRegis.InsurancePolicyId is null)))
                {
                    IsLoading = false;
                    ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                    return;
                }

                if (FormRegis.Id == 0)
                {
                    var patient = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.PatientId == FormRegis.PatientId && x.StagingStatus!.Equals("Planned") && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date));

                    if (patient.Count > 0)
                    {
                        IsLoading = false;
                        ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                        return;
                    }
                }

                if (FormRegis.Id != 0)
                {
                    var text = FormRegis.StagingStatus == "Physician" ? "Consultation Done" : FormRegis.StagingStatus;
                    var index = Stagings.FindIndex(x => x == text);
                    if (text != "Consultation Done")
                    {
                        FormRegis.StagingStatus = Stagings[index + 1];
                        if (index + 1 == 4)
                        {
                            FormRegis.StagingStatus = "Physician";
                        }
                        else if (index + 1 == 5)
                        {
                            FormRegis.StagingStatus = "Finished";
                        }
                        try
                        {
                            StagingText = FormRegis.StagingStatus == "In Consultant" ? "Finished" : Stagings[index + 2];
                        }
                        catch { }
                    }
                    else
                    {
                        FormRegis.StagingStatus = "Finished";
                    }
                    FormRegis = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));
                }
                else
                {
                    FormRegis.StagingStatus = "Confirmed";
                    StagingText = "Nurse Station";
                    FormRegis = await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormRegis));
                }

                var result = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == FormRegis.Id));
                FormRegis = result[0];

                //var text1 = FormRegis.StagingStatus == "Physician" ? "Consultation Done" : FormRegis.StagingStatus;
                //var index1 = Stagings.FindIndex(x => x == text1);
                //if (text1 != "Consultation Done")
                //{
                //    FormRegis.StagingStatus = Stagings[index1 + 1];
                //    if (index1 + 1 == 4)
                //    {
                //        FormRegis.StagingStatus = "Physician";
                //    }
                //    else if (index1 + 1 == 5)
                //    {
                //        FormRegis.StagingStatus = "Finished";
                //    }
                //    try
                //    {
                //        StagingText = FormRegis.StagingStatus == "In Consultant" ? "Finished" : Stagings[index1 + 2];
                //    }
                //    catch { }
                //}

                FormRegis.IsWeather = !string.IsNullOrWhiteSpace(PatientAllergy.Weather);
                FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(PatientAllergy.Farmacology);
                FormRegis.IsFood = !string.IsNullOrWhiteSpace(PatientAllergy.Food);

                IsLoading = false;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task OnCancel2()
        {
            IsLoading = true;
            if (FormRegis.Id != 0)
            {
                FormRegis.StagingStatus = "Canceled";

                await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));

                var result = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == FormRegis.Id));
                FormRegis = result[0];

                ToastService.ShowSuccess("Cancelled..");
            }
            IsLoading = false;
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
            patients = [.. user.Where(x => x.IsPatient == true || x.IsEmployeeRelation == true).ToList()];

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
            IsLoading = true;
            FormValidationState = true;

            if (PopUpProcedureRoom)
                await OnSaveProcedureRoom();
            else
                await OnSave();
            IsLoading = false;
        }

        private async Task OnSaveProcedureRoom()
        {
            try
            {
                PopUpProcedureRoom = false;

                if (FormRegis.Id == 0)
                    return;

                BrowserFiles.Distinct();

                foreach (var item in BrowserFiles)
                {
                    await FileUploadService.UploadFileAsync(item, 0, []);
                }

                if (GeneralConsultanMedicalSupport.Id == 0)
                {
                    GeneralConsultanMedicalSupport.GeneralConsultanServiceId = FormRegis.Id;
                    GeneralConsultanMedicalSupport = await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                }
                else
                    GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));

                if (IsAddOrUpdateOrDeleteLabResult)
                {
                    await Mediator.Send(new DeleteLabResultDetailRequest(ids: DeletedLabTestIds));

                    LabResultDetails.ForEach(x => x.Id = 0);

                    LabResultDetails.ForEach(x =>
                    {
                        x.Id = 0;
                        x.GeneralConsultanMedicalSupportId = GeneralConsultanMedicalSupport.Id;
                    });

                    await Mediator.Send(new CreateListLabResultDetailRequest(LabResultDetails));

                    IsAddOrUpdateOrDeleteLabResult = false;
                }

                ToastService.ShowSuccess("Saved Successfully");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void GridCPPT_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.Grid.GetRowValue(e.VisibleIndex, "Body") is "")
            {
                e.CssClass = "highlighted-item";
            }
        }

        private bool FormValidationStateCPPT = true;

        private void HandleValidSubmitCPPT()
        {
            FormValidationStateCPPT = true;

            OnClickConfirmCPPT();
        }

        private void HandleInvalidSubmitCPPT()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationStateCPPT = false;
        }

        private async Task OnSave()
        {
            try
            {
                IsLoading = true;
                ToastService.ClearInfoToasts();

                if (!FormValidationState)
                    return;

                if (!FormRegis.IsWeather)
                    PatientAllergy.Weather = null;
                if (!FormRegis.IsPharmacology)
                    PatientAllergy.Farmacology = null;
                if (!FormRegis.IsFood)
                    PatientAllergy.Food = null;

                GeneralConsultanMedicalSupport.LabResulLabExaminationtIds = SelectedLabTests.Select(x => x.Id).ToList();

                if (FormRegis.Id == 0)
                {
                    var patient = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.PatientId == FormRegis.PatientId && x.StagingStatus!.Equals("Planned") && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date));

                    if (patient.Count > 0)
                    {
                        ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                        return;
                    }
                }

                if (!FormRegis.Payment!.Equals("Personal") && (FormRegis.InsurancePolicyId == 0 || FormRegis.InsurancePolicyId is null))
                {
                    ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                    return;
                }

                if (IsReferTo)
                {
                    FormRegis.Id = 0;
                    FormRegis.StagingStatus = "Planned";
                    StagingText = "Confirmed";
                    PopUpVisible = false;
                    FormRegis = await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormRegis));
                }
                else if (IsAppoiment)
                {
                    if (FormRegis.AppoimentDate is null)
                    {
                        ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                        return;
                    }

                    FormRegis.Id = 0;
                    FormRegis.StagingStatus = "Planned";
                    StagingText = "Confirmed";
                    PopUpAppoiment = false;
                    FormRegis = await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormRegis));
                    await LoadData();
                }
                else
                {
                    // Ketika Kondisi New
                    if (FormRegis.Id == 0)
                    {
                        switch (FormRegis.StagingStatus)
                        {
                            case "Planned":
                                FormRegis = await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormRegis));

                                PatientAllergy.UserId = FormRegis.PatientId.GetValueOrDefault();

                                if (PatientAllergy.Id == 0)
                                    PatientAllergy = await Mediator.Send(new CreatePatientAllergyRequest(PatientAllergy));
                                else
                                    PatientAllergy = await Mediator.Send(new UpdatePatientAllergyRequest(PatientAllergy));

                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (FormRegis.StagingStatus)
                        {
                            case "Planned":
                                FormRegis = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));

                                PatientAllergy.UserId = FormRegis.PatientId.GetValueOrDefault();

                                if (PatientAllergy.Id == 0)
                                    PatientAllergy = await Mediator.Send(new CreatePatientAllergyRequest(PatientAllergy));
                                else
                                    PatientAllergy = await Mediator.Send(new UpdatePatientAllergyRequest(PatientAllergy));

                                break;

                            case "Nurse Station":

                                if (GeneralConsultantClinical.Id == 0)
                                {
                                    GeneralConsultantClinical.GeneralConsultanServiceId = FormRegis.Id;
                                    GeneralConsultantClinical = await Mediator.Send(new CreateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinical));
                                }
                                else
                                {
                                    GeneralConsultantClinical = await Mediator.Send(new UpdateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinical));
                                }

                                await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(deleteByGeneralServiceId: FormRegis.Id));

                                GeneralConsultanCPPTs.ForEach(x => { x.GeneralConsultanServiceId = FormRegis.Id; x.Id = 0; });
                                await Mediator.Send(new CreateListGeneralConsultanCPPTRequest(GeneralConsultanCPPTs));
                                break;

                            case "Physician":

                                if (GeneralConsultantClinical.Id == 0)
                                {
                                    GeneralConsultantClinical.GeneralConsultanServiceId = FormRegis.Id;
                                    GeneralConsultantClinical = await Mediator.Send(new CreateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinical));
                                }
                                else
                                {
                                    GeneralConsultantClinical = await Mediator.Send(new UpdateGeneralConsultantClinicalAssesmentRequest(GeneralConsultantClinical));
                                }

                                FormRegis = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));

                                await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(deleteByGeneralServiceId: FormRegis.Id));

                                GeneralConsultanCPPTs.ForEach(x => { x.GeneralConsultanServiceId = FormRegis.Id; x.Id = 0; });
                                await Mediator.Send(new CreateListGeneralConsultanCPPTRequest(GeneralConsultanCPPTs));

                                BrowserFiles.Distinct();

                                foreach (var item in BrowserFiles)
                                {
                                    await FileUploadService.UploadFileAsync(item, 0, []);
                                }

                                //if (GeneralConsultanMedicalSupport.Id == 0)
                                //{
                                //    GeneralConsultanMedicalSupport.GeneralConsultanServiceId = FormRegis.Id;
                                //    GeneralConsultanMedicalSupport = await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                                //}
                                //else
                                //{
                                //    GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                                //}
                                break;

                            default:
                                break;
                        }
                    }
                }

                var result = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == FormRegis.Id));
                FormRegis = result[0];

                FormRegis.IsWeather = !string.IsNullOrWhiteSpace(PatientAllergy.Weather);
                FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(PatientAllergy.Farmacology);
                FormRegis.IsFood = !string.IsNullOrWhiteSpace(PatientAllergy.Food);

                ToastService.ShowSuccess("Saved Successfully");
                IsLoading = false;
            }
            catch (Exception exx)
            {
                IsLoading = false;
                exx.HandleException(ToastService);
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        private async Task LoadData()
        {
            showForm = false;
            PanelVisible = true;
            PatientAllergy = new();
            SelectedDataItems = new ObservableRangeCollection<object>();
            GeneralConsultanServices = await Mediator.Send(new GetGeneralConsultanServiceQuery());
            PatientAllergies = await Mediator.Send(new GetPatientAllergyQuery());
            await SelectData();
            IsReferTo = false;
            PopUpVisible = false;
            PanelVisible = false;

            if (Grid is not null)
                Grid.AutoFitColumnWidths();
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

        private bool IsDeletedConsultantService = false;

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;

            try
            {
                if ((GeneralConsultanServiceDto)args.DataItem is null)
                    return;

                IsDeletedConsultantService = ((GeneralConsultanServiceDto)args.DataItem)!.StagingStatus!.Equals("Planned") || ((GeneralConsultanServiceDto)args.DataItem)!.StagingStatus!.Equals("Canceled");
            }
            catch { }

            UpdateEditItemsEnabled(true);
        }

        private async Task NewItem_Click()
        {
            StagingText = "Confirmed";
            await SelectData();
            showForm = true;
            IsReferTo = false;
            IsAppoiment = false;
            FormRegis = new GeneralConsultanServiceDto();
            GeneralConsultantClinical = new GeneralConsultantClinicalAssesmentDto();
            FormInputCPPTGeneralConsultan = new InputCPPTGeneralConsultanCPPT();
            GeneralConsultanCPPTs.Clear();
            GeneralConsultanMedicalSupport = new GeneralConsultanMedicalSupportDto();
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
                    var text = FormRegis.StagingStatus == "Physician" ? "In Consultation" : FormRegis.StagingStatus;
                    var index = Stagings.FindIndex(x => x == text);
                    StagingText = Stagings[index + 1];
                }

                var patientAllergy = PatientAllergies.FirstOrDefault(x => x.UserId == FormRegis!.PatientId);

                if (patientAllergy != null)
                {
                    PatientAllergy = patientAllergy;
                    FormRegis.IsWeather = !string.IsNullOrWhiteSpace(patientAllergy.Weather);
                    FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(patientAllergy.Farmacology);
                    FormRegis.IsFood = !string.IsNullOrWhiteSpace(patientAllergy.Food);
                }
                else
                {
                    PatientAllergy = new PatientAllergyDto(); // Create a new instance if no allergy is found
                    FormRegis.IsWeather = FormRegis.IsPharmacology = FormRegis.IsFood = false;
                }

                // Assign null to properties if patientAllergy is null or clear them if a new instance was created
                PatientAllergy.Food ??= null;
                PatientAllergy.Weather ??= null;
                PatientAllergy.Farmacology ??= null;

                switch (FormRegis.StagingStatus)
                {
                    case "Nurse Station":
                        var clinical = await Mediator.Send(new GetGeneralConsultantClinicalAssesmentQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        AllGeneralConsultanCPPTs = GeneralConsultanCPPTs.Select(item => item).ToList();

                        if (clinical.Count > 0)
                            GeneralConsultantClinical = clinical[0];
                        break;

                    case "Physician":
                        var clinicals = await Mediator.Send(new GetGeneralConsultantClinicalAssesmentQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        if (clinicals.Count > 0)
                            GeneralConsultantClinical = clinicals[0];

                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        AllGeneralConsultanCPPTs = GeneralConsultanCPPTs.Select(item => item).ToList();

                        var support = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        if (support.Count > 0)
                            GeneralConsultanMedicalSupport = support[0];

                        SelectedLabTests = LabTests.Where(x => GeneralConsultanMedicalSupport.LabResulLabExaminationtIds != null && GeneralConsultanMedicalSupport.LabResulLabExaminationtIds.Contains(x.Id)).ToList();
                        break;

                    case "Waiting":
                        var support1 = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        if (support1.Count > 0)
                            GeneralConsultanMedicalSupport = support1[0];

                        var clinicals1 = await Mediator.Send(new GetGeneralConsultantClinicalAssesmentQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        if (clinicals1.Count > 0)
                            GeneralConsultantClinical = clinicals1[0];

                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        AllGeneralConsultanCPPTs = GeneralConsultanCPPTs.Select(item => item).ToList();

                        SelectedLabTests = LabTests.Where(x => GeneralConsultanMedicalSupport.LabResulLabExaminationtIds != null && GeneralConsultanMedicalSupport.LabResulLabExaminationtIds.Contains(x.Id)).ToList();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception exx)
            {
                exx.HandleException(ToastService);
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

        private async Task OnCancel()
        {
            FormRegis = new();
            GeneralConsultantClinical = new GeneralConsultantClinicalAssesmentDto();
            await LoadData();
            showForm = false;
            PopUpVisible = false;
        }

        private void OnCancelReferTo()
        {
            PopUpVisible = false;
        }

        private void OnCancelAppoimentPopup()
        {
            PopUpAppoiment = false;
        }

        private void OnCancelProcedureRoom()
        {
            PopUpProcedureRoom = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                    return;

                if (SelectedDataItems is not null && SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(((GeneralConsultanServiceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>();

                    a = a.Where(x => x.StagingStatus == "Planned" || x.StagingStatus == "Canceled").ToList();

                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        #region Function

        private bool IsEnableField()
        {
            if (true)
                return true;

            return false;
        }

        private bool PopUpVisible = false;
        private bool PopUpHistoricalMedical = false;
        private bool PopUpAppoimentPending = false;
        private bool PopUpAppoiment = false;
        private bool PopUpProcedureRoom = false;
        private bool IsReferTo = false;
        private bool IsAppoiment = false;

        private GeneralConsultanServiceDto GeneralConsultanTemp = new();

        private async Task OnReferToClick()
        {
            GeneralConsultanTemp = FormRegis;
            IsReferTo = true;
            PopUpVisible = true;

            try
            {
                IsPratition = AllDoctors.Where(x => x.DoctorServiceIds is not null && x.DoctorServiceIds.Contains(FormRegis.ServiceId.GetValueOrDefault())).ToList();

                await SetTimeSchedule();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task CloseReferTo()
        {
            PopUpVisible = false;
            var value = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == FormRegis.Id));
            if (value.Count > 0)
                FormRegis = value[0];
        }

        private async Task CloseAppoimentPopUp()
        {
            PopUpAppoiment = false;
            var value = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == FormRegis.Id));
            if (value.Count > 0)
                FormRegis = value[0];
        }

        private async Task OnAppoimentPopUpClick()
        {
            IsAppoiment = true;
            PopUpAppoiment = true;

            try
            {
                IsPratition = AllDoctors.Where(x => x.DoctorServiceIds is not null && x.DoctorServiceIds.Contains(FormRegis.ServiceId.GetValueOrDefault())).ToList();

                await SetTimeSchedule();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void OnClickPopUpHistoricalMedical()
        {
            PopUpHistoricalMedical = true;
        }

        private async Task OnClickPopUpPopUpProcedureRoom()
        {
            PopUpProcedureRoom = true;

            var support = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));

            if (support.Count > 0)
                GeneralConsultanMedicalSupport = support[0];

            DeletedLabTestIds.Clear();

            LabResultDetails = await Mediator.Send(new GetLabResultDetailQuery(x => x.GeneralConsultanMedicalSupportId == GeneralConsultanMedicalSupport.Id));
            DeletedLabTestIds = LabResultDetails.Select(x => x.Id).ToList();
        }

        private void OnClickPopUpAppoimentPending()
        {
            PopUpAppoimentPending = true;
        }

        private void SelectedCountryChanged(string country)
        {
            FormRegis.TypeRegistration = country;
            ToastService.ShowInfo(country);
        }

        private async Task SelectedItemServiceChanged(ServiceDto e)
        {
            try
            {
                if (e is null)

                {
                    FormRegis.PratitionerId = null;
                    IsPratition.Clear();
                    return;
                }

                IsPratition = AllDoctors.Where(x => x.DoctorServiceIds is not null && x.DoctorServiceIds.Contains(e.Id)).ToList();

                await SetTimeSchedule();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SelectedItemPhysicianChanged(UserDto? e)
        {
            try
            {
                await SetTimeSchedule();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private IEnumerable<LabTestDto> SelectedLabTests = [];

        private async Task SelectedItemRegistrationDateChanged(DateTime e)
        {
            FormRegis.RegistrationDate = e;
            FormRegis.AppoimentDate = e;
            await SetTimeSchedule();
        }

        private void SelectedItemPaymentChanged(string e)
        {
            Insurances.Clear();
            Temps.Clear();

            if (e is null)
                return;

            FormRegis.Payment = e;
            _PaymentMethod = e;

            if (PaymentMethod.Equals("BPJS"))
            {
                var all = InsurancePolicies.Where(x => x.UserId == PatientsId && x.Insurance.IsBPJS == true && x.Active == true).ToList();
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
                var all = InsurancePolicies.Where(x => x.UserId == PatientsId && x.Insurance.IsBPJS != true && x.Active == true).ToList();
                Temps = all.Select(x => new InsuranceTemp
                {
                    InsurancePolicyId = x.Id,
                    InsuranceId = x.InsuranceId,
                    InsuranceName = x.Insurance.Name,
                    PolicyNumber = x.PolicyNumber
                }).ToList();
            }
        }

        private void SelectedItemPatientChanged(UserDto e)
        {
            if (e is null)
            {
                FormRegis.InsurancePolicyId = null;
                //FormRegis.Age = null;
                FormRegis.NoRM = null;
                FormRegis.IdentityNumber = null;
                PatientAllergy.Food = null;
                PatientAllergy.Weather = null;
                PatientAllergy.Farmacology = null;
                FormRegis.IsWeather = false;
                FormRegis.IsPharmacology = false;
                FormRegis.IsFood = false;
                return;
            }

            var value = e.Id;

            long PatientsId = value;
            this.PatientsId = value;

            var item = patients.FirstOrDefault(x => x.Id == PatientsId);
            FormRegis.Patient = item;

            try
            {
                if (item is not null && item.DateOfBirth != null)
                {
                    //FormRegis.Age = DateTime.Now.Year - item.DateOfBirth.GetValueOrDefault().Year;
                }

                FormRegis.NoRM = item.NoRm ?? null;
                FormRegis.IdentityNumber = item.NoId ?? null;
                FormRegis.PatientId = item.Id;
            }
            catch { }

            if (PaymentMethod is not null)
            {
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
            }

            var patientAlergy = PatientAllergies.Where(x => x.UserId == item!.Id).FirstOrDefault();

            if (patientAlergy is not null)
            {
                //FormRegis.IsWeather = patientAlergy.Any(x => !string.IsNullOrWhiteSpace(x.Weather));
                //FormRegis.IsPharmacology = patientAlergy.Any(x => !string.IsNullOrWhiteSpace(x.Farmacology));
                //FormRegis.IsFood = patientAlergy.Any(x => !string.IsNullOrWhiteSpace(x.Food));
                PatientAllergy = patientAlergy;
                PatientAllergy.Food = patientAlergy.Food;
                PatientAllergy.Weather = patientAlergy.Weather;
                PatientAllergy.Farmacology = patientAlergy.Farmacology;
                FormRegis.IsWeather = !string.IsNullOrWhiteSpace(patientAlergy.Weather);
                FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(patientAlergy.Farmacology);
                FormRegis.IsFood = !string.IsNullOrWhiteSpace(patientAlergy.Food);
            }
            else
            {
                PatientAllergy.Food = null;
                PatientAllergy.Weather = null;
                PatientAllergy.Farmacology = null;
                FormRegis.IsWeather = false;
                FormRegis.IsPharmacology = false;
                FormRegis.IsFood = false;
            }
        }

        private void SelectedItemChanged(String e)
        {
            if (e is null)
            {
                return;
            }

            if (e.Equals("Emergency"))
            {
                Method =
                [
                    "General",
                    "Work Related Injury",
                    "Road Accident Injury",
                ];
                FormRegis.TypeMedical = Method[0];
            }
            else if (e.Equals("MCU"))
            {
                Method =
                [
                    "Annual MCU",
                    "Pre Employment MCU",
                    "Oil & Gas UK",
                    "HIV & AIDS",
                    "Covid19*",
                    "Drug & Alcohol Test",
                    "Maternity Checkup"
                ];
                FormRegis.TypeMedical = Method[0];
            }
            else if (e.Equals("General Consultation"))

                FormRegis.TypeMedical = null;
        }
    }

    #endregion Function
}
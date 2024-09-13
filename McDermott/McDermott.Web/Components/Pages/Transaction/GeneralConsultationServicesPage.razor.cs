using QuestPDF.Fluent;
using System.ComponentModel;
using Microsoft.Extensions.Caching.Memory;
using static McDermott.Web.Components.Pages.Queue.KioskPage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using McDermott.Extentions;
using static McDermott.Application.Features.Commands.Employee.SickLeaveCommand;
using MediatR;
using DevExpress.Blazor.RichEdit;
using static McDermott.Web.Components.Pages.Transaction.ProcedureRoomPage;
using Microsoft.AspNetCore.HttpLogging;
using Mapster;
using static McDermott.Application.Features.Commands.Transaction.AccidentCommand;
using FluentValidation.Results;
using FluentValidation;
using Path = System.IO.Path;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class GeneralConsultationServicesPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService? UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess { get; set; } = false;

        private async Task GetUserInfo()
        {
            try
            {
                var user = await UserInfoService?.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        private IGrid? Grid { get; set; }
        private IGrid? GridCppt { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsCPPT { get; set; } = [];

        private EnumStatusGeneralConsultantService StagingText { get; set; } = EnumStatusGeneralConsultantService.Confirmed;
        private DxRichEdit richEdit;
        private DevExpress.Blazor.RichEdit.Document documentAPI;
        private bool ShowForm { get; set; } = false;
        private bool PanelVisible { get; set; } = false;
        private bool IsDeletedConsultantService { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedGridTabCPPTRowVisibleIndex { get; set; }
        private bool IsReferTo { get; set; } = false;
        private bool IsLoadingSearchFaskes { get; set; } = false;
        private bool IsFollowUp { get; set; } = false;
        private bool IsHistoricalRecordPatient { get; set; } = false;
        private bool IsAppoimentPendingAlert { get; set; } = false;
        private bool IsPopUpPainScale { get; set; } = false;
        private string SelectedRujukanType { get; set; } = "Rujuk Internal";
        private string SelectedRujukanExternal { get; set; }
        private string SelectedRujukanVertical { get; set; }
        private IEnumerable<string> RujukanTypes = new[] { "Rujuk Internal", "Rujukan External", "McDermott Internal Refferal" };
        private IEnumerable<string> RujukanExtenalTypes = new[] { "Rujukan Horizontal", "Rujukan Vertical" };
        private IEnumerable<string> RujukanExtenalVertical = new[] { "Kondisi Khusus", "Spesialis" };
        private IGrid GridRujukanRefer { get; set; }
        private List<SpesialisRefrensiKhususPCare> SpesialisRefrensiKhusus = [];
        private List<SpesialisPCare> SpesialisPs = [];
        private List<SpesialisSaranaPCare> SpesialisSaranas = [];
        private List<SubSpesialisPCare> SubSpesialisPs = [];
        private List<RujukanFaskesKhususSpesialisPCare> RujukanSubSpesialis = [];

        private List<SickLeaveDto> dataSickLeaves = [];
        private List<UserDto> Users = [];
        public byte[]? DocumentContent;
        private SickLeaveDto SickLeaves = new();
        private UserDto UserForm = new();
        private bool isPrint { get; set; } = false;

        private void OnSelectRiskOfFalling(string e)
        {
            RiskOfFallingDetail.Clear();
            GeneralConsultanService.RiskOfFallingDetail = null;
            if (e is null)
            {
                return;
            }

            if (e == "Humpty Dumpty")
            {
                RiskOfFallingDetail = HumptyDumpty.ToList();
            }
            else if (e == "Morse")
            {
                RiskOfFallingDetail = Morse.ToList();
            }
            else
            {
                RiskOfFallingDetail = Geriati.ToList();
            }
        }

        private List<string> RiskOfFallingDetail = [];

        private List<string> HumptyDumpty =
        [
            "Risiko rendah 0-6",
            "Risiko sedang 7-11",
            "Risiko Tinggi >= 12"
        ];

        private List<string> InformationFrom =
        [
            "Auto Anamnesa",
            "Allo Anamnesa"
        ];

        private List<string> YesNoOptions =
       [
           "Yes",
            "No"
       ];

        private List<string> Morse =
        [
            "Risiko rendah 0-24",
            "Risiko sedang 25-44",
            "Risiko Tinggi >= 45"
        ];

        private List<string> RiwayatPenyakitKeluarga =
        [
            "DM",
            "Hipertensi",
            "Cancer",
            "Jantung",
            "TBC",
            "Anemia",
            "Other",
        ];

        private List<string> Geriati =
       [
           "Risiko rendah 0-3",
            "Risiko Tinggi >= 4"
       ];

        private List<string> RiskOfFalling =
        [
            "Humpty Dumpty",
            "Morse",
            "Geriatri",
        ];

        private List<string> Colors = new List<string>
        {
            "Red",
            "Yellow",
            "Green",
            "Black",
        };

        private async void SendToPrint(long? grid)
        {
            try
            {
                DateTime? startSickLeave = null;
                DateTime? endSickLeave = null;

                var culture = new System.Globalization.CultureInfo("id-ID");

                Users = await Mediator.Send(new GetUserQuery());
                var data = GeneralConsultanServices.Where(x => x.Id == grid).FirstOrDefault()!;
                var patienss = Users.Where(x => x.Id == data.PatientId).FirstOrDefault() ?? new();
                var age = 0;
                if (data.Patient.DateOfBirth == null)
                {
                    age = 0;
                }
                else
                {
                    age = DateTime.Now.Year - patienss.DateOfBirth.Value.Year;
                }
                if (data.IsSickLeave)
                {
                    startSickLeave = data.StartDateSickLeave;
                    endSickLeave = data.EndDateSickLeave;
                }
                else if (data.IsMaternityLeave)
                {
                    startSickLeave = data.StartMaternityLeave;
                    endSickLeave = data.EndMaternityLeave;
                }

                int TotalDays = endSickLeave.Value.Day - startSickLeave.Value.Day;

                string WordDays = ConvertNumberHelper.ConvertNumberToWord(TotalDays);

                string todays = data.RegistrationDate.ToString("dddd", culture);

                //Gender
                string Gender = "";
                string OppositeSex = "";
                if (patienss.GenderId != null)
                {
                    Gender = patienss.Gender.Name == "Male" ? "MALE (L)" : "FEMALE (P)";
                    OppositeSex = patienss.Gender.Name == "Male" ? "<strike>F(P)</strike>" : "<strike>M(L)</strike>";
                }

                isPrint = true;
                string GetDefaultValue(string value, string defaultValue = "-")
                {
                    return value ?? defaultValue;
                }

                var mergeFields = new Dictionary<string, string>
                {
                    {"<<NamePatient>>", GetDefaultValue(patienss.Name)},
                    {"<<startDate>>", GetDefaultValue(startSickLeave?.ToString("dd MMMM yyyy"))},
                    {"<<endDate>>", GetDefaultValue(endSickLeave?.ToString("dd MMMM yyyy"))},
                    {"<<NameDoctor>>", GetDefaultValue(data?.Pratitioner?.Name)},
                    {"<<SIPDoctor>>", GetDefaultValue(data?.Pratitioner?.SipNo)},
                    {"<<AddressPatient>>", GetDefaultValue(patienss.DomicileAddress1) + GetDefaultValue(patienss.DomicileAddress2)},
                    {"<<AgePatient>>", GetDefaultValue(age.ToString())},
                    {"<<WordDays>>", GetDefaultValue(WordDays)},
                    {"<<Days>>", GetDefaultValue(todays)},
                    {"<<TotalDays>>", GetDefaultValue(TotalDays.ToString())},
                    {"<<Dates>>", GetDefaultValue(data?.RegistrationDate.ToString("dd MMMM yyyy"))},
                    {"<<Times>>", GetDefaultValue(data?.RegistrationDate.ToString("H:MM"))},
                    {"<<Date>>", DateTime.Now.ToString("dd MMMM yyyy")},  // Still no null check needed
                    {"<<Genders>>", GetDefaultValue(Gender)},
                    {"<<OppositeSex>>", GetDefaultValue(OppositeSex, "")} // Use empty string if null
                };

                if (patienss.IsEmployee == false)
                {
                    DocumentContent = await DocumentProvider.GetDocumentAsync("SuratIzin.docx", mergeFields);
                }
                else if (patienss.IsEmployee == true)
                {
                    DocumentContent = await DocumentProvider.GetDocumentAsync("Employee.docx", mergeFields);
                }
                // Konversi byte array menjadi base64 string
                //var base64String = Convert.ToBase64String(DocumentContent);

                //// Panggil JavaScript untuk membuka dan mencetak dokumen
                //await JsRuntime.InvokeVoidAsync("printDocument", base64String);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SelectedItemSpesialis(SpesialisPCare e)
        {
            if (e is null)
            {
                GeneralConsultanService.ReferVerticalSpesialisParentSpesialisCode = null;
                GeneralConsultanService.ReferVerticalSpesialisParentSubSpesialisCode = null;
                return;
            }

            GeneralConsultanService.ReferVerticalSpesialisParentSpesialisCode = e.KdSpesialis;

            await SendPcareGetSubSpesialis();
        }

        private void SelectedFaskesRujuk(RujukanFaskesKhususSpesialisPCare e)
        {
            try
            {
                if (e is null)
                {
                    IsReferTo = false;
                    return;
                }

                var rujuk = RujukanSubSpesialis.FirstOrDefault(x => x.Kdppk == e.Kdppk);

                if (rujuk is not null)
                {
                    GeneralConsultanService.PPKRujukanCode = rujuk.Kdppk;
                    GeneralConsultanService.PPKRujukanName = rujuk.Nmppk ?? "-";
                    GeneralConsultanService.ReferVerticalSpesialisParentSpesialisName = SpesialisPs.FirstOrDefault(x => x.KdSpesialis == GeneralConsultanService.ReferVerticalSpesialisParentSpesialisCode)?.NmSpesialis ?? "-";
                    GeneralConsultanService.ReferVerticalSpesialisParentSubSpesialisName = SubSpesialisPs.FirstOrDefault(x => x.KdSubSpesialis == GeneralConsultanService.ReferVerticalSpesialisParentSubSpesialisCode)?.NmSubSpesialis ?? "-";
                    GeneralConsultanService.ReferReason = GeneralConsultanService.ReferReason is null || SelectedRujukanVertical.Equals(RujukanExtenalVertical.ToList()[0]) ? "-" : GeneralConsultanService.ReferReason;
                }
            }
            catch { }

            IsReferTo = false;
        }

        private async Task OnClickSearchFaskes()
        {
            IsLoadingSearchFaskes = true;
            try
            {
                if (!GeneralConsultanService.ReferDateVisit.HasValue)
                {
                    IsLoadingSearchFaskes = false;
                    ToastService.ShowInfo("Please select Visit Date");
                    return;
                }

                if (SelectedRujukanVertical.Equals(RujukanExtenalVertical.ToList()[0])) // Khusus
                {
                    await SendPcareGetFaskesRujukanKhusus();
                }
                else // Spesialis
                {
                    await SendPcareGetFaskesSubSpesialis();
                }
            }
            catch (Exception)
            {
            }
            IsLoadingSearchFaskes = false;
        }

        private async Task GetClinicalAssesmentPatientHistory()
        {
            try
            {
                if (GeneralConsultanService.Height == 0 && GeneralConsultanService.Weight == 0)
                {
                    var prev = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x
                        => x.PatientId == GeneralConsultanService.PatientId && x.Id < GeneralConsultanService.Id && x.Status == EnumStatusGeneralConsultantService.Finished))).Item1
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault() ?? new();

                    GeneralConsultanService.Height = prev?.Height ?? 0;
                    GeneralConsultanService.Weight = prev?.Weight ?? 0;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SendPcareGetFaskesRujukanKhusus()
        {
            if (RujukanSubSpesialis.Count > 0)
                return;

            try
            {
                if (GeneralConsultanService.ReferVerticalKhususCategoryCode is not null && (GeneralConsultanService.ReferVerticalKhususCategoryCode.Equals("THA") || GeneralConsultanService.ReferVerticalKhususCategoryCode.Equals("HEM")))
                {
                    Console.WriteLine("Hit URL: " + JsonConvert.SerializeObject($"spesialis/rujuk/khusus/{GeneralConsultanService.ReferVerticalKhususCategoryCode}/subspesialis/{GeneralConsultanService.ReferVerticalSpesialisParentSubSpesialisCode}/noKartu/{SelectedBPJSIntegration.NoKartu}/tglEstRujuk/{GeneralConsultanService.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", Formatting.Indented));

                    var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"spesialis/rujuk/khusus/{GeneralConsultanService.ReferVerticalKhususCategoryCode}/subspesialis/{GeneralConsultanService.ReferVerticalSpesialisParentSubSpesialisCode}/noKartu/{SelectedBPJSIntegration.NoKartu}/tglEstRujuk/{GeneralConsultanService.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", HttpMethod.Get);

                    if (result.Item2 == 200)
                    {
                        if (result.Item1 is null)
                        {
                            RujukanSubSpesialis.Clear();
                        }
                        else
                        {
                            dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                            var dynamicList = (IEnumerable<dynamic>)data.list;

                            var a = dynamicList.Select(item => new RujukanFaskesKhususSpesialisPCare
                            {
                                Kdppk = item.kdppk,
                                Nmppk = item.nmppk,
                                AlamatPpk = item.alamatPpk,
                                TelpPpk = item.telpPpk,
                                Kelas = item.kelas,
                                Nmkc = item.nmkc,
                                Distance = item.distance,
                                Jadwal = item.jadwal,
                                JmlRujuk = item.jmlRujuk,
                                Kapasitas = item.kapasitas,
                                Persentase = item.persentase,
                            }).ToList();

                            RujukanSubSpesialis.Clear();
                            RujukanSubSpesialis = a;
                        }
                    }
                    else
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                        ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                    }
                }
                else
                {
                    Console.WriteLine($"spesialis/rujuk/khusus/{GeneralConsultanService.ReferVerticalKhususCategoryCode}/noKartu/{SelectedBPJSIntegration.NoKartu}/tglEstRujuk/{GeneralConsultanService.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}");

                    var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"spesialis/rujuk/khusus/{GeneralConsultanService.ReferVerticalKhususCategoryCode}/noKartu/{SelectedBPJSIntegration.NoKartu}/tglEstRujuk/{GeneralConsultanService.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", HttpMethod.Get);

                    if (result.Item2 == 200)
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                        var dynamicList = (IEnumerable<dynamic>)data.list;

                        var a = dynamicList.Select(item => new RujukanFaskesKhususSpesialisPCare
                        {
                            Kdppk = item.kdppk,
                            Nmppk = item.nmppk,
                            AlamatPpk = item.alamatPpk,
                            TelpPpk = item.telpPpk,
                            Kelas = item.kelas,
                            Nmkc = item.nmkc,
                            Distance = item.distance,
                            Jadwal = item.jadwal,
                            JmlRujuk = item.jmlRujuk,
                            Kapasitas = item.kapasitas,
                            Persentase = item.persentase,
                        }).ToList();

                        RujukanSubSpesialis.Clear();
                        RujukanSubSpesialis = a;
                    }
                    else
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                        ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SendPcareGetFaskesSubSpesialis()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(GeneralConsultanService.ReferVerticalSpesialisSaranaCode) || !Convert.ToBoolean(GeneralConsultanService.IsSarana))
                    GeneralConsultanService.ReferVerticalSpesialisSaranaCode = "0";

                var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"spesialis/rujuk/subspesialis/{GeneralConsultanService.ReferVerticalSpesialisParentSubSpesialisCode}/sarana/{GeneralConsultanService.ReferVerticalSpesialisSaranaCode}/tglEstRujuk/{GeneralConsultanService.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", HttpMethod.Get);

                Console.WriteLine("Hit URL: " + JsonConvert.SerializeObject($"spesialis/rujuk/subspesialis/{GeneralConsultanService.ReferVerticalSpesialisParentSubSpesialisCode}/sarana/{GeneralConsultanService.ReferVerticalSpesialisSaranaCode}/tglEstRujuk/{GeneralConsultanService.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", Formatting.Indented));
                if (result.Item2 == 200)
                {
                    if (result.Item1 is null)
                    {
                        RujukanSubSpesialis.Clear();
                    }
                    else
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                        var dynamicList = (IEnumerable<dynamic>)data.list;

                        var a = dynamicList.Select(item => new RujukanFaskesKhususSpesialisPCare
                        {
                            Kdppk = item.kdppk,
                            Nmppk = item.nmppk,
                            AlamatPpk = item.alamatPpk,
                            TelpPpk = item.telpPpk,
                            Kelas = item.kelas,
                            Nmkc = item.nmkc,
                            Distance = item.distance,
                            Jadwal = item.jadwal,
                            JmlRujuk = item.jmlRujuk,
                            Kapasitas = item.kapasitas,
                            Persentase = item.persentase,
                        }).ToList();

                        RujukanSubSpesialis.Clear();
                        RujukanSubSpesialis = a;
                    }
                }
                else
                {
                    RujukanSubSpesialis.Clear();
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                    ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SendPcareGetSubSpesialis()
        {
            try
            {
                var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"spesialis/{GeneralConsultanService.ReferVerticalSpesialisParentSpesialisCode}/subspesialis", HttpMethod.Get);
                if (result.Item2 == 200)
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                    var dynamicList = (IEnumerable<dynamic>)data.list;

                    var a = dynamicList.Select(item => new SubSpesialisPCare
                    {
                        KdSubSpesialis = item.kdSubSpesialis,
                        NmSubSpesialis = item.nmSubSpesialis,
                        KdPoliRujuk = item.kdPoliRujuk,
                    }).ToList();

                    SubSpesialisPs.Clear();
                    SubSpesialisPs = a;
                }
                else
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                    ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];
        private List<AllergyDto> Allergies = [];
        private List<GeneralConsultanServiceDto> GeneralConsultanServices { get; set; } = [];
        private List<GeneralConsultanServiceDto> GeneralConsultanServicesHistoricalMR { get; set; } = [];
        private List<InsurancePolicyDto> InsurancePolicies { get; set; } = [];
        private List<InsurancePolicyDto> ReferToInsurancePolicies { get; set; } = [];
        private List<AwarenessDto> Awareness { get; set; } = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];
        private List<ServiceDto> Services { get; set; } = [];
        private List<ClassTypeDto> ClassTypes = [];
        private List<UserDto> Patients { get; set; } = [];
        private List<UserDto> Physicions { get; set; } = [];
        private List<DiagnosisDto> Diagnoses = [];
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs = [];
        private List<NursingDiagnosesDto> NursingDiagnoses = [];

        private PatientAllergyDto PatientAllergy = new();
        private InsurancePolicyDto SelectedInsurancePolicy { get; set; } = new();

        private DiagnosisDto SelectedDiagnosis { get; set; } = new();
        private NursingDiagnosesDto SelectedNursingDiagnosis { get; set; } = new();
        private InsurancePolicyDto SelectedInsurancePolicyFollowUp { get; set; } = new();
        private InsurancePolicyDto SelectedInsurancePolicyReferTo { get; set; } = new();
        private GeneralConsultanServiceDto GeneralConsultanService { get; set; } = new();
        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport { get; set; } = new();
        private InputCPPTGeneralConsultanCPPT FormInputCPPTGeneralConsultan = new();
        private List<string> Hospitals { get; set; } = new List<string> { "RSBK", "RSE", "RSHB", "RSBP", "RSAB", "RSGH", "RSMA", "RSHBH", "RSSD" };
        private List<string> ExaminationPurposes { get; set; } = new List<string> { "Dentist", "Internist", "Pulmonologist", "Cardiologist", "Eye", "ENT", "Paediatric", "Surgeon", "Obstetrician", "Neurologist", "Urologist", "Neurosurgeon", "Orthopaedic", "Physiotherapist", "Dermatologist", "Psychiatrist", "Laboratorium" };
        private List<string> Categories { get; set; } = new List<string> { "KANKER", "ACCIDENT Inside", "EMPLOYEE", "KELAINAN BAWAAN", "ACCIDENT Outside", "DEPENDENT" };
        private List<string> ExamFor { get; set; } = new List<string> { "Pemeriksaan / penanganan lebih lanjut", "Pembedahan", "Perawatan", "Bersalin" };
        private List<string> InpatientClasses { get; set; } = new List<string> { "VIP Class", "Class 1 B", "Class 2" };

        private class InputCPPTGeneralConsultanCPPT
        {
            public string? Subjective { get; set; }
            public string? Objective { get; set; }

            [DisplayName("Planning")]
            public string? Plan { get; set; }

            public string? Diagnosis { get; set; }

            [DisplayName("Nurse Diagnosis")]
            public string? NursingDiagnosis { get; set; }

            public DateTime Date { get; set; } = DateTime.Now;
        }

        private bool IsStatus(EnumStatusGeneralConsultantService status) => GeneralConsultanService.Status == status;

        private List<string> RegisType = new List<string>
        {
            "General Consultation",
            "Emergency",
            //"MCU"
        };

        private List<string> ClinicVisitTypes = new List<string>
        {
            "Healthy",
            "Sick"
        };

        private List<string> Method = new List<string>
        {
            "MCU",
            "Gas And Oil"
        };

        private List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        #region Methods

        private void SelectedMaternityStartDateChanged(DateTime e)
        {
            GeneralConsultanService.StartMaternityLeave = e;
            GeneralConsultanService.EndMaternityLeave = GeneralConsultanService.StartMaternityLeave.AddMonths(3);
        }

        private BPJSIntegrationDto SelectedBPJSIntegration { get; set; } = new();
        private BPJSIntegrationDto SelectedBPJSIntegrationFollowUp { get; set; } = new();
        private BPJSIntegrationDto SelectedBPJSIntegrationReferTo { get; set; } = new();

        private async Task SelectedItemInsurancePolicyChangedReferTo(InsurancePolicyDto? result)
        {
            ToastService.ClearInfoToasts();
            SelectedInsurancePolicyReferTo = new();
            ReferToGeneralConsultanService.InsurancePolicyId = null;

            SelectedBPJSIntegrationReferTo = new();

            if (result is null)
                return;

            SelectedInsurancePolicyReferTo = result;

            ToastService.ClearWarningToasts();

            var bpjs = (await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == result.Id))).FirstOrDefault();
            if (bpjs is not null)
            {
                var count = GeneralConsultanServices.Where(x => x.PatientId == ReferToGeneralConsultanService.PatientId && x.Status == EnumStatusGeneralConsultantService.Planned).Count();
                if (!string.IsNullOrWhiteSpace(bpjs.KdProviderPstKdProvider))
                {
                    //var parameter = (await Mediator.Send(new GetSystemParameterQuery(x => x.Key.Contains("pcare_code_provider")))).FirstOrDefault();
                    var parameter = (await Mediator.Send(new GetSystemParameterQuery())).FirstOrDefault()?.PCareCodeProvider ?? null;
                    if (parameter is not null)
                    {
                        if (!Convert.ToBoolean(parameter.Equals(bpjs.KdProviderPstKdProvider)))
                        {
                            ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                        }
                        else
                        {
                            SelectedBPJSIntegrationReferTo = bpjs;
                        }
                    }
                }
                else
                {
                    ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                }
            }
        }

        private async Task SelectedItemInsurancePolicyChangedFollowUp(InsurancePolicyDto? result)
        {
            ToastService.ClearInfoToasts();
            SelectedInsurancePolicyFollowUp = new();
            FollowUpGeneralConsultanService.InsurancePolicyId = null;

            SelectedBPJSIntegrationFollowUp = new();

            if (result is null)
                return;

            SelectedInsurancePolicyFollowUp = result;

            ToastService.ClearWarningToasts();

            var bpjs = (await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == result.Id))).FirstOrDefault();
            if (bpjs is not null)
            {
                var count = GeneralConsultanServices.Where(x => x.PatientId == FollowUpGeneralConsultanService.PatientId && x.Status == EnumStatusGeneralConsultantService.Planned).Count();
                if (!string.IsNullOrWhiteSpace(bpjs.KdProviderPstKdProvider))
                {
                    //var parameter = (await Mediator.Send(new GetSystemParameterQuery(x => x.Key.Contains("pcare_code_provider")))).FirstOrDefault();
                    var parameter = (await Mediator.Send(new GetSystemParameterQuery())).FirstOrDefault()?.PCareCodeProvider ?? null;
                    if (parameter is not null)
                    {
                        if (!Convert.ToBoolean(parameter.Equals(bpjs.KdProviderPstKdProvider)))
                        {
                            ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                        }
                        else
                        {
                            SelectedBPJSIntegrationFollowUp = bpjs;
                        }
                    }
                }
                else
                {
                    ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                }
            }
        }

        private async Task SelectedItemInsurancePolicyChanged(InsurancePolicyDto? result)
        {
            ToastService.ClearInfoToasts();
            SelectedBPJSIntegration = new();
            FollowUpGeneralConsultanService.InsurancePolicyId = null;

            SelectedInsurancePolicy = result;
            GeneralConsultanService.InsurancePolicyId = result?.Id ?? null;
            if (result is null)
                return;
            ToastService.ClearWarningToasts();

            var bpjs = (await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == result.Id))).FirstOrDefault();
            if (bpjs is not null)
            {
                var count = GeneralConsultanServices.Where(x => x.PatientId == GeneralConsultanService.PatientId && x.Status == EnumStatusGeneralConsultantService.Planned).Count();
                if (!string.IsNullOrWhiteSpace(bpjs.KdProviderPstKdProvider))
                {
                    //var parameter = (await Mediator.Send(new GetSystemParameterQuery(x => x.Key.Contains("pcare_code_provider")))).FirstOrDefault();
                    var parameter = (await Mediator.Send(new GetSystemParameterQuery())).FirstOrDefault()?.PCareCodeProvider ?? null;

                    if (parameter is not null)
                    {
                        if (!Convert.ToBoolean(parameter.Equals(bpjs.KdProviderPstKdProvider)))
                        {
                            ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                        }
                        else
                        {
                            SelectedBPJSIntegration = bpjs;
                        }
                    }
                }
                else
                {
                    ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                }
            }
        }

        private async Task SelectedItemServiceChangedReferTO(ServiceDto e)
        {
            ReferToGeneralConsultanService.ServiceId = null;

            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                ReferToGeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
            }
            else
            {
                Physicions.Clear();

                if (e is null)
                {
                    ReferToGeneralConsultanService.PratitionerId = null;
                    return;
                }

                Physicions = await Mediator.Send(new GetUserQuery(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(e.Id)));
            }
        }

        private async Task SelectedItemServiceChangedFollowUp(ServiceDto e)
        {
            FollowUpGeneralConsultanService.ServiceId = null;

            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                FollowUpGeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
            }
            else
            {
                Physicions.Clear();

                if (e is null)
                {
                    FollowUpGeneralConsultanService.PratitionerId = null;
                    return;
                }

                Physicions = await Mediator.Send(new GetUserQuery(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(e.Id)));
            }
        }

        private void OnDeleteTabCPPTConfirm(GridDataItemDeletingEventArgs e)
        {
            GeneralConsultanCPPTs.Remove((GeneralConsultanCPPTDto)e.DataItem);
            GridCppt.Reload();
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

        private async Task OnClickSaveCPPT()
        {
            try
            {
                await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(deleteByGeneralServiceId: GeneralConsultanService.Id));

                GeneralConsultanCPPTs.ForEach(x => { x.GeneralConsultanService = null; x.GeneralConsultanServiceId = GeneralConsultanService.Id; x.Id = 0; });
                GeneralConsultanCPPTs = await Mediator.Send(new CreateListGeneralConsultanCPPTRequest(GeneralConsultanCPPTs));

                ToastService.ShowSuccess("Successfully Saving CPPT");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private bool _hasNavigated = false;

        protected override async Task OnInitializedAsync()
        {
            //if (_hasNavigated)
            //{
            //    return;
            //}

            //var uri = new Uri(NavigationManager.Uri);
            //var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            //if (queryDictionary.TryGetValue("id", out var genSetValue))
            //{
            //    var a = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == genSetValue.ToString().ToLong()))).Item1.FirstOrDefault();
            //    if (a is not null)
            //    {
            //        _hasNavigated = true; // Set flag to prevent looping
            //        var targetUrl = NavigationManager.ToAbsoluteUri("/clinic-service/general-consultation-services");
            //        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(targetUrl.ToString(), "id", genSetValue.ToString());
            //        NavigationManager.NavigateTo(query);
            //        await EditItemVoid(true);
            //    }
            //    else
            //    {
            //        ShowForm = false;
            //        GeneralConsultanService = new();
            //    }
            //}
            //else
            //{
            //    ShowForm = false;
            //    GeneralConsultanService = new();
            //}
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
                        Body = (value is null ? "" : value.ToString()) ?? "",
                    });

                    if (title.Equals("Planning"))
                    {
                        if (IsStatus(EnumStatusGeneralConsultantService.NurseStation))
                        {
                            temps.Add(new GeneralConsultanCPPTDto
                            {
                                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                                Title = "Nursing Diagnosis",
                                Body = SelectedNursingDiagnosis.Problem,
                            });
                        }
                        else
                        {
                            temps.Add(new GeneralConsultanCPPTDto
                            {
                                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                                Title = "Diagnosis",
                                Body = SelectedDiagnosis.Name,
                            });
                        }
                    }
                }
            }

            GeneralConsultanCPPTs.AddRange(temps);

            GridCppt.Reload();
            OnClickClearCPPT();
        }

        private void OnClickGetObjectives()
        {
            FormInputCPPTGeneralConsultan.Objective = $"Weight: {GeneralConsultanService.Weight}, Height: {GeneralConsultanService.Height}, RR: {GeneralConsultanService.RR}, SpO2: {GeneralConsultanService.SpO2}, BMIIndex: {Math.Round(GeneralConsultanService.BMIIndex, 2).ToString()}, BMIState: {GeneralConsultanService.BMIState}, Temp: {GeneralConsultanService.Temp}, HR: {GeneralConsultanService.HR}, Systolic: {GeneralConsultanService.Systolic}, DiastolicBP: {GeneralConsultanService.DiastolicBP}, E: {GeneralConsultanService.E}, V: {GeneralConsultanService.V}, M: {GeneralConsultanService.M}";
        }

        private void OnClickClearAllCPPT()
        {
            GeneralConsultanCPPTs.Clear();
        }

        private void OnClickClearCPPT()
        {
            FormInputCPPTGeneralConsultan = new InputCPPTGeneralConsultanCPPT();
            SelectedDiagnosis = new();
            SelectedNursingDiagnosis = new();
        }

        private async Task SelectedItemServiceChanged(ServiceDto e)
        {
            GeneralConsultanService.ServiceId = null;

            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                GeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
            }
            else
            {
                Physicions.Clear();

                if (e is null)
                {
                    GeneralConsultanService.PratitionerId = null;
                    return;
                }

                Physicions = await Mediator.Send(new GetUserQuery(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(e.Id)));
            }
        }

        private async Task SelectedItemPaymentChangedReferTo(string e)
        {
            ReferToGeneralConsultanService.Payment = null;
            ReferToGeneralConsultanService.InsurancePolicyId = null;
            SelectedInsurancePolicyFollowUp = new();

            if (e is null)
                return;

            ReferToInsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == ReferToGeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJSKesehatan == e.Equals("BPJS") && x.Active == true));
        }

        private async Task SelectedItemPaymentChangedFollowUp(string e)
        {
            FollowUpGeneralConsultanService.Payment = null;
            FollowUpGeneralConsultanService.InsurancePolicyId = null;
            SelectedInsurancePolicyFollowUp = new();

            if (e is null)
                return;

            FollowUpInsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == FollowUpGeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJSKesehatan == e.Equals("BPJS") && x.Active == true));
        }

        private async Task SelectedItemPaymentChanged(string e)
        {
            GeneralConsultanService.Payment = null;
            GeneralConsultanService.InsurancePolicyId = null;
            SelectedInsurancePolicy = new();

            if (e is null)
                return;

            if (e.Equals("BPJS"))
                InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJSKesehatan == e.Equals("BPJS") && x.Active == true));
            else
                InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && (x.Insurance.IsBPJSKesehatan == false || x.Insurance.IsBPJSTK == false) && x.Active == true));
        }

        private void SelectedItemRegisTypeChangedReferTo(String e)
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
                ReferToGeneralConsultanService.TypeMedical = Method[0];
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
                ReferToGeneralConsultanService.TypeMedical = Method[0];
            }
            else if (e.Equals("General Consultation"))
                ReferToGeneralConsultanService.TypeMedical = null;
        }

        private void SelectedItemRegisTypeChangedFollowUp(String e)
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
                FollowUpGeneralConsultanService.TypeMedical = Method[0];
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
                FollowUpGeneralConsultanService.TypeMedical = Method[0];
            }
            else if (e.Equals("General Consultation"))
                FollowUpGeneralConsultanService.TypeMedical = null;
        }

        private void SelectedItemRegisTypeChanged(String e)
        {
            if (e is null)
            {
                return;
            }

            //if (e.Equals("Emergency"))
            //{
            //    Method =
            //    [
            //        "General",
            //        "Work Related Injury",
            //        "Road Accident Injury",
            //    ];
            //    FollowUpGeneralConsultanService.TypeMedical = Method[0];
            //}
            //else if (e.Equals("MCU"))
            //{
            //    Method =
            //    [
            //        "Annual MCU",
            //        "Pre Employment MCU",
            //        "Oil & Gas UK",
            //        "HIV & AIDS",
            //        "Covid19*",
            //        "Drug & Alcohol Test",
            //        "Maternity Checkup"
            //    ];
            //    FollowUpGeneralConsultanService.TypeMedical = Method[0];
            //}
            //else if (e.Equals("General Consultation"))
            FollowUpGeneralConsultanService.TypeMedical = null;
        }

        private async Task SelectedItemPatientChangedReferTo(UserDto e)
        {
            ReferToGeneralConsultanService.InsurancePolicyId = null;
            ReferToInsurancePolicies.Clear();
            ReferToGeneralConsultanService.Patient = new();
            SelectedInsurancePolicyReferTo = new();

            if (e is null)
                return;

            ReferToGeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == e.Id) ?? new();
            ReferToGeneralConsultanService.PatientId = e.Id;

            ReferToInsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == e.Id && x.Insurance != null && ReferToGeneralConsultanService.Payment != null && x.Insurance.IsBPJSKesehatan == ReferToGeneralConsultanService.Payment.Equals("BPJS") && x.Active == true));
        }

        private List<InsurancePolicyDto> FollowUpInsurancePolicies { get; set; } = [];

        private async Task SelectedItemPatientChangedFollowUp(UserDto e)
        {
            FollowUpGeneralConsultanService.InsurancePolicyId = null;
            FollowUpInsurancePolicies.Clear();
            FollowUpGeneralConsultanService.Patient = new();
            SelectedInsurancePolicyFollowUp = new();

            if (e is null)
                return;

            FollowUpGeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == e.Id) ?? new();
            FollowUpGeneralConsultanService.PatientId = e.Id;

            FollowUpInsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == e.Id && x.Insurance != null && FollowUpGeneralConsultanService.Payment != null && x.Insurance.IsBPJSKesehatan == FollowUpGeneralConsultanService.Payment.Equals("BPJS") && x.Active == true));
        }

        private async Task SelectedItemPatientChanged(UserDto e)
        {
            GeneralConsultanService.InsurancePolicyId = null;
            InsurancePolicies.Clear();
            GeneralConsultanService.Patient = new();
            SelectedInsurancePolicy = new();

            if (e is null)
                return;

            GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == e.Id) ?? new();
            GeneralConsultanService.PatientId = e.Id;
            UserForm = Patients.FirstOrDefault(x => x.Id == e.Id) ?? new();

            InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == e.Id && x.Insurance != null && GeneralConsultanService.Payment != null && x.Insurance.IsBPJSKesehatan == GeneralConsultanService.Payment.Equals("BPJS") && x.Active == true));

            await GetPatientAllergy();
        }

        private async Task GetPatientAllergy()
        {
            FoodAllergies.Clear();
            WeatherAllergies.Clear();
            PharmacologyAllergies.Clear();
            SelectedFoodAllergies = [];
            SelectedWeatherAllergies = [];
            SelectedPharmacologyAllergies = [];

            // Filter allergies by type
            FoodAllergies = Allergies.Where(x => x.Type == "01").ToList();
            WeatherAllergies = Allergies.Where(x => x.Type == "02").ToList();
            PharmacologyAllergies = Allergies.Where(x => x.Type == "03").ToList();

            var p = (await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId))).FirstOrDefault();

            if (p is null || p.PatientAllergyIds is null)
                return;

            var allergies = await Mediator.Send(new GetAllergyQuery(x => p.PatientAllergyIds.Contains(x.Id)));
            if (allergies is not null && allergies.Count > 0)
            {
                var selectedAllergyIds = allergies.Where(x => x.Type == "01" || x.Type == "02" || x.Type == "03").Select(x => x.Id).ToList();

                SelectedFoodAllergies = FoodAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
                SelectedWeatherAllergies = WeatherAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
                SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();

                if (SelectedFoodAllergies.Count() > 0)
                    GeneralConsultanService.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    GeneralConsultanService.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    GeneralConsultanService.IsPharmacology = true;
            }
        }

        private async Task CloseFollowUpClick()
        {
            IsFollowUp = false;
        }

        private GeneralConsultanServiceDto FollowUpGeneralConsultanService { get; set; } = new();
        private GeneralConsultanServiceDto ReferToGeneralConsultanService { get; set; } = new();

        private async Task CloseReferTo()
        {
            IsReferTo = false;
        }

        private async Task SaveAllergyData()
        {
            try
            {
                if (SelectedFoodAllergies.Count() > 0)
                    GeneralConsultanService.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    GeneralConsultanService.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    GeneralConsultanService.IsPharmacology = true;

                //if (SelectedPharmacologyAllergies.Count() > 0 || SelectedWeatherAllergies.Count() > 0 || SelectedFoodAllergies.Count() > 0)
                //{
                var ids = new List<long>();
                ids.AddRange(SelectedPharmacologyAllergies.Select(x => x.Id).ToList());
                ids.AddRange(SelectedWeatherAllergies.Select(x => x.Id).ToList());
                ids.AddRange(SelectedFoodAllergies.Select(x => x.Id).ToList());

                //var u = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId);
                var u = (await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId))).FirstOrDefault();
                if (u is not null)
                {
                    u = UserForm.Adapt<UserDto>();
                    u.PatientAllergyIds = ids;

                    if (u.CurrentMobile != UserForm.CurrentMobile)
                        u.CurrentMobile = UserForm.CurrentMobile;

                    //u.FamilyMedicalHistory = UserForm.FamilyMedicalHistory;

                    await Mediator.Send(new UpdateUserRequest(u));
                    Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
                    Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true || x.IsEmployeeRelation == true));

                    GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();
                }

                // Ga Kepake
                //PatientAllergy.UserId = GeneralConsultanService.PatientId.GetValueOrDefault();

                //if (PatientAllergy.Id == 0)
                //    PatientAllergy = await Mediator.Send(new CreatePatientAllergyRequest(PatientAllergy));
                //else
                //    PatientAllergy = await Mediator.Send(new UpdatePatientAllergyRequest(PatientAllergy));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        public class PendaftaranRequest
        {
            public string kdProviderPeserta { get; set; }
            public string tglDaftar { get; set; }
            public string noKartu { get; set; }
            public string kdPoli { get; set; }
            public object? keluhan { get; set; } = null;
            public bool kunjSakit { get; set; } = true;
            public int sistole { get; set; } = 0;
            public int diastole { get; set; } = 0;
            public int beratBadan { get; set; } = 0;
            public int tinggiBadan { get; set; } = 0;
            public int respRate { get; set; } = 0;
            public int lingkarPerut { get; set; } = 0;
            public int heartRate { get; set; } = 0;
            public int rujukBalik { get; set; } = 0;
            public string kdTkp { get; set; } = "10";
        }

        public class KunjunganRequest
        {
            [JsonProperty("noKunjungan")]
            public object NoKunjungan { get; set; }

            [JsonProperty("noKartu")]
            public string NoKartu { get; set; }

            [JsonProperty("tglDaftar")]
            public string TglDaftar { get; set; }

            [JsonProperty("kdPoli")]
            public object KdPoli { get; set; }

            [JsonProperty("keluhan")]
            public string Keluhan { get; set; }

            [JsonProperty("kdSadar")]
            public string KdSadar { get; set; }

            [JsonProperty("sistole")]
            public int Sistole { get; set; }

            [JsonProperty("diastole")]
            public int Diastole { get; set; }

            [JsonProperty("beratBadan")]
            public int BeratBadan { get; set; }

            [JsonProperty("tinggiBadan")]
            public int TinggiBadan { get; set; }

            [JsonProperty("respRate")]
            public int RespRate { get; set; }

            [JsonProperty("heartRate")]
            public int HeartRate { get; set; }

            [JsonProperty("lingkarPerut")]
            public int LingkarPerut { get; set; }

            [JsonProperty("kdStatusPulang")]
            public string KdStatusPulang { get; set; }

            [JsonProperty("tglPulang")]
            public string TglPulang { get; set; }

            [JsonProperty("kdDokter")]
            public string KdDokter { get; set; }

            [JsonProperty("kdDiag1")]
            public string KdDiag1 { get; set; }

            [JsonProperty("kdDiag2")]
            public object KdDiag2 { get; set; }

            [JsonProperty("kdDiag3")]
            public object KdDiag3 { get; set; }

            [JsonProperty("kdPoliRujukInternal")]
            public object? KdPoliRujukInternal { get; set; } = null;

            [JsonProperty("rujukLanjut")]
            public RujukLanjut? RujukLanjut { get; set; } = null;

            [JsonProperty("kdTacc")]
            public int KdTacc { get; set; }

            [JsonProperty("alasanTacc")]
            public object? AlasanTacc { get; set; } = null;

            [JsonProperty("anamnesa")]
            public string? Anamnesa { get; set; } = null;

            [JsonProperty("alergiMakan")]
            public string AlergiMakan { get; set; } = "00";

            [JsonProperty("alergiUdara")]
            public string AlergiUdara { get; set; } = "00";

            [JsonProperty("alergiObat")]
            public string AlergiObat { get; set; } = "00";

            [JsonProperty("kdPrognosa")]
            public string KdPrognosa { get; set; } = "01";

            [JsonProperty("terapiObat")]
            public string TerapiObat { get; set; } = null;

            [JsonProperty("terapiNonObat")]
            public string TerapiNonObat { get; set; } = null;

            [JsonProperty("bmhp")]
            public string Bmhp { get; set; } = null;

            [JsonProperty("suhu")]
            public string Suhu { get; set; }
        }

        public class RujukLanjut
        {
            [JsonProperty("tglEstRujuk")]
            public string TglEstRujuk { get; set; }

            [JsonProperty("kdppk")]
            public string Kdppk { get; set; }

            [JsonProperty("subSpesialis")]
            public object SubSpesialis { get; set; }

            [JsonProperty("khusus")]
            public Khusus Khusus { get; set; }
        }

        public class Khusus
        {
            [JsonProperty("kdKhusus")]
            public string KdKhusus { get; set; }

            [JsonProperty("kdSubSpesialis")]
            public object KdSubSpesialis { get; set; }

            [JsonProperty("catatan")]
            public string Catatan { get; set; }
        }

        private async Task<bool> SendPcareRequestRegistration()
        {
            if (GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned) && GeneralConsultanService.Payment is not null && GeneralConsultanService.Payment.Equals("BPJS") && SelectedBPJSIntegration is not null)
            {
                var regis = new PendaftaranRequest
                {
                    kdProviderPeserta = SelectedBPJSIntegration.KdProviderPstKdProvider ?? "",
                    tglDaftar = GeneralConsultanService.RegistrationDate.ToString("dd-MM-yyyy"),
                    noKartu = SelectedBPJSIntegration.NoKartu ?? "",
                    kdPoli = Services.FirstOrDefault(x => x.Id == GeneralConsultanService.ServiceId)!.Code,
                    keluhan = null,
                    kunjSakit = true,
                    kdTkp = "10"
                };

                Console.WriteLine("Sending pendaftaran...");
                var responseApi = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"pendaftaran", HttpMethod.Post, regis);

                dynamic data = JsonConvert.DeserializeObject<dynamic>(responseApi.Item1);

                if (responseApi.Item2 != 201)
                {
                    if (responseApi.Item2 == 412)
                        ToastService.ShowError($"{data.message}\n Code: {responseApi.Item2}");
                    else
                        ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");

                    Console.WriteLine(JsonConvert.SerializeObject(regis, Formatting.Indented));

                    IsLoading = false;
                    return false;
                }
                else
                    GeneralConsultanService.SerialNo = data.message;
            }
            return true;
        }

        private async Task<bool> SendPcareRequestKunjungan()
        {
            if (GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician) && GeneralConsultanService.Payment is not null && GeneralConsultanService.Payment.Equals("BPJS") && SelectedBPJSIntegration is not null)
            {
                var ll = GeneralConsultanCPPTs.Where(x => x.Title == "Diagnosis").Select(x => x.Body).ToList();

                string diag1 = null!;

                if (GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.NurseStation))
                {
                    if (GeneralConsultanCPPTs.Count > 0)
                    {
                        var g = GeneralConsultanCPPTs.LastOrDefault(x => x.Title.Equals("Diagnosis"));
                        if (g is not null)
                        {
                            diag1 = NursingDiagnoses.FirstOrDefault(x => x.Problem.Equals(g.Body))!.Code ?? null!;
                        }
                    }
                }

                if (GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician))
                {
                    if (GeneralConsultanCPPTs.Count > 0)
                    {
                        var g = GeneralConsultanCPPTs.LastOrDefault(x => x.Title.Equals("Diagnosis"));
                        if (g is not null)
                        {
                            diag1 = Diagnoses.FirstOrDefault(x => x.Name.Equals(g.Body))!.Code ?? null!;
                        }
                    }
                }

                var kunj = new KunjunganRequest
                {
                    Sistole = GeneralConsultanService.Systolic.ToInt32(),
                    AlergiMakan = SelectedFoodAllergies.FirstOrDefault()?.KdAllergy ?? null,
                    AlergiObat = SelectedPharmacologyAllergies.FirstOrDefault()?.KdAllergy ?? null,
                    AlergiUdara = SelectedWeatherAllergies.FirstOrDefault()?.KdAllergy ?? null,
                    NoKunjungan = GeneralConsultanService.SerialNo ?? string.Empty,
                    NoKartu = SelectedBPJSIntegration.NoKartu ?? "",
                    TglDaftar = GeneralConsultanService.RegistrationDate.ToString("dd-MM-yyyy"),
                    KdPoli = Services.FirstOrDefault(x => x.Id == GeneralConsultanService.ServiceId)!.Code,
                    KdSadar = Awareness.FirstOrDefault(x => x.Id == GeneralConsultanService.AwarenessId)!.KdSadar,
                    Diastole = GeneralConsultanService.DiastolicBP.ToInt32(),
                    BeratBadan = GeneralConsultanService.Weight.ToInt32(),
                    TinggiBadan = GeneralConsultanService.Height.ToInt32(),
                    RespRate = GeneralConsultanService.RR.ToInt32(),
                    HeartRate = GeneralConsultanService.HR.ToInt32(),
                    LingkarPerut = GeneralConsultanService.WaistCircumference.ToInt32(),
                    KdStatusPulang = "4",
                    TglPulang = GeneralConsultanService.RegistrationDate.ToString("dd-MM-yyyy"),
                    KdDokter = Physicions.FirstOrDefault(x => x.Id == GeneralConsultanService.PratitionerId)!.PhysicanCode,
                    KdDiag1 = diag1,
                    KdDiag2 = null,
                    KdDiag3 = null,
                    Suhu = GeneralConsultanService.Temp.ToString(),
                };

                Console.WriteLine(JsonConvert.SerializeObject(kunj, Formatting.Indented));

                var responseApi = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"kunjungan", HttpMethod.Post, kunj);

                if (responseApi.Item2 != 200)
                {
                    ToastService.ShowError($"{responseApi.Item1}");

                    IsLoading = false;
                    return false;
                }
                else
                {
                    if (responseApi.Item1 is not null)
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(responseApi.Item1);
                        if (!string.IsNullOrWhiteSpace(GeneralConsultanService.SerialNo)) // Check if the serial no is not getting from kiosk
                            GeneralConsultanService.SerialNo = data.response.message;
                    }
                }
            }

            return true;
        }

        private async Task<bool> SendPCareRequestUpdateStatusPanggilAntrean(int status)
        {
            try
            {
                var service = Services.FirstOrDefault(x => x.Id == GeneralConsultanService.ServiceId);

                var antreanRequest = new UpdateStatusPanggilAntreanRequestPCare
                {
                    Tanggalperiksa = DateTime.Now.ToString("yyyy-MM-dd"),
                    Kodepoli = service!.Code ?? string.Empty,
                    Nomorkartu = SelectedBPJSIntegration.NoKartu ?? string.Empty,
                    Status = status, // 1 -> Hadir, 2 -> Tidak Hadir
                    Waktu = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                };

                Console.WriteLine("Sending antrean/panggil...");
                var responseApi = await PcareService.SendPCareService(nameof(SystemParameter.AntreanFKTPBaseURL), $"antrean/panggil", HttpMethod.Post, antreanRequest);

                if (responseApi.Item2 != 200)
                {
                    ToastService.ShowError($"{responseApi.Item1}, Code: {responseApi.Item2}");
                    Console.WriteLine(JsonConvert.SerializeObject(antreanRequest, Formatting.Indented));
                    Console.WriteLine("ResponseAPI antrean/panggil " + Convert.ToString(responseApi.Item1));
                    IsLoading = false;
                    return false;
                }
                else
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(responseApi.Item1);
                    if (data is not null)
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

        private async Task OnClickConfirm()
        {
            IsLoading = true;

            try
            {
                if (!GeneralConsultanService.Payment!.Equals("Personal") && (SelectedInsurancePolicy is null || SelectedInsurancePolicy.Id == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                if (SelectedBPJSIntegration is not null && SelectedBPJSIntegration.Id != 0 && GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned))
                {
                    var isSuccess = await SendPcareRequestRegistration();
                    if (!isSuccess)
                    {
                        IsLoading = false;
                        return;
                    }
                    else
                    {
                        await SendPCareRequestUpdateStatusPanggilAntrean(1);
                    }
                }

                if (SelectedBPJSIntegration is not null && SelectedBPJSIntegration.Id != 0 && GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician))
                {
                    var isSuccessAddKunjungan = await SendPcareRequestKunjungan();

                    if (!isSuccessAddKunjungan)
                    {
                        IsLoading = false;
                        return;
                    }
                }

                GeneralConsultanService.InsurancePolicyId = SelectedInsurancePolicy == null || SelectedInsurancePolicy.Id == 0 ? null : SelectedInsurancePolicy.Id;

                if (IsStatus(EnumStatusGeneralConsultantService.Planned) || IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    var usr = (await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId))).FirstOrDefault();
                    if (usr is not null)
                    {
                        if (usr.CurrentMobile != UserForm.CurrentMobile)
                        {
                            usr.CurrentMobile = UserForm.CurrentMobile;
                            await Mediator.Send(new UpdateUserRequest(usr));
                            Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
                            Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true || x.IsEmployeeRelation == true));
                        }
                    }
                }

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:
                        var patient = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id != GeneralConsultanService.Id && x.ServiceId == GeneralConsultanService.ServiceId && x.PatientId == GeneralConsultanService.PatientId && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned) && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date))).Item1;

                        if (patient.Count > 0)
                        {
                            IsLoading = false;
                            ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                            return;
                        }

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Confirmed;

                        if (GeneralConsultanService.Id == 0)
                            GeneralConsultanService = await Mediator.Send(new CreateGeneralConsultanServiceRequest(GeneralConsultanService));
                        else
                            GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                        GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();

                        await SaveAllergyData();

                        StagingText = EnumStatusGeneralConsultantService.NurseStation;

                        break;

                    case EnumStatusGeneralConsultantService.Confirmed:

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.NurseStation;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        StagingText = EnumStatusGeneralConsultantService.Waiting;

                        break;

                    case EnumStatusGeneralConsultantService.NurseStation:

                        await SaveAllergyData();

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Waiting;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        StagingText = EnumStatusGeneralConsultantService.Physician;

                        break;

                    case EnumStatusGeneralConsultantService.Waiting:

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Physician;
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        StagingText = EnumStatusGeneralConsultantService.Finished;

                        break;

                    case EnumStatusGeneralConsultantService.Physician:

                        await SaveAllergyData();

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Finished;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        StagingText = EnumStatusGeneralConsultantService.Finished;

                        if (GeneralConsultanService.IsSickLeave == true || GeneralConsultanService.IsMaternityLeave == true)
                        {
                            var checkDataSickLeave = await Mediator.Send(new GetSickLeaveQuery(x => x.GeneralConsultansId == GeneralConsultanService.Id));
                            if (checkDataSickLeave != null && checkDataSickLeave.Count == 0)
                            {
                                if (GeneralConsultanService.IsSickLeave == true)
                                    SickLeaves.TypeLeave = "SickLeave";
                                else if (GeneralConsultanService.IsMaternityLeave == true)
                                    SickLeaves.TypeLeave = "Maternity";

                                SickLeaves.GeneralConsultansId = GeneralConsultanService.Id;
                                await Mediator.Send(new CreateSickLeaveRequest(SickLeaves));
                            }
                        }

                        break;

                    case EnumStatusGeneralConsultantService.ProcedureRoom:
                        break;

                    case EnumStatusGeneralConsultantService.Canceled:
                        break;

                    default:
                        break;
                }

                GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();

                if (SelectedFoodAllergies.Count() > 0)
                    GeneralConsultanService.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    GeneralConsultanService.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    GeneralConsultanService.IsPharmacology = true;
            }
            catch (Exception x)
            {
                x.HandleException(ToastService);
            }

            IsLoading = false;
        }

        private async Task OnCancelStatus()
        {
            try
            {
                IsLoading = true;

                if (GeneralConsultanService.Id != 0)
                {
                    if (SelectedBPJSIntegration is not null && SelectedBPJSIntegration.Id != 0)
                    {
                        var isSuccess = await SendPCareRequestUpdateStatusPanggilAntrean(2);
                        if (!isSuccess)
                        {
                            IsLoading = false;
                            return;
                        }
                    }

                    GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Canceled;
                    GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                    GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();
                    StagingText = EnumStatusGeneralConsultantService.Canceled;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        private async Task OnClickPopUpPopUpProcedureRoom()
        {
            var targetUrl = NavigationManager.ToAbsoluteUri("/clinic-service/procedure-room");
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(targetUrl.ToString(), "genserv", GeneralConsultanService.Id.ToString());
            NavigationManager.NavigateTo(query);
        }

        private void OnClickReferralPrescriptionConcoction()
        {
            NavigationManager.NavigateTo($"/pharmacy/prescription/{GeneralConsultanService.Id}");
        }

        private void OnClickPainScalePopUp()
        {
            IsPopUpPainScale = true;
        }

        private async Task OnClickPopUpAppoimentPending()
        {
            IsAppoimentPendingAlert = true;
        }

        private async Task OnPrintDocumentMedical()
        {
            var IdEncrypt = SecureHelper.EncryptIdToBase64(GeneralConsultanService.Id);
            NavigationManager.NavigateTo($"/transaction/print-document-medical/{IdEncrypt}");
        }

        private bool IsHistoricalRecordPatientDetailGC { get; set; } = false;
        private bool IsHistoricalRecordPatientDetailAccident { get; set; } = false;
        private GeneralConsultanServiceDto SelectedDetailHistorical { get; set; } = new();
        private List<GeneralConsultanCPPTDto> SelectedDetailHistoricalGeneralConsultanCPPTs { get; set; } = new();
        private AccidentDto SelectedAccidentHistorical { get; set; } = new();

        private async Task OnClickDetailHistoricalRecordPatient(GeneralConsultanServiceDto e)
        {
            IsHistoricalRecordPatientDetailGC = false;
            IsHistoricalRecordPatientDetailAccident = false;
            SelectedDetailHistorical = new();

            if (e is null)
                return;
            SelectedDetailHistorical = e;

            SelectedDetailHistoricalGeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));

            if (e.TypeRegistration == "General Consultation" || e.TypeRegistration == "Emergency")
            {
                IsHistoricalRecordPatientDetailGC = true;
            }
            else if (e.TypeRegistration == "Accident")
            {
                SelectedAccidentHistorical = (await Mediator.Send(new GetAccidentQuery(x => x.GeneralConsultanServiceId == e.Id))).FirstOrDefault() ?? new();
                IsHistoricalRecordPatientDetailAccident = true;
            }
        }

        private bool IsLoadingHistoricalRecordPatient { get; set; } = false;

        private async Task OnClickPopUpHistoricalMedical()
        {
            IsLoadingHistoricalRecordPatient = true;
            if (GeneralConsultanService.PatientId is null || GeneralConsultanService.PatientId == 0)
            {
                ToastService.ShowInfo("Please select the Patient");
            }
            else
            {
                IsHistoricalRecordPatient = true;
                IsLoadingHistoricalRecordPatient = true;
                GeneralConsultanServicesHistoricalMR = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.PatientId == GeneralConsultanService.PatientId))).Item1.OrderBy(x => x.RegistrationDate).ToList();
                IsLoadingHistoricalRecordPatient = false;
            }
            IsLoadingHistoricalRecordPatient = false;
        }

        private void OnAppoimentPopUpClick()
        {
            FollowUpGeneralConsultanService = new();
            FollowUpGeneralConsultanService = GeneralConsultanService.Adapt<GeneralConsultanServiceDto>();
            FollowUpGeneralConsultanService.Id = 0;
            IsFollowUp = true;
        }

        private async Task OnReferToClick()
        {
            IsReferTo = true;

            ReferToGeneralConsultanService = GeneralConsultanService.Adapt<GeneralConsultanServiceDto>();
            ReferToGeneralConsultanService.Id = 0;
            await SendPCareGetRefrensiKhusus();
            await SendPcareGetSpesialis();
            await SendPcareGetSpesialisSarana();
        }

        private async Task SendPcareGetSpesialis()
        {
            if (SpesialisPs.Count > 0)
                return;

            try
            {
                string cacheKey = $"spesialis";
                if (!MemoryCache.TryGetValue(cacheKey, out List<SpesialisPCare>? r))
                {
                    var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"spesialis", HttpMethod.Get);
                    if (result.Item2 == 200)
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                        var dynamicList = (IEnumerable<dynamic>)data.list;

                        r = dynamicList.Select(item => new SpesialisPCare
                        {
                            KdSpesialis = item.kdSpesialis,
                            NmSpesialis = item.nmSpesialis
                        }).ToList();

                        MemoryCache.Set(cacheKey, r, TimeSpan.FromMinutes(15));
                    }
                    else
                    {
                        MemoryCache.Remove(cacheKey);
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);
                        ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                    }
                }

                SpesialisPs = r ?? [];
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SendPcareGetSpesialisSarana()
        {
            if (SpesialisSaranas.Count > 0)
                return;

            try
            {
                string cacheKey = $"spesialis/sarana";
                if (!MemoryCache.TryGetValue(cacheKey, out List<SpesialisSaranaPCare>? r))
                {
                    var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"spesialis/sarana", HttpMethod.Get);
                    if (result.Item2 == 200)
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                        var dynamicList = (IEnumerable<dynamic>)data.list;

                        r = dynamicList.Select(item => new SpesialisSaranaPCare
                        {
                            KdSarana = item.kdSarana,
                            NmSarana = item.nmSarana
                        }).ToList();

                        MemoryCache.Set(cacheKey, r, TimeSpan.FromMinutes(15));
                    }
                    else
                    {
                        MemoryCache.Remove(cacheKey);
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);
                        ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                    }
                }

                SpesialisSaranas = r ?? [];
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SendPCareGetRefrensiKhusus()
        {
            if (SpesialisRefrensiKhusus.Count > 0)
                return;

            try
            {
                string cacheKey = $"spesialis/khusus";
                if (!MemoryCache.TryGetValue(cacheKey, out List<SpesialisRefrensiKhususPCare>? r))
                {
                    var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"spesialis/khusus", HttpMethod.Get);
                    if (result.Item2 == 200)
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                        var dynamicList = (IEnumerable<dynamic>)data.list;

                        r = dynamicList.Select(item => new SpesialisRefrensiKhususPCare
                        {
                            KdKhusus = item.kdKhusus,
                            NmKhusus = item.nmKhusus
                        }).ToList();
                        MemoryCache.Set(cacheKey, r, TimeSpan.FromMinutes(15));
                    }
                    else
                    {
                        MemoryCache.Remove(cacheKey);
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);
                        ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                    }
                }

                SpesialisRefrensiKhusus = r ?? [];
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private bool IsLoadingFollowUp { get; set; } = false;
        private bool IsLoadingReferTo { get; set; } = false;

        private async Task HandleValidSubmitReferTo()
        {
            IsLoadingReferTo = true;
            try
            {
                if (!ReferToGeneralConsultanService.Payment!.Equals("Personal") && (SelectedInsurancePolicyReferTo is null || SelectedInsurancePolicyReferTo.Id == 0))
                {
                    IsLoadingFollowUp = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                var patient = ((await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id != ReferToGeneralConsultanService.Id && x.ServiceId == ReferToGeneralConsultanService.ServiceId && x.PatientId == ReferToGeneralConsultanService.PatientId && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned) && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date))).Item1);

                if (patient.Count > 0)
                {
                    IsLoadingReferTo = false;
                    ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                    return;
                }

                ReferToGeneralConsultanService.Status = EnumStatusGeneralConsultantService.Planned;

                if (ReferToGeneralConsultanService.Id == 0)
                    await Mediator.Send(new CreateGeneralConsultanServiceRequest(ReferToGeneralConsultanService));

                ToastService.ShowSuccess("Successfully Refer Patient");
                IsReferTo = false;
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
            }
            IsLoadingReferTo = false;
        }

        private async Task HandleValidSubmitFollowUp()
        {
            IsLoadingFollowUp = true;
            try
            {
                GeneralConsultanService.InsurancePolicyId = SelectedInsurancePolicy == null || SelectedInsurancePolicy.Id == 0 ? null : SelectedInsurancePolicy.Id;
                if (!FollowUpGeneralConsultanService.Payment!.Equals("Personal") && (SelectedInsurancePolicyFollowUp is null || SelectedInsurancePolicyFollowUp.Id == 0))
                {
                    IsLoadingFollowUp = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                if (FollowUpGeneralConsultanService.AppointmentDate is null)
                {
                    IsLoadingFollowUp = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                var patient = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id != FollowUpGeneralConsultanService.Id && x.ServiceId == FollowUpGeneralConsultanService.ServiceId && x.PatientId == FollowUpGeneralConsultanService.PatientId && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned) && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date))).Item1;

                if (patient.Count > 0)
                {
                    IsLoadingFollowUp = false;
                    ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                    return;
                }

                FollowUpGeneralConsultanService.Status = EnumStatusGeneralConsultantService.Planned;

                if (FollowUpGeneralConsultanService.Id == 0)
                    await Mediator.Send(new CreateGeneralConsultanServiceRequest(FollowUpGeneralConsultanService));

                ToastService.ShowSuccess("Successfully Follow Up Patient");
                IsFollowUp = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoadingFollowUp = false;
        }

        private async Task HandleValidSubmit()
        {
            IsLoading = true;

            try
            {
                // Execute the validator
                ValidationResult results = new GeneralConsultanServiceValidator().Validate(GeneralConsultanService);

                // Inspect any validation failures.
                bool success = results.IsValid;
                List<ValidationFailure> failures = results.Errors;

                ToastService.ClearInfoToasts();
                if (!success)
                {
                    foreach (var f in failures)
                    {
                        ToastService.ShowInfo(f.ErrorMessage);
                    }
                }

                // Execute the validator
                ValidationResult results2 = new GCGUserFormValidator().Validate(UserForm);

                // Inspect any validation failures.
                bool success2 = results2.IsValid;
                List<ValidationFailure> failures2 = results2.Errors;

                ToastService.ClearInfoToasts();
                if (!success2)
                {
                    foreach (var f in failures2)
                    {
                        ToastService.ShowInfo(f.ErrorMessage);
                    }
                }

                if (!success2 || !success)
                    return;

                if (!GeneralConsultanService.Payment!.Equals("Personal") && (SelectedInsurancePolicy is null || SelectedInsurancePolicy.Id == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                GeneralConsultanService.InsurancePolicyId = SelectedInsurancePolicy == null || SelectedInsurancePolicy.Id == 0 ? null : SelectedInsurancePolicy.Id;

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:

                        var patient = ((await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id != GeneralConsultanService.Id && x.ServiceId == GeneralConsultanService.ServiceId && x.PatientId == GeneralConsultanService.PatientId && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned) && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date))).Item1);

                        if (patient.Count > 0)
                        {
                            IsLoading = false;
                            ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                            return;
                        }

                        if (GeneralConsultanService.Id == 0)
                            GeneralConsultanService = await Mediator.Send(new CreateGeneralConsultanServiceRequest(GeneralConsultanService));
                        else
                            GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                        if (SelectedFoodAllergies.Count() > 0)
                            GeneralConsultanService.IsFood = true;
                        if (SelectedWeatherAllergies.Count() > 0)
                            GeneralConsultanService.IsWeather = true;
                        if (SelectedPharmacologyAllergies.Count() > 0)
                            GeneralConsultanService.IsPharmacology = true;

                        if (GeneralConsultanService.IsPharmacology || GeneralConsultanService.IsFood || GeneralConsultanService.IsWeather)
                        {
                            await SaveAllergyData();
                            //var ids = new List<long>();
                            //ids.AddRange(SelectedPharmacologyAllergies.Select(x => x.Id).ToList());
                            //ids.AddRange(SelectedWeatherAllergies.Select(x => x.Id).ToList());
                            //ids.AddRange(SelectedFoodAllergies.Select(x => x.Id).ToList());

                            //var u = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId);
                            //if (u is not null)
                            //{
                            //    u.PatientAllergyIds = ids;
                            //    await Mediator.Send(new UpdateUserRequest(u));
                            //}

                            //PatientAllergy.UserId = GeneralConsultanService.PatientId.GetValueOrDefault();

                            //if (PatientAllergy.Id == 0)
                            //    PatientAllergy = await Mediator.Send(new CreatePatientAllergyRequest(PatientAllergy));
                            //else
                            //    PatientAllergy = await Mediator.Send(new UpdatePatientAllergyRequest(PatientAllergy));
                        }

                        IsLoading = false;
                        break;

                    case EnumStatusGeneralConsultantService.Confirmed:
                        break;

                    case EnumStatusGeneralConsultantService.NurseStation:
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        await SaveAllergyData();
                        break;

                    case EnumStatusGeneralConsultantService.Waiting:
                        break;

                    case EnumStatusGeneralConsultantService.Physician:
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                        if (GeneralConsultanService.IsSickLeave == true || GeneralConsultanService.IsMaternityLeave == true)
                        {
                            var checkDataSickLeave = await Mediator.Send(new GetSickLeaveQuery(x => x.GeneralConsultansId == GeneralConsultanService.Id));
                            if (checkDataSickLeave != null && checkDataSickLeave.Count == 0)
                            {
                                if (GeneralConsultanService.IsSickLeave == true)
                                    SickLeaves.TypeLeave = "SickLeave";
                                else if (GeneralConsultanService.IsMaternityLeave == true)
                                    SickLeaves.TypeLeave = "Maternity";

                                SickLeaves.GeneralConsultansId = GeneralConsultanService.Id;
                                await Mediator.Send(new CreateSickLeaveRequest(SickLeaves));
                            }
                        }

                        await SaveAllergyData();
                        break;

                    case EnumStatusGeneralConsultantService.Finished:
                        break;

                    case EnumStatusGeneralConsultantService.Canceled:
                        break;

                    case EnumStatusGeneralConsultantService.ProcedureRoom:
                        break;

                    default:
                        break;
                }
                GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();

                ToastService.ClearSuccessToasts();
                ToastService.ShowSuccess("Saved Successfully!");
            }
            catch (Exception x)
            {
                x.HandleException(ToastService);
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    await GetUserInfo();
                    StateHasChanged();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                await LoadComboBox();
                StateHasChanged();

                try
                {
                    Grid?.SelectRow(0, true);
                }
                catch
                {
                }
            }
        }

        private async Task LoadComboBox()
        {
            Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true || x.IsEmployeeRelation == true));
            //Services = await Mediator.Send(new GetServiceQuery());
            ClassTypes = await Mediator.Send(new GetClassTypeQuery());
            Awareness = await Mediator.Send(new GetAwarenessQuery());
            Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
            Allergies = await Mediator.Send(new GetAllergyQuery());
            Allergies.ForEach(x =>
            {
                var a = Helper._allergyTypes.FirstOrDefault(z => x.Type is not null && z.Code == x.Type);
                if (a is not null)
                    x.TypeString = a.Name;
            });

            var Diagnoses = (await Mediator.Send(new GetDiagnosisQuery())).Item1;
            this.Diagnoses = Diagnoses;
            var NursingDiagnoses = (await Mediator.Send(new GetNursingDiagnosesQuery())).Item1; ;
            this.NursingDiagnoses = NursingDiagnoses;
        }

        private bool IsDashboard { get; set; } = false;

        #region Chart

        public class StatusMcuData
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public List<StatusMcuData> GetStatusMcuCounts(List<GeneralConsultanServiceDto> services)
        {
            var aa = services.GroupBy(s => s.Status)
                            .Select(g => new StatusMcuData
                            {
                                Status = g.Key.GetDisplayName(),
                                Count = g.Count()
                            }).ToList();
            return aa;
        }

        #endregion Chart

        private List<StatusMcuData> statusMcuData = [];

        private void OnCancelBack()
        {
            NavigationManager.NavigateTo("clinic-service/general-consultation-service", true);
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = [];
            ShowForm = false;
            GeneralConsultanServices = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.TypeRegistration == "General Consultation" || x.TypeRegistration == "Emergency"))).Item1;
            statusMcuData = GetStatusMcuCounts(GeneralConsultanServices);
            //if (GeneralConsultanServices.FirstOrDefault() != null)
            //    SelectedDataItems = new List<object> { GeneralConsultanServices.FirstOrDefault() };
            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems == null) return;

                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(((GeneralConsultanServiceDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(ids: SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>()
                                .Where(x => x.Status == EnumStatusGeneralConsultantService.Planned || x.Status == EnumStatusGeneralConsultantService.Canceled)
                                .Select(x => x.Id)
                                .ToList()));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task NewItem_Click()
        {
            ShowForm = true;
            GeneralConsultanService = new();
            StagingText = EnumStatusGeneralConsultantService.Confirmed;

            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                GeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
            }
        }

        private async Task OnRowDoubleClick()
        {
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

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

        private async Task OnPrint()
        {
            if (GeneralConsultanService.Id == 0)
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
                                c.Item().Text($"MedRec: {GeneralConsultanService.Patient?.NoRm}");
                                c.Item().Text($"Patient: {GeneralConsultanService.Patient?.Name}");
                                c.Item().Text($"Identity Number: {GeneralConsultanService.Patient?.NoId}");
                                c.Item().Text($"Reg Type: {GeneralConsultanService.TypeRegistration}");
                                c.Item().Text($"Service: {GeneralConsultanService.Service?.Name}");
                                c.Item().Text($"Physicion: {GeneralConsultanService.Pratitioner?.Name}");
                                c.Item().Text($"Payment: {GeneralConsultanService.Payment}");
                                c.Item().Text($"Registration Date: {GeneralConsultanService.RegistrationDate}");
                                c.Item().Text($"Schedule Time: {GeneralConsultanService.ScheduleTime}");
                            });
                        });
                        //page.Header().Text("Slip Registration").SemiBold().FontSize(30);
                    });
                })
            .GeneratePdf(file);

            await Helper.DownloadFile("Slip_Registration.pdf", HttpContextAccessor, HttpClient, JsRuntime);

            //NavigationManager.NavigateTo(Path.Combine(Directory.GetCurrentDirectory(), @"Slip_Registration.pdf"), forceLoad: true);
        }

        private void DeleteItem_Click()
        {
            Grid?.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task EditItem_Click()
        {
            IsLoading = true;
            ShowForm = true;
            try
            {
                GeneralConsultanService = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();
                UserForm = GeneralConsultanService.Patient ?? new();

                if (GeneralConsultanService.Method is not null && GeneralConsultanService.Method.Equals("BPJS"))
                    InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJSKesehatan == true && x.Active == true));
                else
                    InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && (x.Insurance.IsBPJSKesehatan == false || x.Insurance.IsBPJSTK == false) && x.Active == true));

                //var targetUrl = NavigationManager.ToAbsoluteUri("/clinic-service/general-consultation-services");
                //var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(targetUrl.ToString(), "id", GeneralConsultanService.Id.ToString());
                //NavigationManager.NavigateTo(query);

                await GetPatientAllergy();

                SelectedInsurancePolicy = (await Mediator.Send(new GetInsurancePolicyQuery(x => x.Id == GeneralConsultanService.InsurancePolicyId))).FirstOrDefault() ?? new();

                var bpjs = (await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == SelectedInsurancePolicy.Id))).FirstOrDefault();
                if (bpjs is not null)
                {
                    var count = GeneralConsultanServices.Where(x => x.PatientId == GeneralConsultanService.PatientId && x.Status == EnumStatusGeneralConsultantService.Planned).Count();
                    if (!string.IsNullOrWhiteSpace(bpjs.KdProviderPstKdProvider))
                    {
                        //var parameter = (await Mediator.Send(new GetSystemParameterQuery(x => x.Key.Contains("pcare_code_provider")))).FirstOrDefault();
                        var parameter = (await Mediator.Send(new GetSystemParameterQuery())).FirstOrDefault()?.PCareCodeProvider ?? null;

                        if (parameter is not null)
                        {
                            if (!Convert.ToBoolean(parameter.Equals(bpjs.KdProviderPstKdProvider)))
                            {
                                ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                            }
                            else
                            {
                                SelectedBPJSIntegration = bpjs;
                            }
                        }
                    }
                    else
                    {
                        ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                    }
                }

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:
                        StagingText = EnumStatusGeneralConsultantService.Confirmed;
                        break;

                    case EnumStatusGeneralConsultantService.Confirmed:
                        StagingText = EnumStatusGeneralConsultantService.NurseStation;
                        break;

                    case EnumStatusGeneralConsultantService.NurseStation:
                        StagingText = EnumStatusGeneralConsultantService.Waiting;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                        break;

                    case EnumStatusGeneralConsultantService.Waiting:
                        StagingText = EnumStatusGeneralConsultantService.Physician;
                        break;

                    case EnumStatusGeneralConsultantService.Physician:
                        StagingText = EnumStatusGeneralConsultantService.Finished;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));

                        if (GeneralConsultanService.PratitionerId is null)
                        {
                            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                            {
                                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                                GeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
                            }
                        }
                        break;

                    case EnumStatusGeneralConsultantService.Finished:
                        StagingText = EnumStatusGeneralConsultantService.Finished;
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                        break;

                    case EnumStatusGeneralConsultantService.Canceled:
                        StagingText = EnumStatusGeneralConsultantService.Canceled;
                        break;

                    case EnumStatusGeneralConsultantService.ProcedureRoom:
                        StagingText = EnumStatusGeneralConsultantService.ProcedureRoom;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        private void Grid_FocusedRowChanged1(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;

            try
            {
                if ((GeneralConsultanServiceDto)args.DataItem is null)
                    return;

                IsDeletedConsultantService = ((GeneralConsultanServiceDto)args.DataItem)!.Status!.Equals(EnumStatusGeneralConsultantService.Planned) || ((GeneralConsultanServiceDto)args.DataItem)!.Status!.Equals(EnumStatusGeneralConsultantService.Canceled);
            }
            catch { }
        }

        private async Task EditItemVoid(bool isFromQuery = false)
        {
            try
            {
                IsLoading = true;
                ShowForm = true;

                if (!isFromQuery)
                    GeneralConsultanService = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();

                InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && GeneralConsultanService.Payment != null && x.Insurance.IsBPJSKesehatan == GeneralConsultanService.Payment.Equals("BPJS") && x.Active == true));

                //if (GeneralConsultanService.StagingStatus != "Finished")
                //{
                //    var text = GeneralConsultanService.StagingStatus == "Physician" ? "In Consultation" : GeneralConsultanService.StagingStatus;
                //    if (!string.IsNullOrWhiteSpace(text) && text.Equals("Procedure Room"))
                //    {
                //        StagingText = "Procedure Room";
                //    }
                //    else
                //    {
                //        var index = Stagings.FindIndex(x => x == text);
                //        StagingText = Stagings[index + 1];
                //    }
                //}

                //await GetPatientAllergy();

                //switch (GeneralConsultanService.StagingStatus)
                //{
                //    case "Nurse Station":
                //        var clinical = await Mediator.Send(new GetGeneralConsultanServiceAssesmentQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        AllGeneralConsultanCPPTs = GeneralConsultanCPPTs.Select(item => item).ToList();

                //        if (clinical.Count > 0)
                //            GeneralConsultanService = clinical[0];
                //        break;

                //    case "Procedure Room":
                //        var supportP = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        if (supportP.Count > 0)
                //            GeneralConsultanMedicalSupport = supportP[0];
                //        break;

                //    case "Physician":
                //        var clinicals = await Mediator.Send(new GetGeneralConsultanServiceAssesmentQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        if (clinicals.Count > 0)
                //            GeneralConsultanService = clinicals[0];

                //        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        AllGeneralConsultanCPPTs = GeneralConsultanCPPTs.Select(item => item).ToList();

                //        var support = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        if (support.Count > 0)
                //            GeneralConsultanMedicalSupport = support[0];

                //        SelectedLabTests = LabTestDetails.Where(x => GeneralConsultanMedicalSupport.LabResulLabExaminationtIds != null && GeneralConsultanMedicalSupport.LabResulLabExaminationtIds.Contains(x.Id)).ToList();

                //        if (Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                //        {
                //            IsPratition = [.. AllDoctors.Where(x => x.Id == UserLogin.Id).ToList()];
                //            GeneralConsultanService.PratitionerId = IsPratition.Count > 0 ? IsPratition[0].Id : null;
                //        }
                //        else
                //            IsPratition = [.. AllDoctors.Where(x => x.IsDoctor == true && x.IsPhysicion == true).ToList()];

                //        GeneralConsultanService.StartMaternityLeave = DateTime.Now;
                //        GeneralConsultanService.EndMaternityLeave = DateTime.Now.AddMonths(3);
                //        break;

                //    case "Waiting":
                //        var support1 = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        if (support1.Count > 0)
                //            GeneralConsultanMedicalSupport = support1[0];

                //        var clinicals1 = await Mediator.Send(new GetGeneralConsultanServiceAssesmentQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        if (clinicals1.Count > 0)
                //            GeneralConsultanService = clinicals1[0];

                //        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                //        AllGeneralConsultanCPPTs = GeneralConsultanCPPTs.Select(item => item).ToList();

                //        SelectedLabTests = LabTestDetails.Where(x => GeneralConsultanMedicalSupport.LabResulLabExaminationtIds != null && GeneralConsultanMedicalSupport.LabResulLabExaminationtIds.Contains(x.Id)).ToList();
                //        break;

                //    default:
                //        break;
                //}
            }
            catch (Exception exx)
            {
                exx.HandleException(ToastService);
            }

            IsLoading = false;
        }

        #endregion Methods
    }
}
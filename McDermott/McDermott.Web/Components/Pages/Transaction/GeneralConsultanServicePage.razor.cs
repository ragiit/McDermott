using Microsoft.Extensions.Caching.Memory;
using QuestPDF.Fluent;
using System.ComponentModel;
using System.Text.RegularExpressions;
using static McDermott.Application.Features.Commands.Employee.SickLeaveCommand;
using static McDermott.Web.Components.Pages.Queue.KioskPage;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class GeneralConsultanServicePage
    {
        #region OnSave and Staging

        private bool P { get; set; } = false;
        private string SelectedRujukanType { get; set; }
        private string SelectedRujukanExternal { get; set; }
        private string SelectedRujukanVertical { get; set; }
        private IEnumerable<string> RujukanTypes = new[] { "Rujuk Internal", "Rujukan External" };
        private IEnumerable<string> RujukanExtenalTypes = new[] { "Rujukan Horizontal", "Rujukan Vertical" };
        private IEnumerable<string> RujukanExtenalVertical = new[] { "Kondisi Khusus", "Spesialis" };

        public class SpesialisRefrensiKhususPCare
        {
            [JsonProperty("kdKhusus")]
            public string KdKhusus { get; set; }

            [JsonProperty("nmKhusus")]
            public string NmKhusus { get; set; }
        }

        public class SpesialisPCare
        {
            [JsonProperty("kdSpesialis")]
            public string KdSpesialis { get; set; }

            [JsonProperty("nmSpesialis")]
            public string NmSpesialis { get; set; }
        }

        public class SpesialisSaranaPCare
        {
            [JsonProperty("kdSarana")]
            public string KdSarana { get; set; }

            [JsonProperty("nmSarana")]
            public string NmSarana { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class RujukanFaskesKhususSpesialisPCare
        {
            [JsonProperty("kdppk")]
            public string Kdppk { get; set; }

            [JsonProperty("nmppk")]
            public string Nmppk { get; set; }

            [JsonProperty("alamatPpk")]
            public string AlamatPpk { get; set; }

            [JsonProperty("telpPpk")]
            public string TelpPpk { get; set; }

            [JsonProperty("kelas")]
            public string Kelas { get; set; }

            [JsonProperty("nmkc")]
            public string Nmkc { get; set; }

            [JsonProperty("distance")]
            public double Distance { get; set; }

            [JsonProperty("jadwal")]
            public string Jadwal { get; set; }

            [JsonProperty("jmlRujuk")]
            public int JmlRujuk { get; set; }

            [JsonProperty("kapasitas")]
            public int Kapasitas { get; set; }

            [JsonProperty("persentase")]
            public int Persentase { get; set; }
        }

        public class SubSpesialisPCare
        {
            [JsonProperty("kdSubSpesialis")]
            public string KdSubSpesialis { get; set; }

            [JsonProperty("nmSubSpesialis")]
            public string NmSubSpesialis { get; set; }

            [JsonProperty("kdPoliRujuk")]
            public string KdPoliRujuk { get; set; }
        }

        private IGrid GridRujukanRefer { get; set; }
        private List<SpesialisRefrensiKhususPCare> SpesialisRefrensiKhusus = [];
        private List<SpesialisPCare> SpesialisPs = [];

        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];

        private List<RujukanFaskesKhususSpesialisPCare> RujukanSubSpesialis = [];
        private List<SpesialisSaranaPCare> SpesialisSaranas = [];
        private List<SubSpesialisPCare> SubSpesialisPs = [];

        private async Task OnClickConfirm()
        {
            try
            {
                IsLoading = true;
                var currentStatus = FormRegis.StagingStatus;
                ToastService.ClearInfoToasts();
                if (FormRegis.PatientId == null || FormRegis.TypeRegistration == null || FormRegis.ServiceId is null || (!FormRegis.Payment!.Equals("Personal") && (FormRegis.InsurancePolicyId == 0 || FormRegis.InsurancePolicyId is null)))
                {
                    IsLoading = false;
                    ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                    return;
                }

                if (SelectedBPJSIntegration is not null && SelectedBPJSIntegration.Id != 0 && FormRegis.StagingStatus is not null && FormRegis.StagingStatus.Equals("Planned"))
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

                if (SelectedBPJSIntegration is not null && SelectedBPJSIntegration.Id != 0 && FormRegis.StagingStatus is not null && FormRegis.StagingStatus.Equals("Physician"))
                {
                    var isSuccessAddKunjungan = await SendPcareRequestKunjungan();

                    if (!isSuccessAddKunjungan)
                    {
                        IsLoading = false;
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
                    await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));
                }
                else
                {
                    var patient = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.PatientId == FormRegis.PatientId && x.StagingStatus!.Equals("Planned") && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date));

                    if (patient.Count > 0)
                    {
                        IsLoading = false;
                        ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                        return;
                    }

                    FormRegis.StagingStatus = "Confirmed";
                    StagingText = "Nurse Station";
                    FormRegis = await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormRegis));

                    PatientAllergy.UserId = FormRegis.PatientId.GetValueOrDefault();

                    if (PatientAllergy.Id == 0)
                        PatientAllergy = await Mediator.Send(new CreatePatientAllergyRequest(PatientAllergy));
                    else
                        PatientAllergy = await Mediator.Send(new UpdatePatientAllergyRequest(PatientAllergy));
                }

                var result = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == FormRegis.Id));
                FormRegis = result[0];

                //FormRegis.IsWeather = !string.IsNullOrWhiteSpace(PatientAllergy.Weather);
                //FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(PatientAllergy.Farmacology);
                //FormRegis.IsFood = !string.IsNullOrWhiteSpace(PatientAllergy.Food);

                if (SelectedFoodAllergies.Count() > 0)
                    FormRegis.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    FormRegis.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    FormRegis.IsPharmacology = true;

                if (FormRegis.StagingStatus is not null && FormRegis.StagingStatus.Equals("Physician"))
                {
                    if (Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                    {
                        IsPratition = [.. AllDoctors.Where(x => x.Id == UserLogin.Id).ToList()];
                        FormRegis.PratitionerId = IsPratition.Count > 0 ? IsPratition[0].Id : null;
                    }
                    else
                        IsPratition = [.. AllDoctors.Where(x => x.IsDoctor == true && x.IsPhysicion == true).ToList()];
                }

                await ReadHeightWeightPatient();

                IsLoading = false;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task<bool> SendPcareRequestKunjungan()
        {
            if (FormRegis.StagingStatus is not null && FormRegis.StagingStatus.Equals("Nurse Station") && FormRegis.Payment is not null && FormRegis.Payment.Equals("BPJS") && SelectedBPJSIntegration is not null)
            {
                var ll = GeneralConsultanCPPTs.Where(x => x.Title == "Diagnosis").Select(x => x.Body).ToList();

                string diag1 = null!;

                if (FormRegis.StagingStatus.Equals("Nurse Station"))
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

                if (FormRegis.StagingStatus.Equals("Physician"))
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
                    NoKunjungan = FormRegis.SerialNo ?? string.Empty,
                    NoKartu = SelectedBPJSIntegration.NoKartu ?? "",
                    TglDaftar = FormRegis.RegistrationDate.ToString("dd-MM-yyyy"),
                    KdPoli = Services.FirstOrDefault(x => x.Id == FormRegis.ServiceId)!.Code,
                    KdSadar = Awareness.FirstOrDefault(x => x.Id == GeneralConsultantClinical.AwarenessId)!.KdSadar,
                    Sistole = GeneralConsultantClinical.Sistole.ToInt32(),
                    Diastole = GeneralConsultantClinical.Diastole.ToInt32(),
                    BeratBadan = GeneralConsultantClinical.Weight.ToInt32(),
                    TinggiBadan = GeneralConsultantClinical.Height.ToInt32(),
                    RespRate = GeneralConsultantClinical.RR.ToInt32(),
                    HeartRate = GeneralConsultantClinical.HR.ToInt32(),
                    LingkarPerut = GeneralConsultantClinical.WaistCircumference.ToInt32(),
                    KdStatusPulang = "4",
                    TglPulang = FormRegis.RegistrationDate.ToString("dd-MM-yyyy"),
                    KdDokter = IsPratition.FirstOrDefault(x => x.Id == FormRegis.PratitionerId)!.PhysicanCode,
                    KdDiag1 = diag1,
                    KdDiag2 = null,
                    KdDiag3 = null,
                    Suhu = GeneralConsultantClinical.Temp.ToString(),
                };

                var responseApi = await PcareService.SendPCareService($"kunjungan", HttpMethod.Post, kunj);

                if (responseApi.Item2 != 200)
                {
                    ToastService.ShowError($"{responseApi.Item1}");

                    IsLoading = false;
                    return false;
                }
                else
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(responseApi.Item1);
                    if (!string.IsNullOrWhiteSpace(FormRegis.SerialNo)) // Check if the serial no is not getting from kiosk
                        FormRegis.SerialNo = data.response.message;
                }
            }

            return true;
        }

        private async Task<bool> SendPcareRequestRegistration()
        {
            if (FormRegis.StagingStatus is not null && FormRegis.StagingStatus.Equals("Planned") && FormRegis.Payment is not null && FormRegis.Payment.Equals("BPJS") && SelectedBPJSIntegration is not null)
            {
                var regis = new PendaftaranRequest
                {
                    kdProviderPeserta = SelectedBPJSIntegration.KdProviderPstKdProvider ?? "",
                    tglDaftar = FormRegis.RegistrationDate.ToString("dd-MM-yyyy"),
                    noKartu = SelectedBPJSIntegration.NoKartu ?? "",
                    kdPoli = Services.FirstOrDefault(x => x.Id == FormRegis.ServiceId)!.Code,
                    keluhan = null,
                    kunjSakit = true,
                    kdTkp = "10"
                };

                Console.WriteLine("Sending pendaftaran...");
                var responseApi = await PcareService.SendPCareService($"pendaftaran", HttpMethod.Post, regis);

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
                    FormRegis.SerialNo = data.message;
            }
            return true;
        }

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

        private async Task OnSave()
        {
            try
            {
                IsLoading = true;
                ToastService.ClearInfoToasts();

                if (!FormValidationState)
                    return;

                //if (!FormRegis.IsWeather)
                //    PatientAllergy.Weather = null;
                //if (!FormRegis.IsPharmacology)
                //    PatientAllergy.Farmacology = null;
                //if (!FormRegis.IsFood)
                //    PatientAllergy.Food = null;

                GeneralConsultanMedicalSupport.LabResulLabExaminationtIds = SelectedLabTests.Select(x => x.Id).ToList();

                if (FormRegis.Id == 0)
                {
                    var patient = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.ServiceId == FormRegis.ServiceId && x.PatientId == FormRegis.PatientId && x.StagingStatus!.Equals("Planned") && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date));

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

                if (IsReferTo && SelectedRujukanType is not null && SelectedRujukanType == RujukanTypes.ToList()[0])
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
                                if (FormRegis.IsPharmacology || FormRegis.IsFood || FormRegis.IsWeather)
                                {
                                    var ids = new List<long>();
                                    ids.AddRange(SelectedPharmacologyAllergies.Select(x => x.Id).ToList());
                                    ids.AddRange(SelectedWeatherAllergies.Select(x => x.Id).ToList());
                                    ids.AddRange(SelectedFoodAllergies.Select(x => x.Id).ToList());

                                    var u = patients.FirstOrDefault(x => x.Id == FormRegis.PatientId);
                                    if (u is not null)
                                    {
                                        u.PatientAllergyIds = ids;
                                        await Mediator.Send(new UpdateUserRequest(u));
                                    }
                                }

                                FormRegis = await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormRegis));

                                //PatientAllergy.UserId = FormRegis.PatientId.GetValueOrDefault();

                                //if (PatientAllergy.Id == 0)
                                //    PatientAllergy = await Mediator.Send(new CreatePatientAllergyRequest(PatientAllergy));
                                //else
                                //    PatientAllergy = await Mediator.Send(new UpdatePatientAllergyRequest(PatientAllergy));
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

                                if (FormRegis.IsPharmacology || FormRegis.IsFood || FormRegis.IsWeather)
                                {
                                    var ids = new List<long>();
                                    ids.AddRange(SelectedPharmacologyAllergies.Select(x => x.Id).ToList());
                                    ids.AddRange(SelectedWeatherAllergies.Select(x => x.Id).ToList());
                                    ids.AddRange(SelectedFoodAllergies.Select(x => x.Id).ToList());

                                    var u = patients.FirstOrDefault(x => x.Id == FormRegis.PatientId);
                                    if (u is not null)
                                    {
                                        u.PatientAllergyIds = ids;
                                        await Mediator.Send(new UpdateUserRequest(u));
                                    }
                                }

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

                                GeneralConsultanCPPTs.ForEach(x => { x.GeneralConsultanService = null; x.GeneralConsultanServiceId = FormRegis.Id; x.Id = 0; });
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

                                await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));

                                await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(deleteByGeneralServiceId: FormRegis.Id));

                                GeneralConsultanCPPTs.ForEach(x => { x.GeneralConsultanService = null; x.GeneralConsultanServiceId = FormRegis.Id; x.Id = 0; });
                                await Mediator.Send(new CreateListGeneralConsultanCPPTRequest(GeneralConsultanCPPTs));

                                BrowserFiles.Distinct();

                                foreach (var item in BrowserFiles)
                                {
                                    await FileUploadService.UploadFileAsync(item, 0, []);
                                }

                                if (FormRegis.IsSickLeave == true || FormRegis.IsMaternityLeave == true)
                                {
                                    
                                    var checkDataSickLeave = await Mediator.Send(new GetSickLeaveQuery());
                                    var crosschek = checkDataSickLeave.Where(x => x.GeneralConsultansId == FormRegis.Id).FirstOrDefault();
                                    if (crosschek == null)
                                    {
                                        if (FormRegis.IsSickLeave == true)
                                            SickLeaves.TypeLeave = "SickLeave";
                                        else if (FormRegis.IsMaternityLeave == true)
                                            SickLeaves.TypeLeave = "Maternity";
                                        SickLeaves.GeneralConsultansId = FormRegis.Id;
                                        await Mediator.Send(new CreateSickLeaveRequest(SickLeaves));
                                    }
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

                //FormRegis.IsWeather = !string.IsNullOrWhiteSpace(PatientAllergy.Weather);
                //FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(PatientAllergy.Farmacology);
                //FormRegis.IsFood = !string.IsNullOrWhiteSpace(PatientAllergy.Food);

                if (SelectedFoodAllergies.Any())
                    FormRegis.IsFood = true;
                if (SelectedWeatherAllergies.Any())
                    FormRegis.IsWeather = true;
                if (SelectedPharmacologyAllergies.Any())
                    FormRegis.IsPharmacology = true;

                ToastService.ShowSuccess("Saved Successfully");

                IsLoading = false;
            }
            catch (Exception exx)
            {
                IsLoading = false;
                exx.HandleException(ToastService);
            }
        }

        private async Task OnSaveProcedureRoom()
        {
            try
            {
                if (GeneralConsultanMedicalSupport.IsOtherECG && string.IsNullOrWhiteSpace(GeneralConsultanMedicalSupport.OtherDesc))
                {
                    ToastService.ShowInfo("Other Description can't be empty!");
                    return;
                }

                PopUpProcedureRoom = false;

                if (FormRegis.Id == 0)
                    return;

                BrowserFiles.Distinct();

                foreach (var item in BrowserFiles)
                {
                    await FileUploadService.UploadFileAsync(item, 0, []);
                }

                FormRegis.StagingStatus = "Procedure Room";
                await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));

                if (GeneralConsultanMedicalSupport.Id == 0)
                {
                    GeneralConsultanMedicalSupport.GeneralConsultanServiceId = FormRegis.Id;
                    GeneralConsultanMedicalSupport = await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                }
                else
                    GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));

                if (GeneralConsultanMedicalSupport.LabTestId is not null && GeneralConsultanMedicalSupport.LabTestId != 0)
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

        #endregion OnSave and Staging

        #region Grid Lab Test

        private IGrid GridLabTest { get; set; }
        private IReadOnlyList<object> SelectedLabTestDataItems { get; set; } = [];
        private List<LabResultDetailDto> LabResultDetails = [];
        private LabResultDetailDto LabResultDetail = new();
        private List<LabUomDto> LabUoms = [];
        private LabTestDetailDto LabTestDetail = new();

        private List<long> DeletedLabTestIds = [];
        private int FocusedRowLabTestVisibleIndex { get; set; }
        private LabUomDto LabUom = new();
        private bool IsAddOrUpdateOrDeleteLabResult = false;

        private class HomeStatusTemp
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        private List<HomeStatusTemp> _homeStatusTemps = [
                new()
                {
                    Code = "1",
                    Name = "Meninggal",
                },
                new()
                {
                    Code = "3",
                    Name = "Berobat Jalan",
                },
                new()
                {
                    Code = "4",
                    Name = "Rujuk Vertikal",
                },
                new()
                {
                    Code = "6",
                    Name = "Rujuk Horizontal",
                },
            ];

        private List<string> ResultValueTypes =
          [
              "Low",
                "Normal",
                "High",
                "Positive",
                "Negative",
          ];

        private void OnSaveLabTest(GridEditModelSavingEventArgs e)
        {
            //if (LabResultDetail.LabTestDetailId is null || LabResultDetail.LabTestDetailId == 0)
            //    return;

            IsAddOrUpdateOrDeleteLabResult = true;
            var editModel = LabResultDetail;

            //editModel.LabTestDetail = LabTests.FirstOrDefault(l => l.Id == LabResultDetail.LabTestDetailId);

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
            LabTestDetail = new();
            IsAddOrUpdateOrDeleteLabResult = true;
        }

        private async Task AddNewLabResult()
        {
            LabResultDetail = new();
            LabTestDetail = new();
            LabUom = new();
            await GridLabTest.StartEditNewRowAsync();
        }

        private async Task EditLabResult(GridCommandColumnCellDisplayTemplateContext context)
        {
            var selected = (LabResultDetailDto)context.DataItem;

            var copy = selected.Adapt<LabResultDetailDto>();

            await GridLabTest.StartEditRowAsync(FocusedRowLabTestVisibleIndex);

            var w = LabResultDetails.FirstOrDefault(x => x.Id == copy.Id);

            this.LabResultDetail = copy;
        }

        private void OnResultTextChanged(ChangeEventArgs v)
        {
            if (v.Value is null)
                return;

            var value = v.Value.ToString();

            if (long.TryParse(value, out _))
            {
                if (LabResultDetail.NormalRange is not null && !Regex.IsMatch(LabResultDetail.NormalRange, @"^\d+-\d+$"))
                    LabResultDetail.ResultType = "Negative";
                else
                {
                    var splits = LabResultDetail.NormalRange?.Split("-") ?? [];
                    if (value.ToLong() <= splits[0].ToLong())
                    {
                        LabResultDetail.ResultType = "Low";
                    }
                    else
                    {
                        LabResultDetail.ResultType = "Normal";

                        if (value.ToLong() > splits[1].ToLong())
                        {
                            LabResultDetail.ResultType = "High";
                        }
                    }
                }
            }
            else
            {
                LabResultDetail.ResultType = "Negative";
            }
        }

        private async Task CancelEditLabResult()
        {
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

            LabResultDetails.Remove(LabResultDetails.FirstOrDefault(x => x.Id == (SelectedLabTestDataItems?[0]?.Adapt<LabResultDetailDto>()?.Id)));

            SelectedLabTestDataItems = [];
        }

        private long selectedLabTestId { get; set; }

        private void SelectedItemParameter(LabTestDetailDto e)
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

            LabTestDetail = labTest;
            LabUom = labTest.LabUom;
        }

        private List<LabTestDetailDto> LabTestDetails = [];

        private async Task SelectedItemLabTest(LabTestDto e)
        {
            if (e is null)
            {
                GeneralConsultanMedicalSupport.LabTestId = null;
                LabTestDetails.Clear();
                LabResultDetails.Clear();
                return;
            }

            //LabResultDetails.Clear();

            var details = await Mediator.Send(new GetLabTestDetailQuery(x => x.LabTestId == e.Id));
            foreach (var item in details)
            {
                LabResultDetails.Add(new LabResultDetailDto
                {
                    IsFromDB = true,
                    Id = Helper.RandomNumber,
                    NormalRange = FormRegis.Patient.Gender is not null && FormRegis.Patient.Gender.Name.Equals("Male") ? item.NormalRangeMale : item.NormalRangeFemale,
                    Parameter = item.Name,
                    Remark = item.Remark,
                    LabUomId = item.LabUomId,
                    LabUom = item.LabUom,
                    ResultValueType = item.ResultValueType
                });
            }

            //var details = await Mediator.Send(new GetLabTestDetailQuery(x => x.LabTestId == e.Id));
            //if (GeneralConsultanMedicalSupport.Id == 0)
            //{
            //    var temp = new List<LabResultDetailDto>();
            //    foreach (var item in details)
            //    {
            //        temp.Add(new LabResultDetailDto
            //        {
            //            IsFromDB = true,
            //            Id = Helper.RandomNumber,
            //            NormalRange = FormRegis.Patient.Gender.Name.Equals("Male") ? item.NormalRangeMale : item.NormalRangeFemale,
            //            Parameter = item.Name,
            //            Remark = item.Remark,
            //            LabUomId = item.LabUomId,
            //            LabUom = item.LabUom,
            //            ResultValueType = item.ResultValueType
            //        });
            //    }

            //    LabResultDetails.AddRange(temp);
            //}
            //else
            //{
            //    LabResultDetails = await Mediator.Send(new GetLabResultDetailQuery(x => x.GeneralConsultanMedicalSupportId == GeneralConsultanMedicalSupport.Id));
            //}

            //LabResultDetail.LabTestId = e.Id;

            //var a = await Mediator.Send(new GetLabTestDetailQuery(x => x.LabTestId == e.Id));
            //LabTestDetails = [];
            //LabResultDetailsLabTestDetails.AddRange(a);
            GridLabTest.Reload();

            GeneralConsultanMedicalSupport.LabTestId = e.Id;
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
        private SickLeaveDto SickLeaves = new();
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

        public MarkupString GetStatusIconHtml(GeneralConsultanServiceDto priority)
        {
            if (priority is not null)
            {
                string html = $"<span class='badge bg-dark py-1 px-2' title='{priority.StagingStatus} Priority'>{priority.StagingStatus}</span>";

                return new MarkupString(html);
            }
            return new MarkupString("");
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

        private bool visible = true;
        private bool closeOnClick = true;

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

        private List<string> ClinicVisitTypes = new List<string>
        {
            "Healthy",
            "Sick"
        };

        public List<AwarenessDto> Awareness { get; set; } = [];

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
            FormInputCPPTGeneralConsultan.Objective = $"Weight: {GeneralConsultantClinical.Weight}, Height: {GeneralConsultantClinical.Height}, RR: {GeneralConsultantClinical.RR}, SpO2: {GeneralConsultantClinical.SpO2}, BMIIndex: {Math.Round(GeneralConsultantClinical.BMIIndex, 2).ToString()}, BMIState: {GeneralConsultantClinical.BMIState}, Temp: {GeneralConsultantClinical.Temp}, HR: {GeneralConsultantClinical.HR}, Systolic: {GeneralConsultantClinical.Systolic}, DiastolicBP: {GeneralConsultantClinical.DiastolicBP}, E: {GeneralConsultantClinical.E}, V: {GeneralConsultantClinical.V}, M: {GeneralConsultantClinical.M}";
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

        private List<AllergyDto> Allergies = [];

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            ClassTypes = await Mediator.Send(new GetClassTypeQuery());
            Awareness = await Mediator.Send(new GetAwarenessQuery());

            Allergies = await Mediator.Send(new GetAllergyQuery());
            Allergies.ForEach(x =>
            {
                var a = Helper._allergyTypes.FirstOrDefault(z => x.Type is not null && z.Code == x.Type);
                if (a is not null)
                    x.TypeString = a.Name;
            });

            InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery());
            NursingDiagnoses = await Mediator.Send(new GetNursingDiagnosesQuery());
            //LabTests = await Mediator.Send(new GetLabTestQuery());

            var nursingDiagnosesTemps = NursingDiagnoses.Select(x => new NursingDiagnosesTemp
            {
                Id = x.Id,
                Problem = $"{x.Problem}" // Menggunakan interpolasi string untuk menggabungkan Problem dan Code
            }).ToList();
            NursingDiagnosesTemps.AddRange(nursingDiagnosesTemps);

            Diagnoses = await Mediator.Send(new GetDiagnosisQuery());
            var diagnosesTemps = Diagnoses.Select(x => new DiagnosesTemp
            {
                Id = x.Id,
                NameCode = $"{x.Name}" // Menggunakan interpolasi string untuk menggabungkan Problem dan Code
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
        private bool IsSending { get; set; } = false;

        private async Task Send()
        {
            IsLoading = true;
            ToastService.ShowInfo("Start");
            await Task.Delay(5000);
            IsLoading = false;
        }

        private int NumericValue = 0;
        private bool IsEnabled = true;

        private void OnValueChanged2(int newValue)
        {
            NumericValue = newValue;
            if (newValue != 0)
                IsEnabled = false;
            else IsEnabled = true;
        }

        private void Grid_CustomizeElementCPPT(GridCustomizeElementEventArgs e)
        {
            var title = (System.String)e.Grid.GetRowValue(e.VisibleIndex, "Title");
            var body = (System.String)e.Grid.GetRowValue(e.VisibleIndex, "Body");

            if (title is null)
                return;

            if (!title.Equals("Date and Time") && !title.Equals("Subjective") && !title.Equals("Objective") && !title.Equals("Planning") && !title.Equals("Diagnosis"))
            {
                e.CssClass = "highlighted-item";
            }
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

        private async Task ReadHeightWeightPatient()
        {
            if (FormRegis.Id == 0)
                return;

            var services = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.PatientId == FormRegis.PatientId));

            if (services.Count <= 0 || services.Count == 1)
                return;

            var ID = services.OrderByDescending(z => z.CreateDate).ToList();

            var secondLastItem = ID.ToList()[ID.Count - 2];

            var assesments = await Mediator.Send(new GetGeneralConsultantClinicalAssesmentQuery(x => x.GeneralConsultanServiceId == secondLastItem.Id));

            if (assesments.Count <= 0)
                return;

            GeneralConsultantClinical.Weight = assesments[0].Weight;
            GeneralConsultantClinical.Height = assesments[0].Height;
        }

        private async Task OnCancel2()
        {
            IsLoading = true;
            if (FormRegis.Id != 0)
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

                FormRegis.StagingStatus = "Canceled";

                await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));

                var result = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == FormRegis.Id));
                FormRegis = result[0];

                ToastService.ShowSuccess("Cancelled..");
            }
            IsLoading = false;
        }

        private async Task<bool> SendPCareRequestUpdateStatusPanggilAntrean(int status)
        {
            try
            {
                var service = Services.FirstOrDefault(x => x.Id == FormRegis.ServiceId);

                var antreanRequest = new UpdateStatusPanggilAntreanRequestPCare
                {
                    Tanggalperiksa = DateTime.Now.ToString("yyyy-MM-dd"),
                    Kodepoli = service!.Code ?? string.Empty,
                    Nomorkartu = SelectedBPJSIntegration.NoKartu ?? string.Empty,
                    Status = status, // 1 -> Hadir, 2 -> Tidak Hadir
                    Waktu = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                };

                Console.WriteLine("Sending antrean/panggil...");
                var responseApi = await PcareService.SendPCareService($"antrean/panggil", HttpMethod.Post, antreanRequest);

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

            if (Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                IsPratition = [.. user.Where(x => x.Id == UserLogin.Id).ToList()];
                FormRegis.PratitionerId = IsPratition.Count > 0 ? IsPratition[0].Id : null;
            }
            else
                IsPratition = [.. user.Where(x => x.IsDoctor == true && x.IsPhysicion == true).ToList()];

            AllDoctors = [.. user.Where(x => x.IsDoctor == true).ToList()];

            //Insurance
            AllInsurances = await Mediator.Send(new GetInsuranceQuery());

            //Medical Type
            Services = await Mediator.Send(new GetServiceQuery());
        }

        private bool FormValidationState = true;

        private bool IsPopUpPainScale { get; set; } = false;

        private void OnClickPainScalePopUp()
        {
            IsPopUpPainScale = true;
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
            SelectedDataItems = [];
            GeneralConsultanServices = await Mediator.Send(new GetGeneralConsultanServiceQuery());
            PatientAllergies = await Mediator.Send(new GetPatientAllergyQuery());
            await SelectData();
            IsReferTo = false;
            PopUpVisible = false;
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
            FormRegis = new GeneralConsultanServiceDto();
            await SelectData();
            showForm = true;
            IsReferTo = false;
            IsAppoiment = false;
            GeneralConsultantClinical = new GeneralConsultantClinicalAssesmentDto();
            FormInputCPPTGeneralConsultan = new InputCPPTGeneralConsultanCPPT();
            GeneralConsultanCPPTs.Clear();
            GeneralConsultanMedicalSupport = new GeneralConsultanMedicalSupportDto();
        }

        private async Task EditItem_Click()
        {
            await EditItemVoid();
            await ReadHeightWeightPatient();
        }

        private bool LoadingForm { get; set; } = false;

        private async Task EditItemVoid()
        {
            try
            {
                LoadingForm = true;
                showForm = true;

                FormRegis = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();

                if (FormRegis.Payment is not null && FormRegis.Payment.Equals("BPJS"))
                {
                    var all = InsurancePolicies.Where(x => x.UserId == FormRegis.PatientId && x.Insurance is not null && x.Insurance.IsBPJS == true && x.Active == true).ToList();
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
                    var all = InsurancePolicies.Where(x => x.UserId == FormRegis.PatientId && x.Insurance is not null && x.Insurance.IsBPJS != true && x.Active == true).ToList();
                    Temps = all.Select(x => new InsuranceTemp
                    {
                        InsurancePolicyId = x.Id,
                        InsuranceId = x.InsuranceId,
                        InsuranceName = x.Insurance.Name,
                        PolicyNumber = x.PolicyNumber
                    }).ToList();
                }

                SelectedBPJSIntegration = new();

                var bpjs = await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == FormRegis.InsurancePolicyId));
                if (bpjs.Count > 0)
                {
                    var count = GeneralConsultanServices.Where(x => x.PatientId == FormRegis.PatientId && x.StagingStatus == "Planned").Count();
                    if (!string.IsNullOrWhiteSpace(bpjs[0].KdProviderPstKdProvider))
                    {
                        var parameter = await Mediator.Send(new GetSystemParameterQuery(x => x.Key.Contains("pcare_code_provider")));
                        if (parameter.Count > 0)
                        {
                            if (!parameter[0].Value.Equals(bpjs[0].KdProviderPstKdProvider))
                            {
                            }
                            else
                            {
                                SelectedBPJSIntegration = bpjs[0];
                            }
                        }
                    }
                }

                if (FormRegis.StagingStatus != "Finished")
                {
                    var text = FormRegis.StagingStatus == "Physician" ? "In Consultation" : FormRegis.StagingStatus;
                    if (!string.IsNullOrWhiteSpace(text) && text.Equals("Procedure Room"))
                    {
                        StagingText = "Procedure Room";
                    }
                    else
                    {
                        var index = Stagings.FindIndex(x => x == text);
                        StagingText = Stagings[index + 1];
                    }
                }

                //var patientAllergy = PatientAllergies.FirstOrDefault(x => x.UserId == FormRegis!.PatientId);

                //if (patientAllergy != null)
                //{
                //    PatientAllergy = patientAllergy;
                //    FormRegis.IsWeather = !string.IsNullOrWhiteSpace(patientAllergy.Weather);
                //    FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(patientAllergy.Farmacology);
                //    FormRegis.IsFood = !string.IsNullOrWhiteSpace(patientAllergy.Food);
                //}
                //else
                //{
                //    PatientAllergy = new PatientAllergyDto(); // Create a new instance if no allergy is found
                //    FormRegis.IsWeather = FormRegis.IsPharmacology = FormRegis.IsFood = false;
                //}

                //// Assign null to properties if patientAllergy is null or clear them if a new instance was created
                //PatientAllergy.Food ??= null;
                //PatientAllergy.Weather ??= null;
                //PatientAllergy.Farmacology ??= null;

                await GetPatientAllergy();

                switch (FormRegis.StagingStatus)
                {
                    case "Nurse Station":
                        var clinical = await Mediator.Send(new GetGeneralConsultantClinicalAssesmentQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        AllGeneralConsultanCPPTs = GeneralConsultanCPPTs.Select(item => item).ToList();

                        if (clinical.Count > 0)
                            GeneralConsultantClinical = clinical[0];
                        break;

                    case "Procedure Room":
                        var supportP = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));
                        if (supportP.Count > 0)
                            GeneralConsultanMedicalSupport = supportP[0];
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

                        SelectedLabTests = LabTestDetails.Where(x => GeneralConsultanMedicalSupport.LabResulLabExaminationtIds != null && GeneralConsultanMedicalSupport.LabResulLabExaminationtIds.Contains(x.Id)).ToList();

                        if (Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                        {
                            IsPratition = [.. AllDoctors.Where(x => x.Id == UserLogin.Id).ToList()];
                            FormRegis.PratitionerId = IsPratition.Count > 0 ? IsPratition[0].Id : null;
                        }
                        else
                            IsPratition = [.. AllDoctors.Where(x => x.IsDoctor == true && x.IsPhysicion == true).ToList()];

                        FormRegis.StartMaternityLeave = DateTime.Now;
                        FormRegis.EndMaternityLeave = DateTime.Now.AddMonths(3);
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

                        SelectedLabTests = LabTestDetails.Where(x => GeneralConsultanMedicalSupport.LabResulLabExaminationtIds != null && GeneralConsultanMedicalSupport.LabResulLabExaminationtIds.Contains(x.Id)).ToList();
                        break;

                    default:
                        break;
                }

                LoadingForm = false;
            }
            catch (Exception exx)
            {
                LoadingForm = false;
                exx.HandleException(ToastService);
            }
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

            var p = patients.FirstOrDefault(z => z.Id == FormRegis.PatientId);

            if (p is null || p.PatientAllergyIds is null)
                return;

            var allergies = await Mediator.Send(new GetAllergyQuery(x => p.PatientAllergyIds.Contains(x.Id)));
            if (allergies.Count > 0)
            {
                // Assuming you have another list of selected allergies IDs
                var selectedAllergyIds = allergies.Where(x => x.Type == "01" || x.Type == "02" || x.Type == "03").Select(x => x.Id).ToList();

                // Select specific allergies by their IDs
                SelectedFoodAllergies = FoodAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
                SelectedWeatherAllergies = WeatherAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
                SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();

                if (SelectedFoodAllergies.Count() > 0)
                    FormRegis.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    FormRegis.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    FormRegis.IsPharmacology = true;
            }
        }

        private void CheckedChanged(bool value)
        {
            FormRegis.IsMaternityLeave = value;

            if (value)
            {
                FormRegis.StartMaternityLeave = DateTime.Now;
                FormRegis.EndMaternityLeave = DateTime.Now.AddMonths(3);
            }
            else
            {
                FormRegis.EndMaternityLeave = null;
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
                await SendPCareGetRefrensiKhusus();
                await SendPcareGetSpesialis();
                await SendPcareGetSpesialisSarana();

                //await SetTimeSchedule();
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
                if (FormRegis.ReferVerticalKhususCategoryCode is not null && (FormRegis.ReferVerticalKhususCategoryCode.Equals("THA") || FormRegis.ReferVerticalKhususCategoryCode.Equals("HEM")))
                {
                    var result = await PcareService.SendPCareService($"spesialis/rujuk/khusus/{FormRegis.ReferVerticalKhususCategoryCode}/subspesialis/{FormRegis.ReferVerticalSpesialisParentSubSpesialisCode}/noKartu/{SelectedBPJSIntegration.NoKartu}/tglEstRujuk/{FormRegis.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", HttpMethod.Get);

                    Console.WriteLine("Hit URL: " + JsonConvert.SerializeObject($"spesialis/rujuk/khusus/{FormRegis.ReferVerticalKhususCategoryCode}/subspesialis/{FormRegis.ReferVerticalSpesialisParentSubSpesialisCode}/noKartu/{SelectedBPJSIntegration.NoKartu}/tglEstRujuk/{FormRegis.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", Formatting.Indented));

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
                    var result = await PcareService.SendPCareService($"spesialis/rujuk/khusus/{FormRegis.ReferVerticalKhususCategoryCode}/noKartu/{SelectedBPJSIntegration.NoKartu}/tglEstRujuk/{FormRegis.ReferDateVisit}", HttpMethod.Get);
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
                if (string.IsNullOrWhiteSpace(FormRegis.ReferVerticalSpesialisSaranaCode) || !Convert.ToBoolean(FormRegis.IsSarana))
                    FormRegis.ReferVerticalSpesialisSaranaCode = "0";

                var result = await PcareService.SendPCareService($"spesialis/rujuk/subspesialis/{FormRegis.ReferVerticalSpesialisParentSubSpesialisCode}/sarana/{FormRegis.ReferVerticalSpesialisSaranaCode}/tglEstRujuk/{FormRegis.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", HttpMethod.Get);

                Console.WriteLine("Hit URL: " + JsonConvert.SerializeObject($"spesialis/rujuk/subspesialis/{FormRegis.ReferVerticalSpesialisParentSubSpesialisCode}/sarana/{FormRegis.ReferVerticalSpesialisSaranaCode}/tglEstRujuk/{FormRegis.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy")}", Formatting.Indented));
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
                var result = await PcareService.SendPCareService($"spesialis/{FormRegis.ReferVerticalSpesialisParentSpesialisCode}/subspesialis", HttpMethod.Get);
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

        private async Task SendPcareGetSpesialis()
        {
            if (SpesialisPs.Count > 0)
                return;

            try
            {
                string cacheKey = $"spesialis";
                if (!MemoryCache.TryGetValue(cacheKey, out List<SpesialisPCare>? r))
                {
                    var result = await PcareService.SendPCareService($"spesialis", HttpMethod.Get);
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
                    var result = await PcareService.SendPCareService($"spesialis/sarana", HttpMethod.Get);
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
                    var result = await PcareService.SendPCareService($"spesialis/khusus", HttpMethod.Get);
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

        private async Task CloseReferTo()
        {
            PopUpVisible = false;
            if (!IsSelectedFaskes)
            {
                var value = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == FormRegis.Id));
                if (value.Count > 0)
                    FormRegis = value[0];
            }
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
            LabUoms = await Mediator.Send(new GetLabUomQuery());
            LabTests = await Mediator.Send(new GetLabTestQuery());

            LabResultDetails = await Mediator.Send(new GetLabResultDetailQuery(x => x.GeneralConsultanMedicalSupportId == GeneralConsultanMedicalSupport.Id));

            var support = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery(x => x.GeneralConsultanServiceId == FormRegis.Id));

            if (support.Count > 0)
                GeneralConsultanMedicalSupport = support[0];

            DeletedLabTestIds.Clear();

            //LabResultDetails = await Mediator.Send(new GetLabResultDetailQuery(x => x.GeneralConsultanMedicalSupportId == GeneralConsultanMedicalSupport.Id));
            DeletedLabTestIds = LabResultDetails.Select(x => x.Id).ToList();

            PopUpProcedureRoom = true;
        }

        private void OnClickPopUpAppoimentPending()
        {
            PopUpAppoimentPending = true;
        }

        private void OnClickReferralPrescriptionConcoction()
        {
            NavigationManager.NavigateTo($"/pharmacy/prescription/{FormRegis.Id}");
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

        private IEnumerable<LabTestDetailDto> SelectedLabTests = [];

        private async Task SelectedItemRegistrationDateChanged(DateTime e)
        {
            FormRegis.RegistrationDate = e;
            FormRegis.AppoimentDate = e;
            await SetTimeSchedule();
        }

        private void SelectedMaternityStartDateChanged(DateTime e)
        {
            FormRegis.StartMaternityLeave = e;
            FormRegis.EndMaternityLeave = FormRegis.StartMaternityLeave.AddMonths(3);
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

        private BPJSIntegrationDto SelectedBPJSIntegration { get; set; } = new();

        private async Task SelectedItemInsurancePolicyChanged(InsuranceTemp result)
        {
            ToastService.ClearInfoToasts();

            SelectedBPJSIntegration = new();

            if (result is null)
                return;

            ToastService.ClearWarningToasts();

            var bpjs = await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == result.InsurancePolicyId));
            if (bpjs.Count > 0)
            {
                var count = GeneralConsultanServices.Where(x => x.PatientId == FormRegis.PatientId && x.StagingStatus == "Planned").Count();
                if (!string.IsNullOrWhiteSpace(bpjs[0].KdProviderPstKdProvider))
                {
                    var parameter = await Mediator.Send(new GetSystemParameterQuery(x => x.Key.Contains("pcare_code_provider")));
                    if (parameter.Count > 0)
                    {
                        if (!parameter[0].Value.Equals(bpjs[0].KdProviderPstKdProvider))
                        {
                            ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                        }
                        else
                        {
                            SelectedBPJSIntegration = bpjs[0];
                        }
                    }
                }
                else
                {
                    ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                }
            }
        }

        private async Task SelectedItemSpesialis(SpesialisPCare e)
        {
            if (e is null)
            {
                FormRegis.ReferVerticalSpesialisParentSpesialisCode = null;
                FormRegis.ReferVerticalSpesialisParentSubSpesialisCode = null;
                return;
            }

            FormRegis.ReferVerticalSpesialisParentSpesialisCode = e.KdSpesialis;

            await SendPcareGetSubSpesialis();
        }

        private bool IsLoadingSearchFaskes { get; set; } = false;

        private async Task OnClickSearchFaskes()
        {
            IsLoadingSearchFaskes = true;
            try
            {
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

        private bool IsSelectedFaskes { get; set; } = false;

        private void SelectedFaskesRujuk(RujukanFaskesKhususSpesialisPCare e)
        {
            try
            {
                IsSelectedFaskes = true;
                if (e is null)
                {
                    IsSelectedFaskes = false;
                    PopUpVisible = false;
                    return;
                }

                var rujuk = RujukanSubSpesialis.FirstOrDefault(x => x.Kdppk == e.Kdppk);

                if (rujuk is not null)
                {
                    FormRegis.PPKRujukanCode = rujuk.Kdppk;
                    FormRegis.PPKRujukanName = rujuk.Nmppk ?? "-";
                    FormRegis.ReferVerticalSpesialisParentSpesialisName = SpesialisPs.FirstOrDefault(x => x.KdSpesialis == FormRegis.ReferVerticalSpesialisParentSpesialisCode)?.NmSpesialis ?? "-";
                    FormRegis.ReferVerticalSpesialisParentSubSpesialisName = SubSpesialisPs.FirstOrDefault(x => x.KdSubSpesialis == FormRegis.ReferVerticalSpesialisParentSubSpesialisCode)?.NmSubSpesialis ?? "-";
                    FormRegis.ReferReason = FormRegis.ReferReason is null || SelectedRujukanVertical.Equals(RujukanExtenalVertical.ToList()[0]) ? "-" : FormRegis.ReferReason;
                }
                IsSelectedFaskes = true;
            }
            catch { }

            PopUpVisible = false;
        }

        private async Task SelectedItemPatientChanged(UserDto e)
        {
            if (e is null)
            {
                FormRegis.InsurancePolicyId = null;
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
                FormRegis.NoRM = item?.NoRm ?? null;
                FormRegis.IdentityNumber = item?.NoId ?? null;
                FormRegis.PatientId = item.Id;
            }
            catch { }

            if (FormRegis.Payment is not null && FormRegis.Payment.Equals("BPJS"))
            {
                var all = InsurancePolicies.Where(x => x.UserId == PatientsId && x.Insurance != null && x.Insurance.IsBPJS == true).ToList();
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
                var all = InsurancePolicies.Where(x => x.UserId == PatientsId && x.Insurance != null && x.Insurance.IsBPJS != true).ToList();
                Temps = all.Select(x => new InsuranceTemp
                {
                    InsurancePolicyId = x.Id,
                    InsuranceId = x.InsuranceId,
                    InsuranceName = x.Insurance.Name,
                    PolicyNumber = x.PolicyNumber
                }).ToList();
            }

            await GetPatientAllergy();

            //var patientAlergy = PatientAllergies.Where(x => x.UserId == item!.Id).FirstOrDefault();

            //if (patientAlergy is not null)
            //{
            //    PatientAllergy = patientAlergy;
            //    PatientAllergy.Food = patientAlergy.Food;
            //    PatientAllergy.Weather = patientAlergy.Weather;
            //    PatientAllergy.Farmacology = patientAlergy.Farmacology;
            //    FormRegis.IsWeather = !string.IsNullOrWhiteSpace(patientAlergy.Weather);
            //    FormRegis.IsPharmacology = !string.IsNullOrWhiteSpace(patientAlergy.Farmacology);
            //    FormRegis.IsFood = !string.IsNullOrWhiteSpace(patientAlergy.Food);
            //}
            //else
            //{
            //    PatientAllergy.Food = null;
            //    PatientAllergy.Weather = null;
            //    PatientAllergy.Farmacology = null;
            //    FormRegis.IsWeather = false;
            //    FormRegis.IsPharmacology = false;
            //    FormRegis.IsFood = false;
            //}
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
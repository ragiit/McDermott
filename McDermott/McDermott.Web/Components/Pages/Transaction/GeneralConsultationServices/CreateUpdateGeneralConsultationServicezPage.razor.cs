using DevExpress.Blazor.Reporting;
using DevExpress.Blazor.RichEdit;
using DevExpress.XtraReports;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation.Results;
using McDermott.Application.Dtos.ClaimUserManagement;
using McDermott.Application.Features.Services;
using McDermott.Extentions;
using McDermott.Web.Components.Pages.Reports;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json.Linq;
using QuestPDF.Fluent;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.ClaimUserManagement.BenefitConfigurationCommand;
using static McDermott.Application.Features.Commands.ClaimUserManagement.ClaimHistoryCommand;
using static McDermott.Application.Features.Commands.ClaimUserManagement.ClaimRequestCommand;
using static McDermott.Application.Features.Commands.Transaction.GCReferToInternalCommand;
using static McDermott.Web.Components.Pages.Queue.KioskPage;
using ChangeEventArgs = Microsoft.AspNetCore.Components.ChangeEventArgs;

namespace McDermott.Web.Components.Pages.Transaction.GeneralConsultationServices
{
    public partial class CreateUpdateGeneralConsultationServicezPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

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

        #region Binding

        private List<UserDto> Physicions { get; set; } = [];
        private List<UserDto> Patients { get; set; } = [];
        private List<ServiceDto> Services { get; set; } = [];

        private List<LocationDto> Locations { get; set; } = [];
        private List<InsurancePolicyDto> InsurancePolicies { get; set; } = [];
        private List<InsurancePolicyDto> ReferToInsurancePolicies { get; set; } = [];
        private List<GeneralConsultationServiceLogDto> getGeneralConsultationServiceLog { get; set; } = [];
        private List<string> RiskOfFallingDetail = [];

        private List<AwarenessDto> Awareness { get; set; } = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];

        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAOnInputNursingDiagnosesllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];

        private string FormUrl = "clinic-service/general-consultation-services";
        private bool PanelVisible = false;
        private bool IsLoading = false;
        [Parameter] public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private bool IsStatus(EnumStatusGeneralConsultantService status) => GeneralConsultanService.Status == status;

        private EnumStatusGeneralConsultantService StagingText { get; set; } = EnumStatusGeneralConsultantService.Confirmed;
        private GeneralConsultationServiceLogDto postGeneralConsultationServiceLog { get; set; } = new();
        private GeneralConsultanServiceDto GeneralConsultanService { get; set; } = new();
        private UserDto UserForm { get; set; } = new();
        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport { get; set; } = new();

        #endregion Binding

        #region Claim User

        private List<ClaimRequestDto> GetClaimRequests { get; set; } = [];
        private List<UserDto> GetPatient { get; set; } = [];
        private List<UserDto> GetPhycisian { get; set; } = [];
        private List<BenefitConfigurationDto> GetBenefitConfigurations { get; set; } = [];
        private ClaimRequestDto PostClaimRequests = new();
        private ClaimHistoryDto postClaimhistory = new();
        private int FocusedRowVisibleClaimIndex { get; set; }
        private bool VisibleButton { get; set; } = true;
        private bool PanelVisibleClaim { get; set; } = false;

        private async Task OnClickTabClaim()
        {
            await LoadDataBenefit();
            await LoadDataPatients();
            await LoadDataPhycisian();
            await LoadDataClaim();
        }

        private bool isActiveButton { get; set; }

        private void GridClaim_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleClaimIndex = args.VisibleIndex;
            try
            {
                if ((ClaimRequestDto)args.DataItem is null)
                    return;

                isActiveButton = ((ClaimRequestDto)args.DataItem)!.Status!.Equals(EnumClaimRequestStatus.Draft);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #region Searching

        private int pageSizeGridClaim { get; set; } = 10;
        private int totalCountGridClaim = 0;
        private int activePageIndexTotalCountGridClaim { get; set; } = 0;
        private string searchTermGridClaim { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedGridClaim(string searchText)
        {
            searchTermGridClaim = searchText;
            await LoadDataClaim(0, pageSizeGridClaim);
        }

        private async Task OnpageSizeGridClaimIndexChangedGridClaim(int newpageSizeGridClaim)
        {
            pageSizeGridClaim = newpageSizeGridClaim;
            await LoadDataClaim(0, newpageSizeGridClaim);
        }

        private async Task OnPageIndexChangedGridClaim(int newPageIndex)
        {
            await LoadDataClaim(newPageIndex, pageSizeGridClaim);
        }

        #endregion Searching

        #region LoadData Claim

        private async Task LoadDataClaim(int pageIndex = 0, int pageSizeGridClaim = 10)
        {
            try
            {
                PanelVisibleClaim = true;
                SelectedDataItemsClaim = [];
                var ab = await Mediator.Send(new GetClaimRequestQuery
                {
                    SearchTerm = searchTermGridClaim ?? "",
                    Predicate = x => x.GeneralConsultanServiceId == GeneralConsultanService.Id
                });
                GetClaimRequests = ab.Item1;
                totalCountGridClaim = ab.PageCount;
                activePageIndexTotalCountGridClaim = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisibleClaim = false; }
        }

        #endregion LoadData Claim

        #region Click

        private async Task NewItemClaim_Click()
        {
            await GridClaim.StartEditNewRowAsync();
        }

        private async Task EditItemClaim_Click()
        {
            await GridClaim.StartEditRowAsync(FocusedRowVisibleClaimIndex);
        }

        private void DeleteItemClaim_Click()
        {
            GridClaim.ShowRowDeleteConfirmation(FocusedRowVisibleClaimIndex);
        }

        private async Task RefreshClaim_Click()
        {
            await LoadDataClaim();
        }

        private async Task OnDeleteClaim(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisibleClaim = true;
                if (SelectedDataItemsClaim == null || !SelectedDataItemsClaim.Any())
                {
                    await Mediator.Send(new DeleteClaimRequestRequest(((ClaimRequestDto)e.DataItem).Id));
                }
                else
                {
                    var countriesToDelete = SelectedDataItemsClaim.Adapt<List<ClaimRequestDto>>();
                    await Mediator.Send(new DeleteClaimRequestRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                }

                SelectedDataItemsClaim = [];
                await LoadDataClaim();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisibleClaim = false; }
        }

        private async Task OnSaveClaim()
        {
            try
            {
                var item = new ClaimRequestDto();
                if (PostClaimRequests.Id == 0)
                {
                    PostClaimRequests.Status = EnumClaimRequestStatus.Draft;
                    PostClaimRequests.GeneralConsultanServiceId = GeneralConsultanService.Id;
                    item = await Mediator.Send(new CreateClaimRequestRequest(PostClaimRequests));
                    ToastService.ShowSuccess($"Add Data Claim Request Success");
                }
                else
                {
                    PostClaimRequests.GeneralConsultanServiceId = GeneralConsultanService.Id;
                    item = await Mediator.Send(new UpdateClaimRequestRequest(PostClaimRequests));
                    ToastService.ShowSuccess($"Update Data Claim Request Success");
                }
                await LoadDataClaim();
            }
            catch (Exception Ex)
            {
                Ex.HandleException(ToastService);
            }
        }

        private async Task OnDone(ClaimRequestDto Data)
        {
            // Lakukan validasi klaim terlebih dahulu
            await ValidateClaimRequest(Data.PatientId, Data.BenefitId);

            // Jika tombol tidak terlihat setelah validasi, artinya klaim tidak bisa diajukan
            if (!VisibleButton)
            {
                ToastService.ClearAll();
                ToastService.ShowInfo($"Patient benefits can only be claimed again after {nextEligibleDate.ToString("dd MMMM yyyy")}");
                return;
            }
            var item = new ClaimRequestDto();
            if (Data.Id != 0)
            {
                Data.Status = EnumClaimRequestStatus.Done;
                if (Data.PatientId == null)
                {
                    Data.PhycisianId = UserLogin.Id;
                }
                Data.GeneralConsultanServiceId = GeneralConsultanService.Id;
                item = await Mediator.Send(new UpdateClaimRequestRequest(Data));

                ToastService.ShowSuccess("Update Status Success");

                postClaimhistory.PatientId = item.PatientId;
                postClaimhistory.BenefitId = item.BenefitId;
                postClaimhistory.PhycisianId = item.PhycisianId;
                postClaimhistory.ClaimDate = item.ClaimDate;
                postClaimhistory.ClaimedValue = 1;

                await Mediator.Send(new CreateClaimHistoryRequest(postClaimhistory));
            }

            await LoadDataClaim();
        }

        private DateTime nextEligibleDate { get; set; }

        private async Task cekValidasi(BenefitConfigurationDto data)
        {
            if (data is not null)
            {
                if (PostClaimRequests.PatientId is null)
                {
                    PostClaimRequests.BenefitId = null;
                    ToastService.ClearAll();
                    ToastService.ShowInfo("Patient is not null");
                    return;
                }
                PostClaimRequests.BenefitId = data.Id;

                await ValidateClaimRequest(PostClaimRequests.PatientId, PostClaimRequests.BenefitId);
            }
            else
            {
                PostClaimRequests.BenefitId = null;
            }
        }

        private async Task CekPatient(UserDto data)
        {
            VisibleButton = true;
        }

        public async Task ValidateClaimRequest(long? patientId, long? benefitId)
        {
            var benefitConfig = await Mediator.Send(new GetSingleBenefitConfigurationQuery
            {
                Predicate = x => x.Id == benefitId,
            });
            if (benefitConfig == null)
            {
                ToastService.ClearAll();
                ToastService.ShowInfo("Benefit configuration not found.");
                return;
            }

            // Dapatkan riwayat klaim pasien untuk benefit yang sama
            var lastClaims = await Mediator.Send(new GetSingleClaimHistoryQuery
            {
                Predicate = c => c.PatientId == patientId && c.BenefitId == benefitId,
                OrderByList = [
                    (x=>x.ClaimDate, true)
                    ]
            });

            var lastClaimCount = await Mediator.Send(new GetClaimHistoryQuery
            {
                Predicate = c => c.PatientId == patientId && c.BenefitId == benefitId,
            });
            int counts = 0;
            if (benefitConfig.TypeOfBenefit == "Amount")
            {
                counts = lastClaimCount.Item1.Select(x => x.ClaimedValue).Count();
            }
            else if (benefitConfig.TypeOfBenefit == "Qty")
            {
                counts = lastClaimCount.Item1.Select(x => x.ClaimedValue).Count();
            }

            // Cek apakah klaim melebihi durasi yang diizinkan
            if (lastClaims != null)
            {
                nextEligibleDate = CalculateNextEligibleDate(lastClaims.ClaimDate, benefitConfig);

                bool isEligibleForClaim = DateTime.Now <= nextEligibleDate && counts >= benefitConfig.BenefitValue;

                if (isEligibleForClaim)
                {
                    ToastService.ClearAll();
                    ToastService.ShowInfo($"Patient benefits can only be claimed again after {nextEligibleDate.ToString("dd MMMM yyyy")}");
                    VisibleButton = false;
                    return;
                }
            }

            // Jika validasi berhasil
            ToastService.ClearAll();
            ToastService.ShowSuccess("Validation was successful. Claims can be filed.");
            VisibleButton = true;
        }

        private DateTime CalculateNextEligibleDate(DateTime lastClaimDate, BenefitConfigurationDto benefitConfig)
        {
            int duration = benefitConfig.BenefitDuration.GetValueOrDefault();

            return benefitConfig.DurationOfBenefit switch
            {
                "Days" => lastClaimDate.AddDays(duration),
                "Months" => lastClaimDate.AddMonths(duration),
                "Years" => lastClaimDate.AddYears(duration),
                _ => lastClaimDate
            };
        }

        public MarkupString GetIssueStatusIconHtml(EnumClaimRequestStatus? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumClaimRequestStatus.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumClaimRequestStatus.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                //case EnumClaimRequestStatus.Active:
                //    priorityClass = "success";
                //    title = "Active";
                //    break;

                //case EnumClaimRequestStatus.InActive:
                //    priorityClass = "danger";
                //    title = "InActive";
                //    break;

                default:
                    return new MarkupString("");
            }
            string html = $"<div class='row '><div class='col-3'>" +
                         $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #region ComboBox

        #region ComboBox Patient

        private DxComboBox<UserDto, long?> refPatientsComboBox { get; set; }
        private int PatientsComboBoxIndex { get; set; } = 0;
        private int totalCountPatients = 0;

        private async Task OnSearchPatients()
        {
            await LoadDataPatients(0, 10);
        }

        private async Task OnSearchPatientsIndexIncrement()
        {
            if (PatientsComboBoxIndex < (totalCountPatients - 1))
            {
                PatientsComboBoxIndex++;
                await LoadDataPatients(PatientsComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPatientsIndexDecrement()
        {
            if (PatientsComboBoxIndex > 0)
            {
                PatientsComboBoxIndex--;
                await LoadDataPatients(PatientsComboBoxIndex, 10);
            }
        }

        private async Task OnInputPatientsChanged(string e)
        {
            PatientsComboBoxIndex = 0;
            await LoadDataPatients(0, 10);
        }

        private async Task LoadDataPatients(int pageIndex = 0, int pageSize = 10)
        {
            var result = await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.IsPatient == true,
                SearchTerm = refPatientsComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
                Select = x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth
                }
            });
            GetPatient = result.Item1;
            totalCountGridClaim = result.PageCount;
        }

        #endregion ComboBox Patient

        #region ComboBox Benefit

        private DxComboBox<BenefitConfigurationDto, long?> refBenefitComboBox { get; set; }
        private int BenefitComboBoxIndex { get; set; } = 0;
        private int totalCountBenefit = 0;

        private async Task OnSearchBenefit()
        {
            await LoadDataBenefit(0, 10);
        }

        private async Task OnSearchBenefitIndexIncrement()
        {
            if (BenefitComboBoxIndex < (totalCountBenefit - 1))
            {
                BenefitComboBoxIndex++;
                await LoadDataBenefit(BenefitComboBoxIndex, 10);
            }
        }

        private async Task OnSearchBenefitIndexDecrement()
        {
            if (BenefitComboBoxIndex > 0)
            {
                BenefitComboBoxIndex--;
                await LoadDataBenefit(BenefitComboBoxIndex, 10);
            }
        }

        private async Task OnInputBenefitChanged(string e)
        {
            BenefitComboBoxIndex = 0;
            await LoadDataBenefit(0, 10);
        }

        private async Task LoadDataBenefit(int pageIndex = 0, int pageSize = 10)
        {
            var result = await Mediator.Send(new GetBenefitConfigurationQuery
            {
                Predicate = x => x.Status == EnumBenefitStatus.Active,
                SearchTerm = refBenefitComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            GetBenefitConfigurations = result.Item1;
            totalCountGridClaim = result.PageCount;
        }

        #endregion ComboBox Benefit

        #region ComboBox Phycisian

        private DxComboBox<UserDto, long?> refPhycisianComboBox { get; set; }
        private int PhycisianComboBoxIndex { get; set; } = 0;
        private int totalCountPhycisian = 0;

        private async Task OnSearchPhycisian()
        {
            await LoadDataPhycisian(0, 10);
        }

        private async Task OnSearchPhycisianIndexIncrement()
        {
            if (PhycisianComboBoxIndex < (totalCountPhycisian - 1))
            {
                PhycisianComboBoxIndex++;
                await LoadDataPhycisian(PhycisianComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPhycisianIndexDecrement()
        {
            if (PhycisianComboBoxIndex > 0)
            {
                PhycisianComboBoxIndex--;
                await LoadDataPhycisian(PhycisianComboBoxIndex, 10);
            }
        }

        private async Task OnInputPhycisianChanged(string e)
        {
            PhycisianComboBoxIndex = 0;
            await LoadDataPhycisian(0, 10);
        }

        private async Task LoadDataPhycisian(int pageIndex = 0, int pageSize = 10)
        {
            var result = await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.IsPhysicion == true && x.IsDoctor == true,
                SearchTerm = refPhycisianComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
                Select = x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth
                }
            });
            GetPhycisian = result.Item1;
            totalCountGridClaim = result.PageCount;
        }

        #endregion ComboBox Phycisian

        #endregion ComboBox

        #endregion Click

        #endregion Claim User

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                return;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        [SupplyParameterFromQuery] public long? Id { get; set; }

        private bool ReadOnlyForm()
        {
            var a = ((
                GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned) ||
                GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.NurseStation) ||
                GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician)
                ));

            return !a;
        }

        private async Task<GeneralConsultanServiceDto> GetGeneralConsultanServiceById()
        {
            var result = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
            {
                Predicate = x => x.Id == this.Id,

                Select = x => new GeneralConsultanService
                {
                    Id = x.Id,
                    Status = x.Status,
                    PatientId = x.PatientId,
                    Patient = new User
                    {
                        Id = x.PatientId.GetValueOrDefault(),
                        Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        NoRm = x.Patient == null ? string.Empty : x.Patient.NoRm,
                        NoId = x.Patient == null ? string.Empty : x.Patient.NoId,
                        CurrentMobile = x.Patient == null ? string.Empty : x.Patient.CurrentMobile,
                        OccupationalId = x.Patient == null ? null : x.Patient.OccupationalId,
                        DateOfBirth = x.Patient == null ? null : x.Patient.DateOfBirth,

                        IsWeatherPatientAllergyIds = x.Patient != null && x.Patient.IsWeatherPatientAllergyIds,
                        IsFoodPatientAllergyIds = x.Patient != null && x.Patient.IsFoodPatientAllergyIds,
                        IsPharmacologyPatientAllergyIds = x.Patient == null ? false : x.Patient.IsPharmacologyPatientAllergyIds,
                        WeatherPatientAllergyIds = x.Patient == null ? new() : x.Patient.WeatherPatientAllergyIds,
                        FoodPatientAllergyIds = x.Patient == null ? new() : x.Patient.FoodPatientAllergyIds,
                        PharmacologyPatientAllergyIds = x.Patient == null ? new() : x.Patient.PharmacologyPatientAllergyIds,

                        IsFamilyMedicalHistory = x.Patient == null ? string.Empty : x.Patient.IsFamilyMedicalHistory,
                        FamilyMedicalHistory = x.Patient == null ? string.Empty : x.Patient.FamilyMedicalHistory,
                        FamilyMedicalHistoryOther = x.Patient == null ? string.Empty : x.Patient.FamilyMedicalHistoryOther,

                        IsMedicationHistory = x.Patient == null ? string.Empty : x.Patient.IsMedicationHistory,
                        MedicationHistory = x.Patient == null ? string.Empty : x.Patient.MedicationHistory,
                        PastMedicalHistory = x.Patient == null ? string.Empty : x.Patient.PastMedicalHistory,

                        Gender = x.Patient == null ? string.Empty : x.Patient.Gender
                    },
                    PratitionerId = x.PratitionerId,
                    Pratitioner = new User
                    {
                        Name = x.Pratitioner == null ? string.Empty : x.Pratitioner.Name,
                    },
                    ServiceId = x.ServiceId,
                    Service = new Service
                    {
                        Name = x.Service == null ? string.Empty : x.Service.Name,
                    },
                    Payment = x.Payment,
                    InsurancePolicyId = x.InsurancePolicyId,
                    AppointmentDate = x.AppointmentDate,
                    IsAlertInformationSpecialCase = x.IsAlertInformationSpecialCase,
                    RegistrationDate = x.RegistrationDate,
                    ClassType = x.ClassType,
                    TypeRegistration = x.TypeRegistration,

                    InformationFrom = x.InformationFrom,
                    ClinicVisitTypes = x.ClinicVisitTypes,
                    AwarenessId = x.AwarenessId,
                    Weight = x.Weight,
                    Height = x.Height,
                    RR = x.RR,
                    SpO2 = x.SpO2,
                    WaistCircumference = x.WaistCircumference,
                    BMIIndex = x.BMIIndex,
                    BMIIndexString = x.BMIIndexString,
                    ScrinningTriageScale = x.ScrinningTriageScale,
                    E = x.E,
                    V = x.V,
                    M = x.M,
                    Temp = x.Temp,
                    HR = x.HR,
                    Systolic = x.Systolic,
                    DiastolicBP = x.DiastolicBP,
                    PainScale = x.PainScale,
                    BMIState = x.BMIState,
                    RiskOfFalling = x.RiskOfFalling,
                    RiskOfFallingDetail = x.RiskOfFallingDetail,
                    Reference = x.Reference,
                    HomeStatus = x.HomeStatus,
                    IsSickLeave = x.IsSickLeave,
                    StartDateSickLeave = x.StartDateSickLeave,
                    EndDateSickLeave = x.EndDateSickLeave,
                    IsMaternityLeave = x.IsMaternityLeave,
                    StartMaternityLeave = x.StartMaternityLeave,
                    EndMaternityLeave = x.EndMaternityLeave,
                    IsClaim = x.IsClaim,

                    PPKRujukanCode = x.PPKRujukanCode,
                    PPKRujukanName = x.PPKRujukanName,
                    ReferVerticalSpesialisParentSpesialisName = x.ReferVerticalSpesialisParentSpesialisName,
                    ReferVerticalSpesialisParentSubSpesialisName = x.ReferVerticalSpesialisParentSubSpesialisName,
                    ReferReason = x.ReferReason,
                    VisitNumber = x.VisitNumber,
                    BMHP = x.BMHP,
                    KdPrognosa = x.KdPrognosa,
                    Anamnesa = x.Anamnesa,
                    TypeClaim = x.TypeClaim,

                    ReferDateVisit = x.ReferDateVisit,
                    ReferralNo = x.ReferralNo,
                    ReferVerticalSpesialisParentSubSpesialisCode = x.ReferVerticalSpesialisParentSubSpesialisCode,
                }
            });

            if (result.Status == EnumStatusGeneralConsultantService.NurseStation || result.Status == EnumStatusGeneralConsultantService.NurseStation)
            {
                result = await GetClinicalAssesmentPatientHistory(result);
            }

            return result;
        }

        private List<PrognosaRequest> PrognosaRequests { get; set; } = [];

        private async Task LoadPrognosaData()
        {
            PrognosaRequests = await GetPrognosaData();
        }

        private async Task<List<PrognosaRequest>> GetPrognosaData()
        {
            try
            {
                var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"prognosa", HttpMethod.Get);
                if (result.Item2 == 200)
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                    var dynamicList = (IEnumerable<dynamic>)data.list;

                    var a = dynamicList.Select(item => new PrognosaRequest
                    {
                        KdPrognosa = item.kdPrognosa,
                        NmPrognosa = item.nmPrognosa,
                    }).ToList();

                    return a;
                }
                else
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);
                    ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");

                    return [];
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            return [];
        }

        private async Task LoadData()
        {
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                var result = await GetGeneralConsultanServiceById();

                GeneralConsultanService = new();

                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo(FormUrl);
                    return;
                }

                GeneralConsultanService = result;
                UserForm = result.Patient ?? new();
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
                        break;

                    case EnumStatusGeneralConsultantService.Waiting:
                        StagingText = EnumStatusGeneralConsultantService.Physician;
                        break;

                    case EnumStatusGeneralConsultantService.Physician:
                        StagingText = EnumStatusGeneralConsultantService.Finished;

                        PrognosaRequests = await GetPrognosaData();

                        if (GeneralConsultanService.PratitionerId is null)
                        {
                            if ((Convert.ToBoolean(UserLogin.IsUser) || Convert.ToBoolean(UserLogin.IsAdmin)) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                            {
                                await LoadPhysicion(predicate: x => x.IsDoctor == true && x.Id == UserLogin.Id && x.IsPhysicion == true);
                                GeneralConsultanService.PratitionerId = UserLogin.Id;
                            }
                        }
                        else
                        {
                            await LoadPhysicion(predicate: x => x.IsDoctor == true && x.Id == GeneralConsultanService.PratitionerId && x.IsPhysicion == true);
                        }
                        break;

                    case EnumStatusGeneralConsultantService.Finished:
                        StagingText = EnumStatusGeneralConsultantService.Finished;
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

                await LoadPatient(predicate: x => x.IsPatient == true && x.Id == GeneralConsultanService.PatientId);
                await LoadService(predicate: x => x.Id == GeneralConsultanService.ServiceId);

                if (!IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    await LoadPhysicion(predicate: x => x.IsDoctor == true && x.Id == GeneralConsultanService.PratitionerId && x.IsPhysicion == true);
                }

                if (!string.IsNullOrWhiteSpace(GeneralConsultanService.Payment))
                {
                    await LoadDataInsurancePolicy(predicate: x => x.Id == GeneralConsultanService.InsurancePolicyId);
                }

                if (GeneralConsultanService.RiskOfFalling == "Humpty Dumpty")
                {
                    RiskOfFallingDetail = [.. Helper.HumptyDumpty];
                }
                else if (GeneralConsultanService.RiskOfFalling == "Morse")
                {
                    RiskOfFallingDetail = [.. Helper.Morse];
                }
                else
                {
                    RiskOfFallingDetail = [.. Helper.Geriati];
                }

                var resultLog = await Mediator.Send(new GetGeneralConsultanServicesLogQuery
                {
                    Predicate = x => x.Id == GeneralConsultanService.Id
                });

                getGeneralConsultationServiceLog = resultLog.Item1;
            }
            else
            {
                await LoadPatient();
                await LoadService();
                await LoadPhysicion();
            }

            #region Get Patient Allergies

            var alergy = (await Mediator.Send(new GetAllergyQuery()));
            FoodAllergies = alergy.Where(x => x.Type == "01").ToList();
            WeatherAllergies = alergy.Where(x => x.Type == "02").ToList();
            PharmacologyAllergies = alergy.Where(x => x.Type == "03").ToList();

            SelectedFoodAllergies = FoodAllergies.Where(x => UserForm.FoodPatientAllergyIds.Contains(x.Id));
            SelectedWeatherAllergies = WeatherAllergies.Where(x => UserForm.WeatherPatientAllergyIds.Contains(x.Id));
            SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => UserForm.PharmacologyPatientAllergyIds.Contains(x.Id));

            #endregion Get Patient Allergies

            Awareness = await Mediator.Send(new GetAwarenessQuery());
        }

        private IGrid GridClaim { get; set; }

        private IReadOnlyList<object> SelectedDataItemsClaim { get; set; } = [];

        #region ComboBox InsurancePolicy

        private InsurancePolicyDto SelectedInsurancePolicy { get; set; } = new();

        private async Task SelectedItemChanged(InsurancePolicyDto e)
        {
            if (e is null)
            {
                SelectedInsurancePolicy = new();
                await LoadDataInsurancePolicy();
            }
            else
                SelectedInsurancePolicy = e;
        }

        private CancellationTokenSource? _ctsInsurancePolicy;

        private async Task OnInputInsurancePolicy(ChangeEventArgs e)
        {
            try
            {
                _ctsInsurancePolicy?.Cancel();
                _ctsInsurancePolicy?.Dispose();
                _ctsInsurancePolicy = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsInsurancePolicy.Token);

                await LoadDataInsurancePolicy(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsInsurancePolicy?.Dispose();
                _ctsInsurancePolicy = null;
            }
        }

        private async Task LoadDataInsurancePolicy(string? e = "", Expression<Func<InsurancePolicy, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJS == GeneralConsultanService.Payment.Equals("BPJS") && x.Active == true;

                string input = e ?? "";
                string b = input.Split('-')[0].Trim();

                InsurancePolicies = await Mediator.QueryGetComboBox<InsurancePolicy, InsurancePolicyDto>(e, predicate, select: x => new InsurancePolicy
                {
                    Id = x.Id,
                    PolicyNumber = x.PolicyNumber,
                    Insurance = new Insurance
                    {
                        Name = x.Insurance == null ? "" : x.Insurance.Name,
                    },
                    NoKartu = x.NoKartu,
                    KdProviderPstKdProvider = x.KdProviderPstKdProvider,
                    PstPrb = x.PstPrb,
                    PstProl = x.PstProl
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox InsurancePolicy

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
                RiskOfFallingDetail = Helper.HumptyDumpty.ToList();
            }
            else if (e == "Morse")
            {
                RiskOfFallingDetail = Helper.Morse.ToList();
            }
            else
            {
                RiskOfFallingDetail = Helper.Geriati.ToList();
            }
        }

        #region ComboBox Patient

        private async Task SelectedItemChanged(UserDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.InsurancePolicyId = null;
                GeneralConsultanService.PatientId = null;
                GeneralConsultanService.Patient = new();
                UserForm = new();

                InsurancePolicies = [];
                await LoadPatient();
            }
            else
            {
                GeneralConsultanService.PatientId = e.Id;
                GeneralConsultanService.Patient = e;
                UserForm = e;
                await LoadDataInsurancePolicy();
            }
        }

        private CancellationTokenSource? _ctsPatient;

        private async Task OnInputPatient(ChangeEventArgs e)
        {
            try
            {
                _ctsPatient?.Cancel();
                _ctsPatient?.Dispose();
                _ctsPatient = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsPatient.Token);

                await LoadPatient(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsPatient?.Dispose();
                _ctsPatient = null;
            }
        }

        private async Task LoadPatient(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.IsPatient == true;

                Patients = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate, select: x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    NoRm = x.NoRm,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth,
                    NoId = x.NoId,
                    CurrentMobile = x.CurrentMobile,

                    IsWeatherPatientAllergyIds = x.IsWeatherPatientAllergyIds,
                    IsFoodPatientAllergyIds = x.IsFoodPatientAllergyIds,
                    IsPharmacologyPatientAllergyIds = x.IsPharmacologyPatientAllergyIds,
                    WeatherPatientAllergyIds = x.WeatherPatientAllergyIds,
                    FoodPatientAllergyIds = x.FoodPatientAllergyIds,
                    PharmacologyPatientAllergyIds = x.PharmacologyPatientAllergyIds,

                    IsFamilyMedicalHistory = x.IsFamilyMedicalHistory,
                    FamilyMedicalHistory = x.FamilyMedicalHistory,
                    FamilyMedicalHistoryOther = x.FamilyMedicalHistoryOther,

                    IsMedicationHistory = x.IsMedicationHistory,
                    MedicationHistory = x.MedicationHistory,
                    PastMedicalHistory = x.PastMedicalHistory
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Patient

        private void SelectedDateMaternityChanged(DateTime? e)
        {
            GeneralConsultanService.EndMaternityLeave = null;

            if (e is null)
                return;

            GeneralConsultanService.StartMaternityLeave = e;
            GeneralConsultanService.EndMaternityLeave = e.GetValueOrDefault().AddMonths(3).Date;
        }

        #region ComboBox Service

        private ServiceDto SelectedService { get; set; } = new();

        private async Task SelectedItemChanged(ServiceDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.ServiceId = null;
                await LoadService();
                Physicions = [];
            }
            else
            {
                GeneralConsultanService.ServiceId = e.Id;
                await LoadPhysicion();
            }
        }

        private CancellationTokenSource? _ctsService;

        private async Task OnInputService(ChangeEventArgs e)
        {
            try
            {
                _ctsService?.Cancel();
                _ctsService?.Dispose();
                _ctsService = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsService.Token);

                await LoadService(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsService?.Dispose();
                _ctsService = null;
            }
        }

        private async Task LoadService(string? e = "", Expression<Func<Service, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.IsVaccination == false && x.IsMaternity == false && x.IsMcu == false;
                Services = await Mediator.QueryGetComboBox<Service, ServiceDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Service

        #region ComboBox Physicion

        private UserDto SelectedPhysicion { get; set; } = new();

        private async Task SelectedItemChangedPhysicion(UserDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.PratitionerId = null;
                await LoadPhysicion();
            }
            else
                GeneralConsultanService.PratitionerId = e.Id;
        }

        private CancellationTokenSource? _ctsPhysicion;

        private async Task OnInputPhysicion(ChangeEventArgs e)
        {
            try
            {
                _ctsPhysicion?.Cancel();
                _ctsPhysicion?.Dispose();
                _ctsPhysicion = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsPhysicion.Token);

                await LoadPhysicion(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsPhysicion?.Dispose();
                _ctsPhysicion = null;
            }
        }

        private async Task LoadPhysicion(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.IsDoctor == true && x.IsPhysicion == true && x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(GeneralConsultanService.ServiceId.GetValueOrDefault());

                Physicions = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate, select: x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    NoRm = x.NoRm,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    PhysicanCode = x.PhysicanCode,
                    DateOfBirth = x.DateOfBirth,
                    NoId = x.NoId,
                    CurrentMobile = x.CurrentMobile
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Physicion

        private async Task SelectedItemPaymentChanged(string e)
        {
            GeneralConsultanService.Payment = null;
            GeneralConsultanService.InsurancePolicyId = null;

            if (e is null)
                return;

            await LoadDataInsurancePolicy();
        }

        #region OnClick

        [Inject]
        public CustomAuthenticationStateProvider CustomAuth { get; set; }

        private async Task OnCancelStatus()
        {
            try
            {
                IsLoading = true;

                if (GeneralConsultanService.Id != 0)
                {
                    if (GeneralConsultanService.InsurancePolicyId is not null && GeneralConsultanService.InsurancePolicyId != 0)
                    {
                        //var isSuccess = await SendPCareRequestUpdateStatusPanggilAntrean(2);
                        //if (!isSuccess)
                        //{
                        //    IsLoading = false;
                        //    return;
                        //}
                    }

                    GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Canceled;
                    await Mediator.Send(new CancelGeneralConsultanServiceRequest(GeneralConsultanService));
                    StagingText = EnumStatusGeneralConsultantService.Canceled;

                    Id = GeneralConsultanService.Id;
                    GeneralConsultanService = await GetGeneralConsultanServiceById();

                    ToastService.ShowSuccess("The patient has been successfully canceled from the consultation.");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                IsLoading = false;
            }
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

                if (!success2)
                {
                    foreach (var f in failures2)
                    {
                        ToastService.ShowInfo(f.ErrorMessage);
                    }
                }

                if (!success2 || !success)
                    return;

                GeneralConsultanService.IsGC = true;

                if (!GeneralConsultanService.Payment!.Equals("Personal") && (GeneralConsultanService.InsurancePolicyId is null || GeneralConsultanService.InsurancePolicyId == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                UserForm.WeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds
                    ? SelectedWeatherAllergies.Select(x => x.Id).ToList()
                    : [];

                UserForm.PharmacologyPatientAllergyIds = UserForm.IsPharmacologyPatientAllergyIds
                    ? SelectedPharmacologyAllergies.Select(x => x.Id).ToList()
                    : [];

                UserForm.FoodPatientAllergyIds = UserForm.IsFoodPatientAllergyIds
                    ? SelectedFoodAllergies.Select(x => x.Id).ToList()
                    : [];

                GeneralConsultanServiceDto res = new();

                GeneralConsultanService.IsGC = true;

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:
                        var patient = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                        {
                            Includes = [x => x.Patient],
                            Select = x => new GeneralConsultanService
                            {
                                Patient = new User { Name = x.Patient.Name },
                            },
                            Predicate = x => x.Id != GeneralConsultanService.Id
                                          && x.IsGC == true
                                          && x.ServiceId == GeneralConsultanService.ServiceId
                                          && x.PatientId == GeneralConsultanService.PatientId
                                          && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned)
                                          && x.RegistrationDate != null
                                          && x.RegistrationDate.Value.Date <= DateTime.Now.Date
                        });

                        if (patient is not null)
                        {
                            IsLoading = false;
                            ToastService.ShowInfo($"Patient in the name of \"{patient.Patient?.Name}\" still has a pending transaction.");
                            return;
                        }

                        break;

                    case EnumStatusGeneralConsultantService.Physician:
                        if (GeneralConsultanService.IsSickLeave == true || GeneralConsultanService.IsMaternityLeave == true)
                        {
                            //var checkDataSickLeave = await Mediator.Send(new GetSickLeaveQuery(x => x.GeneralConsultansId == GeneralConsultanService.Id));
                            //if (checkDataSickLeave is null || checkDataSickLeave.Count == 0)
                            //{
                            //    var leaveType = GeneralConsultanService.IsSickLeave == true ? "SickLeave" : "Maternity";
                            //    SickLeaves.TypeLeave = leaveType;
                            //    SickLeaves.GeneralConsultansId = GeneralConsultanService.Id;
                            //    await Mediator.Send(new CreateSickLeaveRequest(SickLeaves));
                            //}
                        }
                        break;

                    default:
                        break;
                }

                await HandleGeneralConsultationSaveAsync(GeneralConsultanService, UserForm);

                // Handle user login validation
                if (UserLogin.Id == GeneralConsultanService.PatientId)
                {
                    var user = await Mediator.Send(new GetSingleUserQuery
                    {
                        Predicate = x => x.Id == UserForm.Id,
                        Select = x => new User { Id = x.Id, Name = x.Name }
                    });

                    if (user != null)
                    {
                        await JsRuntime.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);

                        var authProvider = (CustomAuthenticationStateProvider)CustomAuth;
                        await authProvider.UpdateAuthState(string.Empty);

                        await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_INFO, Helper.Encrypt(JsonConvert.SerializeObject(user)), 2);
                    }
                }

                // Refactored Save Logic
                async Task HandleGeneralConsultationSaveAsync(GeneralConsultanServiceDto service, UserDto userForm)
                {
                    if (GeneralConsultanService.Id == 0)
                    {
                        var createRequest = new CreateFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = service,
                            Status = service.Status,
                            UserDto = new UserDto
                            {
                                Id = service.PatientId.GetValueOrDefault(),
                                IsWeatherPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                IsPharmacologyPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                IsFoodPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                WeatherPatientAllergyIds = userForm.WeatherPatientAllergyIds,
                                PharmacologyPatientAllergyIds = userForm.PharmacologyPatientAllergyIds,
                                FoodPatientAllergyIds = userForm.FoodPatientAllergyIds,
                                IsFamilyMedicalHistory = userForm.IsFamilyMedicalHistory,
                                IsMedicationHistory = userForm.IsMedicationHistory,
                                FamilyMedicalHistory = userForm.FamilyMedicalHistory,
                                FamilyMedicalHistoryOther = userForm.FamilyMedicalHistoryOther,
                                MedicationHistory = userForm.MedicationHistory,
                                PastMedicalHistory = userForm.PastMedicalHistory,
                                CurrentMobile = userForm.CurrentMobile
                            }
                        };

                        res = await Mediator.Send(createRequest);
                        if (res is not null)
                        {
                            postGeneralConsultationServiceLog.GeneralConsultationServiceId = res.Id;
                            postGeneralConsultationServiceLog.UserById = UserLogin.Id;
                            postGeneralConsultationServiceLog.Status = service.Status;

                            await Mediator.Send(new CreateGeneralConsultationLogRequest(postGeneralConsultationServiceLog));
                        }
                    }
                    else
                    {
                        var updateRequest = new UpdateFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = service,
                            Status = service.Status,
                            UserDto = new UserDto
                            {
                                Id = service.PatientId.GetValueOrDefault(),
                                IsWeatherPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                IsPharmacologyPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                IsFoodPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                WeatherPatientAllergyIds = userForm.WeatherPatientAllergyIds,
                                PharmacologyPatientAllergyIds = userForm.PharmacologyPatientAllergyIds,
                                FoodPatientAllergyIds = userForm.FoodPatientAllergyIds,
                                IsFamilyMedicalHistory = userForm.IsFamilyMedicalHistory,
                                IsMedicationHistory = userForm.IsMedicationHistory,
                                FamilyMedicalHistory = userForm.FamilyMedicalHistory,
                                FamilyMedicalHistoryOther = userForm.FamilyMedicalHistoryOther,
                                MedicationHistory = userForm.MedicationHistory,
                                PastMedicalHistory = userForm.PastMedicalHistory,
                                CurrentMobile = userForm.CurrentMobile,
                            }
                        };

                        res = await Mediator.Send(updateRequest);
                    }
                }

                ToastService.ClearSuccessToasts();
                ToastService.ShowSuccess("Saved Successfully");

                Id = res.Id;
                GeneralConsultanService = await GetGeneralConsultanServiceById();

                if (PageMode == EnumPageMode.Create.GetDisplayName())
                    NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanService.Id}");
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

        private void OnCancelBack()
        {
            NavigationManager.NavigateTo(FormUrl);
        }

        private bool PopUpConfirmation = false;
        private bool IsContinueCPPT = false;

        private async Task OnPopupConfirmed(bool confirmed)
        {
            PopUpConfirmation = false;

            if (confirmed)
            {
                IsContinueCPPT = true;

                await OnClickConfirm(true);
            }
            else
            {
                IsContinueCPPT = false;
            }
        }

        private async Task<GeneralConsultanServiceDto> GetClinicalAssesmentPatientHistory(GeneralConsultanServiceDto result)
        {
            try
            {
                if (result.Height == 0 && result.Weight == 0)
                {
                    //var prev = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x
                    //    => x.PatientId == result.PatientId && x.Id < result.Id && x.Status == EnumStatusGeneralConsultantService.Finished))).Item1
                    //    .OrderByDescending(x => x.CreatedDate)
                    //    .FirstOrDefault() ?? new();

                    var a = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                    {
                        Select = x => new GeneralConsultanService
                        {
                            Weight = x.Weight,
                            Height = x.Height
                        },
                        Predicate = x => x.PatientId == result.PatientId && x.Id < result.Id && x.Status == EnumStatusGeneralConsultantService.Finished,
                        OrderByList =
                        [
                            (x => x.CreatedDate, true),
                        ],
                        IsDescending = true
                    });

                    if (a is not null)
                    {
                        result.Height = a?.Height ?? 0;
                        result.Weight = a?.Weight ?? 0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            return result;
        }

        private async Task OnClickConfirm(bool? clickConfirm = false, bool? isPopUpCPPT = false)
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

                if (!success2)
                {
                    foreach (var f in failures2)
                    {
                        ToastService.ShowInfo(f.ErrorMessage);
                    }
                }

                if (!success2 || !success)
                    return;

                if (!GeneralConsultanService.Payment!.Equals("Personal") && (GeneralConsultanService.InsurancePolicyId is null || GeneralConsultanService.InsurancePolicyId == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                UserForm.WeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds
                    ? SelectedWeatherAllergies.Select(x => x.Id).ToList()
                    : [];

                UserForm.PharmacologyPatientAllergyIds = UserForm.IsPharmacologyPatientAllergyIds
                    ? SelectedPharmacologyAllergies.Select(x => x.Id).ToList()
                    : [];

                UserForm.FoodPatientAllergyIds = UserForm.IsFoodPatientAllergyIds
                    ? SelectedFoodAllergies.Select(x => x.Id).ToList()
                    : [];

                if (GeneralConsultanService.InsurancePolicyId is not null && GeneralConsultanService.InsurancePolicyId != 0 && GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned))
                {
                    var isSuccess = await SendPcareRequestRegistration();
                    if (!isSuccess)
                    {
                        IsLoading = false;
                        return;
                    }
                    else
                    {
                        //await SendPCareRequestUpdateStatusPanggilAntrean(1);
                    }
                }

                if (IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    var isExistingCPPTs = await Mediator.Send(new CheckExistingGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                    if (!isExistingCPPTs)
                    {
                        if (IsContinueCPPT)
                            PopUpConfirmation = false;
                        else
                        {
                            PopUpConfirmation = true;
                            return;
                        }
                    }
                    else
                        IsContinueCPPT = true;
                }
                else
                    IsContinueCPPT = true;

                // Kirim WS Kunjungan PCare
                if ((!PopUpConfirmation && IsContinueCPPT) || IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    if (GeneralConsultanService.InsurancePolicyId is not null && GeneralConsultanService.InsurancePolicyId != 0 && GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician))
                    {
                        //var isSuccessAddKunjungan = await SendPcareRequestKunjungan();

                        //if (!isSuccessAddKunjungan)
                        //{
                        //    IsLoading = false;
                        //    return;
                        //}

                        if (string.IsNullOrWhiteSpace(GeneralConsultanService.VisitNumber))
                            await SendPcareRequestKunjungan(false);

                        if (!string.IsNullOrWhiteSpace(GeneralConsultanService.VisitNumber))
                        {
                            var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"kunjungan/rujukan/{GeneralConsultanService.VisitNumber}", HttpMethod.Get);
                            if (result.Item2 == 200)
                            {
                                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);
                                GeneralConsultanService.ReferralNo = data.noRujukan;
                            }
                            else
                            {
                                ToastService.ShowError($"Error when Sending Refferal, {result.Item2}");
                            }
                        }
                    }

                    // Fetch existing patient with planned status
                    if (GeneralConsultanService.Status == EnumStatusGeneralConsultantService.Planned)
                    {
                        var patient = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                        {
                            Includes = [x => x.Patient],
                            Select = x => new GeneralConsultanService { Patient = new User { Name = x.Patient.Name } },
                            Predicate = x => x.Id != GeneralConsultanService.Id
                                          && x.ServiceId == GeneralConsultanService.ServiceId
                                          && x.PatientId == GeneralConsultanService.PatientId
                                          && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned)
                                          && x.RegistrationDate <= DateTime.Now.Date
                        });

                        if (patient is not null)
                        {
                            IsLoading = false;
                            ToastService.ShowInfo($"Patient in the name of \"{patient.Patient?.Name}\" still has a pending transaction");
                            return;
                        }

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Confirmed;
                    }

                    GeneralConsultanServiceDto newGC;

                    //StagingText = GeneralConsultanService.Status == EnumStatusGeneralConsultantService.Confirmed
                    //   ? EnumStatusGeneralConsultantService.NurseStation
                    //   : StagingText;

                    // Handle status changes
                    switch (StagingText)
                    {
                        case EnumStatusGeneralConsultantService.Confirmed:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Confirmed;
                            StagingText = EnumStatusGeneralConsultantService.NurseStation;
                            break;

                        case EnumStatusGeneralConsultantService.NurseStation:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.NurseStation;
                            StagingText = EnumStatusGeneralConsultantService.Waiting;
                            break;

                        case EnumStatusGeneralConsultantService.Waiting:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Waiting;
                            PrognosaRequests = await GetPrognosaData();
                            StagingText = EnumStatusGeneralConsultantService.Physician;
                            break;

                        case EnumStatusGeneralConsultantService.Physician:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Physician;
                            StagingText = EnumStatusGeneralConsultantService.Finished;
                            break;

                        case EnumStatusGeneralConsultantService.Finished:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Finished;
                            StagingText = EnumStatusGeneralConsultantService.Finished;
                            break;

                        //case EnumStatusGeneralConsultantService.Physician:
                        //    GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Finished;
                        //    if (GeneralConsultanService.IsSickLeave || GeneralConsultanService.IsMaternityLeave)
                        //    {
                        //        // Logic for sick leave can be re-enabled if needed
                        //    }
                        //    StagingText = EnumStatusGeneralConsultantService.Finished;
                        //    break;

                        default:
                            break;
                    }

                    GeneralConsultanService.IsGC = true;

                    if (GeneralConsultanService.Id == 0)
                    {
                        newGC = await Mediator.Send(new CreateFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = GeneralConsultanService,
                            Status = GeneralConsultanService.Status,
                            UserDto = new UserDto
                            {
                                Id = GeneralConsultanService.PatientId.GetValueOrDefault(),
                                IsWeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                IsPharmacologyPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                IsFoodPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                WeatherPatientAllergyIds = UserForm.WeatherPatientAllergyIds,
                                PharmacologyPatientAllergyIds = UserForm.PharmacologyPatientAllergyIds,
                                FoodPatientAllergyIds = UserForm.FoodPatientAllergyIds,
                                IsFamilyMedicalHistory = UserForm.IsFamilyMedicalHistory,
                                IsMedicationHistory = UserForm.IsMedicationHistory,
                                FamilyMedicalHistory = UserForm.FamilyMedicalHistory,
                                FamilyMedicalHistoryOther = UserForm.FamilyMedicalHistoryOther,
                                MedicationHistory = UserForm.MedicationHistory,
                                PastMedicalHistory = UserForm.PastMedicalHistory,
                                CurrentMobile = UserForm.CurrentMobile
                            }
                        });
                        if (newGC is not null)
                        {
                            postGeneralConsultationServiceLog.GeneralConsultationServiceId = newGC.Id;
                            postGeneralConsultationServiceLog.UserById = UserLogin.Id;
                            postGeneralConsultationServiceLog.Status = GeneralConsultanService.Status;

                            await Mediator.Send(new CreateGeneralConsultationLogRequest(postGeneralConsultationServiceLog));
                        }
                    }
                    else
                    {
                        if (GeneralConsultanService.Status == EnumStatusGeneralConsultantService.Physician && string.IsNullOrWhiteSpace(GeneralConsultanService.ReferralNo) && !string.IsNullOrWhiteSpace(GeneralConsultanService.VisitNumber))
                        {
                            var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"kunjungan/rujukan/{GeneralConsultanService.VisitNumber}", HttpMethod.Get);
                            if (result.Item2 == 200)
                            {
                                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                                GeneralConsultanService.ReferralNo = data.noRujukan;
                            }
                            else
                            {
                                ToastService.ShowError($"Error when Sending Refferal, {result.Item2}");
                            }
                        }

                        newGC = await Mediator.Send(new UpdateConfirmFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = GeneralConsultanService,
                            Status = GeneralConsultanService.Status,
                            UserDto = new UserDto
                            {
                                Id = GeneralConsultanService.PatientId.GetValueOrDefault(),
                                IsWeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                IsPharmacologyPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                IsFoodPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                WeatherPatientAllergyIds = UserForm.WeatherPatientAllergyIds,
                                PharmacologyPatientAllergyIds = UserForm.PharmacologyPatientAllergyIds,
                                FoodPatientAllergyIds = UserForm.FoodPatientAllergyIds,
                                IsFamilyMedicalHistory = UserForm.IsFamilyMedicalHistory,
                                IsMedicationHistory = UserForm.IsMedicationHistory,
                                FamilyMedicalHistory = UserForm.FamilyMedicalHistory,
                                FamilyMedicalHistoryOther = UserForm.FamilyMedicalHistoryOther,
                                MedicationHistory = UserForm.MedicationHistory,
                                PastMedicalHistory = UserForm.PastMedicalHistory,
                                CurrentMobile = UserForm.CurrentMobile
                            }
                        });

                        if (newGC is not null)
                        {
                            postGeneralConsultationServiceLog.GeneralConsultationServiceId = newGC.Id;
                            postGeneralConsultationServiceLog.UserById = UserLogin.Id;
                            postGeneralConsultationServiceLog.Status = GeneralConsultanService.Status;

                            await Mediator.Send(new CreateGeneralConsultationLogRequest(postGeneralConsultationServiceLog));
                        }
                    }

                    // Handle user login state
                    if (UserLogin.Id == GeneralConsultanService.PatientId)
                    {
                        var usr = await Mediator.Send(new GetSingleUserQuery
                        {
                            Predicate = x => x.Id == UserForm.Id,
                            Select = x => new User { Id = x.Id, Name = x.Name, GroupId = x.GroupId }
                        });

                        if (usr != null)
                        {
                            await JsRuntime.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);
                            var aa = (CustomAuthenticationStateProvider)CustomAuth;
                            await aa.UpdateAuthState(string.Empty);
                            await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_INFO, Helper.Encrypt(JsonConvert.SerializeObject(usr)), 2);
                        }
                    }

                    ToastService.ClearSuccessToasts();
                    IsContinueCPPT = false;
                    Id = newGC.Id;
                    GeneralConsultanService = await GetGeneralConsultanServiceById();

                    if (PageMode == EnumPageMode.Create.GetDisplayName())
                        NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanService.Id}");

                    if (IsStatus(EnumStatusGeneralConsultantService.Physician))
                    {
                        if ((Convert.ToBoolean(UserLogin.IsUser) || Convert.ToBoolean(UserLogin.IsAdmin)) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                        {
                            Physicions = (await Mediator.Send(new GetUserQueryNew
                            {
                                Predicate = x => x.IsDoctor == true && x.Id == UserLogin.Id,
                                Select = x => new User
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    NoRm = x.NoRm,
                                    Email = x.Email,
                                    MobilePhone = x.MobilePhone,
                                    PhysicanCode = x.PhysicanCode,
                                    Gender = x.Gender,
                                    DateOfBirth = x.DateOfBirth,
                                    NoId = x.NoId,
                                    CurrentMobile = x.CurrentMobile
                                }
                            })).Item1;
                            GeneralConsultanService.PratitionerId = UserLogin.Id;
                        }
                    }
                }

                // Method to map UserForm to UserDto
                UserDto MapUserDto(UserDto userForm)
                {
                    return new UserDto
                    {
                        Id = GeneralConsultanService.PatientId.GetValueOrDefault(),
                        IsWeatherPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                        IsPharmacologyPatientAllergyIds = userForm.IsPharmacologyPatientAllergyIds,
                        IsFoodPatientAllergyIds = userForm.IsFoodPatientAllergyIds,
                        WeatherPatientAllergyIds = userForm.WeatherPatientAllergyIds,
                        PharmacologyPatientAllergyIds = userForm.PharmacologyPatientAllergyIds,
                        FoodPatientAllergyIds = userForm.FoodPatientAllergyIds,
                        IsFamilyMedicalHistory = userForm.IsFamilyMedicalHistory,
                        IsMedicationHistory = userForm.IsMedicationHistory,
                        FamilyMedicalHistory = userForm.FamilyMedicalHistory,
                        FamilyMedicalHistoryOther = userForm.FamilyMedicalHistoryOther,
                        MedicationHistory = userForm.MedicationHistory,
                        PastMedicalHistory = userForm.PastMedicalHistory,
                        CurrentMobile = userForm.CurrentMobile
                    };
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<bool> SendPcareRequestKunjungan(bool? isSave = false)
        {
            if (GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician) && GeneralConsultanService.Payment is not null && GeneralConsultanService.Payment.Equals("BPJS") && GeneralConsultanService.InsurancePolicyId is not null)
            {
                var ins = InsurancePolicies.FirstOrDefault(x => x.Id == GeneralConsultanService.InsurancePolicyId);
                if (ins is null)
                {
                    ToastService.ShowInfo("Please select the Insurance Policy");
                    return false;
                }

                var g = (await Mediator.Send(new GetSingleGeneralConsultanCPPTsQuery
                {
                    OrderByList =
                    [
                        (x => x.CreatedDate, true),
                    ],
                    Predicate = x => x.GeneralConsultanServiceId == GeneralConsultanService.Id,
                    Select = x => new GeneralConsultanCPPT
                    {
                        Diagnosis = new Diagnosis
                        {
                            Code = x.Diagnosis == null ? "" : x.Diagnosis.Code,
                        }
                    }
                }));

                if (g is null)
                {
                    ToastService.ShowInfo("Please add the CPPT");
                    return false;
                }

                var statusTemp = Helper._homeStatusTemps.FirstOrDefault(x => x.Code == GeneralConsultanService.HomeStatus);

                if (statusTemp is null)
                {
                    ToastService.ShowInfo("Please select the Return Status");
                    return false;
                }

                var kunj = new KunjunganRequest();

                if (statusTemp.Code == "4")
                {
                    kunj = new KunjunganRequest
                    {
                        NoKunjungan = GeneralConsultanService.VisitNumber ?? string.Empty,
                        NoKartu = ins.PolicyNumber ?? "",
                        TglDaftar = GeneralConsultanService.RegistrationDate.ToString("dd-MM-yyyy"),
                        KdPoli = Services.FirstOrDefault(x => x.Id == GeneralConsultanService.ServiceId)!.Code,
                        Keluhan = g.Subjective ?? "",
                        KdSadar = Awareness.FirstOrDefault(x => x.Id == GeneralConsultanService.AwarenessId)!.KdSadar,
                        Sistole = GeneralConsultanService.Systolic.ToInt32(),
                        Diastole = GeneralConsultanService.DiastolicBP.ToInt32(),
                        BeratBadan = GeneralConsultanService.Weight.ToInt32(),
                        TinggiBadan = GeneralConsultanService.Height.ToInt32(),
                        RespRate = GeneralConsultanService.RR.ToInt32(),
                        HeartRate = GeneralConsultanService.HR.ToInt32(),
                        LingkarPerut = GeneralConsultanService.WaistCircumference.ToInt32(),
                        KdStatusPulang = statusTemp.Code,
                        TglPulang = GeneralConsultanService.RegistrationDate.ToString("dd-MM-yyyy"),
                        KdDokter = Physicions.FirstOrDefault(x => x.Id == GeneralConsultanService.PratitionerId)!.PhysicanCode,
                        KdDiag1 = g.Diagnosis?.Code ?? null,
                        KdDiag2 = null,
                        KdDiag3 = null,
                        KdPoliRujukInternal = null,
                        RujukLanjut = new RujukLanjutRequest
                        {
                            Kdppk = GeneralConsultanService.PPKRujukanCode ?? "",
                            TglEstRujuk = GeneralConsultanService.ReferDateVisit.GetValueOrDefault().ToString("dd-MM-yyyy"),
                            SubSpesialis = new SubSpesialisRequestRujuk
                            {
                                KdSubSpesialis1 = GeneralConsultanService.ReferVerticalSpesialisParentSubSpesialisCode ?? ""
                            }
                        },
                        KdTacc = -1,
                        AlasanTacc = null,
                        Anamnesa = g.Anamnesa,
                        AlergiMakan = SelectedFoodAllergies.FirstOrDefault()?.KdAllergy ?? null,
                        AlergiUdara = SelectedWeatherAllergies.FirstOrDefault()?.KdAllergy ?? null,
                        AlergiObat = SelectedPharmacologyAllergies.FirstOrDefault()?.KdAllergy ?? null,
                        KdPrognosa = GeneralConsultanService.KdPrognosa ?? "",
                        TerapiObat = g.MedicationTherapy ?? "",
                        TerapiNonObat = g.NonMedicationTherapy ?? "",
                        Bmhp = GeneralConsultanService.BMHP ?? "",
                        Suhu = "36",
                    };
                    Console.WriteLine(JsonConvert.SerializeObject(kunj, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented));

                    var responseApi = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"kunjungan", HttpMethod.Post, kunj);

                    if (responseApi.Item2 != 201)
                    {
                        var json = JObject.Parse(responseApi.Item1.ToString());
                        string message = json["message"]?.ToString() ?? "Error When sending PCare 'Kunjungan'";

                        ToastService.ShowError(message);

                        return false;

                        IsLoading = false;
                        return false;
                    }
                    else
                    {
                        if (responseApi.Item1 is not null)
                        {
                            dynamic data = JsonConvert.DeserializeObject<dynamic>(responseApi.Item1);
                            var dynamicList = (IEnumerable<dynamic>)data;

                            var xz = dynamicList.Select(item => new
                            {
                                message = item.message
                            }).ToList();

                            GeneralConsultanService.VisitNumber = xz[0].message;

                            if (!Convert.ToBoolean(isSave))
                                return true;

                            var updateRequest = new UpdateFormGeneralConsultanServiceNewRequest
                            {
                                GeneralConsultanServiceDto = GeneralConsultanService,
                                Status = EnumStatusGeneralConsultantService.Physician,
                                IsReferTo = false
                            };

                            await Mediator.Send(updateRequest);
                            Id = GeneralConsultanService.Id;
                            GeneralConsultanService = await GetGeneralConsultanServiceById();
                        }
                    }
                }
                else
                {
                    ToastService.ShowInfo("Please select 'Rujuk Vertical' in the Discharge Plan field when sending a Referral.");
                }
            }

            return true;
        }

        private bool IsLoadingSendReferral = false;

        private async Task OnClickSendRefferal()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(GeneralConsultanService.VisitNumber))
                    await SendPcareRequestKunjungan();

                if (!string.IsNullOrWhiteSpace(GeneralConsultanService.VisitNumber))
                {
                    var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"kunjungan/rujukan/{GeneralConsultanService.VisitNumber}", HttpMethod.Get);
                    if (result.Item2 == 200)
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);
                        GeneralConsultanService.ReferralNo = data.noRujukan;

                        var updateRequest = new UpdateFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = GeneralConsultanService,
                            Status = EnumStatusGeneralConsultantService.Physician,
                            IsReferTo = false
                        };

                        await Mediator.Send(updateRequest);
                        Id = GeneralConsultanService.Id;
                        GeneralConsultanService = await GetGeneralConsultanServiceById();
                    }
                    else
                    {
                        ToastService.ShowError($"Error when Sending Refferal, {result.Item2}");
                    }
                }

                //if (GeneralConsultanService.VisitNumber is not null || GeneralConsultanService.ReferralNo != null)
                //{
                //    var updateRequest = new UpdateFormGeneralConsultanServiceNewRequest
                //    {
                //        GeneralConsultanServiceDto = GeneralConsultanService,
                //        Status = EnumStatusGeneralConsultantService.Physician,
                //        IsReferTo = false
                //    };

                //    await Mediator.Send(updateRequest);
                //    Id = GeneralConsultanService.Id;
                //    GeneralConsultanService = await GetGeneralConsultanServiceById();
                //}
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
            }
        }

        private async Task<bool> SendPcareRequestRegistration()
        {
            if (GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned) && GeneralConsultanService.Payment is not null && GeneralConsultanService.Payment.Equals("BPJS") && GeneralConsultanService.InsurancePolicyId is not null)
            {
                var ins = InsurancePolicies.FirstOrDefault(x => x.Id == GeneralConsultanService.InsurancePolicyId);
                if (ins is null)
                {
                    ToastService.ShowInfo("Please select the Insurance Policy");
                    return false;
                }

                var regis = new PendaftaranRequest
                {
                    kdProviderPeserta = InsurancePolicies.FirstOrDefault(x => x.Id == GeneralConsultanService.InsurancePolicyId)?.KdProviderPstKdProvider ?? "",
                    tglDaftar = GeneralConsultanService.RegistrationDate.ToString("dd-MM-yyyy"),
                    noKartu = ins.PolicyNumber ?? "",
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
                    {
                        ToastService.ShowError($"{data.message}\n Code: {responseApi.Item2}");
                        return true;
                    }
                    else
                        ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");

                    Console.WriteLine(JsonConvert.SerializeObject(regis, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented));

                    IsLoading = false;
                    return false;
                }
                else
                    GeneralConsultanService.SerialNo = data.message;
            }
            return true;
        }

        private async Task<bool> SendPCareRequestUpdateStatusPanggilAntrean(int status)
        {
            try
            {
                var ins = InsurancePolicies.FirstOrDefault(x => x.Id == GeneralConsultanService.InsurancePolicyId);
                if (ins is null)
                {
                    ToastService.ShowInfo("Please select the Insurance Policy");
                    return false;
                }

                var service = Services.FirstOrDefault(x => x.Id == GeneralConsultanService.ServiceId);

                var antreanRequest = new UpdateStatusPanggilAntreanRequestPCare
                {
                    Tanggalperiksa = DateTime.Now.ToString("yyyy-MM-dd"),
                    Kodepoli = service!.Code ?? string.Empty,
                    Nomorkartu = ins.NoKartu ?? string.Empty,
                    Status = status, // 1 -> Hadir, 2 -> Tidak Hadir
                    Waktu = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                };

                Console.WriteLine("Sending antrean/panggil...");
                var responseApi = await PcareService.SendPCareService(nameof(SystemParameter.AntreanFKTPBaseURL), $"antrean/panggil", HttpMethod.Post, antreanRequest);

                if (responseApi.Item2 != 200)
                {
                    ToastService.ShowError($"{responseApi.Item1}, Code: {responseApi.Item2}");
                    Console.WriteLine(JsonConvert.SerializeObject(antreanRequest, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented));
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

        private bool IsLoadingHistoricalRecordPatient { get; set; } = false;

        private void OnClickPopUpHistoricalMedical()
        {
            IsHistoricalRecordPatient = true;
        }

        private bool IsFollowUp = false;
        private bool IsReferTo = false;
        private bool IsAppoimentPending = false;
        private bool IsHistoricalRecordPatient = false;

        private void OnReferToClick()
        {
            IsReferTo = true;
        }

        private async Task HandleClosePopupReferTo()
        {
            IsReferTo = false; // Tutup popup

            await LoadData();
        }

        private void OnAppoimentPopUpClick()
        {
            IsFollowUp = true;
        }

        private void HandleClosePopup()
        {
            IsFollowUp = false;
        }

        private void OnClickPopUpAppoimentPending()
        {
            IsAppoimentPending = true;
        }

        private void OnClickReferralPrescriptionConcoction()
        {
            NavigationManager.NavigateTo($"pharmacy/prescriptions/{EnumPageMode.Update.GetDisplayName()}?GcId={GeneralConsultanService.Id}");
        }

        private void OnPrintDocumentMedical()
        {
            var IdEncrypt = SecureHelper.EncryptIdToBase64(GeneralConsultanService.Id);
            NavigationManager.NavigateTo($"transaction/print-document-medical/{IdEncrypt}");
        }

        private void OnClickPopUpPopUpProcedureRoom()
        {
            NavigationManager.NavigateTo($"clinic-service/procedure-rooms/{EnumPageMode.Update.GetDisplayName()}?GcId={GeneralConsultanService.Id}");
        }

        private bool isPrint { get; set; } = false;
        private DxRichEdit richEdit;
        private DevExpress.Blazor.RichEdit.Document documentAPI;
        public byte[]? DocumentContent;

        private async Task SendToPrint(long id)
        {
            try
            {
                PanelVisible = true;

                DateTime? startSickLeave = null;
                DateTime? endSickLeave = null;

                var culture = new System.Globalization.CultureInfo("id-ID");

                var data = (await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == id,
                    Includes =
                    [
                        x => x.Pratitioner,
                        x => x.Patient
                    ],
                    Select = x => new GeneralConsultanService
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            DateOfBirth = x.Patient.DateOfBirth
                        },
                        RegistrationDate = x.RegistrationDate,
                        PratitionerId = x.PratitionerId,
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner.Name,
                            SipNo = x.Pratitioner.SipNo
                        },
                        StartMaternityLeave = x.StartMaternityLeave,
                        EndMaternityLeave = x.EndMaternityLeave,
                        StartDateSickLeave = x.StartDateSickLeave,
                        EndDateSickLeave = x.EndDateSickLeave,
                    }
                })) ?? new();
                var patienss = (await Mediator.Send(new GetSingleUserQuery
                {
                    Predicate = x => x.Id == data.PatientId,
                    Select = x => new User
                    {
                        Id = x.Id,
                        IsEmployee = x.IsEmployee,
                        Name = x.Name,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth
                    },
                })) ?? new();

                var age = 0;

                data = data == null ? new GeneralConsultanServiceDto() : data;

                if (data is not null && data.Patient is not null && data.Patient.DateOfBirth == null)
                {
                    age = 0;
                }
                else
                {
                    age = DateTime.Now.Year - patienss.DateOfBirth.GetValueOrDefault().Year;
                }
                if (data is not null && data.IsSickLeave)
                {
                    startSickLeave = data.StartDateSickLeave;
                    endSickLeave = data.EndDateSickLeave;
                }
                else if (data.IsMaternityLeave)
                {
                    startSickLeave = data.StartMaternityLeave;
                    endSickLeave = data.EndMaternityLeave;
                }

                int TotalDays = endSickLeave.GetValueOrDefault().Day - startSickLeave.GetValueOrDefault().Day;

                string WordDays = ConvertNumberHelper.ConvertNumberToWord(TotalDays);

                string todays = data.RegistrationDate.ToString("dddd", culture);

                //Gender
                string Gender = "";
                string OppositeSex = "";
                if (patienss.Gender != null)
                {
                    Gender = patienss.Gender == "Male" ? "MALE (L)" : "FEMALE (P)";
                    OppositeSex = patienss.Gender == "Male" ? "<strike>F(P)</strike>" : "<strike>M(L)</strike>";
                }

                isPrint = true;
                string GetDefaultValue(string value, string defaultValue = "-")
                {
                    return value ?? defaultValue;
                }

                var mergeFields = new Dictionary<string, string>
                {
                    {"<<NamePatient>>", GetDefaultValue(patienss.Name)},
                    {"<<startDate>>", GetDefaultValue(startSickLeave.GetValueOrDefault().ToString("dd MMMM yyyy"))},
                    {"<<endDate>>", GetDefaultValue(endSickLeave.GetValueOrDefault().ToString("dd MMMM yyyy"))},
                    {"<<NameDoctor>>", GetDefaultValue(data?.Pratitioner?.Name)},
                    {"<<SIPDoctor>>", GetDefaultValue(data?.Pratitioner?.SipNo)},
                    {"<<AddressPatient>>", GetDefaultValue(patienss.DomicileAddress1) + GetDefaultValue(patienss.DomicileAddress2)},
                    {"<<AgePatient>>", GetDefaultValue(age.ToString())},
                    {"<<WordDays>>", GetDefaultValue(WordDays)},
                    {"<<Days>>", GetDefaultValue(todays)},
                    {"<<TotalDays>>", GetDefaultValue(TotalDays.ToString())},
                    {"<<Dates>>", GetDefaultValue(data.RegistrationDate.ToString("dd MMMM yyyy"))},
                    {"<<Times>>", GetDefaultValue(data.RegistrationDate.ToString("H:MM"))},
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
            finally { PanelVisible = false; }
        }

        #region Print Rujukan BPJS

        private DxReportViewer reportViewerRujukanBPJS { get; set; }
        private IReport ReportRujukanBPJS { get; set; }
        private bool IsPrintRujukanBPJS = false;

        private async Task OnPrintRujukanBPJS()
        {
            await LoadPrintRujukanBPJS();
            IsPrintRujukanBPJS = true;
        }

        private async Task LoadPrintRujukanBPJS()
        {
            try
            {
                var myReport = new ReportRujukanBPJS();

                var gx = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == GeneralConsultanService.Id,
                    Select = x => new GeneralConsultanService
                    {
                        Patient = new User
                        {
                            Name = x.Patient.Name,
                            DateOfBirth = x.Patient.DateOfBirth,
                            Gender = x.Patient.Gender,
                        },
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner.Name,
                        },

                        VisitNumber = x.VisitNumber,
                        ReferVerticalSpesialisParentSpesialisName = x.ReferVerticalSpesialisParentSpesialisName,
                        PPKRujukanName = x.PPKRujukanName,
                        ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                        InsurancePolicyId = x.InsurancePolicyId,
                        ReferDiagnosisNm = x.ReferDiagnosisNm,
                        ReferDateVisit = x.ReferDateVisit,
                        PracticeScheduleTimeDate = x.PracticeScheduleTimeDate,
                        ReferralExpiry = x.ReferralExpiry,
                        ReferSelectFaskesDate = x.ReferSelectFaskesDate,
                        ReferralNo = x.ReferralNo
                    }
                }) ?? new();

                var inspolcy = await Mediator.Send(new GetSingleInsurancePolicyQuery
                {
                    Predicate = x => x.Id == gx.InsurancePolicyId,
                    Select = x => new InsurancePolicy
                    {
                        PolicyNumber = x.PolicyNumber,
                        JnsKelasKode = x.JnsKelasKode
                    }
                }) ?? new();

                // Header
                myReport.xrLabelNoKunjungan.Text = gx.VisitNumber;

                myReport.xrLabelTsDokter.Text = gx.ReferVerticalSpesialisParentSpesialisName; // Spesialis
                myReport.xrLabelDi.Text = gx.PPKRujukanName;

                // Patient
                myReport.xrLabelNama.Text = gx.Patient?.Name ?? "-";
                myReport.xrLabelBPJS.Text = inspolcy.PolicyNumber;
                myReport.xrLabelDiagnosaa.Text = gx.ReferDiagnosisNm; // Diagnosa
                myReport.xrLabelUmur.Text = gx.Patient?.Age.ToString() ?? "-";
                myReport.xrLabelTahun.Text = gx.Patient?.DateOfBirth.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelStatusTanggunan.Text = inspolcy.JnsKelasKode;
                myReport.xrLabelGender.Text = gx.Patient?.Gender == "Male" ? "L" : "P";

                myReport.xrLabelRencana.Text = gx.ReferDateVisit.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelJadwal.Text = gx.PracticeScheduleTimeDate;
                myReport.xrLabelBerlakuDate.Text = gx.ReferralExpiry.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelSalamDate.Text = $"Salam sejawat,\r\n{gx.ReferSelectFaskesDate.GetValueOrDefault().ToString("dd MMMM yyyy")}";
                myReport.xrLabelDokter.Text = gx.Pratitioner?.Name ?? "-";

                if (!string.IsNullOrWhiteSpace(gx.ReferralNo))
                {
                    myReport.xrBarCode1.Text = gx.ReferralNo;
                    myReport.xrBarCode1.ShowText = false;
                }
                else
                    myReport.xrBarCode1.Visible = false;

                ReportRujukanBPJS = myReport;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Print Rujukan BPJS

        #region Print Rujukan BPJS + PRB

        private DxReportViewer reportViewerRujukanBPJSPRB { get; set; }
        private IReport ReportRujukanBPJSPRB { get; set; }
        private bool IsPrintRujukanBPJSPRB = false;

        private async Task OnPrintRujukanBPJSPRB()
        {
            await LoadPrintRujukanBPJSPPRB();
            IsPrintRujukanBPJSPRB = true;
        }

        private async Task LoadPrintRujukanBPJSPPRB()
        {
            try
            {
                var myReport = new ReportRujukanBPJS_PRB();

                var gx = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == GeneralConsultanService.Id,
                    Select = x => new GeneralConsultanService
                    {
                        Patient = new User
                        {
                            Name = x.Patient.Name,
                            DateOfBirth = x.Patient.DateOfBirth,
                            Gender = x.Patient.Gender,
                        },
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner.Name,
                        },

                        VisitNumber = x.VisitNumber,
                        ReferVerticalSpesialisParentSpesialisName = x.ReferVerticalSpesialisParentSpesialisName,
                        PPKRujukanName = x.PPKRujukanName,
                        ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                        InsurancePolicyId = x.InsurancePolicyId,
                        ReferDiagnosisNm = x.ReferDiagnosisNm,
                        ReferDateVisit = x.ReferDateVisit,
                        PracticeScheduleTimeDate = x.PracticeScheduleTimeDate,
                        ReferralExpiry = x.ReferralExpiry,
                        ReferSelectFaskesDate = x.ReferSelectFaskesDate,
                        ReferralNo = x.ReferralNo
                    }
                }) ?? new();

                var inspolcy = await Mediator.Send(new GetSingleInsurancePolicyQuery
                {
                    Predicate = x => x.Id == gx.InsurancePolicyId,
                    Select = x => new InsurancePolicy
                    {
                        PolicyNumber = x.PolicyNumber,
                        JnsKelasKode = x.JnsKelasKode
                    }
                }) ?? new();

                // Header
                myReport.xrLabelNoKunjungan.Text = gx.VisitNumber;

                myReport.xrLabelTsDokter.Text = gx.ReferVerticalSpesialisParentSpesialisName; // Spesialis
                myReport.xrLabelDi.Text = gx.PPKRujukanName;

                // Patient
                myReport.xrLabelNama.Text = gx.Patient?.Name ?? "-";
                myReport.xrLabelBPJS.Text = inspolcy.PolicyNumber;
                myReport.xrLabelDiagnosaa.Text = gx.ReferDiagnosisNm; // Diagnosa
                myReport.xrLabelUmur.Text = gx.Patient?.Age.ToString() ?? "-";
                myReport.xrLabelTahun.Text = gx.Patient?.DateOfBirth.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelStatusTanggunan.Text = inspolcy.JnsKelasKode;
                myReport.xrLabelGender.Text = gx.Patient?.Gender == "Male" ? "L" : "P";

                myReport.xrLabelRencana.Text = gx.ReferDateVisit.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelJadwal.Text = gx.PracticeScheduleTimeDate;
                myReport.xrLabelBerlakuDate.Text = gx.ReferralExpiry.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelSalamDate.Text = $"Salam sejawat,\r\n{gx.ReferSelectFaskesDate.GetValueOrDefault().ToString("dd MMMM yyyy")}";
                myReport.xrLabelDokter.Text = gx.Pratitioner?.Name ?? "-";

                if (!string.IsNullOrWhiteSpace(gx.ReferralNo))
                {
                    myReport.xrBarCode1.Text = gx.ReferralNo;
                    myReport.xrBarCode1.ShowText = false;
                }
                else
                    myReport.xrBarCode1.Visible = false;

                myReport.xrLabelRujukanBalikName.Text = gx.Patient?.Name ?? "-";

                ReportRujukanBPJSPRB = myReport;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Print Refert To Mcd
        DxReportViewer reportViewerReferToMcD { get; set; }
        IReport ReportReferToMcD { get; set; }
        private bool IsPrintReferToMcD = false;

        private async Task OnPrintRujukanMCReferTo()
        {
            await LoadPrintReferToMcD();
            IsPrintReferToMcD = true;
        }

        private async Task LoadPrintReferToMcD()
        {
            try
            {
                
                var ExtraReport = new McDReferral();
                var gs = await Mediator.Send(new GetSingleGCReferToInternalQuery
                {
                    Predicate = x => x.GeneralConsultanServiceId ==GeneralConsultanService.Id,
                    Select = x => new GCReferToInternal
                    {
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        DateRJMCINT = x.DateRJMCINT,
                        Number = x.Number,
                        ReferTo = x.ReferTo,
                        Hospital = x.Hospital,
                        CategoryRJMCINT = x.CategoryRJMCINT,
                        ExamFor = x.ExamFor,
                        TempDiagnosis = x.TempDiagnosis,
                        TherapyProvide = x.TherapyProvide,
                        InpatientClass = x.InpatientClass,
                        Specialist = x.Specialist,
                    }
                }) ?? new();
                var gx = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == gs.GeneralConsultanServiceId,
                    Select = x => new GeneralConsultanService
                    {
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            Name = x.Patient.Name,
                            DateOfBirth = x.Patient.DateOfBirth,
                            Gender = x.Patient.Gender,
                        },

                        VisitNumber = x.VisitNumber,
                        ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                        InsurancePolicyId = x.InsurancePolicyId,

                    }
                }) ?? new();

                var gp = await Mediator.Send(new GetSingleUserQuery
                {
                    Predicate = x => x.Id == gx.PatientId,
                    Select = x => new User
                    {
                        OccupationalId = x.OccupationalId,
                        Occupational = new Occupational
                        {
                            Name = x.Occupational.Name
                        },
                        Name = x.Name,

                    }
                });

                ExtraReport.xrDateRJ.Text = gs.DateRJMCINT.ToString("dd MMMM yyyy");
                ExtraReport.xrNumber.Text = gs.Number ?? "-";
                ExtraReport.xrTo.Text = gs.ReferTo ?? "-";
                ExtraReport.xrPatientName.Text = gp.Name ??"-";
                ExtraReport.xrOccupational.Text = gp.Occupational.Name ?? "-";
                ExtraReport.xrNoEmployee.Text = gp.NIP ?? "-";
                ExtraReport.xrTempDiagnosis.Text = gs.TempDiagnosis ?? "-";
                ExtraReport.xrTherapyProvide.Text = gs.TherapyProvide ?? "-";
                ExtraReport.xrNotes.Text = "";

                // Dynamically set the checkbox for the hospital
                ExtraReport.xrRSE.Checked = gs.Hospital == "RSE";
                ExtraReport.xrRSBK.Checked = gs.Hospital == "RSBK";
                ExtraReport.xrRSHB.Checked = gs.Hospital == "RSHB";
                ExtraReport.xrRSBP.Checked = gs.Hospital == "RSBP";
                ExtraReport.xrRSAB.Checked = gs.Hospital == "RSAB";
                ExtraReport.xrRSGH.Checked = gs.Hospital == "RSGH";
                ExtraReport.xrRSMA.Checked = gs.Hospital == "RSMA";
                ExtraReport.xrRSHBH.Checked = gs.Hospital == "RSHBH";
                ExtraReport.xrRSSD.Checked = gs.Hospital == "RSSD";

                // Dynamically set the checkbox for Category
                ExtraReport.xrKanker.Checked = gs.CategoryRJMCINT == "KANKER";
                ExtraReport.xrDependent.Checked = gs.CategoryRJMCINT == "DEPENDENT";
                ExtraReport.xrEmployee.Checked = gs.CategoryRJMCINT == "EMPLOYEE";
                ExtraReport.xrAccidentInside.Checked = gs.CategoryRJMCINT == "ACCIDENT Inside";
                ExtraReport.xrAccidentOutside.Checked = gs.CategoryRJMCINT == "ACCIDENT Outside";
                ExtraReport.xrKelainan.Checked = gs.CategoryRJMCINT == "KELAINAN BAWAAN";

                // Dynamically set the checkbox for Inpatient Class
                ExtraReport.xrClassVIP.Checked = gs.InpatientClass == "VIP Class";
                ExtraReport.xrClass1B.Checked = gs.InpatientClass == "Class 1 B";
                ExtraReport.xrClass2.Checked = gs.InpatientClass == "Class 2";

                // Dynamically set the checkbox for Examp For
                ExtraReport.xrFurther.Checked = gs.ExamFor == "Pemeriksaan / penanganan lebih lanjut";
                ExtraReport.xrSurgery.Checked = gs.ExamFor == "Pembedahan";
                ExtraReport.xrHospitalization.Checked = gs.ExamFor == "Perawatan";
                ExtraReport.xrMaternity.Checked = gs.ExamFor == "Bersalin";
                ExtraReport.xrRefaction.Checked = gs.ExamFor == "Pemeriksaan Refraksi Mata";
                ExtraReport.xrPhysiotherapy.Checked = gs.ExamFor == "Fisioterapy";

                // Dynamically set the checkbox for Specialist
                ExtraReport.xrDentist.Checked = gs.Specialist == "Dentist";
                ExtraReport.xrInternist.Checked = gs.Specialist == "Internist";
                ExtraReport.xrPulmonologist.Checked = gs.Specialist == "Pulmonologist";
                ExtraReport.xrCardiologist.Checked = gs.Specialist == "Cardiologist";
                ExtraReport.xrEye.Checked = gs.Specialist == "Eye";
                ExtraReport.xrENT.Checked = gs.Specialist == "ENT";
                ExtraReport.xrPaediatric.Checked = gs.Specialist == "Paediatric";
                ExtraReport.xrSurgeon.Checked = gs.Specialist == "Surgeon";
                ExtraReport.xrObstetrician.Checked = gs.Specialist == "Obstetrician";
                ExtraReport.xrNeurologist.Checked = gs.Specialist == "Neurologist";
                ExtraReport.xrUrologist.Checked = gs.Specialist == "Urologist";
                ExtraReport.xrNeurosurgeon.Checked = gs.Specialist == "Neurosurgeon";
                ExtraReport.xrOrthopaedic.Checked = gs.Specialist == "Orthopaedic";
                ExtraReport.xrPhysiotherapist.Checked = gs.Specialist == "Physiotherapist";
                ExtraReport.xrDermatologist.Checked = gs.Specialist == "Dermatologist";
                ExtraReport.xrPsychiatrist.Checked = gs.Specialist == "Psychiatrist";
                ExtraReport.xrLaboratorium.Checked = gs.Specialist == "Laboratorium";

                ReportReferToMcD = ExtraReport;
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion

        #region McD Glasses Referal
        DxReportViewer reportViewerMcDGlasessReferral { get; set; }
        IReport ReportMcDGlasessReferral { get; set; }
        private bool IsPrintMcDGlasessReferral = false;

        private async Task OnPrintMcDGlasessReferral()
        {
            await LoadPrintMcDGlasessReferral();
            IsPrintMcDGlasessReferral = true;
        }

        private async Task LoadPrintMcDGlasessReferral()
        {
            try
            {
                var ExtraReport = new McGlassesReport();

                var gs = await Mediator.Send(new GetSingleGCReferToInternalQuery
                {
                    Predicate = x => x.GeneralConsultanServiceId == GeneralConsultanService.Id,
                    Select = x => new GCReferToInternal
                    {
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        DateRJMCINT = x.DateRJMCINT,
                        Number = x.Number,
                        ReferTo = x.ReferTo,
                        Hospital = x.Hospital,
                        CategoryRJMCINT = x.CategoryRJMCINT,
                        ExamFor = x.ExamFor,
                        TempDiagnosis = x.TempDiagnosis,
                        TherapyProvide = x.TherapyProvide,
                        InpatientClass = x.InpatientClass,
                        Specialist = x.Specialist,
                    }
                }) ?? new();
                var gx = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == gs.GeneralConsultanServiceId,
                    Select = x => new GeneralConsultanService
                    {
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            Name = x.Patient.Name,
                            DateOfBirth = x.Patient.DateOfBirth,
                            Gender = x.Patient.Gender,
                        },

                        VisitNumber = x.VisitNumber,
                        ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                        InsurancePolicyId = x.InsurancePolicyId,

                    }
                }) ?? new();

                var gp = await Mediator.Send(new GetSingleUserQuery
                {
                    Predicate = x => x.Id == gx.PatientId,
                    Select = x => new User
                    {
                        OccupationalId = x.OccupationalId,
                        Occupational = new Occupational
                        {
                            Name = x.Occupational.Name
                        },
                        Name = x.Name,

                    }
                });

                ExtraReport.xrDateRJ.Text = gs.DateRJMCINT.ToString("dd MMMM yyyy");
                ExtraReport.xrNumber.Text = gs.Number ?? "-";
                ExtraReport.xrTo.Text = gs.ReferTo ?? "-";
                ExtraReport.xrPatientName.Text = gp.Name ?? "-";
                ExtraReport.xrOccupational.Text = gp.Occupational.Name ?? "-";
                ExtraReport.xrNoEmployee.Text = gx.Patient.Legacy ?? "-";
                ExtraReport.xrTempDiagnosis.Text = gs.TempDiagnosis ?? "-";
                ExtraReport.xrTherapyProvide.Text = gs.TherapyProvide ?? "-";
                ExtraReport.xrNotes.Text = "";


                // Dynamically set the checkbox for Category
                ExtraReport.xrKanker.Checked = gs.CategoryRJMCINT == "KANKER";
                ExtraReport.xrDependent.Checked = gs.CategoryRJMCINT == "DEPENDENT";
                ExtraReport.xrEmployee.Checked = gs.CategoryRJMCINT == "EMPLOYEE";
                ExtraReport.xrAccidentInside.Checked = gs.CategoryRJMCINT == "ACCIDENT Inside";
                ExtraReport.xrAccidentOutside.Checked = gs.CategoryRJMCINT == "ACCIDENT Outside";
                ExtraReport.xrKelainan.Checked = gs.CategoryRJMCINT == "KELAINAN BAWAAN";


                // Dynamically set the checkbox for Examp For
                ExtraReport.xrFurther.Checked = gs.ExamFor == "Pemeriksaan / penanganan lebih lanjut";
                ExtraReport.xrSurgery.Checked = gs.ExamFor == "Pembedahan";
                ExtraReport.xrHospitalization.Checked = gs.ExamFor == "Perawatan";
                ExtraReport.xrMaternity.Checked = gs.ExamFor == "Bersalin";
                ExtraReport.xrRefaction.Checked = gs.ExamFor == "Pemeriksaan Refraksi Mata";
                ExtraReport.xrPhysiotherapy.Checked = gs.ExamFor == "Fisioterapy";

                ReportMcDGlasessReferral = ExtraReport;
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion

        #region Safety Glasses Referral
        DxReportViewer reportViewerSafetyGlasessReferral { get; set; }
        IReport ReportSafetyGlasessReferral { get; set; }
        private bool IsPrintSafetyGlasessReferral = false;

      
        private async Task OnPrintSafetyGlasessReferral()
        {
            await LoadPrintSafetyGlasessReferral();
            IsPrintSafetyGlasessReferral = true;
        }

        private async Task LoadPrintSafetyGlasessReferral()
        {
            try
            {
                var ExtraReport = new SafetyGlassesReport();

                var gs = await Mediator.Send(new GetSingleGCReferToInternalQuery
                {
                    Predicate = x => x.GeneralConsultanServiceId == GeneralConsultanService.Id,
                    Select = x => new GCReferToInternal
                    {
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        DateRJMCINT = x.DateRJMCINT,
                        Number = x.Number,
                        ReferTo = x.ReferTo,
                        CategoryRJMCINT = x.CategoryRJMCINT,
                        ExamFor = x.ExamFor,
                        TempDiagnosis = x.TempDiagnosis,
                    }
                }) ?? new();
                var gx = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == gs.GeneralConsultanServiceId,
                    Select = x => new GeneralConsultanService
                    {
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            Name = x.Patient.Name,
                            DateOfBirth = x.Patient.DateOfBirth,
                            Gender = x.Patient.Gender,
                        },

                        VisitNumber = x.VisitNumber,
                        ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                        InsurancePolicyId = x.InsurancePolicyId,

                    }
                }) ?? new();

                var gp = await Mediator.Send(new GetSingleUserQuery
                {
                    Predicate = x => x.Id == gx.PatientId,
                    Select = x => new User
                    {
                        OccupationalId = x.OccupationalId,
                        Occupational = new Occupational
                        {
                            Name = x.Occupational.Name
                        },
                        Name = x.Name,

                    }
                });

                ExtraReport.xrDatesRJ.Text = gs.DateRJMCINT.ToString("dd MMMM yyyy");
                ExtraReport.xrNumber.Text = gs.Number ?? "-";
                ExtraReport.xrReferTo.Text = gs.ReferTo ?? "-";
                ExtraReport.xrPatient.Text = gp.Name ?? "-";
                ExtraReport.xrOccupational.Text = gp.Occupational.Name ?? "-";
                ExtraReport.xrNoEmployee.Text = gx.Patient.Legacy ?? "-";
                ExtraReport.xrTempDiagnosis.Text = gs.TempDiagnosis ?? "-";

                ReportSafetyGlasessReferral = ExtraReport;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        private async Task OnPrint()
        {
            try
            {
                if (GeneralConsultanService.Id == 0)
                    return;

                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
                var image = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\mcdermott_logo.png");
                var file = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files\Slip_Registration.pdf");
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
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private bool IsPopUpPainScale = false;

        private void OnClickPainScalePopUp()
        {
            IsPopUpPainScale = true;
        }

        private void OnClosePopup()
        {
            IsPopUpPainScale = false;
        }

        #endregion OnClick

        #region ReferToMC

        #region Searching

        private int pageSizeRMC { get; set; } = 10;
        private int totalCountRMC = 0;
        private int activePageIndexTotalCountRMC { get; set; } = 0;
        private string searchTermRMC { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedRMC(string searchText)
        {
            searchTermRMC = searchText;
            await LoadDataRefertoMC(0, pageSizeRMC);
        }

        private async Task OnpageSizeRMCIndexChangedRMC(int newpageSizeRMC)
        {
            pageSizeRMC = newpageSizeRMC;
            await LoadDataRefertoMC(0, newpageSizeRMC);
        }

        private async Task OnPageIndexChangedRMC(int newPageIndex)
        {
            await LoadDataRefertoMC(newPageIndex, pageSizeRMC);
        }

        #endregion Searching

        private bool IsLoadingRJMCINT { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItemsRMC { get; set; } = [];
        private int FocusedRowVisibleRMCIndex { get; set; }
        private IGrid GridRMC;
        private List<GCReferToInternalDto> GcRefer { get; set; } = [];

        private async Task OnClickReferToMCINT()
        {
            await LoadDataRefertoMC();
        }

        private async Task LoadDataRefertoMC(int pageSize = 0, int pageIndex = 10)
        {
            IsLoadingRJMCINT = true;
            var gcs = await Mediator.Send(new GetGCReferToInternalQuery
            {
                Predicate = x => x.GeneralConsultanServiceId == GeneralConsultanService.Id
            });
            GcRefer = gcs.Item1;
            totalCountRMC = gcs.PageCount;
            activePageIndexTotalCountRMC = gcs.PageIndex;

            IsLoadingRJMCINT = false;
        }

        public MarkupString GetIssueGcReferToMCIconHtml(string? InpatientClass)
        {
            string priorityClass;
            string title;

            switch (InpatientClass)
            {
                case "VIP Class":
                    priorityClass = "Danger";
                    title = "VIP Class";
                    break;

                case "Class 1 B":
                    priorityClass = "warning";
                    title = "Class 1 B";
                    break;

                case "Class 2":
                    priorityClass = "info";
                    title = "Class 2";
                    break;

                //case EnumClaimRequestStatus.InActive:
                //    priorityClass = "danger";
                //    title = "InActive";
                //    break;

                default:
                    return new MarkupString("");
            }
            string html = $"<div class='row '><div class='col-3'>" +
                         $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        private void GridRCM_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleRMCIndex = args.VisibleIndex;
        }

        private async Task RefreshReferToMc_Click()
        {
            await LoadDataRefertoMC();
        }

        #endregion ReferToMC
    }
}
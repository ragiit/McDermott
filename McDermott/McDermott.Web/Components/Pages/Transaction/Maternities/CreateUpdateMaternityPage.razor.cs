using DevExpress.Blazor.Reporting;
using DevExpress.Blazor.RichEdit;
using DevExpress.CodeParser;
using DevExpress.XtraReports;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation.Results;
using McDermott.Application.Dtos.Transaction;
using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using McDermott.Extentions;
using McDermott.Web.Components.Pages.Reports;
using McDermott.Web.Extentions;
using Microsoft.AspNetCore.Components.Web;
using QuestPDF.Fluent;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanServiceAncCommand;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanServiceAncDetailCommand;

namespace McDermott.Web.Components.Pages.Transaction.Maternities
{
    public partial class CreateUpdateMaternityPage
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
        private List<string> RiskOfFallingDetail = [];

        private List<AwarenessDto> Awareness { get; set; } = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];

        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];

        private string FormUrl = "clinic-service/maternities";
        private bool PanelVisible = false;
        private bool IsLoading = false;
        [Parameter] public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private bool IsStatus(EnumStatusGeneralConsultantService status) => GeneralConsultanService.Status == status;

        private EnumStatusGeneralConsultantService StagingText { get; set; } = EnumStatusGeneralConsultantService.Confirmed;
        private GeneralConsultanServiceDto GeneralConsultanService { get; set; } = new();
        private UserDto UserForm { get; set; } = new();
        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport { get; set; } = new();

        #endregion Binding

        #region CPPT

        private IGrid GridCppt { get; set; }
        private IReadOnlyList<object> SelectedDataItemsCPPT { get; set; } = [];
        private int FocusedGridTabCPPTRowVisibleIndex { get; set; }
        private List<DiagnosisDto> Diagnoses = [];
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs = [];
        private List<NursingDiagnosesDto> NursingDiagnoses = [];

        private async Task NewItemCPPT_Click()
        {
            await GridCppt.StartEditNewRowAsync();
        }

        private async Task RefreshCPPT_Click()
        {
            await LoadDataCPPT();
        }

        #region Searching

        private int pageSizeGridCPPT { get; set; } = 10;
        private int totalCountGridCPPT = 0;
        private int activePageIndexTotalCountGridCPPT { get; set; } = 0;
        private string searchTermGridCPPT { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedGridCPPT(string searchText)
        {
            searchTermGridCPPT = searchText;
            await LoadDataCPPT(0, pageSizeGridCPPT);
        }

        private async Task OnpageSizeGridCPPTIndexChangedGridCPPT(int newpageSizeGridCPPT)
        {
            pageSizeGridCPPT = newpageSizeGridCPPT;
            await LoadDataCPPT(0, newpageSizeGridCPPT);
        }

        private async Task OnPageIndexChangedGridCPPT(int newPageIndex)
        {
            await LoadDataCPPT(newPageIndex, pageSizeGridCPPT);
        }

        #endregion Searching

        private async Task LoadDataCPPT(int pageIndex = 0, int pageSizeGridCPPT = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemsCPPT = [];
                var ab = await Mediator.Send(new GetGeneralConsultanCPPTsQuery
                {
                    SearchTerm = searchTermGridCPPT ?? "",
                    Predicate = x => x.GeneralConsultanServiceId == GeneralConsultanService.Id
                });
                GeneralConsultanCPPTs = ab.Item1;
                totalCountGridCPPT = ab.PageCount;
                activePageIndexTotalCountGridCPPT = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task EditItemCPPT_Click()
        {
            try
            {
                PanelVisible = true;
                await GridCppt.StartEditRowAsync(FocusedGridTabCPPTRowVisibleIndex);

                var a = (GridCppt.GetDataItem(FocusedGridTabCPPTRowVisibleIndex) as GeneralConsultanCPPTDto ?? new());
                NursingDiagnoses = (await Mediator.Send(new GetNursingDiagnosesQuery
                {
                    Predicate = x => x.Id == a.NursingDiagnosesId
                })).Item1;
                Diagnoses = (await Mediator.Send(new GetDiagnosisQuery { Predicate = x => x.Id == a.DiagnosisId })).Item1;

                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItemCPPT_Click()
        {
            GridCppt.ShowRowDeleteConfirmation(FocusedGridTabCPPTRowVisibleIndex);
        }

        private void GridCPPT_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;
        }

        private void GridTabCPPT_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnSaveCPPT(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;

                var editModel = (GeneralConsultanCPPTDto)e.EditModel;

                editModel.GeneralConsultanServiceId = GeneralConsultanService.Id;

                if (editModel.Id == 0)
                {
                    await Mediator.Send(new CreateGeneralConsultanCPPTRequest(editModel));
                }
                else
                {
                    await Mediator.Send(new UpdateGeneralConsultanCPPTRequest(editModel));
                }

                await LoadDataCPPT(activePageIndexTotalCountGridCPPT, pageSizeGridCPPT);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnDeleteCPPT(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemsCPPT.Count == 0)
                {
                    await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(((GeneralConsultanCPPTDto)e.DataItem).Id));
                }
                else
                {
                    var selectedGeneralConsultanCPPTs = SelectedDataItemsCPPT.Adapt<List<GeneralConsultanCPPTDto>>();
                    await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(ids: selectedGeneralConsultanCPPTs.Select(x => x.Id).ToList()));
                }

                await LoadDataCPPT(activePageIndexTotalCountGridCPPT, pageSizeGridCPPT);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion CPPT

        #region ANC

        #region Region

        #region ComboboxReference

        private DxComboBox<GeneralConsultanServiceAncDto, string> refReferenceComboBox { get; set; }
        private int ReferenceComboBoxIndex { get; set; } = 0;
        private int totalCountReference = 0;

        private async Task OnSearchReference()
        {
            await LoadDataReference();
            //SelectedReference = GeneralConsultanService.Reference;
            //await LoadDataAnc();
        }

        private async Task OnSearchReferenceIndexIncrement()
        {
            if (ReferenceComboBoxIndex < (totalCountReference - 1))
            {
                ReferenceComboBoxIndex++;
                await LoadDataReference(ReferenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchReferenceIndexDecrement()
        {
            if (ReferenceComboBoxIndex > 0)
            {
                ReferenceComboBoxIndex--;
                await LoadDataReference(ReferenceComboBoxIndex, 10);
            }
        }

        private async Task OnSelectedReferenceChanged(GeneralConsultanServiceAncDto e)
        {
            if (e is null)
            {
                GeneralConsultanServiceAnc = new();
                GeneralConsultanServiceAncDetails = [];
                await OnSearchReference();
                return;
            }

            GeneralConsultanServiceAnc = GeneralConsultanServiceAncReferences.FirstOrDefault(x => x.Reference == e.Reference) ?? new();

            await LoadDataAnc();
        }

        private async Task OnInputReferenceChanged(string e)
        {
            ReferenceComboBoxIndex = 0;

            if (e is null)
            {
                GeneralConsultanServiceAnc = new();
                GeneralConsultanServiceAncDetails = [];
                return;
            }

            GeneralConsultanServiceAnc = GeneralConsultanServiceAncReferences.FirstOrDefault(x => x.Reference == e) ?? new();

            await LoadDataReference();
        }

        private string? SelectedReference { get; set; }
        private List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncReferences { get; set; } = [];

        private async Task LoadDataReference(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetGeneralConsultanServiceAncQuery
                {
                    SearchTerm = refReferenceComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Predicate = x => x.PatientId == GeneralConsultanService.PatientId
                });
                //References = result.Item1.Select(z => z.Reference).ToList();
                //References = References.Distinct().ToList();
                GeneralConsultanServiceAncReferences = result.Item1;
                totalCountReference = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxReference

        #endregion Region

        private IGrid GridAnc { get; set; }
        private IReadOnlyList<object> SelectedDataItemsAnc { get; set; } = [];
        private int FocusedGridTabAncRowVisibleIndex { get; set; }
        private List<string> References { get; set; } = [];
        private List<GeneralConsultanServiceAncDto> GeneralCosultanServiceAncs { get; set; } = [];

        private GeneralConsultanServiceAncDetailDto GeneralConsultanServiceAncDetail { get; set; } = new();

        private async Task NewItemAnc_Click()
        {
            GeneralConsultanServiceAncDetail = new();
            GeneralConsultanServiceAncDetail.BB = GeneralConsultanService.Weight.ToInt32();
            GeneralConsultanServiceAncDetail.TD = GeneralConsultanService.Systolic.ToInt32();
            GeneralConsultanServiceAncDetail.TD2 = GeneralConsultanService.DiastolicBP.ToInt32();
            GeneralConsultanServiceAncDetail.Suhu = GeneralConsultanService.Temp.ToInt32();

            await GridAnc.StartEditNewRowAsync();
        }

        private async Task RefreshAnc_Click()
        {
            await LoadDataAnc();
        }

        #region Searching

        private int pageSizeGridAnc { get; set; } = 10;
        private int totalCountGridAnc = 0;
        private int activePageIndexTotalCountGridAnc { get; set; } = 0;
        private string searchTermGridAnc { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedGridAnc(string searchText)
        {
            searchTermGridAnc = searchText;
            await LoadDataAnc(0, pageSizeGridAnc);
        }

        private async Task OnpageSizeGridAncIndexChangedGridAnc(int newpageSizeGridAnc)
        {
            pageSizeGridAnc = newpageSizeGridAnc;
            await LoadDataAnc(0, newpageSizeGridAnc);
        }

        private async Task OnPageIndexChangedGridAnc(int newPageIndex)
        {
            await LoadDataAnc(newPageIndex, pageSizeGridAnc);
        }

        #endregion Searching

        private List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncs { get; set; } = [];
        private List<GeneralConsultanServiceAncDetailDto> GeneralConsultanServiceAncDetails { get; set; } = [];

        private async Task LoadDataAnc(int pageIndex = 0, int pageSizeGridAnc = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemsAnc = [];
                var ab = await Mediator.Send(new GetGeneralConsultanServiceAncDetailQuery
                {
                    SearchTerm = searchTermGridAnc ?? "",
                    Predicate = x => refReferenceComboBox != null && x.GeneralConsultanServiceAncId == GeneralConsultanServiceAnc.Id,
                });
                GeneralConsultanServiceAncDetails = ab.Item1;
                totalCountGridAnc = ab.PageCount;
                //var aa = GeneralConsultanServiceAncDetails.FirstOrDefault(x => refReferenceComboBox != null && x.Reference == refReferenceComboBox.Text);
                //IsReadOnlyAncFieldAdd = aa?.IsReadOnly ?? false;
                activePageIndexTotalCountGridAnc = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task EditItemAnc_Click()
        {
            try
            {
                PanelVisible = true;
                await GridAnc.StartEditRowAsync(FocusedGridTabAncRowVisibleIndex);
                GeneralConsultanServiceAncDetail = new();
                GeneralConsultanServiceAncDetail = (GridAnc.GetDataItem(FocusedGridTabAncRowVisibleIndex) as GeneralConsultanServiceAncDetailDto ?? new());
                //NursingDiagnoses = (await Mediator.Send(new GetNursingDiagnosesQuery(predicate: x => x.Id == a.NursingDiagnosesId))).Item1;
                //Diagnoses = (await Mediator.Send(new GetDiagnosisQuery(predicate: x => x.Id == a.DiagnosisId))).Item1;

                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItemAnc_Click()
        {
            GridAnc.ShowRowDeleteConfirmation(FocusedGridTabAncRowVisibleIndex);
        }

        private void GridAnc_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabAncRowVisibleIndex = args.VisibleIndex;
        }

        private bool IsReadOnlyAncField { get; set; } = false;
        private bool IsReadOnlyAncFieldAdd { get; set; } = false;

        private void GridTabAnc_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabAncRowVisibleIndex = args.VisibleIndex;

            if (args.DataItem is not null)
                IsReadOnlyAncField = ((GeneralConsultanServiceAncDetailDto)args.DataItem).IsReadOnly;
        }

        private async Task OnSaveAnc(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;

                //var editModel = (GeneralConsultanServiceAncDetailDto)e.EditModel;
                var editModel = GeneralConsultanServiceAncDetail;

                //var cekRef = References.FirstOrDefault(x => x == refReferenceComboBox.Text);
                //if (cekRef is not null)
                //    editModel.Reference = cekRef;
                ////else
                ////    editModel.Reference= GeneralConsultanService.Reference;

                if (editModel.Id == 0)
                {
                    editModel.GeneralConsultanServiceAncId = GeneralConsultanServiceAnc.Id;
                    //editModel.Date = DateTime.Now;
                    var x = await Mediator.Send(new CreateGeneralConsultanServiceAncDetailRequest(editModel));

                    //await LoadDataReference();
                    //SelectedReference = x.Reference;

                    //var ab = await Mediator.Send(new GetGeneralConsultanServiceAncQuery
                    //{
                    //    SearchTerm = searchTermGridAnc ?? "",
                    //    Predicate = x => SelectedReference != null && x.Reference == SelectedReference && x.PatientId == GeneralConsultanService.PatientId,
                    //});
                    //GeneralConsultanServiceAncs = ab.Item1;
                    //totalCountGridAnc = ab.PageCount;
                    //var aa = GeneralConsultanServiceAncs.FirstOrDefault(x => SelectedReference != null && x.Reference == SelectedReference);
                    //IsReadOnlyAncFieldAdd = aa?.IsReadOnly ?? false;
                }
                else
                {
                    await Mediator.Send(new UpdateGeneralConsultanServiceAncDetailRequest(editModel));
                }
                GeneralConsultanServiceAncDetail = new();
                await LoadDataAnc(activePageIndexTotalCountGridAnc, pageSizeGridAnc);

                if (string.IsNullOrWhiteSpace(GeneralConsultanService.ReferenceAnc))
                {
                    GeneralConsultanService.ReferenceAnc = GeneralConsultanServiceAnc.Reference;

                    await Mediator.Send(new UpdateGCReferenceAnc
                    {
                        Id = GeneralConsultanService.Id,
                        ReferenceAnc = GeneralConsultanServiceAnc.Reference
                    });
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnDeleteAnc(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemsAnc.Count == 0)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceAncRequest
                    {
                        Id = ((GeneralConsultanServiceAnc)e.DataItem).Id
                    });
                }
                else
                {
                    var selectedGeneralConsultanAncs = SelectedDataItemsAnc.Adapt<List<GeneralConsultanServiceAnc>>();
                    await Mediator.Send(new DeleteGeneralConsultanServiceAncRequest
                    {
                        Ids = selectedGeneralConsultanAncs.Select(x => x.Id).ToList()
                    });
                }

                await LoadDataAnc(activePageIndexTotalCountGridAnc, pageSizeGridCPPT);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ANC

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                return;
            }
        }

        protected void DateChangedHPHT(DateTime? d)
        {
            if (d is null)
            {
                GeneralConsultanServiceAnc.UK = null;
                GeneralConsultanService.HPL = null;
                return;
            }

            GeneralConsultanServiceAnc.HPHT = d;
            GeneralConsultanServiceAnc.HPL = d.GetValueOrDefault().AddDays(280);

            // Contoh HPHT (Hari Pertama Haid Terakhir)
            DateTime hpht = d.GetValueOrDefault();

            // Tambahkan 14 hari ke HPHT
            DateTime hphtPlus14 = hpht.AddDays(14);

            // Tanggal hari ini
            DateTime today = DateTime.Today;

            // Hitung selisih hari
            int selisihHari = (today - hphtPlus14).Days;

            // Hitung usia kehamilan dalam minggu
            GeneralConsultanServiceAnc.UK = selisihHari / 7;
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
            var a = ((GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned) || GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Midwife)));

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
                        DateOfBirth = x.Patient == null ? null : x.Patient.DateOfBirth,
                        IdCardAddress1 = x.Patient != null ? x.Patient.IdCardAddress1 : "",
                        Occupational = new Occupational
                        {
                            Name = x.Patient.Occupational == null ? "" : x.Patient.Occupational.Name,
                        },
                        OccupationalId = x.Patient == null ? null : x.Patient.OccupationalId,
                        BloodType = x.Patient.BloodType != null ? x.Patient.BloodType : "",

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

                    PPKRujukanCode = x.PPKRujukanCode,
                    PPKRujukanName = x.PPKRujukanName,
                    ReferVerticalSpesialisParentSpesialisName = x.ReferVerticalSpesialisParentSpesialisName,
                    ReferVerticalSpesialisParentSubSpesialisName = x.ReferVerticalSpesialisParentSubSpesialisName,
                    ReferReason = x.ReferReason,

                    PregnancyStatusA = x.PregnancyStatusA,
                    PregnancyStatusG = x.PregnancyStatusG,
                    PregnancyStatusP = x.PregnancyStatusP,
                    HPHT = x.HPHT,
                    HPL = x.HPL,

                    PatientNextVisitSchedule = x.PatientNextVisitSchedule,
                    ReferenceAnc = x.ReferenceAnc,
                    BMHP = x.BMHP,
                    KdPrognosa = x.KdPrognosa,
                    ReferVerticalSpesialisParentSubSpesialisCode = x.ReferVerticalSpesialisParentSubSpesialisCode,
                    ReferDateVisit = x.ReferDateVisit,
                    ReferralNo = x.ReferralNo,
                    VisitNumber = x.VisitNumber
                }
            });

            if (result.Status == EnumStatusGeneralConsultantService.Midwife || result.Status == EnumStatusGeneralConsultantService.Midwife)
            {
                result = await GetClinicalAssesmentPatientHistory(result);
            }

            return result;
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

        #endregion Print Rujukan BPJS + PRB

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

                if (result.Status == EnumStatusGeneralConsultantService.Midwife || result.Status == EnumStatusGeneralConsultantService.Finished)
                {
                    if (!string.IsNullOrWhiteSpace(GeneralConsultanService.ReferenceAnc))
                    {
                        var ph = await Mediator.Send(new GetGeneralConsultanServiceAncQuery
                        {
                            Predicate = x => x.Reference == GeneralConsultanService.ReferenceAnc
                        });
                        GeneralConsultanServiceAncReferences = ph.Item1;

                        GeneralConsultanServiceAnc = await Mediator.Send(new GetSingleGeneralConsultanServiceAncQuery
                        {
                            Predicate = x => x.Reference == GeneralConsultanService.ReferenceAnc
                        });

                        if (GeneralConsultanServiceAnc != null)
                        {
                            var ab = await Mediator.Send(new GetGeneralConsultanServiceAncDetailQuery
                            {
                                Predicate = x => x.GeneralConsultanServiceAncId == GeneralConsultanServiceAnc.Id,
                            });
                            GeneralConsultanServiceAncDetails = ab.Item1;
                        }
                    }
                }

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:
                        StagingText = EnumStatusGeneralConsultantService.Confirmed;
                        break;

                    case EnumStatusGeneralConsultantService.Confirmed:
                        StagingText = EnumStatusGeneralConsultantService.Midwife;
                        break;

                    case EnumStatusGeneralConsultantService.Midwife:
                        StagingText = EnumStatusGeneralConsultantService.Finished;

                        if (GeneralConsultanService.PratitionerId is null)
                        {
                            if ((Convert.ToBoolean(UserLogin.IsUser) || Convert.ToBoolean(UserLogin.IsAdmin)) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                            {
                                await LoadPhysicions(predicate: x => x.Id == UserLogin.Id);
                                GeneralConsultanService.PratitionerId = UserLogin.Id;
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

                await LoadPatient(predicate: x => x.Id == GeneralConsultanService.PatientId && x.IsPatient == true && x.Gender == "Female");
                await LoadService(predicate: x => x.Id == GeneralConsultanService.ServiceId && x.IsMaternity == true);

                if (!IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    await LoadPhysicions(predicate: GeneralConsultanService.PratitionerId is null ? null : x => x.Id == GeneralConsultanService.PratitionerId);
                }

                if (!string.IsNullOrWhiteSpace(GeneralConsultanService.Payment))
                {
                    await LoadInsurancePolicy(predicate: GeneralConsultanService.InsurancePolicyId is null ? null : x => x.Id == GeneralConsultanService.InsurancePolicyId);
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
            }
            else
            {
                await LoadPatient();
                await LoadService();
                await LoadPhysicions();
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

        #region ComboboxNursingDiagnoses

        private DxComboBox<NursingDiagnosesDto, long?> refNursingDiagnosesComboBox { get; set; }
        private int NursingDiagnosesComboBoxIndex { get; set; } = 0;
        private int totalCountNursingDiagnoses = 0;

        private async Task OnSearchNursingDiagnoses()
        {
            await LoadDataNursingDiagnoses();
        }

        private async Task OnSearchNursingDiagnosesIndexIncrement()
        {
            if (NursingDiagnosesComboBoxIndex < (totalCountNursingDiagnoses - 1))
            {
                NursingDiagnosesComboBoxIndex++;
                await LoadDataNursingDiagnoses(NursingDiagnosesComboBoxIndex, 10);
            }
        }

        private async Task OnSearchNursingDiagnosesIndexDecrement()
        {
            if (NursingDiagnosesComboBoxIndex > 0)
            {
                NursingDiagnosesComboBoxIndex--;
                await LoadDataNursingDiagnoses(NursingDiagnosesComboBoxIndex, 10);
            }
        }

        private async Task OnInputNursingDiagnosesChanged(string e)
        {
            NursingDiagnosesComboBoxIndex = 0;
            await LoadDataNursingDiagnoses();
        }

        private async Task OnClickTabCPPT()
        {
            await LoadDataCPPT();

            if (IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
            {
                await LoadDataNursingDiagnoses();
                await LoadDataDiagnoses();
            }
        }

        private async Task LoadDataNursingDiagnoses(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetNursingDiagnosesQuery
                {
                    SearchTerm = refNursingDiagnosesComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize
                });
                NursingDiagnoses = result.Item1;
                totalCountNursingDiagnoses = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxNursingDiagnoses

        #region ComboBox InsurancePolicy

        private InsurancePolicyDto SelectedInsurancePolicy { get; set; } = new();

        private async Task SelectedItemChanged(InsurancePolicyDto e)
        {
            if (e is null)
            {
                SelectedInsurancePolicy = new();
                await LoadInsurancePolicy();
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

                await LoadInsurancePolicy(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsInsurancePolicy?.Dispose();
                _ctsInsurancePolicy = null;
            }
        }

        private async Task LoadInsurancePolicy(string? e = "", Expression<Func<InsurancePolicy, bool>>? predicate = null)
        {
            try
            {
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

        #region ComboboxDiagnoses

        private DxComboBox<DiagnosisDto, long?> refDiagnosesComboBox { get; set; }
        private int DiagnosesComboBoxIndex { get; set; } = 0;
        private int totalCountDiagnoses = 0;

        private async Task OnSearchDiagnoses()
        {
            await LoadDataDiagnoses();
        }

        private async Task OnSearchDiagnosesIndexIncrement()
        {
            if (DiagnosesComboBoxIndex < (totalCountDiagnoses - 1))
            {
                DiagnosesComboBoxIndex++;
                await LoadDataDiagnoses(DiagnosesComboBoxIndex, 10);
            }
        }

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

        private async Task OnSearchDiagnosesIndexDecrement()
        {
            if (DiagnosesComboBoxIndex > 0)
            {
                DiagnosesComboBoxIndex--;
                await LoadDataDiagnoses(DiagnosesComboBoxIndex, 10);
            }
        }

        private async Task OnInputDiagnosesChanged(string e)
        {
            DiagnosesComboBoxIndex = 0;
            await LoadDataDiagnoses();
        }

        private async Task LoadDataDiagnoses(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetDiagnosisQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = refDiagnosesComboBox?.Text ?? ""
                });
                Diagnoses = result.Item1;
                totalCountDiagnoses = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxDiagnoses

        #region ComboBox Patient

        private async Task SelectedItemChanged(UserDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.InsurancePolicyId = null;
                InsurancePolicies.Clear();
                GeneralConsultanService.Patient = new();
                UserForm = new();
                await LoadPatient();
            }
            else
            {
                GeneralConsultanService.PatientId = e.Id;
                GeneralConsultanService.Patient = e;
                UserForm = e;
                await LoadInsurancePolicy();
            }
        }

        private CancellationTokenSource? _ctsUser;

        private async Task OnInputPatient(ChangeEventArgs e)
        {
            try
            {
                _ctsUser?.Cancel();
                _ctsUser?.Dispose();
                _ctsUser = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsUser.Token);

                await LoadPatient(e.Value?.ToString() ?? "", x => x.IsPatient == true && x.Gender == "Female");
            }
            finally
            {
                _ctsUser?.Dispose();
                _ctsUser = null;
            }
        }

        private async Task LoadPatient(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.IsPatient == true && x.Gender == "Female";
                Patients = await Mediator.QueryGetComboBox<User, UserDto>(e,
                    predicate,
                    select: x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        DateOfBirth = x.DateOfBirth,
                        Gender = x.Gender,
                        NoId = x.NoId,
                        CurrentMobile = x.CurrentMobile,
                        IdCardAddress1 = x.IdCardAddress1,
                        Occupational = new Occupational
                        {
                            Name = x.Occupational == null ? "" : x.Occupational.Name,
                        },
                        BloodType = x.BloodType,

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
                await LoadPhysicions();
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

                await LoadService(e.Value?.ToString() ?? "", predicate: x => x.IsMaternity == true);
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
                predicate ??= x => x.IsMaternity == true;

                Services = await Mediator.QueryGetComboBox<Service, ServiceDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Service

        private void SelectedDateMaternityChanged(DateTime? e)
        {
            GeneralConsultanService.EndMaternityLeave = null;

            if (e is null)
                return;

            GeneralConsultanService.StartMaternityLeave = e;
            GeneralConsultanService.EndMaternityLeave = e.GetValueOrDefault().AddMonths(3).Date;
        }

        #region ComboBox Physicions

        private async Task SelectedItemChangedPhysicion(UserDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.PratitionerId = null;
                await LoadPhysicions();
            }
            else
                GeneralConsultanService.PratitionerId = e.Id;
        }

        private CancellationTokenSource? _ctsPhysicions;

        private async Task OnInputPhysicion(ChangeEventArgs e)
        {
            try
            {
                _ctsPhysicions?.Cancel();
                _ctsPhysicions?.Dispose();
                _ctsPhysicions = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsPhysicions.Token);

                await LoadPhysicions(e.Value?.ToString() ?? "", predicate: x => x.IsDoctor == true && x.IsPhysicion == true);
            }
            finally
            {
                _ctsPhysicions?.Dispose();
                _ctsPhysicions = null;
            }
        }

        private async Task LoadPhysicions(string? e = "", Expression<Func<User, bool>>? predicate = null)
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

        #endregion ComboBox Physicions

        private async Task SelectedItemPaymentChanged(string e)
        {
            GeneralConsultanService.Payment = null;
            GeneralConsultanService.InsurancePolicyId = null;

            if (e is null)
                return;

            InsurancePolicies = (await Mediator.Send(new GetInsurancePolicyQuery
            {
                Predicate = x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJS == e.Equals("BPJS") && x.Active == true,
                Select = x => new InsurancePolicy
                {
                    Id = x.Id,
                    Insurance = new Insurance
                    {
                        Name = x.Insurance == null ? "" : x.Insurance.Name,
                    },
                    PolicyNumber = x.PolicyNumber,
                    PstPrb = x.PstPrb,
                    PstProl = x.PstProl
                }
            })).Item1;
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
                    GeneralConsultanService = await Mediator.Send(new CancelGeneralConsultanServiceRequest(GeneralConsultanService));
                    StagingText = EnumStatusGeneralConsultantService.Canceled;

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

        private bool IsClosePopupVisible { get; set; }
        private string CloseNote { get; set; } = string.Empty;

        private void ShowClosePopup()
        {
            IsClosePopupVisible = true;
        }

        private void CancelClosePopup()
        {
            IsClosePopupVisible = false;
        }

        private async Task ConfirmCloseANC()
        {
            // Simulate saving to the database
            await SaveCloseANCAsync(CloseNote);

            // Close the popup
            IsClosePopupVisible = false;
        }

        private async Task SaveCloseANCAsync(string note)
        {
            GeneralConsultanServiceAnc.Notes = note;
            GeneralConsultanServiceAnc.Status = EnumStatusGeneralConsultanServiceAnc.Closed;

            var searchAnc = (await Mediator.Send(new GetGeneralConsultanServiceAncDetailQuery
            {
                IsGetAll = true,
                Predicate = x => x.GeneralConsultanServiceAncId == GeneralConsultanServiceAnc.Id && x.IsReadOnly == false,
                Select = x => x
            })).Item1;

            searchAnc.ForEach(x => x.IsReadOnly = true);

            await Mediator.Send(new UpdateListGeneralConsultanServiceAncDetailRequest(searchAnc));

            await HandleValidSubmitAnc();
        }

        private GeneralConsultanServiceAncDto GeneralConsultanServiceAnc { get; set; } = new();

        private async Task HandleValidSubmitAnc()
        {
            IsLoading = true;

            try
            {
                GeneralConsultanServiceAnc.GeneralConsultanServiceId = GeneralConsultanService.Id;
                GeneralConsultanServiceAnc.PatientId = GeneralConsultanService.PatientId.GetValueOrDefault();

                if (GeneralConsultanServiceAnc.Id == 0)
                {
                    GeneralConsultanServiceAnc = await Mediator.Send(new CreateGeneralConsultanServiceAncRequest(GeneralConsultanServiceAnc));
                    await LoadDataReference();
                }
                else
                    GeneralConsultanServiceAnc = await Mediator.Send(new UpdateGeneralConsultanServiceAncRequest(GeneralConsultanServiceAnc));

                GeneralConsultanService.ReferenceAnc = GeneralConsultanServiceAnc.Reference;

                await Mediator.Send(new UpdateGCReferenceAnc
                {
                    Id = GeneralConsultanService.Id,
                    ReferenceAnc = GeneralConsultanServiceAnc.Reference
                });

                await GetGeneralConsultanServiceById();
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

        private async Task LoadDataAncHeader()
        {
        }

        private async Task HandleValidSubmit()
        {
            IsLoading = true;

            try
            {
                GeneralConsultanService.IsMaternity = true;

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

                GeneralConsultanServiceDto res = new();

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
                                CurrentMobile = userForm.CurrentMobile
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
                    //else
                    //{
                    //    await SendPCareRequestUpdateStatusPanggilAntrean(1);
                    //}
                }

                if (GeneralConsultanService.InsurancePolicyId is not null && GeneralConsultanService.InsurancePolicyId != 0 && GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Midwife))
                {
                    var isSuccessAddKunjungan = await SendPcareRequestKunjungan();

                    if (!isSuccessAddKunjungan)
                    {
                        IsLoading = false;
                        return;
                    }
                }

                if (IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Midwife))
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

                if ((!PopUpConfirmation && IsContinueCPPT) || IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
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
                            StagingText = EnumStatusGeneralConsultantService.Midwife;
                            break;

                        case EnumStatusGeneralConsultantService.Midwife:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Midwife;
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

                    GeneralConsultanService.IsMaternity = true;

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
                    }
                    else
                    {
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

                    if (newGC.Status == EnumStatusGeneralConsultantService.Finished)
                    {
                        var a = await Mediator.Send(new GetSingleGeneralConsultanServiceAncQuery
                        {
                            Predicate = x => x.Reference == GeneralConsultanService.ReferenceAnc && x.PatientId == GeneralConsultanService.PatientId,
                            Select = x => new GeneralConsultanServiceAnc
                            {
                                Id = x.Id
                            }
                        });
                        var searchAnc = (await Mediator.Send(new GetGeneralConsultanServiceAncDetailQuery
                        {
                            IsGetAll = true,
                            Predicate = x => x.GeneralConsultanServiceAncId == a.Id && x.IsReadOnly == false,
                            Select = x => x
                        })).Item1;

                        searchAnc.ForEach(x => x.IsReadOnly = true);

                        await Mediator.Send(new UpdateListGeneralConsultanServiceAncDetailRequest(searchAnc));

                        await LoadDataAnc();
                    }

                    ToastService.ClearSuccessToasts();
                    IsContinueCPPT = false;
                    Id = newGC.Id;
                    GeneralConsultanService = await GetGeneralConsultanServiceById();

                    if (PageMode == EnumPageMode.Create.GetDisplayName())
                        NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanService.Id}");

                    if (IsStatus(EnumStatusGeneralConsultantService.Midwife))
                    {
                        if (GeneralConsultanService.PratitionerId is null)
                        {
                            if ((Convert.ToBoolean(UserLogin.IsUser) || Convert.ToBoolean(UserLogin.IsAdmin)) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                            {
                                await LoadPhysicions(predicate: x => x.Id == UserLogin.Id);
                                GeneralConsultanService.PratitionerId = UserLogin.Id;
                            }
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

        private async Task<bool> SendPcareRequestKunjungan()
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
                        dynamic dataz = JsonConvert.DeserializeObject(responseApi.Item1);

                        ToastService.ShowError($"{dataz.metadata.message}");

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
            //NavigationManager.NavigateTo($"pharmacy/presciptions/{GeneralConsultanService.Id}");
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
    }
}
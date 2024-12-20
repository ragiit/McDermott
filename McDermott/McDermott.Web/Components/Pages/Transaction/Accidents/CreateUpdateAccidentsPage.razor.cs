using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation.Results;
using McDermott.Application.Features.Services;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Transaction.AccidentCommand;
using Project = McDermott.Domain.Entities.Project;

namespace McDermott.Web.Components.Pages.Transaction.Accidents
{
    public partial class CreateUpdateAccidentsPage
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

        private string FormUrl = "clinic-service/accidents";
        private bool PanelVisible = false;
        private bool IsLoading = false;
        [Parameter] public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private bool IsStatus(EnumStatusAccident status) => Accident.Status == status;

        private EnumStatusAccident StagingText = EnumStatusAccident.MedicalTreatment;
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
                Diagnoses = (await Mediator.Send(new GetDiagnosisQuery
                {
                    Predicate = x => x.Id == a.DiagnosisId
                })).Item1;

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

            GeneralConsultanService.TypeRegistration = "Accident";

            PanelVisible = false;
        }

        [SupplyParameterFromQuery] public long? Id { get; set; }

        private bool ReadOnlyForm()
        {
            //var a = ((
            //    GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned) ||
            //    GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.NurseStation) ||
            //    GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician)
            //    ));

            var a = IsStatus(EnumStatusAccident.Done);

            return a;
        }

        private async Task<AccidentDto> GetAccidentById()
        {
            var result = await Mediator.Send(new GetSingleAccidentQuery
            {
                Predicate = x => x.Id == this.Id,
                Select = x => new Accident
                {
                    Id = x.Id,
                    GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                    SafetyPersonnelId = x.SafetyPersonnelId,
                    EmployeeClass = x.EmployeeClass,
                    Sent = x.Sent,
                    AccidentLocation = x.AccidentLocation,
                    DateOfOccurrence = x.DateOfOccurrence,
                    DateOfFirstTreatment = x.DateOfFirstTreatment,
                    AreaOfYard = x.AreaOfYard,
                    ProjectId = x.ProjectId,
                    Status = x.Status,

                    // Employee Cause Of Injury
                    EmployeeCauseOfInjury1 = x.EmployeeCauseOfInjury1,
                    EmployeeCauseOfInjury2 = x.EmployeeCauseOfInjury2,
                    EmployeeCauseOfInjury3 = x.EmployeeCauseOfInjury3,
                    EmployeeCauseOfInjury4 = x.EmployeeCauseOfInjury4,
                    EmployeeCauseOfInjury5 = x.EmployeeCauseOfInjury5,
                    EmployeeCauseOfInjury6 = x.EmployeeCauseOfInjury6,
                    EmployeeCauseOfInjury7 = x.EmployeeCauseOfInjury7,
                    EmployeeCauseOfInjury8 = x.EmployeeCauseOfInjury8,
                    EmployeeCauseOfInjury9 = x.EmployeeCauseOfInjury9,
                    EmployeeCauseOfInjury10 = x.EmployeeCauseOfInjury10,
                    EmployeeCauseOfInjury11 = x.EmployeeCauseOfInjury11,
                    EmployeeCauseOfInjury12 = x.EmployeeCauseOfInjury12,
                    EmployeeCauseOfInjury13 = x.EmployeeCauseOfInjury13,
                    EmployeeCauseOfInjury14 = x.EmployeeCauseOfInjury14,
                    SelectedEmployeeCauseOfInjury1 = x.SelectedEmployeeCauseOfInjury1,
                    SelectedEmployeeCauseOfInjury2 = x.SelectedEmployeeCauseOfInjury2,
                    SelectedEmployeeCauseOfInjury3 = x.SelectedEmployeeCauseOfInjury3,
                    SelectedEmployeeCauseOfInjury4 = x.SelectedEmployeeCauseOfInjury4,
                    SelectedEmployeeCauseOfInjury5 = x.SelectedEmployeeCauseOfInjury5,
                    SelectedEmployeeCauseOfInjury6 = x.SelectedEmployeeCauseOfInjury6,
                    SelectedEmployeeCauseOfInjury7 = x.SelectedEmployeeCauseOfInjury7,
                    SelectedEmployeeCauseOfInjury8 = x.SelectedEmployeeCauseOfInjury8,
                    SelectedEmployeeCauseOfInjury9 = x.SelectedEmployeeCauseOfInjury9,
                    SelectedEmployeeCauseOfInjury10 = x.SelectedEmployeeCauseOfInjury10,
                    SelectedEmployeeCauseOfInjury11 = x.SelectedEmployeeCauseOfInjury11,
                    SelectedEmployeeCauseOfInjury12 = x.SelectedEmployeeCauseOfInjury12,
                    SelectedEmployeeCauseOfInjury13 = x.SelectedEmployeeCauseOfInjury13,
                    SelectedEmployeeCauseOfInjury14 = x.SelectedEmployeeCauseOfInjury14,

                    // Nature Of Injury
                    NatureOfInjury1 = x.NatureOfInjury1,
                    NatureOfInjury2 = x.NatureOfInjury2,
                    NatureOfInjury3 = x.NatureOfInjury3,
                    NatureOfInjury4 = x.NatureOfInjury4,
                    NatureOfInjury5 = x.NatureOfInjury5,
                    NatureOfInjury6 = x.NatureOfInjury6,
                    NatureOfInjury7 = x.NatureOfInjury7,
                    NatureOfInjury8 = x.NatureOfInjury8,
                    SelectedNatureOfInjury1 = x.SelectedNatureOfInjury1,
                    SelectedNatureOfInjury2 = x.SelectedNatureOfInjury2,
                    SelectedNatureOfInjury3 = x.SelectedNatureOfInjury3,
                    SelectedNatureOfInjury4 = x.SelectedNatureOfInjury4,
                    SelectedNatureOfInjury5 = x.SelectedNatureOfInjury5,
                    SelectedNatureOfInjury6 = x.SelectedNatureOfInjury6,
                    SelectedNatureOfInjury7 = x.SelectedNatureOfInjury7,
                    SelectedNatureOfInjury8 = x.SelectedNatureOfInjury8,

                    // Part Of Body
                    PartOfBody1 = x.PartOfBody1,
                    PartOfBody2 = x.PartOfBody2,
                    PartOfBody3 = x.PartOfBody3,
                    PartOfBody4 = x.PartOfBody4,
                    PartOfBody5 = x.PartOfBody5,
                    PartOfBody6 = x.PartOfBody6,
                    PartOfBody7 = x.PartOfBody7,
                    PartOfBody8 = x.PartOfBody8,
                    PartOfBody9 = x.PartOfBody9,
                    PartOfBody10 = x.PartOfBody10,
                    PartOfBody11 = x.PartOfBody11,
                    PartOfBody12 = x.PartOfBody12,
                    SelectedPartOfBody1 = x.SelectedPartOfBody1,
                    SelectedPartOfBody2 = x.SelectedPartOfBody2,
                    SelectedPartOfBody3 = x.SelectedPartOfBody3,
                    SelectedPartOfBody4 = x.SelectedPartOfBody4,
                    SelectedPartOfBody5 = x.SelectedPartOfBody5,
                    SelectedPartOfBody6 = x.SelectedPartOfBody6,
                    SelectedPartOfBody7 = x.SelectedPartOfBody7,
                    SelectedPartOfBody8 = x.SelectedPartOfBody8,
                    SelectedPartOfBody9 = x.SelectedPartOfBody9,
                    SelectedPartOfBody10 = x.SelectedPartOfBody10,
                    SelectedPartOfBody11 = x.SelectedPartOfBody11,
                    SelectedPartOfBody12 = x.SelectedPartOfBody12,

                    // Treatment
                    Treatment1 = x.Treatment1,
                    Treatment2 = x.Treatment2,
                    Treatment3 = x.Treatment3,
                    Treatment4 = x.Treatment4,
                    Treatment5 = x.Treatment5,
                    Treatment6 = x.Treatment6,
                    Treatment7 = x.Treatment7,
                    SelectedTreatment1 = x.SelectedTreatment1,
                    SelectedTreatment2 = x.SelectedTreatment2,
                    SelectedTreatment3 = x.SelectedTreatment3,
                    SelectedTreatment4 = x.SelectedTreatment4,
                    SelectedTreatment5 = x.SelectedTreatment5,
                    SelectedTreatment6 = x.SelectedTreatment6,
                    SelectedTreatment7 = x.SelectedTreatment7,

                    EmployeeDescription = x.EmployeeDescription,
                }
            });

            return result;
        }

        private async Task<GeneralConsultanServiceDto> GetGeneralConsultanServiceById()
        {
            var result = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
            {
                Predicate = x => x.Id == Accident.GeneralConsultanServiceId,

                Select = x => new GeneralConsultanService
                {
                    Id = x.Id,
                    Status = x.Status,
                    PatientId = x.PatientId,
                    Patient = new User
                    {
                        Id = x.PatientId.GetValueOrDefault(),
                        Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        NIP = x.Patient == null ? string.Empty : x.Patient.NIP,
                        NoId = x.Patient == null ? string.Empty : x.Patient.NoId,
                        CurrentMobile = x.Patient == null ? string.Empty : x.Patient.CurrentMobile,
                        DateOfBirth = x.Patient == null ? null : x.Patient.DateOfBirth,
                        Department = new Department
                        {
                            Name = x.Patient.Department == null ? "" : x.Patient.Department.Name
                        },

                        Supervisor = new User
                        {
                            Name = x.Patient.Supervisor == null ? "" : x.Patient.Supervisor.Name
                        },
                        Gender = x.Patient == null ? string.Empty : x.Patient.Gender
                    },
                    PratitionerId = x.PratitionerId,
                    Pratitioner = new User
                    {
                        Name = x.Pratitioner == null ? string.Empty : x.Pratitioner.Name,
                    },
                    Payment = x.Payment,
                    InsurancePolicyId = x.InsurancePolicyId,
                    RegistrationDate = x.RegistrationDate,
                    TypeRegistration = x.TypeRegistration,

                    InformationFrom = x.InformationFrom,
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
                }
            });

            result = await GetClinicalAssesmentPatientHistory(result);

            return result;
        }

        private async Task LoadData()
        {
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                var result = await GetAccidentById();
                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo(FormUrl);
                    return;
                }

                Accident = new();
                Accident = result;

                var resultGC = await GetGeneralConsultanServiceById();
                GeneralConsultanService = new();
                GeneralConsultanService = resultGC;

                await LoadSafetyPersonel(predicate: x => x.Id == Accident.SafetyPersonnelId);
                await LoadProject(predicate: x => x.Id == GeneralConsultanService.ProjectId);

                UserForm = resultGC.Patient ?? new();

                await LoadPatient(predicate: x => x.IsPatient == true && x.IsEmployee == true && x.Id == GeneralConsultanService.PatientId);
                await LoadPhysicion(predicate: x => x.IsDoctor == true && x.Id == GeneralConsultanService.PratitionerId);

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

                RefreshStagingText();
            }
            else
            {
                await LoadPatient();
                await LoadPhysicion();
                await LoadProject();
            }

            Awareness = await Mediator.Send(new GetAwarenessQuery());
        }

        #region ComboboxNursingDiagnoses

        #region ComboBox Project

        private async Task SelectedItemChanged(ProjectDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.ProjectId = new();
                await LoadProject();
            }
            else
                GeneralConsultanService.ProjectId = e.Id;
        }

        private CancellationTokenSource? _ctsProject;

        private async Task OnInputProject(ChangeEventArgs e)
        {
            try
            {
                _ctsProject?.Cancel();
                _ctsProject?.Dispose();
                _ctsProject = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsProject.Token);

                await LoadProject(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsProject?.Dispose();
                _ctsProject = null;
            }
        }

        private List<ProjectDto> Projects { get; set; } = [];

        private async Task LoadProject(string? e = "", Expression<Func<Project, bool>>? predicate = null)
        {
            try
            {
                Projects = await Mediator.QueryGetComboBox<Project, ProjectDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Project

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

            await LoadDataNursingDiagnoses();
            await LoadDataDiagnoses();
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

                if (GeneralConsultanService.PatientId == Accident.SafetyPersonnelId)
                    await LoadSafetyPersonel();

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
                    NIP = x.NIP,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth,
                    NoId = x.NoId,
                    CurrentMobile = x.CurrentMobile,

                    Department = new Department
                    {
                        Name = x.Department == null ? "" : x.Department.Name
                    },

                    Supervisor = new User
                    {
                        Name = x.Supervisor == null ? "" : x.Supervisor.Name
                    }
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Patient

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

        private void OnSelectAccidentLocation(string e)
        {
            if (e is null)
                return;

            if (e.Equals("Outside"))
            {
                Accident.Sent = "Hospital";
            }

            RefreshStagingText();
        }

        #region ComboBox SafetyPersonel

        private UserDto SelectedSafetyPersonel { get; set; } = new();

        private async Task SelectedItemPersonelChanged(UserDto e)
        {
            if (e is null)
            {
                Accident.SafetyPersonnelId = 0;
                await LoadSafetyPersonel();
            }
            else
                Accident.SafetyPersonnelId = e.Id;
        }

        private CancellationTokenSource? _ctsSafetyPersonel;

        private async Task OnInputSafetyPersonel(ChangeEventArgs e)
        {
            try
            {
                _ctsSafetyPersonel?.Cancel();
                _ctsSafetyPersonel?.Dispose();
                _ctsSafetyPersonel = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsSafetyPersonel.Token);

                await LoadSafetyPersonel(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsSafetyPersonel?.Dispose();
                _ctsSafetyPersonel = null;
            }
        }

        private List<UserDto> SafetyPersonels { get; set; } = [];

        private async Task LoadSafetyPersonel(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                predicate = x => x.IsPatient == true && x.IsEmployee == true && x.Id != GeneralConsultanService.PatientId;
                SafetyPersonels = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate, select: x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox SafetyPersonel

        private void SelectedDateMaternityChanged(DateTime? e)
        {
            GeneralConsultanService.EndMaternityLeave = null;

            if (e is null)
                return;

            GeneralConsultanService.StartMaternityLeave = e;
            GeneralConsultanService.EndMaternityLeave = e.GetValueOrDefault().AddMonths(3).Date;
        }

        #region ComboboxService

        private DxComboBox<ServiceDto, long?> refServiceComboBox { get; set; }
        private int ServiceComboBoxIndex { get; set; } = 0;
        private int totalCountService = 0;

        private async Task OnSearchService()
        {
            await LoadDataService();
        }

        private async Task OnSearchServiceIndexIncrement()
        {
            if (ServiceComboBoxIndex < (totalCountService - 1))
            {
                ServiceComboBoxIndex++;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchServiceIndexDecrement()
        {
            if (ServiceComboBoxIndex > 0)
            {
                ServiceComboBoxIndex--;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnInputServiceChanged(string e)
        {
            ServiceComboBoxIndex = 0;
            await LoadDataService();
        }

        private async Task LoadDataService(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetServiceQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = refServiceComboBox?.Text ?? ""
                });
                Services = result.Item1;
                totalCountService = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxService

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
                predicate ??= x => x.IsDoctor == true && x.IsPhysicion == true;

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

        private async Task HandleValidSubmit(int state)
        {
            IsLoading = true;

            try
            {
                // Execute the validator
                ValidationResult results = new AccidentGeneralValidator().Validate(GeneralConsultanService);

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
                ValidationResult results2 = new AccidentValidator().Validate(Accident);

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

                GeneralConsultanService.IsAccident = true;

                if (!GeneralConsultanService.Payment!.Equals("Personal") && (GeneralConsultanService.InsurancePolicyId is null || GeneralConsultanService.InsurancePolicyId == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                // Save
                if (state == 1)
                {
                    if (GeneralConsultanService.Id == 0 && Accident.Id == 0)
                    {
                        var res = await Mediator.Send(new CreateGeneralConsultanServiceRequest(GeneralConsultanService));
                        Accident.GeneralConsultanServiceId = res.Id;
                        Accident = await Mediator.Send(new CreateAccidentRequest(Accident));
                    }
                    else
                    {
                        await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        await Mediator.Send(new UpdateAccidentRequest(Accident));
                    }
                }
                // State == 2 is Confirm
                else
                {
                    if (GeneralConsultanService.Payment is not null && GeneralConsultanService.Payment.Equals("BPJS"))
                    {
                        switch (Accident.Status)
                        {
                            case EnumStatusAccident.Draft:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    Accident.Status = EnumStatusAccident.MedicalTreatment;
                                }
                                else
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.HospitalizationReferral;
                                    }
                                    else
                                    {
                                        Accident.Status = EnumStatusAccident.PatientControlMonitoring;
                                    }
                                }
                                break;

                            case EnumStatusAccident.MedicalTreatment:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.HospitalizationReferral;
                                    }
                                    else
                                    {
                                        Accident.Status = EnumStatusAccident.PatientControlMonitoring;
                                    }
                                }
                                break;

                            case EnumStatusAccident.HospitalizationReferral:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.PatientControlMonitoring;
                                    }
                                }
                                else
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.PatientControlMonitoring;
                                    }
                                }
                                break;

                            case EnumStatusAccident.PatientControlMonitoring:
                                Accident.Status = EnumStatusAccident.Done;
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (Accident.Status)
                        {
                            case EnumStatusAccident.Draft:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    Accident.Status = EnumStatusAccident.MedicalTreatment;
                                }
                                else
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.HospitalizationReferral;
                                    }
                                    else
                                    {
                                        Accident.Status = EnumStatusAccident.Done;
                                    }
                                }
                                break;

                            case EnumStatusAccident.MedicalTreatment:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.HospitalizationReferral;
                                    }
                                    else
                                        Accident.Status = EnumStatusAccident.Done;
                                }
                                break;

                            case EnumStatusAccident.HospitalizationReferral:

                                Accident.Status = EnumStatusAccident.Done;
                                break;

                            default:
                                break;
                        }
                    }

                    if (GeneralConsultanService.Id == 0 && Accident.Id == 0)
                    {
                        var res = await Mediator.Send(new CreateGeneralConsultanServiceRequest(GeneralConsultanService));
                        Accident.GeneralConsultanServiceId = res.Id;
                        Accident = await Mediator.Send(new CreateAccidentRequest(Accident));
                    }
                    else
                    {
                        await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        await Mediator.Send(new UpdateAccidentRequest(Accident));
                    }
                }

                GeneralConsultanService = await GetGeneralConsultanServiceById();
                Id = Accident.Id;
                Accident = await GetAccidentById();

                if (PageMode == EnumPageMode.Create.GetDisplayName())
                    NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={Accident.Id}");

                RefreshStagingText();
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

        private void RefreshStagingText()
        {
            if (GeneralConsultanService.Payment is not null && GeneralConsultanService.Payment.Equals("BPJS"))
            {
                switch (Accident.Status)
                {
                    case EnumStatusAccident.Draft:
                        if (Accident.AccidentLocation.Equals("Inside"))
                        {
                            StagingText = EnumStatusAccident.MedicalTreatment;
                        }
                        else
                        {
                            if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                            {
                                StagingText = EnumStatusAccident.HospitalizationReferral;
                            }
                            else
                            {
                                StagingText = EnumStatusAccident.PatientControlMonitoring;
                            }
                        }
                        break;

                    case EnumStatusAccident.MedicalTreatment:
                        if (Accident.AccidentLocation.Equals("Inside"))
                        {
                            if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                            {
                                StagingText = EnumStatusAccident.HospitalizationReferral;
                            }
                            else
                            {
                                StagingText = EnumStatusAccident.PatientControlMonitoring;
                            }
                        }
                        break;

                    case EnumStatusAccident.HospitalizationReferral:
                        StagingText = EnumStatusAccident.PatientControlMonitoring;
                        break;

                    case EnumStatusAccident.PatientControlMonitoring:
                        StagingText = EnumStatusAccident.Done;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (Accident.Status)
                {
                    case EnumStatusAccident.Draft:
                        if (Accident.AccidentLocation.Equals("Inside"))
                        {
                            StagingText = EnumStatusAccident.MedicalTreatment;
                        }
                        else
                        {
                            if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                            {
                                StagingText = EnumStatusAccident.HospitalizationReferral;
                            }
                            else
                            {
                                StagingText = EnumStatusAccident.Done;
                            }
                        }
                        break;

                    case EnumStatusAccident.MedicalTreatment:
                        if (Accident.AccidentLocation.Equals("Inside"))
                        {
                            if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                            {
                                StagingText = EnumStatusAccident.HospitalizationReferral;
                            }
                            else
                                StagingText = EnumStatusAccident.Done;
                        }
                        break;

                    case EnumStatusAccident.HospitalizationReferral:

                        StagingText = EnumStatusAccident.Done;
                        break;

                    default:
                        break;
                }
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

        private async Task PrintAccident()
        {
            try
            {
                if (GeneralConsultanService.Id == 0)
                    return;

                var gen = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == GeneralConsultanService.Id))).Item1.FirstOrDefault() ?? new();
                //var accident = (await Mediator.Send(new GetAccidentQuery(x => x.GeneralConsultanServiceId == gen.Id))).FirstOrDefault() ?? new();
                var accident = await Mediator.Send(new GetSingleAccidentQuery
                {
                    Predicate = x => x.GeneralConsultanServiceId == gen.Id,
                    Select = x => new Accident
                    {
                    }
                });
                isPrint = true;
                var mergeFields = new Dictionary<string, string>
                {
                    {"<<EmployeeName>>", gen?.Patient?.Name.GetDefaultValue() },
                    {"<<EmployeeNIP>>", gen?.Patient?.NIP?.GetDefaultValue() ?? "-"},
                    {"<<EmployeeDepartment>>", gen?.Patient?.Department?.Name.GetDefaultValue() ?? "-"},
                    {"<<DateOfOccurence>>", accident.DateOfOccurrence.ToString("dd MMMM yyyy")},
                    {"<<TimeOccurence>>", accident.DateOfOccurrence.ToString("HH:mm:ss")},
                    {"<<DateTreatment>>", accident.DateOfFirstTreatment.ToString("dd MMMM yyyy")},
                    {"<<TimeTreatment>>", accident.DateOfFirstTreatment.ToString("HH:mm:ss")},
                    {"<<AreaOfYard>>", accident.AreaOfYard?.GetDefaultValue() ?? "-"},
                    {"<<SupervisorName>>", gen?.Patient?.Supervisor?.Name.GetDefaultValue() ?? "-"},
                };

                //Field dateField = await documentAPI.Fields.Create(, "DATE");

                //await documentAPI.Fields.UpdateAsync(dateField);

                DocumentContent = await DocumentProvider.GetDocumentAsync("AccidentForms.docx", mergeFields);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
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

                //if (IsStatus(EnumStatusAccident.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
                //{
                //    if (GeneralConsultanCPPTs.Count == 0)
                //    {
                //        if (IsContinueCPPT)
                //            PopUpConfirmation = false;
                //        else
                //        {
                //            PopUpConfirmation = true;
                //            return;
                //        }
                //    }
                //    else
                //        IsContinueCPPT = true;
                //}
                //else
                //    IsContinueCPPT = true;

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

        private void HandleClosePopupReferTo()
        {
            IsReferTo = false; // Tutup popup
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

        private bool isPrint { get; set; } = false;
        private DevExpress.Blazor.RichEdit.Document documentAPI;
        public byte[]? DocumentContent;

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

        #region Assesment of Injury

        #region Nature of Injury

        private void OnSelectSent(string e)
        {
            if (e is null)
            {
                return;
            }

            RefreshStagingText();

            if (e.Equals("Hospital"))
            {
                StagingText = EnumStatusAccident.HospitalizationReferral;
            }
        }

        public static IEnumerable<string> NatureOfInjury1 { get; set; } = new List<string>
        {
            "Foreign body",
            "Eye irritation"
        };

        public static IEnumerable<string> NatureOfInjury2 { get; set; } = new List<string>
        {
            "Abrasion",
            "Laceration",
            "Puncture",
            "Scratch"
        };

        public static IEnumerable<string> NatureOfInjury3 { get; set; } = new List<string>
        {
            "Bruise",
            "Contussion",
            "Crushing"
        };

        public static IEnumerable<string> NatureOfInjury4 { get; set; } = new List<string>
        {
            "Sprain"
        };

        public static IEnumerable<string> NatureOfInjury5 { get; set; } = new List<string>
        {
            "Fracture",
            "Dislocation"
        };

        public static IEnumerable<string> NatureOfInjury6 { get; set; } = new List<string>
        {
            "Burn",
            "Chemical burn",
            "Electric burn"
        };

        public static IEnumerable<string> NatureOfInjury7 { get; set; } = new List<string>
        {
            "Occupational illness",
            "LBP",
            "Dermatitis"
        };

        public static IEnumerable<string> NatureOfInjury8 { get; set; } = new List<string>
        {
            "Asphyxia",
            "Intoxication",
            "Amputation",
            "Concussion"
        };

        #endregion Nature of Injury

        #region Part of Body

        public static IEnumerable<string> PartOfBody1 { get; set; } = [
            "Head",
            "face",
            "neck"
        ];

        public static IEnumerable<string> PartOfBody2 { get; set; } = [
            "Eye"
        ];

        public static IEnumerable<string> PartOfBody3 { get; set; } = [
            "Ear"
        ];

        public static IEnumerable<string> PartOfBody4 { get; set; } = [
            "Back"
        ];

        public static IEnumerable<string> PartOfBody5 { get; set; } = [
            "Trunk (except back and internal)"
        ];

        public static IEnumerable<string> PartOfBody6 { get; set; } = [
            "Arm"
        ];

        public static IEnumerable<string> PartOfBody7 { get; set; } = [
            "Hand and wrist"
        ];

        public static IEnumerable<string> PartOfBody8 { get; set; } = [
            "Fingers"
        ];

        public static IEnumerable<string> PartOfBody9 { get; set; } = [
            "Leg"
        ];

        public static IEnumerable<string> PartOfBody10 { get; set; } = [
            "Feet and ankles"
        ];

        public static IEnumerable<string> PartOfBody11 { get; set; } = [
            "Toes"
        ];

        public static IEnumerable<string> PartOfBody12 { get; set; } = [
            "Internal and Others"
        ];

        public static IEnumerable<string> EmployeeCauseOfInjury1 { get; set; } = new[]
        {
            "Falls",
            "Slips",
            "Trips"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury2 { get; set; } = new[]
        {
            "Fire",
            "hot materials"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury3 { get; set; } = new[]
        {
            "Pressurized gas"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury4 { get; set; } = new[]
        {
    "Foreign body"
};

        public static IEnumerable<string> EmployeeCauseOfInjury5 { get; set; } = new[]
        {
            "Electricity"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury6 { get; set; } = new[]
        {
            "Sandblast"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury7 { get; set; } = new[]
        {
            "Animal",
            "plant"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury8 { get; set; } = new[]
        {
            "Struck",
            "caught (by, against, between) objects"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury9 { get; set; } = new[]
        {
            "Chemicals"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury10 { get; set; } = new[]
        {
            "Welding flash"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury11 { get; set; } = new[]
        {
            "Vehicle accident"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury12 { get; set; } = new[]
        {
            "Overexertion"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury13 { get; set; } = new[]
        {
            "Smoke",
            "gas"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury14 { get; set; } = new[]
        {
            "Others"
        };

        #endregion Part of Body

        #region Treatment

        public static IEnumerable<string> Treatment1 { get; set; } = [
            "Cleaning and dressing",
            "Removal of foreign body with cotton wool",
            "Removal of foreign body with needle and magnet"
        ];

        public static IEnumerable<string> Treatment2 { get; set; } = [
            "Stitching"
        ];

        public static IEnumerable<string> Treatment3 { get; set; } = [
            "Splinting"
        ];

        public static IEnumerable<string> Treatment4 { get; set; } = [
            "Antibiotics"
        ];

        public static IEnumerable<string> Treatment5 { get; set; } = [
            "Painkillers"
        ];

        public static IEnumerable<string> Treatment6 { get; set; } = [
            "Tetanus toxoid injection, 0.5 cc"
        ];

        public static IEnumerable<string> Treatment7 { get; set; } = [
            "Others"
        ];

        #endregion Treatment

        public static IEnumerable<string> EmployeeClass { get; set; } = [
           "FA",
           "MTC",
           "RWC",
           "LTA",
           "FATALITY",
           "OCCUPATIONAL ILLNESS",
       ];

        public static string GetNullText(IEnumerable<string> natureOfInjury) =>
   string.Join(" / ", natureOfInjury);

        private AccidentDto Accident { get; set; } = new();

        #endregion Assesment of Injury

        private void HandleUserFormChanged(UserDto updatedUserForm)
        {
            UserForm = updatedUserForm;
            StateHasChanged(); // Pastikan Parent diperbarui
        }
    }
}
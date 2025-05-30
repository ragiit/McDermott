﻿using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Application.Dtos.ClaimUserManagement;
using McDermott.Application.Features.Services;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.AllQueries.CountModelCommand;
using static McDermott.Application.Features.Commands.ClaimUserManagement.ClaimHistoryCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PharmacyCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramAttendanceCommand;

namespace McDermott.Web.Components.Pages.Patient.Patients
{
    public partial class CreateUpdatePatientsPage
    {
        #region Family Relation

        private List<PatientFamilyRelationDto> PatientFamilyRelations { get; set; } = [];
        private List<ClaimHistoryDto> getClaimHistory { get; set; } = [];
        private List<UserDto> UserPatients { get; set; } = [];
        private List<FamilyDto> Families { get; set; } = [];
        public IGrid GridFamilyRelation { get; set; }
        public IGrid GridUserClaim { get; set; }
        private IReadOnlyList<object> SelectedDataItemsFamilyRelation { get; set; } = [];
        private int FocusedRowVisibleIndexFamilyRelation { get; set; }
        private int FocusedRowVisibleIndexUserClaim { get; set; }

        private async Task NewItemFamilyRelation_Click()
        {
            await GridFamilyRelation.StartEditNewRowAsync();
        }

        private PatientFamilyRelationDto TempFamilyRelation { get; set; } = new();

        private async Task EditItemFamilyRelation_Click(IGrid context)
        {
            await GridFamilyRelation.StartEditRowAsync(FocusedRowVisibleIndexFamilyRelation);

            var a = (GridFamilyRelation.GetDataItem(FocusedRowVisibleIndexFamilyRelation) as PatientFamilyRelationDto ?? new());

            PanelVisible = true;
            Families = (await Mediator.QueryGetHelper<Family, FamilyDto>(predicate: x => x.Id == a.FamilyId)).Item1;
            UserPatients = (await Mediator.Send(new GetUserQuery2(
                   x => x.IsPatient == true && x.Id == a.FamilyMemberId,
                   searchTerm: null,
                   pageSize: 1,
                   pageIndex:
                   0,
                   includes: [],
                   select: x => new User
                   {
                       Id = x.Id,
                       Name = x.Name,
                       NoRm = x.NoRm,
                       Email = x.Email,
                       MobilePhone = x.MobilePhone,
                       Gender = x.Gender,
                       DateOfBirth = x.DateOfBirth,
                   }
            ))).Item1;

            TempFamilyRelation = a;

            PanelVisible = false;
        }

        private void DeleteItemGridFamilyRelation_Click()
        {
            GridFamilyRelation.ShowRowDeleteConfirmation(FocusedRowVisibleIndexFamilyRelation);
        }

        private void GridFamilyRelation_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexFamilyRelation = args.VisibleIndex;
        }

        private void DeleteItemFamilyRelation_Click()
        {
            GridFamilyRelation.ShowRowDeleteConfirmation(FocusedRowVisibleIndexFamilyRelation);
        }

        private void GridUserClaim_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexUserClaim = args.VisibleIndex;
        }

        private object Data { get; set; }

        private async Task LoadDataFamilyRelation()
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var dataSource = new GridDevExtremeDataSource<PatientFamilyRelation>(await Mediator.Send(new GetQueryPatientFamilyRelation
                {
                    Predicate = x => x.PatientId == UserForm.Id,
                    Select = x => new PatientFamilyRelation
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        FamilyMemberId = x.FamilyMemberId,
                        FamilyMember = new User
                        {
                            Name = x.FamilyMember.Name
                        },
                        FamilyId = x.FamilyId,
                        Family = new Family
                        {
                            Name = x.Family.Name,
                            InverseRelation = new Family
                            {
                                Name = x.Family.InverseRelation.Name
                            }
                        },
                    }
                }))
                {
                    CustomizeLoadOptions = (loadOptions) =>
                    {
                        loadOptions.PrimaryKey = ["Id"];
                        loadOptions.PaginateViaPrimaryKey = true;
                    }
                };
                Data = dataSource;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSaveFamilyRelation(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (PatientFamilyRelation)e.EditModel;

                var pat2 = (await Mediator.Send(new GetFamilyQuery(x => x.Id == editModel.FamilyId))).Item1.FirstOrDefault() ?? new();
                if (editModel.Id == 0)
                {
                    var patients = new List<PatientFamilyRelationDto>
                    {
                        new()
                        {
                            PatientId = UserForm.Id,
                            FamilyMemberId = editModel.FamilyMemberId,
                            FamilyId = editModel.FamilyId,
                        },
                        new()
                        {
                            PatientId = editModel.FamilyMemberId,
                            FamilyMemberId = UserForm.Id,
                            FamilyId = pat2.InverseRelationId,
                        },
                    };

                    await Mediator.Send(new CreateListPatientFamilyRelationRequest(patients));
                }
                else
                {
                    var patients = new List<PatientFamilyRelationDto>
                    {
                        new()
                        {
                            Id = editModel.Id,
                            PatientId = UserForm.Id,
                            FamilyMemberId = editModel.FamilyMemberId,
                            FamilyId = editModel.FamilyId,
                        }
                    };

                    if (TempFamilyRelation.FamilyMemberId == editModel.FamilyMemberId && TempFamilyRelation.PatientId == UserForm.Id)
                    {
                        var b = (await Mediator.Send(new GetSinglePatientFamilyRelationQuery
                        {
                            Predicate = x => x.PatientId == editModel.FamilyMemberId && x.FamilyMemberId == editModel.PatientId
                        }));

                        if (b is not null)
                        {
                            patients.Add(new PatientFamilyRelationDto
                            {
                                Id = b.Id,
                                PatientId = editModel.FamilyMemberId,
                                FamilyMemberId = UserForm.Id,
                                FamilyId = pat2.InverseRelationId,
                            });
                        }
                    }
                    else
                    {
                        //var b = (await Mediator.Send(new GetPatientFamilyRelationQuery(x => x.PatientId == TempFamilyRelation.FamilyMemberId && x.FamilyMemberId == TempFamilyRelation.PatientId && x.FamilyId == TempFamilyRelation.Family.InverseRelationId))).Item1.FirstOrDefault() ?? null;
                        var b = await Mediator.Send(new GetSinglePatientFamilyRelationQuery
                        {
                            Predicate = x => x.PatientId == TempFamilyRelation.FamilyMemberId && x.FamilyMemberId == TempFamilyRelation.PatientId && x.FamilyId == TempFamilyRelation.Family.InverseRelationId
                        });

                        patients.Add(new PatientFamilyRelationDto
                        {
                            Id = b.Id,
                            PatientId = editModel.FamilyMemberId,
                            FamilyMemberId = UserForm.Id,
                            FamilyId = pat2.InverseRelationId,
                        });
                    }

                    await Mediator.Send(new UpdateListPatientFamilyRelationRequest(patients));
                }

                await LoadDataFamilyRelation();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnDeleteFamilyRelation(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemsFamilyRelation is null || SelectedDataItemsFamilyRelation.Count == 1)
                {
                    var a = ((PatientFamilyRelation)e.DataItem);
                    var p = await Mediator.Send(new GetSinglePatientFamilyRelationQuery { Predicate = x => x.FamilyMemberId == a.PatientId && x.PatientId == a.FamilyMemberId });

                    await Mediator.Send(new DeletePatientFamilyRelationRequest(a.Id));
                    await Mediator.Send(new DeletePatientFamilyRelationRequest(p.Id));
                }
                else
                {
                    var selectedMenus = SelectedDataItemsFamilyRelation.Adapt<List<PatientFamilyRelation>>();

                    // Loop through each selected family relation and delete the corresponding relations
                    foreach (var item in selectedMenus)
                    {
                        // Query for a related PatientFamilyRelation for each selected item
                        var p = await Mediator.Send(new GetSinglePatientFamilyRelationQuery { Predicate = x => x.FamilyMemberId == item.PatientId && x.PatientId == item.FamilyMemberId });

                        // Delete both the selected relation and the counterpart relation
                        await Mediator.Send(new DeletePatientFamilyRelationRequest(item.Id));
                        await Mediator.Send(new DeletePatientFamilyRelationRequest(p.Id));
                    }
                }
                await LoadDataFamilyRelation();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #region ComboboxUserFamilyRelation

        private DxComboBox<UserDto, long> refUserFamilyRelationComboBox { get; set; }
        private int UserFamilyRelationComboBoxIndex { get; set; } = 0;
        private int totalCountUserFamilyRelation = 0;

        private async Task OnSearchUserFamilyRelation()
        {
            await LoadDataUserFamilyRelation();
        }

        private async Task OnSearchUserFamilyRelationIndexIncrement()
        {
            if (UserFamilyRelationComboBoxIndex < (totalCountUserFamilyRelation - 1))
            {
                UserFamilyRelationComboBoxIndex++;
                await LoadDataUserFamilyRelation(UserFamilyRelationComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUserFamilyRelationndexDecrement()
        {
            if (UserFamilyRelationComboBoxIndex > 0)
            {
                UserFamilyRelationComboBoxIndex--;
                await LoadDataUserFamilyRelation(UserFamilyRelationComboBoxIndex, 10);
            }
        }

        private async Task OnInputUserFamilyRelationChanged(string e)
        {
            UserFamilyRelationComboBoxIndex = 0;
            await LoadDataUserFamilyRelation();
        }

        private async Task LoadDataUserFamilyRelation(int pageIndex = 0, int pageSize = 10, long? UserFamilyRelationId = null)
        {
            try
            {
                PanelVisible = true;
                //var result = await Mediator.Send(new GetUserQuery2(x => x.IsPatient == true && x.Id != UserForm.Id, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refUserFamilyRelationComboBox?.Text ?? ""));
                var result = await Mediator.Send(new GetUserQuery2(
                    x => x.IsPatient == true && x.Id != UserForm.Id,
                    searchTerm: refUserFamilyRelationComboBox?.Text ?? "",
                    pageSize: pageSize,
                    pageIndex:
                    pageIndex,
                    includes: [],
                    select: x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                    }
                ));
                UserPatients = result.Item1;
                totalCountUserFamilyRelation = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxUserFamilyRelation

        #region ComboboxFamily

        private DxComboBox<FamilyDto, long?> refFamilyComboBox { get; set; }
        private int FamilyComboBoxIndex { get; set; } = 0;
        private int totalCountFamily = 0;

        private async Task OnSearchFamily()
        {
            await LoadDataFamily();
        }

        private async Task OnSearchFamilyIndexIncrement()
        {
            if (FamilyComboBoxIndex < (totalCountFamily - 1))
            {
                FamilyComboBoxIndex++;
                await LoadDataFamily(FamilyComboBoxIndex, 10);
            }
        }

        private async Task OnSearchFamilyIndexDecrement()
        {
            if (FamilyComboBoxIndex > 0)
            {
                FamilyComboBoxIndex--;
                await LoadDataFamily(FamilyComboBoxIndex, 10);
            }
        }

        private async Task OnInputFamilyChanged(string e)
        {
            FamilyComboBoxIndex = 0;
            await LoadDataFamily();
        }

        private async Task LoadDataFamily(int pageIndex = 0, int pageSize = 10, long? FamilyId = null)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.QueryGetHelper<Family, FamilyDto>(pageIndex, pageSize, refFamilyComboBox?.Text ?? "");
                Families = result.Item1;
                totalCountFamily = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxFamily

        #endregion Family Relation

        #region Grid Vaccination

        private bool IsLoadingGeneralConsultantServiceVaccinations = false;
        private bool IsLoadingUserClaim = false;
        private IGrid GridVaccinations { get; set; }
        private List<GeneralConsultanServiceDto> GeneralConsultanServiceVaccinations { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemVaccinations { get; set; } = [];

        public MarkupString GetIssuePriorityIconHtml(GeneralConsultanServiceDto priority)
        {
            if (priority is not null)
            {
                if (!priority.IsAlertInformationSpecialCase && priority.ClassType is null)
                    return new MarkupString("");

                string priorytyClass = "danger";
                string title = string.Empty;

                if (priority.IsAlertInformationSpecialCase && priority.ClassType is not null)
                    title = $" Priority, {priority.ClassType}";
                else
                {
                    if (priority.ClassType is not null)
                        title = $"{priority.ClassType}";
                    if (priority.IsAlertInformationSpecialCase)
                        title = $" Priority ";
                }

                string html = string.Format("<span class='badge bg-{0} py-1 px-2' title='{1} Priority'>{1}</span>", priorytyClass, title);

                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        private GeneralConsultanServiceDto SelectedGeneralConsultanService { get; set; } = new();
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs { get; set; } = [];
        private bool IsDetailVaccinations { get; set; } = false;

        private async Task OnClickDetailHistoricalRecordPatientVaccinations(GeneralConsultanServiceDto generalConsultanService)
        {
            IsDetailVaccinations = true;
            SelectedGeneralConsultanService = generalConsultanService;
            GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == generalConsultanService.Id));
        }

        #endregion Grid Vaccination

        #region Pop Up

        private async Task OnClickCloseInsurancePolicyPopUp()
        {
            if (UserForm.Id != 0)
            {
                var count = await Mediator.Send(new GetInsurancePolicyCountQuery(x => x.UserId == UserForm.Id));
                InsurancePoliciesCount = count;
            }
        }

        private async Task OnClickClosePrescriptionPopUp()
        {
            if (UserForm.Id != 0)
            {
                var count = await Mediator.Send(new GetPharmacyQuery
                {
                    Predicate = x => x.PatientId == UserForm.Id
                });
                PrescriptionCount = count.Item1.Count;
            }
        }

        #endregion Pop Up

        private List<AwarenessDto> Awareness { get; set; } = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];

        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];

        private List<UserDto> Users = new();
        private UserDto UserForm = new();

        #region KTP Address

        public List<CountryDto> Countries = [];
        public List<ProvinceDto> Provinces = [];
        public List<CityDto> Cities = [];
        public List<DistrictDto> Districts = [];
        public List<VillageDto> Villages = [];

        #endregion KTP Address

        #region Residence  Address

        public List<CountryDto> CountriesResidence = [];
        public List<ProvinceDto> ProvincesResidence = [];
        public List<CityDto> CitiesResidence = [];
        public List<DistrictDto> DistrictsResidence = [];
        public List<VillageDto> VillagesResidence = [];

        #endregion Residence  Address

        private List<OccupationalDto> Occupationals = [];
        public List<GroupDto> Groups = [];
        public List<ReligionDto> Religions = [];

        public List<DepartmentDto> Departments = [];
        public List<JobPositionDto> JobPositions = [];

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
            //}
        }

        [Parameter]
        public long InsurancePoliciesCount { get; set; } = 0;

        public long UserClaimCount { get; set; } = 0;
        public long WellnessAttendanceCount { get; set; } = 0;

        [Parameter]
        public long PrescriptionCount { get; set; } = 0;

        public int VaccinationCount { get; set; } = 0;
        private bool PopUpVisible = false;
        private bool PrescriptionPopUp = false;
        private bool DiseasePopUp = false;
        private bool IsVaccinations = false;
        private bool IsWellnessHistory = false;
        private bool IsUserClaim = false;

        private async Task OnClickSmartButton(string text)
        {
            //NavigationManager.NavigateTo("patient/insurance-policy");
            //var a = new InsurancePolicyPage();
            //a.User = UserForm;

            if (text.Equals("Insurance Policy"))
            {
                PopUpVisible = true;
                return;
            }
            else if (text.Equals("Prescription"))
            {
                PrescriptionPopUp = true;
                return;
            }
            else if (text.Equals("Desease"))
            {
                DiseasePopUp = true;
                return;
            }
            else if (text.Equals("Vaccinations"))
            {
                IsVaccinations = true;
                IsLoadingGeneralConsultantServiceVaccinations = true;
                //GeneralConsultanServiceVaccinations = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.PatientId == UserForm.Id && x.Service != null && x.Service.Name == "Vaccination"))).Item1;
                IsLoadingGeneralConsultantServiceVaccinations = false;
                return;
            }
            else if (text.Equals("User Claim"))
            {
                IsUserClaim = true;
                await LoadData_userClaim();
                return;
            }
            else if (text.Equals("Wellness History"))
            {
                IsWellnessHistory = true;
                await LoadDataAttendances();
                return;
            }

            //TabIndex = text.ToInt32();
        }

        #region Searching

        private int pageSizeAttendances { get; set; } = 10;
        private int totalCountAttendances = 0;
        private int activePageIndexAttendances { get; set; } = 0;
        private string searchTermAttendances { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedAttendances(string searchText)
        {
            searchTermAttendances = searchText;
            await LoadDataAttendances(0, pageSizeAttendances);
        }

        private async Task OnpageSizeAttendancesIndexChanged(int newpageSizeAttendances)
        {
            pageSizeAttendances = newpageSizeAttendances;
            await LoadDataAttendances(0, newpageSizeAttendances);
        }

        private async Task OnPageIndexChangedAttendances(int newPageIndex)
        {
            await LoadDataAttendances(newPageIndex, pageSizeAttendances);
        }

        private IGrid GridAttendance { get; set; }
        private IReadOnlyList<object> SelectedDataItemAttendances { get; set; } = [];
        private List<WellnessProgramAttendanceDto> WellnessProgramAttendances { get; set; } = [];
        private bool LoadingWellness = false;

        private async Task LoadDataAttendances(int pageIndex = 0, int pageSizeAttendances = 10)
        {
            try
            {
                LoadingWellness = true;
                var a = await Mediator.Send(new GetWellnessProgramAttendanceQuery
                {
                    OrderByList =
                    [
                        (x => x.Date, true)
                    ],
                    Predicate = x => x.PatientId == UserForm.Id,
                    SearchTerm = searchTermAttendances,
                    PageIndex = pageIndex,
                    PageSize = pageSizeAttendances,
                    Select = x => new WellnessProgramAttendance
                    {
                        Date = x.Date,
                        WellnessProgram = new WellnessProgram
                        {
                            Name = x.WellnessProgram.Name
                        },
                        WellnessProgramDetail = new WellnessProgramDetail
                        {
                            Name = x.WellnessProgramDetail.Name
                        }
                    }
                });
                WellnessProgramAttendances = a.Item1;
                totalCountAttendances = a.PageCount;
                activePageIndexAttendances = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { LoadingWellness = false; }
        }

        #endregion Searching

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

        private bool PanelVisible { get; set; } = true;

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        public IGrid GridGropMenu { get; set; }
        private bool IsAddMenu { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        private bool EditItemsEnabled { get; set; }
        private bool EditItemsGroupEnabled { get; set; } = false;
        private string GroupName { get; set; }

        [SupplyParameterFromForm]
        private GroupDto Group { get; set; } = new();

        private char Placeholder { get; set; } = '_';

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedUserClaimDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataItemsGroupMenu { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndexGroupMenu { get; set; }
        private int FocusedRowVisibleIndexGroupMenuGroupMenu { get; set; }
        private List<GroupMenuDto> GroupMenus = [];
        private List<GroupMenuDto> DeletedGroupMenus = [];
        private List<MenuDto> Menus = [];

        [Required]
        private IEnumerable<MenuDto> SelectedGroupMenus = [];

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGroupRequest(SelectedDataItems[0].Adapt<GroupDto>().Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GroupDto>>();
                    await Mediator.Send(new DeleteGroupRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private List<SpecialityDto> Specialities = [];

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            Religions = await Mediator.Send(new GetReligionQuery());

            await GetUserInfo();
            await LoadData();

            if (Id.HasValue)
                await LoadComboBoxEdit();
            PanelVisible = false;
            return;
        }

        private async Task LoadComboBoxEdit()
        {
            PanelVisible = true;

            #region KTP Address

            Countries = (await Mediator.Send(new GetCountryQuery
            {
                Predicate = x => x.Id == UserForm.IdCardCountryId,
            })).Item1; Provinces = (await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.Id == UserForm.IdCardProvinceId
            })).Item1;
            Cities = (await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.Id == UserForm.IdCardCityId,
            })).Item1;
            Districts = (await Mediator.Send(new GetDistrictQuery
            {
                Predicate = x => x.Id == UserForm.IdCardDistrictId,
            })).Item1;
            Villages = (await Mediator.QueryGetHelper<Village, VillageDto>(predicate: x => x.Id == UserForm.IdCardVillageId)).Item1;

            #endregion KTP Address

            #region Residence  Address

            CountriesResidence = (await Mediator.Send(new GetCountryQuery
            {
                Predicate = x => x.Id == UserForm.DomicileCountryId,
            })).Item1; Provinces = (await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.Id == UserForm.DomicileProvinceId
            })).Item1;
            CitiesResidence = (await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.Id == UserForm.DomicileCityId,
            })).Item1;
            DistrictsResidence = (await Mediator.Send(new GetDistrictQuery
            {
                Predicate = x => x.Id == UserForm.DomicileDistrictId,
            })).Item1;
            VillagesResidence = (await Mediator.QueryGetHelper<Village, VillageDto>(predicate: x => x.Id == UserForm.DomicileVillageId)).Item1;

            #endregion Residence  Address

            var alergy = (await Mediator.Send(new GetAllergyQuery()));
            FoodAllergies = alergy.Where(x => x.Type == "01").ToList();
            WeatherAllergies = alergy.Where(x => x.Type == "02").ToList();
            PharmacologyAllergies = alergy.Where(x => x.Type == "03").ToList();

            SelectedFoodAllergies = FoodAllergies.Where(x => UserForm.FoodPatientAllergyIds.Contains(x.Id));
            SelectedWeatherAllergies = WeatherAllergies.Where(x => UserForm.WeatherPatientAllergyIds.Contains(x.Id));
            SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => UserForm.PharmacologyPatientAllergyIds.Contains(x.Id));

            await LoadDataFamilyRelation();

            PanelVisible = false;
        }

        private void NewItem_Click()
        {
            ShowForm = true;
            GroupMenus = [];
            Group = new();
        }

        private bool IsLoading { get; set; } = false;

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsGroupEnabled = enabled;
        }

        private IEnumerable<ServiceDto> Services { get; set; } = [];
        private IEnumerable<ServiceDto> SelectedServices { get; set; } = [];

        private async Task LoadData()
        {
            //var result = await MyQuery.GetGroups(HttpClientFactory, 0, 1, Id.HasValue ? Id.ToString() : "");

            var result = await Mediator.Send(new GetUserQuery2(x => x.Id == Id && x.IsPatient == true, 0, 1, includes: []));
            UserForm = new();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("patient/patients");
                    return;
                }

                UserForm = result.Item1.FirstOrDefault() ?? new();

                InsurancePoliciesCount = await Mediator.Send(new GetInsurancePolicyCountQuery(x => x.UserId == UserForm.Id));
                PrescriptionCount = await Mediator.Send(new GetPrescriptionCountQuery(x => x.PatientId == UserForm.Id));
                VaccinationCount = await Mediator.Send(new GetGeneralConsultationCountQuery(x => x.PatientId == UserForm.Id && x.Service != null && x.Service.Name == "Vaccination"));
                UserClaimCount = await Mediator.Send(new GetClaimHistoryCountQuery(x => x.PatientId == UserForm.Id));
                WellnessAttendanceCount = await Mediator.Send(new GetWellnessAttendanceCountQuery(x => x.PatientId == UserForm.Id));
            }
        }

        //private async Task LoadData()
        //{
        //    try
        //    {
        //        PanelVisible = true;
        //        SelectedDataItemsGroupMenu = [];
        //        GroupMenu = new();
        //        SelectedDataItems = [];
        //        Group = new();
        //        ShowForm = false;
        //        GroupMenus = [];
        //        Groups = await Mediator.Send(new GetGroupQuery());
        //        PanelVisible = false;
        //    }
        //    catch (Exception ex) { ex.HandleException(ToastService); }
        //}

        private bool FormValidationState = true;

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await SaveUser();
            else
                FormValidationState = true;
        }

        private bool showPassword = false;
        private string showPasswordIcon = "fa-solid fa-eye-slash";

        private void TogglePasswordVisibility()
        {
            showPassword = !showPassword;

            if (showPassword == true)
            {
                showPasswordIcon = "fa-solid fa-eye";
            }
            else
            {
                showPasswordIcon = "fa-solid fa-eye-slash";
            }
        }

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        [SupplyParameterFromForm]
        private GroupMenuDto GroupMenu { get; set; } = new();

        private void CancelItemGroupMenuGridGropMenu_Click()
        {
            //GroupMenus = [];
            //Group = new();
            //SelectedDataItems = [];
            //SelectedDataItemsGroupMenu = [];
            //ShowForm = false;

            NavigationManager.NavigateTo("patient/patients");
        }

        [Inject]
        public CustomAuthenticationStateProvider CustomAuth { get; set; }

        private IBrowserFile BrowserFile;

        private async Task<bool> CheckExistingNumber(Expression<Func<User, bool>> predicate, string numberType)
        {
            var users = await Mediator.Send(new ValidateUserQuery(predicate));
            var isOkOk = false;
            if (users)
            {
                ToastService.ShowInfo($"{numberType} Number already exists");
                isOkOk = true;
            }
            return isOkOk;
        }

        public async Task<bool> CheckUserFormAsync()
        {
            var checks = new List<Func<Task<bool>>>
            {
                () => CheckExistingNumber(x => UserForm.Id != x.Id && UserForm.NIP != null && x.IsEmployee == true && x.NIP != null && x.NIP.ToLower().Trim().Equals(UserForm.NIP.ToLower().Trim()), "NIP"),
                () => CheckExistingNumber(x => UserForm.Id != x.Id && UserForm.Oracle != null && x.IsEmployee == true && x.Oracle != null && x.Oracle.ToLower().Trim().Equals(UserForm.Oracle.ToLower().Trim()), "Oracle"),
                () => CheckExistingNumber(x => UserForm.Id != x.Id && UserForm.Legacy != null && x.IsEmployee == true && x.Legacy != null && x.Legacy.ToLower().Trim().Equals(UserForm.Legacy.ToLower().Trim()), "Legacy"),
                () => CheckExistingNumber(x => UserForm.Id != x.Id && UserForm.SAP != null && x.IsEmployee == true && x.SAP != null && x.SAP.ToLower().Trim().Equals(UserForm.SAP.ToLower().Trim()), "SAP")
            };

            var isOkOk = true;

            foreach (var check in checks)
            {
                if (await check())
                {
                    isOkOk = false;
                }
            }

            return isOkOk;
        }

        private async Task SaveUser()
        {
            if (!FormValidationState)
                return;

            UserForm.IsPatient = true;

            bool isValid = true;
            var a = await Mediator.Send(new ValidateUserQuery(x => x.Id != UserForm.Id && x.NoId == UserForm.NoId));
            if (a)
            {
                ToastService.ShowInfo("The Identity Number is already exist");
                isValid = false;
            }

            var chekcEmail = await Mediator.Send(new ValidateUserQuery(x => x.Id != UserForm.Id && x.Email == UserForm.Email));
            if (chekcEmail)
            {
                ToastService.ShowInfo("The Email is already exist");
                isValid = false;
            }

            if (Convert.ToBoolean(UserForm.IsEmployee))
            {
                var isOk = await CheckUserFormAsync();
                if (!isOk)
                    isValid = false;
            }

            if (!isValid)
                return;

            if (Convert.ToBoolean(UserForm.IsPatient))
            {
                var date = DateTime.Now;
                var lastId = Users.Where(x => x.IsPatient == true).ToList().LastOrDefault();

                UserForm.NoRm = lastId is null
                         ? $"{date:dd-MM-yyyy}-0001"
                         : $"{date:dd-MM-yyyy}-{(long.Parse(lastId!.NoRm!.Substring(lastId.NoRm.Length - 4)) + 1):0000}";
            }

            if (UserForm.IsSameDomicileAddress)
            {
                UserForm.DomicileAddress1 = UserForm.IdCardAddress1;
                UserForm.DomicileAddress2 = UserForm.IdCardAddress2;
                UserForm.DomicileRtRw = UserForm.IdCardRtRw;
                UserForm.DomicileProvinceId = UserForm.IdCardProvinceId;
                UserForm.DomicileCityId = UserForm.IdCardCityId;
                UserForm.DomicileDistrictId = UserForm.IdCardDistrictId;
                UserForm.DomicileVillageId = UserForm.IdCardVillageId;
                UserForm.DomicileCountryId = UserForm.IdCardCountryId;
            }

            if (!string.IsNullOrWhiteSpace(UserForm.Password))
                UserForm.Password = Helper.HashMD5(UserForm.Password);

            var ax = SelectedServices.Select(x => x.Id).ToList();
            //UserForm.DoctorServiceIds?.AddRange(ax);

            UserForm.WeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds
                ? SelectedWeatherAllergies.Select(x => x.Id).ToList()
                : [];

            UserForm.PharmacologyPatientAllergyIds = UserForm.IsPharmacologyPatientAllergyIds
                ? SelectedPharmacologyAllergies.Select(x => x.Id).ToList()
                : [];

            UserForm.FoodPatientAllergyIds = UserForm.IsFoodPatientAllergyIds
                ? SelectedFoodAllergies.Select(x => x.Id).ToList()
                : [];

            if (UserForm.Id == 0)
            {
                await FileUploadService.UploadFileAsync(BrowserFile);
                UserForm = await Mediator.Send(new CreateUserRequest(UserForm));
            }
            else
            {
                //var userDtoSipFile = SelectedDataItems[0].Adapt<UserDto>().SipFile;

                //if (UserForm.SipFile != userDtoSipFile)
                //{
                //    if (UserForm.SipFile != null)
                //        Helper.DeleteFile(UserForm.SipFile);

                //    if (userDtoSipFile != null)
                //        Helper.DeleteFile(userDtoSipFile);
                //}

                var result = await Mediator.Send(new UpdateUserRequest(UserForm));

                //if (UserForm.SipFile != userDtoSipFile)
                //{
                //    if (UserForm.SipFile != null)
                //        await FileUploadService.UploadFileAsync(BrowserFile);
                //}

                if (UserLogin.Id == result.Id)
                {
                    await JsRuntime.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);

                    var aa = (CustomAuthenticationStateProvider)CustomAuth;
                    await aa.UpdateAuthState(string.Empty);

                    await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_INFO, Helper.Encrypt(JsonConvert.SerializeObject(result)), 2);
                }
            }

            NavigationManager.NavigateTo($"patient/patients/{EnumPageMode.Update.GetDisplayName()}?Id={UserForm.Id}", true);
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "group_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
            ]);
        }

        private List<ExportFileData> ExportFileDatasGroupMenus =
      [
          new()
            {
                Column = "Menu",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Parent Menu",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Is Create",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Is Read",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Is Update",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Is Delete",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Is Import",
                Notes = "Select one: Yes/No"
            }
      ];

        private async Task ExportToExcel2()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "group_menu_template.xlsx", ExportFileDatasGroupMenus.ToList());
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ImportFile2()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput2");
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Name" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var countries = new List<GroupDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new GroupDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                        };

                        if (!Groups.Any(x => x.Name.Trim().ToLower() == country?.Name?.Trim().ToLower()))
                            countries.Add(country);
                    }

                    await Mediator.Send(new CreateListGroupRequest(countries));

                    await LoadData();
                    SelectedDataItems = [];

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            //var Group = (GroupDto)e.EditModel;
            //var menus = await JSRuntime.InvokeAsync<IEnumerable<SelectedDropdown>>("getSelectedOptions", selectedMenus);

            //if (string.IsNullOrWhiteSpace(Group!.Name))
            //    return;

            //if (Group.Id == 0)
            //{
            //    var existingName = await Mediator.Send(new GetGroupByNameQuery(Group!.Name));

            //    if (existingName is not null)
            //        return;

            //    var result = await Mediator.Send(new CreateGroupRequest(Group));

            //    var request = new List<GroupMenuDto>();

            //    var group = await Mediator.Send(new GetGroupByNameQuery(Group.Name));

            //    if (menus.Where(x => x.Value == "0").Any())
            //    {
            //        Menus.ForEach(z =>
            //        {
            //            {
            //                request.Add(new GroupMenuDto
            //                {
            //                    MenuId = z.Id,
            //                    GroupId = group.Id
            //                });

            //        });

            //        await Mediator.Send(new CreateGroupMenuRequest(request));

            //        NavigationManager.NavigateTo("groups", true);
            //        return;
            //    }

            //    foreach (var item in menus)
            //    {
            //        request.Add(new GroupMenuDto
            //        {
            //            GroupId = group.Id,
            //            MenuId = Convert.ToInt32(item.Value)
            //        });
            //    }

            //    for (long i = 0; i < request.Count; i++)
            //    {
            //        var check = Menus.FirstOrDefault(x => x.Id == request[i].MenuId);
            //        var cekP = Menus.FirstOrDefault(x => x.Name == check!.ParentMenu);
            //        if (cekP is not null)
            //        {
            //            var cekLagi = request.FirstOrDefault(x => x.MenuId == cekP.Id);
            //            if (cekLagi is null)
            //            {
            //                request.Add(new GroupMenuDto
            //                {
            //                    GroupId = group.Id,
            //                    MenuId = cekP.Id
            //                });
            //            }
            //        }
            //    }
            //    await Mediator.Send(new CreateGroupMenuRequest(request));

            //    NavigationManager.NavigateTo("groups", true);
            //}
            //else
            //{
            //    var result = await Mediator.Send(new UpdateGroupRequest(Group));

            //    var request = new List<GroupMenuDto>();

            //    var group = await Mediator.Send(new GetGroupByNameQuery(Group.Name));

            //    if (menus.Where(x => x.Value == "0").Any())
            //    {
            //        Menus.ForEach(z =>
            //        {
            //            if (z.Id != 0)
            //            {
            //                request.Add(new GroupMenuDto
            //                {
            //                    MenuId = z.Id,
            //                    GroupId = group.Id
            //                });
            //            }
            //        });

            //        await Mediator.Send(new UpdateGroupMenuRequest(request, SelectedIds));
            //        NavigationManager.NavigateTo("groups", true);
            //        return;
            //    }

            //    foreach (var item in menus)
            //    {
            //        request.Add(new GroupMenuDto
            //        {
            //            GroupId = group.Id,
            //            MenuId = Convert.ToInt32(item.Value)
            //        });
            //    }

            //    for (long i = 0; i < request.Count; i++)
            //    {
            //        var check = Menus.FirstOrDefault(x => x.Id == request[i].MenuId);
            //        var cekP = Menus.FirstOrDefault(x => x.Name == check!.ParentMenu);
            //        if (cekP is not null)
            //        {
            //            var cekLagi = request.FirstOrDefault(x => x.MenuId == cekP.Id);
            //            if (cekLagi is null)
            //            {
            //                request.Add(new GroupMenuDto
            //                {
            //                    GroupId = group.Id,
            //                    MenuId = cekP.Id
            //                });
            //            }
            //        }
            //    }

            //    await Mediator.Send(new UpdateGroupMenuRequest(request, SelectedIds));
            //    NavigationManager.NavigateTo("groups", true);
            //}

            //if (Id is null)
            //{
            //    var result = await Mediator.Send(new CreateMenuRequest(Menu));
            //    if (result is not null)
            //        NavigationManager.NavigateTo("menus");
            //}
            //else
            //{
            //    var result = await Mediator.Send(new UpdateMenuRequest(Menu));
            //    if (result)
            //        NavigationManager.NavigateTo("menus");
            //}
        }

        #region Ktp Address

        #region ComboboxVillage

        private DxComboBox<VillageDto, long?> refVillageComboBox { get; set; }
        private int VillageComboBoxIndex { get; set; } = 0;
        private int totalCountVillage = 0;

        private async Task OnSearchVillage()
        {
            await LoadDataVillage(0, 10);
        }

        private async Task OnSearchVillageIndexIncrement()
        {
            if (VillageComboBoxIndex < (totalCountVillage - 1))
            {
                VillageComboBoxIndex++;
                await LoadDataVillage(VillageComboBoxIndex, 10);
            }
        }

        private async Task OnSearchVillageIndexDecrement()
        {
            if (VillageComboBoxIndex > 0)
            {
                VillageComboBoxIndex--;
                await LoadDataVillage(VillageComboBoxIndex, 10);
            }
        }

        private async Task OnInputVillageChanged(string e)
        {
            VillageComboBoxIndex = 0;
            await LoadDataVillage(0, 10);
        }

        private async Task LoadDataVillage(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var districtId = refDistrictComboBox?.Value.GetValueOrDefault();
            var id = refVillageComboBox?.Value ?? null;
            var result = await Mediator.QueryGetHelper<Village, VillageDto>(pageIndex, pageSize, refVillageComboBox?.Text ?? "", x => x.DistrictId == districtId);
            Villages = result.Item1;
            totalCountVillage = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxVillage

        #region ComboboxCountry

        private DxComboBox<CountryDto, long?> refCountryComboBox { get; set; }
        private int CountryComboBoxIndex { get; set; } = 0;
        private int totalCountCountry = 0;

        private async Task OnSearchCountry()
        {
            await LoadDataCountry(0, 10);
        }

        private async Task OnSearchCountryIndexIncrement()
        {
            if (CountryComboBoxIndex < (totalCountCountry - 1))
            {
                CountryComboBoxIndex++;
                await LoadDataCountry(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCountryIndexDecrement()
        {
            if (CountryComboBoxIndex > 0)
            {
                CountryComboBoxIndex--;
                await LoadDataCountry(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnInputCountryChanged(string e)
        {
            CountryComboBoxIndex = 0;
            await LoadDataCountry(0, 10);
        }

        private async Task LoadDataCountry(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            Provinces.Clear();
            Cities.Clear();
            Districts.Clear();
            Villages.Clear();
            UserForm.IdCardProvinceId = null;
            UserForm.IdCardCityId = null;
            UserForm.IdCardDistrictId = null;
            UserForm.IdCardVillageId = null;
            var result = await Mediator.Send(new GetCountryQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCountryComboBox?.Text ?? ""
            });
            Countries = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCountry

        #region ComboboxCity

        private DxComboBox<CityDto, long?> refCityComboBox { get; set; }
        private int CityComboBoxIndex { get; set; } = 0;
        private int totalCountCity = 0;

        private async Task OnSearchCity()
        {
            await LoadDataCity(0, 10);
        }

        private async Task OnSearchCityIndexIncrement()
        {
            if (CityComboBoxIndex < (totalCountCity - 1))
            {
                CityComboBoxIndex++;
                await LoadDataCity(CityComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCityIndexDecrement()
        {
            if (CityComboBoxIndex > 0)
            {
                CityComboBoxIndex--;
                await LoadDataCity(CityComboBoxIndex, 10);
            }
        }

        private async Task OnInputCityChanged(string e)
        {
            CityComboBoxIndex = 0;
            await LoadDataCity(0, 10);
        }

        private async Task LoadDataCity(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var provinceId = refProvinceComboBox?.Value.GetValueOrDefault();
            UserForm.IdCardDistrictId = null;
            UserForm.IdCardVillageId = null;
            var id = refCityComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.ProvinceId == provinceId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCityComboBox?.Text ?? ""
            });
            Cities = result.Item1;
            totalCountCity = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCity

        #region ComboboxProvince

        private DxComboBox<ProvinceDto, long?> refProvinceComboBox { get; set; }
        private int ProvinceComboBoxIndex { get; set; } = 0;
        private int totalCountProvince = 0;

        private async Task OnSearchProvince()
        {
            await LoadDataProvince(0, 10);
        }

        private async Task OnSearchProvinceIndexIncrement()
        {
            if (ProvinceComboBoxIndex < (totalCountProvince - 1))
            {
                ProvinceComboBoxIndex++;
                await LoadDataProvince(ProvinceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProvinceIndexDecrement()
        {
            if (ProvinceComboBoxIndex > 0)
            {
                ProvinceComboBoxIndex--;
                await LoadDataProvince(ProvinceComboBoxIndex, 10);
            }
        }

        private async Task OnInputProvinceChanged(string e)
        {
            ProvinceComboBoxIndex = 0;
            await LoadDataProvince(0, 10);
        }

        private async Task LoadDataProvince(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var countryId = refCountryComboBox?.Value.GetValueOrDefault();
            UserForm.IdCardCityId = null;
            UserForm.IdCardDistrictId = null;
            UserForm.IdCardVillageId = null;

            var result = await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.CountryId == countryId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refProvinceComboBox?.Text ?? ""
            });
            Provinces = result.Item1;
            totalCountProvince = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxProvince

        #region ComboboxDistrict

        private DxComboBox<DistrictDto, long?> refDistrictComboBox { get; set; }
        private int DistrictComboBoxIndex { get; set; } = 0;
        private int totalCountDistrict = 0;

        private async Task OnSearchDistrict()
        {
            await LoadDataDistrict(0, 10);
        }

        private async Task OnSearchDistrictIndexIncrement()
        {
            if (DistrictComboBoxIndex < (totalCountDistrict - 1))
            {
                DistrictComboBoxIndex++;
                await LoadDataDistrict(DistrictComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDistrictndexDecrement()
        {
            if (DistrictComboBoxIndex > 0)
            {
                DistrictComboBoxIndex--;
                await LoadDataDistrict(DistrictComboBoxIndex, 10);
            }
        }

        private async Task OnInputDistrictChanged(string e)
        {
            DistrictComboBoxIndex = 0;
            await LoadDataDistrict(0, 10);
        }

        private async Task LoadDataDistrict(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var cityId = refCityComboBox?.Value.GetValueOrDefault();
            UserForm.IdCardVillageId = null;
            var id = refDistrictComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetDistrictQuery
            {
                Predicate = x => x.CityId == cityId,
                SearchTerm = refDistrictComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            Districts = result.Item1;
            totalCountDistrict = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDistrict

        #endregion Ktp Address

        #region Residence Address

        #region ComboboxVillageResidence

        private DxComboBox<VillageDto, long?> refVillageResidenceComboBox { get; set; }
        private int VillageResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountVillageResidence = 0;

        private async Task OnSearchVillageResidence()
        {
            await LoadDataVillageResidence(0, 10);
        }

        private async Task OnSearchVillageResidenceIndexIncrement()
        {
            if (VillageResidenceComboBoxIndex < (totalCountVillageResidence - 1))
            {
                VillageResidenceComboBoxIndex++;
                await LoadDataVillageResidence(VillageResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchVillageResidenceIndexDecrement()
        {
            if (VillageResidenceComboBoxIndex > 0)
            {
                VillageResidenceComboBoxIndex--;
                await LoadDataVillageResidence(VillageResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputVillageResidenceChanged(string e)
        {
            VillageResidenceComboBoxIndex = 0;
            await LoadDataVillageResidence(0, 10);
        }

        private async Task LoadDataVillageResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var DistrictResidenceId = refDistrictResidenceComboBox?.Value.GetValueOrDefault();
            var result = await Mediator.QueryGetHelper<Village, VillageDto>(pageIndex, pageSize, refVillageResidenceComboBox?.Text ?? "", x => x.DistrictId == DistrictResidenceId);
            VillagesResidence = result.Item1;
            totalCountVillageResidence = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxVillageResidence

        #region ComboboxCountryResidence

        private DxComboBox<CountryDto, long?> refCountryResidenceComboBox { get; set; }
        private int CountryResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountCountryResidence = 0;

        private async Task OnSearchCountryResidence()
        {
            await LoadDataCountryResidence(0, 10);
        }

        private async Task OnSearchCountryResidenceIndexIncrement()
        {
            if (CountryResidenceComboBoxIndex < (totalCountCountryResidence - 1))
            {
                CountryResidenceComboBoxIndex++;
                await LoadDataCountryResidence(CountryResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCountryResidenceIndexDecrement()
        {
            if (CountryResidenceComboBoxIndex > 0)
            {
                CountryResidenceComboBoxIndex--;
                await LoadDataCountryResidence(CountryResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputCountryResidenceChanged(string e)
        {
            CountryResidenceComboBoxIndex = 0;
            await LoadDataCountryResidence(0, 10);
        }

        private async Task LoadDataCountryResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetCountryQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCountryResidenceComboBox?.Text ?? ""
            });
            CountriesResidence = result.Item1;
            totalCountCountryResidence = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCountryResidence

        #region ComboboxCityResidence

        private DxComboBox<CityDto, long?> refCityResidenceComboBox { get; set; }
        private int CityResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountCityResidence = 0;

        private async Task OnSearchCityResidence()
        {
            await LoadDataCityResidence(0, 10);
        }

        private async Task OnSearchCityResidenceIndexIncrement()
        {
            if (CityResidenceComboBoxIndex < (totalCountCityResidence - 1))
            {
                CityResidenceComboBoxIndex++;
                await LoadDataCityResidence(CityResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCityResidenceIndexDecrement()
        {
            if (CityResidenceComboBoxIndex > 0)
            {
                CityResidenceComboBoxIndex--;
                await LoadDataCityResidence(CityResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputCityResidenceChanged(string e)
        {
            CityResidenceComboBoxIndex = 0;
            await LoadDataCityResidence(0, 10);
        }

        private async Task LoadDataCityResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var ProvinceResidenceId = refProvinceResidenceComboBox?.Value.GetValueOrDefault();
            var result = await Mediator.Send(new GetCityQuery
            {
                Predicate = x => x.ProvinceId == ProvinceResidenceId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCityResidenceComboBox?.Text ?? ""
            });
            CitiesResidence = result.Item1;
            totalCountCityResidence = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCityResidence

        #region ComboboxProvinceResidence

        private DxComboBox<ProvinceDto, long?> refProvinceResidenceComboBox { get; set; }
        private int ProvinceResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountProvinceResidence = 0;

        private async Task OnSearchProvinceResidence()
        {
            await LoadDataProvinceResidence(0, 10);
        }

        private async Task OnSearchProvinceResidenceIndexIncrement()
        {
            if (ProvinceResidenceComboBoxIndex < (totalCountProvinceResidence - 1))
            {
                ProvinceResidenceComboBoxIndex++;
                await LoadDataProvinceResidence(ProvinceResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProvinceResidenceIndexDecrement()
        {
            if (ProvinceResidenceComboBoxIndex > 0)
            {
                ProvinceResidenceComboBoxIndex--;
                await LoadDataProvinceResidence(ProvinceResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputProvinceResidenceChanged(string e)
        {
            ProvinceResidenceComboBoxIndex = 0;
            await LoadDataProvinceResidence(0, 10);
        }

        private async Task LoadDataProvinceResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var CountryResidenceId = refCountryResidenceComboBox?.Value.GetValueOrDefault();
            var id = refProvinceResidenceComboBox?.Value ?? null;
            var result = await Mediator.Send(new GetProvinceQuery
            {
                Predicate = x => x.CountryId == CountryResidenceId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refProvinceResidenceComboBox?.Text ?? ""
            });
            ProvincesResidence = result.Item1;
            totalCountProvinceResidence = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxProvinceResidence

        #region ComboboxDistrictResidence

        private DxComboBox<DistrictDto, long?> refDistrictResidenceComboBox { get; set; }
        private int DistrictResidenceComboBoxIndex { get; set; } = 0;
        private int totalCountDistrictResidence = 0;

        private async Task OnSearchDistrictResidence()
        {
            await LoadDataDistrictResidence(0, 10);
        }

        private async Task OnSearchDistrictResidenceIndexIncrement()
        {
            if (DistrictResidenceComboBoxIndex < (totalCountDistrictResidence - 1))
            {
                DistrictResidenceComboBoxIndex++;
                await LoadDataDistrictResidence(DistrictResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDistrictResidencendexDecrement()
        {
            if (DistrictResidenceComboBoxIndex > 0)
            {
                DistrictResidenceComboBoxIndex--;
                await LoadDataDistrictResidence(DistrictResidenceComboBoxIndex, 10);
            }
        }

        private async Task OnInputDistrictResidenceChanged(string e)
        {
            DistrictResidenceComboBoxIndex = 0;
            await LoadDataDistrictResidence(0, 10);
        }

        private async Task LoadDataDistrictResidence(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var CityResidenceId = refCityResidenceComboBox?.Value.GetValueOrDefault();
            var result = await Mediator.Send(new GetDistrictQuery
            {
                Predicate = x => x.CityId == CityResidenceId,
                SearchTerm = refDistrictResidenceComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            DistrictsResidence = result.Item1;
            totalCountDistrictResidence = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDistrictResidence

        #endregion Residence Address

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task LoadData_userClaim()
        {
            IsLoadingUserClaim = true;
            var result = await Mediator.Send(new GetClaimHistoryQuery
            {
                Predicate = x => x.PatientId == Id,
                SearchTerm = searchTerm,
                PageIndex = 0,
                PageSize = 10,
            });

            getClaimHistory = result.Item1;
            totalCount = result.PageCount;
            activePageIndex = 0;
            IsLoadingUserClaim = false;
        }

        private async Task Refresh_UserClaim_Click()
        {
            await LoadData_userClaim();
        }
    }
}
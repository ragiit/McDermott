using static McDermott.Application.Features.Commands.Config.OccupationalCommand;
using static McDermott.Application.Features.Commands.Pharmacy.PharmacyCommand;

namespace McDermott.Web.Components.Pages.Patient
{
    public partial class PatientDataPage
    {
        #region Relation Data

        private List<AllergyDto> Allergies = [];
        private List<UserDto> Users = [];
        private List<UserDto> Patiens = [];
        private List<UserDto> AllPatiens = [];
        private List<FamilyDto> Families = [];
        private List<CityDto> Cities = [];
        private List<CountryDto> Countries = [];
        private List<ProvinceDto> Provinces = [];
        private List<DistrictDto> Districts = [];
        private List<VillageDto> Villages = [];
        private List<DepartmentDto> Departments = [];
        private List<JobPositionDto> JobPositions = [];
        private List<ReligionDto> Religions = [];
        private List<GenderDto> Genders = [];
        private List<PatientFamilyRelationDto> AllPatientFamilyRelations = [];
        private List<PatientFamilyRelationDto> PatientFamilyRelations = [];
        private List<FamilyDto> Familys = [];
        private List<InsurancePolicyDto> InsurancePolicies = [];

        private UserDto UserForm = new();

        #endregion Relation Data

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        private List<string> YesNoOptions =
       [
           "Yes",
            "No"

       ]; private List<string> RiwayatPenyakitKeluarga =

        [
            "DM",
            "Hipertensi",
            "Cancer",
            "Jantung",
            "TBC",
            "Anemia",
            "Other",
        ];

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    await GetUserInfo();
                    StateHasChanged();

                    await LoadData();
                    StateHasChanged();

                    Allergies = await Mediator.Send(new GetAllergyQuery());

                    Allergies.ForEach(x =>
                    {
                        var a = Helper._allergyTypes.FirstOrDefault(z => x.Type is not null && z.Code == x.Type);
                        if (a is not null)
                            x.TypeString = a.Name;
                    });

                    Countries = await Mediator.Send(new GetCountryQuery());
                    Provinces = await Mediator.Send(new GetProvinceQuery());
                    Cities = await Mediator.Send(new GetCityQuery());
                    Districts = await Mediator.Send(new GetDistrictQuery());
                    Villages = await Mediator.Send(new GetVillageQuery());
                    Religions = await Mediator.Send(new GetReligionQuery());
                    Genders = await Mediator.Send(new GetGenderQuery());
                    Departments = await Mediator.Send(new GetDepartmentQuery());
                    JobPositions = await Mediator.Send(new GetJobPositionQuery());
                    Families = await Mediator.Send(new GetFamilyQuery());
                    Occupationals = await Mediator.Send(new GetOccupationalQuery());
                    StateHasChanged();
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

        private bool PanelVisible = false;
        private bool FormValidationState = true;
        private bool ShowForm { get; set; } = false;
        private bool IsDeleted { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowFamilyMemberVisibleIndex { get; set; }
        private char Placeholder { get; set; } = '_';
        private bool SaveLiterals { get; set; } = true;

        [Parameter]
        public long InsurancePoliciesCount { get; set; } = 0;

        [Parameter]
        public long PrescriptionCount { get; set; } = 0;

        public int VaccinationCount { get; set; } = 0;

        public IGrid Grid { get; set; }
        public IGrid GridFamilyRelation { get; set; }
        private IEnumerable<AllergyDto> SelectedAllergies { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataItemVaccinations { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataFamilyRelationItems { get; set; } = new ObservableRangeCollection<object>();

        private enum Opinion
        { Yes, No, Abstain }

        private Opinion Value = Opinion.Abstain;

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

        private List<string> IdentityTypes = new()
        {
            "KTP",
            "Paspor",
            "SIM",
            "VISA",
        };

        private List<string> MartialStatuss = new()
        {
            "Single",
            "Married",
            "Divorced",
            "Widowed",
            "Separated",
            "Unmarried"
        };

        private List<OccupationalDto> Occupationals = [];

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            return;
            // Create an instance of Form B
            var formB = new InsurancePolicyPageListForm();

            // Create an EventCallback object for the method
            var eventCallback = EventCallback.Factory.Create<List<InsurancePolicyDto>>(this, UpdateInsurancePoliciesCount);

            // Subscribe to the event using the EventCallback object
            formB.InsurancePoliciesUpdated = eventCallback;
        }

        private void UpdateInsurancePoliciesCount(List<InsurancePolicyDto> updatedInsurancePolicies)
        {
            InsurancePoliciesCount = updatedInsurancePolicies.Count;
            StateHasChanged(); // Perbarui tampilan
        }

        private void UpdateIntA(long newValue)
        {
            StateHasChanged(); // Perbarui tampilan
        }

        #region MaskedInput

        private string EmailMask { get; set; } = @"(\w|[.-])+@(\w|-)+\.(\w|-){2,4}";

        private void OnEmailChanged(string email)
        {
            UserForm.Email = email;
        }

        #endregion MaskedInput

        private void CheckedChanged(bool value)
        {
            //UserForm.Name
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedAllergies = [];
            ShowForm = false;
            AllPatientFamilyRelations.Clear();
            PatientFamilyRelations.Clear();
            TabIndex = -1;
            SelectedDataItems = [];
            SelectedDataFamilyRelationItems = [];
            UserForm = new();

            try
            {
                UserForm.PatientAllergy.UserId = UserLogin.Id;
            }
            catch { }

            Users = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true));
            Patiens = Users.Where(x => x.IsPatient == true).ToList();
            AllPatiens = Users.Where(x => x.IsPatient == true).ToList();

            PanelVisible = false;
        }

        public string Alert { get; set; } = "";
        public int TabIndex { get; set; } = -1;
        private bool IsClicked { get; set; } = false;

        private async Task OnTabClick(TabClickEventArgs e)
        {
            IsClicked = true;
            TabIndex = e.TabIndex;
            Alert = $"{e.TabIndex} tab has been clicked";

            switch (TabIndex)
            {
                case 0:
                    await LoadData2();
                    break;

                case 1:
                    break;

                default:
                    break;
            }
        }

        private List<UserDto> Users2 = [];

        private async Task LoadData2()
        {
            PanelVisible = true;

            Users2 = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true));

            PanelVisible = false;
        }

        #region Grid

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            await OnSave();
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        private void OnCancel()
        {
            UserForm = new();
            ShowForm = false;
        }

        private void OnClose()
        {
            TabIndex = -1;
        }

        private bool PopUpVisible = false;
        private bool PrescriptionPopUp = false;
        private bool DiseasePopUp = false;
        private bool IsVaccinations = false;
        private bool IsLoadingGeneralConsultantServiceVaccinations = false;
        private IGrid GridVaccinations { get; set; }
        private List<GeneralConsultanServiceDto> GeneralConsultanServiceVaccinations { get; set; } = [];

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

        private GeneralConsultanServiceDto SelectedGeneralConsultanService { get; set; } = new();
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs { get; set; } = [];
        private bool IsDetailVaccinations { get; set; } = false;

        private async Task OnClickDetailHistoricalRecordPatientVaccinations(GeneralConsultanServiceDto generalConsultanService)
        {
            IsDetailVaccinations = true;
            SelectedGeneralConsultanService = generalConsultanService;
            GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == generalConsultanService.Id));
        }

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
                GeneralConsultanServiceVaccinations = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.PatientId == UserForm.Id && x.Service != null && x.Service.Name == "Vaccination"));
                IsLoadingGeneralConsultantServiceVaccinations = false;
                return;
            }
            TabIndex = text.ToInt32();
        }

        private async Task OnClickCloseInsurancePolicyPopUp()
        {
            if (UserForm.Id != 0)
            {
                var count = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == UserForm.Id));
                InsurancePoliciesCount = count.Count;
            }
        }

        private async Task OnClickClosePrescriptionPopUp()
        {
            if (UserForm.Id != 0)
            {
                var count = await Mediator.Send(new GetPharmacyQuery(x => x.PatientId == UserForm.Id));
                PrescriptionCount = count.Count;
            }
        }

        private void OnSaveFamilyMember(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (PatientFamilyRelationDto)e.EditModel;

                if (editModel.FamilyMemberId == 0)
                    return;

                if (UserForm.Id != 0)
                    editModel.PatientId = UserForm.Id;

                if (PatientFamilyRelations.Any(x => x.FamilyMemberId == editModel.FamilyMemberId && x.FamilyId == editModel.FamilyId))
                    return;

                editModel.FamilyMember = Users.FirstOrDefault(x => x.Id == editModel.FamilyMemberId);
                editModel.Family = Families.FirstOrDefault(x => x.Id == editModel.FamilyId);

                if (editModel.Id == 0)
                {
                    long newId;
                    do
                    {
                        newId = new Random().Next();
                    } while (PatientFamilyRelations.Any(pfr => pfr.Id == newId));

                    editModel.Id = newId;
                    PatientFamilyRelations.Add(editModel);
                }
                else
                    PatientFamilyRelations[FocusedRowFamilyMemberVisibleIndex] = editModel;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSave()
        {
            try
            {
                UserDto result = new();
                if (!FormValidationState)
                    return;

                if (!string.IsNullOrWhiteSpace(UserForm.NoId))
                {
                    var a = Users.FirstOrDefault(x => x.NoId == UserForm.NoId && x.Id != UserForm.Id);
                    if (a != null)
                    {
                        ToastService.ShowInfo("The Identity Number already exist");
                        return;
                    }
                }

                UserForm.IsPatient = true;
                UserForm.PatientAllergyIds = [];

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

                if (SelectedAllergies is not null && SelectedAllergies.Count() > 0)
                    UserForm.PatientAllergyIds.AddRange(SelectedAllergies.Select(x => x.Id).ToList());

                if (UserForm.Id == 0)
                {
                    var date = DateTime.Now;
                    var lastId = Users.ToList().LastOrDefault();

                    UserForm.NoRm = lastId is null
                             ? $"{date:dd-MM-yyyy}-0001"
                             : $"{date:dd-MM-yyyy}-{(long.Parse(lastId!.NoRm!.Substring(lastId.NoRm.Length - 4)) + 1):0000}";

                    if (UserForm.IsEmployeeRelation == true)
                    {
                        var checkName = Users.Where(x => x.Name.Contains("%" + UserForm.Name + "%")).FirstOrDefault();
                        if (checkName != null)
                        {
                            UserForm.IsEmployee = true;
                            UserForm = await Mediator.Send(new UpdateUserRequest(UserForm));
                        }
                        else
                        {
                            result = await Mediator.Send(new CreateUserRequest(UserForm));
                        }
                    }
                    else
                    {
                        result = await Mediator.Send(new CreateUserRequest(UserForm));
                    }

                    UserForm.PatientAllergy.UserId = UserForm.Id;

                    if (!string.IsNullOrWhiteSpace(UserForm.PatientAllergy.Farmacology) || !string.IsNullOrWhiteSpace(UserForm.PatientAllergy.Food) || !string.IsNullOrWhiteSpace(UserForm.PatientAllergy.Weather))
                    {
                        if (UserForm.PatientAllergy.Id == 0)
                            await Mediator.Send(new CreatePatientAllergyRequest(UserForm.PatientAllergy));
                        else
                            await Mediator.Send(new UpdatePatientAllergyRequest(UserForm.PatientAllergy));
                    }

                    await Mediator.Send(new DeletePatientFamilyRelationRequest(ids: AllPatientFamilyRelations.Select(x => x.Id).ToList()));

                    PatientFamilyRelations.ForEach(x => { x.PatientId = UserForm.Id; x.Id = 0; });
                    var temp = new List<PatientFamilyRelationDto>();
                    PatientFamilyRelations.ForEach(x =>
                    {
                        temp.Add(new PatientFamilyRelationDto
                        {
                            PatientId = x.FamilyMemberId.GetValueOrDefault(),
                            FamilyMemberId = x.PatientId,
                            FamilyId = Families.FirstOrDefault(y => y.Name == Families.FirstOrDefault(z => z.Id == x.FamilyId)!.ChildRelation)!.Id
                        });
                    });

                    PatientFamilyRelations.AddRange(temp);
                    await Mediator.Send(new CreateListPatientFamilyRelationRequest(PatientFamilyRelations));

                    UserForm.PatientAllergy.UserId = result.Id;

                    await Mediator.Send(new CreatePatientAllergyRequest(UserForm.PatientAllergy));
                }
                else
                {
                    if (UserForm.IsEmployeeRelation == true)
                    {
                        var checkName = Users.Where(x => x.Name.Contains("%" + UserForm.Name + "%")).FirstOrDefault();
                        if (checkName != null)
                        {
                            UserForm.IsEmployee = true;
                        }
                    }
                    await Mediator.Send(new UpdateUserRequest(UserForm));

                    UserForm.PatientAllergy.UserId = UserForm.Id;

                    if (!string.IsNullOrWhiteSpace(UserForm.PatientAllergy.Farmacology) || !string.IsNullOrWhiteSpace(UserForm.PatientAllergy.Food) || !string.IsNullOrWhiteSpace(UserForm.PatientAllergy.Weather))
                    {
                        if (UserForm.PatientAllergy.Id == 0)
                            await Mediator.Send(new CreatePatientAllergyRequest(UserForm.PatientAllergy));
                        else
                            await Mediator.Send(new UpdatePatientAllergyRequest(UserForm.PatientAllergy));
                    }

                    await Mediator.Send(new DeletePatientFamilyRelationRequest(ids: AllPatientFamilyRelations.Select(x => x.Id).ToList()));

                    PatientFamilyRelations.ForEach(x => { x.PatientId = UserForm.Id; x.Id = 0; });
                    var temps = new List<PatientFamilyRelationDto>();
                    PatientFamilyRelations.ForEach(x =>
                    {
                        temps.Add(new PatientFamilyRelationDto
                        {
                            PatientId = x.FamilyMemberId.GetValueOrDefault(),
                            FamilyMemberId = x.PatientId,
                            FamilyId = Families.FirstOrDefault(y => y.Name == Families.FirstOrDefault(z => z.Id == x.FamilyId)!.ChildRelation).Id
                        });
                    });

                    PatientFamilyRelations.AddRange(temps);
                    await Mediator.Send(new CreateListPatientFamilyRelationRequest(PatientFamilyRelations));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            if (args.DataItem is not null)
            {
                IsDeleted = ((UserDto)args.DataItem).Id == UserLogin.Id;
            }

            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void GridFamilyMember_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowFamilyMemberVisibleIndex = args.VisibleIndex;

            try
            {
                if (args.DataItem is not null)
                {
                    IsDeleted = ((UserDto)args.DataItem).Id == UserLogin.Id;
                }
            }
            catch (Exception)
            {
            }
        }

        private void OnDeleteFamilyRelation(GridDataItemDeletingEventArgs e)
        {
            var aaa = SelectedDataFamilyRelationItems.Adapt<List<PatientFamilyRelationDto>>();

            PatientFamilyRelations.RemoveAll(x => aaa.Select(z => z.FamilyId).Contains(x.FamilyId) && aaa.Select(z => z.FamilyMemberId).Contains(x.FamilyMemberId));
            SelectedDataFamilyRelationItems = new ObservableRangeCollection<object>();
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteUserRequest(((UserDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<UserDto>>();

                    await Mediator.Send(new DeleteUserRequest(ids: a.Where(x => x.Id != UserLogin.Id).Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void NewItem_Click()
        {
            try
            {
                UserForm = new();
                PatientFamilyRelations.Clear();
                ShowForm = true;
            }
            catch { }
        }

        private List<UserDto> originalPatiens = [];

        private async Task NewFamilyRelationItem_Click()
        {
            if (UserForm.Id != 0)
                Patiens = AllPatiens.Where(x => x.Id != UserForm.Id).ToList();

            await GridFamilyRelation.StartEditNewRowAsync();
        }

        private async Task EditFamilyRelationItem_Click()
        {
            await GridFamilyRelation.StartEditRowAsync(FocusedRowFamilyMemberVisibleIndex);
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            await EditItem();
        }

        private async Task EditItem()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                ShowForm = true;

                VaccinationCount = await Mediator.Send(new GetGeneralConsultanServiceCountQuery(x => x.PatientId == UserForm.Id && x.Service != null && x.Service.Name == "Vaccination"));
                PatientFamilyRelations = await Mediator.Send(new GetPatientFamilyByPatientQuery(x => x.PatientId == UserForm.Id));
                AllPatientFamilyRelations = [.. PatientFamilyRelations];
                var count = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == UserForm.Id));
                SelectedAllergies = Allergies.Where(x => UserForm.PatientAllergyIds is not null && UserForm.PatientAllergyIds.Contains(x.Id)).ToList();
                InsurancePoliciesCount = count.Count;
                var counts = await Mediator.Send(new GetPharmacyQuery(x => x.PatientId == UserForm.Id));
                PrescriptionCount = counts.Count;
                var alergy = await Mediator.Send(new GetPatientAllergyQuery(x => x.UserId == UserForm.Id));
                if (alergy.Count == 0)
                {
                    UserForm.PatientAllergy = new PatientAllergyDto();
                }
                else
                {
                    UserForm.PatientAllergy = alergy[0];
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void DeleteFamilyRelationItem_Click()
        {
            GridFamilyRelation.ShowRowDeleteConfirmation(FocusedRowFamilyMemberVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
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

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #endregion Grid
    }
}
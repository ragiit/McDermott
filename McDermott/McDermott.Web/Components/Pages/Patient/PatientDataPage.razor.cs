namespace McDermott.Web.Components.Pages.Patient
{
    public partial class PatientDataPage
    {
        #region Relation Data
        private List<UserDto> Users = [];
        private List<UserDto> Patiens = [];
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
        #endregion
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

        public IGrid Grid { get; set; }
        public IGrid GridFamilyRelation { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataFamilyRelationItems { get; set; } = new ObservableRangeCollection<object>();
        enum Opinion { Yes, No, Abstain }
        Opinion Value = Opinion.Abstain;

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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

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

            await GetUserInfo();
            await LoadData();

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


        void CheckedChanged(bool value)
        {
            //UserForm.Name
        }
        private async Task LoadData()
        {
            PanelVisible = true;

            ShowForm = false;
            AllPatientFamilyRelations.Clear();
            PatientFamilyRelations.Clear();
            TabIndex = -1;
            SelectedDataItems = new ObservableRangeCollection<object>();
            SelectedDataFamilyRelationItems = new ObservableRangeCollection<object>();
            UserForm = new();

            try
            {
                UserForm.PatientAllergy.UserId = (long)_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!.ToInt32()!;
            }
            catch { }

            Users = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true));
            Patiens = Users.Where(x => x.IsPatient == true).ToList();

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

            Users2 = await Mediator.Send(new GetUserPatientQuery());

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

        private void OnClickSmartButton(string text)
        {
            //NavigationManager.NavigateTo("patient/insurance-policy");
            //var a = new InsurancePolicyPage();
            //a.User = UserForm;

            if (text.Equals("Insurance Policy"))
            {
                PopUpVisible = true;
                return;
            }
            TabIndex = text.ToInt32();
        }

        private async Task OnClickCloseInsurancePolicyPopUp()
        {
            if (UserForm.Id != 0)
            {
                InsurancePoliciesCount = await Mediator.Send(new GetInsurancePolicyCountQuery(x => x.UserId == UserForm.Id));
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

                UserForm.IsPatient = true;

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
                            await Mediator.Send(new UpdateUserRequest(UserForm));

                            UserForm.PatientAllergy.UserId = checkName.Id;
                            if (UserForm.PatientAllergy.Id == 0)
                                await Mediator.Send(new CreatePatientAllergyRequest(UserForm.PatientAllergy));
                            else
                                await Mediator.Send(new UpdatePatientAllergyRequest(UserForm.PatientAllergy));

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

                    UserForm.PatientAllergy.UserId = result.Id;

                    await Mediator.Send(new CreatePatientAllergyRequest(UserForm.PatientAllergy));

                    PatientFamilyRelations.ForEach(x => { x.PatientId = result.Id; x.Id = 0; });

                    var temps = new List<PatientFamilyRelationDto>();
                    PatientFamilyRelations.ForEach(x =>
                    {
                        temps.Add(new PatientFamilyRelationDto
                        {
                            PatientId = x.FamilyMemberId.GetValueOrDefault(),
                            FamilyMemberId = x.PatientId,
                            FamilyId = Families.FirstOrDefault(y => y.Name == Families.FirstOrDefault(z => z.Id == x.FamilyId)!.ChildRelation)!.Id
                        });
                    });

                    PatientFamilyRelations.AddRange(temps);
                    await Mediator.Send(new CreateListPatientFamilyRelationRequest(PatientFamilyRelations));
                }
                else
                {
                    if(UserForm.IsEmployeeRelation == true)
                    {
                        UserForm.IsEmployee = true;
                    }
                    await Mediator.Send(new UpdateUserRequest(UserForm));
                    
                    UserForm.PatientAllergy.UserId = UserForm.Id;
                    if (UserForm.PatientAllergy.Id == 0)
                        await Mediator.Send(new CreatePatientAllergyRequest(UserForm.PatientAllergy));
                    else
                        await Mediator.Send(new UpdatePatientAllergyRequest(UserForm.PatientAllergy));

                    await Mediator.Send(new DeletePatientFamilyRelationRequest(ids: AllPatientFamilyRelations.Select(x => x.Id).ToList()));

                    PatientFamilyRelations.ForEach(x => { x.PatientId = UserForm.Id; x.Id = 0; });
                    var temps = new List<PatientFamilyRelationDto>();
                    PatientFamilyRelations.ForEach(x =>
                    {
                        temps.Add(new PatientFamilyRelationDto
                        {
                            PatientId = x.FamilyMemberId.GetValueOrDefault(),
                            FamilyMemberId = x.PatientId,
                            FamilyId = Families.FirstOrDefault(y => y.Name == Families.FirstOrDefault(z => z.Id == x.FamilyId)!.ChildRelation)!.Id
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
                IsDeleted = (bool)_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(((UserDto)args.DataItem).Id.ToString())!;
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
                    IsDeleted = (bool)_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(((UserDto)args.DataItem).Id.ToString())!;
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

                    long userActive = (long)_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!.ToInt32()!;

                    await Mediator.Send(new DeleteUserRequest(ids: a.Where(x => x.Id != userActive).Select(x => x.Id).ToList()));
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
            UserForm = new();
            PatientFamilyRelations.Clear();
            ShowForm = true;
        }

        private async Task NewFamilyRelationItem_Click()
        {
            await GridFamilyRelation.StartEditNewRowAsync();
        }

        private async Task EditFamilyRelationItem_Click()
        {
            await GridFamilyRelation.StartEditRowAsync(FocusedRowFamilyMemberVisibleIndex);
        }

        private void OnRowDoubleClick(GridRowClickEventArgs e)
        {
            EditItem();
        }

        private async void EditItem()
        {
            try
            {
                UserForm = SelectedDataItems[0].Adapt<UserDto>();
                ShowForm = true;

                PatientFamilyRelations = await Mediator.Send(new GetPatientFamilyByPatientQuery(x => x.PatientId == UserForm.Id));
                AllPatientFamilyRelations = [.. PatientFamilyRelations];
                //InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == UserForm.Id));
                InsurancePoliciesCount = await Mediator.Send(new GetInsurancePolicyCountQuery(x => x.UserId == UserForm.Id));
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
using static McDermott.Application.Features.Commands.Transaction.AccidentCommand;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class AccidentPage
    {
        #region UserLoginAndAccessRole

        private bool IsStatus(EnumStatusAccident status) => Accident.SentStatus == status;

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private (bool, GroupMenuDto, User) Test = new();
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
                    StateHasChanged();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                await LoadComboBox();
                StateHasChanged();
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

        private async Task OnClickCancel()
        {
            ShowForm = false;
            Accident = new();
            await LoadData();
        }

        #region Nature of Injury

        private IEnumerable<string> NatureOfInjury1 { get; set; } = new List<string>
        {
            "Foreign body",
            "Eye irritation"
        };

        private IEnumerable<string> NatureOfInjury2 { get; set; } = new List<string>
        {
            "Abrasion",
            "Laceration",
            "Puncture",
            "Scratch"
        };

        private IEnumerable<string> NatureOfInjury3 { get; set; } = new List<string>
        {
            "Bruise",
            "Contussion",
            "Crushing"
        };

        private IEnumerable<string> NatureOfInjury4 { get; set; } = new List<string>
        {
            "Sprain"
        };

        private IEnumerable<string> NatureOfInjury5 { get; set; } = new List<string>
        {
            "Fracture",
            "Dislocation"
        };

        private IEnumerable<string> NatureOfInjury6 { get; set; } = new List<string>
        {
            "Burn",
            "Chemical burn",
            "Electric burn"
        };

        private IEnumerable<string> NatureOfInjury7 { get; set; } = new List<string>
        {
            "Occupational illness",
            "LBP",
            "Dermatitis"
        };

        private IEnumerable<string> NatureOfInjury8 { get; set; } = new List<string>
        {
            "Asphyxia",
            "Intoxication",
            "Amputation",
            "Concussion"
        };

        #endregion Nature of Injury

        #region Part of Body

        private IEnumerable<string> PartOfBody1 { get; set; } = [
            "Head",
            "face",
            "neck"
        ];

        private IEnumerable<string> PartOfBody2 { get; set; } = [
            "Eye"
        ];

        private IEnumerable<string> PartOfBody3 { get; set; } = [
            "Ear"
        ];

        private IEnumerable<string> PartOfBody4 { get; set; } = [
            "Back"
        ];

        private IEnumerable<string> PartOfBody5 { get; set; } = [
            "Trunk (except back and internal)"
        ];

        private IEnumerable<string> PartOfBody6 { get; set; } = [
            "Arm"
        ];

        private IEnumerable<string> PartOfBody7 { get; set; } = [
            "Hand and wrist"
        ];

        private IEnumerable<string> PartOfBody8 { get; set; } = [
            "Fingers"
        ];

        private IEnumerable<string> PartOfBody9 { get; set; } = [
            "Leg"
        ];

        private IEnumerable<string> PartOfBody10 { get; set; } = [
            "Feet and ankles"
        ];

        private IEnumerable<string> PartOfBody11 { get; set; } = [
            "Toes"
        ];

        private IEnumerable<string> PartOfBody12 { get; set; } = [
            "Internal and Others"
        ];

        private IEnumerable<string> EmployeeCauseOfInjury1 { get; set; } = new[]
        {
            "Falls",
            "Slips",
            "Trips"
        };

        private IEnumerable<string> EmployeeCauseOfInjury2 { get; set; } = new[]
        {
            "Fire",
            "hot materials"
        };

        private IEnumerable<string> EmployeeCauseOfInjury3 { get; set; } = new[]
        {
            "Pressurized gas"
        };

        private IEnumerable<string> EmployeeCauseOfInjury4 { get; set; } = new[]
        {
    "Foreign body"
};

        private IEnumerable<string> EmployeeCauseOfInjury5 { get; set; } = new[]
        {
            "Electricity"
        };

        private IEnumerable<string> EmployeeCauseOfInjury6 { get; set; } = new[]
        {
            "Sandblast"
        };

        private IEnumerable<string> EmployeeCauseOfInjury7 { get; set; } = new[]
        {
            "Animal",
            "plant"
        };

        private IEnumerable<string> EmployeeCauseOfInjury8 { get; set; } = new[]
        {
            "Struck",
            "caught (by, against, between) objects"
        };

        private IEnumerable<string> EmployeeCauseOfInjury9 { get; set; } = new[]
        {
            "Chemicals"
        };

        private IEnumerable<string> EmployeeCauseOfInjury10 { get; set; } = new[]
        {
            "Welding flash"
        };

        private IEnumerable<string> EmployeeCauseOfInjury11 { get; set; } = new[]
        {
            "Vehicle accident"
        };

        private IEnumerable<string> EmployeeCauseOfInjury12 { get; set; } = new[]
        {
            "Overexertion"
        };

        private IEnumerable<string> EmployeeCauseOfInjury13 { get; set; } = new[]
        {
            "Smoke",
            "gas"
        };

        private IEnumerable<string> EmployeeCauseOfInjury14 { get; set; } = new[]
        {
            "Others"
        };

        #endregion Part of Body

        #region Treatment

        private IEnumerable<string> Treatment1 { get; set; } = [
            "Cleaning and dressing",
            "Removal of foreign body with cotton wool",
            "Removal of foreign body with needle and magnet"
        ];

        private IEnumerable<string> Treatment2 { get; set; } = [
            "Stitching"
        ];

        private IEnumerable<string> Treatment3 { get; set; } = [
            "Splinting"
        ];

        private IEnumerable<string> Treatment4 { get; set; } = [
            "Antibiotics"
        ];

        private IEnumerable<string> Treatment5 { get; set; } = [
            "Painkillers"
        ];

        private IEnumerable<string> Treatment6 { get; set; } = [
            "Tetanus toxoid injection, 0.5 cc"
        ];

        private IEnumerable<string> Treatment7 { get; set; } = [
            "Others"
        ];

        #endregion Treatment

        private IEnumerable<string> EmployeeClass { get; set; } = [
           "FA",
           "MTC",
           "RWC",
           "LTA",
           "FATALITY",
           "OCCUPATIONAL ILLNESS",
       ];

        #endregion UserLoginAndAccessRole

        private string StagingText = EnumStatusAccident.RestrictedWorkCase.GetDisplayName();

        private bool PanelVisible { get; set; } = true;
        private bool ShowForm { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private AccidentDto Accident { get; set; } = new();
        private List<AccidentDto> Data = [];
        private List<DepartmentDto> Departments = [];
        private List<UserDto> Employees = [];

        private string SelectedNIP { get; set; } = "-";

        private async Task OnClickConfirm()
        {
            try
            {
                IsLoading = true;
                if (Accident.Id != 0)
                {
                    switch (StagingText)
                    {
                        case "Restricted work case":
                            Accident.SentStatus = EnumStatusAccident.RestrictedWorkCase;
                            StagingText = EnumStatusAccident.LostWorkDaysCase.GetDisplayName();
                            break;

                        case "Lost Work days case":
                            Accident.SentStatus = EnumStatusAccident.LostWorkDaysCase;
                            StagingText = EnumStatusAccident.FatalityCase.GetDisplayName();
                            break;

                        case "Fatality case":
                            Accident.SentStatus = EnumStatusAccident.FatalityCase;
                            StagingText = EnumStatusAccident.FatalityCase.GetDisplayName();
                            break;

                        default:
                            break;
                    }

                    await Mediator.Send(new UpdateAccidentRequest(Accident));

                    Accident.Employee = Employees.FirstOrDefault(x => x.Id == Accident.EmployeeId);
                    Accident.Department = Departments.FirstOrDefault(x => x.Id == Accident.DepartmentId);
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            IsLoading = false;
        }

        private void OnSelectEmployee(UserDto e)
        {
            SelectedNIP = "-";
            if (e is null)
                return;

            SelectedNIP = e.NIP ?? "-";
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }

        private async Task LoadComboBox()
        {
            Employees = await Mediator.Send(new GetUserQuery(x => x.IsEmployee == true));
            Departments = await Mediator.Send(new GetDepartmentQuery());
        }

        private async Task OnValidSubmitSave()
        {
            try
            {
                if (Accident.Id == 0)
                    Accident = await Mediator.Send(new CreateAccidentRequest(Accident));
                else
                    await Mediator.Send(new UpdateAccidentRequest(Accident));

                Accident.Employee = Employees.FirstOrDefault(x => x.Id == Accident.EmployeeId);
                Accident.Department = Departments.FirstOrDefault(x => x.Id == Accident.DepartmentId);

                ToastService.ClearInfoToasts();
                ToastService.ShowInfo("Saved Successfully");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void OnInvalidSubmitSave()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            ShowForm = false;
            Data = await Mediator.Send(new GetAccidentQuery());
            Accident = new();
            PanelVisible = false;
        }

        private async Task OnClickBack()
        {
            await LoadData();
        }

        public async Task OnDeleting(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteAccidentRequest(((AccidentDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteAccidentRequest(ids: SelectedDataItems.Adapt<List<AccidentDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task ExportToExcel()
        {
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void NewItem_Click()
        {
            ShowForm = true;
            Accident = new()
            {
                Id = 0,

                SelectedEmployeeCauseOfInjury1 = EmployeeCauseOfInjury1.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury2 = EmployeeCauseOfInjury2.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury3 = EmployeeCauseOfInjury3.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury4 = EmployeeCauseOfInjury4.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury5 = EmployeeCauseOfInjury5.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury6 = EmployeeCauseOfInjury6.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury7 = EmployeeCauseOfInjury7.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury8 = EmployeeCauseOfInjury8.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury9 = EmployeeCauseOfInjury9.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury10 = EmployeeCauseOfInjury10.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury11 = EmployeeCauseOfInjury11.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury12 = EmployeeCauseOfInjury12.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury13 = EmployeeCauseOfInjury13.Select(x => x).AsEnumerable(),
                SelectedEmployeeCauseOfInjury14 = EmployeeCauseOfInjury14.Select(x => x).AsEnumerable(),

                SelectedNatureOfInjury1 = NatureOfInjury1.Select(x => x).AsEnumerable(),
                SelectedNatureOfInjury2 = NatureOfInjury2.Select(x => x).AsEnumerable(),
                SelectedNatureOfInjury3 = NatureOfInjury3.Select(x => x).AsEnumerable(),
                SelectedNatureOfInjury4 = NatureOfInjury4.Select(x => x).AsEnumerable(),
                SelectedNatureOfInjury5 = NatureOfInjury5.Select(x => x).AsEnumerable(),
                SelectedNatureOfInjury6 = NatureOfInjury6.Select(x => x).AsEnumerable(),
                SelectedNatureOfInjury7 = NatureOfInjury7.Select(x => x).AsEnumerable(),
                SelectedNatureOfInjury8 = NatureOfInjury8.Select(x => x).AsEnumerable(),

                SelectedPartOfBody1 = PartOfBody1.Select(x => x).AsEnumerable(),
                SelectedPartOfBody2 = PartOfBody2.Select(x => x).AsEnumerable(),
                SelectedPartOfBody3 = PartOfBody3.Select(x => x).AsEnumerable(),
                SelectedPartOfBody4 = PartOfBody4.Select(x => x).AsEnumerable(),
                SelectedPartOfBody5 = PartOfBody5.Select(x => x).AsEnumerable(),
                SelectedPartOfBody6 = PartOfBody6.Select(x => x).AsEnumerable(),
                SelectedPartOfBody7 = PartOfBody7.Select(x => x).AsEnumerable(),
                SelectedPartOfBody8 = PartOfBody8.Select(x => x).AsEnumerable(),
                SelectedPartOfBody9 = PartOfBody9.Select(x => x).AsEnumerable(),
                SelectedPartOfBody10 = PartOfBody10.Select(x => x).AsEnumerable(),
                SelectedPartOfBody11 = PartOfBody11.Select(x => x).AsEnumerable(),
                SelectedPartOfBody12 = PartOfBody12.Select(x => x).AsEnumerable(),

                SelectedTreatment1 = Treatment1.Select(x => x).AsEnumerable(),
                SelectedTreatment2 = Treatment2.Select(x => x).AsEnumerable(),
                SelectedTreatment3 = Treatment3.Select(x => x).AsEnumerable(),
                SelectedTreatment4 = Treatment4.Select(x => x).AsEnumerable(),
                SelectedTreatment5 = Treatment5.Select(x => x).AsEnumerable(),
                SelectedTreatment6 = Treatment6.Select(x => x).AsEnumerable(),
                SelectedTreatment7 = Treatment7.Select(x => x).AsEnumerable()
            };

            StagingText = EnumStatusAccident.RestrictedWorkCase.GetDisplayName();
        }

        private bool IsLoading { get; set; } = false;

        private async Task EditItem_Click()
        {
            ShowForm = true;
            IsLoading = true;

            try
            {
                Accident = (await Mediator.Send(new GetAccidentQuery(x => x.Id == SelectedDataItems[0].Adapt<AccidentDto>().Id))).FirstOrDefault() ?? new();
                if (Accident is not null)
                {
                    switch (Accident.SentStatus)
                    {
                        case EnumStatusAccident.ReturnToWork:
                            StagingText = EnumStatusAccident.RestrictedWorkCase.GetDisplayName();
                            break;

                        case EnumStatusAccident.RestrictedWorkCase:
                            StagingText = EnumStatusAccident.LostWorkDaysCase.GetDisplayName();
                            break;

                        case EnumStatusAccident.LostWorkDaysCase:
                            StagingText = EnumStatusAccident.FatalityCase.GetDisplayName();
                            break;

                        case EnumStatusAccident.FatalityCase:
                            StagingText = EnumStatusAccident.FatalityCase.GetDisplayName();
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
                ShowForm = false;
            }
            IsLoading = false;
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
    }
}
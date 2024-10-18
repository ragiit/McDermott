using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components.Web;
using SignaturePad;

namespace McDermott.Web.Components.Pages.Transaction.ProcedureRooms
{
    public partial class CreateUpdateProcedureRoomPage
    {
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

        private string FormUrl = "clinic-service/procedure-rooms";
        private bool IsLoading { get; set; } = true;

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [SupplyParameterFromQuery]
        public long? GcId { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            await GetUserInfo();
            await LoadData();
            IsLoading = false;
        }

        private bool IsStatus(EnumStatusGeneralConsultantServiceProcedureRoom status) => GeneralConsultanMedicalSupport.Status == status;

        private bool VisibleBtn => !IsStatus(EnumStatusGeneralConsultantServiceProcedureRoom.Finish);

        private async Task LoadData()
        {
            if (GcId.HasValue)
            {
                var cek = (await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == GcId && x.Status == EnumStatusGeneralConsultantService.Physician,
                    Select = x => new GeneralConsultanService
                    {
                        Status = x.Status,
                        PatientId = x.PatientId
                    }
                }));

                if (cek is null)
                    NavigationManager.NavigateTo(FormUrl);

                var cekIsAlreadyProcedureRoom = (await Mediator.Send(new GetSingleConfinedSpaceOrProcedureRoomQuery
                {
                    Predicate = x => x.GeneralConsultanServiceId == GcId,
                    Select = x => new GeneralConsultanMedicalSupport
                    {
                        Id = x.Id,
                    }
                }));

                if (cekIsAlreadyProcedureRoom is not null)
                    NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={cekIsAlreadyProcedureRoom.Id}", true);
                else
                {
                    GeneralConsultanMedicalSupport.IsConfinedSpace = false;

                    var r = await Mediator.Send(new GetUserQueryNew
                    {
                        Predicate = x => x.Id == cek.PatientId,
                        Select = x => new User
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Email = x.Email,
                            MobilePhone = x.MobilePhone,
                            Gender = x.Gender,
                            DateOfBirth = x.DateOfBirth,
                            NIP = x.NIP,
                            DepartmentId = x.DepartmentId,
                            JoinDate = x.JoinDate,
                            Department = new Department
                            {
                                Name = x.Department == null ? "" : x.Department.Name
                            },
                            IdCardCountryId = x.IdCardCountryId,
                            IdCardCountry = new Country
                            {
                                Name = x.IdCardCountry == null ? "" : x.IdCardCountry.Name
                            }
                        }
                    });
                    Users = r.Item1;
                    GeneralConsultanMedicalSupport.EmployeeId = cek.PatientId;
                }
            }
            else
            {
                var result = await Mediator.Send(new GetSingleConfinedSpaceOrProcedureRoomQuery
                {
                    Predicate = x => x.Id == Id
                });

                GeneralConsultanMedicalSupport = new();

                if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    if (result is null || !Id.HasValue)
                    {
                        NavigationManager.NavigateTo(FormUrl);
                        return;
                    }

                    GeneralConsultanMedicalSupport = result ?? new();

                    if (GeneralConsultanMedicalSupport.IsConfinedSpace)
                    {
                        var emp1 = (await Mediator.Send(new GetUserQueryNew
                        {
                            Predicate = x => x.Id == GeneralConsultanMedicalSupport.SignatureEmployeeId,
                            Select = x => new User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                MobilePhone = x.MobilePhone,
                                Gender = x.Gender,
                                DateOfBirth = x.DateOfBirth,
                            }
                        })).Item1;
                        Employees1 = emp1;

                        var emp2 = (await Mediator.Send(new GetUserQueryNew
                        {
                            Predicate = x => x.Id == GeneralConsultanMedicalSupport.ExaminedPhysicianId,
                            Select = x => new User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                MobilePhone = x.MobilePhone,
                                Gender = x.Gender,
                                DateOfBirth = x.DateOfBirth,
                            }
                        })).Item1;
                        Employees2 = emp2;
                    }
                    else
                    {
                        PractitionerLabResults = (await Mediator.Send(new GetUserQueryNew
                        {
                            Predicate = x => x.Id == GeneralConsultanMedicalSupport.PractitionerLabEximinationId,
                            Select = x => new User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                MobilePhone = x.MobilePhone,
                                Gender = x.Gender,
                                DateOfBirth = x.DateOfBirth,
                            }
                        })).Item1;
                        PractitionerRadiologyResults = (await Mediator.Send(new GetUserQueryNew
                        {
                            Predicate = x => x.Id == GeneralConsultanMedicalSupport.PractitionerRadiologyEximinationId,
                            Select = x => new User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                MobilePhone = x.MobilePhone,
                                Gender = x.Gender,
                                DateOfBirth = x.DateOfBirth,
                            }
                        })).Item1;
                        PractitionerAlcoholResults = (await Mediator.Send(new GetUserQueryNew
                        {
                            Predicate = x => x.Id == GeneralConsultanMedicalSupport.PractitionerAlcoholEximinationId,
                            Select = x => new User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                MobilePhone = x.MobilePhone,
                                Gender = x.Gender,
                                DateOfBirth = x.DateOfBirth,
                            }
                        })).Item1;
                        PractitionerDrugResults = (await Mediator.Send(new GetUserQueryNew
                        {
                            Predicate = x => x.Id == GeneralConsultanMedicalSupport.PractitionerDrugEximinationId,
                            Select = x => new User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                MobilePhone = x.MobilePhone,
                                Gender = x.Gender,
                                DateOfBirth = x.DateOfBirth,
                            }
                        })).Item1;
                        PractitionerECGResults = (await Mediator.Send(new GetUserQueryNew
                        {
                            Predicate = x => x.Id == GeneralConsultanMedicalSupport.PractitionerECGId,
                            Select = x => new User
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Email = x.Email,
                                MobilePhone = x.MobilePhone,
                                Gender = x.Gender,
                                DateOfBirth = x.DateOfBirth,
                            }
                        })).Item1;

                        switch (GeneralConsultanMedicalSupport.Status)
                        {
                            case EnumStatusGeneralConsultantServiceProcedureRoom.Draft:
                                StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.InProgress;
                                break;

                            case EnumStatusGeneralConsultantServiceProcedureRoom.InProgress:
                                StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.Finish;
                                break;

                            case EnumStatusGeneralConsultantServiceProcedureRoom.Finish:
                                StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.Finish;
                                break;

                            default:
                                break;
                        }
                    }
                }
                else
                {
                    GeneralConsultanMedicalSupport.IsConfinedSpace = true;
                }

                Users = (await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.Id == GeneralConsultanMedicalSupport.EmployeeId,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NIP = x.NIP,
                        DepartmentId = x.DepartmentId,
                        JoinDate = x.JoinDate,
                        Department = new Department
                        {
                            Name = x.Department == null ? "" : x.Department.Name
                        },
                        IdCardCountryId = x.IdCardCountryId,
                        IdCardCountry = new Country
                        {
                            Name = x.IdCardCountry == null ? "" : x.IdCardCountry.Name
                        }
                    }
                })).Item1;

                SelectedPatient = Users.FirstOrDefault() ?? new();
            }
        }

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                return;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        private async Task HandleValidSubmit(int state)
        {
            IsLoading = true;
            try
            {
                // Save
                if (state == 1)
                {
                    if (!GeneralConsultanMedicalSupport.IsConfinedSpace)
                        GeneralConsultanMedicalSupport.GeneralConsultanServiceId = GcId;

                    if (GeneralConsultanMedicalSupport.Id == 0)
                    {
                        await Mediator.Send(new UpdateStatusGeneralConsultanServiceRequest
                        {
                            Id = GcId.GetValueOrDefault(),
                            Status = EnumStatusGeneralConsultantService.ProcedureRoom
                        });

                        GeneralConsultanMedicalSupport = await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                    }
                    else
                        GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                }
                // Save with Confirmation
                else
                {
                    switch (StagingText)
                    {
                        case EnumStatusGeneralConsultantServiceProcedureRoom.InProgress:

                            if (GeneralConsultanMedicalSupport.Id == 0)
                            {
                                StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.Finish;

                                await Mediator.Send(new UpdateStatusGeneralConsultanServiceRequest
                                {
                                    Id = GcId.GetValueOrDefault(),
                                    Status = EnumStatusGeneralConsultantService.ProcedureRoom
                                });

                                GeneralConsultanMedicalSupport.GeneralConsultanServiceId = GcId;
                                GeneralConsultanMedicalSupport.Status = EnumStatusGeneralConsultantServiceProcedureRoom.InProgress;
                                GeneralConsultanMedicalSupport = await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                            }
                            break;

                        case EnumStatusGeneralConsultantServiceProcedureRoom.Finish:
                            await Mediator.Send(new UpdateStatusGeneralConsultanServiceRequest
                            {
                                Id = GeneralConsultanMedicalSupport.GeneralConsultanServiceId.GetValueOrDefault(),
                                Status = EnumStatusGeneralConsultantService.Waiting
                            });

                            GeneralConsultanMedicalSupport.Status = EnumStatusGeneralConsultantServiceProcedureRoom.Finish;
                            GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                            break;

                        default:
                            break;
                    }
                }
                IsLoading = true;

                NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanMedicalSupport.Id}");
            }
            catch (Exception ex)
            {
                IsLoading = false;
                ex.HandleException(ToastService);
            }
            finally
            {
                IsLoading = false;
            }
        }

        #region Procedure Room

        #region ComboboxUser

        private List<UserDto> Users { get; set; } = [];
        private DxComboBox<UserDto?, long?> refUserComboBox { get; set; }
        private int UserComboBoxIndex { get; set; } = 0;
        private int totalCountUser = 0;

        private async Task OnSearchUser()
        {
            await LoadDataUser();
        }

        private async Task OnSearchUserIndexIncrement()
        {
            if (UserComboBoxIndex < (totalCountUser - 1))
            {
                UserComboBoxIndex++;
                await LoadDataUser(UserComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUserIndexDecrement()
        {
            if (UserComboBoxIndex > 0)
            {
                UserComboBoxIndex--;
                await LoadDataUser(UserComboBoxIndex, 10);
            }
        }

        private async Task OnInputUserChanged(string e)
        {
            UserComboBoxIndex = 0;
            await LoadDataUser();
        }

        private UserDto SelectedPatient { get; set; } = new();

        private void SelectedItemPatientChanged(UserDto? e)
        {
            SelectedPatient = new();
            if (e is null)
                return;

            SelectedPatient = e;
        }

        private async Task LoadDataUser(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    SearchTerm = refUserComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NIP = x.NIP,
                        DepartmentId = x.DepartmentId,
                        JoinDate = x.JoinDate,
                        Department = new Department
                        {
                            Name = x.Department == null ? "" : x.Department.Name
                        },
                        IdCardCountryId = x.IdCardCountryId,
                        IdCardCountry = new Country
                        {
                            Name = x.IdCardCountry == null ? "" : x.IdCardCountry.Name
                        }
                    }
                });
                Users = result.Item1;
                totalCountUser = result.PageCount;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
        }

        #endregion ComboboxUser

        public class YesNoOptions
        {
            public string Text { get; set; }
            public bool Value { get; set; }
        }

        private IEnumerable<YesNoOptions> Options = new[]
        {
            new YesNoOptions { Text = "Yes", Value = true },
            new YesNoOptions { Text = "No", Value = false }
        };

        #region ComboboxEmployee1

        private DxComboBox<UserDto, long?> refEmployee1ComboBox { get; set; }
        private int Employee1ComboBoxIndex { get; set; } = 0;
        private int totalCountEmployee1 = 0;

        private async Task OnSearchEmployee1()
        {
            await LoadDataEmployee1();
        }

        private async Task OnSearchEmployee1IndexIncrement()
        {
            if (Employee1ComboBoxIndex < (totalCountEmployee1 - 1))
            {
                Employee1ComboBoxIndex++;
                await LoadDataEmployee1(Employee1ComboBoxIndex, 10);
            }
        }

        private async Task OnSearchEmployee1IndexDecrement()
        {
            if (Employee1ComboBoxIndex > 0)
            {
                Employee1ComboBoxIndex--;
                await LoadDataEmployee1(Employee1ComboBoxIndex, 10);
            }
        }

        private async Task OnInputEmployee1Changed(string e)
        {
            Employee1ComboBoxIndex = 0;
            await LoadDataEmployee1();
        }

        private SignaturePadOptions _options = new SignaturePadOptions
        {
            LineCap = LineCap.Round,
            LineJoin = LineJoin.Round,
        };

        private IEnumerable<string> Recommends =
        [
            "FIT",
            "UNFIT",
            "REASSESS"
        ];

        private List<UserDto> Employees1 { get; set; } = [];

        private async Task LoadDataEmployee1(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    SearchTerm = refEmployee1ComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                    }
                });
                Employees1 = result.Item1;
                totalCountEmployee1 = result.PageCount;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
        }

        #endregion ComboboxEmployee1

        #region ComboboxEmployee2

        private DxComboBox<UserDto, long?> refEmployee2ComboBox { get; set; }
        private int Employee2ComboBoxIndex { get; set; } = 0;
        private int totalCountEmployee2 = 0;

        private async Task OnSearchEmployee2()
        {
            await LoadDataEmployee2();
        }

        private async Task OnSearchEmployee2IndexIncrement()
        {
            if (Employee2ComboBoxIndex < (totalCountEmployee2 - 1))
            {
                Employee2ComboBoxIndex++;
                await LoadDataEmployee2(Employee2ComboBoxIndex, 10);
            }
        }

        private async Task OnSearchEmployee2IndexDecrement()
        {
            if (Employee2ComboBoxIndex > 0)
            {
                Employee2ComboBoxIndex--;
                await LoadDataEmployee2(Employee2ComboBoxIndex, 10);
            }
        }

        private async Task OnInputEmployee2Changed(string e)
        {
            Employee2ComboBoxIndex = 0;
            await LoadDataEmployee2();
        }

        private List<UserDto> Employees2 { get; set; } = [];

        private async Task LoadDataEmployee2(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    SearchTerm = refEmployee2ComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                    }
                });
                Employees2 = result.Item1;
                totalCountEmployee2 = result.PageCount;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
        }

        #endregion ComboboxEmployee2

        #endregion Procedure Room

        #region Procedure Room

        #region ComboboxPractitionerLabResult

        private DxComboBox<UserDto, long?> refPractitionerLabResultComboBox { get; set; }
        private int PractitionerLabResultComboBoxIndex { get; set; } = 0;
        private int totalCountPractitionerLabResult = 0;

        private async Task OnSearchPractitionerLabResult()
        {
            await LoadDataPractitionerLabResult();
        }

        private async Task OnSearchPractitionerLabResultIndexIncrement()
        {
            if (PractitionerLabResultComboBoxIndex < (totalCountPractitionerLabResult - 1))
            {
                PractitionerLabResultComboBoxIndex++;
                await LoadDataPractitionerLabResult(PractitionerLabResultComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPractitionerLabResultIndexDecrement()
        {
            if (PractitionerLabResultComboBoxIndex > 0)
            {
                PractitionerLabResultComboBoxIndex--;
                await LoadDataPractitionerLabResult(PractitionerLabResultComboBoxIndex, 10);
            }
        }

        private async Task OnInputPractitionerLabResultChanged(string e)
        {
            PractitionerLabResultComboBoxIndex = 0;
            await LoadDataPractitionerLabResult();
        }

        private List<UserDto> PractitionerLabResults { get; set; } = [];

        private async Task LoadDataPractitionerLabResult(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.IsDoctor == true && x.IsPhysicion == true,
                    SearchTerm = refPractitionerLabResultComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                    }
                });
                PractitionerLabResults = result.Item1;
                totalCountPractitionerLabResult = result.PageCount;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
        }

        #endregion ComboboxPractitionerLabResult

        #region ComboboxPractitionerRadiologyResult

        private DxComboBox<UserDto, long?> refPractitionerRadiologyResultComboBox { get; set; }
        private int PractitionerRadiologyResultComboBoxIndex { get; set; } = 0;
        private int totalCountPractitionerRadiologyResult = 0;

        private async Task OnSearchPractitionerRadiologyResult()
        {
            await LoadDataPractitionerRadiologyResult();
        }

        private async Task OnSearchPractitionerRadiologyResultIndexIncrement()
        {
            if (PractitionerRadiologyResultComboBoxIndex < (totalCountPractitionerRadiologyResult - 1))
            {
                PractitionerRadiologyResultComboBoxIndex++;
                await LoadDataPractitionerRadiologyResult(PractitionerRadiologyResultComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPractitionerRadiologyResultIndexDecrement()
        {
            if (PractitionerRadiologyResultComboBoxIndex > 0)
            {
                PractitionerRadiologyResultComboBoxIndex--;
                await LoadDataPractitionerRadiologyResult(PractitionerRadiologyResultComboBoxIndex, 10);
            }
        }

        private async Task OnInputPractitionerRadiologyResultChanged(string e)
        {
            PractitionerRadiologyResultComboBoxIndex = 0;
            await LoadDataPractitionerRadiologyResult();
        }

        private List<UserDto> PractitionerRadiologyResults { get; set; } = [];

        private async Task LoadDataPractitionerRadiologyResult(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.IsDoctor == true && x.IsPhysicion == true,
                    SearchTerm = refPractitionerRadiologyResultComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                    }
                });
                PractitionerRadiologyResults = result.Item1;
                totalCountPractitionerRadiologyResult = result.PageCount;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
        }

        #endregion ComboboxPractitionerRadiologyResult

        #region ComboboxPractitionerAlcoholResult

        private DxComboBox<UserDto, long?> refPractitionerAlcoholResultComboBox { get; set; }
        private int PractitionerAlcoholResultComboBoxIndex { get; set; } = 0;
        private int totalCountPractitionerAlcoholResult = 0;

        private async Task OnSearchPractitionerAlcoholResult()
        {
            await LoadDataPractitionerAlcoholResult();
        }

        private async Task OnSearchPractitionerAlcoholResultIndexIncrement()
        {
            if (PractitionerAlcoholResultComboBoxIndex < (totalCountPractitionerAlcoholResult - 1))
            {
                PractitionerAlcoholResultComboBoxIndex++;
                await LoadDataPractitionerAlcoholResult(PractitionerAlcoholResultComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPractitionerAlcoholResultIndexDecrement()
        {
            if (PractitionerAlcoholResultComboBoxIndex > 0)
            {
                PractitionerAlcoholResultComboBoxIndex--;
                await LoadDataPractitionerAlcoholResult(PractitionerAlcoholResultComboBoxIndex, 10);
            }
        }

        private async Task OnInputPractitionerAlcoholResultChanged(string e)
        {
            PractitionerAlcoholResultComboBoxIndex = 0;
            await LoadDataPractitionerAlcoholResult();
        }

        private List<UserDto> PractitionerAlcoholResults { get; set; } = [];

        private async Task LoadDataPractitionerAlcoholResult(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.IsDoctor == true && x.IsPhysicion == true,
                    SearchTerm = refPractitionerAlcoholResultComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                    }
                });
                PractitionerAlcoholResults = result.Item1;
                totalCountPractitionerAlcoholResult = result.PageCount;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
        }

        #endregion ComboboxPractitionerAlcoholResult

        #region ComboboxPractitionerDrugResult

        private DxComboBox<UserDto, long?> refPractitionerDrugResultComboBox { get; set; }
        private int PractitionerDrugResultComboBoxIndex { get; set; } = 0;
        private int totalCountPractitionerDrugResult = 0;

        private async Task OnSearchPractitionerDrugResult()
        {
            await LoadDataPractitionerDrugResult();
        }

        private async Task OnSearchPractitionerDrugResultIndexIncrement()
        {
            if (PractitionerDrugResultComboBoxIndex < (totalCountPractitionerDrugResult - 1))
            {
                PractitionerDrugResultComboBoxIndex++;
                await LoadDataPractitionerDrugResult(PractitionerDrugResultComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPractitionerDrugResultIndexDecrement()
        {
            if (PractitionerDrugResultComboBoxIndex > 0)
            {
                PractitionerDrugResultComboBoxIndex--;
                await LoadDataPractitionerDrugResult(PractitionerDrugResultComboBoxIndex, 10);
            }
        }

        private async Task OnInputPractitionerDrugResultChanged(string e)
        {
            PractitionerDrugResultComboBoxIndex = 0;
            await LoadDataPractitionerDrugResult();
        }

        private List<UserDto> PractitionerDrugResults { get; set; } = [];

        private async Task LoadDataPractitionerDrugResult(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.IsDoctor == true && x.IsPhysicion == true,
                    SearchTerm = refPractitionerDrugResultComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                    }
                });
                PractitionerDrugResults = result.Item1;
                totalCountPractitionerDrugResult = result.PageCount;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
        }

        #endregion ComboboxPractitionerDrugResult

        #region ComboboxPractitionerECGResult

        private DxComboBox<UserDto, long?> refPractitionerECGResultComboBox { get; set; }
        private int PractitionerECGResultComboBoxIndex { get; set; } = 0;
        private int totalCountPractitionerECGResult = 0;

        private async Task OnSearchPractitionerECGResult()
        {
            await LoadDataPractitionerECGResult();
        }

        private async Task OnSearchPractitionerECGResultIndexIncrement()
        {
            if (PractitionerECGResultComboBoxIndex < (totalCountPractitionerECGResult - 1))
            {
                PractitionerECGResultComboBoxIndex++;
                await LoadDataPractitionerECGResult(PractitionerECGResultComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPractitionerECGResultIndexDecrement()
        {
            if (PractitionerECGResultComboBoxIndex > 0)
            {
                PractitionerECGResultComboBoxIndex--;
                await LoadDataPractitionerECGResult(PractitionerECGResultComboBoxIndex, 10);
            }
        }

        private async Task OnInputPractitionerECGResultChanged(string e)
        {
            PractitionerECGResultComboBoxIndex = 0;
            await LoadDataPractitionerECGResult();
        }

        private List<UserDto> PractitionerECGResults { get; set; } = [];

        private async Task LoadDataPractitionerECGResult(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.IsDoctor == true && x.IsPhysicion == true,
                    SearchTerm = refPractitionerECGResultComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                    }
                });
                PractitionerECGResults = result.Item1;
                totalCountPractitionerECGResult = result.PageCount;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoading = false; }
        }

        #endregion ComboboxPractitionerECGResult

        private EnumStatusGeneralConsultantServiceProcedureRoom StagingText { get; set; } = EnumStatusGeneralConsultantServiceProcedureRoom.InProgress;

        private async Task OnClickConfirm()
        {
        }

        #endregion Procedure Room
    }
}
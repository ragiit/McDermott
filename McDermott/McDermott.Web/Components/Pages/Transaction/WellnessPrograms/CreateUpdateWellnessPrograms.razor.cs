using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramAttendanceCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramSessionCommand;

namespace McDermott.Web.Components.Pages.Transaction.WellnessPrograms
{
    public partial class CreateUpdateWellnessPrograms
    {
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
                await GetUserInfo();
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

        private bool IsStatus(EnumWellness status) => WellnessProgram.Status == status;

        private EnumWellness StagingText { get; set; } = EnumWellness.Active;

        private async Task OnCancelStatus()
        {
            try
            {
                IsLoading = true;

                if (WellnessProgram.Id != 0)
                {
                    WellnessProgram.Status = EnumWellness.Inactive;
                    WellnessProgram = await Mediator.Send(new CancelWellnessProgramRequest(WellnessProgram));
                    StagingText = EnumWellness.Inactive;
                    ToastService.ShowSuccess("The Wellness Program has been successfully set to inactive. The patient is no longer active in the program.");
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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid GridAttendance { get; set; }

        //private List<AttendanceDto> AttendanceRecords { get; set; } = [];
        private bool PanelVisible { get; set; } = false;

        private bool IsLoading { get; set; } = false;
        private WellnessProgramDto WellnessProgram { get; set; } = new();
        private List<string> Categories = new() { "Mental Health", "Dietary Awareness", "Physical Exercise" };

        //private List<string> Diagnoses = new() { "Hypertension", "Diabetes", "Depression" };
        private async Task ClickConfirm()
        {
            try
            {
                PanelVisible = true;

                switch (WellnessProgram.Status)
                {
                    case EnumWellness.Draft:
                        WellnessProgram.Status = EnumWellness.Active;
                        StagingText = EnumWellness.Completed;
                        break;

                    case EnumWellness.Active:
                        WellnessProgram.Status = EnumWellness.Completed;
                        StagingText = EnumWellness.Completed;
                        break;

                    default:
                        break;
                }

                if (WellnessProgram.Status == EnumWellness.Active)
                {
                    // Convert the ProgramName to a slug (lowercase and spaces replaced with hyphens)
                    WellnessProgram.Slug = WellnessProgram.Name?
                        .ToLower()
                        .Replace(" ", "-")
                        .Replace(",", "")
                        .Replace(".", "");
                }

                if (WellnessProgram.Id == 0)
                {
                    var ye = await Mediator.Send(new CreateWellnessProgramRequest(WellnessProgram));
                    NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={ye.Id}", true);
                }
                else
                {
                    WellnessProgram = await Mediator.Send(new UpdateWellnessProgramRequest(WellnessProgram));

                    NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={WellnessProgram.Id}");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                PanelVisible = true;

                if (WellnessProgram.Id == 0)
                {
                    var ye = await Mediator.Send(new CreateWellnessProgramRequest(WellnessProgram));
                    NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={ye.Id}", true);
                }
                else
                {
                    WellnessProgram = await Mediator.Send(new UpdateWellnessProgramRequest(WellnessProgram));

                    NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={WellnessProgram.Id}");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }

        private void CancelForm_Click()
        {
            NavigationManager.NavigateTo("clinic-service/wellness");
        }

        private int FocusedRowVisibleIndex { get; set; }

        private async Task NewSession_Click()
        {
            await GridSessionSchedule.StartEditNewRowAsync();
        }

        private async Task EditSession_Click()
        {
            await GridSessionSchedule.StartEditRowAsync(FocusedRowScheduleVisibleIndex);
            //var a = (GridSessionSchedule.GetDataItem(FocusedRowScheduleVisibleIndex) as WellnessProgramAttendanceDto ?? new());
            //Users = (await Mediator.Send(new GetUserQueryNew
            //{
            //    Predicate = x => x.Id == a.PatientId,
            //})).Item1;
        }

        private void DeleteSession_Click()
        {
            GridSessionSchedule.ShowRowDeleteConfirmation(FocusedRowScheduleVisibleIndex);
        }

        private void RefreshSessions_Click()
        {
            // Logic to refresh session list
        }

        private async Task NewAttendance_Click()
        {
            await GridAttendance.StartEditNewRowAsync();
        }

        private int FocusedRowAttendenceVisibleIndex { get; set; }

        private async Task EditAttendance_Click()
        {
            await GridAttendance.StartEditRowAsync(FocusedRowAttendenceVisibleIndex);
            var a = (GridAttendance.GetDataItem(FocusedRowAttendenceVisibleIndex) as WellnessProgramAttendanceDto ?? new());
            Users = (await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.Id == a.PatientId,
            })).Item1;
        }

        private int FocusedRowSessionVisibleIndex { get; set; }

        private void GridFocusedRowSessionVisibleIndex_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowAttendenceVisibleIndex = args.VisibleIndex;
        }

        private void DeleteAttendance_Click()
        {
            GridAttendance.ShowRowDeleteConfirmation(FocusedRowAttendenceVisibleIndex);
        }

        private async Task OnDeleteAttendence(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemWellnessProgramSessions == null || !SelectedDataItemWellnessProgramSessions.Any())
                {
                    await Mediator.Send(new DeleteWellnessProgramSessionRequest
                    {
                        Id = ((WellnessProgramSessionDto)e.DataItem).Id
                    });
                }
                else
                {
                    var countriesToDelete = SelectedDataItemWellnessProgramSessions.Adapt<List<WellnessProgramSessionDto>>();
                    //await Mediator.Send(new DeleteWellnessProgramSessionRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                    await Mediator.Send(new DeleteWellnessProgramSessionRequest
                    {
                        Ids = countriesToDelete.Select(x => x.Id).ToList()
                    });
                }

                SelectedDataItemWellnessProgramSessions = [];
                //await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSaveAttendence(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (WellnessProgramAttendanceDto)e.EditModel;

                //bool validate = await Mediator.Send(new ValidateCountryQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.Code == editModel.Code));

                //if (validate)
                //{
                //    ToastService.ShowInfo($"Country with name '{editModel.Name}' and code '{editModel.Code}' is already exists");
                //    e.Cancel = true;
                //    return;
                //}

                editModel.WellnessProgramId = WellnessProgram.Id;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateWellnessProgramAttendanceRequest(editModel));
                else
                    await Mediator.Send(new UpdateWellnessProgramAttendanceRequest(editModel));

                SelectedDataItemWellnessProgramAttendances = [];
                await LoadDataOnSearchBoxChangedWellnessProgramSession();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void RefreshAttendance_Click()
        {
            // Logic to refresh attendance list
        }

        #region ComboboxDiagnosis

        private DxComboBox<DiagnosisDto, long> refDiagnosisComboBox { get; set; }
        private int DiagnosisComboBoxIndex { get; set; } = 0;
        private int totalCountDiagnosis = 0;

        private async Task OnSearchDiagnosis()
        {
            await LoadDataDiagnosis();
        }

        private async Task OnSearchDiagnosisIndexIncrement()
        {
            if (DiagnosisComboBoxIndex < (totalCountDiagnosis - 1))
            {
                DiagnosisComboBoxIndex++;
                await LoadDataDiagnosis(DiagnosisComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDiagnosisIndexDecrement()
        {
            if (DiagnosisComboBoxIndex > 0)
            {
                DiagnosisComboBoxIndex--;
                await LoadDataDiagnosis(DiagnosisComboBoxIndex, 10);
            }
        }

        private async Task OnInputDiagnosisChanged(string e)
        {
            DiagnosisComboBoxIndex = 0;
            await LoadDataDiagnosis();
        }

        private List<DiagnosisDto> Diagnoses { get; set; } = [];

        private async Task LoadDataDiagnosis(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetDiagnosisQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDiagnosisComboBox?.Text ?? ""));

                Diagnoses = result.Item1;
                totalCountDiagnosis = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxDiagnosis

        #region Schedule

        //private List<WellnessProgramSessionDto>

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        private async Task LoadData()
        {
            //var result = await MyQuery.GetGroups(HttpClientFactory, 0, 1, Id.HasValue ? Id.ToString() : "");

            //Id = McDermott.Extentions.SecureHelper.DecryptIdFromBase64(Ids);
            //var result = await Mediator.QueryGetHelper<Group, GroupDto>(0, 0, predicate: x => x.Id == Id);

            var result = await Mediator.Send(new GetSingleWellnessProgramQuery
            {
                Predicate = x => x.Id == Id
            });

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result == null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("clinic-service/wellness");
                    return;
                }

                WellnessProgram = result ?? new();

                switch (WellnessProgram.Status)
                {
                    case EnumWellness.Draft:
                        StagingText = EnumWellness.Active;
                        break;

                    case EnumWellness.Active:
                        StagingText = EnumWellness.Completed;
                        break;

                    case EnumWellness.Completed:
                        StagingText = EnumWellness.Completed;
                        break;

                    case EnumWellness.Inactive:
                        StagingText = EnumWellness.Inactive;
                        break;

                    default:
                        break;
                }

                await LoadDataOnSearchBoxChangedWellnessProgramAttendance();
                await LoadDataOnSearchBoxChangedWellnessProgramSession();
            }
        }

        #region Searching

        private IGrid GridSessionSchedule { get; set; }
        private int pageSizeWellnessProgramSession { get; set; } = 10;
        private int totalCountWellnessProgramSession = 0;
        private int activePageIndexWellnessProgramSession { get; set; } = 0;
        private string searchTermWellnessProgramSession { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedWellnessProgramSession(string searchText)
        {
            searchTermWellnessProgramSession = searchText;
            await LoadDataOnSearchBoxChangedWellnessProgramSession(0, pageSizeWellnessProgramSession);
        }

        private async Task OnpageSizeWellnessProgramSessionIndexChanged(int newpageSizeWellnessProgramSession)
        {
            pageSizeWellnessProgramSession = newpageSizeWellnessProgramSession;
            await LoadDataOnSearchBoxChangedWellnessProgramSession(0, newpageSizeWellnessProgramSession);
        }

        private async Task OnPageIndexChangedOnSearchBoxChangedWellnessProgramSession(int newPageIndex)
        {
            await LoadDataOnSearchBoxChangedWellnessProgramSession(newPageIndex, pageSizeWellnessProgramSession);
        }

        private IReadOnlyList<object> SelectedDataItemWellnessProgramSessions { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemWellnessProgramAttendances { get; set; } = [];
        private List<WellnessProgramSessionDto> WellnessProgramSessions { get; set; } = [];

        private async Task LoadDataOnSearchBoxChangedWellnessProgramSession(int pageIndex = 0, int pageSizeWellnessProgramSession = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemWellnessProgramSessions = [];
                var result = await Mediator.Send(new GetWellnessProgramSessionQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSizeWellnessProgramSession,
                    SearchTerm = searchTermWellnessProgramSession,
                });
                WellnessProgramSessions = result.Item1;
                totalCountWellnessProgramSession = result.PageCount;
                activePageIndexWellnessProgramSession = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        private int FocusedRowScheduleVisibleIndex { get; set; }

        private void GridFocusedRowScheduleVisibleIndex_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowScheduleVisibleIndex = args.VisibleIndex;
        }

        private async Task OnDeleteSchedule(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemWellnessProgramSessions == null || !SelectedDataItemWellnessProgramSessions.Any())
                {
                    //await Mediator.Send(new DeleteWellnessProgramSessionRequest(((WellnessProgramSessionDto)e.DataItem).Id));
                }
                else
                {
                    var countriesToDelete = SelectedDataItemWellnessProgramSessions.Adapt<List<WellnessProgramSessionDto>>();
                    //await Mediator.Send(new DeleteWellnessProgramSessionRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                }

                SelectedDataItemWellnessProgramSessions = [];
                //await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSaveSchedule(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (WellnessProgramSessionDto)e.EditModel;

                //bool validate = await Mediator.Send(new ValidateCountryQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.Code == editModel.Code));

                //if (validate)
                //{
                //    ToastService.ShowInfo($"Country with name '{editModel.Name}' and code '{editModel.Code}' is already exists");
                //    e.Cancel = true;
                //    return;
                //}

                editModel.WellnessProgramId = WellnessProgram.Id;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateWellnessProgramSessionRequest(editModel));
                else
                    await Mediator.Send(new UpdateWellnessProgramSessionRequest(editModel));

                SelectedDataItemWellnessProgramSessions = [];
                await LoadDataOnSearchBoxChangedWellnessProgramSession();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Schedule

        #region Searching

        private int pageSizeWellnessProgramAttendance { get; set; } = 10;
        private int totalCountWellnessProgramAttendance = 0;
        private int activePageIndexWellnessProgramAttendance { get; set; } = 0;
        private string searchTermWellnessProgramAttendance { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedWellnessProgramAttendance(string searchText)
        {
            searchTermWellnessProgramAttendance = searchText;
            await LoadDataOnSearchBoxChangedWellnessProgramAttendance(0, pageSizeWellnessProgramAttendance);
        }

        private async Task OnpageSizeWellnessProgramAttendanceIndexChanged(int newpageSizeWellnessProgramAttendance)
        {
            pageSizeWellnessProgramAttendance = newpageSizeWellnessProgramAttendance;
            await LoadDataOnSearchBoxChangedWellnessProgramAttendance(0, newpageSizeWellnessProgramAttendance);
        }

        private async Task OnPageIndexChangedOnSearchBoxChangedWellnessProgramAttendance(int newPageIndex)
        {
            await LoadDataOnSearchBoxChangedWellnessProgramAttendance(newPageIndex, pageSizeWellnessProgramAttendance);
        }

        private List<WellnessProgramAttendanceDto> WellnessProgramAttendances { get; set; } = [];

        private async Task LoadDataOnSearchBoxChangedWellnessProgramAttendance(int pageIndex = 0, int pageSizeWellnessProgramAttendance = 10)
        {
            try
            {
                PanelVisible = true;
                //SelectedDataItems = new ObservableRangeCollection<object>();
                //var result = await Mediator.Send(new GetWellnessProgramSessionQuery
                //{
                //    PageIndex = pageIndex,
                //    PageSize = pageSizeWellnessProgramAttendance,
                //    SearchTerm = searchTermWellnessProgramAttendance,
                //});
                //WellnessProgramAttendances = result.Item1;
                //totalCountWellnessProgramAttendance = result.PageCount;
                activePageIndexWellnessProgramAttendance = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #region ComboboxUser

        private List<UserDto> Users { get; set; } = [];
        private DxComboBox<UserDto, long> refUserComboBox { get; set; }
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

        private async Task LoadDataUser(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    SearchTerm = refUserComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                Users = result.Item1;
                totalCountUser = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxUser

        #endregion Searching

        #region Attendence

        #region Searching

        private int pageSizeWellnessProgramAttendanceAttendance { get; set; } = 10;
        private int totalCountWellnessProgramAttendanceAttendance = 0;
        private int activePageIndexWellnessProgramAttendanceAttendance { get; set; } = 0;
        private string searchTermWellnessProgramAttendanceAttendance { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedWellnessProgramAttendanceAttendance(string searchText)
        {
            searchTermWellnessProgramAttendanceAttendance = searchText;
            await LoadDataOnSearchBoxChanged(0, pageSizeWellnessProgramAttendanceAttendance);
        }

        private async Task OnpageSizeWellnessProgramAttendanceAttendanceIndexChanged(int newpageSizeWellnessProgramAttendanceAttendance)
        {
            pageSizeWellnessProgramAttendanceAttendance = newpageSizeWellnessProgramAttendanceAttendance;
            await LoadDataOnSearchBoxChanged(0, newpageSizeWellnessProgramAttendanceAttendance);
        }

        private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
        {
            await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeWellnessProgramAttendanceAttendance);
        }

        private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeWellnessProgramAttendanceAttendance = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemWellnessProgramAttendances = new ObservableRangeCollection<object>();
                var result = await Mediator.Send(new GetWellnessProgramAttendanceQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSizeWellnessProgramAttendanceAttendance,
                    SearchTerm = searchTermWellnessProgramAttendanceAttendance,
                });
                WellnessProgramAttendances = result.Item1;
                totalCountWellnessProgramAttendanceAttendance = result.PageCount;
                activePageIndexWellnessProgramAttendanceAttendance = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        #endregion Attendence
    }
}
using Blazored.TextEditor;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramAttendanceCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramDetailCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramParticipantCommand;
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

        #region HTML Editor

        private bool IsShowPreviewOutput { get; set; } = false;
        private BlazoredTextEditor QuillHtml;
        private MarkupString preview;

        private async Task ShowAoutPutPreview(bool b)
        {
            IsShowPreviewOutput = b;

            if (!string.IsNullOrWhiteSpace(await QuillHtml.GetText()))
            {
                preview = (MarkupString)await QuillHtml.GetHTML();
            }
        }

        #endregion HTML Editor

        private async Task OnClickSlugProgram()
        {
            //NavigationManager.NavigateTo($"clinic-service/wellness/join-participant/{WellnessProgram.Slug}");
            var url = $"clinic-service/wellness/join-participant/{WellnessProgram.Slug}";
            await JsRuntime.InvokeVoidAsync("openInNewTab", NavigationManager.ToAbsoluteUri(url).ToString());
        }

        private async Task OnClickSlugProgramActivity(string slug)
        {
            //NavigationManager.NavigateTo($"clinic-service/wellness/join-participant/{WellnessProgram.Slug}");
            var url = $"clinic-service/wellness/attendance/{slug}";
            await JsRuntime.InvokeVoidAsync("openInNewTab", NavigationManager.ToAbsoluteUri(url).ToString());
        }

        private bool IsStatus(EnumWellness status) => WellnessProgram.Status == status;

        private EnumWellness StagingText { get; set; } = EnumWellness.Active;

        private bool EnableForm => !IsStatus(EnumWellness.Inactive) && !IsStatus(EnumWellness.Completed);

        private async Task OnCancelStatus()
        {
            try
            {
                IsLoading = true;

                if (WellnessProgram.Id != 0)
                {
                    WellnessProgram.Status = EnumWellness.Inactive;
                    await Mediator.Send(new CancelWellnessProgramRequest(WellnessProgram));
                    StagingText = EnumWellness.Active;
                    ToastService.ShowSuccess("The Wellness Program has been successfully set to inactive. The patient is no longer active in the program.");
                    WellnessProgram = await Mediator.Send(new GetSingleWellnessProgramQuery
                    {
                        Predicate = x => x.Id == Id
                    });
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

        //private List<AttendanceDto> AttendanceRecords { get; set; } = [];
        private bool PanelVisible { get; set; } = false;

        private bool IsLoading { get; set; } = false;
        private WellnessProgramDto WellnessProgram { get; set; } = new();
        private List<string> Categories = new() { "Mental Health", "Dietary Awareness", "Physical Exercise" };

        //private List<string> Diagnoses = new() { "Hypertension", "Diabetes", "Depression" };
        private async Task ClickSetToDraft()
        {
            try
            {
                PanelVisible = true;
                WellnessProgram.Status = EnumWellness.Draft;
                await Mediator.Send(new CancelWellnessProgramRequest(WellnessProgram));
                ToastService.ShowSuccess("The program status has been successfully reset to Draft from Inactive.");
                NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={WellnessProgram.Id}");
                StagingText = EnumWellness.Active;
                WellnessProgram = await Mediator.Send(new GetSingleWellnessProgramQuery
                {
                    Predicate = x => x.Id == Id
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task ClickConfirm()
        {
            try
            {
                PanelVisible = true;
                if (WellnessProgram.Status == EnumWellness.Draft)
                {
                    // Convert the ProgramName to a slug (lowercase and spaces replaced with hyphens)
                    WellnessProgram.Slug = WellnessProgram.Name?
                        .ToLower()
                        .Replace(" ", "-")
                        .Replace(",", "")
                        .Replace(".", "");
                }

                var cek = await Mediator.Send(new ValidateWellnessProgram(x => x.Slug == WellnessProgram.Slug && x.Id != WellnessProgram.Id));
                if (cek)
                {
                    ToastService.ShowWarning("Program Name is already exist!");
                    return;
                }

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

                var content = await QuillHtml.GetContent();
                // Cek apakah konten kosong atau hanya berisi newline
                if (!string.IsNullOrWhiteSpace(content) && content != "{\"ops\":[{\"insert\":\" \\n\"}]}")
                {
                    WellnessProgram.Content = await QuillHtml.GetHTML();
                }
                else
                {
                    // Jika konten kosong atau hanya newline, tidak melakukan apa-apa
                }

                if (WellnessProgram.EndDate is not null)
                    WellnessProgram.EndDate = WellnessProgram.EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                WellnessProgram.StartDate = WellnessProgram.StartDate.Date.AddHours(0).AddMinutes(0).AddSeconds(0);

                var temp = new WellnessProgramDto();
                if (WellnessProgram.Id == 0)
                {
                    temp = await Mediator.Send(new CreateWellnessProgramRequest(WellnessProgram));
                    NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={temp.Id}", true);
                    //var resultx = await Mediator.Send(new GetAwarenessEduCategoryQuery
                    //{
                    //    Predicate = x => x.Id == WellnessProgram.AwarenessEduCategoryId.GetValueOrDefault()
                    //});
                    //AwarenessEduCategories = resultx.Item1;
                }
                else
                {
                    temp = await Mediator.Send(new UpdateWellnessProgramRequest(WellnessProgram));

                    NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={temp.Id}");
                }

                Id = temp.Id;
                var result = await Mediator.Send(new GetSingleWellnessProgramQuery
                {
                    Predicate = x => x.Id == Id
                });

                WellnessProgram = result ?? new();
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

                WellnessProgram.Slug = WellnessProgram.Name?
                    .ToLower()
                    .Replace(" ", "-")
                    .Replace(",", "")
                    .Replace(".", "");

                var cek = await Mediator.Send(new ValidateWellnessProgram(x => x.Slug == WellnessProgram.Slug && x.Id != WellnessProgram.Id));
                if (cek)
                {
                    ToastService.ShowWarning("Program Name is already exist!");
                    return;
                }

                //if (!string.IsNullOrWhiteSpace(await QuillHtml.GetContent()))
                //{
                //    WellnessProgram.Content = await QuillHtml.GetHTML();
                //}

                var content = await QuillHtml.GetContent();

                // Cek apakah konten kosong atau hanya berisi newline
                if (!string.IsNullOrWhiteSpace(content) && content != "{\"ops\":[{\"insert\":\" \\n\"}]}")
                {
                    WellnessProgram.Content = await QuillHtml.GetHTML();
                }
                else
                {
                    // Jika konten kosong atau hanya newline, tidak melakukan apa-apa
                }

                if (WellnessProgram.EndDate is not null)
                    WellnessProgram.EndDate = WellnessProgram.EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                WellnessProgram.StartDate = WellnessProgram.StartDate.Date.AddHours(0).AddMinutes(0).AddSeconds(0);

                var temp = new WellnessProgramDto();
                if (WellnessProgram.Id == 0)
                {
                    temp = await Mediator.Send(new CreateWellnessProgramRequest(WellnessProgram));
                    NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={temp.Id}", true);
                    var resultx = await Mediator.Send(new GetAwarenessEduCategoryQuery
                    {
                        Predicate = x => x.Id == WellnessProgram.AwarenessEduCategoryId.GetValueOrDefault()
                    });
                    AwarenessEduCategories = resultx.Item1;
                }
                else
                {
                    temp = await Mediator.Send(new UpdateWellnessProgramRequest(WellnessProgram));
                    NavigationManager.NavigateTo($"clinic-service/wellness/{EnumPageMode.Update.GetDisplayName()}?Id={temp.Id}");
                }

                Id = temp.Id;
                var result = await Mediator.Send(new GetSingleWellnessProgramQuery
                {
                    Predicate = x => x.Id == Id
                });

                WellnessProgram = result ?? new();
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

        private void DeleteAttendance_Click()
        {
            GridAttendance.ShowRowDeleteConfirmation(FocusedRowAttendenceVisibleIndex);
        }

        private async Task OnDeleteAttendence(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemWellnessProgramDetails == null || !SelectedDataItemWellnessProgramDetails.Any())
                {
                    await Mediator.Send(new DeleteWellnessProgramSessionRequest
                    {
                        Id = ((WellnessProgramSessionDto)e.DataItem).Id
                    });
                }
                else
                {
                    var countriesToDelete = SelectedDataItemWellnessProgramDetails.Adapt<List<WellnessProgramSessionDto>>();
                    //await Mediator.Send(new DeleteWellnessProgramSessionRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                    await Mediator.Send(new DeleteWellnessProgramSessionRequest
                    {
                        Ids = countriesToDelete.Select(x => x.Id).ToList()
                    });
                }

                SelectedDataItemWellnessProgramDetails = [];
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

                //editModel.WellnessProgramId = WellnessProgram.Id;

                //if (editModel.Id == 0)
                //    await Mediator.Send(new CreateWellnessProgramAttendanceRequest(editModel));
                //else
                //    await Mediator.Send(new UpdateWellnessProgramAttendanceRequest(editModel));
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

        private bool isContentLoaded = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (!firstRender)
            //    return;

            ////if (!string.IsNullOrWhiteSpace(WellnessProgram.Content))
            ////{
            ////    await QuillHtml.LoadHTMLContent(WellnessProgram.Content);
            ////    StateHasChanged();
            ////}

            //if (QuillHtml != null && WellnessProgram != null && !string.IsNullOrWhiteSpace(WellnessProgram.Content))
            //{
            //    await QuillHtml.LoadHTMLContent(WellnessProgram.Content);
            //    StateHasChanged();
            //}


            //await GetUserInfo();
            //StateHasChanged();

            //await base.OnAfterRenderAsync(firstRender);
        }

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

                var resultx = await Mediator.Send(new GetAwarenessEduCategoryQuery
                {
                    Predicate = x => x.Id == WellnessProgram.AwarenessEduCategoryId.GetValueOrDefault()
                });
                AwarenessEduCategories = resultx.Item1;
                StateHasChanged();

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

                //await LoadDataOnSearchBoxChangedWellnessProgramAttendance();
                await LoadDataOnSearchBoxChangedWellnessProgramDetail();
            }
        }

        #region Searching

        private IGrid GridActivityDetail { get; set; }
        private List<WellnessProgramDetailDto> WellnessProgramDetails { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemWellnessProgramDetails { get; set; } = [];

        private async Task OnDeleteActivityDetail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemWellnessProgramDetails == null || !SelectedDataItemWellnessProgramDetails.Any())
                {
                    await Mediator.Send(new DeleteWellnessProgramDetailRequest
                    {
                        Id = ((WellnessProgramDetailDto)e.DataItem).Id
                    });
                }
                else
                {
                    var countriesToDelete = SelectedDataItemWellnessProgramDetails.Adapt<List<WellnessProgramDetailDto>>();
                    await Mediator.Send(new DeleteWellnessProgramDetailRequest
                    {
                        Ids = countriesToDelete.Select(x => x.Id).ToList()
                    });
                }

                SelectedDataItemWellnessProgramDetails = [];
                await LoadDataOnSearchBoxChangedWellnessProgramDetail(activePageIndexWellnessProgramDetail, pageSizeWellnessProgramDetail);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSaveActivityDetail(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (WellnessProgramDetailDto)e.EditModel;

                if (WellnessProgram.EndDate != null && editModel.EndDate > WellnessProgram.EndDate)
                {
                    ToastService.ShowInfo("The selected end date exceeds the allowed program end date.");
                    e.Cancel = true;
                    return;
                }

                if (editModel.EndDate is null)
                    editModel.EndDate = WellnessProgram.EndDate;

                editModel.WellnessProgramId = WellnessProgram.Id;

                // Convert the ProgramName to a slug (lowercase and spaces replaced with hyphens)
                editModel.Slug = editModel.Name?
                    .ToLower()
                    .Replace(" ", "-")
                    .Replace(",", "")
                    .Replace(".", "");

                var cek = await Mediator.Send(new ValidateWellnessProgramDetail(x => x.Slug == editModel.Slug && x.Id != editModel.Id));
                if (cek)
                {
                    ToastService.ShowWarning("Activity Name is already exist!");
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateWellnessProgramDetailRequest(editModel));
                else
                    await Mediator.Send(new UpdateWellnessProgramDetailRequest(editModel));

                SelectedDataItemWellnessProgramDetails = [];

                await LoadDataOnSearchBoxChangedWellnessProgramDetail(activePageIndexWellnessProgramDetail, pageSizeWellnessProgramDetail);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private int FocusedRowActivityDetailVisibleIndex { get; set; }

        private void GridFocusedRowActivityDetailVisibleIndex_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowActivityDetailVisibleIndex = args.VisibleIndex;
        }

        private async Task NewActivityDetail_Click()
        {
            await GridActivityDetail.StartEditNewRowAsync();
        }

        private async Task EditActivityDetail_Click()
        {
            await GridActivityDetail.StartEditRowAsync(FocusedRowActivityDetailVisibleIndex);
        }

        private void DeleteActivityDetail_Click()
        {
            GridActivityDetail.ShowRowDeleteConfirmation(FocusedRowActivityDetailVisibleIndex);
        }

        private async Task RefreshActivityDetails_Click()
        {
            await LoadDataOnSearchBoxChangedWellnessProgramDetail();
        }

        private int pageSizeWellnessProgramDetail { get; set; } = 10;
        private int totalCountWellnessProgramDetail = 0;
        private int activePageIndexWellnessProgramDetail { get; set; } = 0;
        private string searchTermWellnessProgramDetail { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedWellnessProgramDetail(string searchText)
        {
            searchTermWellnessProgramDetail = searchText;
            await LoadDataOnSearchBoxChangedWellnessProgramDetail(0, pageSizeWellnessProgramDetail);
        }

        private async Task OnpageSizeWellnessProgramDetailIndexChanged(int newpageSizeWellnessProgramDetail)
        {
            pageSizeWellnessProgramDetail = newpageSizeWellnessProgramDetail;
            await LoadDataOnSearchBoxChangedWellnessProgramDetail(0, newpageSizeWellnessProgramDetail);
        }

        private async Task OnPageIndexChangedOnSearchBoxChangedWellnessProgramDetail(int newPageIndex)
        {
            await LoadDataOnSearchBoxChangedWellnessProgramDetail(newPageIndex, pageSizeWellnessProgramDetail);
        }

        private async Task LoadDataOnSearchBoxChangedWellnessProgramDetail(int pageIndex = 0, int pageSizeWellnessProgramDetail = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemWellnessProgramDetails = [];
                var result = await Mediator.Send(new GetWellnessProgramDetailQuery
                {
                    OrderByList =
                    [
                        (x => x.StartDate, true)
                    ],
                    Predicate = x => x.WellnessProgramId == WellnessProgram.Id,
                    PageIndex = pageIndex,
                    PageSize = pageSizeWellnessProgramDetail,
                    SearchTerm = searchTermWellnessProgramDetail,
                });
                WellnessProgramDetails = result.Item1;
                totalCountWellnessProgramDetail = result.PageCount;
                activePageIndexWellnessProgramDetail = pageIndex;
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

        private async Task LoadDataOnSearchBoxChangedWellnessProgramAttendance(int pageIndex = 0, int pageSizeWellnessProgramAttendance = 10)
        {
            try
            {
                PanelVisible = true;
                //SelectedDataItems = new ObservableRangeCollection<object>();
                //var result = await Mediator.Send(new GetWellnessProgramDetailQuery
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

        #region Participants

        #region Searching

        private int pageSizeParticipants { get; set; } = 10;
        private int totalCountParticipants = 0;
        private int activePageIndexParticipants { get; set; } = 0;
        private string searchTermParticipants { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedParticipants(string searchText)
        {
            searchTermParticipants = searchText;
            await LoadDataParticipants(0, pageSizeParticipants);
        }

        private async Task OnpageSizeParticipantsIndexChanged(int newpageSizeParticipants)
        {
            pageSizeParticipants = newpageSizeParticipants;
            await LoadDataParticipants(0, newpageSizeParticipants);
        }

        private async Task OnPageIndexChangedParticipants(int newPageIndex)
        {
            await LoadDataParticipants(newPageIndex, pageSizeParticipants);
        }

        private IGrid GridParticipant { get; set; }
        private IReadOnlyList<object> SelectedDataItemParticipants { get; set; } = [];
        private List<WellnessProgramParticipantDto> WellnessProgramParticipants { get; set; } = [];

        private async Task LoadDataParticipants(int pageIndex = 0, int pageSizeParticipants = 10)
        {
            try
            {
                PanelVisible = true;
                var a = await Mediator.Send(new GetWellnessProgramParticipantQuery
                {
                    OrderByList =
                    [
                        (x => x.Date, true)
                    ],
                    Predicate = x => x.WellnessProgramId == WellnessProgram.Id,
                    SearchTerm = searchTermParticipants,
                    PageIndex = pageIndex,
                    PageSize = pageSizeParticipants,
                });
                WellnessProgramParticipants = a.Item1;
                totalCountParticipants = a.PageCount;
                activePageIndexParticipants = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        #endregion Participants

        #region Attendances

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

        private async Task LoadDataAttendances(int pageIndex = 0, int pageSizeAttendances = 10)
        {
            try
            {
                PanelVisible = true;
                var a = await Mediator.Send(new GetWellnessProgramAttendanceQuery
                {
                    OrderByList =
                    [
                        (x => x.Date, true)
                    ],
                    Predicate = x => x.WellnessProgramId == WellnessProgram.Id,
                    SearchTerm = searchTermAttendances,
                    PageIndex = pageIndex,
                    PageSize = pageSizeAttendances,
                });
                WellnessProgramAttendances = a.Item1;
                totalCountAttendances = a.PageCount;
                activePageIndexAttendances = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        #endregion Attendances

        #region ComboboxAwarenessEduCategory

        private List<AwarenessEduCategoryDto> AwarenessEduCategories { get; set; } = [];
        private DxComboBox<AwarenessEduCategoryDto, long?> refAwarenessEduCategoryComboBox { get; set; }
        private int AwarenessEduCategoryComboBoxIndex { get; set; } = 0;
        private int totalCountAwarenessEduCategory = 0;

        private async Task OnSearchAwarenessEduCategory()
        {
            await LoadDataAwarenessEduCategory();
        }

        private async Task OnSearchAwarenessEduCategoryIndexIncrement()
        {
            if (AwarenessEduCategoryComboBoxIndex < (totalCountAwarenessEduCategory - 1))
            {
                AwarenessEduCategoryComboBoxIndex++;
                await LoadDataAwarenessEduCategory(AwarenessEduCategoryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchAwarenessEduCategoryIndexDecrement()
        {
            if (AwarenessEduCategoryComboBoxIndex > 0)
            {
                AwarenessEduCategoryComboBoxIndex--;
                await LoadDataAwarenessEduCategory(AwarenessEduCategoryComboBoxIndex, 10);
            }
        }

        private async Task OnInputAwarenessEduCategoryChanged(string e)
        {
            AwarenessEduCategoryComboBoxIndex = 0;
            await LoadDataAwarenessEduCategory();
        }

        private async Task LoadDataAwarenessEduCategory(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetAwarenessEduCategoryQuery
                {
                    SearchTerm = refAwarenessEduCategoryComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                AwarenessEduCategories = result.Item1;
                totalCountAwarenessEduCategory = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxAwarenessEduCategory
    }
}
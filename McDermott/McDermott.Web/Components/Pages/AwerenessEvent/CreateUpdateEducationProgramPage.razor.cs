﻿using Blazored.TextEditor;
using McDermott.Application.Dtos.AwarenessEvent;
using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;
using static McDermott.Application.Features.Commands.AwarenessEvent.EducationProgramCommand;
using static McDermott.Application.Features.Commands.AwarenessEvent.ParticipanEduCommand;

namespace McDermott.Web.Components.Pages.AwerenessEvent
{
    public partial class CreateUpdateEducationProgramPage
    {
        #region Relation  Data

        private List<EducationProgramDto> getEducationPrograms { get; set; } = [];
        private List<AwarenessEduCategoryDto> getAwarenessEduCategories { get; set; } = [];
        private EducationProgramDto postEducationPrograms = new();
        private List<ParticipanEduDto> GetParticipanEdus { get; set; } = [];

        #endregion Relation  Data

        #region variable static

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid Grid { get; set; }
        private IGrid GridParticipan { get; set; }
        private bool PanelVisible { get; set; }
        private bool PanelVisibleParticipan { get; set; }
        private bool FormValidationState { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }

        private bool IsReadOnly => !(postEducationPrograms.Status == EnumStatusEducationProgram.Done || postEducationPrograms.Status == EnumStatusEducationProgram.InActive);

        private bool IsReadOnlyEvent => postEducationPrograms.Status != EnumStatusEducationProgram.Draft && postEducationPrograms.Status != EnumStatusEducationProgram.Active;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsEducation { get; set; } = [];

        #endregion variable static

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadData(newPageIndex, pageSize);
        }

        #endregion Searching

        #region Load data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            try
            {
                await GetUserInfo();

                var loadTasks = new[]
                {
                 GetUserInfo(),
                 LoadDataCategory(),
                 LoadData()
            };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;

            var result = await Mediator.Send(new GetSingleEducationProgramQuery
            {
                Predicate = x => x.Id == Id,
            });
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("awereness-event/education-program");
                    return;
                }
                postEducationPrograms = result ?? new();

                var result_category = await Mediator.Send(new GetAwarenessEduCategoryQuery
                {
                    Predicate = x => x.Id == postEducationPrograms.EventCategoryId.GetValueOrDefault()
                });

                getAwarenessEduCategories = result_category.Item1;
                StateHasChanged();

                await LoadData_Participant();
                // log
            }
            PanelVisible = false;
        }

        #endregion Load data

        #region HTML Editor

        private bool IsShowPreviewOutput { get; set; } = false;
        private bool IsShowPreviewOutput2 { get; set; } = false;
        private BlazoredTextEditor QuillHtml;
        private BlazoredTextEditor QuillHtml2;
        private MarkupString preview;
        private MarkupString preview2;

        private async Task ShowAoutPutPreview(bool b)
        {
            IsShowPreviewOutput = b;

            if (!string.IsNullOrWhiteSpace(await QuillHtml.GetText()))
            {
                preview = (MarkupString)await QuillHtml.GetHTML();
            }
        }

        private async Task ShowAoutPutPreview2(bool b)
        {
            IsShowPreviewOutput2 = b;

            if (!string.IsNullOrWhiteSpace(await QuillHtml2.GetText()))
            {
                preview2 = (MarkupString)await QuillHtml2.GetHTML();
            }
        }

        #endregion HTML Editor

        #region select File

        private IBrowserFile BrowserFile;

        private void RemoveSelectedFile()
        {
            postEducationPrograms.Attendance = null;
        }

        private void SelectFiles(InputFileChangeEventArgs e)
        {
            BrowserFile = e.File;
            postEducationPrograms.Attendance = e.File.Name;
        }

        private async Task SelectFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "sipFile");
        }

        private bool isShowPopUp { get; set; } = false;

        private async Task DownloadFile()
        {
            if (postEducationPrograms.Id != 0 && !string.IsNullOrWhiteSpace(postEducationPrograms.Attendance))
            {
                //await Helper.DownloadFile(postEducationPrograms.Attendance, HttpContextAccessor, HttpClient, JsRuntime);
                isShowPopUp = true;
            }
        }

        private async Task YesDownload()
        {
            try
            {
                // Generate the absolute URL for the file
                var url = $"new/files/{postEducationPrograms.Attendance}";
                var absoluteUrl = NavigationManager.ToAbsoluteUri(url).ToString();

                // Invoke JavaScript to open and close the tab
                await JsRuntime.InvokeVoidAsync("openAndCloseTab", absoluteUrl);
                isShowPopUp = false;
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur
                Console.Error.WriteLine($"Error during file download: {ex.Message}");
            }
        }

        #endregion select File

        #region ComboBox Category

        private DxComboBox<AwarenessEduCategoryDto, long?> refCategoryComboBox { get; set; }
        private int CategoryComboBoxIndex { get; set; } = 0;
        private int totalCountCategory = 0;

        private async Task OnSearchCategory()
        {
            await LoadDataCategory();
        }

        private async Task OnSearchCategoryIndexIncrement()
        {
            if (CategoryComboBoxIndex < (totalCountCategory - 1))
            {
                CategoryComboBoxIndex++;
                await LoadDataCategory(CategoryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCategoryIndexDecrement()
        {
            if (CategoryComboBoxIndex > 0)
            {
                CategoryComboBoxIndex--;
                await LoadDataCategory(CategoryComboBoxIndex, 10);
            }
        }

        private async Task OnInputCategoryChanged(string e)
        {
            CategoryComboBoxIndex = 0;
            await LoadDataCategory(0, 10);
        }

        private async Task LoadDataCategory(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetAwarenessEduCategoryQuery
            {
                SearchTerm = refCategoryComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            getAwarenessEduCategories = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Category

        #region Grid

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Grid

        #region HandleSubmit

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await OnSave();
            else
                FormValidationState = true;
        }

        private async Task HandleInvalidSubmit()
        {
            FormValidationState = false;
        }

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        #endregion HandleSubmit

        #region Save

        private async Task OnSave()
        {
            try
            {
                var data = new EducationProgramDto();

                if (postEducationPrograms.Id == 0)
                {
                    if (!string.IsNullOrWhiteSpace(await QuillHtml.GetContent()))
                    {
                        postEducationPrograms.HTMLContent = await QuillHtml.GetHTML();
                    }

                    if (postEducationPrograms.EndDate is not null)
                        postEducationPrograms.EndDate = postEducationPrograms.EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    postEducationPrograms.StartDate = postEducationPrograms.StartDate.Date.AddHours(0).AddMinutes(0).AddSeconds(0);

                    postEducationPrograms.Status = EnumStatusEducationProgram.Draft;
                    data = await Mediator.Send(new CreateEducationProgramRequest(postEducationPrograms));
                    ToastService.ShowSuccess("Add Data Success...");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(await QuillHtml.GetContent()))
                    {
                        postEducationPrograms.HTMLContent = await QuillHtml.GetHTML();
                    }
                    if (!string.IsNullOrWhiteSpace(await QuillHtml2.GetContent()))
                    {
                        postEducationPrograms.HTMLMaterial = await QuillHtml2.GetHTML();
                    }

                    var cekdata = await Mediator.Send(new GetSingleEducationProgramQuery
                    {
                        Predicate = x => x.Id == Id,
                    });

                    if (postEducationPrograms.Attendance != cekdata.Attendance)
                    {
                        if (postEducationPrograms.Attendance != null)
                            Helper.DeleteFile(postEducationPrograms.Attendance);

                        if (cekdata.Attendance != null)
                            Helper.DeleteFile(cekdata.Attendance);
                    }

                    if (postEducationPrograms.Attendance != cekdata.Attendance)
                    {
                        if (postEducationPrograms.Attendance != null)
                            _ = await FileUploadService.UploadFileAsync(BrowserFile);
                    }

                    if (postEducationPrograms.EndDate is not null)
                        postEducationPrograms.EndDate = postEducationPrograms.EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                    postEducationPrograms.StartDate = postEducationPrograms.StartDate.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                    await FileUploadService.UploadFileAsync(BrowserFile);

                    data = await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
                    ToastService.ShowSuccess("Update Data Success...");
                }

                ToastService.ClearSuccessToasts();
                NavigationManager.NavigateTo($"awereness-event/education-program/{EnumPageMode.Update.GetDisplayName()}?Id={data.Id}", true);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SaveEvent()
        {
            if (!string.IsNullOrWhiteSpace(await QuillHtml2.GetContent()))
            {
                postEducationPrograms.HTMLMaterial = await QuillHtml2.GetHTML();
            }
            await FileUploadService.UploadFileAsync(BrowserFile);

            var cekdata = await Mediator.Send(new GetSingleEducationProgramQuery
            {
                Predicate = x => x.Id == Id,
            });

            if (postEducationPrograms.Attendance != cekdata.Attendance)
            {
                if (postEducationPrograms.Attendance != null)
                    Helper.DeleteFile(postEducationPrograms.Attendance);

                if (cekdata.Attendance != null)
                    Helper.DeleteFile(cekdata.Attendance);
            }

            if (postEducationPrograms.Attendance != cekdata.Attendance)
            {
                if (postEducationPrograms.Attendance != null)
                    _ = await FileUploadService.UploadFileAsync(BrowserFile);
            }

            await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
            ToastService.ShowSuccess("Save Material Data Success...");
        }

        #endregion Save

        #region Click

        private void onDiscard()
        {
            NavigationManager.NavigateTo($"awereness-event/education-program/{EnumPageMode.Create.GetDisplayName()}");
        }

        private async Task onActive()
        {
            try
            {
                string? textSlug = string.Empty;
                if (postEducationPrograms.Status == EnumStatusEducationProgram.Draft)
                {
                    // Convert the ProgramName to a slug (lowercase and spaces replaced with hyphens)
                    textSlug = postEducationPrograms.EventName?
                        .ToLower()
                        .Replace(" ", "-")
                        .Replace(",", "")
                        .Replace(".", "");
                }
                postEducationPrograms.Slug = textSlug;
                postEducationPrograms.Status = EnumStatusEducationProgram.Active;

                var cek = await Mediator.Send(new ValidateEducationProgramQuery(x => x.Slug == postEducationPrograms.Slug && x.Id != postEducationPrograms.Id));
                if (cek)
                {
                    ToastService.ShowWarning("Program Name is already exist!");
                    return;
                }
                await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
                var currentUri = NavigationManager.Uri;
                if (Id.HasValue)
                {
                    NavigationManager.NavigateTo($"{currentUri}", forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to activate program: {ex.Message}");
            }
        }

        private async Task onInActive()
        {
            try
            {
                postEducationPrograms.Status = EnumStatusEducationProgram.InActive;
                await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
                var currentUri = NavigationManager.Uri;
                if (Id.HasValue)
                {
                    NavigationManager.NavigateTo($"{currentUri}", forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to activate program: {ex.Message}");
            }
        }

        private async Task onCancel()
        {
            try
            {
                postEducationPrograms.Status = EnumStatusEducationProgram.Cancel;
                await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
                var currentUri = NavigationManager.Uri;
                if (Id.HasValue)
                {
                    NavigationManager.NavigateTo($"{currentUri}", forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to activate program: {ex.Message}");
            }
        }

        private async Task onDone()
        {
            try
            {
                postEducationPrograms.Status = EnumStatusEducationProgram.Done;
                await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
                var currentUri = NavigationManager.Uri;
                if (Id.HasValue)
                {
                    NavigationManager.NavigateTo($"{currentUri}", forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to activate program: {ex.Message}");
            }
        }

        private async Task ClickOpenTo()
        {
            if (postEducationPrograms.Slug != null)
            {
                var url = $"awereness-event/education-program/join-participant/{postEducationPrograms.Slug}";
                await JsRuntime.InvokeVoidAsync("openInNewTab", NavigationManager.ToAbsoluteUri(url).ToString());
            }
        }

        private async Task ClickCopy()
        {
            var url = $"awereness-event/education-program/join-participant/{postEducationPrograms.Slug}";
            JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", NavigationManager.ToAbsoluteUri(url).ToString());

            ToastService.ClearAll();
            ToastService.ShowSuccess("Copy link Success");
        }

        private async Task SendToDraft()
        {
            try
            {
                postEducationPrograms.Status = EnumStatusEducationProgram.Draft;
                await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
                var currentUri = NavigationManager.Uri;
                if (Id.HasValue)
                {
                    NavigationManager.NavigateTo($"{currentUri}", forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to activate program: {ex.Message}");
            }
        }

        private async Task LoadData_Participant()
        {
            var resultParticipan = await Mediator.Send(new GetParticipanEduQuery
            {
                Predicate = x => x.EducationProgramId == postEducationPrograms.Id,
            });

            GetParticipanEdus = resultParticipan.Item1;
        }

        private async Task RefreshParticipan_Click()
        {
            await LoadData_Participant();
        }

        #endregion Click
    }
}
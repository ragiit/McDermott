using DevExpress.SpreadsheetSource.Xlsx.Import;
using DocumentFormat.OpenXml.Office2010.Excel;
using MailKit.Search;
using McDermott.Application.Dtos.AwarenessEvent;
using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;
using static McDermott.Application.Features.Commands.AwarenessEvent.EducationProgramCommand;

namespace McDermott.Web.Components.Pages.AwerenessEvent
{
    public partial class CreateUpdateEducationProgramPage
    {
        #region Relation  Data
        private List<EducationProgramDto> getEducationPrograms = [];
        private List<AwarenessEduCategoryDto> getAwarenessEduCategories = [];
        private EducationProgramDto postEducationPrograms = new();
        #endregion

        #region variable static

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();
        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; }
        private bool FormValidationState { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        #endregion

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
                Predicate = x => x.Id == Id
            });
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("awereness-event/education-program");
                    return;
                }
                postEducationPrograms = result ?? new();
                // log

            }
            PanelVisible = false;
        }

        #endregion

        #region ComboBox Category

        private DxComboBox<AwarenessEduCategoryDto, long?> refCategoryComboBox { get; set; }
        private int CategoryComboBoxIndex { get; set; } = 0;
        private int totalCountCategory = 0;

        private async Task OnSearchCategory()
        {
            await LoadDataCategory(0, 10);
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
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetAwarenessEduCategoryQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refCategoryComboBox?.Text ?? "",
                Select = x => new AwarenessEduCategory
                {
                    Id = x.Id,
                    Name = x.Name,
                }

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
        #endregion

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
        #endregion

        #region Save
        private async Task OnSave()
        {
            try
            {
                var data = new EducationProgramDto();

                if (postEducationPrograms.Id == 0)
                {
                    postEducationPrograms.Status = EnumStatusEducationProgram.Draft;
                    data = await Mediator.Send(new CreateEducationProgramRequest(postEducationPrograms));
                    ToastService.ShowSuccess("Add Data Success...");
                }
                else
                {
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



        #endregion

        #region Click
        private void onDiscard()
        {
            NavigationManager.NavigateTo($"awerenessevent/education-program/{EnumPageMode.Create.GetDisplayName()}");
        }

        private async Task onActive()
        {
            try
            {
                postEducationPrograms.Status = EnumStatusEducationProgram.Active;
                await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
                var currentUri = NavigationManager.Uri;
                if (Id.HasValue)
                {
                    NavigationManager.NavigateTo($"{currentUri}?Id={Id.Value}", forceLoad: true);
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
                    NavigationManager.NavigateTo($"{currentUri}?Id={Id.Value}", forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to activate program: {ex.Message}");
            }
        }
        private async Task onCancel ()
        {
            try
            {
                postEducationPrograms.Status = EnumStatusEducationProgram.Cancel;
                await Mediator.Send(new UpdateEducationProgramRequest(postEducationPrograms));
                var currentUri = NavigationManager.Uri;
                if (Id.HasValue)
                {
                    NavigationManager.NavigateTo($"{currentUri}?Id={Id.Value}", forceLoad: true);
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
                    NavigationManager.NavigateTo($"{currentUri}?Id={Id.Value}", forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to activate program: {ex.Message}");
            }
        }
        #endregion

    }
}

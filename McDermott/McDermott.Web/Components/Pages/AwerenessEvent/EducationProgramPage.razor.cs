using DocumentFormat.OpenXml.Office2010.Excel;
using MailKit.Search;
using McDermott.Application.Dtos.AwarenessEvent;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;
using static McDermott.Application.Features.Commands.AwarenessEvent.EducationProgramCommand;

namespace McDermott.Web.Components.Pages.AwerenessEvent
{
    public partial class EducationProgramPage
    {
        #region Data Relation
        private List<EducationProgramDto> getEducationPrograms = [];
        private EducationProgramDto postEducationPrograms = new();
        #endregion

        #region variabel static
        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();
        private IGrid Grid {  get; set; }
        private bool PanelVisible { get; set; }
        private bool FormValidationState { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #region Enum Status
        public MarkupString GetIssueStatusIconHtml(EnumStatusEducationProgram? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusEducationProgram.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumStatusEducationProgram.Active:
                    priorityClass = "success";
                    title = "Active";
                    break;

                case EnumStatusEducationProgram.InActive:
                    priorityClass = "danger";
                    title = "InActive";
                    break;
                case EnumStatusEducationProgram.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

               

                default:
                    return new MarkupString("");
            }
            string html = $"<div class='row '><div class='col-3'>" +
                         $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }
        #endregion

        #endregion

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

        #region Load async Data
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetEducationProgramQuery
            {
                OrderByList =
                    [
                        (x => x.CreatedDate, true),  // ThenByDescending Created
                        (x => x.Status, true),               // OrderByDescending Status
                    ],
                SearchTerm = searchTerm,
                PageSize = pageSize,
                PageIndex = pageIndex,
            });
            getEducationPrograms = result.Item1;
            totalCount = result.PageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }
        #endregion

        #region Grid Events

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            NavigationManager.NavigateTo($"awereness-event/education-program/{EnumPageMode.Create.GetDisplayName()}");
        }

        private async Task EditItem_Click()
        {
            var data = SelectedDataItems[0].Adapt<EducationProgramDto>();
            NavigationManager.NavigateTo($"awereness-event/education-program/{EnumPageMode.Update.GetDisplayName()}/?Id={data.Id}");
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteEducationProgramRequest(((EducationProgramDto)e.DataItem).Id));
                }
                else
                {
                    var countriesToDelete = SelectedDataItems.Adapt<List<EducationProgramDto>>();
                    await Mediator.Send(new DeleteEducationProgramRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                }

                SelectedDataItems = [];
                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task ClickCopy()
        {
            var url = $"awereness-event/education-program/join-participant/{postEducationPrograms.Slug}";
            JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", NavigationManager.ToAbsoluteUri(url).ToString());

            ToastService.ClearAll();
            ToastService.ShowSuccess("Copy link Success");
        }
        private async Task ClickOpenTo()
        {
            if (postEducationPrograms.Slug != null)
            {
                if (Id.HasValue)
                {
                    var url = $"awereness-event/education-program/join-participant/{postEducationPrograms.Slug}";
                    await JsRuntime.InvokeVoidAsync("openInNewTab", NavigationManager.ToAbsoluteUri(url).ToString());
                }
            }
        }

        #endregion Grid Events


    }
}

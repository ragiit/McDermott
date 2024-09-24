using DocumentFormat.OpenXml.Office2010.Excel;
using System.Drawing;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;

namespace McDermott.Web.Components.Pages.Inventory.Maintainance
{
    public partial class MaintainanceHistoryPage
    {
        #region Relation Data
        private List<ProductDto> GetProducts = [];
        private List<MaintainanceDto> GetMaintainances = [];
        private ProductDto PostProducts = new();
        #endregion

        #region Variabel Static
        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid Grid;
        private bool PanelVisible { get; set; } = false;
        private string NameProduct { get; set; } = string.Empty;
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
                    StateHasChanged();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                    }
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

        #region Load Data
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            PanelVisible = false;
        }
        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetProductQuery(x => x.Id == Id, pageSize: pageSize, pageIndex: pageIndex));
                GetProducts = result.Item1;
                PostProducts = GetProducts.FirstOrDefault() ?? new();

                if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    if (result.Item1.Count == 0 || !Id.HasValue)
                    {
                        NavigationManager.NavigateTo("inventory/products");
                        return;
                    }

                    var resultMaintainance = await Mediator.Send(new GetMaintainanceQuery(x => x.EquipmentId == PostProducts.Id, pageSize: pageSize, pageIndex: pageIndex));
                    GetMaintainances = resultMaintainance.Item1.Where(x => x.Status != EnumStatusMaintainance.Scrap).ToList();
                    NameProduct = PostProducts.Name ?? "";
                }
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        #endregion

        #region Fungsional Button
        public MarkupString GetIssueStatusIconHtmlMaintainance(EnumStatusMaintainance? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusMaintainance.Request:
                    priorityClass = "info";
                    title = "Request";
                    break;

                case EnumStatusMaintainance.InProgress:
                    priorityClass = "primary";
                    title = "In Progress";
                    break;

                case EnumStatusMaintainance.Repaired:
                    priorityClass = "warning";
                    title = "Repaire";
                    break;

                case EnumStatusMaintainance.Scrap:
                    priorityClass = "warning";
                    title = "Scrap";
                    break;

                case EnumStatusMaintainance.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusMaintainance.Canceled:
                    priorityClass = "danger";
                    title = "Cancel";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        private async Task Back_Click()
        {
            NavigationManager.NavigateTo("inventory/products");
            return;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }
        #endregion

    }
}

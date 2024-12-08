using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class StockMovePage
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

        public IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private List<TransactionStockDto> TransactionStocks { get; set; } = [];

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    await GetUserInfo();
                    PanelVisible = true;
                    StateHasChanged();

                    await LoadData();
                    StateHasChanged();

                    Grid?.CollapseAllGroupRows();
                }
                catch { }
            }
        }

        private void Grid_CustomSummary(GridCustomGroupEventArgs e)
        {
            //if (e.s)
            //{
            //}
            var a = "adwad";
            var ad = "adwad";
            var add = "adwad";
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            TransactionStocks = [.. (await Mediator.Send(new GetTransactionStockQuery())).OrderByDescending(x => x.CreatedDate)];
            StateHasChanged();
            Grid?.CollapseAllGroupRows();
            StateHasChanged();
            PanelVisible = false;
        }
    }
}
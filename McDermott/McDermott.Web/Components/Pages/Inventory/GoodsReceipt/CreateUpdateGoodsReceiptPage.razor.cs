using DevExpress.XtraPrinting.Native.Properties;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GreenDonut;
using MailKit.Search;
using static McDermott.Application.Features.Commands.Inventory.GoodsReceiptCommand;

namespace McDermott.Web.Components.Pages.Inventory.GoodsReceipt
{
    public partial class CreateUpdateGoodsReceiptPage
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
            //    try
            //    {
            //        await GetUserInfo();
            //        StateHasChanged();
            //    }
            //    catch { }

            //    await LoadData();
            //    StateHasChanged();

            //    try
            //    {
            //        if (Grid is not null)
            //        {
            //            await Grid.WaitForDataLoadAsync();
            //            Grid.ExpandGroupRow(1);
            //            await Grid.WaitForDataLoadAsync();
            //            Grid.ExpandGroupRow(2);
            //            StateHasChanged();
            //        }
            //    }
            //    catch { }

            //    var user_group = await Mediator.Send(new GetUserQuery());
            //    NameUser = user_group.FirstOrDefault(x => x.GroupId == UserAccessCRUID.GroupId && x.Id == UserLogin.Id) ?? new();
            //    StateHasChanged();

            //    await LoadAsyncData();
            //    StateHasChanged();
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

        #region Relation Data
        private List<GoodsReceiptDetailDto> getGoodsReceiptDetails = [];
        private List<GoodsReceiptLogDto> getGoodsReceiptLogs = [];
        private List<ProductDto> getProduct = [];
        private List<LocationDto> getResource = [];
        private List<LocationDto> getDestination = [];
        private List<TransactionStockDto> getTransactionStocks = [];

        private GoodsReceiptDto postGoodsReceipt = new();
        private GoodsReceiptDetailDto postGoodsReceiptDetail = new();
        #endregion

        #region variabel Data

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        #endregion

        #region Load data
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            PanelVisible = false;


        }

        private async Task LoadData()
        {
            var result = await Mediator.Send(new GetSingleGoodsReceiptQuery()
            {
                Predicate : x=>x.Id == Id,
            });
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("inventory/goods-receipts");
                    return;
                }
            }
        }
        #endregion

    }
}

using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Web.Components.Pages.Inventory.Products
{
    public partial class StockProductPage
    {
        #region relation data
        private List<ProductDto> GetProducts = [];
        private List<TransactionStockDto> GetTransactionStocks = [];
        private List<StockProductDto> GetStockProducts = [];
        private ProductDto PostProducts = new();
        #endregion

        #region variable static
        [SupplyParameterFromQuery]
        private long? Id { get; set; }
        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();
        private IGrid Grid;
        private bool PanelVisible { get; set; } = false;
        private string? NameProduct { get; set; }
        private bool? FieldHideStock { get; set; } = false;
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
                        StateHasChanged();
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
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                var result = await Mediator.Send(new GetProductQuery(x => x.Id == Id, pageIndex : pageIndex, pageSize : pageSize));
                GetProducts = result.Item1;
                totalCount = result.pageCount;
                GetTransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                PostProducts = GetProducts.FirstOrDefault() ?? new();
                if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    if (result.Item1.Count == 0 || !Id.HasValue)
                    {
                        NavigationManager.NavigateTo("inventory/products");
                        return;
                    }
                    if (PostProducts.TraceAbility == true)
                    {
                        GetStockProducts = GetTransactionStocks.Where(x => x.ProductId == PostProducts.Id && x.Validate == true)
                            .GroupBy(z => new { z.ProductId, z.Batch, z.LocationId, z.UomId })
                            .Select(y => new StockProductDto
                            {
                                ProductId = y.Key.ProductId,
                                Batch = y.Key.Batch ?? "-",
                                DestinanceId = y.Key.LocationId,
                                DestinanceName = y.First()?.Location?.Name ?? "-",
                                UomId = y.Key.UomId,
                                UomName = y.First()?.Uom?.Name ?? "-",
                                Expired = y.First().ExpiredDate,
                                ProductName = y.First()?.Product?.Name ?? "-",
                                Qty = y.Sum(item => item.Quantity)
                            }).ToList();
                        FieldHideStock = false;
                    }
                    else
                    {
                        GetStockProducts = GetTransactionStocks.Where(x => x.ProductId == PostProducts.Id && x.Validate == true)
                        .GroupBy(z => new { z.ProductId, z.LocationId, z.UomId })
                        .Select(y => new StockProductDto
                        {
                            ProductId = y.Key.ProductId,
                            DestinanceId = y.Key.LocationId,
                            DestinanceName = y.First()?.Location?.Name ?? "-",
                            UomId = y.Key.UomId,
                            UomName = y.First()?.Uom?.Name ?? "-",
                            ProductName = y.First()?.Product?.Name ?? "-",
                            Qty = y.Sum(item => item.Quantity)
                        }).ToList();
                        if (PostProducts.TraceAbility == true)
                        {
                            FieldHideStock = true;
                        }
                        else
                        {
                            FieldHideStock = false;
                        }
                    }
                    NameProduct = PostProducts.Name;
                }
            }
            catch (Exception ex) 
            {
                ex.HandleException(ToastService);
            }

        }
        #endregion

        private void Back_Click()
        {
            NavigationManager.NavigateTo("inventory/products");
            return;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }
    }
}

using static McDermott.Application.Features.Commands.GetDataCommand;
using static McDermott.Domain.Entities.GoodsReceipt;
using DevExpress.Data.Linq;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Inventory.GoodsReceiptCommand;

namespace McDermott.Web.Components.Pages.Inventory.GoodsReceipts
{
    public partial class GoodsReceiptPage
    {
        #region Relation Data

        private List<GoodsReceiptDto> getGoodsReceipts = [];
        private List<TransactionStockDto> TransactionStocks = [];

        private GoodsReceiptDto GetGoodsReceipt = new();
        private StockProductDto FormStockProduct = new();
        private GoodsReceiptDto postGoodsReceipts = new();
        private TransactionStockDto FormTransactionStock = new();
        private object Data { get; set; }

        #endregion Relation Data

        #region Variable Static

        private IGrid? Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool IsAddReceived { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private string? header { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Variable Static

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

        #region async Data

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
            Data = new GridDevExtremeDataSource<GoodsReceipt>(await Mediator.Send(new GetQueryGoodReceipt()))
            {
                CustomizeLoadOptions = (loadOptions) =>
                {
                    loadOptions.PrimaryKey = ["Id"];
                    loadOptions.PaginateViaPrimaryKey = true;
                }
            };
            PanelVisible = false;
        }

        public MarkupString GetIssueStatusIconHtml(EnumStatusGoodsReceipt? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusGoodsReceipt.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumStatusGoodsReceipt.Process:
                    priorityClass = "warning";
                    title = "Process";
                    break;

                case EnumStatusGoodsReceipt.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusGoodsReceipt.Cancel:
                    priorityClass = "danger";
                    title = "Cancel";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #endregion async Data

        #region Configuration Grid

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;

            try
            {
                if ((GoodsReceipt)args.DataItem is null)
                    return;

                isActiveButton = ((GoodsReceipt)args.DataItem)!.Status!.Equals(EnumStatusGoodsReceipt.Draft);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            var Id = e.VisibleIndex;
            NavigationManager.NavigateTo($"inventory/goods-receipts/{EnumPageMode.Update.GetDisplayName()}?Id={Id}");
        }

        #endregion Configuration Grid

        #region Click Button

        private async Task NewItem_Click()
        {
            NavigationManager.NavigateTo($"inventory/goods-receipts/{EnumPageMode.Create.GetDisplayName()}");
        }

        private async Task EditItem_Click()
        {
            var data = SelectedDataItems[0].Adapt<GoodsReceiptDto>();
            NavigationManager.NavigateTo($"inventory/goods-receipts/{EnumPageMode.Update.GetDisplayName()}?Id={data.Id}");
        }

        private async Task DeleteItem_Click()
        {
            Grid!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        #endregion Click Button

        #region Function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var getGoodsReceiptDetail = await Mediator.Send(new GetAllGoodsReceiptDetailQuery());
                var getGoodsReceiptLog = await Mediator.Send(new GetAllGoodsReceiptLogQuery());
                List<GoodsReceiptDto> receivings = SelectedDataItems.Adapt<List<GoodsReceiptDto>>();
                long id = SelectedDataItems[0].Adapt<GoodsReceiptDto>().Id;
                List<long> ids = receivings.Select(x => x.Id).ToList();
                List<long> ProductIdsToDelete = new();
                List<long> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //Delete Data Receiving stock Product

                    ProductIdsToDelete = getGoodsReceiptDetail
                        .Where(x => x.GoodsReceiptId == id)
                        .Select(x => x.Id)
                        .ToList();
                    await Mediator.Send(new DeleteGoodsReceiptDetailRequest(ids: ProductIdsToDelete));

                    //Delete Data Transfer Detail (log)

                    DetailsIdsToDelete = getGoodsReceiptLog
                        .Where(x => x.GoodsReceiptId == id)
                        .Select(x => x.Id)
                        .ToList();
                    await Mediator.Send(new DeleteGoodsReceiptLogRequest(ids: DetailsIdsToDelete));

                    //Delete Receiving Stock

                    await Mediator.Send(new DeleteGoodsReceiptRequest(SelectedDataItems[0].Adapt<GoodsReceiptDto>().Id));
                }
                else
                {
                    foreach (var Uid in ids)
                    {
                        //Delete Data Receiving Stock Product
                        ProductIdsToDelete = getGoodsReceiptDetail
                       .Where(x => x.GoodsReceiptId == Uid)
                       .Select(x => x.Id)
                       .ToList();
                        await Mediator.Send(new DeleteGoodsReceiptDetailRequest(ids: ProductIdsToDelete));

                        //Delete Data Transfer Detail (log)

                        DetailsIdsToDelete = getGoodsReceiptLog
                            .Where(x => x.GoodsReceiptId == Uid)
                            .Select(x => x.Id)
                            .ToList();
                        await Mediator.Send(new DeleteGoodsReceiptLogRequest(ids: DetailsIdsToDelete));
                    }
                    //Delete list Id GoodsReceipt
                    await Mediator.Send(new DeleteGoodsReceiptRequest(ids: ids));
                }
                ToastService.ShowSuccess("Data Deleting Success!..");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Function Delete
    }
}
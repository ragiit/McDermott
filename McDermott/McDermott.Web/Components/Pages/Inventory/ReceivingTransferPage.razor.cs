using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class ReceivingTransferPage
    {
        #region Relation Data

        private List<ReceivingStockDto> ReceivingStocks = [];
        private List<ReceivingStockProductDto> receivingStockDetails = [];
        private List<ReceivingStockProductDto> TempReceivingStockDetails = [];
        private List<TransactionStockDetailDto> TransactionStockDetails = [];
        private List<LocationDto> Locations = [];
        private List<ProductDto> Products = [];
        private List<StockProductDto> Stocks = [];
        private List<UomDto> Uoms = [];
        private ReceivingStockProductDto FormReceivingDetailStock = new();
        private ReceivingStockProductDto TempFormReceivingStockDetail = new();
        private ReceivingStockDto GetReceivingStock = new();
        private StockProductDto FormStockProduct = new();
        private ReceivingStockDto FormReceivingStocks = new();
        private TransactionStockDetailDto FormTransactionDetail = new();

        #endregion Relation Data

        #region Variable Static

        private IGrid? Grid { get; set; }
        private IGrid? GridProduct { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool IsAddReceived { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private string? header { get; set; }
        private long? receivingId { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsDetail { get; set; } = new ObservableRangeCollection<object>();

        #endregion Variable Static

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

        #region async Data

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
            }
            catch { }

            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            showForm = false;
            ReceivingStocks = await Mediator.Send(new GetReceivingStockQuery());
            receivingStockDetails = await Mediator.Send(new GetReceivingStockProductQuery());
            Locations = await Mediator.Send(new GetLocationQuery());
            Products = await Mediator.Send(new GetProductQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            Stocks = await Mediator.Send(new GetStockProductQuery());
            PanelVisible = false;
        }

        private async Task LoadData_Detail()
        {
            PanelVisible = true;
            SelectedDataItemsDetail = [];
            receivingStockDetails = await Mediator.Send(new GetReceivingStockProductQuery());
            PanelVisible = false;
        }

        private void SelectedChangeProduct(ProductDto product)
        {
            if (product is not null)
            {
                var productName = Products.Where(p => p.Id == product.Id).FirstOrDefault();
                var uomName = Uoms.Where(u => u.Id == product.UomId).Select(x => x.Name).FirstOrDefault();
                var purchaseName = Uoms.Where(u => u.Id == product.PurchaseUomId).Select(x => x.Name).FirstOrDefault();
                TempFormReceivingStockDetail.PurchaseName = purchaseName;
                TempFormReceivingStockDetail.UomName = uomName;
                TempFormReceivingStockDetail.ProductName = productName.Name;
                TempFormReceivingStockDetail.TraceAbility = productName.TraceAbility;
            }
        }

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;
            if (FormValidationState)
            {
                await OnSave();
            }
            else
            {
                FormValidationState = true;
            }
        }

        private async Task HandleInvalidSubmit()
        {
            showForm = true;
            FormValidationState = false;
        }

        public MarkupString GetIssueStatusIconHtml(string status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case "Draft":
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case "Done":
                    priorityClass = "success";
                    title = "Done";
                    break;

                case "Cancel":
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
                if ((ReceivingStockDto)args.DataItem is null)
                    return;

                isActiveButton = ((ReceivingStockDto)args.DataItem)!.StatusReceived!.Equals("Draft");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void Grid_FocusedRowChangedDetail(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            await EditItem_Click(null);
        }

        private async Task OnRowDoubleClickDetail(GridRowClickEventArgs e)
        {
            //await EditItemDetail_Click();
        }

        #endregion Configuration Grid

        #region Click Button

        // Grid
        private async Task NewItem_Click()
        {
            showForm = true;
            header = "Add Data";
            TempReceivingStockDetails.Clear();
            FormReceivingStocks = new();
            isActiveButton = true;
        }

        private async Task EditItem_Click(ReceivingStockDto? p = null)
        {
            showForm = true;
            PanelVisible = true;
            header = "Edit Data";
            FormReceivingStocks = p ?? SelectedDataItems[0].Adapt<ReceivingStockDto>();

            if (FormReceivingStocks.StatusReceived != "Draft")
            {
                isActiveButton = false;
            }

            receivingId = FormReceivingStocks.Id;

            receivingStockDetails = await Mediator.Send(new GetReceivingStockProductQuery(x => x.ReceivingStockId == FormReceivingStocks.Id));
            TempReceivingStockDetails = receivingStockDetails.Select(x => x).ToList();
            foreach (var item in TempReceivingStockDetails)
            {
                var d = Products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                item.UomName = Uoms.Where(u => u.Id == d.UomId).Select(x => x.Name).FirstOrDefault();
                item.PurchaseName = Uoms.Where(p => p.Id == d.PurchaseUomId).Select(x => x.Name).FirstOrDefault();
            }
        }

        private async Task DeleteItem_Click()
        {
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        //Grid Detail
        private async Task NewItemDetail_Click()
        {
            IsAddReceived = true;
            TempFormReceivingStockDetail = new();
            await GridProduct!.StartEditNewRowAsync();
        }

        private async Task EditItemDetail_Click()
        {
            await GridProduct!.StartEditRowAsync(FocusedRowVisibleIndex);
            IsAddReceived = false;
        }

        private async Task DeleteItemDetail_Click()
        {
            GridProduct!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task onDiscard()
        {
            await LoadData();
        }

        #endregion Click Button

        #region Validation

        private async Task onValidation()
        {
            var Stock = await Mediator.Send(new GetStockProductQuery());
            var receivedStock = await Mediator.Send(new GetReceivingStockQuery());
            var receivedProductStock = await Mediator.Send(new GetReceivingStockProductQuery());
            FormReceivingStocks = receivedStock.Where(x => x.Id == receivingId).FirstOrDefault()!;

            if (FormReceivingStocks is not null)
            {
                FormReceivingStocks.StatusReceived = "Done";
                GetReceivingStock = await Mediator.Send(new UpdateReceivingStockRequest(FormReceivingStocks));

                var CheckReceivedProduct = receivedProductStock.Where(x => x.ReceivingStockId == GetReceivingStock.Id).ToList()!;
                foreach (var a in CheckReceivedProduct)
                {
                    var x = Uoms.Where(x => x.Id == a?.Product?.PurchaseUomId).FirstOrDefault();
                    var checkStok = Stocks.Where(x => x.ProductId == a.ProductId && x.SourceId == GetReceivingStock.DestinationId && x.Batch == a.Batch).FirstOrDefault();
                    if (checkStok is null)
                    {
                        FormStockProduct.SourceId = GetReceivingStock.DestinationId;
                        FormStockProduct.ProductId = a.ProductId;
                        FormStockProduct.UomId = a?.Product?.UomId;

                        FormStockProduct.Batch = a?.Batch;
                        FormStockProduct.Expired = a?.ExpiredDate;

                        FormStockProduct.Qty = a.Qty * x.BiggerRatio.ToLong();
                        await Mediator.Send(new CreateStockProductRequest(FormStockProduct));
                    }
                    else
                    {
                        checkStok.Expired = a.ExpiredDate;
                        checkStok.Batch = a.Batch;

                        var totalQty = a.Qty * x?.BiggerRatio?.ToLong();
                        checkStok!.Qty = checkStok.Qty + a.Qty;

                        await Mediator.Send(new UpdateStockProductRequest(checkStok));
                    }
                }

                //Save Log..

                FormTransactionDetail.DestinationId = GetReceivingStock.DestinationId;
                FormTransactionDetail.ReceivingStockId = GetReceivingStock.Id;
                FormTransactionDetail.StatusTransfer = GetReceivingStock.StatusReceived;
                FormTransactionDetail.TypeTransaction = "Received";

                await Mediator.Send(new CreateTransactionStockDetailRequest(FormTransactionDetail));

                isActiveButton = false;
            }
            await LoadData();
        }

        private async Task onCancel()
        {
            var receivedStock = await Mediator.Send(new GetReceivingStockQuery());
            var receivedProductStock = await Mediator.Send(new GetReceivingStockProductQuery());
            FormReceivingStocks = receivedStock.Where(x => x.Id == receivingId).FirstOrDefault()!;

            if (FormReceivingStocks is not null)
            {
                FormReceivingStocks.StatusReceived = " Cancel";
                GetReceivingStock = await Mediator.Send(new UpdateReceivingStockRequest(FormReceivingStocks));
            }

            //Save Log
            FormTransactionDetail.DestinationId = GetReceivingStock.DestinationId;
            FormTransactionDetail.ReceivingStockId = GetReceivingStock.Id;
            FormTransactionDetail.StatusTransfer = GetReceivingStock.StatusReceived;
            FormTransactionDetail.TypeTransaction = "Received";

            await Mediator.Send(new CreateTransactionStockDetailRequest(FormTransactionDetail));
        }

        #endregion Validation

        #region Function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                List<ReceivingStockDto> receivings = SelectedDataItems.Adapt<List<ReceivingStockDto>>();
                List<long> id = receivings.Select(x => x.Id).ToList();
                List<long> ProductIdsToDelete = new();
                List<long> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //Delete Data Receiving stock Product

                    ProductIdsToDelete = receivingStockDetails
                        .Where(x => x.ReceivingStockId == receivingId)
                        .Select(x => x.Id)
                        .ToList();
                    await Mediator.Send(new DeleteReceivingStockPoductRequest(ids: ProductIdsToDelete));

                    //Delete Data Transaction Detail (log)

                    DetailsIdsToDelete = TransactionStockDetails
                        .Where(x => x.ReceivingStockId == receivingId)
                        .Select(x => x.Id)
                        .ToList();
                    await Mediator.Send(new DeleteTransactionStockDetailRequest(ids: DetailsIdsToDelete));

                    //Delete Receiving Stock

                    await Mediator.Send(new DeleteReceivingStockRequest(SelectedDataItems[0].Adapt<ReceivingStockDto>().Id));
                }
                else
                {
                    foreach (var Uid in id)
                    {
                        //Delete Data Receiving Stock Product
                        ProductIdsToDelete = receivingStockDetails
                       .Where(x => x.ReceivingStockId == Uid)
                       .Select(x => x.Id)
                       .ToList();
                        await Mediator.Send(new DeleteReceivingStockPoductRequest(ids: ProductIdsToDelete));

                        //Delete Data Transaction Detail (log)

                        DetailsIdsToDelete = TransactionStockDetails
                            .Where(x => x.ReceivingStockId == Uid)
                            .Select(x => x.Id)
                            .ToList();
                        await Mediator.Send(new DeleteTransactionStockDetailRequest(ids: DetailsIdsToDelete));
                    }
                    //Delete list Id ReceivingStock
                    await Mediator.Send(new DeleteReceivingStockRequest(ids: id));
                }
                ToastService.ShowSuccess("Data Deleting Success!..");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnDelete_Detail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                StateHasChanged();
                var data = SelectedDataItemsDetail.Adapt<List<ReceivingStockProductDto>>();
                var cek = data.Select(x => x.ReceivingStockId).FirstOrDefault();
                if (cek == null)
                {
                    TempReceivingStockDetails.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                }
                else
                {
                    TempReceivingStockDetails.RemoveAll(x => data.Select(z => z.ReceivingStockId).Contains(x.ReceivingStockId));
                }
                SelectedDataItemsDetail = new ObservableRangeCollection<object>();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Function Delete

        #region Function Save

        private async Task OnSave()
        {
            try
            {
                if (FormValidationState == false)

                    return;

                if (FormReceivingStocks.Id == 0)
                {
                    var getReceiving = ReceivingStocks.Where(r => r.DestinationId == FormReceivingStocks.DestinationId).OrderByDescending(x => x.KodeReceiving).Select(x => x.KodeReceiving).FirstOrDefault();
                    if (getReceiving == null)
                    {
                        var nextTransactionNumber = 1;
                        FormReceivingStocks.KodeReceiving = $"WH-IN/{nextTransactionNumber.ToString("0000")}";
                    }
                    else
                    {
                        var lastTransactionNumber = 0;
                        if (getReceiving.Contains("WH-IN/"))
                        {
                            var lastTransactionNumberStr = getReceiving.Split('/')[1];
                            int.TryParse(lastTransactionNumberStr, out lastTransactionNumber);
                        }
                        var nextTransactionNumber = lastTransactionNumber + 1;
                        FormReceivingStocks.KodeReceiving = $"WH-IN/{nextTransactionNumber.ToString("0000")}";
                    }

                    FormReceivingStocks.StatusReceived = "Draft";

                    GetReceivingStock = await Mediator.Send(new CreateReceivingStockRequest(FormReceivingStocks));

                    TempReceivingStockDetails.ForEach(x =>
                    {
                        x.ReceivingStockId = GetReceivingStock.Id;
                        x.Id = 0;
                    });
                    await Mediator.Send(new CreateListReceivingStockProductRequest(TempReceivingStockDetails));
                    ToastService.ShowSuccess("Add Data Success...");

                    FormTransactionDetail.DestinationId = GetReceivingStock.DestinationId;
                    FormTransactionDetail.ReceivingStockId = GetReceivingStock.Id;
                    FormTransactionDetail.StatusTransfer = GetReceivingStock.StatusReceived;
                    FormTransactionDetail.TypeTransaction = "Received";

                    await Mediator.Send(new CreateTransactionStockDetailRequest(FormTransactionDetail));
                }
                else
                {
                    GetReceivingStock = await Mediator.Send(new UpdateReceivingStockRequest(FormReceivingStocks));
                    ToastService.ShowSuccess("Update Data Success...");
                }

                ToastService.ClearSuccessToasts();

                await EditItem_Click(GetReceivingStock);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSave_Detail(GridEditModelSavingEventArgs e)
        {
            if (e is null)
                return;

            var r = (ReceivingStockProductDto)e.EditModel;

            if (FormReceivingStocks.Id == 0)
            {
                try
                {
                    ReceivingStockProductDto updates = new();

                    if (r.Id == 0)
                    {
                        r.Id = Helper.RandomNumber;
                        TempReceivingStockDetails.Add(r);
                    }
                    else
                    {
                        var q = SelectedDataItemsDetail.Adapt<ReceivingStockProductDto>();

                        updates = TempReceivingStockDetails.FirstOrDefault(x => x.Id == q.Id)!;

                        var index = TempReceivingStockDetails.IndexOf(updates!);
                        TempReceivingStockDetails[index] = r;
                    }
                    SelectedDataItemsDetail = [];
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                r.ReceivingStockId = FormReceivingStocks.Id;
                if (r.Id == 0)
                    await Mediator.Send(new CreateReceivingStockProductRequest(r));
                else
                    await Mediator.Send(new UpdateReceivingStockProductRequest(r));

                await LoadData_Detail();
            }
        }

        #endregion Function Save
    }
}
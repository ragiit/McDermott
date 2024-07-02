using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class InternalTransfer
    {
        #region relation Data

        private List<TransactionStockDto> TransactionStocks = [];
        private List<TransactionStockProductDto> TempTransactionStocks = [];
        private List<TransactionStockDetailDto> AllLogs = [];
        private List<TransactionStockDetailDto> Logs = [];
        private List<TransactionStockProductDto> TransactionStockProducts = [];
        private List<TransactionStockDetailDto> TransactionStockDetails = [];
        private List<StockProductDto> StockProducts = [];
        private List<LocationDto> Locations = [];
        private List<ProductDto> Products = [];
        private List<ProductDto> filteredProducts = [];
        private List<UomDto> Uoms = [];
        private TransactionStockDto FormInternalTransfer = new();
        private TransactionStockDto getInternalTransfer = new();
        private TransactionStockProductDto TempFormInternalTransfer = new();
        private TransactionStockDetailDto FormInternalTransferDetail = new();
        private StockProductDto FormStock = new();
        private UserDto NameUser = new();

        #endregion relation Data

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

                await LoadAsyncData();
                StateHasChanged();
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

        #region static Variable

        private IGrid? Grid { get; set; }
        private IGrid? GridDetailTransferStock { get; set; }
        private IGrid? GridDetailTransferStockLogs { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool showFormDetail { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private bool IsAddTransfer { get; set; } = false;
        private bool showButton { get; set; } = false;
        private bool ActiveButton { get; set; } = false;
        private bool showMatching { get; set; } = false;

        //private bool HasValueFalse { get; set; }
        private long? transactionId { get; set; }

        private string? header { get; set; } = string.Empty;
        private string? headerDetail { get; set; } = string.Empty;

        private bool isActiveButton = false;
        private string? UomName { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsDetail { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataItemsDetailLogs { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexDetail { get; set; }

        #endregion static Variable

        #region Load

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }

        private async Task LoadAsyncData()
        {
            StockProducts = await Mediator.Send(new GetStockProductQuery());
            Locations = await Mediator.Send(new GetLocationQuery());
            Products = await Mediator.Send(new GetProductQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            UomName = Uoms.Select(x => x.Name).FirstOrDefault();
            TransactionStockDetails = await Mediator.Send(new GetTransactionStockDetailQuery());
            var user_group = await Mediator.Send(new GetUserQuery());
            NameUser = user_group.FirstOrDefault(x => x.GroupId == UserAccessCRUID.GroupId && x.Id == UserLogin.Id) ?? new();
        }
        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                showForm = false;
                showFormDetail = false;
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task LoadData_Detail()
        {
            PanelVisible = true;
            SelectedDataItemsDetail = [];
            TempTransactionStocks = await Mediator.Send(new GetTransactionStockProductQuery());
            PanelVisible = false;
        }

        private List<StockProductDto> Batch = [];
        private DateTime? SelectedBatchExpired { get; set; }

        public class StatusComparer : IComparer<EnumStatusInternalTransfer>
        {
            private static readonly List<EnumStatusInternalTransfer> StatusOrder = new List<EnumStatusInternalTransfer> { EnumStatusInternalTransfer.Draft, EnumStatusInternalTransfer.Request, EnumStatusInternalTransfer.ApproveRequest, EnumStatusInternalTransfer.Waiting, EnumStatusInternalTransfer.Ready, EnumStatusInternalTransfer.Done, EnumStatusInternalTransfer.Cancel };

            public int Compare(EnumStatusInternalTransfer x, EnumStatusInternalTransfer y)
            {
                int indexX = StatusOrder.IndexOf(x);
                int indexY = StatusOrder.IndexOf(y);

                // Compare the indices
                return indexX.CompareTo(indexY);
            }
        }
        private async Task SelectedBatch(StockProductDto stockProduct)
        {
            SelectedBatchExpired = null;

            if (stockProduct is not null)
            {
                SelectedBatchExpired = stockProduct.Expired;

                //current Stock
                var _currentStock = StockProducts.Where(x => x.SourceId == FormInternalTransfer.SourceId && x.ProductId == TempFormInternalTransfer.ProductId && x.Batch == stockProduct.Batch).FirstOrDefault();
                if (_currentStock is not null)
                {
                    if (_currentStock.Qty > 0)
                    {
                        TempFormInternalTransfer.StockProductId= stockProduct.Id;
                        TempFormInternalTransfer.Batch = stockProduct.Batch;
                        TempFormInternalTransfer.ExpiredDate = stockProduct.Expired;
                        TempFormInternalTransfer.CurrentStock = _currentStock.Qty;
                    }
                    else
                    {
                        ToastService.ClearCustomToasts();
                        ToastService.ShowWarning("Empty Stock!.. ");
                    }
                }
            }

        }

        private async Task SelectedItemProduct(LocationDto value)
        {
            try
            {
                filteredProducts = Products.Where(p => StockProducts.Any(sp => sp.ProductId == p.Id && sp.SourceId == value.Id)).ToList();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SelectedItemProduct(ProductDto product)
        {
            if (product is not null)
            {
                var data = Products.Where(p => p.Id == product.Id).FirstOrDefault();
                TempFormInternalTransfer.ProductName = data?.Name;
                TempFormInternalTransfer.TraceAvability = data.TraceAbility;
                var uomName = Uoms.Where(u => u.Id == data?.UomId).Select(x => x.Name).FirstOrDefault();
                TempFormInternalTransfer.UomName = uomName;
                var StockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == product.Id && s.SourceId == FormInternalTransfer.SourceId &&  s.Qty != 0));

                TempFormInternalTransfer.TraceAvability = product.TraceAbility;


                Batch.Clear();
                if (StockProducts is not null)
                    Batch = StockProducts;
                else
                    ToastService.ShowWarning("The selected product does not have stock in all batches!!");


                if (product.TraceAbility)
                {
                    TempFormInternalTransfer.Batch = StockProducts.FirstOrDefault(x => x.SourceId == FormInternalTransfer.SourceId)?.Batch ;
                    TempFormInternalTransfer.ExpiredDate = StockProducts.FirstOrDefault(x => x.SourceId == FormInternalTransfer.SourceId)?.Expired;
                    TempFormInternalTransfer.CurrentStock = 0;
                }
                else
                {
                    TempFormInternalTransfer.Batch = "-";
                    TempFormInternalTransfer.ExpiredDate = null;
                    TempFormInternalTransfer.CurrentStock = StockProducts.Where(x => x.SourceId == FormInternalTransfer.SourceId && x.ProductId == product.Id).Select(x => x.Qty).FirstOrDefault();
                }
            }
        }

        private void checkStock(long value)
        {
            if (value > TempFormInternalTransfer.CurrentStock)
            {
                ToastService.ClearCustomToasts();
                ToastService.ShowWarning("The stock sent is less than the available stock!!");
            }
            else
            {
                ActiveButton = true;
                TempFormInternalTransfer.QtyStock = value;
            }
        }
        public MarkupString GetIssueStatusIconHtml(EnumStatusInternalTransfer? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusInternalTransfer.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumStatusInternalTransfer.Request:
                    priorityClass = "info";
                    title = "Request";
                    break;

                case EnumStatusInternalTransfer.ApproveRequest:
                    priorityClass = "info";
                    title = "Approve Request";
                    break;

                case EnumStatusInternalTransfer.Waiting:
                    priorityClass = "warning";
                    title = "Waiting";
                    break;

                case EnumStatusInternalTransfer.Ready:
                    priorityClass = "primary";
                    title = "Ready";
                    break;

                case EnumStatusInternalTransfer.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusInternalTransfer.Cancel:
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

        #endregion Load

        #region Grid

        private async Task HandleValidSubmit()
        {
            //IsLoading = true;
            FormValidationState = true;
            await OnSave();
            //IsLoading = false;
        }

        private async Task HandleInvalidSubmit()
        {
            showForm = true;
            FormValidationState = false;
        }

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
                if ((TransactionStockDto)args.DataItem is null)
                    return;

                isActiveButton = ((TransactionStockDto)args.DataItem)!.Status!.Equals(EnumStatusInternalTransfer.Draft);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void Grid_FocusedRowChangedDetail(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexDetail = args.VisibleIndex;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            await EditItem_Click();
        }

        private async Task OnRowDoubleClickDetail(GridRowClickEventArgs e)
        {
            EditItemDetail_Click();
        }

        #endregion Grid

        #region Click

        private void NewItem_Click()
        {
            showForm = true;
            Logs.Clear();
            FormInternalTransfer = new();
            TempTransactionStocks.Clear();
            isActiveButton = true;
            header = "Add Transfer Internal";
        }

        private async Task NewItemDetail_Click()
        {
            try
            {
                showFormDetail = true;
                IsAddTransfer = true;
                TempFormInternalTransfer = new();
                //Products.Clear();
                headerDetail = "Add product Transfer Internal";
                await GridDetailTransferStock.StartEditNewRowAsync();
            
            }catch(Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task EditItem_Click(TransactionStockDto? p = null)
        {
            try
            {
                showForm = true;
                PanelVisible = true;
                header = "Edit Data";

                FormInternalTransfer = p ?? SelectedDataItems[0].Adapt<TransactionStockDto>();
                isActiveButton = FormInternalTransfer.Status == EnumStatusInternalTransfer.Draft;
                transactionId = FormInternalTransfer.Id;

                TransactionStockProducts = await Mediator.Send(new GetTransactionStockProductQuery(x => x.TransactionStockId == FormInternalTransfer.Id));
                TempTransactionStocks = TransactionStockProducts.ToList();

                foreach (var item in TempTransactionStocks)
                {
                    var product = Products.FirstOrDefault(x => x.Id == item.ProductId);
                    item.UomName = Uoms.FirstOrDefault(u => u.Id == product?.UomId)?.Name;

                    var stockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == item.ProductId && s.SourceId == FormInternalTransfer.SourceId));
                    var stockProduct = stockProducts.FirstOrDefault();

                    if (item.Product?.TraceAbility == true)
                    {
                        item.Batch = stockProduct?.Batch ?? "-";
                        item.ExpiredDate = stockProduct?.Expired;
                    }
                    else
                    {
                        item.Batch = "-";
                        item.ExpiredDate = null;
                    }
                }

                await LoadLogs();

                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task LoadLogs()
        {
            Logs = await Mediator.Send(new GetTransactionStockDetailQuery(x => x.TransactionStockId == FormInternalTransfer.Id));
        }

        private async Task EditItemDetail_Click()
        {
            showFormDetail = true;
            IsAddTransfer = false;

            await GridDetailTransferStock.StartEditRowAsync(FocusedRowVisibleIndexDetail);
        }

        private async Task Request()
        {
            try
            {
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                FormInternalTransfer = TransactionStocks.Where(x => x.Id == transactionId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Request;
                    getInternalTransfer = await Mediator.Send(new UpdateTransactionStockRequest(FormInternalTransfer));
                }

                //Save Log
                FormInternalTransferDetail.TransactionStockId = getInternalTransfer.Id;
                FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                FormInternalTransferDetail.Status = EnumStatusInternalTransfer.Request;

                await Mediator.Send(new CreateTransactionStockDetailRequest(FormInternalTransferDetail));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Ready()
        {
            try
            {
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                FormInternalTransfer = TransactionStocks.Where(x => x.Id == transactionId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Ready;
                    getInternalTransfer = await Mediator.Send(new UpdateTransactionStockRequest(FormInternalTransfer));
                }

                //Save Log
                FormInternalTransferDetail.TransactionStockId = getInternalTransfer.Id;
                FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                FormInternalTransferDetail.Status = EnumStatusInternalTransfer.Ready;

                await Mediator.Send(new CreateTransactionStockDetailRequest(FormInternalTransferDetail));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Approv_Check()
        {
            try
            {
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                FormInternalTransfer = TransactionStocks.Where(x => x.Id == transactionId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.ApproveRequest;
                    getInternalTransfer = await Mediator.Send(new UpdateTransactionStockRequest(FormInternalTransfer));
                }

                //Save Log
                FormInternalTransferDetail.TransactionStockId = getInternalTransfer.Id;
                FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                FormInternalTransferDetail.Status = EnumStatusInternalTransfer.ApproveRequest;

                await Mediator.Send(new CreateTransactionStockDetailRequest(FormInternalTransferDetail));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        //private async Task ToDoCheck()
        //{
        //    ToastService.ClearAll();

        //    List<bool> allMatched = new List<bool>();

        //    var asyncData = await Mediator.Send(new GetTransactionStockProductQuery(x => x.TransactionStockId == transactionId));
        //    if (asyncData.Count > 0)
        //    {
        //        foreach (var item in asyncData)
        //        {
        //            var StockSent = asyncData.Where(t => t.TransactionStock is not null && t.TransactionStock.SourceId == item.TransactionStock!.SourceId && t.ProductId == item.ProductId).FirstOrDefault();
        //            var warehouse_stock = StockProducts.Where(sp => sp.SourceId == item?.TransactionStock?.SourceId && sp.ProductId == item?.ProductId).FirstOrDefault();
        //            if (warehouse_stock is not null && StockSent is not null)
        //            {
        //                if (StockSent.QtyStock <= warehouse_stock.Qty)
        //                {
        //                    allMatched.Add(true);
        //                }
        //                else
        //                {
        //                    allMatched.Add(false);
        //                }
        //            }
        //            else
        //            {
        //                allMatched.Add(false);
        //            }
        //        }

        //        bool HasValueFalse = allMatched.Any(x => x == false);
        //        var datas = await Mediator.Send(new GetTransactionStockQuery());
        //        FormInternalTransfer = datas.Where(x => x.Id == transactionId).FirstOrDefault()!;
        //        if (HasValueFalse)
        //        {
        //            FormInternalTransfer.Status = EnumStatusInternalTransfer.Waiting;
        //            ToastService.ShowError("Pastikan Stock Disemua Produk Terpenuhi!..");
        //        }
        //        else
        //        {
        //            FormInternalTransfer.Status = EnumStatusInternalTransfer.Ready;
        //            ToastService.ShowSuccess(" Stock Disemua Produk Terpenuhi..");
        //        }
        //        getInternalTransfer = await Mediator.Send(new UpdateTransactionStockRequest(FormInternalTransfer));

        //        FormInternalTransferDetail.TransactionStockId = getInternalTransfer.Id;
        //        FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
        //        FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
        //        FormInternalTransferDetail.Status = getInternalTransfer.Status;

        //        await Mediator.Send(new CreateTransactionStockDetailRequest(FormInternalTransferDetail));

        //        await EditItem_Click(getInternalTransfer);
        //    }
        //    else
        //    {
        //        ToastService.ShowError("Product Data Not Found!!..");
        //    }
        //}

        private async Task onDiscard()
        {
            await LoadData();
        }

        private async Task validation()
        {
            try
            {
                ToastService.ClearAll();
                var Stock = await Mediator.Send(new GetStockProductQuery());
                var transactionProductStock = await Mediator.Send(new GetTransactionStockProductQuery());
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                FormInternalTransfer = TransactionStocks.Where(x => x.Id == transactionId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Done;
                    getInternalTransfer = await Mediator.Send(new UpdateTransactionStockRequest(FormInternalTransfer));

                    var checkTransactionProduct = transactionProductStock.Where(x => x.TransactionStockId == getInternalTransfer.Id).ToList();
                    foreach (var a in checkTransactionProduct)
                    {
                        //check product stock OUT availability
                        var checkedStockOut = Stock.Where(x => x.ProductId == a.ProductId && x.SourceId == getInternalTransfer.SourceId).FirstOrDefault();
                        checkedStockOut.Qty = checkedStockOut.Qty - a.QtyStock;
                        await Mediator.Send(new UpdateStockProductRequest(checkedStockOut));

                        //check product stock IN availability
                        var checkStockIn = Stock.Where(x => x.ProductId == a.ProductId && x.SourceId == getInternalTransfer.DestinationId).FirstOrDefault();
                        if (checkStockIn is null)
                        {
                            FormStock.ProductId = a.ProductId;
                            FormStock.SourceId = getInternalTransfer.DestinationId;
                            FormStock.UomId = a?.Product?.UomId;
                            FormStock.Qty = a.QtyStock;
                            if (TempFormInternalTransfer.TraceAvability == true && checkedStockOut.Batch != null && checkedStockOut.Expired != null)
                            {
                                FormStock.Batch = checkedStockOut?.Batch;
                                FormStock.Expired = checkedStockOut?.Expired;
                            }
                            await Mediator.Send(new CreateStockProductRequest(FormStock));
                        }
                        else
                        {
                            if (TempFormInternalTransfer.TraceAvability == true && checkedStockOut.Batch != null && checkedStockOut.Expired != null)
                            {
                                if (checkStockIn.Batch == null && checkStockIn.Expired == null)
                                {
                                    checkStockIn.Batch = checkedStockOut?.Batch;
                                    checkStockIn.Expired = checkedStockOut?.Expired;
                                }
                            }
                            checkStockIn!.Qty = checkStockIn.Qty + a.QtyStock;
                            await Mediator.Send(new UpdateStockProductRequest(checkStockIn));
                        }
                    }

                    //Save Log
                    FormInternalTransferDetail.TransactionStockId = getInternalTransfer.Id;
                    FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                    FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                    FormInternalTransferDetail.Status = getInternalTransfer.Status;
                    FormInternalTransferDetail.TypeTransaction = "Transfer";

                    await Mediator.Send(new CreateTransactionStockDetailRequest(FormInternalTransferDetail));

                    await LoadLogs();
                }
                else
                {
                    ToastService.ShowError("Data Is Not Found!..");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Cancel()
        {
            try
            {
                var transactionProductStock = await Mediator.Send(new GetTransactionStockProductQuery());
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                FormInternalTransfer = TransactionStocks.Where(x => x.Id == transactionId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Cancel;
                    getInternalTransfer = await Mediator.Send(new UpdateTransactionStockRequest(FormInternalTransfer));
                }

                //Save Log
                FormInternalTransferDetail.TransactionStockId = getInternalTransfer.Id;
                FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                FormInternalTransferDetail.Status = getInternalTransfer.Status;

                await Mediator.Send(new CreateTransactionStockDetailRequest(FormInternalTransferDetail));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void DeleteItem_Click()
        {
            Grid!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task DeleteItemDetail_Click()
        {
            GridDetailTransferStock!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        #endregion Click

        #region function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                ToastService.ClearAll();
                List<TransactionStockDto> transactions = SelectedDataItems.Adapt<List<TransactionStockDto>>();
                List<long> id = transactions.Select(x => x.Id).ToList();
                List<long> ProductIdsToDelete = new();
                List<long> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //delete data Transaction Stock Product
                    ProductIdsToDelete = TransactionStockProducts
                               .Where(x => x.TransactionStockId == transactionId)
                               .Select(x => x.Id)
                               .ToList();
                    await Mediator.Send(new DeleteTransactionStockProductRequest(ids: ProductIdsToDelete));

                    //Delete data transaction Detal transfer (Log)

                    DetailsIdsToDelete = TransactionStockDetails
                       .Where(x => x.TransactionStockId == transactionId)
                       .Select(x => x.Id)
                       .ToList();
                    await Mediator.Send(new DeleteTransactionStockDetailRequest(ids: DetailsIdsToDelete));

                    //Delete Transaction

                    await Mediator.Send(new DeleteTransactionStockRequest(SelectedDataItems[0].Adapt<TransactionStockDto>().Id));
                }
                else
                {
                    foreach (var uid in id)
                    {
                        //delete data Transaction Stock Product
                        ProductIdsToDelete = TransactionStockProducts
                                   .Where(x => x.TransactionStockId == transactionId)
                                   .Select(x => x.Id)
                                   .ToList();
                        await Mediator.Send(new DeleteTransactionStockProductRequest(ids: ProductIdsToDelete));

                        //Delete data transaction Detal transfer (Log)

                        DetailsIdsToDelete = TransactionStockDetails
                           .Where(x => x.TransactionStockId == transactionId)
                           .Select(x => x.Id)
                           .ToList();
                        await Mediator.Send(new DeleteTransactionStockDetailRequest(ids: DetailsIdsToDelete));
                    }
                    await Mediator.Send(new DeleteTransactionStockRequest(ids: id));
                }
                ToastService.ShowSuccess("Data Deleting success..");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        List<TransactionStockProductDto> IdDeleteDetail = new List<TransactionStockProductDto>();
        private async Task onDelete_Detail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                StateHasChanged();
                var data = SelectedDataItemsDetail.Adapt<List<TransactionStockProductDto>>();
                var cek = data.Select(x => x.TransactionStockId).FirstOrDefault();
                if (cek == null)
                {
                    TempTransactionStocks.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                }
                else
                {
                    foreach (var item in data)
                    {
                        TempTransactionStocks.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                        IdDeleteDetail.Add(item);
                    }
                }
                SelectedDataItemsDetail = new ObservableRangeCollection<object>();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion function Delete

        #region Function Save

        private async Task OnSave_Detail(GridEditModelSavingEventArgs e)
        {
            if (e is null)
                return;

            var i = (TransactionStockProductDto)e.EditModel;

            if (FormInternalTransfer.Id == 0)
            {
                /*
                 Kesalahan Batch yang di Simpan
                 */
                try
                {
                    TransactionStockProductDto Updates = new();

                    if (i.Id == 0)
                    {
                        i.Id = Helper.RandomNumber;
                        TempTransactionStocks.Add(i);
                    }
                    else
                    {
                        var q = i;

                        Updates = TempTransactionStocks.FirstOrDefault(x => x.Id == q.Id)!;

                        var index = TempTransactionStocks.IndexOf(Updates!);
                        TempTransactionStocks[index] = i;
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
                i.TransactionStockId = FormInternalTransfer.Id;
                if (i.Id == 0)
                    await Mediator.Send(new CreateTransactionStockProductRequest(i));
                else
                    await Mediator.Send(new UpdateTransactionStockProductRequest(i));

                StateHasChanged();
                await EditItem_Click(null);
            }
        }

        private async Task OnSave()
        {
            try
            {
                if (FormValidationState == false)
                {
                    return;
                }

                if (FormInternalTransfer.Id == 0)
                {
                    var sourcname = Locations.Where(x => x.Id == FormInternalTransfer.SourceId).Select(x => x.Name).FirstOrDefault();
                    var getKodeTransaksi = TransactionStocks.Where(t => t.SourceId == FormInternalTransfer.SourceId).OrderByDescending(x => x.KodeTransaksi).Select(x => x.KodeTransaksi).FirstOrDefault();

                    if (getKodeTransaksi == null)
                    {
                        var nextTransactionNumber = 1;
                        FormInternalTransfer.KodeTransaksi = $"{sourcname}/INT/{nextTransactionNumber.ToString("00000")}";
                    }
                    else
                    {
                        var lastTransactionNumber = 0;
                        if (getKodeTransaksi.Contains("/INT/"))
                        {
                            var lastTransactionNumberStr = getKodeTransaksi.Split('/')[2];
                            int.TryParse(lastTransactionNumberStr, out lastTransactionNumber);
                        }

                        var nextTransactionNumber = lastTransactionNumber + 1;
                        FormInternalTransfer.KodeTransaksi = $"{sourcname}/INT/{nextTransactionNumber.ToString("00000")}";
                    }

                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Draft;

                    getInternalTransfer = await Mediator.Send(new CreateTransactionStockRequest(FormInternalTransfer));
                    TempTransactionStocks.ForEach(x =>
                    {
                        x.TransactionStockId = getInternalTransfer.Id;
                        x.Id = 0;
                    });
                    await Mediator.Send(new CreateListTransactionStockProductRequest(TempTransactionStocks));
                    ToastService.ShowSuccess("Add Data Success...");

                    FormInternalTransferDetail.TransactionStockId = getInternalTransfer.Id;
                    FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                    FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                    FormInternalTransferDetail.Status = getInternalTransfer.Status;
                    FormInternalTransferDetail.TypeTransaction = "Transfer";

                    await Mediator.Send(new CreateTransactionStockDetailRequest(FormInternalTransferDetail));
                }
                else
                {
                    getInternalTransfer = await Mediator.Send(new UpdateTransactionStockRequest(FormInternalTransfer));
                    if (IdDeleteDetail.Count > 0)
                    {
                        foreach (var id in IdDeleteDetail)
                        {
                            var cek = TransactionStockDetails.Where(z => z.Id == id.Id).FirstOrDefault();
                            await Mediator.Send(new DeleteTransactionStockProductRequest(id.Id));
                        }
                    }
                    ToastService.ShowSuccess("Update Data Success...");
                }

                await EditItem_Click(getInternalTransfer);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Function Save
    }
}
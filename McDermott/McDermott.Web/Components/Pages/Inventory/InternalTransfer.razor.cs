using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Inventory.TransferStockCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class InternalTransfer
    {
        #region relation Data

        private List<TransferStockDto> TransferStocks = [];
        private List<TransferStockProductDto> TempTransferStocks = [];
        private List<TransferStockLogDto> AllLogs = [];
        private List<TransferStockLogDto> Logs = [];
        private List<TransferStockProductDto> TransferStockProducts = [];
        private List<TransferStockLogDto> TransferStockLogs = [];
        private List<TransactionStockDto> TransactionStocks = [];
        private List<StockProductDto> StockProducts = [];
        private List<LocationDto> Locations = [];
        private List<ProductDto> Products = [];
        private List<ProductDto> filteredProducts = [];
        private List<UomDto> Uoms = [];
        private TransferStockDto FormInternalTransfer = new();
        private TransferStockDto getInternalTransfer = new();
        private TransferStockProductDto TempFormInternalTransfer = new();
        private TransferStockLogDto FormInternalTransferDetail = new();
        private TransactionStockDto FormTransactionStocks = new();
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
        private long? TransferId { get; set; }

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
            TransferStockProducts = await Mediator.Send(new GetTransferStockProductQuery());
            TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
            Locations = await Mediator.Send(new GetLocationQuery());
            Products = await Mediator.Send(new GetProductQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            UomName = Uoms.Select(x => x.Name).FirstOrDefault();
            TransferStockLogs = await Mediator.Send(new GetTransferStockLogQuery());
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
                TransferStocks = await Mediator.Send(new GetTransferStockQuery());
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
            TempTransferStocks = await Mediator.Send(new GetTransferStockProductQuery());
            PanelVisible = false;
        }

        private List<TransactionStockDto> Batch = [];
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
        private async Task SelectedBatch(TransactionStockDto stockProduct)
        {
            SelectedBatchExpired = null;

            if (stockProduct is not null)
            {
                SelectedBatchExpired = stockProduct.ExpiredDate;

                //    //current Stock
                var _currentStock = TransactionStocks.Where(x => x.LocationId == FormInternalTransfer.SourceId && x.ProductId == TempFormInternalTransfer.ProductId && x.Batch == stockProduct.Batch && x.Validate == true).FirstOrDefault();
                var Qty = TransactionStocks.Where(x => x.LocationId == FormInternalTransfer.SourceId && x.ProductId == TempFormInternalTransfer.ProductId && x.Batch == stockProduct.Batch && x.Validate == true).Sum(x => x.Quantity);
                if (_currentStock is not null)
                {
                    if (Qty > 0)
                    {

                        TempFormInternalTransfer.Batch = stockProduct.Batch;
                        TempFormInternalTransfer.ExpiredDate = stockProduct.ExpiredDate;
                        TempFormInternalTransfer.CurrentStock = Qty;
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
                filteredProducts = Products.Where(p => TransactionStocks.Any(sp => sp.ProductId == p.Id && sp.LocationId == value.Id)).ToList();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        List<string> BeteAh = [];
        private async Task SelectedItemProduct(ProductDto product)
        {
            if (product is not null)
            {
                var data = Products.Where(p => p.Id == product.Id).FirstOrDefault();
                TempFormInternalTransfer.ProductName = data?.Name;
                TempFormInternalTransfer.TraceAvability = data.TraceAbility;
                var uomName = Uoms.Where(u => u.Id == data?.UomId).Select(x => x.Name).FirstOrDefault();
                TempFormInternalTransfer.UomName = uomName;
                List<TransactionStockDto> listbatch = TransactionStocks.Where(s => s.ProductId == product.Id && s.LocationId == FormInternalTransfer.SourceId).ToList();
                TempFormInternalTransfer.TraceAvability = product.TraceAbility;

                var groupedBatch = TransactionStocks
                    .Where(s => s.ProductId == product.Id && s.LocationId == FormInternalTransfer.SourceId)
                    .GroupBy(s => s.Batch)
                    .Select(g => new TransactionStockDto
                    {
                        ProductId = product.Id, 
                        Batch = g.Key,
                        ExpiredDate = g.First().ExpiredDate,
                        Quantity = g.Sum(x => x.Quantity),
                        LocationId = FormInternalTransfer.SourceId
                    }).ToList();

                Batch.Clear();
                if (StockProducts is not null)
                {
                    Batch = groupedBatch;
                    
                }
                else
                    ToastService.ShowWarning("The selected product does not have stock in all batches!!");

                if (product.TraceAbility)
                {
                    TempFormInternalTransfer.Batch = listbatch.FirstOrDefault(x => x.LocationId == FormInternalTransfer.SourceId)?.Batch;
                    TempFormInternalTransfer.ExpiredDate = listbatch.FirstOrDefault(x => x.LocationId == FormInternalTransfer.SourceId)?.ExpiredDate;
                    TempFormInternalTransfer.CurrentStock = listbatch?.Where(x => x.LocationId == FormInternalTransfer.SourceId && x.ProductId == product.Id && x.Batch == TempFormInternalTransfer.Batch && x.Validate == true).Sum(x => x.Quantity) ?? 0; ;

                }
                else
                {
                    TempFormInternalTransfer.Batch = "-";
                    TempFormInternalTransfer.ExpiredDate = null;
                    TempFormInternalTransfer.CurrentStock = listbatch?.Where(x => x.LocationId == FormInternalTransfer.SourceId && x.ProductId == product.Id).Sum(x => x.Quantity) ?? 0;
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
                if ((TransferStockDto)args.DataItem is null)
                    return;

                isActiveButton = ((TransferStockDto)args.DataItem)!.Status!.Equals(EnumStatusInternalTransfer.Draft);
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
            TempTransferStocks.Clear();
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
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task EditItem_Click(TransferStockDto? p = null)
        {
            try
            {
                showForm = true;
                PanelVisible = true;
                header = "Edit Data";

                FormInternalTransfer = p ?? SelectedDataItems[0].Adapt<TransferStockDto>();
                isActiveButton = FormInternalTransfer.Status == EnumStatusInternalTransfer.Draft;
                TransferId = FormInternalTransfer.Id;
                getInternalTransfer = FormInternalTransfer;

                TransferStockProducts = await Mediator.Send(new GetTransferStockProductQuery(x => x.TransferStockId == FormInternalTransfer.Id));
                TempTransferStocks = TransferStockProducts.ToList();

                foreach (var item in TempTransferStocks)
                {
                    var product = Products.FirstOrDefault(x => x.Id == item.ProductId);
                    item.UomName = Uoms.FirstOrDefault(u => u.Id == product?.UomId)?.Name;

                    var stockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == item.ProductId && s.SourceId == FormInternalTransfer.SourceId));
                    var stockProduct = stockProducts.FirstOrDefault();

                    if (item.Product?.TraceAbility == true)
                    {
                        item.Batch = stockProduct?.Batch;
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
            Logs = await Mediator.Send(new GetTransferStockLogQuery(x => x.TransferStockId == FormInternalTransfer.Id));
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
                TransferStocks = await Mediator.Send(new GetTransferStockQuery());
                FormInternalTransfer = TransferStocks.Where(x => x.Id == TransferId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Request;
                    getInternalTransfer = await Mediator.Send(new UpdateTransferStockRequest(FormInternalTransfer));
                }

                //Save Log
                FormInternalTransferDetail.TransferStockId = getInternalTransfer.Id;
                FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                FormInternalTransferDetail.Status = EnumStatusInternalTransfer.Request;

                await Mediator.Send(new CreateTransferStockLogRequest(FormInternalTransferDetail));
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
                await LoadAsyncData();
                await LoadData();
                FormInternalTransfer = TransferStocks.Where(x => x.Id == TransferId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(TransferStock)).OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                    int NextReferenceNumber = 1;
                    if (cekReference != null)
                    {
                        int.TryParse(cekReference?.Substring("ITR#".Length), out NextReferenceNumber);
                        NextReferenceNumber++;
                    }

                    string referenceNumber = $"ITR#{NextReferenceNumber:D3}";
                    var checkTranferProduct = TransferStockProducts.Where(x => x.TransferStockId == getInternalTransfer.Id).ToList()!;
                    foreach (var items in checkTranferProduct)
                    {
                        var Cek_Uom = Uoms.Where(x => x.Id == items.Product?.UomId).FirstOrDefault();

                        // out 
                        FormTransactionStocks.SourceTable = nameof(TransferStock);
                        FormTransactionStocks.SourcTableId = getInternalTransfer.Id;
                        FormTransactionStocks.ProductId = items.ProductId;
                        FormTransactionStocks.LocationId = getInternalTransfer.SourceId;
                        FormTransactionStocks.Batch = items.Batch;
                        FormTransactionStocks.ExpiredDate = items.ExpiredDate;
                        FormTransactionStocks.Reference = referenceNumber;
                        FormTransactionStocks.Quantity = -(items.QtyStock * Cek_Uom?.BiggerRatio?.ToLong()) ?? 0;
                        FormTransactionStocks.UomId = items.Product?.UomId;
                        FormTransactionStocks.Validate = false;

                        await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStocks));


                        //In

                        FormTransactionStocks.SourceTable = nameof(TransferStock);
                        FormTransactionStocks.SourcTableId = getInternalTransfer.Id;
                        FormTransactionStocks.LocationId = getInternalTransfer.DestinationId;
                        FormTransactionStocks.ProductId = items.ProductId;
                        FormTransactionStocks.Batch = items.Batch;
                        FormTransactionStocks.ExpiredDate = items.ExpiredDate;
                        FormTransactionStocks.Reference = referenceNumber;
                        FormTransactionStocks.Quantity = items.QtyStock * Cek_Uom?.BiggerRatio?.ToLong() ?? 0;
                        FormTransactionStocks.UomId = items.Product?.UomId;
                        FormTransactionStocks.Validate = false;

                        await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStocks));

                    }

                    //Save Status InternalTransfer
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Ready;
                    getInternalTransfer = await Mediator.Send(new UpdateTransferStockRequest(FormInternalTransfer));
                }

                //Save Log
                FormInternalTransferDetail.TransferStockId = getInternalTransfer.Id;
                FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                FormInternalTransferDetail.Status = EnumStatusInternalTransfer.Ready;

                await Mediator.Send(new CreateTransferStockLogRequest(FormInternalTransferDetail));

                await EditItem_Click(getInternalTransfer);
                StateHasChanged();
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
                TransferStocks = await Mediator.Send(new GetTransferStockQuery());
                FormInternalTransfer = TransferStocks.Where(x => x.Id == TransferId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.ApproveRequest;
                    getInternalTransfer = await Mediator.Send(new UpdateTransferStockRequest(FormInternalTransfer));
                }

                //Save Log
                FormInternalTransferDetail.TransferStockId = getInternalTransfer.Id;
                FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                FormInternalTransferDetail.Status = EnumStatusInternalTransfer.ApproveRequest;

                await Mediator.Send(new CreateTransferStockLogRequest(FormInternalTransferDetail));
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

        //    var asyncData = await Mediator.Send(new GetTransferStockProductQuery(x => x.TransferStockId == TransferId));
        //    if (asyncData.Count > 0)
        //    {
        //        foreach (var item in asyncData)
        //        {
        //            var StockSent = asyncData.Where(t => t.TransferStock is not null && t.TransferStock.SourceId == item.TransferStock!.SourceId && t.ProductId == item.ProductId).FirstOrDefault();
        //            var warehouse_stock = StockProducts.Where(sp => sp.SourceId == item?.TransferStock?.SourceId && sp.ProductId == item?.ProductId).FirstOrDefault();
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
        //        var datas = await Mediator.Send(new GetTransferStockQuery());
        //        FormInternalTransfer = datas.Where(x => x.Id == TransferId).FirstOrDefault()!;
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
        //        getInternalTransfer = await Mediator.Send(new UpdateTransferStockRequest(FormInternalTransfer));

        //        FormInternalTransferDetail.TransferStockId = getInternalTransfer.Id;
        //        FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
        //        FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
        //        FormInternalTransferDetail.Status = getInternalTransfer.Status;

        //        await Mediator.Send(new CreateTransferStockLogRequest(FormInternalTransferDetail));

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
                await LoadData();
                await LoadAsyncData();
                FormInternalTransfer = TransferStocks.Where(x => x.Id == TransferId).FirstOrDefault()!;
                var data_TransactionStock = TransactionStocks.Where(x => x.SourceTable == nameof(TransferStock) && x.SourcTableId == TransferId).ToList();
                if (FormInternalTransfer is not null)
                {
                    foreach (var item in data_TransactionStock)
                    {

                        item.Validate = true;
                        var aa = await Mediator.Send(new UpdateTransactionStockRequest(item));
                    }

                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Done;
                    getInternalTransfer = await Mediator.Send(new UpdateTransferStockRequest(FormInternalTransfer));

                    //Save Log
                    FormInternalTransferDetail.TransferStockId = TransferId;
                    FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                    FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                    FormInternalTransferDetail.Status = EnumStatusInternalTransfer.Done;

                    await Mediator.Send(new CreateTransferStockLogRequest(FormInternalTransferDetail));

                    await EditItem_Click(getInternalTransfer);
                    StateHasChanged();
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
                var TransferProductStock = await Mediator.Send(new GetTransferStockProductQuery());
                TransferStocks = await Mediator.Send(new GetTransferStockQuery());
                FormInternalTransfer = TransferStocks.Where(x => x.Id == TransferId).FirstOrDefault()!;
                if (FormInternalTransfer is not null)
                {
                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Cancel;
                    getInternalTransfer = await Mediator.Send(new UpdateTransferStockRequest(FormInternalTransfer));
                }

                //Save Log
                FormInternalTransferDetail.TransferStockId = getInternalTransfer.Id;
                FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                FormInternalTransferDetail.Status = EnumStatusInternalTransfer.Cancel;

                await Mediator.Send(new CreateTransferStockLogRequest(FormInternalTransferDetail));
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
                List<TransferStockDto> Transfers = SelectedDataItems.Adapt<List<TransferStockDto>>();
                List<long> id = Transfers.Select(x => x.Id).ToList();
                List<long> ProductIdsToDelete = new();
                List<long> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //delete data Transfer Stock Product
                    ProductIdsToDelete = TransferStockProducts
                               .Where(x => x.TransferStockId == TransferId)
                               .Select(x => x.Id)
                               .ToList();
                    await Mediator.Send(new DeleteTransferStockProductRequest(ids: ProductIdsToDelete));

                    //Delete data Transfer Detal transfer (Log)

                    DetailsIdsToDelete = TransferStockLogs
                       .Where(x => x.TransferStockId == TransferId)
                       .Select(x => x.Id)
                       .ToList();
                    await Mediator.Send(new DeleteTransferStockLogRequest(ids: DetailsIdsToDelete));

                    //Delete Transfer

                    await Mediator.Send(new DeleteTransferStockRequest(SelectedDataItems[0].Adapt<TransferStockDto>().Id));
                }
                else
                {
                    foreach (var uid in id)
                    {
                        //delete data Transfer Stock Product
                        ProductIdsToDelete = TransferStockProducts
                                   .Where(x => x.TransferStockId == TransferId)
                                   .Select(x => x.Id)
                                   .ToList();
                        await Mediator.Send(new DeleteTransferStockProductRequest(ids: ProductIdsToDelete));

                        //Delete data Transfer Detal transfer (Log)

                        DetailsIdsToDelete = TransferStockLogs
                           .Where(x => x.TransferStockId == TransferId)
                           .Select(x => x.Id)
                           .ToList();
                        await Mediator.Send(new DeleteTransferStockLogRequest(ids: DetailsIdsToDelete));
                    }
                    await Mediator.Send(new DeleteTransferStockRequest(ids: id));
                }
                ToastService.ShowSuccess("Data Deleting success..");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        List<TransferStockProductDto> IdDeleteDetail = new List<TransferStockProductDto>();
        private async Task onDelete_Detail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                StateHasChanged();
                var data = SelectedDataItemsDetail.Adapt<List<TransferStockProductDto>>();
                var cek = data.Select(x => x.TransferStockId).FirstOrDefault();
                if (cek == null)
                {
                    TempTransferStocks.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                }
                else
                {
                    foreach (var item in data)
                    {
                        TempTransferStocks.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
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

            var i = (TransferStockProductDto)e.EditModel;

            if (FormInternalTransfer.Id == 0)
            {
                /*
                 Kesalahan Batch yang di Simpan
                 */
                try
                {
                    TransferStockProductDto Updates = new();

                    if (i.Id == 0)
                    {
                        var a = new TransferStockProductDto();
                        a.Id = Helper.RandomNumber;
                        a.ProductName = i.ProductName;
                        a.QtyStock = i.QtyStock;
                        a.Batch = i.Batch;
                        a.ExpiredDate = i.ExpiredDate;
                        a.UomName = i.UomName;
                        a.ProductId = i.ProductId;
                        a.TransferStockId = i.TransferStockId;
                        TempTransferStocks.Add(a);
                    }
                    else
                    {
                        var q = i;

                        Updates = TempTransferStocks.FirstOrDefault(x => x.Id == q.Id)!;

                        var index = TempTransferStocks.IndexOf(Updates!);
                        TempTransferStocks[index] = i;
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
                i.TransferStockId = FormInternalTransfer.Id;
                if (i.Id == 0)
                    await Mediator.Send(new CreateTransferStockProductRequest(i));
                else
                    await Mediator.Send(new UpdateTransferStockProductRequest(i));

                StateHasChanged();
                await EditItem_Click(null);
            }
        }

        private async Task OnSave()
        {
            try
            {
                if (FormInternalTransfer.SourceId is null)
                {
                    return;
                }

                if (FormInternalTransfer.Id == 0)
                {
                    var sourcname = Locations.Where(x => x.Id == FormInternalTransfer.SourceId).Select(x => x.Name).FirstOrDefault();
                    var getKodeTransaksi = TransferStocks.Where(t => t.SourceId == FormInternalTransfer.SourceId).OrderByDescending(x => x.KodeTransaksi).Select(x => x.KodeTransaksi).FirstOrDefault();

                    if (getKodeTransaksi == null)
                    {
                        var nextTransferNumber = 1;
                        FormInternalTransfer.KodeTransaksi = $"{sourcname}/INT/{nextTransferNumber.ToString("00000")}";
                    }
                    else
                    {
                        var lastTransferNumber = 0;
                        if (getKodeTransaksi.Contains("/INT/"))
                        {
                            var lastTransferNumberStr = getKodeTransaksi.Split('/')[2];
                            int.TryParse(lastTransferNumberStr, out lastTransferNumber);
                        }

                        var nextTransferNumber = lastTransferNumber + 1;
                        FormInternalTransfer.KodeTransaksi = $"{sourcname}/INT/{nextTransferNumber.ToString("00000")}";
                    }

                    FormInternalTransfer.Status = EnumStatusInternalTransfer.Draft;

                    getInternalTransfer = await Mediator.Send(new CreateTransferStockRequest(FormInternalTransfer));
                    TempTransferStocks.ForEach(x =>
                    {
                        x.TransferStockId = getInternalTransfer.Id;
                        x.Id = 0;
                    });
                    await Mediator.Send(new CreateListTransferStockProductRequest(TempTransferStocks));
                    ToastService.ShowSuccess("Add Data Success...");

                    FormInternalTransferDetail.TransferStockId = getInternalTransfer.Id;
                    FormInternalTransferDetail.SourceId = getInternalTransfer.SourceId;
                    FormInternalTransferDetail.DestinationId = getInternalTransfer.DestinationId;
                    FormInternalTransferDetail.Status = EnumStatusInternalTransfer.Draft;

                    await Mediator.Send(new CreateTransferStockLogRequest(FormInternalTransferDetail));
                }
                else
                {
                    getInternalTransfer = await Mediator.Send(new UpdateTransferStockRequest(FormInternalTransfer));
                    if (IdDeleteDetail.Count > 0)
                    {
                        foreach (var id in IdDeleteDetail)
                        {
                            var cek = TransferStockLogs.Where(z => z.Id == id.Id).FirstOrDefault();
                            await Mediator.Send(new DeleteTransferStockProductRequest(id.Id));
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
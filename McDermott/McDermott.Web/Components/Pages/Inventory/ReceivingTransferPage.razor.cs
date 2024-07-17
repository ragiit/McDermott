using static McDermott.Application.Features.Commands.Inventory.ReceivingCommand;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Inventory.TransferStockCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class ReceivingTransferPage
    {
        #region Relation Data

        private List<ReceivingStockDto> ReceivingStocks = [];
        private List<TransactionStockDto> TransactionStocks = [];
        private List<ReceivingStockProductDto> receivingStockDetails = [];
        private List<ReceivingStockProductDto> TempReceivingStockDetails = [];
        private List<ReceivingLogDto> ReceivingLogs = [];
        private List<LocationDto> Locations = [];
        private List<ProductDto> Products = [];
        private List<StockProductDto> Stocks = [];
        private List<UomDto> Uoms = [];
        private List<TransferStockDetailDto> AllLogs = [];
        private List<ReceivingLogDto> Logs = [];
        private UserDto NameUser = new();

        private ReceivingStockProductDto FormReceivingDetailStock = new();
        private ReceivingStockProductDto TempFormReceivingStockDetail = new();
        private ReceivingStockDto GetReceivingStock = new();
        private StockProductDto FormStockProduct = new();
        private ReceivingStockDto FormReceivingStocks = new();
        private ReceivingLogDto FormReceivingLog = new();
        private TransactionStockDto FormTransactionStock = new();

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

                receivingStockDetails = await Mediator.Send(new GetReceivingStockProductQuery());
                Locations = await Mediator.Send(new GetLocationQuery());
                Products = await Mediator.Send(new GetProductQuery());
                Uoms = await Mediator.Send(new GetUomQuery());
                Stocks = await Mediator.Send(new GetStockProductQuery());
                var user_group = await Mediator.Send(new GetUserQuery());
                NameUser = user_group.FirstOrDefault(x => x.GroupId == UserAccessCRUID.GroupId && x.Id == UserLogin.Id) ?? new();
                StateHasChanged();

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

        #region async Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            showForm = false;
            ReceivingStocks = await Mediator.Send(new GetReceivingStockQuery());
            PanelVisible = false;
        }

        private async Task LoadAsyncData()
        {
            PanelVisible = true;
            TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
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

        private async Task LoadLogs()
        {
            Logs = await Mediator.Send(new GetReceivingLogQuery(x => x.ReceivingId == FormReceivingStocks.Id));
        }

        private async Task CheckBatch(string value)
        {
            var checkData = TransactionStocks.Where(x => x.Batch == value).FirstOrDefault();

            if (checkData != null)
            {
                TempFormReceivingStockDetail.Batch = value;
                TempFormReceivingStockDetail.ExpiredDate = checkData.ExpiredDate;
            }
            else
            {
                TempFormReceivingStockDetail.Batch = value;
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

        public MarkupString GetIssueStatusIconHtml(EnumStatusReceiving? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusReceiving.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumStatusReceiving.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusReceiving.Cancel:
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

                isActiveButton = ((ReceivingStockDto)args.DataItem)!.Status!.Equals(EnumStatusReceiving.Draft);
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
            try
            {
                showForm = true;
                PanelVisible = true;
                header = "Edit Data";
                FormReceivingStocks = p ?? SelectedDataItems[0].Adapt<ReceivingStockDto>();

                isActiveButton = FormReceivingStocks.Status == EnumStatusReceiving.Draft;

                receivingId = FormReceivingStocks.Id;

                receivingStockDetails = await Mediator.Send(new GetReceivingStockProductQuery(x => x.ReceivingStockId == FormReceivingStocks.Id));
                TempReceivingStockDetails = receivingStockDetails.Select(x => x).ToList();

                foreach (var item in TempReceivingStockDetails)
                {
                    var product = Products.FirstOrDefault(x => x.Id == item.ProductId);
                    if (product != null)
                    {
                        item.UomName = Uoms.FirstOrDefault(u => u.Id == product.UomId)?.Name;
                        item.PurchaseName = Uoms.FirstOrDefault(p => p.Id == product.PurchaseUomId)?.Name;
                    }
                }

                await LoadLogs();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
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
            var data_TransactionStock = new TransactionStockDto();
            List<TransactionStockDto> Tempdata_TransactionStock = new List<TransactionStockDto>();

            if (FormReceivingStocks is not null)
            {
                FormReceivingStocks.Status = EnumStatusReceiving.Done;
                GetReceivingStock = await Mediator.Send(new UpdateReceivingStockRequest(FormReceivingStocks));

                //ReferenceKode
                var cekReference = TransactionStocks.OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                int NextReferenceNumber = 1;
                if (cekReference != null)
                {
                    int.TryParse(cekReference?.Substring("RCV#".Length), out NextReferenceNumber);
                    NextReferenceNumber++;
                }

                string referenceNumber = $"RCV#{NextReferenceNumber:D3}";

                var CheckReceivedProduct = receivedProductStock.Where(x => x.ReceivingStockId == GetReceivingStock.Id).ToList()!;
                foreach (var a in CheckReceivedProduct)
                {
                    var Cek_Uom = Uoms.Where(x => x.Id == a.Product.UomId).FirstOrDefault();

                    var x = Uoms.Where(x => x.Id == a?.Product?.PurchaseUomId).FirstOrDefault();

                    FormTransactionStock.SourcTableId = FormReceivingStocks.Id;
                    FormTransactionStock.ProductId = a.ProductId;
                    FormTransactionStock.Batch = a.Batch;
                    FormTransactionStock.ExpiredDate = a.ExpiredDate;
                    FormTransactionStock.Reference = referenceNumber;
                    FormTransactionStock.Quantity = a.Qty * Cek_Uom.BiggerRatio.ToLong();
                    FormTransactionStock.DestinationId = GetReceivingStock.DestinationId;
                    FormTransactionStock.UomId = a.Product.UomId;
                    FormTransactionStock.Validate = true;

                    var data_save = await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));
                    Tempdata_TransactionStock.Add(data_save);
                }

                //Save Stock Product
                await LoadAsyncData();
                foreach (var data_item in Tempdata_TransactionStock)
                {
                    var cek_data_stockProduct = Stocks.Where(x => x.ProductId == data_item.ProductId && x.DestinanceId == data_item.DestinationId && x.Batch == data_item.Batch).FirstOrDefault();
                    var inStock = TransactionStocks.Where(x => x.ProductId == data_item.ProductId && x.DestinationId == data_item.DestinationId && x.Batch == data_item.Batch).Sum(x => x.Quantity);

                    FormStockProduct.ProductId = data_item.ProductId;
                    FormStockProduct.DestinanceId = data_item.DestinationId;
                    FormStockProduct.Batch = data_item.Batch;
                    FormStockProduct.Expired = data_item.ExpiredDate;
                    FormStockProduct.UomId = data_item.UomId;
                    FormStockProduct.Qty = inStock;
                    if (cek_data_stockProduct == null)
                    {
                        await Mediator.Send(new CreateStockProductRequest(FormStockProduct));
                    }
                    else
                    {
                        FormStockProduct.Id = cek_data_stockProduct.Id;
                        var cree= await Mediator.Send(new UpdateStockProductRequest(FormStockProduct));
                    }
                }
                //Save Log..

                FormReceivingLog.SourceId = GetReceivingStock.DestinationId;
                FormReceivingLog.UserById = NameUser.Id;
                FormReceivingLog.ReceivingId = GetReceivingStock.Id;
                FormReceivingLog.Status = GetReceivingStock.Status;

                await Mediator.Send(new CreateReceivingLogRequest(FormReceivingLog));

                isActiveButton = false;
            }
            StateHasChanged();
            await LoadData();
        }

        private async Task onCancel()
        {
            var receivedStock = await Mediator.Send(new GetReceivingStockQuery());
            var receivedProductStock = await Mediator.Send(new GetReceivingStockProductQuery());
            FormReceivingStocks = receivedStock.Where(x => x.Id == receivingId).FirstOrDefault()!;

            if (FormReceivingStocks is not null)
            {
                FormReceivingStocks.Status = EnumStatusReceiving.Cancel;
                GetReceivingStock = await Mediator.Send(new UpdateReceivingStockRequest(FormReceivingStocks));
            }

            //Save Log
            FormReceivingLog.SourceId = GetReceivingStock.DestinationId;
            FormReceivingLog.UserById = NameUser.Id;
            FormReceivingLog.ReceivingId = GetReceivingStock.Id;
            FormReceivingLog.Status = GetReceivingStock.Status;

            await Mediator.Send(new CreateReceivingLogRequest(FormReceivingLog));
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

                    //Delete Data Transfer Detail (log)

                    DetailsIdsToDelete = ReceivingLogs
                        .Where(x => x.ReceivingId == receivingId)
                        .Select(x => x.Id)
                        .ToList();
                    await Mediator.Send(new DeleteTransferStockDetailRequest(ids: DetailsIdsToDelete));

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

                        //Delete Data Transfer Detail (log)

                        DetailsIdsToDelete = ReceivingLogs
                            .Where(x => x.ReceivingId == Uid)
                            .Select(x => x.Id)
                            .ToList();
                        await Mediator.Send(new DeleteTransferStockDetailRequest(ids: DetailsIdsToDelete));
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
                        var nextTransferNumber = 1;
                        FormReceivingStocks.KodeReceiving = $"WH-IN/{nextTransferNumber.ToString("0000")}";
                    }
                    else
                    {
                        var lastTransferNumber = 0;
                        if (getReceiving.Contains("WH-IN/"))
                        {
                            var lastTransferNumberStr = getReceiving.Split('/')[1];
                            int.TryParse(lastTransferNumberStr, out lastTransferNumber);
                        }
                        var nextTransferNumber = lastTransferNumber + 1;
                        FormReceivingStocks.KodeReceiving = $"WH-IN/{nextTransferNumber.ToString("0000")}";
                    }

                    FormReceivingStocks.Status = EnumStatusReceiving.Draft;

                    GetReceivingStock = await Mediator.Send(new CreateReceivingStockRequest(FormReceivingStocks));

                    TempReceivingStockDetails.ForEach(x =>
                    {
                        x.ReceivingStockId = GetReceivingStock.Id;
                        x.Id = 0;
                    });

                    await Mediator.Send(new CreateListReceivingStockProductRequest(TempReceivingStockDetails));
                    ToastService.ShowSuccess("Add Data Success...");

                    FormReceivingLog.SourceId = GetReceivingStock.DestinationId;
                    FormReceivingLog.UserById = NameUser.Id;
                    FormReceivingLog.ReceivingId = GetReceivingStock.Id;
                    FormReceivingLog.Status = EnumStatusReceiving.Draft;

                    await Mediator.Send(new CreateReceivingLogRequest(FormReceivingLog));
                }
                else
                {
                    GetReceivingStock = await Mediator.Send(new UpdateReceivingStockRequest(FormReceivingStocks));
                    ToastService.ShowSuccess("Update Data Success...");
                }

                ToastService.ClearSuccessToasts();
                StateHasChanged();
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
                        r.Batch = r.Batch;
                        TempReceivingStockDetails.Add(r);
                    }
                    else
                    {
                        var q = r;

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
                {
                    await Mediator.Send(new CreateReceivingStockProductRequest(r));
                }
                else
                {
                    var sdsds = await Mediator.Send(new UpdateReceivingStockProductRequest(r));
                    TempReceivingStockDetails.Clear();
                }
                await EditItem_Click(null);
            }
        }

        #endregion Function Save
    }
}
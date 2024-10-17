using DocumentFormat.OpenXml.Bibliography;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.GoodsReceiptCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;


namespace McDermott.Web.Components.Pages.Inventory.GoodsReceipt
{
    public partial class GoodsReceiptPage
    {
        #region Relation Data

        private List<GoodsReceiptDto> getGoodsReceipts = [];
        private List<TransactionStockDto> TransactionStocks = [];
        private List<GoodsReceiptLogDto> GoodsReceiptLogs = [];
        
        private UserDto NameUser = new();
        
        private GoodsReceiptDto GetGoodsReceipt = new();
        private StockProductDto FormStockProduct = new();
        private GoodsReceiptDto postGoodsReceipts = new();
        private GoodsReceiptLogDto FormGoodsReceiptLog = new();
        private TransactionStockDto FormTransactionStock = new();

        #endregion Relation Data

        #region Variable Static

        private IGrid? Grid { get; set; }
        private IGrid? GridProduct { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool IsAddReceived { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private string? header { get; set; }
        private long? receivingId { get; set; }
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
            var result = await Mediator.Send(new GetGoodsReceiptQuery
            {
                OrderByList = [
                    (x=>x.Status == EnumStatusReceiving.Draft, true)
                    ],
                SearchTerm = searchTerm,
                PageSize = pageSize,
                PageIndex = pageIndex,
            });
            getGoodsReceipts = result.Item1;
            totalCount = result.PageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }


        //private async Task LoadData_Detail()
        //{
        //    PanelVisible = true;
        //    SelectedDataItemsDetail = [];
        //    GoodsReceiptDetails = await Mediator.Send(new GetGoodsReceiptDetailQuery());
        //    PanelVisible = false;
        //}

        //private void SelectedChangeProduct(ProductDto e)
        //{
        //    try
        //    {
        //        ResetFormProductDetail();
        //        if (e is null)
        //            return;

        //        TempFormGoodsReceiptDetail.TraceAbility = e.TraceAbility;

        //        if (e is not null)
        //        {
        //            var productName = Products.Where(p => p.Id == e.Id).FirstOrDefault()!;
        //            var uomName = Uoms.Where(u => u.Id == e.UomId).Select(x => x.Name).FirstOrDefault();
        //            var purchaseName = Uoms.Where(u => u.Id == e.PurchaseUomId).Select(x => x.Name).FirstOrDefault()!;
        //            TempFormGoodsReceiptDetail.PurchaseName = purchaseName;
        //            TempFormGoodsReceiptDetail.UomName = uomName;
        //            TempFormGoodsReceiptDetail.ProductName = productName.Name;
        //            TempFormGoodsReceiptDetail.TraceAbility = productName.TraceAbility;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.HandleException(ToastService);
        //    }
        //}

        //private void ResetFormProductDetail()
        //{
        //    TempFormGoodsReceiptDetail.PurchaseName = null;
        //    TempFormGoodsReceiptDetail.UomName = null;
        //    TempFormGoodsReceiptDetail.ProductName = null;
        //    TempFormGoodsReceiptDetail.TraceAbility = false;
        //}

        //private async Task LoadLogs()
        //{
        //    Logs = await Mediator.Send(new GetGoodsReceiptLogQuery(x => x.ReceivingId == FormGoodsReceipts.Id));
        //}

        //private async Task CheckBatch(string value)
        //{
        //    var checkData = TransactionStocks.Where(x => x.Batch == value).FirstOrDefault();

        //    if (checkData != null)
        //    {
        //        TempFormGoodsReceiptDetail.Batch = value;
        //        TempFormGoodsReceiptDetail.ExpiredDate = checkData.ExpiredDate;
        //    }
        //    else
        //    {
        //        TempFormGoodsReceiptDetail.Batch = value;
        //    }
        //}

        //private async Task HandleValidSubmit()
        //{
        //    FormValidationState = true;
        //    if (FormValidationState)
        //    {
        //        await OnSave();
        //    }
        //    else
        //    {
        //        FormValidationState = true;
        //    }
        //}

        //private async Task HandleInvalidSubmit()
        //{
            
        //    FormValidationState = false;
        //}

        

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

                case EnumStatusReceiving.Process:
                    priorityClass = "warning";
                    title = "Process";
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
                if ((GoodsReceiptDto)args.DataItem is null)
                    return;

                isActiveButton = ((GoodsReceiptDto)args.DataItem)!.Status!.Equals(EnumStatusReceiving.Draft);
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

        private async Task NewItem_Click()
        {
            NavigationManager.NavigateTo($"inventory/goods-receipts{EnumPageMode.Create.GetDisplayName()}");
            
        }

        private async Task EditItem_Click()
        {
            var Id = SelectedDataItems[0].Adapt<GoodsReceiptDto>();
            NavigationManager.NavigateTo($"inventory/goods-receipts{EnumPageMode.Update.GetDisplayName()}?Id={Id}");

            //try
            //{

            //    PanelVisible = true;
            //    header = "Edit Data";

            //    // Ensure SelectedDataItems is not null or empty before accessing

            //    if (p != null)
            //    {
            //        FormGoodsReceipts = p;
            //    }
            //    else if (SelectedDataItems.Count > 0)
            //    {
            //        FormGoodsReceipts = SelectedDataItems[0].Adapt<GoodsReceiptDto>();
            //    }
            //    else
            //    {
            //        // Handle the case where SelectedDataItems is empty
            //        ToastService.ShowWarning("No item selected for editing.");
            //        return;
            //    }

            //    // Set the form's receiving stock
            //    //FormGoodsReceipts = p ?? SelectedDataItems[0].Adapt<GoodsReceiptDto>();
            //    receivingId = FormGoodsReceipts.Id;
            //    GetGoodsReceipt = FormGoodsReceipts;

            //    // Pre-load Uoms and Products (if not already loaded)
            //    if (Uoms == null || Uoms.Count == 0)
            //    {
            //        //Uoms = await Mediator.Send(new GetUomQuery());
            //    }

            //    if (Products == null || Products.Count == 0)
            //    {
            //        //Products = await Mediator.Send(new GetProductQuery());
            //    }

            //    // Filter and update receiving stock details
            //    GoodsReceiptDetails = await Mediator.Send(new GetGoodsReceiptDetailQuery(x => x.GoodsReceiptId == FormGoodsReceipts.Id));
            //    TempGoodsReceiptDetails = GoodsReceiptDetails.ToList();

            //    await UpdateProductDetailsAsync(TempGoodsReceiptDetails, FormGoodsReceipts.DestinationId);

            //    // Load logs
            //    await LoadLogs().ConfigureAwait(false);

            //    // Set panel visibility to false
            //    PanelVisible = false;
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions
            //    ex.HandleException(ToastService);
            //}
        }

        //private async Task UpdateProductDetailsAsync(List<GoodsReceiptDetailDto> items, long? sourceLocationId)
        //{
        //    foreach (var item in items)
        //    {
        //        var product = Products.FirstOrDefault(x => x.Id == item.ProductId);
        //        item.UomName = Uoms.FirstOrDefault(u => u.Id == product?.UomId)?.Name;
        //        item.PurchaseName = Uoms.FirstOrDefault(u => u.Id == product?.PurchaseUomId)?.Name;

        //        var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == item.ProductId && s.LocationId == sourceLocationId && s.SourceTable == nameof(TransferStock)));
        //        var stockProduct = stockProducts.FirstOrDefault();

        //        if (item.Product?.TraceAbility == true)
        //        {
        //            //item.Batch = stockProduct?.Batch;
        //            //item.ExpiredDate = stockProduct?.ExpiredDate;
        //        }
        //        else
        //        {
        //            item.Batch = "-";
        //            item.ExpiredDate = null;
        //        }
        //    }
        //}

        private async Task DeleteItem_Click()
        {
            Grid!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        //Grid Detail
        private async Task NewItemDetail_Click()
        {
           
            await GridProduct!.StartEditNewRowAsync();
        }

        private async Task EditItemDetail_Click(IGrid context)
        {
            await GridProduct.StartEditRowAsync(FocusedRowVisibleIndex);
            
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

        #region Process

        private async Task onProcess()
        {
            //PanelVisible = true;
            //await LoadAsyncData();
            //FormGoodsReceipts = GoodsReceipts.Where(x => x.Id == receivingId).FirstOrDefault()!;

            //if (FormGoodsReceipts is not null)
            //{
            //    //ReferenceKode
            //    var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(GoodsReceipt)).OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
            //    int NextReferenceNumber = 1;
            //    if (cekReference != null)
            //    {
            //        int.TryParse(cekReference?.Substring("RCV#".Length), out NextReferenceNumber);
            //        NextReferenceNumber++;
            //    }

            //    string referenceNumber = $"RCV#{NextReferenceNumber:D3}";

            //    var CheckReceivedProduct = GoodsReceiptDetails.Where(x => x.GoodsReceiptId == GetGoodsReceipt.Id).ToList()!;
            //    foreach (var a in CheckReceivedProduct)
            //    {
            //        var Cek_Uom = Uoms.Where(x => x.Id == a.Product?.UomId).FirstOrDefault();

            //        var x = Uoms.Where(x => x.Id == a?.Product?.PurchaseUomId).FirstOrDefault();

            //        FormTransactionStock.SourceTable = nameof(GoodsReceipt);
            //        FormTransactionStock.SourcTableId = FormGoodsReceipts.Id;
            //        FormTransactionStock.ProductId = a.ProductId;
            //        FormTransactionStock.Batch = a.Batch;
            //        FormTransactionStock.ExpiredDate = a.ExpiredDate;
            //        FormTransactionStock.Reference = referenceNumber;
            //        FormTransactionStock.Quantity = a.Qty * Cek_Uom?.BiggerRatio?.ToLong() ?? 0;
            //        FormTransactionStock.LocationId = GetGoodsReceipt.DestinationId;
            //        FormTransactionStock.UomId = a.Product?.UomId;
            //        FormTransactionStock.Validate = false;

            //        await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));
            //    }

            //    //UpdateReceiving Stock
            //    FormGoodsReceipts.Status = EnumStatusReceiving.Process;
            //    GetGoodsReceipt = await Mediator.Send(new UpdateGoodsReceiptRequest(FormGoodsReceipts));

            //    //Save Log..

            //    FormGoodsReceiptLog.SourceId = FormGoodsReceipts.DestinationId;
            //    FormGoodsReceiptLog.UserById = NameUser.Id;
            //    FormGoodsReceiptLog.ReceivingId = FormGoodsReceipts.Id;
            //    FormGoodsReceiptLog.Status = EnumStatusReceiving.Process;

            //    await Mediator.Send(new CreateGoodsReceiptLogRequest(FormGoodsReceiptLog));

            //    PanelVisible = true;

            //    await EditItem_Click(GetGoodsReceipt);
            //    StateHasChanged();
            //}
        }

        #endregion Process

        #region Validation

        private async Task onValidation()
        {
            //PanelVisible = true;
            //await LoadAsyncData();
            //FormGoodsReceipts = GoodsReceipts.Where(x => x.Id == receivingId).FirstOrDefault()!;

            //var data_TransactionStock = TransactionStocks.Where(x => x.SourceTable == nameof(GoodsReceipt) && x.SourcTableId == receivingId).ToList();

            //foreach (var item in data_TransactionStock)
            //{
            //    item.Validate = true;
            //    var aa = await Mediator.Send(new UpdateTransactionStockRequest(item));
            //}

            ////UpdateReceiving Stock
            //FormGoodsReceipts.Status = EnumStatusReceiving.Done;
            //GetGoodsReceipt = await Mediator.Send(new UpdateGoodsReceiptRequest(FormGoodsReceipts));

            ////Save Log..

            //FormGoodsReceiptLog.SourceId = FormGoodsReceipts.DestinationId;
            //FormGoodsReceiptLog.UserById = NameUser.Id;
            //FormGoodsReceiptLog.ReceivingId = FormGoodsReceipts.Id;
            //FormGoodsReceiptLog.Status = EnumStatusReceiving.Done;

            //await Mediator.Send(new CreateGoodsReceiptLogRequest(FormGoodsReceiptLog));

            //isActiveButton = false;
            //await EditItem_Click(GetGoodsReceipt);
            //StateHasChanged();
            //PanelVisible = false;
        }

        private async Task onCancel()
        {
            //var receivedStock = await Mediator.Send(new GetGoodsReceiptQuery());
            //var receivedProductStock = await Mediator.Send(new GetGoodsReceiptDetailQuery());
            //FormGoodsReceipts = receivedStock.Where(x => x.Id == receivingId).FirstOrDefault()!;

            //if (FormGoodsReceipts is not null)
            //{
            //    FormGoodsReceipts.Status = EnumStatusReceiving.Cancel;
            //    GetGoodsReceipt = await Mediator.Send(new UpdateGoodsReceiptRequest(FormGoodsReceipts));
            //}

            ////Update Receiving Stock

            ////Save Log
            //FormGoodsReceiptLog.SourceId = GetGoodsReceipt.DestinationId;
            //FormGoodsReceiptLog.UserById = NameUser.Id;
            //FormGoodsReceiptLog.ReceivingId = GetGoodsReceipt.Id;
            //FormGoodsReceiptLog.Status = GetGoodsReceipt.Status;

            //await Mediator.Send(new CreateGoodsReceiptLogRequest(FormGoodsReceiptLog));
        }

        #endregion Validation

        #region Function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
              
                List<GoodsReceiptDto> receivings = SelectedDataItems.Adapt<List<GoodsReceiptDto>>();
                long id = SelectedDataItems[0].Adapt<GoodsReceiptDto>().Id;
                List<long> ids = receivings.Select(x => x.Id).ToList();
                List<long> ProductIdsToDelete = new();
                List<long> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //Delete Data Receiving stock Product

                    ProductIdsToDelete = GoodsReceiptDetails
                        .Where(x => x.GoodsReceiptId == id)
                        .Select(x => x.Id)
                        .ToList();
                    await Mediator.Send(new DeleteGoodsReceiptPoductRequest(ids: ProductIdsToDelete));

                    //Delete Data Transfer Detail (log)

                    DetailsIdsToDelete = GoodsReceiptLogs
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
                        ProductIdsToDelete = GoodsReceiptDetails
                       .Where(x => x.GoodsReceiptId == Uid)
                       .Select(x => x.Id)
                       .ToList();
                        await Mediator.Send(new DeleteGoodsReceiptPoductRequest(ids: ProductIdsToDelete));

                        //Delete Data Transfer Detail (log)

                        DetailsIdsToDelete = GoodsReceiptLogs
                            .Where(x => x.GoodsReceiptId == Uid)
                            .Select(x => x.Id)
                            .ToList();
                        await Mediator.Send(new DeleteGoodsReceiptPoductRequest(ids: DetailsIdsToDelete));
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

        //private async Task OnDelete_Detail(GridDataItemDeletingEventArgs e)
        //{
        //    try
        //    {
        //        StateHasChanged();
        //        var data = SelectedDataItemsDetail.Adapt<List<GoodsReceiptDetailDto>>();
        //        var cek = data.Select(x => x.GoodsReceiptId).FirstOrDefault();
        //        if (cek == null)
        //        {
        //            TempGoodsReceiptDetails.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
        //        }
        //        else
        //        {
        //            foreach (var item in data)
        //            {
        //                TempGoodsReceiptDetails.RemoveAll(x => x.Id == item.Id);
        //                await Mediator.Send(new DeleteGoodsReceiptPoductRequest(item.Id));
        //            }
        //        }
        //        SelectedDataItemsDetail = new ObservableRangeCollection<object>();
        //        StateHasChanged();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.HandleException(ToastService);
        //    }
        //}

        #endregion Function Delete

        #region Function Save

        private async Task OnSave()
        {
            try
            {
                if (TempGoodsReceiptDetails.Count <= 0)

                    return;

                if (FormGoodsReceipts.Id == 0)
                {
                    var getReceiving = getGoodsReceipts.Where(r => r.DestinationId == FormGoodsReceipts.DestinationId).OrderByDescending(x => x.ReceiptNumber).Select(x => x.ReceiptNumber).FirstOrDefault();
                    if (getReceiving == null)
                    {
                        var nextTransferNumber = 1;
                        FormGoodsReceipts.ReceiptNumber = $"WH-IN/{nextTransferNumber.ToString("0000")}";
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
                        FormGoodsReceipts.ReceiptNumber = $"WH-IN/{nextTransferNumber.ToString("0000")}";
                    }

                    FormGoodsReceipts.Status = EnumStatusReceiving.Draft;

                    GetGoodsReceipt = await Mediator.Send(new CreateGoodsReceiptRequest(FormGoodsReceipts));

                    TempGoodsReceiptDetails.ForEach(x =>
                    {
                        x.GoodsReceiptId = GetGoodsReceipt.Id;
                        x.Id = 0;
                    });

                    await Mediator.Send(new CreateListGoodsReceiptDetailRequest(TempGoodsReceiptDetails));
                    ToastService.ShowSuccess("Add Data Success...");

                    FormGoodsReceiptLog.SourceId = GetGoodsReceipt.DestinationId;
                    FormGoodsReceiptLog.UserById = NameUser.Id;
                    FormGoodsReceiptLog.GoodsReceiptId = GetGoodsReceipt.Id;
                    FormGoodsReceiptLog.Status = EnumStatusReceiving.Draft;

                    await Mediator.Send(new CreateGoodsReceiptLogRequest(FormGoodsReceiptLog));
                }
                else
                {
                    GetGoodsReceipt = await Mediator.Send(new UpdateGoodsReceiptRequest(FormGoodsReceipts));
                    ToastService.ShowSuccess("Update Data Success...");
                }

                ToastService.ClearSuccessToasts();
                await EditItem_Click(GetGoodsReceipt);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        //private async Task OnSave_Detail(GridEditModelSavingEventArgs e)
        //{
        //    if (e is null)
        //        return;

        //    var r = (GoodsReceiptDetailDto)e.EditModel;

        //    if (FormGoodsReceipts.Id == 0)
        //    {
        //        try
        //        {
        //            if (r.TraceAbility == true)
        //            {
        //                if (r.Batch == null || r.ExpiredDate == null)
        //                {
        //                    ToastService.ShowInfo("Batch And Expired Date not null");
        //                }
        //            }

        //            GoodsReceiptDetailDto updates = new();

        //            if (r.Id == 0)
        //            {
        //                r.Id = Helper.RandomNumber;
        //                r.Batch = r.Batch;
        //                TempGoodsReceiptDetails.Add(r);
        //            }
        //            else
        //            {
        //                var q = r;

        //                updates = TempGoodsReceiptDetails.FirstOrDefault(x => x.Id == q.Id)!;

        //                var index = TempGoodsReceiptDetails.IndexOf(updates!);
        //                TempGoodsReceiptDetails[index] = r;
        //            }
        //            SelectedDataItemsDetail = [];
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.HandleException(ToastService);
        //        }
        //    }
        //    else
        //    {
        //        r.GoodsReceiptId = FormGoodsReceipts.Id;
        //        if (r.Id == 0)
        //            await Mediator.Send(new CreateGoodsReceiptDetailRequest(r));
        //        else
        //            await Mediator.Send(new UpdateGoodsReceiptDetailRequest(r));
        //        await EditItem_Click(GetGoodsReceipt);
        //        StateHasChanged();
        //    }
        //}

        #endregion Function Save
    }
}
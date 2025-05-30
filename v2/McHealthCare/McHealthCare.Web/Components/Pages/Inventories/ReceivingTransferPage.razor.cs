﻿using DevExpress.Data.XtraReports.Native;

namespace McHealthCare.Web.Components.Pages.Inventories
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
        private List<TransferStockLogDto> AllLogs = [];
        private List<ReceivingLogDto> Logs = [];
        private ApplicationUserDto NameUser = new();

        private ReceivingStockProductDto FormReceivingDetailStock = new();
        private ReceivingStockProductDto TempFormReceivingStockDetail = new();
        private ReceivingStockDto GetReceivingStock = new();
        private StockProductDto FormStockProduct = new();
        private ReceivingStockDto FormReceivingStocks = new();
        private ReceivingLogDto FormReceivingLog = new();
        private TransactionStockDto FormTransactionStock = new();

        #endregion Relation Data

        #region Variable Static

        public bool IsAccess { get; set; } = true;
        private IGrid? Grid { get; set; }
        private IGrid? GridProduct { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool IsAddReceived { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private string? header { get; set; }
        private Guid? receivingId { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsDetail { get; set; } = new ObservableRangeCollection<object>();

        #endregion Variable Static

        #region async Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            PanelVisible = false;
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            showForm = false;
            await LoadAsyncData();
            PanelVisible = false;
        }

        private async Task LoadAsyncData()
        {
            ReceivingStocks = await Mediator.Send(new GetReceivingStockQuery());
            TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
            Locations = await Mediator.Send(new GetLocationQuery());
            Products = await Mediator.Send(new GetProductQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            receivingStockDetails = await Mediator.Send(new GetReceivingStockProductQuery());
            ReceivingLogs = await Mediator.Send(new GetReceivingLogQuery());
        }

        private async Task LoadData_Detail()
        {
            PanelVisible = true;
            SelectedDataItemsDetail = [];
            receivingStockDetails = await Mediator.Send(new GetReceivingStockProductQuery());
            PanelVisible = false;
        }

        private void SelectedChangeProduct(ProductDto e)
        {
            try
            {
                ResetFormProductDetail();
                if (e is null)
                    return;

                TempFormReceivingStockDetail.TraceAbility = e.TraceAbility;

                if (e is not null)
                {
                    var productName = Products.Where(p => p.Id == e.Id).FirstOrDefault()!;
                    var uomName = Uoms.Where(u => u.Id == e.UomId).Select(x => x.Name).FirstOrDefault();
                    var purchaseName = Uoms.Where(u => u.Id == e.PurchaseUomId).Select(x => x.Name).FirstOrDefault()!;
                    TempFormReceivingStockDetail.PurchaseName = purchaseName;
                    TempFormReceivingStockDetail.UomName = uomName;
                    TempFormReceivingStockDetail.ProductName = productName.Name;
                    TempFormReceivingStockDetail.TraceAbility = productName.TraceAbility;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void ResetFormProductDetail()
        {
            TempFormReceivingStockDetail.PurchaseName = null;
            TempFormReceivingStockDetail.UomName = null;
            TempFormReceivingStockDetail.ProductName = null;
            TempFormReceivingStockDetail.TraceAbility = false;
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

        public class StatusComparer : IComparer<EnumStatusReceiving>
        {
            private static readonly List<EnumStatusReceiving> StatusOrder = new List<EnumStatusReceiving> { EnumStatusReceiving.Draft, EnumStatusReceiving.Process, EnumStatusReceiving.Done, EnumStatusReceiving.Cancel };

            public int Compare(EnumStatusReceiving x, EnumStatusReceiving y)
            {
                int indexX = StatusOrder.IndexOf(x);
                int indexY = StatusOrder.IndexOf(y);

                // Compare the indices
                return indexX.CompareTo(indexY);
            }
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

                // Ensure SelectedDataItems is not null or empty before accessing

                if (p != null)
                {
                    FormReceivingStocks = p;
                }
                else if (SelectedDataItems.Count > 0)
                {
                    FormReceivingStocks = SelectedDataItems[0].Adapt<ReceivingStockDto>();
                }
                else
                {
                    // Handle the case where SelectedDataItems is empty
                    ToastService.ShowWarning("No item selected for editing.");
                    return;
                }

                // Set the form's receiving stock
                //FormReceivingStocks = p ?? SelectedDataItems[0].Adapt<ReceivingStockDto>();
                receivingId = FormReceivingStocks.Id;
                GetReceivingStock = FormReceivingStocks;

                // Pre-load Uoms and Products (if not already loaded)
                if (Uoms == null || Uoms.Count == 0)
                {
                    Uoms = await Mediator.Send(new GetUomQuery());
                }

                if (Products == null || Products.Count == 0)
                {
                    Products = await Mediator.Send(new GetProductQuery());
                }

                // Filter and update receiving stock details
                receivingStockDetails = await Mediator.Send(new GetReceivingStockProductQuery(x => x.ReceivingStockId == FormReceivingStocks.Id));
                TempReceivingStockDetails = receivingStockDetails.ToList();

                await UpdateProductDetailsAsync(TempReceivingStockDetails, FormReceivingStocks.DestinationId);

                // Load logs
                await LoadLogs().ConfigureAwait(false);

                // Set panel visibility to false
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ex.HandleException(ToastService);
            }
        }

        private async Task UpdateProductDetailsAsync(List<ReceivingStockProductDto> items, Guid? sourceLocationId)
        {
            foreach (var item in items)
            {
                var product = Products.FirstOrDefault(x => x.Id == item.ProductId);
                item.UomName = Uoms.FirstOrDefault(u => u.Id == product?.UomId)?.Name;
                item.PurchaseName = Uoms.FirstOrDefault(u => u.Id == product?.PurchaseUomId)?.Name;

                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == item.ProductId && s.LocationId == sourceLocationId && s.SourceTable == nameof(TransferStock)));
                var stockProduct = stockProducts.FirstOrDefault();

                if (item.Product?.TraceAbility == true)
                {
                    //item.Batch = stockProduct?.Batch;
                    //item.ExpiredDate = stockProduct?.ExpiredDate;
                }
                else
                {
                    item.Batch = "-";
                    item.ExpiredDate = null;
                }
            }
        }

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
            IsAddReceived = true;
            TempFormReceivingStockDetail = new();
            await GridProduct!.StartEditNewRowAsync();
        }

        private async Task EditItemDetail_Click(IGrid context)
        {
            await GridProduct.StartEditRowAsync(FocusedRowVisibleIndex);
            var traceavibility = context.SelectedDataItem.Adapt<ReceivingStockProductDto>();
            if (traceavibility?.Product?.TraceAbility == true)
            {
                TempFormReceivingStockDetail.TraceAbility = true;
            }
            StateHasChanged();
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

        //private string GetSourceTableName()
        //{
        //    var tableName = typeof(ReceivingStockDto)
        //        .GetCustomAttribute<TableAttribute>()
        //        ?.Name;
        //    return tableName ?? "ReceivingStocks";
        //}

        #region Process

        private async Task onProcess()
        {
            PanelVisible = true;
            await LoadAsyncData();
            FormReceivingStocks = ReceivingStocks.Where(x => x.Id == receivingId).FirstOrDefault()!;

            if (FormReceivingStocks is not null)
            {
                //ReferenceKode
                var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(ReceivingStock)).OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                int NextReferenceNumber = 1;
                if (cekReference != null)
                {
                    int.TryParse(cekReference?.Substring("RCV#".Length), out NextReferenceNumber);
                    NextReferenceNumber++;
                }

                string referenceNumber = $"RCV#{NextReferenceNumber:D3}";

                var CheckReceivedProduct = receivingStockDetails.Where(x => x.ReceivingStockId == GetReceivingStock.Id).ToList()!;
                foreach (var a in CheckReceivedProduct)
                {
                    var Cek_Uom = Uoms.Where(x => x.Id == a.Product?.UomId).FirstOrDefault();

                    var x = Uoms.Where(x => x.Id == a?.Product?.PurchaseUomId).FirstOrDefault();

                    FormTransactionStock.SourceTable = nameof(ReceivingStock);
                    FormTransactionStock.SourcTableId = FormReceivingStocks.Id;
                    FormTransactionStock.ProductId = a.ProductId;
                    FormTransactionStock.Batch = a.Batch;
                    FormTransactionStock.ExpiredDate = a.ExpiredDate;
                    FormTransactionStock.Reference = referenceNumber;
                    FormTransactionStock.Quantity = a.Qty * Convert.ToInt64(Cek_Uom?.BiggerRatio);
                    FormTransactionStock.LocationId = GetReceivingStock.DestinationId;
                    FormTransactionStock.UomId = a.Product?.UomId;
                    FormTransactionStock.Validate = false;

                    await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));
                }

                //UpdateReceiving Stock
                FormReceivingStocks.Status = EnumStatusReceiving.Process;
                GetReceivingStock = await Mediator.Send(new UpdateReceivingStockRequest(FormReceivingStocks));

                //Save Log..

                FormReceivingLog.SourceId = FormReceivingStocks.DestinationId;
                FormReceivingLog.UserById = NameUser.Id;
                FormReceivingLog.ReceivingId = FormReceivingStocks.Id;
                FormReceivingLog.Status = EnumStatusReceiving.Process;

                await Mediator.Send(new CreateReceivingLogRequest(FormReceivingLog));

                PanelVisible = true;

                await EditItem_Click(GetReceivingStock);
                StateHasChanged();
            }
        }

        #endregion Process

        #region Validation

        private async Task onValidation()
        {
            PanelVisible = true;
            await LoadAsyncData();
            FormReceivingStocks = ReceivingStocks.Where(x => x.Id == receivingId).FirstOrDefault()!;

            var data_TransactionStock = TransactionStocks.Where(x => x.SourceTable == nameof(ReceivingStock) && x.SourcTableId == receivingId).ToList();

            foreach (var item in data_TransactionStock)
            {
                item.Validate = true;
                var aa = await Mediator.Send(new UpdateTransactionStockRequest(item));
            }

            //UpdateReceiving Stock
            FormReceivingStocks.Status = EnumStatusReceiving.Done;
            GetReceivingStock = await Mediator.Send(new UpdateReceivingStockRequest(FormReceivingStocks));

            //Save Log..

            FormReceivingLog.SourceId = FormReceivingStocks.DestinationId;
            FormReceivingLog.UserById = NameUser.Id;
            FormReceivingLog.ReceivingId = FormReceivingStocks.Id;
            FormReceivingLog.Status = EnumStatusReceiving.Done;

            await Mediator.Send(new CreateReceivingLogRequest(FormReceivingLog));

            isActiveButton = false;
            await EditItem_Click(GetReceivingStock);
            StateHasChanged();
            PanelVisible = false;
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

            //Update Receiving Stock

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
                await LoadAsyncData();
                List<ReceivingStockDto> receivings = SelectedDataItems.Adapt<List<ReceivingStockDto>>();
                Guid id = SelectedDataItems[0].Adapt<ReceivingStockDto>().Id;
                List<Guid> ids = receivings.Select(x => x.Id).ToList();
                List<Guid> ProductIdsToDelete = new();
                List<Guid> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //Delete Data Receiving stock Product

                    ProductIdsToDelete = receivingStockDetails
                        .Where(x => x.ReceivingStockId == id)
                        .Select(x => x.Id)
                        .ToList();
                    await Mediator.Send(new DeleteReceivingStockPoductRequest(ids: ProductIdsToDelete));

                    //Delete Data Transfer Detail (log)

                    DetailsIdsToDelete = ReceivingLogs
                        .Where(x => x.ReceivingId == id)
                        .Select(x => x.Id)
                        .ToList();
                    await Mediator.Send(new DeleteReceivingLogRequest(ids: DetailsIdsToDelete));

                    //Delete Receiving Stock

                    await Mediator.Send(new DeleteReceivingStockRequest(SelectedDataItems[0].Adapt<ReceivingStockDto>().Id));
                }
                else
                {
                    foreach (var Uid in ids)
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
                        await Mediator.Send(new DeleteTransferStockLogRequest(ids: DetailsIdsToDelete));
                    }
                    //Delete list Id ReceivingStock
                    await Mediator.Send(new DeleteReceivingStockRequest(ids: ids));
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
                    foreach (var item in data)
                    {
                        TempReceivingStockDetails.RemoveAll(x => x.Id == item.Id);
                        await Mediator.Send(new DeleteReceivingStockPoductRequest(item.Id));
                    }
                }
                SelectedDataItemsDetail = new ObservableRangeCollection<object>();
                StateHasChanged();
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
                if (TempReceivingStockDetails.Count <= 0)

                    return;

                if (FormReceivingStocks.Id == Guid.Empty)
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
                        x.Id = Guid.Empty;
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
                await EditItem_Click(GetReceivingStock);
                StateHasChanged();
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

            if (FormReceivingStocks.Id == Guid.Empty)
            {
                try
                {
                    if (r.TraceAbility == true)
                    {
                        if (r.Batch == null || r.ExpiredDate == null)
                        {
                            return;
                        }
                    }

                    ReceivingStockProductDto updates = new();

                    if (r.Id == Guid.Empty)
                    {
                        r.Id = Guid.NewGuid();
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
                if (r.Id == Guid.Empty)
                    await Mediator.Send(new CreateReceivingStockProductRequest(r));
                else
                    await Mediator.Send(new UpdateReceivingStockProductRequest(r));
                await EditItem_Click(GetReceivingStock);
                StateHasChanged();
            }
        }

        #endregion Function Save
    }
}
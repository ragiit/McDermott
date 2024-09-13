using DevExpress.Blazor.Internal;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class InventoryAdjusmentPage
    {
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

        public IGrid Grid { get; set; }
        public IGrid GridDetail { get; set; }

        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDetailDataItems { get; set; } = [];

        private string StagingText { get; set; } = "Start Inventory";
        private bool PanelVisible { get; set; } = true;
        private bool ShowConfirmation { get; set; } = false;
        private bool FormValidationState { get; set; } = true;
        private bool ActiveButton { get; set; } = false;
        private bool ShowForm { get; set; } = false;

        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowDetailVisibleIndex { get; set; }

        private InventoryAdjusmentDto InventoryAdjusment { get; set; } = new();

        private List<InventoryAdjusmentDto> InventoryAdjusments { get; set; } = [];
        private List<InventoryAdjusmentDetailDto> InventoryAdjusmentDetails { get; set; } = [];
        private List<LocationDto> Locations { get; set; } = [];
        private List<CompanyDto> Companies { get; set; } = [];
        private List<ProductDto> AllProducts { get; set; } = [];
        private List<ProductDto> Products { get; set; } = [];
        private List<UomDto> Uoms { get; set; } = [];

        private bool IsStatus(EnumStatusInventoryAdjustment status) => InventoryAdjusment.Status == status;

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            var Locations = (await Mediator.Send(new GetLocationQuery())).Item1;
            this.Locations = Locations;
            var Companies = (await Mediator.Send(new GetCompanyQuery())).Item1;
            this.Companies = Companies;
            Uoms = await Mediator.Send(new GetUomQuery());
            Products = await Mediator.Send(new GetProductQuery());
            AllProducts = Products.Select(x => x).ToList();

            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            ShowForm = false;
            SelectedDetailDataItems = [];
            SelectedDataItems = [];
            InventoryAdjusments = await Mediator.Send(new GetInventoryAdjusmentQuery());
            PanelVisible = false;
        }

        #endregion LoadData

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    long adjId = ((InventoryAdjusmentDto)e.DataItem).Id;

                    var detailIds = (await Mediator.Send(new GetInventoryAdjusmentDetailQuery(x => x.InventoryAdjusmentId == adjId))).Select(x => x.Id).ToList();
                    var stockIds = (await Mediator.Send(new GetTransactionStockQuery(x => detailIds.Contains(x.SourcTableId.GetValueOrDefault()) && x.SourceTable == nameof(InventoryAdjusment)))).Select(x => x.Id).ToList();

                    await Mediator.Send(new DeleteTransactionStockRequest(ids: stockIds));
                    await Mediator.Send(new DeleteInventoryAdjusmentRequest(((InventoryAdjusmentDto)e.DataItem).Id));
                }
                else
                {
                    var ids = SelectedDataItems.Adapt<List<InventoryAdjusmentDto>>().Select(x => x.Id).ToList();

                    foreach (var adjId in ids)
                    {
                        var detailIds = (await Mediator.Send(new GetInventoryAdjusmentDetailQuery(x => x.InventoryAdjusmentId == adjId))).Select(x => x.Id).ToList();
                        var stockIds = (await Mediator.Send(new GetTransactionStockQuery(x => detailIds.Contains(x.SourcTableId.GetValueOrDefault()) && x.SourceTable == nameof(InventoryAdjusment)))).Select(x => x.Id).ToList();

                        await Mediator.Send(new DeleteTransactionStockRequest(ids: stockIds));
                    }

                    await Mediator.Send(new DeleteInventoryAdjusmentRequest(ids: ids));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private void NewItem_Click()
        {
            ShowForm = true;
            InventoryAdjusment = new();

            if (Companies.Count > 0)
                InventoryAdjusment.CompanyId = Companies[0].Id;

            InventoryAdjusmentDetails = [];
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private bool IsDeletedAdjusment { get; set; } = false;

        private void GridDetail_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowDetailVisibleIndex = args.VisibleIndex;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;

            try
            {
                if (args.DataItem is null)
                    return;

                IsDeletedAdjusment = ((InventoryAdjusmentDto)args.DataItem)!.Status != EnumStatusInventoryAdjustment.Invalidate;
            }
            catch { }
        }

        private async Task EditItem_Click()
        {
            ShowForm = true;

            var inventoryAdjusment = await Mediator.Send(new GetInventoryAdjusmentQuery(x => x.Id == SelectedDataItems[0].Adapt<InventoryAdjusmentDto>().Id));

            if (inventoryAdjusment.Count > 0)
            {
                InventoryAdjusment = inventoryAdjusment[0];

                switch (InventoryAdjusment.Status)
                {
                    case EnumStatusInventoryAdjustment.Draft:
                        StagingText = "Start Inventory";
                        break;

                    case EnumStatusInventoryAdjustment.InProgress:
                        StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();
                        break;

                    case EnumStatusInventoryAdjustment.Invalidate:
                        StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();
                        break;

                    default:
                        break;
                }

                await LoadInventoryAdjustmentDetails();

                //InventoryAdjusmentDetails = await Mediator.Send(new GetInventoryAdjusmentDetailQuery(x => x.InventoryAdjusmentId == InventoryAdjusment.Id));
                //InventoryAdjusmentDetails.ForEach(async e =>
                //{
                //    var StockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id));

                //    TotalQty = StockProducts.Sum(x => x.Qty) ?? 0;

                //    UomId = e.UomId ?? null;

                //    if (e.Product is not null && e.Product.TraceAbility)
                //    {
                //        LotSerialNumber = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId)?.Batch ?? "-";
                //        ExpiredDate = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId)?.Expired;
                //    }
                //    else
                //    {
                //        LotSerialNumber = "-";
                //        ExpiredDate = null;
                //    }
                //});
            }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task RefreshDetails_Click()
        {
            await LoadInventoryAdjustmentDetails();
        }

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await SaveInventoryAdjusment();
            else
                FormValidationState = true;
        }

        private async Task SaveInventoryAdjusment()
        {
            try
            {
                IsLoadingConfirm = true;
                if (!FormValidationState && InventoryAdjusmentDetails.Count == 0)
                {
                    ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                    return;
                }

                if (InventoryAdjusment.Id == 0)
                {
                    InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Draft;
                    StagingText = "Start Inventory";

                    var trx = await Mediator.Send(new GetTransactionStockQuery(x => x.SourceTable == nameof(InventoryAdjusment) && x.SourcTableId == InventoryAdjusment.Id));
                    // Get the last reference code
                    var cekReference = (await Mediator.Send(new GetTransactionStockQuery(x => x.SourceTable == nameof(InventoryAdjusment))))
                                        .OrderByDescending(x => x.SourcTableId)
                                        .Select(x => x.Reference)
                                        .FirstOrDefault();
                    int NextReferenceNumber = 1;
                    if (cekReference != null)
                    {
                        int.TryParse(cekReference?["ADJ#".Length..], out NextReferenceNumber);
                        NextReferenceNumber++;
                    }

                    InventoryAdjusment.Reference = $"ADJ#{NextReferenceNumber:D3}";
                    InventoryAdjusment = await Mediator.Send(new CreateInventoryAdjusmentRequest(InventoryAdjusment));
                    InventoryAdjusmentDetails.ForEach(x =>
                    {
                        x.Id = 0;
                        x.InventoryAdjusmentId = InventoryAdjusment.Id;
                    });

                    InventoryAdjusmentDetails = await Mediator.Send(new CreateListInventoryAdjusmentDetailRequest(InventoryAdjusmentDetails));
                    InventoryAdjusmentDetails.ForEach(x =>
                    {
                        x.Product = Products.FirstOrDefault(z => z.Id == x.ProductId);
                        x.UomId = x.Product?.UomId;
                    });
                }
                else
                {
                    InventoryAdjusment = await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
                }

                //await LoadData();
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
            }
            finally
            {
                IsLoadingConfirm = false;
            }
        }

        private async Task SelectLocation(LocationDto e)
        {
            if (e is null)
            {
                Products.Clear();
                return;
            }

            var st = await Mediator.Send(new GetTransactionStockQuery(x => x.LocationId == e.Id));

            Products = AllProducts.Where(x => st.Select(s => s.ProductId).Contains(x.Id)).ToList();
        }

        private async Task OnCancelStatus()
        {
            PanelVisible = true;
            InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Cancel;
            StagingText = EnumStatusInventoryAdjustment.Cancel.GetDisplayName();

            InventoryAdjusment = await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
            PanelVisible = false;
        }

        private async Task SaveInventoryAdjusmentDetail(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (e is null)
                    return;

                var IsBatch = Products.FirstOrDefault(x => x.Id == FormInventoryAdjusmentDetail.ProductId)?.TraceAbility ?? false;
                if (IsBatch && string.IsNullOrWhiteSpace(FormInventoryAdjusmentDetail.Batch))
                {
                    ToastService.ShowInfo("Please Select or Insert Batch Number");
                }
                else
                {
                    var inventoryAdjusmentDetail = FormInventoryAdjusmentDetail;
                    var inventoryAdjusmentDetailA = (InventoryAdjusmentDetailDto)e.EditModel;

                    if (InventoryAdjusment.Id == 0)
                    {
                        try
                        {
                            InventoryAdjusmentDetailDto update = new();

                            if (inventoryAdjusmentDetail.Id == 0)
                            {
                                inventoryAdjusmentDetail.Id = Helper.RandomNumber;
                                inventoryAdjusmentDetail.Product = Products.FirstOrDefault(x => x.Id == inventoryAdjusmentDetail.ProductId);

                                InventoryAdjusmentDetails.Add(inventoryAdjusmentDetail);
                            }
                            else
                            {
                                var q = SelectedDetailDataItems[0].Adapt<InventoryAdjusmentDetailDto>();

                                update = InventoryAdjusmentDetails.FirstOrDefault(x => x.Id == q.Id)!;
                                inventoryAdjusmentDetail.Product = Products.FirstOrDefault(x => x.Id == inventoryAdjusmentDetail.ProductId);

                                var index = InventoryAdjusmentDetails.IndexOf(update!);
                                InventoryAdjusmentDetails[index] = inventoryAdjusmentDetail;
                            }

                            SelectedDetailDataItems = [];
                        }
                        catch (Exception ex)
                        {
                            ex.HandleException(ToastService);
                        }
                    }
                    else
                    {
                        inventoryAdjusmentDetail.InventoryAdjusmentId = InventoryAdjusment.Id;
                        if (inventoryAdjusmentDetail.Id == 0)
                        {
                            inventoryAdjusmentDetail = await Mediator.Send(new CreateInventoryAdjusmentDetailRequest(inventoryAdjusmentDetail));
                            await Mediator.Send(new CreateTransactionStockRequest(new TransactionStockDto
                            {
                                Reference = InventoryAdjusment.Reference,
                                Batch = inventoryAdjusmentDetail.Batch,
                                ExpiredDate = inventoryAdjusmentDetail.ExpiredDate,
                                LocationId = InventoryAdjusment.LocationId,
                                Quantity = inventoryAdjusmentDetail.Difference,
                                UomId = FormInventoryAdjusmentDetail.UomId,
                                Validate = false,
                                SourceTable = nameof(InventoryAdjusment),
                                SourcTableId = inventoryAdjusmentDetail.Id,
                                ProductId = inventoryAdjusmentDetail.ProductId,
                            }));
                            await LoadInventoryAdjustmentDetails();
                        }
                        else
                        {
                            await Mediator.Send(new UpdateInventoryAdjusmentDetailRequest(inventoryAdjusmentDetail));

                            var updtStock = (await Mediator.Send(new GetTransactionStockQuery(x => x.SourceTable == nameof(InventoryAdjusment) && x.SourcTableId == inventoryAdjusmentDetail.Id))).FirstOrDefault() ?? new();
                            updtStock.Quantity = inventoryAdjusmentDetail.Difference;

                            await Mediator.Send(new UpdateTransactionStockRequest(updtStock));
                            await LoadInventoryAdjustmentDetails();
                        }

                        //if (inventoryAdjusmentDetail.Difference != 0)
                        //{
                        //    // Map InventoryAdjusmentDetail to TransactionStockDto using Mapster
                        //    var transactionStockDto = inventoryAdjusmentDetail.Adapt<TransactionStockDto>();
                        //    transactionStockDto.LocationId = InventoryAdjusment.LocationId;
                        //    transactionStockDto.Validate = false;
                        //    transactionStockDto.Quantity = inventoryAdjusmentDetail.Difference;
                        //    transactionStockDto.SourcTableId = inventoryAdjusmentDetail.InventoryAdjusmentId;
                        //    transactionStockDto.SourceTable = nameof(InventoryAdjusment);

                        //    await Mediator.Send(new CreateTransactionStockRequest(transactionStockDto));
                        //}
                    }

                    await LoadInventoryAdjustmentDetails();
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnDeleteInventoryAdjusmentDetail(GridDataItemDeletingEventArgs e)
        {
            if (InventoryAdjusment.Id == 0)
            {
                try
                {
                    if (SelectedDetailDataItems is null || SelectedDetailDataItems.Count == 1)
                    {
                        InventoryAdjusmentDetails.Remove((InventoryAdjusmentDetailDto)e.DataItem);
                    }
                    else
                    {
                        SelectedDetailDataItems.Adapt<List<InventoryAdjusmentDetailDto>>().Select(x => x.Id).ToList().ForEach(x =>
                        {
                            InventoryAdjusmentDetails.Remove(InventoryAdjusmentDetails.FirstOrDefault(z => z.Id == x));
                        });
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                try
                {
                    if (SelectedDetailDataItems is null || SelectedDetailDataItems.Count == 1)
                    {
                        var id = (await Mediator.Send(new GetTransactionStockQuery(x => x.SourcTableId == ((InventoryAdjusmentDetailDto)e.DataItem).Id && x.SourceTable == nameof(InventoryAdjusment)))).FirstOrDefault() ?? new();
                        await Mediator.Send(new DeleteTransactionStockRequest(id.Id));
                        await Mediator.Send(new DeleteInventoryAdjusmentDetailRequest(((InventoryAdjusmentDetailDto)e.DataItem).Id));
                    }
                    else
                    {
                        var a = SelectedDetailDataItems.Adapt<List<InventoryAdjusmentDetailDto>>();

                        foreach (var id in a)
                        {
                            var idd = (await Mediator.Send(new GetTransactionStockQuery(x => x.SourcTableId == ((InventoryAdjusmentDto)e.DataItem).Id && x.SourceTable == nameof(InventoryAdjusment)))).FirstOrDefault() ?? new();
                            await Mediator.Send(new DeleteTransactionStockRequest(idd.Id));
                        }

                        await Mediator.Send(new DeleteInventoryAdjusmentDetailRequest(ids: a.Select(x => x.Id).ToList()));
                    }
                    SelectedDetailDataItems = [];
                    await LoadInventoryAdjustmentDetails();
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        private async Task LoadInventoryAdjustmentDetails()
        {
            PanelVisible = true;
            InventoryAdjusmentDetails = await Mediator.Send(new GetInventoryAdjusmentDetailQuery(x => x.InventoryAdjusmentId == InventoryAdjusment.Id));

            //var tasks = InventoryAdjusmentDetails.Select(async x =>
            //{
            //    var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>  s.ProductId == x.ProductId && s.SourceId == InventoryAdjusment.LocationId));

            //    if (stockProducts.Any())
            //    {
            //        x.TeoriticalQty = stockProducts.FirstOrDefault()?.Qty ?? 0;

            //        if (x.Product != null)
            //        {
            //            x.UomId = x.Product.UomId;
            //            x.Product.Uom = Uoms.FirstOrDefault(z => z.Id == x.Product.UomId);

            //            if (x.Product.TraceAbility)
            //            {
            //                var stockProduct = stockProducts.FirstOrDefault(sp => sp.SourceId == InventoryAdjusment.LocationId);
            //                if (stockProduct != null)
            //                {
            //                    x.LotSerialNumber = stockProduct.Batch ?? "-";
            //                    x.ExpiredDate = stockProduct.Expired;
            //                }
            //                else
            //                {
            //                    x.LotSerialNumber = "-";
            //                    x.ExpiredDate = null;
            //                }
            //            }
            //            else
            //            {
            //                x.LotSerialNumber = "-";
            //                x.ExpiredDate = null;
            //            }
            //        }
            //        else
            //        {
            //            x.UomId = null;
            //            x.LotSerialNumber = "-";
            //            x.ExpiredDate = null;
            //        }
            //    }
            //});

            //await Task.WhenAll(tasks);

            PanelVisible = false;
        }

        private async Task NewItemDetail_Click()
        {
            if (InventoryAdjusment.LocationId is null || InventoryAdjusment.LocationId == 0)
            {
                ToastService.ClearInfoToasts();
                ToastService.ShowInfo("Please select the Location first.");
                return;
            }

            Products = await Mediator.Send(new GetProductQuery());
            AllProducts = Products.Select(x => x).ToList();

            FormInventoryAdjusmentDetail = new();
            TotalQty = 0;
            LotSerialNumber = "-";
            UomId = null;

            await GridDetail.StartEditNewRowAsync();
        }

        private async Task EditItemDetail_Click(IGrid context)
        {
            if (!IsStatus(EnumStatusInventoryAdjustment.InProgress))
                return;

            await GridDetail.StartEditRowAsync(FocusedRowDetailVisibleIndex);

            var aa = Uoms;
            var bb = Batch;

            // Ensure the context is not null and has selected data item
            if (context.SelectedDataItem != null)
            {
                FormInventoryAdjusmentDetail = (await Mediator.Send(new GetInventoryAdjusmentDetailQuery(x => x.Id == context.SelectedDataItem.Adapt<InventoryAdjusmentDetailDto>().Id))).FirstOrDefault() ?? new();

                // Check if Product and StockProduct are not null before accessing their properties
                if (FormInventoryAdjusmentDetail.Product != null)
                {
                    FormInventoryAdjusmentDetail.UomId = FormInventoryAdjusmentDetail.Product.UomId;
                }

                if (FormInventoryAdjusmentDetail.StockProduct != null)
                {
                    FormInventoryAdjusmentDetail.TransactionStockId = FormInventoryAdjusmentDetail.StockProduct.Id;
                    FormInventoryAdjusmentDetail.TeoriticalQty = FormInventoryAdjusmentDetail.TransactionStock?.Quantity ?? 0L;
                }

                StateHasChanged();
            }
        }

        private void DeleteItemDetail_Click()
        {
            GridDetail.ShowRowDeleteConfirmation(FocusedRowDetailVisibleIndex);
        }

        private async Task CancelItem_Click()
        {
            await LoadData();
        }

        private bool IsLoadingConfirm { get; set; } = false;
        private bool YesConfirm { get; set; } = false;

        private async Task OnConfirmYes()
        {
            YesConfirm = true;
            await OnClickConfirm(true);
        }

        private async Task OnConfirmNo()
        {
            ShowConfirmation = false;
        }

        private async Task OnClickConfirm(bool? confirm = false)
        {
            IsLoadingConfirm = true;

            //switch (StagingText)
            //{
            //    case "Start Inventory":
            //        InventoryAdjusment.Status = EnumStatusInventoryAdjustment.InProgress;
            //        StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();
            //        await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
            //        break;

            //    case "In-Progress":
            //        InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
            //        StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();
            //        await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
            //        break;

            //    case "Invalidate":
            //        InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
            //        await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

            //        foreach (var x in InventoryAdjusmentDetails)
            //        {
            //            var stockProduct = (await Mediator.Send(new GetTransactionStockQuery(s => s.Id == x.TransactionStockId)))
            //                                               .FirstOrDefault();

            //            if (stockProduct != null)
            //            {
            //                x.TeoriticalQty = x.RealQty;
            //                stockProduct.Qty = x.RealQty;
            //                await Mediator.Send(new UpdateStockProductRequest(stockProduct));
            //            }

            //            await Mediator.Send(new UpdateInventoryAdjusmentDetailRequest(x));
            //        }

            //        await LoadInventoryAdjustmentDetails();

            //        break;

            //    default:
            //        break;
            //}
            try
            {
                if ((InventoryAdjusmentDetails == null || InventoryAdjusmentDetails.Count == 0) && IsStatus(EnumStatusInventoryAdjustment.InProgress))
                {
                    ToastService.ShowInfo("Please add at least one inventory adjustment detail before proceeding.");
                    return;
                }

                if (InventoryAdjusmentDetails.Count > 0 && InventoryAdjusmentDetails.Any(x => x.RealQty == 0))
                {
                    if (YesConfirm)
                        ShowConfirmation = false;
                    else
                        ShowConfirmation = true;
                }
                else
                    YesConfirm = true;

                if ((!ShowConfirmation && YesConfirm) || IsStatus(EnumStatusInventoryAdjustment.Draft))
                {
                    switch (StagingText)
                    {
                        case "Start Inventory":
                            InventoryAdjusment.Status = EnumStatusInventoryAdjustment.InProgress;
                            StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();

                            await SelectLocation(new LocationDto { Id = InventoryAdjusment.LocationId.GetValueOrDefault(), });

                            await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

                            var temps = new List<InventoryAdjusmentDetailDto>();
                            foreach (var o in Products)
                            {
                                var sp = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == o.Id && s.LocationId == InventoryAdjusment.LocationId && s.Validate == true));

                                if (o.TraceAbility)
                                {
                                    var allBatch = sp.Select(x => x.Batch);
                                    allBatch = allBatch.Distinct().ToList();
                                    foreach (var b in allBatch)
                                    {
                                        var spb = sp.Where(x => x.ProductId == o.Id && x.Batch != null && x.Batch == b).FirstOrDefault() ?? new();

                                        temps.Add(new InventoryAdjusmentDetailDto
                                        {
                                            InventoryAdjusmentId = InventoryAdjusment.Id,
                                            ProductId = o.Id,
                                            ExpiredDate = spb.ExpiredDate,
                                            UomId = o.UomId,
                                            TeoriticalQty = sp.Where(x => x.Batch == b).Sum(x => x.Quantity),
                                            Batch = b,
                                            RealQty = 0
                                        });
                                    }
                                }
                                else
                                {
                                    temps.Add(new InventoryAdjusmentDetailDto
                                    {
                                        InventoryAdjusmentId = InventoryAdjusment.Id,
                                        ProductId = o.Id,
                                        ExpiredDate = sp.FirstOrDefault()?.ExpiredDate,
                                        TeoriticalQty = sp.Sum(x => x.Quantity),
                                        Batch = null,
                                        RealQty = 0
                                    });
                                }
                            }

                            temps = await Mediator.Send(new CreateListInventoryAdjusmentDetailRequest(temps));

                            var list = new List<TransactionStockDto>();
                            foreach (var item in temps)
                            {
                                list.Add(new TransactionStockDto
                                {
                                    SourceTable = nameof(InventoryAdjusment),
                                    SourcTableId = item.Id,
                                    ProductId = item.ProductId,
                                    Reference = InventoryAdjusment.Reference,
                                    Batch = item.Batch,
                                    ExpiredDate = item.ExpiredDate,
                                    Quantity = item.Difference,
                                    LocationId = InventoryAdjusment.LocationId,
                                    UomId = Products.FirstOrDefault(x => item.ProductId == x.Id)?.UomId,
                                    Validate = false
                                });
                            }

                            await Mediator.Send(new CreateListTransactionStockRequest(list));
                            await LoadInventoryAdjustmentDetails();
                            //if (inventoryAdjusmentDetail.Difference != 0)
                            //{
                            //    // Map InventoryAdjusmentDetail to TransactionStockDto using Mapster
                            //    var transactionStockDto = inventoryAdjusmentDetail.Adapt<TransactionStockDto>();
                            //    transactionStockDto.LocationId = InventoryAdjusment.LocationId;
                            //    transactionStockDto.Validate = false;
                            //    transactionStockDto.Quantity = inventoryAdjusmentDetail.Difference;
                            //    transactionStockDto.SourcTableId = inventoryAdjusmentDetail.InventoryAdjusmentId;
                            //    transactionStockDto.SourceTable = nameof(InventoryAdjusment);

                            //    await Mediator.Send(new CreateTransactionStockRequest(transactionStockDto));
                            //}

                            break;

                        case "In-Progress":
                            InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
                            StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();

                            await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

                            if (StagingText == EnumStatusInventoryAdjustment.Invalidate.GetDisplayName())
                            {
                                var update = await Mediator.Send(new GetTransactionStockQuery(x => x.Reference == InventoryAdjusment.Reference));
                                foreach (var item in update)
                                {
                                    item.Validate = true;
                                }

                                await Mediator.Send(new UpdateListTransactionStockRequest(update));

                                return;
                                var stockProductsToUpdate = new List<TransactionStockDto>();
                                var adjustmentDetailsToUpdate = new List<InventoryAdjusmentDetailDto>();

                                foreach (var detail in InventoryAdjusmentDetails)
                                {
                                    var stockProduct = (await Mediator.Send(new GetTransactionStockQuery(s => s.Id == detail.TransactionStockId))).FirstOrDefault();

                                    if (stockProduct != null)
                                    {
                                        //detail.TeoriticalQty = detail.RealQty;
                                        //stockProduct.Qty = detail.RealQty;
                                        //stockProductsToUpdate.Add(stockProduct);
                                    }

                                    adjustmentDetailsToUpdate.Add(detail);
                                }

                                //foreach (var stockProduct in stockProductsToUpdate)
                                //{
                                //    await Mediator.Send(new UpdateStockProductRequest(stockProduct));
                                //}

                                //foreach (var detail in adjustmentDetailsToUpdate)
                                //{
                                //    await Mediator.Send(new UpdateInventoryAdjusmentDetailRequest(detail));
                                //}

                                //await Mediator.Send(new UpdateListStockProductRequest(stockProductsToUpdate));
                                //await Mediator.Send(new UpdateListTransactionStockRequest());
                                //await Mediator.Send(new UpdateListInventoryAdjusmentDetailRequest(adjustmentDetailsToUpdate));

                                await LoadInventoryAdjustmentDetails();
                            }
                            break;

                        case "Invalidate":
                            InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
                            await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

                            if (StagingText == EnumStatusInventoryAdjustment.Invalidate.GetDisplayName())
                            {
                                var update = await Mediator.Send(new GetTransactionStockQuery(x => x.Reference == InventoryAdjusment.Reference));
                                foreach (var item in update)
                                {
                                    item.Validate = true;
                                }

                                await Mediator.Send(new UpdateListTransactionStockRequest(update));
                            }
                            break;

                        default:
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                YesConfirm = false;
                IsLoadingConfirm = false;
            }
        }

        private long TotalQty { get; set; } = 0;
        private long? UomId { get; set; } = 0;
        private string LotSerialNumber { get; set; } = "-";
        private DateTime? ExpiredDate { get; set; }

        //private List<TransactionStockDto> Batch = [];
        private List<string> Batch = [];

        private DateTime? SelectedBatchExpired { get; set; }

        private async Task SelectedBatch(string stockProduct)
        {
            // Reset FormInventoryAdjustmentDetail fields to default values
            FormInventoryAdjusmentDetail.TransactionStockId = null;
            FormInventoryAdjusmentDetail.UomId = null;
            FormInventoryAdjusmentDetail.ExpiredDate = null;
            FormInventoryAdjusmentDetail.TeoriticalQty = 0;

            // Return if stockProduct is null
            if (stockProduct is null)
            {
                return;
            }

            // Assign stockProduct to Batch
            FormInventoryAdjusmentDetail.Batch = stockProduct;

            // Proceed only if ProductId is not null
            if (FormInventoryAdjusmentDetail.ProductId is not null)
            {
                // Retrieve stock products matching the given criteria
                //var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                //    s.ProductId == FormInventoryAdjusmentDetail.ProductId &&
                //    s.LocationId == InventoryAdjusment.LocationId &&
                //    s.Validate == true
                //));

                //// Find the first matching product
                //var matchedProduct = stockProducts.FirstOrDefault(x =>
                //    x.LocationId == InventoryAdjusment.LocationId &&
                //    x.ProductId == FormInventoryAdjusmentDetail.ProductId &&
                //    x.Batch == FormInventoryAdjusmentDetail.Batch
                //);

                //// Set UomId and ExpiredDate from the matched product
                //FormInventoryAdjusmentDetail.UomId = matchedProduct?.UomId;
                //FormInventoryAdjusmentDetail.ExpiredDate = matchedProduct?.ExpiredDate;

                //// Retrieve all valid stock products for the given ProductId and LocationId
                //var validProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                //    s.ProductId == FormInventoryAdjusmentDetail.ProductId &&
                //    s.Validate == true &&
                //    s.LocationId == InventoryAdjusment.LocationId
                //));

                //// Retrieve stock products with matching batch and validation
                //var batchProducts = await Mediator.Send(new GetTransactionStockQuery(x =>
                //    x.ProductId == FormInventoryAdjusmentDetail.ProductId &&
                //    x.Batch != null &&
                //    x.Batch == stockProduct &&
                //    x.Validate == true
                //));

                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == FormInventoryAdjusmentDetail.ProductId &&
                    s.LocationId == InventoryAdjusment.LocationId &&
                    s.Validate == true
                ));

                // Find the first matching product
                var matchedProduct = stockProducts.FirstOrDefault(x =>
                    x.LocationId == InventoryAdjusment.LocationId &&
                    x.ProductId == FormInventoryAdjusmentDetail.ProductId &&
                    x.Batch == FormInventoryAdjusmentDetail.Batch
                );

                // Set UomId and ExpiredDate from the matched product
                FormInventoryAdjusmentDetail.UomId = matchedProduct?.UomId;
                FormInventoryAdjusmentDetail.ExpiredDate = matchedProduct?.ExpiredDate;

                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == FormInventoryAdjusmentDetail.ProductId
                && x.LocationId == InventoryAdjusment.LocationId && x.Batch == FormInventoryAdjusmentDetail.Batch));

                // Calculate the sum of quantities for batch products
                FormInventoryAdjusmentDetail.TeoriticalQty = aa.Sum(x => x.Quantity);
            }

            //FormInventoryAdjusmentDetail.TransactionStockId = null;
            //FormInventoryAdjusmentDetail.UomId = null;
            //FormInventoryAdjusmentDetail.ExpiredDate = null;
            //FormInventoryAdjusmentDetail.TeoriticalQty = 0;

            //if (stockProduct is null)
            //{
            //    return;
            //}

            //FormInventoryAdjusmentDetail.Batch = stockProduct;

            //if (FormInventoryAdjusmentDetail.ProductId is not null)
            //{
            //    var StockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == FormInventoryAdjusmentDetail.ProductId && s.DestinationId == InventoryAdjusment.LocationId && s.Validate == true));
            //    FormInventoryAdjusmentDetail.UomId = StockProducts.FirstOrDefault(x => x.DestinationId == InventoryAdjusment.LocationId && x.ProductId == FormInventoryAdjusmentDetail.ProductId && x.Batch == FormInventoryAdjusmentDetail.Batch)?.UomId ?? null;
            //    FormInventoryAdjusmentDetail.ExpiredDate = StockProducts.FirstOrDefault(x => x.DestinationId == InventoryAdjusment.LocationId && x.ProductId == FormInventoryAdjusmentDetail.ProductId && x.Batch == FormInventoryAdjusmentDetail.Batch)?.ExpiredDate;

            //    var stockProducts2 = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == FormInventoryAdjusmentDetail.ProductId && s.Validate == true && s.DestinationId == InventoryAdjusment.LocationId));

            //    var s = await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == FormInventoryAdjusmentDetail.ProductId && x.Batch != null && x.Batch == stockProduct && x.Validate == true));
            //    var firstStockProduct = stockProducts2.FirstOrDefault(x => x.Batch == FormInventoryAdjusmentDetail.Batch);

            //    FormInventoryAdjusmentDetail.TeoriticalQty = s.Sum(x => x.Quantity);

            //    //FormInventoryAdjusmentDetail.TeoriticalQty = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == FormInventoryAdjusmentDetail.ProductId && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.TransactionStockId)?.Batch)?.Qty ?? 0;
            //}
        }

        private InventoryAdjusmentDetailDto FormInventoryAdjusmentDetail = new();

        private void ResetFormInventoryAdjustmentDetail()
        {
            FormInventoryAdjusmentDetail.ExpiredDate = null;
            FormInventoryAdjusmentDetail.ProductId = null;
            FormInventoryAdjusmentDetail.TransactionStockId = null;
            FormInventoryAdjusmentDetail.UomId = null;
            FormInventoryAdjusmentDetail.TeoriticalQty = 0;
        }

        private void UpdateFormInventoryAdjustmentDetail2(TransactionStockDto stockProduct, long qty)
        {
            if (stockProduct != null)
            {
                FormInventoryAdjusmentDetail.UomId = stockProduct.UomId;
                FormInventoryAdjusmentDetail.TeoriticalQty = qty;
                FormInventoryAdjusmentDetail.ExpiredDate = stockProduct.ExpiredDate;
            }
        }

        private void UpdateFormInventoryAdjustmentDetail(TransactionStockDto stockProduct)
        {
            if (stockProduct != null)
            {
                FormInventoryAdjusmentDetail.UomId = stockProduct.UomId;
                //FormInventoryAdjusmentDetail.TeoriticalQty = stockProduct.Qty;
                //FormInventoryAdjusmentDetail.ExpiredDate = stockProduct.Expired;
            }
        }

        private async Task OnSelectProduct(ProductDto e)
        {
            try
            {
                Batch.Clear();
                ResetFormInventoryAdjustmentDetail();

                if (e == null) return;

                FormInventoryAdjusmentDetail.ProductId = e.Id;

                var stockProducts2 = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == InventoryAdjusment.LocationId));
                if (e.TraceAbility)
                {
                    var s = await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.Batch != null && x.Batch == FormInventoryAdjusmentDetail.Batch));
                    Batch = stockProducts2?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();

                    var firstStockProduct = stockProducts2.Where(x => x.Batch == FormInventoryAdjusmentDetail.Batch);
                    //var inn = s.Select(x => x.InStock).Sum();
                    //var outt = s.Select(x => x.OutStock).Sum();
                    //var final = inn - outt;

                    UpdateFormInventoryAdjustmentDetail2(firstStockProduct.FirstOrDefault() ?? new(), firstStockProduct.Sum(x => x.Quantity));
                }
                else
                {
                    var s = (await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.LocationId == InventoryAdjusment.LocationId)));
                    //var inn = s.Select(x => x.InStock).Sum();
                    //var outt = s.Select(x => x.OutStock).Sum();
                    //var final = inn - outt;

                    var firstStockProduct = stockProducts2.FirstOrDefault();
                    FormInventoryAdjusmentDetail.TransactionStockId = firstStockProduct?.Id ?? null;
                    UpdateFormInventoryAdjustmentDetail2(firstStockProduct ?? new(), s.Sum(x => x.Quantity));
                }

                return;
                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == InventoryAdjusment.LocationId));

                //if (e.TraceAbility)
                //{
                //    Batch = stockProducts;
                //    var firstStockProduct = stockProducts.FirstOrDefault(x => x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.TransactionStockId)?.Batch);
                //    //UpdateFormInventoryAdjustmentDetail(firstStockProduct);
                //}
                //else
                //{
                //    var firstStockProduct = stockProducts.FirstOrDefault();
                //    //UpdateFormInventoryAdjustmentDetail(firstStockProduct);
                //}
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            //try
            //{
            //    Batch.Clear();
            //    FormInventoryAdjusmentDetail.ExpiredDate = null;
            //    FormInventoryAdjusmentDetail.ProductId = null;
            //    FormInventoryAdjusmentDetail.TransactionStockId = null;
            //    FormInventoryAdjusmentDetail.UomId = null;
            //    FormInventoryAdjusmentDetail.TeoriticalQty = 0;

            //    if (e is null)
            //    {
            //        return;
            //    }

            //    FormInventoryAdjusmentDetail.ProductId = e.Id;

            //    if (e.TraceAbility)
            //    {
            //        var StockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.SourceId == InventoryAdjusment.LocationId));
            //        Batch = StockProducts;
            //        FormInventoryAdjusmentDetail.UomId = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.TransactionStockId)?.Batch)?.UomId ?? null;
            //        FormInventoryAdjusmentDetail.TeoriticalQty = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.TransactionStockId)?.Batch)?.Qty ?? 0;
            //        //FormInventoryAdjusmentDetail.LotSerialNumber = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id)?.Batch ?? "-";
            //        FormInventoryAdjusmentDetail.ExpiredDate = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.TransactionStockId)?.Batch)?.Expired;
            //    }
            //    else
            //    {
            //        var StockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.SourceId == InventoryAdjusment.LocationId));
            //        FormInventoryAdjusmentDetail.UomId = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id)?.UomId ?? null;

            //        FormInventoryAdjusmentDetail.TeoriticalQty = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id)?.Qty ?? 0;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.HandleException(ToastService);
            //}
        }
    }
}
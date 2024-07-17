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
        private bool FormValidationState { get; set; } = true;
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

            Locations = await Mediator.Send(new GetLocationQuery());
            Companies = await Mediator.Send(new GetCompanyQuery());
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
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteInventoryAdjusmentRequest(((InventoryAdjusmentDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteInventoryAdjusmentRequest(ids: SelectedDataItems.Adapt<List<InventoryAdjusmentDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
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
                        StagingText = EnumStatusInventoryAdjustment.InProgress.GetDisplayName();
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
                //    var StockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == e.Id));

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
                if (!FormValidationState && InventoryAdjusmentDetails.Count == 0)
                {
                    ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                    return;
                }

                if (InventoryAdjusment.Id == 0)
                {
                    InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Draft;
                    StagingText = "Start Inventory";
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
        }

        private async Task SelectLocation(LocationDto e)
        {
            if (e is null)
            {
                Products.Clear();
                return;
            }

            var st = await Mediator.Send(new GetStockProductQuery(x => x.SourceId == e.Id));

            Products = AllProducts.Where(x => st.Select(s => s.ProductId).Contains(x.Id)).ToList();
        }

        private async Task SaveInventoryAdjusmentDetail(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (e is null)
                    return;

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
                        await Mediator.Send(new CreateInventoryAdjusmentDetailRequest(inventoryAdjusmentDetail));
                    else
                        await Mediator.Send(new UpdateInventoryAdjusmentDetailRequest(inventoryAdjusmentDetail));

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
                        await Mediator.Send(new DeleteInventoryAdjusmentDetailRequest(((InventoryAdjusmentDetailDto)e.DataItem).Id));
                    }
                    else
                    {
                        var a = SelectedDetailDataItems.Adapt<List<InventoryAdjusmentDetailDto>>();
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
            //    var stockProducts = await Mediator.Send(new GetStockProductQuery(s =>  s.ProductId == x.ProductId && s.SourceId == InventoryAdjusment.LocationId));

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

            FormInventoryAdjusmentDetail = new();
            TotalQty = 0;
            LotSerialNumber = "-";
            UomId = null;

            await GridDetail.StartEditNewRowAsync();
        }

        private async Task EditItemDetail_Click(IGrid context)
        {
            await GridDetail.StartEditRowAsync(FocusedRowDetailVisibleIndex);

            var aa = Uoms;
            var bb = Batch;

            // Ensure the context is not null and has selected data item
            if (context.SelectedDataItem != null)
            {
                FormInventoryAdjusmentDetail = context.SelectedDataItem.Adapt<InventoryAdjusmentDetailDto>();

                // Check if Product and StockProduct are not null before accessing their properties
                if (FormInventoryAdjusmentDetail.Product != null)
                {
                    FormInventoryAdjusmentDetail.UomId = FormInventoryAdjusmentDetail.Product.UomId;
                }

                if (FormInventoryAdjusmentDetail.StockProduct != null)
                {
                    FormInventoryAdjusmentDetail.StockProductId = FormInventoryAdjusmentDetail.StockProduct.Id;
                    FormInventoryAdjusmentDetail.TeoriticalQty = FormInventoryAdjusmentDetail.TransactionStock?.InStock ?? 0L;
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

        private async Task OnClickConfirm()
        {
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
            //            var stockProduct = (await Mediator.Send(new GetStockProductQuery(s => s.Id == x.StockProductId)))
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

            switch (StagingText)
            {
                case "Start Inventory":
                    InventoryAdjusment.Status = EnumStatusInventoryAdjustment.InProgress;
                    StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();
                    break;

                case "In-Progress":
                    InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
                    StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();
                    break;

                case "Invalidate":
                    InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
                    break;

                default:
                    return;
            }

            await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

            if (StagingText == EnumStatusInventoryAdjustment.Invalidate.GetDisplayName())
            {
                var stockProductsToUpdate = new List<StockProductDto>();
                var adjustmentDetailsToUpdate = new List<InventoryAdjusmentDetailDto>();

                foreach (var detail in InventoryAdjusmentDetails)
                {
                    var stockProduct = (await Mediator.Send(new GetStockProductQuery(s => s.Id == detail.StockProductId))).FirstOrDefault();

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

                await Mediator.Send(new UpdateListStockProductRequest(stockProductsToUpdate));
                await Mediator.Send(new UpdateListInventoryAdjusmentDetailRequest(adjustmentDetailsToUpdate));

                await LoadInventoryAdjustmentDetails();
            }
        }

        private long TotalQty { get; set; } = 0;
        private long? UomId { get; set; } = 0;
        private string LotSerialNumber { get; set; } = "-";
        private DateTime? ExpiredDate { get; set; }

        private List<StockProductDto> Batch = [];
        private DateTime? SelectedBatchExpired { get; set; }

        private async Task SelectedBatch(StockProductDto stockProduct)
        {
            FormInventoryAdjusmentDetail.StockProductId = null;
            FormInventoryAdjusmentDetail.UomId = null;
            FormInventoryAdjusmentDetail.ExpiredDate = null;
            FormInventoryAdjusmentDetail.TeoriticalQty = 0;

            if (stockProduct is null)
            {
                return;
            }

            FormInventoryAdjusmentDetail.StockProductId = stockProduct.Id;

            if (FormInventoryAdjusmentDetail.ProductId is not null)
            {
                var StockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == FormInventoryAdjusmentDetail.ProductId && s.SourceId == InventoryAdjusment.LocationId));
                FormInventoryAdjusmentDetail.UomId = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == FormInventoryAdjusmentDetail.ProductId && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch)?.UomId ?? null;
                FormInventoryAdjusmentDetail.ExpiredDate = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == FormInventoryAdjusmentDetail.ProductId && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch)?.Expired;

                //var s = await Mediator.Send(new GetStockProductQuery(x => x.ProductId == FormInventoryAdjusmentDetail.ProductId));
                //var inn = s.Select(x => x.InStock).Sum();
                //var outt = s.Select(x => x.OutStock).Sum();
                //var final = inn - outt;

                var stockProducts2 = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == FormInventoryAdjusmentDetail.ProductId && s.SourceId == InventoryAdjusment.LocationId));

                var s = (await Mediator.Send(new GetStockProductQuery(x => x.ProductId == FormInventoryAdjusmentDetail.ProductId && x.Batch != null && x.Batch == stockProduct.Batch))).FirstOrDefault(); 
                var firstStockProduct = stockProducts2.FirstOrDefault(x => x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch);
                //var inn = s.Select(x => x.InStock).Sum();
                //var outt = s.Select(x => x.OutStock).Sum();
                //var final = inn - outt;

                FormInventoryAdjusmentDetail.TeoriticalQty = s?.Qty ?? 0;

                //FormInventoryAdjusmentDetail.TeoriticalQty = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == FormInventoryAdjusmentDetail.ProductId && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch)?.Qty ?? 0;
            }
        }

        private InventoryAdjusmentDetailDto FormInventoryAdjusmentDetail = new();

        private void ResetFormInventoryAdjustmentDetail()
        {
            FormInventoryAdjusmentDetail.ExpiredDate = null;
            FormInventoryAdjusmentDetail.ProductId = null;
            FormInventoryAdjusmentDetail.StockProductId = null;
            FormInventoryAdjusmentDetail.UomId = null;
            FormInventoryAdjusmentDetail.TeoriticalQty = 0;
        }

        private void UpdateFormInventoryAdjustmentDetail2(StockProductDto stockProduct, long qty)
        {
            if (stockProduct != null)
            {
                FormInventoryAdjusmentDetail.UomId = stockProduct.UomId;    
                FormInventoryAdjusmentDetail.TeoriticalQty = qty;
                FormInventoryAdjusmentDetail.ExpiredDate = stockProduct.Expired;
            }
        }
        private void UpdateFormInventoryAdjustmentDetail(StockProductDto stockProduct)
        {
            if (stockProduct != null)
            {
                FormInventoryAdjusmentDetail.UomId = stockProduct.UomId;
                FormInventoryAdjusmentDetail.TeoriticalQty = stockProduct.Qty;
                FormInventoryAdjusmentDetail.ExpiredDate = stockProduct.Expired;
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

                var stockProducts2 = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == e.Id && s.SourceId == InventoryAdjusment.LocationId));
                if (e.TraceAbility)
                {
                    var batch = Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch ?? string.Empty;

                    var s = await Mediator.Send(new GetStockProductQuery(x => x.ProductId == e.Id && x.Batch != null && x.Batch == batch));
                    Batch = stockProducts2;

                    var firstStockProduct = stockProducts2.FirstOrDefault(x => x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch) ?? new();
                    //var inn = s.Select(x => x.InStock).Sum();
                    //var outt = s.Select(x => x.OutStock).Sum();
                    //var final = inn - outt;

                    UpdateFormInventoryAdjustmentDetail2(firstStockProduct ?? new(), firstStockProduct?.Qty ?? 0);
                }
                else
                {
                    var s = (await Mediator.Send(new GetStockProductQuery(x => x.ProductId == e.Id && x.SourceId == InventoryAdjusment.LocationId))).FirstOrDefault() ?? new();
                    //var inn = s.Select(x => x.InStock).Sum();
                    //var outt = s.Select(x => x.OutStock).Sum();
                    //var final = inn - outt;

                    var firstStockProduct = stockProducts2.FirstOrDefault();
                    FormInventoryAdjusmentDetail.StockProductId = firstStockProduct?.Id ?? null;
                    UpdateFormInventoryAdjustmentDetail2(firstStockProduct ?? new(), s.Qty);
                }

                return;
                var stockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == e.Id && s.SourceId == InventoryAdjusment.LocationId));

                if (e.TraceAbility)
                {
                    Batch = stockProducts;
                    var firstStockProduct = stockProducts.FirstOrDefault(x => x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch);
                    //UpdateFormInventoryAdjustmentDetail(firstStockProduct);
                }
                else
                {
                    var firstStockProduct = stockProducts.FirstOrDefault();
                    //UpdateFormInventoryAdjustmentDetail(firstStockProduct);
                }
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
            //    FormInventoryAdjusmentDetail.StockProductId = null;
            //    FormInventoryAdjusmentDetail.UomId = null;
            //    FormInventoryAdjusmentDetail.TeoriticalQty = 0;

            //    if (e is null)
            //    {
            //        return;
            //    }

            //    FormInventoryAdjusmentDetail.ProductId = e.Id;

            //    if (e.TraceAbility)
            //    {
            //        var StockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == e.Id && s.SourceId == InventoryAdjusment.LocationId));
            //        Batch = StockProducts;
            //        FormInventoryAdjusmentDetail.UomId = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch)?.UomId ?? null;
            //        FormInventoryAdjusmentDetail.TeoriticalQty = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch)?.Qty ?? 0;
            //        //FormInventoryAdjusmentDetail.LotSerialNumber = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id)?.Batch ?? "-";
            //        FormInventoryAdjusmentDetail.ExpiredDate = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId && x.ProductId == e.Id && x.Batch == Batch.FirstOrDefault(z => z.Id == FormInventoryAdjusmentDetail.StockProductId)?.Batch)?.Expired;
            //    }
            //    else
            //    {
            //        var StockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == e.Id && s.SourceId == InventoryAdjusment.LocationId));
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
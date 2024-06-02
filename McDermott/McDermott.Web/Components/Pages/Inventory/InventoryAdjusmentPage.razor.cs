using DevExpress.Blazor.Internal;
using McDermott.Application.Dtos.Inventory;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using Unipluss.Sign.Client.Code;
using static McDermott.Application.Extentions.EnumHelper;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;

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
                var user = await UserInfoService.GetUserInfo();
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

        private bool IsStatus(EnumStatusInventoryAdjusment status) => InventoryAdjusment.Status == status;

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

        private void GridDetail_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowDetailVisibleIndex = args.VisibleIndex;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
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
                    case EnumStatusInventoryAdjusment.Draft:
                        StagingText = EnumStatusInventoryAdjusment.InProgress.GetDisplayName();
                        break;

                    case EnumStatusInventoryAdjusment.InProgress:
                        StagingText = EnumStatusInventoryAdjusment.Invalidate.GetDisplayName();
                        break;

                    case EnumStatusInventoryAdjusment.Invalidate:
                        StagingText = EnumStatusInventoryAdjusment.Invalidate.GetDisplayName();
                        break;

                    default:
                        break;
                }

                InventoryAdjusmentDetails = await Mediator.Send(new GetInventoryAdjusmentDetailQuery(x => x.InventoryAdjusmentId == InventoryAdjusment.Id));
                InventoryAdjusmentDetails.ForEach(async e =>
                {
                    var StockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == e.Id));

                    TotalQty = StockProducts.Sum(x => x.Qty) ?? 0;

                    UomId = e.UomId ?? null;

                    if (e.Product is not null && e.Product.TraceAbility)
                    {
                        LotSerialNumber = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId)?.Batch ?? "-";
                        ExpiredDate = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId)?.Expired;
                    }
                    else
                    {
                        LotSerialNumber = "-";
                        ExpiredDate = null;
                    }
                });
            }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
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
                    InventoryAdjusment.Status = EnumStatusInventoryAdjusment.Draft;
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

                var inventoryAdjusmentDetail = (InventoryAdjusmentDetailDto)e.EditModel;

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

            await GridDetail.StartEditNewRowAsync();
        }

        private async Task EditItemDetail_Click(IGrid context)
        {
            await GridDetail.StartEditRowAsync(FocusedRowDetailVisibleIndex);
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
            switch (StagingText)
            {
                case "Start Inventory":
                    InventoryAdjusment.Status = EnumStatusInventoryAdjusment.InProgress;
                    StagingText = EnumStatusInventoryAdjusment.InProgress.GetDisplayName();
                    await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
                    break;

                case "In-Progress":
                    InventoryAdjusment.Status = EnumStatusInventoryAdjusment.Invalidate;
                    StagingText = EnumStatusInventoryAdjusment.Invalidate.GetDisplayName();
                    await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
                    break;

                case "Invalidate":
                    InventoryAdjusment.Status = EnumStatusInventoryAdjusment.Invalidate;
                    await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
                    break;

                default:
                    break;
            }
        }

        private long TotalQty { get; set; } = 0;
        private long? UomId { get; set; } = 0;
        private string LotSerialNumber { get; set; } = "-";
        private DateTime? ExpiredDate { get; set; }

        private async Task OnSelectProduct(ProductDto e)
        {
            try
            {
                if (e is null)
                {
                    return;
                }

                var StockProducts = await Mediator.Send(new GetStockProductQuery(s => s.ProductId == e.Id));

                TotalQty = StockProducts.Sum(x => x.Qty) ?? 0;

                UomId = e.UomId ?? null;

                if (e.TraceAbility)
                {
                    LotSerialNumber = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId)?.Batch ?? "-";
                    ExpiredDate = StockProducts.FirstOrDefault(x => x.SourceId == InventoryAdjusment.LocationId)?.Expired;
                }
                else
                {
                    LotSerialNumber = "-";
                    ExpiredDate = null;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
    }
}
using McDermott.Domain.Entities;
using MediatR;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentGroupCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class ReceivingTransferPage
    {
        #region Relation Data

        private List<StockProductDto> stockProducts = [];
        private List<ReceivingStockDetailDto> receivingStockDetails = [];
        private List<ReceivingStockDetailDto> TempReceivingStockDetails = [];
        private List<LocationDto> Locations = [];
        private List<ProductDto> Products = [];
        private List<UomDto> Uoms = [];
        private ReceivingStockDetailDto FormReceivingStock = new();
        private ReceivingStockDetailDto TempFormReceivingStock = new();

        #endregion Relation Data

        #region Variable Static

        private IGrid? Grid { get; set; }
        private IGrid? GridProduct { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool IsAddReceived { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private string? header { get; set; }
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
                var user = await UserInfoService.GetUserInfo();
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
            stockProducts = await Mediator.Send(new GetStockProductQuery());
            receivingStockDetails = await Mediator.Send(new GetReceivingStockDetailQuery());
            Locations = await Mediator.Send(new GetLocationQuery());
            Products = await Mediator.Send(new GetProductQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            PanelVisible = false;
        }

        private async Task SelectedChangeProduct(ProductDto product)
        {
            var productName = Products.Where(p => p.Id == product.Id).FirstOrDefault();
            var uomName = Uoms.Where(u => u.Id == product.UomId).Select(x => x.Name).FirstOrDefault();
            var purchaseName = Uoms.Where(u => u.Id == product.PurchaseUomId).Select(x => x.Name).FirstOrDefault();
            TempFormReceivingStock.PurchaseName = purchaseName;
            TempFormReceivingStock.UomName = uomName;
            TempFormReceivingStock.ProductName = productName.Name;
            //TempFormReceivingStock.TraceAbility = productName.TraceAbility;
        }

        private async Task HandleValidSubmit()
        {
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

            //try
            //{
            //    if ((TransactionStockDto)args.DataItem is null)
            //        return;

            //    isActiveButton = ((TransactionStockDto)args.DataItem)!.StatusTransfer!.Equals("Draft");
            //}
            //catch (Exception ex)
            //{
            //    ex.HandleException(ToastService);
            //}
        }

        private void Grid_FocusedRowChangedDetail(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            //await EditItem_Click();
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
        }

        private async Task EditItem_Click()
        {
            showForm = true;
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
            TempFormReceivingStock = new();
            await GridProduct.StartEditNewRowAsync();
        }

        private async Task EditItemDetail_Click()
        {
            await GridProduct.StartEditRowAsync(FocusedRowVisibleIndex);
            IsAddReceived = false;
        }

        private async Task DeleteItemDetail_Click()
        {
        }

        #endregion Click Button

        #region Function Delete

        private async Task OnDelete()
        {
        }

        private async Task OnDelete_Detail()
        {
        }

        #endregion Function Delete

        #region Function Save

        private async Task OnSave()
        {
            try
            {
                if (FormValidationState == false)
                {
                    return;
                }
                if (FormReceivingStock.Id == 0)
                {
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSave_Detail(GridEditModelSavingEventArgs e)
        {
            try
            {
                var tempReceiving = (ReceivingStockDetailDto)e.EditModel;
                ReceivingStockDetailDto updates = new();

                if (IsAddReceived)
                {
                    // Cek apakah medicament dengan MedicamentId yang sama sudah ada
                    if (TempReceivingStockDetails.Any(x => x.ProductId == tempReceiving.ProductId))
                        return;

                    TempReceivingStockDetails.Add(TempFormReceivingStock);
                }
                else
                {
                    var d = SelectedDataItemsDetail[0].Adapt<ReceivingStockDetailDto>();
                    var checkData = TempReceivingStockDetails.FirstOrDefault(x => x.ProductId == d.ProductId);
                    if (checkData is not null && d.ProductId == tempReceiving.ProductId)
                        return;

                    updates = TempReceivingStockDetails.FirstOrDefault(x => x.ProductId == d.ProductId);

                    var index = TempReceivingStockDetails.IndexOf(updates!);

                    TempReceivingStockDetails[index] = tempReceiving;
                }
                SelectedDataItemsDetail = new ObservableRangeCollection<object>();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Function Save
    }
}
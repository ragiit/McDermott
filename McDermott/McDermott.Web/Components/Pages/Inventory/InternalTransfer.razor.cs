using DevExpress.ClipboardSource.SpreadsheetML;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class InternalTransfer
    {
        #region relation Data

        private List<TransactionStockDto> TransactionStocks = [];
        private List<StockProductDto> StockProducts = [];
        private List<LocationDto> Locations = [];
        private List<ProductDto> Products = [];
        private List<UomDto> Uoms = [];
        private TransactionStockDto FormInternalTransfer = new();

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

        #region static Variable

        private IGrid? Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private string? header { get; set; } = string.Empty;
        private string? UomName { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }

        #endregion static Variable

        #region Load

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                showForm = false;
                //TransactionStocks = a
                StockProducts = await Mediator.Send(new GetStockProductQuery());
                Locations = await Mediator.Send(new GetLocationQuery());
                Products = await Mediator.Send(new GetProductQuery());
                Uoms = await Mediator.Send(new GetUomQuery());
                UomName = Uoms.Select(x => x.Name).FirstOrDefault();
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void SelectedItemProduct(ProductDto product)
        {
            var data = Products.Where(p => p.Id == product.Id).FirstOrDefault();
            var uomName = Uoms.Where(u => u.Id == data.UomId).Select(x => x.Name).FirstOrDefault();
            FormInternalTransfer.UomName = uomName;
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
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            //    await EditItem_Click();
        }

        #endregion Grid

        #region Click

        private async Task NewItem_Click()
        {
            showForm = true;
            FormInternalTransfer = new();
            header = "Add Transfer Internal";
        }

        private async Task EditItem_Click()
        {
        }

        private async Task onDiscard()
        {
            await LoadData();
        }

        private void DeleteItem_Click()
        {
            Grid!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        #endregion Click

        #region function Delete

        private async Task OnDelete()
        {
        }

        #endregion function Delete

        #region Function Save

        private async Task OnSave()
        {
            try
            {
                if (FormValidationState == false)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Function Save
    }
}
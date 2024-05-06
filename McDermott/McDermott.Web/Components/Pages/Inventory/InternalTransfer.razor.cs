using DevExpress.ClipboardSource.SpreadsheetML;
using McDermott.Domain.Entities;
using System.Runtime.ConstrainedExecution;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class InternalTransfer
    {
        #region relation Data

        private List<TransactionStockDto> TransactionStocks = [];
        private List<TransactionStockDetailDto> TempTransactionStocks = [];
        private List<StockProductDto> StockProducts = [];
        private List<LocationDto> Locations = [];
        private List<ProductDto> Products = [];
        private List<UomDto> Uoms = [];
        private TransactionStockDto FormInternalTransfer = new();
        private TransactionStockDto getInternalTransfer = new();
        private TransactionStockDetailDto TempFormInternalTransfer = new();

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
        private IGrid? GridDetailTransferStock { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool showFormDetail { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private bool IsAddTransfer { get; set; } = false;
        private bool showButton { get; set; } = false;
        private bool showMatching { get; set; } = false;
        private string? header { get; set; } = string.Empty;
        private string? headerDetail { get; set; } = string.Empty;

        private string? UomName { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsDetail { get; set; } = new ObservableRangeCollection<object>();
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
                showFormDetail = false;
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
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
            TempFormInternalTransfer.ProductName = data.Name;
            var uomName = Uoms.Where(u => u.Id == data.UomId).Select(x => x.Name).FirstOrDefault();
            TempFormInternalTransfer.UomName = uomName;
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
            await EditItem_Click();
        }

        private async Task OnRowDoubleClickDetail(GridRowClickEventArgs e)
        {
            await EditItemDetail_Click();
        }

        #endregion Grid

        #region Click

        private async Task NewItem_Click()
        {
            showForm = true;
            FormInternalTransfer = new();
            header = "Add Transfer Internal";
        }

        private async Task NewItemDetail_Click()
        {
            showFormDetail = true;
            IsAddTransfer = true;
            TempFormInternalTransfer = new();
            headerDetail = "Add product Transfer Internal";
            await GridDetailTransferStock.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            showForm = true;
        }

        private async Task EditItemDetail_Click()
        {
            showFormDetail = true;
            IsAddTransfer = false;
        }

        private async Task ToDoCheck()
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

        private void DeleteItemDetail_Click()
        {
            GridDetailTransferStock!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
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

        private async Task OnSave_Detail(GridEditModelSavingEventArgs e)
        {
            try
            {
                var tempDetailTransactions = (TransactionStockDetailDto)e.EditModel;
                TransactionStockDetailDto Updates = new();

                //Jika Menambahkan Product
                if (IsAddTransfer)
                {
                    // Cek apakah medicament dengan MedicamentId yang sama sudah ada
                    if (TempTransactionStocks.Any(x => x.ProductId == tempDetailTransactions.ProductId))
                        return;

                    TempTransactionStocks.Add(TempFormInternalTransfer);
                }
                else
                {
                    var d = SelectedDataItemsDetail[0].Adapt<TransactionStockDetailDto>();
                    var check = TempTransactionStocks.FirstOrDefault(x => x.ProductId == tempDetailTransactions.ProductId);
                    if (check is not null && d.ProductId != tempDetailTransactions.ProductId)
                        return;

                    Updates = TempTransactionStocks.FirstOrDefault(s => s.ProductId == d.ProductId);

                    var index = TempTransactionStocks.IndexOf(Updates!);

                    TempTransactionStocks[index] = tempDetailTransactions;
                }
                SelectedDataItemsDetail = new ObservableRangeCollection<object>();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
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

                var getKodeTransaksi = TransactionStocks.Where(t => t.SourceId == FormInternalTransfer.SourceId).Select(x => x.KodeTransaksi).FirstOrDefault();

                if (getKodeTransaksi == null)
                {
                    var nextTransactionNumber = 1;
                    FormInternalTransfer.KodeTransaksi = $"INT/{nextTransactionNumber.ToString("00000")}";
                }
                else
                {
                    // Jika kode transaksi sudah ada, kita perlu mengekstrak nomor urutnya, menambahkannya, dan membuat kode transaksi baru
                    var lastTransactionNumber = int.Parse(getKodeTransaksi.Split('/')[1]);
                    var nextTransactionNumber = lastTransactionNumber + 1;
                    FormInternalTransfer.KodeTransaksi = $"INT/{nextTransactionNumber.ToString("00000")}";
                }

                FormInternalTransfer.StatusTranfer = "Draft";
                if (FormInternalTransfer.Id == 0)
                {
                    getInternalTransfer = await Mediator.Send(new CreateTransactionStockRequest(FormInternalTransfer));
                    if (TempTransactionStocks.Count > 0)
                    {
                        foreach (var item in TempTransactionStocks)
                        {
                            item.TransactionStockId = getInternalTransfer.Id;
                            item.ProductId = TempFormInternalTransfer.ProductId;
                            item.QtyStock = TempFormInternalTransfer.QtyStock;
                            item.StatusStock = "IN";

                            await Mediator.Send(new CreateTransactionStockDetailRequest(item));
                        }
                    }
                }
                ToastService.ShowSuccess("Add Data Success...");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Function Save
    }
}
using DevExpress.ClipboardSource.SpreadsheetML;
using McDermott.Domain.Entities;
using System.Runtime.ConstrainedExecution;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentGroupCommand;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class InternalTransfer
    {
        #region relation Data

        private List<TransactionStockDto> TransactionStocks = [];
        private List<TransactionStockDetailDto> TempTransactionStocks = [];
        private List<TransactionStockDetailDto> TransactionStockDetails = [];
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
        private long? transactionId { get; set; }
        private string? header { get; set; } = string.Empty;
        private string? headerDetail { get; set; } = string.Empty;

        private bool isActiveButton = false;
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

        public MarkupString GetIssueStatusIconHtml(string status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case "Draft":
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case "Waiting":
                    priorityClass = "warning";
                    title = "Waiting";
                    break;

                case "Ready":
                    priorityClass = "primary";
                    title = "Ready";
                    break;

                case "Done":
                    priorityClass = "success";
                    title = "Done";
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
                if ((TransactionStockDto)args.DataItem is null)
                    return;

                isActiveButton = ((TransactionStockDto)args.DataItem)!.StatusTransfer!.Equals("Draft");
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
            TempTransactionStocks.Clear();
            isActiveButton = true;
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
            try
            {
                showForm = true;
                FormInternalTransfer = SelectedDataItems[0].Adapt<TransactionStockDto>();

                transactionId = FormInternalTransfer.Id;

                TransactionStockDetails = await Mediator.Send(new GetTransactionStockDetailQuery(x => x.TransactionStockId == FormInternalTransfer.Id));
                TempTransactionStocks = TransactionStockDetails.Select(x => x).ToList();
                foreach (var item in TempTransactionStocks)
                {
                    var d = Products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                    item.UomName = Uoms.Where(u => u.Id == d.UomId).Select(x => x.Name).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task EditItemDetail_Click()
        {
            showFormDetail = true;
            IsAddTransfer = false;
        }

        private async Task ToDoCheck()
        {
            var asyncData = await Mediator.Send(new GetTransactionStockDetailQuery(x => x.TransactionStockId == transactionId));
            foreach (var item in asyncData)
            {
                var StockSent = asyncData.Where(t => t.TransactionStock.SourceId == FormInternalTransfer.SourceId && t.ProductId == item.ProductId).FirstOrDefault();
            }
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

                if (FormInternalTransfer.Id == 0)
                {
                    var sourcname = Locations.Where(x => x.Id == FormInternalTransfer.SourceId).Select(x => x.Name).FirstOrDefault();
                    var getKodeTransaksi = TransactionStocks.Where(t => t.SourceId == FormInternalTransfer.SourceId).OrderByDescending(x => x.KodeTransaksi).Select(x => x.KodeTransaksi).FirstOrDefault();

                    if (getKodeTransaksi == null)
                    {
                        var nextTransactionNumber = 1;
                        FormInternalTransfer.KodeTransaksi = $"{sourcname}/INT/{nextTransactionNumber.ToString("00000")}";
                    }
                    else
                    {
                        var lastTransactionNumber = 0;
                        if (getKodeTransaksi.Contains("/INT/"))
                        {
                            var lastTransactionNumberStr = getKodeTransaksi.Split('/')[2];
                            int.TryParse(lastTransactionNumberStr, out lastTransactionNumber);
                        }

                        var nextTransactionNumber = lastTransactionNumber + 1;
                        FormInternalTransfer.KodeTransaksi = $"{sourcname}/INT/{nextTransactionNumber.ToString("00000")}";
                    }

                    FormInternalTransfer.StatusTransfer = "Draft";

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
                    ToastService.ShowSuccess("Add Data Success...");
                }
                else
                {
                    var bb = FormInternalTransfer;
                    getInternalTransfer = await Mediator.Send(new UpdateTransactionStockRequest(FormInternalTransfer));

                    var stock_detail = await Mediator.Send(new GetTransactionStockQuery(x => x.Id == FormInternalTransfer.Id));
                    var aa = TransactionStockDetails;
                    await Mediator.Send(new DeleteTransactionStockDetailRequest(ids: TransactionStockDetails.Select(x => x.Id).ToList()));

                    var request = new List<TransactionStockDetailDto>();
                    var ss = TransactionStockDetails;
                    if (TempTransactionStocks.Count > 0)
                    {
                        TempTransactionStocks.ForEach(x =>
                        {
                            x.Id = 0;
                            x.TransactionStockId = stock_detail[0].Id;
                        });

                        for (int i = 0; i < TempTransactionStocks.Count; i++)
                        {
                            var cekLagi = TempTransactionStocks.FirstOrDefault(x => x.TransactionStockId == TempTransactionStocks[i].TransactionStockId);
                            if (cekLagi is null)
                            {
                                TempTransactionStocks.Add(new TransactionStockDetailDto
                                {
                                    Id = 0,
                                    TransactionStockId = stock_detail[0].Id
                                });
                            }
                        }

                        await Mediator.Send(new CreateListTransactionStockDetailRequest(TempTransactionStocks));
                    }

                    ToastService.ShowSuccess("Update Data Success...");
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
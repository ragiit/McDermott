using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Inventory.TransferStockCommand;

namespace McDermott.Web.Components.Pages.Inventory.InternalTransfer
{
    public partial class CreateUpdateInternalTransferPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

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

        #region Relation Data
        private List<TransferStockDto> getTransferStocks = [];
        private List<ProductDto> getProducts = [];
        private List<TransactionStockDto> getTransactionStocks = [];
        private List<LocationDto> getDestination = [];
        private List<LocationDto> getSource = [];
        private List<TransferStockProductDto> getTransferStockProducts = [];
        private List<TransferStockLogDto> getTransferStockLog = [];
        private List<UomDto> getUoms = [];

        private TransferStockDto postTransferStocks = new();
        private TransferStockProductDto postTransferStockProducts = new();
        private TransferStockLogDto postTransferStockLogs = new();
        private TransactionStockDto postTransactionStocks = new();

        #endregion

        #region Variabel Static

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; }
        private bool FormValidationState { get; set; }
        private long currentStocks { get; set; } = 0;
        private List<string> Batch = [];
        private DateTime? SelectedBatchExpired { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        #endregion

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadDataProduct(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadDataProduct(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadDataProduct(newPageIndex, pageSize);
        }

        #endregion Searching


        #region Load Data
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataDestination();
            await LoadDataSource();
            await LoadDataProduct();
            await LoadDataAsync();
            PanelVisible = false;
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetSingleTransferStockQuery
                {
                    Predicate = x => x.Id == Id,
                });
                if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    if (result is null || !Id.HasValue)
                    {
                        NavigationManager.NavigateTo("inventory/internal-transfers");
                        return;
                    }
                    postTransferStocks = result ?? new();
                    await LoadDataDetail();
                    var resultLog = await Mediator.Send(new GetTransferStockLogQuery
                    {
                        Predicate = x => x.TransferStockId == postTransferStocks.Id
                    });

                    getTransferStockLog = resultLog.Item1;
                }
                PanelVisible = false;
            }
            catch
            {

            }
        }

        private async Task LoadDataDetail()
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetTransferStockProductQuery
                {
                    Predicate = x => x.TransferStockId == postTransferStocks.Id

                });
                getTransferStockProducts = result.Item1;
                PanelVisible = false;
            }
            catch
            {

            }
        }

        private async Task LoadDataAsync()
        {
            PanelVisible = true;
            getTransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
            var resultGet = await Mediator.Send(new GetTransferStockQuery
            {
                Predicate = x => x.Id == postTransferStocks.Id
            });
            getTransferStocks = resultGet.Item1;
            getUoms = await Mediator.Send(new GetAllUomQuery());
            PanelVisible = false;
        }

        #endregion

        #region Load ComboBox

        #region ComboBox Destination

        private DxComboBox<LocationDto, long?> refDestinationComboBox { get; set; }
        private int DestinationComboBoxIndex { get; set; } = 0;
        private int totalCountDestination = 0;

        private async Task OnSearchDestination()
        {
            await LoadDataDestination(0, 10);
        }

        private async Task OnSearchDestinationIndexIncrement()
        {
            if (DestinationComboBoxIndex < (totalCountDestination - 1))
            {
                DestinationComboBoxIndex++;
                await LoadDataDestination(DestinationComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDestinationIndexDecrement()
        {
            if (DestinationComboBoxIndex > 0)
            {
                DestinationComboBoxIndex--;
                await LoadDataDestination(DestinationComboBoxIndex, 10);
            }
        }

        private async Task OnInputDestinationChanged(string e)
        {
            DestinationComboBoxIndex = 0;
            await LoadDataDestination(0, 10);
        }

        private async Task LoadDataDestination(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLocationQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refDestinationComboBox?.Text ?? "",
                Select = x => new Locations
                {
                    Id = x.Id,
                    Name = x.Name,
                }
            }); getDestination = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Destination

        #region ComboBox Product

        private DxComboBox<ProductDto, long?> refProductComboBox { get; set; }
        private int ProductComboBoxIndex { get; set; } = 0;
        private int totalCountProduct = 0;

        private async Task OnSearchProduct()
        {
            await LoadDataProduct(0, 10);
        }

        private async Task OnSearchProductIndexIncrement()
        {
            if (ProductComboBoxIndex < (totalCountProduct - 1))
            {
                ProductComboBoxIndex++;
                await LoadDataProduct(ProductComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProductIndexDecrement()
        {
            if (ProductComboBoxIndex > 0)
            {
                ProductComboBoxIndex--;
                await LoadDataProduct(ProductComboBoxIndex, 10);
            }
        }

        private async Task OnInputProductChanged(string e)
        {
            // Reset UomComboBoxIndex and load new data based on user input
            ProductComboBoxIndex = 0;
            await LoadDataProduct(0, 10);
        }

        private async Task LoadDataProduct(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProductQuery(searchTerm: refProductComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getProducts = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Product

        #region Combo Box Source

        private DxComboBox<LocationDto, long?> refSourceComboBox { get; set; }
        private int SourceComboBoxIndex { get; set; } = 0;
        private int totalCountSource = 0;

        private async Task OnSearchSource()
        {
            await LoadDataSource(0, 10);
        }

        private async Task OnSearchSourceIndexIncrement()
        {
            if (SourceComboBoxIndex < (totalCountSource - 1))
            {
                SourceComboBoxIndex++;
                await LoadDataSource(SourceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchSourceIndexDecrement()
        {
            if (SourceComboBoxIndex > 0)
            {
                SourceComboBoxIndex--;
                await LoadDataSource(SourceComboBoxIndex, 10);
            }
        }

        private async Task OnInputSourceChanged(string e)
        {
            SourceComboBoxIndex = 0;
            await LoadDataSource(0, 10);
        }

        private async Task LoadDataSource(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLocationQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refSourceComboBox?.Text ?? "",
                Select = x => new Locations
                {
                    Id = x.Id,
                    Name = x.Name,
                }
            }); getSource = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion Combo Box DrugForm

        #endregion Load ComboBox

        #region Grid
        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }
        #endregion

        #region Select change data

        private async Task SelectedBatch(string value)
        {
            // Reset fields to default if the batch value is null
            if (value is null)
            {
                postTransferStockProducts.TransactionStockId = null;
                postTransferStockProducts.UomId = null;
                postTransferStockProducts.ExpiredDate = null;
                postTransferStockProducts.CurrentStock = 0;
                return;
            }

            // Assign batch value
            postTransferStockProducts.Batch = value;

            // Proceed if ProductId is not null
            if (postTransferStockProducts.ProductId is not null)
            {
                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == postTransferStockProducts.ProductId &&
                    s.LocationId == postTransferStocks.SourceId &&
                    s.Validate == true
                ));

                // Get the first matching product
                var matchedProduct = stockProducts.FirstOrDefault(x =>
                    x.ProductId == postTransferStockProducts.ProductId &&
                    x.LocationId == postTransferStocks.SourceId &&
                    x.Batch == postTransferStockProducts.Batch
                );

                // Assign UomId, UomName, and ExpiredDate
                postTransferStockProducts.UomId = matchedProduct?.UomId;
                postTransferStockProducts.UomName = matchedProduct?.Uom?.Name;
                postTransferStockProducts.ExpiredDate = matchedProduct?.ExpiredDate;

                // Calculate the current stock for the batch
                var batchStock = await Mediator.Send(new GetTransactionStockQuery(x =>
                    x.Validate == true &&
                    x.ProductId == postTransferStockProducts.ProductId &&
                    x.LocationId == postTransferStocks.SourceId &&
                    x.Batch == postTransferStockProducts.Batch
                ));

                postTransferStockProducts.CurrentStock = batchStock.Sum(x => x.Quantity);
                currentStocks = postTransferStockProducts.CurrentStock;
            }
        }

        private async Task SelectedProductByLocation(LocationDto e)
        {
            if (e is null)
            {
                getProducts.Clear();
                return;
            }

            var st = await Mediator.Send(new GetTransactionStockQuery(x => x.LocationId == e.Id));

            //Products = AllProducts.Where(x => st.Select(s => s.ProductId).Contains(x.Id)).ToList();
        }


        private async Task SelectedItemByProduct(ProductDto e)
        {
            try
            {
                Batch.Clear();
                ResetFormProductDetail();

                if (e == null) return;

                postTransferStockProducts.ProductId = e.Id;
                postTransferStockProducts.TraceAvability = e.TraceAbility;

                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == postTransferStocks.SourceId));
                if (e.TraceAbility)
                {
                    var s = await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.Batch != null && x.Batch == postTransferStockProducts.Batch));
                    Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();

                    var firstStockProduct = stockProducts.Where(x => x.Batch == postTransferStockProducts.Batch);

                    UpdateFormProductDetail(firstStockProduct.FirstOrDefault() ?? new(), firstStockProduct.Sum(x => x.Quantity), e);
                }
                else
                {
                    var s = await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.LocationId == postTransferStocks.SourceId));

                    var firstStockProduct = stockProducts.FirstOrDefault();
                    postTransferStockProducts.TransactionStockId = firstStockProduct?.Id ?? null;
                    UpdateFormProductDetail(firstStockProduct ?? new(), s.Sum(x => x.Quantity), e);
                }

                return;
                var stockProducts2 = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == postTransferStocks.SourceId));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void ResetFormProductDetail()
        {
            postTransferStockProducts.ExpiredDate = null;
            postTransferStockProducts.ProductId = null;
            postTransferStockProducts.UomId = null;
            currentStocks = 0;
            postTransferStockProducts.Batch = null;
        }

        private void UpdateFormProductDetail(TransactionStockDto stockProduct, long qty, ProductDto items)
        {
            if (stockProduct != null)
            {
                postTransferStockProducts.UomId = items.UomId;
                postTransferStockProducts.UomName = items.Uom.Name;
                currentStocks = qty;
                postTransferStockProducts.ExpiredDate = stockProduct.ExpiredDate;
            }
        }

        private void checkStock(long value)
        {
            if (value > currentStocks)
            {
                ToastService.ClearCustomToasts();
                ToastService.ShowWarning("The stock sent is less than the available stock!!");

            }
            else
            {
                postTransferStockProducts.QtyStock = value;
            }
        }

        #endregion

        #region HandleSubmit
        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await OnSave();
            else
                FormValidationState = true;

        }

        private async Task HandleInvalidSubmit()
        {
            FormValidationState = false;
        }

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }
        #endregion

        #region Function Save

        private async Task OnSave()
        {
            try
            {
                if (postTransferStocks.SourceId is null)
                {
                    return;
                }

                if (postTransferStocks.Id == 0)
                {
                    var sourcname = getSource.Where(x => x.Id == postTransferStocks.SourceId).Select(x => x.Name).FirstOrDefault();
                    var getKodeTransaksi = getTransferStocks.Where(t => t.SourceId == postTransferStocks.SourceId).OrderByDescending(x => x.KodeTransaksi).Select(x => x.KodeTransaksi).FirstOrDefault();

                    if (getKodeTransaksi == null)
                    {
                        var nextTransferNumber = 1;
                        postTransferStocks.KodeTransaksi = $"{sourcname}/INT/{nextTransferNumber.ToString("00000")}";
                    }
                    else
                    {
                        var lastTransferNumber = 0;
                        if (getKodeTransaksi.Contains("/INT/"))
                        {
                            var lastTransferNumberStr = getKodeTransaksi.Split('/')[2];
                            int.TryParse(lastTransferNumberStr, out lastTransferNumber);
                        }

                        var nextTransferNumber = lastTransferNumber + 1;
                        postTransferStocks.KodeTransaksi = $"{sourcname}/INT/{nextTransferNumber.ToString("00000")}";
                    }

                    postTransferStocks.Status = EnumStatusInternalTransfer.Draft;

                    var tempTransferStock = await Mediator.Send(new CreateTransferStockRequest(postTransferStocks));

                    ToastService.ShowSuccess("Add Data Success...");

                    postTransferStockLogs.TransferStockId = tempTransferStock.Id;
                    postTransferStockLogs.SourceId = tempTransferStock.SourceId;
                    postTransferStockLogs.DestinationId = tempTransferStock.DestinationId;
                    postTransferStockLogs.Status = EnumStatusInternalTransfer.Draft;
                    postTransferStockLogs.UserById = UserLogin.Id;

                    await Mediator.Send(new CreateTransferStockLogRequest(postTransferStockLogs));
                    NavigationManager.NavigateTo($"inventory/internal-transfers/{EnumPageMode.Update.GetDisplayName()}?Id={tempTransferStock.Id}", true);

                }
                else
                {
                    var tempTransferStock = await Mediator.Send(new UpdateTransferStockRequest(postTransferStocks));
                    ToastService.ShowSuccess("Update Data Success...");
                    NavigationManager.NavigateTo($"inventory/internal-transfers/{EnumPageMode.Update.GetDisplayName()}?Id={tempTransferStock.Id}",true);
                }

               
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSaveProduct()
        {
            try
            {
                if(postTransferStockProducts.Id == 0)
                {
                    postTransferStockProducts.TransferStockId = postTransferStocks.Id;
                    await Mediator.Send(new CreateTransferStockProductRequest(postTransferStockProducts));
                    ToastService.ShowSuccess("Save Data Product Success..");
                }
                else
                {
                    await Mediator.Send(new UpdateTransferStockProductRequest(postTransferStockProducts));
                    ToastService.ShowSuccess("Save Data Product Success..");
                }

                await LoadDataDetail();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        #endregion Function Save

        #region Delete
        private async Task OnDelete()
        {

        }
        #endregion

        #region Click Button
        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click(IGrid context)
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);

            if (context.SelectedDataItem == null) return;

            var selectedItem = context.SelectedDataItem.Adapt<TransferStockProductDto>();
            postTransferStockProducts = getTransferStockProducts.FirstOrDefault(x => x.Id == selectedItem.Id);

            var product = getProducts.FirstOrDefault(x => x.Id == postTransferStockProducts?.ProductId);
            if (product?.TraceAbility == true)
            {
                await SelectedBatch(postTransferStockProducts.Batch);
                postTransferStockProducts.TraceAvability = true;
            }
            else
            {
                await SelectedItemByProduct(product);
            }
        }


        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task RefreshItem_Click()
        {
            await LoadData();
        }
        #endregion

        #region Stage
        private async Task Request()
        {
            try
            {
                PanelVisible = true;


                if (postTransferStocks is null)
                {
                    ToastService.ShowError("Process ");
                    return;
                }
                postTransferStocks.Status = EnumStatusInternalTransfer.Request;
                var tempTransferStock = await Mediator.Send(new UpdateTransferStockRequest(postTransferStocks));

                //Save Log
                postTransferStockLogs.TransferStockId = tempTransferStock.Id;
                postTransferStockLogs.SourceId = tempTransferStock.SourceId;
                postTransferStockLogs.DestinationId = tempTransferStock.DestinationId;
                postTransferStockLogs.Status = EnumStatusInternalTransfer.Request;
                postTransferStockLogs.UserById = UserLogin.Id;

                await Mediator.Send(new CreateTransferStockLogRequest(postTransferStockLogs));

                PanelVisible = false;
                await LoadData();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Ready()
        {
            try
            {
                PanelVisible = true;
                if (postTransferStocks is null)
                {
                    ToastService.ShowError("");
                    return;
                }
                var cekReference = getTransactionStocks.Where(x => x.SourceTable == nameof(TransferStock)).OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                int NextReferenceNumber = 1;
                if (cekReference != null)
                {
                    int.TryParse(cekReference?.Substring("ITR#".Length), out NextReferenceNumber);
                    NextReferenceNumber++;
                }

                string referenceNumber = $"ITR#{NextReferenceNumber:D3}";
                var checkTranferProduct = getTransferStockProducts.Where(x => x.TransferStockId == postTransferStocks?.Id).ToList()!;
                foreach (var items in checkTranferProduct)
                {
                    var Cek_Uom = getUoms.Where(x => x.Id == items.Product?.UomId).FirstOrDefault();

                    // out
                    postTransactionStocks.SourceTable = nameof(TransferStock);
                    postTransactionStocks.SourcTableId = postTransferStocks.Id;
                    postTransactionStocks.ProductId = items.ProductId;
                    postTransactionStocks.LocationId = postTransferStocks.SourceId;
                    postTransactionStocks.Batch = items.Batch;
                    postTransactionStocks.ExpiredDate = items.ExpiredDate;
                    postTransactionStocks.Reference = referenceNumber;
                    postTransactionStocks.Quantity = -(items.QtyStock * Cek_Uom?.BiggerRatio?.ToLong()) ?? 0;
                    postTransactionStocks.UomId = items.Product?.UomId;
                    postTransactionStocks.Validate = false;

                    await Mediator.Send(new CreateTransactionStockRequest(postTransactionStocks));

                    //In

                    postTransactionStocks.SourceTable = nameof(TransferStock);
                    postTransactionStocks.SourcTableId = postTransferStocks.Id;
                    postTransactionStocks.LocationId = postTransferStocks.DestinationId;
                    postTransactionStocks.ProductId = items.ProductId;
                    postTransactionStocks.Batch = items.Batch;
                    postTransactionStocks.ExpiredDate = items.ExpiredDate;
                    postTransactionStocks.Reference = referenceNumber;
                    postTransactionStocks.Quantity = items.QtyStock * Cek_Uom?.BiggerRatio?.ToLong() ?? 0;
                    postTransactionStocks.UomId = items.Product?.UomId;
                    postTransactionStocks.Validate = false;

                    await Mediator.Send(new CreateTransactionStockRequest(postTransactionStocks));
                }

                //Save Status InternalTransfer
                postTransferStocks.Status = EnumStatusInternalTransfer.Ready;
                await Mediator.Send(new UpdateTransferStockRequest(postTransferStocks));


                //Save Log
                postTransferStockLogs.TransferStockId = postTransferStocks.Id;
                postTransferStockLogs.SourceId = postTransferStocks.SourceId;
                postTransferStockLogs.DestinationId = postTransferStocks.DestinationId;
                postTransferStockLogs.Status = EnumStatusInternalTransfer.Ready;
                postTransferStockLogs.UserById = UserLogin.Id;

                await Mediator.Send(new CreateTransferStockLogRequest(postTransferStockLogs));

                PanelVisible = false;
                await LoadData();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Approv_Check()
        {
            try
            {
                PanelVisible = true;
                if (postTransferStocks is null)
                {
                    return;
                }
                postTransferStocks.Status = EnumStatusInternalTransfer.ApproveRequest;
                await Mediator.Send(new UpdateTransferStockRequest(postTransferStocks));

                //Save Log
                postTransferStockLogs.TransferStockId = postTransferStocks.Id;
                postTransferStockLogs.SourceId = postTransferStocks.SourceId;
                postTransferStockLogs.DestinationId = postTransferStocks.DestinationId;
                postTransferStockLogs.Status = EnumStatusInternalTransfer.ApproveRequest;
                postTransferStockLogs.UserById = UserLogin.Id;

                await Mediator.Send(new CreateTransferStockLogRequest(postTransferStockLogs));

                PanelVisible = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task onDiscard()
        {
            await LoadData();
        }

        private async Task validation()
        {
            try
            {
                PanelVisible = true;

                var data_TransactionStock = getTransactionStocks.Where(x => x.SourceTable == nameof(TransferStock) && x.SourcTableId == postTransferStocks.Id).ToList();
                if (postTransferStocks is null)
                {
                    return;
                }
                foreach (var item in data_TransactionStock)
                {
                    item.Validate = true;
                    var aa = await Mediator.Send(new UpdateTransactionStockRequest(item));
                }

                postTransferStocks.Status = EnumStatusInternalTransfer.Done;
                await Mediator.Send(new UpdateTransferStockRequest(postTransferStocks));

                //Save Log
                postTransferStockLogs.TransferStockId = postTransferStocks.Id;
                postTransferStockLogs.SourceId = postTransferStocks.SourceId;
                postTransferStockLogs.DestinationId = postTransferStocks.DestinationId;
                postTransferStockLogs.Status = EnumStatusInternalTransfer.Done;
                postTransferStockLogs.UserById = UserLogin.Id;

                await Mediator.Send(new CreateTransferStockLogRequest(postTransferStockLogs));

                StateHasChanged();

                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Cancel()
        {
            try
            {
                var TransferProductStock = await Mediator.Send(new GetTransferStockProductQuery());


                if (postTransferStocks is null)
                {
                    return;
                }
                postTransferStocks.Status = EnumStatusInternalTransfer.Cancel;
                await Mediator.Send(new UpdateTransferStockRequest(postTransferStocks));

                //Save Log
                postTransferStockLogs.TransferStockId = postTransferStocks.Id;
                postTransferStockLogs.SourceId = postTransferStocks.SourceId;
                postTransferStockLogs.DestinationId = postTransferStocks.DestinationId;
                postTransferStockLogs.Status = EnumStatusInternalTransfer.Cancel;

                await Mediator.Send(new CreateTransferStockLogRequest(postTransferStockLogs));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion

    }
}

using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.GoodsReceiptCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Web.Components.Pages.Inventory.Maintenance
{
    public partial class CreateUpdateMaintenancePage
    {
        #region Relation data

        private List<MaintenanceDto> getMaintenance = [];
        private List<MaintenanceProductDto> getMaintenanceProduct = [];
        private List<UserDto> getResponsibleBy = [];
        private List<UserDto> getRequestBy = [];
        private List<ProductDto> getProduct = [];
        private List<LocationDto> getLocation = [];
        private List<TransactionStockDto> getTransactionStocks = [];
        private MaintenanceDto postMaintenance = new();
        private MaintenanceDto getMaintenanceById = new();
        private MaintenanceProductDto getMaintenanceProductById = new();
        private MaintenanceProductDto postMaintenanceProduct = new();
        private TransactionStockDto postTransactionStock = new();

        #endregion Relation data

        #region variable Static

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid Grid { get; set; }
        private Timer _timer;
        private long? TransactionId { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool PanelVisibleProduct { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private string? StatusString { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private DateTime? currentExpiryDate { get; set; }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }
        #endregion variable Static

        #region Boolean Data

        private void unCheckIN(bool newValue)
        {
            postMaintenance.isInternal = true;
            if (newValue)
                postMaintenance.isExternal = false;
            else
                postMaintenance.isInternal = false;
        }

        private void unCheckEX(bool newValue)
        {
            postMaintenance.isExternal = true;
            if (newValue)
                postMaintenance.isInternal = false;
            else
                postMaintenance.isExternal = false;
        }

        private void unCheckCR(bool newValue)
        {
            postMaintenance.isCorrective = true;
            if (newValue)
            {
                postMaintenance.isPreventive = false;
                postMaintenance.Recurrent = false;
            }
            else
            {
                postMaintenance.isCorrective = false;
            }
        }

        private void unCheckPR(bool newValue)
        {
            postMaintenance.isPreventive = true;
            if (newValue)
            {
                postMaintenance.isCorrective = false;
            }
            else
            {
                postMaintenance.isPreventive = false;
                postMaintenance.Recurrent = false;
            }
        }

        private void unCheckRE(bool newValue)
        {
            postMaintenance.Recurrent = true;
        }

        private List<string> Batch = [];

        private async Task selectByBatch(string value)
        {
            if (value is not null)
            {
                var result = getTransactionStocks.Where(x => x.Batch == value);
                currentExpiryDate = result.Select(x => x.ExpiredDate).FirstOrDefault();
                TransactionId = result.Select(x => x.Id).FirstOrDefault();
                if (postMaintenance.Recurrent == true)
                {
                    postMaintenanceProduct.Expired = Helper.CalculateNewExpiryDate(currentExpiryDate, postMaintenance.RepeatNumber, postMaintenance.RepeatWork);
                }
            }
        }

        private async Task selectByProduct(ProductDto e)
        {
            try
            {
                Batch.Clear();
                ResetFormProductDetail();

                if (e == null) return;

                postMaintenanceProduct.ProductId = e.Id;

                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == postMaintenance.LocationId));
                if (stockProducts.Count < 1)
                {
                    ToastService.ShowInfo("The product does not have Stock yet...");
                    return;
                }
                if (e.TraceAbility)
                {
                    var s = await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.Batch != null && x.Batch == postMaintenanceProduct.SerialNumber));
                    Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();

                    var firstStockProduct = stockProducts.Where(x => x.Batch == postMaintenanceProduct.SerialNumber);

                    UpdateFormProductDetail(firstStockProduct.FirstOrDefault() ?? new(), e);
                }
                else
                {
                    var s = (await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.LocationId == postMaintenance.LocationId)));

                    var firstStockProduct = stockProducts.FirstOrDefault();
                    postMaintenanceProduct.MaintenanceId = firstStockProduct?.Id ?? null;
                    UpdateFormProductDetail(firstStockProduct ?? new(), e);
                }

                return;
                var stockProducts2 = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == postMaintenance.LocationId));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            //if (postMaintenance.LocationId != 0 || postMaintenance.LocationId != null)
            //{
            //    var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == value.Id && s.LocationId == postMaintenance.LocationId));
            //    Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
            //    Batch = Batch.Distinct().ToList();
            //    currentExpiryDate = stockProducts?.Select(x=>x.ExpiredDate).FirstOrDefault();
            //}
            //else
            //{
            //    ToastService.ShowInfo("Select Location Or Location Not Null..");
            //    return;
            //}
        }

        private void ResetFormProductDetail()
        {
            currentExpiryDate = null;
            postMaintenanceProduct.ProductId = null;
            postMaintenanceProduct.SerialNumber = null;
        }

        private void UpdateFormProductDetail(TransactionStockDto stockProduct, ProductDto items)
        {
            if (stockProduct != null)
            {
                currentExpiryDate = stockProduct.ExpiredDate;
                if (currentExpiryDate is not null)
                {
                    postMaintenanceProduct.Expired = Helper.CalculateNewExpiryDate(currentExpiryDate, postMaintenance.RepeatNumber, postMaintenance.RepeatWork);
                }
            }
        }

        private async Task selectByLocation(LocationDto value)
        {
            postMaintenance.LocationId = value.Id;
        }

       

        #endregion Boolean Data

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

            //    await LoadDataLocation();
            //    await LoadDataProducts();
            //    await LoadDataRequestBy();
            //    await LoadDataResponsibleBy();
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

        #region Load data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            try
            {
                await GetUserInfo();

                var loadTasks = new[]
                {
                    LoadDataAsync(),
                    LoadDataProducts(),
                    LoadDataLocation(),
                    LoadDataRequestBy(),
                    LoadDataResponsibleBy(),
                    LoadData()
                };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task LoadDataAsync()
        {
            getTransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
        }
        private async Task LoadData()
        {

            await InvokeAsync(() => PanelVisible = true);
            try
            {
                var result = await Mediator.Send(new GetMaintenanceQuery(x => x.Id == Id, pageSize: 1, pageIndex: 0));
                postMaintenance = new();
                if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    if (result.Item1 == null || result.Item1.Count == 0 || !Id.HasValue)
                    {
                        NavigationManager.NavigateTo("inventory/Maintenance");
                        return;
                    }

                    postMaintenance = result.Item1.FirstOrDefault() ?? new();
                    await LoadDataDetail();
                }
            }
            finally
            {
                await InvokeAsync(() => PanelVisible = false);
            }

        }

        private async Task selectedStatus(long? Id, string value)
        {
            var DataProduct = getMaintenanceProduct.FirstOrDefault(x => x.Id == Id);

            if (DataProduct is not null)
            {
                // Mengupdate status produk berdasarkan value
                DataProduct.Status = value switch
                {
                    "Scrap" => EnumStatusMaintenance.Scrap,
                    "Done" => EnumStatusMaintenance.Done,
                    _ => DataProduct.Status
                };

                // Mengirim permintaan update untuk produk pemeliharaan
                await Mediator.Send(new UpdateMaintenanceProductRequest(DataProduct));

                // Jika status diubah menjadi "Scrap"
                if (value == "Scrap")
                {
                    PanelVisibleProduct = true;
                    getTransactionStocks = await Mediator.Send(new GetTransactionStockQuery());

                    var result = await Mediator.Send(new GetAllMaintenanceProductQuery());
                    getMaintenanceProduct = result.Where(x => x.Id == Id).ToList();

                    var cekReference = getTransactionStocks
                        .Where(x => x.SourceTable == nameof(Maintenance))
                        .OrderByDescending(x => x.SourcTableId)
                        .Select(z => z.Reference)
                        .FirstOrDefault();

                    int NextReferenceNumber = 1;
                    if (cekReference != null)
                    {
                        int.TryParse(cekReference.Substring("MNT#".Length), out NextReferenceNumber);
                        NextReferenceNumber++;
                    }
                    string referenceNumber = $"MNT#{NextReferenceNumber:D3}";

                    // Membuat entri transaksi untuk setiap produk yang di-maintenance
                    foreach (var items in getMaintenanceProduct)
                    {
                        var cekUom = getProduct.Where(x => x.Id == DataProduct.ProductId).Select(x => x.UomId).FirstOrDefault();

                        postTransactionStock.SourceTable = nameof(Maintenance);
                        postTransactionStock.SourcTableId = postMaintenance.Id;
                        postTransactionStock.ProductId = items.ProductId;
                        postTransactionStock.LocationId = postMaintenance.LocationId;
                        postTransactionStock.Batch = items.SerialNumber;
                        postTransactionStock.ExpiredDate = items.Expired;
                        postTransactionStock.Reference = referenceNumber;
                        postTransactionStock.Quantity = -1;
                        postTransactionStock.UomId = cekUom;
                        postTransactionStock.Validate = true;

                        await Mediator.Send(new CreateTransactionStockRequest(postTransactionStock));
                    }

                    // Menyembunyikan panel setelah transaksi
                    PanelVisibleProduct = false;
                }
            }

            // Mengupdate status pemeliharaan jika semua produk terkait sudah diupdate
            if (postMaintenance.Status == EnumStatusMaintenance.InProgress)
            {
                var AllProduct = getMaintenanceProduct.Where(x => x.MaintenanceId == postMaintenance.Id).ToList();

                if (AllProduct.All(x => x.Status != null))
                {
                    postMaintenance.Status = EnumStatusMaintenance.Done;
                    await Mediator.Send(new UpdateMaintenanceRequest(postMaintenance));
                }
            }

            // Memuat ulang data detail setelah semua perubahan
            await LoadDataDetail();
        }


        private async Task LoadDataDetail(int pageIndex = 0, int pageSize = 10)
        {
            await InvokeAsync(() => PanelVisibleProduct = true);

            try
            {
                var result = await Mediator.Send(new GetMaintenanceProductQuery
                {
                  Predicate =  x => x.MaintenanceId == postMaintenance.Id
                });
                getMaintenanceProduct = result.Item1;
                totalCount = result.PageCount;
                await HandlerData();
            }
            finally
            {
                await InvokeAsync(() => PanelVisibleProduct = false);
            }
        }

        private async Task HandlerData()
        {


            foreach (var item in getMaintenanceProduct)
            {
                postMaintenanceProduct = item ?? new();
            }
        }

        private List<string> listString = [
            "Scrap",
            "Done"
            ];

        #endregion Load data

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            //await LoadLabTestDetails(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            //await LoadLabTestDetails(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            //await LoadLabTestDetails(newPageIndex, pageSize);
        }

        #endregion Searching

        #region Load ComboBox

        #region ComboBox Product

        private DxComboBox<ProductDto, long?> refProductsComboBox { get; set; }
        private int ProductsComboBoxIndex { get; set; } = 0;
        private int totalCountProducts = 0;

        private async Task OnSearchProducts()
        {
            await LoadDataProducts(0, 10);
        }

        private async Task OnSearchProductsIndexIncrement()
        {
            if (ProductsComboBoxIndex < (totalCountProducts - 1))
            {
                ProductsComboBoxIndex++;
                await LoadDataProducts(ProductsComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProductsIndexDecrement()
        {
            if (ProductsComboBoxIndex > 0)
            {
                ProductsComboBoxIndex--;
                await LoadDataProducts(ProductsComboBoxIndex, 10);
            }
        }

        private async Task OnInputProductsChanged(string e)
        {
            ProductsComboBoxIndex = 0;
            await LoadDataProducts(0, 10);
        }

        private async Task LoadDataProducts(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProductQuery(searchTerm: refProductsComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getProduct = result.Item1.Where(x => x.HospitalType == "Medical Equipment").ToList();
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Product

        #region Combo Box Request By

        private DxComboBox<UserDto, long?> refRequestByComboBox { get; set; }
        private int RequestByComboBoxIndex { get; set; } = 0;
        private int totalCountRequestBy = 0;

        private async Task OnSearchRequestBy()
        {
            await LoadDataRequestBy(0, 10);
        }

        private async Task OnSearchRequestByIndexIncrement()
        {
            if (RequestByComboBoxIndex < (totalCountRequestBy - 1))
            {
                RequestByComboBoxIndex++;
                await LoadDataRequestBy(RequestByComboBoxIndex, 10);
            }
        }

        private async Task OnSearchRequestByIndexDecrement()
        {
            if (RequestByComboBoxIndex > 0)
            {
                RequestByComboBoxIndex--;
                await LoadDataRequestBy(RequestByComboBoxIndex, 10);
            }
        }

        private async Task OnInputRequestByChanged(string e)
        {
            RequestByComboBoxIndex = 0;
            await LoadDataRequestBy(0, 10);
        }

        private async Task LoadDataRequestBy(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetUserQuery2(searchTerm: refRequestByComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getRequestBy = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion Combo Box Request By

        #region Combo Box Location

        private DxComboBox<LocationDto, long?> refLocationComboBox { get; set; }
        private int LocationComboBoxIndex { get; set; } = 0;
        private int totalCountLocation = 0;

        private async Task OnSearchLocation()
        {
            await LoadDataLocation(0, 10);
        }

        private async Task OnSearchLocationIndexIncrement()
        {
            if (LocationComboBoxIndex < (totalCountLocation - 1))
            {
                LocationComboBoxIndex++;
                await LoadDataLocation(LocationComboBoxIndex, 10);
            }
        }

        private async Task OnSearchLocationIndexDecrement()
        {
            if (LocationComboBoxIndex > 0)
            {
                LocationComboBoxIndex--;
                await LoadDataLocation(LocationComboBoxIndex, 10);
            }
        }

        private async Task OnInputLocationChanged(string e)
        {
            LocationComboBoxIndex = 0;
            await LoadDataLocation(0, 10);
        }

        private async Task LoadDataLocation(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLocationQuery
            {
                SearchTerm = refLocationComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            activePageIndex = pageIndex;
            totalCount = result.PageCount;
            getLocation = result.Item1;
            PanelVisible = false;
        }

        #endregion Combo Box Location

        #region Combo Box Responsible

        private DxComboBox<UserDto, long?> refResponsibleByComboBox { get; set; }
        private int ResponsibleByComboBoxIndex { get; set; } = 0;
        private int totalCountResponsibleBy = 0;

        private async Task OnSearchResponsibleBy()
        {
            await LoadDataResponsibleBy(0, 10);
        }

        private async Task OnSearchResponsibleByIndexIncrement()
        {
            if (ResponsibleByComboBoxIndex < (totalCountResponsibleBy - 1))
            {
                ResponsibleByComboBoxIndex++;
                await LoadDataResponsibleBy(ResponsibleByComboBoxIndex, 10);
            }
        }

        private async Task OnSearchResponsibleByIndexDecrement()
        {
            if (ResponsibleByComboBoxIndex > 0)
            {
                ResponsibleByComboBoxIndex--;
                await LoadDataResponsibleBy(ResponsibleByComboBoxIndex, 10);
            }
        }

        private async Task OnInputResponsibleByChanged(string e)
        {
            ResponsibleByComboBoxIndex = 0;
            await LoadDataResponsibleBy(0, 10);
        }

        private async Task LoadDataResponsibleBy(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetUserQuery2(searchTerm: refResponsibleByComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getResponsibleBy = result.Item1.Where(x => x.IsEmployee == true).ToList();
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion Combo Box Responsible

        #endregion Load ComboBox

        #region function step

        private async Task InProgress_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintenance.Status = EnumStatusMaintenance.InProgress;
                getMaintenanceById = await Mediator.Send(new UpdateMaintenanceRequest(postMaintenance));
                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/Maintenance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintenanceById.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Repaired_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintenance.Status = EnumStatusMaintenance.Repaired;
                getMaintenanceById = await Mediator.Send(new UpdateMaintenanceRequest(postMaintenance));
                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/Maintenance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintenanceById.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Scrap_Click()
        {
            try
            {
                PanelVisible = true;
                getTransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                var result = await Mediator.Send(new GetMaintenanceProductQuery());
                getMaintenanceProduct = result.Item1;
                var cekReference = getTransactionStocks.Where(x => x.SourceTable == nameof(Maintenance))
                           .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                int NextReferenceNumber = 1;
                if (cekReference != null)
                {
                    int.TryParse(cekReference?.Substring("MNT#".Length), out NextReferenceNumber);
                    NextReferenceNumber++;
                }
                string referenceNumber = $"MNT#{NextReferenceNumber:D3}";

                foreach (var items in getMaintenanceProduct)
                {
                    var cekUom = getProduct.Where(x => x.Id == postMaintenanceProduct.ProductId).Select(x => x.UomId).FirstOrDefault();

                    postTransactionStock.SourceTable = nameof(Maintenance);
                    postTransactionStock.SourcTableId = postMaintenance.Id;
                    postTransactionStock.ProductId = items.ProductId;
                    postTransactionStock.LocationId = postMaintenance.LocationId;
                    postTransactionStock.Batch = items.SerialNumber;
                    postTransactionStock.ExpiredDate = null;
                    postTransactionStock.Reference = referenceNumber;
                    postTransactionStock.Quantity = -1;
                    postTransactionStock.UomId = cekUom;
                    postTransactionStock.Validate = true;

                    await Mediator.Send(new CreateTransactionStockRequest(postTransactionStock));
                }

                //update Maintenance
                postMaintenance.Status = EnumStatusMaintenance.Scrap;
                getMaintenanceProductById = await Mediator.Send(new UpdateMaintenanceProductRequest(postMaintenanceProduct));
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Done_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintenanceProduct.Status = EnumStatusMaintenance.Done;
                getMaintenanceProductById = await Mediator.Send(new UpdateMaintenanceProductRequest(postMaintenanceProduct));
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Cancel_Click()
        { }

        private async Task onDiscard()
        {
            NavigationManager.NavigateTo($"inventory/Maintenance/");
            StateHasChanged();
        }

        #endregion function step

        #region Click Button
        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }
        private async Task EditItem_Click(IGrid context)
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            postMaintenanceProduct = (MaintenanceProductDto)context.SelectedDataItem;

        }
        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
        private async Task Refresh_Click()
        {
            await LoadDataDetail();
        }
        #endregion

        #region save
        private string Message = string.Empty;
        private async Task OnSave()
        {
            try
            {
                PanelVisible = true;

                if (postMaintenance is null || postMaintenance.LocationId is null)
                {
                    ToastService.ShowError("Maintenance data is null.");
                    Message = "Location Is Not Null";
                    return;
                }

                // Generate sequence if this is a new Maintenance entry
                if (postMaintenance.Id == 0)
                {
                    string prefix = "MNT-";
                    string datePart = DateTime.Now.ToString("ddMMyy");

                    // Find the last sequence for the current month and year
                    var lastSequence = getMaintenance
                        .Where(x => x.Sequence != null && x.Sequence.Substring(7, 4) == DateTime.Now.ToString("MMyy"))
                        .OrderByDescending(x => x.Sequence)
                        .FirstOrDefault();

                    int nextSequence = 1;

                    // Extract and increment sequence number if it exists
                    if (lastSequence?.Sequence != null)
                    {
                        var lastNumberPart = lastSequence.Sequence.Substring(lastSequence.Sequence.Length - 3);
                        nextSequence = int.Parse(lastNumberPart) + 1;
                    }

                    postMaintenance.Sequence = $"{prefix}{datePart}-{nextSequence:D3}";
                    postMaintenance.Status = EnumStatusMaintenance.Request;

                    // Create new Maintenance entry
                    getMaintenanceById = await Mediator.Send(new CreateMaintenanceRequest(postMaintenance));
                    ToastService.ShowSuccess("Save Data Success.");
                }
                else
                {
                    // Update existing Maintenance entry
                    getMaintenanceById = await Mediator.Send(new UpdateMaintenanceRequest(postMaintenance));
                    ToastService.ShowSuccess("Update Data Success.");
                }

                // Navigate after successful save/update
                NavigationManager.NavigateTo($"inventory/Maintenance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintenanceById.Id}", true);
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
        private async Task OnSaveProduct()
        {
            try
            {
                if (postMaintenanceProduct is null || postMaintenance is null)
                {
                    ToastService.ShowError("Maintenance product or main data is null.");
                    return;
                }

                // Create a new Maintenance product
                if (postMaintenanceProduct.Id == 0)
                {
                    postMaintenanceProduct.MaintenanceId = postMaintenance.Id;
                    if (postMaintenance.Recurrent == false)
                    {
                        postMaintenanceProduct.Expired = currentExpiryDate;
                    }
                    var tempDataProduct = await Mediator.Send(new CreateMaintenanceProductRequest(postMaintenanceProduct));

                    // If Maintenance is recurrent, update transaction stock's expired date
                    if (postMaintenance.Recurrent == true && tempDataProduct != null)
                    {
                        var DataTransaction = getTransactionStocks.FirstOrDefault(x => x.Id == TransactionId);

                        if (DataTransaction != null)
                        {
                            DataTransaction.ExpiredDate = tempDataProduct.Expired;
                            await Mediator.Send(new UpdateTransactionStockRequest(DataTransaction));
                        }

                        ToastService.ShowSuccess("Save Data Product Success.");
                    }
                }
                else
                {
                    // Update existing Maintenance product
                    await Mediator.Send(new UpdateMaintenanceProductRequest(postMaintenanceProduct));
                    ToastService.ShowSuccess("Update Data Product Success.");
                }

                // Reload the details after save/update
                await LoadDataDetail();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }
        #endregion save

        #region Delete
        private async Task DeleteProduct(GridDataItemDeletingEventArgs e)
        {
            try
            {
                // Casting sekali untuk data yang dihapus
                var data = (MaintenanceProductDto)e.DataItem;

                // Jika tidak ada item yang dipilih, atau hanya satu item yang dipilih, hapus item tunggal
                if (SelectedDataItems is null || SelectedDataItems.Count <= 1)
                {
                    await Mediator.Send(new DeleteMaintenanceProductRequest(data.Id));
                }
                else
                {
                    // Adaptasi SelectedDataItems menjadi list of MaintenanceProductDto
                    var selectedProducts = SelectedDataItems.Adapt<List<MaintenanceProductDto>>();
                    var idsToDelete = selectedProducts.Select(x => x.Id).ToList();

                    // Mengirim permintaan penghapusan untuk banyak item
                    await Mediator.Send(new DeleteMaintenanceProductRequest(ids: idsToDelete));
                }

                // Kosongkan SelectedDataItems setelah penghapusan
                SelectedDataItems = [];

                // Muat ulang data setelah penghapusan
                await LoadDataDetail();

                ToastService.ShowSuccess("Product(s) deleted successfully.");
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Error deleting product(s): {ex.Message}");
            }
        }

        #endregion

        #region Handler Vaidation

        private async Task HandleValidSubmit()
        {
            //IsLoading = true;
            FormValidationState = true;
            await OnSave();
            //IsLoading = false;
        }

        private async Task HandleInvalidSubmit()
        {

            FormValidationState = false;
        }

        #endregion Handler Vaidation
    }
}
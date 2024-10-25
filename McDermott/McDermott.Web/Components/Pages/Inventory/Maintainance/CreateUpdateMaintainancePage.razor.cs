using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.GoodsReceiptCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Web.Components.Pages.Inventory.Maintainance
{
    public partial class CreateUpdateMaintainancePage
    {
        #region Relation data

        private List<MaintainanceDto> getMaintainance = [];
        private List<MaintainanceProductDto> getMaintainanceProduct = [];
        private List<UserDto> getResponsibleBy = [];
        private List<UserDto> getRequestBy = [];
        private List<ProductDto> getProduct = [];
        private List<LocationDto> getLocation = [];
        private List<TransactionStockDto> getTransactionStocks = [];
        private MaintainanceDto postMaintainance = new();
        private MaintainanceDto getMaintainanceById = new();
        private MaintainanceProductDto getMaintainanceProductById = new();
        private MaintainanceProductDto postMaintainanceProduct = new();
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
            postMaintainance.isInternal = true;
            if (newValue)
                postMaintainance.isExternal = false;
            else
                postMaintainance.isInternal = false;
        }

        private void unCheckEX(bool newValue)
        {
            postMaintainance.isExternal = true;
            if (newValue)
                postMaintainance.isInternal = false;
            else
                postMaintainance.isExternal = false;
        }

        private void unCheckCR(bool newValue)
        {
            postMaintainance.isCorrective = true;
            if (newValue)
            {
                postMaintainance.isPreventive = false;
                postMaintainance.Recurrent = false;
            }
            else
            {
                postMaintainance.isCorrective = false;
            }
        }

        private void unCheckPR(bool newValue)
        {
            postMaintainance.isPreventive = true;
            if (newValue)
            {
                postMaintainance.isCorrective = false;
            }
            else
            {
                postMaintainance.isPreventive = false;
                postMaintainance.Recurrent = false;
            }
        }

        private void unCheckRE(bool newValue)
        {
            postMaintainance.Recurrent = true;
        }

        private List<string> Batch = [];

        private async Task selectByBatch(string value)
        {
            if (value is not null)
            {
                var result = getTransactionStocks.Where(x => x.Batch == value);
                currentExpiryDate = result.Select(x => x.ExpiredDate).FirstOrDefault();
                TransactionId = result.Select(x => x.Id).FirstOrDefault();
                if (postMaintainance.Recurrent == true)
                {
                    postMaintainanceProduct.Expired = Helper.CalculateNewExpiryDate(currentExpiryDate, postMaintainance.RepeatNumber, postMaintainance.RepeatWork);
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

                postMaintainanceProduct.ProductId = e.Id;

                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == postMaintainance.LocationId));
                if (stockProducts.Count < 1)
                {
                    ToastService.ShowInfo("The product does not have Stock yet...");
                    return;
                }
                if (e.TraceAbility)
                {
                    var s = await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.Batch != null && x.Batch == postMaintainanceProduct.SerialNumber));
                    Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();

                    var firstStockProduct = stockProducts.Where(x => x.Batch == postMaintainanceProduct.SerialNumber);

                    UpdateFormProductDetail(firstStockProduct.FirstOrDefault() ?? new(), e);
                }
                else
                {
                    var s = (await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.LocationId == postMaintainance.LocationId)));

                    var firstStockProduct = stockProducts.FirstOrDefault();
                    postMaintainanceProduct.MaintainanceId = firstStockProduct?.Id ?? null;
                    UpdateFormProductDetail(firstStockProduct ?? new(), e);
                }

                return;
                var stockProducts2 = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == postMaintainance.LocationId));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            //if (postMaintainance.LocationId != 0 || postMaintainance.LocationId != null)
            //{
            //    var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == value.Id && s.LocationId == postMaintainance.LocationId));
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
            postMaintainanceProduct.ProductId = null;
            postMaintainanceProduct.SerialNumber = null;
        }

        private void UpdateFormProductDetail(TransactionStockDto stockProduct, ProductDto items)
        {
            if (stockProduct != null)
            {
                currentExpiryDate = stockProduct.ExpiredDate;
                if (currentExpiryDate is not null)
                {
                    postMaintainanceProduct.Expired = Helper.CalculateNewExpiryDate(currentExpiryDate, postMaintainance.RepeatNumber, postMaintainance.RepeatWork);
                }
            }
        }

        private async Task selectByLocation(LocationDto value)
        {
            postMaintainance.LocationId = value.Id;
        }

        private List<string> RepeatWork = new List<string>()
        {
            "Days",
            "Weeks",
            "Months",
            "Years"
        };

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
                var result = await Mediator.Send(new GetMaintainanceQuery(x => x.Id == Id, pageSize: 1, pageIndex: 0));
                postMaintainance = new();
                if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    if (result.Item1 == null || result.Item1.Count == 0 || !Id.HasValue)
                    {
                        NavigationManager.NavigateTo("inventory/maintainance");
                        return;
                    }

                    postMaintainance = result.Item1.FirstOrDefault() ?? new();
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
            var DataProduct = getMaintainanceProduct.FirstOrDefault(x => x.Id == Id);

            if (DataProduct is not null)
            {
                DataProduct.Status = value switch
                {
                    "Scrap" => EnumStatusMaintainance.Scrap,
                    "Done" => EnumStatusMaintainance.Done,
                    _ => DataProduct.Status
                };

                if (value == "Scrap" || value == "Done")
                {
                    await Mediator.Send(new UpdateMaintainanceProductRequest(DataProduct));
                }
            }

            await LoadDataDetail();
        }


        private async Task LoadDataDetail(int pageIndex = 0, int pageSize = 10)
        {
            await InvokeAsync(() => PanelVisible = true);

            try
            {
                var result = await Mediator.Send(new GetMaintainanceProductQuery(x => x.MaintainanceId == postMaintainance.Id, pageIndex: pageIndex, pageSize: pageSize));
                getMaintainanceProduct = result.Item1;
                totalCount = result.pageCount;
                await HandlerData();
            }
            finally
            {
                await InvokeAsync(() => PanelVisible = false);
            }
        }

        private async Task HandlerData()
        {
            StatusString = listString[0];

            foreach(var item in getMaintainanceProduct)
            {
                postMaintainanceProduct = item ?? new();
            }
        }

        private List<string> listString = [
            "Repair",
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
                postMaintainance.Status = EnumStatusMaintainance.InProgress;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/maintainance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintainanceById.Id}");
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
                postMaintainance.Status = EnumStatusMaintainance.Repaired;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/maintainance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintainanceById.Id}");
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
                var result = await Mediator.Send(new GetMaintainanceProductQuery(searchTerm: searchTerm, pageSize: 0, pageIndex: 1));
                getMaintainanceProduct = result.Item1;
                var cekReference = getTransactionStocks.Where(x => x.SourceTable == nameof(Maintainance))
                           .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                int NextReferenceNumber = 1;
                if (cekReference != null)
                {
                    int.TryParse(cekReference?.Substring("MNT#".Length), out NextReferenceNumber);
                    NextReferenceNumber++;
                }
                string referenceNumber = $"MNT#{NextReferenceNumber:D3}";

                foreach (var items in getMaintainanceProduct)
                {
                    var cekUom = getProduct.Where(x => x.Id == postMaintainanceProduct.ProductId).Select(x => x.UomId).FirstOrDefault();

                    postTransactionStock.SourceTable = nameof(Maintainance);
                    postTransactionStock.SourcTableId = postMaintainance.Id;
                    postTransactionStock.ProductId = items.ProductId;
                    postTransactionStock.LocationId = postMaintainance.LocationId;
                    postTransactionStock.Batch = items.SerialNumber;
                    postTransactionStock.ExpiredDate = null;
                    postTransactionStock.Reference = referenceNumber;
                    postTransactionStock.Quantity = -1;
                    postTransactionStock.UomId = cekUom;
                    postTransactionStock.Validate = true;

                    await Mediator.Send(new CreateTransactionStockRequest(postTransactionStock));
                }

                //update Maintainance
                postMaintainance.Status = EnumStatusMaintainance.Scrap;
                getMaintainanceProductById = await Mediator.Send(new UpdateMaintainanceProductRequest(postMaintainanceProduct));
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
                postMaintainanceProduct.Status = EnumStatusMaintainance.Done;
                getMaintainanceProductById = await Mediator.Send(new UpdateMaintainanceProductRequest(postMaintainanceProduct));
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
            NavigationManager.NavigateTo($"inventory/maintainance/");
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
            postMaintainanceProduct = (MaintainanceProductDto)context.SelectedDataItem;

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

        private async Task OnSave()
        {
            try
            {
                PanelVisible = true;

                if (postMaintainance is null)
                {
                    ToastService.ShowError("Maintainance data is null.");
                    return;
                }

                // Generate sequence if this is a new maintainance entry
                if (postMaintainance.Id == 0)
                {
                    string prefix = "MNT-";
                    string datePart = DateTime.Now.ToString("ddMMyy");

                    // Find the last sequence for the current month and year
                    var lastSequence = getMaintainance
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

                    postMaintainance.Sequence = $"{prefix}{datePart}-{nextSequence:D3}";
                    postMaintainance.Status = EnumStatusMaintainance.Request;

                    // Create new maintainance entry
                    getMaintainanceById = await Mediator.Send(new CreateMaintainanceRequest(postMaintainance));
                    ToastService.ShowSuccess("Save Data Success.");
                }
                else
                {
                    // Update existing maintainance entry
                    getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                    ToastService.ShowSuccess("Update Data Success.");
                }

                // Navigate after successful save/update
                NavigationManager.NavigateTo($"inventory/maintainance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintainanceById.Id}", true);
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
                if (postMaintainanceProduct is null || postMaintainance is null)
                {
                    ToastService.ShowError("Maintainance product or main data is null.");
                    return;
                }

                // Create a new maintainance product
                if (postMaintainanceProduct.Id == 0)
                {
                    postMaintainanceProduct.MaintainanceId = postMaintainance.Id;
                    var tempDataProduct = await Mediator.Send(new CreateMaintainanceProductRequest(postMaintainanceProduct));

                    // If maintainance is recurrent, update transaction stock's expired date
                    if (postMaintainance.Recurrent == true && tempDataProduct != null)
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
                    // Update existing maintainance product
                    await Mediator.Send(new UpdateMaintainanceProductRequest(postMaintainanceProduct));
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
                var data = (MaintainanceProductDto)e.DataItem;

                // Jika tidak ada item yang dipilih, atau hanya satu item yang dipilih, hapus item tunggal
                if (SelectedDataItems is null || SelectedDataItems.Count <= 1)
                {
                    await Mediator.Send(new DeleteMaintainanceProductRequest(data.Id));
                }
                else
                {
                    // Adaptasi SelectedDataItems menjadi list of MaintainanceProductDto
                    var selectedProducts = SelectedDataItems.Adapt<List<MaintainanceProductDto>>();
                    var idsToDelete = selectedProducts.Select(x => x.Id).ToList();

                    // Mengirim permintaan penghapusan untuk banyak item
                    await Mediator.Send(new DeleteMaintainanceProductRequest(ids: idsToDelete));
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
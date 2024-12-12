using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Web.Extentions;
using System.Linq.Expressions;
using DevExpress.Blazor.Upload;
using static McDermott.Application.Features.Commands.GetDataCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using System.IO.Compression;
using System.Net;
using McDermott.Domain.Entities;
using McDermott.Application.Dtos.AwarenessEvent;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceRecordCommand;
using static System.Net.WebRequestMethods;
using System.Security.Policy;
using System.Reflection.Metadata;

namespace McDermott.Web.Components.Pages.Inventory.Maintenances
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
        private object DataDetailProduct { get; set; }
        private List<TransactionStockDto> getTransactionStocks = [];
        private MaintenanceDto postMaintenance = new();
        private MaintenanceDto getMaintenanceById = new();
        private MaintenanceProductDto getMaintenanceProductById = new();
        private MaintenanceProduct postMaintenanceProduct { get; set; } = new();
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
        private bool showPopUpUpload { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool IsReadOnly => postMaintenance.Id != 0 && postMaintenance.Status != EnumStatusMaintenance.Request;
        private string? StatusString { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private DateTime? currentExpiryDate { get; set; }
        private int FocusedRowVisibleIndex { get; set; }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            try
            {
                if ((MaintenanceProduct)args.DataItem is null)
                    return;

                isActiveButton = ((MaintenanceProduct)args.DataItem)!.Status != null;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

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


            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

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
                    LoadProduct(),
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
                var result = await Mediator.Send(new GetSingleMaintenanceQuery
                {
                    Predicate = x => x.Id == Id
                });
                postMaintenance = new();
                if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    if (result == null || !Id.HasValue)
                    {
                        NavigationManager.NavigateTo("inventory/Maintenance");
                        return;
                    }

                    postMaintenance = result ?? new();
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
            var DataProduct = await Mediator.Send(new GetSingleMaintenanceProductQuery
            {
                Predicate = x => x.Id == Id,
            });

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

                    var result = await Mediator.Send(new GetMaintenanceProductQuery
                    {
                        Predicate = x => x.Id == Id,
                    });
                    getMaintenanceProduct = result.Item1;

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
                DataDetailProduct = new GridDevExtremeDataSource<MaintenanceProduct>(await Mediator.Send(new GetQueryMaintenanceProduct()))
                {
                    CustomizeLoadOptions = (loadOptions) =>
                    {
                        loadOptions.PrimaryKey = ["Id"];
                        loadOptions.PaginateViaPrimaryKey = true;
                    }
                };
                Console.WriteLine(DataDetailProduct);
            }
            finally
            {
                await InvokeAsync(() => PanelVisibleProduct = false);
            }
        }

        public MarkupString GetIssueStatusIconHtml(EnumStatusMaintenance? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusMaintenance.Request:
                    priorityClass = "info";
                    title = "Request";
                    break;

                case EnumStatusMaintenance.InProgress:
                    priorityClass = "primary";
                    title = "In Progress";
                    break;

                case EnumStatusMaintenance.Repaired:
                    priorityClass = "warning";
                    title = "Repaire";
                    break;

                case EnumStatusMaintenance.Scrap:
                    priorityClass = "warning";
                    title = "Scrap";
                    break;

                case EnumStatusMaintenance.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusMaintenance.Canceled:
                    priorityClass = "danger";
                    title = "Cancel";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
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

        private CancellationTokenSource? _cts;

        #region ComboBox Location

        private LocationDto SelectedLocation { get; set; } = new();

        private async Task SelectedItemChangedDestination(LocationDto e)
        {
            if (e is null)
            {
                SelectedLocation = new();
                await LoadDataLocation(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedLocation = e;
        }

        private async Task OnInputLocation(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadDataLocation(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadDataLocation(string? e = "", Expression<Func<Locations, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                getLocation = await Mediator.QueryGetComboBox<Locations, LocationDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Location

        #region ComboBox Product

        private ProductDto SelectedProduct { get; set; } = new();

        private async Task SelectedItemChanged(ProductDto e)
        {
            if (e is null)
            {
                SelectedProduct = new();
                await LoadProduct(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedProduct = e;
        }


        private async Task OnInputProduct(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadProduct(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadProduct(string? e = "", Expression<Func<Product, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                getProduct = await Mediator.QueryGetComboBox<Product, ProductDto>(e, predicate = x => x.HospitalType == "Medical Equipment");
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Product

        #region Combo Box Request By
        private UserDto SelectedRequestBy { get; set; } = new();

        private async Task SelectedItemChanged(UserDto e)
        {
            if (e is null)
            {
                SelectedRequestBy = new();
                await LoadDataRequestBy(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedRequestBy = e;
        }


        private async Task OnInputRequestBy(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadDataRequestBy(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadDataRequestBy(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                getRequestBy = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate = x => x.IsEmployee);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Combo Box Request By

        #region Combo Box Responsible

        private UserDto SelectedResponsibleBy { get; set; } = new();

        private async Task SelectedItemResponsibleChanged(UserDto e)
        {
            if (e is null)
            {
                SelectedResponsibleBy = new();
                await LoadDataResponsibleBy(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedResponsibleBy = e;
        }


        private async Task OnInputResponsibleBy(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadDataResponsibleBy(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadDataResponsibleBy(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                getResponsibleBy = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate = x => x.IsEmployee);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
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
                getMaintenanceProductById = await Mediator.Send(new UpdateMaintenanceProductRequest(postMaintenanceProduct.Adapt<MaintenanceProductDto>()));
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
                getMaintenanceProductById = await Mediator.Send(new UpdateMaintenanceProductRequest(postMaintenanceProduct.Adapt<MaintenanceProductDto>()));
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
            // Memulai edit baris berdasarkan indeks
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);

            // Ambil data MaintenanceProduct dari Grid
            postMaintenanceProduct = Grid.GetDataItem(FocusedRowVisibleIndex) as MaintenanceProduct ?? new();

            // Cek apakah ada tanggal kadaluarsa
            if (postMaintenance.Recurrent == false)
            {
                currentExpiryDate = postMaintenanceProduct.Expired;

            }
            else
            {

                // Ambil detail produk jika tidak ada tanggal kadaluarsa
                ProductDto productData = await Mediator.Send(new GetSingleProductQueryNew
                {
                    Predicate = x => x.Id == postMaintenanceProduct.ProductId
                });

                if (productData != null)
                {
                    // Panggil metode selectByProduct untuk memproses data produk
                    var stockProducts = await Mediator.Send(new GetSingleTransactionStockQueryNew
                    {
                        Predicate = s => s.ProductId == productData.Id && s.LocationId == postMaintenance.LocationId && s.Batch == postMaintenanceProduct.SerialNumber,
                        Select = x => new TransactionStock
                        {
                            Batch = x.Batch,
                            ExpiredDate = x.ExpiredDate,
                            ProductId = x.ProductId,

                        }
                    });


                    currentExpiryDate = stockProducts.ExpiredDate;
                }
            }
        }


        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadDataDetail();
        }

        #endregion Click Button

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

        private async Task OnSaveProduct(GridEditModelSavingEventArgs e)
        {
            try
            {
                var dataItems = (MaintenanceProduct)e.EditModel;
                if (dataItems is null || dataItems is null)
                {
                    ToastService.ShowError("Maintenance product or main data is null.");
                    return;
                }

                // Create a new Maintenance product
                if (dataItems.Id == 0)
                {
                    postMaintenanceProduct.MaintenanceId = postMaintenance.Id;
                    if (postMaintenance.Recurrent == false)
                    {
                        postMaintenanceProduct.Expired = currentExpiryDate;
                    }
                    var tempDataProduct = await Mediator.Send(new CreateMaintenanceProductRequest(dataItems.Adapt<MaintenanceProductDto>()));

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
                    await Mediator.Send(new UpdateMaintenanceProductRequest(dataItems.Adapt<MaintenanceProductDto>()));
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

        #endregion Delete

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

        #region Upload Document
        private int SelectedFilesCount = 0;
        private long? productId { get; set; }
        private long? maintenanceId { get; set; }
        private string? sequenceNumber { get; set; }
        private bool UploadSuccess = false;  // Status upload sukses

        private string UploadErrorMessage = string.Empty;
        private string UploadSuccessMessage = string.Empty;

        private async Task OpenPopUp(MaintenanceProduct dataDoc)
        {
            var cekDataMaintenance = await Mediator.Send(new GetSingleMaintenanceQuery
            {
                Predicate = x => x.Id == dataDoc.MaintenanceId,
            });
            productId = dataDoc.ProductId;
            maintenanceId = dataDoc.MaintenanceId;
            sequenceNumber = cekDataMaintenance.Sequence;
            showPopUpUpload = true;
        }

        private async Task OnPopupClosed()
        {
            showPopUpUpload = false;
            await LoadDataDetail();
            StateHasChanged();
        }


        protected void OnSelectedFilesChanged(IEnumerable<UploadFileInfo> files)
        {
            SelectedFilesCount = files.ToList().Count;
            UploadSuccess = false; // Reset status sukses saat file baru dipilih
            UploadErrorMessage = string.Empty;
            InvokeAsync(StateHasChanged);
        }



        protected string GetUploadUrl(string url)
        {
            return $"{url}?productId={productId}&maintenanceId={maintenanceId}&sequenceNumber={sequenceNumber}";
        }
        protected async Task GetUploadUrlsx(string url)
        {
            var link = $"{url}?productId={productId}&maintenanceId={maintenanceId}&sequenceNumber={sequenceNumber}";
            var succes = NavigationManager.ToAbsoluteUri(link).AbsoluteUri;


        }

        #endregion

        #region Read Document
        private bool showPopUpFile { get; set; } = false;
        private bool isLoadingFile { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItemsFile { get; set; } = [];
        private object eData { get; set; }
        private MaintenanceProduct zsh { get; set; }

        private async Task OpenFile(MaintenanceProduct e)
        {
            zsh = e;
            await LoadDataFile(e);
            showPopUpFile = true;
        }

        private async Task LoadDataFile(MaintenanceProduct f)
        {
            await InvokeAsync(() => isLoadingFile = true);

            try
            {
                var xsx = await Mediator.Send(new GetSingleMaintenanceQuery
                {
                    Predicate = x => x.Id == f.MaintenanceId,
                });

                eData = new GridDevExtremeDataSource<MaintenanceRecord>(await Mediator.Send(new GetQueryMaintenanceRecord
                {
                    Predicate = x => x.ProductId == f.ProductId && x.MaintenanceId == xsx.Id && x.SequenceProduct == xsx.Sequence,

                }))
                {
                    CustomizeLoadOptions = (loadOptions) =>
                    {
                        loadOptions.PrimaryKey = ["Id"];
                        loadOptions.PaginateViaPrimaryKey = true;
                    }
                };
            }
            finally
            {
                await InvokeAsync(() => isLoadingFile = false);
            }
        }
        private int FocusedRowVisibleIndexFile { get; set; }

        private void GridFile_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexFile = args.VisibleIndex;
        }

        private async Task RefreshFile_Click()
        {
            await LoadDataFile(zsh);
        }
        private async Task OnPopupFileClosed()
        {
            showPopUpFile = false;
            await LoadDataDetail();
            StateHasChanged();
        }

        private async Task DownloadFile(MaintenanceRecord file)
        {
            try
            {
                // URL API endpoint Anda
                string url = $"{NavigationManager.BaseUri}api/UploadFiles/DownloadFile?fileName={file.DocumentName}";

                // Permintaan file
                var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    // Dapatkan stream file
                    var fileStream = await response.Content.ReadAsStreamAsync();

                    // Nama file yang diunduh
                    var fileName = file.DocumentName;

                    // Simpan file ke local (folder Downloads atau lokasi lain)
                    string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", fileName);

                    using (var files = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileStream.CopyToAsync(files);
                    }
                    ToastService.ClearAll();
                    ToastService.ShowSuccess($"Success Download File {file.DocumentName}");
                }
                else
                {
                    ToastService.ClearAll();
                    ToastService.ShowError($"Failed to download file: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowInfo($"Error during file download: {ex.Message}");
            }
        }

        #endregion
    }
}
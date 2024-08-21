using McHealthCare.Application.Extentions;
using McHealthCare.Domain.Entities.Configuration;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace McHealthCare.Web.Components.Pages.Inventories
{
    public partial class InternalTransfer
    {
        #region relation Data

        private List<TransferStockDto> getInternalTransfers = [];
        private List<TransferStockProductDto> tempInternalTransferProducts = [];
        private List<TransferStockProductDto> getInternalTransferProducts = [];
        private List<TransferStockLogDto> AllLogs = [];
        private List<TransferStockLogDto> getLogs = [];
        private List<TransferStockLogDto> getTransferStockLogs = [];
        private List<TransactionStockDto> getTransactionStocks = [];
        private List<LocationDto> getLocations = [];
        private List<ProductDto> getProducts = [];
        private List<ProductDto> AllProducts = [];
        private List<ProductDto> filteredProducts = [];
        private List<UomDto> getUoms = [];
        private TransferStockDto postInternalTransfer = new();
        private TransferStockProductDto tempPostInternalTransfer = new();
        private TransferStockLogDto PostInternalTransferDetail = new();
        private TransactionStockDto PostTransactionStocks = new();
        private StockProductDto FormStock = new();
        private ApplicationUserDto NameUser = new();
        private string Url => Helper.URLS.FirstOrDefault(x => x == "configuration/groups") ?? string.Empty;
        #endregion relation Data

        #region static Variable
        private IGrid? Grid { get; set; }
        [Parameter] public string? PageMode { get; set; }
        private IGrid? Gridproduct { get; set; }
        private IGrid? GridDetailTransferStockLogs { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private Guid? TransferId { get; set; }
        private string? UomName { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsProduct { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsProductLogs { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; } = -1;
        private int FocusedRowVisibleIndexProduct { get; set; } = -1;
        private HubConnection? hubConnection;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        #endregion static Variable

        #region Load
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            try
            {
                UserAccess = await UserService.GetUserInfo(ToastService);

                var aa = NavigationManager.ToAbsoluteUri("/notificationHub");
                hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHub"))
                .Build();

                hubConnection.On<ReceiveDataDto>("ReceiveNotification", async message =>
                {
                    await LoadData();
                });

                await hubConnection.StartAsync();

                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }

                //await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                getInternalTransfers = await Mediator.Send(new GetTransferStockQuery());
                getInternalTransferProducts = await Mediator.Send(new GetTransferStockProductQuery());
                getTransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                getLocations = await Mediator.Send(new GetLocationQuery());
                getProducts = await Mediator.Send(new GetProductQuery());
                getUoms = await Mediator.Send(new GetUomQuery());
                UomName = getUoms.Select(x => x.Name).FirstOrDefault();
                getTransferStockLogs = await Mediator.Send(new GetTransferStockLogQuery());
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
        private void SetLoading(bool isLoading)
        {
            PanelVisible = isLoading;
        }

        private async Task LoadDataProductAsync()
        {
            try
            {
                getInternalTransferProducts = await Mediator.Send(new GetTransferStockProductQuery(x => x.TransferStockId == postInternalTransfer.Id));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private async Task BackButtonAsync()
        {
            try
            {
                SetLoading(true);
                NavigationManager.NavigateToUrl(Url);
                getInternalTransfers = await Mediator.Send(new GetTransferStockQuery());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error navigating back: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }
        private async Task CancelItemGroupMenuGrid_Click()
        {
            await BackButtonAsync();
        }
        private async Task LoadData_Detail()
        {
            PanelVisible = true;
            SelectedDataItemsProduct = [];
            tempInternalTransferProducts = await Mediator.Send(new GetTransferStockProductQuery());
            PanelVisible = false;
        }
        #endregion

        #region Change  Data
        private List<string> Batch = [];
        private DateTime? SelectedBatchExpired { get; set; }
        private async Task ChangeProduct(ProductDto item)
        {
            try
            {
                Batch.Clear();
                ResetFormProductDetail();

                if (item == null) return;

                tempPostInternalTransfer.ProductId = item.Id;
                tempPostInternalTransfer.TraceAvability = item.TraceAbility;


                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == item.Id && s.LocationId == postInternalTransfer.SourceId));
                if (item.TraceAbility)
                {
                    var s = await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == item.Id && x.Batch != null && x.Batch == tempPostInternalTransfer.Batch));
                    Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();

                    var firstStockProduct = stockProducts.Where(x => x.Batch == tempPostInternalTransfer.Batch);

                    UpdateFormProductDetail(firstStockProduct.FirstOrDefault() ?? new(), firstStockProduct.Sum(x => x.Quantity), item);
                }
                else
                {
                    var s = (await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == item.Id && x.LocationId == postInternalTransfer.SourceId)));

                    var firstStockProduct = stockProducts.FirstOrDefault();
                    tempPostInternalTransfer.TransactionStockId = firstStockProduct?.Id ?? null;
                    UpdateFormProductDetail(firstStockProduct ?? new(), s.Sum(x => x.Quantity), item);
                }

                return;
                var stockProducts2 = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == item.Id && s.LocationId == postInternalTransfer.SourceId));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private async Task ChangeBatch(string value)
        {

        }
        private void ResetFormProductDetail()
        {
            tempPostInternalTransfer.ExpiredDate = null;
            tempPostInternalTransfer.ProductId = null;
            tempPostInternalTransfer.UomId = null;
            tempPostInternalTransfer.CurrentStock = 0;
            tempPostInternalTransfer.Batch = null;
        }

        private void UpdateFormProductDetail(TransactionStockDto stockProduct, long qty, ProductDto items)
        {
            if (stockProduct != null)
            {
                tempPostInternalTransfer.UomId = items.UomId;
                tempPostInternalTransfer.UomName = items.Uom.Name;
                tempPostInternalTransfer.CurrentStock = qty;
                tempPostInternalTransfer.ExpiredDate = stockProduct.ExpiredDate;
            }
        }

        #endregion
        public class StatusComparer : IComparer<EnumStatusInternalTransfer>
        {
            private static readonly List<EnumStatusInternalTransfer> StatusOrder = new List<EnumStatusInternalTransfer> { EnumStatusInternalTransfer.Draft, EnumStatusInternalTransfer.Request, EnumStatusInternalTransfer.ApproveRequest, EnumStatusInternalTransfer.Waiting, EnumStatusInternalTransfer.Ready, EnumStatusInternalTransfer.Done, EnumStatusInternalTransfer.Cancel };

            public int Compare(EnumStatusInternalTransfer x, EnumStatusInternalTransfer y)
            {
                int indexX = StatusOrder.IndexOf(x);
                int indexY = StatusOrder.IndexOf(y);

                // Compare the indices
                return indexX.CompareTo(indexY);
            }
        }

        #region HandlerValidation
        private async Task HandleValidSubmitAsync()
        {
            try
            {
                postInternalTransfer = postInternalTransfer.Id == Guid.Empty
                    ? await Mediator.Send(new CreateTransferStockRequest(postInternalTransfer))
                    : await Mediator.Send(new UpdateTransferStockRequest(postInternalTransfer));

                NavigationManager.NavigateToUrl($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{postInternalTransfer.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void HandleInvalidSubmitAsync()
        {
            ToastService.ShowInfoSubmittingForm();
        }
        #endregion

        #region function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                ToastService.ClearAll();
                List<TransferStockDto> Transfers = SelectedDataItems.Adapt<List<TransferStockDto>>();
                List<Guid> id = Transfers.Select(x => x.Id).ToList();
                List<Guid> ProductIdsToDelete = new();
                List<Guid> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //delete data Transfer Stock Product
                    ProductIdsToDelete = getInternalTransferProducts
                               .Where(x => x.TransferStockId == TransferId)
                               .Select(x => x.Id)
                               .ToList();
                    await Mediator.Send(new DeleteTransferStockProductRequest(ids: ProductIdsToDelete));

                    //Delete data Transfer Detal transfer (Log)

                    DetailsIdsToDelete = getTransferStockLogs
                       .Where(x => x.TransferStockId == TransferId)
                       .Select(x => x.Id)
                       .ToList();
                    await Mediator.Send(new DeleteTransferStockLogRequest(ids: DetailsIdsToDelete));

                    //Delete Transfer

                    await Mediator.Send(new DeleteTransferStockRequest(SelectedDataItems[0].Adapt<TransferStockDto>().Id));
                }
                else
                {
                    foreach (var uid in id)
                    {
                        //delete data Transfer Stock Product
                        ProductIdsToDelete = getInternalTransferProducts
                                   .Where(x => x.TransferStockId == TransferId)
                                   .Select(x => x.Id)
                                   .ToList();
                        await Mediator.Send(new DeleteTransferStockProductRequest(ids: ProductIdsToDelete));

                        //Delete data Transfer Detal transfer (Log)

                        DetailsIdsToDelete = getTransferStockLogs
                           .Where(x => x.TransferStockId == TransferId)
                           .Select(x => x.Id)
                           .ToList();
                        await Mediator.Send(new DeleteTransferStockLogRequest(ids: DetailsIdsToDelete));
                    }
                    await Mediator.Send(new DeleteTransferStockRequest(ids: id));
                }
                ToastService.ShowSuccess("Data Deleting success..");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private bool IsDeletedMenu { get; set; } = false;

        private void CanDeleteSelectedItemsMenu(GridFocusedRowChangedEventArgs e)
        {
            FocusedRowVisibleIndexProduct = e.VisibleIndex;

            if (e.DataItem is not null)
                IsDeletedMenu = e.DataItem.Adapt<GroupMenuDto>().IsDefaultData;
        }

        private List<TransferStockProductDto> IdDeleteDetail = new List<TransferStockProductDto>();

        private async Task OnDeleteProduct(GridDataItemDeletingEventArgs e)
        {
            try
            {
                StateHasChanged();
                var data = SelectedDataItemsProduct.Adapt<List<TransferStockProductDto>>();
                var cek = data.Select(x => x.TransferStockId).FirstOrDefault();
                if (cek == null)
                {
                    tempInternalTransferProducts.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                }
                else
                {
                    foreach (var item in data)
                    {
                        tempInternalTransferProducts.RemoveAll(x => x.Id == item.Id);
                        await Mediator.Send(new DeleteTransferStockProductRequest(item.Id));
                    }
                }
                SelectedDataItemsProduct = [];
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion function Delete

        #region Function Save
        private async Task OnSave()
        {

        }

        private async Task OnSaveProduct()
        {

        }
        #endregion Function Save

        #region Export & Import
        private List<ExportFileData> ExportFileDataProduct =
        [
            new()
            {
                Column = "Menu",
                Notes = "Mandatory"
            },

        ];

        private List<ExportFileData> ExportFileDatasGroup =
        [
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            }
        ];
        #endregion
    }
}
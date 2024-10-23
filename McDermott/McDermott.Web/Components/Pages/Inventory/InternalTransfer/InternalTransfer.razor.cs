using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Inventory.TransferStockCommand;

namespace McDermott.Web.Components.Pages.Inventory.InternalTransfer
{
    public partial class InternalTransfer
    {
        #region relation Data

        private List<TransferStockDto> TransferStocks = [];
        private List<TransferStockProductDto> TempTransferStocks = [];
        private List<TransferStockProductDto> TransferStockProducts = [];
        private List<TransferStockLogDto> AllLogs = [];
        private List<TransferStockLogDto> Logs = [];
        private List<TransferStockLogDto> TransferStockLogs = [];
        private List<TransactionStockDto> TransactionStocks = [];
        private List<StockProductDto> StockProducts = [];
        private List<LocationDto> Locations = [];
        private List<ProductDto> Products = [];
        private List<ProductDto> AllProducts = [];
        private List<ProductDto> filteredProducts = [];
        private List<UomDto> Uoms = [];
        private TransferStockDto FormInternalTransfer = new();
        private TransferStockDto getInternalTransfer = new();
        private TransferStockProductDto TempFormInternalTransfer = new();
        private TransferStockLogDto FormInternalTransferDetail = new();
        private TransactionStockDto FormTransactionStocks = new();
        private StockProductDto FormStock = new();
        private UserDto NameUser = new();

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
                    StateHasChanged();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                        StateHasChanged();
                    }
                }
                catch { }

                await LoadAsyncData();
                StateHasChanged();
            }
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

        #region static Variable

        private IGrid? Grid { get; set; }
        private IGrid? GridDetailTransferStock { get; set; }
        private IGrid? GridDetailTransferStockLogs { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool showFormDetail { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private bool IsAddTransfer { get; set; } = false;
        private bool showButton { get; set; } = false;
        private bool ActiveButton { get; set; } = true;
        private bool showMatching { get; set; } = false;

        //private bool HasValueFalse { get; set; }
        private long? TransferId { get; set; }

        private string? header { get; set; } = string.Empty;
        private string? headerDetail { get; set; } = string.Empty;

        private bool isActiveButton = false;
        private string? UomName { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsDetail { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDataItemsDetailLogs { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexDetail { get; set; }

        #endregion static Variable

        #region Load

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }

        private async Task LoadAsyncData()
        {
            //TransferStocks = await Mediator.Send(new GetTransferStockQuery());
            //TransferStockProducts = await Mediator.Send(new GetTransferStockProductQuery());
            //TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
            //var Locations = (await Mediator.Send(new GetLocationQuery())).Item1;
            //this.Locations = Locations;
            //Products = await Mediator.Send(new GetProductQuery());
            //Uoms = await Mediator.Send(new GetUomQuery());
            //UomName = Uoms.Select(x => x.Name).FirstOrDefault();
            //TransferStockLogs = await Mediator.Send(new GetTransferStockLogQuery());
            //var user_group = await Mediator.Send(new GetUserQuery());
            //NameUser = user_group.FirstOrDefault(x => x.GroupId == UserAccessCRUID.GroupId && x.Id == UserLogin.Id) ?? new();
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                showForm = false;
                showFormDetail = false;
                await LoadAsyncData();
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

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

        public MarkupString GetIssueStatusIconHtml(EnumStatusInternalTransfer? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusInternalTransfer.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumStatusInternalTransfer.Request:
                    priorityClass = "info";
                    title = "Request";
                    break;

                case EnumStatusInternalTransfer.ApproveRequest:
                    priorityClass = "info";
                    title = "Approve Request";
                    break;

                case EnumStatusInternalTransfer.Waiting:
                    priorityClass = "warning";
                    title = "Waiting";
                    break;

                case EnumStatusInternalTransfer.Ready:
                    priorityClass = "primary";
                    title = "Ready";
                    break;

                case EnumStatusInternalTransfer.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusInternalTransfer.Cancel:
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

        #endregion Load

        #region Grid

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
                if ((TransferStockDto)args.DataItem is null)
                    return;

                isActiveButton = ((TransferStockDto)args.DataItem)!.Status!.Equals(EnumStatusInternalTransfer.Draft);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void Grid_FocusedRowChangedDetail(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexDetail = args.VisibleIndex;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            await EditItem_Click();
        }

        #endregion Grid

        #region Click

        private void NewItem_Click()
        {
            NavigationManager.NavigateTo($"inventory/internal-transfers/{EnumPageMode.Create.GetDisplayName()}");
        }

        private async Task EditItem_Click()
        {
            var data = SelectedDataItems[0].Adapt<TransferStockDto>();
            NavigationManager.NavigateTo($"inventory/internal-transfers/{EnumPageMode.Update.GetDisplayName()}?Id={data.Id}");
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

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                ToastService.ClearAll();
                List<TransferStockDto> Transfers = SelectedDataItems.Adapt<List<TransferStockDto>>();
                List<long> id = Transfers.Select(x => x.Id).ToList();
                List<long> ProductIdsToDelete = new();
                List<long> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //delete data Transfer Stock Product
                    ProductIdsToDelete = TransferStockProducts
                               .Where(x => x.TransferStockId == TransferId)
                               .Select(x => x.Id)
                               .ToList();
                    await Mediator.Send(new DeleteTransferStockProductRequest(ids: ProductIdsToDelete));

                    //Delete data Transfer Detal transfer (Log)

                    DetailsIdsToDelete = TransferStockLogs
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
                        ProductIdsToDelete = TransferStockProducts
                                   .Where(x => x.TransferStockId == TransferId)
                                   .Select(x => x.Id)
                                   .ToList();
                        await Mediator.Send(new DeleteTransferStockProductRequest(ids: ProductIdsToDelete));

                        //Delete data Transfer Detal transfer (Log)

                        DetailsIdsToDelete = TransferStockLogs
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

        #endregion function Delete

    }
}
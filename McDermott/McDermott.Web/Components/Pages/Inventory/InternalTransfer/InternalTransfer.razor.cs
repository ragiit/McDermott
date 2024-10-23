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

        private List<TransferStockDto> getTransferStocks = [];        

        #endregion relation Data

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

            //    await LoadAsyncData();
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

        #region static Variable

        private IGrid? Grid { get; set; }
        private bool PanelVisible { get; set; } = false;       
        private bool isActiveButton { get; set; } = false;       
        private bool FormValidationState { get; set; } = false;      
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }

        #endregion static Variable

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadData(newPageIndex, pageSize);
        }

        #endregion Searching

        #region Load

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetTransferStockQuery
                {
                    OrderByList = [
                    (x=>x.Status == EnumStatusInternalTransfer.Draft, true)
                    ],
                    SearchTerm = searchTerm,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                });
                getTransferStocks = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = pageIndex;
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
                var TransferStockProducts = await Mediator.Send(new GetAllTransferStockProductQuery());
                var TransferStockLogs = await Mediator.Send(new GetAllTransferStockLogQuery());
                List<TransferStockDto> Transfers = SelectedDataItems.Adapt<List<TransferStockDto>>();
                long id = SelectedDataItems[0].Adapt<TransferStockDto>().Id;
                List<long> ids = Transfers.Select(x => x.Id).ToList();
                List<long> ProductIdsToDelete = new();
                List<long> DetailsIdsToDelete = new();

                if (SelectedDataItems.Count == 1)
                {
                    //delete data Transfer Stock Product
                    ProductIdsToDelete = TransferStockProducts
                               .Where(x => x.TransferStockId == id)
                               .Select(x => x.Id)
                               .ToList();
                    await Mediator.Send(new DeleteTransferStockProductRequest(ids: ProductIdsToDelete));

                    //Delete data Transfer Detal transfer (Log)

                    DetailsIdsToDelete = TransferStockLogs
                       .Where(x => x.TransferStockId == id)
                       .Select(x => x.Id)
                       .ToList();
                    await Mediator.Send(new DeleteTransferStockLogRequest(ids: DetailsIdsToDelete));

                    //Delete Transfer

                    await Mediator.Send(new DeleteTransferStockRequest(SelectedDataItems[0].Adapt<TransferStockDto>().Id));
                }
                else
                {
                    foreach (var uid in ids)
                    {
                        //delete data Transfer Stock Product
                        ProductIdsToDelete = TransferStockProducts
                                   .Where(x => x.TransferStockId == id)
                                   .Select(x => x.Id)
                                   .ToList();
                        await Mediator.Send(new DeleteTransferStockProductRequest(ids: ProductIdsToDelete));

                        //Delete data Transfer Detal transfer (Log)

                        DetailsIdsToDelete = TransferStockLogs
                           .Where(x => x.TransferStockId == id)
                           .Select(x => x.Id)
                           .ToList();
                        await Mediator.Send(new DeleteTransferStockLogRequest(ids: DetailsIdsToDelete));
                    }
                    await Mediator.Send(new DeleteTransferStockRequest(ids: ids));
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
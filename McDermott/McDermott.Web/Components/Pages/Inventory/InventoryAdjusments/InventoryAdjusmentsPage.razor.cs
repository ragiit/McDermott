namespace McDermott.Web.Components.Pages.Inventory.InventoryAdjusments
{
    public partial class InventoryAdjusmentsPage
    {
        private int FocusedRowVisibleIndexGroupMenu { get; set; }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexGroupMenu = args.VisibleIndex;
        }

        public IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = true;
        private bool IsLoading { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        private InventoryAdjusmentDto InventoryAdjusment { get; set; } = new();
        private List<InventoryAdjusmentDto> InventoryAdjusments = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new List<object>();

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
                await GetUserInfo();
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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var a = await Mediator.Send(new GetInventoryAdjusmentQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                });

                InventoryAdjusments = a.Item1;
                totalCount = a.PageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        private void NewItem_Click()
        {
            NavigationManager.NavigateTo($"inventory/inventory-adjusments/{EnumPageMode.Create.GetDisplayName()}");
            return;
        }

        private void EditItem_Click()
        {
            try
            {
                InventoryAdjusment = SelectedDataItems[0].Adapt<InventoryAdjusmentDto>();
                NavigationManager.NavigateTo($"inventory/inventory-adjusments/{EnumPageMode.Update.GetDisplayName()}?Id={InventoryAdjusment.Id}");
                return;
            }
            catch (Exception e)
            {
                var zz = e;
            }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndexGroupMenu);
        }

        private async Task HandleValidSubmit()
        {
            // Save data
            ShowForm = false;
            await LoadData();
        }

        private void CancelForm_Click()
        {
            ShowForm = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteInventoryAdjusmentRequest
                    {
                        Id = ((InventoryAdjusmentDto)e.DataItem).Id
                    });
                }
                else
                {
                    var countriesToDelete = SelectedDataItems.Adapt<List<InventoryAdjusmentDto>>();
                    await Mediator.Send(new DeleteInventoryAdjusmentRequest
                    {
                        Ids = countriesToDelete.Select(x => x.Id).ToList()
                    });
                }

                SelectedDataItems = [];
                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }
    }
}
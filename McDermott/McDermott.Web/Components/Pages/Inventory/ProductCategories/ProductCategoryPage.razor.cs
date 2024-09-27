namespace McDermott.Web.Components.Pages.Inventory.ProductCategories
{
    public partial class ProductCategoryPage
    {
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

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                    }
                }
                catch { }
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

        #region Static

        private IGrid? Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private List<ProductCategoryDto> GetProductCategory = [];

        private List<string> CostingMethod = new List<string>
        {
            "Standart Price",
            "First In Firs Out (FIFO)",
            "Average Cost (AVCO)"
        };

        private List<string> InventoryValiation = new List<string>
        {
            "Manual",
            "Automated"
        };

        #endregion Static

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
            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProductCategoryQuery(searchTerm: searchTerm, pageIndex: pageIndex, pageSize: pageSize));
            GetProductCategory = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion Load

        #region Click

        private async Task NewItem_Click()
        {
            await Grid!.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            await Grid!.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid!.ShowColumnChooser();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid!.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            }); ;
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid!.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportCsvItem_Click()
        {
            await Grid!.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteProductCategoryRequest(((ProductCategoryDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteProductCategoryRequest(ids: SelectedDataItems.Adapt<List<ProductCategoryDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (ProductCategoryDto)e.EditModel;
            bool exists = await Mediator.Send(new ValidateProductCategoryQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.Code == editModel.Code));
            if (exists)
            {
                ToastService.ShowWarning($"Category Product with name '{editModel.Name}' & Code '{editModel.Code}' already exists.");
                return;
            }
            if (editModel.Id == 0)
                await Mediator.Send(new CreateProductCategoryRequest(editModel));
            else
                await Mediator.Send(new UpdateProductCategoryRequest(editModel));

            await LoadData();
        }

        #endregion Click

        #region Grid

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Grid
    }
}
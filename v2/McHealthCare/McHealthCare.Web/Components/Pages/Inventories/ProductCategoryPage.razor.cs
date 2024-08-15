namespace McHealthCare.Web.Components.Pages.Inventories
{
    public partial class ProductCategoryPage
    {
        #region Static

        public bool IsAccess { get; set; } = true;
        private IGrid? Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private List<ProductCategoryDto> ProductCategories = [];

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

        #region Load

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = [];
            ProductCategories = await Mediator.Send(new GetProductCategoryQuery());
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

            if (editModel.Id == Guid.Empty)
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
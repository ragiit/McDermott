using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;

namespace McDermott.Web.Components.Pages.Inventory.Products
{
    public partial class ProductPage
    {
        private List<ColorItem> colors =
            [ new ColorItem { Name = "Red", Color = "red" },
            new ColorItem { Name = "Green", Color = "green" },
            new ColorItem { Name = "Blue", Color = "blue" }
            ];

        private string selectedColor;

        public class ColorItem
        {
            public string Name { get; set; }
            public string Color { get; set; }
        }

        #region Relation Data

        private List<ProductDto> Products = [];
        private List<ProductDto> DataProducts = [];        
        private List<MaintenanceDto> getMaintenance = [];
        private List<MaintenanceDto> getMaintenanceDone = [];
        private List<MaintenanceDto> getMaintenanceScrap = [];
        private List<TransactionStockDto> TransactionStocks = [];
        private List<MedicamentDto> Medicaments = [];
        private TransactionStockDto FormTransactionStocks = new();
        private ProductDetailDto FormProductDetails = new();
        private ProductDto FormProducts = new();
        private StockProductDto FormStockProduct = new();
        private MedicamentDto FormMedicaments = new();

        #endregion Relation Data

        #region Static

        private IGrid? Grid { get; set; }
        private IGrid? GridStock { get; set; }
        private bool smartButtonShow { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool FormStockPopUp { get; set; } = false;
        private bool StockProductView { get; set; } = false;
        private bool StockEquipmentView { get; set; } = false;
        private bool StockEquipmentScrap { get; set; } = false;
        private bool PanelVisible { get; set; } = false;
        private bool showTabs { get; set; } = false;       
        private bool IsLoading { get; set; } = false;
        private bool? FieldHideStock { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsStock { get; set; } = [];
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        private bool? FormValidationState { get; set; }
        private string? NameProduct { get; set; }
        private string? NameUom { get; set; }
        private CultureInfo Culture = CultureInfo.GetCultureInfo("id-ID");

        #endregion Static

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
            //        }
            //    }
            //    catch { }
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
                // Inisialisasi variabel
                PanelVisible = true;
                SelectedDataItems = [];


                // Mengambil data produk
                var result = await Mediator.Send(new GetProductQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
                Products = result.Item1
                    .GroupBy(x => x.Id)
                    .Select(group => group.First())
                    .ToList();
                
                // Mengambil data stok produk dan menghitung jumlahnya
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                 
                foreach (var product in Products)
                {
                    var Qty = TransactionStocks.Where(s => s.ProductId == product.Id && s.Validate == true).Sum(x => x.Quantity);
                    product.Qtys = Qty;
                }

                // Menyembunyikan panel setelah selesai
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
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
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            await EditItem_Click();
        }

        #endregion Grid

        #region Click

        private async Task NewItem_Click()
        {
            NavigationManager.NavigateTo($"inventory/products/{EnumPageMode.Create.GetDisplayName()}");
            return;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            var DataId = SelectedDataItems[0].Adapt<ProductDto>();
            NavigationManager.NavigateTo($"inventory/products/{EnumPageMode.Update.GetDisplayName()}?Id={DataId.Id}");
            return;
        }

        private async Task onDiscard()
        {
            await LoadData();
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

        #endregion Click

        #region Delete Product

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            { 
                var products = (ProductDto)e.DataItem;
                if (SelectedDataItems is null)
                { 
                    var idProduct = await Mediator.Send(new GetSingleMedicamentQuery
                    {
                        Predicate = x => x.ProductId == products.Id,
                        Select = x => new Medicament
                        {
                            Id = x.Id
                        }
                    }); 
                    if (idProduct != null)
                    {
                        await Mediator.Send(new DeleteMedicamentRequest(idProduct.Id));
                    }
                    await Mediator.Send(new DeleteProductRequest(((ProductDto)e.DataItem).Id));
                }
                else
                {
                    List<long> MProductId = SelectedDataItems.Adapt<List<ProductDto>>().Select(x => x.Id).ToList();
                    var idProduct = (await Mediator.Send(new GetMedicamentQuery
                    {
                        Predicate = x => MProductId.Contains(x.Id),
                        Select = x => new Medicament
                        {
                            Id = x.Id
                        }
                    })).Item1;
                    foreach (var data in MProductId)
                    {
                        // Jika ada item yang dipilih, hapus medicament dan stok produk yang sesuai dengan produk yang dipilih
                        

                        var checkData = idProduct.Where(m => m.ProductId == data).FirstOrDefault();
                        if (checkData != null)
                        {
                            await Mediator.Send(new DeleteMedicamentRequest(checkData.Id));
                        }
                    }
                    // Hapus produk yang dipilih
                    await Mediator.Send(new DeleteProductRequest(ids: SelectedDataItems.Adapt<List<ProductDto>>().Select(x => x.Id).ToList()));
                    ToastService.ShowSuccess("Success Delete Data Product..");
                }

                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        #endregion Delete Product

    }
}
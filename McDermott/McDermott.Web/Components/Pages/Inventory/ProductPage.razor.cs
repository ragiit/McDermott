using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.ProductCommand;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacy.SignaCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class ProductPage
    {
        #region Relation Data

        private List<ProductDto> Products = [];
        private List<ProductDto> DataProducts = [];
        private List<MedicamentDto> Medicaments = [];
        private List<BpjsClassificationDto> BpjsClassifications = [];
        private List<UomDto> Uoms = [];
        private List<DrugFormDto> DrugForms = [];
        private List<DrugRouteDto> DrugRoutes = [];
        private List<ProductCategoryDto> productCategories = [];
        private List<ActiveComponentDto> ActiveComponents = [];
        private List<SignaDto> Signas = [];
        private List<LocationDto> Locations = [];
        private List<StockProductDto> StockProducts = [];
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
        private bool PanelVisible { get; set; } = false;
        private bool showTabs { get; set; } = true;
        private bool Checkins { get; set; } = false;
        private bool Chronis { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private long? TotalQty { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsStock { get; set; } = [];
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        private bool? FormValidationState { get; set; }
        private string? NameProduct { get; set; }
        private string? NameUom { get; set; }

        private List<string> ProductTypes = new List<string>
        {
            "Consumable",
            "Service",
            "Storable Product"
        };

        private List<string> HospitalProducts = new List<string>
        {
            "Medicament",
            "Medical Supplies",
            "Medical Equipment",
            "Vactination",
            "Consultation",
            "Laboratory",
            "Radiology",
            "Procedure"
        };

        private void SelectedItemChanged(string Hospital)
        {
            if (Hospital != "Medicament")
            {
                showTabs = false;
            }
            else
            {
                showTabs = true;
            }
        }

        private void SelectedChangeUoM(UomDto UomId)
        {
            if (UomId != null)
            {
                var UoMId = Uoms.Where(u => u.Id == UomId.Id).FirstOrDefault();
                FormProductDetails.PurchaseUomId = UoMId.Id;
            }
        }

        private bool Checkin
        {
            get => Checkins;
            set
            {
                bool Checkins = value;
                this.Checkins = value;
                if (Checkins)
                {
                    Chronis = true;
                    FormProductDetails.Cronies = true;
                }
                else
                {
                    Chronis = false;
                }
            }
        }

        #endregion Static

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
                var user = await UserInfoService.GetUserInfo();
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        #region Load

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                // Inisialisasi variabel
                PanelVisible = true;
                showForm = false;
                StockProductView = false;
                SelectedDataItems = [];

                // Mengambil data produk
                Products = (await Mediator.Send(new GetProductQuery()))
                    .GroupBy(x => x.Id)
                    .Select(group => group.First())
                    .ToList();

                // Mengambil data lainnya
                BpjsClassifications = await Mediator.Send(new GetBpjsClassificationQuery());
                Uoms = await Mediator.Send(new GetUomQuery());
                productCategories = await Mediator.Send(new GetProductCategoryQuery());
                DrugForms = await Mediator.Send(new GetFormDrugQuery());
                DrugRoutes = await Mediator.Send(new GetDrugRouteQuery());
                Signas = await Mediator.Send(new GetSignaQuery());
                ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
                Medicaments = await Mediator.Send(new GetMedicamentQuery());
                Locations = await Mediator.Send(new GetLocationQuery());

                // Mengambil data stok produk dan menghitung jumlahnya
                StockProducts = await Mediator.Send(new GetStockProductQuery());
                foreach (var product in Products)
                {
                    var stockIn = StockProducts.Where(s => s.ProductId == product.Id && s.StatusTransaction == "IN").Sum(x => x.Qty);
                    var stockOut = StockProducts.Where(s => s.ProductId == product.Id && s.StatusTransaction == "OUT").Sum(x => x.Qty);
                    product.Qtys = stockIn - stockOut;
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
        private async Task HandleValidSubmit()
        {
            IsLoading = true;
            FormValidationState = true;
            await OnSave();
            IsLoading = false;
        }

        private async Task HandleInvalidSubmit()
        {

            showForm = true;
            FormValidationState = false;
        }
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
            await LoadData();
            showForm = true;
            FormProductDetails = new ProductDetailDto
            {
                ProductType = ProductTypes[2],
                HospitalType = HospitalProducts[0],
                SalesPrice = "100",
                Tax = "11%"
            };
        }


        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click(ProductDto? p = null)
        {
            try
            {
                showForm = true;
                PanelVisible = true;
                smartButtonShow = true;

                // Inisialisasi data produk
                var products = p ?? SelectedDataItems[0].Adapt<ProductDto>();
                FormProductDetails = products.Adapt<ProductDetailDto>();

                // Jika produk adalah "Medicament", isi detail tambahan
                var medicamen = Medicaments.FirstOrDefault(z => z.ProductId == products.Id);
                if (products.HospitalType == "Medicament")
                {
                    if (medicamen != null)
                    {
                        FormProductDetails.MedicamentId = medicamen.Id;
                        FormProductDetails.FormId = medicamen.FormId;
                        FormProductDetails.RouteId = medicamen.RouteId;
                        FormProductDetails.Dosage = medicamen.Dosage;
                        FormProductDetails.UomMId = medicamen.UomId;
                        FormProductDetails.Cronies = medicamen.Cronies;
                        FormProductDetails.MontlyMax = medicamen.MontlyMax;
                        FormProductDetails.SignaId = medicamen.SignaId;

                        // Ambil komponen aktif jika tersedia
                        if (medicamen.ActiveComponentId != null)
                        {
                            selectedActiveComponents = ActiveComponents.Where(a => medicamen.ActiveComponentId.Contains(a.Id)).ToList();
                        }

                        FormProductDetails.PregnancyWarning = medicamen.PregnancyWarning;
                        FormProductDetails.Pharmacologi = medicamen.Pharmacologi;
                        FormProductDetails.Weather = medicamen.Weather;
                        FormProductDetails.Food = medicamen.Food;
                    }
                }

                // Kelola informasi stok
                var stockIN = StockProducts.Where(s => s.ProductId == products.Id && s.StatusTransaction == "IN").ToList();
                var stockOUT = StockProducts.Where(s => s.ProductId == products.Id && s.StatusTransaction == "OUT").ToList();
                TotalQty = stockIN.Sum(x => x.Qty) - stockOUT.Sum(x => x.Qty);

                // Ambil nama satuan ukur
                 NameUom = Uoms.FirstOrDefault(u => u.Id == FormProductDetails.UomId)?.Name;

                // Atur visibilitas panel
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
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
        #endregion
        #region Delete Product
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                Medicaments = await Mediator.Send(new GetMedicamentQuery());
                var products = (ProductDto)e.DataItem;
                if (SelectedDataItems is null)
                {
                    var idProduct = Medicaments.Where(m => m.ProductId == products.Id).FirstOrDefault();
                    if (idProduct != null)
                    {
                        await Mediator.Send(new DeleteMedicamentRequest(idProduct.Id));
                    }
                    await Mediator.Send(new DeleteProductRequest(((ProductDto)e.DataItem).Id));
                }
                else
                {
                    List<long> MProductId = SelectedDataItems.Adapt<List<ProductDto>>().Select(x => x.Id).ToList();
                    foreach (var data in MProductId)
                    {
                        // Jika ada item yang dipilih, hapus medicament dan stok produk yang sesuai dengan produk yang dipilih
                        var CheckStock = StockProducts.Where(s => s.ProductId == data).ToList();
                        foreach (var stc in CheckStock)
                        {
                            if (stc != null)
                            {
                                await Mediator.Send(new DeleteStockProductRequest(stc.Id));
                            }
                        }

                        var checkData = Medicaments.Where(m => m.ProductId == data).FirstOrDefault();
                        if (checkData != null)
                        {
                            await Mediator.Send(new DeleteMedicamentRequest(checkData.Id));
                        }
                    }
                    // Hapus produk yang dipilih
                    await Mediator.Send(new DeleteProductRequest(ids: SelectedDataItems.Adapt<List<ProductDto>>().Select(x => x.Id).ToList()));
                    ToastService.ShowError("Success Delete Data Product..");
                }

                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        #endregion Click
        ProductDto getProduct = new();
        #region Save

        private async Task OnSave()
        {
            try
            {
                if (FormValidationState == false)
                {
                    return;
                }
               
                var a = FormProductDetails;
                if (a.Name != null)
                {
                    FormProducts.Id = FormProductDetails.Id;
                    FormProducts.Name = FormProductDetails.Name;
                    FormProducts.ProductCategoryId = FormProductDetails.ProductCategoryId;
                    FormProducts.ProductType = FormProductDetails.ProductType;
                    FormProducts.HospitalType = FormProductDetails.HospitalType;
                    FormProducts.BpjsClasificationId = FormProductDetails.BpjsClasificationId;
                    FormProducts.UomId = FormProductDetails.UomId;
                    FormProducts.PurchaseUomId = FormProductDetails.PurchaseUomId;
                    FormProducts.SalesPrice = FormProductDetails.SalesPrice;
                    FormProducts.Tax = FormProductDetails.Tax;
                    FormProducts.Cost = FormProductDetails.Cost;
                    FormProducts.ProductCategoryId = FormProductDetails.ProductCategoryId;
                    FormProducts.InternalReference = FormProductDetails.InternalReference;
                    FormProducts.TraceAbility = FormProductDetails.TraceAbility;

                    //Medicament
                    FormMedicaments.Id = FormProductDetails.MedicamentId ?? 0;
                    FormMedicaments.ProductId = getProduct.Id;
                    FormMedicaments.FormId = FormProductDetails.FormId;
                    FormMedicaments.RouteId = FormProductDetails.RouteId;
                    FormMedicaments.Dosage = FormProductDetails.Dosage;
                    FormMedicaments.UomId = FormProductDetails.UomMId;
                    FormMedicaments.Cronies = FormProductDetails.Cronies;
                    FormMedicaments.MontlyMax = FormProductDetails.MontlyMax;
                    FormMedicaments.SignaId = FormProductDetails.SignaId;
                    FormMedicaments.PregnancyWarning = FormProductDetails.PregnancyWarning;
                    FormMedicaments.Pharmacologi = FormProductDetails.Pharmacologi;
                    FormMedicaments.Weather = FormProductDetails.Weather;
                    FormMedicaments.Food = FormProductDetails.Food;
                    if (selectedActiveComponents != null)
                    {
                        var listActiveComponent = selectedActiveComponents.Select(x => x.Id).ToList();
                        FormMedicaments.ActiveComponentId?.AddRange(listActiveComponent);
                    }

                    //function Save

                    if (FormProductDetails.Id == 0)
                    {
                        if (FormProducts.Id == 0)
                        {
                            getProduct = await Mediator.Send(new CreateProductRequest(FormProducts));
                        }


                        // Medicament
                        if (FormProductDetails.HospitalType == "Medicament")
                        {
                            FormMedicaments.ProductId = getProduct.Id;
                            if (FormMedicaments.Id == 0)
                            {
                                await Mediator.Send(new CreateMedicamentRequest(FormMedicaments));
                            }
                            else
                            {
                                await Mediator.Send(new UpdateMedicamentRequest(FormMedicaments));
                            }
                        }
                        ToastService.ShowSuccess("Successfully Add Data...");
                    }
                    else
                    {
                        getProduct = await Mediator.Send(new UpdateProductRequest(FormProducts));

                        if (FormProductDetails.HospitalType == "Medicament")
                        {
                            FormMedicaments.ProductId = getProduct.Id;
                            if (FormMedicaments.Id == 0)
                            {
                                await Mediator.Send(new CreateMedicamentRequest(FormMedicaments));
                            }
                            else
                            {
                                await Mediator.Send(new UpdateMedicamentRequest(FormMedicaments));
                            }
                        }
                        ToastService.ShowSuccess("Successfully Update Data...");
                    }

                    await EditItem_Click(getProduct);
                }
                else
                {
                    await HandleInvalidSubmit();
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }                       
        }

        #endregion Save

        #region Stock Produk
        private async void onDiscardStock()
        {
            FormStockPopUp = false;
            await NewTableStock_Item();
        }
        private async Task RefreshStock_Click()
        {
            await NewTableStock_Item();
        }
        private async Task NewTableStock_Item()
        {
            try
            {
                // Inisialisasi
                showForm = false;
                PanelVisible = true;
                StockProductView = true;

                // Mengambil data stok produk
                StockProducts = await Mediator.Send(new GetStockProductQuery());

                if (SelectedDataItems.Count == 0)
                {
                    // Jika tidak ada item yang dipilih, gunakan produk yang sedang dipertimbangkan
                    StockProducts = StockProducts.Where(x => x.ProductId == getProduct.Id).ToList();
                    NameProduct = getProduct.Name;
                }
                else
                {
                    // Jika ada item yang dipilih, gunakan produk yang dipilih
                    StockProducts = StockProducts.Where(x => x.ProductId == SelectedDataItems[0].Adapt<ProductDto>().Id).ToList();
                    NameProduct = SelectedDataItems[0].Adapt<ProductDto>().Name;
                }

                // Menyembunyikan panel setelah selesai
                PanelVisible = false;

            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private async Task NewItemStock_Click()
            {
            FormStockProduct = new();
            FormStockPopUp = true;
            DataProducts = await Mediator.Send(new GetProductQuery());
            if (SelectedDataItems.Count == 0)
            {

                FormStockProduct.UomId = getProduct.UomId;
                FormStockProduct.ProductId =getProduct.Id;
                NameProduct = getProduct.Name;
            }
            else
            {
                FormStockProduct.UomId = Products.Where(p => p.Id == SelectedDataItems[0].Adapt<ProductDto>().Id).Select(x => x.UomId).FirstOrDefault();
                FormStockProduct.ProductId = Products.Where(p => p.Id == SelectedDataItems[0].Adapt<ProductDto>().Id).Select(x => x.Id).FirstOrDefault();
                NameProduct = SelectedDataItems[0].Adapt<ProductDto>().Name;
            }                
           
        }
        private async Task EditItemStock_Click()
        {
            FormStockPopUp = true;
            var DataEdit = SelectedDataItemsStock[0].Adapt<StockProductDto>();
            FormStockProduct = DataEdit;
        }
        private void DeleteItemStock_Click()
        {
            GridStock!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
        private async Task Back_Click()
        {
            await LoadData();
        }
        private async Task onDeleteStock(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var stocks = (StockProductDto)e.DataItem;
                if (SelectedDataItemsStock is null)
                {
                    await Mediator.Send(new DeleteStockProductRequest(((StockProductDto)e.DataItem).Id));
                    ToastService.ShowError("Success Delete Data Stock..");
                }
                else
                {
                    await Mediator.Send(new DeleteStockProductRequest(ids: SelectedDataItemsStock.Adapt<List<StockProductDto>>().Select(x => x.Id).ToList()));
                    ToastService.ShowError("Success Delete Data Stock..");
                }
                await NewTableStock_Item();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task onSaveStock()
        {
            try
            {
                var a = FormStockProduct;
                if (FormStockProduct.SourceId is null)
                {
                    return;
                }
                FormStockProduct.StatusTransaction = "IN";
                if (FormStockProduct.Id == 0)
                {
                    await Mediator.Send(new CreateStockProductRequest(FormStockProduct));
                    ToastService.ShowSuccess("Success Add Data Stock..");
                }
                else
                {
                    await Mediator.Send(new UpdateStockProductRequest(FormStockProduct));
                    ToastService.ShowSuccess("Success Update Data Stock..");
                }
                FormStockPopUp = false;
                await NewTableStock_Item();

            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

        }
        #endregion
    }
}
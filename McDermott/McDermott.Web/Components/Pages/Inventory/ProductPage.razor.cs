using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;

namespace McDermott.Web.Components.Pages.Inventory
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
        private List<MedicamentDto> Medicaments = [];
        private List<BpjsClassificationDto> BpjsClassifications = [];
        private List<UomDto> Uoms = [];
        private List<DrugFormDto> DrugForms = [];
        private List<DrugRouteDto> DrugRoutes = [];
        private List<ProductCategoryDto> productCategories = [];
        private List<ActiveComponentDto> ActiveComponents = [];
        private List<DrugDosageDto> Frequencys = [];
        private List<LocationDto> Locations = [];
        private List<StockProductDto> StockProducts = [];
        private List<TransactionStockDto> TransactionStocks = [];
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
        private bool PanelVisible { get; set; } = false;
        private bool showTabs { get; set; } = true;
        private bool Checkins { get; set; } = false;
        private bool Chronis { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private bool? FieldHideStock { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private long? TotalQty { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsStock { get; set; } = [];
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        private bool? FormValidationState { get; set; }
        private string? NameProduct { get; set; }
        private string? NameUom { get; set; }
        private CultureInfo Culture = CultureInfo.GetCultureInfo("id-ID");

        private List<string> ProductTypes =
        [
            "Consumable",
            "Service",
            "Storable Product"
        ];

        private List<string> EquipmentConditions =
            [
                "Good",
                "Partially Damaged",
                "Broken"
            ];

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
                var UoMId = Uoms.Where(u => u.Id == UomId.Id).FirstOrDefault() ?? new();
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

        #region Load

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }

        private async Task LoadData()
        {
            try
            {
                // Inisialisasi variabel
                PanelVisible = true;
                smartButtonShow = false;
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
                Frequencys = await Mediator.Send(new GetDrugDosageQuery());
                ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
                Medicaments = await Mediator.Send(new GetMedicamentQuery());
                Locations = await Mediator.Send(new GetLocationQuery());

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

        private async Task HandleValidSubmit()
        {
            IsLoading = true;
            FormValidationState = true;
            await OnSave();
            IsLoading = false;
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfoSubmittingForm();
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
                SalesPrice = 100,
                Tax = "11%",
                UomId = Uoms.FirstOrDefault(x => x.Name == "Unit")?.Id ?? 0
            };

            SelectedChangeUoM(Uoms.FirstOrDefault(x => x.Name == "Unit") ?? new());
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
                StockProductView = false;
                smartButtonShow = true;

                // Inisialisasi data produk
                var products = p ?? SelectedDataItems[0].Adapt<ProductDto>();
                getProduct = products;
                FormProductDetails = products.Adapt<ProductDetailDto>();

                // Jika produk adalah "Medicament", isi detail tambahan
                Medicaments = await Mediator.Send(new GetMedicamentQuery(x => x.ProductId == products.Id));
                var medicamen = Medicaments.FirstOrDefault();
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
                        FormProductDetails.FrequencyId = medicamen.FrequencyId;

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
                TotalQty = TransactionStocks.Where(x => x.ProductId == products.Id && x.Validate == true).Sum(z => z.Quantity);

                // Ambil nama satuan ukur
                NameUom = Uoms.FirstOrDefault(u => u.Id == FormProductDetails.UomId)?.Name;

                if (FormProductDetails.UomId == 0)
                {
                    FormProductDetails.UomId = Uoms.FirstOrDefault(x => x.Name == "Unit")?.Id ?? 0;

                    SelectedChangeUoM(Uoms.FirstOrDefault(x => x.Name == "Unit") ?? new());
                }

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

        #endregion Click

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

        #endregion Delete Product

        private ProductDto getProduct = new();

        #region Save

        private async Task OnSave()
        {
            try
            {
                if (FormValidationState == false)
                {
                    return;
                }

                if (FormProductDetails?.Name == null)
                {
                    HandleInvalidSubmit();
                    return;
                }

                SetFormProductDetails();
                SetFormMedicamentDetails();

                if (FormProductDetails.Id == 0)
                {
                    await CreateNewProductAndMedicament();

                    ToastService.ShowSuccess("Successfully Added Data...");
                }
                else
                {
                    await UpdateExistingProductAndMedicament();
                    ToastService.ShowSuccess("Successfully Updated Data...");
                }

                await EditItem_Click(getProduct);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void SetFormProductDetails()
        {
            FormProducts.Id = FormProductDetails.Id;
            FormProducts.Name = FormProductDetails.Name;
            FormProducts.ProductCategoryId = FormProductDetails.ProductCategoryId;
            FormProducts.ProductType = FormProductDetails.ProductType;
            FormProducts.HospitalType = FormProductDetails.HospitalType;
            FormProducts.BpjsClassificationId = FormProductDetails.BpjsClassificationId;
            FormProducts.UomId = FormProductDetails.UomId;
            FormProducts.PurchaseUomId = FormProductDetails.PurchaseUomId;
            FormProducts.SalesPrice = FormProductDetails.SalesPrice;
            FormProducts.Tax = FormProductDetails.Tax;
            FormProducts.Cost = FormProductDetails.Cost;
            FormProducts.InternalReference = FormProductDetails.InternalReference;
            FormProducts.IsOralMedication = FormProductDetails.IsOralMedication;
            FormProducts.IsTopicalMedication = FormProductDetails.IsTopicalMedication;
            FormProducts.TraceAbility = FormProductDetails.TraceAbility;
            FormProducts.Brand = FormProductDetails.Brand;
            FormProducts.EquipmentCode = FormProductDetails.EquipmentCode;
            FormProducts.YearOfPurchase = FormProductDetails.YearOfPurchase;
            FormProducts.LastCalibrationDate = FormProductDetails.LastCalibrationDate;
            FormProducts.NextCalibrationDate = FormProductDetails.NextCalibrationDate;
            FormProducts.EquipmentCondition = FormProductDetails.EquipmentCondition;
        }

        private void SetFormMedicamentDetails()
        {
            FormMedicaments.Id = FormProductDetails.MedicamentId ?? 0;
            FormMedicaments.ProductId = getProduct?.Id ?? 0;
            FormMedicaments.FormId = FormProductDetails.FormId;
            FormMedicaments.RouteId = FormProductDetails.RouteId;
            FormMedicaments.Dosage = FormProductDetails.Dosage;
            FormProducts.IsOralMedication = FormProductDetails.IsOralMedication;
            FormProducts.IsTopicalMedication = FormProductDetails.IsTopicalMedication;
            FormMedicaments.UomId = FormProductDetails.UomMId;
            FormMedicaments.Cronies = FormProductDetails.Cronies;
            FormMedicaments.MontlyMax = FormProductDetails.MontlyMax;
            FormMedicaments.FrequencyId = FormProductDetails.FrequencyId;
            FormMedicaments.PregnancyWarning = FormProductDetails.PregnancyWarning;
            FormMedicaments.Pharmacologi = FormProductDetails.Pharmacologi;
            FormMedicaments.Weather = FormProductDetails.Weather;
            FormMedicaments.Food = FormProductDetails.Food;

            if (selectedActiveComponents != null)
            {
                FormMedicaments.ActiveComponentId?.AddRange(selectedActiveComponents.Select(x => x.Id));
            }
        }

        private async Task CreateNewProductAndMedicament()
        {
            getProduct = await Mediator.Send(new CreateProductRequest(FormProducts));

            if (FormProductDetails.HospitalType == "Medicament")
            {
                FormMedicaments.ProductId = getProduct.Id;

                if (FormMedicaments.Id == 0)
                {
                    var aazd = await Mediator.Send(new CreateMedicamentRequest(FormMedicaments));
                }
                else
                {
                    await Mediator.Send(new UpdateMedicamentRequest(FormMedicaments));
                }
            }
        }

        private async Task UpdateExistingProductAndMedicament()
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
        }

        #endregion Save

        #region Stock Produk

        private async Task onDiscardStock()
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
                await LoadData();
                // Inisialisasi
                showForm = false;
                StockProductView = false;
                PanelVisible = true;
                StockProductView = true;

                // Mengambil data stok produk

                if (SelectedDataItems.Count == 0)
                {
                    // Jika tidak ada item yang dipilih, gunakan produk yang sedang dipertimbangkan

                    if (getProduct.TraceAbility == true)
                    {
                        StockProducts = TransactionStocks.Where(x => x.ProductId == getProduct.Id && x.Validate == true)
                        .GroupBy(z => new { z.ProductId, z.Batch, z.LocationId, z.UomId })
                        .Select(y => new StockProductDto
                        {
                            ProductId = y.Key.ProductId,
                            Batch = y.Key.Batch ?? "-",
                            DestinanceId = y.Key.LocationId,
                            DestinanceName = y.First()?.Location?.Name ?? "-",
                            UomId = y.Key.UomId,
                            UomName = y.First()?.Uom?.Name ?? "-",
                            Expired = y.First().ExpiredDate,
                            ProductName = y.First()?.Product?.Name ?? "-",
                            Qty = y.Sum(item => item.Quantity)


                        }).ToList();

                        FieldHideStock = true;
                    }
                    else
                    {
                        StockProducts = TransactionStocks.Where(x => x.ProductId == getProduct.Id && x.Validate == true)
                        .GroupBy(z => new { z.ProductId, z.LocationId, z.UomId })
                        .Select(y => new StockProductDto
                        {
                            ProductId = y.Key.ProductId,
                            DestinanceId = y.Key.LocationId,
                            DestinanceName = y.First()?.Location?.Name ?? "-",
                            UomId = y.Key.UomId,
                            UomName = y.First()?.Uom?.Name ?? "-",
                            ProductName = y.First()?.Product?.Name ?? "-",
                            Qty = y.Sum(item => item.Quantity)


                        }).ToList();
                        FieldHideStock = false;
                    }
                    NameProduct = getProduct.Name;
                }
                else
                {
                    // Jika ada item yang dipilih, gunakan produk yang dipilih
                    StockProducts = TransactionStocks.Where(x => x.ProductId == SelectedDataItems[0].Adapt<ProductDto>().Id && x.Validate == true)
                        .GroupBy(z => new { z.ProductId, z.Batch, z.LocationId })
                        .Select(y => new StockProductDto
                        {
                            ProductId = y.Key.ProductId,
                            Batch = y.Key.Batch ?? "-",
                            DestinanceId = y.Key.LocationId,
                            DestinanceName = y.First()?.Location?.Name ?? "-",
                            UomId = y.First().UomId,
                            UomName = y.First()?.Uom?.Name ?? "-",
                            Expired = y.First().ExpiredDate,
                            ProductName = y.First()?.Product?.Name ?? "-",
                            Qty = y.Sum(item => item.Quantity)


                        }).ToList();
                    NameProduct = SelectedDataItems[0].Adapt<ProductDto>().Name;
                    if (SelectedDataItems[0].Adapt<ProductDto>().TraceAbility == true)
                    {
                        FieldHideStock = true;
                    }
                    else
                    {
                        FieldHideStock = false;
                    }
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
                FormStockProduct.ProductId = getProduct.Id;
                NameProduct = getProduct.Name;
                if (getProduct.TraceAbility == true)
                {
                    FieldHideStock = true;
                }
                else
                {
                    FieldHideStock = false;
                }
            }
            else
            {
                FormStockProduct.UomId = Products.Where(p => p.Id == SelectedDataItems[0].Adapt<ProductDto>().Id).Select(x => x.UomId).FirstOrDefault();
                FormStockProduct.ProductId = Products.Where(p => p.Id == SelectedDataItems[0].Adapt<ProductDto>().Id).Select(x => x.Id).FirstOrDefault();
                NameProduct = SelectedDataItems[0].Adapt<ProductDto>().Name;
                if (SelectedDataItems[0].Adapt<ProductDto>().TraceAbility == true)
                {
                    FieldHideStock = true;
                }
                else
                {
                    FieldHideStock = false;
                }
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
            //await LoadData();
            //await EditItem_Click(null);
            var data_product = await Mediator.Send(new GetProductQuery(x => x.Id == getProduct.Id));

            await EditItem_Click(getProduct);
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
                if (FieldHideStock == false)
                {
                    FormStockProduct.Batch = null;
                    FormStockProduct.Expired = null;
                }
                //FormStockProduct.StatusTransaction = "IN";
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

        #endregion Stock Produk
    }
}
using DevExpress.XtraCharts.Native;
using DocumentFormat.OpenXml.Bibliography;
using McDermott.Application.Dtos.Pharmacies;
using McDermott.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacies.MedicamentCommand;

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

        private List<ExportFileData> MedicalEquipment = new List<ExportFileData>
        {
            new() { Column = "Name", Notes = "Mandatory, Unique" },
            new() { Column = "Product Type", Notes = "Mandatory" },
            new() { Column = "Hospital Type" },
            new() { Column = "Brand" },
            new() { Column = "Equipment Code" },
            new() { Column = "Year Of Purchase", Notes = "Format: YYYY-MM-DD" },
            new() { Column = "Last Calibration Date", Notes = "Format: YYYY-MM-DD" },
            new() { Column = "Next Calibration Date", Notes = "Format: YYYY-MM-DD" },
            new() { Column = "Equipment Condition" },
            new() { Column = "BPJS Classification" },
            new() { Column = "Purchase UOM" },
            new() { Column = "UOM" },
            new() { Column = "Batch & Expired", Notes = "True or False" },
            new() { Column = "Sales Price" },
            new() { Column = "Customer Tax" },
            new() { Column = "Cost" },
            new() { Column = "Category" },
            new() { Column = "Internal Reference" }
        };

        private async Task ExportMedicalEquipmentTemplate()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "product_medical_equipment_template.xlsx", MedicalEquipment);
        }

        private async Task ImportFileMedicalEquipmentTemplate()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInputMedicalEquipment");
        }

        public async Task ImportExcelFileMedicalEquipment(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => MedicalEquipment.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<ProductDto>();

                    var listPurchaseUom = new List<UomDto>();
                    var listUom = new List<UomDto>();
                    var listCategory = new List<ProductCategoryDto>();
                    var listBpjsClassifications = new List<BpjsClassificationDto>();

                    var purchaseUoms = new HashSet<string>();
                    var uoms = new HashSet<string>();
                    var categories = new HashSet<string>();
                    var bpjsClassifications = new HashSet<string>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var bpjs = ws.Cells[row, 10].Value?.ToString()?.Trim();
                        var purchaseUom = ws.Cells[row, 11].Value?.ToString()?.Trim();
                        var Uom = ws.Cells[row, 12].Value?.ToString()?.Trim();
                        var categoriesz = ws.Cells[row, 17].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(purchaseUom))
                            purchaseUoms.Add(purchaseUom.ToLower());

                        if (!string.IsNullOrEmpty(Uom))
                            uoms.Add(Uom.ToLower());

                        if (!string.IsNullOrEmpty(categoriesz))
                            categories.Add(categoriesz.ToLower());

                        if (!string.IsNullOrEmpty(bpjs))
                            bpjsClassifications.Add(bpjs.ToLower());
                    }

                    listBpjsClassifications = (await Mediator.Send(new GetBpjsClassificationQuery
                    {
                        IsGetAll = true,
                        Predicate = x => bpjsClassifications.Contains(x.Name.ToLower()),
                        Select = x => new BpjsClassification
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    listPurchaseUom = (await Mediator.Send(new GetUomQuery
                    {
                        IsGetAll = true,
                        Predicate = x => purchaseUoms.Contains(x.Name.ToLower()),
                        Select = x => new Uom
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    listUom = (await Mediator.Send(new GetUomQuery
                    {
                        IsGetAll = true,
                        Predicate = x => uoms.Contains(x.Name.ToLower()),
                        Select = x => new Uom
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    listCategory = (await Mediator.Send(new GetProductCategoryQuery
                    {
                        IsGetAll = true,
                        Predicate = x => categories.Contains(x.Name.ToLower()),
                        Select = x => new ProductCategory
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var bpjs = ws.Cells[row, 10].Value?.ToString()?.Trim();
                        var purchaseUom = ws.Cells[row, 11].Value?.ToString()?.Trim();
                        var Uom = ws.Cells[row, 12].Value?.ToString()?.Trim();
                        var categoriesz = ws.Cells[row, 17].Value?.ToString()?.Trim();

                        long? bpjsId = null;
                        long? purchaseUomId = null;
                        long? uomId = null;
                        long? categoryId = null;

                        bool isValid = true;

                        if (!string.IsNullOrEmpty(bpjs))
                        {
                            var cachedParent = listBpjsClassifications.FirstOrDefault(x => x.Name.Equals(bpjs, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 10, bpjs ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                bpjsId = cachedParent.Id;
                            }
                        }
                        if (!string.IsNullOrEmpty(purchaseUom))
                        {
                            var cachedParent = listPurchaseUom.FirstOrDefault(x => x.Name.Equals(purchaseUom, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 11, purchaseUom ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                purchaseUomId = cachedParent.Id;
                            }
                        }
                        if (!string.IsNullOrEmpty(Uom))
                        {
                            var cachedParent = listUom.FirstOrDefault(x => x.Name.Equals(Uom, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 12, Uom ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                uomId = cachedParent.Id;
                            }
                        }
                        if (!string.IsNullOrEmpty(categoriesz))
                        {
                            var cachedParent = listCategory.FirstOrDefault(x => x.Name.Equals(categoriesz, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 17, categoriesz ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                categoryId = cachedParent.Id;
                            }
                        }

                        if (!isValid)
                            continue;

                        var c = new ProductDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            ProductType = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                            HospitalType = ws.Cells[row, 3].Value?.ToString()?.Trim(),
                            Brand = ws.Cells[row, 4].Value?.ToString()?.Trim(),
                            EquipmentCode = ws.Cells[row, 5].Value?.ToString()?.Trim(),
                            YearOfPurchase = ws.Cells[row, 6].Value != null ? DateTime.Parse(ws.Cells[row, 6].Value.ToString()) : (DateTime?)null,
                            LastCalibrationDate = ws.Cells[row, 7].Value != null ? DateTime.Parse(ws.Cells[row, 7].Value.ToString()) : (DateTime?)null,
                            NextCalibrationDate = ws.Cells[row, 8].Value != null ? DateTime.Parse(ws.Cells[row, 8].Value.ToString()) : (DateTime?)null,
                            EquipmentCondition = ws.Cells[row, 9].Value?.ToString()?.Trim(),
                            BpjsClassificationId = bpjsId,
                            PurchaseUomId = purchaseUomId,
                            UomId = uomId,
                            TraceAbility = ws.Cells[row, 13].Value != null && Convert.ToBoolean(ws.Cells[row, 13].Value),
                            SalesPrice = ws.Cells[row, 14].Value != null ? Convert.ToInt64(ws.Cells[row, 14].Value) : (long?)null,
                            Tax = ws.Cells[row, 15].Value?.ToString()?.Trim(),
                            Cost = ws.Cells[row, 16].Value != null ? Convert.ToInt64(ws.Cells[row, 16].Value) : (long?)null,
                            ProductCategoryId = categoryId,
                            InternalReference = ws.Cells[row, 18].Value?.ToString()?.Trim(),
                        };

                        list.Add(c);
                    }

                    //if (list.Count > 0)
                    //{
                    //    list = list.DistinctBy(x => new { x.Name, x.Code, }).ToList();

                    //    // Panggil BulkValidateCountryQuery untuk validasi bulk
                    //    var existingCountrys = await Mediator.Send(new BulkValidateCountryQuery(list));

                    //    // Filter Country baru yang tidak ada di database
                    //    list = list.Where(Country =>
                    //        !existingCountrys.Any(ev =>
                    //            ev.Name == Country.Name &&
                    //            ev.Code == Country.Code
                    //        )
                    //    ).ToList();

                    await Mediator.Send(new CreateListProductRequest(list));
                    //    await LoadData(0, pageSize);
                    //    SelectedDataItems = [];
                    //}

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
                finally { PanelVisible = false; }
            }
            PanelVisible = false;
        }
    }
}
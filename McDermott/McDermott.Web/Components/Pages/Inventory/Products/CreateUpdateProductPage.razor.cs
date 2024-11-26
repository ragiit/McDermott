using MailKit.Search;
using McDermott.Application.Dtos.Pharmacies;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacies.DrugFormCommand;
using static McDermott.Application.Features.Commands.Pharmacies.MedicamentCommand;

namespace McDermott.Web.Components.Pages.Inventory.Products
{
    public partial class CreateUpdateProductPage
    {
        #region Relation Data

        //List Data
        private List<ProductDto> GetProduct = [];

        private List<MedicamentDto> GetMedicaments = [];
        private List<BpjsClassificationDto> GetBPJSCl = [];
        private List<UomDto> GetUoms = [];
        private List<UomDto> GetUomPurchases = [];
        private List<DrugFormDto> GetDrugForms = [];
        private List<DrugRouteDto> GetDrugRoutes = [];
        private List<ProductCategoryDto> GetProductCategories = [];
        private List<ActiveComponentDto> ActiveComponents = [];
        private List<DrugDosageDto> GetDrugDosage = [];
        private List<LocationDto> GetLocations = [];
        private List<StockProductDto> StockProducts = [];
        private List<TransactionStockDto> getTransactionStocks = [];
        private List<MaintenanceDto> GetMaintenance = [];
        private List<MaintenanceProductDto> GetMaintenanceProduct = [];
        private List<MaintenanceProductDto> GetMaintenanceScrap = [];
        private List<MaintenanceProductDto> GetMaintenanceHistory = [];

        //Post data
        private ProductDto PostProduct = new();

        private ProductDto TempProduct = new();
        private MedicamentDto PostMedicaments = new();
        private ProductDetailDto PostProductDetails = new();

        #endregion Relation Data

        #region Variable Static

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid Grid;
        private IGrid GridStock;
        private IGrid GridScrap;
        private bool PanelVisible { get; set; } = false;
        private bool _SmartButton { get; set; } = false;
        private bool showTabs { get; set; } = false;
        private bool Checkins { get; set; } = false;
        private bool Chronis { get; set; } = false;
        private bool FieldHideStock { get; set; } = false;
        private bool showStockProduct { get; set; } = false;
        private bool showScrapProduct { get; set; } = false;
        private bool showMaintaiananaceProduct { get; set; } = false;
        private long TotalQty { get; set; } = 0;
        private long? TotalScrapQty { get; set; }
        private long? TotalMaintenanceQty { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private string? NameUom { get; set; }
        private string? NameProduct { get; set; }
        private bool FormValidationState { get; set; } = true;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataStockItems { get; set; } = [];
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        private CultureInfo Culture = CultureInfo.GetCultureInfo("id-ID");

        #endregion Variable Static

        #region Status Maintenance

        public MarkupString GetIssueStatusIconHtmlMaintenance(EnumStatusMaintenance? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusMaintenance.Request:
                    priorityClass = "info";
                    title = "Request";
                    break;

                case EnumStatusMaintenance.InProgress:
                    priorityClass = "primary";
                    title = "In Progress";
                    break;

                case EnumStatusMaintenance.Repaired:
                    priorityClass = "warning";
                    title = "Repaire";
                    break;

                case EnumStatusMaintenance.Scrap:
                    priorityClass = "warning";
                    title = "Scrap";
                    break;

                case EnumStatusMaintenance.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusMaintenance.Canceled:
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

        #endregion Status Maintenance

        #region select data static

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
                    PostProductDetails.Cronies = true;
                }
                else
                {
                    Chronis = false;
                }
            }
        }

        #endregion select data static

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

        #region Load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            ActiveComponents = (await Mediator.QueryGetHelper<ActiveComponent, ActiveComponentDto>()).Item1;
            // Run multiple load tasks concurrently
            var loadTasks = new List<Task>
            {
                LoadDataUom(),
                LoadDataUomPurchase(),
                LoadDataAsync(),
                LoadDataDrugForm(),
                LoadDataDrugRoute(),
                LoadDataDrugDosage(),


                LoadData(),
                // LoadDataBPJSCl(),
                // LoadDataLocation(),
                LoadDataProductCategory()
            };

            await Task.WhenAll(loadTasks);

            PanelVisible = false;
        }


        private async Task LoadDataAsync()
        {
            getTransactionStocks = await Mediator.Send(new GetTransactionStockQuery());

            var resultScrap = await Mediator.Send(new GetMaintenanceProductQuery());
            GetMaintenanceProduct = resultScrap.Item1;

        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            showStockProduct = false;
            // Fetch product data
            var result = await Mediator.Send(new GetProductQuery(x => x.Id == Id, 0, 1));
            PostProduct = result.Item1.FirstOrDefault() ?? new();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("inventory/products");
                    return;
                }

                _SmartButton = true;

                // Fetch related data
                GetMedicaments = (await Mediator.Send(new GetMedicamentQuery
                {
                    Predicate = x => x.ProductId == PostProduct.Id
                })).Item1;
                PostMedicaments = GetMedicaments.FirstOrDefault() ?? new();
                GetMaintenance = await Mediator.Send(new GetAllMaintenanceQuery());

                // Map product details
                PostProductDetails = PostProduct.Adapt<ProductDetailDto>();
                NameProduct = PostProductDetails.Name;
                NameUom = GetUoms.FirstOrDefault(u => u.Id == PostProductDetails.UomId)?.Name;

                // Medicament-specific details
                if (PostProduct.HospitalType == "Medicament")
                {
                    if (PostMedicaments.ProductId != null)
                    {
                        // Update specific medicament details instead of replacing the entire object
                        UpdateMedicamentDetails(PostProductDetails, PostMedicaments);
                        if (PostMedicaments.ActiveComponentId != null)
                        {
                            selectedActiveComponents = ActiveComponents.Where(a => PostMedicaments.ActiveComponentId.Contains(a.Id)).ToList();
                        }

                        TotalQty = getTransactionStocks
                            .Where(x => x.ProductId == PostProduct.Id && x.Validate == true)
                            .Sum(z => z.Quantity);
                    }
                }
                // Medical equipment-specific details
                else if (PostProduct.HospitalType == "Medical Equipment")
                {
                    PostProductDetails.Brand = PostProduct.Brand;
                    HandleMedicalEquipmentStock();
                }
                else
                {
                    TotalQty = getTransactionStocks
                        .Where(x => x.ProductId == PostProduct.Id && x.Validate == true)
                        .Sum(z => z.Quantity);
                }
            }
            else
            {
                HandleNewProductDefaults();
            }

            // Fetch additional data
            //GetBPJSCl = await Mediator.Send(new GetBpjsClassificationQuery());
        }

        private void UpdateMedicamentDetails(ProductDetailDto postProductDetails, MedicamentDto medicament)
        {
            postProductDetails.MedicamentId = medicament.Id;
            postProductDetails.FormId = medicament.FormId;
            postProductDetails.RouteId = medicament.RouteId;
            postProductDetails.Dosage = medicament.Dosage;
            postProductDetails.UomMId = medicament.UomId;
            postProductDetails.Cronies = medicament.Cronies;
            postProductDetails.MontlyMax = medicament.MontlyMax;
            postProductDetails.FrequencyId = medicament.FrequencyId;
            postProductDetails.PregnancyWarning = medicament.PregnancyWarning;
            postProductDetails.Pharmacologi = medicament.Pharmacologi;
            postProductDetails.Weather = medicament.Weather;
            postProductDetails.Food = medicament.Food;
        }

        // Handles stock and maintenance for medical equipment
        private void HandleMedicalEquipmentStock()
        {
            TotalScrapQty = GetMaintenanceProduct
                .Count(x => x.Status == EnumStatusMaintenance.Scrap);

            TotalMaintenanceQty = GetMaintenanceProduct
                .Count(x => x.Status != EnumStatusMaintenance.Scrap);

            TotalQty = getTransactionStocks
                .Where(x => x.ProductId == PostProduct.Id && x.Validate)
                .Sum(z => z.Quantity);
        }

        // Handles default values for new products
        private void HandleNewProductDefaults()
        {
            if (PostProductDetails.UomId == 0)
            {
                PostProductDetails.UomId = GetUoms.FirstOrDefault(x => x.Name == "Unit")?.Id;
                SelectedChangeUoM(GetUoms.FirstOrDefault(x => x.Name == "Unit") ?? new());
            }

            PostProductDetails = new ProductDetailDto
            {
                ProductType = ProductTypes[2],
                HospitalType = HospitalProducts[0],
                SalesPrice = 100,
                Tax = "11%",
                UomId = GetUoms.FirstOrDefault(x => x.Name == "Unit")?.Id
            };

            SelectedChangeUoM(GetUoms.FirstOrDefault(x => x.Name == "Unit") ?? new());
        }

        #endregion Load Data

        #region Select Data

        private void SelectedChangeUoM(UomDto selectedUom)
        {
            if (selectedUom != null)
            {
                var dataUoM = GetUoms.FirstOrDefault(u => u.Id == selectedUom.Id) ?? new();
                if (dataUoM.Id != 0)
                {
                    PostProductDetails.UomId = dataUoM.Id;

                    // Only set PurchaseUomId if it's not manually changed

                    PostProductDetails.PurchaseUomId = dataUoM.Id;

                    StateHasChanged();
                }
            }
        }

        #endregion Select Data

        #region Load ComboBox

        #region ComboBox Uom

        private DxComboBox<UomDto, long?> refUomComboBox { get; set; }
        private int UomComboBoxIndex { get; set; } = 0;
        private int totalCountUom = 0;

        private async Task OnSearchUom()
        {
            await LoadDataUom(0, 10);
        }

        private async Task OnSearchUomIndexIncrement()
        {
            if (UomComboBoxIndex < (totalCountUom - 1))
            {
                UomComboBoxIndex++;
                await LoadDataUom(UomComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUomIndexDecrement()
        {
            if (UomComboBoxIndex > 0)
            {
                UomComboBoxIndex--;
                await LoadDataUom(UomComboBoxIndex, 10);
            }
        }

        private async Task OnInputUomChanged(string e)
        {
            UomComboBoxIndex = 0;
            await LoadDataUom(0, 10);
        }

        private async Task LoadDataUom(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetUomQuery
            {
                SearchTerm = refUomComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize
            });
            GetUoms = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Uom

        #region ComboBox UomPurchase

        private DxComboBox<UomDto, long?> refUomPurchaseComboBox { get; set; }
        private int UomPurchaseComboBoxIndex { get; set; } = 0;
        private int totalCountUomPurchase = 0;

        private async Task OnSearchUomPurchase()
        {
            await LoadDataUomPurchase(0, 10);
        }

        private async Task OnSearchUomPurchaseIndexIncrement()
        {
            if (UomPurchaseComboBoxIndex < (totalCountUom - 1))
            {
                UomPurchaseComboBoxIndex++;
                await LoadDataUomPurchase(UomPurchaseComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUomPurchaseIndexDecrement()
        {
            if (UomPurchaseComboBoxIndex > 0)
            {
                UomPurchaseComboBoxIndex--;
                await LoadDataUomPurchase(UomPurchaseComboBoxIndex, 10);
            }
        }

        private async Task OnInputUomPurchaseChanged(string e)
        {
            // Reset UomPurchaseComboBoxIndex and load new data based on user input
            UomPurchaseComboBoxIndex = 0;
            await LoadDataUomPurchase(0, 10);
        }

        private async Task LoadDataUomPurchase(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetUomQuery
            {
                SearchTerm = refUomPurchaseComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize
            });
            GetUomPurchases = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Uom

        #region Combo Box DrugForm

        private DxComboBox<DrugFormDto, long?> refDrugFormComboBox { get; set; }
        private int DrugFormComboBoxIndex { get; set; } = 0;
        private int totalCountDrugForm = 0;

        private async Task OnSearchDrugForm()
        {
            await LoadDataDrugForm(0, 10);
        }

        private async Task OnSearchDrugFormIndexIncrement()
        {
            if (DrugFormComboBoxIndex < (totalCountDrugForm - 1))
            {
                DrugFormComboBoxIndex++;
                await LoadDataDrugForm(DrugFormComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDrugFormIndexDecrement()
        {
            if (DrugFormComboBoxIndex > 0)
            {
                DrugFormComboBoxIndex--;
                await LoadDataDrugForm(DrugFormComboBoxIndex, 10);
            }
        }

        private async Task OnInputDrugFormChanged(string e)
        {
            DrugFormComboBoxIndex = 0;
            await LoadDataDrugForm(0, 10);
        }

        private async Task LoadDataDrugForm(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetDrugFormQuery { SearchTerm = refDrugFormComboBox?.Text, PageSize = pageSize, PageIndex= pageIndex});
            GetDrugForms = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion Combo Box DrugForm

        #region Combo Box DrugRoute

        private DxComboBox<DrugRouteDto, long?> refDrugRouteComboBox { get; set; }
        private int DrugRouteComboBoxIndex { get; set; } = 0;
        private int totalCountDrugRoute = 0;

        private async Task OnSearchDrugRoute()
        {
            await LoadDataDrugRoute(0, 10);
        }

        private async Task OnSearchDrugRouteIndexIncrement()
        {
            if (DrugRouteComboBoxIndex < (totalCountDrugRoute - 1))
            {
                DrugRouteComboBoxIndex++;
                await LoadDataDrugRoute(DrugRouteComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDrugRouteIndexDecrement()
        {
            if (DrugRouteComboBoxIndex > 0)
            {
                DrugRouteComboBoxIndex--;
                await LoadDataDrugRoute(DrugRouteComboBoxIndex, 10);
            }
        }

        private async Task OnInputDrugRouteChanged(string e)
        {
            DrugRouteComboBoxIndex = 0;
            await LoadDataDrugRoute(0, 10);
        }

        private async Task LoadDataDrugRoute(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetDrugRouteQuery { SearchTerm = refDrugRouteComboBox?.Text, PageSize = pageSize, PageIndex = pageIndex });
            GetDrugRoutes = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion Combo Box DrugRoute

        //#region Combo Box Location
        //private DxComboBox<LocationDto, long?> refLocationComboBox { get; set; }
        //private int LocationComboBoxIndex { get; set; } = 0;
        //private int totalCountLocation = 0;

        //private async Task OnSearchLocation()
        //{
        //    await LoadDataLocation(0, 10);
        //}

        //private async Task OnSearchLocationIndexIncrement()
        //{
        //    if (LocationComboBoxIndex < (totalCountLocation - 1))
        //    {
        //        LocationComboBoxIndex++;
        //        await LoadDataLocation(LocationComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnSearchLocationIndexDecrement()
        //{
        //    if (LocationComboBoxIndex > 0)
        //    {
        //        LocationComboBoxIndex--;
        //        await LoadDataLocation(LocationComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnInputLocationChanged(string e)
        //{
        //    LocationComboBoxIndex = 0;
        //    await LoadDataLocation(0, 10);
        //}

        //private async Task LoadDataLocation(int pageIndex = 0, int pageSize = 10)
        //{
        //    PanelVisible = true;
        //    SelectedDataItems = [];
        //    var result = await Mediator.Send(new GetLocationQuery(searchTerm: refLocationComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
        //    GetLocations = result.Item1;
        //    totalCount = result.pageCount;
        //    PanelVisible = false;
        //}
        //#endregion

        #region Combo Box Product Category

        private DxComboBox<ProductCategoryDto, long?> refProductCategoryComboBox { get; set; }
        private int ProductCategoryComboBoxIndex { get; set; } = 0;
        private int totalCountProductCategory = 0;

        private async Task OnSearchProductCategory()
        {
            await LoadDataProductCategory(0, 10);
        }

        private async Task OnSearchProductCategoryIndexIncrement()
        {
            if (ProductCategoryComboBoxIndex < (totalCountProductCategory - 1))
            {
                ProductCategoryComboBoxIndex++;
                await LoadDataProductCategory(ProductCategoryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProductCategoryIndexDecrement()
        {
            if (ProductCategoryComboBoxIndex > 0)
            {
                ProductCategoryComboBoxIndex--;
                await LoadDataProductCategory(ProductCategoryComboBoxIndex, 10);
            }
        }

        private async Task OnInputProductCategoryChanged(string e)
        {
            ProductCategoryComboBoxIndex = 0;
            await LoadDataProductCategory(0, 10);
        }

        private async Task LoadDataProductCategory(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProductCategoryQuery(searchTerm: refProductCategoryComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetProductCategories = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion Combo Box Product Category

        #region ComboBox Drug Dosage

        private DxComboBox<DrugDosageDto, long?> refDrugDosageComboBox { get; set; }
        private int DrugDosageComboBoxIndex { get; set; } = 0;
        private int totalCountDrugDosage = 0;

        private async Task OnSearchDrugDosage()
        {
            await LoadDataDrugDosage(0, 10);
        }

        private async Task OnSearchDrugDosageIndexIncrement()
        {
            if (DrugDosageComboBoxIndex < (totalCountDrugDosage - 1))
            {
                DrugDosageComboBoxIndex++;
                await LoadDataDrugDosage(DrugDosageComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDrugDosageIndexDecrement()
        {
            if (DrugDosageComboBoxIndex > 0)
            {
                DrugDosageComboBoxIndex--;
                await LoadDataDrugDosage(DrugDosageComboBoxIndex, 10);
            }
        }

        private async Task OnInputDrugDosageChanged(string e)
        {
            DrugDosageComboBoxIndex = 0;
            await LoadDataDrugDosage(0, 10);
        }

        private async Task LoadDataDrugDosage(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetDrugDosageQuery { SearchTerm = refDrugDosageComboBox?.Text, PageSize = pageSize, PageIndex = pageIndex });
            GetDrugDosage = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Uom

        //#region ComboBox BPJS Classification
        //private DxComboBox<BpjsClassificationDto, long?> refBPJSClComboBox { get; set; }
        //private int BPJSClComboBoxIndex { get; set; } = 0;
        //private int totalCountBPJSCl = 0;

        //private async Task OnSearchBPJSCl()
        //{
        //    await LoadDataBPJSCl(0, 10);
        //}

        //private async Task OnSearchBPJSClIndexIncrement()
        //{
        //    if (BPJSClComboBoxIndex < (totalCountBPJSCl - 1))
        //    {
        //        BPJSClComboBoxIndex++;
        //        await LoadDataBPJSCl(BPJSClComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnSearchBPJSClIndexDecrement()
        //{
        //    if (BPJSClComboBoxIndex > 0)
        //    {
        //        BPJSClComboBoxIndex--;
        //        await LoadDataBPJSCl(BPJSClComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnInputBPJSClChanged(string e)
        //{
        //    BPJSClComboBoxIndex = 0;
        //    await LoadDataBPJSCl(0, 10);
        //}

        //private async Task LoadDataBPJSCl(int pageIndex = 0, int pageSize = 10)
        //{
        //    PanelVisible = true;
        //    SelectedDataItems = [];
        //    var result = await Mediator.Send(new GetBPJSIntegrationQuery(searchTerm: refUomComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
        //    GetBPJSCls = result.Item1;
        //    totalCount = result.pageCount;
        //    PanelVisible = false;
        //}
        //#endregion

        #endregion Load ComboBox

        #region Smart Button

        private async Task NewTableStock_Item()
        {
            //NavigationManager.NavigateTo($"inventory/products/stock-product/{EnumPageMode.Update.GetDisplayName()}?Id={PostProductDetails.Id}");
            //return;

            try
            {
                showStockProduct = true;
                PanelVisible = true;
                if (SelectedDataItems.Count == 0)
                {
                    // Jika tidak ada item yang dipilih, gunakan produk yang sedang dipertimbangkan

                    if (PostProduct.TraceAbility == true)
                    {
                        StockProducts = getTransactionStocks.Where(x => x.ProductId == PostProduct.Id && x.Validate == true)
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
                        StockProducts = getTransactionStocks.Where(x => x.ProductId == PostProduct.Id && x.Validate == true)
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
                }
                else
                {
                    // Jika ada item yang dipilih, gunakan produk yang dipilih
                    StockProducts = getTransactionStocks.Where(x => x.ProductId == PostProduct.Id && x.Validate == true)
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

                    if (PostProduct.TraceAbility == true)
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

        private async Task NewTableEquipment_Scrap()
        {
            try
            {
                PanelVisible = true;
                showScrapProduct = true;
                GetMaintenanceScrap = GetMaintenanceProduct.Where(x => x.ProductId == Id && x.Status == EnumStatusMaintenance.Scrap).ToList();
                NameProduct = GetMaintenanceScrap[0].Product.Name;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task NewTableEquipment_Item()
        {
            try
            {
                showMaintaiananaceProduct = true;
                PanelVisible = true;

                // Modifikasi query untuk menampilkan 1 jika Statusnya NULL
                GetMaintenanceHistory = GetMaintenanceProduct
                    .Where(x => x.ProductId == Id && (x.Status ?? EnumStatusMaintenance.InProgress) != EnumStatusMaintenance.Scrap)
                    .Select(x =>
                    {
                        // Menampilkan Status sebagai 1 jika NULL
                        x.Status = x.Status ?? EnumStatusMaintenance.InProgress; // Asumsi: Pending atau 1 sebagai default
                        return x;
                    })
                    .ToList();

                NameProduct = GetMaintenanceHistory[0].Product.Name;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }


        #endregion Smart Button

        #region Button

        private void onDiscard()
        {
            NavigationManager.NavigateTo("inventory/products");
        }

        private async Task Back_Click()
        {
            showStockProduct = false;
            StockProducts = [];
            showScrapProduct = false;
            GetMaintenanceScrap = [];
            showMaintaiananaceProduct = false;
            GetMaintenanceHistory = [];
        }

        private async Task RefreshStock_Click()
        {
            await NewTableStock_Item();
        }

        private async Task RefreshScrap_Click()
        {
            await NewTableEquipment_Scrap();
        }

        private async Task RefreshMaintenance_Click()
        {
            await NewTableEquipment_Item();
        }

        #endregion Button

        #region Handler Vaidation

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await OnSave();
            else
                FormValidationState = true;

        }

        private async Task HandleInvalidSubmit()
        {
            FormValidationState = false;
        }

        #endregion Handler Vaidation

        #region Save

        private async Task OnSave()
        {
            try
            {
                if (FormValidationState == false)
                {
                    return;
                }

                if (PostProductDetails?.Name == null)
                {
                    HandleInvalidSubmit();
                    return;
                }

                SetPostProductDetails();
                if (PostProductDetails.HospitalType == "Medicament")
                {
                    // Validasi jika SetFormMedicamentDetails() tidak memiliki data
                    if (!ValidateMedicamentDetails())
                    {
                        ToastService.ShowError("Medicament details are incomplete or invalid.");
                        return;
                    }
                    SetFormMedicamentDetails();
                }

                if (PostProductDetails.Id == 0)
                {
                    await CreateNewProductAndMedicament();

                    ToastService.ShowSuccess("Successfully Added Data...");
                }
                else
                {
                    await UpdateExistingProductAndMedicament();
                    ToastService.ShowSuccess("Successfully Updated Data...");
                }

                NavigationManager.NavigateTo($"inventory/products/{EnumPageMode.Update.GetDisplayName()}?Id={TempProduct.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void SetPostProductDetails()
        {
            PostProduct.Id = PostProductDetails.Id;
            PostProduct.Name = PostProductDetails.Name;
            PostProduct.ProductCategoryId = PostProductDetails.ProductCategoryId;
            PostProduct.ProductType = PostProductDetails.ProductType;
            PostProduct.HospitalType = PostProductDetails.HospitalType;
            PostProduct.BpjsClassificationId = PostProductDetails.BpjsClassificationId;
            PostProduct.UomId = PostProductDetails.UomId;
            PostProduct.PurchaseUomId = PostProductDetails.PurchaseUomId;
            PostProduct.SalesPrice = PostProductDetails.SalesPrice;
            PostProduct.Tax = PostProductDetails.Tax;
            PostProduct.Cost = PostProductDetails.Cost;
            PostProduct.InternalReference = PostProductDetails.InternalReference;
            PostProduct.IsOralMedication = PostProductDetails.IsOralMedication;
            PostProduct.IsTopicalMedication = PostProductDetails.IsTopicalMedication;
            PostProduct.TraceAbility = PostProductDetails.TraceAbility;
            PostProduct.Brand = PostProductDetails.Brand;
            PostProduct.EquipmentCode = PostProductDetails.EquipmentCode;
            PostProduct.YearOfPurchase = PostProductDetails.YearOfPurchase;
            PostProduct.LastCalibrationDate = PostProductDetails.LastCalibrationDate;
            PostProduct.NextCalibrationDate = PostProductDetails.NextCalibrationDate;
            PostProduct.EquipmentCondition = PostProductDetails.EquipmentCondition;
        }

        private void SetFormMedicamentDetails()
        {
            PostMedicaments.Id = PostProductDetails.MedicamentId ?? 0;
            PostMedicaments.FormId = PostProductDetails.FormId;
            PostMedicaments.RouteId = PostProductDetails.RouteId;
            PostMedicaments.Dosage = PostProductDetails.Dosage;
            PostProduct.IsOralMedication = PostProductDetails.IsOralMedication;
            PostProduct.IsTopicalMedication = PostProductDetails.IsTopicalMedication;
            PostMedicaments.UomId = PostProductDetails.UomMId;
            PostMedicaments.Cronies = PostProductDetails.Cronies;
            PostMedicaments.MontlyMax = PostProductDetails.MontlyMax;
            PostMedicaments.FrequencyId = PostProductDetails.FrequencyId;
            PostMedicaments.PregnancyWarning = PostProductDetails.PregnancyWarning;
            PostMedicaments.Pharmacologi = PostProductDetails.Pharmacologi;
            PostMedicaments.Weather = PostProductDetails.Weather;
            PostMedicaments.Food = PostProductDetails.Food;

            if (selectedActiveComponents != null)
            {
                PostMedicaments.ActiveComponentId?.AddRange(selectedActiveComponents.Select(x => x.Id));
            }

        }

        private bool ValidateMedicamentDetails()
        {
            // Validasi jika data dalam PostMedicaments kosong atau tidak valid
            if (PostProductDetails.FormId == null ||
                PostProductDetails.RouteId == null ||
                PostProductDetails.Dosage == 0 ||
                PostProductDetails.UomMId == null)
            {
                return false;
            }

            return true;
        }

        private async Task CreateNewProductAndMedicament()
        {
            TempProduct = await Mediator.Send(new CreateProductRequest(PostProduct));

            if (PostProductDetails.HospitalType == "Medicament")
            {
                PostMedicaments.ProductId = TempProduct.Id;

                if (PostMedicaments.Id == 0)
                {
                    var aazd = await Mediator.Send(new CreateMedicamentRequest(PostMedicaments));
                }
                else
                {
                    await Mediator.Send(new UpdateMedicamentRequest(PostMedicaments));
                }
            }
        }

        private async Task UpdateExistingProductAndMedicament()
        {
            TempProduct = await Mediator.Send(new UpdateProductRequest(PostProduct));

            if (PostProductDetails.HospitalType == "Medicament")
            {
                PostMedicaments.ProductId = TempProduct.Id;

                if (PostMedicaments.Id == 0)
                {
                    await Mediator.Send(new CreateMedicamentRequest(PostMedicaments));
                }
                else
                {
                    await Mediator.Send(new UpdateMedicamentRequest(PostMedicaments));
                }
            }
        }

        #endregion Save
    }
}
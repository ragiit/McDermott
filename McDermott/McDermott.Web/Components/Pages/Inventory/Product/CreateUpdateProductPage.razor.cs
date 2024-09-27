using MailKit.Search;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;
using static McDermott.Application.Features.Commands.Pharmacy.DrugFormCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;

namespace McDermott.Web.Components.Pages.Inventory.Product
{
    public partial class CreateUpdateProductPage
    {
        #region Relation Data
        private List<ProductDto> GetProduct = [];
        private List<MedicamentDto> GetMedicaments = [];
        private List<BpjsClassificationDto> GetBPJSCl = [];
        private List<UomDto> GetUoms = [];
        private List<DrugFormDto> GetDrugForms = [];
        private List<DrugRouteDto> GetDrugRoutes = [];
        private List<ProductCategoryDto> GetProductCategories = [];
        private List<ActiveComponentDto> ActiveComponents = [];
        private List<DrugDosageDto> GetDrugDosage = [];
        private List<LocationDto> GetLocations = [];
        private List<StockProductDto> StockProducts = [];
        private List<TransactionStockDto> TransactionStocks = [];
        private List<MaintainanceDto> GetMaintainance = [];
        private ProductDto PostProduct = new();
        private ProductDto TempProduct = new();
        private MedicamentDto PostMedicaments = new();
        private ProductDetailDto PostProductDetails = new();
        #endregion

        #region Variable Static
        [SupplyParameterFromQuery]
        private long? Id { get; set; }
        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();
        private IGrid Grid;
        private bool PanelVisible { get; set; } = false;
        private bool _SmartButton { get; set; } = false;
        private bool showTabs { get; set; } = false;
        private bool Checkins { get; set; } = false;
        private bool Chronis { get; set; } = false;
        private bool FieldHideStock { get; set; } = false;
        private long? TotalQty { get; set; }
        private long? TotalScrapQty { get; set; }
        private long? TotalMaintainanceQty { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private string? NameUom { get; set; }
        private string? NameProduct { get; set; }
        private bool FormValidationState { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        private CultureInfo Culture = CultureInfo.GetCultureInfo("id-ID");
        #endregion

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
        #endregion

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
            await LoadDataUom();
            //await LoadDataBPJSCl();
            await LoadDataDrugForm();
            await LoadDataDrugRoute();
            await LoadDataLocation();
            await LoadDataProductCategory();
            PanelVisible = false;
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetProductQuery(x => x.Id == Id, 0, 1));
            PostProductDetails = new();
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("inventory/products");
                    return;
                }

                PostProduct = result.Item1.FirstOrDefault() ?? new();
                GetMedicaments = await Mediator.Send(new GetMedicamentQuery(x => x.ProductId == PostProduct.Id));
                PostMedicaments = GetMedicaments.FirstOrDefault() ?? new();
                var maintainanceResult = await Mediator.Send(new GetMaintainanceQuery(searchTerm: searchTerm ?? "", pageSize: 0, pageIndex: 1));
                GetMaintainance = maintainanceResult.Item1;

                //Type Medicament 
                if (PostProduct.HospitalType == "Medicament")
                {
                    if (PostMedicaments != null)
                    {
                        PostProductDetails.MedicamentId = PostMedicaments.Id;
                        PostProductDetails.FormId = PostMedicaments.FormId;
                        PostProductDetails.RouteId = PostMedicaments.RouteId;
                        PostProductDetails.Dosage = PostMedicaments.Dosage;
                        PostProductDetails.UomMId = PostMedicaments.UomId;
                        PostProductDetails.Cronies = PostMedicaments.Cronies;
                        PostProductDetails.MontlyMax = PostMedicaments.MontlyMax;
                        PostProductDetails.FrequencyId = PostMedicaments.FrequencyId;

                        // Ambil komponen aktif jika tersedia
                        if (PostMedicaments.ActiveComponentId != null)
                        {
                            selectedActiveComponents = ActiveComponents.Where(a => PostMedicaments.ActiveComponentId.Contains(a.Id)).ToList();
                        }

                        PostProductDetails.PregnancyWarning = PostMedicaments.PregnancyWarning;
                        PostProductDetails.Pharmacologi = PostMedicaments.Pharmacologi;
                        PostProductDetails.Weather = PostMedicaments.Weather;
                        PostProductDetails.Food = PostMedicaments.Food;
                    }
                }
                // Kelola informasi stok
                if (PostProduct.HospitalType != "Medical Equipment")
                {
                    TotalQty = TransactionStocks.Where(x => x.ProductId == PostProduct.Id && x.Validate == true).Sum(z => z.Quantity);
                }
                else
                {
                    var productScrap = GetMaintainance.Where(x => x.Status == EnumStatusMaintainance.Scrap).FirstOrDefault();
                    var productMaintainance = GetMaintainance.Where(x => x.Status != EnumStatusMaintainance.Scrap).FirstOrDefault();
                    TotalScrapQty = GetMaintainance.Where(x => x.EquipmentId == PostProduct.Id && x.Status == EnumStatusMaintainance.Scrap).Count();
                    TotalQty = TransactionStocks.Where(x => x.ProductId == PostProduct.Id && x.Validate == true).Sum(z => z.Quantity);
                    TotalMaintainanceQty = GetMaintainance.Where(x => x.EquipmentId == PostProduct.Id && x.Status != EnumStatusMaintainance.Scrap).Count();
                }
                // Ambil nama satuan ukur
               
            }

            NameUom = GetUoms.FirstOrDefault(u => u.Id == PostProductDetails.UomId)?.Name;

            if (PostProductDetails.UomId == 0)
            {
                PostProductDetails.UomId = GetUoms.FirstOrDefault(x => x.Name == "Unit")?.Id ?? 0;

                SelectedChangeUoM(GetUoms.FirstOrDefault(x => x.Name == "Unit") ?? new());
            }

            PostProductDetails = new ProductDetailDto
            {
                ProductType = ProductTypes[2],
                HospitalType = HospitalProducts[0],
                SalesPrice = 100,
                Tax = "11%",
                UomId = GetUoms.FirstOrDefault(x => x.Name == "Unit")?.Id ?? 0
            };

            SelectedChangeUoM(GetUoms.FirstOrDefault(x => x.Name == "Unit") ?? new());

            GetBPJSCl = await Mediator.Send(new GetBpjsClassificationQuery());
            ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
        }
        #endregion

        #region Select Data
        private void SelectedChangeUoM(UomDto UomId)
        {
            if (UomId != null)
            {
                var UoMId = GetUoms.Where(u => u.Id == UomId.Id).FirstOrDefault() ?? new();
                PostProductDetails.PurchaseUomId = UoMId.Id;
            }
        }
        #endregion

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
            var result = await Mediator.Send(new GetUomQuery(searchTerm: refUomComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetUoms = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion

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
            var result = await Mediator.Send(new GetDrugFormQuery(searchTerm: refDrugFormComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetDrugForms = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion

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
            var result = await Mediator.Send(new GetDrugRouteQuery(searchTerm: refDrugRouteComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetDrugRoutes = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion

        #region Combo Box Location
        private DxComboBox<LocationDto, long?> refLocationComboBox { get; set; }
        private int LocationComboBoxIndex { get; set; } = 0;
        private int totalCountLocation = 0;

        private async Task OnSearchLocation()
        {
            await LoadDataLocation(0, 10);
        }

        private async Task OnSearchLocationIndexIncrement()
        {
            if (LocationComboBoxIndex < (totalCountLocation - 1))
            {
                LocationComboBoxIndex++;
                await LoadDataLocation(LocationComboBoxIndex, 10);
            }
        }

        private async Task OnSearchLocationIndexDecrement()
        {
            if (LocationComboBoxIndex > 0)
            {
                LocationComboBoxIndex--;
                await LoadDataLocation(LocationComboBoxIndex, 10);
            }
        }

        private async Task OnInputLocationChanged(string e)
        {
            LocationComboBoxIndex = 0;
            await LoadDataLocation(0, 10);
        }

        private async Task LoadDataLocation(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLocationQuery(searchTerm: refLocationComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetLocations = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion

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
        #endregion

        #region ComboBox Uom
        private DxComboBox<DrugDosageDto, long?> refDrugDosageComboBox { get; set; }
        private int DrugDosageComboBoxIndex { get; set; } = 0;
        private int totalCountDrugDosage = 0;

        private async Task OnSearchDrugDosage()
        {
            await LoadDataDrugDosage(0, 10);
        }

        private async Task OnSearchDrugDosageIndexIncrement()
        {
            if (UomComboBoxIndex < (totalCountUom - 1))
            {
                UomComboBoxIndex++;
                await LoadDataDrugDosage(DrugDosageComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDrugDosageIndexDecrement()
        {
            if (DrugDosageComboBoxIndex > 0)
            {
                UomComboBoxIndex--;
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
            var result = await Mediator.Send(new GetDrugDosageQuery(searchTerm: refUomComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetDrugDosage = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion


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

        #endregion

        #region Smart Button
        private async Task NewTableStock_Item()
        {
            NavigationManager.NavigateTo($"inventory/maintainance/scrap/{EnumPageMode.Update.GetDisplayName()}?Id={PostProductDetails.Id}");
            return;

            try
            {


                if (SelectedDataItems.Count == 0)
                {
                    // Jika tidak ada item yang dipilih, gunakan produk yang sedang dipertimbangkan

                    if (PostProduct.TraceAbility == true)
                    {
                        StockProducts = TransactionStocks.Where(x => x.ProductId == PostProduct.Id && x.Validate == true)
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
                        StockProducts = TransactionStocks.Where(x => x.ProductId == PostProduct.Id && x.Validate == true)
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
                    NameProduct = PostProduct.Name;
                }
                else
                {
                    // Jika ada item yang dipilih, gunakan produk yang dipilih
                    StockProducts = TransactionStocks.Where(x => x.ProductId == PostProduct.Id && x.Validate == true)
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
            NavigationManager.NavigateTo($"inventory/maintainance-scrap/{EnumPageMode.Update.GetDisplayName()}?Id={PostProductDetails.ProductId}");
            return;
        }

        private async Task NewTableEquipment_Item()
        {
            NavigationManager.NavigateTo($"inventory/maintainance-history/{EnumPageMode.Update.GetDisplayName()}?Id={PostProductDetails.ProductId}");
            return;
        }
        #endregion

        #region Button
        private async Task onDiscard()
        {
            NavigationManager.NavigateTo($"inventory/products");
            return;
        }

        #endregion

        #region Handler Vaidation
        private async Task HandleValidSubmit()
        {
            //IsLoading = true;
            FormValidationState = true;
            //await OnSave();
            //IsLoading = false;
        }

        private async Task HandleInvalidSubmit()
        {

            FormValidationState = false;
        }

        #endregion

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
                SetFormMedicamentDetails();

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
            PostMedicaments.ProductId = TempProduct?.Id ?? 0;
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

        private async Task CreateNewProductAndMedicament()
        {
            TempProduct = await Mediator.Send(new CreateProductRequest(PostProduct));

            if (PostProductDetails.HospitalType == "Medicament")
            {
                PostMedicaments.ProductId = PostProduct.Id;

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

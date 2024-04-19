using static McDermott.Application.Features.Commands.Inventory.ProductCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacy.SignaCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class ProductPage
    {
        #region Relation Data

        private List<ProductDto> Products = [];
        private List<MedicamentDto> Medicaments = [];
        private List<BpjsClassificationDto> BpjsClassifications = [];
        private List<UomDto> Uoms = [];
        private List<DrugFormDto> DrugForms = [];
        private List<DrugRouteDto> DrugRoutes = [];
        private List<ProductCategoryDto> productCategories = [];
        private List<ActiveComponentDto> ActiveComponents = [];
        private List<SignaDto> Signas = [];
        private ProductDetailDto FormProductDetails = new();
        private ProductDto FormProducts = new();
        private MedicamentDto FormMedicaments = new();

        #endregion Relation Data

        #region Static

        private IGrid? Grid { get; set; }
        private bool showForm { get; set; } = false;
        private bool PanelVisible { get; set; } = false;
        private bool showTabs { get; set; } = true;
        private bool Checkins { get; set; } = false;
        private bool Chronis { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];

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
            PanelVisible = true;
            showForm = false;
            SelectedDataItems = [];
            Products = await Mediator.Send(new GetProductQuery());
            PanelVisible = false;
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

        #endregion Grid

        #region Click

        private async Task NewItem_Click()
        {
            
            showForm = true;
            FormProductDetails = new();
            FormProductDetails.ProductType = ProductTypes[2];
            FormProductDetails.HospitalType = HospitalProducts[0];
            BpjsClassifications = await Mediator.Send(new GetBpjsClassificationQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            productCategories = await Mediator.Send(new GetProductCategoryQuery());
            DrugForms = await Mediator.Send(new GetFormDrugQuery());
            DrugRoutes = await Mediator.Send(new GetDrugRouteQuery());
            FormProductDetails.SalesPrice = "100";
            Signas = await Mediator.Send(new GetSignaQuery());
            ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
            FormProductDetails.Tax = "11%";
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            showForm = true;
            var products = SelectedDataItems[0].Adapt<ProductDto>();
            var medicamen = Medicaments.FirstOrDefault(z => z.ProductId == products?.Id);
            FormProductDetails.Name = products.Name;
            FormProductDetails.ProductCategoryId = products.ProductCategoryId;
            FormProductDetails.ProductType = products.ProductType;
            FormProductDetails.HospitalType = products.HospitalType;
            FormProductDetails.BpjsClasificationId = products.BpjsClasificationId;
            FormProductDetails.UomId = products.UomId;
            FormProductDetails.PurchaseUomId = products.PurchaseUomId;
            FormProductDetails.SalesPrice = products.SalesPrice;
            FormProductDetails.Tax = products.Tax;
            FormProductDetails.Cost = products.Cost;
            FormProductDetails.ProductCategoryId = products.ProductCategoryId;
            FormProductDetails.InternalReference = products.InternalReference;
            if (products.HospitalType == "Medicament")
            {
                if (medicamen != null)
                {
                    FormProductDetails.FormId = medicamen.FormId;
                    FormProductDetails.RouteId = medicamen.RouteId;
                    FormProductDetails.Dosage = medicamen.Dosage;
                    FormProductDetails.UomId = medicamen.UomId;
                    FormProductDetails.Cronies = medicamen.Cronies;
                    FormProductDetails.MontlyMax = medicamen.MontlyMax;
                    FormProductDetails.SignaId = medicamen.SignaId;
                    FormProductDetails.ActiveComponentId = medicamen.ActiveComponentId;
                    FormProductDetails.PregnancyWarning = medicamen.PregnancyWarning;
                    FormProductDetails.Pharmacologi = medicamen.Pharmacologi;
                    FormProductDetails.Weather = medicamen.Weather;
                    FormProductDetails.Food = medicamen.Food;
                }
            }
        }

        private async void onDiscard()
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
                    await Mediator.Send(new DeleteProductCategoryRequest(((ProductDto)e.DataItem).Id));
                }
                else
                {
                    List<long> MProductId = SelectedDataItems.Adapt<List<ProductDto>>().Select(x => x.Id).ToList();
                    foreach(var data in MProductId)
                    {
                        
                        var checkData = Medicaments.Where(m => m.ProductId == data).FirstOrDefault();
                        if (checkData != null)
                        {
                            await Mediator.Send(new DeleteMedicamentRequest(checkData.Id));
                        }
                    }
                    await Mediator.Send(new DeleteProductRequest(ids: SelectedDataItems.Adapt<List<ProductDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        #endregion Click

        #region Save

        private async Task OnSave()
        {
            try
            {
                if (FormProductDetails.Id == 0)
                {
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
                    if (selectedActiveComponents != null)
                    {
                        var listActiveComponent = selectedActiveComponents.Select(x => x.Id).ToList();
                        FormProductDetails.ActiveComponentId?.AddRange(listActiveComponent);
                    }
                    ProductDto getProduct = new();
                    if (FormProducts.Id == 0)
                    {
                        getProduct = await Mediator.Send(new CreateProductRequest(FormProducts));
                    }

                    // Medicament
                    if (FormProductDetails.HospitalType == "Medicament")
                    {
                        FormMedicaments.ProductId = getProduct.Id;
                        FormMedicaments.FormId = FormProductDetails.FormId;
                        FormMedicaments.RouteId = FormProductDetails.RouteId;
                        FormMedicaments.Dosage = FormProductDetails.Dosage;
                        FormMedicaments.UomId = FormProductDetails.UomMId;
                        FormMedicaments.Cronies = FormProductDetails.Cronies;
                        FormMedicaments.MontlyMax = FormProductDetails.MontlyMax;
                        FormMedicaments.SignaId = FormProductDetails.SignaId;
                        FormMedicaments.ActiveComponentId = FormProductDetails.ActiveComponentId;
                        FormMedicaments.PregnancyWarning = FormProductDetails.PregnancyWarning;
                        FormMedicaments.Pharmacologi = FormProductDetails.Pharmacologi;
                        FormMedicaments.Weather = FormProductDetails.Weather;
                        FormMedicaments.Food = FormProductDetails.Food;

                        if (FormMedicaments.Id == 0)
                        {
                            await Mediator.Send(new CreateMedicamentRequest(FormMedicaments));
                        }
                        else
                        {
                            await Mediator.Send(new UpdateMedicamentRequest(FormMedicaments));
                        }
                    }
                    await LoadData();
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            await LoadData();
        }

        #endregion Save
    }
}
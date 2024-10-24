using DevExpress.XtraPrinting.Native.Properties;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GreenDonut;
using MailKit.Search;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.Inventory.GoodsReceiptCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacy.DrugFormCommand;

namespace McDermott.Web.Components.Pages.Inventory.GoodsReceipt
{
    public partial class CreateUpdateGoodsReceiptPage
    {
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
            //            StateHasChanged();
            //        }
            //    }
            //    catch { }

            //    var user_group = await Mediator.Send(new GetUserQuery());
            //    NameUser = user_group.FirstOrDefault(x => x.GroupId == UserAccessCRUID.GroupId && x.Id == UserLogin.Id) ?? new();
            //    StateHasChanged();

            //    await LoadAsyncData();
            //    StateHasChanged();
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

        #region Relation Data
        private List<GoodsReceiptDto> getGoodsReceipts = [];
        private List<GoodsReceiptLogDto> getGoodsReceiptLogs = [];
        private List<GoodsReceiptDetailDto> getGoodsReceiptDetails = [];
        private List<UomDto> getUoms = [];
        private List<ProductDto> getProduct = [];
        private List<LocationDto> getSource = [];
        private List<LocationDto> getDestination = [];
        private List<TransactionStockDto> getTransactionStocks = [];

        private GoodsReceiptDto postGoodsReceipt = new();
        private GoodsReceiptLogDto postGoodsReceiptLogs = new();
        private GoodsReceiptDetailDto postGoodsReceiptDetail = new();
        private TransactionStockDto postTransaction = new();
        #endregion

        #region variabel Data

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool FormValidationState { get; set; } = true;
        private bool isGrChecked { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        #endregion

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadDetailData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadDetailData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadDetailData(newPageIndex, pageSize);
        }

        #endregion Searching

        #region Load data
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataDestination();
            await LoadDataSource();
            await LoadAsyncData();
            await LoadDataProduct();
            PanelVisible = false;

        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetSingleGoodsReceiptQuery
            {
                Predicate = x => x.Id == Id
            });
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("inventory/goods-receipts");
                    return;
                }
                postGoodsReceipt = result ?? new();
                await LoadDetailData();
                // log
                var resultLog = await Mediator.Send(new GetGoodsReceiptLogQuery
                {
                    Predicate = x => x.GoodsReceiptId == postGoodsReceipt.Id
                });

                getGoodsReceiptLogs = resultLog.Item1 ?? new();
            }

            
            PanelVisible = false;
        }

        private async Task LoadAsyncData()
        {
            getTransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
            getUoms = await Mediator.Send(new GetAllUomQuery());

        }

        private async Task LoadDetailData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetGoodsReceiptDetailQuery
                {
                    Predicate = x => x.GoodsReceiptId == postGoodsReceipt.Id,
                    Includes = [
                        x=>x.Product.PurchaseUom,
                        x=>x.Product.Uom,
                        ],
                    SearchTerm = searchTerm,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                });
                getGoodsReceiptDetails = result.Item1 ?? new();
                totalCount = result.PageCount;
                activePageIndex = pageIndex;
                PanelVisible = false;
            }
            catch
            {

            }
        }

        private async Task CheckBatch(string value)
        {
            var checkData = getTransactionStocks.Where(x => x.Batch == value).FirstOrDefault();

            if (checkData != null)
            {
                postGoodsReceiptDetail.Batch = value;
                postGoodsReceiptDetail.ExpiredDate = checkData.ExpiredDate;
            }
            else
            {
                postGoodsReceiptDetail.Batch = value;
            }
        }
        #endregion

        #region Change Data
        private void SelectedChangeProduct(ProductDto e)
        {
            try
            {
                ResetFormProductDetail();
                if (e is null)
                    return;

                postGoodsReceiptDetail.TraceAbility = e.TraceAbility;

                if (e is not null)
                {
                    var productName = getProduct.Where(p => p.Id == e.Id).FirstOrDefault()!;
                    var uomName = getUoms.Where(u => u.Id == e.UomId).Select(x => x.Name).FirstOrDefault();
                    var purchaseName = getUoms.Where(u => u.Id == e.PurchaseUomId).Select(x => x.Name).FirstOrDefault()!;
                    postGoodsReceiptDetail.PurchaseName = purchaseName;
                    postGoodsReceiptDetail.UomName = uomName;
                    postGoodsReceiptDetail.ProductName = productName.Name;
                    postGoodsReceiptDetail.TraceAbility = productName.TraceAbility;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void ResetFormProductDetail()
        {
            postGoodsReceiptDetail.PurchaseName = null;
            postGoodsReceiptDetail.UomName = null;
            postGoodsReceiptDetail.ProductName = null;
            postGoodsReceiptDetail.TraceAbility = false;
        }

        public MarkupString GetIssueStatusIconHtml(EnumStatusGoodsReceipt? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusGoodsReceipt.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumStatusGoodsReceipt.Process:
                    priorityClass = "warning";
                    title = "Process";
                    break;

                case EnumStatusGoodsReceipt.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusGoodsReceipt.Cancel:
                    priorityClass = "danger";
                    title = "Cancel";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #endregion

        #region Grid
        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }
        #endregion

        #region Load ComboBox

        #region ComboBox Destination

        private DxComboBox<LocationDto, long?> refDestinationComboBox { get; set; }
        private int DestinationComboBoxIndex { get; set; } = 0;
        private int totalCountDestination = 0;

        private async Task OnSearchDestination()
        {
            await LoadDataDestination(0, 10);
        }

        private async Task OnSearchDestinationIndexIncrement()
        {
            if (DestinationComboBoxIndex < (totalCountDestination - 1))
            {
                DestinationComboBoxIndex++;
                await LoadDataDestination(DestinationComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDestinationIndexDecrement()
        {
            if (DestinationComboBoxIndex > 0)
            {
                DestinationComboBoxIndex--;
                await LoadDataDestination(DestinationComboBoxIndex, 10);
            }
        }

        private async Task OnInputDestinationChanged(string e)
        {
            DestinationComboBoxIndex = 0;
            await LoadDataDestination(0, 10);
        }

        private async Task LoadDataDestination(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLocationQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refDestinationComboBox?.Text ?? "",
                Select = x => new Locations
                {
                    Id = x.Id,
                    Name = x.Name,
                }
            }); getDestination = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Destination

        #region ComboBox Product

        private DxComboBox<ProductDto, long?> refProductComboBox { get; set; }
        private int ProductComboBoxIndex { get; set; } = 0;
        private int totalCountProduct = 0;

        private async Task OnSearchProduct()
        {
            await LoadDataProduct(0, 10);
        }

        private async Task OnSearchProductIndexIncrement()
        {
            if (ProductComboBoxIndex < (totalCountProduct - 1))
            {
                ProductComboBoxIndex++;
                await LoadDataProduct(ProductComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProductIndexDecrement()
        {
            if (ProductComboBoxIndex > 0)
            {
                ProductComboBoxIndex--;
                await LoadDataProduct(ProductComboBoxIndex, 10);
            }
        }

        private async Task OnInputProductChanged(string e)
        {
            // Reset UomComboBoxIndex and load new data based on user input
            ProductComboBoxIndex = 0;
            await LoadDataProduct(0, 10);
        }

        private async Task LoadDataProduct(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProductQuery(searchTerm: refProductComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getProduct = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Uom

        #region Combo Box Source

        private DxComboBox<LocationDto, long?> refSourceComboBox { get; set; }
        private int SourceComboBoxIndex { get; set; } = 0;
        private int totalCountSource = 0;

        private async Task OnSearchSource()
        {
            await LoadDataSource(0, 10);
        }

        private async Task OnSearchSourceIndexIncrement()
        {
            if (SourceComboBoxIndex < (totalCountSource - 1))
            {
                SourceComboBoxIndex++;
                await LoadDataSource(SourceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchSourceIndexDecrement()
        {
            if (SourceComboBoxIndex > 0)
            {
                SourceComboBoxIndex--;
                await LoadDataSource(SourceComboBoxIndex, 10);
            }
        }

        private async Task OnInputSourceChanged(string e)
        {
            SourceComboBoxIndex = 0;
            await LoadDataSource(0, 10);
        }

        private async Task LoadDataSource(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLocationQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchTerm = refSourceComboBox?.Text ?? "",
                Select = x => new Locations
                {
                    Id = x.Id,
                    Name = x.Name,
                }
            });
            getSource = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion Combo Box DrugForm

        //#region Combo Box DrugRoute

        //private DxComboBox<DrugRouteDto, long?> refDrugRouteComboBox { get; set; }
        //private int DrugRouteComboBoxIndex { get; set; } = 0;
        //private int totalCountDrugRoute = 0;

        //private async Task OnSearchDrugRoute()
        //{
        //    await LoadDataDrugRoute(0, 10);
        //}

        //private async Task OnSearchDrugRouteIndexIncrement()
        //{
        //    if (DrugRouteComboBoxIndex < (totalCountDrugRoute - 1))
        //    {
        //        DrugRouteComboBoxIndex++;
        //        await LoadDataDrugRoute(DrugRouteComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnSearchDrugRouteIndexDecrement()
        //{
        //    if (DrugRouteComboBoxIndex > 0)
        //    {
        //        DrugRouteComboBoxIndex--;
        //        await LoadDataDrugRoute(DrugRouteComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnInputDrugRouteChanged(string e)
        //{
        //    DrugRouteComboBoxIndex = 0;
        //    await LoadDataDrugRoute(0, 10);
        //}

        //private async Task LoadDataDrugRoute(int pageIndex = 0, int pageSize = 10)
        //{
        //    PanelVisible = true;
        //    SelectedDataItems = [];
        //    var result = await Mediator.Send(new GetDrugRouteQuery(searchTerm: refDrugRouteComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
        //    GetDrugRoutes = result.Item1;
        //    totalCount = result.pageCount;
        //    PanelVisible = false;
        //}

        //#endregion Combo Box DrugRoute

        //#region Combo Box Product Category

        //private DxComboBox<ProductCategoryDto, long?> refProductCategoryComboBox { get; set; }
        //private int ProductCategoryComboBoxIndex { get; set; } = 0;
        //private int totalCountProductCategory = 0;

        //private async Task OnSearchProductCategory()
        //{
        //    await LoadDataProductCategory(0, 10);
        //}

        //private async Task OnSearchProductCategoryIndexIncrement()
        //{
        //    if (ProductCategoryComboBoxIndex < (totalCountProductCategory - 1))
        //    {
        //        ProductCategoryComboBoxIndex++;
        //        await LoadDataProductCategory(ProductCategoryComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnSearchProductCategoryIndexDecrement()
        //{
        //    if (ProductCategoryComboBoxIndex > 0)
        //    {
        //        ProductCategoryComboBoxIndex--;
        //        await LoadDataProductCategory(ProductCategoryComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnInputProductCategoryChanged(string e)
        //{
        //    ProductCategoryComboBoxIndex = 0;
        //    await LoadDataProductCategory(0, 10);
        //}

        //private async Task LoadDataProductCategory(int pageIndex = 0, int pageSize = 10)
        //{
        //    PanelVisible = true;
        //    SelectedDataItems = [];
        //    var result = await Mediator.Send(new GetProductCategoryQuery(searchTerm: refProductCategoryComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
        //    GetProductCategories = result.Item1;
        //    totalCount = result.pageCount;
        //    PanelVisible = false;
        //}

        //#endregion Combo Box Product Category

        //#region ComboBox Drug Dosage

        //private DxComboBox<DrugDosageDto, long?> refDrugDosageComboBox { get; set; }
        //private int DrugDosageComboBoxIndex { get; set; } = 0;
        //private int totalCountDrugDosage = 0;

        //private async Task OnSearchDrugDosage()
        //{
        //    await LoadDataDrugDosage(0, 10);
        //}

        //private async Task OnSearchDrugDosageIndexIncrement()
        //{
        //    if (UomComboBoxIndex < (totalCountUom - 1))
        //    {
        //        UomComboBoxIndex++;
        //        await LoadDataDrugDosage(DrugDosageComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnSearchDrugDosageIndexDecrement()
        //{
        //    if (DrugDosageComboBoxIndex > 0)
        //    {
        //        UomComboBoxIndex--;
        //        await LoadDataDrugDosage(DrugDosageComboBoxIndex, 10);
        //    }
        //}

        //private async Task OnInputDrugDosageChanged(string e)
        //{
        //    DrugDosageComboBoxIndex = 0;
        //    await LoadDataDrugDosage(0, 10);
        //}

        //private async Task LoadDataDrugDosage(int pageIndex = 0, int pageSize = 10)
        //{
        //    PanelVisible = true;
        //    SelectedDataItems = [];
        //    var result = await Mediator.Send(new GetDrugDosageQuery(searchTerm: refUomComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
        //    GetDrugDosage = result.Item1;
        //    totalCount = result.pageCount;
        //    PanelVisible = false;
        //}

        //#endregion ComboBox Uom


        #endregion Load ComboBox

        #region HandleSubmit
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

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }
        #endregion

        #region Save
        private async Task OnSave()
        {
            try
            {
                var goodsReceipt = new GoodsReceiptDto();
                var result = await Mediator.Send(new GetGoodsReceiptQuery
                {
                    SearchTerm = searchTerm,
                    PageSize = pageSize,
                    PageIndex = 0,
                });
                getGoodsReceipts = result.Item1;

                if (postGoodsReceipt.Id == 0)
                {
                    var getReceiving = getGoodsReceipts.Where(r => r.DestinationId == postGoodsReceipt.DestinationId).OrderByDescending(x => x.ReceiptNumber).Select(x => x.ReceiptNumber).FirstOrDefault();
                    if (getReceiving == null)
                    {
                        var nextTransferNumber = 1;
                        postGoodsReceipt.ReceiptNumber = $"GR/{nextTransferNumber.ToString("0000")}";
                    }
                    else
                    {
                        var lastTransferNumber = 0;
                        if (getReceiving.Contains("GN/"))
                        {
                            var lastTransferNumberStr = getReceiving.Split('/')[1];
                            int.TryParse(lastTransferNumberStr, out lastTransferNumber);
                        }
                        var nextTransferNumber = lastTransferNumber + 1;
                        postGoodsReceipt.ReceiptNumber = $"GN/{nextTransferNumber.ToString("0000")}";
                    }

                    postGoodsReceipt.Status = EnumStatusGoodsReceipt.Draft;

                    goodsReceipt = await Mediator.Send(new CreateGoodsReceiptRequest(postGoodsReceipt));


                    ToastService.ShowSuccess("Add Data Success...");

                    postGoodsReceiptLogs.SourceId = goodsReceipt.DestinationId;
                    postGoodsReceiptLogs.UserById = UserLogin.Id;
                    postGoodsReceiptLogs.GoodsReceiptId = goodsReceipt.Id;
                    postGoodsReceiptLogs.Status = EnumStatusGoodsReceipt.Draft;

                    await Mediator.Send(new CreateGoodsReceiptLogRequest(postGoodsReceiptLogs));
                }
                else
                {
                    goodsReceipt = await Mediator.Send(new UpdateGoodsReceiptRequest(postGoodsReceipt));
                    ToastService.ShowSuccess("Update Data Success...");
                }

                ToastService.ClearSuccessToasts();
                NavigationManager.NavigateTo($"inventory/goods-receipts/{EnumPageMode.Update.GetDisplayName()}?Id={goodsReceipt.Id}", true);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSaveProduct(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (postGoodsReceiptDetail.Id == 0)
                {
                    postGoodsReceiptDetail.GoodsReceiptId = postGoodsReceipt.Id;

                    await Mediator.Send(new CreateGoodsReceiptDetailRequest(postGoodsReceiptDetail));
                }
                else
                {
                    await Mediator.Send(new UpdateGoodsReceiptDetailRequest(postGoodsReceiptDetail));
                }
                await LoadDetailData();

            }
            catch
            {

            }
        }

        #endregion

        #region Delete
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {

            try
            {
                var data = (GoodsReceiptDetailDto)e.DataItem;
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGoodsReceiptDetailRequest(((GoodsReceiptDetailDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GoodsReceiptDetailDto>>();
                    await Mediator.Send(new DeleteGoodsReceiptDetailRequest(ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadDetailData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }


        #endregion

        #region Process

        private async Task onProcess()
        {
            PanelVisible = true;
            var results = await Mediator.Send(new GetGoodsReceiptQuery
            {
                Predicate= x=>x.Id == postGoodsReceipt.Id,
            });

            getGoodsReceipts = results.Item1;
            if (getGoodsReceipts is not null)
            {
                //ReferenceKode
                var cekReference = getTransactionStocks.Where(x => x.SourceTable == nameof(GoodsReceipt)).OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                int NextReferenceNumber = 1;
                if (cekReference != null)
                {
                    int.TryParse(cekReference?.Substring("GN#".Length), out NextReferenceNumber);
                    NextReferenceNumber++;
                }

                string referenceNumber = $"GN#{NextReferenceNumber:D3}";

                var CheckReceivedProduct = getGoodsReceiptDetails.Where(x => x.GoodsReceiptId == postGoodsReceipt.Id).ToList()!;
                foreach (var a in CheckReceivedProduct)
                {
                    var Cek_Uom = getUoms.Where(x => x.Id == a.Product?.UomId).FirstOrDefault();

                    var x = getUoms.Where(x => x.Id == a?.Product?.UomId).FirstOrDefault();

                    postTransaction.SourceTable = nameof(GoodsReceipt);
                    postTransaction.SourcTableId = postGoodsReceipt.Id;
                    postTransaction.ProductId = a.ProductId;
                    postTransaction.Batch = a.Batch;
                    postTransaction.ExpiredDate = a.ExpiredDate;
                    postTransaction.Reference = referenceNumber;
                    postTransaction.Quantity = a.Qty * Cek_Uom?.BiggerRatio?.ToLong() ?? 0;
                    postTransaction.LocationId = postGoodsReceipt.DestinationId;
                    postTransaction.UomId = a.Product?.UomId;
                    postTransaction.Validate = false;

                    await Mediator.Send(new CreateTransactionStockRequest(postTransaction));
                }

                //UpdateReceiving Stock
                postGoodsReceipt.Status = EnumStatusGoodsReceipt.Process;
                var goodsReceipt = await Mediator.Send(new UpdateGoodsReceiptRequest(postGoodsReceipt));

                //Save Log..
                postGoodsReceiptLogs.SourceId = goodsReceipt.DestinationId;
                postGoodsReceiptLogs.UserById = UserLogin.Id;
                postGoodsReceiptLogs.GoodsReceiptId = goodsReceipt.Id;
                postGoodsReceiptLogs.Status = EnumStatusGoodsReceipt.Process;

                await Mediator.Send(new CreateGoodsReceiptLogRequest(postGoodsReceiptLogs));
                await LoadData();
                PanelVisible = false;


            }
        }

        #endregion Process

        #region Validation

        private async Task onValidation()
        {
            PanelVisible = true;
            var results = await Mediator.Send(new GetGoodsReceiptQuery
            {
                Predicate = x => x.Id == postGoodsReceipt.Id,
            });

            getGoodsReceipts = results.Item1;

            var data_TransactionStock = getTransactionStocks.Where(x => x.SourceTable == nameof(GoodsReceipt) && x.SourcTableId == postGoodsReceipt.Id).ToList();

            foreach (var item in data_TransactionStock)
            {
                item.Validate = true;
                var aa = await Mediator.Send(new UpdateTransactionStockRequest(item));
            }

            //UpdateReceiving Stock
            postGoodsReceipt.Status = EnumStatusGoodsReceipt.Done;
            var goodReceipts = await Mediator.Send(new UpdateGoodsReceiptRequest(postGoodsReceipt));

            //Save Log..

            postGoodsReceiptLogs.SourceId = goodReceipts.DestinationId;
            postGoodsReceiptLogs.UserById = UserLogin.Id;
            postGoodsReceiptLogs.GoodsReceiptId = goodReceipts.Id;
            postGoodsReceiptLogs.Status = EnumStatusGoodsReceipt.Done;

            await Mediator.Send(new CreateGoodsReceiptLogRequest(postGoodsReceiptLogs));

            await LoadData();

            PanelVisible = false;
        }

        #endregion Validation

        #region Cancel
        private async Task onDiscard()
        {
            NavigationManager.NavigateTo($"inventory/goods-receipts");
        }
        private async Task onCancel()
        {

            var tempGoodsReceipts = getGoodsReceipts.Where(x => x.Id == postGoodsReceipt.Id).FirstOrDefault()!;

            if (tempGoodsReceipts is null)
            {
                return;
            }

            tempGoodsReceipts.Status = EnumStatusGoodsReceipt.Cancel;
            var goodReceipts = await Mediator.Send(new UpdateGoodsReceiptRequest(tempGoodsReceipts));

            //Update Receiving Stock

            //Save Log
            postGoodsReceiptLogs.SourceId = goodReceipts.DestinationId;
            postGoodsReceiptLogs.UserById = UserLogin.Id;
            postGoodsReceiptLogs.GoodsReceiptId = goodReceipts.Id;
            postGoodsReceiptLogs.Status = goodReceipts.Status;

            await Mediator.Send(new CreateGoodsReceiptLogRequest(postGoodsReceiptLogs));
        }

        #endregion

        #region Click Button
        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click(IGrid context)
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            postGoodsReceiptDetail = (GoodsReceiptDetailDto)context.SelectedDataItem;

        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task RefreshItem_Click()
        {
            await LoadData();
        }
        #endregion


    }
}

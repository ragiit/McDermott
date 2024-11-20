using McDermott.Application.Dtos.Pharmacies;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PharmacyCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PrescriptionCommand;

namespace McDermott.Web.Components.Pages.Pharmacies.Prescription
{
    public partial class PrescriptionPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

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

        #region relation Data

        private PharmacyDto postPharmacy { get; set; } = new();
        private List<PharmacyDto> getPharmacy { get; set; } = [];

        #endregion relation Data

        #region Static Variables

        [Parameter]
        public long Id { get; set; }

        private IGrid Grid
        {
            get; set;
        }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        [Parameter]
        public bool IsPopUpForm { get; set; } = true;
        private bool PanelVisible { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private string header { get; set; } = string.Empty;

        public MarkupString GetIssueStatusIconHtml(EnumStatusPharmacy? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusPharmacy.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumStatusPharmacy.SendToPharmacy:
                    priorityClass = "primary";
                    title = "Pharmacy";
                    break;

                case EnumStatusPharmacy.Received:
                    priorityClass = "primary";
                    title = "Received";
                    break;

                case EnumStatusPharmacy.Processed:
                    priorityClass = "warning";
                    title = "Processed";
                    break;

                case EnumStatusPharmacy.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusPharmacy.Cancel:
                    priorityClass = "danger";
                    title = "Canceled";
                    break;

                default:
                    return new MarkupString("");
            }
            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #endregion Static Variables

        #region Load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        //#region Cut Stock

        //private bool isDetailPrescription { get; set; } = false;
        //private bool isDetailLines { get; set; } = false;

        ////private long? StockIds { get; set; }
        //private bool traceAvailability { get; set; } = false;

        //private long PrescripId { get; set; } = 0;
        //private long Lines_Id { get; set; } = 0;

        //private async Task cancelCutStock()
        //{
        //    isDetailPrescription = false;
        //    SelectedDataItemsStockOut = [];
        //    await EditItemPharmacy_Click(Pharmacy);
        //    StateHasChanged();
        //}

        //private async Task cancelCutStockLines()
        //{
        //    isDetailLines = false;
        //    SelectedDataItemsStockOut = [];
        //    await EditItemPharmacy_Click(Pharmacy);
        //    StateHasChanged();
        //}

        //private async Task ShowCutStockLines(long LinesId)
        //{
        //    await RefreshData();
        //    PanelVisible = true;
        //    Lines_Id = LinesId;
        //    //Get the ConcoctionLines by ID
        //    var concoctionLines = ConcoctionLines.FirstOrDefault(x => x.Id == LinesId);
        //    if (concoctionLines == null)
        //    {
        //        // Handle case where the ConcoctionLine is not found
        //        return;
        //    }

        //    //Get the Product associated with the ConcoctionLine
        //    var product = Products.FirstOrDefault(x => x.Id == concoctionLines.ProductId);
        //    if (product == null)
        //    {
        //        // Handle case where the product is not found
        //        return;
        //    }
        //    // Update state variables
        //    traceAvailability = product.TraceAbility;

        //    isDetailLines = true;

        //    // Filter stock products based on specific conditions

        //    StockOutProducts = TransactionStocks
        //            .Where(s => s.ProductId == product.Id && s.LocationId == Pharmacy.LocationId && s.Quantity > 0)
        //            .OrderBy(x => x.ExpiredDate)
        //            .GroupBy(s => s.Batch)
        //            .Select(g => new TransactionStockDto
        //            {
        //                ProductId = product.Id,
        //                Batch = g.Key,
        //                ExpiredDate = g.First().ExpiredDate,
        //                Quantity = g.Sum(x => x.Quantity),
        //                LocationId = Pharmacy.LocationId
        //            }).ToList();

        //    // Fetch stock out prescription data
        //    var listDataStockCut = await Mediator.Send(new GetStockOutLineQuery());

        //    if (product.TraceAbility)
        //    {
        //        var dataStock = listDataStockCut.Where(x => x.LinesId == LinesId).ToList();
        //        if (dataStock == null || dataStock.Count == 0)
        //        {
        //            long currentStockInput = 0;
        //            FormStockOutLines.CutStock = 0;
        //            var listStockOutLines = new List<StockOutLinesDto>();

        //            foreach (var item in StockOutProducts)
        //            {
        //                if (currentStockInput >= concoctionLines.TotalQty) break;

        //                var newStockOutLines = new StockOutLinesDto
        //                {
        //                    Id = Helper.RandomNumber,
        //                    LinesId = concoctionLines.Id,
        //                    TransactionStockId = item.Id,
        //                    Batch = item.Batch,
        //                    ExpiredDate = item.ExpiredDate,
        //                    CurrentStock = item.Quantity
        //                };

        //                if (currentStockInput == 0)
        //                {
        //                    newStockOutLines.CutStock = item.Quantity > concoctionLines.TotalQty ? concoctionLines.TotalQty : item.Quantity;
        //                }
        //                else
        //                {
        //                    long remainingNeeded = concoctionLines.TotalQty - currentStockInput;
        //                    newStockOutLines.CutStock = item.Quantity >= remainingNeeded ? remainingNeeded : item.Quantity;
        //                }

        //                currentStockInput += newStockOutLines.CutStock;
        //                listStockOutLines.Add(newStockOutLines);
        //            }

        //            StockOutLines = listStockOutLines;
        //        }
        //        else
        //        {
        //            // Updating batch and expired values from StockOutProducts
        //            foreach (var stock in dataStock)
        //            {
        //                var qtys = StockOutProducts.Sum(x => x.Quantity);
        //                var stockProduct = TransactionStocks.FirstOrDefault(x => x.Id == stock.TransactionStockId);
        //                if (stockProduct != null)
        //                {
        //                    stock.TransactionStockId = stockProduct.Id;
        //                    stock.Batch = stockProduct.Batch;
        //                    stock.ExpiredDate = stockProduct.ExpiredDate;
        //                    stock.CurrentStock = qtys;
        //                }
        //            }
        //            StockOutLines = dataStock;
        //        }
        //    }
        //    else
        //    {
        //        var dataStock = listDataStockCut.Where(x => x.LinesId == LinesId).ToList();

        //        if (dataStock == null || dataStock.Count == 0)
        //        {
        //            long currentStockInput = 0;
        //            FormStockOutLines.CutStock = 0;
        //            var listStockOutLines = new List<StockOutLinesDto>();

        //            foreach (var item in StockOutProducts)
        //            {
        //                if (currentStockInput >= concoctionLines.TotalQty) break;

        //                var newStockOutLines = new StockOutLinesDto
        //                {
        //                    Id = Helper.RandomNumber,
        //                    LinesId = concoctionLines.Id,
        //                    TransactionStockId = item.Id,
        //                    Batch = item.Batch,
        //                    ExpiredDate = item.ExpiredDate,
        //                    CurrentStock = item.Quantity
        //                };

        //                if (currentStockInput == 0)
        //                {
        //                    newStockOutLines.CutStock = item.Quantity > concoctionLines.TotalQty ? concoctionLines.TotalQty : item.Quantity;
        //                }
        //                else
        //                {
        //                    long remainingNeeded = concoctionLines.TotalQty - currentStockInput;
        //                    newStockOutLines.CutStock = item.Quantity >= remainingNeeded ? remainingNeeded : item.Quantity;
        //                }

        //                currentStockInput += newStockOutLines.CutStock;
        //                listStockOutLines.Add(newStockOutLines);
        //            }

        //            StockOutLines = listStockOutLines;
        //        }
        //        else
        //        {
        //            // Updating batch and expired values from StockOutProducts
        //            foreach (var stock in dataStock)
        //            {
        //                var qtys = StockOutProducts.Sum(x => x.Quantity);
        //                var stockProduct = StockOutProducts.FirstOrDefault(x => x.Id == stock.TransactionStockId);
        //                if (stockProduct != null)
        //                {
        //                    stock.CurrentStock = qtys;
        //                }
        //            }
        //            StockOutLines = dataStock;
        //        }
        //    }
        //    PanelVisible = false;
        //}

        //private async Task ShowCutStock(long prescriptionId)
        //{
        //    await RefreshData();

        //    PanelVisible = true;
        //    PrescripId = prescriptionId;
        //    // Get the prescription by ID
        //    var prescription = Prescriptions.FirstOrDefault(x => x.Id == prescriptionId && x.PharmacyId == Pharmacy.Id);
        //    if (prescription == null)
        //    {
        //        // Handle case where the prescription is not found
        //        return;
        //    }

        //    // Get the product associated with the prescription
        //    var product = Products.FirstOrDefault(x => x.Id == prescription.ProductId);
        //    if (product == null)
        //    {
        //        // Handle case where the product is not found
        //        return;
        //    }

        //    // Update state variables
        //    traceAvailability = product.TraceAbility;
        //    isDetailPrescription = true;

        //    // Filter stock products based on specific conditions
        //    StockOutProducts = TransactionStocks
        //            .Where(s => s.ProductId == product.Id && s.LocationId == Pharmacy.LocationId && s.Quantity > 0)
        //            .OrderBy(x => x.ExpiredDate)
        //            .GroupBy(s => s.Batch)
        //            .Select(g => new TransactionStockDto
        //            {
        //                ProductId = product.Id,
        //                Batch = g.Key,
        //                ExpiredDate = g.First().ExpiredDate,
        //                Quantity = g.Sum(x => x.Quantity),
        //                LocationId = Pharmacy.LocationId
        //            }).ToList();

        //    //StockOutProducts = TransactionStocks
        //    //    .Where(x => x.ProductId == prescription.ProductId && x.LocationId == Pharmacy.LocationId && x.Quantity != 0)
        //    //    .OrderBy(x => x.ExpiredDate)
        //    //    .ToList();

        //    // Fetch stock out prescription data
        //    var listDataStockCut = await Mediator.Send(new GetStockOutPrescriptionQuery());

        //    // Process stock data if the product has traceability
        //    if (product.TraceAbility)
        //    {
        //        var dataStock = listDataStockCut.Where(x => x.PrescriptionId == prescriptionId).ToList();
        //        if (dataStock == null || dataStock.Count == 0)
        //        {
        //            long currentStockInput = 0;
        //            FormStockOutPrescriptions.CutStock = 0;
        //            var listStockOutPrescription = new List<StockOutPrescriptionDto>();

        //            foreach (var item in StockOutProducts)
        //            {
        //                if (currentStockInput >= prescription.GivenAmount) break;

        //                var newStockOutPrescription = new StockOutPrescriptionDto
        //                {
        //                    Id = Helper.RandomNumber,
        //                    PrescriptionId = prescription.Id,
        //                    TransactionStockId = item.Id,
        //                    Batch = item.Batch,
        //                    ExpiredDate = item.ExpiredDate,
        //                    CurrentStock = item.Quantity
        //                };

        //                if (currentStockInput == 0)
        //                {
        //                    newStockOutPrescription.CutStock = item.Quantity > prescription.GivenAmount ? prescription.GivenAmount : item.Quantity;
        //                }
        //                else
        //                {
        //                    long remainingNeeded = prescription.GivenAmount - currentStockInput;
        //                    newStockOutPrescription.CutStock = item.Quantity >= remainingNeeded ? remainingNeeded : item.Quantity;
        //                }

        //                currentStockInput += newStockOutPrescription.CutStock;
        //                listStockOutPrescription.Add(newStockOutPrescription);
        //            }

        //            StockOutPrescriptions = listStockOutPrescription;
        //        }
        //        else
        //        {
        //            // Updating batch and expired values from StockOutProducts
        //            foreach (var stock in dataStock)
        //            {
        //                var qtys = StockOutProducts.Sum(x => x.Quantity);
        //                var stockProduct = TransactionStocks.FirstOrDefault(x => x.Id == stock.TransactionStockId);
        //                if (stockProduct != null)
        //                {
        //                    stock.TransactionStockId = stockProduct.Id;
        //                    stock.Batch = stockProduct.Batch;
        //                    stock.ExpiredDate = stockProduct.ExpiredDate;
        //                    stock.CurrentStock = qtys;
        //                }
        //            }
        //            StockOutPrescriptions = dataStock;
        //        }
        //    }
        //    else
        //    {
        //        var dataStock = listDataStockCut.Where(x => x.PrescriptionId == prescriptionId).ToList();

        //        if (dataStock == null || dataStock.Count == 0)
        //        {
        //            long currentStockInput = 0;
        //            FormStockOutPrescriptions.CutStock = 0;
        //            var listStockOutPrescription = new List<StockOutPrescriptionDto>();

        //            foreach (var item in StockOutProducts)
        //            {
        //                if (currentStockInput >= prescription.GivenAmount) break;
        //                if (item.Quantity <= 0) continue;

        //                var newStockOutPrescription = new StockOutPrescriptionDto
        //                {
        //                    Id = Helper.RandomNumber,
        //                    TransactionStockId = item.Id,
        //                    PrescriptionId = prescription.Id,
        //                    CurrentStock = item.Quantity
        //                };

        //                if (currentStockInput == 0)
        //                {
        //                    newStockOutPrescription.CutStock = item.Quantity > prescription.GivenAmount ? prescription.GivenAmount : item.Quantity;
        //                }
        //                else
        //                {
        //                    long remainingNeeded = prescription.GivenAmount - currentStockInput;
        //                    newStockOutPrescription.CutStock = item.Quantity >= remainingNeeded ? remainingNeeded : item.Quantity;
        //                }

        //                currentStockInput += newStockOutPrescription.CutStock;
        //                listStockOutPrescription.Add(newStockOutPrescription);
        //            }

        //            StockOutPrescriptions = listStockOutPrescription;
        //        }
        //        else
        //        {
        //            // Updating batch and expired values from StockOutProducts
        //            foreach (var stock in dataStock)
        //            {
        //                var qtys = StockOutProducts.Sum(x => x.Quantity);
        //                var stockProduct = TransactionStocks.FirstOrDefault(x => x.Id == stock.TransactionStockId);
        //                if (stockProduct != null)
        //                {
        //                    stock.CurrentStock = qtys;
        //                }
        //            }
        //            StockOutPrescriptions = dataStock;
        //        }
        //    }
        //    PanelVisible = false;
        //}

        //private async Task RefreshData()
        //{
        //    // Fetch the latest data from the server or database
        //    Prescriptions = await Mediator.Send(new GetPrescriptionQuery(x => x.PharmacyId == Pharmacy.Id));
        //    Concoctions = await Mediator.Send(new GetConcoctionQuery());
        //    var concoctionId = Concoctions.Where(x => x.PharmacyId == Pharmacy.Id).FirstOrDefault();
        //    //Products = await Mediator.Send(new GetProductQuery());
        //    TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
        //    if (concoctionId is not null)
        //    {
        //        ConcoctionLines = await Mediator.Send(new GetConcoctionLineQuery(x => x.ConcoctionId == concoctionId.Id));
        //    }
        //}

        //private void HandleDiscard()
        //{
        //    isDetailPrescription = false;
        //}

        //private void HandleDiscardLines()
        //{
        //    isDetailLines = false;
        //}

        //private async Task SaveStockOut()
        //{
        //    try
        //    {
        //        foreach (var item in StockOutPrescriptions)
        //        {
        //            if (item.TransactionStockId == 0 || item.TransactionStockId is null)
        //            {
        //                var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(Prescription))
        //                    .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
        //                int NextReferenceNumber = 1;
        //                if (cekReference != null)
        //                {
        //                    int.TryParse(cekReference?.Substring("PHP#".Length), out NextReferenceNumber);
        //                    NextReferenceNumber++;
        //                }
        //                string referenceNumber = $"PHP#{NextReferenceNumber:D3}";

        //                var prescription = Prescriptions.Where(x => x.Id == item.PrescriptionId).FirstOrDefault();
        //                var productd = Products.Where(x => x.Id == prescription.ProductId).FirstOrDefault();

        //                FormTransactionStock.ProductId = productd?.Id;
        //                FormTransactionStock.SourceTable = nameof(Prescription);
        //                FormTransactionStock.SourcTableId = prescription?.Id;
        //                FormTransactionStock.LocationId = prescription?.Pharmacy?.LocationId;
        //                FormTransactionStock.ExpiredDate = item?.ExpiredDate;
        //                FormTransactionStock.Batch = item?.Batch;
        //                FormTransactionStock.Quantity = -(item?.CutStock) ?? 0;
        //                FormTransactionStock.Reference = referenceNumber;
        //                FormTransactionStock.UomId = productd?.UomId;
        //                FormTransactionStock.Validate = false;
        //                var datas = await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));

        //                item.TransactionStockId = datas.Id;
        //            }

        //            // Create a DTO for the current item
        //            var item_cutstock = new StockOutPrescriptionDto
        //            {
        //                CutStock = item.CutStock,
        //                TransactionStockId = item.TransactionStockId,
        //                PrescriptionId = item.PrescriptionId
        //            };

        //            // Check if the item exists in the database
        //            var existingStockOut = await Mediator.Send(new GetStockOutPrescriptionQuery(x => x.TransactionStockId == item.TransactionStockId && x.PrescriptionId == item.PrescriptionId));

        //            if (!existingStockOut.Any()) // If the item does not exist
        //            {
        //                await Mediator.Send(new CreateStockOutPrescriptionRequest(item_cutstock));
        //            }
        //            else // If the item exists, update it
        //            {
        //                // Assuming you have a method to update existing data
        //                var existingItem = existingStockOut.First();
        //                existingItem.CutStock = item.CutStock;
        //                await Mediator.Send(new UpdateStockOutPrescriptionRequest(existingItem));
        //            }
        //        }
        //        isDetailPrescription = false;
        //        await EditItemPharmacy_Click(Pharmacy);
        //        StateHasChanged();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.HandleException(ToastService);
        //    }
        //}

        //private async Task SaveStockOutLines()
        //{
        //    try
        //    {
        //        foreach (var item in StockOutLines)
        //        {
        //            var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(ConcoctionLine))
        //                .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
        //            int NextReferenceNumber = 1;
        //            if (cekReference != null)
        //            {
        //                int.TryParse(cekReference?.Substring("PHCL#".Length), out NextReferenceNumber);
        //                NextReferenceNumber++;
        //            }
        //            string referenceNumber = $"PHCL#{NextReferenceNumber:D3}";

        //            var check_CutStock = ConcoctionLines.Where(x => x.Id == item.LinesId).FirstOrDefault();
        //            var concoction_data = Concoctions.Where(x => x.Id == check_CutStock?.ConcoctionId).FirstOrDefault();

        //            FormTransactionStock.ProductId = check_CutStock?.Product?.Id;
        //            FormTransactionStock.SourceTable = nameof(ConcoctionLine);
        //            FormTransactionStock.SourcTableId = check_CutStock?.Id;
        //            FormTransactionStock.LocationId = concoction_data?.Pharmacy?.LocationId;
        //            FormTransactionStock.ExpiredDate = item?.ExpiredDate;
        //            FormTransactionStock.Batch = item?.Batch;
        //            FormTransactionStock.Quantity = -(item?.CutStock) ?? 0;
        //            FormTransactionStock.Reference = referenceNumber;
        //            FormTransactionStock.UomId = check_CutStock?.Product?.UomId;
        //            FormTransactionStock.Validate = false;
        //            var datas = await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));

        //            item.TransactionStockId = datas.Id;

        //            var item_lines = new StockOutLinesDto
        //            {
        //                CutStock = item.CutStock,
        //                TransactionStockId = item.TransactionStockId,
        //                LinesId = item.LinesId
        //            };

        //            var existingStock = await Mediator.Send(new GetStockOutLineQuery(x => x.TransactionStockId == item.TransactionStockId && x.LinesId == item.LinesId));

        //            if (!existingStock.Any())
        //            {
        //                await Mediator.Send(new CreateStockOutLinesRequest(item_lines));
        //            }
        //            else
        //            {
        //                var existingItem = existingStock.First();
        //                existingItem.CutStock = item.CutStock;
        //                await Mediator.Send(new UpdateStockOutLinesRequest(existingItem));
        //            }
        //        }
        //        isDetailLines = false;
        //        await EditItemPharmacy_Click(Pharmacy);
        //        StateHasChanged();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.HandleException(ToastService);
        //    }
        //}

        //private DateTime? SelectedBatchExpired { get; set; }

        //private async Task ChangeOutStock(string value)
        //{
        //    SelectedBatchExpired = null;

        //    if (value is not null)
        //    {
        //        var presc = (await Mediator.Send(new GetPrescriptionQuery(x => x.Id == PrescripId))).FirstOrDefault()!;
        //        FormStockOutPrescriptions.Batch = value;
        //        //current Stock
        //        var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
        //            s.ProductId == presc.ProductId &&
        //            s.LocationId == Pharmacy.LocationId &&
        //            s.Validate == true
        //        ));
        //        // Find the first matching product
        //        var matchedProduct = stockProducts.FirstOrDefault(x =>
        //            x.LocationId == Pharmacy.LocationId &&
        //            x.ProductId == presc?.ProductId &&
        //            x.Batch == FormStockOutPrescriptions.Batch
        //        );

        //        FormStockOutPrescriptions.ExpiredDate = matchedProduct?.ExpiredDate ?? new();

        //        var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == presc.ProductId
        //        && x.LocationId == Pharmacy.LocationId && x.Batch == FormStockOutPrescriptions.Batch));

        //        FormStockOutPrescriptions.CurrentStock = aa.Sum(x => x.Quantity);
        //    }
        //}

        //private async Task ChangeOutStockLines(string value)
        //{
        //    SelectedBatchExpired = null;

        //    if (value is not null)
        //    {
        //        var line = (await Mediator.Send(new GetConcoctionLineQuery(x => x.Id == Lines_Id))).FirstOrDefault()!;
        //        FormStockOutLines.Batch = value;

        //        var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
        //            s.ProductId == line.ProductId &&
        //            s.LocationId == Pharmacy.LocationId &&
        //            s.Validate == true
        //        ));
        //        // Find the first matching product
        //        var matchedProduct = stockProducts.FirstOrDefault(x =>
        //            x.LocationId == Pharmacy.LocationId &&
        //            x.ProductId == line.ProductId &&
        //            x.Batch == FormStockOutLines.Batch
        //        );

        //        FormStockOutLines.ExpiredDate = matchedProduct?.ExpiredDate ?? new();

        //        var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == line.ProductId
        //        && x.LocationId == Pharmacy.LocationId && x.Batch == FormStockOutLines.Batch));

        //        FormStockOutLines.CurrentStock = aa.Sum(x => x.Quantity);
        //    }
        //}

        //#endregion Cut Stock

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



        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetPharmacyQuery());
                getPharmacy = result.Item1;

                PanelVisible = false;
            }
            catch (Exception ex) { ex.HandleException(ToastService); }
        }



        #endregion Load Data

        #region Delete Function

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var data_delete = (PharmacyDto)e.DataItem;
                var Prescriptions = await Mediator.Send(new GetAllPrescriptionQuery());
                var Concoctions = await Mediator.Send(new GetAllConcoctionQuery());
                var ConcoctionLines = await Mediator.Send(new GetAllConcoctionLineQuery());
                var StockOutPrescriptions = await Mediator.Send(new GetAllStockOutPrescriptionQuery());
                var Logs = await Mediator.Send(new GetAllPharmacyLogQuery());
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    var prescription_data = Prescriptions.Where(x => x.PharmacyId == data_delete.Id).ToList();
                    if (prescription_data.Count > 0)
                    {
                        foreach (var item_delete in prescription_data)
                        {
                            var StockOutPrescriptions_data = StockOutPrescriptions.Where(x => x.PrescriptionId == item_delete.Id).ToList();
                            if (StockOutPrescriptions_data.Count > 0)
                            {
                                foreach (var items_delete in StockOutPrescriptions_data)
                                {
                                    await Mediator.Send(new DeleteConcoctionLineRequest(items_delete.Id));
                                }
                            }
                            await Mediator.Send(new DeletePrescriptionRequest(item_delete.Id));
                        }
                    }

                    var concoction_data = Concoctions.Where(x => x.PharmacyId == data_delete.Id).ToList();
                    if (concoction_data.Count > 0)
                    {
                        foreach (var item_delete in concoction_data)
                        {
                            var concoctionLine_data = ConcoctionLines.Where(x => x.ConcoctionId == item_delete.Id).ToList();
                            if (concoctionLine_data.Count > 0)
                            {
                                foreach (var items_delete in concoctionLine_data)
                                {
                                    await Mediator.Send(new DeleteConcoctionLineRequest(items_delete.Id));
                                }
                            }
                            await Mediator.Send(new DeleteConcoctionRequest(item_delete.Id));
                        }
                    }

                    var data_log = Logs.Where(x => x.PharmacyId == data_delete.Id).ToList();
                    foreach (var item in data_log)
                    {
                        await Mediator.Send(new DeletePharmacyLogRequest(item.Id));
                    }

                    await Mediator.Send(new DeletePharmacyRequest(((PharmacyDto)e.DataItem).Id));
                }
                else
                {
                    var datas = SelectedDataItems.Adapt<List<PharmacyDto>>().Select(x => x.Id).ToList();
                    foreach (var item in datas)
                    {
                        var prescription_data = Prescriptions.Where(x => x.PharmacyId == item).ToList();
                        if (prescription_data.Count > 0)
                        {
                            foreach (var item_delete in prescription_data)
                            {
                                var StockOutPrescriptions_data = StockOutPrescriptions.Where(x => x.PrescriptionId == item_delete.Id).ToList();
                                if (StockOutPrescriptions_data.Count > 0)
                                {
                                    foreach (var items_delete in StockOutPrescriptions_data)
                                    {
                                        await Mediator.Send(new DeleteConcoctionLineRequest(items_delete.Id));
                                    }
                                }
                                await Mediator.Send(new DeletePrescriptionRequest(item_delete.Id));
                            }
                        }

                        var concoction_data = Concoctions.Where(x => x.PharmacyId == item).ToList();
                        if (concoction_data.Count > 0)
                        {
                            foreach (var item_delete in concoction_data)
                            {
                                var concoctionLine_data = ConcoctionLines.Where(x => x.ConcoctionId == item_delete.Id).ToList();
                                if (concoctionLine_data.Count > 0)
                                {
                                    foreach (var items_delete in concoctionLine_data)
                                    {
                                        await Mediator.Send(new DeleteConcoctionLineRequest(items_delete.Id));
                                    }
                                }
                                await Mediator.Send(new DeleteConcoctionRequest(item_delete.Id));
                            }
                        }
                        var data_log = Logs.Where(x => x.PharmacyId == item).ToList();
                        foreach (var items in data_log)
                        {
                            await Mediator.Send(new DeletePharmacyLogRequest(items.Id));
                        }

                        await Mediator.Send(new DeletePharmacyRequest(item));
                    }
                }
                ToastService.ShowSuccess("Delete Data Success!!");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Delete Function

        #region function Edit Click

        private async Task EditItem_Click()
        {
            var data = SelectedDataItems[0].Adapt<PharmacyDto>();
            NavigationManager.NavigateTo($"pharmacy/prescriptions/{EnumPageMode.Update.GetDisplayName()}/?Id={data.Id}");
        }

        #endregion function Edit Click

        #region Refresh fuction

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        #endregion Refresh fuction

        #region Delete Grid Config

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Delete Grid Config



    }
}
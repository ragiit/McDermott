using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;
using static McDermott.Application.Features.Commands.Pharmacies.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacies.MedicamentGroupCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PharmacyCommand;

namespace McDermott.Web.Components.Pages.Pharmacies.Prescription
{
    public partial class CreateUpdateConcoctionPage
    {
        #region Relation Data

        private List<ConcoctionDto> GetConcoctions { get; set; } = [];
        private List<ConcoctionLineDto> GetConcoctionLine { get; set; } = [];
        private List<StockOutLinesDto> GetStockOutLines { get; set; } = [];
        private List<PharmacyDto> GetPharmacy { get; set; } = [];
        private List<UserDto> GetPractitioners { get; set; } = [];
        private List<ActiveComponentDto> ActiveComponents { get; set; } = [];
        private List<TransactionStockDto> GetTransactionStocks { get; set; } = [];
        private List<StockOutLinesDto> StockOutLines { get; set; } = [];
        private List<ProductDto> GetProducts { get; set; } = [];
        private List<MedicamentGroupDto> GetMedicamentGroup { get; set; } = [];
        private List<MedicamentDto> GetMedicament { get; set; } = [];
        private List<DrugRouteDto> GetDrugRoutes { get; set; } = [];
        private List<DrugFormDto> GetDrugForms { get; set; } = [];
        private List<DrugDosageDto> GetDrugDosages { get; set; } = [];
        private List<UomDto> GetUom { get; set; } = [];

        private ConcoctionDto PostConcoction { get; set; } = new();
        private ConcoctionLineDto PostConcoctionLine { get; set; } = new();
        private StockOutLinesDto PostStockOutLines { get; set; } = new();
        private PharmacyDto PostPharmacy { get; set; } = new();
        private TransactionStockDto PostTransactionStock { get; set; } = new();

        #endregion Relation Data

        #region Static Variabel

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        [SupplyParameterFromQuery]
        public long? PId { get; set; }

        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool PanelVisibleCl { get; set; } = false;
        public bool FormValidationState { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsCl { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexCl { get; set; }

        #endregion Static Variabel

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

        #region Searching

        private int pageSizeConcoctionLine { get; set; } = 10;
        private int totalCountConcoctionLine = 0;
        private int activePageIndexConcoctionLine { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadDataConcoctionLine(0, pageSizeConcoctionLine);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSizeConcoctionLine = newPageSize;
            await LoadDataConcoctionLine(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadDataConcoctionLine(newPageIndex, pageSizeConcoctionLine);
        }

        #endregion Searching

        #region Load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadProduct();
            //await LoadDataTransaction();
            //await LoadDataActiveComponent();
            await LoadDosage();
            await LoadRoute();
            //await LoadDataSigna();
            await LoadForm();
            await LoadData();
            //await LoadDataPatient();
            await LoadDataPractitioner();

            PanelVisible = false;
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                SelectedDataItemsCl = [];
                if (PId.HasValue)
                {
                    var cekPharmacy = await Mediator.Send(new GetSinglePharmacyQuery
                    {
                        Predicate = x => x.Id == PId
                    });
                    PostPharmacy = cekPharmacy ?? new();

                    PostConcoction.PharmacyId = PostPharmacy.Id;
                    PostConcoction.PractitionerId = PostPharmacy.PractitionerId;
                }
                else
                {
                    if (PageMode == EnumPageMode.Update.GetDisplayName())
                    {
                        var result = await Mediator.Send(new GetSingleConcoctionQuery
                        {
                            Predicate = x => x.Id == Id,
                        });
                        if (result is null || !Id.HasValue)
                        {
                            NavigationManager.NavigateTo("pharmacy/prescriptions");
                            return;
                        }

                        PostConcoction = result ?? new();

                        await LoadDataConcoctionLine();
                    }
                }
                PanelVisible = false;
            }
            catch (Exception ex) { ex.HandleException(ToastService); }
        }

        private async Task LoadDataConcoctionLine(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisibleCl = true;

                var result = await Mediator.Send(new GetConcoctionLineQuery
                {
                    Predicate = x => x.ConcoctionId == PostConcoction.Id,
                });
                GetConcoctionLine = result.Item1;
                totalCountConcoctionLine = result.PageCount;
                activePageIndexConcoctionLine = result.PageIndex;
                PanelVisibleCl = false;
            }
            catch { }
        }

        private async Task LoadDataPractitioner()
        {
            var getPractitioner = await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.Id == PostPharmacy.PractitionerId,
            });

            GetPractitioners = getPractitioner.Item1;
        }

        #endregion Load Data

        #region Selected Change

        private async Task SelectedMedicamentGroupConcoction(MedicamentGroupDto value)
        {
            try
            {
                if (value is null)
                    return;

                var medicamentGroup = GetMedicamentGroup.Where(x => x.Id == value.Id).FirstOrDefault();
                PostConcoction.MedicamenName = medicamentGroup?.Name;
                PostConcoction.DrugFormId = medicamentGroup?.FormDrugId;

                var a = await Mediator.Send(new GetMedicamentGroupDetailQuery());
                var data = a.Where(x => x.MedicamentGroupId == value?.Id).ToList();
                List<ConcoctionLineDto> concoctionLinesList = new();

                foreach (var item in data)
                {
                    var checkProduct = GetProducts.FirstOrDefault(x => x.Id == item.MedicamentId);
                    if (checkProduct == null)
                        return;

                    var stockProduct = GetTransactionStocks
                                    .Where(x => x.ProductId == checkProduct.Id && x.LocationId is not null && x.LocationId == PostPharmacy.LocationId && x.Validate == true)
                                    .Sum(x => x.Quantity);

                    var medicamentData = GetMedicament.FirstOrDefault(x => x.ProductId == checkProduct.Id);
                    if (medicamentData is null)
                        return;

                    var newConcoctionLine = new ConcoctionLineDto
                    {
                        ProductId = checkProduct.Id,
                        ProductName = checkProduct.Name,
                    };
                    float? QtyPerDay = 0;
                    if (medicamentData.FrequencyId != null)
                    {
                        QtyPerDay = medicamentData?.Frequency?.TotalQtyPerDay;
                    }
                    else
                    {
                        QtyPerDay = 0;
                    }
                    newConcoctionLine.Id = Helper.RandomNumber;
                    newConcoctionLine.MedicamentDosage = medicamentData!.Dosage;
                    newConcoctionLine.ActiveComponentId = medicamentData?.ActiveComponentId;
                    newConcoctionLine.Dosage = medicamentData!.Dosage;
                    newConcoctionLine.UomId = medicamentData?.UomId;
                    newConcoctionLine.UomName = medicamentData?.Uom?.Name;

                    if (newConcoctionLine.Dosage <= newConcoctionLine.MedicamentDosage)
                    {
                        newConcoctionLine.TotalQty = 1;
                    }
                    else
                    {
                        var temp = (newConcoctionLine.Dosage / newConcoctionLine.MedicamentDosage) + (newConcoctionLine.Dosage % newConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
                        newConcoctionLine.TotalQty = temp * PostConcoction.ConcoctionQty;
                    }
                    if (stockProduct == 0)
                    {
                        newConcoctionLine.AvaliableQty = 0;
                    }
                    else
                    {
                        newConcoctionLine.AvaliableQty = stockProduct;
                    }
                    newConcoctionLine.ActiveComponentName = string.Join(",", ActiveComponents.Where(a => medicamentData?.ActiveComponentId is not null && medicamentData.ActiveComponentId.Contains(a.Id)).Select(n => n.Name));
                    concoctionLinesList.Add(newConcoctionLine);
                }

                GetConcoctionLine = concoctionLinesList;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task ChangeProduct(ProductDto product)
        {
            try
            {
                if (product is null)
                    return;

                var ChekMedicament = await Mediator.Send(new GetSingleMedicamentQuery
                {
                    Predicate = x => x.ProductId == product.Id
                });
                var checkUom = await Mediator.Send(new GetSingleUomQuery { Predicate = x => x.Id == ChekMedicament.UomId });

                PostConcoctionLine.Dosage = ChekMedicament?.Dosage ?? 0;
                PostConcoctionLine.MedicamentDosage = ChekMedicament?.Dosage ?? 0;
                PostConcoctionLine.UomId = ChekMedicament?.UomId ?? null;

                if (PostConcoctionLine.Dosage <= PostConcoctionLine.MedicamentDosage)
                {
                    PostConcoctionLine.TotalQty = 1;
                }
                else
                {
                    var temp = (PostConcoctionLine.Dosage / PostConcoctionLine.MedicamentDosage) + (PostConcoctionLine.Dosage % PostConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
                    PostConcoctionLine.TotalQty = temp * PostConcoction.ConcoctionQty;
                }
                selectedActiveComponents = ActiveComponents.Where(a => ChekMedicament != null && ChekMedicament.ActiveComponentId != null && ChekMedicament.ActiveComponentId.Contains(a.Id)).ToList();
                var aa = PostPharmacy.LocationId;
                var checkStock = GetTransactionStocks.Where(x => x.ProductId == product.Id && x.LocationId == PostPharmacy.LocationId && x.Validate == true).Sum(x => x.Quantity);
                PostConcoctionLine.AvaliableQty = checkStock;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void ChangeTotalQtyInHeader(long value)
        {
            PostConcoction.ConcoctionQty = value;
            if (PostConcoctionLine.Dosage <= PostConcoctionLine.MedicamentDosage)
            {
                PostConcoctionLine.TotalQty = 1;
            }
            else
            {
                double temp = ((double)PostConcoctionLine.Dosage / (double)PostConcoctionLine.MedicamentDosage) * (double)value;
                PostConcoctionLine.TotalQty = (long)Math.Ceiling(temp);
            }
        }

        private void ChangeTotalQtyMedicament(long value)
        {
            //Convert Variabel
            if (value == 0)
                return;

            PostConcoctionLine.Dosage = value;
            if (value <= PostConcoctionLine.MedicamentDosage)
            {
                PostConcoctionLine.TotalQty = 1;
            }
            else
            {
                if (PostConcoctionLine.MedicamentDosage != 0)
                {
                    double temp = ((double)value / (double)PostConcoctionLine.MedicamentDosage) * (double)PostConcoction.ConcoctionQty;
                    PostConcoctionLine.TotalQty = (long)Math.Ceiling(temp);
                }
                else
                {
                    ToastService.ShowInfo("Medicament Dosage Not Null!");
                }
            }
        }

        #endregion Selected Change

        #region Grid

        private void GridCl_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexCl = args.VisibleIndex;
        }

        #endregion Grid

        #region ComboBox

        private CancellationTokenSource? _cts;

        #region ComboBox Product

        private ProductDto SelectedProducts { get; set; } = new();

        private async Task SelectedItemChanged(ProductDto e)
        {
            if (e is null)
            {
                SelectedProducts = new();
                await LoadProduct(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedProducts = e;
        }

        private async Task OnInputProduct(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadProduct(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadProduct(string? e = "", Expression<Func<Product, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                GetProducts = await Mediator.QueryGetComboBox<Product, ProductDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Product

        #region ComboBox Drug Route

        private DrugRouteDto SelectedRoute { get; set; } = new();

        private async Task SelectedItemChanged(DrugRouteDto e)
        {
            if (e is null)
            {
                SelectedRoute = new();
                await LoadRoute(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedRoute = e;
        }

        private async Task OnInputRoute(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadRoute(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadRoute(string? e = "", Expression<Func<DrugRoute, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                GetDrugRoutes = await Mediator.QueryGetComboBox<DrugRoute, DrugRouteDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Drug Route

        #region ComboBox Drug Form

        private DrugFormDto SelectedForm { get; set; } = new();

        private async Task SelectedItemChanged(DrugFormDto e)
        {
            if (e is null)
            {
                SelectedForm = new();
                await LoadForm(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedForm = e;
        }

        private async Task OnInputForm(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadForm(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadForm(string? e = "", Expression<Func<DrugForm, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                GetDrugForms = await Mediator.QueryGetComboBox<DrugForm, DrugFormDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Drug Form

        #region ComboBox Drug Dosage

        private DrugDosageDto SelectedDosage { get; set; } = new();

        private async Task SelectedItemChanged(DrugDosageDto e)
        {
            if (e is null)
            {
                SelectedDosage = new();
                await LoadDosage(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedDosage = e;
        }

        private async Task OnInputDosage(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadDosage(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadDosage(string? e = "", Expression<Func<DrugDosage, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                GetDrugDosages = await Mediator.QueryGetComboBox<DrugDosage, DrugDosageDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Drug Dosage

        #endregion ComboBox

        #region New Edit Delete Refresh Click

        private async Task NewItemCl_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItemCl_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndexCl);
        }

        private async Task DeleteItemCl_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndexCl);
        }

        private async Task Refresh_Click()
        {
            await LoadDataConcoctionLine();
        }

        private async Task OnDiscard()
        {
            NavigationManager.NavigateTo($"pharmacy/prescriptions/{EnumPageMode.Update.GetDisplayName()}/?Id={PostConcoction.PharmacyId}");
        }

        #endregion New Edit Delete Refresh Click

        #region Delete Function

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisibleCl = true;
            var data = SelectedDataItemsCl[0].Adapt<ConcoctionLineDto>();
            if (data is not null)
            {
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteConcoctionLineRequest(((ConcoctionLineDto)e.DataItem).Id));
                }
                else
                {
                    var ClToDelete = SelectedDataItemsCl.Adapt<List<ConcoctionLineDto>>();
                    await Mediator.Send(new DeleteConcoctionLineRequest(ids: ClToDelete.Select(x => x.Id).ToList()));
                }
            }
            PanelVisibleCl = false;
        }

        #endregion Delete Function

        #region Function Save

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            if (FormValidationState)
                await OnSave();
        }

        private async Task HandleInvalidSubmit()
        {
            FormValidationState = true;
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }

        private async Task OnSave()
        {
            try
            {
                if (PostConcoction.Id == 0)
                {
                    PostConcoction.PharmacyId = PostPharmacy.Id;
                    PostConcoction.PractitionerId = PostPharmacy.PractitionerId;
                    await Mediator.Send(new CreateConcoctionRequest(PostConcoction));
                    ToastService.ShowSuccess("Add Data Concoction Success");
                }
                else
                {
                    await Mediator.Send(new UpdateConcoctionRequest(PostConcoction));
                    ToastService.ShowSuccess("Add Data Concoction Success");
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSaveCl()
        {
            try
            {
                if (PostConcoctionLine.Id == 0)
                {
                    PostConcoctionLine.ConcoctionId = PostConcoction.Id;
                    await Mediator.Send(new CreateConcoctionLineRequest(PostConcoctionLine));
                    ToastService.ShowSuccess("Add ConcoctionLine Success");
                }
                else
                {
                    await Mediator.Send(new UpdateConcoctionLineRequest(PostConcoctionLine));
                    ToastService.ShowSuccess("Update ConcoctionLine Success");
                }

                await LoadDataConcoctionLine();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Function Save

        #region Cut Stock

        private bool isDetailLines { get; set; } = false;

        //private long? StockIds { get; set; }
        private bool traceAvailability { get; set; } = false;

        private long Lines_Id { get; set; } = 0;

        private async Task cancelCutStockLines()
        {
            isDetailLines = false;
            SelectedDataItemsCl = [];
            StateHasChanged();
        }

        private async Task ShowCutStockLines(long LinesId)
        {
            PanelVisible = true;
            Lines_Id = LinesId;
            //Get the ConcoctionLines by ID
            var concoctionLines = await Mediator.Send(new GetSingleConcoctionLineQuery
            {
                Predicate = x => x.Id == LinesId
            });
            if (concoctionLines == null)
            {
                // Handle case where the ConcoctionLine is not found
                return;
            }

            //Get the Product associated with the ConcoctionLine
            var product = GetProducts.FirstOrDefault(x => x.Id == concoctionLines.ProductId);
            if (product == null)
            {
                // Handle case where the product is not found
                return;
            }
            // Update state variables
            traceAvailability = product.TraceAbility;

            isDetailLines = true;

            // Filter stock products based on specific conditions

            var StockOutProducts = GetTransactionStocks
                    .Where(s => s.ProductId == product.Id && s.LocationId == PostPharmacy.LocationId && s.Quantity > 0)
                    .OrderBy(x => x.ExpiredDate)
                    .GroupBy(s => s.Batch)
                    .Select(g => new TransactionStockDto
                    {
                        ProductId = product.Id,
                        Batch = g.Key,
                        ExpiredDate = g.First().ExpiredDate,
                        Quantity = g.Sum(x => x.Quantity),
                        LocationId = PostPharmacy.LocationId
                    }).ToList();

            // Fetch stock out prescription data

            if (product.TraceAbility)
            {
                var dataStock = await Mediator.Send(new GetStockOutLinesQuery { Predicate = x => x.LinesId == LinesId });
                if (dataStock.Item1 == null || dataStock.Item1.Count == 0)
                {
                    long currentStockInput = 0;
                    PostStockOutLines.CutStock = 0;
                    var listStockOutLines = new List<StockOutLinesDto>();

                    foreach (var item in StockOutProducts)
                    {
                        if (currentStockInput >= concoctionLines.TotalQty) break;

                        var newStockOutLines = new StockOutLinesDto
                        {
                            Id = Helper.RandomNumber,
                            LinesId = concoctionLines.Id,
                            TransactionStockId = item.Id,
                            Batch = item.Batch,
                            ExpiredDate = item.ExpiredDate,
                            CurrentStock = item.Quantity
                        };

                        if (currentStockInput == 0)
                        {
                            newStockOutLines.CutStock = item.Quantity > concoctionLines.TotalQty ? concoctionLines.TotalQty : item.Quantity;
                        }
                        else
                        {
                            long remainingNeeded = concoctionLines.TotalQty - currentStockInput;
                            newStockOutLines.CutStock = item.Quantity >= remainingNeeded ? remainingNeeded : item.Quantity;
                        }

                        currentStockInput += newStockOutLines.CutStock;
                        listStockOutLines.Add(newStockOutLines);
                    }

                    StockOutLines = listStockOutLines;
                }
                else
                {
                    // Updating batch and expired values from StockOutProducts
                    foreach (var stock in dataStock.Item1)
                    {
                        var qtys = StockOutProducts.Sum(x => x.Quantity);
                        var stockProduct = GetTransactionStocks.FirstOrDefault(x => x.Id == stock.TransactionStockId);
                        if (stockProduct != null)
                        {
                            stock.TransactionStockId = stockProduct.Id;
                            stock.Batch = stockProduct.Batch;
                            stock.ExpiredDate = stockProduct.ExpiredDate;
                            stock.CurrentStock = qtys;
                        }
                    }
                    StockOutLines = dataStock.Item1;
                }
            }
            else
            {
                var dataStock = await Mediator.Send(new GetStockOutLinesQuery { Predicate = x => x.LinesId == LinesId });

                if (dataStock.Item1 == null || dataStock.Item1.Count == 0)
                {
                    long currentStockInput = 0;
                    PostStockOutLines.CutStock = 0;
                    var listStockOutLines = new List<StockOutLinesDto>();

                    foreach (var item in StockOutProducts)
                    {
                        if (currentStockInput >= concoctionLines.TotalQty) break;

                        var newStockOutLines = new StockOutLinesDto
                        {
                            Id = Helper.RandomNumber,
                            LinesId = concoctionLines.Id,
                            TransactionStockId = item.Id,
                            Batch = item.Batch,
                            ExpiredDate = item.ExpiredDate,
                            CurrentStock = item.Quantity
                        };

                        if (currentStockInput == 0)
                        {
                            newStockOutLines.CutStock = item.Quantity > concoctionLines.TotalQty ? concoctionLines.TotalQty : item.Quantity;
                        }
                        else
                        {
                            long remainingNeeded = concoctionLines.TotalQty - currentStockInput;
                            newStockOutLines.CutStock = item.Quantity >= remainingNeeded ? remainingNeeded : item.Quantity;
                        }

                        currentStockInput += newStockOutLines.CutStock;
                        listStockOutLines.Add(newStockOutLines);
                    }

                    StockOutLines = listStockOutLines;
                }
                else
                {
                    // Updating batch and expired values from StockOutProducts
                    foreach (var stock in dataStock.Item1)
                    {
                        var qtys = StockOutProducts.Sum(x => x.Quantity);
                        var stockProduct = StockOutProducts.FirstOrDefault(x => x.Id == stock.TransactionStockId);
                        if (stockProduct != null)
                        {
                            stock.CurrentStock = qtys;
                        }
                    }
                    StockOutLines = dataStock.Item1;
                }
            }
            PanelVisible = false;
        }

        private async Task SaveStockOutLines()
        {
            try
            {
                foreach (var item in StockOutLines)
                {
                    var cekReference = GetTransactionStocks.Where(x => x.SourceTable == nameof(ConcoctionLine))
                        .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                    int NextReferenceNumber = 1;
                    if (cekReference != null)
                    {
                        int.TryParse(cekReference?.Substring("PHCL#".Length), out NextReferenceNumber);
                        NextReferenceNumber++;
                    }
                    string referenceNumber = $"PHCL#{NextReferenceNumber:D3}";

                    var check_CutStock = await Mediator.Send(new GetSingleConcoctionLineQuery
                    {
                        Predicate = x => x.Id == item.LinesId
                    });
                    var concoction_data = await Mediator.Send(new GetSingleConcoctionQuery
                    {
                        Predicate = x => x.Id == check_CutStock.ConcoctionId
                    });

                    PostTransactionStock.ProductId = check_CutStock?.Product?.Id;
                    PostTransactionStock.SourceTable = nameof(ConcoctionLine);
                    PostTransactionStock.SourcTableId = check_CutStock?.Id;
                    PostTransactionStock.LocationId = concoction_data?.Pharmacy?.LocationId;
                    PostTransactionStock.ExpiredDate = item?.ExpiredDate;
                    PostTransactionStock.Batch = item?.Batch;
                    PostTransactionStock.Quantity = -(item?.CutStock) ?? 0;
                    PostTransactionStock.Reference = referenceNumber;
                    PostTransactionStock.UomId = check_CutStock?.Product?.UomId;
                    PostTransactionStock.Validate = false;
                    var datas = await Mediator.Send(new CreateTransactionStockRequest(PostTransactionStock));

                    item.TransactionStockId = datas.Id;

                    var item_lines = new StockOutLinesDto
                    {
                        CutStock = item.CutStock,
                        TransactionStockId = item.TransactionStockId,
                        LinesId = item.LinesId
                    };

                    var existingStock = await Mediator.Send(new GetStockOutLinesQuery { Predicate = x => x.TransactionStockId == item.TransactionStockId && x.LinesId == item.LinesId });

                    if (!existingStock.Item1.Any())
                    {
                        await Mediator.Send(new CreateStockOutLinesRequest(item_lines));
                    }
                    else
                    {
                        var existingItem = existingStock.Item1.First();
                        existingItem.CutStock = item.CutStock;
                        await Mediator.Send(new UpdateStockOutLinesRequest(existingItem));
                    }
                }
                isDetailLines = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private DateTime? SelectedBatchExpired { get; set; }

        private async Task ChangeOutStockLines(string value)
        {
            SelectedBatchExpired = null;

            if (value is not null)
            {
                var line = await Mediator.Send(new GetSingleConcoctionLineQuery { Predicate = x => x.Id == Lines_Id });
                PostStockOutLines.Batch = value;

                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == line.ProductId &&
                    s.LocationId == PostPharmacy.LocationId &&
                    s.Validate == true
                ));
                // Find the first matching product
                var matchedProduct = stockProducts.FirstOrDefault(x =>
                    x.LocationId == PostPharmacy.LocationId &&
                    x.ProductId == line.ProductId &&
                    x.Batch == PostStockOutLines.Batch
                );

                PostStockOutLines.ExpiredDate = matchedProduct?.ExpiredDate ?? new();

                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == line.ProductId
                && x.LocationId == PostPharmacy.LocationId && x.Batch == PostStockOutLines.Batch));

                PostStockOutLines.CurrentStock = aa.Sum(x => x.Quantity);
            }
        }

        #endregion Cut Stock
    }
}
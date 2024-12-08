using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;
using static McDermott.Application.Features.Commands.Pharmacies.DrugFormCommand;
using static McDermott.Application.Features.Commands.Pharmacies.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacies.MedicamentGroupCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PharmacyCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PrescriptionCommand;
using static McDermott.Application.Features.Commands.Pharmacies.SignaCommand;

namespace McDermott.Web.Components.Pages.Pharmacies.Prescription
{
    public partial class CreateUpdatePrescriptionPage
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

        #region relation Data

        private PharmacyDto postPharmacy { get; set; } = new();
        private StockOutPrescriptionDto postStockOutPrescriptions { get; set; } = new();
        private PrescriptionDto postPrescription { get; set; } = new();
        private ConcoctionDto postConcoction { get; set; } = new();
        private ConcoctionLineDto postConcoctionLine { get; set; } = new();
        private PatientAllergyDto PatientAllergy { get; set; } = new();
        private UserDto postPatient { get; set; } = new();
        private PharmacyLogDto PharmaciesLog { get; set; } = new();

        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];
        private List<StockOutPrescriptionDto> ListStockOutPrescriptions { get; set; } = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];
        private List<PharmacyDto> getPharmacies { get; set; } = [];
        private List<PrescriptionDto> getPrescriptions { get; set; } = [];
        private List<ConcoctionDto> getConcoctions { get; set; } = [];
        private List<ConcoctionLineDto> getConcoctionLines { get; set; } = [];
        private List<StockOutPrescriptionDto> StockOutPrescriptions { get; set; } = [];
        private List<AllergyDto> allergies { get; set; } = [];
        private List<PatientAllergyDto> PatientAllergies { get; set; } = [];
        private List<PharmacyLogDto> Logs { get; set; } = [];
        private List<UserDto> Patients { get; set; } = [];
        private List<UserDto> Practitioners { get; set; } = [];
        private List<UomDto> Uoms { get; set; } = [];
        private List<LocationDto> getLocations { get; set; } = [];
        private List<ServiceDto> Services { get; set; } = [];
        private List<ProductDto> Products { get; set; } = [];
        private List<SignaDto> Signas { get; set; } = [];
        private List<DrugDosageDto> DrugDosages { get; set; } = [];
        private List<MedicamentDto> Medicaments { get; set; } = [];
        private List<MedicamentGroupDto> MedicamentGroups { get; set; } = [];
        private List<MedicamentGroupDto> MedicamentGroupsConcoction { get; set; } = [];
        private List<DrugFormDto> DrugForms { get; set; } = [];
        private List<DrugRouteDto> DrugRoutes { get; set; } = [];
        private List<ActiveComponentDto> ActiveComponents { get; set; } = [];
        private List<TransactionStockDto> TransactionStocks { get; set; } = [];
        private List<TransactionStockDto> StockOutProducts { get; set; } = [];
        private List<ActiveComponentDto> ActiveComponentt { get; set; } = [];
        private TransactionStockDto postTransactionStock { get; set; } = new();
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        private IEnumerable<ActiveComponentDto>? selectedActiveComponentPrescriptions { get; set; } = [];

        #endregion relation Data

        #region variable Static

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        [SupplyParameterFromQuery]
        public long? GcId { get; set; }

        private IGrid Grid { get; set; }
        private IGrid GridPrescription { get; set; }
        private IGrid GridConcoction { get; set; }
        private IGrid GridCutStockOut { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool PanelVisiblePrescription { get; set; } = false;
        private bool PanelVisibleConcoction { get; set; } = false;
        private bool onShowForm { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool isSavePrescription { get; set; } = false;

        [Parameter]
        public bool IsPopUpForm { get; set; } = false;

        public bool FormValidationState { get; set; } = false;

        private List<string> Batch = [];
        private IReadOnlyList<object> SelectedDataItemsPrescription { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsConcoction { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsStockOut { get; set; } = [];

        private int FocusedRowVisibleIndexPrescription { get; set; }
        private int FocusedRowVisibleIndexConcoction { get; set; }
        private int FocusedRowVisibleIndexCutStockOut { get; set; }

        #region Enum

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

        #endregion Enum

        #endregion variable Static

        #region changeData

        private List<GeneralConsultanServiceDto> getGeneralConsultantService = [];
        private GeneralConsultanServiceDto postGeneralConsultantService { get; set; } = new();

        private async Task GetPatientAllergy(long? q = null)
        {
            FoodAllergies.Clear();
            WeatherAllergies.Clear();
            PharmacologyAllergies.Clear();
            SelectedFoodAllergies = [];
            SelectedWeatherAllergies = [];
            SelectedPharmacologyAllergies = [];

            var p = await Mediator.Send(new GetSingleUserQuery
            {
                Predicate = z => z.Id == postPharmacy.PatientId || z.Id == q
            });

            if (p is null || p.PatientAllergyIds is null)
                return;

            #region Get Patient Allergies

            var alergy = (await Mediator.Send(new GetAllergyQuery()));
            FoodAllergies = alergy.Where(x => x.Type == "01").ToList();
            WeatherAllergies = alergy.Where(x => x.Type == "02").ToList();
            PharmacologyAllergies = alergy.Where(x => x.Type == "03").ToList();

            SelectedFoodAllergies = FoodAllergies.Where(x => p.FoodPatientAllergyIds.Contains(x.Id));
            SelectedWeatherAllergies = WeatherAllergies.Where(x => p.WeatherPatientAllergyIds.Contains(x.Id));
            SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => p.PharmacologyPatientAllergyIds.Contains(x.Id));

            if (SelectedFoodAllergies.Count() > 0)
                postPharmacy.IsFood = true;
            if (SelectedWeatherAllergies.Count() > 0)
                postPharmacy.IsWeather = true;
            if (SelectedPharmacologyAllergies.Count() > 0)
                postPharmacy.IsFarmacologi = true;

            #endregion Get Patient Allergies

            //}
        }

        private List<string> Payments = new List<string>
         {
             "Personal",
             "Insurance",
             "BPJS"
         };

        public class StatusComparer : IComparer<EnumStatusPharmacy>
        {
            private static readonly List<EnumStatusPharmacy> StatusOrder = new List<EnumStatusPharmacy> { EnumStatusPharmacy.Draft, EnumStatusPharmacy.SendToPharmacy, EnumStatusPharmacy.Received, EnumStatusPharmacy.Processed, EnumStatusPharmacy.Done, EnumStatusPharmacy.Cancel };

            public int Compare(EnumStatusPharmacy x, EnumStatusPharmacy y)
            {
                int indexX = StatusOrder.IndexOf(x);
                int indexY = StatusOrder.IndexOf(y);

                // Compare the indices
                return indexX.CompareTo(indexY);
            }
        }

        private void checkStock(long value)
        {
            try
            {
                if (value == 0)
                    return;

                var checkStock = TransactionStocks.Where(x => x.ProductId == postPrescription.ProductId && x.LocationId == postPharmacy.LocationId && x.Quantity != 0).Sum(x => x.Quantity)!;
                if (value > checkStock)
                {
                    ToastService.ShowInfo($"Total Stock is less than the requested quantity");
                    isSavePrescription = false;
                }
                else
                {
                    postPrescription.GivenAmount = value;
                    isSavePrescription = true;
                    return;
                }
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
                var checkUom = Uoms.Where(x => x.Id == ChekMedicament?.UomId).FirstOrDefault();

                postConcoctionLine.Dosage = ChekMedicament?.Dosage ?? 0;
                postConcoctionLine.MedicamentDosage = ChekMedicament?.Dosage ?? 0;
                postConcoctionLine.UomId = ChekMedicament?.UomId ?? null;

                if (postConcoctionLine.Dosage <= postConcoctionLine.MedicamentDosage)
                {
                    postConcoctionLine.TotalQty = 1;
                }
                else
                {
                    var temp = (postConcoctionLine.Dosage / postConcoctionLine.MedicamentDosage) + (postConcoctionLine.Dosage % postConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
                    postConcoctionLine.TotalQty = temp * postConcoction.ConcoctionQty;
                }
                selectedActiveComponents = ActiveComponents.Where(a => ChekMedicament != null && ChekMedicament.ActiveComponentId != null && ChekMedicament.ActiveComponentId.Contains(a.Id)).ToList();
                var aa = postPharmacy.LocationId;
                var checkStock = TransactionStocks.Where(x => x.ProductId == product.Id && x.LocationId == postPharmacy.LocationId && x.Validate == true).Sum(x => x.Quantity);
                postConcoctionLine.AvaliableQty = checkStock;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void ChangeTotalQtyInHeader(long value)
        {
            postConcoction.ConcoctionQty = value;
            if (postConcoctionLine.Dosage <= postConcoctionLine.MedicamentDosage)
            {
                postConcoctionLine.TotalQty = 1;
            }
            else
            {
                double temp = ((double)postConcoctionLine.Dosage / (double)postConcoctionLine.MedicamentDosage) * (double)value;
                postConcoctionLine.TotalQty = (long)Math.Ceiling(temp);
            }
        }

        private void ChangeTotalQtyMedicament(long value)
        {
            //Convert Variabel
            if (value == 0)
                return;

            postConcoctionLine.Dosage = value;
            if (value <= postConcoctionLine.MedicamentDosage)
            {
                postConcoctionLine.TotalQty = 1;
            }
            else
            {
                if (postConcoctionLine.MedicamentDosage != 0)
                {
                    double temp = ((double)value / (double)postConcoctionLine.MedicamentDosage) * (double)postConcoction.ConcoctionQty;
                    postConcoctionLine.TotalQty = (long)Math.Ceiling(temp);
                }
                else
                {
                    ToastService.ShowInfo("Medicament Dosage Not Null!");
                }
            }
        }

        private async Task SelectedChangePractition(long? Id)
        {
            MedicamentGroups = MedicamentGroups.Where(x => x.IsConcoction == false && x.PhycisianId == Id).ToList();
        }

        private async Task SelectedMedicament(MedicamentGroupDto medicament)
        {
            if (medicament is null)
                return;

            var a = await Mediator.Send(new GetMedicamentGroupDetailQuery());
            var details = a.Where(x => x.MedicamentGroupId == medicament.Id).ToList();
            List<PrescriptionDto> prescriptionsList = new();

            foreach (var item in details)
            {
                var checkProduct = Products.FirstOrDefault(p => p.Id == item.MedicamentId);
                if (checkProduct == null)
                {
                    continue; // Skip if product is not found
                }

                var stock = TransactionStocks
                    .Where(s => s.ProductId == checkProduct.Id && s.LocationId == postPharmacy.LocationId && s.Quantity != 0).Sum(x => x.Quantity);

                if (stock == 0)
                {
                    stock = 0;
                    ToastService.ClearCustomToasts();
                    ToastService.ShowWarning($"The {checkProduct.Name} product is out of stock, please contact the pharmacy department!!");
                }

                var medicamentDetails = Medicaments.FirstOrDefault(s => s.ProductId == checkProduct.Id);
                if (medicamentDetails == null)
                {
                    continue; // Skip if medicament details are not found
                }

                var newPrescription = new PrescriptionDto
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Name,
                    Stock = stock
                };

                if (medicamentDetails.Dosage != 0 && medicamentDetails.FrequencyId.HasValue)
                {
                    newPrescription.DrugDosageId = medicamentDetails.FrequencyId;
                    newPrescription.Dosage = medicamentDetails.Dosage;
                    newPrescription.DosageFrequency = $"{medicamentDetails.Dosage}/{medicamentDetails.Frequency?.Frequency}";
                }

                newPrescription.Id = Helper.RandomNumber;
                newPrescription.Product = medicamentDetails.Product;
                newPrescription.DrugRoute = medicamentDetails.Route;
                newPrescription.UomId = medicamentDetails.UomId;
                newPrescription.DrugRouteId = medicamentDetails.RouteId;
                newPrescription.PriceUnit = checkProduct.SalesPrice;
                newPrescription.DrugRoutName = medicamentDetails.Route?.Route;
                newPrescription.ActiveComponentId = medicamentDetails.ActiveComponentId;
                newPrescription.ActiveComponentNames = string.Join(",", ActiveComponents.Where(a => medicamentDetails?.ActiveComponentId is not null && medicamentDetails.ActiveComponentId.Contains(a.Id)).Select(n => n.Name));

                prescriptionsList.Add(newPrescription);
            }

            getPrescriptions = prescriptionsList;
        }

        private async Task SelectedProductPrescriptions(ProductDto value)
        {
            if (value is null)
            {
                selectedActiveComponentPrescriptions = [];

                postPrescription.PriceUnit = null;
                postPrescription.ActiveComponentId = null;
                postPrescription.UomId = null;  // Set to null if you want to clear this field
                postPrescription.DrugFromId = null;  // Set to null if you want to clear this field
                postPrescription.Dosage = null;
                postPrescription.DrugDosageId = null;
                postPrescription.DrugRouteId = null;
                postPrescription.Stock = null;

                return;
            }
            var checkMedicament = await Mediator.Send(new GetSingleMedicamentQuery
            {
                Predicate = x => x.ProductId == value.Id
            });
            if (checkMedicament is not null)
            {
                postPrescription.PriceUnit = value.Cost;
                postPrescription.ActiveComponentId = checkMedicament?.ActiveComponentId;
                postPrescription.UomId = checkMedicament?.UomId;
                postPrescription.DrugFromId = checkMedicament?.FormId;
                postPrescription.Dosage = checkMedicament?.Dosage;
                postPrescription.DrugDosageId = checkMedicament?.FrequencyId;
                postPrescription.DrugRouteId = checkMedicament?.RouteId;
                selectedActiveComponentPrescriptions = ActiveComponents.Where(z => checkMedicament.ActiveComponentId is not null && checkMedicament.ActiveComponentId.Contains(z.Id)).ToList();

                var checkStock = TransactionStocks.Where(x => x.ProductId == value.Id && x.LocationId == postPharmacy.LocationId && x.Quantity != 0 && x.Validate == true).Sum(x => x.Quantity);
                if (checkStock == 0)
                {
                    postPrescription.Stock = 0;
                    ToastService.ShowWarning($"The {value.Name} product is out of stock, or choose another product!!");
                    isSavePrescription = false;
                }
                else
                {
                    postPrescription.Stock = checkStock;
                    isSavePrescription = true;
                }
            }
            else
            {
                postPrescription = new PrescriptionDto();
                return;
            }
        }

        private async Task SelectedMedicamentGroupConcoction(MedicamentGroupDto value)
        {
            try
            {
                if (value is null)
                    return;

                var medicamentGroup = MedicamentGroupsConcoction.Where(x => x.Id == value.Id).FirstOrDefault();
                postConcoction.MedicamenName = medicamentGroup?.Name;
                postConcoction.DrugFormId = medicamentGroup?.FormDrugId;

                var a = await Mediator.Send(new GetMedicamentGroupDetailQuery());
                var data = a.Where(x => x.MedicamentGroupId == value?.Id).ToList();
                List<ConcoctionLineDto> concoctionLinesList = new();

                foreach (var item in data)
                {
                    var checkProduct = Products.FirstOrDefault(x => x.Id == item.MedicamentId);
                    if (checkProduct == null)
                        return;

                    var stockProduct = TransactionStocks
                                    .Where(x => x.ProductId == checkProduct.Id && x.LocationId is not null && x.LocationId == postPharmacy.LocationId && x.Validate == true)
                                    .Sum(x => x.Quantity);

                    var medicamentData = Medicaments.FirstOrDefault(x => x.ProductId == checkProduct.Id);
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
                        newConcoctionLine.TotalQty = temp * postConcoction.ConcoctionQty;
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

                getConcoctionLines = concoctionLinesList;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion changeData

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

        #region Searching Prescription

        private int pageSizePrescription { get; set; } = 10;
        private int totalCountPrescription = 0;
        private int activePageIndexPrescription { get; set; } = 0;
        private string searchTermPrescription { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedPrescription(string searchText)
        {
            searchTerm = searchText;
            await LoadDataPrescription(0, pageSize);
        }

        private async Task OnPageSizeIndexChangedPrescription(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadDataPrescription(0, newPageSize);
        }

        private async Task OnPageIndexChangedPrescription(int newPageIndex)
        {
            await LoadDataPrescription(newPageIndex, pageSize);
        }

        #endregion Searching Prescription

        #region Searching Concoction

        private int pageSizeConcoction { get; set; } = 10;
        private int totalCountConcoction = 0;
        private int activePageIndexConcoction { get; set; } = 0;
        private string searchTermConcoction { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedConcoction(string searchText)
        {
            searchTermConcoction = searchText;
            await LoadDataConcoction(0, pageSize);
        }

        private async Task OnPageSizeIndexChangedConcoction(int newPageSize)
        {
            pageSizeConcoction = newPageSize;
            await LoadDataConcoction(0, newPageSize);
        }

        private async Task OnPageIndexChangedConcoction(int newPageIndex)
        {
            await LoadDataConcoction(newPageIndex, pageSize);
        }

        #endregion Searching Concoction

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadDataLocation();
            await LoadProduct();
            await LoadDataTransaction();
            await LoadDataActiveComponent();
            await LoadDataDrugDosage();
            await LoadDataDrugRoute();
            await LoadDataSigna();
            await LoadDataDrugForm();
            await LoadData();
            await LoadDataPatient();
            await LoadDataPractitioner();

            PanelVisible = false;
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemsConcoction = [];
                SelectedDataItemsPrescription = [];
                if (GcId.HasValue)
                {
                    var cekGc = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                    {
                        Predicate = x => x.Id == GcId
                    });
                    postGeneralConsultantService = cekGc ?? new();

                    postPharmacy.Status = EnumStatusPharmacy.Draft;

                    postPharmacy.PatientId = postGeneralConsultantService.PatientId;

                    postPharmacy.PractitionerId = postGeneralConsultantService.PratitionerId;

                    postPharmacy.ServiceId = postGeneralConsultantService.ServiceId;
                    postPharmacy.PaymentMethod = postGeneralConsultantService.Payment;

                    await SelectedChangePractition(postGeneralConsultantService.PratitionerId);
                    allergies = await Mediator.Send(new GetAllergyQuery());

                    await GetPatientAllergy(postPharmacy.PatientId);
                }
                else
                {
                    if (PageMode == EnumPageMode.Update.GetDisplayName())
                    {
                        var result = await Mediator.Send(new GetSinglePharmacyQuery
                        {
                            Predicate = x => x.Id == Id,
                        });
                        if (result is null || !Id.HasValue)
                        {
                            NavigationManager.NavigateTo("pharmacy/prescriptions");
                            return;
                        }

                        postPharmacy = result ?? new();
                        await GetPatientAllergy(postPharmacy.PatientId);
                        await LoadDataPrescription();
                        await LoadDataConcoction();
                    }
                }
                PanelVisible = false;
            }
            catch (Exception ex) { ex.HandleException(ToastService); }
        }

        private async Task LoadDataPrescription(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisiblePrescription = true;

                var result = await Mediator.Send(new GetPrescriptionQuery
                {
                    Predicate = x => x.PharmacyId == postPharmacy.Id,
                });
                getPrescriptions = result.Item1;
                totalCountPrescription = result.PageCount;
                activePageIndexPrescription = result.PageIndex;
                PanelVisiblePrescription = false;
            }
            catch { }
        }

        private async Task LoadDataConcoction(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisibleConcoction = true;
                var result = await Mediator.Send(new GetConcoctionQuery
                {
                    Predicate = x => x.PharmacyId == postPharmacy.Id,
                });
                getConcoctions = result.Item1;
                totalCountConcoction = result.PageCount;
                activePageIndexConcoction = result.PageIndex;
                PanelVisibleConcoction = false;
            }
            catch { }
        }

        #endregion LoadData

        #region New

        private async Task NewItemPrescription_Click()
        {
            await GridPrescription.StartEditNewRowAsync();
        }

        private async Task NewItemConcoction_Click()
        {
            NavigationManager.NavigateTo($"pharmacy/prescriptions/cl/{EnumPageMode.Update.GetDisplayName()}/?PId={postPharmacy.Id}", true);
        }

        #endregion New

        #region refresh

        private async Task RefreshPrescription_Click()
        {
            await LoadDataPrescription();
        }

        private async Task RefreshConcoction_Click()
        {
            await LoadDataConcoction();
        }

        #endregion refresh

        private void GridPrescription_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexPrescription = args.VisibleIndex;
            try
            {
                if (postPrescription.PharmacyId == 0)
                    return;
                if (postPharmacy.Status == EnumStatusPharmacy.Draft)
                    isActiveButton = true;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void GridConcoction_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexConcoction = args.VisibleIndex;
        }

        #region Save

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
                var data = new PharmacyDto();
                if (postPharmacy.Id == 0)
                {
                    postPharmacy.Status = EnumStatusPharmacy.Draft;
                    data = await Mediator.Send(new CreatePharmacyRequest(postPharmacy));

                    PharmaciesLog.status = data.Status;
                    PharmaciesLog.PharmacyId = data.Id;
                    PharmaciesLog.UserById = UserLogin.Id;

                    await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));

                    ToastService.ShowSuccess("Add Pharmacy Data Success");
                }
                else
                {
                    await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));
                    ToastService.ShowSuccess("Update Pharmacy Data Success");
                }
                NavigationManager.NavigateTo($"pharmacy/prescriptions/{EnumPageMode.Update.GetDisplayName()}/?Id={data.Id}", true);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSavePrescription()
        {
            try
            {
                if (postPrescription.Id == 0)
                {
                    postPrescription.PharmacyId = postPharmacy.Id;
                    await Mediator.Send(new CreatePrescriptionRequest(postPrescription));
                    ToastService.ShowSuccess("Add Prescription Success..");
                }
                else
                {
                    await Mediator.Send(new UpdatePrescriptionRequest(postPrescription));
                    ToastService.ShowSuccess("Add Prescription Success..");
                }
                await LoadDataPrescription();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Save

        #region Delete

        private void DeleteItemPrescription_Click()
        {
            GridPrescription.ShowRowDeleteConfirmation(FocusedRowVisibleIndexPrescription);
        }

        private void DeleteItemConcoction_Click()
        {
            GridConcoction.ShowRowDeleteConfirmation(FocusedRowVisibleIndexConcoction);
        }

        private async Task OnDeletePrescription(GridDataItemDeletingEventArgs e)
        {
            PanelVisiblePrescription = true;
            var data = SelectedDataItemsPrescription[0].Adapt<PrescriptionDto>();
            if (data is not null)
            {
                if (SelectedDataItemsPrescription == null || !SelectedDataItemsPrescription.Any())
                {
                    await Mediator.Send(new DeletePrescriptionRequest(((PrescriptionDto)e.DataItem).Id));
                }
                else
                {
                    var presToDelete = SelectedDataItemsPrescription.Adapt<List<PrescriptionDto>>();
                    await Mediator.Send(new DeletePrescriptionRequest(ids: presToDelete.Select(x => x.Id).ToList()));
                }
            }
            PanelVisiblePrescription = false;
        }

        private async Task OnDeleteConcoction(GridDataItemDeletingEventArgs e)
        {
            PanelVisibleConcoction = true;
            var data = SelectedDataItemsConcoction[0].Adapt<ConcoctionDto>();
            var cl = await Mediator.Send(new GetConcoctionLineQuery
            {
                Predicate = x => x.ConcoctionId == data.Id
            });
            if (data is not null)
            {
                if (SelectedDataItemsConcoction == null || !SelectedDataItemsConcoction.Any())
                {
                    if (cl.Item1.Count > 0)
                    {
                        foreach (var a in cl.Item1)
                        {
                            await Mediator.Send(new DeleteConcoctionLineRequest(a.Id));
                        }
                    }
                    await Mediator.Send(new DeleteConcoctionRequest(((ConcoctionDto)e.DataItem).Id));
                }
                else
                {
                    if (cl.Item1.Count > 0)
                    {
                        foreach (var a in cl.Item1)
                        {
                            await Mediator.Send(new DeleteConcoctionLineRequest(a.Id));
                        }
                    }

                    var CocToDelete = SelectedDataItemsConcoction.Adapt<List<ConcoctionDto>>();
                    await Mediator.Send(new DeleteConcoctionRequest(ids: CocToDelete.Select(x => x.Id).ToList()));
                }
            }
            PanelVisibleConcoction = false;
        }

        #endregion Delete

        #region Edit

        private async Task EditItemPrescription_Click()
        {
            await GridPrescription.StartEditRowAsync(FocusedRowVisibleIndexPrescription);
            var c = (GridPrescription.GetDataItem(FocusedRowVisibleIndexPrescription) as PrescriptionDto ?? new());
            var CheckMedicament = await Mediator.Send(new GetSingleMedicamentQuery
            {
                Predicate = x => x.ProductId == c.ProductId
            });
            if (CheckMedicament.ActiveComponentId != null)
            {
                selectedActiveComponentPrescriptions = ActiveComponents.Where(a => CheckMedicament.ActiveComponentId.Contains(a.Id)).ToList();
            }

            // Refresh UI
            StateHasChanged();
        }

        private async Task EditItemConcoction_Click()
        {
            var data = SelectedDataItemsConcoction[0].Adapt<ConcoctionDto>();
            NavigationManager.NavigateTo($"pharmacy/prescriptions/cl/{EnumPageMode.Update.GetDisplayName()}/?Id={data.Id}");
        }

        #endregion Edit

        #region ComboBox

        private async Task LoadDataDrugForm()
        {
            var result = await Mediator.Send(new GetDrugFormQuery());
            DrugForms = result.Item1;
        }

        private async Task LoadDataDrugDosage()
        {
            var result = await Mediator.Send(new GetDrugDosageQuery());
            DrugDosages = result.Item1;
        }

        private async Task LoadDataDrugRoute()
        {
            var result = await Mediator.Send(new GetDrugRouteQuery());
            DrugRoutes = result.Item1;
        }

        private async Task LoadDataSigna()
        {
            var result = await Mediator.Send(new GetSignaQuery());
            Signas = result.Item1;
        }

        private async Task LoadDataActiveComponent()
        {
            var result = await Mediator.Send(new GetActiveComponentQuery());
            ActiveComponents = result.Item1;
        }

        private async Task LoadDataTransaction()
        {
            TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
        }

        //private async Task LoadDataProduct()
        //{
        //    var getProducts = await Mediator.Send(new GetProductQuery());
        //    Products = getProducts.Item1;
        //}
        private async Task LoadDataLocation()
        {
            var getLocation = await Mediator.Send(new GetLocationQuery());

            getLocations = getLocation.Item1;
        }

        private async Task LoadDataPatient()
        {
            var getPatients = await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.Id == postPharmacy.PatientId,
                Select = x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsFoodPatientAllergyIds = x.IsFoodPatientAllergyIds,
                    FoodPatientAllergyIds = x.FoodPatientAllergyIds,
                    IsWeatherPatientAllergyIds = x.IsWeatherPatientAllergyIds,
                    WeatherPatientAllergyIds = x.WeatherPatientAllergyIds,
                    IsPharmacologyPatientAllergyIds = x.IsPharmacologyPatientAllergyIds,
                    PharmacologyPatientAllergyIds = x.PharmacologyPatientAllergyIds
                }
            });

            Patients = getPatients.Item1; ;
        }

        private async Task LoadDataPractitioner()
        {
            var getPractitioner = await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.Id == postPharmacy.PractitionerId,
            });

            Practitioners = getPractitioner.Item1;
        }

        #region ComboBox Location

        private LocationDto SelectedLocation { get; set; } = new();

        private async Task SelectedItemChanged(LocationDto e)
        {
            if (e is null)
            {
                SelectedLocation = new();
                await LoadDataLocation(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedLocation = e;
        }

        private async Task OnInputLocation(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadDataLocation(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadDataLocation(string? e = "", Expression<Func<Locations, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                getLocations = await Mediator.QueryGetComboBox<Locations, LocationDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Location

        #region ComboBox Product

        private ProductDto SelectedProduct { get; set; } = new();

        private async Task SelectedItemChanged(ProductDto e)
        {
            if (e is null)
            {
                SelectedProduct = new();
                await LoadProduct(); // untuk refresh lagi ketika user klik clear
            }
            else
                SelectedProduct = e;
        }

        private CancellationTokenSource? _cts;

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
                Products = await Mediator.QueryGetComboBox<Product, ProductDto>(e, predicate = x => x.HospitalType == "Medicament");
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Product

        #endregion ComboBox

        #region Status

        public async void SendToPharmacy()
        {
            try
            {
                PanelVisible = true;
                var checkData = getPharmacies.Where(x => x.Id == postPharmacy.Id).FirstOrDefault();
                postPharmacy.Status = EnumStatusPharmacy.SendToPharmacy;
                var updates = await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

                PharmaciesLog.PharmacyId = postPharmacy.Id;
                PharmaciesLog.UserById = UserLogin.Id;
                PharmaciesLog.status = EnumStatusPharmacy.SendToPharmacy;

                PanelVisible = false;
                await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        public async void Pharmacied()
        {
            PanelVisible = true;
            var checkData = getPharmacies.Where(x => x.Id == postPharmacy.Id).FirstOrDefault();
            postPharmacy.Status = EnumStatusPharmacy.Received;
            var updates = await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

            PharmaciesLog.PharmacyId = postPharmacy.Id;
            PharmaciesLog.UserById = UserLogin.Id;
            PharmaciesLog.status = EnumStatusPharmacy.Received;
            await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));

            PanelVisible = false;
        }

        public async Task Received()
        {
            var checkData = getPharmacies.Where(x => x.Id == postPharmacy.Id).FirstOrDefault();

            postPharmacy.Status = EnumStatusPharmacy.Processed;
            var updates = await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

            PharmaciesLog.PharmacyId = postPharmacy.Id;
            PharmaciesLog.UserById = UserLogin.Id;
            PharmaciesLog.status = EnumStatusPharmacy.Processed;

            await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));
            await LoadData();
        }

        public async Task ValidateAsync()
        {
            try
            {
                StockOutPrescriptions = (await Mediator.Send(new GetStockOutPrescriptionQuery())).Item1;
                var StockOutLines = (await Mediator.Send(new GetStockOutLinesQuery())).Item1;
                var pharmacyData = getPharmacies.FirstOrDefault(x => x.Id == postPharmacy.Id);
                if (pharmacyData == null)
                {
                    return;
                }

                #region Prescription CutStock

                var prescriptions = getPrescriptions.Where(x => x.PharmacyId == pharmacyData.Id).ToList();
                if (prescriptions.Count > 0)
                {
                    foreach (var prescription in prescriptions)
                    {
                        var stockProducts = TransactionStocks.Where(x => x.SourceTable == nameof(Prescription) && x.SourcTableId == prescription.Id).ToList();
                        if (stockProducts.Count > 0)
                        {
                            foreach (var item in stockProducts)
                            {
                                item.Validate = true;
                                await Mediator.Send(new UpdateTransactionStockRequest(item));
                            }
                        }
                    }
                }

                #endregion Prescription CutStock

                #region ConcoctionLine CutStock

                var dataConcoction = getConcoctions.FirstOrDefault(x => x.PharmacyId == pharmacyData?.Id);
                if (dataConcoction != null)
                {
                    var dataLines = getConcoctionLines.Where(x => x.ConcoctionId == dataConcoction.Id).ToList();
                    if (dataLines.Count > 0)
                    {
                        foreach (var line in dataLines)
                        {
                            var stockProducts = TransactionStocks.Where(x => x.SourceTable == nameof(ConcoctionLine) && x.SourcTableId == line.Id).ToList();
                            if (stockProducts.Count > 0)
                            {
                                foreach (var item in stockProducts)
                                {
                                    item.Validate = true;
                                    await Mediator.Send(new UpdateTransactionStockRequest(item));
                                }
                            }
                        }
                    }
                }

                #endregion ConcoctionLine CutStock

                postPharmacy.Status = EnumStatusPharmacy.Done;
                var updatedPharmacy = await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

                var pharmacyLog = new PharmacyLogDto
                {
                    PharmacyId = pharmacyData.Id,
                    UserById = UserLogin.Id,
                    status = EnumStatusPharmacy.Done
                };

                await Mediator.Send(new CreatePharmacyLogRequest(pharmacyLog));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        public async void onCancel()
        {
            try
            {
                var checkData = getPharmacies.Where(x => x.Id == postPharmacy.Id).FirstOrDefault();
                postPharmacy.Status = EnumStatusPharmacy.Cancel;
                await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

                PharmaciesLog.PharmacyId = postPharmacy.Id;
                PharmaciesLog.UserById = UserLogin.Id;
                PharmaciesLog.status = EnumStatusPharmacy.Cancel;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        // private async Task Etiket()
        // {
        //     try
        //     {
        //         var cetakPrescription = await Mediator.Send(new GetPrescriptionQuery(x => x.PharmacyId == postPharmacy.Id));
        //         var queueNumber = "123";
        //         var NamePatient = "John Doe";
        //         var ServiceKiosk = "Poliklinik Umum";
        //         var nameBPJS = "BPJS Kesehatan";
        //         var noBPJS = "987654321";
        //         var TypeNumber = "Antrian";
        //         var numbers = "456";

        //         // HTML untuk dokumen cetak
        //         bool isBlueBackground = true; // Atur kondisi boolean sesuai kebutuhan
        //         var backgroundColor = isBlueBackground ? "lightblue" : "white";

        //         var contentToPrint = $@"
        // <!DOCTYPE html>
        // <html lang='en'>
        //<head>
        //     <meta charset='UTF-8'>
        //     <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        //     <title>Cetak Antrian</title>
        //     <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css'>
        //     <style>
        //         .border {{
        //             border: 1px solid black;
        //             padding: 10px;
        //             width: 100%;
        //             max-width: 600px;
        //             margin: auto;
        //             background-color: {backgroundColor};
        //         }}
        //         .title {{
        //             text-align: center;
        //             font-weight: bold;
        //             margin-top: 10px;
        //         }}
        //         .content {{
        //             padding: 15px;
        //         }}
        //         .row {{
        //             display: flex;
        //             justify-content: space-between;
        //             margin-bottom: 10px;
        //         }}
        //         .column {{
        //             flex: 1;
        //             padding: 5px;
        //         }}
        //         .bottom-center {{
        //             text-align: center;
        //             margin-top: 20px;
        //             font-weight: bold;
        //         }}
        //         .logo {{
        //             display: block;
        //             margin: 0 auto 10px;
        //             max-width: 100px;
        //         }}
        //     </style>
        // </head>
        // <body>
        //     <div class='border'>
        //         <img src='/image/logo.png' alt='Logo' class='logo' />
        //         <div class='title'>PT. McDERMOTT INDONESIA</div>
        //         <div class='title'>BASE CLINIC</div>
        //         <div class='content'>
        //             <div class='row'>
        //                 <div class='column'><strong>Nama:</strong> ________________________</div>
        //                 <div class='column' style='text-align: right;'><strong>Tgl:</strong> ______</div>
        //             </div>
        //             <div class='row'>
        //                 <div class='column'><strong>X Sehari</strong></div>
        //             </div>
        //             <div class='row'>
        //                 <div class='column'><strong>Tetes Telinga</strong></div>
        //             </div>
        //             <div class='row'>
        //                 <div class='column'><strong>Tetes Mata</strong></div>
        //             </div>
        //             <div class='row'>
        //                 <div class='column'><strong>Oles</strong></div>
        //             </div>
        //             <div class='bottom-center'>
        //                 <strong>Obat Luar</strong>
        //             </div>
        //         </div>
        //     </div>
        // </body>

        // </html>";

        //         // Panggil JavaScript Interop untuk memicu pencetakan
        //         await JsRuntime.InvokeVoidAsync("printJSX", contentToPrint);
        //     }
        //     catch (Exception ex)
        //     {
        //         ex.HandleException(ToastService);
        //     }
        // }

        // private bool PopUpRecipe { get; set; } = false;

        // private async Task Recipe_print()
        // {
        //     try
        //     {
        //         DateTime? startSickLeave = null;
        //         DateTime? endSickLeave = null;
        //         var gc = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.PatientId == postPharmacy.PatientId))).Item1.FirstOrDefault();
        //         var culture = new System.Globalization.CultureInfo("id-ID");

        //         var patienss = Patients.Where(x => x.Id == postPharmacy.PatientId).FirstOrDefault() ?? new();
        //         var age = 0;
        //         if (patienss.DateOfBirth == null)
        //         {
        //             age = 0;
        //         }
        //         else
        //         {
        //             age = DateTime.Now.Year - patienss.DateOfBirth.Value.Year;
        //         }
        //         string todays = gc.RegistrationDate.ToString("dddd", culture) ?? "-";

        //         //int TotalDays = endSickLeave.Value.Day - startSickLeave.Value.Day;

        //         //string WordDays = ConvertNumberHelper.ConvertNumberToWord(TotalDays);

        //         //Gender
        //         string Gender = "";
        //         string OppositeSex = "";
        //         if (patienss.Gender != null)
        //         {
        //             Gender = patienss.Gender == "Male" ? "MALE (L)" : "FEMALE (P)";
        //             OppositeSex = patienss.Gender == "Male" ? "<strike>F(P)</strike>" : "<strike>M(L)</strike>";
        //         }

        //         PopUpRecipe = true;
        //         string GetDefaultValue(string value, string defaultValue = "-")
        //         {
        //             return value ?? defaultValue;
        //         }

        //         var mergeFields = new Dictionary<string, string>
        //         {
        //             {"%NamePatient%", GetDefaultValue(patienss.Name)},
        //             {"%NIP%", GetDefaultValue(patienss.NIP ?? "-")},
        //             {"%Departement%", GetDefaultValue(patienss.Department?.Name ?? "-")},
        //             {"%NameDoctor%", GetDefaultValue(gc?.Pratitioner?.Name ?? "-")},
        //             {"%SIPDoctor%", GetDefaultValue(gc?.Pratitioner?.SipNo ?? "-")},
        //             {"%AddressPatient%", GetDefaultValue(patienss.DomicileAddress1 ?? "-") + GetDefaultValue(patienss.DomicileAddress2 ?? "-")},
        //             {"%AgePatient%", GetDefaultValue(age.ToString())},
        //             //{"%WordDays%", GetDefaultValue(WordDays)},
        //             {"%Days%", GetDefaultValue(todays)},
        //             //{"%TotalDays%", GetDefaultValue(TotalDays.ToString())},
        //             {"%Dates%", GetDefaultValue(gc?.RegistrationDate.ToString("dd MMMM yyyy") ?? "-")},
        //             {"%Times%", GetDefaultValue(gc?.RegistrationDate.ToString("H:MM")?? "-")},
        //             {"%Date%", DateTime.Now.ToString("dd MMMM yyyy")},  // Still no null check needed
        //             {"%Genders%", GetDefaultValue(Gender)},
        //             {"%OppositeSex%", GetDefaultValue(OppositeSex, "")} // Use empty string if null
        //         };

        //         DocumentContent = await DocumentProvider.GetDocumentAsync("Recipets.docx", mergeFields);

        //         // Konversi byte array menjadi base64 string
        //         //var base64String = Convert.ToBase64String(DocumentContent);

        //         //// Panggil JavaScript untuk membuka dan mencetak dokumen
        //         //await JsRuntime.InvokeVoidAsync("printDocument", base64String);
        //     }
        //     catch (Exception ex)
        //     {
        //         ex.HandleException(ToastService);
        //     }
        // }
        private async Task OnDiscard()
        {
            //await LoadDataPharmacy();
        }

        #endregion Status

        #region CutStock

        private bool isDetailPrescription { get; set; } = false;
        private bool traceAvailability { get; set; } = false;
        private long PrescripId { get; set; } = 0;

        private async Task cancelCutStock()
        {
            isDetailPrescription = false;
            SelectedDataItemsStockOut = [];
            StateHasChanged();
        }

        private async Task ShowCutStock(long prescriptionId)
        {
            PanelVisible = true;
            PrescripId = prescriptionId;
            // Get the prescription by ID
            var prescription = await Mediator.Send(new GetSinglePrescriptionQuery
            {
                Predicate = x => x.Id == prescriptionId && x.PharmacyId == postPharmacy.Id
            });
            if (prescription == null)
            {
                // Handle case where the prescription is not found
                return;
            }

            // Get the product associated with the prescription
            var product = Products.FirstOrDefault(x => x.Id == prescription.ProductId);
            if (product == null)
            {
                // Handle case where the product is not found
                return;
            }

            // Update state variables
            traceAvailability = product.TraceAbility;
            isDetailPrescription = true;

            // Filter stock products based on specific conditions
            StockOutProducts = TransactionStocks
                    .Where(s => s.ProductId == product.Id && s.LocationId == postPharmacy.LocationId && s.Quantity > 0)
                    .OrderBy(x => x.ExpiredDate)
                    .GroupBy(s => s.Batch)
                    .Select(g => new TransactionStockDto
                    {
                        ProductId = product.Id,
                        Batch = g.Key,
                        ExpiredDate = g.First().ExpiredDate,
                        Quantity = g.Sum(x => x.Quantity),
                        LocationId = postPharmacy.LocationId
                    }).ToList();

            //StockOutProducts = TransactionStocks
            //    .Where(x => x.ProductId == prescription.ProductId && x.LocationId == Pharmacy.PrescriptionLocationId && x.Quantity != 0)
            //    .OrderBy(x => x.ExpiredDate)
            //    .ToList();

            // Fetch stock out prescription data
            var listDataStockCut = await Mediator.Send(new GetStockOutPrescriptionQuery());

            // Process stock data if the product has traceability
            if (product.TraceAbility)
            {
                var dataStock = await Mediator.Send(new GetStockOutPrescriptionQuery
                {
                    Predicate = x => x.PrescriptionId == prescriptionId
                });
                if (dataStock.Item1 == null || dataStock.Item1.Count == 0)
                {
                    long currentStockInput = 0;
                    postStockOutPrescriptions.CutStock = 0;
                    var listStockOutPrescription = new List<StockOutPrescriptionDto>();

                    foreach (var item in StockOutProducts)
                    {
                        if (currentStockInput >= prescription.GivenAmount) break;

                        var newStockOutPrescription = new StockOutPrescriptionDto
                        {
                            Id = Helper.RandomNumber,
                            PrescriptionId = prescription.Id,
                            TransactionStockId = item.Id,
                            Batch = item.Batch,
                            ExpiredDate = item.ExpiredDate,
                            CurrentStock = item.Quantity
                        };

                        if (currentStockInput == 0)
                        {
                            newStockOutPrescription.CutStock = item.Quantity > prescription.GivenAmount ? prescription.GivenAmount : item.Quantity;
                        }
                        else
                        {
                            long remainingNeeded = prescription.GivenAmount - currentStockInput;
                            newStockOutPrescription.CutStock = item.Quantity >= remainingNeeded ? remainingNeeded : item.Quantity;
                        }

                        currentStockInput += newStockOutPrescription.CutStock;
                        listStockOutPrescription.Add(newStockOutPrescription);
                    }

                    StockOutPrescriptions = listStockOutPrescription;
                }
                else
                {
                    // Updating batch and expired values from StockOutProducts
                    foreach (var stock in dataStock.Item1)
                    {
                        var qtys = StockOutProducts.Sum(x => x.Quantity);
                        var stockProduct = TransactionStocks.FirstOrDefault(x => x.Id == stock.TransactionStockId);
                        if (stockProduct != null)
                        {
                            stock.TransactionStockId = stockProduct.Id;
                            stock.Batch = stockProduct.Batch;
                            stock.ExpiredDate = stockProduct.ExpiredDate;
                            stock.CurrentStock = qtys;
                        }
                    }
                    StockOutPrescriptions = dataStock.Item1;
                }
            }
            else
            {
                var dataStock = await Mediator.Send(new GetStockOutPrescriptionQuery
                {
                    Predicate = x => x.PrescriptionId == prescriptionId
                });

                if (dataStock.Item1 == null || dataStock.Item1.Count == 0)
                {
                    long currentStockInput = 0;
                    postStockOutPrescriptions.CutStock = 0;
                    var listStockOutPrescription = new List<StockOutPrescriptionDto>();

                    foreach (var item in StockOutProducts)
                    {
                        if (currentStockInput >= prescription.GivenAmount) break;
                        if (item.Quantity <= 0) continue;

                        var newStockOutPrescription = new StockOutPrescriptionDto
                        {
                            Id = Helper.RandomNumber,
                            TransactionStockId = item.Id,
                            PrescriptionId = prescription.Id,
                            CurrentStock = item.Quantity
                        };

                        if (currentStockInput == 0)
                        {
                            newStockOutPrescription.CutStock = item.Quantity > prescription.GivenAmount ? prescription.GivenAmount : item.Quantity;
                        }
                        else
                        {
                            long remainingNeeded = prescription.GivenAmount - currentStockInput;
                            newStockOutPrescription.CutStock = item.Quantity >= remainingNeeded ? remainingNeeded : item.Quantity;
                        }

                        currentStockInput += newStockOutPrescription.CutStock;
                        listStockOutPrescription.Add(newStockOutPrescription);
                    }

                    StockOutPrescriptions = listStockOutPrescription;
                }
                else
                {
                    // Updating batch and expired values from StockOutProducts
                    foreach (var stock in dataStock.Item1)
                    {
                        var qtys = StockOutProducts.Sum(x => x.Quantity);
                        var stockProduct = TransactionStocks.FirstOrDefault(x => x.Id == stock.TransactionStockId);
                        if (stockProduct != null)
                        {
                            stock.CurrentStock = qtys;
                        }
                    }
                    StockOutPrescriptions = dataStock.Item1;
                }
            }
            PanelVisible = false;
        }

        private async Task SaveStockOut()
        {
            try
            {
                foreach (var item in StockOutPrescriptions)
                {
                    if (item.TransactionStockId == 0 || item.TransactionStockId is null)
                    {
                        var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(Prescription))
                            .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                        int NextReferenceNumber = 1;
                        if (cekReference != null)
                        {
                            int.TryParse(cekReference?.Substring("PHP#".Length), out NextReferenceNumber);
                            NextReferenceNumber++;
                        }
                        string referenceNumber = $"PHP#{NextReferenceNumber:D3}";

                        var prescription = await Mediator.Send(new GetSinglePrescriptionQuery
                        {
                            Predicate = x => x.Id == item.PrescriptionId
                        });
                        var productd = Products.Where(x => x.Id == prescription.ProductId).FirstOrDefault();

                        postTransactionStock.ProductId = productd?.Id;
                        postTransactionStock.SourceTable = nameof(Prescription);
                        postTransactionStock.SourcTableId = prescription?.Id;
                        postTransactionStock.LocationId = prescription?.Pharmacy?.LocationId;
                        postTransactionStock.ExpiredDate = item?.ExpiredDate;
                        postTransactionStock.Batch = item?.Batch;
                        postTransactionStock.Quantity = -(item?.CutStock) ?? 0;
                        postTransactionStock.Reference = referenceNumber;
                        postTransactionStock.UomId = productd?.UomId;
                        postTransactionStock.Validate = false;
                        var datas = await Mediator.Send(new CreateTransactionStockRequest(postTransactionStock));

                        item.TransactionStockId = datas.Id;
                    }

                    // Create a DTO for the current item
                    var item_cutstock = new StockOutPrescriptionDto
                    {
                        CutStock = item.CutStock,
                        TransactionStockId = item.TransactionStockId,
                        PrescriptionId = item.PrescriptionId
                    };

                    // Check if the item exists in the database
                    var existingStockOut = await Mediator.Send(new GetStockOutPrescriptionQuery { Predicate = x => x.TransactionStockId == item.TransactionStockId && x.PrescriptionId == item.PrescriptionId });

                    if (!existingStockOut.Item1.Any()) // If the item does not exist
                    {
                        await Mediator.Send(new CreateStockOutPrescriptionRequest(item_cutstock));
                    }
                    else // If the item exists, update it
                    {
                        // Assuming you have a method to update existing data
                        var existingItem = existingStockOut.Item1.First();
                        existingItem.CutStock = item.CutStock;
                        await Mediator.Send(new UpdateStockOutPrescriptionRequest(existingItem));
                    }
                }
                isDetailPrescription = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private DateTime? SelectedBatchExpired { get; set; }

        private async Task ChangeOutStock(string value)
        {
            SelectedBatchExpired = null;

            if (value is not null)
            {
                var presc = await Mediator.Send(new GetSinglePrescriptionQuery { Predicate = x => x.Id == PrescripId });
                postStockOutPrescriptions.Batch = value;
                //current Stock
                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == presc.ProductId &&
                    s.LocationId == postPharmacy.LocationId &&
                    s.Validate == true
                ));
                // Find the first matching product
                var matchedProduct = stockProducts.FirstOrDefault(x =>
                    x.LocationId == postPharmacy.LocationId &&
                    x.ProductId == presc?.ProductId &&
                    x.Batch == postStockOutPrescriptions.Batch
                );

                postStockOutPrescriptions.ExpiredDate = matchedProduct?.ExpiredDate ?? new();

                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == presc.ProductId
                && x.LocationId == postPharmacy.LocationId && x.Batch == postStockOutPrescriptions.Batch));

                postStockOutPrescriptions.CurrentStock = aa.Sum(x => x.Quantity);
            }
        }

        #endregion CutStock
    }
}
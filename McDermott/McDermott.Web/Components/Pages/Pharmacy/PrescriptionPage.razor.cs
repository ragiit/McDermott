using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Domain.Entities;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using static McDermott.Application.Features.Commands.Inventory.ProductCommand;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacy.ConcoctionCommand;
using static McDermott.Application.Features.Commands.Pharmacy.ConcoctionLineCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentGroupCommand;
using static McDermott.Application.Features.Commands.Pharmacy.PharmacyCommand;
using static McDermott.Application.Features.Commands.Pharmacy.PrescriptionCommand;
using static McDermott.Application.Features.Commands.Pharmacy.SignaCommand;

namespace McDermott.Web.Components.Pages.Pharmacy
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
                        StateHasChanged();

                    }
                }
                catch { }

                await LoadAsyncData();
                StateHasChanged();

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

        #region relation Data

        private PharmacyDto Pharmacy { get; set; } = new();
        private StockOutPrescriptionDto FormStockOutPrescriptions { get; set; } = new();
        private StockOutLinesDto FormStockOutLines { get; set; } = new();
        private PrescriptionDto Prescription { get; set; } = new();
        private ConcoctionDto Concoction { get; set; } = new();
        private ConcoctionLineDto ConcoctionLine { get; set; } = new();
        private PatientAllergyDto PatientAllergy = new();
        private UserDto NameUser { get; set; } = new();
        private GroupDto NameGroup { get; set; } = new();
        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];
        private List<StockOutPrescriptionDto> ListStockOutPrescriptions { get; set; } = [];
        private List<StockOutLinesDto> ListStockOutLines { get; set; } = [];
        private List<GroupDto> groups = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];
        private List<PharmacyDto> Pharmacies { get; set; } = [];
        private List<PrescriptionDto> Prescriptions { get; set; } = [];
        private List<ConcoctionDto> Concoctions { get; set; } = [];
        private List<ConcoctionLineDto> ConcoctionLines { get; set; } = [];
        private List<StockOutPrescriptionDto> StockOutPrescriptions { get; set; } = [];
        private List<StockOutLinesDto> StockOutLines { get; set; } = [];
        private List<AllergyDto> allergies { get; set; } = [];
        private List<PatientAllergyDto> PatientAllergies { get; set; } = [];
        private List<PharmacyLogDto> Logs { get; set; } = [];
        private List<UserDto> Patients { get; set; } = [];
        private List<UserDto> Practitioners { get; set; } = [];
        private List<UomDto> Uoms { get; set; } = [];
        private List<LocationDto> PreciptionLocations { get; set; } = [];
        private List<ServiceDto> Services { get; set; } = [];
        private List<ProductDto> Products { get; set; } = [];
        private List<SignaDto> Signas { get; set; } = [];
        private List<DrugDosageDto> DrugDosages { get; set; } = [];
        private List<MedicamentDto> Medicaments { get; set; } = [];
        private List<MedicamentGroupDto> MedicamentGroups { get; set; } = [];
        private List<MedicamentGroupDto> MedicamentGroupsConcoction { get; set; } = [];
        private List<DrugFormDto> DrugForms { get; set; } = [];
        private List<ActiveComponentDto> ActiveComponentt { get; set; } = [];
        private List<DrugRouteDto> DrugRoutes { get; set; } = [];
        private List<TransactionStockDto> TransactionStocks { get; set; } = [];
        private List<TransactionStockDto> StockOutProducts { get; set; } = [];
        private List<ActiveComponentDto> ActiveComponents { get; set; } = [];
        private TransactionStockDto FormTransactionStock { get; set; } = new();
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        private IEnumerable<ActiveComponentDto>? selectedActiveComponentPrescriptions { get; set; } = [];

        #endregion relation Data

        #region Static Variables

        [Parameter]
        public long Id { get; set; }

        private IGrid Grid { get; set; }
        private IGrid GridPrescriptionLines { get; set; }
        private IGrid GridConcoction { get; set; }
        private IGrid GridConcoctionLines { get; set; }
        private IGrid GridStockOut { get; set; }
        private IGrid GridStockOutLines { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsPrescriptionLines { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsConcoction { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsConcoctionLines { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsStockOut { get; set; } = [];

        [Parameter]
        public bool showForm { get; set; } = false;

        [Parameter]
        public UserDto User { get; set; } = new()
        {
            Name = "-"
        };

        private List<string> Batch = [];
        private bool PanelVisible { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private bool isView { get; set; } = false;
        private bool isActive { get; set; } = true;
        private bool onShowForm { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool PopUpConcoctionDetail { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexPrescriptionLines { get; set; }
        private int FocusedRowVisibleIndexPrescriptionConcoction { get; set; }
        private int FocusedRowVisibleIndexConcoctionLines { get; set; }
        private int FocusedRowVisibleIndexStockOut { get; set; }
        private int FocusedRowVisibleIndexStockOutLines { get; set; }
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

        #region change Data

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (Id == 0)
                return;

            var generalConsultantService = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == Id));
            if (generalConsultantService.Count == 0 || generalConsultantService is null)
                return;

            onShowForm = true;
            isActiveButton = true;
            Pharmacy.Status = EnumStatusPharmacy.Draft;

            Pharmacy.PatientId = generalConsultantService.FirstOrDefault()!.PatientId;
            Pharmacy.PractitionerId = generalConsultantService.FirstOrDefault()!.PratitionerId;
            Pharmacy.ServiceId = generalConsultantService.FirstOrDefault()!.ServiceId;
            Pharmacy.PaymentMethod = generalConsultantService.FirstOrDefault()!.Payment;

            await GetPatientAllergy();
        }

        private async Task GetPatientAllergy(long? q = null)
        {
            FoodAllergies.Clear();
            WeatherAllergies.Clear();
            PharmacologyAllergies.Clear();
            SelectedFoodAllergies = [];
            SelectedWeatherAllergies = [];
            SelectedPharmacologyAllergies = [];

            // Filter allergies by type
            FoodAllergies = allergies.Where(x => x.Type == "01").ToList();
            WeatherAllergies = allergies.Where(x => x.Type == "02").ToList();
            PharmacologyAllergies = allergies.Where(x => x.Type == "03").ToList();

            var p = Patients.FirstOrDefault(z => z.Id == Pharmacy.PatientId || z.Id == q);

            if (p is null || p.PatientAllergyIds is null)
                return;

            var Allergies = await Mediator.Send(new GetAllergyQuery(x => p.PatientAllergyIds.Contains(x.Id)));
            if (Allergies.Count > 0)
            {
                // Assuming you have another list of selected allergies IDs
                var selectedAllergyIds = Allergies.Where(x => x.Type == "01" || x.Type == "02" || x.Type == "03").Select(x => x.Id).ToList();

                // Select specific allergies by their IDs
                SelectedFoodAllergies = FoodAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
                SelectedWeatherAllergies = WeatherAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
                SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();

                if (SelectedFoodAllergies.Count() > 0)
                    Pharmacy.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    Pharmacy.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    Pharmacy.IsFarmacologi = true;
            }
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

                var checkStock = TransactionStocks.Where(x => x.ProductId == Prescription.ProductId && x.LocationId == Pharmacy.PrescriptionLocationId && x.Quantity != 0).Sum(x => x.Quantity)!;
                if (value > checkStock)
                {
                    ToastService.ShowInfo($"Total Stock is less than the requested quantity");
                    isSavePrescription = false;
                }
                else
                {
                    Prescription.GivenAmount = value;
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

                var a = await Mediator.Send(new GetMedicamentQuery());
                var ChekMedicament = a.Where(m => m.ProductId == product.Id).FirstOrDefault();
                var checkUom = Uoms.Where(x => x.Id == ChekMedicament?.UomId).FirstOrDefault();

                ConcoctionLine.Qty = ChekMedicament?.Dosage ?? 0;
                ConcoctionLine.MedicamentDosage = ChekMedicament?.Dosage ?? 0;
                ConcoctionLine.UomId = ChekMedicament?.UomId ?? null;

                if (ConcoctionLine.Qty <= ConcoctionLine.MedicamentDosage)
                {
                    ConcoctionLine.TotalQty = 1;
                }
                else
                {
                    var temp = (ConcoctionLine.Qty / ConcoctionLine.MedicamentDosage) + (ConcoctionLine.Qty % ConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
                    ConcoctionLine.TotalQty = temp * Concoction.TotalQty;
                }
                selectedActiveComponents = ActiveComponents.Where(a => ChekMedicament != null && ChekMedicament.ActiveComponentId != null && ChekMedicament.ActiveComponentId.Contains(a.Id)).ToList();
                var aa = Pharmacy.PrescriptionLocationId;
                var checkStock = TransactionStocks.Where(x => x.ProductId == product.Id && x.LocationId == Pharmacy.PrescriptionLocationId && x.Validate == true).Sum(x => x.Quantity);
                ConcoctionLine.AvaliableQty = checkStock;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void OnValueChangedTotalQtyDays(long value)
        {
            Concoction.QtyByDay = value;
            Concoction.TotalQty = Concoction.Qty * value;
        }

        private void OnValueChangedTotalQty(long value)
        {
            Concoction.Qty = value;
            Concoction.TotalQty = Concoction.QtyByDay * value;
        }

        private void changeMedicamentDosage(long value)
        {
            ConcoctionLine.MedicamentDosage = value;
            if (ConcoctionLine.Qty <= value)
            {
                ConcoctionLine.TotalQty = 1;
            }
            else
            {
                var temp = (ConcoctionLine.Qty / value) + (ConcoctionLine.Qty % value != 0 ? 1 : 0);
                ConcoctionLine.TotalQty = temp * Concoction.TotalQty;
            }
        }

        private void ChangeTotalQtyInHeader(long value)
        {
            Concoction.TotalQty = value;
            if (ConcoctionLine.Qty <= ConcoctionLine.MedicamentDosage)
            {
                ConcoctionLine.TotalQty = 1;
            }
            else
            {
                var temp = (ConcoctionLine.Qty / ConcoctionLine.MedicamentDosage) + (ConcoctionLine.Qty % ConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
                ConcoctionLine.TotalQty = temp * value;
            }
        }

        private void ChangeTotalQtyMedicament(long value)
        {
            if (value == 0)
                return;

            ConcoctionLine.Qty = value;
            if (value <= ConcoctionLine.MedicamentDosage)
            {
                ConcoctionLine.TotalQty = 1;
            }
            else
            {
                if (ConcoctionLine.MedicamentDosage != 0)
                {
                    var temp = (value / ConcoctionLine.MedicamentDosage) + (value % ConcoctionLine.MedicamentDosage != 0 ? 1 : 1);
                    ConcoctionLine.TotalQty = temp * Concoction.TotalQty;
                }
                else
                {
                    ToastService.ShowInfo("Medicament Dosage Not Null!");
                }
            }
        }

        private void SelectedChangePractition(UserDto? user)
        {
            MedicamentGroups = MedicamentGroups.Where(x => x.IsConcoction == false && x.PhycisianId == user?.Id).ToList();
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
                    .Where(s => s.ProductId == checkProduct.Id && s.LocationId == Pharmacy.PrescriptionLocationId && s.Quantity != 0).Sum(x => x.Quantity);

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

            Prescriptions = prescriptionsList;
        }

        private bool isSavePrescription { get; set; } = false;

        private async Task SelectedProductPrescriptions(ProductDto value)
        {
            if (value is null)
            {
                selectedActiveComponentPrescriptions = [];

                Prescription.PriceUnit = null;
                Prescription.ActiveComponentId = null;
                Prescription.UomId = null;  // Set to null if you want to clear this field
                Prescription.DrugFromId = null;  // Set to null if you want to clear this field
                Prescription.Dosage = null;
                Prescription.DrugDosageId = null;
                Prescription.DrugRouteId = null;
                Prescription.Stock = null;

                return;
            }
            var checkMedicament = Medicaments.Where(x => x.ProductId == value.Id).FirstOrDefault();
            if (checkMedicament is not null)
            {
                Prescription.PriceUnit = value.Cost;
                Prescription.ActiveComponentId = checkMedicament?.ActiveComponentId;
                Prescription.UomId = checkMedicament?.UomId;
                Prescription.DrugFromId = checkMedicament?.FormId;
                Prescription.Dosage = checkMedicament?.Dosage;
                Prescription.DrugDosageId = checkMedicament?.FrequencyId;
                Prescription.DrugRouteId = checkMedicament?.RouteId;
                selectedActiveComponentPrescriptions = ActiveComponents.Where(z => checkMedicament.ActiveComponentId is not null && checkMedicament.ActiveComponentId.Contains(z.Id)).ToList();

                var checkStock = TransactionStocks.Where(x => x.ProductId == value.Id && x.LocationId == Pharmacy.PrescriptionLocationId && x.Quantity != 0 && x.Validate == true).Sum(x => x.Quantity);
                if ( checkStock == 0)
                {
                    Prescription.Stock = 0;
                    ToastService.ShowWarning($"The {value.Name} product is out of stock, or choose another product!!");
                    isSavePrescription = false;
                }
                else
                {
                    Prescription.Stock = checkStock;
                    isSavePrescription = true;
                }
            }
            else
            {
                Prescription = new PrescriptionDto();
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
                Concoction.MedicamenName = medicamentGroup?.Name;
                Concoction.UomId = medicamentGroup?.UoMId;
                Concoction.DrugFormId = medicamentGroup?.FormDrugId;

                var a = await Mediator.Send(new GetMedicamentGroupDetailQuery());
                var data = a.Where(x => x.MedicamentGroupId == value?.Id).ToList();
                List<ConcoctionLineDto> concoctionLinesList = new();

                foreach (var item in data)
                {
                    var checkProduct = Products.FirstOrDefault(x => x.Id == item.MedicamentId);
                    if (checkProduct == null)
                        return;

                    var stockProduct = TransactionStocks
                                    .Where(x => x.ProductId == checkProduct.Id && x.LocationId is not null && x.LocationId == Pharmacy.PrescriptionLocationId && x.Validate == true)
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
                    newConcoctionLine.Qty = medicamentData!.Dosage;
                    newConcoctionLine.UomId = medicamentData?.UomId;
                    newConcoctionLine.UomName = medicamentData?.Uom?.Name;

                    if (newConcoctionLine.Qty <= newConcoctionLine.MedicamentDosage)
                    {
                        newConcoctionLine.TotalQty = 1;
                    }
                    else
                    {
                        var temp = (newConcoctionLine.Qty / newConcoctionLine.MedicamentDosage) + (newConcoctionLine.Qty % newConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
                        newConcoctionLine.TotalQty = temp * Concoction.TotalQty;
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

                ConcoctionLines = concoctionLinesList;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion change Data

        #region Load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }

        #region Cut Stock
        private bool isDetailPrescription { get; set; } = false;
        private bool isDetailLines { get; set; } = false;
        //private long? StockIds { get; set; }
        private bool traceAvailability { get; set; } = false;
        private long PrescripId { get; set; } = 0;
        private long Lines_Id { get; set; } = 0;

        private async Task cancelCutStock()
        {
            isDetailPrescription = false;
            SelectedDataItemsStockOut = [];
            await EditItemPharmacy_Click(Pharmacy);
            StateHasChanged();
        }
        private async Task cancelCutStockLines()
        {
            isDetailLines = false;
            SelectedDataItemsStockOut = [];
            await EditItemPharmacy_Click(Pharmacy);
            StateHasChanged();
        }
        private async Task ShowCutStockLines(long LinesId)
        {
            await RefreshData();
            PanelVisible = true;
            Lines_Id = LinesId;
            //Get the ConcoctionLines by ID
            var concoctionLines = ConcoctionLines.FirstOrDefault(x => x.Id == LinesId);
            if (concoctionLines == null)
            {
                // Handle case where the ConcoctionLine is not found
                return;
            }

            //Get the Product associated with the ConcoctionLine
            var product = Products.FirstOrDefault(x => x.Id == concoctionLines.ProductId);
            if (product == null)
            {
                // Handle case where the product is not found
                return;
            }
            // Update state variables
            traceAvailability = product.TraceAbility;

            isDetailLines = true;

            // Filter stock products based on specific conditions

            StockOutProducts = TransactionStocks
                    .Where(s => s.ProductId == product.Id && s.LocationId == Pharmacy.PrescriptionLocationId && s.Quantity>0)
                    .OrderBy(x => x.ExpiredDate)
                    .GroupBy(s => s.Batch)
                    .Select(g => new TransactionStockDto
                    {
                        ProductId = product.Id,
                        Batch = g.Key,
                        ExpiredDate = g.First().ExpiredDate,
                        Quantity = g.Sum(x => x.Quantity),
                        LocationId = Pharmacy.PrescriptionLocationId
                    }).ToList();

           
            // Fetch stock out prescription data
            var listDataStockCut = await Mediator.Send(new GetStockOutLineQuery());

            if (product.TraceAbility)
            {
                var dataStock = listDataStockCut.Where(x => x.LinesId == LinesId).ToList();
                if (dataStock == null || dataStock.Count == 0)
                {
                    long currentStockInput = 0;
                    FormStockOutLines.CutStock = 0;
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
                    foreach (var stock in dataStock)
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
                    StockOutLines = dataStock;
                }
            }
            else
            {
                var dataStock = listDataStockCut.Where(x => x.LinesId == LinesId).ToList();

                if (dataStock == null || dataStock.Count == 0)
                {
                    long currentStockInput = 0;
                    FormStockOutLines.CutStock = 0;
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
                    foreach (var stock in dataStock)
                    {
                        var qtys = StockOutProducts.Sum(x => x.Quantity);
                        var stockProduct = StockOutProducts.FirstOrDefault(x => x.Id == stock.TransactionStockId);
                        if (stockProduct != null)
                        {
                            stock.CurrentStock = qtys;
                        }
                    }
                    StockOutLines = dataStock;
                }
            }
            PanelVisible = false;
        }
        private async Task ShowCutStock(long prescriptionId)
        {

            await RefreshData();

            PanelVisible = true;
            PrescripId = prescriptionId;
            // Get the prescription by ID
            var prescription = Prescriptions.FirstOrDefault(x => x.Id == prescriptionId && x.PharmacyId == Pharmacy.Id);
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
                    .Where(s => s.ProductId == product.Id && s.LocationId == Pharmacy.PrescriptionLocationId && s.Quantity>0)
                    .OrderBy(x => x.ExpiredDate)
                    .GroupBy(s => s.Batch)
                    .Select(g => new TransactionStockDto
                    {
                        ProductId = product.Id,
                        Batch = g.Key,
                        ExpiredDate = g.First().ExpiredDate,
                        Quantity = g.Sum(x => x.Quantity),
                        LocationId = Pharmacy.PrescriptionLocationId
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
                var dataStock = listDataStockCut.Where(x => x.PrescriptionId == prescriptionId).ToList();
                if (dataStock == null || dataStock.Count == 0)
                {
                    long currentStockInput = 0;
                    FormStockOutPrescriptions.CutStock = 0;
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
                    foreach (var stock in dataStock)
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
                    StockOutPrescriptions = dataStock;
                }
            }
            else
            {
                var dataStock = listDataStockCut.Where(x => x.PrescriptionId == prescriptionId).ToList();

                if (dataStock == null || dataStock.Count == 0)
                {
                    long currentStockInput = 0;
                    FormStockOutPrescriptions.CutStock = 0;
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
                    foreach (var stock in dataStock)
                    {
                        var qtys = StockOutProducts.Sum(x => x.Quantity);
                        var stockProduct = TransactionStocks.FirstOrDefault(x => x.Id == stock.TransactionStockId);
                        if (stockProduct != null)
                        {
                            stock.CurrentStock = qtys;
                        }
                    }
                    StockOutPrescriptions = dataStock;
                }
            }
            PanelVisible = false;

        }

        private async Task RefreshData()
        {
            // Fetch the latest data from the server or database
            Prescriptions = await Mediator.Send(new GetPrescriptionQuery(x => x.PharmacyId == Pharmacy.Id));
            Concoctions = await Mediator.Send(new GetConcoctionQuery());
            var concoctionId = Concoctions.Where(x => x.PharmacyId == Pharmacy.Id).FirstOrDefault();
            Products = await Mediator.Send(new GetProductQuery());
            TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
            ConcoctionLines = await Mediator.Send(new GetConcoctionLineQuery(x => x.ConcoctionId == concoctionId.Id));
        }

        private void HandleDiscard()
        {
            isDetailPrescription = false;
        }
        private void HandleDiscardLines()
        {
            isDetailLines = false;
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

                        var prescription = Prescriptions.Where(x => x.Id == item.PrescriptionId).FirstOrDefault();
                        var productd = Products.Where(x => x.Id == prescription.ProductId).FirstOrDefault();

                        FormTransactionStock.ProductId = productd?.Id;
                        FormTransactionStock.SourceTable = nameof(Prescription);
                        FormTransactionStock.SourcTableId = prescription?.Id;
                        FormTransactionStock.LocationId = prescription?.Pharmacy?.PrescriptionLocationId;
                        FormTransactionStock.ExpiredDate = item?.ExpiredDate;
                        FormTransactionStock.Batch = item?.Batch;
                        FormTransactionStock.Quantity = -(item?.CutStock) ?? 0;
                        FormTransactionStock.Reference = referenceNumber;
                        FormTransactionStock.UomId = productd?.UomId;
                        FormTransactionStock.Validate = false;
                        var datas = await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));

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
                    var existingStockOut = await Mediator.Send(new GetStockOutPrescriptionQuery(x => x.TransactionStockId == item.TransactionStockId && x.PrescriptionId == item.PrescriptionId));

                    if (!existingStockOut.Any()) // If the item does not exist
                    {
                        await Mediator.Send(new CreateStockOutPrescriptionRequest(item_cutstock));
                    }
                    else // If the item exists, update it
                    {
                        // Assuming you have a method to update existing data
                        var existingItem = existingStockOut.First();
                        existingItem.CutStock = item.CutStock;
                        await Mediator.Send(new UpdateStockOutPrescriptionRequest(existingItem));
                    }
                }
                isDetailPrescription = false;
                await EditItemPharmacy_Click(Pharmacy);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private async Task SaveStockOutLines()
        {
            try
            {
                foreach (var item in StockOutLines)
                {

                    var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(ConcoctionLine))
                        .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                    int NextReferenceNumber = 1;
                    if (cekReference != null)
                    {
                        int.TryParse(cekReference?.Substring("PHCL#".Length), out NextReferenceNumber);
                        NextReferenceNumber++;
                    }
                    string referenceNumber = $"PHCL#{NextReferenceNumber:D3}";

                    var check_CutStock = ConcoctionLines.Where(x => x.Id == item.LinesId).FirstOrDefault();
                    var concoction_data = Concoctions.Where(x => x.Id == check_CutStock?.ConcoctionId).FirstOrDefault();

                    FormTransactionStock.ProductId = check_CutStock?.Product?.Id;
                    FormTransactionStock.SourceTable = nameof(ConcoctionLine);
                    FormTransactionStock.SourcTableId = check_CutStock?.Id;
                    FormTransactionStock.LocationId = concoction_data?.Pharmacy?.PrescriptionLocationId;
                    FormTransactionStock.ExpiredDate = item?.ExpiredDate;
                    FormTransactionStock.Batch = item?.Batch;
                    FormTransactionStock.Quantity = -(item?.CutStock) ?? 0;
                    FormTransactionStock.Reference = referenceNumber;
                    FormTransactionStock.UomId = check_CutStock?.Product?.UomId;
                    FormTransactionStock.Validate = false;
                    var datas = await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));

                    item.TransactionStockId = datas.Id;


                    var item_lines = new StockOutLinesDto
                    {
                        CutStock = item.CutStock,
                        TransactionStockId = item.TransactionStockId,
                        LinesId = item.LinesId
                    };

                    var existingStock = await Mediator.Send(new GetStockOutLineQuery(x => x.TransactionStockId == item.TransactionStockId && x.LinesId == item.LinesId));

                    if (!existingStock.Any())
                    {
                        await Mediator.Send(new CreateStockOutLinesRequest(item_lines));
                    }
                    else
                    {
                        var existingItem = existingStock.First();
                        existingItem.CutStock = item.CutStock;
                        await Mediator.Send(new UpdateStockOutLinesRequest(existingItem));
                    }

                }
                isDetailLines = false;
                await EditItemPharmacy_Click(Pharmacy);
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
                var presc = (await Mediator.Send(new GetPrescriptionQuery(x => x.Id == PrescripId))).FirstOrDefault()!;
                FormStockOutPrescriptions.Batch = value;
                //current Stock
                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == presc.ProductId &&
                    s.LocationId == Pharmacy.PrescriptionLocationId &&
                    s.Validate == true
                ));
                // Find the first matching product
                var matchedProduct = stockProducts.FirstOrDefault(x =>
                    x.LocationId == Pharmacy.PrescriptionLocationId &&
                    x.ProductId == presc?.ProductId &&
                    x.Batch == FormStockOutPrescriptions.Batch
                );



                FormStockOutPrescriptions.ExpiredDate = matchedProduct?.ExpiredDate ??  new();


                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == presc.ProductId
                && x.LocationId == Pharmacy.PrescriptionLocationId && x.Batch == FormStockOutPrescriptions.Batch));

                FormStockOutPrescriptions.CurrentStock = aa.Sum(x => x.Quantity);

            }

        }

        private async Task ChangeOutStockLines(string value)
        {
            SelectedBatchExpired = null;

            if (value is not null)
            {
                var line = (await Mediator.Send(new GetConcoctionLineQuery(x=>x.Id == Lines_Id))).FirstOrDefault()!;
                FormStockOutLines.Batch = value;

                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == line.ProductId &&
                    s.LocationId == Pharmacy.PrescriptionLocationId &&
                    s.Validate == true
                ));
                // Find the first matching product
                var matchedProduct = stockProducts.FirstOrDefault(x =>
                    x.LocationId == Pharmacy.PrescriptionLocationId &&
                    x.ProductId == line.ProductId &&
                    x.Batch == FormStockOutLines.Batch
                );

                FormStockOutLines.ExpiredDate = matchedProduct?.ExpiredDate ?? new();

                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == line.ProductId
                && x.LocationId == Pharmacy.PrescriptionLocationId && x.Batch == FormStockOutLines.Batch));

                FormStockOutLines.CurrentStock = aa.Sum(x => x.Quantity);
            }
        }
        #endregion

        private async Task LoadAsyncData()
        {
            try
            {
                PanelVisible = true;
                Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true));
                Practitioners = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
                PreciptionLocations = await Mediator.Send(new GetLocationQuery());
                ActiveComponentt = await Mediator.Send(new GetActiveComponentQuery());
                Services = await Mediator.Send(new GetServiceQuery());
                Products = await Mediator.Send(new GetProductQuery());
                DrugDosages = await Mediator.Send(new GetDrugDosageQuery());
                MedicamentGroups = await Mediator.Send(new GetMedicamentGroupQuery(x => x.IsConcoction == false));
                MedicamentGroupsConcoction = await Mediator.Send(new GetMedicamentGroupQuery(x => x.IsConcoction == true));
                DrugForms = await Mediator.Send(new GetFormDrugQuery());
                Signas = await Mediator.Send(new GetSignaQuery());
                DrugRoutes = await Mediator.Send(new GetDrugRouteQuery());
                Uoms = await Mediator.Send(new GetUomQuery());
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                Medicaments = await Mediator.Send(new GetMedicamentQuery());
                ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
                Pharmacies = await Mediator.Send(new GetPharmacyQuery());
                Concoctions = new List<ConcoctionDto>();
                PatientAllergies = await Mediator.Send(new GetPatientAllergyQuery());
                allergies = await Mediator.Send(new GetAllergyQuery());
                groups = await Mediator.Send(new GetGroupQuery());
                NameGroup = groups.FirstOrDefault(x => x.Id == UserAccessCRUID.GroupId) ?? new();
                var user_group = await Mediator.Send(new GetUserQuery());
                NameUser = user_group.FirstOrDefault(x => x.GroupId == UserAccessCRUID.GroupId && x.Id == UserLogin.Id) ?? new();

                allergies.ForEach(x =>
                {
                    var a = Helper._allergyTypes.FirstOrDefault(z => x.Type is not null && z.Code == x.Type);
                    if (a is not null)
                        x.TypeString = a.Name;
                });
                var c = Concoctions;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SelectedItemMedicalNamePresciptionLinesChanged(ProductDto e)
        {
            try
            {
                Prescription.ProductId = null;
                Prescription.DrugFromId = null;
                Prescription.DrugDosageId = null;
                Prescription.DrugRouteId = null;
                Prescription.PriceUnit = 0;
                Prescription.Stock = 0;

                if (e is null)
                    return;

                Prescription.ProductId = e.Id;

                var medicament = await Mediator.Send(new GetMedicamentQuery(x => x.ProductId == e.Id));
                if (medicament.Count > 0)
                {
                    Prescription.DrugFromId = medicament[0].FormId;
                    Prescription.DrugRouteId = medicament[0].RouteId;
                    //Prescription.DrugRouteId = medicament[0].Dosage;
                }

                var stock = await Mediator.Send(new GetStockProductQuery(x => x.ProductId == e.Id));
                if (stock.Count > 0)
                    Prescription.Stock = stock.Where(x => x.StatusTransaction == "IN").Select(x => x.Qty).Sum();

                if (Products.Count > 0)
                {
                    var p = Products.FirstOrDefault(x => x.Id == e.Id);
                    Prescription.PriceUnit = p!.SalesPrice!.ToLong();
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task LoadData(bool v = false)
        {
            PanelVisible = true;
            if (User != null && User.Id != 0)
            {
                Pharmacies = Pharmacies.Where(x => x.PatientId == User.Id).ToList();
            }

            if (v)
                User = new() { Name = "-" };

            groups = await Mediator.Send(new GetGroupQuery());
            NameGroup = groups.FirstOrDefault(x => x.Id == UserAccessCRUID.GroupId) ?? new();
            var user_group = await Mediator.Send(new GetUserQuery());
            NameUser = user_group.FirstOrDefault(x => x.GroupId == UserAccessCRUID.GroupId && x.Id == UserLogin.Id) ?? new();
            if (Pharmacy.Id == 0 || Pharmacy.Status!.Equals("Draft") || (Pharmacy.Status!.Equals("SendToPharmacy") || Pharmacy.Status!.Equals("Received") && NameGroup.Name.Equals("Admin")))
            {
                isActive = false;
            }

            PanelVisible = false;
        }

        private async Task LoadDataPharmacy()
        {
            try
            {
                onShowForm = false;
                IsLoading = true;
                SelectedDataItems = [];
                SelectedDataItemsConcoction = [];
                SelectedDataItemsPrescriptionLines = [];
                Pharmacies = await Mediator.Send(new GetPharmacyQuery());

                IsLoading = false;
            }
            catch (Exception ex) { ex.HandleException(ToastService); }
        }

        private async Task LoadDataPresciptions()
        {
            IsLoading = true;
            SelectedDataItems = [];
            Prescriptions = await Mediator.Send(new GetPrescriptionQuery());
            IsLoading = false;
        }

        private async Task LoadDataConcoctions()
        {
            PanelVisible = true;
            SelectedDataItems = [];
            Concoctions = await Mediator.Send(new GetConcoctionQuery(x => x.PharmacyId == Pharmacy.Id));
            PanelVisible = false;
        }

        private async Task LoadDataConcoction()
        {
            IsLoading = true;
            SelectedDataItems = [];
            StateHasChanged();
            IsLoading = false;
        }

        private async Task LoadDataConcoctionLines()
        {
            IsLoading = true;
            ConcoctionLines = await Mediator.Send(new GetConcoctionLineQuery());
            IsLoading = false;
            SelectedDataItems = [];
        }

        private async Task LoadDataStockOut()
        {
            IsLoading = true;
            StockOutPrescriptions = await Mediator.Send(new GetStockOutPrescriptionQuery());
            IsLoading = false;
            SelectedDataItems = [];
        }

        private async Task LoadLogs()
        {
            Logs = await Mediator.Send(new GetPharmacyLogQuery(x => x.PharmacyId == Pharmacy.Id));
        }

        #endregion Load Data

        #region Delete Function

        private async Task OnDeletePharmacy(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var data_delete = (PharmacyDto)e.DataItem;
                Prescriptions = await Mediator.Send(new GetPrescriptionQuery());
                Concoctions = await Mediator.Send(new GetConcoctionQuery());
                ConcoctionLines = await Mediator.Send(new GetConcoctionLineQuery());
                StockOutPrescriptions = await Mediator.Send(new GetStockOutPrescriptionQuery());
                Logs = await Mediator.Send(new GetPharmacyLogQuery());
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
                await LoadDataPharmacy();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private string? Prescription_Name { get; set; }

        List<PrescriptionDto> Prescriptions_Data = new List<PrescriptionDto>();
        private void OnDeletePrescriptionLines(GridDataItemDeletingEventArgs e)
        {
            var data = SelectedDataItemsPrescriptionLines.Adapt<List<PrescriptionDto>>();
            if (Pharmacy.Id == 0)
            {
                try
                {
                    if (SelectedDataItemsPrescriptionLines is null || SelectedDataItemsPrescriptionLines.Count == 1)
                    {
                        Prescriptions.Remove((PrescriptionDto)e.DataItem);
                    }
                    else
                    {
                        SelectedDataItemsPrescriptionLines.Adapt<List<LabTestDetailDto>>().Select(x => x.Id).ToList().ForEach(x =>
                        {
                            Prescriptions.Remove(Prescriptions.FirstOrDefault(z => z.Id == x) ?? new());
                        });
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                try
                {
                    Prescription_Name = "Prescription";
                    foreach (var item in data)
                    {
                        Prescriptions.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                        Prescriptions_Data.Add(item);
                    }

                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        private string? Concoction_Name { get; set; }
        List<ConcoctionDto> Concoction_data = new List<ConcoctionDto>();
        private void OnDeleteConcoction(GridDataItemDeletingEventArgs e)
        {
            var data = SelectedDataItemsConcoction.Adapt<List<ConcoctionDto>>();
            if (Pharmacy.Id == 0)
            {
                try
                {
                    if (SelectedDataItemsConcoction is null || SelectedDataItemsConcoction.Count == 1)
                    {
                        Concoctions.Remove((ConcoctionDto)e.DataItem);
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                try
                {

                    Concoction_Name = "Concoction";
                    foreach (var item in data)
                    {
                        Concoctions.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                        Concoction_data.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        private string? ConcoctionLine_Name { get; set; }
        List<ConcoctionLineDto> ConcoctionLine_Data = new List<ConcoctionLineDto>();
        private void OnDeleteConcoctionLine(GridDataItemDeletingEventArgs e)
        {
            var data = SelectedDataItemsConcoctionLines.Adapt<List<ConcoctionLineDto>>();
            if (Pharmacy.Id == 0)
            {
                try
                {
                    if (SelectedDataItemsConcoctionLines is null || SelectedDataItemsConcoctionLines.Count == 1)
                    {
                        ConcoctionLines.Remove((ConcoctionLineDto)e.DataItem);
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                try
                {
                    ConcoctionLine_Name = "ConcoctionLine";
                    foreach (var item in data)
                    {
                        ConcoctionLines.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                        ConcoctionLine_Data.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        private string? StockCut_Name { get; set; }
        List<StockOutPrescriptionDto> StockOut_data = new List<StockOutPrescriptionDto>();
        private string? StockCutLines_Name { get; set; }
        List<StockOutLinesDto> StockOutLines_data = new List<StockOutLinesDto>();
        private  void OnDeleteStockCutPrescription(GridDataItemDeletingEventArgs e)
        {
            var data = SelectedDataItemsStockOut.Adapt<List<StockOutPrescriptionDto>>();
            if (Pharmacy.Id == 0)
            {
                try
                {
                    if (SelectedDataItemsStockOut is null || SelectedDataItemsStockOut.Count == 1)
                    {
                        StockOutPrescriptions.Remove((StockOutPrescriptionDto)e.DataItem);
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                try
                {
                    StockCut_Name = "StockCut";
                    foreach (var item in data)
                    {
                        StockOutPrescriptions.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                        StockOut_data.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        private void OnDeleteStockCutLines(GridDataItemDeletingEventArgs e)
        {
            var data = SelectedDataItemsStockOut.Adapt<List<StockOutLinesDto>>();
            if (Pharmacy.Id == 0)
            {
                try
                {
                    if (SelectedDataItemsStockOut is null || SelectedDataItemsStockOut.Count == 1)
                    {
                        StockOutLines.Remove((StockOutLinesDto)e.DataItem);
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                try
                {
                    StockCutLines_Name = "StockCut";
                    foreach (var item in data)
                    {
                        StockOutLines.RemoveAll(x => data.Select(z => z.Id).Contains(x.Id));
                        StockOutLines_data.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        #endregion Delete Function

        #region handleValidation

        private bool FormValidationState = true;

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            if (FormValidationState)
                await OnSavePharmacy();
        }

        private async Task HandleInvalidConcoctionSubmit()
        {
            FormValidationState = true;
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }

        private async Task HandleValidConcoctionSubmit()
        {
            FormValidationState = true;

            if (FormValidationState)
                await OnSaveConcoction();
        }

        private async Task HandleValidStockOutSubmit()
        {
            FormValidationState = true;

        }

        private async Task HandleInvalidSubmit()
        {
            FormValidationState = true;
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }

        private async Task HandleInvalidStockOutSubmit()
        {
            FormValidationState = true;
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }
        #endregion handleValidation

        #region Fungsi Save

        private async Task OnSaveConcoction()
        {
            if (Concoction is null)
                return;

            if (Pharmacy.Id == 0)
            {
                try
                {
                    ConcoctionDto update = new();

                    Concoction.PrescribingDoctorId = Pharmacy.PractitionerId;

                    if (Concoction.Id == 0)
                    {
                        Concoction.Id = Helper.RandomNumber;
                        if (Concoction.MedicamentGroupId is not null)
                        {
                            Concoction.MedicamenName = MedicamentGroupsConcoction.Where(x => x.Id == Concoction.MedicamentGroupId).Select(x => x.Name).FirstOrDefault();
                        }
                        else
                        {
                            Concoction.MedicamenName = Concoction.MedicamenName;
                        }
                        Concoction.UomName = Uoms.Where(x => x.Id == Concoction.UomId).Select(x => x.Name).FirstOrDefault();
                        Concoction.DrugDosageName = DrugDosages.Where(x => x.Id == Concoction.DrugDosageId).Select(x => x.Frequency).FirstOrDefault();
                        Concoctions.Add(Concoction);
                    }
                    else
                    {
                        var q = SelectedDataItemsConcoction[0].Adapt<ConcoctionDto>();
                        update = Concoctions.FirstOrDefault(x => x.Id == q.Id)!;
                        var index = Concoctions.IndexOf(update!);
                        Concoctions[index] = Concoction;
                    }

                    //await EditItemPharmacy_Click();
                    SelectedDataItemsConcoction = [];
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                Concoction.PharmacyId = Pharmacy.Id;
                if (Concoction.Id == 0)
                {
                    await Mediator.Send(new CreateConcoctionRequest(Concoction));
                }
                else
                {
                    await Mediator.Send(new UpdateConcoctionRequest(Concoction));
                }
                await LoadDataConcoctions();
            }
            PopUpConcoctionDetail = false;
            GridConcoction.Reload();
            StateHasChanged();
        }

        private async Task OnSaveConcoctionLines(GridEditModelSavingEventArgs e)
        {
            if (ConcoctionLine is null)
                return;

            var Cl = (ConcoctionLineDto)e.EditModel;

            // If Pharmacy.Id is 0, handle local updates
            if (Pharmacy.Id == 0)
            {
                try
                {
                    var a = ConcoctionLine;
                    var product = Medicaments.FirstOrDefault(x => x.ProductId == ConcoctionLine.ProductId);
                    if (product != null)
                    {
                        Cl.MedicamentDosage = product.Dosage;
                        Cl.ProductName = product.Product?.Name;
                        Cl.Qty = product.Dosage != 0 ? product.Dosage : ConcoctionLine.Qty;
                        Cl.UomId = product.UomId != null ? product.UomId : ConcoctionLine.UomId;
                        Cl.UomName = Uoms.Where(x => x.Id == Cl.UomId).Select(x => x.Name).FirstOrDefault();
                        Cl.ActiveComponentId = product.ActiveComponentId.Count != 0 ? product.ActiveComponentId : selectedActiveComponents?.Select(x => x.Id).ToList();
                    }

                    Cl.ActiveComponentName = string.Join(",", ActiveComponents
                        .Where(a => ConcoctionLine.ActiveComponentId?.Contains(a.Id) == true)
                        .Select(n => n.Name));

                    if (Cl.Id == 0)
                    {
                        Cl.Id = Helper.RandomNumber; // Assuming this generates a unique ID
                        ConcoctionLines.Add(Cl);
                    }
                    else
                    {
                        var existingIndex = ConcoctionLines.FindIndex(x => x.Id == Cl.Id);
                        if (existingIndex >= 0)
                        {
                            ConcoctionLines[existingIndex] = Cl;
                        }
                    }

                    SelectedDataItemsConcoctionLines = []; // Reset selections
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService); // Assuming a custom error handling method
                }
            }
            // Otherwise, handle server updates
            else
            {
                Cl.ConcoctionId = Concoction.Id;

                if (Cl.Id == 0)
                {
                    await Mediator.Send(new CreateConcoctionLineRequest(Cl));
                }
                else
                {
                    await Mediator.Send(new UpdateConcoctionLineRequest(Cl));
                }

                await LoadDataConcoctionLines();
            }

        }

        private async Task OnSaveStockOut(GridEditModelSavingEventArgs e)
        {

            //if (Pharmacy.Id == 0)
            //{
            try
            {
                if (FormStockOutPrescriptions.CutStock == 0)

                    return;

                var Sc = (StockOutPrescriptionDto)e.EditModel;
                StockOutPrescriptionDto update = new();


                if (Sc.Id == 0)
                {
                    Sc.Id = Helper.RandomNumber;
                    Sc.PrescriptionId = PrescripId;
                    StockOutPrescriptions.Add(Sc);
                }
                else
                {
                    var q = Sc;
                    update = StockOutPrescriptions.FirstOrDefault(x => x.Id == q.Id)!;
                    var index = StockOutPrescriptions.IndexOf(update);
                    StockOutPrescriptions[index] = FormStockOutPrescriptions;
                }
                SelectedDataItemsStockOut = [];
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

        }

        private async Task OnSaveStockOutLines(GridEditModelSavingEventArgs e)
        {

            //if (Pharmacy.Id == 0)
            //{
            try
            {
                if (FormStockOutLines.CutStock == 0)
                    return;

                var Sc = (StockOutLinesDto)e.EditModel;
                StockOutLinesDto update = new();


                if (Sc.Id == 0)
                {
                    Sc.Id = Helper.RandomNumber;
                    Sc.LinesId = Lines_Id;
                    StockOutLines.Add(Sc);
                }
                else
                {
                    var q = SelectedDataItemsStockOut[0].Adapt<StockOutLinesDto>();
                    update = StockOutLines.FirstOrDefault(x => x.Id == q.Id)!;
                    var index = StockOutLines.IndexOf(update);
                    StockOutLines[index] = FormStockOutLines;
                }
                SelectedDataItemsStockOut = [];
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

        }


        private async Task OnSavePrescription(GridEditModelSavingEventArgs e)
        {
            if (e is null)
                return;

            var t = (PrescriptionDto)e.EditModel;

            if (Pharmacy.Id == 0)
            {
                try
                {
                    PrescriptionDto update = new();

                    var medicamentDetails = Medicaments.FirstOrDefault(s => s.ProductId == t.ProductId);
                    if (medicamentDetails != null)
                    {
                        if (medicamentDetails.Dosage != 0 && medicamentDetails.FrequencyId.HasValue)
                        {
                            t.DosageFrequency = $"{medicamentDetails.Dosage}/{medicamentDetails.Frequency?.Frequency}";
                        }
                    }

                    if (t.Id == 0)
                    {
                        t.Id = Helper.RandomNumber;
                        t.Product = Products.FirstOrDefault(x => x.Id == t.ProductId);
                        t.DrugRoute = DrugRoutes.FirstOrDefault(x => x.Id == t.DrugRouteId);
                        t.DrugDosage = DrugDosages.FirstOrDefault(x => x.Id == t.DrugDosageId);
                        t.ActiveComponentId = medicamentDetails.ActiveComponentId;
                        t.ActiveComponentNames = string.Join(",", ActiveComponents.Where(a => t.ActiveComponentId is not null && t.ActiveComponentId.Contains(a.Id)).Select(n => n.Name));
                        Prescriptions.Add(t);
                    }
                    else
                    {
                        var q = SelectedDataItemsPrescriptionLines[0].Adapt<PrescriptionDto>();

                        update = Prescriptions.FirstOrDefault(x => x.Id == q.Id)!;
                        t.Product = Products.FirstOrDefault(x => x.Id == t.ProductId);
                        t.DrugRoute = DrugRoutes.FirstOrDefault(x => x.Id == t.DrugRouteId);
                        t.DrugDosage = DrugDosages.FirstOrDefault(x => x.Id == t.DrugDosageId);

                        var index = Prescriptions.IndexOf(update!);
                        Prescriptions[index] = t;
                    }

                    SelectedDataItemsPrescriptionLines = [];
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                t.PharmacyId = Pharmacy.Id;
                if (t.Id == 0)
                    await Mediator.Send(new CreatePrescriptionRequest(t));
                else
                    await Mediator.Send(new UpdatePrescriptionRequest(t));

                await EditItemPharmacy_Click(Pharmacy);
                StateHasChanged();
            }
        }

        private PharmacyLogDto PharmaciesLog = new PharmacyLogDto();

        private async Task OnSavePharmacy()
        {
            try
            {
                if (Prescriptions.Count <= 0 && Concoctions.Count <= 0)
                {
                    ToastService.ShowInfo("Prescription or Concoction is not Null");
                    return;
                }
                var data_pharmacy = new PharmacyDto();
                var data_concoctions = new List<ConcoctionDto>();
                if (Pharmacy.Id == 0)
                {
                    Pharmacy.Status = EnumStatusPharmacy.Draft;
                    data_pharmacy = await Mediator.Send(new CreatePharmacyRequest(Pharmacy));
                    Prescriptions.ForEach(x =>
                    {
                        x.PharmacyId = data_pharmacy.Id;
                        x.Id = 0;
                    });
                    await Mediator.Send(new CreateListPrescriptionRequest(Prescriptions));

                    Concoctions.ForEach(x =>
                    {
                        x.PharmacyId = data_pharmacy.Id;
                        x.Id = 0;
                    });
                    data_concoctions = await Mediator.Send(new CreateListConcoctionRequest(Concoctions));

                    // Assign new ConcoctionId to ConcoctionLines and reset Ids
                    ConcoctionLines.ForEach(x =>
                    {
                        x.ConcoctionId = data_concoctions.FirstOrDefault()?.Id ?? 0;
                        x.Id = 0;
                    });

                    await Mediator.Send(new CreateListConcoctionLineRequest(ConcoctionLines));

                    PharmaciesLog.PharmacyId = data_pharmacy.Id;
                    PharmaciesLog.UserById = NameUser.Id;
                    PharmaciesLog.status = Pharmacy.Status;

                    await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));
                    ToastService.ShowSuccess("Add Data Success..");
                }
                else
                {
                    data_pharmacy = await Mediator.Send(new UpdatePharmacyRequest(Pharmacy));

                    if (Prescription_Name is not null)
                    {
                        if (Prescriptions_Data.Count > 0)
                        {
                            foreach (var items in Prescriptions_Data)
                            {
                                await Mediator.Send(new DeletePrescriptionRequest(items.Id));
                            }
                        }
                    }

                    if (Concoction_Name is not null)
                    {
                        if (Concoction_data.Count > 0)
                        {
                            foreach (var items in Concoction_data)
                            {
                                await Mediator.Send(new DeleteConcoctionRequest(items.Id));
                            }
                        }
                    }

                    if (ConcoctionLine_Name is not null)
                    {
                        if (ConcoctionLine_Data.Count > 0)
                        {
                            foreach (var items in ConcoctionLine_Data)
                            {
                                await Mediator.Send(new DeleteConcoctionLineRequest(items.Id));
                            }
                        }
                    }
                    ToastService.ShowSuccess("Update Data Success..");
                }

                await EditItemPharmacy_Click(data_pharmacy);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Fungsi Save

        #region NewItem Click

        private async Task NewItem_Click()
        {
            onShowForm = true;
            header = "Add Data Pharmacy";
            Pharmacy = new();
            Concoctions.Clear();
            Prescriptions.Clear();
        }

        private async Task NewItemPrescriptionLines_Click()
        {
            if (Pharmacy.PrescriptionLocationId == 0 || Pharmacy.PrescriptionLocationId == null)
            {
                ToastService.ClearCustomToasts();
                ToastService.ShowInfo("Location is Not Null");
                return;
            }
            await GridPrescriptionLines.StartEditNewRowAsync();
        }

        private void NewItemConcoction_Click()
        {
            if (Pharmacy.PrescriptionLocationId == 0 || Pharmacy.PrescriptionLocationId == null)
            {
                ToastService.ClearCustomToasts();
                ToastService.ShowInfo("Location is Not Null");
                return;
            }
            PopUpConcoctionDetail = true;
            Concoction = new();
            Concoction.Qty = 1;
            Concoction.QtyByDay = 1;
            Concoction.Days = 1;
            Concoction.TotalQty = 1;
            ConcoctionLines.Clear();
        }

        private async Task NewItemConcoctionLines_Click()
        {
            selectedActiveComponents = [];
            await GridConcoctionLines.StartEditNewRowAsync();
        }

        private async Task NewItemStockOut_Click()
        {
            selectedActiveComponents = [];
            var data_products = Prescriptions.Where(x => x.Id == PrescripId).Select(x => x.ProductId).FirstOrDefault();
            if (!traceAvailability)
            {
                var Qty_stock = TransactionStocks.Where(x => x.ProductId == data_products && x.LocationId == Pharmacy.PrescriptionLocationId).Select(x => x.Quantity).FirstOrDefault();
                FormStockOutLines.CurrentStock = Qty_stock;
            }

            var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == data_products && s.LocationId == Pharmacy.PrescriptionLocationId));

            Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
            Batch = Batch.Distinct().ToList();

            var firstStockProduct = stockProducts.Where(x => x.Batch == FormStockOutPrescriptions.Batch);
            UpdateFormInventoryAdjustmentDetail2(firstStockProduct.FirstOrDefault() ?? new(), firstStockProduct.Sum(x => x.Quantity));
            await GridStockOut.StartEditNewRowAsync();
        }

        private void UpdateFormInventoryAdjustmentDetail2(TransactionStockDto stockProduct, long qty)
        {
            if (stockProduct != null)
            {
                FormStockOutPrescriptions.uomId = stockProduct.UomId;
                FormStockOutPrescriptions.CurrentStock = qty;
                FormStockOutPrescriptions.ExpiredDate = stockProduct.ExpiredDate;
            }
        }

        private async Task NewItemStockOutLines_Click()
        {
            selectedActiveComponents = [];
            var data_products = ConcoctionLines.Where(x => x.Id == Lines_Id).Select(x => x.ProductId).FirstOrDefault();
            if (!traceAvailability)
            {
                var Qty_stock = TransactionStocks.Where(x => x.ProductId == data_products && x.LocationId == Pharmacy.PrescriptionLocationId).Select(x => x.Quantity).FirstOrDefault();
                FormStockOutLines.CurrentStock = Qty_stock;
            }

            var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == data_products && s.LocationId == Pharmacy.PrescriptionLocationId));

            Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
            Batch = Batch.Distinct().ToList();

            var firstStockProduct = stockProducts.Where(x => x.Batch == FormStockOutLines.Batch);
            UpdateFormInventoryAdjustmentDetail2(firstStockProduct.FirstOrDefault() ?? new(), firstStockProduct.Sum(x => x.Quantity));
            await GridStockOutLines.StartEditNewRowAsync();
        }

        #endregion NewItem Click

        #region function Edit Click

        private async Task EditItemPharmacy_Click(PharmacyDto? q = null)
        {

            try
            {
                PanelVisible = true;
                onShowForm = true;
                header = "Data Pharmacy";
                PharmacyDto? p = null;

                // Check if SelectedDataItems has at least one item
                if (q != null)
                {
                    p = q;
                }
                else if (SelectedDataItems != null && SelectedDataItems.Count > 0)
                {
                    p = SelectedDataItems[0].Adapt<PharmacyDto>();
                }
                else
                {
                    throw new InvalidOperationException("No pharmacy item selected or provided.");
                }

                // Check if the pharmacy status is "Draft"

                if (p.Status!.Equals(EnumStatusPharmacy.Draft) || ((p.Status!.Equals(EnumStatusPharmacy.SendToPharmacy) || p.Status!.Equals(EnumStatusPharmacy.Processed)) && NameUser.IsPharmacy == true))
                {
                    isActive = false;
                    isActiveButton = true;
                }
                else
                {
                    isActive = true;
                }

                Pharmacy = p;

                // Fetch related data
                await GetPatientAllergy(Pharmacy.PatientId);
                Prescriptions = await Mediator.Send(new GetPrescriptionQuery(x => x.PharmacyId == Pharmacy.Id));
                Concoctions = await Mediator.Send(new GetConcoctionQuery(x => x.PharmacyId == Pharmacy.Id));
                var ConcoctionId = Concoctions.FirstOrDefault()?.Id ?? 0;
                ConcoctionLines = await Mediator.Send(new GetConcoctionLineQuery(x => x.ConcoctionId == ConcoctionId));

                await LoadLogs();

                PanelVisible = false;
            }
            catch (Exception e)
            {
                // Handle the exception and provide feedback
                e.HandleException(ToastService);
                // Optionally log the error message
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private void EditItem_Click()
        {
            onShowForm = true;
            Pharmacy = new();
            SelectedDataItemsPrescriptionLines = [];
            Prescriptions = [];
        }

        private async Task EditItemPrescriptionLines_Click(IGrid context)
        {
            var selected = (PrescriptionDto)context.SelectedDataItem;
            var copy = selected.Adapt<LabTestDetailDto>();
            await GridPrescriptionLines.StartEditRowAsync(FocusedRowVisibleIndexPrescriptionLines);
            var w = Prescriptions.FirstOrDefault(x => x.Id == copy.Id);
        }

        private async Task EditItemPrescriptionConcoction_Click(IGrid grid)
        {
            PopUpConcoctionDetail = true;
            IsLoading = true;
            Concoction = (ConcoctionDto)grid.SelectedDataItem;
            var data_concoctionLine = ConcoctionLines.Where(x => x.ConcoctionId == Concoction.Id).ToList();
            foreach (var items in data_concoctionLine)
            {
                items.ActiveComponentName = string.Join(",", ActiveComponents.Where(x => items.ActiveComponentId.Contains(x.Id)).Select(z => z.Name));
            }

            IsLoading = false;
        }

        private async Task EditItemConcoctionLines_Click(IGrid context)
        {
            var selected = (ConcoctionLineDto)context.SelectedDataItem;
            await GridConcoctionLines.StartEditRowAsync(FocusedRowVisibleIndexConcoctionLines);
        }

        private async Task EditItemStockOut_Click(IGrid context)
        {
            //var selected = (StockOutPrescriptionDto)context.SelectedDataItem;
            await GridStockOut.StartEditRowAsync(FocusedRowVisibleIndexStockOut);
        }

        private async Task EditItemStockOutLines_Click(IGrid context)
        {
            await GridStockOutLines.StartEditRowAsync(FocusedRowVisibleIndexStockOutLines);
        }
        #endregion function Edit Click

        #region Refresh fuction

        private async Task Refresh_Click()
        {
            await LoadDataPharmacy();
        }

        private async Task RefreshPrescriptionLines_Click()
        {
        }

        private async Task RefreshPrescriptionConcoction_Click()
        {
            await LoadDataConcoction();
        }

        private async Task RefreshConcoctionLines_Click()
        {
        }

        #endregion Refresh fuction

        #region Status

        public async void SendToPharmacy()
        {
            try
            {
                var checkData = Pharmacies.Where(x => x.Id == Pharmacy.Id).FirstOrDefault();
                Pharmacy.Status = EnumStatusPharmacy.SendToPharmacy;
                await Mediator.Send(new UpdatePharmacyRequest(Pharmacy));

                PharmaciesLog.PharmacyId = Pharmacy.Id;
                PharmaciesLog.UserById = NameUser.Id;
                PharmaciesLog.status = EnumStatusPharmacy.SendToPharmacy;

                await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        public async void Pharmacied()
        {
            var checkData = Pharmacies.Where(x => x.Id == Pharmacy.Id).FirstOrDefault();
            Pharmacy.Status = EnumStatusPharmacy.Received;
            await Mediator.Send(new UpdatePharmacyRequest(Pharmacy));

            PharmaciesLog.PharmacyId = Pharmacy.Id;
            PharmaciesLog.UserById = NameUser.Id;
            PharmaciesLog.status = EnumStatusPharmacy.Received;

            await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));
        }
        public async void Received()
        {
            var checkData = Pharmacies.Where(x => x.Id == Pharmacy.Id).FirstOrDefault();

            //if (Prescriptions.Count > 0)
            //{
            //    var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(Prescription))
            //        .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
            //    int NextReferenceNumber = 1;
            //    if (cekReference != null)
            //    {
            //        int.TryParse(cekReference?.Substring("PHP#".Length), out NextReferenceNumber);
            //        NextReferenceNumber++;
            //    }
            //    string referenceNumber = $"PHP#{NextReferenceNumber:D3}";

            //    foreach (var item in Prescriptions)
            //    {
            //        var check_CutStock = StockOutPrescriptions.Where(x => x.PrescriptionId == item.Id).ToList();
            //        var productd = Products.Where(x => x.Id == item.ProductId).FirstOrDefault();
            //        foreach (var items in check_CutStock)
            //        {
            //            FormTransactionStock.ProductId = productd?.Id;
            //            FormTransactionStock.SourceTable = nameof(Prescription);
            //            FormTransactionStock.SourcTableId = items?.PrescriptionId;
            //            FormTransactionStock.LocationId = checkData?.PrescriptionLocationId;
            //            FormTransactionStock.ExpiredDate = items?.ExpiredDate;
            //            FormTransactionStock.Batch = items?.Batch;
            //            FormTransactionStock.Quantity = -(items?.CutStock) ?? 0;
            //            FormTransactionStock.Reference = referenceNumber;
            //            FormTransactionStock.UomId = productd?.UomId;
            //            FormTransactionStock.Validate = false;
            //            var datas = await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));


            //            items.TransactionStockId = datas.Id;
            //            await Mediator.Send(new UpdateStockOutPrescriptionRequest(items));

            //        }
            //    }
            //}

            //if (ConcoctionLines.Count > 0)
            //{
            //    var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(ConcoctionLine))
            //        .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
            //    int NextReferenceNumber = 1;
            //    if (cekReference != null)
            //    {
            //        int.TryParse(cekReference?.Substring("PHCL#".Length), out NextReferenceNumber);
            //        NextReferenceNumber++;
            //    }
            //    string referenceNumber = $"PHCL#{NextReferenceNumber:D3}";

            //    foreach (var item in ConcoctionLines)
            //    {
            //        var check_CutStock = StockOutLines.Where(x => x.LinesId == item.Id).ToList();
            //        var concoction_data = Concoctions.Where(x => x.Id == Lines_Id).FirstOrDefault();
            //        foreach (var items in check_CutStock)
            //        {
            //            FormTransactionStock.ProductId = items?.Lines?.ProductId;
            //            FormTransactionStock.SourceTable = nameof(ConcoctionLine);
            //            FormTransactionStock.SourcTableId = items?.LinesId;
            //            FormTransactionStock.LocationId = concoction_data?.Pharmacy?.PrescriptionLocationId;
            //            FormTransactionStock.ExpiredDate = items?.ExpiredDate;
            //            FormTransactionStock.Batch = items?.Batch;
            //            FormTransactionStock.Quantity = items?.CutStock ?? 0;
            //            FormTransactionStock.Reference = referenceNumber;
            //            FormTransactionStock.UomId = items?.Lines?.Product?.UomId;
            //            FormTransactionStock.Validate = false;
            //            var datas = await Mediator.Send(new CreateTransactionStockRequest(FormTransactionStock));

            //            items.TransactionStockId = datas.Id;
            //            await Mediator.Send(new UpdateStockOutLinesRequest(items));
            //        }
            //    }
            //}
            Pharmacy.Status = EnumStatusPharmacy.Processed;
            var updates = await Mediator.Send(new UpdatePharmacyRequest(Pharmacy));

            PharmaciesLog.PharmacyId = Pharmacy.Id;
            PharmaciesLog.UserById = NameUser.Id;
            PharmaciesLog.status = EnumStatusPharmacy.Processed;

            await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));

            await EditItemPharmacy_Click(updates);
            StateHasChanged();
        }

        public async Task ValidateAsync()
        {
            try
            {
                await LoadAsyncData();
                await LoadData();
                await RefreshData();

                StockOutPrescriptions = await Mediator.Send(new GetStockOutPrescriptionQuery());
                StockOutLines = await Mediator.Send(new GetStockOutLineQuery());
                var pharmacyData = Pharmacies.FirstOrDefault(x => x.Id == Pharmacy.Id);
                if (pharmacyData == null)
                {
                    return;
                }

                #region Prescription CutStock
                var prescriptions = Prescriptions.Where(x => x.PharmacyId == pharmacyData.Id).ToList();
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
                #endregion
                #region ConcoctionLine CutStock
                var dataConcoction = Concoctions.FirstOrDefault(x => x.PharmacyId == pharmacyData?.Id);
                if (dataConcoction != null)
                {
                    var dataLines = ConcoctionLines.Where(x => x.ConcoctionId == dataConcoction.Id).ToList();
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
                #endregion
                Pharmacy.Status = EnumStatusPharmacy.Done;
                var updatedPharmacy = await Mediator.Send(new UpdatePharmacyRequest(Pharmacy));

                var pharmacyLog = new PharmacyLogDto
                {
                    PharmacyId = pharmacyData.Id,
                    UserById = NameUser.Id,
                    status = EnumStatusPharmacy.Done
                };

                await Mediator.Send(new CreatePharmacyLogRequest(pharmacyLog));
                await EditItemPharmacy_Click(updatedPharmacy);
                StateHasChanged();
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
                var checkData = Pharmacies.Where(x => x.Id == Pharmacy.Id).FirstOrDefault();
                Pharmacy.Status = EnumStatusPharmacy.Cancel;
                await Mediator.Send(new UpdatePharmacyRequest(Pharmacy));

                PharmaciesLog.PharmacyId = Pharmacy.Id;
                PharmaciesLog.UserById = NameUser.Id;
                PharmaciesLog.status = EnumStatusPharmacy.Cancel;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Status

        #region Delete Grid Config

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void DeleteItemConcoctionLines_Click()
        {
            GridConcoctionLines.ShowRowDeleteConfirmation(FocusedRowVisibleIndexConcoctionLines);
        }

        private void DeleteItemStockOut_Click()
        {
            GridStockOut.ShowRowDeleteConfirmation(FocusedRowVisibleIndexStockOut);
        }
        private void DeleteItemStockOutLines_Click()
        {
            GridStockOutLines.ShowRowDeleteConfirmation(FocusedRowVisibleIndexStockOutLines);
        }

        private void DeleteItemPrescriptionLines_Click()
        {
            GridPrescriptionLines.ShowRowDeleteConfirmation(FocusedRowVisibleIndexPrescriptionLines);
        }

        private void DeleteItemPrescriptionConcoction_Click()
        {
            GridConcoction.ShowRowDeleteConfirmation(FocusedRowVisibleIndexPrescriptionConcoction);
        }

        #endregion Delete Grid Config

        private async Task OnDiscard()
        {
            await LoadDataPharmacy();
        }

        private void OnDiscardConcoctionLines()
        {
            PopUpConcoctionDetail = false;
        }

        #region Grid Properties

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void ColumnChooserButtonGridPrescriptionLines_Click()
        {
            GridPrescriptionLines.ShowColumnChooser();
        }

        private void ColumnChooserButtonConcoction_Click()
        {
            GridConcoction.ShowColumnChooser();
        }
        private void ColumnChooserButtonConcoctionLine_Click()
        {
            GridConcoctionLines.ShowColumnChooser();
        }


        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
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

        private void Grid_CustomizeFilterRowEditor(GridCustomizeFilterRowEditorEventArgs e)
        {
            if (e.FieldName == "CreatedDate" || e.FieldName == "ModifiedDate" || e.FieldName == "FixedDate")
                ((ITextEditSettings)e.EditSettings).ClearButtonDisplayMode = DataEditorClearButtonDisplayMode.Never;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void GridFocusedRowVisibleIndexPrescriptionLines_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexPrescriptionLines = args.VisibleIndex;
        }

        private bool IsDeletedPharmacy { get; set; }
        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;

            try
            {
                if ((PharmacyDto)args.DataItem is null)
                    return;

                IsDeletedPharmacy = !((PharmacyDto)args.DataItem)!.Status!.Equals(EnumStatusPharmacy.Done);
            }
            catch { }
        }

        private void GridFocusedRowVisibleIndexPrescriptionConcoction_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexPrescriptionConcoction = args.VisibleIndex;
        }

        private void GridFocusedRowVisibleIndexConcoctionLines_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexConcoctionLines = args.VisibleIndex;
        }

        private void GridFocusedRowVisibleIndexStockOut_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexStockOut = args.VisibleIndex;
        }
        private void GridFocusedRowVisibleIndexStockOutLines_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexStockOutLines = args.VisibleIndex;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {

            await EditItemPharmacy_Click(null);
        }

        #endregion Grid Properties
    }
}
using DocumentFormat.OpenXml.Office2010.Excel;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PharmacyCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PrescriptionCommand;

namespace McDermott.Web.Components.Pages.Pharmacies.CreatUpdatePrescription
{
    public partial class CreateUpdatePrescriptionPage
    {
       // #region UserLoginAndAccessRole

       // [Inject]
       // public UserInfoService UserInfoService { get; set; }

       // private GroupMenuDto UserAccessCRUID = new();
       // private User UserLogin { get; set; } = new();
       // private bool IsAccess = false;

       // protected override async Task OnAfterRenderAsync(bool firstRender)
       // {
       //     //await base.OnAfterRenderAsync(firstRender);

       //     //if (firstRender)
       //     //{
       //     //    try
       //     //    {
       //     //        await GetUserInfo();
       //     //        StateHasChanged();
       //     //    }
       //     //    catch { }

       //     //    await LoadData();
       //     //    StateHasChanged();

       //     //    try
       //     //    {
       //     //        if (Grid is not null)
       //     //        {
       //     //            await Grid.WaitForDataLoadAsync();
       //     //            Grid.ExpandGroupRow(1);
       //     //            await Grid.WaitForDataLoadAsync();
       //     //            Grid.ExpandGroupRow(2);
       //     //            StateHasChanged();
       //     //        }
       //     //    }
       //     //    catch { }

       //     //    await LoadAsyncData();
       //     //    StateHasChanged();
       //     //}
       // }

       // private async Task GetUserInfo()
       // {
       //     try
       //     {
       //         var user = await UserInfoService.GetUserInfo(ToastService);
       //         IsAccess = user.Item1;
       //         UserAccessCRUID = user.Item2;
       //         UserLogin = user.Item3;
       //     }
       //     catch { }
       // }

       // #endregion UserLoginAndAccessRole
       // #region relation Data

       // private PharmacyDto postPharmacy { get; set; } = new();
       // private StockOutPrescriptionDto postStockOutPrescriptions { get; set; } = new();
       // private PrescriptionDto Prescription { get; set; } = new();
       // private PatientAllergyDto PatientAllergy = new();
       // private PharmacyLogDto PharmaciesLog = new ();
       // private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
       // private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
       // private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];
       // private List<StockOutPrescriptionDto> ListStockOutPrescriptions { get; set; } = [];
       // private List<AllergyDto> WeatherAllergies = [];
       // private List<AllergyDto> FoodAllergies = [];
       // private List<AllergyDto> PharmacologyAllergies = [];
       // private List<PharmacyDto> Pharmacies { get; set; } = [];
       // private List<PrescriptionDto> Prescriptions { get; set; } = [];
       // private List<ConcoctionDto> Concoctions { get; set; } = [];
       // private List<ConcoctionLineDto> ConcoctionLines { get; set; } = [];
       // private List<StockOutPrescriptionDto> StockOutPrescriptions { get; set; } = [];
       // private List<StockOutLinesDto> StockOutLines { get; set; } = [];
       // private List<AllergyDto> allergies { get; set; } = [];
       // private List<PatientAllergyDto> PatientAllergies { get; set; } = [];
       // private List<PharmacyLogDto> Logs { get; set; } = [];
       // private List<UserDto> Patients { get; set; } = [];
       // private List<UserDto> Practitioners { get; set; } = [];
       // private List<UomDto> Uoms { get; set; } = [];
       // private List<LocationDto> Locations { get; set; } = [];
       // private List<ServiceDto> Services { get; set; } = [];
       // private List<ProductDto> Products { get; set; } = [];
       // private List<SignaDto> Signas { get; set; } = [];
       // private List<DrugDosageDto> DrugDosages { get; set; } = [];
       // private List<MedicamentDto> Medicaments { get; set; } = [];
       // private List<MedicamentGroupDto> MedicamentGroups { get; set; } = [];
       // private List<MedicamentGroupDto> MedicamentGroupsConcoction { get; set; } = [];
       // private List<DrugFormDto> DrugForms { get; set; } = [];
       // private List<ActiveComponentDto> ActiveComponentt { get; set; } = [];
       // private List<DrugRouteDto> DrugRoutes { get; set; } = [];
       // private List<TransactionStockDto> TransactionStocks { get; set; } = [];
       // private List<TransactionStockDto> StockOutProducts { get; set; } = [];
       // private List<ActiveComponentDto> ActiveComponents { get; set; } = [];
       // private TransactionStockDto FormTransactionStock { get; set; } = new();
       // private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];

       // #endregion relation Data

       // #region variable Static
       // [Parameter]
       // public long PatientId { get; set; }

       // private IGrid Grid { get; set; }
       // private IGrid GridPrescriptionLines { get; set; }
       // private IGrid GridConcoction { get; set; }
       // private IGrid GridCutStockOut { get; set; }
       // private bool PanelVisible { get; set; } = false;
       // private bool onShowForm { get; set; } = false;
       // private bool isActiveButton { get; set; } = false;
       // [Parameter]
       // public bool IsPopUpForm { get; set; } = false;
       // public bool FormValidationState { get; set; } = false;

       // private List<string> Batch = [];
       // private IReadOnlyList<object> SelectedDataItemsPrescriptionLines { get; set; } = [];
       // private IReadOnlyList<object> SelectedDataItemsConcoction { get; set; } = [];
       // private IReadOnlyList<object> SelectedDataItemsStockOut { get; set; } = [];

       // private int FocusedRowVisibleIndexPrescription { get; set; }
       // private int FocusedRowVisibleIndexConcoction { get; set; }
       // private int FocusedRowVisibleIndexCutStockOut { get; set; }

       // #region Enum
       // public MarkupString GetIssueStatusIconHtml(EnumStatusPharmacy? status)
       // {
       //     string priorityClass;
       //     string title;

       //     switch (status)
       //     {
       //         case EnumStatusPharmacy.Draft:
       //             priorityClass = "info";
       //             title = "Draft";
       //             break;

       //         case EnumStatusPharmacy.SendToPharmacy:
       //             priorityClass = "primary";
       //             title = "Pharmacy";
       //             break;

       //         case EnumStatusPharmacy.Received:
       //             priorityClass = "primary";
       //             title = "Received";
       //             break;

       //         case EnumStatusPharmacy.Processed:
       //             priorityClass = "warning";
       //             title = "Processed";
       //             break;

       //         case EnumStatusPharmacy.Done:
       //             priorityClass = "success";
       //             title = "Done";
       //             break;

       //         case EnumStatusPharmacy.Cancel:
       //             priorityClass = "danger";
       //             title = "Canceled";
       //             break;

       //         default:
       //             return new MarkupString("");
       //     }
       //     string html = $"<div class='row '><div class='col-3'>" +
       //                   $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

       //     return new MarkupString(html);
       // }

       // #endregion
       // #endregion

       // #region changeData

       // private List<GeneralConsultanServiceDto> generalConsultantService = [];
       // protected override async Task OnParametersSetAsync()
       // {
       //     await base.OnParametersSetAsync();
       //     if (PatientId == 0)
       //         return;

       //     generalConsultantService = (await Mediator.Send(new GetGeneralConsultanServicesQuery
       //     {
       //         Predicate = x => x.Id == PatientId
       //     })).Item1;
       //     if (generalConsultantService.Count == 0 || generalConsultantService is null)
       //         return;

       //     onShowForm = true;
       //     isActiveButton = true;
       //     postPharmacy.Status = EnumStatusPharmacy.Draft;

       //     postPharmacy.PatientId = generalConsultantService.FirstOrDefault()!.PatientId;
       //     postPharmacy.PractitionerId = generalConsultantService.FirstOrDefault()!.PratitionerId;
       //     postPharmacy.ServiceId = generalConsultantService.FirstOrDefault()!.ServiceId;
       //     postPharmacy.PaymentMethod = generalConsultantService.FirstOrDefault()!.Payment;
       //     var result = await Mediator.Send(new GetUserQuery2(
       //       x => x.IsPatient == true,
       //       searchTerm: "",
       //       includes: [],
       //       select: x => new User
       //       {
       //           Id = x.Id,
       //           Name = x.Name,
       //           NoRm = x.NoRm,
       //           Email = x.Email,
       //           MobilePhone = x.MobilePhone,
       //           Gender = x.Gender,
       //           DateOfBirth = x.DateOfBirth,
       //       }
       //   ));
       //     Patients = result.Item1;
       //     allergies = await Mediator.Send(new GetAllergyQuery());

       //     await GetPatientAllergy(postPharmacy.PatientId);
       // }

       // private async Task GetPatientAllergy(long? q = null)
       // {
       //     FoodAllergies.Clear();
       //     WeatherAllergies.Clear();
       //     PharmacologyAllergies.Clear();
       //     SelectedFoodAllergies = [];
       //     SelectedWeatherAllergies = [];
       //     SelectedPharmacologyAllergies = [];

       //     // Filter allergies by type
       //     FoodAllergies = allergies.Where(x => x.Type == "01").ToList();
       //     WeatherAllergies = allergies.Where(x => x.Type == "02").ToList();
       //     PharmacologyAllergies = allergies.Where(x => x.Type == "03").ToList();

       //     var p = Patients.FirstOrDefault(z => z.Id == postPharmacy.PatientId || z.Id == q);

       //     if (p is null || p.PatientAllergyIds is null)
       //         return;

       //     var Allergies = await Mediator.Send(new GetAllergyQuery(x => p.PatientAllergyIds.Contains(x.Id)));
       //     if (Allergies.Count > 0)
       //     {
       //         // Assuming you have another list of selected allergies IDs
       //         var selectedAllergyIds = Allergies.Where(x => x.Type == "01" || x.Type == "02" || x.Type == "03").Select(x => x.Id).ToList();

       //         // Select specific allergies by their IDs
       //         SelectedFoodAllergies = FoodAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
       //         SelectedWeatherAllergies = WeatherAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
       //         SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();

       //         if (SelectedFoodAllergies.Count() > 0)
       //             postPharmacy.IsFood = true;
       //         if (SelectedWeatherAllergies.Count() > 0)
       //             postPharmacy.IsWeather = true;
       //         if (SelectedPharmacologyAllergies.Count() > 0)
       //             postPharmacy.IsFarmacologi = true;
       //     }
       // }

       // private List<string> Payments = new List<string>
       // {
       //     "Personal",
       //     "Insurance",
       //     "BPJS"
       // };

       // public class StatusComparer : IComparer<EnumStatusPharmacy>
       // {
       //     private static readonly List<EnumStatusPharmacy> StatusOrder = new List<EnumStatusPharmacy> { EnumStatusPharmacy.Draft, EnumStatusPharmacy.SendToPharmacy, EnumStatusPharmacy.Received, EnumStatusPharmacy.Processed, EnumStatusPharmacy.Done, EnumStatusPharmacy.Cancel };

       //     public int Compare(EnumStatusPharmacy x, EnumStatusPharmacy y)
       //     {
       //         int indexX = StatusOrder.IndexOf(x);
       //         int indexY = StatusOrder.IndexOf(y);

       //         // Compare the indices
       //         return indexX.CompareTo(indexY);
       //     }
       // }
       // private void checkStock(long value)
       // {
       //     try
       //     {
       //         if (value == 0)
       //             return;

       //         var checkStock = TransactionStocks.Where(x => x.ProductId == Prescription.ProductId && x.LocationId == Pharmacy.LocationId && x.Quantity != 0).Sum(x => x.Quantity)!;
       //         if (value > checkStock)
       //         {
       //             ToastService.ShowInfo($"Total Stock is less than the requested quantity");
       //             isSavePrescription = false;
       //         }
       //         else
       //         {
       //             Prescription.GivenAmount = value;
       //             isSavePrescription = true;
       //             return;
       //         }
       //     }
       //     catch (Exception ex)
       //     {
       //         ex.HandleException(ToastService);
       //     }
       // }

       // private async Task ChangeProduct(ProductDto product)
       // {
       //     try
       //     {
       //         if (product is null)
       //             return;

       //         var ChekMedicament = await Mediator.Send(new GetSingleMedicamentQuery
       //         {
       //             Predicate = x => x.ProductId == product.Id
       //         });
       //         var checkUom = Uoms.Where(x => x.Id == ChekMedicament?.UomId).FirstOrDefault();

       //         ConcoctionLine.Dosage = ChekMedicament?.Dosage ?? 0;
       //         ConcoctionLine.MedicamentDosage = ChekMedicament?.Dosage ?? 0;
       //         ConcoctionLine.UomId = ChekMedicament?.UomId ?? null;

       //         if (ConcoctionLine.Dosage <= ConcoctionLine.MedicamentDosage)
       //         {
       //             ConcoctionLine.TotalQty = 1;
       //         }
       //         else
       //         {
       //             var temp = (ConcoctionLine.Dosage / ConcoctionLine.MedicamentDosage) + (ConcoctionLine.Dosage % ConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
       //             ConcoctionLine.TotalQty = temp * Concoction.ConcoctionQty;
       //         }
       //         selectedActiveComponents = ActiveComponents.Where(a => ChekMedicament != null && ChekMedicament.ActiveComponentId != null && ChekMedicament.ActiveComponentId.Contains(a.Id)).ToList();
       //         var aa = Pharmacy.LocationId;
       //         var checkStock = TransactionStocks.Where(x => x.ProductId == product.Id && x.LocationId == Pharmacy.LocationId && x.Validate == true).Sum(x => x.Quantity);
       //         ConcoctionLine.AvaliableQty = checkStock;
       //     }
       //     catch (Exception ex)
       //     {
       //         ex.HandleException(ToastService);
       //     }
       // }

       // private void ChangeTotalQtyInHeader(long value)
       // {
       //     Concoction.ConcoctionQty = value;
       //     if (ConcoctionLine.Dosage <= ConcoctionLine.MedicamentDosage)
       //     {
       //         ConcoctionLine.TotalQty = 1;
       //     }
       //     else
       //     {
       //         double temp = ((double)ConcoctionLine.Dosage / (double)ConcoctionLine.MedicamentDosage) * (double)value;
       //         ConcoctionLine.TotalQty = (long)Math.Ceiling(temp);
       //     }
       // }

       // private void ChangeTotalQtyMedicament(long value)
       // {
       //     //Convert Variabel
       //     if (value == 0)
       //         return;

       //     ConcoctionLine.Dosage = value;
       //     if (value <= ConcoctionLine.MedicamentDosage)
       //     {
       //         ConcoctionLine.TotalQty = 1;
       //     }
       //     else
       //     {
       //         if (ConcoctionLine.MedicamentDosage != 0)
       //         {
       //             double temp = ((double)value / (double)ConcoctionLine.MedicamentDosage) * (double)Concoction.ConcoctionQty;
       //             ConcoctionLine.TotalQty = (long)Math.Ceiling(temp);
       //         }
       //         else
       //         {
       //             ToastService.ShowInfo("Medicament Dosage Not Null!");
       //         }
       //     }
       // }

       // private void SelectedChangePractition(UserDto? user)
       // {
       //     MedicamentGroups = MedicamentGroups.Where(x => x.IsConcoction == false && x.PhycisianId == user?.Id).ToList();
       // }

       // private async Task SelectedMedicament(MedicamentGroupDto medicament)
       // {
       //     if (medicament is null)
       //         return;

       //     var a = await Mediator.Send(new GetMedicamentGroupDetailQuery());
       //     var details = a.Where(x => x.MedicamentGroupId == medicament.Id).ToList();
       //     List<PrescriptionDto> prescriptionsList = new();

       //     foreach (var item in details)
       //     {
       //         var checkProduct = Products.FirstOrDefault(p => p.Id == item.MedicamentId);
       //         if (checkProduct == null)
       //         {
       //             continue; // Skip if product is not found
       //         }

       //         var stock = TransactionStocks
       //             .Where(s => s.ProductId == checkProduct.Id && s.LocationId == Pharmacy.LocationId && s.Quantity != 0).Sum(x => x.Quantity);

       //         if (stock == 0)
       //         {
       //             stock = 0;
       //             ToastService.ClearCustomToasts();
       //             ToastService.ShowWarning($"The {checkProduct.Name} product is out of stock, please contact the pharmacy department!!");
       //         }

       //         var medicamentDetails = Medicaments.FirstOrDefault(s => s.ProductId == checkProduct.Id);
       //         if (medicamentDetails == null)
       //         {
       //             continue; // Skip if medicament details are not found
       //         }

       //         var newPrescription = new PrescriptionDto
       //         {
       //             ProductId = checkProduct.Id,
       //             ProductName = checkProduct.Name,
       //             Stock = stock
       //         };

       //         if (medicamentDetails.Dosage != 0 && medicamentDetails.FrequencyId.HasValue)
       //         {
       //             newPrescription.DrugDosageId = medicamentDetails.FrequencyId;
       //             newPrescription.Dosage = medicamentDetails.Dosage;
       //             newPrescription.DosageFrequency = $"{medicamentDetails.Dosage}/{medicamentDetails.Frequency?.Frequency}";
       //         }

       //         newPrescription.Id = Helper.RandomNumber;
       //         newPrescription.Product = medicamentDetails.Product;
       //         newPrescription.DrugRoute = medicamentDetails.Route;
       //         newPrescription.UomId = medicamentDetails.UomId;
       //         newPrescription.DrugRouteId = medicamentDetails.RouteId;
       //         newPrescription.PriceUnit = checkProduct.SalesPrice;
       //         newPrescription.DrugRoutName = medicamentDetails.Route?.Route;
       //         newPrescription.ActiveComponentId = medicamentDetails.ActiveComponentId;
       //         newPrescription.ActiveComponentNames = string.Join(",", ActiveComponents.Where(a => medicamentDetails?.ActiveComponentId is not null && medicamentDetails.ActiveComponentId.Contains(a.Id)).Select(n => n.Name));

       //         prescriptionsList.Add(newPrescription);
       //     }

       //     Prescriptions = prescriptionsList;
       // }

       // private bool isSavePrescription { get; set; } = false;

       // private async Task SelectedProductPrescriptions(ProductDto value)
       // {
       //     if (value is null)
       //     {
       //         selectedActiveComponentPrescriptions = [];

       //         Prescription.PriceUnit = null;
       //         Prescription.ActiveComponentId = null;
       //         Prescription.UomId = null;  // Set to null if you want to clear this field
       //         Prescription.DrugFromId = null;  // Set to null if you want to clear this field
       //         Prescription.Dosage = null;
       //         Prescription.DrugDosageId = null;
       //         Prescription.DrugRouteId = null;
       //         Prescription.Stock = null;

       //         return;
       //     }
       //     var checkMedicament = Medicaments.Where(x => x.ProductId == value.Id).FirstOrDefault();
       //     if (checkMedicament is not null)
       //     {
       //         Prescription.PriceUnit = value.Cost;
       //         Prescription.ActiveComponentId = checkMedicament?.ActiveComponentId;
       //         Prescription.UomId = checkMedicament?.UomId;
       //         Prescription.DrugFromId = checkMedicament?.FormId;
       //         Prescription.Dosage = checkMedicament?.Dosage;
       //         Prescription.DrugDosageId = checkMedicament?.FrequencyId;
       //         Prescription.DrugRouteId = checkMedicament?.RouteId;
       //         selectedActiveComponentPrescriptions = ActiveComponents.Where(z => checkMedicament.ActiveComponentId is not null && checkMedicament.ActiveComponentId.Contains(z.Id)).ToList();

       //         var checkStock = TransactionStocks.Where(x => x.ProductId == value.Id && x.LocationId == Pharmacy.LocationId && x.Quantity != 0 && x.Validate == true).Sum(x => x.Quantity);
       //         if (checkStock == 0)
       //         {
       //             Prescription.Stock = 0;
       //             ToastService.ShowWarning($"The {value.Name} product is out of stock, or choose another product!!");
       //             isSavePrescription = false;
       //         }
       //         else
       //         {
       //             Prescription.Stock = checkStock;
       //             isSavePrescription = true;
       //         }
       //     }
       //     else
       //     {
       //         Prescription = new PrescriptionDto();
       //         return;
       //     }
       // }

       // private async Task SelectedMedicamentGroupConcoction(MedicamentGroupDto value)
       // {
       //     try
       //     {
       //         if (value is null)
       //             return;

       //         var medicamentGroup = MedicamentGroupsConcoction.Where(x => x.Id == value.Id).FirstOrDefault();
       //         Concoction.MedicamenName = medicamentGroup?.Name;
       //         Concoction.DrugFormId = medicamentGroup?.FormDrugId;

       //         var a = await Mediator.Send(new GetMedicamentGroupDetailQuery());
       //         var data = a.Where(x => x.MedicamentGroupId == value?.Id).ToList();
       //         List<ConcoctionLineDto> concoctionLinesList = new();

       //         foreach (var item in data)
       //         {
       //             var checkProduct = Products.FirstOrDefault(x => x.Id == item.MedicamentId);
       //             if (checkProduct == null)
       //                 return;

       //             var stockProduct = TransactionStocks
       //                             .Where(x => x.ProductId == checkProduct.Id && x.LocationId is not null && x.LocationId == Pharmacy.LocationId && x.Validate == true)
       //                             .Sum(x => x.Quantity);

       //             var medicamentData = Medicaments.FirstOrDefault(x => x.ProductId == checkProduct.Id);
       //             if (medicamentData is null)
       //                 return;

       //             var newConcoctionLine = new ConcoctionLineDto
       //             {
       //                 ProductId = checkProduct.Id,
       //                 ProductName = checkProduct.Name,
       //             };
       //             float? QtyPerDay = 0;
       //             if (medicamentData.FrequencyId != null)
       //             {
       //                 QtyPerDay = medicamentData?.Frequency?.TotalQtyPerDay;
       //             }
       //             else
       //             {
       //                 QtyPerDay = 0;
       //             }
       //             newConcoctionLine.Id = Helper.RandomNumber;
       //             newConcoctionLine.MedicamentDosage = medicamentData!.Dosage;
       //             newConcoctionLine.ActiveComponentId = medicamentData?.ActiveComponentId;
       //             newConcoctionLine.Dosage = medicamentData!.Dosage;
       //             newConcoctionLine.UomId = medicamentData?.UomId;
       //             newConcoctionLine.UomName = medicamentData?.Uom?.Name;

       //             if (newConcoctionLine.Dosage <= newConcoctionLine.MedicamentDosage)
       //             {
       //                 newConcoctionLine.TotalQty = 1;
       //             }
       //             else
       //             {
       //                 var temp = (newConcoctionLine.Dosage / newConcoctionLine.MedicamentDosage) + (newConcoctionLine.Dosage % newConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
       //                 newConcoctionLine.TotalQty = temp * Concoction.ConcoctionQty;
       //             }
       //             if (stockProduct == 0)
       //             {
       //                 newConcoctionLine.AvaliableQty = 0;
       //             }
       //             else
       //             {
       //                 newConcoctionLine.AvaliableQty = stockProduct;
       //             }
       //             newConcoctionLine.ActiveComponentName = string.Join(",", ActiveComponents.Where(a => medicamentData?.ActiveComponentId is not null && medicamentData.ActiveComponentId.Contains(a.Id)).Select(n => n.Name));
       //             concoctionLinesList.Add(newConcoctionLine);
       //         }

       //         ConcoctionLines = concoctionLinesList;
       //     }
       //     catch (Exception ex)
       //     {
       //         ex.HandleException(ToastService);
       //     }
       // }
       // #endregion

       // #region Click_Button
       // private async Task NewItemPrescription_Click()
       // {
       //     if (postPharmacy.LocationId == 0 || postPharmacy.LocationId == null)
       //     {
       //         ToastService.ClearCustomToasts();
       //         ToastService.ShowInfo("Location is Not Null");
       //         return;
       //     }
       //     await GridPrescriptionLines.StartEditNewRowAsync();
       // }

       // private void NewItemConcoction_Click()
       // {
       //     //    if (Pharmacy.LocationId == 0 || Pharmacy.LocationId == null)
       //     //    {
       //     //        ToastService.ClearCustomToasts();
       //     //        ToastService.ShowInfo("Location is Not Null");
       //     //        return;
       //     //    }
       //     //    PopUpConcoctionDetail = true;
       //     //    Concoction = new();
       //     //    Concoction.ConcoctionQty = 1;
       //     //    ConcoctionLines.Clear();
       //     return;
       // }

       // #endregion

       // #region Save

       // private async Task HandleValidSubmit()
       // {
       //     FormValidationState = true;

       //     //if (FormValidationState)
       //         //await OnSavePharmacy();
       // }

       // private async Task HandleInvalidSubmit()
       // {
       //     FormValidationState = true;
       //     ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
       // }
       // #endregion

       // #region Status

       // public async void SendToPharmacy()
       // {
       //     try
       //     {
       //         PanelVisible = true;
       //         var checkData = Pharmacies.Where(x => x.Id == postPharmacy.Id).FirstOrDefault();
       //         postPharmacy.Status = EnumStatusPharmacy.SendToPharmacy;
       //         var updates = await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

       //         PharmaciesLog.PharmacyId = postPharmacy.Id;
       //         PharmaciesLog.UserById = UserLogin.Id;
       //         PharmaciesLog.status = EnumStatusPharmacy.SendToPharmacy;

       //         PanelVisible = false;
       //         await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));
       //     }
       //     catch (Exception ex)
       //     {
       //         ex.HandleException(ToastService);
       //     }
       // }

       // public async void Pharmacied()
       // {
       //     PanelVisible = true;
       //     var checkData = Pharmacies.Where(x => x.Id == postPharmacy.Id).FirstOrDefault();
       //     postPharmacy.Status = EnumStatusPharmacy.Received;
       //     var updates = await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

       //     PharmaciesLog.PharmacyId = postPharmacy.Id;
       //     PharmaciesLog.UserById = UserLogin.Id;
       //     PharmaciesLog.status = EnumStatusPharmacy.Received;

       //     PanelVisible = false;
       //     await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));
       // }

       // public async void Received()
       // {
            
       //     var checkData = Pharmacies.Where(x => x.Id == postPharmacy.Id).FirstOrDefault();

       //     postPharmacy.Status = EnumStatusPharmacy.Processed;
       //     var updates = await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

       //     PharmaciesLog.PharmacyId = postPharmacy.Id;
       //     PharmaciesLog.UserById = UserLogin.Id;
       //     PharmaciesLog.status = EnumStatusPharmacy.Processed;

       //     await Mediator.Send(new CreatePharmacyLogRequest(PharmaciesLog));

            
       // }

       // public async Task ValidateAsync()
       // {
       //     try
       //     {
       //         StockOutPrescriptions = await Mediator.Send(new GetStockOutPrescriptionQuery());
       //         StockOutLines = await Mediator.Send(new GetStockOutLineQuery());
       //         var pharmacyData = Pharmacies.FirstOrDefault(x => x.Id == postPharmacy.Id);
       //         if (pharmacyData == null)
       //         {
       //             return;
       //         }

       //         #region Prescription CutStock

       //         var prescriptions = Prescriptions.Where(x => x.PharmacyId == pharmacyData.Id).ToList();
       //         if (prescriptions.Count > 0)
       //         {
       //             foreach (var prescription in prescriptions)
       //             {
       //                 var stockProducts = TransactionStocks.Where(x => x.SourceTable == nameof(Prescription) && x.SourcTableId == prescription.Id).ToList();
       //                 if (stockProducts.Count > 0)
       //                 {
       //                     foreach (var item in stockProducts)
       //                     {
       //                         item.Validate = true;
       //                         await Mediator.Send(new UpdateTransactionStockRequest(item));
       //                     }
       //                 }
       //             }
       //         }

       //         #endregion Prescription CutStock

       //         #region ConcoctionLine CutStock

       //         var dataConcoction = Concoctions.FirstOrDefault(x => x.PharmacyId == pharmacyData?.Id);
       //         if (dataConcoction != null)
       //         {
       //             var dataLines = ConcoctionLines.Where(x => x.ConcoctionId == dataConcoction.Id).ToList();
       //             if (dataLines.Count > 0)
       //             {
       //                 foreach (var line in dataLines)
       //                 {
       //                     var stockProducts = TransactionStocks.Where(x => x.SourceTable == nameof(ConcoctionLine) && x.SourcTableId == line.Id).ToList();
       //                     if (stockProducts.Count > 0)
       //                     {
       //                         foreach (var item in stockProducts)
       //                         {
       //                             item.Validate = true;
       //                             await Mediator.Send(new UpdateTransactionStockRequest(item));
       //                         }
       //                     }
       //                 }
       //             }
       //         }

       //         #endregion ConcoctionLine CutStock

       //         postPharmacy.Status = EnumStatusPharmacy.Done;
       //         var updatedPharmacy = await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

       //         var pharmacyLog = new PharmacyLogDto
       //         {
       //             PharmacyId = pharmacyData.Id,
       //             UserById = UserLogin.Id,
       //             status = EnumStatusPharmacy.Done
       //         };

       //         await Mediator.Send(new CreatePharmacyLogRequest(pharmacyLog));

                
       //     }
       //     catch (Exception ex)
       //     {
       //         ex.HandleException(ToastService);
       //     }
       // }

       // public async void onCancel()
       // {
       //     try
       //     {
       //         var checkData = Pharmacies.Where(x => x.Id == postPharmacy.Id).FirstOrDefault();
       //         postPharmacy.Status = EnumStatusPharmacy.Cancel;
       //         await Mediator.Send(new UpdatePharmacyRequest(postPharmacy));

       //         PharmaciesLog.PharmacyId = postPharmacy.Id;
       //         PharmaciesLog.UserById = UserLogin.Id;
       //         PharmaciesLog.status = EnumStatusPharmacy.Cancel;
       //     }
       //     catch (Exception ex)
       //     {
       //         ex.HandleException(ToastService);
       //     }
       // }

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
       // private async Task OnDiscard()
       // {
       //     //await LoadDataPharmacy();
       // }
       // #endregion Status

    }
}

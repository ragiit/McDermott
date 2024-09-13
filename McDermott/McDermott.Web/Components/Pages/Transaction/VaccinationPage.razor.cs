using DocumentFormat.OpenXml.Office2010.Excel;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.Security.Policy;
using static McDermott.Application.Features.Commands.Employee.SickLeaveCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Transaction.VaccinationPlanCommand;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class VaccinationPage
    {
        [Parameter]
        public string? PageMode { get; set; }

        [Parameter]
        public long? Id { get; set; }

        private class InputCPPTGeneralConsultanCPPT
        {
            public string? Subjective { get; set; }
            public string? Objective { get; set; }

            [DisplayName("Planning")]
            public string? Plan { get; set; }

            public string? Diagnosis { get; set; }

            [DisplayName("Nurse Diagnosis")]
            public string? NursingDiagnosis { get; set; }

            public DateTime Date { get; set; } = DateTime.Now;
        }

        private InputCPPTGeneralConsultanCPPT FormInputCPPTGeneralConsultan = new();
        private DiagnosisDto SelectedDiagnosis { get; set; } = new();
        private IGrid? GridCppt { get; set; }
        private IReadOnlyList<object> SelectedDataItemsCPPT { get; set; } = [];
        private NursingDiagnosesDto SelectedNursingDiagnosis { get; set; } = new();

        public static List<string> GetPropertyNames<T>(T obj)
        {
            List<string> propertyNames = new List<string>();
            Type type = typeof(T);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                propertyNames.Add(prop.Name);
            }

            return propertyNames;
        }

        private void OnDeleteTabCPPTConfirm(GridDataItemDeletingEventArgs e)
        {
            GeneralConsultanCPPTs.Remove((GeneralConsultanCPPTDto)e.DataItem);
            GridCppt.Reload();
        }

        private async Task OnClickSaveCPPT()
        {
            try
            {
                await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(deleteByGeneralServiceId: GeneralConsultanService.Id));

                GeneralConsultanCPPTs.ForEach(x => { x.GeneralConsultanService = null; x.GeneralConsultanServiceId = GeneralConsultanService.Id; x.Id = 0; });
                GeneralConsultanCPPTs = await Mediator.Send(new CreateListGeneralConsultanCPPTRequest(GeneralConsultanCPPTs));

                ToastService.ShowSuccess("Successfully Saving CPPT");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private int FocusedGridTabCPPTRowVisibleIndex { get; set; }

        private void GridTabCPPT_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;
        }

        private void OnClickConfirmCPPT()
        {
            var temps = new List<GeneralConsultanCPPTDto>();
            temps.Add(new GeneralConsultanCPPTDto
            {
                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                Title = UserLogin.Name,
            });
            temps.Add(new GeneralConsultanCPPTDto
            {
                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                Title = "Date and Time",
                Body = DateTime.Now.ToString("dd-MMM-yyy HH:mm:tt")
            });

            foreach (var key in GetPropertyNames(new InputCPPTGeneralConsultanCPPT()))
            {
                if (key == "Date")
                    continue;

                PropertyInfo property = typeof(InputCPPTGeneralConsultanCPPT).GetProperty(key);
                object? value = property?.GetValue(FormInputCPPTGeneralConsultan);

                if (value != null)
                {
                    string title = key;

                    if (title.Equals("Plan"))
                        title = "Planning";
                    else if (title.Equals("NursingDiagnosis"))
                        title = "Nurse Diagnosis";

                    temps.Add(new GeneralConsultanCPPTDto
                    {
                        Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                        Title = title,
                        Body = (value is null ? "" : value.ToString()) ?? "",
                    });

                    if (title.Equals("Planning"))
                    {
                        if (IsStatus(EnumStatusGeneralConsultantService.NurseStation))
                        {
                            temps.Add(new GeneralConsultanCPPTDto
                            {
                                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                                Title = "Nursing Diagnosis",
                                Body = SelectedNursingDiagnosis.Problem,
                            });
                        }
                        else
                        {
                            temps.Add(new GeneralConsultanCPPTDto
                            {
                                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                                Title = "Diagnosis",
                                Body = SelectedDiagnosis.Name,
                            });
                        }
                    }
                }
            }

            GeneralConsultanCPPTs.AddRange(temps);

            GridCppt.Reload();
            OnClickClearCPPT();
        }

        private void OnClickGetObjectives()
        {
            FormInputCPPTGeneralConsultan.Objective = $"Weight: {GeneralConsultanService.Weight}, Height: {GeneralConsultanService.Height}, RR: {GeneralConsultanService.RR}, SpO2: {GeneralConsultanService.SpO2}, BMIIndex: {Math.Round(GeneralConsultanService.BMIIndex, 2).ToString()}, BMIState: {GeneralConsultanService.BMIState}, Temp: {GeneralConsultanService.Temp}, HR: {GeneralConsultanService.HR}, Systolic: {GeneralConsultanService.Systolic}, DiastolicBP: {GeneralConsultanService.DiastolicBP}, E: {GeneralConsultanService.E}, V: {GeneralConsultanService.V}, M: {GeneralConsultanService.M}";
        }

        private void OnClickClearAllCPPT()
        {
            GeneralConsultanCPPTs.Clear();
        }

        private void OnClickClearCPPT()
        {
            FormInputCPPTGeneralConsultan = new InputCPPTGeneralConsultanCPPT();
            SelectedDiagnosis = new();
            SelectedNursingDiagnosis = new();
        }

        private bool ShowForm { get; set; } = false;
        private bool PanelVisible { get; set; } = false;
        private bool IsLoadingPage { get; set; } = true;
        private bool IsDeletedConsultantService { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IGrid? Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private GeneralConsultanServiceDto GeneralConsultanService { get; set; } = new();
        private List<GeneralConsultanServiceDto> GeneralConsultanServices { get; set; } = new();
        private List<ServiceDto> Services { get; set; } = [];
        private List<ClassTypeDto> ClassTypes = [];
        private List<UserDto> Physicions { get; set; } = [];
        private List<UserDto> SalesPerson { get; set; } = [];
        private List<UserDto> Educators { get; set; } = [];
        private List<UserDto> Patients { get; set; } = [];
        private UserDto UserForm = new();
        private List<InsurancePolicyDto> InsurancePolicies { get; set; } = [];
        private InsurancePolicyDto SelectedInsurancePolicy { get; set; } = new();
        private BPJSIntegrationDto SelectedBPJSIntegration { get; set; } = new();

        private List<DiagnosisDto> Diagnoses = [];
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs = [];
        private List<NursingDiagnosesDto> NursingDiagnoses = [];
        private List<AllergyDto> Allergies = [];
        private List<AwarenessDto> Awareness { get; set; } = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];
        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];

        #region Vaccination Tabs

        private async Task OnClickCancelGiveVaccine(VaccinationPlanDto e)
        {
            var s = (await Mediator.Send(new GetVaccinationPlanQuery(x => x.Id == e.Id))).FirstOrDefault();
            if (s is not null)
            {
                s.Status = EnumStatusVaccination.Scheduled;
                s.GeneralConsultanServiceId = null;
                await Mediator.Send(new UpdateVaccinationPlanRequest(s));
                await LoadVaccinationGivens();
            }
        }

        private async Task OnClickGiveVaccine(VaccinationPlanDto e)
        {
            var s = (await Mediator.Send(new GetVaccinationPlanQuery(x => x.Id == e.Id))).FirstOrDefault();
            if (s is not null)
            {
                s.Status = EnumStatusVaccination.InProgress;
                s.GeneralConsultanServiceId = GeneralConsultanService.Id;
                await Mediator.Send(new UpdateVaccinationPlanRequest(s));
                await LoadVaccinationPlans();
            }
        }

        private List<ProductDto> Products = [];
        private List<LocationDto> Locations = [];
        private VaccinationPlanDto VaccinationPlan { get; set; } = new();
        private VaccinationPlanDto VaccinationGiven { get; set; } = new();
        private List<VaccinationPlanDto> VaccinationPlans { get; set; } = [];
        private List<VaccinationPlanDto> VaccinationGivens { get; set; } = [];
        private List<VaccinationPlanDto> VaccinationHistoryGivens { get; set; } = [];
        private IGrid GridVaccinationPlan { get; set; }
        private IGrid GridVaccinationGiven { get; set; }
        private IGrid GridVaccinationHistoryGiven { get; set; }
        private IReadOnlyList<object> SelectedDataVaccinationPlanItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataVaccinationGivenItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataVaccinationHistoryGivenItems { get; set; } = [];
        private List<string> Batch = [];

        private async Task SelectedBatchGiven(string stockProduct)
        {
            VaccinationGiven.TeoriticalQty = 0;

            if (stockProduct is null)
            {
                return;
            }

            VaccinationGiven.Batch = stockProduct;

            if (VaccinationGiven.ProductId != 0)
            {
                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == VaccinationGiven.ProductId &&
                    s.LocationId == GeneralConsultanService.LocationId &&
                    s.Validate == true
                ));

                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == VaccinationGiven.ProductId
                && x.LocationId == GeneralConsultanService.LocationId && x.Batch == VaccinationGiven.Batch));

                VaccinationGiven.TeoriticalQty = aa.Sum(x => x.Quantity);
            }
        }

        private async Task SelectedBatchPlan(string stockProduct)
        {
            VaccinationPlan.TeoriticalQty = 0;

            if (stockProduct is null)
            {
                return;
            }

            VaccinationPlan.Batch = stockProduct;

            if (VaccinationPlan.ProductId != 0)
            {
                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == VaccinationPlan.ProductId &&
                    s.LocationId == GeneralConsultanService.LocationId &&
                    s.Validate == true
                ));

                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == VaccinationPlan.ProductId
                && x.LocationId == GeneralConsultanService.LocationId && x.Batch == VaccinationPlan.Batch));

                VaccinationPlan.TeoriticalQty = aa.Sum(x => x.Quantity);
            }
        }

        private async Task OnSelectProduct(ProductDto e)
        {
            try
            {
                Batch.Clear();

                if (e == null) return;

                var stockProducts2 = (await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == GeneralConsultanService.LocationId)) ?? []);
                if (e.TraceAbility)
                {
                    Batch = stockProducts2?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();
                }
                else
                {
                }

                return;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SaveVaccinationPlan(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (GeneralConsultanService.PatientId is null)
                {
                    ToastService.ShowInfo("Please select the Patient");
                    e.Cancel = true;
                    return;
                }

                if (GeneralConsultanService.LocationId is null)
                {
                    ToastService.ShowInfo("Please select the Location");
                    e.Cancel = true;
                    return;
                }

                if (VaccinationPlan.TeoriticalQty <= 0)
                {
                    ToastService.ShowInfo("The Theoretical quantity must be greater than zero. Please adjust the quantity and try again.");
                    e.Cancel = true;
                    return;
                }

                if (VaccinationPlan.Quantity > VaccinationPlan.TeoriticalQty)
                {
                    ToastService.ShowInfo("The quantity cannot exceed the theoretical quantity. Please enter a valid amount and try again.");
                    e.Cancel = true;
                    return;
                }

                VaccinationPlan.PatientId = GeneralConsultanService.PatientId.GetValueOrDefault();

                if (VaccinationPlan.Id == 0)
                    await Mediator.Send(new CreateVaccinationPlanRequest(VaccinationPlan));
                else
                    await Mediator.Send(new UpdateVaccinationPlanRequest(VaccinationPlan));

                await LoadVaccinationPlans();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SaveVaccinationGiven(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (GeneralConsultanService.PatientId is null)
                {
                    ToastService.ShowInfo("Please select the Patient");
                    e.Cancel = true;
                    return;
                }

                if (GeneralConsultanService.LocationId is null)
                {
                    ToastService.ShowInfo("Please select the Location");
                    e.Cancel = true;
                    return;
                }

                if (VaccinationGiven.TeoriticalQty <= 0)
                {
                    ToastService.ShowInfo("The Theoretical quantity must be greater than zero. Please adjust the quantity and try again.");
                    e.Cancel = true;
                    return;
                }

                if (VaccinationGiven.Quantity > VaccinationGiven.TeoriticalQty)
                {
                    ToastService.ShowInfo("The quantity cannot exceed the theoretical quantity. Please enter a valid amount and try again.");
                    e.Cancel = true;
                    return;
                }

                VaccinationGiven.PatientId = GeneralConsultanService.PatientId.GetValueOrDefault();
                VaccinationGiven.GeneralConsultanServiceId = GeneralConsultanService.Id;
                VaccinationGiven.Status = EnumStatusVaccination.InProgress;

                if (VaccinationGiven.Id == 0)
                    await Mediator.Send(new CreateVaccinationPlanRequest(VaccinationGiven));
                else
                    await Mediator.Send(new UpdateVaccinationPlanRequest(VaccinationGiven));

                await LoadVaccinationGivens();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task NewItemDetailVaccinationGiven_Click()
        {
            if (GeneralConsultanService.PatientId == null || GeneralConsultanService.PatientId == 0)
            {
                ToastService.ClearInfoToasts();
                ToastService.ShowInfo("Please select the Patient first");
                return;
            }

            await GridVaccinationGiven.StartEditNewRowAsync();
        }

        private async Task NewItemDetailVaccinationPlan_Click()
        {
            if (GeneralConsultanService.PatientId == null || GeneralConsultanService.PatientId == 0)
            {
                ToastService.ClearInfoToasts();
                ToastService.ShowInfo("Please select the Patient first");
                return;
            }

            await GridVaccinationPlan.StartEditNewRowAsync();
        }

        private async Task EditItemDetailVaccinationGiven_Click(IGrid context)
        {
            // Ensure the context is not null and has selected data item
            if (context.SelectedDataItem != null)
            {
                VaccinationGiven = (await Mediator.Send(new GetVaccinationPlanQuery(x => x.Id == context.SelectedDataItem.Adapt<VaccinationPlanDto>().Id))).FirstOrDefault() ?? new();
                try
                {
                    var stockProducts2 = (await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == VaccinationGiven.ProductId)) ?? []);
                    if (VaccinationGiven.Product?.TraceAbility ?? false)
                    {
                        Batch = stockProducts2?.Select(x => x.Batch)?.ToList() ?? [];
                        Batch = Batch.Distinct().ToList();
                    }

                    await SelectedBatchGiven(VaccinationGiven.Batch);
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
                StateHasChanged();
            }

            await GridVaccinationGiven.StartEditRowAsync(FocusedRowVisibleIndexVaccinationGiven);
        }

        private async Task EditItemDetailVaccinationPlan_Click(IGrid context)
        {
            // Ensure the context is not null and has selected data item
            if (context.SelectedDataItem != null)
            {
                VaccinationPlan = (await Mediator.Send(new GetVaccinationPlanQuery(x => x.Id == context.SelectedDataItem.Adapt<VaccinationPlanDto>().Id))).FirstOrDefault() ?? new();

                await SelectedBatchPlan(VaccinationGiven.Batch);

                StateHasChanged();
            }

            await GridVaccinationPlan.StartEditRowAsync(FocusedRowVisibleIndexVaccinationPlan);
        }

        private async Task NewItemDetailGiven_Click()
        {
            //if (InventoryAdjusment.LocationId is null || InventoryAdjusment.LocationId == 0)
            //{
            //    ToastService.ClearInfoToasts();
            //    ToastService.ShowInfo("Please select the Location first.");
            //    return;
            //}

            //Products = await Mediator.Send(new GetProductQuery());
            //AllProducts = Products.Select(x => x).ToList();

            //FormInventoryAdjusmentDetail = new();
            //TotalQty = 0;
            //LotSerialNumber = "-";
            //UomId = null;

            //await GridDetail.StartEditNewRowAsync();
        }

        private int FocusedRowVisibleIndexVaccinationPlan { get; set; }
        private int FocusedRowVisibleIndexVaccinationGiven { get; set; }

        private void GridDetailVaccinationPlan_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexVaccinationPlan = args.VisibleIndex;
        }

        private void GridDetailVaccinationGiven_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexVaccinationGiven = args.VisibleIndex;
        }

        private bool IsLoadingVaccinationPlan = false;
        private bool IsLoadingVaccinationGiven = false;
        private bool IsLoadingVaccinationHistoryGiven = false;

        private async Task LoadVaccinationHistoryGivens()
        {
            IsLoadingVaccinationHistoryGiven = true;
            VaccinationHistoryGivens = await Mediator.Send(new GetVaccinationPlanQuery(x => x.PatientId == GeneralConsultanService.PatientId && x.Status == EnumStatusVaccination.Done && (x.GeneralConsultanServiceId != 0 || x.GeneralConsultanServiceId != null)));
            IsLoadingVaccinationHistoryGiven = false;
        }

        private async Task LoadVaccinationPlans()
        {
            IsLoadingVaccinationPlan = true;
            VaccinationPlans = await Mediator.Send(new GetVaccinationPlanQuery(x => x.PatientId == GeneralConsultanService.PatientId && (x.GeneralConsultanServiceId == 0 || x.GeneralConsultanServiceId == null)));
            IsLoadingVaccinationPlan = false;
        }

        private async Task LoadVaccinationGivens()
        {
            IsLoadingVaccinationGiven = true;
            VaccinationGivens = await Mediator.Send(new GetVaccinationPlanQuery(x => x.PatientId == GeneralConsultanService.PatientId && x.GeneralConsultanServiceId != null && x.GeneralConsultanServiceId == GeneralConsultanService.Id));
            IsLoadingVaccinationGiven = false;
        }

        private async Task OnDeleteVaccinationGiven(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataVaccinationGivenItems is null || SelectedDataVaccinationGivenItems.Count == 1)
                {
                    await Mediator.Send(new DeleteVaccinationPlanRequest(((VaccinationPlanDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataVaccinationGivenItems.Adapt<List<VaccinationPlanDto>>();
                    await Mediator.Send(new DeleteVaccinationPlanRequest(ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataVaccinationGivenItems = [];
                await LoadVaccinationGivens();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnDeleteVaccinationPlan(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataVaccinationPlanItems is null || SelectedDataVaccinationPlanItems.Count == 1)
                {
                    await Mediator.Send(new DeleteVaccinationPlanRequest(((VaccinationPlanDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataVaccinationPlanItems.Adapt<List<VaccinationPlanDto>>();
                    await Mediator.Send(new DeleteVaccinationPlanRequest(ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataVaccinationPlanItems = [];
                await LoadVaccinationPlans();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Vaccination Tabs

        public List<string> InformationFrom =
        [
            "Auto Anamnesa",
            "Allo Anamnesa"
        ];

        private List<string> YesNoOptions =
      [
          "Yes",
    "No"
      ];

        private List<string> RiwayatPenyakitKeluarga =
           [
           "DM",
    "Hipertensi",
    "Cancer",
    "Jantung",
    "TBC",
    "Anemia",
    "Other",
    ];

        private Task OnSelectedFoodAllergiesChanged(IEnumerable<AllergyDto> selectedFoodAllergies)
        {
            SelectedFoodAllergies = selectedFoodAllergies;
            return Task.CompletedTask;
        }

        private Task OnSelectedPharmacologyAllergiesChanged(IEnumerable<AllergyDto> selectedPharmacologyAllergies)
        {
            SelectedPharmacologyAllergies = selectedPharmacologyAllergies;
            return Task.CompletedTask;
        }

        private Task OnSelectedWeatherAllergiesChanged(IEnumerable<AllergyDto> selectedWeatherAllergies)
        {
            SelectedWeatherAllergies = selectedWeatherAllergies;
            return Task.CompletedTask;
        }

        private PatientAllergyDto PatientAllergy = new();
        private bool IsLoading { get; set; } = false;

        private bool IsStatus(EnumStatusGeneralConsultantService status) => GeneralConsultanService.Status == status;

        private EnumStatusGeneralConsultantService StagingText { get; set; } = EnumStatusGeneralConsultantService.Confirmed;

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService? UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess { get; set; } = false;

        private async Task GetUserInfo()
        {
            try
            {
                var user = await UserInfoService?.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        #region Methods

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await GetUserInfo();
        }

        private async Task LoadDataByIdAsync()
        {
        }

        private string Url => "clinic-service/vaccinations" ?? string.Empty;

        private string PageName => new Uri(NavigationManager.Uri).PathAndQuery.Replace(NavigationManager.BaseUri, "/");

        private void InitializeNew(bool isParam = false)
        {
            GeneralConsultanService = new();
            ShowForm = true;
            GeneralConsultanService = new();
            StagingText = EnumStatusGeneralConsultantService.Confirmed;

            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                //Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                GeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
            }
            if (!isParam)
                NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Create.GetDisplayName()}");

            if (Services.Count > 0)
                GeneralConsultanService.ServiceId = Services.FirstOrDefault()?.Id ?? null;

            UserForm.IsFamilyMedicalHistory = "No";
            UserForm.IsMedicationHistory = "No";
        }

        protected override async Task OnParametersSetAsync()
        {
            // Periksa apakah PageMode diatur
            if (!string.IsNullOrWhiteSpace(PageMode))
            {
                // Cek apakah PageMode adalah Create
                if (PageMode == EnumPageMode.Create.GetDisplayName())
                {
                    // Periksa apakah URL saat ini tidak diakhiri dengan mode Create
                    if (!PageName.EndsWith($"/{EnumPageMode.Create.GetDisplayName()}", StringComparison.OrdinalIgnoreCase))
                    {
                        // Redirect ke URL dengan mode Create
                        NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Create.GetDisplayName()}", true);
                        return; // Pastikan kode berikutnya tidak dieksekusi
                    }
                    else
                    {
                        InitializeNew(true);
                    }
                }
                // Cek apakah PageMode adalah Update
                else if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    // Logika untuk update
                    if (Id > 0)
                    {
                        await EditItem_Click(true);
                    }
                    else
                    {
                        NavigationManager.NavigateTo($"{Url}", true);
                        return; // Pastikan kode berikutnya tidak dieksekusi
                    }
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoadingPage = true;
            await LoadData();
            await GetUserInfo();
            await LoadComboBox();
            IsLoadingPage = false;
        }

        private async Task LoadComboBox()
        {
            var Locations = (await Mediator.Send(new GetLocationQuery())).Item1;
            this.Locations = Locations;
            Products = await Mediator.Send(new GetProductQuery(x => x.HospitalType != null && x.HospitalType.Equals("Vactination")));
            Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true || x.IsEmployeeRelation == true));
            //Services = await Mediator.Send(new GetServiceQuery(x => x.Name.Equals("Vaccination")));
            ClassTypes = await Mediator.Send(new GetClassTypeQuery());
            Awareness = await Mediator.Send(new GetAwarenessQuery());
            Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
            Allergies = await Mediator.Send(new GetAllergyQuery());
            Allergies.ForEach(x =>
            {
                var a = Helper._allergyTypes.FirstOrDefault(z => x.Type is not null && z.Code == x.Type);
                if (a is not null)
                    x.TypeString = a.Name;
            });

            var Diagnoses = (await Mediator.Send(new GetDiagnosisQuery())).Item1;
            this.Diagnoses = Diagnoses;
            var NursingDiagnoses = (await Mediator.Send(new GetNursingDiagnosesQuery())).Item1; ;
            this.NursingDiagnoses = NursingDiagnoses;
        }

        private async Task OnCancelBack()
        {
            ShowForm = false;
            await LoadData();
            await LoadComboBox();
            NavigationManager.NavigateTo(Url);
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            GeneralConsultanServices = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Service != null && x.Service.IsVaccination == true))).Item1;
            PanelVisible = false;
        }

        private bool IsVaccinationPlan = false;

        private void OnClickVaccinationPlan()
        {
            IsVaccinationPlan = true;
        }

        private async Task EditItem_Click(bool isParam = false)
        {
            IsLoading = true;
            ShowForm = true;
            try
            {
                if (SelectedDataItems.Count > 0 && Id != 0)
                    GeneralConsultanService = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();
                else
                {
                    GeneralConsultanService = ((await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == Id))).Item1).FirstOrDefault() ?? new();
                    if (GeneralConsultanService.Id == 0)
                    {
                        Id = 0;
                        NavigationManager.NavigateTo($"{Url}", true);
                    }
                }

                if (!isParam)
                    NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{GeneralConsultanService.Id}");

                UserForm = GeneralConsultanService.Patient ?? new();
                //var targetUrl = NavigationManager.ToAbsoluteUri("/clinic-service/general-consultation-services");
                //var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(targetUrl.ToString(), "id", GeneralConsultanService.Id.ToString());
                //NavigationManager.NavigateTo(query);

                await GetPatientAllergy();

                SelectedInsurancePolicy = (await Mediator.Send(new GetInsurancePolicyQuery(x => x.Id == GeneralConsultanService.InsurancePolicyId))).FirstOrDefault() ?? new();

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:
                        StagingText = EnumStatusGeneralConsultantService.Confirmed;
                        break;

                    case EnumStatusGeneralConsultantService.Confirmed:
                        StagingText = EnumStatusGeneralConsultantService.NurseStation;
                        break;

                    case EnumStatusGeneralConsultantService.NurseStation:
                        StagingText = EnumStatusGeneralConsultantService.Waiting;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                        break;

                    case EnumStatusGeneralConsultantService.Waiting:
                        StagingText = EnumStatusGeneralConsultantService.Physician;
                        break;

                    case EnumStatusGeneralConsultantService.Physician:
                        StagingText = EnumStatusGeneralConsultantService.Finished;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));

                        if (GeneralConsultanService.PratitionerId is null)
                        {
                            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
                            {
                                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                                GeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
                            }
                        }
                        break;

                    case EnumStatusGeneralConsultantService.Finished:
                        StagingText = EnumStatusGeneralConsultantService.Finished;
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                        break;

                    case EnumStatusGeneralConsultantService.Canceled:
                        StagingText = EnumStatusGeneralConsultantService.Canceled;
                        break;

                    case EnumStatusGeneralConsultantService.ProcedureRoom:
                        StagingText = EnumStatusGeneralConsultantService.ProcedureRoom;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        private async Task OnCancelStatus()
        {
            try
            {
                IsLoading = true;

                if (GeneralConsultanService.Id != 0)
                {
                    if (SelectedBPJSIntegration is not null && SelectedBPJSIntegration.Id != 0)
                    {
                        //var isSuccess = await SendPCareRequestUpdateStatusPanggilAntrean(2);
                        //if (!isSuccess)
                        //{
                        //    IsLoading = false;
                        //    return;
                        //}
                    }

                    GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Canceled;
                    GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                    GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();
                    StagingText = EnumStatusGeneralConsultantService.Canceled;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        public MarkupString GetIssuePriorityIconHtml(GeneralConsultanServiceDto priority)
        {
            if (priority is not null)
            {
                if (!priority.IsAlertInformationSpecialCase && priority.ClassType is null)
                    return new MarkupString("");

                string priorytyClass = "danger";
                string title = string.Empty;

                if (priority.IsAlertInformationSpecialCase && priority.ClassType is not null)
                    title = $" Priority, {priority.ClassType.Name}";
                else
                {
                    if (priority.ClassType is not null)
                        title = $"{priority.ClassType.Name}";
                    if (priority.IsAlertInformationSpecialCase)
                        title = $" Priority ";
                }

                string html = string.Format("<span class='badge bg-{0} py-1 px-2' title='{1} Priority'>{1}</span>", priorytyClass, title);

                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task NewItem_Click()
        {
            ShowForm = true;
            GeneralConsultanService = new();
            StagingText = EnumStatusGeneralConsultantService.Confirmed;

            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                GeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
            }
        }

        private void DeleteItem_Click()
        {
            Grid?.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void Grid_FocusedRowChanged1(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;

            try
            {
                if ((GeneralConsultanServiceDto)args.DataItem is null)
                    return;

                IsDeletedConsultantService = ((GeneralConsultanServiceDto)args.DataItem)!.Status!.Equals(EnumStatusGeneralConsultantService.Planned) || ((GeneralConsultanServiceDto)args.DataItem)!.Status!.Equals(EnumStatusGeneralConsultantService.Canceled);
            }
            catch { }
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems == null) return;

                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(((GeneralConsultanServiceDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(ids: SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>()
                                .Where(x => x.Status == EnumStatusGeneralConsultantService.Planned || x.Status == EnumStatusGeneralConsultantService.Canceled)
                                .Select(x => x.Id)
                                .ToList()));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task GetClinicalAssesmentPatientHistory()
        {
            try
            {
                if (GeneralConsultanService.Height == 0 && GeneralConsultanService.Weight == 0)
                {
                    var prev = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x
                        => x.PatientId == GeneralConsultanService.PatientId && x.Id < GeneralConsultanService.Id && x.Status == EnumStatusGeneralConsultantService.Finished)))
                        .Item1
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault() ?? new();

                    GeneralConsultanService.Height = prev?.Height ?? 0;
                    GeneralConsultanService.Weight = prev?.Weight ?? 0;
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnClickConfirm()
        {
            IsLoading = true;

            try
            {
                if (!GeneralConsultanService.Payment!.Equals("Personal") && (SelectedInsurancePolicy is null || SelectedInsurancePolicy.Id == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                GeneralConsultanService.InsurancePolicyId = SelectedInsurancePolicy == null || SelectedInsurancePolicy.Id == 0 ? null : SelectedInsurancePolicy.Id;

                if (IsStatus(EnumStatusGeneralConsultantService.Planned) || IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    var usr = (await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId))).FirstOrDefault();
                    if (usr is not null)
                    {
                        if (usr.CurrentMobile != UserForm.CurrentMobile)
                        {
                            usr.CurrentMobile = UserForm.CurrentMobile;
                            await Mediator.Send(new UpdateUserRequest(usr));
                            Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
                            Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true || x.IsEmployeeRelation == true));
                        }
                    }
                }

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:
                        var patient = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id != GeneralConsultanService.Id && x.ServiceId == GeneralConsultanService.ServiceId && x.PatientId == GeneralConsultanService.PatientId && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned) && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date))).Item1;

                        if (patient.Count > 0)
                        {
                            IsLoading = false;
                            ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                            return;
                        }

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Confirmed;

                        if (GeneralConsultanService.Id == 0)
                            GeneralConsultanService = await Mediator.Send(new CreateGeneralConsultanServiceRequest(GeneralConsultanService));
                        else
                            GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                        GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();

                        await SaveAllergyData();

                        StagingText = EnumStatusGeneralConsultantService.NurseStation;

                        Id = GeneralConsultanService.Id;
                        NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{GeneralConsultanService.Id}");

                        break;

                    case EnumStatusGeneralConsultantService.Confirmed:

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.NurseStation;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        StagingText = EnumStatusGeneralConsultantService.Waiting;

                        break;

                    case EnumStatusGeneralConsultantService.NurseStation:

                        await SaveAllergyData();

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Waiting;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        StagingText = EnumStatusGeneralConsultantService.Physician;

                        break;

                    case EnumStatusGeneralConsultantService.Waiting:

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Physician;
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        StagingText = EnumStatusGeneralConsultantService.Finished;

                        break;

                    case EnumStatusGeneralConsultantService.Physician:

                        await SaveAllergyData();

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Finished;
                        await GetClinicalAssesmentPatientHistory();
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        StagingText = EnumStatusGeneralConsultantService.Finished;

                        if (GeneralConsultanService.IsSickLeave == true || GeneralConsultanService.IsMaternityLeave == true)
                        {
                            var checkDataSickLeave = await Mediator.Send(new GetSickLeaveQuery(x => x.GeneralConsultansId == GeneralConsultanService.Id));
                            //if (checkDataSickLeave != null && checkDataSickLeave.Count == 0)
                            //{
                            //    if (GeneralConsultanService.IsSickLeave == true)
                            //        SickLeaves.TypeLeave = "SickLeave";
                            //    else if (GeneralConsultanService.IsMaternityLeave == true)
                            //        SickLeaves.TypeLeave = "Maternity";

                            //    SickLeaves.GeneralConsultansId = GeneralConsultanService.Id;
                            //    await Mediator.Send(new CreateSickLeaveRequest(SickLeaves));
                            //}
                        }

                        #region Cut Product Stock

                        var vaccinations = await Mediator.Send(new GetVaccinationPlanQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                        var stocks = new List<TransactionStockDto>();

                        foreach (var i in vaccinations)
                        {
                            i.Status = EnumStatusVaccination.Done;

                            var s = (await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == i.ProductId && x.Batch == i.Batch && x.LocationId == GeneralConsultanService.LocationId))).FirstOrDefault() ?? new();

                            stocks.Add(new TransactionStockDto
                            {
                                SourceTable = nameof(VaccinationPlan),
                                SourcTableId = i.Id,
                                ProductId = i.ProductId,
                                Reference = GeneralConsultanService.Reference,
                                Batch = i.Batch,
                                LocationId = GeneralConsultanService.LocationId,
                                Quantity = -i.Quantity,
                                Validate = true,
                                ExpiredDate = s.ExpiredDate,
                                UomId = s.UomId
                            });
                        }

                        await Mediator.Send(new UpdateListVaccinationPlanRequest(vaccinations));
                        await Mediator.Send(new CreateListTransactionStockRequest(stocks));

                        #endregion Cut Product Stock

                        break;

                    case EnumStatusGeneralConsultantService.ProcedureRoom:
                        break;

                    case EnumStatusGeneralConsultantService.Canceled:
                        break;

                    default:
                        break;
                }

                GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();

                if (SelectedFoodAllergies.Count() > 0)
                    GeneralConsultanService.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    GeneralConsultanService.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    GeneralConsultanService.IsPharmacology = true;
            }
            catch (Exception x)
            {
                x.HandleException(ToastService);
            }

            IsLoading = false;
        }

        private async Task SaveAllergyData()
        {
            try
            {
                if (SelectedFoodAllergies.Count() > 0)
                    GeneralConsultanService.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    GeneralConsultanService.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    GeneralConsultanService.IsPharmacology = true;

                //if (SelectedPharmacologyAllergies.Count() > 0 || SelectedWeatherAllergies.Count() > 0 || SelectedFoodAllergies.Count() > 0)
                //{
                var ids = new List<long>();
                ids.AddRange(SelectedPharmacologyAllergies.Select(x => x.Id).ToList());
                ids.AddRange(SelectedWeatherAllergies.Select(x => x.Id).ToList());
                ids.AddRange(SelectedFoodAllergies.Select(x => x.Id).ToList());

                //var u = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId);
                var u = (await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId))).FirstOrDefault();
                if (u is not null)
                {
                    u = UserForm.Adapt<UserDto>();
                    u.PatientAllergyIds = ids;

                    if (u.CurrentMobile != UserForm.CurrentMobile)
                        u.CurrentMobile = UserForm.CurrentMobile;

                    //u.FamilyMedicalHistory = UserForm.FamilyMedicalHistory;

                    await Mediator.Send(new UpdateUserRequest(u));
                    Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
                    Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true || x.IsEmployeeRelation == true));
                    // Filter allergies by type
                    await GetPatientAllergy();
                    GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();
                }

                // Ga Kepake
                //PatientAllergy.UserId = GeneralConsultanService.PatientId.GetValueOrDefault();

                //if (PatientAllergy.Id == 0)
                //    PatientAllergy = await Mediator.Send(new CreatePatientAllergyRequest(PatientAllergy));
                //else
                //    PatientAllergy = await Mediator.Send(new UpdatePatientAllergyRequest(PatientAllergy));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task HandleValidSubmit()
        {
            IsLoading = true;

            try
            {
                if (!GeneralConsultanService.Payment!.Equals("Personal") && (SelectedInsurancePolicy is null || SelectedInsurancePolicy.Id == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                GeneralConsultanService.InsurancePolicyId = SelectedInsurancePolicy == null || SelectedInsurancePolicy.Id == 0 ? null : SelectedInsurancePolicy.Id;

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:

                        var patient = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id != GeneralConsultanService.Id && x.ServiceId == GeneralConsultanService.ServiceId && x.PatientId == GeneralConsultanService.PatientId && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned) && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date))).Item1;

                        if (patient.Count > 0)
                        {
                            IsLoading = false;
                            ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                            return;
                        }

                        if (GeneralConsultanService.Id == 0)
                            GeneralConsultanService = await Mediator.Send(new CreateGeneralConsultanServiceRequest(GeneralConsultanService));
                        else
                            GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                        if (SelectedFoodAllergies.Count() > 0)
                            GeneralConsultanService.IsFood = true;
                        if (SelectedWeatherAllergies.Count() > 0)
                            GeneralConsultanService.IsWeather = true;
                        if (SelectedPharmacologyAllergies.Count() > 0)
                            GeneralConsultanService.IsPharmacology = true;

                        if (GeneralConsultanService.IsPharmacology || GeneralConsultanService.IsFood || GeneralConsultanService.IsWeather)
                        {
                            await SaveAllergyData();
                        }

                        Id = GeneralConsultanService.Id;
                        NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{GeneralConsultanService.Id}");

                        IsLoading = false;
                        break;

                    case EnumStatusGeneralConsultantService.Confirmed:
                        break;

                    case EnumStatusGeneralConsultantService.NurseStation:
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                        await SaveAllergyData();
                        break;

                    case EnumStatusGeneralConsultantService.Waiting:
                        break;

                    case EnumStatusGeneralConsultantService.Physician:
                        GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                        if (GeneralConsultanService.IsSickLeave == true || GeneralConsultanService.IsMaternityLeave == true)
                        {
                            var checkDataSickLeave = await Mediator.Send(new GetSickLeaveQuery(x => x.GeneralConsultansId == GeneralConsultanService.Id));
                            //if (checkDataSickLeave != null && checkDataSickLeave.Count == 0)
                            //{
                            //    if (GeneralConsultanService.IsSickLeave == true)
                            //        SickLeaves.TypeLeave = "SickLeave";
                            //    else if (GeneralConsultanService.IsMaternityLeave == true)
                            //        SickLeaves.TypeLeave = "Maternity";

                            //    SickLeaves.GeneralConsultansId = GeneralConsultanService.Id;
                            //    await Mediator.Send(new CreateSickLeaveRequest(SickLeaves));
                            //}
                        }

                        await SaveAllergyData();
                        break;

                    case EnumStatusGeneralConsultantService.Finished:
                        break;

                    case EnumStatusGeneralConsultantService.Canceled:
                        break;

                    case EnumStatusGeneralConsultantService.ProcedureRoom:
                        break;

                    default:
                        break;
                }
                GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();

                ToastService.ClearSuccessToasts();
                ToastService.ShowSuccess("Saved Successfully!");
            }
            catch (Exception x)
            {
                x.HandleException(ToastService);
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        private async Task SelectedItemInsurancePolicyChanged(InsurancePolicyDto? result)
        {
            ToastService.ClearInfoToasts();
            SelectedBPJSIntegration = new();

            SelectedInsurancePolicy = result;
            GeneralConsultanService.InsurancePolicyId = result?.Id ?? null;

            if (result is null)
                return;

            ToastService.ClearWarningToasts();

            var bpjs = (await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == result.Id))).FirstOrDefault();
            if (bpjs is not null)
            {
                var count = GeneralConsultanServices.Where(x => x.PatientId == GeneralConsultanService.PatientId && x.Status == EnumStatusGeneralConsultantService.Planned).Count();
                if (!string.IsNullOrWhiteSpace(bpjs.KdProviderPstKdProvider))
                {
                    //var parameter = (await Mediator.Send(new GetSystemParameterQuery(x => x.Key.Contains("pcare_code_provider")))).FirstOrDefault();
                    var parameter = (await Mediator.Send(new GetSystemParameterQuery())).FirstOrDefault()?.PCareCodeProvider ?? null;

                    if (parameter is not null)
                    {
                        if (!Convert.ToBoolean(parameter.Equals(bpjs.KdProviderPstKdProvider)))
                        {
                            ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                        }
                        else
                        {
                            SelectedBPJSIntegration = bpjs;
                        }
                    }
                }
                else
                {
                    ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                }
            }
        }

        private async Task SelectedItemPatientChanged(UserDto e)
        {
            GeneralConsultanService.InsurancePolicyId = null;
            InsurancePolicies.Clear();
            GeneralConsultanService.Patient = new();
            SelectedInsurancePolicy = new();

            if (e is null)
                return;

            GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == e.Id) ?? new();
            GeneralConsultanService.PatientId = e.Id;
            UserForm = Patients.FirstOrDefault(x => x.Id == e.Id) ?? new();

            InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == e.Id && x.Insurance != null && GeneralConsultanService.Payment != null && x.Insurance.IsBPJSKesehatan == GeneralConsultanService.Payment.Equals("BPJS") && x.Active == true));

            await GetPatientAllergy();
        }

        private async Task GetPatientAllergy()
        {
            FoodAllergies.Clear();
            WeatherAllergies.Clear();
            PharmacologyAllergies.Clear();
            SelectedFoodAllergies = [];
            SelectedWeatherAllergies = [];
            SelectedPharmacologyAllergies = [];

            // Filter allergies by type
            FoodAllergies = Allergies.Where(x => x.Type == "01").ToList();
            WeatherAllergies = Allergies.Where(x => x.Type == "02").ToList();
            PharmacologyAllergies = Allergies.Where(x => x.Type == "03").ToList();

            var p = (await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId))).FirstOrDefault();

            if (p is null || p.PatientAllergyIds is null)
                return;

            var allergies = await Mediator.Send(new GetAllergyQuery(x => p.PatientAllergyIds.Contains(x.Id)));
            if (allergies is not null && allergies.Count > 0)
            {
                var selectedAllergyIds = allergies.Where(x => x.Type == "01" || x.Type == "02" || x.Type == "03").Select(x => x.Id).ToList();

                SelectedFoodAllergies = FoodAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
                SelectedWeatherAllergies = WeatherAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();
                SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => selectedAllergyIds.Contains(x.Id)).ToList();

                //await OnSelectedFoodAllergiesChanged(SelectedFoodAllergies);
                //await OnSelectedWeatherAllergiesChanged(SelectedWeatherAllergies);
                //await OnSelectedPharmacologyAllergiesChanged(SelectedPharmacologyAllergies);

                if (SelectedFoodAllergies.Count() > 0)
                    GeneralConsultanService.IsFood = true;
                if (SelectedWeatherAllergies.Count() > 0)
                    GeneralConsultanService.IsWeather = true;
                if (SelectedPharmacologyAllergies.Count() > 0)
                    GeneralConsultanService.IsPharmacology = true;
            }
        }

        private async Task SelectedItemPaymentChanged(string e)
        {
            GeneralConsultanService.Payment = null;
            GeneralConsultanService.InsurancePolicyId = null;
            SelectedInsurancePolicy = new();

            if (e is null)
                return;

            if (e.Equals("BPJS"))
                InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJSKesehatan == e.Equals("BPJS") && x.Active == true));
            else
                InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && (x.Insurance.IsBPJSKesehatan == false || x.Insurance.IsBPJSTK == false) && x.Active == true));
        }

        private void SelectedItemRegisTypeChanged(String e)
        {
            if (e is null)
            {
                return;
            }
        }

        public static List<string> RegisType = new List<string>
        {
            "General Consultation",
            "Emergency",
            //"MCU"
        };

        public static List<string> ClinicVisitTypes = new List<string>
        {
            "Healthy",
            "Sick"
        };

        public static List<string> Method = new List<string>
        {
            "MCU",
            "Gas And Oil"
        };

        public static List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        private async Task SelectedItemServiceChanged(ServiceDto e)
        {
            GeneralConsultanService.ServiceId = null;

            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                GeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
            }
            else
            {
                Physicions.Clear();

                if (e is null)
                {
                    GeneralConsultanService.PratitionerId = null;
                    return;
                }

                Physicions = await Mediator.Send(new GetUserQuery(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(e.Id)));
            }
        }

        private void SelectedMaternityStartDateChanged(DateTime e)
        {
            GeneralConsultanService.StartMaternityLeave = e;
            GeneralConsultanService.EndMaternityLeave = GeneralConsultanService.StartMaternityLeave.AddMonths(3);
        }

        #endregion Methods
    }
}
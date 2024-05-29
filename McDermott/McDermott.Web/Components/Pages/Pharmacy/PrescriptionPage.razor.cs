using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
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

        #region Static Variables

        [Parameter]
        public long Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (Id == 0)
                return;

            var generalConsultantService = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == Id));
            if (generalConsultantService.Count == 0 || generalConsultantService is null)
                return;

            ShowForm = true;
            Pharmacy.PatientId = generalConsultantService.FirstOrDefault()!.PatientId;
            Pharmacy.PractitionerId = generalConsultantService.FirstOrDefault()!.PratitionerId;
            Pharmacy.ServiceId = generalConsultantService.FirstOrDefault()!.ServiceId;
            Pharmacy.PaymentMethod = generalConsultantService.FirstOrDefault()!.Payment;
            Pharmacy.IsWeather = generalConsultantService.FirstOrDefault()!.IsWeather;
            Pharmacy.IsFarmacologi = generalConsultantService.FirstOrDefault()!.IsPharmacology;
            Pharmacy.IsFood = generalConsultantService.FirstOrDefault()!.IsFood;
        }

        private IGrid Grid { get; set; }
        private IGrid GridPrescriptionLines { get; set; }
        private IGrid GridConcoction { get; set; }
        private IGrid GridConcoctionLines { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsPrescriptionLines { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsConcoction { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsConcoctionLines { get; set; } = [];

        private TaskCompletionSource<bool> DataLoadedTcs { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        [Parameter]
        public bool IsPopUpForm { get; set; } = false;

        [Parameter]
        public UserDto User { get; set; } = new()
        {
            Name = "-"
        };

        private bool PanelVisible { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        private bool PopUpConcoctionDetail { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexPrescriptionLines { get; set; }
        private int FocusedRowVisibleIndexPrescriptionConcoction { get; set; }
        private int FocusedRowVisibleIndexConcoctionLines { get; set; }

        private PharmacyDto Pharmacy { get; set; } = new();
        private PrescriptionDto Prescription { get; set; } = new();
        private ConcoctionDto Concoction { get; set; } = new();
        private ConcoctionLineDto ConcoctionLine { get; set; } = new();
        private List<PharmacyDto> Pharmacies { get; set; } = [];
        private List<PrescriptionDto> Prescriptions { get; set; } = [];
        private List<ConcoctionDto> Concoctions { get; set; } = [];
        private List<ConcoctionLineDto> ConcoctionLines { get; set; } = [];
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
        private List<StockProductDto> StockProducts { get; set; } = [];
        private List<ActiveComponentDto> ActiveComponents { get; set; } = [];

        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];

        private List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        private async Task ChangeProduct(ProductDto product)
        {
            try
            {
                if (product is null)
                {
                    ConcoctionLine = new();
                    selectedActiveComponents = [];
                    return;
                }

                var a = await Mediator.Send(new GetMedicamentQuery());
                var ChekMedicament = a.Where(m => m.ProductId == product.Id).FirstOrDefault();
                var checkUom = Uoms.Where(x => x.Id == ChekMedicament?.UomId).FirstOrDefault();

                ConcoctionLine.Qty = ChekMedicament?.Dosage ?? 0;
                ConcoctionLine.MedicamentDosage = ChekMedicament?.Dosage ?? 0;
                ConcoctionLine.UomId = ChekMedicament?.UomId ?? null;
                selectedActiveComponents = ActiveComponents.Where(a => ChekMedicament != null && ChekMedicament.ActiveComponentId != null && ChekMedicament.ActiveComponentId.Contains(a.Id)).ToList();
                var aa = Pharmacy.PrescriptionLocationId;
                var checkStock = StockProducts.Where(x => x.ProductId == product.Id && x.SourceId == Pharmacy.PrescriptionLocationId).Select(x => x.Qty).FirstOrDefault();
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

        private void SelectChangeFrequency(DrugDosageDto datas)
        {
            var data = DrugDosages.Where(f => f.Id == datas.Id).FirstOrDefault();

            Concoction.QtyByDay = data!.TotalQtyPerDay.ToLong();
            Concoction.Days = data.Days.ToLong();
            Concoction.TotalQty = Concoction?.Qty * Concoction?.QtyByDay;
        }

        private void SelectedChangePractition(UserDto? user)
        {
            MedicamentGroups = MedicamentGroups.Where(x => x.IsConcoction == false && x.PhycisianId == user.Id).ToList();
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

                var stock = StockProducts
                    .Where(s => s.ProductId == checkProduct.Id && s.SourceId == Pharmacy.PrescriptionLocationId)
                    .Select(x => x.Qty)
                    .FirstOrDefault();

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

                prescriptionsList.Add(newPrescription);
            }

            Prescriptions = prescriptionsList;
        }

        #endregion Static Variables

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true));
            Practitioners = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
            PreciptionLocations = await Mediator.Send(new GetLocationQuery());
            ActiveComponentt = await Mediator.Send(new GetActiveComponentQuery());
            Services = await Mediator.Send(new GetServiceQuery());
            Products = await Mediator.Send(new GetProductQuery());
            DrugDosages = await Mediator.Send(new GetDrugDosageQuery());
            MedicamentGroups = await Mediator.Send(new GetMedicamentGroupQuery());
            MedicamentGroupsConcoction = await Mediator.Send(new GetMedicamentGroupQuery(x => x.IsConcoction == true));
            DrugForms = await Mediator.Send(new GetFormDrugQuery());
            Signas = await Mediator.Send(new GetSignaQuery());
            DrugRoutes = await Mediator.Send(new GetDrugRouteQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            StockProducts = await Mediator.Send(new GetStockProductQuery());
            Medicaments = await Mediator.Send(new GetMedicamentQuery());
            ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
            Pharmacies = await Mediator.Send(new GetPharmacyQuery());
            Concoctions = new List<ConcoctionDto>();
            var c = Concoctions;
            await GetUserInfo();

            IsLoading = false;
            await LoadData();
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

        #region Load Data

        private async Task LoadData(bool v = false)
        {
            IsLoading = true;
            if (User != null && User.Id != 0)
            {
                Pharmacies = Pharmacies.Where(x => x.PatientId == User.Id).ToList();
            }

            if (v)
                User = new() { Name = "-" };
            IsLoading = false;
        }

        private async Task LoadDataPharmacy()
        {
            ShowForm = false;
            IsLoading = true;
            SelectedDataItems = [];
            SelectedDataItemsConcoction = [];
            SelectedDataItemsPrescriptionLines = [];
            Pharmacies = await Mediator.Send(new GetPharmacyQuery());

            IsLoading = false;
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
            IsLoading = true;
            SelectedDataItems = [];
            Concoctions = await Mediator.Send(new GetConcoctionQuery());
            IsLoading = false;
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

        #endregion Load Data

        #region Delete Function

        private async Task OnDeletePharmacy(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeletePharmacyRequest(((PharmacyDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeletePharmacyRequest(ids: SelectedDataItems.Adapt<List<PharmacyDto>>().Select(x => x.Id).ToList()));
                }
                await LoadDataPharmacy();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnDeletePrescriptionLines(GridDataItemDeletingEventArgs e)
        {
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
                    if (SelectedDataItemsPrescriptionLines is null || SelectedDataItemsPrescriptionLines.Count == 1)
                    {
                        await Mediator.Send(new DeletePrescriptionRequest(((PrescriptionDto)e.DataItem).Id));
                    }
                    else
                    {
                        await Mediator.Send(new DeletePrescriptionRequest(ids: SelectedDataItemsPrescriptionLines.Adapt<List<PrescriptionDto>>().Select(x => x.Id).ToList()));
                    }
                    await LoadDataPresciptions();
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        private async Task OnDeleteConcoction(GridDataItemDeletingEventArgs e)
        {
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
                    if (SelectedDataItemsConcoction is null || SelectedDataItemsConcoction.Count == 1)
                    {
                        await Mediator.Send(new DeleteConcoctionRequest(((ConcoctionDto)e.DataItem).Id));
                    }
                    else
                    {
                        await Mediator.Send(new DeleteConcoctionRequest(ids: SelectedDataItemsConcoction.Adapt<List<ConcoctionDto>>().Select(x => x.Id).ToList()));
                    }
                    await LoadDataConcoctions();
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        private async Task OnDeleteConcoctionLine(GridDataItemDeletingEventArgs e)
        {
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
                    if (SelectedDataItemsConcoctionLines is null || SelectedDataItemsConcoctionLines.Count == 1)
                    {
                        await Mediator.Send(new DeleteConcoctionLineRequest(((ConcoctionLineDto)e.DataItem).Id));
                    }
                    else
                    {
                        await Mediator.Send(new DeleteConcoctionLineRequest(ids: SelectedDataItemsConcoctionLines.Adapt<List<ConcoctionLineDto>>().Select(x => x.Id).ToList()));
                    }
                    await LoadDataConcoctionLines();
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

        private async Task HandleInvalidSubmit()
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

                    //var medicamentGroup = MedicamentGroupsConcoction.FirstOrDefault(x => x.Id == Concoction.MedicamentGroupId);
                    //if (medicamentGroup != null)
                    //{
                    //    Concoction.MedicamentName = medicamentGroup.Name;
                    //    Concoction.UomId = medicamentGroup.UoMId;
                    //    Concoction.DrugFromId = medicamentGroup.FormDrugId;
                    //}
                    Concoction.PrescribingDoctorId = Pharmacy.PractitionerId;

                    if (Concoction.Id == 0)
                    {
                        Concoction.Id = Helper.RandomNumber;
                        Concoction.MedicamentGroupName = MedicamentGroupsConcoction.Where(x => x.Id == Concoction.MedicamentGroupId).Select(x => x.Name).FirstOrDefault();
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

                    StateHasChanged();
                    SelectedDataItemsConcoction = [];
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                Concoction.PharmacyId = Pharmacy.Id;
                if (Concoction.Id != 0)
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
            if (Pharmacy.Id == 0)
            {
                try
                {
                    ConcoctionLineDto update = new();

                    var products = Medicaments.Where(x => x.ProductId == ConcoctionLine.ProductId).FirstOrDefault();
                    if (products is not null)
                    {
                        Cl.MedicamentDosage = products.Dosage;
                        Cl.ProductName = products.Product?.Name;
                        Cl.Qty = products.Dosage;
                        Cl.UomId = products.UomId;
                        Cl.UomName = Uoms.Where(x => x.Id == ConcoctionLine.UomId).Select(x => x.Name).FirstOrDefault();
                        Cl.ActiveComponentId = products.ActiveComponentId;
                        Cl.ActiveComponentName = string.Join(",", ActiveComponents.Where(a => ConcoctionLine.ActiveComponentId is not null && ConcoctionLine.ActiveComponentId.Contains(a.Id)).Select(n => n.Name));
                    }
                    if (Cl.Id == 0)
                    {
                        Cl.Id = Helper.RandomNumber;
                        ConcoctionLines.Add(Cl);
                    }
                    else
                    {
                        var q = SelectedDataItemsConcoctionLines[0].Adapt<ConcoctionLineDto>();
                        update = ConcoctionLines.FirstOrDefault(x => x.Id == q.Id)!;
                        var index = ConcoctionLines.IndexOf(update);
                        ConcoctionLines[index] = ConcoctionLine;
                    }
                    SelectedDataItemsConcoctionLines = [];
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            else
            {
                Cl.ConcoctionId = Concoction.Id;
                if (Cl.Id == 0)
                {
                    await Mediator.Send(new CreateConcoctionLineRequest(Cl));
                }
                else
                {
                    await Mediator.Send(new UpdateConcoctionLineRequest(ConcoctionLine));
                }
                await LoadDataConcoctionLines();
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

                await LoadDataPresciptions();
            }
        }

        private async Task OnSavePharmacy()
        {
            try
            {
                if (Pharmacy.Id == 0)
                {
                    Pharmacy = await Mediator.Send(new CreatePharmacyRequest(Pharmacy));
                    Prescriptions.ForEach(x =>
                    {
                        x.PharmacyId = Pharmacy.Id;
                        x.Id = 0;
                    });
                    await Mediator.Send(new CreateListPrescriptionRequest(Prescriptions));

                    Concoctions.ForEach(x =>
                    {
                        x.PharmacyId = Pharmacy.Id;
                        x.Id = 0;
                    });
                    await Mediator.Send(new CreateListConcoctionRequest(Concoctions));
                }
                else
                {
                    await Mediator.Send(new UpdatePharmacyRequest(Pharmacy));
                }

                await LoadDataPharmacy();
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
            ShowForm = true;
            Pharmacy = new();
            Concoctions.Clear();
            Prescriptions.Clear();
        }

        private async Task NewItemPrescriptionLines_Click()
        {
            await GridPrescriptionLines.StartEditNewRowAsync();
        }

        private void NewItemConcoction_Click()
        {
            PopUpConcoctionDetail = true;
            Concoction = new();
            ConcoctionLines.Clear();
        }

        private async Task NewItemConcoctionLines_Click()
        {
            selectedActiveComponents = [];
            await GridConcoctionLines.StartEditNewRowAsync();
        }

        #endregion NewItem Click

        #region function Edit Click

        private async Task EditItemPharmacy_Click()
        {
            ShowForm = true;
            IsLoading = true;

            try
            {
                var p = await Mediator.Send(new GetPharmacyQuery(x => x.Id == SelectedDataItems[0].Adapt<PharmacyDto>().Id));

                if (p.Count > 0)
                {
                    Pharmacy = p[0];
                    Prescriptions = await Mediator.Send(new GetPrescriptionQuery(x => x.PharmacyId == Pharmacy.Id));
                    Concoctions = await Mediator.Send(new GetConcoctionQuery(x => x.PharmacyId == Pharmacy.Id));
                }
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
            }

            IsLoading = false;
        }

        private void EditItem_Click()
        {
            ShowForm = true;
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
            IsLoading = false;
        }

        private async Task EditItemConcoctionLines_Click(IGrid context)
        {
            var selected = (ConcoctionLineDto)context.SelectedDataItem;
            await GridConcoctionLines.StartEditRowAsync(FocusedRowVisibleIndexConcoctionLines);
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

        #region Delete Grid Config

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void DeleteItemConcoctionLines_Click()
        {
            GridConcoctionLines.ShowRowDeleteConfirmation(FocusedRowVisibleIndexConcoctionLines);
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

        #endregion Methods

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

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            }); ;
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

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void GridFocusedRowVisibleIndexPrescriptionConcoction_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexPrescriptionConcoction = args.VisibleIndex;
        }

        private void GridFocusedRowVisibleIndexConcoctionLines_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexConcoctionLines = args.VisibleIndex;
        }

        #endregion Grid Properties
    }
}
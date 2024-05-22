using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;
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
        private IReadOnlyList<object> SelectedDataItemsPrescriptionConcoction { get; set; } = [];

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
        private List<DrugFormDto> DrugForms { get; set; } = [];
        private List<DrugRouteDto> DrugRoutes { get; set; } = [];
        private List<StockProductDto> StockProducts { get; set; } = [];
        private List<ActiveComponentDto> ActiveComponents { get; set; } = [];

        private List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        private async Task SelectedChangePractition(UserDto? user)
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
            Services = await Mediator.Send(new GetServiceQuery());
            Products = await Mediator.Send(new GetProductQuery());
            DrugDosages = await Mediator.Send(new GetDrugDosageQuery());
            MedicamentGroups = await Mediator.Send(new GetMedicamentGroupQuery());
            DrugForms = await Mediator.Send(new GetFormDrugQuery());
            Signas = await Mediator.Send(new GetSignaQuery());
            DrugRoutes = await Mediator.Send(new GetDrugRouteQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            StockProducts = await Mediator.Send(new GetStockProductQuery());
            Medicaments = await Mediator.Send(new GetMedicamentQuery());
            ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
            Pharmacies = await Mediator.Send(new GetPharmacyQuery());

            await GetUserInfo();

            IsLoading = false;
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

        private async Task LoadDataPharmacy()
        {
            ShowForm = false;
            IsLoading = true;
            SelectedDataItems = [];
            SelectedDataItemsPrescriptionConcoction = [];
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
                }
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
            }

            IsLoading = false;
        }

        private async Task NewItem_Click()
        {
            ShowForm = true;
            Pharmacy = new();
        }

        private async Task Refresh_Click()
        {
            await LoadDataPharmacy();
        }

        private async Task EditItem_Click()
        {
            ShowForm = true;
            Pharmacy = new();
            SelectedDataItemsPrescriptionLines = [];
            Prescriptions = [];
        }

        private async Task Back_Click()
        {
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private bool FormValidationState = true;
        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            if (FormValidationState)
                await OnSavePharmacy();
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

        private async Task HandleInvalidSubmit()
        {
            FormValidationState = true;
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }

        private async Task HandleValidConcoctionSubmit()
        {
        }

        private async Task HandleInvalidConcoctionSubmit()
        {
        }

        private async Task NewItemPrescriptionLines_Click()
        {
            await GridPrescriptionLines.StartEditNewRowAsync();
        }

        private async Task RefreshPrescriptionLines_Click()
        {
        }

        private async Task OnDiscard()
        {
            await LoadDataPharmacy();
        }

        private async Task EditItemPrescriptionLines_Click(IGrid context)
        {
            var selected = (PrescriptionDto)context.SelectedDataItem;
            var copy = selected.Adapt<LabTestDetailDto>();
            await GridPrescriptionLines.StartEditRowAsync(FocusedRowVisibleIndexPrescriptionLines);
            var w = Prescriptions.FirstOrDefault(x => x.Id == copy.Id);
        }

        private void DeleteItemPrescriptionLines_Click()
        {
            GridPrescriptionLines.ShowRowDeleteConfirmation(FocusedRowVisibleIndexPrescriptionLines);
        }

        private void NewItemPrescriptionConcoction_Click()
        {
            PopUpConcoctionDetail = true;
            //await GridPrescriptionConcoction.StartEditNewRowAsync();
        }

        private async Task RefreshPrescriptionConcoction_Click()
        {
        }

        private async Task EditItemPrescriptionConcoction_Click()
        {
        }

        private void DeleteItemPrescriptionConcoction_Click()
        {
            GridConcoction.ShowRowDeleteConfirmation(FocusedRowVisibleIndexPrescriptionConcoction);
        }

        private async Task NewItemConcoctionLines_Click()
        {
            await GridConcoctionLines.StartEditNewRowAsync();
        }

        private async Task RefreshConcoctionLines_Click()
        {
        }

        private async Task EditItemConcoctionLines_Click()
        {
        }

        private void DeleteItemConcoctionLines_Click()
        {
            GridConcoctionLines.ShowRowDeleteConfirmation(FocusedRowVisibleIndexConcoctionLines);
        }

        #endregion Methods

        #region Grid Properties

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
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

        #endregion Grid Properties
    }
}
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentGroupCommand;
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
        private int ActiveSelectedTabIndex { get; set; } = 0;

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
        private List<ActiveComponentDto> ActiveComponents { get; set; } = [];

        private List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

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
            Medicaments = await Mediator.Send(new GetMedicamentQuery());
            MedicamentGroups = await Mediator.Send(new GetMedicamentGroupQuery());
            DrugForms = await Mediator.Send(new GetFormDrugQuery());
            Signas = await Mediator.Send(new GetSignaQuery());
            DrugRoutes = await Mediator.Send(new GetDrugRouteQuery());
            Uoms = await Mediator.Send(new GetUomQuery());
            ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());

            await GetUserInfo();

            IsLoading = false;
        }

        private async Task LoadData()
        {
            ShowForm = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private async Task OnDeletePrescriptionLines(GridDataItemDeletingEventArgs e)
        {
            try
            {
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private async Task NewItem_Click()
        {
            ShowForm = true;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            ShowForm = true;
        }

        private async Task Back_Click()
        {
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task HandleValidSubmit()
        {
        }

        private async Task HandleInvalidSubmit()
        {
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

        private async Task EditItemPrescriptionLines_Click()
        {
        }

        private void DeleteItemPrescriptionLines_Click()
        {
            GridPrescriptionLines.ShowRowDeleteConfirmation(FocusedRowVisibleIndexPrescriptionLines);
        }

        private async Task NewItemPrescriptionConcoction_Click()
        {
            PopUpConcoctionDetail = true;
            if (GridConcoctionLines is not null)
                GridConcoctionLines.AutoFitColumnWidths();
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
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.ProductCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentGroupCommand;
using static McDermott.Application.Features.Commands.Pharmacy.SignaCommand;

namespace McDermott.Web.Components.Pages.Pharmacy
{
    public partial class MedicamentGroupPage
    {
        #region Relation Data

        private List<MedicamentGroupDto> medicamentGroups = [];
        private List<MedicamentGroupDetailDto> medicamentGroupDetails = [];
        private List<UserDto> Phy = new();
        private List<UomDto> UoMs = new();
        private List<SignaDto> Signas = new();
        private List<ActiveComponentDto> ActiveComponents = [];
        private List<DrugFormDto> FormDrugs = new();
        private List<ProductDto> Products = [];
        private MedicamentGroupDto MGForm = new();
        private MedicamentGroupDetailDto FormMedicamenDetails = new();
        MedicamentGroupDto getMedicament = new();
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];

        #endregion Relation Data

        #region variabel static

        private IGrid Grid { get; set; }
        public IGrid GridDoctorScheduleDetail { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool Checkins { get; set; } = false;
        private bool PopUpFormMedicaments { get; set; } = false;
        private bool Concotions { get; set; } = false;
        public bool KeyboardNavigationEnabled { get; set; }
        private bool IsAddMedicament { get; set; } = false;
        private double Dosages { get; set; }
        private string? chars { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedMedicamentGroupDetailDataItems { get; set; } = [];

        private async Task SelectChangeItem(ProductDto product)
        {
            try
            {
                var a = await Mediator.Send(new GetMedicamentQuery());
                var ChekMedicament = a.Where(m => m.ProductId == product.Id).FirstOrDefault();
                var checkUom = UoMs.Where(x => x.Id == ChekMedicament?.UomId).FirstOrDefault();
                FormMedicamenDetails.MedicaneUnitDosage = checkUom?.Name;
                FormMedicamenDetails.Dosage = ChekMedicament?.Dosage;
                FormMedicamenDetails.MedicaneDosage = ChekMedicament?.Dosage;
                if (FormMedicamenDetails.Dosage != null && FormMedicamenDetails.Days != null)
                {
                    var totalQty = (Int64.Parse(FormMedicamenDetails?.Dosage) * Int64.Parse(FormMedicamenDetails?.Days));
                    FormMedicamenDetails.TotalQty = totalQty.ToString();
                }
                if (FormMedicamenDetails.SignaId != null)
                {
                    FormMedicamenDetails.SignaId = ChekMedicament.SignaId;
                }
                selectedActiveComponents = ActiveComponents.Where(a => ChekMedicament.ActiveComponentId.Contains(a.Id)).ToList();
                FormMedicamenDetails.RegimentOfUseId = ChekMedicament.UomId;
                FormMedicamenDetails.MedicaneName = product.Name;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private bool Checkin
        {
            get => Checkins;
            set
            {
                bool Checkins = value;
                this.Checkins = value;
                if (Checkins)
                {
                    Concotions = true;
                    MGForm.IsConcoction = true;
                }
                else
                {
                    Concotions = false;
                }
            }
        }

        private bool IsNumeric(string value)
        {
            return double.TryParse(value, out _);
        }

        #endregion variabel static

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

        #region async Data

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
            }
            catch { }

            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            SelectedMedicamentGroupDetailDataItems = new ObservableRangeCollection<object>();
            var user = await Mediator.Send(new GetUserQuery());
            FormDrugs = await Mediator.Send(new GetFormDrugQuery());
            UoMs = await Mediator.Send(new GetUomQuery());
            ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
            Signas = await Mediator.Send(new GetSignaQuery());
            Phy = [.. user.Where(x => x.IsPhysicion == true)];
            Products = await Mediator.Send(new GetProductQuery());
            PanelVisible = false;
        }

        private void onChangeTotalQty(string numDosage)
        {
            var a = Int64.Parse(numDosage);
            var b = Int64.Parse(FormMedicamenDetails.Days);
            FormMedicamenDetails.Dosage = numDosage;
            var tempt = a * b;
            FormMedicamenDetails.TotalQty = tempt.ToString();
        }

        private void onChangeTotalQtyDays(string numDays)
        {
            var a = Int64.Parse(numDays);
            var b = Int64.Parse(FormMedicamenDetails.Dosage);
            FormMedicamenDetails.Days = numDays;
            var tempt = a * b;
            FormMedicamenDetails.TotalQty = tempt.ToString();
        }

        #endregion async Data

        #region Grid

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

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }
        private void GridMedicamentGroupDetail_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Grid

        #region Click

        private async Task NewItem_Click()
        {
            medicamentGroupDetails.Clear();
            showForm = true;
        }

        private async Task NewItemMedicamentGroupDetail_Click()
        {
            FormMedicamenDetails = new();
            IsAddMedicament = true;
            await GridDoctorScheduleDetail.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private async Task EditItemMedicamentGroupDetail_Click()
        {

        }
        private async Task SaveItemMedicamentGroupDetailGrid_Click()
        {
            if (MGForm.Name is null) return;

            MedicamentGroupDto result = new();
            if (MGForm.Id == 0)
            {
                getMedicament = await Mediator.Send(new CreateMedicamentGroupRequest(MGForm));

                foreach (var a in medicamentGroupDetails)
                {
                    a.MedicamentGroupId = getMedicament.Id;
                    a.MedicamentId = FormMedicamenDetails.MedicamentId;
                    a.MedicaneDosage = FormMedicamenDetails.MedicaneDosage;
                    a.QtyByDay = FormMedicamenDetails.QtyByDay;
                    a.Days = FormMedicamenDetails.Days;
                    a.Comment = FormMedicamenDetails.Comment;
                    a.TotalQty = FormMedicamenDetails.TotalQty;
                    a.SignaId = FormMedicamenDetails.SignaId;
                    a.RegimentOfUseId = FormMedicamenDetails.RegimentOfUseId;
                    a.MedicaneUnitDosage = FormMedicamenDetails.MedicaneUnitDosage;
                    a.Dosage = FormMedicamenDetails.Dosage;
                    if (selectedActiveComponents != null)
                    {
                        var listActiveComponent = selectedActiveComponents.Select(x => x.Id).ToList();
                        FormMedicamenDetails.ActiveComponentId?.AddRange(listActiveComponent);
                    }
                    a.ActiveComponentId = FormMedicamenDetails.ActiveComponentId;
                    await Mediator.Send(new CreateMedicamentGroupDetailRequest(a));

                }

            }
        }

        private async Task Back_Click()
        {
            showForm = false;
        }

        private async Task CancelItemMedicamentGroupDetailGrid_Click()
        {
            showForm = false;
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void DeleteItemMedicamentGroupDetail_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #endregion Click

        #region function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteMedicamentGroupRequest(((MedicamentGroupDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteMedicamentGroupRequest(ids: SelectedDataItems.Adapt<List<MedicamentGroupDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private async Task OnDeleteDoctorScheduleDetail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteMedicamentGroupRequest(((MedicamentGroupDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteMedicamentGroupRequest(ids: SelectedDataItems.Adapt<List<MedicamentGroupDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        #endregion function Delete

        #region Function Save

        private async Task OnSaveDoctorScheduleDetail(GridEditModelSavingEventArgs e)
        {
            try
            {
                var FormMedicamenGroupDetails = (MedicamentGroupDetailDto)e.EditModel;

                // Jika menambahkan medicament
                if (IsAddMedicament)
                {
                    // Cek apakah medicament dengan MedicamentId yang sama sudah ada
                    if (medicamentGroupDetails.Any(x => x.MedicamentId == FormMedicamenGroupDetails.MedicamentId))
                        return;

                    // Ambil daftar ID komponen aktif yang dipilih
                    var listActiveComponentIds = selectedActiveComponents?.Select(x => x.Id).ToList();

                    // Jika ada komponen aktif yang dipilih, tambahkan ke FormMedicamenGroupDetails
                    if (FormMedicamenGroupDetails.ActiveComponentId != null)
                        FormMedicamenGroupDetails.ActiveComponentId.AddRange(listActiveComponentIds);
                    else
                        FormMedicamenGroupDetails.ActiveComponentId = listActiveComponentIds;

                    // Tambahkan FormMedicamenGroupDetails ke daftar medicamentGroupDetails
                    medicamentGroupDetails.Add(FormMedicamenGroupDetails);

                    // Update nama komponen aktif untuk setiap item dalam daftar medicamentGroupDetails
                    foreach (var detail in medicamentGroupDetails)
                    {
                        detail.ActiveComponentName = string.Join(",", ActiveComponents
                            .Where(a => detail.ActiveComponentId.Contains(a.Id))
                            .Select(a => a.Name));
                    }
                }

                // Bersihkan koleksi SelectedMedicamentGroupDetailDataItems
                SelectedMedicamentGroupDetailDataItems = new ObservableRangeCollection<object>();

            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Function Save
    }
}
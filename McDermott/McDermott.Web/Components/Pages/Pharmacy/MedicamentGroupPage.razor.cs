using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentCommand;
using static McDermott.Application.Features.Commands.Pharmacy.MedicamentGroupCommand;

namespace McDermott.Web.Components.Pages.Pharmacy
{
    public partial class MedicamentGroupPage
    {
        #region Relation Data

        private List<MedicamentGroupDto> medicamentGroups = [];
        private List<MedicamentGroupDetailDto> medicamentGroupDetails = [];
        private List<UserDto> Phy = new();
        private List<UomDto> UoMs = new();
        private List<DrugDosageDto> Frequencys = new();
        private List<ActiveComponentDto> ActiveComponents = [];
        private List<DrugFormDto> FormDrugs = new();
        private List<ProductDto> Products = [];
        private MedicamentGroupDto MGForm = new();
        private MedicamentGroupDetailDto FormMedicamenDetails = new();
        private MedicamentGroupDto getMedicament = new();
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];

        #endregion Relation Data

        #region variabel static

        private IGrid Grid { get; set; }
        public IGrid GridMedicamenGroupDetail { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool Checkins { get; set; } = false;
        private bool PopUpFormMedicaments { get; set; } = false;
        private bool Concotions { get; set; } = false;
        public bool KeyboardNavigationEnabled { get; set; }
        private bool IsAddMedicament { get; set; } = false;
        private bool FormValidationState { get; set; } = true;
        private bool IsLoading { get; set; } = false;
        private double Dosages { get; set; }
        private string? chars { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexMedicamentGroup { get; set; }
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
                FormMedicamenDetails.FrequencyId = ChekMedicament?.FrequencyId;
                FormMedicamenDetails.FrequencyName = ChekMedicament.Frequency.Frequency;
                FormMedicamenDetails.Dosage = ChekMedicament.Dosage;
                FormMedicamenDetails.MedicaneDosage = ChekMedicament?.Dosage;
                FormMedicamenDetails.Days = ChekMedicament.Frequency.Days.ToLong();
                FormMedicamenDetails.QtyByDay = ChekMedicament.Frequency.TotalQtyPerDay.ToLong();

                if (Concotions == true)
                {
                    FormMedicamenDetails.TotalQty = FormMedicamenDetails.Dosage;
                }
                else
                {
                    if (FormMedicamenDetails.Dosage != null && FormMedicamenDetails.QtyByDay != null)
                    {
                        FormMedicamenDetails.TotalQty = FormMedicamenDetails?.Dosage * FormMedicamenDetails?.QtyByDay;
                    }
                    FormMedicamenDetails.FrequencyId = ChekMedicament.FrequencyId;
                }
                selectedActiveComponents = ActiveComponents.Where(a => ChekMedicament.ActiveComponentId.Contains(a.Id)).ToList();
                FormMedicamenDetails.UnitOfDosageId = ChekMedicament.UomId;
                FormMedicamenDetails.MedicaneName = ChekMedicament.Product.Name;
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
            medicamentGroups = await Mediator.Send(new GetMedicamentGroupQuery());
            var user = await Mediator.Send(new GetUserQuery());
            FormDrugs = await Mediator.Send(new GetFormDrugQuery());
            UoMs = await Mediator.Send(new GetUomQuery());
            ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
            Frequencys = await Mediator.Send(new GetDrugDosageQuery());
            Phy = [.. user.Where(x => x.IsPhysicion == true)];
            Products = await Mediator.Send(new GetProductQuery());
            PanelVisible = false;
        }

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
            {
                await OnSave();
            }
            else
            {
                FormValidationState = true;
            }
        }

        private async Task HandleInvalidSubmit()
        {
            showForm = true;
            FormValidationState = false;
        }

        private void OnValueChangedTotalQty(long numDosage)
        {
            if (numDosage != null)
            {
                FormMedicamenDetails.Dosage = numDosage;
            }
            FormMedicamenDetails.TotalQty = numDosage * FormMedicamenDetails.QtyByDay;
        }

        private void OnValueChangedTotalQtyDays(long? numDays)
        {
            FormMedicamenDetails.TotalQty = numDays * FormMedicamenDetails.Dosage;
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
            selectedActiveComponents = [];
            IsAddMedicament = true;
            await GridMedicamenGroupDetail.StartEditNewRowAsync();
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

        private async Task OnSave()
        {
            try
            {
                if (MGForm.Name == "")
                {
                    return;
                }
                var aa = MGForm;

                MedicamentGroupDto result = new();
                if (MGForm.Id == 0)
                {
                    getMedicament = await Mediator.Send(new CreateMedicamentGroupRequest(MGForm));

                    foreach (var a in medicamentGroupDetails)
                    {
                        a.MedicamentGroupId = getMedicament.Id;
                        a.MedicamentId = FormMedicamenDetails.MedicamentId;
                        a.MedicaneDosage = FormMedicamenDetails.MedicaneDosage;
                        a.MedicaneUnitDosage = FormMedicamenDetails.MedicaneUnitDosage;
                        a.QtyByDay = FormMedicamenDetails.QtyByDay;
                        a.Days = FormMedicamenDetails.Days;
                        a.Comment = FormMedicamenDetails.Comment;
                        a.TotalQty = FormMedicamenDetails.TotalQty;
                        a.FrequencyId = FormMedicamenDetails.FrequencyId;
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
                showForm = false;
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
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
            GridMedicamenGroupDetail.ShowRowDeleteConfirmation(FocusedRowVisibleIndexMedicamentGroup);
        }

        #endregion Click

        #region function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteMedicamentGroupRequest(SelectedDataItems[0].Adapt<MedicamentGroupDto>().Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<MedicamentGroupDto>>();
                    await Mediator.Send(new DeleteMedicamentGroupRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private async Task OnDeleteMedicamentGroupDetail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                StateHasChanged();
                var aaa = SelectedMedicamentGroupDetailDataItems.Adapt<List<MedicamentGroupDetailDto>>();
                medicamentGroupDetails.RemoveAll(x => aaa.Select(z => z.MedicamentId).Contains(x.MedicamentId));
                SelectedMedicamentGroupDetailDataItems = new ObservableRangeCollection<object>();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        #endregion function Delete

        #region Function Save

        private async Task OnSaveMedicamentGroupDetail(GridEditModelSavingEventArgs e)
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
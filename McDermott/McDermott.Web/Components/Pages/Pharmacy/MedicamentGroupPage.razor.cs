using McDermott.Application.Dtos.Pharmacy;
using McDermott.Application.Dtos.Queue;
using McDermott.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
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
        private MedicamentGroupDto MGFrom = new();
        private List<DrugFormDto> FormDrugs = new();
        private MedicamentGroupDetailDto FormMedicamenDetails = new();
        private IEnumerable<ActiveComponentDto>? selectedActiveComponents { get; set; } = [];
        #endregion

        #region variabel static
        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool Checkins { get; set; } = false;
        private bool FormMedicaments { get; set; } = false;
        private bool Concotions { get; set; } = false;
        public bool KeyboardNavigationEnabled { get; set; }
        private bool IsAddMedicament { get; set; } = false;
        private double Dosages { get; set; }
        private string? chars { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedMedicamentGroupDetailDataItems { get; set; } = new ObservableRangeCollection<object>();

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
                    MGFrom.IsConcoction = true;
                    chars = " awokkkkk";
                }
                else
                {
                    Concotions = false;
                }
            }
        }


        bool IsNumeric(string value)
        {
            return double.TryParse(value, out _);
        }
        #endregion

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
            var user = await Mediator.Send(new GetUserQuery());
            FormDrugs = await Mediator.Send(new GetFormDrugQuery());
            UoMs = await Mediator.Send(new GetUomQuery());
            ActiveComponents = await Mediator.Send(new GetActiveComponentQuery());
            Signas = await Mediator.Send(new GetSignaQuery());
            Phy = [.. user.Where(x => x.IsPhysicion == true)];
            PanelVisible = false;
        }

        void onChangeTotalQty(string numDosage)
        {
            var a = Int64.Parse(numDosage);
            var b = Int64.Parse(FormMedicamenDetails.Days);
            FormMedicamenDetails.Dosage = numDosage;
            var tempt = a * b;
            FormMedicamenDetails.TotalQty = tempt.ToString();
        }
        void onChangeTotalQtyDays(string numDays)
        {
            var a = Int64.Parse(numDays);
            var b = Int64.Parse(FormMedicamenDetails.Dosage);
            FormMedicamenDetails.Days = numDays;
            var tempt = a * b;
            FormMedicamenDetails.TotalQty = tempt.ToString();
        }
        #endregion

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

        void Grid_CustomizeFilterRowEditor(GridCustomizeFilterRowEditorEventArgs e)
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

        #endregion Grid

        #region Click
        private async Task NewItem_Click()
        {
            
            showForm = true;
        }
        private async Task NewItemMedicamentGroupDetail_Click()
        {
            FormMedicamenDetails = new();
            IsAddMedicament = true;
            FormMedicamenDetails.Days = "1";
            FormMedicaments = true;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }
        private async Task SaveItemMedicamentGroupDetailGrid_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
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
        #endregion

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
        #endregion
        #region Function Save
        private async Task onSaveDetail()
        {
            try
            {
                MedicamentGroupDetailDto updateMedicamentGroupDetail = new();
                if (IsAddMedicament)
                {
                    if (medicamentGroupDetails.Where(x => x.MedicamentId == FormMedicamenDetails.MedicamentId).Any())
                        return;

                    medicamentGroupDetails.Add(FormMedicamenDetails);
                }

                //if (IsAddMedicament)
                //{

                //}

                SelectedMedicamentGroupDetailDataItems = new ObservableRangeCollection<object>();
                FormMedicaments = false;
            }
            catch(Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        #endregion
    }
}

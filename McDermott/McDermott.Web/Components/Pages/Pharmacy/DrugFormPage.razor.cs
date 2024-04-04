using MediatR;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;
using static McDermott.Application.Features.Commands.Pharmacy.FormDrugCommand;

namespace McDermott.Web.Components.Pages.Pharmacy
{
    public partial class DrugFormPage
    {
        #region Relation Data
        private List<DrugFormDto> DataFormDrugs = [];
        private DrugFormDto FormDrugs = new();
        #endregion

        #region Properties Grid
        private IGrid Grid;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        #endregion

        #region Variabel static
        private bool showForm { get; set; } = false;
        private bool PanelVisible { get; set; } = false;
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
            showForm = false;
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            DataFormDrugs = await Mediator.Send(new GetFormDrugQuery());            
            PanelVisible = false;
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
       
        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            var general = SelectedDataItems[0].Adapt<DrugFormDto>();
            FormDrugs = general;
            showForm = true;
        }
        
        private async Task Back_Click()
        {
            showForm = false;
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task onCancle()
        {
            await LoadData();
        }
       
        #endregion

        #region function Delete
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteFormDrugRequest(((DrugFormDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteFormDrugRequest(ids: SelectedDataItems.Adapt<List<DrugFormDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }
        #endregion

        #region function Save
        private async Task onSave()
        {
            try
            {
                if(FormDrugs.Id == 0)
                {
                    await Mediator.Send(new CreateFormDrugRequest(FormDrugs));
                }
                else
                {
                    await Mediator.Send(new UpdateFormDrugRequest(FormDrugs));
                }

                await LoadData();
            }catch(Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        #endregion
    }
}

using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.KioskDepartementCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class KioskDepartementPage
    {
        #region Relation Data

        private List<KioskDepartementDto> kioskDepartements = [];
        private IEnumerable<ServiceDto> ServicesK = [];
        private IEnumerable<ServiceDto> ServicesP = [];
        private KioskDepartementDto FormKioskDepart = new();

        #endregion Relation Data

        #region Grid Properties

        private GroupMenuDto UserAccessCRUID = new();

        private bool IsAccess = false;
        private bool PanelVisible { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;
        private string TextPopUp { get; set; } = string.Empty;
        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        #endregion Grid Properties

        #region Get And Async Data

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

            await LoadData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    var result = await NavigationManager.CheckAccessUser(oLocal);
                    IsAccess = result.Item1;
                    UserAccessCRUID = result.Item2;
                }
                catch { }
            }
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            kioskDepartements = await Mediator.Send(new GetKioskDepartementQuery());
            PanelVisible = false;
        }

        #endregion Get And Async Data

        #region Grid Function

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #endregion Grid Function

        #region Button function

        private async Task NewItem_Click()
        {
            PopUpVisible = true;
            TextPopUp = "Add Data Kiosk";
            var s = await Mediator.Send(new GetServiceQuery());
            ServicesK = [.. s.Where(x => x.IsKiosk == true)];
            ServicesP = [.. s.Where(x => x.IsPatient == true)];
        }

        private async Task EditItem_Click()
        {
            try
            {
                var General = SelectedDataItems[0].Adapt<KioskDepartementDto>();
                FormKioskDepart = General;
                PopUpVisible = true;
                TextPopUp = "Edit Data Kiosk";
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            }
            catch { }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void OnCancel()
        {
            FormKioskDepart = new();
            PopUpVisible = false;
        }

        #endregion Button function

        #region Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteKioskDepartementRequest(((KioskDepartementDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<KioskDepartementDto>>();
                    await Mediator.Send(new DeleteListKioskDepartementRequest(a.Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch { }
        }

        #endregion Delete

        #region Save

        private async Task OnSave()
        {
            try
            {
                if (FormKioskDepart.Id == 0)
                {
                    //var a = SelectedServices.Select(x => x.Id).ToList();
                    //FormKioskConfig.ServiceIds?.AddRange(a);
                    await Mediator.Send(new CreateKioskDepartementRequest(FormKioskDepart));
                }
                else
                {
                    //FormKioskConfig.ServiceIds = SelectedServices.Select(x => x.Id).ToList();
                    await Mediator.Send(new UpdateKioskDepartementRequest(FormKioskDepart));
                }
                await LoadData();
            }
            catch { }
        }

        #endregion Save
    }
}
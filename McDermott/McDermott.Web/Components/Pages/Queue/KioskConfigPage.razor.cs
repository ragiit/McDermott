using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.KioskConfigCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class KioskConfigPage
    {
        #region Relation Data

        private List<KioskConfigDto> kioskConfigs = [];
        private IEnumerable<ServiceDto> Services = [];
        private IEnumerable<ServiceDto> SelectedServices { get; set; } = [];
        private KioskConfigDto FormKioskConfig = new();

        #endregion Relation Data

        #region Setting Grid

        private string width = null, height = null;   
        private bool PanelVisible { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;
        private string TextPopUp { get; set; } = string.Empty;
        public IGrid Grid { get; set; }
        private List<string> Names { get; set; } = new();

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }


        #endregion Setting Grid

        private void SelectedItemsChanged(IEnumerable<KioskConfigDto> e)
        {
            if (e is not null)
            {
                Names.Clear();

                foreach (var item in e)
                {
                    var n = item.ServiceName.Split(",");

                    foreach (var item1 in n)
                    {
                        if (Names.Contains(item1))
                            continue;

                        Names.Add(item1);
                    }
                }
            }
        }

        #region async data
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
            PopUpVisible = false;
            SelectedDataItems = new ObservableRangeCollection<object>();
            var a = await Mediator.Send(new GetKioskConfigQuery());
            var service = await Mediator.Send(new GetServiceQuery());
            Services = [.. service.Where(x => x.IsPatient == true)];
            a.ForEach(x => x.ServiceName = string.Join(",", Services.Where(z => x.ServiceIds != null && x.ServiceIds.Contains(z.Id)).Select(x => x.Name).ToList()));
            kioskConfigs = a;
            PanelVisible = false;
        }

        #endregion async data

        #region Config Grid

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
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

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Config Grid

        #region Button function

        private async Task NewItem_Click(IGrid context)
        {
            FormKioskConfig = new();
            PopUpVisible = true;
            TextPopUp = "Add Config Kiosk";
        }

        private async Task EditItem_Click()
        {
            TextPopUp = "Edit Config Kiosk";
            FormKioskConfig = SelectedDataItems[0].Adapt<KioskConfigDto>();
            SelectedServices = Services.Where(x => FormKioskConfig.ServiceIds.Contains(x.Id)).ToList();
            PopUpVisible = true;
        }

        private async void OnRenderTo(KioskConfigDto context)
        {
            var configId = context.Id;
            NavigationManager.NavigateTo($"queue/kiosk/{configId}", true);
        }

        private void OnCancel()
        {
            FormKioskConfig = new();
            PopUpVisible = false;
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
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

        #endregion Button function

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteKioskConfigRequest(((KioskConfigDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<KioskConfigDto>>();
                    await Mediator.Send(new DeleteListKioskConfigRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private async Task OnSave()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FormKioskConfig.Name))
                    return;

                if (FormKioskConfig.Id == 0)
                {
                    var a = SelectedServices.Select(x => x.Id).ToList();
                    FormKioskConfig.ServiceIds?.AddRange(a);
                    await Mediator.Send(new CreateKioskConfigRequest(FormKioskConfig));
                }
                else
                {
                    FormKioskConfig.ServiceIds = SelectedServices.Select(x => x.Id).ToList();
                    await Mediator.Send(new UpdateKioskConfigRequest(FormKioskConfig));
                }
                await LoadData();
            }
            catch { }
        }
    }
}
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
                var user = await UserInfoService.GetUserInfo(ToastService);
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

            await LoadDataKioskConfig();
        }

        private List<CountryDto> Countries { get; set; }
        private long aaa { get; set; }

        #endregion async data

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadDataKioskConfig(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadDataKioskConfig(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadDataKioskConfig(newPageIndex, pageSize);
        }

        private async Task LoadDataKioskConfig(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetKioskConfigQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                });
                kioskConfigs = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = pageIndex;

                foreach (var q in kioskConfigs)
                {
                    var s = (await Mediator.Send(new GetServiceQuery
                    {
                        Predicate = x => q.ServiceIds != null && q.ServiceIds.Contains(x.Id)
                    })).Item1;

                    q.ServiceName = string.Join(", ", s.Select(z => z.Name).ToList());
                }

                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

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
            await Grid.StartEditNewRowAsync();

            return;
            FormKioskConfig = new();
            PopUpVisible = true;
            TextPopUp = "Add Config Kiosk";
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);

            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as KioskConfigDto ?? new());

            var selectedCounter = (await Mediator.Send(new GetServiceQuery
            {
                Predicate = x => a.ServiceIds != null && a.ServiceIds.Contains(x.Id),
            })).Item1;
            Services = selectedCounter;
            SelectedServices = selectedCounter;
            return;
            TextPopUp = "Edit Config Kiosk";
            FormKioskConfig = SelectedDataItems[0].Adapt<KioskConfigDto>();
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
                    await Mediator.Send(new DeleteKioskConfigRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadDataKioskConfig();
            }
            catch { }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                FormKioskConfig = (KioskConfigDto)e.EditModel;
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
                await LoadDataKioskConfig();
            }
            catch { }
        }

        #region ComboboxService

        private DxComboBox<ServiceDto?, long?> refServiceComboBox { get; set; }
        private int ServiceComboBoxIndex { get; set; } = 0;
        private int totalCountService = 0;

        public string SearchTextService { get; set; }

        private async Task OnSearchService(string e)
        {
            SearchTextService = e;
            await LoadDataService();
        }

        private async Task OnSearchServiceClick()
        {
            await LoadDataService();
        }

        private async Task OnSearchServiceIndexIncrement()
        {
            if (ServiceComboBoxIndex < (totalCountService - 1))
            {
                ServiceComboBoxIndex++;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchServicendexDecrement()
        {
            if (ServiceComboBoxIndex > 0)
            {
                ServiceComboBoxIndex--;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnInputServiceChanged(string e)
        {
            ServiceComboBoxIndex = 0;
            await LoadDataService();
        }

        private async Task LoadDataService(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetServiceQuery
                {
                    Predicate = x => x.IsPatient == true,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = refServiceComboBox?.Text ?? ""
                });
                Services = result.Item1.Where(x => !SelectedServices.Select(z => z.Id).Contains(x.Id)).ToList();
                totalCountService = result.PageCount;

                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxService
    }
}
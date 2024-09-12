namespace McDermott.Web.Components.Pages.Medical
{
    public partial class ServicePage
    {
        public List<ServiceDto> Services = [];
        public List<ServiceDto> ServicesK = new();
        public ServiceDto FormService = new();

        #region Default Grid

        private bool PanelVisible { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;
        private string TextPopUp { get; set; } = string.Empty;
        private Timer _timer;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

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

        private string KioskName { get; set; } = String.Empty;

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadData(newPageIndex, pageSize);
        }

        #endregion Searching

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            await GetUserInfo();
            PanelVisible = false;

            return;

            try
            {
                _timer = new Timer(async (_) => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await GetUserInfo();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            PopUpVisible = false;
            SelectedDataItems = new ObservableRangeCollection<object>();
            var result = await Mediator.Send(new GetServiceQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            Services = result.Item1;
            totalCount = result.pageCount;
            ServicesK = [.. Services.Where(x => x.IsKiosk == true).ToList()];

            foreach (var i in Services)
            {
                if (i.IsKiosk == true && i.IsPatient == false)
                {
                    i.Flag = "Counter";
                    if (i.ServicedId != null && i.ServicedId != 0)
                    {
                        i.KioskName = Services.Where(x => x.Id == i.ServicedId).Select(z => z.Name).FirstOrDefault();
                    }
                    else
                    {
                        i.KioskName = "-";
                    }
                }
                else if (i.IsKiosk == false && i.IsPatient == true)
                {
                    i.Flag = "Patient";
                    if (i.ServicedId != null && i.ServicedId != 0)
                    {
                        i.KioskName = Services.Where(x => x.Id == i.ServicedId).Select(z => z.Name).FirstOrDefault();
                    }
                    else
                    {
                        i.KioskName = "-";
                    }
                }
                else if (i.IsKiosk == true && i.IsPatient == true)
                {
                    i.Flag = " Patient, Counter";
                    if (i.ServicedId != null && i.ServicedId != 0)
                    {
                        i.KioskName = Services.Where(x => x.Id == i.ServicedId).Select(z => z.Name).FirstOrDefault();
                    }
                    else
                    {
                        i.KioskName = "-";
                    }
                }
                else
                {
                    i.Flag = "-";
                    i.KioskName = "-";
                }
            }

            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteServiceRequest(((ServiceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<ServiceDto>>();
                    await Mediator.Send(new DeleteServiceRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            
            try
            {
                var editModel = (ServiceDto)e.EditModel;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateServiceRequest(editModel));
                else
                    await Mediator.Send(new UpdateServiceRequest(editModel));

                //await hubConnection.SendAsync("SendCountry", editModel);

                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            EditItemsEnabled = true;
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

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();

            return;
            FormService = new();
            PopUpVisible = true;
            TextPopUp = "Add Services";
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            return;
            FormService = SelectedDataItems[0].Adapt<ServiceDto>();

            PopUpVisible = true;
            TextPopUp = "Edit Services";
        }

        private void OnCancel()
        {
            FormService = new();
            PopUpVisible = false;
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        
        

        #endregion Default Grid
    }
}
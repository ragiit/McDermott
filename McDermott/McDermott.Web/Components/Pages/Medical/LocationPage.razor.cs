using DocumentFormat.OpenXml.Spreadsheet;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class LocationPage
    {
        public List<LocationDto> Locations = [];
        public List<LocationDto> ParentLocations = [];
        public List<CompanyDto> Companies = [];

        private List<string> Types = new()
        {
            "Internal Location"
        };

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

        #region Default Grid

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private Timer _timer;
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

        #region Load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataParentLocations();
            await LoadDataCompanies();
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
            try
            {
                PanelVisible = true;
                var result = await Mediator.QueryGetHelper<Locations, LocationDto>(pageIndex, pageSize, searchTerm);
                Locations = result.Item1;
                totalCount = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Load Data

        #region Load ComboBox

        private DxComboBox<LocationDto, long?> refParentLocationsComboBox { get; set; }
        private int ParentLocationsComboBoxIndex { get; set; } = 0;
        private int totalCountParentLocations = 0;

        private async Task OnSearchParentLocations()
        {
            await LoadDataParentLocations(0, 10);
        }

        private async Task OnSearchParentLocationsIndexIncrement()
        {
            if (ParentLocationsComboBoxIndex < (totalCountParentLocations - 1))
            {
                ParentLocationsComboBoxIndex++;
                await LoadDataParentLocations(ParentLocationsComboBoxIndex, 10);
            }
        }

        private async Task OnSearchParentLocationsndexDecrement()
        {
            if (ParentLocationsComboBoxIndex > 0)
            {
                ParentLocationsComboBoxIndex--;
                await LoadDataParentLocations(ParentLocationsComboBoxIndex, 10);
            }
        }

        private async Task OnInputParentLocationsChanged(string e)
        {
            ParentLocationsComboBoxIndex = 0;
            await LoadDataParentLocations(0, 10);
        }

        private async Task LoadDataParentLocations(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetLocationQuery(x => x.ParentLocationId != null, searchTerm: refParentLocationsComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
                if (result.Item1 != null)
                {
                    ParentLocations = [.. result.Item1.Where(x => x.ParentLocationId is not null).OrderBy(x => x.Name)];
                    totalCountParentLocations = result.pageCount;
                }
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private DxComboBox<CompanyDto, long?> refCompaniesComboBox { get; set; }
        private int CompaniesComboBoxIndex { get; set; } = 0;
        private int totalCountCompanies = 0;

        private async Task OnSearchCompanies()
        {
            await LoadDataCompanies(0, 10);
        }

        private async Task OnSearchCompaniesIndexIncrement()
        {
            if (CompaniesComboBoxIndex < (totalCountCompanies - 1))
            {
                CompaniesComboBoxIndex++;
                await LoadDataCompanies(CompaniesComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCompaniesndexDecrement()
        {
            if (CompaniesComboBoxIndex > 0)
            {
                CompaniesComboBoxIndex--;
                await LoadDataCompanies(CompaniesComboBoxIndex, 10);
            }
        }

        private async Task OnInputCompaniesChanged(string e)
        {
            CompaniesComboBoxIndex = 0;
            await LoadDataCompanies(0, 10);
        }

        private async Task LoadDataCompanies(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetCompanyQuery(searchTerm: refCompaniesComboBox?.Text ?? "", pageSize: pageSize, pageIndex: pageIndex));
                Companies = result.Item1;
                totalCountCompanies = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Load ComboBox

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems is not null && SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteLocationRequest(((LocationDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<LocationDto>>();
                    await Mediator.Send(new DeleteLocationRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (LocationDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateLocationRequest(editModel));
                else
                    await Mediator.Send(new UpdateLocationRequest(editModel));

                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            EditItemsEnabled = true;
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            try
            {
                PanelVisible = true;
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
                var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as LocationDto ?? new());
                ParentLocations = (await Mediator.QueryGetHelper<Locations, LocationDto>(predicate: x => x.Id == a.ParentLocationId)).Item1;
                Companies = (await Mediator.QueryGetHelper<Company, CompanyDto>(predicate: x => x.Id == a.ParentLocationId)).Item1;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #endregion Default Grid
    }
}
namespace McDermott.Web.Components.Pages.Config
{
    public partial class CompanyPage
    {
        private CompanyDto CompanyForm = new();

        public List<CompanyDto> Companys = new();
        public CompanyDto formCompanies = new();
        public CompanyDto DetailCompanies = new();
        public List<CountryDto> Countries { get; set; }
        public List<ProvinceDto> Provinces { get; set; }
        public List<CityDto> Cities { get; set; }

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

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                    }
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

        #region Default Grid Components

        private bool showForm { get; set; } = false;
        private bool isDetail { get; set; } = false;
        private string textPopUp = "";
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool PanelVisible { get; set; } = true;

        private async Task LoadData()
        {
            showForm = false;
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            Companys = await Mediator.Send(new GetCompanyQuery());
            //DetailCompanies = [.. Companys.ToList()];
            PanelVisible = false;
        }

        protected override async Task OnInitializedAsync()
        {
            SelectedDataItems = new ObservableRangeCollection<object>();
            Countries = await Mediator.Send(new GetCountryQuery());
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Cities = await Mediator.Send(new GetCityQuery());

            await GetUserInfo();
            await LoadData();
        }

        private void OnCancel()
        {
            formCompanies = new();
            showForm = false;
            isDetail = false;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            showForm = false;
            isDetail = true;
            var company = SelectedDataItems[0].Adapt<CompanyDto>();
            DetailCompanies = company;
        }

        private async Task OnSave()
        {
            try
            {
                var editModel = formCompanies;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCompanyRequest(editModel));
                else
                    await Mediator.Send(new UpdateCompanyRequest(editModel));

                await LoadData();
            }
            catch { }
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteCompanyRequest(((CompanyDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<CompanyDto>>();
                    await Mediator.Send(new DeleteCompanyRequest(ids: a.Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
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
            isDetail = false;
            showForm = true;
            textPopUp = "Add Data Companies";
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            var company = SelectedDataItems[0].Adapt<CompanyDto>();
            formCompanies = company;
            showForm = true;
            isDetail = false;
            textPopUp = "Edit Data Companies";
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

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
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

        #endregion Default Grid Components
    }
}
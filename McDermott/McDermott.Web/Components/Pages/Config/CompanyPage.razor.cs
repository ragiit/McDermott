using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Medical.DiagnosisCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class CompanyPage
    {
        public List<CompanyDto> Companys = [];
        public List<CountryDto> Countries { get; set; } = [];
        public List<ProvinceDto> Provinces { get; set; } = [];
        public List<CityDto> Cities { get; set; } = [];
        public List<VillageDto> Villages { get; set; } = [];

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

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

        #region Default Grid Components

        private bool showForm { get; set; } = false;
        private bool isDetail { get; set; } = false;
        private string textPopUp = "";
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private bool PanelVisible { get; set; } = true;

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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var countries = await Mediator.Send(new GetCompanyQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            Companys = countries.Item1;
            totalCount = countries.Item4;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataProvince();
            await LoadDataCity();
            await LoadDataCountry();
            PanelVisible = false;
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (CompanyDto)e.EditModel;

                bool validate = await Mediator.Send(new ValidateCompanyQuery(x => x.Id != editModel.Id && x.Name == editModel.Name));

                if (validate)
                {
                    ToastService.ShowInfo($"Company with name '{editModel.Name}' is already exists");
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCompanyRequest(editModel));
                else
                    await Mediator.Send(new UpdateCompanyRequest(editModel));

                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
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

                await LoadData(0, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);

            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as CompanyDto ?? new());

            PanelVisible = true;

            var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
            Countries = resultz.Item1;
            totalCountCountry = resultz.pageCount;

            var result = await Mediator.Send(new GetProvinceQuery(x => x.CountryId == a.CountryId && x.Id == a.ProvinceId));
            Provinces = result.Item1;
            totalCountProvince = result.pageCount;

            var resultx = await Mediator.Send(new GetCityQuery(x => x.Id == a.CityId && x.ProvinceId == a.ProvinceId));
            Cities = resultx.Item1;
            totalCountCity = resultx.pageCount;

            PanelVisible = false;
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #endregion Default Grid Components

        #region ComboboxProvince

        private DxComboBox<ProvinceDto, long?> refProvinceComboBox { get; set; }
        private int ProvinceComboBoxIndex { get; set; } = 0;
        private int totalCountProvince = 0;

        private async Task OnSearchProvince()
        {
            await LoadDataProvince(0, 10);
        }

        private async Task OnSearchProvinceIndexIncrement()
        {
            if (ProvinceComboBoxIndex < (totalCountProvince - 1))
            {
                ProvinceComboBoxIndex++;
                await LoadDataProvince(ProvinceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProvinceIndexDecrement()
        {
            if (ProvinceComboBoxIndex > 0)
            {
                ProvinceComboBoxIndex--;
                await LoadDataProvince(ProvinceComboBoxIndex, 10);
            }
        }

        private async Task OnInputProvinceChanged(string e)
        {
            ProvinceComboBoxIndex = 0;
            await LoadDataProvince(0, 10);
        }

        private async Task LoadDataProvince(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var countryId = refCountryComboBox?.Value.GetValueOrDefault();
            var result = await Mediator.Send(new GetProvinceQuery(x => x.CountryId == countryId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refProvinceComboBox?.Text ?? ""));
            Provinces = result.Item1;
            totalCountProvince = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxProvince

        #region ComboboxCity

        private DxComboBox<CityDto, long?> refCityComboBox { get; set; }
        private int CityComboBoxIndex { get; set; } = 0;
        private int totalCountCity = 0;

        private async Task OnSearchCity()
        {
            await LoadDataCity(0, 10);
        }

        private async Task OnSearchCityIndexIncrement()
        {
            if (CityComboBoxIndex < (totalCountCity - 1))
            {
                CityComboBoxIndex++;
                await LoadDataCity(CityComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCityIndexDecrement()
        {
            if (CityComboBoxIndex > 0)
            {
                CityComboBoxIndex--;
                await LoadDataCity(CityComboBoxIndex, 10);
            }
        }

        private async Task OnInputCityChanged(string e)
        {
            CityComboBoxIndex = 0;
            await LoadDataCity(0, 10);
        }

        private async Task LoadDataCity(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];

            var provinceId = refProvinceComboBox?.Value.GetValueOrDefault();
            var result = await Mediator.Send(new GetCityQuery(x => x.ProvinceId == provinceId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refCityComboBox?.Text ?? ""));
            Cities = result.Item1;
            totalCountCity = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCity

        #region ComboboxCountry

        private DxComboBox<CountryDto, long?> refCountryComboBox { get; set; }
        private int CountryComboBoxIndex { get; set; } = 0;
        private int totalCountCountry = 0;

        private async Task OnSearchCountry()
        {
            await LoadDataCountry(0, 10);
        }

        private async Task OnSearchCountryIndexIncrement()
        {
            if (CountryComboBoxIndex < (totalCountCountry - 1))
            {
                CountryComboBoxIndex++;
                await LoadDataCountry(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCountryIndexDecrement()
        {
            if (CountryComboBoxIndex > 0)
            {
                CountryComboBoxIndex--;
                await LoadDataCountry(CountryComboBoxIndex, 10);
            }
        }

        private async Task OnInputCountryChanged(string e)
        {
            CountryComboBoxIndex = 0;
            await LoadDataCountry(0, 10);
        }

        private async Task LoadDataCountry(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetCountryQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refCountryComboBox?.Text ?? ""));
            Countries = result.Item1;
            totalCountCountry = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxCountry
    }
}
using DevExpress.Blazor.Internal;
using System.Linq.Expressions;

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
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetCompanyQuery
                {
                    SearchTerm = searchTerm ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                Companys = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadCountry();
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
            await LoadCountry();
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            try
            {
                PanelVisible = true;
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
                var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as CompanyDto ?? new());
                await LoadCountry(predicate: x => x.Id == a.CountryId);
                await LoadProvince(predicate: x => x.Id == a.ProvinceId && a.CountryId == x.CountryId);
                await LoadCity(predicate: x => x.Id == a.CityId && x.ProvinceId == a.ProvinceId);
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

        #endregion Default Grid Components

        #region ComboBox

        #region ComboBox Country

        private CountryDto SelectedCountry { get; set; } = new();

        private async Task SelectedItemChanged(CountryDto e)
        {
            if (e is null)
            {
                SelectedCountry = new();
                await LoadCountry();
            }
            else
            {
                SelectedCountry = e;
                await LoadProvince();
            }
        }

        private CancellationTokenSource? _ctsCountry;

        private async Task OnInputCountry(ChangeEventArgs e)
        {
            try
            {
                _ctsCountry?.Cancel();
                _ctsCountry?.Dispose();
                _ctsCountry = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsCountry.Token);

                await LoadCountry(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsCountry?.Dispose();
                _ctsCountry = null;
            }
        }

        private async Task LoadCountry(string? e = "", Expression<Func<Country, bool>>? predicate = null)
        {
            try
            {
                Countries = await Mediator.QueryGetComboBox<Country, CountryDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Country

        #region ComboBox Province

        private ProvinceDto SelectedProvince { get; set; } = new();

        private async Task SelectedItemChanged(ProvinceDto e)
        {
            if (e is null)
            {
                SelectedProvince = new();
                await LoadProvince();
            }
            else
            {
                SelectedProvince = e;
                await LoadCity();
            }
        }

        private CancellationTokenSource? _cts;

        private async Task OnInputProvince(ChangeEventArgs e)
        {
            try
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadProvince(e.Value?.ToString() ?? "", x => x.CountryId == SelectedCountry.Id);
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }

        private async Task LoadProvince(string? e = "", Expression<Func<Province, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.CountryId == SelectedCountry.Id;

                Provinces = await Mediator.QueryGetComboBox<Province, ProvinceDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Province

        #region ComboBox City

        private CityDto SelectedCity { get; set; } = new();

        private async Task SelectedItemChanged(CityDto e)
        {
            if (e is null)
            {
                SelectedCity = new();
                await LoadCity();
            }
            else
                SelectedCity = e;
        }

        private CancellationTokenSource? _ctsCity;

        private async Task OnInputCity(ChangeEventArgs e)
        {
            try
            {
                _ctsCity?.Cancel();
                _ctsCity?.Dispose();
                _ctsCity = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsCity.Token);

                await LoadCity(e.Value?.ToString() ?? "", x => x.ProvinceId == SelectedProvince.Id);
            }
            finally
            {
                _ctsCity?.Dispose();
                _ctsCity = null;
            }
        }

        private async Task LoadCity(string? e = "", Expression<Func<City, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.ProvinceId == SelectedProvince.Id;

                Cities = await Mediator.QueryGetComboBox<City, CityDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox City

        #endregion ComboBox
    }
}
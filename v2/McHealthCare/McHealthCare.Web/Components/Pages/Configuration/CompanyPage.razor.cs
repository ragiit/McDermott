namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class CompanyPage
    {
        #region Variables

        private bool PanelVisible { get; set; } = true;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private List<CompanyDto> Companies = [];
        private List<CountryDto> Countries = [];
        private List<ProvinceDto> Provinces = [];
        private List<CityDto> Cities = [];

        private List<ExportFileData> ExportFileDatas =
        [
            new()
            {
                Column = "Country",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Code",
                Notes = "Mandatory"
            }
        ];

        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Variables

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            try
            {
                UserAccess = await UserService.GetUserInfo(ToastService);
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }
            }
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                Companies = await Mediator.Send(new GetCompanyQuery());
                Provinces = await Mediator.Send(new GetProvinceQuery());
                Countries = await Mediator.Send(new GetCountryQuery());
                Cities = await Mediator.Send(new GetCityQuery());
                SelectedDataItems = [];

                try
                {
                    Grid?.SelectRow(0, true);
                }
                catch { }

                PanelVisible = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteCompanyRequest(((CompanyDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<CompanyDto>>();
                    await Mediator.Send(new DeleteCompanyRequest(Ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                var editModel = (CompanyDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateCompanyRequest(editModel));
                else
                    await Mediator.Send(new UpdateCompanyRequest(editModel));

                await LoadData();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                PanelVisible = false;
            }
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
        }
    }
}
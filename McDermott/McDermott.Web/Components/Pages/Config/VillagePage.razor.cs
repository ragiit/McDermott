namespace McDermott.Web.Components.Pages.Config
{
    public partial class VillagePage
    {
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

        public IGrid Grid { get; set; }
        private List<ProvinceDto> Provinces = new();
        private List<DistrictDto> Districts = new();
        private List<CountryDto> Countrys = new();
        private List<CityDto> Cities = new();

        private List<VillageDto> Villages = new();

        //private object Data { get; set; }
        private IEnumerable<VillageDto> Data { get; set; }

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Countrys = await Mediator.Send(new GetCountryQuery());
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Districts = await Mediator.Send(new GetDistrictQuery());
            Cities = await Mediator.Send(new GetCityQuery());

            await GetUserInfo();
            await LoadData();
        }

        private bool EditItemsEnabled;
        private bool PanelVisible { get; set; } = true;

        private async Task LoadData()
        {
            PanelVisible = true;
            //var dataSource = new GridDevExtremeDataSource<VillageDto>(a.AsQueryable())
            //{
            //    CustomizeLoadOptions = (loadOptions) =>
            //{
            //    // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            //    // This can make SQL execution plans more efficient.
            //    loadOptions.PrimaryKey = new[] { "Id" };
            //    loadOptions.PaginateViaPrimaryKey = true;
            //}
            //};

            Data = await Mediator.Send(new GetVillageQuery());

            SelectedDataItems = [];
            PanelVisible = false;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
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
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            }); ;
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

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var aq = SelectedDataItems.Count;
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteVillageRequest(((VillageDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<VillageDto>>();
                    await Mediator.Send(new DeleteDistrictRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                await JsRuntime.InvokeVoidAsync("alert", ee.InnerException.Message); // Alert
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (VillageDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateVillageRequest(editModel));
            else
                await Mediator.Send(new UpdateVillageRequest(editModel));

            await LoadData();
        }
    }
}
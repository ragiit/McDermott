namespace McDermott.Web.Components.Pages.Config
{
    public partial class CityPage
    {
        private DxUpload MyUpload { get; set; }

        private List<CityDto> Cities = [];
        private List<ProvinceDto> Provinces = [];
        private List<CountryDto> Countriestk = [];
        private HubConnection hubConnection;

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

        #region Default Grid Components

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private string names { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            Provinces = await Mediator.Send(new GetProvinceQuery());
            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            Cities = await Mediator.Send(new GetCityQuery());
            PanelVisible = false;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Province", "Name" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<CityDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var provName = Provinces.FirstOrDefault(x => x.Name == ws.Cells[row, 1].Value?.ToString()?.Trim());

                        if (provName is null)
                        {
                            PanelVisible = false;
                            ToastService.ShowInfo($"Province with name \"{ws.Cells[row, 1].Value?.ToString()?.Trim()}\" is not found");
                            return;
                        }

                        var c = new CityDto
                        {
                            ProvinceId = provName.Id,
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        if (!Cities.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.ProvinceId == c.ProvinceId))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListCityRequest(list));

                    await LoadData();
                    SelectedDataItems = [];

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "city_template.xlsx",
            [
                new()
                {
                    Column = "Province",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
            ]);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteCityRequest(((CityDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<CityDto>>();
                    await Mediator.Send(new DeleteCityRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (CityDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCityRequest(editModel));
                else
                    await Mediator.Send(new UpdateCityRequest(editModel));

                await LoadData();
            }
            catch { }
        }

        #endregion Default Grid Components
    }
}
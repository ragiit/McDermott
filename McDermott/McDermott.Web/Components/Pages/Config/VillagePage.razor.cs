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
                    StateHasChanged();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                        StateHasChanged();
                    }
                }
                catch { }

                Countrys = await Mediator.Send(new GetCountryQuery());
                Provinces = await Mediator.Send(new GetProvinceQuery());
                Districts = await Mediator.Send(new GetDistrictQuery());
                Cities = await Mediator.Send(new GetCityQuery());
                StateHasChanged();
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

        public IGrid Grid { get; set; }
        private List<ProvinceDto> Provinces = [];
        private List<DistrictDto> Districts = [];
        private List<CountryDto> Countrys = [];
        private List<CityDto> Cities = [];
        private List<VillageDto> Villages = [];

        //private object Data { get; set; }
        private IEnumerable<VillageDto> Data { get; set; } = [];

        private bool PanelVisible { get; set; } = true;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            Data = await Mediator.Send(new GetVillageQuery());
            SelectedDataItems = [];
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
                ee.HandleException(ToastService);
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

                    var headerNames = new List<string>() { "Province", "City", "District", "Name", "Postal Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<VillageDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = Provinces.FirstOrDefault(x => x.Name == ws.Cells[row, 1].Value?.ToString()?.Trim());

                        if (a is null)
                        {
                            PanelVisible = false;
                            ToastService.ShowInfo($"Province with name \"{ws.Cells[row, 1].Value?.ToString()?.Trim()}\" is not found");
                            return;
                        }

                        var b = Cities.FirstOrDefault(x => x.Name == ws.Cells[row, 2].Value?.ToString()?.Trim());

                        if (b is null)
                        {
                            PanelVisible = false;
                            ToastService.ShowInfo($"City with name \"{ws.Cells[row, 2].Value?.ToString()?.Trim()}\" is not found");
                            return;
                        }

                        var c = Districts.FirstOrDefault(x => x.Name == ws.Cells[row, 3].Value?.ToString()?.Trim());

                        if (c is null)
                        {
                            PanelVisible = false;
                            ToastService.ShowInfo($"District with name \"{ws.Cells[row, 3].Value?.ToString()?.Trim()}\" is not found");
                            return;
                        }

                        var ee = new VillageDto
                        {
                            ProvinceId = a.Id,
                            CityId = b.Id,
                            DistrictId = c.Id,
                            Name = ws.Cells[row, 4].Value?.ToString()?.Trim(),
                            PostalCode = ws.Cells[row, 5].Value?.ToString()?.Trim(),
                        };

                        if (!Villages.Any(x => x.CityId == ee.CityId && x.ProvinceId == ee.ProvinceId && x.DistrictId == ee.DistrictId && x.Name.Trim().ToLower() == ee?.Name?.Trim().ToLower() && x.PostalCode.Trim().ToLower() == ee?.PostalCode?.Trim().ToLower()))
                            list.Add(ee);
                    }

                    await Mediator.Send(new CreateListVillageRequest(list));

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
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "village_template.xlsx",
            [
                new()
                {
                    Column = "Province",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "City",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "District",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Postal Code"
                },
            ]);
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }
    }
}
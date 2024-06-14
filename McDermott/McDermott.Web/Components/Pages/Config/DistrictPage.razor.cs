namespace McDermott.Web.Components.Pages.Config
{
    public partial class DistrictPage
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
                var user = await UserInfoService.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        public IGrid Grid { get; set; }
        private List<DistrictDto> Districts = new();
        private List<ProvinceDto> Provinces = new();
        private List<CityDto> Cities = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private dynamic dd;
        private int Value { get; set; } = 0;
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private bool PanelVisible { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Cities = await Mediator.Send(new GetCityQuery());
            await GetUserInfo();
            await LoadData();
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "district_template.xlsx",
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
                    Column = "Name",
                    Notes = "Mandatory"
                },
            ]);
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

                    var headerNames = new List<string>() { "Province", "City", "Name" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<DistrictDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var provName = Provinces.FirstOrDefault(x => x.Name == ws.Cells[row, 1].Value?.ToString()?.Trim());

                        if (provName is null)
                        {
                            PanelVisible = false;
                            ToastService.ShowInfo($"Province with name \"{ws.Cells[row, 1].Value?.ToString()?.Trim()}\" is not found");
                            return;
                        }

                        var cityName = Cities.FirstOrDefault(x => x.Name == ws.Cells[row, 2].Value?.ToString()?.Trim());

                        if (cityName is null)
                        {
                            PanelVisible = false;
                            ToastService.ShowInfo($"City with name \"{ws.Cells[row, 1].Value?.ToString()?.Trim()}\" is not found");
                            return;
                        }

                        var c = new DistrictDto
                        {
                            ProvinceId = provName.Id,
                            CityId = cityName.Id,
                            Name = ws.Cells[row, 3].Value?.ToString()?.Trim(),
                        };

                        if (!Districts.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.ProvinceId == c.ProvinceId && x.CityId == c.CityId))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListDistrictRequest(list));

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

        private async Task LoadData()
        {
            SelectedDataItems = new ObservableRangeCollection<object>();
            Districts = await Mediator.Send(new GetDistrictQuery());
        }

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
            UpdateEditItemsEnabled(true);
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

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
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

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var aq = SelectedDataItems.Count;
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteDistrictRequest(((DistrictDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DistrictDto>>();
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
            var editModel = (DistrictDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateDistrictRequest(editModel));
            else
                await Mediator.Send(new UpdateDistrictRequest(editModel));

            await LoadData();
        }
    }
}
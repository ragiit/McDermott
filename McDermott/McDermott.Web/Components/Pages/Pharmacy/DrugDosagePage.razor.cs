namespace McDermott.Web.Components.Pages.Pharmacy
{
    public partial class DrugDosagePage
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

        #region Static

        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        public bool IsAddForm { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private List<DrugDosageDto> GetDrugDosages = [];
        private List<DrugRouteDto> DrugRoutes = [];

        #endregion Static

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

        #region Load

        protected override async Task OnInitializedAsync()
        {
            IsAccess = false;

            //DrugRoutes = await Mediator.Send(new GetDrugRouteQuery());

            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetDrugDosageQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            GetDrugDosages = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion Load

        #region Click

        private async Task NewItem_Click()
        {
            IsAddForm = true;
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            IsAddForm = false;
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

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "DrugDosages_template.xlsx",
            [
                new() { Column = "Frequency", Notes = "Mandatory" },
                new() { Column = "Total Qty Per Day" },
                new() { Column = "Days" },
                new() { Column = "Route"}
            ]);
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

                    var headerNames = new List<string>() { "Frequency", "Total QTy PerDay", "Day", "Route" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<DrugDosageDto>();

                    var sampleTypes = new HashSet<string>();
                    var list1 = new List<DrugRouteDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 3].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            sampleTypes.Add(a.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetDrugRouteQuery(x => sampleTypes.Contains(x.Route.ToLower()), 0, 0))).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        var a = ws.Cells[row, 4].Value?.ToString()?.Trim();

                        long? sampleTypeId = null;
                        if (!string.IsNullOrEmpty(a))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Route.Equals(a, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 3, a ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                sampleTypeId = cachedParent.Id;
                            }
                        }
                        else
                        {
                            ToastService.ShowErrorImport(row, 3, a ?? string.Empty);
                            isValid = false;
                        }

                        if (!isValid)
                            continue;

                        list.Add(new DrugDosageDto
                        {
                            Frequency = ws.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty,
                            TotalQtyPerDay = float.Parse(ws.Cells[row, 2].Value?.ToString()?.Trim() ?? "0", CultureInfo.InvariantCulture),
                            Days = float.Parse(ws.Cells[row, 3].Value?.ToString()?.Trim() ?? "0", CultureInfo.InvariantCulture),

                        });
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Frequency, x.TotalQtyPerDay,x.Days, }).ToList();

                        // Panggil BulkValidateLabTestQuery untuk validasi bulk
                        var existingLabTests = await Mediator.Send(new BulkValidateDrugDosageQuery(list));

                        // Filter LabTest baru yang tidak ada di database
                        list = list.Where(DrugDosage =>
                            !existingLabTests.Any(ev =>
                                ev.Frequency == DrugDosage.Frequency &&
                                ev.TotalQtyPerDay == DrugDosage.TotalQtyPerDay &&
                                ev.Days == DrugDosage.Days 
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListDrugDosageRequest(list));
                        await LoadData(0, pageSize);
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
                finally { PanelVisible = false; }
            }
            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteDrugDosageRequest(((DrugDosageDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteDrugDosageRequest(ids: SelectedDataItems.Adapt<List<DrugDosageDto>>().Select(x => x.Id).ToList()));
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
            var editModel = (DrugDosageDto)e.EditModel;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateDrugDosageRequest(editModel));
            else
                await Mediator.Send(new UpdateDrugDosageRequest(editModel));

            await LoadData();
        }

        #endregion Click

        #region Grid

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

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Grid
    }
}
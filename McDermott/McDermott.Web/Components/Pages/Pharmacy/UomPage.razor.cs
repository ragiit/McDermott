using McDermott.Domain.Entities;

namespace McDermott.Web.Components.Pages.Pharmacy
{
    public partial class UomPage
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
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private List<UomCategoryDto> UomCategories = [];
        private List<UomDto> Uoms = [];

        private List<string> Types = new List<string>
        {
            "Bigger than the reference Unit of Measure",
            "Reference Unit of Measure for this category",
            "Smaller than the reference Unit of Measure",
        };

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
            await GetUserInfo();
            await LoadDataUomCategory();
            await LoadData();
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetUomQuery(searchTerm: searchTerm, pageIndex: pageIndex, pageSize: pageSize));
                Uoms = result.Item1;
                totalCount = result.pageCount;
                PanelVisible = false;
            }catch(Exception ex) {
                ex.HandleException(ToastService);
            }
        }

        #endregion Load

        #region Combo Box Lab Uoms

        private DxComboBox<UomCategoryDto, long?> refUomCategoryComboBox { get; set; }
        private int UomCategoryComboBoxIndex { get; set; } = 0;
        private int totalCountUomCategory = 0;

        private async Task OnSearchUomCategory()
        {
            await LoadDataUomCategory(0, 10);
        }

        private async Task OnSearchUomCategoryIndexIncrement()
        {
            if (UomCategoryComboBoxIndex < (totalCountUomCategory - 1))
            {
                UomCategoryComboBoxIndex++;
                await LoadDataUomCategory(UomCategoryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUomCategoryIndexDecrement()
        {
            if (UomCategoryComboBoxIndex > 0)
            {
                UomCategoryComboBoxIndex--;
                await LoadDataUomCategory(UomCategoryComboBoxIndex, 10);
            }
        }

        private async Task OnInputUomCategoryChanged(string e)
        {
            UomCategoryComboBoxIndex = 0;
            await LoadDataUomCategory(0, 10);
        }

        private async Task LoadDataUomCategory(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetUomCategoryQuery(searchTerm: refUomCategoryComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            UomCategories = result.Item1;
            totalCountUomCategory = result.pageCount;
            PanelVisible = false;
        }

        #endregion Combo Box Lab Uoms


        #region Click

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
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteUomRequest(((UomDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteUomRequest(ids: SelectedDataItems.Adapt<List<UomDto>>().Select(x => x.Id).ToList()));
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
            var editModel = (UomDto)e.EditModel;
            bool exists = await Mediator.Send(new ValidateUomQuery(x => x.Id != editModel.Id && x.Name == editModel.Name ));
            if (exists)
            {
                ToastService.ShowWarning($"Uom with name '{editModel.Name}' already exists.");
                return;
            }
            if (editModel.Id == 0)
                await Mediator.Send(new CreateUomRequest(editModel));
            else
                await Mediator.Send(new UpdateUomRequest(editModel));

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

        #region Import && Export
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

                    var headerNames = new List<string>() { "Name", "Type", "Category uom" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<UomDto>();

                    var sampleTypes = new HashSet<string>();
                    var list1 = new List<UomCategoryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 3].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            sampleTypes.Add(a.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetUomCategoryQuery(x => sampleTypes.Contains(x.Name.ToLower()), 0, 0))).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        var a = ws.Cells[row, 3].Value?.ToString()?.Trim();

                        long? CategoryUomId = null;
                        if (!string.IsNullOrEmpty(a))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(a, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 3, a ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                CategoryUomId = cachedParent.Id;
                            }
                        }
                        else
                        {
                            ToastService.ShowErrorImport(row, 3, a ?? string.Empty);
                            isValid = false;
                        }

                        var resultType = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        if (!string.IsNullOrWhiteSpace(resultType))
                        {
                            var exist = Helper.ResultValueTypes.Any(x => x.Equals(resultType, StringComparison.CurrentCultureIgnoreCase));
                            if (!exist)
                            {
                                ToastService.ShowErrorImport(row, 4, resultType ?? string.Empty);
                                isValid = false;
                            }
                        }
                        else
                        {
                            ToastService.ShowErrorImport(row, 4, resultType ?? string.Empty);
                            isValid = false;
                        }

                        if (!isValid)
                            continue;

                        list.Add(new UomDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Type = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                            UomCategoryId = CategoryUomId,
                        });
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Type, }).ToList();

                        // Panggil BulkValidateLabTestQuery untuk validasi bulk
                        var existingLabTests = await Mediator.Send(new BulkValidateUomQuery(list));

                        // Filter LabTest baru yang tidak ada di database
                        list = list.Where(Uom =>
                            !existingLabTests.Any(ev =>
                                ev.Name == Uom.Name &&
                                ev.Type == Uom.Type &&
                                ev.UomCategoryId == Uom.UomCategoryId 
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListUomRequest(list));
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

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "Uom_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Type"
                },
                new()
                {
                    Column = "Category Uom",
                },
               
            ]);
        }

        #endregion
    }
}
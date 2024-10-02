using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Domain.Entities;
using McDermott.Persistence.Migrations;

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
                var result = await Mediator.QueryGetHelper<Uom, UomDto>(pageIndex, pageSize, searchTerm);
                Uoms = result.Item1;
                totalCount = result.pageCount;
                activePageIndex = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
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
            var result = await Mediator.QueryGetHelper<UomCategory, UomCategoryDto>(pageIndex, pageSize, refUomCategoryComboBox?.Text ?? "");
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
            try
            {
                PanelVisible = true;
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
                var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as UomDto ?? new());
                UomCategories = (await Mediator.QueryGetHelper<UomCategory, UomCategoryDto>(predicate: x => x.Id == a.UomCategoryId)).Item1;
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

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;

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
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                var editModel = (UomDto)e.EditModel;
                bool exists = await Mediator.Send(new ValidateUomQuery(x => x.Id != editModel.Id && x.Name == editModel.Name));
                if (exists)
                {
                    ToastService.ShowWarning($"Uom with name '{editModel.Name}' already exists.");
                    return;
                }
                if (editModel.Id == 0)
                    await Mediator.Send(new CreateUomRequest(editModel));
                else
                    await Mediator.Send(new UpdateUomRequest(editModel));

                await LoadData(0, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Click

        #region Grid

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

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => ExportTemp.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<UomDto>();

                    var uomNames = new HashSet<string>();
                    var list1 = new List<UomCategoryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 2].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            uomNames.Add(a.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetUomCategoryQuery(x => uomNames.Contains(x.Name.ToLower()), 0, 0))).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        var name = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var uomCategory = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var type = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var ratio = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        var active = ws.Cells[row, 5].Value?.ToString()?.Trim();
                        var roundingPrecision = ws.Cells[row, 6].Value?.ToString()?.Trim();

                        long? CategoryUomId = null;
                        if (!string.IsNullOrEmpty(uomCategory))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(uomCategory, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 2, uomCategory ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                CategoryUomId = cachedParent.Id;
                            }
                        }
                        else
                        {
                            ToastService.ShowErrorImport(row, 2, uomCategory ?? string.Empty);
                            isValid = false;
                        }

                        if (!string.IsNullOrEmpty(type) && !Types.Contains(type))
                        {
                            isValid = false;
                            ToastService.ShowErrorImport(row, 3, type ?? string.Empty);
                        }

                        // Parse string to float
                        if (float.TryParse(roundingPrecision, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
                        {
                            roundingPrecision = result.ToString();
                        }
                        else
                        {
                            // Handle the case when the string cannot be parsed
                            roundingPrecision = null; // or set to a default value if needed
                        }

                        if (float.TryParse(ratio, NumberStyles.Float, CultureInfo.InvariantCulture, out var rr))
                        {
                            ratio = rr.ToString();
                        }
                        else
                        {
                            // Handle the case when the string cannot be parsed
                            ratio = null; // or set to a default value if needed
                        }

                        if (!isValid)
                            continue;

                        list.Add(new UomDto
                        {
                            Name = name,
                            UomCategoryId = CategoryUomId,
                            Type = type,
                            BiggerRatio = type == "Reference Unit of Measure for this category" ? 0 : ratio.ToLong(),
                            Active = active == "Yes",
                            RoundingPrecision = float.Parse(roundingPrecision)
                        });
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Type, x.UomCategoryId }).ToList();

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

        private List<ExportFileData> ExportTemp =
        [
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "UoM Category",
            },
            new()
            {
                Column = "Type",
                Notes = "Select one: Bigger than the reference Unit of Measure, Reference Unit of Measure for this category, Smaller than the reference Unit of Measure"
            },
            new()
            {
                Column = "Ratio"
            },
            new()
            {
                Column = "Active",
                Notes = "Select one: Yes/No"
            },
            new()
            {
                Column = "Rounding Precision"
            },
        ];

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "uom_template.xlsx", ExportTemp);
        }

        #endregion Import && Export
    }
}
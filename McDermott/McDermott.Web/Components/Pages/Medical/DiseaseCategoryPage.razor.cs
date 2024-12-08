namespace McDermott.Web.Components.Pages.Medical
{
    public partial class DiseaseCategoryPage
    {
        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private List<DiseaseCategoryDto> DiseaseCategorys = [];
        private List<DiseaseCategoryDto> ParentCategoryDto = [];
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }

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

        #region ComboboxDiseaseCategory

        private DxComboBox<DiseaseCategoryDto, long?> refDiseaseCategoryComboBox { get; set; }
        private int DiseaseCategoryComboBoxIndex { get; set; } = 0;
        private int totalCountDiseaseCategory = 0;

        private async Task OnSearchDiseaseCategory()
        {
            await LoadDataDiseaseCategory();
        }

        private async Task OnSearchDiseaseCategoryIndexIncrement()
        {
            if (DiseaseCategoryComboBoxIndex < (totalCountDiseaseCategory - 1))
            {
                DiseaseCategoryComboBoxIndex++;
                await LoadDataDiseaseCategory(DiseaseCategoryComboBoxIndex, 10);
            }
        }

        private async Task OnSearchDiseaseCategoryndexDecrement()
        {
            if (DiseaseCategoryComboBoxIndex > 0)
            {
                DiseaseCategoryComboBoxIndex--;
                await LoadDataDiseaseCategory(DiseaseCategoryComboBoxIndex, 10);
            }
        }

        private async Task OnInputDiseaseCategoryChanged(string e)
        {
            DiseaseCategoryComboBoxIndex = 0;
            await LoadDataDiseaseCategory();
        }

        private async Task LoadDataDiseaseCategory(int pageIndex = 0, int pageSize = 10, long? a = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetDiseaseCategoryQuery(a == null ? null : x => x.ParentDiseaseCategoryId == null && x.Id == a, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDiseaseCategoryComboBox?.Text ?? ""));
            ParentCategoryDto = result.Item1;
            totalCountDiseaseCategory = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDiseaseCategory

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var a = await Mediator.Send(new GetDiseaseCategoryQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            DiseaseCategorys = a.Item1;
            totalCount = a.pageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "disease_category_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Parent"
                },
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

                    var headerNames = new List<string>() { "Name", "Parent", };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<DiseaseCategoryDto>();

                    var parentNames = new HashSet<string>();

                    var list1 = new List<DiseaseCategoryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 2].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            parentNames.Add(a.ToLower());
                    }

                    list1 = (await Mediator.Send(new GetDiseaseCategoryQuery(x => parentNames.Contains(x.Name.ToLower()), 0, 0,
                        select: x => new DiseaseCategory
                        {
                            Id = x.Id,
                            Name = x.Name
                        }))).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;
                        string parentName = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        long? parentId = null;
                        if (!string.IsNullOrEmpty(parentName))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(parentName, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 2, parentName ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                parentId = cachedParent.Id;
                            }
                        }

                        // Lewati baris jika tidak valid
                        if (!isValid)
                            continue;

                        var country = new DiseaseCategoryDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            ParentDiseaseCategoryId = parentId,
                        };

                        list.Add(country);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.ParentDiseaseCategoryId, }).ToList();

                        // Panggil BulkValidateCountryQuery untuk validasi bulk
                        var existingCountrys = await Mediator.Send(new BulkValidateDiseaseCategoryQuery(list));

                        // Filter Country baru yang tidak ada di database
                        list = list.Where(Country =>
                            !existingCountrys.Any(ev =>
                                ev.Name == Country.Name &&
                                ev.ParentDiseaseCategoryId == Country.ParentDiseaseCategoryId
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListDiseaseCategoryRequest(list));
                        await LoadData(0, pageSize);
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
                finally { PanelVisible = false; }
            }
            PanelVisible = false;
        }

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
            await LoadDataDiseaseCategory();
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

            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
            await LoadComboboxEdit(a);
        }

        private async Task LoadComboboxEdit(DiseaseCategoryDto a)
        {
            ParentCategoryDto = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.ParentDiseaseCategoryId == null && x.Id == a.ParentDiseaseCategoryId))).Item1;
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

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteDiseaseCategoryRequest(((DiseaseCategoryDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DiseaseCategoryDto>>();
                    await Mediator.Send(new DeleteDiseaseCategoryRequest(ids: a.Select(x => x.Id).ToList()));
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
            var editModel = (DiseaseCategoryDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateDiseaseCategoryRequest(editModel));
            else
                await Mediator.Send(new UpdateDiseaseCategoryRequest(editModel));

            await LoadData();
        }
    }
}
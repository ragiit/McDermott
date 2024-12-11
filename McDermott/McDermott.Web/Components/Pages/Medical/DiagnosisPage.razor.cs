using System.Linq.Expressions;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class DiagnosisPage
    {
        private bool PanelVisible { get; set; } = true;

        public IGrid Grid { get; set; }
        private List<DiagnosisDto> Diagnoses = new();
        private List<DiseaseCategoryDto> Diseases = new();
        private List<CronisCategoryDto> Cronises = new();

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

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

        private List<ExportFileData> ExportTemp =
        [
            new() { Column = "Code"},
            new() { Column = "Name (en)", Notes = "Mandatory" },
            new() { Column = "Name (id)" },
            new() { Column = "Chronic Category"},
        ];

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "diagnosis_template.xlsx", ExportTemp);
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            long maxAllowedSize = 3 * 1024 * 1024; // 2MB
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream(maxAllowedSize).CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => ExportTemp.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<DiagnosisDto>();

                    //var a = new HashSet<string>();
                    var b = new HashSet<string>();

                    //var list1 = new List<DiseaseCategoryDto>();
                    var list2 = new List<CronisCategoryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        //var aa = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var bb = ws.Cells[row, 4].Value?.ToString()?.Trim();

                        //if (!string.IsNullOrEmpty(aa))
                        //    a.Add(aa.ToLower());

                        if (!string.IsNullOrEmpty(bb))
                            b.Add(bb.ToLower());
                    }

                    //list1 = (await Mediator.Send(new GetDiseaseCategoryQuery(x => a.Contains(x.Name.ToLower()), 0, 0,
                    //    select: x => new DiseaseCategory
                    //    {
                    //        Id = x.Id,
                    //        Name = x.Name
                    //    }))).Item1;

                    list2 = (await Mediator.Send(new GetCronisCategoryQuery
                    {
                        Predicate = x => b.Contains(x.Name.ToLower()),
                        Select = x => new CronisCategory
                        {
                            Id = x.Id,
                            Name = x.Name
                        },
                        IsGetAll = true
                    }
                    )).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        //var aaa = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var bbb = ws.Cells[row, 4].Value?.ToString()?.Trim();

                        //long? aId = null;
                        //if (!string.IsNullOrEmpty(aaa))
                        //{
                        //    var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(aaa, StringComparison.CurrentCultureIgnoreCase));
                        //    if (cachedParent is null)
                        //    {
                        //        isValid = false;
                        //        ToastService.ShowErrorImport(row, 3, aaa ?? string.Empty);
                        //    }
                        //    else
                        //    {
                        //        aId = cachedParent.Id;
                        //    }
                        //}

                        long? bId = null;
                        if (!string.IsNullOrEmpty(bbb))
                        {
                            var cachedParent = list2.FirstOrDefault(x => x.Name.Equals(bbb, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                isValid = false;
                                ToastService.ShowErrorImport(row, 4, bbb ?? string.Empty);
                            }
                            else
                            {
                                bId = cachedParent.Id;
                            }
                        }

                        if (!isValid)
                            continue;

                        var country = new DiagnosisDto
                        {
                            Code = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim() ?? "",
                            NameInd = ws.Cells[row, 3].Value?.ToString()?.Trim() ?? "",
                            //DiseaseCategoryId = aId,
                            CronisCategoryId = bId,
                        };

                        list.Add(country);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.NameInd, x.Code, x.CronisCategoryId }).ToList();

                        // Panggil BulkValidateVillageQuery untuk validasi bulk
                        var existingVillages = await Mediator.Send(new BulkValidateDiagnosisQuery(list));

                        // Filter village baru yang tidak ada di database
                        list = list.Where(village =>
                            !existingVillages.Any(ev =>
                                ev.Name == village.Name &&
                                ev.NameInd == village.NameInd &&
                                ev.Code == village.Code &&
                                ev.CronisCategoryId == village.CronisCategoryId
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListDiagnosisRequest(list));
                        await LoadData();
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
        }

        #region ComboBox CronisCategory

        private CronisCategoryDto SelectedCronisCategory { get; set; } = new();

        private async Task SelectedItemChanged(CronisCategoryDto e)
        {
            if (e is null)
            {
                SelectedCronisCategory = new();
                await LoadCronisCategory();
            }
            else
                SelectedCronisCategory = e;
        }

        private CancellationTokenSource? _ctsCronisCategory;

        private async Task OnInputCronisCategory(ChangeEventArgs e)
        {
            try
            {
                _ctsCronisCategory?.Cancel();
                _ctsCronisCategory?.Dispose();
                _ctsCronisCategory = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsCronisCategory.Token);

                await LoadCronisCategory(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsCronisCategory?.Dispose();
                _ctsCronisCategory = null;
            }
        }

        private async Task LoadCronisCategory(string? e = "", Expression<Func<CronisCategory, bool>>? predicate = null)
        {
            try
            {
                Cronises = await Mediator.QueryGetComboBox<CronisCategory, CronisCategoryDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox CronisCategory

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

        private async Task LoadDataDiseaseCategory(int pageIndex = 0, int pageSize = 10, long? DiseaseCategoryId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetDiseaseCategoryQuery(DiseaseCategoryId == null ? null : x => x.Id == DiseaseCategoryId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDiseaseCategoryComboBox?.Text ?? ""));
            Diseases = result.Item1;
            totalCountDiseaseCategory = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxDiseaseCategory

        private object Data { get; set; }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var dataSource = new GridDevExtremeDataSource<Diagnosis>(await Mediator.Send(new GetQueryDiagnosis()))
                {
                    CustomizeLoadOptions = (loadOptions) =>
                    {
                        loadOptions.PrimaryKey = ["Id"];
                        loadOptions.PaginateViaPrimaryKey = true;
                    }
                };
                Data = dataSource;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            await LoadCronisCategory();
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiagnosisDto ?? new());
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            await LoadComboboxEdit(a);
        }

        private async Task LoadComboboxEdit(DiagnosisDto a)
        {
            await LoadCronisCategory(predicate: x => x.Id == a.CronisCategoryId);
            Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
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
                    await Mediator.Send(new DeleteDiagnosisRequest(((DiagnosisDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DiagnosisDto>>();
                    await Mediator.Send(new DeleteDiagnosisRequest(ids: a.Select(x => x.Id).ToList()));
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
            try
            {
                var editModel = (Diagnosis)e.EditModel;

                var checkNameEn = await Mediator.Send(new ValidateDiagnosisQuery(x
                    => x.Id != editModel.Id && x.Name == editModel.Name));
                if (checkNameEn)
                {
                    ToastService.ShowInfo($"The diagnosis name (English) '{editModel.Name}' is already in use. Please enter a different name.");
                    e.Cancel = true;
                    return;
                }

                var checkNameIdn = await Mediator.Send(new ValidateDiagnosisQuery(x
                    => x.Id != editModel.Id && x.NameInd == editModel.NameInd));
                if (checkNameEn)
                {
                    ToastService.ShowInfo($"The diagnosis name (Indonesian) '{editModel.NameInd}' is already in use. Please enter a different name.");
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateDiagnosisRequest(editModel.Adapt<DiagnosisDto>()));
                else
                    await Mediator.Send(new UpdateDiagnosisRequest(editModel.Adapt<DiagnosisDto>()));

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }
    }
}
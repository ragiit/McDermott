namespace McDermott.Web.Components.Pages.Medical
{
    public partial class InsurancePage
    {
        public List<InsuranceDto> Insurances = [];

        private List<string> Types = new List<string>
        {
            "Swasta",
            "Negeri",
        };

        private List<ExportFileData> ExportTemp =
        [
            new() { Column = "Name", Notes = "Mandatory" },
            new() { Column = "Code"},
            new() { Column = "Payment Method", Notes = "Select one: Swasta/Negeri"},
            new() { Column = "Admin Fee"},
            new() { Column = "Presentase"},
            new() { Column = "Admin Fee Max"},
            new() { Column = "Select one: BPJS Kesehatan/BPJS TK"},
        ];

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "insurance_template.xlsx", ExportTemp);
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

                    var list = new List<InsuranceDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        var a = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var b = ws.Cells[row, 6].Value?.ToString()?.Trim();
                        if (!Types.Contains(a) && !string.IsNullOrEmpty(a))
                        {
                            isValid = false;
                            ToastService.ShowErrorImport(row, 3, a ?? string.Empty);
                        }

                        bool isKesehatan = false;
                        bool isTK = false;
                        if (!string.IsNullOrWhiteSpace(b))
                        {
                            if (b.Equals("BPJS Kesehatan"))
                                isKesehatan = true;
                            if (b.Equals("BPJS TK"))
                                isTK = true;
                        }

                        if (!isValid)
                            continue;

                        var c = new InsuranceDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                            Type = a,
                            AdminFee = ws.Cells[row, 4].Value.ToLong(),
                            Presentase = ws.Cells[row, 5].Value.ToLong(),
                            AdminFeeMax = ws.Cells[row, 6].Value.ToLong(),
                            IsBPJSKesehatan = isKesehatan,
                            IsBPJSTK = isTK,
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Code, }).ToList();

                        // Panggil BulkValidateInsuranceQuery untuk validasi bulk
                        var existingInsurances = await Mediator.Send(new BulkValidateInsuranceQuery(list));

                        // Filter Insurance baru yang tidak ada di database
                        list = list.Where(Insurance =>
                            !existingInsurances.Any(ev =>
                                ev.Name == Insurance.Name &&
                                ev.Code == Insurance.Code &&
                                ev.Type == Insurance.Name &&
                                ev.AdminFee == Insurance.AdminFee &&
                                ev.Presentase == Insurance.Presentase &&
                                ev.AdminFeeMax == Insurance.AdminFeeMax
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListInsuranceRequest(list));
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

        #region Default Grid

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private Timer _timer;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = new ObservableRangeCollection<object>();
                var result = await Mediator.Send(new GetInsuranceQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                });
                Insurances = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

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

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteInsuranceRequest(((InsuranceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<InsuranceDto>>();
                    await Mediator.Send(new DeleteInsuranceRequest(ids: a.Select(x => x.Id).ToList()));
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
            var editModel = (InsuranceDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateInsuranceRequest(editModel));
            else
                await Mediator.Send(new UpdateInsuranceRequest(editModel));

            await LoadData();
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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            await GetUserInfo();
            PanelVisible = false;

            return;

            try
            {
                _timer = new Timer(async (_) => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await GetUserInfo();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Default Grid
    }
}
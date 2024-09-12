namespace McDermott.Web.Components.Pages.Medical
{
    public partial class NursingDiagnosesPage
    {
        private List<NursingDiagnosesDto> NursingDiagnoses = [];

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

        #region Grid Properties

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

        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }

        public IGrid Grid { get; set; }
        private Timer _timer;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        #endregion Grid Properties

        #region LoadData

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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetNursingDiagnosesQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            NursingDiagnoses = result.Item1;
            totalCount = result.pageCount;
            SelectedDataItems = new ObservableRangeCollection<object>();
            PanelVisible = false;
        }

        #endregion LoadData

        #region Grid Function

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

                    var headerNames = new List<string>() { "Problems", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<NursingDiagnosesDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new NursingDiagnosesDto
                        {
                            Problem = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        if (!NursingDiagnoses.Any(x => x.Problem.Trim().ToLower() == c?.Problem?.Trim().ToLower() && x.Code.Trim().ToLower() == c?.Code?.Trim().ToLower()))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListNursingDiagnosesRequest(list));

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
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "NursingDiagnoses_template.xlsx",
            [
                new()
                {
                    Column = "Problems",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Code"
                },
            ]);
        }


        #region SaveDelete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteNursingDiagnosesRequest(((NursingDiagnosesDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteNursingDiagnosesRequest(ids: SelectedDataItems.Adapt<List<NursingDiagnosesDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch { }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (NursingDiagnosesDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Problem))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateNursingDiagnosesRequest(editModel));
                else
                    await Mediator.Send(new UpdateNursingDiagnosesRequest(editModel));

                await LoadData();
            }
            catch { }
        }

        #endregion SaveDelete

        #region ToolBar Button


        private async Task Refresh_Click()
        {
            await LoadData();
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

        #endregion ToolBar Button

        #endregion Grid Function
    }
}
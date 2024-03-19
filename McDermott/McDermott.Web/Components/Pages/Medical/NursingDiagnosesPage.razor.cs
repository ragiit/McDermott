namespace McDermott.Web.Components.Pages.Medical
{
    public partial class NursingDiagnosesPage
    {
        private List<NursingDiagnosesDto> NursingDiagnoses = [];

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
                var user = await UserInfoService.GetUserInfo();
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
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        #endregion Grid Properties

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            NursingDiagnoses = await Mediator.Send(new GetNursingDiagnosesQuery());
            SelectedDataItems = new ObservableRangeCollection<object>();
            PanelVisible = false;
        }

        #endregion LoadData

        #region Grid Function

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
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

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
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

                    var headerNames = new List<string>() { "Problem", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString().Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match the grid.");
                        return;
                    }

                    var nursings = new List<NursingDiagnosesDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var nursing = new NursingDiagnosesDto
                        {
                            Problem = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim()
                        };

                        if (!NursingDiagnoses.Any(x => x.Problem.Trim().ToLower() == nursing.Problem.Trim().ToLower()) && !nursings.Any(x => x.Problem.Trim().ToLower() == nursing.Problem.Trim().ToLower()))
                            nursings.Add(nursing);
                    }

                    await Mediator.Send(new CreateListNursingDiagnosesRequest(nursings));

                    await LoadData();
                }
                catch { }
            }
        }

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

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile");
        }

        #endregion ToolBar Button

        #endregion Grid Function
    }
}
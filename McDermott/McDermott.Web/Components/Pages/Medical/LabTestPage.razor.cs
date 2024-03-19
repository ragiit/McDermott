namespace McDermott.Web.Components.Pages.Medical
{
    public partial class LabTestPage
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

        #region Relations

        private LabTestDto LabTest = new();
        private List<LabTestDto> LabTests = [];
        private List<LabUomDto> LabUoms = [];
        private List<SampleTypeDto> SampleTypes = [];

        private List<string> ResultTypes =
            [
                "Numeric",
                "Qualitative",
                "Binary"
            ];

        private List<string> ResultValueTypes =
            [
                "Numeric",
                "Qualitative",
                "Binary"
            ];

        #endregion Relations

        #region Static

        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        #endregion Static

        #region SaveDelete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteLabTestRequest(((LabTestDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteLabTestRequest(ids: SelectedDataItems.Adapt<List<LabTestDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                LabTest = ((LabTestDto)e.EditModel);
                if (LabTest.Id == 0)
                    await Mediator.Send(new CreateLabTestRequest(LabTest));
                else
                    await Mediator.Send(new UpdateLabTestRequest(LabTest));

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion SaveDelete

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            LabUoms = await Mediator.Send(new GetLabUomQuery());
            SampleTypes = await Mediator.Send(new GetSampleTypeQuery());

            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            LabTest = new();
            SelectedDataItems = [];
            LabTests = await Mediator.Send(new GetLabTestQuery());
            PanelVisible = false;
        }

        #endregion LoadData

        #region Grid Function

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
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #region ToolBar Button

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles(1))
            {
                //try
                //{
                //    using MemoryStream ms = new();
                //    await file.OpenReadStream().CopyToAsync(ms);
                //    ms.Position = 0;

                //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                //    using ExcelPackage package = new(ms);
                //    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                //    var headerNames = new List<string>() { "Name", "Code" };

                //    if (Enumerable.Range(1, ws.Dimension.End.Column)
                //        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString().Trim().ToLower()))
                //    {
                //        ToastService.ShowInfo("The header must match the grid.");
                //        return;
                //    }

                //    var countries = new List<CountryDto>();

                //    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                //    {
                //        var country = new CountryDto
                //        {
                //            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                //            Code = ws.Cells[row, 2].Value?.ToString()?.Trim()
                //        };

                //        if (!Countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()) && !countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()))
                //            countries.Add(country);
                //    }

                //    await Mediator.Send(new CreateListCountryRequest(countries));

                //    await LoadData();
                //}
                //catch { }
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
            LabTest = SelectedDataItems[0].Adapt<LabTestDto>();
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
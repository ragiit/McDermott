using Microsoft.AspNetCore.Components.Web;

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
        private List<LabTestDetailDto> LabDetailTests = [];
        private List<LabTestDto> LabTests = [];
        private List<LabTestDetailDto> LabTestDetailForms = [];
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
                "Quantitative",
                "Qualitative",
            ];

        #endregion Relations

        #region Static

        private bool ShowForm { get; set; } = false;
        private bool FormValidationState { get; set; } = true;
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowDetailVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        public IGrid GridDetail { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDetailDataItems { get; set; } = [];

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

        private async Task OnDeleteLabTestDetail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    LabTestDetailForms.Remove((LabTestDetailDto)e.DataItem);
                }
                else
                {
                    SelectedDataItems.Adapt<List<LabTestDetailDto>>().Select(x => x.Id).ToList().ForEach(x =>
                    {
                        LabTestDetailForms.Remove(LabTestDetailForms.FirstOrDefault(z => z.Id == x));
                    });
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var labTest = (LabTestDetailDto)e.EditModel;

                LabTestDetailDto update = new();

                if (labTest.Id == 0)
                {
                    labTest.Id = Helper.RandomNumber;
                    labTest.LabUom = LabUoms.FirstOrDefault(x => x.Id == labTest.LabUomId);

                    LabTestDetailForms.Add(labTest);
                }
                else
                {
                    var q = SelectedDetailDataItems[0].Adapt<LabTestDetailDto>();

                    update = LabTestDetailForms.FirstOrDefault(x => x.Id == q.Id)!;
                    labTest.LabUom = LabUoms.FirstOrDefault(x => x.Id == labTest.LabUomId);

                    var index = LabTestDetailForms.IndexOf(update!);
                    LabTestDetailForms[index] = labTest;
                }

                SelectedDetailDataItems = [];
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
            ShowForm = false;
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

        private void GridDetail_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowDetailVisibleIndex = args.VisibleIndex;
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

        private async Task NewItemDetail_Click()
        {
            await GridDetail.StartEditNewRowAsync();
        }

        private string EditedResultType = string.Empty;

        private async Task EditItem_Click()
        {
            ShowForm = true;
            var labTest = await Mediator.Send(new GetLabTestQuery(x => x.Id == SelectedDataItems[0].Adapt<GroupDto>().Id));

            if (labTest.Count > 0)
            {
                LabTest = labTest[0];
                EditedResultType = LabTest.ResultType;
                LabTestDetailForms = await Mediator.Send(new GetLabTestDetailQuery(x => x.LabTestId == LabTest.Id));
            }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void NewItem_Click()
        {
            ShowForm = true;
            LabTest = new();
            LabTestDetailForms = [];
        }

        private async Task EditItemDetail_Click(IGrid context)
        {
            var selected = (LabTestDetailDto)context.SelectedDataItem;
            // Buat salinan objek yang akan diedit menggunakan Mapster
            var copy = selected.Adapt<LabTestDetailDto>(); // GroupMenu adalah objek yang sedang diedit

            await GridDetail.StartEditRowAsync(FocusedRowDetailVisibleIndex);

            var w = LabTestDetailForms.FirstOrDefault(x => x.Id == copy.Id);

            //if (copy is not null)
            // Gunakan salinan objek yang diedit
            //this.GroupMenu = copy;
        }

        private void DeleteItemDetail_Click()
        {
            GridDetail.ShowRowDeleteConfirmation(FocusedRowDetailVisibleIndex);
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

        #region Form Function

        private void LoadLabTestDetail()
        {
        }

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
                await SaveItemLabTest();
            else
                FormValidationState = true;
        }

        private async Task SaveItemLabTest()
        {
            try
            {
                if (!FormValidationState && LabTestDetailForms.Count == 0)
                {
                    ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                    return;
                }

                if (LabTest.Id == 0)
                {
                    var result = await Mediator.Send(new CreateLabTestRequest(LabTest));
                    LabTestDetailForms.ForEach(x =>
                    {
                        x.Id = 0;
                        x.LabTestId = result.Id;
                    });
                    await Mediator.Send(new CreateListLabTestDetailRequest(LabTestDetailForms));
                }
                else
                {
                    await Mediator.Send(new UpdateLabTestRequest(LabTest));
                    await Mediator.Send(new DeleteLabTestDetailRequest(ids: LabTestDetailForms.Select(x => x.Id).ToList()));

                    LabTestDetailForms.ForEach(x =>
                    {
                        x.Id = 0;
                        x.LabTestId = LabTest.Id;

                        if (!LabTest.ResultType.Equals(EditedResultType))
                        {
                            x.ResultValueType = LabTest.ResultType;
                        }
                    });
                    await Mediator.Send(new CreateListLabTestDetailRequest(LabTestDetailForms));
                }

                ShowForm = false;
                LabTest = new();
                LabTestDetailForms = [];
                SelectedDetailDataItems = [];

                await LoadData();
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
            }
        }

        private void CancelItem_Click()
        {
            SelectedDataItems = [];
            ShowForm = false;
        }

        #endregion Form Function
    }
}
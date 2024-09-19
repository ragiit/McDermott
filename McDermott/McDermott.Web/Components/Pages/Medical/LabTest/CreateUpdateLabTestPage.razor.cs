using Microsoft.AspNetCore.Components.Web;

namespace McDermott.Web.Components.Pages.Medical.LabTest
{
    public partial class CreateUpdateLabTestPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
            //}
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

        #region Static Variabel

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private bool PanelVisible { get; set; } = true;
        private Timer _timer;
        public IGrid GridDetail;
        private bool FormValidationState { get; set; } = true;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDetailDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowDetailVisibleIndex { get; set; }
        #endregion

        #region Relation

        [SupplyParameterFromForm]
        private LabTestDto LabTest { get; set; } = new();
        private List<LabUomDto> LabUoms = [];
        private List<SampleTypeDto> SampleTypes = [];
        private List<LabTestDetailDto> LabTestDetailForms = [];

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

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }
        private async Task NewItemDetail_Click()
        {
            await GridDetail.StartEditNewRowAsync();
        }
        #endregion

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadLabTestDetails(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadLabTestDetails(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadLabTestDetails(newPageIndex, pageSize);
        }

        #endregion Searching

        #region Load ComboBox
        #region ComboBox Sampel Type
        private DxComboBox<SampleTypeDto, long?> refSampleTypesComboBox { get; set; }
        private int SampleTypesComboBoxIndex { get; set; } = 0;
        private int totalCountSampleTypes = 0;

        private async Task OnSearchSampleTypes()
        {
            await LoadDataSampleTypes(0, 10);
        }

        private async Task OnSearchSampleTypesIndexIncrement()
        {
            if (SampleTypesComboBoxIndex < (totalCountSampleTypes - 1))
            {
                SampleTypesComboBoxIndex++;
                await LoadDataSampleTypes(SampleTypesComboBoxIndex, 10);
            }
        }

        private async Task OnSearchSampleTypesIndexDecrement()
        {
            if (SampleTypesComboBoxIndex > 0)
            {
                SampleTypesComboBoxIndex--;
                await LoadDataSampleTypes(SampleTypesComboBoxIndex, 10);
            }
        }

        private async Task OnInputSampleTypesChanged(string e)
        {
            SampleTypesComboBoxIndex = 0;
            await LoadDataSampleTypes(0, 10);
        }

        private async Task LoadDataSampleTypes(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetSampleTypeQuery(searchTerm: refSampleTypesComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            SampleTypes = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion
        #region Combo Box Lab Uoms
        private DxComboBox<LabUomDto, long?> refLabUomComboBox { get; set; }
        private int LabUomComboBoxIndex { get; set; } = 0;
        private int totalCountLabUom = 0;

        private async Task OnSearchLabUom()
        {
            await LoadDataLabUom(0, 10);
        }

        private async Task OnSearchLabUomIndexIncrement()
        {
            if (LabUomComboBoxIndex < (totalCountLabUom - 1))
            {
                LabUomComboBoxIndex++;
                await LoadDataLabUom(LabUomComboBoxIndex, 10);
            }
        }

        private async Task OnSearchLabUomIndexDecrement()
        {
            if (LabUomComboBoxIndex > 0)
            {
                LabUomComboBoxIndex--;
                await LoadDataLabUom(LabUomComboBoxIndex, 10);
            }
        }

        private async Task OnInputLabUomChanged(string e)
        {
            LabUomComboBoxIndex = 0;
            await LoadDataLabUom(0, 10);
        }

        private async Task LoadDataLabUom(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLabUomQuery(searchTerm: refLabUomComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            LabUoms = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion
        #endregion

        #region load Data
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadLabTestDetails();
            await LoadDataSampleTypes();
            await LoadDataLabUom();
            await LoadData();
            await GetUserInfo();
            PanelVisible = false;

            return;

            try
            {
                _timer = new Timer(async (_) => await LoadLabTestDetails(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await GetUserInfo();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private async Task LoadLabTestDetails(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDetailDataItems = [];
            var result = await Mediator.Send(new GetLabTestDetailQuery(x => x.LabTestId == LabTest.Id, pageSize: pageSize, pageIndex: pageIndex));
            LabTestDetailForms = result.Item1;
            totalCount = result.pageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }

        private async Task LoadData()
        {
            var result = await Mediator.Send(new GetLabTestQuery(x => x.Id == Id, 0, 1));
            LabTest = new();
            LabTestDetailForms.Clear();
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("medical/lab-tests");
                    return;
                }

                LabTest = result.Item1.FirstOrDefault() ?? new();
                await LoadLabTestDetails();

            }
        }
        #endregion

        #region Handle Submit
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
        #endregion

        #region Save
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
                    var existingName = await Mediator.Send(new ValidateLabTestQuery(x => x.Name == LabTest.Name));

                    if (existingName)
                    {
                        ToastService.ShowInfo("Lab Test name already exist");
                        return;
                    }
                    var result = await Mediator.Send(new CreateLabTestRequest(LabTest));
                    LabTestDetailForms.ForEach(x =>
                    {
                        x.Id = 0;
                        x.LabTestId = result.Id;
                    });
                    await Mediator.Send(new CreateListLabTestDetailRequest(LabTestDetailForms));
                    NavigationManager.NavigateTo($"medical/lab-tests/{EnumPageMode.Update.GetDisplayName()}?Id={result.Id}", true);
                }
                else
                {
                    var result = await Mediator.Send(new UpdateLabTestRequest(LabTest));
                    NavigationManager.NavigateTo($"medical/lab-tests/{EnumPageMode.Update.GetDisplayName()}?Id={result.Id}", true);
                }

            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
            }
        }
        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (e is null)
                    return;

                var labTestDetail = (LabTestDetailDto)e.EditModel;


                if (labTestDetail.Id == 0)
                {
                    labTestDetail.LabTestId = LabTest.Id;
                    await Mediator.Send(new CreateLabTestDetailRequest(labTestDetail));
                }
                else
                {
                    await Mediator.Send(new UpdateLabTestDetailRequest(labTestDetail));
                }
                await LoadLabTestDetails();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Delete
        private async Task OnDeleteLabTestDetail(GridDataItemDeletingEventArgs e)
        {
            if (LabTest.Id == 0)
            {
                try
                {
                    if (SelectedDetailDataItems is null || SelectedDetailDataItems.Count == 1)
                    {
                        LabTestDetailForms.Remove((LabTestDetailDto)e.DataItem);
                    }
                    else
                    {
                        SelectedDetailDataItems.Adapt<List<LabTestDetailDto>>().Select(x => x.Id).ToList().ForEach(x =>
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
            else
            {
                try
                {
                    if (SelectedDetailDataItems is null || SelectedDetailDataItems.Count == 1)
                    {
                        await Mediator.Send(new DeleteLabTestDetailRequest(((LabTestDetailDto)e.DataItem).Id));
                    }
                    else
                    {
                        var a = SelectedDetailDataItems.Adapt<List<LabTestDetailDto>>();
                        await Mediator.Send(new DeleteLabTestDetailRequest(ids: a.Select(x => x.Id).ToList()));
                    }
                    SelectedDetailDataItems = [];
                    await LoadLabTestDetails();
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
        }

        #endregion

        #region click Methode
        private void DeleteItemDetail_Click()
        {
            GridDetail.ShowRowDeleteConfirmation(FocusedRowDetailVisibleIndex);
        }
        private bool IsLoading { get; set; } = false;
        private async Task EditItem_Click()
        {
            try
            {
                IsLoading = true;
                LabTest = SelectedDataItems[0].Adapt<LabTestDto>();

                if (LabTest != null)
                {
                    await LoadLabTestDetails();
                }
            }
            catch (Exception e)
            {
                var zz = e;
            }
            //await GridGropMenu.StartEditRowAsync(FocusedRowVisibleIndexGroupMenu);
            IsLoading = false;
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
        private async Task RefreshDetail_Click()
        {
            await LoadLabTestDetails();
        }
        #endregion

        #region Impor & Export
        private async Task ImportFile2()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        public async Task ImportExcelFile2(InputFileChangeEventArgs e)
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

                    var headerNames = new List<string>() { "Name", "Result Type" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<LabTestDetailDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new LabTestDetailDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            ResultType = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        if (!LabTestDetailForms.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.ResultType.Trim().ToLower() == c?.ResultType?.Trim().ToLower()))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListLabTestDetailRequest(list));

                    await LoadLabTestDetails();
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

        private async Task ExportToExcel2()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "project_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Result Type"
                },
            ]);
        }
        #endregion
        private void CancelItem_Click()
        {
            NavigationManager.NavigateTo($"medical/lab-tests");
        }
    }
}

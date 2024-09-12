using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using McDermott.Application.Dtos.Medical;
using McDermott.Domain.Entities;
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
                var user = await UserInfoService.GetUserInfo(ToastService);
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

        private Timer _timer;
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

        private async Task LoadLabTestDetails(int pageIndex = 0, int pageSize = 10)
        {
            SelectedDetailDataItems = [];
            var result = await Mediator.Send(new GetLabTestDetailQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            LabTestDetailForms = result.Item1.Where(x => x.LabTestId == LabTest.Id).ToList();
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (e is null)
                    return;

                var labTest = (LabTestDetailDto)e.EditModel;

                if (LabTest.Id == 0)
                {
                    try
                    {
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
                else
                {
                    labTest.LabTestId = LabTest.Id;
                    if (labTest.Id == 0)
                        await Mediator.Send(new CreateLabTestDetailRequest(labTest));
                    else
                        await Mediator.Send(new UpdateLabTestDetailRequest(labTest));

                    await LoadLabTestDetails();
                }
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
            ShowForm = false;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLabTestQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            LabTests = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion LoadData

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
            var result = await Mediator.Send(new GetSampleTypeQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
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
            var result = await Mediator.Send(new GetLabUomQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            LabUoms = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion
        #endregion

        #region Grid Function



        private void GridDetail_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowDetailVisibleIndex = args.VisibleIndex;
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


        private async Task Refresh_Click()
        {
            await LoadData();
        }
        private async Task RefreshDetail_Click()
        {
            await LoadLabTestDetails();
        }

        private async Task NewItemDetail_Click()
        {
            await GridDetail.StartEditNewRowAsync();
        }

        private string EditedResultType = string.Empty;

        private async Task EditItem_Click()
        {
            ShowForm = true;
            //var labTest = await Mediator.Send(new GetLabTestQuery(x => x.Id == SelectedDataItems[0].Adapt<GroupDto>().Id));

            //if (labTest.Count > 0)
            //{
            //    LabTest = labTest[0];
            //    EditedResultType = LabTest.ResultType;
            //    LabTestDetailForms = await Mediator.Send(new GetLabTestDetailQuery(x => x.LabTestId == LabTest.Id));
            //}
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task NewItem_Click()
        {
            ShowForm = true;
            LabTest = new();
            LabTestDetailForms = [];
            await LoadLabTestDetails();
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

                    var headerNames = new List<string>() { "Name", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<LabTestDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new LabTestDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        if (!LabTests.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.Code.Trim().ToLower() == c?.Code?.Trim().ToLower()))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListLabTestRequest(list));

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
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "project_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Code"
                },
            ]);
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
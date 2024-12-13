using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components.Web;

namespace McDermott.Web.Components.Pages.Medical.LabTests
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
        public IGrid GridDetail { get; set; }
        private bool FormValidationState { get; set; } = true;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDetailDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowDetailVisibleIndex { get; set; }

        #endregion Static Variabel

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

        #endregion Relation

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        #region Load ComboBox

        #region ComboBox Sampel Type

        protected async Task<LoadResult> LoadCustomDataSampleType(DataSourceLoadOptionsBase options, CancellationToken cancellationToken)
        {
            return await QueryComboBoxHelper.LoadCustomData<SampleType>(
                options: options,
                queryProvider: async () => await Mediator.Send(new GetQuerySampleType()),
                cancellationToken: cancellationToken);
        }

        #endregion ComboBox Sampel Type

        protected async Task<LoadResult> LoadCustomDataLabUom(DataSourceLoadOptionsBase options, CancellationToken cancellationToken)
        {
            return await QueryComboBoxHelper.LoadCustomData<LabUom>(
                options: options,
                queryProvider: async () => await Mediator.Send(new GetQueryLabUom()),
                cancellationToken: cancellationToken);
        }

        #endregion Load ComboBox

        #region load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadLabTestDetails();
            PanelVisible = false;
        }

        private object Data { get; set; }

        private async Task LoadLabTestDetails()
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var dataSource = new GridDevExtremeDataSource<LabTestDetail>(await Mediator.Send(new GetQueryLabTestDetail
                {
                    Predicate = x => x.LabTestId == LabTest.Id
                }))
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

        private async Task LoadData()
        {
            var result = await Mediator.Send(new GetSingleLabTestQuery
            {
                Predicate = x => x.Id == Id
            });

            LabTest = new();
            LabTestDetailForms.Clear();
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("medical/lab-tests");
                    return;
                }

                LabTest = result ?? new();
                await LoadLabTestDetails();
            }
        }

        #endregion load Data

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

        #endregion Handle Submit

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

                var labTestDetail = (LabTestDetail)e.EditModel;

                if (labTestDetail.Id == 0)
                {
                    labTestDetail.LabTestId = LabTest.Id;
                    await Mediator.Send(new CreateLabTestDetailRequest(labTestDetail.Adapt<LabTestDetailDto>()));
                }
                else
                {
                    await Mediator.Send(new UpdateLabTestDetailRequest(labTestDetail.Adapt<LabTestDetailDto>()));
                }
                await LoadLabTestDetails();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion Save

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

        #endregion Delete

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
            await GridDetail.StartEditRowAsync(FocusedRowVisibleIndex);
            //var a = (GridDetail.GetDataItem(FocusedRowVisibleIndex) as LabTestDetailDto ?? new());

            //await LoadComboboxEdit(a);
        }

        private async Task LoadComboboxEdit(LabTestDetailDto a)
        {
            LabUoms = (await Mediator.QueryGetHelper<LabUom, LabUomDto>(predicate: x => x.Id == a.LabUomId)).Item1;
            var aa = "daw";
        }

        private async Task RefreshDetail_Click()
        {
            await LoadLabTestDetails();
        }

        #endregion click Methode

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

        #endregion Impor & Export

        private void CancelItem_Click()
        {
            NavigationManager.NavigateTo($"medical/lab-tests");
        }
    }
}
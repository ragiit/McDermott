using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using McDermott.Application.Features.Services;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class LabSampleTypePage
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

        private SampleTypeDto SampleType = new();
        private List<SampleTypeDto> SampleTypes = [];

        #endregion Relations

        #region Static

        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private Timer _timer;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

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
                    await Mediator.Send(new DeleteSampleTypeRequest(((SampleTypeDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteSampleTypeRequest(ids: SelectedDataItems.Adapt<List<SampleTypeDto>>().Select(x => x.Id).ToList()));
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
                SampleType = ((SampleTypeDto)e.EditModel);

                bool validate = await Mediator.Send(new ValidateSampleTypeQuery(x => x.Id != SampleType.Id && x.Name == SampleType.Name));

                if (validate)
                {
                    ToastService.ShowInfo($"Sample Type with name '{SampleType.Name}' is already exists");
                    e.Cancel = true;
                    return;
                }

                if (SampleType.Id == 0)
                    await Mediator.Send(new CreateSampleTypeRequest(SampleType));
                else
                    await Mediator.Send(new UpdateSampleTypeRequest(SampleType));

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
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
            try
            {
                PanelVisible = true;
                SampleType = new();
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetSampleTypeQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
                SampleTypes = result.Item1;
                totalCount = result.pageCount;
                activePageIndex = pageIndex;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion LoadData

        #region Grid Function

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #region ToolBar Button

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

                    var headerNames = new List<string>() { "Name", "Description" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<SampleTypeDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var SampleType = new SampleTypeDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Description = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        list.Add(SampleType);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Description, }).ToList();

                        // Panggil BulkValidateSampleTypeQuery untuk validasi bulk
                        var existingSampleTypes = await Mediator.Send(new BulkValidateSampleTypeQuery(list));

                        // Filter SampleType baru yang tidak ada di database
                        list = list.Where(SampleType =>
                            !existingSampleTypes.Any(ev =>
                                ev.Name == SampleType.Name
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListSampleTypeRequest(list));
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
            SampleType = SelectedDataItems[0].Adapt<SampleTypeDto>();
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "sample_type_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Description"
                },
            ]);
        }

        public async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jSRuntime, IFileExportService file, string fileName, DotNetStreamReference streamReference, List<ExportFileData> data, string? name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

        #endregion ToolBar Button

        #endregion Grid Function
    }
}
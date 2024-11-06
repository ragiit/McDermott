using MailKit.Search;
using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;
using static McDermott.Application.Features.Commands.Inventory.GoodsReceiptCommand;

namespace McDermott.Web.Components.Pages.AwerenessEvent
{
    public partial class AwarenessEduCategoryPage
    {
        #region UserLoginAndAccessRole

        [Parameter]
        public long Id { get; set; }

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

        #region relation data
        private List<AwarenessEduCategoryDto> getAwarenessEduCategory = [];
        private AwarenessEduCategoryDto postAwarenessEducategory = new();
        #endregion

        #region variabel static
        private IGrid Grid {  get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        #endregion

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

        #region Async Load
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetAwarenessEduCategoryQuery
            {
                
                SearchTerm = searchTerm,
                PageSize = pageSize,
                PageIndex = pageIndex,
            });
            getAwarenessEduCategory = result.Item1;
            totalCount = result.PageCount;
            activePageIndex = pageIndex;
            PanelVisible = false;
        }
        #endregion

        #region ImportExport

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "AwarenessEducategory_template.xlsx", new List<ExportFileData>
            {
                new() { Column = "Name", Notes = "Mandatory" },
            });
        }

        public async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jsRuntime, IFileExportService file, string fileName, DotNetStreamReference streamReference, List<ExportFileData> data, string name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jsRuntime.InvokeVoidAsync("saveFileExcellExporrt", fileName, streamRef);
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

                    var headerNames = new List<string>() { "Name", "Code", };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<AwarenessEduCategoryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var AwarenessEducategory = new AwarenessEduCategoryDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                        };

                        list.Add(AwarenessEducategory);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name }).ToList();

                        // Panggil BulkValidateCountryQuery untuk validasi bulk
                        var existingCountrys = await Mediator.Send(new BulkValidateAwarenessEduCategoryQuery(list));

                        // Filter Country baru yang tidak ada di database
                        list = list.Where(AwarenessEducategory =>
                            !existingCountrys.Any(ev =>
                                ev.Name == AwarenessEducategory.Name 
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListAwarenessEduCategoryRequest(list));
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

        #endregion ImportExport

        #region Grid Events

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
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

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteAwarenessEduCategoryRequest(((AwarenessEduCategoryDto)e.DataItem).Id));
                }
                else
                {
                    var countriesToDelete = SelectedDataItems.Adapt<List<AwarenessEduCategoryDto>>();
                    await Mediator.Send(new DeleteAwarenessEduCategoryRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                }

                SelectedDataItems = [];
                await LoadData(activePageIndex, pageSize);
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
                PanelVisible = true;
                var editModel = (AwarenessEduCategoryDto)e.EditModel;

                bool validate = await Mediator.Send(new ValidateAwarenessEduCategoryQuery(x => x.Id != editModel.Id && x.Name == editModel.Name ));

                if (validate)
                {
                    ToastService.ShowInfo($"Category with name '{editModel.Name}'" );
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateAwarenessEduCategoryRequest(editModel));
                else
                    await Mediator.Send(new UpdateAwarenessEduCategoryRequest(editModel));

                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Grid Events


    }
}

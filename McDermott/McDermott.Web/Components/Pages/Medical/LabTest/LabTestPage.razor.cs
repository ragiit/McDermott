using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using McDermott.Application.Dtos.Medical;
using McDermott.Domain.Entities;
using McDermott.Extentions;
using Microsoft.AspNetCore.Components.Web;

namespace McDermott.Web.Components.Pages.Medical.LabTest
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

        private List<LabTestDto> LabTests = [];

        #endregion Relations

        #region Static

        private Timer _timer;
        private bool ShowForm { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        //private int FocusedRowDetailVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        GridSelectAllCheckboxMode CurrentSelectAllCheckboxMode { get; set; }

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

        private async Task OnDelete()
        {
            try
            {
                var LabTest = SelectedDataItems[0].Adapt<LabTestDto>();
                var labTestIds = SelectedDataItems is null || SelectedDataItems.Count == 1
                    ? new List<long> { LabTest.Id }
                    : SelectedDataItems.Adapt<List<LabTestDto>>().Select(x => x.Id).ToList();

                foreach (var id in labTestIds)
                {
                    var labTestDetails = await Mediator.Send(new GetAllLabTestDetailQuery(x => x.LabTestId == id));
                    if (labTestDetails is not null)
                    {
                        foreach (var detail in labTestDetails)
                        {
                            await Mediator.Send(new DeleteLabTestDetailRequest(detail.Id));
                        }
                    }
                    await Mediator.Send(new DeleteLabTestRequest(id));
                }

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

        #region Grid Function

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
              
        private string EditedResultType = string.Empty;

        private async Task EditItem_Click()
        {
            try
            {
                var LabTest = SelectedDataItems[0].Adapt<LabTestDto>();
                var Ids = SecureHelper.EncryptIdToBase64(LabTest.Id);
                NavigationManager.NavigateTo($"medical/lab-tests/{EnumPageMode.Update.GetDisplayName()}?Id={LabTest.Id}");
                return;
                
            }
            catch (Exception ex) { }
        }

        private void DeleteItem_Click()
        {
            var selectedItemCount = SelectedDataItems?.Count ?? 0;

            if (selectedItemCount > 0)
            {
                // Use GridSelectAllCheckboxMode for managing selection logic
                if (CurrentSelectAllCheckboxMode == GridSelectAllCheckboxMode.AllPages)
                {
                    // Implement logic for selecting all pages
                }
                else
                {
                    // Implement logic for the current page selection
                }

                // Show confirmation based on selected item count
                Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
            }
        }

        private async Task NewItem_Click()
        {
            NavigationManager.NavigateTo($"medical/lab-tests/{EnumPageMode.Create.GetDisplayName()}");
            return;
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

        
    }
}
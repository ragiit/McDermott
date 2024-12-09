using McDermott.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceRecordCommand;

namespace McDermott.Web.Components.Pages.Inventory.MaintenanceRecord
{
    public partial class MaintenanceRecordPage
    {
        #region Relation Data

        private List<MaintenanceRecordDto> GetMaintenanceRecords = [];
        private List<MaintenanceDto> GetMaintenances = [];
        private List<ProductDto> GetProducts = [];
        private MaintenanceRecordDto PostMaintenanceRecords = new();

        #endregion Relation Data

        #region Variable Static

        [Inject]
        protected IUploadDocumentService UploadDocumentService { get; set; }

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        private bool PanelVisible { get; set; } = false;
        private bool showPopUp { get; set; } = false;
        private string? NameProduct { get; set; }
        private int? SelectedFilesCount { get; set; }
        private bool FormValidationState { get; set; } = true;
        private Timer _timer;
        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Variable Static

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

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            // Initiate both tasks at once
            var loadDataTask = LoadData();
            var getUserInfoTask = GetUserInfo();

            // Await both tasks to complete
            await Task.WhenAll(loadDataTask, getUserInfoTask);

            PanelVisible = false;
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;

            // Only clear SelectedDataItems if necessary
            if (SelectedDataItems != null)
                SelectedDataItems = [];

            var result = await Mediator.Send(new GetMaintenanceRecordQuery
            {
                Predicate = x => x.ProductId == Id,
                SearchTerm = searchTerm,
                PageSize = pageSize,
                PageIndex = pageIndex
            });
            GetMaintenanceRecords = result.Item1;
            totalCount = result.PageCount;

            PanelVisible = false;
        }

        #endregion LoadData

        #region KeyPress

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
            {
                await OnSave();
            }
        }

        private async Task HandleInvalidSubmit()
        {
            FormValidationState = false;
        }

        #endregion KeyPress

        #region SaveDelete

        private async Task OnDelete()
        {
            try
            {
                var MaintenanceRecords = SelectedDataItems[0].Adapt<MaintenanceRecordDto>();
                var MaintenanceRecordsIds = SelectedDataItems is null || SelectedDataItems.Count == 1
                    ? new List<long> { PostMaintenanceRecords.Id }
                    : SelectedDataItems.Adapt<List<MaintenanceRecordDto>>().Select(x => x.Id).ToList();

                foreach (var id in MaintenanceRecordsIds)
                {
                    await Mediator.Send(new DeleteLabTestRequest(id));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSave()
        {
        }

        private List<UploadFileInfo> SelectedFiles = new List<UploadFileInfo>();

        // Event handler untuk `FilesChanged`
        protected async Task SelectedFilesChanged(InputFileChangeEventArgs e)
        {
            var files = e.GetMultipleFiles();
            foreach (var file in files)
            {
                var allowedExtensions = new[] { ".pdf" };
                var (status, fileName) = await UploadDocumentService.UploadDocumentAsync(file, 1048576, allowedExtensions);

                if (status == 1)
                {
                    // Dapatkan user yang sedang login
                    var authState = UserLogin.Id;

                    // Panggil OnSave untuk menyimpan ke database
                    //await OnSave(fileName, authState, DateTime.Now);

                    // Tampilkan notifikasi sukses
                    ToastService.ShowInfo($"File {file.Name} uploaded successfully as {fileName}.");
                }
                else
                {
                    // Tampilkan notifikasi error
                    ToastService.ShowError($"Failed to upload file {file.Name}. Reason: {fileName}");
                }
            }

            InvokeAsync(StateHasChanged);
        }

        #endregion SaveDelete

        #region Load ComboBox

        #region ComboBox Maintenance

        private DxComboBox<MaintenanceDto, long?> refMaintenanceComboBox { get; set; }
        private int MaintenanceComboBoxIndex { get; set; } = 0;
        private int totalCountMaintenance = 0;

        private async Task OnSearchMaintenance()
        {
            await LoadDataMaintenance(0, 10);
        }

        private async Task OnSearchMaintenanceIndexIncrement()
        {
            if (MaintenanceComboBoxIndex < (totalCountMaintenance - 1))
            {
                MaintenanceComboBoxIndex++;
                await LoadDataMaintenance(MaintenanceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchMaintenanceIndexDecrement()
        {
            if (MaintenanceComboBoxIndex > 0)
            {
                MaintenanceComboBoxIndex--;
                await LoadDataMaintenance(MaintenanceComboBoxIndex, 10);
            }
        }

        private async Task OnInputMaintenanceChanged(string e)
        {
            MaintenanceComboBoxIndex = 0;
            await LoadDataMaintenance(0, 10);
        }

        private async Task LoadDataMaintenance(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetMaintenanceQuery
            {

            });
            GetMaintenances = result.Item1;
            totalCount = result.PageCount;
            PanelVisible = false;
        }

        #endregion ComboBox Maintenance

        #region Combo Box Product

        private DxComboBox<ProductDto, long?> refProductComboBox { get; set; }
        private int ProductComboBoxIndex { get; set; } = 0;
        private int totalCountProduct = 0;

        private async Task OnSearchProduct()
        {
            await LoadDataProduct(0, 10);
        }

        private async Task OnSearchProductIndexIncrement()
        {
            if (ProductComboBoxIndex < (totalCountProduct - 1))
            {
                ProductComboBoxIndex++;
                await LoadDataProduct(ProductComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProductIndexDecrement()
        {
            if (ProductComboBoxIndex > 0)
            {
                ProductComboBoxIndex--;
                await LoadDataProduct(ProductComboBoxIndex, 10);
            }
        }

        private async Task OnInputProductChanged(string e)
        {
            ProductComboBoxIndex = 0;
            await LoadDataProduct(0, 10);
        }

        private async Task LoadDataProduct(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProductQuery(searchTerm: refProductComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetProducts = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }

        #endregion Combo Box Product

        #endregion Load ComboBox

        #region Grid Configure

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Grid Configure

        #region Function Button

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task NewItem_Click()
        {
            PanelVisible = true;
            showPopUp = true;
            PanelVisible = false;
        }

        private async Task EditItem_Click()
        {
            try
            {
                var LabTest = SelectedDataItems[0].Adapt<MaintenanceRecordDto>();
            }
            catch (Exception ex) { }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Back_Click()
        {
            showPopUp = false;
            PostMaintenanceRecords = new();
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

                    var headerNames = new List<string>() { "Document Name", "Sequence Product" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<MaintenanceRecordDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new MaintenanceRecordDto
                        {
                            DocumentName = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            SequenceProduct = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        if (!GetMaintenanceRecords.Any(x => x.DocumentName?.Trim().ToLower() == c?.DocumentName?.Trim().ToLower() && x.SequenceProduct?.Trim().ToLower() == c?.SequenceProduct?.Trim().ToLower()))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListMaintenanceRecordRequest(list));

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
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "Maintenance_Record_template.xlsx",
            [
                new()
                {
                    Column = "Document Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Sequence Product"
                },
            ]);
        }

        #endregion Function Button
    }
}
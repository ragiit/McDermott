
using McDermott.Application.Dtos.ClaimUserManagement;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;
using static McDermott.Application.Features.Commands.ClaimUserManagement.BenefitConfigurationCommand;

namespace McDermott.Web.Components.Pages.ClaimUserManagement
{
    public partial class BenefitConfigurationPage
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
        #region Relation Data
        private List<BenefitConfigurationDto> GetBenefitConfigurations { get; set; } = [];
        private BenefitConfigurationDto PostBenefitConfigurations = new();
        #endregion

        #region Variable Static
        private IGrid Grid { get; set; }
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
            var result = await Mediator.Send(new GetBenefitConfigurationQuery
            {

                SearchTerm = searchTerm,
                PageSize = pageSize,
                PageIndex = pageIndex,
            });
            GetBenefitConfigurations = result.Item1;
            totalCount = result.PageCount;
            activePageIndex = pageIndex;

            PostBenefitConfigurations = result.Item1.FirstOrDefault();

            PanelVisible = false;
        }
        #endregion

        #region ImportExport

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "BenefitConfiguration_template.xlsx", [
                new() { Column = "BenefitName", Notes = "Mandatory" },
                new() {Column = "TypeOfBenefit", Notes = "Mandatory"},
                new() {Column = "DurationOfBenefit"},
                new() {Column = "BenefitDuration"},
                new() {Column = "Eligibility"},
            ]);
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

                    var headerNames = new List<string>() { "BenefitName", "TypeOfBenefit", "BenefitValue", "DurationOfBenefit", "BenefitDuration", "Eligibility", "Status" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<BenefitConfigurationDto>(); // Adjusted to your benefit DTO

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        var benefitName = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var typeOfBenefit = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var benefitValue = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        var durationOfBenefit = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        var benefitDuration = ws.Cells[row, 5].Value?.ToString()?.Trim();
                        var eligibility = ws.Cells[row, 6].Value?.ToString()?.Trim();
                        var status = ws.Cells[row, 7].Value?.ToString()?.Trim();

                        // Parse enum values


                        if (!Enum.TryParse<EnumWorksDays>(durationOfBenefit, true, out var parsedDurationOfBenefit))
                        {
                            ToastService.ShowErrorImport(row, 4, durationOfBenefit ?? string.Empty);
                            isValid = false;
                        }


                        if (!Enum.TryParse<EnumBenefitStatus>(status, true, out var parsedStatus))
                        {
                            ToastService.ShowErrorImport(row, 7, status ?? string.Empty);
                            isValid = false;
                        }

                        // Validate benefitValue and benefitDuration if they are numeric
                        if (!int.TryParse(benefitValue, out var parsedBenefitValue))
                        {
                            ToastService.ShowErrorImport(row, 3, benefitValue ?? string.Empty);
                            isValid = false;
                        }

                        if (!int.TryParse(benefitDuration, out var parsedBenefitDuration))
                        {
                            ToastService.ShowErrorImport(row, 5, benefitDuration ?? string.Empty);
                            isValid = false;
                        }

                        if (!isValid)
                            continue;

                        var benefit = new BenefitConfigurationDto
                        {
                            BenefitName = benefitName,
                            BenefitValue = parsedBenefitValue,
                            BenefitDuration = parsedBenefitDuration,
                            Status = parsedStatus
                        };

                        list.Add(benefit);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.BenefitName, x.TypeOfBenefit }).ToList();
                        await Mediator.Send(new CreateListBenefitConfigurationRequest(list));
                        await LoadData(0, pageSize);
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex) { ex.HandleException(ToastService); }
                finally { PanelVisible = false; }
            }
        }

        #endregion ImportExport

        #region Click
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

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task onActive()
        {
            var data = SelectedDataItems[0].Adapt<BenefitConfigurationDto>();
            if(data.Id == 0)
            {
                
            }
        }
        #endregion

        #region Save & Delete
        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                PostBenefitConfigurations = (BenefitConfigurationDto)e.EditModel;
                var data = new BenefitConfigurationDto();

                bool validate = await Mediator.Send(new ValidateBenefitConfigurationQuery(x => x.Id != PostBenefitConfigurations.Id && x.BenefitName == PostBenefitConfigurations.BenefitName));

                if (validate)
                {
                    ToastService.ShowInfo($"Benefit with name '{PostBenefitConfigurations.BenefitName}");
                    e.Cancel = true;
                    return;
                }

                if (PostBenefitConfigurations.Id == 0)
                {
                    PostBenefitConfigurations.Status = EnumBenefitStatus.Draft;
                    data = await Mediator.Send(new CreateBenefitConfigurationRequest(PostBenefitConfigurations));
                    ToastService.ShowSuccess($"Add Data Benefit Name {data.BenefitName} Success");
                }
                else
                {
                    data = await Mediator.Send(new UpdateBenefitConfigurationRequest(PostBenefitConfigurations));
                    ToastService.ShowSuccess($"Update Data Benefit Name {data.BenefitName} Success");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnDelete()
        {

        }
        #endregion

    }
}

using McDermott.Application.Features.Services;
using MediatR;
using Microsoft.JSInterop;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class ProjectPage
    {
        private DxUpload MyUpload { get; set; }
         
        private List<ProjectDto> Projects = []; 
        private HubConnection hubConnection;

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

        #region Default Grid Components

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private string names { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            Projects = await Mediator.Send(new GetProjectQuery());
            SelectedDataItems = new ObservableRangeCollection<object>();
            PanelVisible = false;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
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

                    var headerNames = new List<string>() { "Project", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<ProjectDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    { 
                        var c = new ProjectDto
                        { 
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        if (!Projects.Any(x => x.Name.Trim().ToLower() == c?.Name?.Trim().ToLower() && x.Code.Trim().ToLower() == c?.Code?.Trim().ToLower()))
                            list.Add(c);
                    }

                    await Mediator.Send(new CreateListProjectRequest(list));

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
                    Column = "Project",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Code" 
                },
            ]);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteProjectRequest(((ProjectDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<ProjectDto>>();
                    await Mediator.Send(new DeleteProjectRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (ProjectDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateProjectRequest(editModel));
                else
                    await Mediator.Send(new UpdateProjectRequest(editModel));

                await LoadData();
            }
            catch { }
        }

        #endregion Default Grid Components
    }
}

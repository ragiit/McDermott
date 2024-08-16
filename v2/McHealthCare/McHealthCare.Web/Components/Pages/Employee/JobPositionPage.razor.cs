using McHealthCare.Application.Dtos.Employees;
using McHealthCare.Application.Extentions;
using Microsoft.AspNetCore.SignalR.Client;

namespace McHealthCare.Web.Components.Pages.Employee
{
    public partial class JobPositionPage : IAsyncDisposable
    {
        #region Variables

        private bool PanelVisible { get; set; } = true;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private HubConnection? hubConnection;
        private List<JobPositionDto> JobPositions = [];
        private List<DepartmentDto> Departments = [];

        private List<ExportFileData> ExportFileDatas =
        [
            new()
            {
                Column = "Code",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            }
        ];

        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Variables

        private bool shouldAddW100Class = true;

        private async Task LoadCombobox()
        {
            Departments = await Mediator.Send(new GetDepartmentQuery());
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            try
            {
                UserAccess = await UserService.GetUserInfo(ToastService);

                var aa = NavigationManager.ToAbsoluteUri("/notificationHub");
                hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHub"))
                .Build();

                hubConnection.On<ReceiveDataDto>("ReceiveNotification", async message =>
                {
                    await LoadData();
                });

                await hubConnection.StartAsync();

                await LoadCombobox();

                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }
            }
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                JobPositions.Clear();
                JobPositions = await Mediator.Send(new GetJobPositionQuery());
                try
                {
                    Grid?.SelectRow(0, true);
                }
                catch { }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteJobPositionRequest(((JobPositionDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<JobPositionDto>>();
                    await Mediator.Send(new DeleteJobPositionRequest(Ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                var editModel = (JobPositionDto)e.EditModel;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateJobPositionRequest(editModel));
                else
                    await Mediator.Send(new UpdateJobPositionRequest(editModel));

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                PanelVisible = false;
            }
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

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Code", "Name" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var JobPositions = new List<JobPositionDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var JobPosition = new JobPositionDto
                        {
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty,
                        };

                        //if (!JobPositions.Any(x => x.Name.Trim().ToLower() == JobPosition?.Name?.Trim().ToLower() && x.Code.Trim().ToLower() == JobPosition?.Code?.Trim().ToLower()))
                        //    JobPositions.Add(JobPosition);
                    }

                    await Mediator.Send(new CreateListJobPositionRequest(JobPositions));

                    await LoadData();

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            PanelVisible = false;
        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
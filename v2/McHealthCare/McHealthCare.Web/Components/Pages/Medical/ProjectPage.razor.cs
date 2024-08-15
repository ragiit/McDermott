using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Application.Extentions;
using Microsoft.AspNetCore.SignalR.Client;
using static McHealthCare.Application.Features.CommandsQueries.Medical.ProjectCommand;

namespace McHealthCare.Web.Components.Pages.Medical
{
    public partial class ProjectPage
    {
        #region Relation Data
        private List<ProjectDto> getProject = [];
        private ProjectDto postProject = new();
        #endregion
        #region Variabel
        private bool PanelVisible { get; set; } = false;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private HubConnection? hubConnection;

        private List<ExportFileData> ExportFileDatas =
        [
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            },

        ];

        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        #endregion

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

                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }

                //await LoadData();
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
                getProject.Clear();
                getProject = await Mediator.Send(new GetProjectQuery());

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

        #region Delete
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteProjectRequest(((ProjectDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<ProjectDto>>();
                    await Mediator.Send(new DeleteProjectRequest(Ids: a.Select(x => x.Id).ToList()));
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
        #endregion
        #region Save
        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                var editModel = (ProjectDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateProjectRequest(editModel));
                else
                    await Mediator.Send(new UpdateProjectRequest(editModel));

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
        #endregion

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}

using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Application.Extentions;
using McHealthCare.Domain.Entities.Medical;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using static McHealthCare.Application.Features.CommandsQueries.Medical.ServiceCommand;

namespace McHealthCare.Web.Components.Pages.Medical
{
    public partial class ServicePage
    {
        #region Relation Data
        private List<ServiceDto> getService = [];
        private List<ServiceDto> getServiceCounter = [];
        private ServiceDto postService = new();
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
                getService.Clear();
                getService = await Mediator.Send(new GetServiceQuery());

                foreach (var service in getService)
                {
                    service.Flag = GetServiceFlag(service);
                    service.ServiceCounter = GetServiceCounter(service);
                }

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

        private string GetServiceFlag(ServiceDto service)
        {
            return service.IsKiosk && service.IsPatient ? "Patient, Counter" :
                   service.IsKiosk ? "Counter" :
                   service.IsPatient ? "Patient" : "-";
        }

        private string GetServiceCounter(ServiceDto service)
        {
            return service.ServicedId != null
                ? getService.FirstOrDefault(x => x.Id == service.ServicedId)?.Name ?? "-"
                : "-";
        }


        #region Delete
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteServiceRequest(((ServiceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<ServiceDto>>();
                    await Mediator.Send(new DeleteServiceRequest(Ids: a.Select(x => x.Id).ToList()));
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
                var editModel = (ServiceDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateServiceRequest(editModel));
                else
                    await Mediator.Send(new UpdateServiceRequest(editModel));

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

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
                ConfigureHubConnection();
                await hubConnection.StartAsync();
                SelectFirstRow();
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ConfigureHubConnection()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHub"), options =>
                {
                    options.Transports = HttpTransportType.WebSockets; // Memastikan penggunaan WebSockets
                    options.SkipNegotiation = true; // Menghindari negosiasi transportasi untuk efisiensi
                })
                .WithAutomaticReconnect() // Menyambungkan kembali secara otomatis jika koneksi terputus
                .Build();

            hubConnection.On<ReceiveDataDto>("ReceiveNotification", async message =>
            {
                await LoadData();
            });
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

                SelectFirstRow();
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

        private void SelectFirstRow()
        {
            try
            {
                Grid?.SelectRow(0, true);
            }
            catch { }
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
                    await Mediator.Send(new CreateServiceRequest(editModel));

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

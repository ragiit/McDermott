﻿using Blazored.Toast.Services;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Application.Extentions;
using McHealthCare.Web.Services;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using static McHealthCare.Application.Features.CommandsQueries.Medical.DoctorScheduleCommand;
using static McHealthCare.Application.Features.CommandsQueries.Medical.InsuranceCommand;
using static McHealthCare.Application.Features.CommandsQueries.Medical.ServiceCommand;

namespace McHealthCare.Web.Components.Pages.Medical
{
    public partial class DoctorSchedule
    {
        #region Relation Data
        private List<DoctorScheduleDto> getDoctorSchedule = [];
        private List<ServiceDto> getServices = [];
        private InsuranceDto postDoctorSchedule = new();
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
                getDoctorSchedule.Clear();
                getDoctorSchedule = await Mediator.Send(new GetDoctorScheduleQuery());
                getServices = await Mediator.Send(new GetServiceQuery());

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
                    await Mediator.Send(new DeleteDoctorScheduleRequest(((DoctorScheduleDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DoctorScheduleDto>>();
                    await Mediator.Send(new DeleteDoctorScheduleRequest(Ids: a.Select(x => x.Id).ToList()));
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
                var editModel = (DoctorScheduleDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateDoctorScheduleRequest(editModel));
                else
                    await Mediator.Send(new UpdateDoctorScheduleRequest(editModel));

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

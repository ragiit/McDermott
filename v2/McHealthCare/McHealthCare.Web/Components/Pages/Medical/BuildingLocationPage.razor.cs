﻿using McHealthCare.Application.Dtos.Inventory;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Application.Extentions;
using Microsoft.AspNetCore.SignalR.Client;
using static McHealthCare.Application.Features.CommandsQueries.Inventory.LocationCommand;
using static McHealthCare.Application.Features.CommandsQueries.Medical.BuildingCommand;
using static McHealthCare.Application.Features.CommandsQueries.Medical.BuildingLocationCommand;
using static McHealthCare.Application.Features.CommandsQueries.Medical.HealthCenterCommand;

namespace McHealthCare.Web.Components.Pages.Medical
{
    public partial class BuildingLocationPage
    {
        #region Relation Data
        private List<BuildingLocationDto> getBuildingLocations = [];
        private List<BuildingDto> getBuildings = [];
        private List<LocationDto> getLocations = [];
        private BuildingLocationDto postBuildingLocations = new();
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
                getBuildingLocations.Clear();
                getBuildingLocations = await Mediator.Send(new GetBuildingLocationQuery());
                getBuildings = await Mediator.Send(new GetBuildingQuery());
                getLocations = await Mediator.Send(new GetLocationQuery());

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
                    await Mediator.Send(new DeleteBuildingLocationRequest(((BuildingLocationDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<BuildingLocationDto>>();
                    await Mediator.Send(new DeleteBuildingLocationRequest(Ids: a.Select(x => x.Id).ToList()));
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
                var editModel = (BuildingLocationDto)e.EditModel;

                if (editModel is null)
                    return;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateBuildingLocationRequest(editModel));
                else
                    await Mediator.Send(new UpdateBuildingLocationRequest(editModel));

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

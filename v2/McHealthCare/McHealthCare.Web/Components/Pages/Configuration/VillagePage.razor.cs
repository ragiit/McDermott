using Blazored.Toast.Services;
using McHealthCare.Application.Extentions;
using McHealthCare.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.VillageCommand;

namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class VillagePage
    {
        #region Variables

        private bool PanelVisible { get; set; } = true;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private HubConnection? hubConnection; 
        private List<VillageDto> Villages = [];
        private List<DistrictDto> Districts = [];
        private List<ProvinceDto> Provinces = [];
        private List<CityDto> Cities = [];

        private List<ExportFileData> ExportFileDatas =
        [
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Province",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "City",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "District",
                Notes = "Mandatory"
            }
        ];

        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Variables

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

                await LoadData();
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
                Cities.Clear();
                Villages = await Mediator.Send(new GetVillageQuery());
                Districts = await Mediator.Send(new GetDistrictQuery());
                Cities = await Mediator.Send(new GetCityQuery());
                Provinces = await Mediator.Send(new GetProvinceQuery());
                Districts = await Mediator.Send(new GetDistrictQuery());
                //SelectedDataItems = [];
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
                    await Mediator.Send(new DeleteVillageRequest(((VillageDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<VillageDto>>();
                    await Mediator.Send(new DeleteVillageRequest(Ids: a.Select(x => x.Id).ToList()));
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
                var editModel = (VillageDto)e.EditModel; 

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateVillageRequest(editModel));
                else
                    await Mediator.Send(new UpdateVillageRequest(editModel));

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

                    var headerNames = ExportFileDatas.Select(x => x.Column).ToList();

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var v = new List<VillageDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool IsValid = true;
                        var a = Provinces.FirstOrDefault(x => x.Name == ws.Cells[row, 2].Value?.ToString()?.Trim())?.Id ?? Guid.Empty;

                        if (ws.Cells[row, 2].Value?.ToString()?.Trim() is not null)
                        {
                            if (a == Guid.Empty)
                            {
                                ToastService.ShowErrorImport(row, 1, ws.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty);
                                IsValid = false;
                            }
                        }

                        var b = Cities.FirstOrDefault(x => x.Name == ws.Cells[row, 3].Value?.ToString()?.Trim())?.Id ?? Guid.Empty;

                        if (ws.Cells[row, 3].Value?.ToString()?.Trim() is not null)
                        {
                            if (a == Guid.Empty)
                            {
                                ToastService.ShowErrorImport(row, 1, ws.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty);
                                IsValid = false;
                            }
                        }

                        var c = Districts.FirstOrDefault(x => x.Name == ws.Cells[row, 4].Value?.ToString()?.Trim())?.Id ?? Guid.Empty;

                        if (ws.Cells[row, 4].Value?.ToString()?.Trim() is not null)
                        {
                            if (a == Guid.Empty)
                            {
                                ToastService.ShowErrorImport(row, 1, ws.Cells[row, 4].Value?.ToString()?.Trim() ?? string.Empty);
                                IsValid = false;
                            }
                        }

                        if (!IsValid)
                            continue;

                        var City = new VillageDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty,
                            ProvinceId = a,
                            CityId = b,
                            DistrictId = c, 
                        };

                        if (!Villages.Any(x => x.Name.Trim().ToLower() == City?.Name?.Trim().ToLower() && x.ProvinceId == City.ProvinceId && x.CityId == City.CityId && x.DistrictId == City.DistrictId))
                            v.Add(City);
                    }

                    await Mediator.Send(new CreateListVillageRequest(v));

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

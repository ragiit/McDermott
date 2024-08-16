using McHealthCare.Application.Extentions;
using Microsoft.AspNetCore.SignalR.Client;

namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class CountryPage : IAsyncDisposable
    {
        #region Variables

        private bool PanelVisible { get; set; } = true;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private HubConnection? hubConnection;
        private List<CountryDto> Countries = [];

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

                await LoadData();

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
                Countries.Clear();
                Countries = await Mediator.Send(new GetCountryQuery());
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
                    await Mediator.Send(new DeleteCountryRequest(((CountryDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<CountryDto>>();
                    await Mediator.Send(new DeleteCountryRequest(Ids: a.Select(x => x.Id).ToList()));
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
                var editModel = (CountryDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateCountryRequest(editModel));
                else
                    await Mediator.Send(new UpdateCountryRequest(editModel));

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

                    var countries = new List<CountryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new CountryDto
                        {
                            Code = ws.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty,
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty,
                        };

                        if (!Countries.Any(x => x.Name.Trim().ToLower() == country?.Name?.Trim().ToLower() && x.Code.Trim().ToLower() == country?.Code?.Trim().ToLower()))
                            countries.Add(country);
                    }

                    await Mediator.Send(new CreateListCountryRequest(countries));

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
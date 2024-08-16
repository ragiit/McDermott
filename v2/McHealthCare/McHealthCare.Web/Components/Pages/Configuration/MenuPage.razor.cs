using McHealthCare.Application.Extentions;
using Microsoft.AspNetCore.SignalR.Client;

namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class MenuPage : IAsyncDisposable
    {
        #region Variables

        private bool PanelVisible { get; set; } = true;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private HubConnection? hubConnection;
        private List<MenuDto> Menus = [];
        private List<MenuDto> ParentMenuDto = [];

        private List<ExportFileData> ExportFileDatas =
        [
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Parent"
            },
            new()
            {
                Column = "Sequence",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "URL"
            }
        ];

        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Variables

        private bool IsDeletedMenu { get; set; } = false;

        private void CanDeleteSelectedItemsMenu(GridFocusedRowChangedEventArgs e)
        {
            FocusedRowVisibleIndex = e.VisibleIndex;

            if (e.DataItem is not null)
                IsDeletedMenu = e.DataItem.Adapt<MenuDto>().IsDefaultData;
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = false;
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
                Menus = await Mediator.Send(new GetMenuQuery());
                ParentMenuDto = Menus.Where(x => x.ParentId == Guid.Empty || x.ParentId == null).ToList();
                SelectedDataItems = [];
                try
                {
                    Grid?.SelectRow(0, true);
                }
                catch { }
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                var a = ex;
                throw;
            }
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    if (((MenuDto)e.DataItem).IsDefaultData)
                        return;

                    await Mediator.Send(new DeleteMenuRequest(((MenuDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<MenuDto>>();
                    await Mediator.Send(new DeleteMenuRequest(Ids: a.Where(x => x.IsDefaultData == false).Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
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
                var editModel = (MenuDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateMenuRequest(editModel));
                else
                    await Mediator.Send(new UpdateMenuRequest(editModel));

                await LoadData();
            }
            catch (Exception ex)
            {
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

                    if (ws == null)
                    {
                        ToastService.ShowInfo("The Excel file does not contain any worksheets.");
                        return;
                    }

                    if (ws.Dimension == null || ws.Dimension.End.Row < 1)
                    {
                        ToastService.ShowInfo("The worksheet is empty.");
                        return;
                    }

                    var actualHeaders = Enumerable.Range(1, ws.Dimension.End.Column)
                        .Select(i => ws.Cells[1, i]?.Value?.ToString()?.Trim())
                        .ToList();

                    if (actualHeaders.Count != headerNames.Count)
                    {
                        ToastService.ShowInfo("The number of headers does not match the template.");
                        return;
                    }

                    var Menus = new List<MenuDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool IsValid = true;
                        var a = this.Menus.FirstOrDefault(x => x.Name == ws.Cells[row, 2].Value?.ToString()?.Trim())?.Id ?? Guid.Empty;

                        if (ws.Cells[row, 2].Value?.ToString()?.Trim() is not null)
                        {
                            if (a == Guid.Empty)
                            {
                                ToastService.ShowErrorImport(row, 1, ws.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty);
                                IsValid = false;
                            }
                        }

                        if (!IsValid)
                            continue;

                        var Menu = new MenuDto
                        {
                            ParentId = a == Guid.Empty ? null : a,
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty,
                            Sequence = Convert.ToInt64(ws.Cells[row, 3].Value?.ToString()?.Trim()),
                            Url = ws.Cells[row, 4].Value?.ToString()?.Trim() ?? null,
                        };

                        if (!this.Menus.Any(x => x.Name.Trim().ToLower() == Menu?.Name?.Trim().ToLower() && x.ParentId == Menu.ParentId && x.Sequence == Menu.Sequence && x.Url == Menu.Url))
                            Menus.Add(Menu);
                    }

                    await Mediator.Send(new CreateListMenuRequest(Menus));

                    await LoadData();

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
                finally
                {
                    PanelVisible = false;
                }
            }
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
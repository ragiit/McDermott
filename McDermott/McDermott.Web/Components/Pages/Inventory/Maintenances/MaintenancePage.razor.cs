using static McDermott.Application.Features.Commands.GetDataCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;

namespace McDermott.Web.Components.Pages.Inventory.Maintenances
{
    public partial class MaintenancePage
    {
        private List<MaintenanceDto> getMaintenance = [];
        private MaintenanceDto postMaintenance = new();
        private object Data {  get; set; }

        #region Variable

        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }

        #endregion Variable

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
            //    try
            //    {
            //        await GetUserInfo();
            //        StateHasChanged();
            //    }
            //    catch { }

            //    await LoadData();
            //    StateHasChanged();

            //    try
            //    {
            //        if (Grid is not null)
            //        {
            //            await Grid.WaitForDataLoadAsync();
            //            Grid.ExpandGroupRow(1);
            //            await Grid.WaitForDataLoadAsync();
            //            Grid.ExpandGroupRow(2);
            //            StateHasChanged();
            //        }
            //    }
            //    catch { }

            //    await LoadData();
            //    StateHasChanged();
            //}
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

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadData(newPageIndex, pageSize);
        }

        #endregion Searching

        #region Load

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            try
            {
                await GetUserInfo();

                var loadTasks = new[]
                {
                    LoadData()
                };

                await Task.WhenAll(loadTasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                showForm = false;
                Data = new GridDevExtremeDataSource<Maintenance>(await Mediator.Send(new GetQueryMaintenance()))
                {
                    CustomizeLoadOptions = (loadOptions) =>
                    {
                        loadOptions.PrimaryKey = ["Id"];
                        loadOptions.PaginateViaPrimaryKey = true;
                    }
                };
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

        public MarkupString GetIssueStatusIconHtml(EnumStatusMaintenance? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusMaintenance.Request:
                    priorityClass = "info";
                    title = "Request";
                    break;

                case EnumStatusMaintenance.InProgress:
                    priorityClass = "primary";
                    title = "In Progress";
                    break;

                case EnumStatusMaintenance.Repaired:
                    priorityClass = "warning";
                    title = "Repaire";
                    break;

                case EnumStatusMaintenance.Scrap:
                    priorityClass = "warning";
                    title = "Scrap";
                    break;

                case EnumStatusMaintenance.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusMaintenance.Canceled:
                    priorityClass = "danger";
                    title = "Cancel";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #endregion Load

        #region Grid

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;

            try
            {
                if ((MaintenanceDto)args.DataItem is null)
                    return;

                isActiveButton = ((MaintenanceDto)args.DataItem)!.Status!.Equals(EnumStatusMaintenance.Request);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            await EditItem_Click();
        }

        #endregion Grid

        #region button

        private async Task NewItem_Click()
        {
            NavigationManager.NavigateTo($"inventory/Maintenance/{EnumPageMode.Create.GetDisplayName()}");
            return;
        }

        private async Task EditItem_Click(MaintenanceDto? p = null)
        {
            var DataId = SelectedDataItems[0].Adapt<MaintenanceDto>();
            NavigationManager.NavigateTo($"inventory/Maintenance/{EnumPageMode.Update.GetDisplayName()}?Id={DataId.Id}");
            return;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void DeleteItem_Click()
        {
            Grid!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #endregion button

        #region Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteMaintenanceRequest(((MaintenanceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<MaintenanceDto>>();
                    await Mediator.Send(new DeleteMaintenanceRequest(ids: a.Select(x => x.Id).ToList()));
                }
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                await LoadData();
                StateHasChanged();
            }
        }

        #endregion Delete
    }
}
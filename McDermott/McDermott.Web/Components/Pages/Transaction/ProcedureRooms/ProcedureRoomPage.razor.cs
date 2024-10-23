namespace McDermott.Web.Components.Pages.Transaction.ProcedureRooms
{
    public partial class ProcedureRoomPage
    {
        private List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupports { get; set; } = [];

        #region Default Grid

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

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

        private string FormUrl = "clinic-service/procedure-rooms";
        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                PanelVisible = true;
                await GetUserInfo();
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new UpdateStatusGeneralConsultanServiceRequest
                    {
                        Id = ((GeneralConsultanMedicalSupportDto)e.DataItem).GeneralConsultanServiceId.GetValueOrDefault(),
                        Status = EnumStatusGeneralConsultantService.Physician,
                    });

                    await Mediator.Send(new DeleteGeneralConsultanMedicalSupportRequest(SelectedDataItems[0].Adapt<GeneralConsultanMedicalSupportDto>().Id));
                }
                else
                {
                    var selectedItems = SelectedDataItems.Adapt<List<GeneralConsultanMedicalSupportDto>>();
                    foreach (var item in selectedItems)
                    {
                        await Mediator.Send(new UpdateStatusGeneralConsultanServiceRequest
                        {
                            Id = item.GeneralConsultanServiceId.GetValueOrDefault(),
                            Status = EnumStatusGeneralConsultantService.Physician,
                        });
                    }

                    var a = SelectedDataItems.Adapt<List<GeneralConsultanMedicalSupportDto>>();
                    await Mediator.Send(new DeleteGeneralConsultanMedicalSupportRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var a = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                });
                GeneralConsultanMedicalSupports = a.Item1;
                totalCount = a.PageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        private void NewItem_Click()
        {
            NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Create.GetDisplayName()}");
            return;
        }

        private void EditItem_Click()
        {
            try
            {
                var GeneralConsultanMedicalSupport = SelectedDataItems[0].Adapt<GeneralConsultanMedicalSupportDto>();
                NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanMedicalSupport.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        #endregion Default Grid
    }
}
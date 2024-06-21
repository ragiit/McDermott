namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class MedicalCheckupPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private (bool, GroupMenuDto, User) Test = new();
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

        private async Task ClearCanvas()
        {
            await JsRuntime.InvokeVoidAsync("clearCanvas");
        }

        private async Task SaveCanvas()
        {
            await JsRuntime.InvokeVoidAsync("saveCanvas");
        }

        private IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool ShowForm { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private List<GeneralConsultanServiceDto> GeneralConsultanServices { get; set; } = [];
        private GeneralConsultanServiceDto GeneralConsultanService { get; set; } = new();

        private async Task OnDeleting()
        {
        }

        private void NewItem_Click()
        {
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
        }

        private async Task EditItem_Click()
        {
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
        }

        private async Task LoadComboBox()
        {
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task LoadData()
        {
        }

        private async Task OnValidSubmitSave()
        {
        }

        private async Task OnInvalidSubmitSave()
        {
        }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    await GetUserInfo();
                    StateHasChanged();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                await LoadComboBox();
                StateHasChanged();

                await JsRuntime.InvokeVoidAsync("initializeSignaturePad");
            }
        }

        private async Task OnClickCancel()
        {
            ShowForm = false;
            await LoadData();
        }
    }
}
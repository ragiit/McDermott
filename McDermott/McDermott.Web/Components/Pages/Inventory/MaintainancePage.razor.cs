using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class MaintainancePage
    {
        private List<MaintainanceDto> getMaintainance = [];
        private List<UserDto> getResponsibleBy = [];
        private List<UserDto> getRequestBy = [];
        private List<ProductDto> getEquipment = [];
        private MaintainanceDto postMaintainance = new();

        #region Variable
        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool isActiveButton { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexDetail { get; set; }

        private void unCheckIN(bool newValue)
        {
            postMaintainance.isInternal = true;
            if (newValue)
                postMaintainance.isExternal = false;
            else
                postMaintainance.isInternal = false;

        }
        private void unCheckEX(bool newValue)
        {
            postMaintainance.isExternal = true;
            if (newValue)
                postMaintainance.isInternal = false;
            else
                postMaintainance.isExternal = false;

        }
        private void unCheckCR(bool newValue)
        {
            postMaintainance.isCorrective = true;
            if (newValue)
                postMaintainance.isPreventive = false;
            else
                postMaintainance.isCorrective = false;
        }
        private void unCheckPR(bool newValue)
        {
            postMaintainance.isPreventive = true;
            if (newValue)
                postMaintainance.isCorrective = false;
            else
                postMaintainance.isPreventive = false;
        }
        private void unCheckRE(bool newValue)
        {
            postMaintainance.Recurrent = true;

        }

        private List<string> RepeatWork = new List<string>()
        {
            "Days",
            "Weeks",
            "Months",
            "Years"
        };
        #endregion
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

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

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                        StateHasChanged();
                    }
                }
                catch { }

                await LoadData();
                StateHasChanged();
            }
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

        #region Load
        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                getMaintainance = await Mediator.Send(new GetMaintainanceQuery());
                getEquipment = await Mediator.Send(new GetProductQuery(x => x.HospitalType == "Medical Equipment"));
                getRequestBy = await Mediator.Send(new GetUserQuery());
                getResponsibleBy = await Mediator.Send(new GetUserQuery(x => x.IsEmployee == true));
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

        public MarkupString GetIssueStatusIconHtml(EnumStatusMaintainance? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusMaintainance.Request:
                    priorityClass = "info";
                    title = "Request";
                    break;

                case EnumStatusMaintainance.InProgress:
                    priorityClass = "primary";
                    title = "In Progress";
                    break;

                case EnumStatusMaintainance.Repaired:
                    priorityClass = "warning";
                    title = "Repaire";
                    break;

                case EnumStatusMaintainance.Scrap:
                    priorityClass = "warning";
                    title = "Scrap";
                    break;
                case EnumStatusMaintainance.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;
                case EnumStatusMaintainance.Canceled:
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
        #endregion
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
                if ((MaintainanceDto)args.DataItem is null)
                    return;

                isActiveButton = ((MaintainanceDto)args.DataItem)!.Status!.Equals(EnumStatusMaintainance.Request);
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
        #endregion

        #region button

        private async Task HandleValidSubmit()
        {
            //IsLoading = true;
            FormValidationState = true;
            await OnSave();
            //IsLoading = false;
        }

        private async Task HandleInvalidSubmit()
        {
            showForm = true;
            FormValidationState = false;
        }

        private async Task NewItem_Click()
        {
            showForm = true;
        }

        private async Task EditItem_Click(MaintainanceDto? p = null)
        {
            try
            {
                showForm = true;
                PanelVisible = true;

                if (p != null)
                {
                    postMaintainance = p;
                }
                else if (SelectedDataItems.Count > 0)
                {
                    postMaintainance = SelectedDataItems[0].Adapt<MaintainanceDto>();
                }
                else
                {
                    ToastService.ShowWarning("No item selected for editing.");
                    return;
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

        private async Task Cancel_Click() { }
        private async Task InProgress_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintainance.Status = EnumStatusMaintainance.InProgress;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                await EditItem_Click(getMaintainanceById);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private async Task Repaired_Click() { }
        private async Task Scrap_Click() { }
        private async Task onDiscard() { }
        private async Task Refresh_Click()
        {
            await LoadData();
        }
        private void DeleteItem_Click()
        {
            Grid!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
        #endregion

        #region Delete
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        { }
        #endregion


        #region save
        MaintainanceDto getMaintainanceById = new();
        private async Task OnSave()
        {
            try
            {
                PanelVisible = true;
                if (postMaintainance is null)
                {
                    return;
                }
                if (postMaintainance.Id == 0)
                {
                    string prefix = "MNT-";
                    string datePart = DateTime.Now.ToString("ddMMyy");

                    var lastSequence = getMaintainance.Where(x => x.Sequence is null || x.Sequence.Substring(7, 4) == DateTime.Now.ToString("MMyy")).OrderByDescending(x => x.Sequence).FirstOrDefault();

                    int nextSequence = 1;
                    if (lastSequence != null)
                    {
                        var lastNumberPart = lastSequence?.Sequence?.Substring(lastSequence.Sequence.Length - 3);
                        nextSequence = int.Parse(lastNumberPart) + 1;
                    }

                    postMaintainance.Sequence = $"{prefix}{datePart}-{nextSequence.ToString("D3")}";
                    postMaintainance.Status = EnumStatusMaintainance.Request;

                    getMaintainanceById = await Mediator.Send(new CreateMaintainanceRequest(postMaintainance));
                    ToastService.ShowSuccess("Save Data Success..");
                }
                else
                {
                    getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                    ToastService.ShowSuccess("Update Data Success..");
                }

                PanelVisible = false;
                await EditItem_Click(getMaintainanceById);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

        }
        #endregion

    }
}

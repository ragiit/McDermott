using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Pharmacy.PharmacyCommand;

namespace McDermott.Web.Components.Pages.Inventory
{
    public partial class MaintainancePage
    {
        private List<MaintainanceDto> getMaintainance = [];
        private List<UserDto> getResponsibleBy = [];
        private List<UserDto> getRequestBy = [];
        private List<ProductDto> getEquipment = [];
        private List<LocationDto> getLocation = [];
        private List<TransactionStockDto> TransactionStocks = [];
        private MaintainanceDto postMaintainance = new();
        private TransactionStockDto postTransactionStock = new();

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

        private List<string> Batch = [];

        private async Task selectByProduct(ProductDto value)
        {
            var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == value.Id && s.LocationId == postMaintainance.LocationId));
            Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
            Batch = Batch.Distinct().ToList();
        }

        private async Task selectByLocation(LocationDto value)
        {
            postMaintainance.LocationId = value.Id;
            postMaintainance.EquipmentId = null;
            postMaintainance.SerialNumber = null;
        }

        private List<string> RepeatWork = new List<string>()
        {
            "Days",
            "Weeks",
            "Months",
            "Years"
        };

        #endregion Variable

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
                showForm = false;
                getMaintainance = await Mediator.Send(new GetMaintainanceQuery());
                getEquipment = await Mediator.Send(new GetProductQuery(x => x.HospitalType == "Medical Equipment"));
                getRequestBy = await Mediator.Send(new GetUserQuery());
                var Locations = (await Mediator.Send(new GetLocationQuery())).Item1;
                this.getLocation = Locations;
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

        #endregion Grid

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

        private async Task Cancel_Click()
        { }

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

        private async Task Repaired_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintainance.Status = EnumStatusMaintainance.Repaired;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                await EditItem_Click(getMaintainanceById);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Scrap_Click()
        {
            try
            {
                PanelVisible = true;
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                getMaintainance = await Mediator.Send(new GetMaintainanceQuery());
                var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(Maintainance))
                           .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                int NextReferenceNumber = 1;
                if (cekReference != null)
                {
                    int.TryParse(cekReference?.Substring("MNT#".Length), out NextReferenceNumber);
                    NextReferenceNumber++;
                }
                string referenceNumber = $"MNT#{NextReferenceNumber:D3}";

                foreach (var items in getMaintainance)
                {
                    var cekUom = getEquipment.Where(x => x.Id == postMaintainance.EquipmentId).Select(x => x.UomId).FirstOrDefault();

                    postTransactionStock.SourceTable = nameof(Maintainance);
                    postTransactionStock.SourcTableId = postMaintainance.Id;
                    postTransactionStock.ProductId = items.EquipmentId;
                    postTransactionStock.LocationId = items.LocationId;
                    postTransactionStock.Batch = items.SerialNumber;
                    postTransactionStock.ExpiredDate = null;
                    postTransactionStock.Reference = referenceNumber;
                    postTransactionStock.Quantity = -1;
                    postTransactionStock.UomId = cekUom;
                    postTransactionStock.Validate = true;

                    await Mediator.Send(new CreateTransactionStockRequest(postTransactionStock));
                }

                //update Maintainance
                postMaintainance.Status = EnumStatusMaintainance.Scrap;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                await EditItem_Click(getMaintainanceById);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Done_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintainance.Status = EnumStatusMaintainance.Done;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                await EditItem_Click(getMaintainanceById);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task onDiscard()
        {
            await LoadData();
            StateHasChanged();
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
                    await Mediator.Send(new DeleteMaintainanceRequest(((MaintainanceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<MaintainanceDto>>();
                    await Mediator.Send(new DeleteMaintainanceRequest(ids: a.Select(x => x.Id).ToList()));
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

        #region save

        private MaintainanceDto getMaintainanceById = new();

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

        #endregion save
    }
}
using McDermott.Application.Dtos.Pharmacies;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PharmacyCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PrescriptionCommand;

namespace McDermott.Web.Components.Pages.Pharmacies.Prescription
{
    public partial class PrescriptionPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

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

        #region relation Data

        private PharmacyDto postPharmacy { get; set; } = new();
        private List<PharmacyDto> getPharmacy { get; set; } = [];

        #endregion relation Data

        #region Static Variables

        [Parameter]
        public long Id { get; set; }

        private IGrid Grid
        {
            get; set;
        }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        [Parameter]
        public bool IsPopUpForm { get; set; } = true;
        private bool PanelVisible { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private string header { get; set; } = string.Empty;

        public MarkupString GetIssueStatusIconHtml(EnumStatusPharmacy? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumStatusPharmacy.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumStatusPharmacy.SendToPharmacy:
                    priorityClass = "primary";
                    title = "Pharmacy";
                    break;

                case EnumStatusPharmacy.Received:
                    priorityClass = "primary";
                    title = "Received";
                    break;

                case EnumStatusPharmacy.Processed:
                    priorityClass = "warning";
                    title = "Processed";
                    break;

                case EnumStatusPharmacy.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                case EnumStatusPharmacy.Cancel:
                    priorityClass = "danger";
                    title = "Canceled";
                    break;

                default:
                    return new MarkupString("");
            }
            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #endregion Static Variables

        #region Load Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
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

        #endregion Searching

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var result = await Mediator.Send(new GetPharmacyQuery
                {
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    PageSize =pageSize
                });
                getPharmacy = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = result.PageIndex;

                PanelVisible = false;
            }
            catch (Exception ex) { ex.HandleException(ToastService); }
        }



        #endregion Load Data

        #region Delete Function

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                var data_delete = (PharmacyDto)e.DataItem;
                var Prescriptions = await Mediator.Send(new GetAllPrescriptionQuery());
                var Concoctions = await Mediator.Send(new GetAllConcoctionQuery());
                var ConcoctionLines = await Mediator.Send(new GetAllConcoctionLineQuery());
                var StockOutPrescriptions = await Mediator.Send(new GetAllStockOutPrescriptionQuery());
                var Logs = await Mediator.Send(new GetAllPharmacyLogQuery());
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    var prescription_data = Prescriptions.Where(x => x.PharmacyId == data_delete.Id).ToList();
                    if (prescription_data.Count > 0)
                    {
                        foreach (var item_delete in prescription_data)
                        {
                            var StockOutPrescriptions_data = StockOutPrescriptions.Where(x => x.PrescriptionId == item_delete.Id).ToList();
                            if (StockOutPrescriptions_data.Count > 0)
                            {
                                foreach (var items_delete in StockOutPrescriptions_data)
                                {
                                    await Mediator.Send(new DeleteConcoctionLineRequest(items_delete.Id));
                                }
                            }
                            await Mediator.Send(new DeletePrescriptionRequest(item_delete.Id));
                        }
                    }

                    var concoction_data = Concoctions.Where(x => x.PharmacyId == data_delete.Id).ToList();
                    if (concoction_data.Count > 0)
                    {
                        foreach (var item_delete in concoction_data)
                        {
                            var concoctionLine_data = ConcoctionLines.Where(x => x.ConcoctionId == item_delete.Id).ToList();
                            if (concoctionLine_data.Count > 0)
                            {
                                foreach (var items_delete in concoctionLine_data)
                                {
                                    await Mediator.Send(new DeleteConcoctionLineRequest(items_delete.Id));
                                }
                            }
                            await Mediator.Send(new DeleteConcoctionRequest(item_delete.Id));
                        }
                    }

                    var data_log = Logs.Where(x => x.PharmacyId == data_delete.Id).ToList();
                    foreach (var item in data_log)
                    {
                        await Mediator.Send(new DeletePharmacyLogRequest(item.Id));
                    }

                    await Mediator.Send(new DeletePharmacyRequest(((PharmacyDto)e.DataItem).Id));
                }
                else
                {
                    var datas = SelectedDataItems.Adapt<List<PharmacyDto>>().Select(x => x.Id).ToList();
                    foreach (var item in datas)
                    {
                        var prescription_data = Prescriptions.Where(x => x.PharmacyId == item).ToList();
                        if (prescription_data.Count > 0)
                        {
                            foreach (var item_delete in prescription_data)
                            {
                                var StockOutPrescriptions_data = StockOutPrescriptions.Where(x => x.PrescriptionId == item_delete.Id).ToList();
                                if (StockOutPrescriptions_data.Count > 0)
                                {
                                    foreach (var items_delete in StockOutPrescriptions_data)
                                    {
                                        await Mediator.Send(new DeleteConcoctionLineRequest(items_delete.Id));
                                    }
                                }
                                await Mediator.Send(new DeletePrescriptionRequest(item_delete.Id));
                            }
                        }

                        var concoction_data = Concoctions.Where(x => x.PharmacyId == item).ToList();
                        if (concoction_data.Count > 0)
                        {
                            foreach (var item_delete in concoction_data)
                            {
                                var concoctionLine_data = ConcoctionLines.Where(x => x.ConcoctionId == item_delete.Id).ToList();
                                if (concoctionLine_data.Count > 0)
                                {
                                    foreach (var items_delete in concoctionLine_data)
                                    {
                                        await Mediator.Send(new DeleteConcoctionLineRequest(items_delete.Id));
                                    }
                                }
                                await Mediator.Send(new DeleteConcoctionRequest(item_delete.Id));
                            }
                        }
                        var data_log = Logs.Where(x => x.PharmacyId == item).ToList();
                        foreach (var items in data_log)
                        {
                            await Mediator.Send(new DeletePharmacyLogRequest(items.Id));
                        }

                        await Mediator.Send(new DeletePharmacyRequest(item));
                    }
                }
                ToastService.ShowSuccess("Delete Data Success!!");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Delete Function

        #region function Edit Click

        private async Task EditItem_Click()
        {
            var data = SelectedDataItems[0].Adapt<PharmacyDto>();
            NavigationManager.NavigateTo($"pharmacy/prescriptions/{EnumPageMode.Update.GetDisplayName()}/?Id={data.Id}");
        }

        #endregion function Edit Click

        #region Refresh fuction

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        #endregion Refresh fuction

        #region Delete Grid Config

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Delete Grid Config



    }
}
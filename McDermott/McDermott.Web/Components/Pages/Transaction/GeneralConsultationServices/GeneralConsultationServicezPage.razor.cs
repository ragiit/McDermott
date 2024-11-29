using DocumentFormat.OpenXml.InkML;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;

namespace McDermott.Web.Components.Pages.Transaction.GeneralConsultationServices
{
    public partial class GeneralConsultationServicezPage
    {
        private List<GeneralConsultanServiceDto> GeneralConsultanServices { get; set; } = [];

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

        private string FormUrl = "clinic-service/general-consultation-services";
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

        private List<StatusMcuData> StatusMcus = [];

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>().Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>();
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(ids: a.Select(x => x.Id).ToList()));
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
                //Data = "GeneralConsultanServices.OrderByDescending(x => x.RegistrationDate).ThenByDescending(x => x.IsAlertInformationSpecialCase).ThenByDescending(x => x.ClassType is not null)"

                PanelVisible = true;
                SelectedDataItems = [];
                var a = await Mediator.Send(new GetGeneralConsultanServicesQuery
                {
                    OrderByList =
                    [
                        (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
                        (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
                        (x => x.ClassType != null, true),               // ThenByDescending ClassType is not null
                        (x=>x.IsClaim, true),
                    ],
                    Predicate = x => x.Service != null && x.Service.IsVaccination == false && x.Service.IsMcu == false && x.Service.IsMaternity == false && x.Service.IsTelemedicine == false,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                });
                GeneralConsultanServices = a.Item1;
                totalCount = a.PageCount;
                activePageIndex = pageIndex;
                //var a = await Mediator.QueryGetHelper<GeneralConsultanService, GeneralConsultanServiceDto>(pageIndex, pageSize, searchTerm);
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
                var GeneralConsultanService = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();
                NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanService.Id}");
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

        public MarkupString GetIssuePriorityIconHtml(GeneralConsultanServiceDto priority)
        {
            if (priority is not null)
            {
                if (!priority.IsAlertInformationSpecialCase && priority.ClassType is null)
                    return new MarkupString("");

                string priorytyClass = "danger";
                string title = string.Empty;

                if (priority.IsAlertInformationSpecialCase && priority.ClassType is not null)
                    title = $" Priority, {priority.ClassType}";
                else
                {
                    if (priority.ClassType is not null)
                        title = $"{priority.ClassType}";
                    if (priority.IsAlertInformationSpecialCase)
                        title = $" Priority ";
                }

                string html = string.Format("<span class='badge bg-{0} py-1 px-2' title='{1} Priority'>{1}</span>", priorytyClass, title);

                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        public MarkupString GetIssueClaimIconHtml(GeneralConsultanServiceDto priority)
        {
            if (priority is not null)
            {
                if (!priority.IsClaim)
                    return new MarkupString("");

                string priorytyClass = "info";
                string title = string.Empty;

                if (priority.IsClaim)
                    title = $" Claim";
                else
                {
                    if (priority.IsClaim)
                        title = $"Claim";
                }

                string html = string.Format("<span class='badge bg-{0} py-1 px-2' title='{1} Priority'>{1}</span>", priorytyClass, title);

                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        private bool IsDashboard { get; set; } = false;

        #region Chart

        private async Task LoadDashboard(bool f)
        {
            if (f)
            {
                IsDashboard = true;
                var ser = await Mediator.Send(new GetGeneralConsultanServicesQuery
                {
                    IsGetAll = true,
                    Select = x => new GeneralConsultanService
                    {
                        Status = x.Status
                    }
                });

                StatusMcus = GetStatusMcuCounts(ser.Item1);
            }
            else
                IsDashboard = false;
        }

        public class StatusMcuData
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public List<StatusMcuData> GetStatusMcuCounts(List<GeneralConsultanServiceDto> services)
        {
            var aa = services.GroupBy(s => s.Status)
                            .Select(g => new StatusMcuData
                            {
                                Status = g.Key.GetDisplayName(),
                                Count = g.Count()
                            }).ToList();
            return aa;
        }

        #endregion Chart
    }
}
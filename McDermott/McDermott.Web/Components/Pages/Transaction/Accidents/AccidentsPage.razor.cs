using DocumentFormat.OpenXml.Office2010.Excel;
using static McDermott.Application.Features.Commands.Transaction.AccidentCommand;

namespace McDermott.Web.Components.Pages.Transaction.Accidents
{
    public partial class AccidentsPage
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

        private string FormUrl = "clinic-service/accidents";
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
                    var id = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>().Id;

                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(id: id));
                    await Mediator.Send(new DeleteAccidentByGcIdRequest
                    {
                        Id = id,
                    });
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>();
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(ids: a.Select(x => x.Id).ToList()));
                    await Mediator.Send(new DeleteAccidentByGcIdRequest
                    {
                        Ids = a.Select(x => x.Id).ToList()
                    });
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
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
                var a = await Mediator.Send(new GetGeneralConsultanServicesQuery
                {
                    OrderByList =
                    [
                        (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
                        (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
                        (x => x.ClassType != null, true)               // ThenByDescending ClassType is not null
                    ],
                    Select = x => new GeneralConsultanService
                    {
                        Id = x.Id,
                        Patient = new User
                        {
                            Name = x.Patient == null ? "" : x.Patient.Name,
                        },
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner == null ? "" : x.Pratitioner.Name
                        }
                    },
                    Predicate = x => x.IsAccident == true,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                });
                GeneralConsultanServices = a.Item1;

                foreach (var service in GeneralConsultanServices)
                {
                    service.Accident = await Mediator.Send(new GetSingleAccidentQuery
                    {
                        Predicate = x => x.GeneralConsultanServiceId == service.Id,
                        Select = x => new Accident
                        {
                            Id = x.Id,
                            SafetyPersonnel = new User
                            {
                                Name = x.SafetyPersonnel == null ? "" : x.SafetyPersonnel.Name
                            },
                            DateOfOccurrence = x.DateOfOccurrence,
                            DateOfFirstTreatment = x.DateOfFirstTreatment,
                            RibbonSpecialCase = x.RibbonSpecialCase,
                            Sent = x.Sent,
                            EmployeeClass = x.EmployeeClass,
                            EstimatedDisability = x.EstimatedDisability,
                            AreaOfYard = x.AreaOfYard,
                            Status = x.Status,
                            EmployeeDescription = x.EmployeeDescription,
                            AccidentLocation = x.AccidentLocation,
                        }
                    });
                }

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
                var GeneralConsultanService = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();
                NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanService.Accident?.Id}");
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
    }
}
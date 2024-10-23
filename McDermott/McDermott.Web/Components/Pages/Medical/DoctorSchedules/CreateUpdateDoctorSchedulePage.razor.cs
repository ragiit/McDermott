using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.Medical.DoctorScheduleCommand;
using static McDermott.Application.Features.Commands.Medical.DoctorScheduledCommand;
using static McDermott.Application.Features.Commands.Medical.DoctorScheduleDetailCommand;

namespace McDermott.Web.Components.Pages.Medical.DoctorSchedules
{
    public partial class CreateUpdateDoctorSchedulePage
    {
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

        private string FormUrl = "medical/doctor-schedules";
        private bool PanelVisible { get; set; } = true;

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private DoctorScheduleDto DoctorSchedule { get; set; } = new();
        private List<InsuranceDto> Insurances { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataUser();
            await LoadDataService();

            PanelVisible = false;
        }

        #region ComboboxUser

        private DxComboBox<UserDto, long> refUserComboBox { get; set; }
        private int UserComboBoxIndex { get; set; } = 0;
        private int totalCountUser = 0;

        private async Task OnSearchUser()
        {
            await LoadDataUser();
        }

        private async Task OnSearchUserIndexIncrement()
        {
            if (UserComboBoxIndex < (totalCountUser - 1))
            {
                UserComboBoxIndex++;
                await LoadDataUser(UserComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUserndexDecrement()
        {
            if (UserComboBoxIndex > 0)
            {
                UserComboBoxIndex--;
                await LoadDataUser(UserComboBoxIndex, 10);
            }
        }

        private async Task OnInputUserChanged(string e)
        {
            UserComboBoxIndex = 0;
            await LoadDataUser();
        }

        private List<UserDto> Users = [];

        private async Task LoadDataUser(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetUserQuery2(
                 predicate: x => x.IsDoctor == true && x.IsPhysicion == true,
                 pageIndex: pageIndex,
                 pageSize: pageSize,
                 searchTerm: refUserComboBox?.Text ?? "",
                 select: x => new User
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Email = x.Email
                 }

            ));
            Users = result.Item1;
            totalCountUser = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxUser

        private async Task LoadData()
        {
            var result = await Mediator.Send(new GetSingleDoctorScheduledQuery
            {
                Predicate = x => x.Id == Id
            });

            DoctorSchedule = new();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo(FormUrl);
                    return;
                }

                DoctorSchedule = result ?? new();

                await LoadDataDoctorScheduleDetail();
            }
        }

        private bool IsLoading { get; set; } = false;

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                return;
            }
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                PanelVisible = true;

                if (DoctorSchedule.Id == 0)
                    DoctorSchedule = await Mediator.Send(new CreateDoctorScheduledRequest(DoctorSchedule));
                else
                    await Mediator.Send(new UpdateDoctorScheduledRequest(DoctorSchedule));

                NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={DoctorSchedule.Id}");
            }
            catch (Exception ex)
            {
                PanelVisible = false;
                ex.HandleException(ToastService);
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task HandleInvalidSubmit()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        #region Doctor Schedule Detail

        private List<DoctorScheduleDetailDto> DoctorScheduleDetails { get; set; } = [];
        private List<ServiceDto> Services { get; set; } = [];
        public IGrid GridDoctorScheduleDetail { get; set; }
        private IReadOnlyList<object> SelectedDataItemsDoctorScheduleDetail { get; set; } = [];
        private int FocusedRowVisibleIndexDoctorScheduleDetail { get; set; }

        private async Task NewItemDoctorScheduleDetail_Click()
        {
            await GridDoctorScheduleDetail.StartEditNewRowAsync();
        }

        private DoctorScheduleDetailDto TempDoctorScheduleDetail { get; set; } = new();

        private async Task EditItemDoctorScheduleDetail_Click(IGrid context)
        {
            await GridDoctorScheduleDetail.StartEditRowAsync(FocusedRowVisibleIndexDoctorScheduleDetail);

            var a = (GridDoctorScheduleDetail.GetDataItem(FocusedRowVisibleIndexDoctorScheduleDetail) as DoctorScheduleDetailDto ?? new());

            PanelVisible = true;
            Services = (await Mediator.Send(new GetServiceQuery
            {
                Predicate = x => x.Id == a.ServiceId,
            })).Item1;
            TempDoctorScheduleDetail = a;

            PanelVisible = false;
        }

        private void DeleteItemGridDoctorScheduleDetail_Click()
        {
            GridDoctorScheduleDetail.ShowRowDeleteConfirmation(FocusedRowVisibleIndexDoctorScheduleDetail);
        }

        private void GridDoctorScheduleDetail_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexDoctorScheduleDetail = args.VisibleIndex;
        }

        private void DeleteItemDoctorScheduleDetail_Click()
        {
            GridDoctorScheduleDetail.ShowRowDeleteConfirmation(FocusedRowVisibleIndexDoctorScheduleDetail);
        }

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadDataDoctorScheduleDetail(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadDataDoctorScheduleDetail(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadDataDoctorScheduleDetail(newPageIndex, pageSize);
        }

        #endregion Searching

        private int totalCountDoctorScheduleDetail { get; set; } = 0;

        private async Task LoadDataDoctorScheduleDetail(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemsDoctorScheduleDetail = [];
                var a = await Mediator.Send(new GetDoctorScheduleDetailQuery
                {
                    Includes =
                    [
                        x => x.Service
                    ],
                    SearchTerm = searchTerm,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    Predicate = x => x.DoctorScheduleId == DoctorSchedule.Id
                });

                DoctorScheduleDetails = a.Item1;
                totalCountDoctorScheduleDetail = a.PageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSaveDoctorScheduleDetail(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (DoctorScheduleDetailDto)e.EditModel;

                editModel.DoctorScheduleId = DoctorSchedule.Id;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateDoctorScheduleDetailRequest(editModel));
                else
                    await Mediator.Send(new UpdateDoctorScheduleDetailRequest(editModel));

                await LoadDataDoctorScheduleDetail(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private List<string> DayOfWeeks = new List<string>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday",
        };

        private async Task OnDeleteDoctorScheduleDetail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var aq = SelectedDataItemsDoctorScheduleDetail.Count;
                if (SelectedDataItemsDoctorScheduleDetail is null)
                {
                    var id = ((DoctorScheduleDetailDto)e.DataItem).Id;
                    await Mediator.Send(new DeleteDoctorScheduleDetailRequest
                    {
                        Id = id
                    });
                }
                else
                {
                    var a = SelectedDataItemsDoctorScheduleDetail.Adapt<List<DoctorScheduleDetailDto>>();
                    await Mediator.Send(new DeleteDoctorScheduleDetailRequest
                    {
                        Ids = a.Select(x => x.Id).ToList()
                    });
                }
                await LoadDataDoctorScheduleDetail(0, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Doctor Schedule Detail

        #region ComboboxService

        private DxComboBox<ServiceDto, long> refServiceComboBox { get; set; }
        private int ServiceComboBoxIndex { get; set; } = 0;
        private int totalCountService = 0;

        private async Task OnSearchService()
        {
            await LoadDataService();
        }

        private async Task OnSearchServiceIndexIncrement()
        {
            if (ServiceComboBoxIndex < (totalCountService - 1))
            {
                ServiceComboBoxIndex++;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchServiceIndexDecrement()
        {
            if (ServiceComboBoxIndex > 0)
            {
                ServiceComboBoxIndex--;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnInputServiceChanged(string e)
        {
            ServiceComboBoxIndex = 0;
            await LoadDataService();
        }

        private async Task LoadDataService(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetServiceQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = refServiceComboBox?.Text ?? ""
                });
                Services = result.Item1;
                totalCountService = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxService
    }
}
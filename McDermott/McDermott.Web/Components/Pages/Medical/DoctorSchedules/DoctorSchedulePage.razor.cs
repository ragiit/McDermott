namespace McDermott.Web.Components.Pages.Medical.DoctorSchedules
{
    public partial class DoctorSchedulePage
    {
        private List<DoctorScheduleDto> DoctorSchedules { get; set; } = [];

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

        private string FormUrl = "medical/doctor-schedules";
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
                    await Mediator.Send(new DeleteDoctorScheduledWithDetail(SelectedDataItems[0].Adapt<DoctorScheduleDto>().Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DoctorScheduleDto>>();
                    await Mediator.Send(new DeleteDoctorScheduledWithDetail
                    {
                        Ids = a.Select(x => x.Id).ToList()
                    });
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
                var a = await Mediator.Send(new GetDoctorScheduledQuery
                {
                    Includes =
                    [
                        x => x.Physicion
                    ],
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm ?? "",
                });
                DoctorSchedules = a.Item1;
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

        private async Task GenerateScheduleDoctor_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private void NewItem_Click()
        {
            NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Create.GetDisplayName()}");
            return;
        }

        private void EditItem_Click()
        {
            try
            {
                var DoctorSchedule = SelectedDataItems[0].Adapt<DoctorScheduleDto>();
                NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={DoctorSchedule.Id}");
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

        #region Doctor Schedule Slot

        #region ComboboxUser

        private List<UserDto> Physicians { get; set; } = [];
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

        private async Task OnSearchUserIndexDecrement()
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

        private async Task LoadDataUser(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.IsDoctor == true && x.IsPhysicion == true,
                    SearchTerm = refUserComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        IsPhysicion = x.IsPhysicion,
                    }
                });
                Physicians = result.Item1;
                totalCountUser = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxUser

        #endregion Doctor Schedule Slot

        private DoctorScheduleSlotDto DoctorScheduleSlot { get; set; } = new();

        private async Task SaveScheduleSlot()
        {
            try
            {
                PanelVisible = true;

                var doctorSchedule = await Mediator.Send(new GetSingleDoctorScheduledQuery
                {
                    Predicate = x => x.PhysicionId == DoctorScheduleSlot.PhysicianId,
                    Select = x => new DoctorSchedule
                    {
                        Id = x.Id
                    }
                });

                var doctorScheduleDetails = (await Mediator.Send(new GetDoctorScheduleDetailQuery
                {
                    Predicate = x => x.DoctorScheduleId == doctorSchedule.Id,
                    IsGetAll = true,
                    Select = x => new DoctorScheduleDetail
                    {
                        DoctorScheduleId = x.DoctorScheduleId,
                        DayOfWeek = x.DayOfWeek,
                        WorkFrom = x.WorkFrom,
                        WorkTo = x.WorkTo,
                        Quota = x.Quota,
                        ServiceId = x.ServiceId,
                    }
                })).Item1;

                var doctorSchedultSlots = new List<DoctorScheduleSlotDto>();
                for (DateTime date = DoctorScheduleSlot.StartDate.Date; date <= DoctorScheduleSlot.EndDate.Date; date = date.Date.AddDays(1))
                {
                    var details = doctorScheduleDetails.Where(x => x.DayOfWeek.Trim().Equals(date.DayOfWeek.ToString().Trim())).ToList();

                    if (details.Count != 0)
                    {
                        foreach (var d in details)
                        {
                            doctorSchedultSlots.Add(new DoctorScheduleSlotDto
                            {
                                PhysicianId = DoctorScheduleSlot.PhysicianId,
                                DayOfWeek = d.DayOfWeek,
                                WorkFrom = d.WorkFrom,
                                WorkTo = d.WorkTo,
                                Quota = d.Quota,
                                ServiceId = d.ServiceId.GetValueOrDefault(),
                                StartDate = date,
                            });
                        }
                    }
                }

                if (doctorSchedultSlots.Count > 0)
                {
                    doctorSchedultSlots = doctorSchedultSlots.DistinctBy(x => new { x.PhysicianId, x.DayOfWeek, x.WorkFrom, x.WorkTo, x.Quota, x.ServiceId, x.StartDate }).ToList();

                    // Panggil BulkValidateCountryQuery untuk validasi bulk
                    var existingCountrys = await Mediator.Send(new BulkValidateDoctorScheduleSlotQuery(doctorSchedultSlots));

                    // Filter Country baru yang tidak ada di database
                    doctorSchedultSlots = doctorSchedultSlots.Where(Country =>
                        !existingCountrys.Any(ev =>
                            ev.PhysicianId == Country.PhysicianId &&
                            ev.DayOfWeek == Country.DayOfWeek &&
                            ev.WorkFrom == Country.WorkFrom &&
                            ev.WorkTo == Country.WorkTo &&
                            ev.Quota == Country.Quota &&
                            ev.ServiceId == Country.ServiceId &&
                            ev.StartDate == Country.StartDate
                        )
                    ).ToList();

                    await Mediator.Send(new CreateListDoctorScheduleSlotRequest(doctorSchedultSlots));
                    await LoadData(0, pageSize);
                    SelectedDataItems = [];
                }

                ToastService.ShowSuccess($"{doctorSchedultSlots.Count} schedule were successfully added.");

                DoctorScheduleSlot = new();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Default Grid
    }
}
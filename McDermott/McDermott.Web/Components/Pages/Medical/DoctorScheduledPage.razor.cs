using Microsoft.AspNetCore.Razor.Language.Extensions;
using System.ComponentModel.DataAnnotations;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class DoctorScheduledPage
    {
        private List<HealthCenterDto> HealthCenters = [];
        public List<BuildingDto> Buildings = [];
        public List<LocationDto> Locations = [];
        public BuildingDto Building = new();

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

        //public List<DoctorScheduleDetailDto> DoctorScheduleDetails = [];

        private List<ServiceDto> Services = [];
        private IEnumerable<UserDto> AllUsers = [];
        private IEnumerable<UserDto> Users = [];
        private IEnumerable<UserDto> SelectedPhysicions = [];
        private List<DoctorScheduleDto> DoctorSchedules = [];
        private List<DoctorScheduleDto> Schedules = new List<DoctorScheduleDto>();

        [Required]
        private IEnumerable<DoctorScheduleDto> SelectedSchedules = [];

        private DoctorScheduleDto DoctorSchedule = new();

        private List<DoctorScheduleDetailDto> DoctorScheduleDetails = [];
        public List<DoctorScheduleDetailDto> DeletedDoctorScheduleDetails = [];

        private DateTime StartDate = DateTime.Now;
        private DateTime EndDate = DateTime.Now;

        private long newId = 0;
        private string PhysicionName { get; set; }

        private long? _ServiceId { get; set; }

        private long? ServiceId
        {
            get => _ServiceId;
            set
            {
                Users = [];
                DoctorSchedule.ServiceId = value.GetValueOrDefault().ToInt32();
                SelectedPhysicions = [];
                _ServiceId = value;
                Users = AllUsers.Where(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(value.GetValueOrDefault().ToInt32())).AsEnumerable();
            }
        }

        private long Value
        {
            get => newId;
            set
            {
                Names.Clear();
                long newId = value; InvokeAsync(StateHasChanged);
                this.newId = value;

                var item = Schedules.FirstOrDefault(x => x.Id == newId);

                //PhysicionName = item?.Physicions ?? string.Empty;

                //var n = item?.Physicions.Split(",") ?? [];

                //foreach (var item1 in n)
                //{
                //    if (Names.Contains(item1))
                //        continue;

                //    Names.Add(item1);
                //}

                SelectedNames = Names.Distinct();
            }
        }

        #region Default Grid

        //private string DisplayFormat { get; } = string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? "HH:mm" : "h:mm tt";
        private string DisplayFormat { get; } = "HH:mm";

        private bool PanelVisible { get; set; } = true;
        private bool LoadingGenerateScheduleDoctor { get; set; } = false;
        private bool IsAddMenu { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        public IGrid Grid { get; set; }
        public IGrid GridDoctorScheduleDetail { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowDoctorScheduleDetailVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedDoctorScheduleDetailDataItems { get; set; } = new ObservableRangeCollection<object>();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            //Services = await Mediator.Send(new GetServiceQuery());

            await GetUserInfo();
            await LoadData();
        }

        private List<string> Names { get; set; } = new();
        private List<string> DeletedNames { get; set; } = new();
        private IEnumerable<string> SelectedNames { get; set; } = new List<string>();
        private DoctorScheduleDto tt { get; set; } = new();

        private string selectedItem;

        private List<Item> dataSource = new List<Item>
    {
        new Item { Id = 1, Name = "Item 1" },
        new Item { Id = 2, Name = "Item 2" },
        new Item { Id = 3, Name = "Item 3" }
    };

        // Event handler untuk ComboBox

        public class Item
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        private void ComboBoxValueChanged(string newValue)
        {
            // Lakukan sesuatu ketika nilai combobox berubah
            Console.WriteLine("Nilai combobox berubah menjadi: " + newValue);
        }

        private void SelectedItemsChanged(IEnumerable<DoctorScheduleDto> e)
        {
            SelectedNames = new List<string>();

            if (e is not null)
            {
                Names.Clear();

                foreach (var item in e)
                {
                    //var n = item.Physicions.Split(",");

                    //foreach (var item1 in n)
                    //{
                    //    if (Names.Contains(item1))
                    //        continue;

                    //    Names.Add(item1);
                    //}
                }

                SelectedNames = Names.Distinct();
            }
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            var users = await Mediator.Send(new GetUserQuery());
            AllUsers = users.Where(x => x.IsDoctor == true && x.IsPhysicion == true).AsEnumerable();
            Users = users.Where(x => x.IsDoctor == true && x.IsPhysicion == true).AsEnumerable();

            DoctorSchedules.Clear();

            var doctorSchedules = await Mediator.Send(new GetDoctorScheduleQuery());
            //doctorSchedules.ForEach(x => x.Physicions = string.Join(", ", users.Where(z => x.PhysicionIds != null && x.PhysicionIds.Contains(z.Id)).Select(z => z.Name).ToList()));

            DoctorSchedules = doctorSchedules;

            Schedules = doctorSchedules;

            SelectedDataItems = new ObservableRangeCollection<object>();
            var Buildings = await Mediator.Send(new GetBuildingQuery());
            this.Buildings = Buildings.Item1;

            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteDoctorScheduleRequest(SelectedDataItems[0].Adapt<DoctorScheduleDto>().Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DoctorScheduleDto>>();
                    await Mediator.Send(new DeleteListDoctorScheduleRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private void OnDeleteDoctorScheduleDetail(GridDataItemDeletingEventArgs e)
        {
            var aaa = SelectedDoctorScheduleDetailDataItems.Adapt<List<DoctorScheduleDetailDto>>();
            DoctorScheduleDetails.RemoveAll(x => aaa.Select(z => z.Name).Contains(x.Name));
            SelectedDoctorScheduleDetailDataItems = [];
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (!SelectedSchedules.Any() && EndDate.Date > StartDate.Date)
                    return;

                #region MyRegion

                //var addedSlots = new List<DoctorScheduleSlotDto>();
                //var addedDetails = new List<DoctorScheduleDetailDto>();
                //var result = new List<DoctorScheduleSlotDto>();

                //for (DateTime date = StartDate; date < EndDate; date = date.Date.AddDays(1))
                //{
                //    var currentdate = date;
                //    foreach (DoctorScheduleDto doctorSchedule in SelectedSchedules)
                //    {
                //        if (addedSlots.Any(x => x.DoctorScheduleId == doctorSchedule.Id))
                //        {
                //            if (!addedSlots.Any(x => x.DoctorScheduleId == doctorSchedule.Id && x.StartDate.Date == date.Date))
                //            {
                //                var checkDayOfWeek = addedDetails.FirstOrDefault(x => x.DoctorScheduleId == doctorSchedule.Id && x.DayOfWeek.Trim().Equals(date.DayOfWeek.ToString().Trim()));

                //                if (checkDayOfWeek is not null)
                //                {
                //                    result.Add(new DoctorScheduleSlotDto
                //                    {
                //                        DoctorScheduleId = doctorSchedule.Id,
                //                        StartDate = date.Date
                //                    });
                //                }
                //            }
                //            continue;
                //        }

                //        var checkSlotTemp = addedSlots.FirstOrDefault(x => x.DoctorScheduleId == doctorSchedule.Id && x.StartDate.Date == date.Date);

                //        if (checkSlotTemp is null)
                //        {
                //            var scheduleSlots = await Mediator.Send(new GetDoctorScheduleSlotByDoctorScheduleIdRequest(doctorSchedule.Id));
                //            addedSlots.AddRange(scheduleSlots);

                //            var details = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(doctorSchedule.Id));
                //            addedDetails.AddRange(details);

                //            if (scheduleSlots.FirstOrDefault(x => x.DoctorScheduleId == doctorSchedule.Id && x.StartDate.Date == date.Date) is not null)
                //                continue;

                //            var checkDayOfWeek = addedDetails.FirstOrDefault(x => x.DoctorScheduleId == doctorSchedule.Id && x.DayOfWeek.Trim().Equals(date.DayOfWeek.ToString().Trim()));

                //            if (checkDayOfWeek is not null)
                //            {
                //                result.Add(new DoctorScheduleSlotDto
                //                {
                //                    DoctorScheduleId = doctorSchedule.Id,
                //                    StartDate = date.Date
                //                });
                //            }
                //        }
                //    }
                //}

                #endregion MyRegion

                #region MyRegion

                var addedSlots = new List<DoctorScheduleSlotDto>();
                var addedDetails = new List<DoctorScheduleDetailDto>();
                var result = new List<DoctorScheduleSlotDto>();

                // Memuat data yang dibutuhkan sebelum loop tanggal
                var doctorDetailsDict = new Dictionary<long, List<DoctorScheduleDetailDto>>();
                var doctorSlotsDict = new Dictionary<long, List<DoctorScheduleSlotDto>>();

                foreach (DoctorScheduleDto doctorSchedule in SelectedSchedules)
                {
                    var doctorScheduleId = doctorSchedule.Id;
                    var doctorSlots = await Mediator.Send(new GetDoctorScheduleSlotByDoctorScheduleIdRequest(doctorScheduleId));
                    var doctorDetails = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(doctorScheduleId));

                    doctorSlotsDict[doctorScheduleId] = doctorSlots;
                    doctorDetailsDict[doctorScheduleId] = doctorDetails;
                }

                for (DateTime date = StartDate.Date; date <= EndDate.Date; date = date.Date.AddDays(1))
                {
                    foreach (DoctorScheduleDto doctorSchedule in SelectedSchedules)
                    {
                        var doctorScheduleId = doctorSchedule.Id;
                        var doctorSlots = doctorSlotsDict[doctorScheduleId];
                        var doctorDetails = doctorDetailsDict[doctorScheduleId];

                        // Check apakah slot untuk tanggal tersebut sudah ada
                        if (doctorSlots.Any(x => x.DoctorScheduleId == doctorScheduleId && x.StartDate.Date == date.Date))
                            continue;

                        // Mendapatkan detail jadwal dokter untuk hari tersebut
                        var details = doctorDetails.Where(x => x.DoctorScheduleId == doctorScheduleId && x.DayOfWeek.Trim().Equals(date.DayOfWeek.ToString().Trim())).ToList();

                        // Jika ada detail jadwal, tambahkan slot baru ke hasil
                        if (details.Count > 0)
                        {
                            foreach (var physicion in doctorSchedule.PhysicionIds!)
                            {
                                foreach (var detail in details)
                                {
                                    result.Add(new DoctorScheduleSlotDto
                                    {
                                        WorkFrom = detail.WorkFrom,
                                        WorkTo = detail.WorkTo,
                                        DoctorScheduleId = doctorScheduleId,
                                        PhysicianId = physicion,
                                        StartDate = date.Date
                                    });
                                }
                            }
                        }
                    }
                }

                //for (DateTime date = StartDate; date <= EndDate; date = date.Date.AddDays(1))
                //{
                //    long tempId = 0;
                //    var doctorDetails = new List<DoctorScheduleDetailDto>();
                //    var doctorSlots = new List<DoctorScheduleSlotDto>();

                //    foreach (DoctorScheduleDto doctorSchedule in SelectedSchedules)
                //    {
                //        if (tempId == 0)
                //        {
                //            doctorSlots = await Mediator.Send(new GetDoctorScheduleSlotByDoctorScheduleIdRequest(doctorSchedule.Id));

                //            doctorDetails = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(doctorSchedule.Id));
                //        }

                //        if (doctorSlots.FirstOrDefault(x => x.DoctorScheduleId == doctorSchedule.Id && x.StartDate.Date == date.Date) is not null)
                //            continue;

                //        var details = doctorDetails.Where(x => x.DoctorScheduleId == doctorSchedule.Id && x.DayOfWeek.Trim().Equals(date.DayOfWeek.ToString().Trim())).ToList();

                //        if (details.Count > 0)
                //        {
                //            foreach (var physicion in doctorSchedule.PhysicionIds!)
                //            {
                //                foreach (var detail in details)
                //                {
                //                    result.Add(new DoctorScheduleSlotDto
                //                    {
                //                        WorkFrom = detail.WorkFrom,
                //                        WorkTo = detail.WorkTo,
                //                        DoctorScheduleId = doctorSchedule.Id,
                //                        PhysicianId = physicion,
                //                        StartDate = date.Date
                //                    });
                //                }
                //            }
                //        }

                //        tempId = 0;

                //        //if (!addedSlots.Any(x => x.DoctorScheduleId == doctorSchedule.Id))
                //        //{
                //        //    var scheduleSlots = await Mediator.Send(new GetDoctorScheduleSlotByDoctorScheduleIdRequest(doctorSchedule.Id));
                //        //    addedSlots.AddRange(scheduleSlots);

                //        //    var details = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(doctorSchedule.Id));
                //        //    addedDetails.AddRange(details.Distinct());
                //        //    addedDetails.Distinct();

                //        //    if (scheduleSlots.FirstOrDefault(x => x.DoctorScheduleId == doctorSchedule.Id && x.StartDate.Date == date.Date) is not null)
                //        //        continue;
                //        //}

                //        //var checkSlotTemp = addedSlots.FirstOrDefault(x => x.DoctorScheduleId == doctorSchedule.Id && x.StartDate.Date == date.Date);

                //        //if (checkSlotTemp is null)
                //        //{
                //        //    var checkDayOfWeek = addedDetails.Where(x => x.DoctorScheduleId == doctorSchedule.Id && x.DayOfWeek.Trim().Equals(date.DayOfWeek.ToString().Trim())).ToList();

                //        //    if (checkDayOfWeek is not null && checkDayOfWeek.Count > 0)
                //        //    {
                //        //        foreach (var p in doctorSchedule.PhysicionIds!)
                //        //        {
                //        //            foreach (var c in checkDayOfWeek.Distinct())
                //        //            {
                //        //                result.Add(new DoctorScheduleSlotDto
                //        //                {
                //        //                    WorkFrom = c.WorkFrom,
                //        //                    WorkTo = c.WorkTo,
                //        //                    DoctorScheduleId = doctorSchedule.Id,
                //        //                    PhysicianId = p,
                //        //                    StartDate = date.Date
                //        //                });
                //        //            }
                //        //        }

                //        //        //doctorSchedule.PhysicionIds!.ForEach(physicionId =>
                //        //        //{
                //        //        //    checkDayOfWeek.ForEach(x =>
                //        //        //    {
                //        //        //        result.Add(new DoctorScheduleSlotDto
                //        //        //        {
                //        //        //            WorkFrom = x.WorkFrom,
                //        //        //            WorkTo = x.WorkTo,
                //        //        //            DoctorScheduleId = doctorSchedule.Id,
                //        //        //            PhysicianId = physicionId,
                //        //        //            StartDate = date.Date
                //        //        //        });
                //        //        //    });
                //        //        //});
                //        //    }
                //        //}
                //    }
                //}

                #endregion MyRegion

                await Mediator.Send(new CreateListDoctorScheduleSlotRequest(result));

                ToastService.ShowSuccess("Generated Schedule Doctor Successfully");
            }
            catch { }
        }

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
                }
                catch { }
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

        private async Task OnRowClick(GridRowClickEventArgs e)
        {
            var a = e.Grid.GetRowValue(e.VisibleIndex, "Name");
            var b = e.Grid.GetRowValue(e.VisibleIndex, "Id");
            var c = e.Grid.GetRowValue(e.VisibleIndex, "Service");
            // await JsRuntime.InvokeVoidAsync("alert", $"{a} {b} {c}"); // Alert
        }

        private void OnSaveDoctorScheduleDetail(GridEditModelSavingEventArgs e)
        {
            var DoctorScheduleDetail = (DoctorScheduleDetailDto)e.EditModel;

            DoctorScheduleDetailDto updateBuilding = new();

            if (IsAddMenu)
            {
                if (DoctorScheduleDetails.Where(x => x.Name == DoctorScheduleDetail.Name).Any())
                    return;

                DoctorScheduleDetails.Add(DoctorScheduleDetail);
            }
            else
            {
                var q = SelectedDoctorScheduleDetailDataItems[0].Adapt<DoctorScheduleDetailDto>();
                var check = DoctorScheduleDetails.FirstOrDefault(x => x.Name == DoctorScheduleDetail.Name);

                if (check is not null && q.Name != DoctorScheduleDetail.Name)
                    return;

                updateBuilding = DoctorScheduleDetails.FirstOrDefault(x => x.Name == q.Name)!;

                var index = DoctorScheduleDetails.IndexOf(updateBuilding!);

                DoctorScheduleDetails[index] = DoctorScheduleDetail;
            }

            SelectedDoctorScheduleDetailDataItems = new ObservableRangeCollection<object>();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            EditItemsEnabled = true;
        }

        private void GridDoctorScheduleDetail_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowDoctorScheduleDetailVisibleIndex = args.VisibleIndex;
        }

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

        private void NewItem_Click()
        {
            ShowForm = true;
            DoctorScheduleDetails.Clear();
            ServiceId = null;
            DoctorSchedule = new();
        }

        private async Task GenerateScheduleDoctor_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task OnRowDoubleClick()
        {
            try
            {
                DoctorSchedule = SelectedDataItems[0].Adapt<DoctorScheduleDto>();
                ShowForm = true;

                if (DoctorSchedule != null)
                {
                    DeletedDoctorScheduleDetails = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(DoctorSchedule.Id));
                    DoctorScheduleDetails = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(DoctorSchedule.Id));
                    var a = Users.Where(x => DoctorSchedule.PhysicionIds != null && DoctorSchedule.PhysicionIds.Contains(x.Id)).ToList();
                    SelectedPhysicions = a;
                }
            }
            catch { }
        }

        private async Task EditItem_Click()
        {
            try
            {
                DoctorSchedule = SelectedDataItems[0].Adapt<DoctorScheduleDto>();
                ServiceId = DoctorSchedule.ServiceId;
                ShowForm = true;

                if (DoctorSchedule != null)
                {
                    DeletedDoctorScheduleDetails = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(DoctorSchedule.Id));
                    DoctorScheduleDetails = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(DoctorSchedule.Id));
                    var a = Users.Where(x => DoctorSchedule.PhysicionIds != null && DoctorSchedule.PhysicionIds.Contains(x.Id)).ToList();
                    SelectedPhysicions = a;
                }
            }
            catch { }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void CancelItemDoctorScheduleDetailGrid_Click()
        {
            DoctorScheduleDetails = [];
            DoctorSchedule = new();
            SelectedDoctorScheduleDetailDataItems = [];
            ShowForm = false;
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task NewItemDoctorScheduleDetail_Click()
        {
            IsAddMenu = true;
            await GridDoctorScheduleDetail.StartEditNewRowAsync();
        }

        private async Task EditItemDoctorScheduleDetail_Click()
        {
            IsAddMenu = false;
            await GridDoctorScheduleDetail.StartEditRowAsync(FocusedRowDoctorScheduleDetailVisibleIndex);
        }

        private void DeleteItemDoctorScheduleDetail_Click()
        {
            GridDoctorScheduleDetail.ShowRowDeleteConfirmation(FocusedRowDoctorScheduleDetailVisibleIndex);
        }

        private async Task SaveItemDoctorScheduleDetailGrid_Click()
        {
            try
            {
                if (DoctorScheduleDetails is null) return;

                await Mediator.Send(new DeleteDoctorScheduleDetailByScheduleIdRequest(DeletedDoctorScheduleDetails.Select(x => x.Id).ToList()));

                DoctorScheduleDto result = new();

                DoctorSchedule.PhysicionIds = SelectedPhysicions.Select(x => x.Id).ToList();

                if (DoctorSchedule.Id == 0)
                {
                    result = await Mediator.Send(new CreateDoctorScheduleRequest(DoctorSchedule));
                }
                else
                {
                    await Mediator.Send(new UpdateDoctorScheduleRequest(DoctorSchedule));
                }

                DoctorScheduleDetails.ForEach(x =>
                {
                    x.Id = 0;
                    x.DoctorSchedule = null;
                    x.DoctorScheduleId = DoctorSchedule.Id == 0 ? result.Id : DoctorSchedule.Id;
                });

                await Mediator.Send(new DeleteDoctorScheduleSlotByPhysicionIdRequest(DoctorSchedule.PhysicionIds, DoctorSchedule.Id));

                //await Mediator.Send(new CreateDoctorScheduleDetailRequest(DoctorScheduleDetails));

                ShowForm = false;

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void ColumnChooserButtonDoctorScheduleDetail_Click()
        {
            GridDoctorScheduleDetail.ShowColumnChooser();
        }

        private async Task ExportXlsxItemDoctorScheduleDetail_Click()
        {
            await GridDoctorScheduleDetail.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportXlsItemDoctorScheduleDetail_Click()
        {
            await GridDoctorScheduleDetail.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportCsvItemDoctorScheduleDetail_Click()
        {
            await GridDoctorScheduleDetail.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        #endregion Default Grid
    }
}
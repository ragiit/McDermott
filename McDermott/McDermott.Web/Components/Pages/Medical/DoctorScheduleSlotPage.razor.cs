namespace McDermott.Web.Components.Pages.Medical
{
    public partial class DoctorScheduleSlotPage
    {
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
                var user = await UserInfoService.GetUserInfo();
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        public List<DoctorScheduleGrid> DoctorScheduleGrids { get; set; } = [];

        public class DoctorScheduleGrid
        {
            public long PhysicionId { get; set; }
            public string Physicion { get; set; } = string.Empty;
            public List<long> DoctorScheduleIds { get; set; } = [];
        };

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequest(SelectedDataItems[0].Adapt<DoctorScheduleGrid>().DoctorScheduleIds, SelectedDataItems[0].Adapt<DoctorScheduleGrid>().PhysicionId));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DoctorScheduleGrid>>();

                    await Mediator.Send(new DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequest(a.SelectMany(d => d.DoctorScheduleIds).ToList(), a.Select(d => d.PhysicionId).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            try
            {
                DoctorScheduleGrids.Clear();

                var schedules = await Mediator.Send(new GetDoctorScheduleQuery());

                var users = await Mediator.Send(new GetUserQuery());

                foreach (var schedule in schedules)
                {
                    var physicions = schedule.PhysicionIds;

                    foreach (var physicion in physicions!)
                    {
                        var doctorScheduleGrids = DoctorScheduleGrids.FirstOrDefault(x => x.PhysicionId == physicion);

                        var a = await Mediator.Send(new GetDoctorScheduleSlotByDoctorScheduleIdRequest(schedule.Id, physicion));
                        if (a.Count > 0)
                        {
                            if (doctorScheduleGrids is null)
                            {
                                DoctorScheduleGrids.Add(new DoctorScheduleGrid
                                {
                                    PhysicionId = physicion,
                                    Physicion = users.FirstOrDefault(x => x.Id == physicion)!.Name,
                                    DoctorScheduleIds = [schedule.Id],
                                });
                            }
                            else
                            {
                                if (schedule.PhysicionIds!.Contains(physicion))
                                {
                                    doctorScheduleGrids.DoctorScheduleIds.Add(schedule.Id);
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            PanelVisible = false;
        }

        private async Task Edit_Click()
        {
        }

        private async Task Delete_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #region Default Grid

        public IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = true;
        private bool IsAddMenu { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private List<DoctorScheduleDto> DoctorSchedules { get; set; } = [];

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            EditItemsEnabled = true;
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

        #endregion Default Grid
    }
}
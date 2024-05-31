namespace McDermott.Web.Components.Pages.Employee
{
    public partial class SickLeavePage
    {
        #region Relation Data

        private List<GeneralConsultanServiceDto> generalConsultans = [];
        private List<GeneralConsultanCPPTDto> CPPTs = [];
        private List<DiagnosisDto> Diagnoses = [];
        private List<SickLeaveDto> SickLeaves = [];
        private List<UserDto> Users = [];

        #endregion Relation Data

        #region Static Variabel

        [Parameter]
        public long Id { get; set; }

        private IGrid Grid { get; set; }
        private bool IsLoading { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Static Variabel

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

        #region async Data

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
            }
            catch { }

            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                IsLoading = true;
                generalConsultans = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.IsSickLeave == true || x.IsMaternityLeave == true));
                Users = await Mediator.Send(new GetUserQuery());
                CPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery());
                Diagnoses = await Mediator.Send(new GetDiagnosisQuery());

                foreach (var item in generalConsultans)
                {
                    var BodyCPPT = CPPTs.Where(x => x.GeneralConsultanServiceId == item.Id && x.Title == "Diagnosis").Select(x => x.Body).FirstOrDefault();
                    var diagnosis = Diagnoses.Where(x => BodyCPPT!.Contains(x.Name)).Select(x => x.Name).FirstOrDefault();
                    var newDataGridSickLeave = new SickLeaveDto
                    {
                        PatientName = item.Patient.Name,
                        NoRM = item.NoRM,
                        StartSickLeave = item.StartDateSickLeave,
                        EndSickLeave = item.EndDateSickLeave,
                        Diagnosis = diagnosis
                    };

                    SickLeaves.Add(newDataGridSickLeave);
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion async Data

        #region Grid Configuration

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
        }

        private void Grid_FocusedRowChangedDetail(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            //await EditItem_Click(null);
        }

        #endregion Grid Configuration
    }
}
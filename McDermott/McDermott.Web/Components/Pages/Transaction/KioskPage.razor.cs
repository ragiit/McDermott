using DevExpress.Data.XtraReports.Native;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class KioskPage
    {
        #region Relation Data

        public List<KioskDto> Kiosks = new();
        public List<ServiceDto> Services = new();
        public List<UserDto> Patients = new();
        public List<UserDto> Physician = new();
        public List<DoctorScheduleDto> Physicians = [];
        public List<DoctorScheduleDto> DoctorSchedules = new();

        #endregion Relation Data

        #region setings Grid

        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool showForm { get; set; } = false;
        private string textPopUp = "";
        public IGrid Grid { get; set; }
        private int ActiveTabIndex { get; set; } = 1;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private GroupMenuDto UserAccessCRUID = new();

        #endregion setings Grid

        #region Data Static And Variable Additional

        private List<string> type = new List<string>
        {
            "NIP",
            "Oracle",
            "SAP",
            "NIP"
        };

        private string? NamePatient { get; set; } = string.Empty;

        private KioskDto FormKios = new();
        private int ServicedId = 0;
        private List<string> Names { get; set; } = new List<string>();
        private IEnumerable<string> SelectedNames { get; set; } = new List<string>();
        private string PhysicionName { get; set; }

        private int Serviced
        {
            get => ServicedId;
            set
            {
                int ServicedId = value; InvokeAsync(StateHasChanged);
                this.ServicedId = value;

                Names.Clear();

                var item = DoctorSchedules.Where(x => x.ServiceId == ServicedId).ToList();
                item.ForEach(x => x.Physicions = string.Join(", ", Physician.Where(z => x.PhysicionIds != null && x.PhysicionIds.Contains(z.Id)).Select(z => z.Name).ToList()));
                Physicians = item;
            }
        }

        #endregion Data Static And Variable Additional

        #region Async Data And Auth

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    var result = await NavigationManager.CheckAccessUser(oLocal);
                    IsAccess = result.Item1;
                    UserAccessCRUID = result.Item2;
                }
                catch { }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }
            //var by =

            Kiosks = await Mediator.Send(new GetKioskQuery());
            await LoadData();
        }

        private async Task LoadData()
        {
            showForm = false;
            PanelVisible = true;
            Physician = await Mediator.Send(new GetUserQuery());
            SelectedDataItems = new ObservableRangeCollection<object>();
            Kiosks = await Mediator.Send(new GetKioskQuery());
            Services = await Mediator.Send(new GetServiceQuery());
            DoctorSchedules = await Mediator.Send(new GetDoctorScheduleQuery());
            PanelVisible = false;
        }

        #endregion Async Data And Auth

        #region Config Grid

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
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

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        #endregion Config Grid

        #region Button Function

        private async Task NewItem_Click()
        {
            showForm = true;
            textPopUp = "Add Data Kiosk";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            try
            {
                var General = SelectedDataItems[0].Adapt<KioskDto>();
                FormKios = General;
                showForm = true;
                textPopUp = "Edit Data Kiosk";
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            }
            catch { }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #region Button Export, Import And Colmn Chooser

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

        #endregion Button Export, Import And Colmn Chooser

        private void OnCancel()
        {
            FormKios = new();
            showForm = false;
        }

        private async Task OnSearch()
        {
            var types = FormKios.Type;
            var InputSearch = FormKios.NumberType;
            Patients = await Mediator.Send(new GetDataUserForKioskQuery(types, InputSearch));
            if (Patients != null)
            {
                showForm = true;
                NamePatient = Patients.Select(x => x.Name).FirstOrDefault();
                FormKios.PatientId = Patients.Select(x => x.Id).FirstOrDefault();
                FormKios.Insurance = "Personal";
            }
            else
            {
                showForm = false;
                //AlertColor alertColor = AlertColor.Primary;
                //IconName alertIconName = IconName.CheckCircleFill;
                //string alertMessage = "A simple alert - check it out!";
            }
        }

        #endregion Button Function

        #region Methode Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteKioskRequest(((KioskDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<KioskDto>>();
                    await Mediator.Send(new DeleteListKioskRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        #endregion Methode Delete

        #region Methode Save And Update

        private async Task OnSave()
        {
            try
            {
                var edit = FormKios;
                if (FormKios.Id == 0)
                    await Mediator.Send(new CreateKioskRequest(FormKios));
                else
                    await Mediator.Send(new UpdateKioskRequest(FormKios));

                FormKios = new();
                await LoadData();
            }
            catch { }
        }

        #endregion Methode Save And Update
    }
}
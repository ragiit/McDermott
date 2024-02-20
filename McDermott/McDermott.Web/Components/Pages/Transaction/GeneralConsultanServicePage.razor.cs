using DevExpress.Data.XtraReports.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;
using System.Runtime.InteropServices;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class GeneralConsultanServicePage
    {
        #region Relation Data

        private List<GeneralConsultanServiceDto> GeneralConsultanServices = new();
        private List<UserDto> IsPatient = new();
        private List<UserDto> patients = new List<UserDto>();
        private int PatientIds = new();
        private IEnumerable<GeneralConsultanServiceDto> SelectPatient = [];

        private List<UserDto> IsPratition = new();
        private List<UserDto> AllDoctors = new();
        private List<InsuranceDto> Insurances = [];
        private List<InsuranceDto> AllInsurances = [];
        private List<InsurancePolicyDto> InsurancePolicies = [];
        private List<ServiceDto> Services = new();

        private IEnumerable<DoctorScheduleDto> SelectedSchedules = [];
        private IEnumerable<string> SelectedNames { get; set; } = new List<string>();
        private List<string> Names { get; set; } = new();

        #endregion Relation Data

        #region Data Statis

        public class NurseStation
        {
            public int Id { get; set; }
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public IEnumerable<NurseStation> NurseStations { get; set; } = new List<NurseStation>
        {
            new NurseStation { Id = 1, Status = "Planned", Count = 10 },
            new NurseStation { Id = 2, Status = "Confirmed", Count = 5 },
            new NurseStation { Id = 3, Status = "Waiting", Count = 2 },
            new NurseStation { Id = 4, Status = "Physician", Count = 1 },
            new NurseStation { Id = 5, Status = "Finished", Count = 0 },
        };

        private List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        private List<string> RegisType = new List<string>
        {
            "General Consultation",
            "Emergency",
            "MCU"
        };

        private List<string> Method = new List<string>
        {
            "MCU",
            "Gas And Oil"
        };

        private int PatientsId = 0;

        private int PractitionerId = 0;

        private int Age = 0;
        private DateTime? Birthdate { get; set; }
        private string IdentityNum { get; set; }

        #endregion Data Statis

        #region Grid Setting

        private GeneralConsultanServiceDto FormRegis = new();
        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool showForm { get; set; } = false;
        private string textPopUp = "";
        private string Timeee = "";
        private string DisplayFormat { get; } = string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? "HH:mm" : "h:mm tt";
        public IGrid Grid { get; set; }
        private int ActiveTabIndex { get; set; } = 0;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private GroupMenuDto UserAccessCRUID = new();
        private List<Temppp> Temppps { get; set; } = [];

        private class Temppp
        {
            public string Title { get; set; }
            public string Body { get; set; }
        }

        #endregion Grid Setting

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

            GeneralConsultanServices = await Mediator.Send(new GetGeneralConsultanServiceQuery());
            InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery());

            await LoadData();
        }

        private int _InsurancePolicyId { get; set; }
        private int InsuranceId { get; set; }

        private int InsurancePolicyId
        {
            get => _InsurancePolicyId;
            set
            {
                _InsurancePolicyId = value;
            }
        }

        private void GetInsurancePhysician(int value)
        {
            if (string.IsNullOrWhiteSpace(PaymentMethod))
                return;

            InsurancePolicies.Clear();

            if (PaymentMethod.Equals("BPJS"))
            {
                Insurances = AllInsurances.Where(x => InsurancePolicies.Select(z => z.InsuranceId).Contains(x.Id) && x.IsBPJS == true).ToList();
            }
            else
            {
                Insurances = AllInsurances.Where(x => InsurancePolicies.Select(z => z.InsuranceId).Contains(x.Id) && x.IsBPJS == false).ToList();
            }
        }

        private int _ServiceId { get; set; }

        private int ServiceId
        {
            get => _ServiceId;
            set
            {
                _ServiceId = value;
                FormRegis.ServiceId = value;
                IsPratition = AllDoctors.Where(x => x.DoctorServiceIds.Contains(value)).ToList();

                //var schedules = await Mediator.Send(new GetDoctorScheduleQuery());
            }
        }

        private int _DoctorId { get; set; }

        private int DoctorId
        {
            get => _DoctorId;
            set
            {
                _DoctorId = value;
                SetTimeSchedule(value, RegistrationDate);
            }
        }

        private DateTime _RegistrationDate { get; set; } = DateTime.Now;

        private DateTime RegistrationDate
        {
            get => _RegistrationDate;
            set
            {
                _RegistrationDate = value;
                SetTimeSchedule(DoctorId, value);
            }
        }

        private List<string> Times = [];

        private async Task GetScheduleTimesUser(int value)
        {
            var slots = await Mediator.Send(new GetDoctorScheduleSlotQuery(x => x.PhysicianId == value));
        }

        private async Task SetTimeSchedule(int patientId, DateTime date)
        {
            var slots = await Mediator.Send(new GetDoctorScheduleSlotQuery(x => x.PhysicianId == patientId && x.StartDate.Date == date.Date && x.DoctorSchedule.ServiceId == ServiceId));

            Times.Clear();

            Times.AddRange(slots.Select(x => $"{x.WorkFromFormatString} - {x.WorkToFormatString}"));
        }

        private string _PaymentMethod { get; set; }
        private string MedicalTypee { get; set; }
        private List<InsuranceTemp> Temps = [];

        private class InsuranceTemp
        {
            public int InsurancePolicyId { get; set; }
            public int InsuranceId { get; set; }
            public string InsuranceName { get; set; }
            public string PolicyNumber { get; set; }

            public string ConcatInsurancePolicy
            { get { return PolicyNumber + " - " + InsuranceName; } }
        }

        private string PaymentMethod
        {
            get => _PaymentMethod;
            set
            {
                _PaymentMethod = value;

                Insurances.Clear();

                if (PaymentMethod.Equals("BPJS"))
                {
                    var all = InsurancePolicies.Where(x => x.UserId == PatientsId && x.Insurance.IsBPJS == true).ToList();
                    Temps = all.Select(x => new InsuranceTemp
                    {
                        InsurancePolicyId = x.Id,
                        InsuranceId = x.InsuranceId,
                        InsuranceName = x.Insurance.Name,
                        PolicyNumber = x.PolicyNumber
                    }).ToList();
                }
                else
                {
                    var all = InsurancePolicies.Where(x => x.UserId == PatientsId && x.Insurance.IsBPJS != true).ToList();
                    Temps = all.Select(x => new InsuranceTemp
                    {
                        InsurancePolicyId = x.Id,
                        InsuranceId = x.InsuranceId,
                        InsuranceName = x.Insurance.Name,
                        PolicyNumber = x.PolicyNumber
                    }).ToList();
                }

                //GetInsurancePhysician(Value);
            }
        }

        private int Value
        {
            get => PatientsId;
            set
            {
                //Names.Clear();
                int PatientsId = value; InvokeAsync(StateHasChanged);
                this.PatientsId = value;

                var item = patients.FirstOrDefault(x => x.Id == PatientsId);

                if (item.DateOfBirth != null)
                {
                    DateTime currentDate = DateTime.UtcNow;
                    Birthdate = item.DateOfBirth;
                    Age = currentDate.Year - Birthdate!.Value.Year;
                }

                FormRegis.Age = Age;
                FormRegis.NoRM = item.NoRm;
                FormRegis.IdentityNumber = item.NoId.ToString();
                FormRegis.PatientId = item.Id;
            }
        }

        private void SelectedService(DoctorScheduleDto docter)
        {
            //var selectedServices = DoctorScheduleDto
            //    .Where(service => PhysicionIds.Any(physicionId =>
            //        physicions.Any(physicion => physicion.Id == physicionId && service.PhysicionIds.Contains(physicionId.Id))))
            //    .ToList();
        }

        private async Task SelectData()
        {
            var user = await Mediator.Send(new GetUserQuery());

            //patient
            patients = [.. user.Where(x => x.IsPatient == true).ToList()];
            PatientIds = IsPatient.Select(x => x.Id).FirstOrDefault();

            //IsDocter
            IsPratition = [.. user.Where(x => x.IsDoctor == true).ToList()];
            AllDoctors = [.. user.Where(x => x.IsDoctor == true).ToList()];

            //Insurance
            AllInsurances = await Mediator.Send(new GetInsuranceQuery());

            //Medical Type
            Services = await Mediator.Send(new GetServiceQuery());

            //Schendule
            var schendule = await Mediator.Send(new GetDoctorScheduleQuery());
        }

        private async Task LoadData()
        {
            showForm = false;
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            GeneralConsultanServices = await Mediator.Send(new GetGeneralConsultanServiceQuery());
            PanelVisible = false;
        }

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

        private async Task NewItem_Click()
        {
            await SelectData();
            showForm = true;
            textPopUp = "Add Data Registration";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            try
            {
                var General = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();
                FormRegis = General;
                await SelectData();
                showForm = true;
                textPopUp = "Edit Data Registration";
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            }
            catch { }
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
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

        private void OnCancel()
        {
            FormRegis = new();
            showForm = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(((GeneralConsultanServiceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>();
                    await Mediator.Send(new DeleteListGeneralConsultanServiceRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task OnSave()
        {
            try
            {
                var edit = FormRegis;
                if (FormRegis.Id == 0)
                    await Mediator.Send(new CreateGeneralConsultanServiceRequest(FormRegis));
                else
                    await Mediator.Send(new UpdateGeneralConsultanServiceRequest(FormRegis));

                FormRegis = new();
                await LoadData();
            }
            catch { }
        }
    }
}
using McDermott.Application.Dtos.Queue;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Linq;
using static Azure.Core.HttpHeader;
using static McDermott.Application.Features.Commands.Queue.KioskConfigCommand;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class KioskPage
    {
        #region Relation Data

        public List<KioskDto> Kiosks = new();
        public List<KioskQueueDto> KioskQueues = new();
        public List<KioskConfigDto> KioskConf = new();
        public List<ServiceDto> Services = new();
        public List<ServiceDto> serv = new List<ServiceDto>();
        public List<UserDto> Phys = new();
        public ServiceDto servs = new();
        public List<UserDto> Patients = new();
        public List<UserDto> Physician = new();

        #endregion Relation Data

        #region setings Grid

        //private BaseAuthorizationLayout AuthorizationLayout = new();
        //private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;

        private bool showForm { get; set; } = false;
        private bool showPhysician { get; set; } = false;
        private string textPopUp = "";
        private string HeaderName { get; set; } = string.Empty;
        public IGrid Grid { get; set; }
        private int ActiveTabIndex { get; set; } = 1;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        //private GroupMenuDto UserAccessCRUID = new();

        #endregion setings Grid

        #region Data Static And Variable Additional

        [Parameter]
        public int id { get; set; }

        private List<string> type = new List<string>
        {
            "NIP",
            "Oracle",
            "SAP",
            "Legacy"
        };

        private string? NamePatient { get; set; } = string.Empty;
        private int? CountServiceId { get; set; }

        private KioskDto FormKios = new();
        private KioskQueueDto FormQueue = new();
        private bool showQueue { get; set; } = false;
        private int _ServiceId { get; set; }
        private string Bpjs { get; set; } = string.Empty;
        private IEnumerable<ServiceDto> SelectedServices = [];
        private IEnumerable<string> SelectedNames { get; set; } = new List<string>();

        #endregion Data Static And Variable Additional

        #region Async Data And Auth

        private int ServiceId
        {
            get => _ServiceId;
            set
            {
                _ServiceId = value;
                FormKios.ServiceId = value;
                Phys = Physician.Where(x => x.DoctorServiceIds.Contains(value)).ToList();

                showPhysician = true;

                //var schedules = await Mediator.Send(new GetDoctorScheduleQuery());
            }
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await base.OnAfterRenderAsync(firstRender);

        //    if (firstRender)
        //    {
        //        try
        //        {
        //            var result = await NavigationManager.CheckAccessUser(oLocal);
        //            IsAccess = result.Item1;
        //            UserAccessCRUID = result.Item2;
        //        }
        //        catch { }
        //    }
        //}

        protected override async Task OnInitializedAsync()
        {
            //    try
            //    {
            //        var result = await NavigationManager.CheckAccessUser(oLocal);
            //        IsAccess = result.Item1;
            //        UserAccessCRUID = result.Item2;
            //    }
            //    catch { }
            //var by =

            Kiosks = await Mediator.Send(new GetKioskQuery());
            await LoadData();
            foreach (var i in KioskConf)
            {
                HeaderName = i.Name;
                break;
            }
        }

        private async Task LoadData()
        {
            showForm = false;
            PanelVisible = true;
            Physician = await Mediator.Send(new GetUserQuery());
            SelectedDataItems = new ObservableRangeCollection<object>();
            Kiosks = await Mediator.Send(new GetKioskQuery());
            Services = await Mediator.Send(new GetServiceQuery());
            var kconfig = await Mediator.Send(new GetKioskConfigQuery());
            KioskConf = kconfig.Where(x => x.Id == id).ToList();
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
                FormKios.BPJS = Patients.Select(x => x.NoBpjsKs).FirstOrDefault();
                if (FormKios.BPJS != null)
                {
                    FormKios.StageBpjs = true;
                }

                foreach (var kiosk in KioskConf)
                {
                    var serviceIds = kiosk.ServiceIds;
                    CountServiceId = KioskConf.SelectMany(k => k.ServiceIds).Count();

                    if (CountServiceId > 1)
                    {
                        serv.AddRange(Services.Where(service => serviceIds.Contains(service.Id)));
                    }
                    else
                    {
                        FormKios.ServiceId = Services.FirstOrDefault(service => serviceIds.Contains(service.Id))?.Id;
                        var serviceId = FormKios.ServiceId;

                        var matchingPhysicians = Physician.Where(phy => phy.DoctorServiceIds.Contains((int)serviceId));

                        if (matchingPhysicians.Any())
                        {
                            showPhysician = true;
                            Phys.AddRange(matchingPhysicians.Where(phy => phy.IsDoctor == true));
                        }
                    }
                }
            }
            else
            {
                showForm = false;
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
                {
                    // Mendapatkan ID dari hasil CreateKioskRequest
                    var checkId = await Mediator.Send(new CreateKioskRequest(FormKios));

                    // Mendapatkan antrian kiosk
                    var kioskQueues = await Mediator.Send(new GetKioskQueueQuery());
                    var todayQueues = kioskQueues
                        .Where(x => x.ServiceId == checkId.ServiceId && x.CreatedDate?.Date == DateTime.Now.Date)
                        .ToList();

                    // Menentukan nomor antrian
                    FormQueue.NoQueue = todayQueues.Count == 0
                        ? 1
                        : todayQueues.Max(x => x.NoQueue) + 1;

                    // Mengisi informasi antrian
                    FormQueue.KioskId = checkId.Id;
                    FormQueue.ServiceId = checkId.ServiceId;

                    // Membuat KioskQueue baru
                    await Mediator.Send(new CreateKioskQueueRequest(FormQueue));
                    showQueue = true;
                }
                else
                {
                    await Mediator.Send(new UpdateKioskRequest(FormKios));
                }

                FormKios = new();
                await LoadData();
            }
            catch { }
        }

        #endregion Methode Save And Update
    }
}
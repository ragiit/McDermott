using McDermott.Application.Dtos.Queue;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Linq;
using static McDermott.Application.Features.Commands.Queue.KioskConfigCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class KioskPage
    {
        #region Relation Data

        public List<KioskDto> Kiosks = new();
        public List<KioskConfigDto> KioskConf = new();
        public List<ServiceDto> Services = new();
        public List<ServiceDto> serv = new List<ServiceDto>();
        public ServiceDto servs = new();
        public List<UserDto> Patients = new();
        public List<UserDto> Physician = new();
        public List<DoctorScheduleDto> Physicians = [];
        public List<DoctorScheduleDto> DoctorSchedules = new();

        #endregion Relation Data

        #region setings Grid

        //private BaseAuthorizationLayout AuthorizationLayout = new();
        //private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;

        private bool showForm { get; set; } = false;
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
        private int ServicedId = 0;
        private List<string> Names { get; set; } = new List<string>();
        private string Bpjs { get; set; } = string.Empty;
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
            }
        }

        #endregion Data Static And Variable Additional

        #region Async Data And Auth

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
                FormKios.BPJS = Patients.Select(x => x.NoBpjsKs).FirstOrDefault();
                if (FormKios.BPJS != null)
                {
                    FormKios.StageBpjs = true;
                }

                foreach (var item in KioskConf)
                {
                    var n = item.ServiceIds;
                    CountServiceId = KioskConf.SelectMany(item => item.ServiceIds).Count();

                    if (item.ServiceIds.Count > 1)
                    {
                        serv.AddRange(Services.Where(x => item.ServiceIds.Contains(x.Id)));
                    }
                    else
                    {
                        FormKios.ServiceId = Services.FirstOrDefault(x => item.ServiceIds.Contains(x.Id))?.Id;
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
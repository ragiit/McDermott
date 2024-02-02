using DevExpress.Data.XtraReports.Native;
using McDermott.Domain.Entities;
using MediatR;
using System.Globalization;
using static McDermott.Application.Features.Commands.CompanyCommand;
using static McDermott.Application.Features.Commands.DoctorScheduleCommand;
using static McDermott.Application.Features.Commands.GeneralConsultanServiceCommand;
using static McDermott.Application.Features.Commands.InsuranceCommand;
using static McDermott.Application.Features.Commands.ServiceCommand;
using static McDermott.Application.Features.Commands.UserCommand;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class GeneralConsultanServicePage
    {
        #region Relation Data

        private List<GeneralConsultanServiceDto> GeneralConsultanServices = new();
        private List<UserDto> IsPatient = new();

        private List<UserDto> IsPratition = new();
        private List<InsuranceDto> Insurances = new();
        private List<ServiceDto> Services = new();
        private List<DoctorScheduleDto> DoctorSchedules = new();

        #endregion Relation Data

        #region Data Statis

        private List<string> RegisType = new List<string>
        {
            "Rawat Jalan",
            "IGD"
        };

        private List<string> Method = new List<string>
        {
            "MCU",
            "Oil & Gas"
        };

        #endregion Data Statis

        #region Grid Setting

        private GeneralConsultanServiceDto FormRegis = new();
        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool showForm { get; set; } = false;
        private string textPopUp = "";
        private string DisplayFormat { get; } = string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? "HH:mm" : "h:mm tt";
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private GroupMenuDto UserAccessCRUID = new();

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
            await LoadData();
        }

        private async Task SelectData()
        {
            var user = await Mediator.Send(new GetUserQuery());
            //patient
            IsPatient = [.. user.Where(x => x.IsPatient == true).ToList()];

            //IsDocter
            IsPratition = [.. user.Where(x => x.IsDoctor == true).ToList()];

            //Insurance

            Insurances = await Mediator.Send(new GetInsuranceQuery());

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
using DevExpress.Data.XtraReports.Native;
using McDermott.Domain.Entities;
using MediatR;
using static McDermott.Application.Features.Commands.DoctorScheduleCommand;
using static McDermott.Application.Features.Commands.GeneralConsultanServiceCommand;
using static McDermott.Application.Features.Commands.InsuranceCommand;
using static McDermott.Application.Features.Commands.ServiceCommand;
using static McDermott.Application.Features.Commands.UserCommand;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class GeneralConsultanServicePage
    {
        private List<GeneralConsultanServiceDto> GeneralConsultanServices = new();
        private List<string> IsPatient = new();
        private List<string> IsPratition = new();
        private List<string> Insurances = new();
        private List<string> Services = new();

        private GeneralConsultanServiceDto FormRegis = new();
        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool PopupVisible { get; set; } = false;
        private string textPopUp = "";
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private GroupMenuDto UserAccessCRUID = new();
        private List<string> RegisType = new List<string>
        {
            "Rawat Jalan",
            "IGD"
        };
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
            IsPatient = [.. user.Where(x => x.IsPatient == true).Select(x => x.Name).ToList()];

            //IsDocter
            IsPratition = [.. user.Where(x => x.IsDoctor == true).Select(x => x.Name).ToList()];

            //Insurance
            var Insurance = await Mediator.Send(new GetInsuranceQuery());
            Insurances = [.. Insurance.Select(x => x.Name).ToList()];

            //Medical Type
            var service = await Mediator.Send(new GetServiceQuery());
            Services = [.. service.Select(x => x.Name).ToList()];

            //Schendule
            var schendule = await Mediator.Send(new GetDoctorScheduleQuery());

            //Register Type

        }
        private async Task LoadData()
        {
            PopupVisible = false;
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
            PopupVisible = true;
            textPopUp = "Add Data Registration";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {

            PopupVisible = true;
            textPopUp = "Edit Form Template Email";
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
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

        private async Task OnDelete()
        {
            try
            {
                await LoadData();
            }
            catch { }
        }


    }
}


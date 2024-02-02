using DevExpress.Data.XtraReports.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Globalization;
using static McDermott.Application.Features.Commands.BuildingCommand;
using static McDermott.Application.Features.Commands.DoctorScheduleCommand;
using static McDermott.Application.Features.Commands.ServiceCommand;
using static McDermott.Application.Features.Commands.UserCommand;
using static McDermott.Web.Components.Pages.Medical.DoctorScheduledPage;

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
        public List<DoctorScheduleDetailDto> DeletedDoctorScheduleDetails = [];

        private List<ServiceDto> Services = [];
        private IEnumerable<UserDto> Users = [];
        private IEnumerable<UserDto> SelectedPhysicions = [];
        private List<DoctorScheduleDto> DoctorSchedules = [];
        private List<DoctorScheduleDto> Schedules = new List<DoctorScheduleDto>();
        private IEnumerable<DoctorScheduleDto> SelectedSchedules = [];
        private DoctorScheduleDto DoctorSchedule = new();

        private List<DoctorScheduleDetailDto> DoctorScheduleDetails = [];
        public List<DoctorScheduleDetailDto> DeletedDoctorSchedules = [];

        private DateTime StartDate = DateTime.Now;
        private DateTime EndDate = DateTime.Now;

        private int newId = 0;
        private string PhysicionName { get; set; }

        private int Value
        {
            get => newId;
            set
            {
                Names.Clear();
                int newId = value; InvokeAsync(StateHasChanged);
                this.newId = value;

                var item = Schedules.FirstOrDefault(x => x.Id == newId);

                PhysicionName = item.Physicions;

                var n = item.Physicions.Split(",");

                foreach (var item1 in n)
                {
                    if (Names.Contains(item1))
                        continue;

                    Names.Add(item1);
                }

                SelectedNames = Names.Distinct();
            }
        }

        #region Default Grid

        private string DisplayFormat { get; } = string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? "HH:mm" : "h:mm tt";

        private bool PanelVisible { get; set; } = true;
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

            Services = await Mediator.Send(new GetServiceQuery());

            await LoadData();
        }

        private List<string> Names { get; set; } = new();
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
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void ComboBoxValueChanged(string newValue)
        {
            // Lakukan sesuatu ketika nilai combobox berubah
            Console.WriteLine("Nilai combobox berubah menjadi: " + newValue);
        }

        private void SelectedItemsChanged(IEnumerable<DoctorScheduleDto> e)
        {
            Names.Clear();
            foreach (var item in e)
            {
                var n = item.Physicions.Split(",");

                foreach (var item1 in n)
                {
                    if (Names.Contains(item1))
                        continue;

                    Names.Add(item1);
                }
            }

            SelectedNames = Names.Distinct();
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            var users = await Mediator.Send(new GetUserQuery());
            Users = users.Where(x => x.IsDoctor == true && x.IsPhysicion == true).AsEnumerable();

            DoctorSchedules.Clear();

            var doctorSchedules = await Mediator.Send(new GetDoctorScheduleQuery());
            doctorSchedules.ForEach(x => x.Physicions = string.Join(", ", users.Where(z => x.PhysicionIds != null && x.PhysicionIds.Contains(z.Id)).Select(z => z.Name).ToList()));

            DoctorSchedules = doctorSchedules;

            Schedules = doctorSchedules;

            SelectedDataItems = new ObservableRangeCollection<object>();
            Buildings = await Mediator.Send(new GetBuildingQuery());

            tt = Schedules[0];

            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteBuildingRequest(SelectedDataItems[0].Adapt<BuildingDto>().Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<BuildingDto>>();
                    await Mediator.Send(new DeleteListBuildingRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private async Task OnDeleteDoctorScheduleDetail(GridDataItemDeletingEventArgs e)
        {
            var aaa = SelectedDoctorScheduleDetailDataItems.Adapt<List<DoctorScheduleDetailDto>>();
            //DoctorScheduleDetails.RemoveAll(x => aaa.Select(z => z.LocationId).Contains(x.LocationId));
            SelectedDoctorScheduleDetailDataItems = new ObservableRangeCollection<object>();
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            Console.WriteLine("Megalodon Ganjar");
        }

        private async Task OnSaveDoctorScheduleDetail(GridEditModelSavingEventArgs e)
        {
            var DoctorScheduleDetail = (DoctorScheduleDetailDto)e.EditModel;

            DoctorScheduleDetailDto updateBuilding = new();

            if (IsAddMenu)
            {
                if (DoctorScheduleDetails.Where(x => x.Name == DoctorScheduleDetail.Name).Any())
                    return;

                updateBuilding = DoctorScheduleDetails.FirstOrDefault(x => x.ServiceId == DoctorScheduleDetail.ServiceId)!;
                DoctorScheduleDetail.Service = Services.FirstOrDefault(x => x.Id == DoctorScheduleDetail.ServiceId);
                DoctorScheduleDetails.Add(DoctorScheduleDetail);
            }
            else
            {
                if (!DoctorScheduleDetails.Where(x => x.Name == DoctorScheduleDetail.Name).Any())
                    return;

                var q = SelectedDoctorScheduleDetailDataItems[0].Adapt<DoctorScheduleDetailDto>();

                updateBuilding = DoctorScheduleDetails.FirstOrDefault(x => x.Name == DoctorScheduleDetail.Name)!;
                DoctorScheduleDetail.Service = Services.FirstOrDefault(x => x.Id == DoctorScheduleDetail.ServiceId);
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

        private async Task NewItem_Click()
        {
            ShowForm = true;
            DoctorSchedules = [];
            DoctorSchedule = new();
        }

        private async Task GenerateScheduleDoctor_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            try
            {
                DoctorSchedule = SelectedDataItems[0].Adapt<DoctorScheduleDto>();
                ShowForm = true;

                if (DoctorSchedule != null)
                {
                    DeletedDoctorSchedules = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(DoctorSchedule.Id));
                    DoctorScheduleDetails = await Mediator.Send(new GetDoctorScheduleDetailByScheduleIdQuery(DoctorSchedule.Id));
                    var a = Users.Where(x => DoctorSchedule.PhysicionIds != null && DoctorSchedule.PhysicionIds.Contains(x.Id)).ToList();
                    SelectedPhysicions = a;
                }
            }
            catch { }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void CancelItemDoctorScheduleDetailGrid_Click()
        {
            DoctorScheduleDetails = [];
            DoctorSchedule = new();
            SelectedDoctorScheduleDetailDataItems = new ObservableRangeCollection<object>();
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
                x.DoctorScheduleId = DoctorSchedule.Id == 0 ? result.Id : DoctorSchedule.Id;
                x.Service = null;
            });

            await Mediator.Send(new CreateDoctorScheduleDetailRequest(DoctorScheduleDetails));

            ShowForm = false;

            await LoadData();
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
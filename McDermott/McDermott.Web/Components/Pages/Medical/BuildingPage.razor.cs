using DevExpress.Data.XtraReports.Native;
using McDermott.Domain.Entities;
using Microsoft.JSInterop;
using static McDermott.Application.Features.Commands.BuildingCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;
using static McDermott.Application.Features.Commands.HealthCenterCommand;
using static McDermott.Application.Features.Commands.LocationCommand;
using static McDermott.Application.Features.Commands.GroupCommand;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class BuildingPage
    {
        private List<HealthCenterDto> HealthCenters = [];
        public List<BuildingDto> Buildings = [];
        public List<LocationDto> Locations = [];
        public BuildingDto Building = new();
        public List<BuildingLocationDto> BuildingLocations = [];
        public List<BuildingLocationDto> DeletedBuildingLocations = [];

        #region Default Grid
        private GroupMenuDto UserAccessCRUID = new();
        private bool PanelVisible { get; set; } = true;
        private bool IsAddMenu { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        public IGrid Grid { get; set; }
        public IGrid GridBuildingLocation { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowBuildingLocationVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private IReadOnlyList<object> SelectedBuildingLocationDataItems { get; set; } = new ObservableRangeCollection<object>();
        private bool IsAccess = false;

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
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            Buildings = await Mediator.Send(new GetBuildingQuery());
            PanelVisible = false;
        }
        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
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

        private async Task OnDeleteBuildingLocation(GridDataItemDeletingEventArgs e)
        {
            var aaa = SelectedBuildingLocationDataItems.Adapt<List<BuildingLocationDto>>();
            BuildingLocations.RemoveAll(x => aaa.Select(z => z.LocationId).Contains(x.LocationId));
            SelectedBuildingLocationDataItems = new ObservableRangeCollection<object>();
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (BuildingDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Name))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateBuildingRequest(editModel));
            else
                await Mediator.Send(new UpdateBuildingRequest(editModel));

            await LoadData();
        }

        private async Task OnSaveBuildingLocation(GridEditModelSavingEventArgs e)
        {
            var buildingLocation = (BuildingLocationDto)e.EditModel;

            if (BuildingLocations.Where(x => x.LocationId == buildingLocation.LocationId).Any())
                return;

            BuildingLocationDto updateBuilding = new();

            if (IsAddMenu)
            {
                updateBuilding = BuildingLocations.FirstOrDefault(x => x.LocationId == buildingLocation.LocationId)!;
                buildingLocation.Location = Locations.FirstOrDefault(x => x.Id == buildingLocation.LocationId);
                BuildingLocations.Add(buildingLocation);
            }
            else
            {
                var q = SelectedBuildingLocationDataItems[0].Adapt<BuildingLocationDto>();

                updateBuilding = BuildingLocations.FirstOrDefault(x => x.LocationId == buildingLocation.LocationId)!;
                buildingLocation.Location = Locations.FirstOrDefault(x => x.Id == buildingLocation.LocationId);
                var index = BuildingLocations.IndexOf(updateBuilding!);
                BuildingLocations[index] = buildingLocation;
            }

            SelectedBuildingLocationDataItems = new ObservableRangeCollection<object>();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            EditItemsEnabled = true;
        }

        private void GridBuildingLocation_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowBuildingLocationVisibleIndex = args.VisibleIndex;
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
            BuildingLocations = [];
            Building = new();
        }

        private async Task EditItem_Click()
        {
            try
            {
                Building = SelectedDataItems[0].Adapt<BuildingDto>();
                ShowForm = true;

                if (Building != null)
                {
                    DeletedBuildingLocations = await Mediator.Send(new GetBuildingLocationByBuildingIdRequest(Building.Id));
                    BuildingLocations = await Mediator.Send(new GetBuildingLocationByBuildingIdRequest(Building.Id));
                }
            }
            catch { }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void CancelItemBuildingLocationGrid_Click()
        {
            BuildingLocations = new();
            Building = new();
            SelectedBuildingLocationDataItems = new ObservableRangeCollection<object>();
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


        private async Task NewItemBuildingLocation_Click()
        {
            IsAddMenu = true;
            await GridBuildingLocation.StartEditNewRowAsync();
        }

        private async Task EditItemBuildingLocation_Click()
        {
            IsAddMenu = false;
            await GridBuildingLocation.StartEditRowAsync(FocusedRowBuildingLocationVisibleIndex);
        }

        private void DeleteItemBuildingLocation_Click()
        {
            GridBuildingLocation.ShowRowDeleteConfirmation(FocusedRowBuildingLocationVisibleIndex);
        }

        private async Task SaveItemBuildingLocationGrid_Click()
        {
            var a = BuildingLocations;

            if (a is null) return;

            if (Building.Id == 0)
            {
                var result = await Mediator.Send(new CreateBuildingRequest(Building));

                await Mediator.Send(new DeleteBuildingLocationByIdRequest(DeletedBuildingLocations.Select(x => x.Id).ToList()));

                a.ForEach(x =>
                {
                    x.Id = 0;
                    x.BuildingId = result.Id;
                    x.Location = null;
                });

                await Mediator.Send(new CreateBuildingLocationRequest(a));
            }
            else
            {
                var result = await Mediator.Send(new UpdateBuildingRequest(Building));

                await Mediator.Send(new DeleteBuildingLocationByIdRequest(DeletedBuildingLocations.Select(x => x.Id).ToList()));

                a.ForEach(x =>
                {
                    x.Id = 0;
                    x.BuildingId = Building.Id;
                    x.Location = null;
                });

                await Mediator.Send(new CreateBuildingLocationRequest(a));
            }

            ShowForm = false;

            await LoadData();
        }

        private void ColumnChooserButtonBuildingLocation_Click()
        {
            GridBuildingLocation.ShowColumnChooser();
        }

        private async Task ExportXlsxItemBuildingLocation_Click()
        {
            await GridBuildingLocation.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportXlsItemBuildingLocation_Click()
        {
            await GridBuildingLocation.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportCsvItemBuildingLocation_Click()
        {
            await GridBuildingLocation.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        #endregion Default Grid
    }
}
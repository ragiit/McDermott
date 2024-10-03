using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using McDermott.Persistence.Migrations;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace McDermott.Web.Components.Pages.Medical.Buildings
{
    public partial class CreateUpdateBuildingPage
    {
        private string baseUrl = "medical/building-and-locations/";

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
            //}
        }

        private async Task GetUserInfo()
        {
            try
            {
                var user = await UserInfoService.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        #region Binding

        private bool PanelVisible { get; set; } = true;

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private BuildingDto Building { get; set; } = new();

        #region Grid Locations

        private List<BuildingLocationDto> BuildingLocations { get; set; } = [];
        public IGrid GridBuildingLocations { get; set; }
        private IReadOnlyList<object> SelectedDataItemsBuildingLocations { get; set; } = [];
        private int FocusedRowVisibleIndexBuildingLocations { get; set; }

        private void GridBuldingLocation_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexBuildingLocations = args.VisibleIndex;
        }

        private async Task OnSaveBuildingLocation(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (BuildingLocationDto)e.EditModel;

                editModel.BuildingId = Building.Id;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateBuildingLocationRequest(editModel));
                else
                    await Mediator.Send(new UpdateBuildingLocationRequest(editModel));

                NavigationManager.NavigateTo($"{baseUrl}{EnumPageMode.Update.GetDisplayName()}?Id={Building.Id}", true);

                await LoadDataBuildingLocation(activePageIndex, pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task NewItemBuildingLocation_Click()
        {
            await GridBuildingLocations.StartEditNewRowAsync();
        }

        private async Task EditItemBuildingLocation_Click()
        {
            try
            {
                PanelVisible = true;
                await GridBuildingLocations.StartEditNewRowAsync();
                var a = (GridBuildingLocations.GetDataItem(FocusedRowVisibleIndexBuildingLocations) as BuildingLocationDto ?? new());
                Locations = (await Mediator.QueryGetHelper<Locations, LocationDto>(predicate: x => x.Id == a.LocationId)).Item1;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnDeleteBuildingLocation(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemsBuildingLocations is null)
                {
                    await Mediator.Send(new DeleteBuildingLocationRequest(((BuildingLocationDto)e.DataItem).Id));
                }
                else
                {
                    var selectedMenus = SelectedDataItemsBuildingLocations.Adapt<List<BuildingLocationDto>>();
                    await Mediator.Send(new DeleteBuildingLocationRequest(ids: selectedMenus.Select(x => x.Id).ToList()));
                }
                await LoadDataBuildingLocation(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItemBuildingLocation_Click()
        {
            GridBuildingLocations.ShowRowDeleteConfirmation(FocusedRowVisibleIndexBuildingLocations);
        }

        #region ComboboxLocation

        private DxComboBox<LocationDto, long> refLocationComboBox { get; set; }
        private int LocationComboBoxIndex { get; set; } = 0;
        private int totalCountLocation = 0;

        private async Task OnSearchLocation()
        {
            await LoadDataLocation();
        }

        private async Task OnSearchLocationIndexIncrement()
        {
            if (LocationComboBoxIndex < (totalCountLocation - 1))
            {
                LocationComboBoxIndex++;
                await LoadDataLocation(LocationComboBoxIndex, 10);
            }
        }

        private async Task OnSearchLocationIndexDecrement()
        {
            if (LocationComboBoxIndex > 0)
            {
                LocationComboBoxIndex--;
                await LoadDataLocation(LocationComboBoxIndex, 10);
            }
        }

        private async Task OnInputLocationChanged(string e)
        {
            LocationComboBoxIndex = 0;
            await LoadDataLocation();
        }

        private async Task LoadDataLocation(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.QueryGetHelper<Locations, LocationDto>(pageIndex, pageSize, refLocationComboBox?.Text ?? "");
                Locations = result.Item1;
                totalCountLocation = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxLocation

        #endregion Grid Locations

        private List<LocationDto> Locations { get; set; } = [];

        #endregion Binding

        private async Task HandleValidSubmit()
        {
            try
            {
                PanelVisible = true;

                var existingName = await Mediator.Send(new ValidateBuildingQuery(x => x.Name == Building.Name && x.Id != Building.Id));

                if (existingName)
                {
                    ToastService.ShowInfo("Building name already exist");
                    return;
                }

                if (Building.Id == 0)
                {
                    Building = await Mediator.Send(new CreateBuildingRequest(Building));
                }
                else
                {
                    Building = await Mediator.Send(new UpdateBuildingRequest(Building));
                }
                NavigationManager.NavigateTo($"{baseUrl}{EnumPageMode.Update.GetDisplayName()}?Id={Building.Id}", true);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataLocation();
            PanelVisible = false;
        }

        private async Task LoadData()
        {
            //var result = await MyQuery.GetGroups(HttpClientFactory, 0, 1, Id.HasValue ? Id.ToString() : "");

            //Id = McDermott.Extentions.SecureHelper.DecryptIdFromBase64(Ids);
            var result = await Mediator.QueryGetHelper<Building, BuildingDto>(predicate: x => x.Id == Id);

            Building = new();
            BuildingLocations.Clear();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo(baseUrl);
                    return;
                }

                Building = result.Item1.FirstOrDefault() ?? new();
                await LoadDataBuildingLocation();

                HealthCenters = (await Mediator.QueryGetHelper<HealthCenter, HealthCenterDto>(predicate: x => x.Id == Building.HealthCenterId)).Item1;
            }
        }

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadDataBuildingLocation(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadDataBuildingLocation(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadDataBuildingLocation(newPageIndex, pageSize);
        }

        #endregion Searching

        private async Task LoadDataBuildingLocation(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var a = await Mediator.QueryGetHelper<BuildingLocation, BuildingLocationDto>(pageIndex, pageSize, searchTerm, x => x.BuildingId == Building.Id);
                BuildingLocations = a.Item1;
                totalCount = a.pageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void CancelItemBuilding_Click()
        {
            NavigationManager.NavigateTo(baseUrl);
        }

        #region ComboboxHealthCenter

        private List<HealthCenterDto> HealthCenters { get; set; } = [];
        private DxComboBox<HealthCenterDto, long?> refHealthCenterComboBox { get; set; }
        private int HealthCenterComboBoxIndex { get; set; } = 0;
        private int totalCountHealthCenter = 0;

        private async Task OnSearchHealthCenter()
        {
            await LoadDataHealthCenter();
        }

        private async Task OnSearchHealthCenterIndexIncrement()
        {
            if (HealthCenterComboBoxIndex < (totalCountHealthCenter - 1))
            {
                HealthCenterComboBoxIndex++;
                await LoadDataHealthCenter(HealthCenterComboBoxIndex, 10);
            }
        }

        private async Task OnSearchHealthCenterIndexDecrement()
        {
            if (HealthCenterComboBoxIndex > 0)
            {
                HealthCenterComboBoxIndex--;
                await LoadDataHealthCenter(HealthCenterComboBoxIndex, 10);
            }
        }

        private async Task OnInputHealthCenterChanged(string e)
        {
            HealthCenterComboBoxIndex = 0;
            await LoadDataHealthCenter();
        }

        private async Task LoadDataHealthCenter(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetHealthCenterQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refHealthCenterComboBox?.Text ?? ""));
                HealthCenters = result.Item1;
                totalCountHealthCenter = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxHealthCenter

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                return;
            }
        }
    }
}
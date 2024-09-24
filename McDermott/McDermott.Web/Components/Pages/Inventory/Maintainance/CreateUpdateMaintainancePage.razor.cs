using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Web.Components.Pages.Inventory.Maintainance
{
    public partial class CreateUpdateMaintainancePage
    {
        #region Relation data
        private List<MaintainanceDto> getMaintainance = [];
        private List<UserDto> getResponsibleBy = [];
        private List<UserDto> getRequestBy = [];
        private List<ProductDto> getEquipment = [];
        private List<LocationDto> getLocation = [];
        private List<TransactionStockDto> TransactionStocks = [];
        private MaintainanceDto postMaintainance = new();
        private MaintainanceDto getMaintainanceById = new();
        private TransactionStockDto postTransactionStock = new();
        #endregion

        #region variable Static
        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private IGrid Grid {  get; set; }
        private Timer _timer;
        private bool PanelVisible { get; set; } = false;
        private bool FormValidationState { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }

        #endregion

        #region Boolean Data
        private void unCheckIN(bool newValue)
        {
            postMaintainance.isInternal = true;
            if (newValue)
                postMaintainance.isExternal = false;
            else
                postMaintainance.isInternal = false;
        }

        private void unCheckEX(bool newValue)
        {
            postMaintainance.isExternal = true;
            if (newValue)
                postMaintainance.isInternal = false;
            else
                postMaintainance.isExternal = false;
        }

        private void unCheckCR(bool newValue)
        {
            postMaintainance.isCorrective = true;
            if (newValue)
                postMaintainance.isPreventive = false;
            else
                postMaintainance.isCorrective = false;
        }

        private void unCheckPR(bool newValue)
        {
            postMaintainance.isPreventive = true;
            if (newValue)
                postMaintainance.isCorrective = false;
            else
                postMaintainance.isPreventive = false;
        }

        private void unCheckRE(bool newValue)
        {
            postMaintainance.Recurrent = true;
        }

        private List<string> Batch = [];

        private async Task selectByProduct(ProductDto value)
        {
            var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == value.Id && s.LocationId == postMaintainance.LocationId));
            Batch = stockProducts?.Select(x => x.Batch)?.ToList() ?? [];
            Batch = Batch.Distinct().ToList();
        }

        private async Task selectByLocation(LocationDto value)
        {
            postMaintainance.LocationId = value.Id;
            postMaintainance.EquipmentId = null;
            postMaintainance.SerialNumber = null;
        }

        private List<string> RepeatWork = new List<string>()
        {
            "Days",
            "Weeks",
            "Months",
            "Years"
        };
        #endregion

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
                    StateHasChanged();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                        StateHasChanged();
                    }
                }
                catch { }

                await LoadData();
                StateHasChanged();
            }
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

        #region Load data
        private async Task LoadData()
        {
            var result = await Mediator.Send(new GetMaintainanceQuery(x=>x.Id == Id, pageSize: 0, pageIndex: 1));
            postMaintainance = new();
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result.Item1.Count == 0 || !Id.HasValue)
                {
                    NavigationManager.NavigateTo("inventory/maintainance");
                    return;
                }

                postMaintainance = result.Item1.FirstOrDefault() ?? new();
               

            }
        }
        #endregion

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            //await LoadLabTestDetails(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            //await LoadLabTestDetails(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            //await LoadLabTestDetails(newPageIndex, pageSize);
        }

        #endregion Searching

        #region Load ComboBox
        #region ComboBox Product
        private DxComboBox<ProductDto, long?> refProductsComboBox { get; set; }
        private int ProductsComboBoxIndex { get; set; } = 0;
        private int totalCountProducts = 0;

        private async Task OnSearchProducts()
        {
            await LoadDataProducts(0, 10);
        }

        private async Task OnSearchProductsIndexIncrement()
        {
            if (ProductsComboBoxIndex < (totalCountProducts - 1))
            {
                ProductsComboBoxIndex++;
                await LoadDataProducts(ProductsComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProductsIndexDecrement()
        {
            if (ProductsComboBoxIndex > 0)
            {
                ProductsComboBoxIndex--;
                await LoadDataProducts(ProductsComboBoxIndex, 10);
            }
        }

        private async Task OnInputProductsChanged(string e)
        {
            ProductsComboBoxIndex = 0;
            await LoadDataProducts(0, 10);
        }

        private async Task LoadDataProducts(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProductQuery(searchTerm: refProductsComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getEquipment = result.Item1.Where(x=>x.HospitalType == "Medical Equipment").ToList();
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion
        #region Combo Box Request By
        private DxComboBox<UserDto, long?> refRequestByComboBox { get; set; }
        private int RequestByComboBoxIndex { get; set; } = 0;
        private int totalCountRequestBy = 0;

        private async Task OnSearchRequestBy()
        {
            await LoadDataRequestBy(0, 10);
        }

        private async Task OnSearchRequestByIndexIncrement()
        {
            if (RequestByComboBoxIndex < (totalCountRequestBy - 1))
            {
                RequestByComboBoxIndex++;
                await LoadDataRequestBy(RequestByComboBoxIndex, 10);
            }
        }

        private async Task OnSearchRequestByIndexDecrement()
        {
            if (RequestByComboBoxIndex > 0)
            {
                RequestByComboBoxIndex--;
                await LoadDataRequestBy(RequestByComboBoxIndex, 10);
            }
        }

        private async Task OnInputRequestByChanged(string e)
        {
            RequestByComboBoxIndex = 0;
            await LoadDataRequestBy(0, 10);
        }

        private async Task LoadDataRequestBy(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetUserQuery2(searchTerm: refRequestByComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getRequestBy = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion
        #region Combo Box Location
        private DxComboBox<LocationDto, long?> refLocationComboBox { get; set; }
        private int LocationComboBoxIndex { get; set; } = 0;
        private int totalCountLocation = 0;

        private async Task OnSearchLocation()
        {
            await LoadDataLocation(0, 10);
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
            await LoadDataLocation(0, 10);
        }

        private async Task LoadDataLocation(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetLocationQuery(searchTerm: refLocationComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getLocation = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion
        #region Combo Box Responsible
        private DxComboBox<UserDto, long?> refResponsibleByComboBox { get; set; }
        private int ResponsibleByComboBoxIndex { get; set; } = 0;
        private int totalCountResponsibleBy = 0;

        private async Task OnSearchResponsibleBy()
        {
            await LoadDataResponsibleBy(0, 10);
        }

        private async Task OnSearchResponsibleByIndexIncrement()
        {
            if (ResponsibleByComboBoxIndex < (totalCountResponsibleBy - 1))
            {
                ResponsibleByComboBoxIndex++;
                await LoadDataResponsibleBy(ResponsibleByComboBoxIndex, 10);
            }
        }

        private async Task OnSearchResponsibleByIndexDecrement()
        {
            if (ResponsibleByComboBoxIndex > 0)
            {
                ResponsibleByComboBoxIndex--;
                await LoadDataResponsibleBy(ResponsibleByComboBoxIndex, 10);
            }
        }

        private async Task OnInputResponsibleByChanged(string e)
        {
            ResponsibleByComboBoxIndex = 0;
            await LoadDataResponsibleBy(0, 10);
        }

        private async Task LoadDataResponsibleBy(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetUserQuery2(searchTerm: refResponsibleByComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            getResponsibleBy = result.Item1.Where(x => x.IsEmployee == true).ToList();
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion


        #endregion

        #region function step
        private async Task InProgress_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintainance.Status = EnumStatusMaintainance.InProgress;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/maintainance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintainanceById.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Repaired_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintainance.Status = EnumStatusMaintainance.Repaired;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/maintainance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintainanceById.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Scrap_Click()
        {
            try
            {
                PanelVisible = true;
                TransactionStocks = await Mediator.Send(new GetTransactionStockQuery());
                var result = await Mediator.Send(new GetMaintainanceQuery(searchTerm: searchTerm, pageSize: 0, pageIndex: 1));
                getMaintainance = result.Item1;
                var cekReference = TransactionStocks.Where(x => x.SourceTable == nameof(Maintainance))
                           .OrderByDescending(x => x.SourcTableId).Select(z => z.Reference).FirstOrDefault();
                int NextReferenceNumber = 1;
                if (cekReference != null)
                {
                    int.TryParse(cekReference?.Substring("MNT#".Length), out NextReferenceNumber);
                    NextReferenceNumber++;
                }
                string referenceNumber = $"MNT#{NextReferenceNumber:D3}";

                foreach (var items in getMaintainance)
                {
                    var cekUom = getEquipment.Where(x => x.Id == postMaintainance.EquipmentId).Select(x => x.UomId).FirstOrDefault();

                    postTransactionStock.SourceTable = nameof(Maintainance);
                    postTransactionStock.SourcTableId = postMaintainance.Id;
                    postTransactionStock.ProductId = items.EquipmentId;
                    postTransactionStock.LocationId = items.LocationId;
                    postTransactionStock.Batch = items.SerialNumber;
                    postTransactionStock.ExpiredDate = null;
                    postTransactionStock.Reference = referenceNumber;
                    postTransactionStock.Quantity = -1;
                    postTransactionStock.UomId = cekUom;
                    postTransactionStock.Validate = true;

                    await Mediator.Send(new CreateTransactionStockRequest(postTransactionStock));
                }

                //update Maintainance
                postMaintainance.Status = EnumStatusMaintainance.Scrap;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/maintainance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintainanceById.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Done_Click()
        {
            try
            {
                PanelVisible = true;
                postMaintainance.Status = EnumStatusMaintainance.Done;
                getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/maintainance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintainanceById.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Cancel_Click()
        { }

        private async Task onDiscard()
        {
            NavigationManager.NavigateTo($"inventory/maintainance/");
            StateHasChanged();
        }
        #endregion

        #region save

        private async Task OnSave()
        {
            try
            {
                PanelVisible = true;
                if (postMaintainance is null)
                {
                    return;
                }
                if (postMaintainance.Id == 0)
                {
                    string prefix = "MNT-";
                    string datePart = DateTime.Now.ToString("ddMMyy");

                    var lastSequence = getMaintainance.Where(x => x.Sequence is null || x.Sequence.Substring(7, 4) == DateTime.Now.ToString("MMyy")).OrderByDescending(x => x.Sequence).FirstOrDefault();

                    int nextSequence = 1;
                    if (lastSequence != null)
                    {
                        var lastNumberPart = lastSequence?.Sequence?.Substring(lastSequence.Sequence.Length - 3);
                        nextSequence = int.Parse(lastNumberPart) + 1;
                    }

                    postMaintainance.Sequence = $"{prefix}{datePart}-{nextSequence.ToString("D3")}";
                    postMaintainance.Status = EnumStatusMaintainance.Request;

                    getMaintainanceById = await Mediator.Send(new CreateMaintainanceRequest(postMaintainance));
                    ToastService.ShowSuccess("Save Data Success..");
                }
                else
                {
                    getMaintainanceById = await Mediator.Send(new UpdateMaintainanceRequest(postMaintainance));
                    ToastService.ShowSuccess("Update Data Success..");
                }

                PanelVisible = false;
                NavigationManager.NavigateTo($"inventory/maintainance/{EnumPageMode.Update.GetDisplayName()}?Id={getMaintainanceById.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion save


        #region Handler Vaidation
        private async Task HandleValidSubmit()
        {
            //IsLoading = true;
            FormValidationState = true;
            await OnSave();
            //IsLoading = false;
        }

        private async Task HandleInvalidSubmit()
        {
          
            FormValidationState = false;
        }

        #endregion
    }
}

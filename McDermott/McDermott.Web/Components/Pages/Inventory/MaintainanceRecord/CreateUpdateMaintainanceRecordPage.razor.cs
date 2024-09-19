using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceCommand;
namespace McDermott.Web.Components.Pages.Inventory.MaintainanceRecord
{
    public partial class CreateUpdateMaintainanceRecordPage
    {
        #region Relation Data
        private List<MaintainanceDto> GetMaintainances = [];
        private List<ProductDto> GetProducts = [];
        private MaintainanceRecordDto PostMaintainanceRecords = new();
        #endregion

        #region Variable Static
        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private bool PanelVisible { get; set; } = true;
        private Timer _timer;
        public IGrid Grid;
        private bool FormValidationState { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        #endregion

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

        #region load Data
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadDataMaintainance();
            await LoadDataProduct();           
            await GetUserInfo();
            PanelVisible = false;

            return;
                       
        }

        #endregion

        #region Load ComboBox
        #region ComboBox Maintainance
        private DxComboBox<MaintainanceDto, long?> refMaintainanceComboBox { get; set; }
        private int MaintainanceComboBoxIndex { get; set; } = 0;
        private int totalCountMaintainance = 0;

        private async Task OnSearchMaintainance()
        {
            await LoadDataMaintainance(0, 10);
        }

        private async Task OnSearchMaintainanceIndexIncrement()
        {
            if (MaintainanceComboBoxIndex < (totalCountMaintainance - 1))
            {
                MaintainanceComboBoxIndex++;
                await LoadDataMaintainance(MaintainanceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchMaintainanceIndexDecrement()
        {
            if (MaintainanceComboBoxIndex > 0)
            {
                MaintainanceComboBoxIndex--;
                await LoadDataMaintainance(MaintainanceComboBoxIndex, 10);
            }
        }

        private async Task OnInputMaintainanceChanged(string e)
        {
            MaintainanceComboBoxIndex = 0;
            await LoadDataMaintainance(0, 10);
        }

        private async Task LoadDataMaintainance(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetMaintainanceQuery(searchTerm: refMaintainanceComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetMaintainances = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion
        #region Combo Box Product
        private DxComboBox<ProductDto, long?> refProductComboBox { get; set; }
        private int ProductComboBoxIndex { get; set; } = 0;
        private int totalCountProduct = 0;

        private async Task OnSearchProduct()
        {
            await LoadDataProduct(0, 10);
        }

        private async Task OnSearchProductIndexIncrement()
        {
            if (ProductComboBoxIndex < (totalCountProduct - 1))
            {
                ProductComboBoxIndex++;
                await LoadDataProduct(ProductComboBoxIndex, 10);
            }
        }

        private async Task OnSearchProductIndexDecrement()
        {
            if (ProductComboBoxIndex > 0)
            {
                ProductComboBoxIndex--;
                await LoadDataProduct(ProductComboBoxIndex, 10);
            }
        }

        private async Task OnInputProductChanged(string e)
        {
            ProductComboBoxIndex = 0;
            await LoadDataProduct(0, 10);
        }

        private async Task LoadDataProduct(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetProductQuery(searchTerm: refProductComboBox?.Text, pageSize: pageSize, pageIndex: pageIndex));
            GetProducts = result.Item1;
            totalCount = result.pageCount;
            PanelVisible = false;
        }
        #endregion
        #endregion


        #region KeyPress
        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }
        #endregion

        #region Grid Configuration
        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Components.Web;

namespace McDermott.Web.Components.Pages.Inventory.ProductCategories
{
    public partial class CreateUpdateProductCategoryPage
    {
        #region relation data
        private List<ProductCategoryDto> GetProductCategory = [];
        private ProductCategoryDto PostProductCategory = new();
        private ProductCategoryDto tempProductCategory = new();
        #endregion

        #region Variabel Static
        [SupplyParameterFromQuery]
        private long? Id { get; set; }
        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();
        private IGrid Grid;
        private bool FormValidationState { get; set; } = false;
        private bool PanelVisible { get; set; } = false;

        private List<string> CostingMethod = new List<string>
        {
            "Standart Price",
            "First In Firs Out (FIFO)",
            "Average Cost (AVCO)"
        };

        private List<string> InventoryValiation = new List<string>
        {
            "Manual",
            "Automated"
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
                    }
                }
                catch { }
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

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadData(newPageIndex, pageSize);
        }

        #endregion Searching

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }
        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetProductCategoryQuery(searchTerm: searchTerm, pageIndex: pageIndex, pageSize: pageSize));
                GetProductCategory = result.Item1;
                totalCount = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        #endregion

        #region Handle Submit

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        private async Task HandleValidSubmit()
        {
            if (FormValidationState)
            {
                await OnSave();
            }
            else
            {
                FormValidationState = true;
            }
        }

        #endregion Handle Submit

        private void Discard_Click()
        {
            NavigationManager.NavigateTo($"inventory/product-categories");
            return;
        }
        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        private async Task OnSave()
        {
            try
            {
                bool exists = await Mediator.Send(new ValidateProductCategoryQuery(x => x.Id != PostProductCategory.Id && x.Name == PostProductCategory.Name && x.Code == PostProductCategory.Code));
                if (exists)
                {
                    ToastService.ShowWarning($"Category Product with name '{PostProductCategory.Name}' already exists.");
                    return;
                }
                if (PostProductCategory.Id == 0)
                {
                    tempProductCategory = await Mediator.Send(new CreateProductCategoryRequest(PostProductCategory));
                    ToastService.ShowSuccess("Add Data Success....");
                }
                else
                {
                    tempProductCategory = await Mediator.Send(new UpdateProductCategoryRequest(PostProductCategory));
                    ToastService.ShowSuccess("Update Data Success....");
                }
                NavigationManager.NavigateTo($"inventory/product-categories/{EnumPageMode.Update.GetDisplayName()}?Id={tempProductCategory.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
    }
}

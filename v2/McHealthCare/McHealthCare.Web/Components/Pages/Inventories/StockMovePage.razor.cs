namespace McHealthCare.Web.Components.Pages.Inventories
{
    public partial class StockMovePage
    {
        public IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private List<TransactionStockDto> TransactionStocks { get; set; } = [];

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    PanelVisible = true;
                    StateHasChanged();

                    await LoadData();
                    StateHasChanged();

                    Grid?.CollapseAllGroupRows();
                }
                catch { }
            }
        }

        private void Grid_CustomSummary(GridCustomGroupEventArgs e)
        {
            //if (e.s)
            //{
            //}
            var a = "adwad";
            var ad = "adwad";
            var add = "adwad";
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            TransactionStocks = [.. (await Mediator.Send(new GetTransactionStockQuery())).OrderByDescending(x => x.CreatedDate)];
            StateHasChanged();
            Grid?.CollapseAllGroupRows();
            StateHasChanged();
            PanelVisible = false;
        }
    }
}
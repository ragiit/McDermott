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

private async Task LoadData(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    SelectedDataItems = [];
    var countries = await Mediator.Send(new GetCountryQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
    Countries = countries.Item1;
    totalCount = countries.Item4;
    activePageIndex = pageIndex;
    PanelVisible = false;
}

try
{
    var editModel = (CountryDto)e.EditModel;

    bool validate = await Mediator.Send(new ValidateCountryQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.Code == editModel.Code));

    if (validate)
    {
        ToastService.ShowInfo($"Country with name '{editModel.Name}' and code '{editModel.Code}' is already exists");
        e.Cancel = true;
        return;
    }

    if (editModel.Id == 0)
        await Mediator.Send(new CreateCountryRequest(editModel));
    else
        await Mediator.Send(new UpdateCountryRequest(editModel));

    SelectedDataItems = [];
    await LoadData();
}
catch (Exception ex)
{
    ex.HandleException(ToastService);
}

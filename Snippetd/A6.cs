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
 private async Task LoadData(int pageIndex = 0, int pageSize = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetKioskConfigQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchTerm = searchTerm,
        });
        KioskConfigs = result.Item1;
        totalCount = result.PageCount;
        activePageIndex = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching




SearchTextChanged="OnSearchBoxChanged"
try
{
    var editModel = (SignaDto)e.EditModel;

    bool validate = await Mediator.Send(new ValidateSignaQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.Code == editModel.Code));

    if (validate)
    {
        ToastService.ShowInfo($"Signa with name '{editModel.Name}' and code '{editModel.Code}' is already exists");
        e.Cancel = true;
        return;
    }

    if (editModel.Id == 0)
        await Mediator.Send(new CreateSignaRequest(editModel));
    else
        await Mediator.Send(new UpdateSignaRequest(editModel));

    SelectedDataItems = [];
    await LoadData();
}
  catch (Exception ex)
  {
      ex.HandleException(ToastService);
  }
  finally { PanelVisible = false; }
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
     try
     { 
         PanelVisible = true;
         SelectedDataItems = [];
         var a = await Mediator.QueryGetHelper<GeneralConsultanCPPT, GeneralConsultanCPPTDto>(pageIndex, pageSize, searchTerm);
         var a = await Mediator.Send(new GetSignaQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
         Signas = a.Item1;
         totalCount = a.pageCount;
         activePageIndex = pageIndex; 
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastService);
     }
     finally { PanelVisible = false; }
 }

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
await LoadDataCounter();

 #region ComboboxCounter

 private DxComboBox<CounterDto, long?> refCounterComboBox { get; set; }
 private int CounterComboBoxIndex { get; set; } = 0;
 private int totalCountCounter = 0;

 private async Task OnSearchCounter()
 {
     await LoadDataCounter();
 }

 private async Task OnSearchCounterIndexIncrement()
 {
     if (CounterComboBoxIndex < (totalCountCounter - 1))
     {
         CounterComboBoxIndex++;
         await LoadDataCounter(CounterComboBoxIndex, 10);
     }
 }

 private async Task OnSearchCounterIndexDecrement()
 {
     if (CounterComboBoxIndex > 0)
     {
         CounterComboBoxIndex--;
         await LoadDataCounter(CounterComboBoxIndex, 10);
     }
 }

 private async Task OnInputCounterChanged(string e)
 {
     CounterComboBoxIndex = 0;
     await LoadDataCounter();
 }



  private async Task LoadDataCounter(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetCounterQuery
          {
              SearchTerm = refCounterComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Counters = result.Item1;
          totalCountCounter = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastCounter);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxCounter

private async Task LoadDataCounter(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetCounterQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refCounterComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentCounter,
            x => x.Counter,
        ],
        select: x => new Counter
        {
            Id = x.Id,
            Name = x.Name,
            ParentCounter = new Domain.Entities.Counter
            {
                Name = x.Name
            },
            Counter = new Domain.Entities.Counter
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.Counter
            {
                Name = x.Name
            },
            CounterCategory = x.CounterCategory
        }

    ));
    Counters = result.Item1;
    totalCountCounter = result.pageCount;
    PanelVisible = false;
}

await LoadDataCounter(CounterId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).CounterId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Counter" ColSpanMd="12">
    <MyDxComboBox Data="@Counters"
                  NullText="Select Counter"
                  @ref="refCounterComboBox"
                  @bind-Value="@a.CounterId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputCounterChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchCounterIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchCounter"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchCounterIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(CounterDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Counter.Name" Caption="Counter" />
            <DxListEditorColumn FieldName="@nameof(CounterDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.CounterId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetCounterQuery(x => x.Id == a.CounterId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
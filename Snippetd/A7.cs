await LoadDataDrugRoute();

 #region ComboboxDrugRoute

 private DxComboBox<DrugRouteDto, long?> refDrugRouteComboBox { get; set; }
 private int DrugRouteComboBoxIndex { get; set; } = 0;
 private int totalCountDrugRoute = 0;

 private async Task OnSearchDrugRoute()
 {
     await LoadDataDrugRoute();
 }

 private async Task OnSearchDrugRouteIndexIncrement()
 {
     if (DrugRouteComboBoxIndex < (totalCountDrugRoute - 1))
     {
         DrugRouteComboBoxIndex++;
         await LoadDataDrugRoute(DrugRouteComboBoxIndex, 10);
     }
 }

 private async Task OnSearchDrugRouteIndexDecrement()
 {
     if (DrugRouteComboBoxIndex > 0)
     {
         DrugRouteComboBoxIndex--;
         await LoadDataDrugRoute(DrugRouteComboBoxIndex, 10);
     }
 }

 private async Task OnInputDrugRouteChanged(string e)
 {
     DrugRouteComboBoxIndex = 0;
     await LoadDataDrugRoute();
 }



private async Task LoadDataDrugRoute(int pageIndex = 0, int pageSize = 10)
 {
    try
    {
        PanelVisible = true; 
        var result = await Mediator.Send(new GetDrugRouteQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDrugRouteComboBox?.Text ?? ""));
        DrugRoutes = result.Item1;
        totalCountDrugRoute = result.pageCount;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
 }

 #endregion ComboboxDrugRoute
private async Task LoadDataDrugRoute(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetDrugRouteQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refDrugRouteComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentDrugRoute,
            x => x.DrugRoute,
        ],
        select: x => new DrugRoute
        {
            Id = x.Id,
            Name = x.Name,
            ParentDrugRoute = new Domain.Entities.DrugRoute
            {
                Name = x.Name
            },
            DrugRoute = new Domain.Entities.DrugRoute
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.User
            {
                Name = x.Name
            },
            DrugRouteCategory = x.DrugRouteCategory
        }

    ));
    DrugRoutes = result.Item1;
    totalCountDrugRoute = result.pageCount;
    PanelVisible = false;
}

await LoadDataDrugRoute(DrugRouteId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).DrugRouteId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="DrugRoute" ColSpanMd="12">
    <MyDxComboBox Data="@DrugRoutes"
                  NullText="Select DrugRoute"
                  @ref="refDrugRouteComboBox"
                  @bind-Value="@a.DrugRouteId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputDrugRouteChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchDrugRouteIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchDrugRoute"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchDrugRouteIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(DrugRouteDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="DrugRoute.Name" Caption="DrugRoute" />
            <DxListEditorColumn FieldName="@nameof(DrugRouteDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.DrugRouteId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetDrugRouteQuery(x => x.Id == a.DrugRouteId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
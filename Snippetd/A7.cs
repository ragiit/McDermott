await LoadDataLocation();

 #region ComboboxLocation

 private DxComboBox<LocationDto, long?> refLocationComboBox { get; set; }
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
          var result = await Mediator.Send(new GetLocationQuery
          {
              SearchTerm = refLocationComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Locations = result.Item1;
          totalCountLocation = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxLocation

private async Task LoadDataLocation(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetLocationQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refLocationComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentLocation,
            x => x.Location,
        ],
        select: x => new Location
        {
            Id = x.Id,
            Name = x.Name,
            ParentLocation = new Domain.Entities.Location
            {
                Name = x.Name
            },
            Location = new Domain.Entities.Location
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.Location
            {
                Name = x.Name
            },
            LocationCategory = x.LocationCategory
        }

    ));
    Locations = result.Item1;
    totalCountLocation = result.pageCount;
    PanelVisible = false;
}

await LoadDataLocation(LocationId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).LocationId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Location" ColSpanMd="12">
    <MyDxComboBox Data="@Locations"
                  NullText="Select Location"
                  @ref="refLocationComboBox"
                  @bind-Value="@a.LocationId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputLocationChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchLocationIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchLocation"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchLocationIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(LocationDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Location.Name" Caption="Location" />
            <DxListEditorColumn FieldName="@nameof(LocationDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.LocationId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetLocationQuery(x => x.Id == a.LocationId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;

 var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new());
 Countries = (await Mediator.Send(new GetCountryQuery
 {
     Predicate = x => x.Id == a.CountryId,
 })).Item1;
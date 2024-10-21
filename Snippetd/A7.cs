await LoadDataLabUom();

 #region ComboboxLabUom

 private DxComboBox<LabUomDto, long?> refLabUomComboBox { get; set; }
 private int LabUomComboBoxIndex { get; set; } = 0;
 private int totalCountLabUom = 0;

 private async Task OnSearchLabUom()
 {
     await LoadDataLabUom();
 }

 private async Task OnSearchLabUomIndexIncrement()
 {
     if (LabUomComboBoxIndex < (totalCountLabUom - 1))
     {
         LabUomComboBoxIndex++;
         await LoadDataLabUom(LabUomComboBoxIndex, 10);
     }
 }

 private async Task OnSearchLabUomIndexDecrement()
 {
     if (LabUomComboBoxIndex > 0)
     {
         LabUomComboBoxIndex--;
         await LoadDataLabUom(LabUomComboBoxIndex, 10);
     }
 }

 private async Task OnInputLabUomChanged(string e)
 {
     LabUomComboBoxIndex = 0;
     await LoadDataLabUom();
 }



  private async Task LoadDataLabUom(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetLabUomQuery
          {
              SearchTerm = refLabUomComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          LabUoms = result.Item1;
          totalCountLabUom = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxLabUom

private async Task LoadDataLabUom(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetLabUomQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refLabUomComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentLabUom,
            x => x.LabUom,
        ],
        select: x => new LabUom
        {
            Id = x.Id,
            Name = x.Name,
            ParentLabUom = new Domain.Entities.LabUom
            {
                Name = x.Name
            },
            LabUom = new Domain.Entities.LabUom
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.LabUom
            {
                Name = x.Name
            },
            LabUomCategory = x.LabUomCategory
        }

    ));
    LabUoms = result.Item1;
    totalCountLabUom = result.pageCount;
    PanelVisible = false;
}

await LoadDataLabUom(LabUomId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).LabUomId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="LabUom" ColSpanMd="12">
    <MyDxComboBox Data="@LabUoms"
                  NullText="Select LabUom"
                  @ref="refLabUomComboBox"
                  @bind-Value="@a.LabUomId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputLabUomChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchLabUomIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchLabUom"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchLabUomIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(LabUomDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="LabUom.Name" Caption="LabUom" />
            <DxListEditorColumn FieldName="@nameof(LabUomDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.LabUomId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetLabUomQuery(x => x.Id == a.LabUomId))).Item1;
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
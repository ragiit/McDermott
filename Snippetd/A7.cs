await LoadDataLabTest();

 #region ComboboxLabTest

 private DxComboBox<LabTestDto, long?> refLabTestComboBox { get; set; }
 private int LabTestComboBoxIndex { get; set; } = 0;
 private int totalCountLabTest = 0;

 private async Task OnSearchLabTest()
 {
     await LoadDataLabTest();
 }

 private async Task OnSearchLabTestIndexIncrement()
 {
     if (LabTestComboBoxIndex < (totalCountLabTest - 1))
     {
         LabTestComboBoxIndex++;
         await LoadDataLabTest(LabTestComboBoxIndex, 10);
     }
 }

 private async Task OnSearchLabTestIndexDecrement()
 {
     if (LabTestComboBoxIndex > 0)
     {
         LabTestComboBoxIndex--;
         await LoadDataLabTest(LabTestComboBoxIndex, 10);
     }
 }

 private async Task OnInputLabTestChanged(string e)
 {
     LabTestComboBoxIndex = 0;
     await LoadDataLabTest();
 }



  private async Task LoadDataLabTest(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetLabTestQuery
          {
              SearchTerm = refLabTestComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          LabTests = result.Item1;
          totalCountLabTest = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxLabTest

private async Task LoadDataLabTest(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetLabTestQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refLabTestComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentLabTest,
            x => x.LabTest,
        ],
        select: x => new LabTest
        {
            Id = x.Id,
            Name = x.Name,
            ParentLabTest = new Domain.Entities.LabTest
            {
                Name = x.Name
            },
            LabTest = new Domain.Entities.LabTest
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.LabTest
            {
                Name = x.Name
            },
            LabTestCategory = x.LabTestCategory
        }

    ));
    LabTests = result.Item1;
    totalCountLabTest = result.pageCount;
    PanelVisible = false;
}

await LoadDataLabTest(LabTestId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).LabTestId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="LabTest" ColSpanMd="12">
    <MyDxComboBox Data="@LabTests"
                  NullText="Select LabTest"
                  @ref="refLabTestComboBox"
                  @bind-Value="@a.LabTestId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputLabTestChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchLabTestIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchLabTest"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchLabTestIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(LabTestDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="LabTest.Name" Caption="LabTest" />
            <DxListEditorColumn FieldName="@nameof(LabTestDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.LabTestId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetLabTestQuery(x => x.Id == a.LabTestId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
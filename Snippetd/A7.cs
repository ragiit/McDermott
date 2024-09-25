await LoadDataFamily();

 #region ComboboxFamily

 private DxComboBox<FamilyDto, long?> refFamilyComboBox { get; set; }
 private int FamilyComboBoxIndex { get; set; } = 0;
 private int totalCountFamily = 0;

 private async Task OnSearchFamily()
 {
     await LoadDataFamily();
 }

 private async Task OnSearchFamilyIndexIncrement()
 {
     if (FamilyComboBoxIndex < (totalCountFamily - 1))
     {
         FamilyComboBoxIndex++;
         await LoadDataFamily(FamilyComboBoxIndex, 10);
     }
 }

 private async Task OnSearchFamilyndexDecrement()
 {
     if (FamilyComboBoxIndex > 0)
     {
         FamilyComboBoxIndex--;
         await LoadDataFamily(FamilyComboBoxIndex, 10);
     }
 }

 private async Task OnInputFamilyChanged(string e)
 {
     FamilyComboBoxIndex = 0;
     await LoadDataFamily();
 }

private async Task LoadDataFamily(int pageIndex = 0, int pageSize = 10, long? FamilyId = null)
 {
    try
    {
        PanelVisible = true; 
        var result = await Mediator.Send(new GetFamilyQuery(FamilyId == null ? null : x => x.Id == FamilyId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refFamilyComboBox?.Text ?? ""));
        Familys = result.Item1;
        totalCountFamily = result.pageCount;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
 }

 #endregion ComboboxFamily


await LoadDataFamily(FamilyId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).FamilyId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Family" ColSpanMd="12">
    <MyDxComboBox Data="@Familys"
                  NullText="Select Family"
                  @ref="refFamilyComboBox"
                  @bind-Value="@a.FamilyId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputFamilyChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchFamilyndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchFamily"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchFamilyIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(FamilyDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Family.Name" Caption="Family" />
            <DxListEditorColumn FieldName="@nameof(FamilyDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.FamilyId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetFamilyQuery(x => x.Id == a.FamilyId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
await LoadDataDiagnoses();

 #region ComboboxDiagnoses

 private DxComboBox<DiagnosesDto, long?> refDiagnosesComboBox { get; set; }
 private int DiagnosesComboBoxIndex { get; set; } = 0;
 private int totalCountDiagnoses = 0;

 private async Task OnSearchDiagnoses()
 {
     await LoadDataDiagnoses();
 }

 private async Task OnSearchDiagnosesIndexIncrement()
 {
     if (DiagnosesComboBoxIndex < (totalCountDiagnoses - 1))
     {
         DiagnosesComboBoxIndex++;
         await LoadDataDiagnoses(DiagnosesComboBoxIndex, 10);
     }
 }

 private async Task OnSearchDiagnosesIndexDecrement()
 {
     if (DiagnosesComboBoxIndex > 0)
     {
         DiagnosesComboBoxIndex--;
         await LoadDataDiagnoses(DiagnosesComboBoxIndex, 10);
     }
 }

 private async Task OnInputDiagnosesChanged(string e)
 {
     DiagnosesComboBoxIndex = 0;
     await LoadDataDiagnoses();
 }



private async Task LoadDataDiagnoses(int pageIndex = 0, int pageSize = 10)
 {
    try
    {
        PanelVisible = true; 
        var result = await Mediator.Send(new GetDiagnosesQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDiagnosesComboBox?.Text ?? ""));
        Diagnosess = result.Item1;
        totalCountDiagnoses = result.pageCount;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastDiagnoses);
    }
    finally { PanelVisible = false; }
 }

 #endregion ComboboxDiagnoses
private async Task LoadDataDiagnoses(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetDiagnosesQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refDiagnosesComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentDiagnoses,
            x => x.Diagnoses,
        ],
        select: x => new Diagnoses
        {
            Id = x.Id,
            Name = x.Name,
            ParentDiagnoses = new Domain.Entities.Diagnoses
            {
                Name = x.Name
            },
            Diagnoses = new Domain.Entities.Diagnoses
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.User
            {
                Name = x.Name
            },
            DiagnosesCategory = x.DiagnosesCategory
        }

    ));
    Diagnosess = result.Item1;
    totalCountDiagnoses = result.pageCount;
    PanelVisible = false;
}

await LoadDataDiagnoses(DiagnosesId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).DiagnosesId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Diagnoses" ColSpanMd="12">
    <MyDxComboBox Data="@Diagnosess"
                  NullText="Select Diagnoses"
                  @ref="refDiagnosesComboBox"
                  @bind-Value="@a.DiagnosesId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputDiagnosesChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchDiagnosesIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchDiagnoses"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchDiagnosesIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(DiagnosesDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Diagnoses.Name" Caption="Diagnoses" />
            <DxListEditorColumn FieldName="@nameof(DiagnosesDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.DiagnosesId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetDiagnosesQuery(x => x.Id == a.DiagnosesId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
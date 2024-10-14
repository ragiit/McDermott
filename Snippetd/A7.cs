await LoadDataInsurance();

 #region ComboboxInsurance

 private DxComboBox<InsuranceDto, long?> refInsuranceComboBox { get; set; }
 private int InsuranceComboBoxIndex { get; set; } = 0;
 private int totalCountInsurance = 0;

 private async Task OnSearchInsurance()
 {
     await LoadDataInsurance();
 }

 private async Task OnSearchInsuranceIndexIncrement()
 {
     if (InsuranceComboBoxIndex < (totalCountInsurance - 1))
     {
         InsuranceComboBoxIndex++;
         await LoadDataInsurance(InsuranceComboBoxIndex, 10);
     }
 }

 private async Task OnSearchInsuranceIndexDecrement()
 {
     if (InsuranceComboBoxIndex > 0)
     {
         InsuranceComboBoxIndex--;
         await LoadDataInsurance(InsuranceComboBoxIndex, 10);
     }
 }

 private async Task OnInputInsuranceChanged(string e)
 {
     InsuranceComboBoxIndex = 0;
     await LoadDataInsurance();
 }



  private async Task LoadDataInsurance(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetInsuranceQuery
          {
              SearchTerm = refInsuranceComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Insurances = result.Item1;
          totalCountInsurance = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxInsurance
private async Task LoadDataInsurance(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetInsuranceQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refInsuranceComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentInsurance,
            x => x.Insurance,
        ],
        select: x => new Insurance
        {
            Id = x.Id,
            Name = x.Name,
            ParentInsurance = new Domain.Entities.Insurance
            {
                Name = x.Name
            },
            Insurance = new Domain.Entities.Insurance
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.User
            {
                Name = x.Name
            },
            InsuranceCategory = x.InsuranceCategory
        }

    ));
    Insurances = result.Item1;
    totalCountInsurance = result.pageCount;
    PanelVisible = false;
}

await LoadDataInsurance(InsuranceId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).InsuranceId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Insurance" ColSpanMd="12">
    <MyDxComboBox Data="@Insurances"
                  NullText="Select Insurance"
                  @ref="refInsuranceComboBox"
                  @bind-Value="@a.InsuranceId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputInsuranceChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchInsuranceIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchInsurance"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchInsuranceIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(InsuranceDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Insurance.Name" Caption="Insurance" />
            <DxListEditorColumn FieldName="@nameof(InsuranceDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.InsuranceId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetInsuranceQuery(x => x.Id == a.InsuranceId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
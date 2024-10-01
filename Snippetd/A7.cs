await LoadDataDepartment();

 #region ComboboxDepartment

 private DxComboBox<DepartmentDto, long?> refDepartmentComboBox { get; set; }
 private int DepartmentComboBoxIndex { get; set; } = 0;
 private int totalCountDepartment = 0;

 private async Task OnSearchDepartment()
 {
     await LoadDataDepartment();
 }

 private async Task OnSearchDepartmentIndexIncrement()
 {
     if (DepartmentComboBoxIndex < (totalCountDepartment - 1))
     {
         DepartmentComboBoxIndex++;
         await LoadDataDepartment(DepartmentComboBoxIndex, 10);
     }
 }

 private async Task OnSearchDepartmentIndexDecrement()
 {
     if (DepartmentComboBoxIndex > 0)
     {
         DepartmentComboBoxIndex--;
         await LoadDataDepartment(DepartmentComboBoxIndex, 10);
     }
 }

 private async Task OnInputDepartmentChanged(string e)
 {
     DepartmentComboBoxIndex = 0;
     await LoadDataDepartment();
 }

private async Task LoadDataDepartment(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetDepartmentQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refDepartmentComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentDepartment,
            x => x.Company,
        ],
        select: x => new Department
        {
            Id = x.Id,
            Name = x.Name,
            ParentDepartment = new Domain.Entities.Department
            {
                Name = x.Name
            },
            Company = new Domain.Entities.Company
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.User
            {
                Name = x.Name
            },
            DepartmentCategory = x.DepartmentCategory
        }

    ));
    Departments = result.Item1;
    totalCountDepartment = result.pageCount;
    PanelVisible = false;
}

private async Task LoadDataDepartment(int pageIndex = 0, int pageSize = 10, long? DepartmentId = null)
 {
    try
    {
        PanelVisible = true; 
        var result = await Mediator.Send(new GetDepartmentQuery(DepartmentId == null ? null : x => x.Id == DepartmentId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refDepartmentComboBox?.Text ?? ""));
        Departments = result.Item1;
        totalCountDepartment = result.pageCount;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
 }

 #endregion ComboboxDepartment


await LoadDataDepartment(DepartmentId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).DepartmentId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Department" ColSpanMd="12">
    <MyDxComboBox Data="@Departments"
                  NullText="Select Department"
                  @ref="refDepartmentComboBox"
                  @bind-Value="@a.DepartmentId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputDepartmentChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchDepartmentIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchDepartment"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchDepartmentIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(DepartmentDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Department.Name" Caption="Department" />
            <DxListEditorColumn FieldName="@nameof(DepartmentDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.DepartmentId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetDepartmentQuery(x => x.Id == a.DepartmentId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
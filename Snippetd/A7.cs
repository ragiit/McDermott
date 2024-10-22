await LoadDataProject();

 #region ComboboxProject

 private DxComboBox<ProjectDto, long?> refProjectComboBox { get; set; }
 private int ProjectComboBoxIndex { get; set; } = 0;
 private int totalCountProject = 0;

 private async Task OnSearchProject()
 {
     await LoadDataProject();
 }

 private async Task OnSearchProjectIndexIncrement()
 {
     if (ProjectComboBoxIndex < (totalCountProject - 1))
     {
         ProjectComboBoxIndex++;
         await LoadDataProject(ProjectComboBoxIndex, 10);
     }
 }

 private async Task OnSearchProjectIndexDecrement()
 {
     if (ProjectComboBoxIndex > 0)
     {
         ProjectComboBoxIndex--;
         await LoadDataProject(ProjectComboBoxIndex, 10);
     }
 }

 private async Task OnInputProjectChanged(string e)
 {
     ProjectComboBoxIndex = 0;
     await LoadDataProject();
 }



  private async Task LoadDataProject(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetProjectQuery
          {
              SearchTerm = refProjectComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Projects = result.Item1;
          totalCountProject = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxProject

private async Task LoadDataProject(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetProjectQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refProjectComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentProject,
            x => x.Project,
        ],
        select: x => new Project
        {
            Id = x.Id,
            Name = x.Name,
            ParentProject = new Domain.Entities.Project
            {
                Name = x.Name
            },
            Project = new Domain.Entities.Project
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.Project
            {
                Name = x.Name
            },
            ProjectCategory = x.ProjectCategory
        }

    ));
    Projects = result.Item1;
    totalCountProject = result.pageCount;
    PanelVisible = false;
}

await LoadDataProject(ProjectId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).ProjectId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Project" ColSpanMd="12">
    <MyDxComboBox Data="@Projects"
                  NullText="Select Project"
                  @ref="refProjectComboBox"
                  @bind-Value="@a.ProjectId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputProjectChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchProjectIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchProject"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchProjectIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(ProjectDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Project.Name" Caption="Project" />
            <DxListEditorColumn FieldName="@nameof(ProjectDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.ProjectId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetProjectQuery(x => x.Id == a.ProjectId))).Item1;
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
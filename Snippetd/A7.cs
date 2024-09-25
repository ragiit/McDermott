await LoadDataGroup();

 #region ComboboxGroup

 private DxComboBox<GroupDto, long?> refGroupComboBox { get; set; }
 private int GroupComboBoxIndex { get; set; } = 0;
 private int totalCountGroup = 0;

 private async Task OnSearchGroup()
 {
     await LoadDataGroup();
 }

 private async Task OnSearchGroupIndexIncrement()
 {
     if (GroupComboBoxIndex < (totalCountGroup - 1))
     {
         GroupComboBoxIndex++;
         await LoadDataGroup(GroupComboBoxIndex, 10);
     }
 }

 private async Task OnSearchGroupndexDecrement()
 {
     if (GroupComboBoxIndex > 0)
     {
         GroupComboBoxIndex--;
         await LoadDataGroup(GroupComboBoxIndex, 10);
     }
 }

 private async Task OnInputGroupChanged(string e)
 {
     GroupComboBoxIndex = 0;
     await LoadDataGroup();
 }

private async Task LoadDataGroup(int pageIndex = 0, int pageSize = 10, long? GroupId = null)
 {
     PanelVisible = true; 
     var result = await Mediator.Send(new GetGroupQuery(GroupId == null ? null : x => x.Id == GroupId, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refGroupComboBox?.Text ?? ""));
     Groups = result.Item1;
     totalCountGroup = result.pageCount;
     PanelVisible = false;
 }

 #endregion ComboboxGroup


await LoadDataGroup(GroupId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).GroupId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Group" ColSpanMd="12">
    <MyDxComboBox Data="@Groups"
                  NullText="Select Group"
                  @ref="refGroupComboBox"
                  @bind-Value="@a.GroupId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputGroupChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchGroupndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchGroup"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchGroupIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(GroupDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Group.Name" Caption="Group" />
            <DxListEditorColumn FieldName="@nameof(GroupDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.GroupId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetGroupQuery(x => x.Id == a.GroupId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
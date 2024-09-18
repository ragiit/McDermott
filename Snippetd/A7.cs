await LoadDataOccupational(0, 10);

 #region ComboboxOccupational

 private DxComboBox<OccupationalDto, long?> refOccupationalComboBox { get; set; }
 private int OccupationalComboBoxIndex { get; set; } = 0;
 private int totalCountOccupational = 0;

 private async Task OnSearchOccupational()
 {
     await LoadDataOccupational(0, 10);
 }

 private async Task OnSearchOccupationalIndexIncrement()
 {
     if (OccupationalComboBoxIndex < (totalCountOccupational - 1))
     {
         OccupationalComboBoxIndex++;
         await LoadDataOccupational(OccupationalComboBoxIndex, 10);
     }
 }

 private async Task OnSearchOccupationalndexDecrement()
 {
     if (OccupationalComboBoxIndex > 0)
     {
         OccupationalComboBoxIndex--;
         await LoadDataOccupational(OccupationalComboBoxIndex, 10);
     }
 }

 private async Task OnInputOccupationalChanged(string e)
 {
     OccupationalComboBoxIndex = 0;
     await LoadDataOccupational(0, 10);
 }

 private async Task LoadDataOccupational(int pageIndex = 0, int pageSize = 10)
 {
     PanelVisible = true;
     SelectedDataItems = [];
     var result = await Mediator.Send(new GetOccupationalQuery(pageIndex: pageIndex, pageSize: pageSize, searchTerm: refOccupationalComboBox?.Text ?? ""));
     Occupationals = result.Item1;
     totalCountOccupational = result.pageCount;
     PanelVisible = false;
 }

 #endregion ComboboxOccupational





<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Occupational" ColSpanMd="12">
    <MyDxComboBox Data="@Occupationals"
                  NullText="Select Occupational..."
                  @ref="refOccupationalComboBox"
                  @bind-Value="@a.OccupationalId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputOccupationalChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchOccupationalndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchOccupational"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchOccupationalIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(OccupationalDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Occupational.Name" Caption="Occupational" />
            <DxListEditorColumn FieldName="@nameof(OccupationalDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.OccupationalId)" />
</DxFormLayoutItem>
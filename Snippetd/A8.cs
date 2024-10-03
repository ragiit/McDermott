<TotalSummary>
    <DxGridSummaryItem SummaryType="GridSummaryItemType.Count"
                       FieldName="Name"
                       Visible="true" />
</TotalSummary>

 <EditFormTemplate Context="EditFormContext">
     @{
         var a = (CountryDto)EditFormContext.EditModel;
     }
     <DxFormLayout CssClass="w-100">
         <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Name" ColSpanMd="12">
             <DxTextBox @bind-Text="@a.Name" ShowValidationIcon="true" ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" NullText="Name" />
             <ValidationMessage For="@(()=> a.Name)"   />
         </DxFormLayoutItem>
         <DxFormLayoutItem CaptionCssClass="caption normal-caption" Caption="Code" ColSpanMd="12">
             <DxTextBox @bind-Text="@a.Code" ShowValidationIcon="true" ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" NullText="Code" />
             <ValidationMessage For="@(()=> a.Code)"   />
         </DxFormLayoutItem>
     </DxFormLayout>
 </EditFormTemplate>

    <div class="row">
        <DxFormLayout>
            <div class="col-md-9">
                <DxFormLayoutItem>
                    <DxPager PageCount="totalCount"
                            ActivePageIndexChanged="OnPageIndexChanged"
                            ActivePageIndex="activePageIndex"
                            VisibleNumericButtonCount="10"
                            SizeMode="SizeMode.Medium"
                            NavigationMode="PagerNavigationMode.Auto">
                    </DxPager>
                </DxFormLayoutItem>
            </div>
            <div class="col-md-3 d-flex justify-content-end">
                <DxFormLayoutItem Caption="Page Size:">
                    <MyDxComboBox Data="(new[] { 10, 25, 50, 100 })"
                                NullText="Select Page Size"
                                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Never"
                                SelectedItemChanged="((int e ) => OnPageSizeIndexChanged(e))"
                                @bind-Value="pageSize">
                    </MyDxComboBox>
                </DxFormLayoutItem>
            </div>
        </DxFormLayout>
    </div>
  <_BaseMyComboBox TData="UomCategory"
                   TValue="long?"
                   NullText="Select Job Position"
                   CustomData="@LoadCustomDataUomCategory"
                   @bind-Value="@UserForm.UomCategoryId"
                   TextFieldName="Name"
                   ValueFieldName="Id">
      <Columns>
          <DxListEditorColumn FieldName="@nameof(UomCategoryDto.Name)" Caption="Name" />
          <DxListEditorColumn FieldName="Department.Name" Caption="Department" />
      </Columns>
  </_BaseMyComboBox>

      protected async Task<LoadResult> LoadCustomDataUomCategory(DataSourceLoadOptionsBase options, CancellationToken cancellationToken)
    {
        return await QueryComboBoxHelper.LoadCustomData<UomCategory>(
            options: options,
            queryProvider: async () => await Mediator.Send(new GetQueryUomCategory()),
            cancellationToken: cancellationToken);
    }
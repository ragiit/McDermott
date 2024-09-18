<DxFormLayoutItem CaptionCssClass="caption normal-caption" Caption="Address" ColSpanMd="12">
    <DxGridLayout ColumnSpacing="8px" RowSpacing="8px">
        <Rows>
            <DxGridLayoutRow />
            <DxGridLayoutRow />
            <DxGridLayoutRow />
            <DxGridLayoutRow />
        </Rows>
        <Columns>
            <DxGridLayoutColumn />
            <DxGridLayoutColumn />
        </Columns>
        <Items>
            <DxGridLayoutItem Row="0" Column="0" ColumnSpan="12">
                <DxTextBox @bind-Text="@a.Street1" ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" NullText="Street 1" />
            </DxGridLayoutItem>
            <DxGridLayoutItem Row="1" Column="0" ColumnSpan="12">
                <DxTextBox @bind-Text="@a.Street2" ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" NullText="Street 2" />
            </DxGridLayoutItem>
            <DxFormLayoutItem Context="Ctxz" CaptionCssClass="required-caption normal-caption" Caption="Country" ColSpanMd="12">
                <MyDxComboBox Data="@Countries"
                                Context="Ctxz"
                                NullText="Select Country..."
                                @ref="refCountryComboBox"
                                @bind-Value="@a.CountryId"
                                TextFieldName="Name"
                                ValueFieldName="Id"
                                TextChanged="((string e) => OnInputCountryChanged(e))">
                    <Buttons>
                        <DxEditorButton Click="OnSearchCountryIndexDecrement"
                                        IconCssClass="fa-solid fa-caret-left"
                                        Tooltip="Previous Index" />
                        <DxEditorButton Click="OnSearchCountry"
                                        IconCssClass="fa-solid fa-magnifying-glass"
                                        Tooltip="Search" />
                        <DxEditorButton Click="OnSearchCountryIndexIncrement"
                                        IconCssClass="fa-solid fa-caret-right"
                                        Tooltip="Next Index" />
                    </Buttons>
                    <Columns>
                        <DxListEditorColumn FieldName="@nameof(Country.Name)" Caption="Name" />
                        <DxListEditorColumn FieldName="@nameof(Country.Code)" Caption="Code" />
                    </Columns>
                </MyDxComboBox>
                <ValidationMessage For="@(() => a.CountryId)" />
            </DxFormLayoutItem>
            <DxFormLayoutItem Context="Ctxz1" CaptionCssClass="required-caption normal-caption" Caption="Province" ColSpanMd="12">
                <MyDxComboBox Data="@Provinces" Context="Ctxz1"
                                NullText="Select Province..."
                                @ref="refProvinceComboBox"
                                @bind-Value="@a.ProvinceId"
                                TextFieldName="Name"
                                ValueFieldName="Id"
                                TextChanged="((string e) => OnInputProvinceChanged(e))">
                    <Buttons>
                        <DxEditorButton Click="OnSearchProvinceIndexDecrement"
                                        IconCssClass="fa-solid fa-caret-left"
                                        Tooltip="Previous Index" />
                        <DxEditorButton Click="OnSearchProvince"
                                        IconCssClass="fa-solid fa-magnifying-glass"
                                        Tooltip="Search" />
                        <DxEditorButton Click="OnSearchProvinceIndexIncrement"
                                        IconCssClass="fa-solid fa-caret-right"
                                        Tooltip="Next Index" />
                    </Buttons>
                    <Columns>
                        <DxListEditorColumn FieldName="@nameof(ProvinceDto.Name)" Caption="Name" />
                        <DxListEditorColumn FieldName="Country.Name" Caption="Country" />
                        <DxListEditorColumn FieldName="@nameof(ProvinceDto.Code)" Caption="Code" />
                    </Columns>
                </MyDxComboBox>
                <ValidationMessage For="@(() => a.ProvinceId)" />
            </DxFormLayoutItem>
            <DxFormLayoutItem Context="Ctxz2" CaptionCssClass="required-caption normal-caption" Caption="City" ColSpanMd="12">
                <MyDxComboBox Data="@Cities" Context="Ctxz2"
                                NullText="Select City..."
                                @ref="refCityComboBox"
                                @bind-Value="@a.CityId"
                                TextFieldName="Name"
                                ValueFieldName="Id"
                                TextChanged="((string e) => OnInputCityChanged(e))">
                    <Buttons>
                        <DxEditorButton Click="OnSearchCityIndexDecrement"
                                        IconCssClass="fa-solid fa-caret-left"
                                        Tooltip="Previous Index" />
                        <DxEditorButton Click="OnSearchCity"
                                        IconCssClass="fa-solid fa-magnifying-glass"
                                        Tooltip="Search" />
                        <DxEditorButton Click="OnSearchCityIndexIncrement"
                                        IconCssClass="fa-solid fa-caret-right"
                                        Tooltip="Next Index" />
                    </Buttons>
                    <Columns>
                        <DxListEditorColumn FieldName="@nameof(CityDto.Name)" Caption="Name" />
                        <DxListEditorColumn FieldName="Province.Name" Caption="Province" />
                    </Columns>
                </MyDxComboBox>
                <ValidationMessage For="@(() => a.CityId)" />
            </DxFormLayoutItem>
            <DxGridLayoutItem Row="3" Column="1" CssClass="ml">
                <DxTextBox @bind-Text="@a.Zip" ShowValidationIcon="true" ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" NullText="Postal Code" />
            </DxGridLayoutItem>
        </Items>
    </DxGridLayout>
</DxFormLayoutItem>

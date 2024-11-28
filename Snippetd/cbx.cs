
#region ComboBox City

    private CityDto SelectedCity { get; set; } = new();
    async Task SelectedItemChanged(CityDto e)
    {
        if (e is null)
        {
            SelectedCity = new();
            await LoadCity(); 
        }
        else
            SelectedCity = e;
    }

    private CancellationTokenSource? _ctsCity;
    private async Task OnInputCity(ChangeEventArgs e)
    {
        try
        { 
            _ctsCity?.Cancel();
            _ctsCity?.Dispose();
            _ctsCity = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsCity.Token);

            await LoadCity(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsCity?.Dispose();
            _ctsCity = null;
        }
    }

    private async Task LoadCity(string? e = "", Expression<Func<City, bool>>? predicate = null)
    {
        try
        { 
            Citys = await Mediator.QueryGetComboBox<City, CityDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastService);
        }
        finally { PanelVisible = false; }
    }

#endregion


<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="City" ColSpanMd="12">
    <MyDxComboBox Data="Citys"
                NullText="Select City"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputCity"
                SelectedItemChanged="((CityDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.CityId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(City.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(City.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.CityId)" />
</DxFormLayoutItem>


City
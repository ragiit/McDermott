
#region ComboBox Location

    private UserDto SelectedLocation { get; set; } = new();
    async Task SelectedItemChanged(UserDto e)
    {
        if (e is null)
        {
            SelectedLocation = new();
            await LoadLocation(); 
        }
        else
            SelectedLocation = e;
    }

    private CancellationTokenSource? _ctsLocation;
    private async Task OnInputLocation(ChangeEventArgs e)
    {
        try
        { 
            _ctsLocation?.Cancel();
            _ctsLocation?.Dispose();
            _ctsLocation = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsLocation.Token);

            await LoadLocation(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsLocation?.Dispose();
            _ctsLocation = null;
        }
    }

    private async Task LoadLocation(string? e = "", Expression<Func<User, bool>>? predicate = null)
    {
        try
        { 
            Locations = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastService);
        }
        finally { PanelVisible = false; }
    }

#endregion


<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Location" ColSpanMd="12">
    <MyDxComboBox Data="Locations"
                NullText="Select Location"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputLocation"
                SelectedItemChanged="((UserDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.LocationId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Location.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Location.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.LocationId)" />
</DxFormLayoutItem>


Location
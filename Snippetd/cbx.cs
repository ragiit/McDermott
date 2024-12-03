
#region ComboBox Patient

    private UserDto SelectedPatient { get; set; } = new();
    async Task SelectedItemChanged(UserDto e)
    {
        if (e is null)
        {
            SelectedPatient = new();
            await LoadPatient(); 
        }
        else
            SelectedPatient = e;
    }

    private CancellationTokenSource? _ctsPatient;
    private async Task OnInputPatient(ChangeEventArgs e)
    {
        try
        { 
            _ctsPatient?.Cancel();
            _ctsPatient?.Dispose();
            _ctsPatient = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsPatient.Token);

            await LoadPatient(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsPatient?.Dispose();
            _ctsPatient = null;
        }
    }

    private async Task LoadPatient(string? e = "", Expression<Func<User, bool>>? predicate = null)
    {
        try
        { 
            Patients = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastService);
        }
        finally { PanelVisible = false; }
    }

#endregion


<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Patient" ColSpanMd="12">
    <MyDxComboBox Data="Patients"
                NullText="Select Patient"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputPatient"
                SelectedItemChanged="((UserDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.PatientId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Patient.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Patient.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.PatientId)" />
</DxFormLayoutItem>


Patient
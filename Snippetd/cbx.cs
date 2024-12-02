
#region ComboBox Diagnosis

    private DiagnosisDto SelectedDiagnosis { get; set; } = new();
    async Task SelectedItemChanged(DiagnosisDto e)
    {
        if (e is null)
        {
            SelectedDiagnosis = new();
            await LoadDiagnosis(); 
        }
        else
            SelectedDiagnosis = e;
    }

    private CancellationTokenSource? _ctsDiagnosis;
    private async Task OnInputDiagnosis(ChangeEventArgs e)
    {
        try
        { 
            _ctsDiagnosis?.Cancel();
            _ctsDiagnosis?.Dispose();
            _ctsDiagnosis = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsDiagnosis.Token);

            await LoadDiagnosis(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsDiagnosis?.Dispose();
            _ctsDiagnosis = null;
        }
    }

    private async Task LoadDiagnosis(string? e = "", Expression<Func<Diagnosis, bool>>? predicate = null)
    {
        try
        { 
            Diagnosiss = await Mediator.QueryGetComboBox<Diagnosis, DiagnosisDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastService);
        }
        finally { PanelVisible = false; }
    }

#endregion


<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Diagnosis" ColSpanMd="12">
    <MyDxComboBox Data="Diagnosiss"
                NullText="Select Diagnosis"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputDiagnosis"
                SelectedItemChanged="((DiagnosisDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.DiagnosisId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Diagnosis.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Diagnosis.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.DiagnosisId)" />
</DxFormLayoutItem>


Diagnosis
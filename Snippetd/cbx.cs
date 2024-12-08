
#region ComboBox Project

    private UserDto SelectedProject { get; set; } = new();
    async Task SelectedItemChanged(UserDto e)
    {
        if (e is null)
        {
            SelectedProject = new();
            await LoadProject(); 
        }
        else
            SelectedProject = e;
    }

    private CancellationTokenSource? _ctsProject;
    private async Task OnInputProject(ChangeEventArgs e)
    {
        try
        { 
            _ctsProject?.Cancel();
            _ctsProject?.Dispose();
            _ctsProject = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsProject.Token);

            await LoadProject(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsProject?.Dispose();
            _ctsProject = null;
        }
    }

    private async Task LoadProject(string? e = "", Expression<Func<User, bool>>? predicate = null)
    {
        try
        { 
            Projects = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastService);
        }
        finally { PanelVisible = false; }
    }

#endregion


<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Project" ColSpanMd="12">
    <MyDxComboBox Data="Projects"
                NullText="Select Project"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputProject"
                SelectedItemChanged="((UserDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.ProjectId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Project.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Project.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.ProjectId)" />
</DxFormLayoutItem>


Project
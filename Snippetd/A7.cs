await LoadDataUser();

 #region ComboboxUser

 private DxComboBox<UserDto, long?> refUserComboBox { get; set; }
 private int UserComboBoxIndex { get; set; } = 0;
 private int totalCountUser = 0;

 private async Task OnSearchUser()
 {
     await LoadDataUser();
 }

 private async Task OnSearchUserIndexIncrement()
 {
     if (UserComboBoxIndex < (totalCountUser - 1))
     {
         UserComboBoxIndex++;
         await LoadDataUser(UserComboBoxIndex, 10);
     }
 }

 private async Task OnSearchUserIndexDecrement()
 {
     if (UserComboBoxIndex > 0)
     {
         UserComboBoxIndex--;
         await LoadDataUser(UserComboBoxIndex, 10);
     }
 }

 private async Task OnInputUserChanged(string e)
 {
     UserComboBoxIndex = 0;
     await LoadDataUser();
 }



  private async Task LoadDataUser(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetUserQuery
          {
              SearchTerm = refUserComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Users = result.Item1;
          totalCountUser = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxUser

private async Task LoadDataUser(int pageIndex = 0, int pageSize = 10)
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetUserQuery(
        pageIndex: pageIndex,
        pageSize: pageSize,
        searchTerm: refUserComboBox?.Text ?? "",
        includes:
        [
            x => x.Manager,
            x => x.ParentUser,
            x => x.User,
        ],
        select: x => new User
        {
            Id = x.Id,
            Name = x.Name,
            ParentUser = new Domain.Entities.User
            {
                Name = x.Name
            },
            User = new Domain.Entities.User
            {
                Name = x.Name
            },
            Manager = new Domain.Entities.User
            {
                Name = x.Name
            },
            UserCategory = x.UserCategory
        }

    ));
    Users = result.Item1;
    totalCountUser = result.pageCount;
    PanelVisible = false;
}

await LoadDataUser(UserId: (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new()).UserId);


var id = refDistrictComboBox?.Value ?? null;
&& (id == null || x.Id == id)

   var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="User" ColSpanMd="12">
    <MyDxComboBox Data="@Users"
                  NullText="Select User"
                  @ref="refUserComboBox"
                  @bind-Value="@a.UserId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputUserChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchUserIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchUser"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchUserIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(UserDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="User.Name" Caption="User" />
            <DxListEditorColumn FieldName="@nameof(UserDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.UserId)" />
</DxFormLayoutItem>

await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());

 await LoadComboboxEdit(a);

  private async Task LoadComboboxEdit(DiagnosisDto a)
 {
     Cronises = (await Mediator.Send(new GetUserQuery(x => x.Id == a.UserId))).Item1;
     Diseases = (await Mediator.Send(new GetDiseaseCategoryQuery(x => x.Id == a.DiseaseCategoryId))).Item1;
 }

 
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as DiseaseCategoryDto ?? new());
PanelVisible = true; 

var resultz = await Mediator.Send(new GetCountryQuery(x => x.Id == a.CountryId));
Countries = resultz.Item1;
totalCountCountry = resultz.pageCount;  

PanelVisible = false;
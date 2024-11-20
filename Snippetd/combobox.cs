NEW COMBOBOX VIRAL 2024

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Country" ColSpanMd="12">
    <MyDxComboBox Data="Countries"
                NullText="Select Country"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputCountry"
                SelectedItemChanged="((CountryDto e) => SelectedItemChanged(e))" // untuk refresh ketika clear cbx nya
                @bind-Value="a.CountryId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Country.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Country.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.CountryId)" />
</DxFormLayoutItem>


#region ComboBox Country

private CountryDto SelectedCountry { get; set; } = new();
async Task SelectedItemChanged(CountryDto e)
{
    if (e is null)
    {
        SelectedCountry = new();
        await LoadCounty(); // untuk refresh lagi ketika user klik clear 
    }
    else
        SelectedCountry = e;
}

private CancellationTokenSource? _cts;
private async Task OnInputCountry(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        await Task.Delay(Helper.CBX_DELAY, _cts.Token);

        await LoadCounty(e.Value?.ToString() ?? "");
    }
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _cts?.Dispose();
        _cts = null;
    }
}

private async Task LoadCounty(string? e = "", Expression<Func<Country, bool>>? predicate = null)
{
    try
    {
        PanelVisible = true;
        Countries = await Mediator.QueryGetComboBox<Country, CountryDto>(e, predicate);
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}

#endregion

// di method ini ga usah manggil comboboxnya,,, 
protected override async Task OnInitializedAsync()
{
    PanelVisible = true;
    await LoadData();
    PanelVisible = false;
}

// panggil ketika klik new
private async Task NewItem_Click()
{
    await LoadCounty();
    await Grid.StartEditNewRowAsync();
}


// kondisi ketika klik edit
private async Task EditItem_Click()
{
    try
    {
        PanelVisible = true;
        await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as ProvinceDto ?? new());
        await LoadCounty(predicate: x => x.Id == a.CountryId); // panggil countryid nya 
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}



// CASE KE 2 
// Mencari data dari combobox lain,
// misalnya mencari City berdasarkan Province

// Province
<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Province" ColSpanMd="12">
    <MyDxComboBox Data="Provinces" 
                NullText="Select Province" 
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputProvince"
                SelectedItemChanged="((ProvinceDto e) => SelectedItemChanged(e))"
                @bind-Value="a.ProvinceId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(ProvinceDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Country.Name" Caption="Country" /> 
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.ProvinceId)" />
</DxFormLayoutItem>

// City
<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="City" ColSpanMd="12">
    <MyDxComboBox Data="Cities" 
                NullText="Select City" 
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputCity"
                SelectedItemChanged="((CityDto e) => SelectedItemCityChanged(e))"
                @bind-Value="a.CityId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(CityDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Province.Name" Caption="Province" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.CityId)" />
</DxFormLayoutItem>


 #region ComboBox Province
 private ProvinceDto SelectedProvince { get; set; } = new();
 async Task SelectedItemChanged(ProvinceDto e)
 {
     if (e is null)
         SelectedProvince = new();
     else
     {
         SelectedProvince = e;

         await LoadCity(); // refresh datanya
     }
 }

 private CancellationTokenSource? _ctsProvince;
 private async Task OnInputProvince(ChangeEventArgs e)
 {
     try
     {
         PanelVisible = true;

         _ctsProvince?.Cancel();
         _ctsProvince?.Dispose();
         _ctsProvince = new CancellationTokenSource();

         await Task.Delay(Helper.CBX_DELAY, _ctsProvince.Token);

         await LoadProvince(e.Value?.ToString() ?? ""); // ini kosong
     }
     finally
     {
         PanelVisible = false;

         // Untuk menghindari kebocoran memori (memory leaks).
         _ctsProvince?.Dispose();
         _ctsProvince = null;
     }
 }

 private async Task LoadProvince(string? e = "", Expression<Func<Province, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         Provinces = await Mediator.QueryGetComboBox<Province, ProvinceDto>(e, predicate);
         PanelVisible = false;
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastService);
     }
     finally { PanelVisible = false; }
 }

 #endregion


  #region ComboBox City
 private CityDto SelectedCity { get; set; } = new();
 async Task SelectedItemCityChanged(CityDto e)
 {
     if (e is null)
         SelectedCity = new();
     else
     {
         SelectedCity = e;

         await LoadDistrict(); // refresh districtnya ketika sudah milih city
     }
 }
 private CancellationTokenSource? _ctsCity;
 private async Task OnInputCity(ChangeEventArgs e)
 {
     try
     {
         PanelVisible = true;

         _ctsCity?.Cancel();
         _ctsCity?.Dispose();
         _ctsCity = new CancellationTokenSource();

         await Task.Delay(Helper.CBX_DELAY, _ctsCity.Token);

         await LoadCity(e.Value?.ToString() ?? "", x => x.ProvinceId == SelectedProvince.Id); // cari berdasarkan selected ProvinceId
     }
     finally
     {
         PanelVisible = false;

         // Untuk menghindari kebocoran memori (memory leaks).
         _ctsCity?.Dispose();
         _ctsCity = null;
     }
 }

 private async Task LoadCity(string? e = "", Expression<Func<City, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         Cities = await Mediator.QueryGetComboBox<City, CityDto>(e, predicate);
         PanelVisible = false;
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastService);
     }
     finally { PanelVisible = false; }
 }

 #endregion
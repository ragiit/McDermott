Country

public class CountryCommand
 {
     #region GET

    public class GetSingleCountryQuery : IRequest<CountryDto>
    {
        public List<Expression<Func<Country, object>>> Includes { get; set; }
        public Expression<Func<Country, bool>> Predicate { get; set; }
        public Expression<Func<Country, Country>> Select { get; set; }

        public List<(Expression<Func<Country, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetCountryQuery : IRequest<(List<CountryDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<Country, object>>> Includes { get; set; }
        public Expression<Func<Country, bool>> Predicate { get; set; }
        public Expression<Func<Country, Country>> Select { get; set; }

        public List<(Expression<Func<Country, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateCountry(Expression<Func<Country, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<Country, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateCountryRequest(CountryDto CountryDto) : IRequest<CountryDto>
     {
         public CountryDto CountryDto { get; set; } = CountryDto;
     }

     public class BulkValidateCountry(List<CountryDto> CountrysToValidate) : IRequest<List<CountryDto>>
     {
         public List<CountryDto> CountrysToValidate { get; } = CountrysToValidate;
     }

     public class CreateListCountryRequest(List<CountryDto> CountryDtos) : IRequest<List<CountryDto>>
     {
         public List<CountryDto> CountryDtos { get; set; } = CountryDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateCountryRequest(CountryDto CountryDto) : IRequest<CountryDto>
     {
         public CountryDto CountryDto { get; set; } = CountryDto;
     }

     public class UpdateListCountryRequest(List<CountryDto> CountryDtos) : IRequest<List<CountryDto>>
     {
         public List<CountryDto> CountryDtos { get; set; } = CountryDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteCountryRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateCountryQuery, List<CountryDto>>,
  
IRequestHandler<GetCountryQuery, (List<CountryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleCountryQuery, CountryDto>,
public class CountryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetCountryQuery, (List<CountryDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleCountryQuery, CountryDto>, 
     IRequestHandler<ValidateCountry, bool>,
     IRequestHandler<CreateCountryRequest, CountryDto>,
     IRequestHandler<BulkValidateCountry, List<CountryDto>>,
     IRequestHandler<CreateListCountryRequest, List<CountryDto>>,
     IRequestHandler<UpdateCountryRequest, CountryDto>,
     IRequestHandler<UpdateListCountryRequest, List<CountryDto>>,
     IRequestHandler<DeleteCountryRequest, bool>
{
    #region GET
    public async Task<List<CountryDto>> Handle(BulkValidateCountry request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.CountrysToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var CountryNames = CountryDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = CountryDtos.Select(x => x.Code).Distinct().ToList();

        //var existingCountrys = await _unitOfWork.Repository<Country>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => CountryNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingCountrys.Adapt<List<CountryDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidateCountry request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Country>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<CountryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCountryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Country>().Entities.AsNoTracking(); 

            if (request.Predicate is not null)
                query = query.Where(request.Predicate);

            // Apply ordering
            if (request.OrderByList.Count != 0)
            {
                var firstOrderBy = request.OrderByList.First();
                query = firstOrderBy.IsDescending
                    ? query.OrderByDescending(firstOrderBy.OrderBy)
                    : query.OrderBy(firstOrderBy.OrderBy);

                foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                {
                    query = additionalOrderBy.IsDescending
                        ? ((IOrderedQueryable<Country>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Country>)query).ThenBy(additionalOrderBy.OrderBy);
                }
            }

            // Apply dynamic includes
            if (request.Includes is not null)
            {
                foreach (var includeExpression in request.Includes)
                {
                    query = query.Include(includeExpression);
                }
            }
    
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Country.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Country
                {
                    Id = x.Id, 
                });

            if (!request.IsGetAll)
            { // Paginate and sort
                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                    query,
                    request.PageSize,
                    request.PageIndex,
                    cancellationToken
                );

                return (pagedItems.Adapt<List<CountryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<CountryDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<CountryDto> Handle(GetSingleCountryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Country>().Entities.AsNoTracking();

            if (request.Predicate is not null)
                query = query.Where(request.Predicate);
            
            // Apply ordering
            if (request.OrderByList.Count != 0)
            {
                var firstOrderBy = request.OrderByList.First();
                query = firstOrderBy.IsDescending
                    ? query.OrderByDescending(firstOrderBy.OrderBy)
                    : query.OrderBy(firstOrderBy.OrderBy);

                foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                {
                    query = additionalOrderBy.IsDescending
                        ? ((IOrderedQueryable<Country>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Country>)query).ThenBy(additionalOrderBy.OrderBy);
                }
            }

            // Apply dynamic includes
            if (request.Includes is not null)
            {
                foreach (var includeExpression in request.Includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(v =>
                    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    EF.Functions.Like(v.Country.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Country
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<CountryDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<CountryDto> Handle(CreateCountryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDto.Adapt<CreateUpdateCountryDto>().Adapt<Country>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<CountryDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<CountryDto>> Handle(CreateListCountryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDtos.Adapt<List<Country>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<CountryDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<CountryDto> Handle(UpdateCountryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDto.Adapt<CountryDto>().Adapt<Country>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<CountryDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<CountryDto>> Handle(UpdateListCountryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDtos.Adapt<List<Country>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<CountryDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteCountryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<Country>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<Country>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateCountryQuery(List<CountryDto> CountrysToValidate) : IRequest<List<CountryDto>>
{
    public List<CountryDto> CountrysToValidate { get; } = CountrysToValidate;
}a


IRequestHandler<BulkValidateCountryQuery, List<CountryDto>>,
  
IRequestHandler<GetCountryQuery, (List<CountryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleCountryQuery, CountryDto>,



 var a = await Mediator.Send(new GetCountrysQuery
 {
     OrderByList =
     [
         (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
         (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
         (x => x.ClassType != null, true)               // ThenByDescending ClassType is not null
     ],
     PageIndex = pageIndex,
     PageSize = pageSize,
 });

var patienss = (await Mediator.Send(new GetSingleCountryQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Country
    {
        Id = x.Id,
        IsEmployee = x.IsEmployee,
        Name = x.Name,
        Gender = x.Gender,
        DateOfBirth = x.DateOfBirth
    },
})) ?? new();

try
{
    PanelVisible = true;
    var result = await Mediator.Send(new GetCountryQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    Countrys = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastService);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetCountryQuery
 {
     Predicate = x => x.CountryId == CountryId,
     SearchTerm = refCountryComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 Countrys = result.Item1;
 totalCountCountry = result.PageCount;

 Countrys = (await Mediator.Send(new GetCountryQuery
 {
     Predicate = x => x.Id == CountryForm.IdCardCountryId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleCountrysQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Country
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Country
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Country
        {
            Name = x.Pratitioner.Name,
            SipNo = x.Pratitioner.SipNo
        },
        StartMaternityLeave = x.StartMaternityLeave,
        EndMaternityLeave = x.EndMaternityLeave,
        StartDateSickLeave = x.StartDateSickLeave,
        EndDateSickLeave = x.EndDateSickLeave,
    }
})) ?? new();


#region ComboboxCountry

 private DxComboBox<CountryDto, long?> refCountryComboBox { get; set; }
 private int CountryComboBoxIndex { get; set; } = 0;
 private int totalCountCountry = 0;

 private async Task OnSearchCountry()
 {
     await LoadDataCountry();
 }

 private async Task OnSearchCountryIndexIncrement()
 {
     if (CountryComboBoxIndex < (totalCountCountry - 1))
     {
         CountryComboBoxIndex++;
         await LoadDataCountry(CountryComboBoxIndex, 10);
     }
 }

 private async Task OnSearchCountryIndexDecrement()
 {
     if (CountryComboBoxIndex > 0)
     {
         CountryComboBoxIndex--;
         await LoadDataCountry(CountryComboBoxIndex, 10);
     }
 }

 private async Task OnInputCountryChanged(string e)
 {
     CountryComboBoxIndex = 0;
     await LoadDataCountry();
 }

 
  private async Task LoadDataCountry(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetCountryQuery
          {
              SearchTerm = refCountryComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Countrys = result.Item1;
          totalCountCountry = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxCountry

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Country" ColSpanMd="12">
    <MyDxComboBox Data="@Countrys"
                  NullText="Select Country"
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
            <DxListEditorColumn FieldName="@nameof(CountryDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Country.Name" Caption="Country" />
            <DxListEditorColumn FieldName="@nameof(CountryDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.CountryId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDto.Adapt<CreateUpdateCountryDto>().Adapt<Country>());
var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDtos.Adapt<List<CreateUpdateCountryDto>>().Adapt<List<Country>>()); 

var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDto.Adapt<CreateUpdateCountryDto>().Adapt<Country>());  
var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDtos.Adapt<List<CreateUpdateCountryDto>>().Adapt<List<Country>>());

list3 = (await Mediator.Send(new GetCountryQuery
{
    Predicate = x => CountryNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new Country
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeCountryAttendance { get; set; } = 10;
    private int totalCountCountryAttendance = 0;
    private int activePageIndexCountryAttendance { get; set; } = 0;
    private string searchTermCountryAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedCountryAttendance(string searchText)
    {
        searchTermCountryAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeCountryAttendance);
    }

    private async Task OnpageSizeCountryAttendanceIndexChanged(int newpageSizeCountryAttendance)
    {
        pageSizeCountryAttendance = newpageSizeCountryAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeCountryAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeCountryAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeCountryAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetCountryAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeCountryAttendance,
            SearchTerm = searchTermCountryAttendance,
        });
        CountryAttendances = result.Item1;
        totalCountCountryAttendance = result.PageCount;
        activePageIndexCountryAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching




      #region Searching

  private int pageSize { get; set; } = 10;
  private int totalCount = 0;
  private int activePageIndex { get; set; } = 0;
  private string searchTerm { get; set; } = string.Empty;

  private async Task OnSearchBoxChanged(string searchText)
  {
      searchTerm = searchText;
      await LoadData(0, pageSize);
  }

  private async Task OnPageSizeIndexChanged(int newPageSize)
  {
      pageSize = newPageSize;
      await LoadData(0, newPageSize);
  }

  private async Task OnPageIndexChanged(int newPageIndex)
  {
      await LoadData(newPageIndex, pageSize);
  }

  private async Task LoadData(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          SelectedDataItems = [];
          var a = await Mediator.Send(new GetGeneralConsultanServicesQuery
          {
              OrderByList =
              [
                  (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
                  (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
                  (x => x.ClassType != null, true)               // ThenByDescending ClassType is not null
              ],
              Predicate = x => x.IsVaccination == true,
              PageIndex = pageIndex,
              PageSize = pageSize,
              SearchTerm = searchTerm,
          });

          GeneralConsultanServices = a.Item1;
          totalCount = a.PageCount;
          activePageIndex = pageIndex;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

  #endregion Searching


   <MyGridPaginate @ref="GridDetail"
                 Data="Countrys"
                 @bind-SelectedDataItems="@SelectedDetailDataItems"
                 EditModelSaving="OnSaveInventoryAdjumentDetail"
                 DataItemDeleting="OnDeleteInventoryAdjumentDetail"
                 EditFormButtonsVisible="false"
                 FocusedRowChanged="GridDetail_FocusedRowChanged"
                 SearchTextChanged="OnSearchBoxChanged"
                 KeyFieldName="Id">


     <ToolbarTemplate>
         <MyDxToolbarBase TItem="CountryDto"
                          Items="@Countrys"
                          Grid="GridDetail"
                          SelectedDataItems="@SelectedDetailDataItems"
                          NewItem_Click="@NewItem_Click"
                          EditItem_Click="@EditItem_Click"
                          DeleteItem_Click="@DeleteItem_Click"
                          Refresh_Click="@(async () => await LoadData())"
                          IsImport="UserAccessCRUID.IsImport"
                          VisibleNew="UserAccessCRUID.IsCreate"
                          VisibleEdit="UserAccessCRUID.IsUpdate"
                          VisibleDelete="UserAccessCRUID.IsDelete" />
     </ToolbarTemplate>


     <Columns>
         <DxGridSelectionColumn Width="15px" />
         <DxGridDataColumn FieldName="Country.Name" Caption="Country"></DxGridDataColumn>
         <DxGridDataColumn FieldName="TeoriticalQty" Caption="Teoritical Qty" />
         <DxGridDataColumn FieldName="RealQty" Caption="Real Qty" />
         <DxGridDataColumn FieldName="Difference" Caption="Difference" />
         <DxGridDataColumn FieldName="Batch" Caption="Lot Serial Number" />
         <DxGridDataColumn FieldName="ExpiredDate" Caption="Expired Date" SortIndex="0" DisplayFormat="@Helper.DefaultFormatDate" />
         <DxGridDataColumn FieldName="Country.Country.Name" Caption="Country" />
     </Columns>
     <EditFormTemplate Context="EditFormContext">
         @{
             if (EditFormContext.DataItem is null)
             {
                 FormCountry = (CountryDto)EditFormContext.EditModel;
             }
             var IsBatch = Countrys.FirstOrDefault(x => x.Id == FormCountry.CountryId)?.TraceAbility ?? false;

             ActiveButton = FormCountry.CountryId is null ||
             string.IsNullOrWhiteSpace(FormCountry.Batch) && IsBatch ||
             FormCountry.ExpiredDate is null ||
             FormCountry.CountryId is null;
         }
         <div class="row w-100">
             <DxFormLayout CssClass="w-100">
                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Country" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox Data="@Countrys"
                                     @bind-Value="@FormCountry.CountryId"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     NullText="Select Country..."
                                     TextFieldName="Name"
                                     ReadOnly="@(FormCountry.Id != 0)"
                                     ValueFieldName="Id"
                                     SelectedItemChanged="@(async (CountryDto freq) => await OnSelectCountry(freq))"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     ShowValidationIcon="true" />
                         <ValidationMessage For="@(()=> FormCountry.CountryId)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem Caption="Batch" Enabled="FormCountry.Id == 0" Visible="IsBatch" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <MyDxComboBox Data="@Batch"
                                       ReadOnly="@(FormCountry.Id != 0)"
                                       NullText="Select Batch..."
                                       AllowUserInput="true"
                                       @bind-Value="@FormCountry.Batch"
                                       @bind-Text="@FormCountry.Batch"
                                       SelectedItemChanged="@((string a)=> SelectedBatch(a))" />
                         <ValidationMessage For="@(() => FormCountry.Batch)" />

                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Teoritical Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     ReadOnly
                                     MinValue="0"
                                     @bind-Value="@FormCountry.TeoriticalQty"
                                     NullText="Teoritical Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormCountry.TeoriticalQty)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Real Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     MinValue="0"
                                     @bind-Value="@FormCountry.RealQty"
                                     NullText="Real Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormCountry.RealQty)"   />
                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Expired Date" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxDateEdit ShowValidationIcon="true"
                                     ReadOnly="@(FormCountry.Id != 0)"
                                     DisplayFormat="@Helper.DefaultFormatDate"
                                     @bind-Date="@FormCountry.ExpiredDate"
                                     NullText="Expired Date">
                         </DxDateEdit>
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption required-caption" Caption="Country" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox ShowValidationIcon="true" Data="@Countrys"
                                     NullText="Country"
                                     ReadOnly="@(FormCountry.Id != 0)"
                                     TextFieldName="Name"
                                     ValueFieldName="Id"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     @bind-Value="FormCountry.CountryId">
                         </DxComboBox>
                         <ValidationMessage For="@(() => FormCountry.CountryId)" />
                     </DxFormLayoutItem>
                 </div>
             </DxFormLayout>

             <div class="col-md-12 d-flex justify-content-end">
                 <DxButton Enabled="@(!ActiveButton)" RenderStyle="ButtonRenderStyle.Primary" RenderStyleMode="@ButtonRenderStyleMode.Contained" IconCssClass="fa-solid fa-sd-card" Text="Save" SubmitFormOnClick="true"></DxButton>
                 <DxButton RenderStyle="ButtonRenderStyle.Danger" RenderStyleMode="@ButtonRenderStyleMode.Contained" IconCssClass="fa-solid fa-xmark" Text="Cancel" Click="@(() => GridDetail.CancelEditAsync())"></DxButton>
             </div>

         </div>
     </EditFormTemplate>
 </MyGridPaginate>




 NEW COMBOBOX VIRAL 2024

Country

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Country" ColSpanMd="12">
    <DxComboBox Data="Countrys"
                AllowUserInput="true"
                NullText="Select Country"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputCountry"
                @bind-Value="a.CountryId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(Country.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(Country.Code)" Caption="Code" />
        </Columns>
    </DxComboBox>
    <ValidationMessage For="@(()=>a.CountryId)" />
</DxFormLayoutItem>

#region ComboBox Country
 
private CancellationTokenSource? _ctsCountry;
private async Task OnInputCountry(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;
            
        _ctsCountry?.Cancel();
        _ctsCountry?.Dispose();
        _ctsCountry = new CancellationTokenSource();
            
        await Task.Delay(700, _ctsCountry.Token);
            
        await LoadCountry(e.Value?.ToString() ?? "");
    } 
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsCountry?.Dispose();
        _ctsCountry = null;
    } 
}

private async Task LoadCountry(string? e = "", long? id = null)
{
    try
    {
        PanelVisible = true;
        Countrys = await Mediator.QueryGetComboBox<Country, CountryDto>(e, x => x.Id == id);
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}

#endregion

await LoadCountry(id:  a.CountryId);

Country
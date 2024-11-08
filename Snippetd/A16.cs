public class AwarenessEduCategoryCommand
 {
     #region GET

    public class GetSingleAwarenessEduCategoryQuery : IRequest<AwarenessEduCategoryDto>
    {
        public List<Expression<Func<AwarenessEduCategory, object>>> Includes { get; set; }
        public Expression<Func<AwarenessEduCategory, bool>> Predicate { get; set; }
        public Expression<Func<AwarenessEduCategory, AwarenessEduCategory>> Select { get; set; }

        public List<(Expression<Func<AwarenessEduCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetAwarenessEduCategoryQuery : IRequest<(List<AwarenessEduCategoryDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<AwarenessEduCategory, object>>> Includes { get; set; }
        public Expression<Func<AwarenessEduCategory, bool>> Predicate { get; set; }
        public Expression<Func<AwarenessEduCategory, AwarenessEduCategory>> Select { get; set; }

        public List<(Expression<Func<AwarenessEduCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateAwarenessEduCategory(Expression<Func<AwarenessEduCategory, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<AwarenessEduCategory, bool>> Predicate { get; } = predicate!;
     }

     public class BulkValidateAwarenessEduCategory(List<AwarenessEduCategoryDto> AwarenessEduCategorysToValidate) : IRequest<List<AwarenessEduCategoryDto>>
     {
         public List<AwarenessEduCategoryDto> AwarenessEduCategorysToValidate { get; } = AwarenessEduCategorysToValidate;
     }

     #endregion GET

     #region CREATE

     public class CreateAwarenessEduCategoryRequest(AwarenessEduCategoryDto AwarenessEduCategoryDto) : IRequest<AwarenessEduCategoryDto>
     {
         public AwarenessEduCategoryDto AwarenessEduCategoryDto { get; set; } = AwarenessEduCategoryDto;
     }

     public class CreateListAwarenessEduCategoryRequest(List<AwarenessEduCategoryDto> AwarenessEduCategoryDtos) : IRequest<List<AwarenessEduCategoryDto>>
     {
         public List<AwarenessEduCategoryDto> AwarenessEduCategoryDtos { get; set; } = AwarenessEduCategoryDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateAwarenessEduCategoryRequest(AwarenessEduCategoryDto AwarenessEduCategoryDto) : IRequest<AwarenessEduCategoryDto>
     {
         public AwarenessEduCategoryDto AwarenessEduCategoryDto { get; set; } = AwarenessEduCategoryDto;
     }

     public class UpdateListAwarenessEduCategoryRequest(List<AwarenessEduCategoryDto> AwarenessEduCategoryDtos) : IRequest<List<AwarenessEduCategoryDto>>
     {
         public List<AwarenessEduCategoryDto> AwarenessEduCategoryDtos { get; set; } = AwarenessEduCategoryDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteAwarenessEduCategoryRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateAwarenessEduCategoryQuery, List<AwarenessEduCategoryDto>>,
  
IRequestHandler<GetAwarenessEduCategoryQuery, (List<AwarenessEduCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleAwarenessEduCategoryQuery, AwarenessEduCategoryDto>,
public class AwarenessEduCategoryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetAwarenessEduCategoryQuery, (List<AwarenessEduCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleAwarenessEduCategoryQuery, AwarenessEduCategoryDto>, 
     IRequestHandler<ValidateAwarenessEduCategory, bool>,
     IRequestHandler<CreateAwarenessEduCategoryRequest, AwarenessEduCategoryDto>,
     IRequestHandler<BulkValidateAwarenessEduCategory, List<AwarenessEduCategoryDto>>,
     IRequestHandler<CreateListAwarenessEduCategoryRequest, List<AwarenessEduCategoryDto>>,
     IRequestHandler<UpdateAwarenessEduCategoryRequest, AwarenessEduCategoryDto>,
     IRequestHandler<UpdateListAwarenessEduCategoryRequest, List<AwarenessEduCategoryDto>>,
     IRequestHandler<DeleteAwarenessEduCategoryRequest, bool>
{
    #region GET
    public async Task<List<AwarenessEduCategoryDto>> Handle(BulkValidateAwarenessEduCategory request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.AwarenessEduCategorysToValidate;

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
    public async Task<bool> Handle(ValidateAwarenessEduCategory request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<AwarenessEduCategory>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<AwarenessEduCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetAwarenessEduCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<AwarenessEduCategory>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<AwarenessEduCategory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<AwarenessEduCategory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.AwarenessEduCategory.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new AwarenessEduCategory
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

                return (pagedItems.Adapt<List<AwarenessEduCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<AwarenessEduCategoryDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<AwarenessEduCategoryDto> Handle(GetSingleAwarenessEduCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<AwarenessEduCategory>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<AwarenessEduCategory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<AwarenessEduCategory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.AwarenessEduCategory.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new AwarenessEduCategory
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<AwarenessEduCategoryDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<AwarenessEduCategoryDto> Handle(CreateAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<AwarenessEduCategory>().AddAsync(request.AwarenessEduCategoryDto.Adapt<CreateUpdateAwarenessEduCategoryDto>().Adapt<AwarenessEduCategory>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<AwarenessEduCategoryDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<AwarenessEduCategoryDto>> Handle(CreateListAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<AwarenessEduCategory>().AddAsync(request.AwarenessEduCategoryDtos.Adapt<List<AwarenessEduCategory>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<AwarenessEduCategoryDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<AwarenessEduCategoryDto> Handle(UpdateAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<AwarenessEduCategory>().UpdateAsync(request.AwarenessEduCategoryDto.Adapt<AwarenessEduCategoryDto>().Adapt<AwarenessEduCategory>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<AwarenessEduCategoryDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<AwarenessEduCategoryDto>> Handle(UpdateListAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<AwarenessEduCategory>().UpdateAsync(request.AwarenessEduCategoryDtos.Adapt<List<AwarenessEduCategory>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<AwarenessEduCategoryDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<AwarenessEduCategory>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<AwarenessEduCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateAwarenessEduCategoryQuery(List<AwarenessEduCategoryDto> AwarenessEduCategorysToValidate) : IRequest<List<AwarenessEduCategoryDto>>
{
    public List<AwarenessEduCategoryDto> AwarenessEduCategorysToValidate { get; } = AwarenessEduCategorysToValidate;
}a


IRequestHandler<BulkValidateAwarenessEduCategoryQuery, List<AwarenessEduCategoryDto>>,
  
IRequestHandler<GetAwarenessEduCategoryQuery, (List<AwarenessEduCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleAwarenessEduCategoryQuery, AwarenessEduCategoryDto>,



 var a = await Mediator.Send(new GetAwarenessEduCategorysQuery
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

var patienss = (await Mediator.Send(new GetSingleAwarenessEduCategoryQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new AwarenessEduCategory
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
    var result = await Mediator.Send(new GetAwarenessEduCategoryQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    AwarenessEduCategorys = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastAwarenessEduCategory);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetAwarenessEduCategoryQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refAwarenessEduCategoryComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 AwarenessEduCategorys = result.Item1;
 totalCountAwarenessEduCategory = result.PageCount;

 AwarenessEduCategorys = (await Mediator.Send(new GetAwarenessEduCategoryQuery
 {
     Predicate = x => x.Id == AwarenessEduCategoryForm.IdCardAwarenessEduCategoryId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleAwarenessEduCategorysQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new AwarenessEduCategory
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new AwarenessEduCategory
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new AwarenessEduCategory
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


#region ComboboxAwarenessEduCategory

 private DxComboBox<AwarenessEduCategoryDto, long?> refAwarenessEduCategoryComboBox { get; set; }
 private int AwarenessEduCategoryComboBoxIndex { get; set; } = 0;
 private int totalCountAwarenessEduCategory = 0;

 private async Task OnSearchAwarenessEduCategory()
 {
     await LoadDataAwarenessEduCategory();
 }

 private async Task OnSearchAwarenessEduCategoryIndexIncrement()
 {
     if (AwarenessEduCategoryComboBoxIndex < (totalCountAwarenessEduCategory - 1))
     {
         AwarenessEduCategoryComboBoxIndex++;
         await LoadDataAwarenessEduCategory(AwarenessEduCategoryComboBoxIndex, 10);
     }
 }

 private async Task OnSearchAwarenessEduCategoryIndexDecrement()
 {
     if (AwarenessEduCategoryComboBoxIndex > 0)
     {
         AwarenessEduCategoryComboBoxIndex--;
         await LoadDataAwarenessEduCategory(AwarenessEduCategoryComboBoxIndex, 10);
     }
 }

 private async Task OnInputAwarenessEduCategoryChanged(string e)
 {
     AwarenessEduCategoryComboBoxIndex = 0;
     await LoadDataAwarenessEduCategory();
 }

 
  private async Task LoadDataAwarenessEduCategory(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetAwarenessEduCategoryQuery
          {
              SearchTerm = refAwarenessEduCategoryComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          AwarenessEduCategorys = result.Item1;
          totalCountAwarenessEduCategory = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastAwarenessEduCategory);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxAwarenessEduCategory

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="AwarenessEduCategory" ColSpanMd="12">
    <MyDxComboBox Data="@AwarenessEduCategorys"
                  NullText="Select AwarenessEduCategory"
                  @ref="refAwarenessEduCategoryComboBox"
                  @bind-Value="@a.AwarenessEduCategoryId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputAwarenessEduCategoryChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchAwarenessEduCategoryIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchAwarenessEduCategory"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchAwarenessEduCategoryIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(AwarenessEduCategoryDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="AwarenessEduCategory.Name" Caption="AwarenessEduCategory" />
            <DxListEditorColumn FieldName="@nameof(AwarenessEduCategoryDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.AwarenessEduCategoryId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<AwarenessEduCategory>().AddAsync(request.AwarenessEduCategoryDto.Adapt<CreateUpdateAwarenessEduCategoryDto>().Adapt<AwarenessEduCategory>());
var result = await _unitOfWork.Repository<AwarenessEduCategory>().AddAsync(request.AwarenessEduCategoryDtos.Adapt<List<CreateUpdateAwarenessEduCategoryDto>>().Adapt<List<AwarenessEduCategory>>()); 

var result = await _unitOfWork.Repository<AwarenessEduCategory>().UpdateAsync(request.AwarenessEduCategoryDto.Adapt<CreateUpdateAwarenessEduCategoryDto>().Adapt<AwarenessEduCategory>());  
var result = await _unitOfWork.Repository<AwarenessEduCategory>().UpdateAsync(request.AwarenessEduCategoryDtos.Adapt<List<CreateUpdateAwarenessEduCategoryDto>>().Adapt<List<AwarenessEduCategory>>());

list3 = (await Mediator.Send(new GetAwarenessEduCategoryQuery
{
    Predicate = x => AwarenessEduCategoryNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new AwarenessEduCategory
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeAwarenessEduCategoryAttendance { get; set; } = 10;
    private int totalCountAwarenessEduCategoryAttendance = 0;
    private int activePageIndexAwarenessEduCategoryAttendance { get; set; } = 0;
    private string searchTermAwarenessEduCategoryAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedAwarenessEduCategoryAttendance(string searchText)
    {
        searchTermAwarenessEduCategoryAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeAwarenessEduCategoryAttendance);
    }

    private async Task OnpageSizeAwarenessEduCategoryAttendanceIndexChanged(int newpageSizeAwarenessEduCategoryAttendance)
    {
        pageSizeAwarenessEduCategoryAttendance = newpageSizeAwarenessEduCategoryAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeAwarenessEduCategoryAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeAwarenessEduCategoryAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeAwarenessEduCategoryAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetAwarenessEduCategoryAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeAwarenessEduCategoryAttendance,
            SearchTerm = searchTermAwarenessEduCategoryAttendance,
        });
        AwarenessEduCategoryAttendances = result.Item1;
        totalCountAwarenessEduCategoryAttendance = result.PageCount;
        activePageIndexAwarenessEduCategoryAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastAwarenessEduCategory);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching

        <div class="row">
        <DxFormLayout>
            <div class="col-md-9">
                <DxFormLayoutItem>
                    <DxPager PageCount="totalCount"
                             ActivePageIndexChanged="OnPageIndexChanged"
                             ActivePageIndex="activePageIndex"
                             VisibleNumericButtonCount="10"
                             SizeMode="SizeMode.Medium"
                             NavigationMode="PagerNavigationMode.Auto">
                    </DxPager>
                </DxFormLayoutItem>
            </div>
            <div class="col-md-3 d-flex justify-content-end">
                <DxFormLayoutItem Caption="Page Size:">
                    <MyDxComboBox Data="(new[] { 10, 25, 50, 100 })"
                                  NullText="Select Page Size"
                                  ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Never"
                                  SelectedItemChanged="((int e ) => OnPageSizeIndexChanged(e))"
                                  @bind-Value="pageSize">
                    </MyDxComboBox>
                </DxFormLayoutItem>
            </div>
        </DxFormLayout>
    </div>
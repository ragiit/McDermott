public class CompanyCommand
 {
     #region GET

    public class GetSingleCompanyQuery : IRequest<CompanyDto>
    {
        public List<Expression<Func<Company, object>>> Includes { get; set; }
        public Expression<Func<Company, bool>> Predicate { get; set; }
        public Expression<Func<Company, Company>> Select { get; set; }

        public List<(Expression<Func<Company, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetCompanyQuery : IRequest<(List<CompanyDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<Company, object>>> Includes { get; set; }
        public Expression<Func<Company, bool>> Predicate { get; set; }
        public Expression<Func<Company, Company>> Select { get; set; }

        public List<(Expression<Func<Company, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateCompany(Expression<Func<Company, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<Company, bool>> Predicate { get; } = predicate!;
     }

     public class BulkValidateCompany(List<CompanyDto> CompanysToValidate) : IRequest<List<CompanyDto>>
     {
         public List<CompanyDto> CompanysToValidate { get; } = CompanysToValidate;
     }

     #endregion GET

     #region CREATE

     public class CreateCompanyRequest(CompanyDto CompanyDto) : IRequest<CompanyDto>
     {
         public CompanyDto CompanyDto { get; set; } = CompanyDto;
     }

     public class CreateListCompanyRequest(List<CompanyDto> CompanyDtos) : IRequest<List<CompanyDto>>
     {
         public List<CompanyDto> CompanyDtos { get; set; } = CompanyDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateCompanyRequest(CompanyDto CompanyDto) : IRequest<CompanyDto>
     {
         public CompanyDto CompanyDto { get; set; } = CompanyDto;
     }

     public class UpdateListCompanyRequest(List<CompanyDto> CompanyDtos) : IRequest<List<CompanyDto>>
     {
         public List<CompanyDto> CompanyDtos { get; set; } = CompanyDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteCompanyRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateCompanyQuery, List<CompanyDto>>,
  
IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleCompanyQuery, CompanyDto>,
public class CompanyHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleCompanyQuery, CompanyDto>, 
     IRequestHandler<ValidateCompany, bool>,
     IRequestHandler<CreateCompanyRequest, CompanyDto>,
     IRequestHandler<BulkValidateCompany, List<CompanyDto>>,
     IRequestHandler<CreateListCompanyRequest, List<CompanyDto>>,
     IRequestHandler<UpdateCompanyRequest, CompanyDto>,
     IRequestHandler<UpdateListCompanyRequest, List<CompanyDto>>,
     IRequestHandler<DeleteCompanyRequest, bool>
{
    #region GET
    public async Task<List<CompanyDto>> Handle(BulkValidateCompany request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.CompanysToValidate;

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
    public async Task<bool> Handle(ValidateCompany request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Company>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<CompanyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Company>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<Company>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Company>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Company
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

                return (pagedItems.Adapt<List<CompanyDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<CompanyDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<CompanyDto> Handle(GetSingleCompanyQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Company>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<Company>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Company>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Company
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<CompanyDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<CompanyDto> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDto.Adapt<CreateUpdateCompanyDto>().Adapt<Company>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<CompanyDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<CompanyDto>> Handle(CreateListCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDtos.Adapt<List<Company>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<CompanyDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<CompanyDto> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDto.Adapt<CompanyDto>().Adapt<Company>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<CompanyDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<CompanyDto>> Handle(UpdateListCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDtos.Adapt<List<Company>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<CompanyDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<Company>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<Company>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateCompanyQuery(List<CompanyDto> CompanysToValidate) : IRequest<List<CompanyDto>>
{
    public List<CompanyDto> CompanysToValidate { get; } = CompanysToValidate;
}a


IRequestHandler<BulkValidateCompanyQuery, List<CompanyDto>>,
  
IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleCompanyQuery, CompanyDto>,



 var a = await Mediator.Send(new GetCompanysQuery
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

var patienss = (await Mediator.Send(new GetSingleCompanyQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Company
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
    var result = await Mediator.Send(new GetCompanyQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    Companys = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastCompany);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetCompanyQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refCompanyComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 Companys = result.Item1;
 totalCountCompany = result.PageCount;

 Companys = (await Mediator.Send(new GetCompanyQuery
 {
     Predicate = x => x.Id == CompanyForm.IdCardCompanyId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleCompanysQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Company
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Company
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Company
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


#region ComboboxCompany

 private DxComboBox<CompanyDto, long?> refCompanyComboBox { get; set; }
 private List<CompanyDto> Companies { get; set; } = [];
 private int CompanyComboBoxIndex { get; set; } = 0;
 private int totalCountCompany = 0;

 private async Task OnSearchCompany()
 {
     await LoadDataCompany();
 }

 private async Task OnSearchCompanyIndexIncrement()
 {
     if (CompanyComboBoxIndex < (totalCountCompany - 1))
     {
         CompanyComboBoxIndex++;
         await LoadDataCompany(CompanyComboBoxIndex, 10);
     }
 }

 private async Task OnSearchCompanyIndexDecrement()
 {
     if (CompanyComboBoxIndex > 0)
     {
         CompanyComboBoxIndex--;
         await LoadDataCompany(CompanyComboBoxIndex, 10);
     }
 }

 private async Task OnInputCompanyChanged(string e)
 {
     CompanyComboBoxIndex = 0;
     await LoadDataCompany();
 }

 
  private async Task LoadDataCompany(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetCompanyQuery
          {
              SearchTerm = refCompanyComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Companies = result.Item1;
          totalCountCompany = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxCompany

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Company" ColSpanMd="12">
    <MyDxComboBox Data="@Companies"
                  NullText="Select Company"
                  @ref="refCompanyComboBox"
                  @bind-Value="@a.CompanyId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputCompanyChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchCompanyIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchCompany"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchCompanyIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(CompanyDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Company.Name" Caption="Company" />
            <DxListEditorColumn FieldName="@nameof(CompanyDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.CompanyId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDto.Adapt<CreateUpdateCompanyDto>().Adapt<Company>());
var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDtos.Adapt<List<CreateUpdateCompanyDto>>().Adapt<List<Company>>()); 

var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDto.Adapt<CreateUpdateCompanyDto>().Adapt<Company>());  
var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDtos.Adapt<List<CreateUpdateCompanyDto>>().Adapt<List<Company>>());

list3 = (await Mediator.Send(new GetCompanyQuery
{
    Predicate = x => CompanyNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new Company
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeCompanyAttendance { get; set; } = 10;
    private int totalCountCompanyAttendance = 0;
    private int activePageIndexCompanyAttendance { get; set; } = 0;
    private string searchTermCompanyAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedCompanyAttendance(string searchText)
    {
        searchTermCompanyAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeCompanyAttendance);
    }

    private async Task OnpageSizeCompanyAttendanceIndexChanged(int newpageSizeCompanyAttendance)
    {
        pageSizeCompanyAttendance = newpageSizeCompanyAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeCompanyAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeCompanyAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeCompanyAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetCompanyAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeCompanyAttendance,
            SearchTerm = searchTermCompanyAttendance,
        });
        CompanyAttendances = result.Item1;
        totalCountCompanyAttendance = result.PageCount;
        activePageIndexCompanyAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastCompany);
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
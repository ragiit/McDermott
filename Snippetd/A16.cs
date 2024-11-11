public class BpjsWebServiceTemporaryCommand
 {
     #region GET

    public class GetSingleBpjsWebServiceTemporaryQuery : IRequest<BpjsWebServiceTemporaryDto>
    {
        public List<Expression<Func<BpjsWebServiceTemporary, object>>> Includes { get; set; }
        public Expression<Func<BpjsWebServiceTemporary, bool>> Predicate { get; set; }
        public Expression<Func<BpjsWebServiceTemporary, BpjsWebServiceTemporary>> Select { get; set; }

        public List<(Expression<Func<BpjsWebServiceTemporary, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetBpjsWebServiceTemporaryQuery : IRequest<(List<BpjsWebServiceTemporaryDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<BpjsWebServiceTemporary, object>>> Includes { get; set; }
        public Expression<Func<BpjsWebServiceTemporary, bool>> Predicate { get; set; }
        public Expression<Func<BpjsWebServiceTemporary, BpjsWebServiceTemporary>> Select { get; set; }

        public List<(Expression<Func<BpjsWebServiceTemporary, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateBpjsWebServiceTemporary(Expression<Func<BpjsWebServiceTemporary, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<BpjsWebServiceTemporary, bool>> Predicate { get; } = predicate!;
     }

     public class BulkValidateBpjsWebServiceTemporary(List<BpjsWebServiceTemporaryDto> BpjsWebServiceTemporarysToValidate) : IRequest<List<BpjsWebServiceTemporaryDto>>
     {
         public List<BpjsWebServiceTemporaryDto> BpjsWebServiceTemporarysToValidate { get; } = BpjsWebServiceTemporarysToValidate;
     }

     #endregion GET

     #region CREATE

     public class CreateBpjsWebServiceTemporaryRequest(BpjsWebServiceTemporaryDto BpjsWebServiceTemporaryDto) : IRequest<BpjsWebServiceTemporaryDto>
     {
         public BpjsWebServiceTemporaryDto BpjsWebServiceTemporaryDto { get; set; } = BpjsWebServiceTemporaryDto;
     }

     public class CreateListBpjsWebServiceTemporaryRequest(List<BpjsWebServiceTemporaryDto> BpjsWebServiceTemporaryDtos) : IRequest<List<BpjsWebServiceTemporaryDto>>
     {
         public List<BpjsWebServiceTemporaryDto> BpjsWebServiceTemporaryDtos { get; set; } = BpjsWebServiceTemporaryDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateBpjsWebServiceTemporaryRequest(BpjsWebServiceTemporaryDto BpjsWebServiceTemporaryDto) : IRequest<BpjsWebServiceTemporaryDto>
     {
         public BpjsWebServiceTemporaryDto BpjsWebServiceTemporaryDto { get; set; } = BpjsWebServiceTemporaryDto;
     }

     public class UpdateListBpjsWebServiceTemporaryRequest(List<BpjsWebServiceTemporaryDto> BpjsWebServiceTemporaryDtos) : IRequest<List<BpjsWebServiceTemporaryDto>>
     {
         public List<BpjsWebServiceTemporaryDto> BpjsWebServiceTemporaryDtos { get; set; } = BpjsWebServiceTemporaryDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteBpjsWebServiceTemporaryRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateBpjsWebServiceTemporaryQuery, List<BpjsWebServiceTemporaryDto>>,
  
IRequestHandler<GetBpjsWebServiceTemporaryQuery, (List<BpjsWebServiceTemporaryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleBpjsWebServiceTemporaryQuery, BpjsWebServiceTemporaryDto>,
public class BpjsWebServiceTemporaryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetBpjsWebServiceTemporaryQuery, (List<BpjsWebServiceTemporaryDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleBpjsWebServiceTemporaryQuery, BpjsWebServiceTemporaryDto>, 
     IRequestHandler<ValidateBpjsWebServiceTemporary, bool>,
     IRequestHandler<CreateBpjsWebServiceTemporaryRequest, BpjsWebServiceTemporaryDto>,
     IRequestHandler<BulkValidateBpjsWebServiceTemporary, List<BpjsWebServiceTemporaryDto>>,
     IRequestHandler<CreateListBpjsWebServiceTemporaryRequest, List<BpjsWebServiceTemporaryDto>>,
     IRequestHandler<UpdateBpjsWebServiceTemporaryRequest, BpjsWebServiceTemporaryDto>,
     IRequestHandler<UpdateListBpjsWebServiceTemporaryRequest, List<BpjsWebServiceTemporaryDto>>,
     IRequestHandler<DeleteBpjsWebServiceTemporaryRequest, bool>
{
    #region GET
    public async Task<List<BpjsWebServiceTemporaryDto>> Handle(BulkValidateBpjsWebServiceTemporary request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.BpjsWebServiceTemporarysToValidate;

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
    public async Task<bool> Handle(ValidateBpjsWebServiceTemporary request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<BpjsWebServiceTemporary>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<BpjsWebServiceTemporaryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetBpjsWebServiceTemporaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<BpjsWebServiceTemporary>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<BpjsWebServiceTemporary>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<BpjsWebServiceTemporary>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.BpjsWebServiceTemporary.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new BpjsWebServiceTemporary
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

                return (pagedItems.Adapt<List<BpjsWebServiceTemporaryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<BpjsWebServiceTemporaryDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<BpjsWebServiceTemporaryDto> Handle(GetSingleBpjsWebServiceTemporaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<BpjsWebServiceTemporary>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<BpjsWebServiceTemporary>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<BpjsWebServiceTemporary>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.BpjsWebServiceTemporary.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new BpjsWebServiceTemporary
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<BpjsWebServiceTemporaryDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<BpjsWebServiceTemporaryDto> Handle(CreateBpjsWebServiceTemporaryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<BpjsWebServiceTemporary>().AddAsync(request.BpjsWebServiceTemporaryDto.Adapt<CreateUpdateBpjsWebServiceTemporaryDto>().Adapt<BpjsWebServiceTemporary>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsWebServiceTemporaryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<BpjsWebServiceTemporaryDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<BpjsWebServiceTemporaryDto>> Handle(CreateListBpjsWebServiceTemporaryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<BpjsWebServiceTemporary>().AddAsync(request.BpjsWebServiceTemporaryDtos.Adapt<List<BpjsWebServiceTemporary>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsWebServiceTemporaryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<BpjsWebServiceTemporaryDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<BpjsWebServiceTemporaryDto> Handle(UpdateBpjsWebServiceTemporaryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<BpjsWebServiceTemporary>().UpdateAsync(request.BpjsWebServiceTemporaryDto.Adapt<BpjsWebServiceTemporaryDto>().Adapt<BpjsWebServiceTemporary>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsWebServiceTemporaryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<BpjsWebServiceTemporaryDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<BpjsWebServiceTemporaryDto>> Handle(UpdateListBpjsWebServiceTemporaryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<BpjsWebServiceTemporary>().UpdateAsync(request.BpjsWebServiceTemporaryDtos.Adapt<List<BpjsWebServiceTemporary>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsWebServiceTemporaryQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<BpjsWebServiceTemporaryDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteBpjsWebServiceTemporaryRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<BpjsWebServiceTemporary>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<BpjsWebServiceTemporary>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsWebServiceTemporaryQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateBpjsWebServiceTemporaryQuery(List<BpjsWebServiceTemporaryDto> BpjsWebServiceTemporarysToValidate) : IRequest<List<BpjsWebServiceTemporaryDto>>
{
    public List<BpjsWebServiceTemporaryDto> BpjsWebServiceTemporarysToValidate { get; } = BpjsWebServiceTemporarysToValidate;
}a


IRequestHandler<BulkValidateBpjsWebServiceTemporaryQuery, List<BpjsWebServiceTemporaryDto>>,
  
IRequestHandler<GetBpjsWebServiceTemporaryQuery, (List<BpjsWebServiceTemporaryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleBpjsWebServiceTemporaryQuery, BpjsWebServiceTemporaryDto>,



 var a = await Mediator.Send(new GetBpjsWebServiceTemporarysQuery
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

var patienss = (await Mediator.Send(new GetSingleBpjsWebServiceTemporaryQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new BpjsWebServiceTemporary
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
    var result = await Mediator.Send(new GetBpjsWebServiceTemporaryQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    BpjsWebServiceTemporarys = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastBpjsWebServiceTemporary);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetBpjsWebServiceTemporaryQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refBpjsWebServiceTemporaryComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 BpjsWebServiceTemporarys = result.Item1;
 totalCountBpjsWebServiceTemporary = result.PageCount;

 BpjsWebServiceTemporarys = (await Mediator.Send(new GetBpjsWebServiceTemporaryQuery
 {
     Predicate = x => x.Id == BpjsWebServiceTemporaryForm.IdCardBpjsWebServiceTemporaryId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleBpjsWebServiceTemporarysQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new BpjsWebServiceTemporary
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new BpjsWebServiceTemporary
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new BpjsWebServiceTemporary
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


#region ComboboxBpjsWebServiceTemporary

 private DxComboBox<BpjsWebServiceTemporaryDto, long?> refBpjsWebServiceTemporaryComboBox { get; set; }
 private int BpjsWebServiceTemporaryComboBoxIndex { get; set; } = 0;
 private int totalCountBpjsWebServiceTemporary = 0;

 private async Task OnSearchBpjsWebServiceTemporary()
 {
     await LoadDataBpjsWebServiceTemporary();
 }

 private async Task OnSearchBpjsWebServiceTemporaryIndexIncrement()
 {
     if (BpjsWebServiceTemporaryComboBoxIndex < (totalCountBpjsWebServiceTemporary - 1))
     {
         BpjsWebServiceTemporaryComboBoxIndex++;
         await LoadDataBpjsWebServiceTemporary(BpjsWebServiceTemporaryComboBoxIndex, 10);
     }
 }

 private async Task OnSearchBpjsWebServiceTemporaryIndexDecrement()
 {
     if (BpjsWebServiceTemporaryComboBoxIndex > 0)
     {
         BpjsWebServiceTemporaryComboBoxIndex--;
         await LoadDataBpjsWebServiceTemporary(BpjsWebServiceTemporaryComboBoxIndex, 10);
     }
 }

 private async Task OnInputBpjsWebServiceTemporaryChanged(string e)
 {
     BpjsWebServiceTemporaryComboBoxIndex = 0;
     await LoadDataBpjsWebServiceTemporary();
 }

 
  private async Task LoadDataBpjsWebServiceTemporary(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetBpjsWebServiceTemporaryQuery
          {
              SearchTerm = refBpjsWebServiceTemporaryComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          BpjsWebServiceTemporarys = result.Item1;
          totalCountBpjsWebServiceTemporary = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastBpjsWebServiceTemporary);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxBpjsWebServiceTemporary

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="BpjsWebServiceTemporary" ColSpanMd="12">
    <MyDxComboBox Data="@BpjsWebServiceTemporarys"
                  NullText="Select BpjsWebServiceTemporary"
                  @ref="refBpjsWebServiceTemporaryComboBox"
                  @bind-Value="@a.BpjsWebServiceTemporaryId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputBpjsWebServiceTemporaryChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchBpjsWebServiceTemporaryIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchBpjsWebServiceTemporary"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchBpjsWebServiceTemporaryIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(BpjsWebServiceTemporaryDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="BpjsWebServiceTemporary.Name" Caption="BpjsWebServiceTemporary" />
            <DxListEditorColumn FieldName="@nameof(BpjsWebServiceTemporaryDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.BpjsWebServiceTemporaryId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<BpjsWebServiceTemporary>().AddAsync(request.BpjsWebServiceTemporaryDto.Adapt<CreateUpdateBpjsWebServiceTemporaryDto>().Adapt<BpjsWebServiceTemporary>());
var result = await _unitOfWork.Repository<BpjsWebServiceTemporary>().AddAsync(request.BpjsWebServiceTemporaryDtos.Adapt<List<CreateUpdateBpjsWebServiceTemporaryDto>>().Adapt<List<BpjsWebServiceTemporary>>()); 

var result = await _unitOfWork.Repository<BpjsWebServiceTemporary>().UpdateAsync(request.BpjsWebServiceTemporaryDto.Adapt<CreateUpdateBpjsWebServiceTemporaryDto>().Adapt<BpjsWebServiceTemporary>());  
var result = await _unitOfWork.Repository<BpjsWebServiceTemporary>().UpdateAsync(request.BpjsWebServiceTemporaryDtos.Adapt<List<CreateUpdateBpjsWebServiceTemporaryDto>>().Adapt<List<BpjsWebServiceTemporary>>());

list3 = (await Mediator.Send(new GetBpjsWebServiceTemporaryQuery
{
    Predicate = x => BpjsWebServiceTemporaryNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new BpjsWebServiceTemporary
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeBpjsWebServiceTemporaryAttendance { get; set; } = 10;
    private int totalCountBpjsWebServiceTemporaryAttendance = 0;
    private int activePageIndexBpjsWebServiceTemporaryAttendance { get; set; } = 0;
    private string searchTermBpjsWebServiceTemporaryAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedBpjsWebServiceTemporaryAttendance(string searchText)
    {
        searchTermBpjsWebServiceTemporaryAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeBpjsWebServiceTemporaryAttendance);
    }

    private async Task OnpageSizeBpjsWebServiceTemporaryAttendanceIndexChanged(int newpageSizeBpjsWebServiceTemporaryAttendance)
    {
        pageSizeBpjsWebServiceTemporaryAttendance = newpageSizeBpjsWebServiceTemporaryAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeBpjsWebServiceTemporaryAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeBpjsWebServiceTemporaryAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeBpjsWebServiceTemporaryAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetBpjsWebServiceTemporaryAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeBpjsWebServiceTemporaryAttendance,
            SearchTerm = searchTermBpjsWebServiceTemporaryAttendance,
        });
        BpjsWebServiceTemporaryAttendances = result.Item1;
        totalCountBpjsWebServiceTemporaryAttendance = result.PageCount;
        activePageIndexBpjsWebServiceTemporaryAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastBpjsWebServiceTemporary);
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
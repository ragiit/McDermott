public class WellnessProgramSessionCommand
 {
     #region GET

    public class GetSingleWellnessProgramSessionQuery : IRequest<WellnessProgramSessionDto>
    {
        public List<Expression<Func<WellnessProgramSession, object>>> Includes { get; set; }
        public Expression<Func<WellnessProgramSession, bool>> Predicate { get; set; }
        public Expression<Func<WellnessProgramSession, WellnessProgramSession>> Select { get; set; }

        public List<(Expression<Func<WellnessProgramSession, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetWellnessProgramSessionQuery : IRequest<(List<WellnessProgramSessionDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<WellnessProgramSession, object>>> Includes { get; set; }
        public Expression<Func<WellnessProgramSession, bool>> Predicate { get; set; }
        public Expression<Func<WellnessProgramSession, WellnessProgramSession>> Select { get; set; }

        public List<(Expression<Func<WellnessProgramSession, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateWellnessProgramSession(Expression<Func<WellnessProgramSession, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<WellnessProgramSession, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateWellnessProgramSessionRequest(WellnessProgramSessionDto WellnessProgramSessionDto) : IRequest<WellnessProgramSessionDto>
     {
         public WellnessProgramSessionDto WellnessProgramSessionDto { get; set; } = WellnessProgramSessionDto;
     }

     public class BulkValidateWellnessProgramSession(List<WellnessProgramSessionDto> WellnessProgramSessionsToValidate) : IRequest<List<WellnessProgramSessionDto>>
     {
         public List<WellnessProgramSessionDto> WellnessProgramSessionsToValidate { get; } = WellnessProgramSessionsToValidate;
     }

     public class CreateListWellnessProgramSessionRequest(List<WellnessProgramSessionDto> WellnessProgramSessionDtos) : IRequest<List<WellnessProgramSessionDto>>
     {
         public List<WellnessProgramSessionDto> WellnessProgramSessionDtos { get; set; } = WellnessProgramSessionDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateWellnessProgramSessionRequest(WellnessProgramSessionDto WellnessProgramSessionDto) : IRequest<WellnessProgramSessionDto>
     {
         public WellnessProgramSessionDto WellnessProgramSessionDto { get; set; } = WellnessProgramSessionDto;
     }

     public class UpdateListWellnessProgramSessionRequest(List<WellnessProgramSessionDto> WellnessProgramSessionDtos) : IRequest<List<WellnessProgramSessionDto>>
     {
         public List<WellnessProgramSessionDto> WellnessProgramSessionDtos { get; set; } = WellnessProgramSessionDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteWellnessProgramSessionRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateWellnessProgramSessionQuery, List<WellnessProgramSessionDto>>,
  
IRequestHandler<GetWellnessProgramSessionQuery, (List<WellnessProgramSessionDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleWellnessProgramSessionQuery, WellnessProgramSessionDto>,
public class WellnessProgramSessionHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetWellnessProgramSessionQuery, (List<WellnessProgramSessionDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleWellnessProgramSessionQuery, WellnessProgramSessionDto>, IRequestHandler<ValidateWellnessProgramSession, bool>,
     IRequestHandler<CreateWellnessProgramSessionRequest, WellnessProgramSessionDto>,
     IRequestHandler<BulkValidateWellnessProgramSession, List<WellnessProgramSessionDto>>,
     IRequestHandler<CreateListWellnessProgramSessionRequest, List<WellnessProgramSessionDto>>,
     IRequestHandler<UpdateWellnessProgramSessionRequest, WellnessProgramSessionDto>,
     IRequestHandler<UpdateListWellnessProgramSessionRequest, List<WellnessProgramSessionDto>>,
     IRequestHandler<DeleteWellnessProgramSessionRequest, bool>
{
    #region GET
    public async Task<List<WellnessProgramSessionDto>> Handle(BulkValidateWellnessProgramSession request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.WellnessProgramSessionsToValidate;

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
    public async Task<bool> Handle(ValidateWellnessProgramSession request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<WellnessProgramSession>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<WellnessProgramSessionDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetWellnessProgramSessionQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<WellnessProgramSession>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<WellnessProgramSession>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<WellnessProgramSession>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.WellnessProgramSession.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new WellnessProgramSession
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

                return (pagedItems.Adapt<List<WellnessProgramSessionDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<WellnessProgramSessionDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<WellnessProgramSessionDto> Handle(GetSingleWellnessProgramSessionQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<WellnessProgramSession>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<WellnessProgramSession>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<WellnessProgramSession>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.WellnessProgramSession.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new WellnessProgramSession
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<WellnessProgramSessionDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<WellnessProgramSessionDto> Handle(CreateWellnessProgramSessionRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<WellnessProgramSession>().AddAsync(request.WellnessProgramSessionDto.Adapt<CreateUpdateWellnessProgramSessionDto>().Adapt<WellnessProgramSession>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<WellnessProgramSessionDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<WellnessProgramSessionDto>> Handle(CreateListWellnessProgramSessionRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<WellnessProgramSession>().AddAsync(request.WellnessProgramSessionDtos.Adapt<List<WellnessProgramSession>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<WellnessProgramSessionDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<WellnessProgramSessionDto> Handle(UpdateWellnessProgramSessionRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<WellnessProgramSession>().UpdateAsync(request.WellnessProgramSessionDto.Adapt<WellnessProgramSessionDto>().Adapt<WellnessProgramSession>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<WellnessProgramSessionDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<WellnessProgramSessionDto>> Handle(UpdateListWellnessProgramSessionRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<WellnessProgramSession>().UpdateAsync(request.WellnessProgramSessionDtos.Adapt<List<WellnessProgramSession>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<WellnessProgramSessionDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteWellnessProgramSessionRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<WellnessProgramSession>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<WellnessProgramSession>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateWellnessProgramSessionQuery(List<WellnessProgramSessionDto> WellnessProgramSessionsToValidate) : IRequest<List<WellnessProgramSessionDto>>
{
    public List<WellnessProgramSessionDto> WellnessProgramSessionsToValidate { get; } = WellnessProgramSessionsToValidate;
}a


IRequestHandler<BulkValidateWellnessProgramSessionQuery, List<WellnessProgramSessionDto>>,
  
IRequestHandler<GetWellnessProgramSessionQuery, (List<WellnessProgramSessionDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleWellnessProgramSessionQuery, WellnessProgramSessionDto>,



 var a = await Mediator.Send(new GetWellnessProgramSessionsQuery
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

var patienss = (await Mediator.Send(new GetSingleWellnessProgramSessionQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new WellnessProgramSession
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
    var result = await Mediator.Send(new GetWellnessProgramSessionQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    WellnessProgramSessions = result.Item1;
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

 var result = await Mediator.Send(new GetWellnessProgramSessionQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refWellnessProgramSessionComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 WellnessProgramSessions = result.Item1;
 totalCountWellnessProgramSession = result.PageCount;

 WellnessProgramSessions = (await Mediator.Send(new GetWellnessProgramSessionQuery
 {
     Predicate = x => x.Id == WellnessProgramSessionForm.IdCardWellnessProgramSessionId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleWellnessProgramSessionsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new WellnessProgramSession
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new WellnessProgramSession
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new WellnessProgramSession
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


#region ComboboxWellnessProgramSession

 private DxComboBox<WellnessProgramSessionDto, long?> refWellnessProgramSessionComboBox { get; set; }
 private int WellnessProgramSessionComboBoxIndex { get; set; } = 0;
 private int totalCountWellnessProgramSession = 0;

 private async Task OnSearchWellnessProgramSession()
 {
     await LoadDataWellnessProgramSession();
 }

 private async Task OnSearchWellnessProgramSessionIndexIncrement()
 {
     if (WellnessProgramSessionComboBoxIndex < (totalCountWellnessProgramSession - 1))
     {
         WellnessProgramSessionComboBoxIndex++;
         await LoadDataWellnessProgramSession(WellnessProgramSessionComboBoxIndex, 10);
     }
 }

 private async Task OnSearchWellnessProgramSessionIndexDecrement()
 {
     if (WellnessProgramSessionComboBoxIndex > 0)
     {
         WellnessProgramSessionComboBoxIndex--;
         await LoadDataWellnessProgramSession(WellnessProgramSessionComboBoxIndex, 10);
     }
 }

 private async Task OnInputWellnessProgramSessionChanged(string e)
 {
     WellnessProgramSessionComboBoxIndex = 0;
     await LoadDataWellnessProgramSession();
 }

 
  private async Task LoadDataWellnessProgramSession(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetWellnessProgramSessionQuery
          {
              SearchTerm = refWellnessProgramSessionComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          WellnessProgramSessions = result.Item1;
          totalCountWellnessProgramSession = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxWellnessProgramSession

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="WellnessProgramSession" ColSpanMd="12">
    <MyDxComboBox Data="@WellnessProgramSessions"
                  NullText="Select WellnessProgramSession"
                  @ref="refWellnessProgramSessionComboBox"
                  @bind-Value="@a.WellnessProgramSessionId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputWellnessProgramSessionChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchWellnessProgramSessionIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchWellnessProgramSession"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchWellnessProgramSessionIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(WellnessProgramSessionDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="WellnessProgramSession.Name" Caption="WellnessProgramSession" />
            <DxListEditorColumn FieldName="@nameof(WellnessProgramSessionDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.WellnessProgramSessionId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<WellnessProgramSession>().AddAsync(request.WellnessProgramSessionDto.Adapt<CreateUpdateWellnessProgramSessionDto>().Adapt<WellnessProgramSession>());
var result = await _unitOfWork.Repository<WellnessProgramSession>().AddAsync(request.WellnessProgramSessionDtos.Adapt<List<CreateUpdateWellnessProgramSessionDto>>().Adapt<List<WellnessProgramSession>>()); 

var result = await _unitOfWork.Repository<WellnessProgramSession>().UpdateAsync(request.WellnessProgramSessionDto.Adapt<CreateUpdateWellnessProgramSessionDto>().Adapt<WellnessProgramSession>());  
var result = await _unitOfWork.Repository<WellnessProgramSession>().UpdateAsync(request.WellnessProgramSessionDtos.Adapt<List<CreateUpdateWellnessProgramSessionDto>>().Adapt<List<WellnessProgramSession>>());

list3 = (await Mediator.Send(new GetWellnessProgramSessionQuery
{
    Predicate = x => WellnessProgramSessionNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new WellnessProgramSession
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeWellnessProgramSessionAttendance { get; set; } = 10;
    private int totalCountWellnessProgramSessionAttendance = 0;
    private int activePageIndexWellnessProgramSessionAttendance { get; set; } = 0;
    private string searchTermWellnessProgramSessionAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedWellnessProgramSessionAttendance(string searchText)
    {
        searchTermWellnessProgramSessionAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeWellnessProgramSessionAttendance);
    }

    private async Task OnpageSizeWellnessProgramSessionAttendanceIndexChanged(int newpageSizeWellnessProgramSessionAttendance)
    {
        pageSizeWellnessProgramSessionAttendance = newpageSizeWellnessProgramSessionAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeWellnessProgramSessionAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeWellnessProgramSessionAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeWellnessProgramSessionAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetWellnessProgramSessionAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeWellnessProgramSessionAttendance,
            SearchTerm = searchTermWellnessProgramSessionAttendance,
        });
        WellnessProgramSessionAttendances = result.Item1;
        totalCountWellnessProgramSessionAttendance = result.PageCount;
        activePageIndexWellnessProgramSessionAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching
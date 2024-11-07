public class WellnessProgramDetailCommand
 {
     #region GET

    public class GetSingleWellnessProgramDetailQuery : IRequest<WellnessProgramDetailDto>
    {
        public List<Expression<Func<WellnessProgramDetail, object>>> Includes { get; set; }
        public Expression<Func<WellnessProgramDetail, bool>> Predicate { get; set; }
        public Expression<Func<WellnessProgramDetail, WellnessProgramDetail>> Select { get; set; }

        public List<(Expression<Func<WellnessProgramDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetWellnessProgramDetailQuery : IRequest<(List<WellnessProgramDetailDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<WellnessProgramDetail, object>>> Includes { get; set; }
        public Expression<Func<WellnessProgramDetail, bool>> Predicate { get; set; }
        public Expression<Func<WellnessProgramDetail, WellnessProgramDetail>> Select { get; set; }

        public List<(Expression<Func<WellnessProgramDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateWellnessProgramDetail(Expression<Func<WellnessProgramDetail, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<WellnessProgramDetail, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateWellnessProgramDetailRequest(WellnessProgramDetailDto WellnessProgramDetailDto) : IRequest<WellnessProgramDetailDto>
     {
         public WellnessProgramDetailDto WellnessProgramDetailDto { get; set; } = WellnessProgramDetailDto;
     }

     public class BulkValidateWellnessProgramDetail(List<WellnessProgramDetailDto> WellnessProgramDetailsToValidate) : IRequest<List<WellnessProgramDetailDto>>
     {
         public List<WellnessProgramDetailDto> WellnessProgramDetailsToValidate { get; } = WellnessProgramDetailsToValidate;
     }

     public class CreateListWellnessProgramDetailRequest(List<WellnessProgramDetailDto> WellnessProgramDetailDtos) : IRequest<List<WellnessProgramDetailDto>>
     {
         public List<WellnessProgramDetailDto> WellnessProgramDetailDtos { get; set; } = WellnessProgramDetailDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateWellnessProgramDetailRequest(WellnessProgramDetailDto WellnessProgramDetailDto) : IRequest<WellnessProgramDetailDto>
     {
         public WellnessProgramDetailDto WellnessProgramDetailDto { get; set; } = WellnessProgramDetailDto;
     }

     public class UpdateListWellnessProgramDetailRequest(List<WellnessProgramDetailDto> WellnessProgramDetailDtos) : IRequest<List<WellnessProgramDetailDto>>
     {
         public List<WellnessProgramDetailDto> WellnessProgramDetailDtos { get; set; } = WellnessProgramDetailDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteWellnessProgramDetailRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateWellnessProgramDetailQuery, List<WellnessProgramDetailDto>>,
  
IRequestHandler<GetWellnessProgramDetailQuery, (List<WellnessProgramDetailDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleWellnessProgramDetailQuery, WellnessProgramDetailDto>,
public class WellnessProgramDetailHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetWellnessProgramDetailQuery, (List<WellnessProgramDetailDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleWellnessProgramDetailQuery, WellnessProgramDetailDto>, IRequestHandler<ValidateWellnessProgramDetail, bool>,
     IRequestHandler<CreateWellnessProgramDetailRequest, WellnessProgramDetailDto>,
     IRequestHandler<BulkValidateWellnessProgramDetail, List<WellnessProgramDetailDto>>,
     IRequestHandler<CreateListWellnessProgramDetailRequest, List<WellnessProgramDetailDto>>,
     IRequestHandler<UpdateWellnessProgramDetailRequest, WellnessProgramDetailDto>,
     IRequestHandler<UpdateListWellnessProgramDetailRequest, List<WellnessProgramDetailDto>>,
     IRequestHandler<DeleteWellnessProgramDetailRequest, bool>
{
    #region GET
    public async Task<List<WellnessProgramDetailDto>> Handle(BulkValidateWellnessProgramDetail request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.WellnessProgramDetailsToValidate;

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
    public async Task<bool> Handle(ValidateWellnessProgramDetail request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<WellnessProgramDetail>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<WellnessProgramDetailDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetWellnessProgramDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<WellnessProgramDetail>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<WellnessProgramDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<WellnessProgramDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.WellnessProgramDetail.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new WellnessProgramDetail
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

                return (pagedItems.Adapt<List<WellnessProgramDetailDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<WellnessProgramDetailDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<WellnessProgramDetailDto> Handle(GetSingleWellnessProgramDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<WellnessProgramDetail>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<WellnessProgramDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<WellnessProgramDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.WellnessProgramDetail.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new WellnessProgramDetail
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<WellnessProgramDetailDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<WellnessProgramDetailDto> Handle(CreateWellnessProgramDetailRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<WellnessProgramDetail>().AddAsync(request.WellnessProgramDetailDto.Adapt<CreateUpdateWellnessProgramDetailDto>().Adapt<WellnessProgramDetail>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<WellnessProgramDetailDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<WellnessProgramDetailDto>> Handle(CreateListWellnessProgramDetailRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<WellnessProgramDetail>().AddAsync(request.WellnessProgramDetailDtos.Adapt<List<WellnessProgramDetail>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<WellnessProgramDetailDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<WellnessProgramDetailDto> Handle(UpdateWellnessProgramDetailRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<WellnessProgramDetail>().UpdateAsync(request.WellnessProgramDetailDto.Adapt<WellnessProgramDetailDto>().Adapt<WellnessProgramDetail>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<WellnessProgramDetailDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<WellnessProgramDetailDto>> Handle(UpdateListWellnessProgramDetailRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<WellnessProgramDetail>().UpdateAsync(request.WellnessProgramDetailDtos.Adapt<List<WellnessProgramDetail>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<WellnessProgramDetailDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteWellnessProgramDetailRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<WellnessProgramDetail>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<WellnessProgramDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateWellnessProgramDetailQuery(List<WellnessProgramDetailDto> WellnessProgramDetailsToValidate) : IRequest<List<WellnessProgramDetailDto>>
{
    public List<WellnessProgramDetailDto> WellnessProgramDetailsToValidate { get; } = WellnessProgramDetailsToValidate;
}a


IRequestHandler<BulkValidateWellnessProgramDetailQuery, List<WellnessProgramDetailDto>>,
  
IRequestHandler<GetWellnessProgramDetailQuery, (List<WellnessProgramDetailDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleWellnessProgramDetailQuery, WellnessProgramDetailDto>,



 var a = await Mediator.Send(new GetWellnessProgramDetailsQuery
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

var patienss = (await Mediator.Send(new GetSingleWellnessProgramDetailQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new WellnessProgramDetail
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
    var result = await Mediator.Send(new GetWellnessProgramDetailQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    WellnessProgramDetails = result.Item1;
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

 var result = await Mediator.Send(new GetWellnessProgramDetailQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refWellnessProgramDetailComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 WellnessProgramDetails = result.Item1;
 totalCountWellnessProgramDetail = result.PageCount;

 WellnessProgramDetails = (await Mediator.Send(new GetWellnessProgramDetailQuery
 {
     Predicate = x => x.Id == WellnessProgramDetailForm.IdCardWellnessProgramDetailId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleWellnessProgramDetailsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new WellnessProgramDetail
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new WellnessProgramDetail
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new WellnessProgramDetail
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


#region ComboboxWellnessProgramDetail

 private DxComboBox<WellnessProgramDetailDto, long?> refWellnessProgramDetailComboBox { get; set; }
 private int WellnessProgramDetailComboBoxIndex { get; set; } = 0;
 private int totalCountWellnessProgramDetail = 0;

 private async Task OnSearchWellnessProgramDetail()
 {
     await LoadDataWellnessProgramDetail();
 }

 private async Task OnSearchWellnessProgramDetailIndexIncrement()
 {
     if (WellnessProgramDetailComboBoxIndex < (totalCountWellnessProgramDetail - 1))
     {
         WellnessProgramDetailComboBoxIndex++;
         await LoadDataWellnessProgramDetail(WellnessProgramDetailComboBoxIndex, 10);
     }
 }

 private async Task OnSearchWellnessProgramDetailIndexDecrement()
 {
     if (WellnessProgramDetailComboBoxIndex > 0)
     {
         WellnessProgramDetailComboBoxIndex--;
         await LoadDataWellnessProgramDetail(WellnessProgramDetailComboBoxIndex, 10);
     }
 }

 private async Task OnInputWellnessProgramDetailChanged(string e)
 {
     WellnessProgramDetailComboBoxIndex = 0;
     await LoadDataWellnessProgramDetail();
 }

 
  private async Task LoadDataWellnessProgramDetail(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetWellnessProgramDetailQuery
          {
              SearchTerm = refWellnessProgramDetailComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          WellnessProgramDetails = result.Item1;
          totalCountWellnessProgramDetail = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxWellnessProgramDetail

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="WellnessProgramDetail" ColSpanMd="12">
    <MyDxComboBox Data="@WellnessProgramDetails"
                  NullText="Select WellnessProgramDetail"
                  @ref="refWellnessProgramDetailComboBox"
                  @bind-Value="@a.WellnessProgramDetailId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputWellnessProgramDetailChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchWellnessProgramDetailIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchWellnessProgramDetail"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchWellnessProgramDetailIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(WellnessProgramDetailDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="WellnessProgramDetail.Name" Caption="WellnessProgramDetail" />
            <DxListEditorColumn FieldName="@nameof(WellnessProgramDetailDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.WellnessProgramDetailId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<WellnessProgramDetail>().AddAsync(request.WellnessProgramDetailDto.Adapt<CreateUpdateWellnessProgramDetailDto>().Adapt<WellnessProgramDetail>());
var result = await _unitOfWork.Repository<WellnessProgramDetail>().AddAsync(request.WellnessProgramDetailDtos.Adapt<List<CreateUpdateWellnessProgramDetailDto>>().Adapt<List<WellnessProgramDetail>>()); 

var result = await _unitOfWork.Repository<WellnessProgramDetail>().UpdateAsync(request.WellnessProgramDetailDto.Adapt<CreateUpdateWellnessProgramDetailDto>().Adapt<WellnessProgramDetail>());  
var result = await _unitOfWork.Repository<WellnessProgramDetail>().UpdateAsync(request.WellnessProgramDetailDtos.Adapt<List<CreateUpdateWellnessProgramDetailDto>>().Adapt<List<WellnessProgramDetail>>());

list3 = (await Mediator.Send(new GetWellnessProgramDetailQuery
{
    Predicate = x => WellnessProgramDetailNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new WellnessProgramDetail
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeWellnessProgramDetailAttendance { get; set; } = 10;
    private int totalCountWellnessProgramDetailAttendance = 0;
    private int activePageIndexWellnessProgramDetailAttendance { get; set; } = 0;
    private string searchTermWellnessProgramDetailAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedWellnessProgramDetailAttendance(string searchText)
    {
        searchTermWellnessProgramDetailAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeWellnessProgramDetailAttendance);
    }

    private async Task OnpageSizeWellnessProgramDetailAttendanceIndexChanged(int newpageSizeWellnessProgramDetailAttendance)
    {
        pageSizeWellnessProgramDetailAttendance = newpageSizeWellnessProgramDetailAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeWellnessProgramDetailAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeWellnessProgramDetailAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeWellnessProgramDetailAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetWellnessProgramDetailAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeWellnessProgramDetailAttendance,
            SearchTerm = searchTermWellnessProgramDetailAttendance,
        });
        WellnessProgramDetailAttendances = result.Item1;
        totalCountWellnessProgramDetailAttendance = result.PageCount;
        activePageIndexWellnessProgramDetailAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching
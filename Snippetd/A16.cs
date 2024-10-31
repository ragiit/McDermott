public class GeneralCosultanServiceAncCommand
 {
     #region GET

    public class GetSingleGeneralCosultanServiceAncQuery : IRequest<GeneralCosultanServiceAncDto>
    {
        public List<Expression<Func<GeneralCosultanServiceAnc, object>>> Includes { get; set; }
        public Expression<Func<GeneralCosultanServiceAnc, bool>> Predicate { get; set; }
        public Expression<Func<GeneralCosultanServiceAnc, GeneralCosultanServiceAnc>> Select { get; set; }

        public List<(Expression<Func<GeneralCosultanServiceAnc, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetGeneralCosultanServiceAncQuery : IRequest<(List<GeneralCosultanServiceAncDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<GeneralCosultanServiceAnc, object>>> Includes { get; set; }
        public Expression<Func<GeneralCosultanServiceAnc, bool>> Predicate { get; set; }
        public Expression<Func<GeneralCosultanServiceAnc, GeneralCosultanServiceAnc>> Select { get; set; }

        public List<(Expression<Func<GeneralCosultanServiceAnc, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateGeneralCosultanServiceAnc(Expression<Func<GeneralCosultanServiceAnc, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<GeneralCosultanServiceAnc, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateGeneralCosultanServiceAncRequest(GeneralCosultanServiceAncDto GeneralCosultanServiceAncDto) : IRequest<GeneralCosultanServiceAncDto>
     {
         public GeneralCosultanServiceAncDto GeneralCosultanServiceAncDto { get; set; } = GeneralCosultanServiceAncDto;
     }

     public class BulkValidateGeneralCosultanServiceAnc(List<GeneralCosultanServiceAncDto> GeneralCosultanServiceAncsToValidate) : IRequest<List<GeneralCosultanServiceAncDto>>
     {
         public List<GeneralCosultanServiceAncDto> GeneralCosultanServiceAncsToValidate { get; } = GeneralCosultanServiceAncsToValidate;
     }

     public class CreateListGeneralCosultanServiceAncRequest(List<GeneralCosultanServiceAncDto> GeneralCosultanServiceAncDtos) : IRequest<List<GeneralCosultanServiceAncDto>>
     {
         public List<GeneralCosultanServiceAncDto> GeneralCosultanServiceAncDtos { get; set; } = GeneralCosultanServiceAncDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateGeneralCosultanServiceAncRequest(GeneralCosultanServiceAncDto GeneralCosultanServiceAncDto) : IRequest<GeneralCosultanServiceAncDto>
     {
         public GeneralCosultanServiceAncDto GeneralCosultanServiceAncDto { get; set; } = GeneralCosultanServiceAncDto;
     }

     public class UpdateListGeneralCosultanServiceAncRequest(List<GeneralCosultanServiceAncDto> GeneralCosultanServiceAncDtos) : IRequest<List<GeneralCosultanServiceAncDto>>
     {
         public List<GeneralCosultanServiceAncDto> GeneralCosultanServiceAncDtos { get; set; } = GeneralCosultanServiceAncDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteGeneralCosultanServiceAncRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateGeneralCosultanServiceAncQuery, List<GeneralCosultanServiceAncDto>>,
  
IRequestHandler<GetGeneralCosultanServiceAncQuery, (List<GeneralCosultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleGeneralCosultanServiceAncQuery, GeneralCosultanServiceAncDto>,
public class GeneralCosultanServiceAncHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetGeneralCosultanServiceAncQuery, (List<GeneralCosultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleGeneralCosultanServiceAncQuery, GeneralCosultanServiceAncDto>, IRequestHandler<ValidateGeneralCosultanServiceAnc, bool>,
     IRequestHandler<CreateGeneralCosultanServiceAncRequest, GeneralCosultanServiceAncDto>,
     IRequestHandler<BulkValidateGeneralCosultanServiceAnc, List<GeneralCosultanServiceAncDto>>,
     IRequestHandler<CreateListGeneralCosultanServiceAncRequest, List<GeneralCosultanServiceAncDto>>,
     IRequestHandler<UpdateGeneralCosultanServiceAncRequest, GeneralCosultanServiceAncDto>,
     IRequestHandler<UpdateListGeneralCosultanServiceAncRequest, List<GeneralCosultanServiceAncDto>>,
     IRequestHandler<DeleteGeneralCosultanServiceAncRequest, bool>
{
    #region GET
    public async Task<(List<GeneralCosultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralCosultanServiceAncQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<GeneralCosultanServiceAnc>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<GeneralCosultanServiceAnc>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<GeneralCosultanServiceAnc>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.GeneralCosultanServiceAnc.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new GeneralCosultanServiceAnc
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

                return (pagedItems.Adapt<List<GeneralCosultanServiceAncDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralCosultanServiceAncDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<GeneralCosultanServiceAncDto> Handle(GetSingleGeneralCosultanServiceAncQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<GeneralCosultanServiceAnc>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<GeneralCosultanServiceAnc>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<GeneralCosultanServiceAnc>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.GeneralCosultanServiceAnc.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new GeneralCosultanServiceAnc
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralCosultanServiceAncDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }

    public async Task<List<GeneralCosultanServiceAncDto>> Handle(BulkValidateGeneralCosultanServiceAncQuery request, CancellationToken cancellationToken)
    {
        var GeneralCosultanServiceAncDtos = request.GeneralCosultanServiceAncsToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        var GeneralCosultanServiceAncNames = GeneralCosultanServiceAncDtos.Select(x => x.Name).Distinct().ToList();
        var postalCodes = GeneralCosultanServiceAncDtos.Select(x => x.PostalCode).Distinct().ToList();
        var provinceIds = GeneralCosultanServiceAncDtos.Select(x => x.ProvinceId).Distinct().ToList();
        var cityIds = GeneralCosultanServiceAncDtos.Select(x => x.CityId).Distinct().ToList();
        var GeneralCosultanServiceAncIds = GeneralCosultanServiceAncDtos.Select(x => x.GeneralCosultanServiceAncId).Distinct().ToList();

        var existingGeneralCosultanServiceAncs = await _unitOfWork.Repository<GeneralCosultanServiceAnc>()
            .Entities
            .AsNoTracking()
            .Where(v => GeneralCosultanServiceAncNames.Contains(v.Name)
                        && postalCodes.Contains(v.PostalCode)
                        && provinceIds.Contains(v.ProvinceId)
                        && cityIds.Contains(v.CityId)
                        && GeneralCosultanServiceAncIds.Contains(v.GeneralCosultanServiceAncId))
            .ToListAsync(cancellationToken);

        return existingGeneralCosultanServiceAncs.Adapt<List<GeneralCosultanServiceAncDto>>();
    } 
    #endregion GET

     #region CREATE

     public async Task<GeneralCosultanServiceAncDto> Handle(CreateGeneralCosultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralCosultanServiceAnc>().AddAsync(request.GeneralCosultanServiceAncDto.Adapt<CreateUpdateGeneralCosultanServiceAncDto>().Adapt<GeneralCosultanServiceAnc>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralCosultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<GeneralCosultanServiceAncDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<GeneralCosultanServiceAncDto>> Handle(CreateListGeneralCosultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralCosultanServiceAnc>().AddAsync(request.GeneralCosultanServiceAncDtos.Adapt<List<GeneralCosultanServiceAnc>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralCosultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<GeneralCosultanServiceAncDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<GeneralCosultanServiceAncDto> Handle(UpdateGeneralCosultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralCosultanServiceAnc>().UpdateAsync(request.GeneralCosultanServiceAncDto.Adapt<GeneralCosultanServiceAncDto>().Adapt<GeneralCosultanServiceAnc>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralCosultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<GeneralCosultanServiceAncDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<GeneralCosultanServiceAncDto>> Handle(UpdateListGeneralCosultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralCosultanServiceAnc>().UpdateAsync(request.GeneralCosultanServiceAncDtos.Adapt<List<GeneralCosultanServiceAnc>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralCosultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<GeneralCosultanServiceAncDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteGeneralCosultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<GeneralCosultanServiceAnc>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<GeneralCosultanServiceAnc>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralCosultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateGeneralCosultanServiceAncQuery(List<GeneralCosultanServiceAncDto> GeneralCosultanServiceAncsToValidate) : IRequest<List<GeneralCosultanServiceAncDto>>
{
    public List<GeneralCosultanServiceAncDto> GeneralCosultanServiceAncsToValidate { get; } = GeneralCosultanServiceAncsToValidate;
}a


IRequestHandler<BulkValidateGeneralCosultanServiceAncQuery, List<GeneralCosultanServiceAncDto>>,
  
IRequestHandler<GetGeneralCosultanServiceAncQuery, (List<GeneralCosultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleGeneralCosultanServiceAncQuery, GeneralCosultanServiceAncDto>,



 var a = await Mediator.Send(new GetGeneralCosultanServiceAncsQuery
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

var patienss = (await Mediator.Send(new GetSingleGeneralCosultanServiceAncQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new GeneralCosultanServiceAnc
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
    var result = await Mediator.Send(new GetGeneralCosultanServiceAncQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    GeneralCosultanServiceAncs = result.Item1;
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

 var result = await Mediator.Send(new GetGeneralCosultanServiceAncQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refGeneralCosultanServiceAncComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 GeneralCosultanServiceAncs = result.Item1;
 totalCountGeneralCosultanServiceAnc = result.PageCount;

 GeneralCosultanServiceAncs = (await Mediator.Send(new GetGeneralCosultanServiceAncQuery
 {
     Predicate = x => x.Id == UserForm.IdCardGeneralCosultanServiceAncId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleGeneralCosultanServiceAncsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new GeneralCosultanServiceAnc
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new GeneralCosultanServiceAnc
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new GeneralCosultanServiceAnc
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


#region ComboboxGeneralCosultanServiceAnc

 private DxComboBox<GeneralCosultanServiceAncDto, long?> refGeneralCosultanServiceAncComboBox { get; set; }
 private int GeneralCosultanServiceAncComboBoxIndex { get; set; } = 0;
 private int totalCountGeneralCosultanServiceAnc = 0;

 private async Task OnSearchGeneralCosultanServiceAnc()
 {
     await LoadDataGeneralCosultanServiceAnc();
 }

 private async Task OnSearchGeneralCosultanServiceAncIndexIncrement()
 {
     if (GeneralCosultanServiceAncComboBoxIndex < (totalCountGeneralCosultanServiceAnc - 1))
     {
         GeneralCosultanServiceAncComboBoxIndex++;
         await LoadDataGeneralCosultanServiceAnc(GeneralCosultanServiceAncComboBoxIndex, 10);
     }
 }

 private async Task OnSearchGeneralCosultanServiceAncIndexDecrement()
 {
     if (GeneralCosultanServiceAncComboBoxIndex > 0)
     {
         GeneralCosultanServiceAncComboBoxIndex--;
         await LoadDataGeneralCosultanServiceAnc(GeneralCosultanServiceAncComboBoxIndex, 10);
     }
 }

 private async Task OnInputGeneralCosultanServiceAncChanged(string e)
 {
     GeneralCosultanServiceAncComboBoxIndex = 0;
     await LoadDataGeneralCosultanServiceAnc();
 }

 
  private async Task LoadDataGeneralCosultanServiceAnc(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetGeneralCosultanServiceAncQuery
          {
              SearchTerm = refGeneralCosultanServiceAncComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          GeneralCosultanServiceAncs = result.Item1;
          totalCountGeneralCosultanServiceAnc = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxGeneralCosultanServiceAnc

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="GeneralCosultanServiceAnc" ColSpanMd="12">
    <MyDxComboBox Data="@GeneralCosultanServiceAncs"
                  NullText="Select GeneralCosultanServiceAnc"
                  @ref="refGeneralCosultanServiceAncComboBox"
                  @bind-Value="@a.GeneralCosultanServiceAncId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputGeneralCosultanServiceAncChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchGeneralCosultanServiceAncIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchGeneralCosultanServiceAnc"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchGeneralCosultanServiceAncIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(GeneralCosultanServiceAncDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="GeneralCosultanServiceAnc.Name" Caption="GeneralCosultanServiceAnc" />
            <DxListEditorColumn FieldName="@nameof(GeneralCosultanServiceAncDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.GeneralCosultanServiceAncId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<GeneralCosultanServiceAnc>().AddAsync(request.GeneralCosultanServiceAncDto.Adapt<CreateUpdateGeneralCosultanServiceAncDto>().Adapt<GeneralCosultanServiceAnc>());
var result = await _unitOfWork.Repository<GeneralCosultanServiceAnc>().AddAsync(request.GeneralCosultanServiceAncDtos.Adapt<List<CreateUpdateGeneralCosultanServiceAncDto>>().Adapt<List<GeneralCosultanServiceAnc>>()); 

var result = await _unitOfWork.Repository<GeneralCosultanServiceAnc>().UpdateAsync(request.GeneralCosultanServiceAncDto.Adapt<CreateUpdateGeneralCosultanServiceAncDto>().Adapt<GeneralCosultanServiceAnc>());  
var result = await _unitOfWork.Repository<GeneralCosultanServiceAnc>().UpdateAsync(request.GeneralCosultanServiceAncDtos.Adapt<List<CreateUpdateGeneralCosultanServiceAncDto>>().Adapt<List<GeneralCosultanServiceAnc>>());

list3 = (await Mediator.Send(new GetGeneralCosultanServiceAncQuery
{
    Predicate = x => GeneralCosultanServiceAncNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new GeneralCosultanServiceAnc
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


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
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetGeneralCosultanServiceAncQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchTerm = searchTerm,
        });
        GeneralCosultanServiceAncs = result.Item1;
        totalCount = result.PageCount;
        activePageIndex = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching
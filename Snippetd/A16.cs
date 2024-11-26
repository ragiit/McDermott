GeneralConsultanServiceAnc

public class GeneralConsultanServiceAncCommand
 {
     #region GET

    public class GetSingleGeneralConsultanServiceAncQuery : IRequest<GeneralConsultanServiceAncDto>
    {
        public List<Expression<Func<GeneralConsultanServiceAnc, object>>> Includes { get; set; }
        public Expression<Func<GeneralConsultanServiceAnc, bool>> Predicate { get; set; }
        public Expression<Func<GeneralConsultanServiceAnc, GeneralConsultanServiceAnc>> Select { get; set; }

        public List<(Expression<Func<GeneralConsultanServiceAnc, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetGeneralConsultanServiceAncQuery : IRequest<(List<GeneralConsultanServiceAncDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<GeneralConsultanServiceAnc, object>>> Includes { get; set; }
        public Expression<Func<GeneralConsultanServiceAnc, bool>> Predicate { get; set; }
        public Expression<Func<GeneralConsultanServiceAnc, GeneralConsultanServiceAnc>> Select { get; set; }

        public List<(Expression<Func<GeneralConsultanServiceAnc, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateGeneralConsultanServiceAnc(Expression<Func<GeneralConsultanServiceAnc, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<GeneralConsultanServiceAnc, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateGeneralConsultanServiceAncRequest(GeneralConsultanServiceAncDto GeneralConsultanServiceAncDto) : IRequest<GeneralConsultanServiceAncDto>
     {
         public GeneralConsultanServiceAncDto GeneralConsultanServiceAncDto { get; set; } = GeneralConsultanServiceAncDto;
     }

     public class BulkValidateGeneralConsultanServiceAnc(List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncsToValidate) : IRequest<List<GeneralConsultanServiceAncDto>>
     {
         public List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncsToValidate { get; } = GeneralConsultanServiceAncsToValidate;
     }

     public class CreateListGeneralConsultanServiceAncRequest(List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncDtos) : IRequest<List<GeneralConsultanServiceAncDto>>
     {
         public List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncDtos { get; set; } = GeneralConsultanServiceAncDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateGeneralConsultanServiceAncRequest(GeneralConsultanServiceAncDto GeneralConsultanServiceAncDto) : IRequest<GeneralConsultanServiceAncDto>
     {
         public GeneralConsultanServiceAncDto GeneralConsultanServiceAncDto { get; set; } = GeneralConsultanServiceAncDto;
     }

     public class UpdateListGeneralConsultanServiceAncRequest(List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncDtos) : IRequest<List<GeneralConsultanServiceAncDto>>
     {
         public List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncDtos { get; set; } = GeneralConsultanServiceAncDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteGeneralConsultanServiceAncRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateGeneralConsultanServiceAncQuery, List<GeneralConsultanServiceAncDto>>,
  
IRequestHandler<GetGeneralConsultanServiceAncQuery, (List<GeneralConsultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleGeneralConsultanServiceAncQuery, GeneralConsultanServiceAncDto>,
public class GeneralConsultanServiceAncHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetGeneralConsultanServiceAncQuery, (List<GeneralConsultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleGeneralConsultanServiceAncQuery, GeneralConsultanServiceAncDto>, 
     IRequestHandler<ValidateGeneralConsultanServiceAnc, bool>,
     IRequestHandler<CreateGeneralConsultanServiceAncRequest, GeneralConsultanServiceAncDto>,
     IRequestHandler<BulkValidateGeneralConsultanServiceAnc, List<GeneralConsultanServiceAncDto>>,
     IRequestHandler<CreateListGeneralConsultanServiceAncRequest, List<GeneralConsultanServiceAncDto>>,
     IRequestHandler<UpdateGeneralConsultanServiceAncRequest, GeneralConsultanServiceAncDto>,
     IRequestHandler<UpdateListGeneralConsultanServiceAncRequest, List<GeneralConsultanServiceAncDto>>,
     IRequestHandler<DeleteGeneralConsultanServiceAncRequest, bool>
{
    #region GET
    public async Task<List<GeneralConsultanServiceAncDto>> Handle(BulkValidateGeneralConsultanServiceAnc request, CancellationToken cancellationToken)
    {
        var GeneralConsultanServiceAncDtos = request.GeneralConsultanServiceAncsToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var GeneralConsultanServiceAncNames = GeneralConsultanServiceAncDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = GeneralConsultanServiceAncDtos.Select(x => x.Code).Distinct().ToList();

        //var existingGeneralConsultanServiceAncs = await _unitOfWork.Repository<GeneralConsultanServiceAnc>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => GeneralConsultanServiceAncNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingGeneralConsultanServiceAncs.Adapt<List<GeneralConsultanServiceAncDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidateGeneralConsultanServiceAnc request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<GeneralConsultanServiceAnc>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<GeneralConsultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanServiceAncQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<GeneralConsultanServiceAnc>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<GeneralConsultanServiceAnc>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<GeneralConsultanServiceAnc>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.GeneralConsultanServiceAnc.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new GeneralConsultanServiceAnc
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

                return (pagedItems.Adapt<List<GeneralConsultanServiceAncDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanServiceAncDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<GeneralConsultanServiceAncDto> Handle(GetSingleGeneralConsultanServiceAncQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<GeneralConsultanServiceAnc>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<GeneralConsultanServiceAnc>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<GeneralConsultanServiceAnc>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.GeneralConsultanServiceAnc.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new GeneralConsultanServiceAnc
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanServiceAncDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<GeneralConsultanServiceAncDto> Handle(CreateGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().AddAsync(request.GeneralConsultanServiceAncDto.Adapt<CreateUpdateGeneralConsultanServiceAncDto>().Adapt<GeneralConsultanServiceAnc>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<GeneralConsultanServiceAncDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<GeneralConsultanServiceAncDto>> Handle(CreateListGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().AddAsync(request.GeneralConsultanServiceAncDtos.Adapt<List<GeneralConsultanServiceAnc>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<GeneralConsultanServiceAncDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<GeneralConsultanServiceAncDto> Handle(UpdateGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().UpdateAsync(request.GeneralConsultanServiceAncDto.Adapt<GeneralConsultanServiceAncDto>().Adapt<GeneralConsultanServiceAnc>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<GeneralConsultanServiceAncDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<GeneralConsultanServiceAncDto>> Handle(UpdateListGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().UpdateAsync(request.GeneralConsultanServiceAncDtos.Adapt<List<GeneralConsultanServiceAnc>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<GeneralConsultanServiceAncDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<GeneralConsultanServiceAnc>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<GeneralConsultanServiceAnc>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateGeneralConsultanServiceAncQuery(List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncsToValidate) : IRequest<List<GeneralConsultanServiceAncDto>>
{
    public List<GeneralConsultanServiceAncDto> GeneralConsultanServiceAncsToValidate { get; } = GeneralConsultanServiceAncsToValidate;
}a


IRequestHandler<BulkValidateGeneralConsultanServiceAncQuery, List<GeneralConsultanServiceAncDto>>,
  
IRequestHandler<GetGeneralConsultanServiceAncQuery, (List<GeneralConsultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleGeneralConsultanServiceAncQuery, GeneralConsultanServiceAncDto>,



 var a = await Mediator.Send(new GetGeneralConsultanServiceAncsQuery
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

var patienss = (await Mediator.Send(new GetSingleGeneralConsultanServiceAncQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new GeneralConsultanServiceAnc
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
    var result = await Mediator.Send(new GetGeneralConsultanServiceAncQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    GeneralConsultanServiceAncs = result.Item1;
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

 var result = await Mediator.Send(new GetGeneralConsultanServiceAncQuery
 {
     Predicate = x => x.GeneralConsultanServiceAncId == GeneralConsultanServiceAncId,
     SearchTerm = refGeneralConsultanServiceAncComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 GeneralConsultanServiceAncs = result.Item1;
 totalCountGeneralConsultanServiceAnc = result.PageCount;

 GeneralConsultanServiceAncs = (await Mediator.Send(new GetGeneralConsultanServiceAncQuery
 {
     Predicate = x => x.Id == GeneralConsultanServiceAncForm.IdCardGeneralConsultanServiceAncId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleGeneralConsultanServiceAncsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new GeneralConsultanServiceAnc
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new GeneralConsultanServiceAnc
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new GeneralConsultanServiceAnc
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


#region ComboboxGeneralConsultanServiceAnc

 private DxComboBox<GeneralConsultanServiceAncDto, long?> refGeneralConsultanServiceAncComboBox { get; set; }
 private int GeneralConsultanServiceAncComboBoxIndex { get; set; } = 0;
 private int totalCountGeneralConsultanServiceAnc = 0;

 private async Task OnSearchGeneralConsultanServiceAnc()
 {
     await LoadDataGeneralConsultanServiceAnc();
 }

 private async Task OnSearchGeneralConsultanServiceAncIndexIncrement()
 {
     if (GeneralConsultanServiceAncComboBoxIndex < (totalCountGeneralConsultanServiceAnc - 1))
     {
         GeneralConsultanServiceAncComboBoxIndex++;
         await LoadDataGeneralConsultanServiceAnc(GeneralConsultanServiceAncComboBoxIndex, 10);
     }
 }

 private async Task OnSearchGeneralConsultanServiceAncIndexDecrement()
 {
     if (GeneralConsultanServiceAncComboBoxIndex > 0)
     {
         GeneralConsultanServiceAncComboBoxIndex--;
         await LoadDataGeneralConsultanServiceAnc(GeneralConsultanServiceAncComboBoxIndex, 10);
     }
 }

 private async Task OnInputGeneralConsultanServiceAncChanged(string e)
 {
     GeneralConsultanServiceAncComboBoxIndex = 0;
     await LoadDataGeneralConsultanServiceAnc();
 }

 
  private async Task LoadDataGeneralConsultanServiceAnc(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetGeneralConsultanServiceAncQuery
          {
              SearchTerm = refGeneralConsultanServiceAncComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          GeneralConsultanServiceAncs = result.Item1;
          totalCountGeneralConsultanServiceAnc = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxGeneralConsultanServiceAnc

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="GeneralConsultanServiceAnc" ColSpanMd="12">
    <MyDxComboBox Data="@GeneralConsultanServiceAncs"
                  NullText="Select GeneralConsultanServiceAnc"
                  @ref="refGeneralConsultanServiceAncComboBox"
                  @bind-Value="@a.GeneralConsultanServiceAncId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputGeneralConsultanServiceAncChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchGeneralConsultanServiceAncIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchGeneralConsultanServiceAnc"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchGeneralConsultanServiceAncIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(GeneralConsultanServiceAncDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="GeneralConsultanServiceAnc.Name" Caption="GeneralConsultanServiceAnc" />
            <DxListEditorColumn FieldName="@nameof(GeneralConsultanServiceAncDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.GeneralConsultanServiceAncId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().AddAsync(request.GeneralConsultanServiceAncDto.Adapt<CreateUpdateGeneralConsultanServiceAncDto>().Adapt<GeneralConsultanServiceAnc>());
var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().AddAsync(request.GeneralConsultanServiceAncDtos.Adapt<List<CreateUpdateGeneralConsultanServiceAncDto>>().Adapt<List<GeneralConsultanServiceAnc>>()); 

var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().UpdateAsync(request.GeneralConsultanServiceAncDto.Adapt<CreateUpdateGeneralConsultanServiceAncDto>().Adapt<GeneralConsultanServiceAnc>());  
var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().UpdateAsync(request.GeneralConsultanServiceAncDtos.Adapt<List<CreateUpdateGeneralConsultanServiceAncDto>>().Adapt<List<GeneralConsultanServiceAnc>>());

list3 = (await Mediator.Send(new GetGeneralConsultanServiceAncQuery
{
    Predicate = x => GeneralConsultanServiceAncNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new GeneralConsultanServiceAnc
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeGeneralConsultanServiceAnc { get; set; } = 10;
    private int totalCountGeneralConsultanServiceAnc = 0;
    private int activePageIndexGeneralConsultanServiceAnc { get; set; } = 0;
    private string searchTermGeneralConsultanServiceAnc { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedGeneralConsultanServiceAnc(string searchText)
    {
        searchTermGeneralConsultanServiceAnc = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeGeneralConsultanServiceAnc);
    }

    private async Task OnpageSizeGeneralConsultanServiceAncIndexChanged(int newpageSizeGeneralConsultanServiceAnc)
    {
        pageSizeGeneralConsultanServiceAnc = newpageSizeGeneralConsultanServiceAnc;
        await LoadDataOnSearchBoxChanged(0, newpageSizeGeneralConsultanServiceAnc);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeGeneralConsultanServiceAnc);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeGeneralConsultanServiceAnc = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetGeneralConsultanServiceAncQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeGeneralConsultanServiceAnc,
            SearchTerm = searchTermGeneralConsultanServiceAnc,
        });
        GeneralConsultanServiceAncs = result.Item1;
        totalCountGeneralConsultanServiceAnc = result.PageCount;
        activePageIndexGeneralConsultanServiceAnc = pageIndex;
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
                 Data="GeneralConsultanServiceAncs"
                 @bind-SelectedDataItems="@SelectedDetailDataItems"
                 EditModelSaving="OnSaveInventoryAdjumentDetail"
                 DataItemDeleting="OnDeleteInventoryAdjumentDetail"
                 EditFormButtonsVisible="false"
                 FocusedRowChanged="GridDetail_FocusedRowChanged"
                 SearchTextChanged="OnSearchBoxChanged"
                 KeyFieldName="Id">


     <ToolbarTemplate>
         <MyDxToolbarBase TItem="GeneralConsultanServiceAncDto"
                          Items="@GeneralConsultanServiceAncs"
                          Grid="GridDetail"
                          SelectedDataItems="@SelectedDetailDataItems"
                          NewItem_Click="@NewItem_Click"
                          EditItem_Click="@EditItem_Click"
                          DeleteItem_Click="@DeleteItem_Click"
                          Refresh_Click="@(async () => await LoadData())"
                          IsImport="GeneralConsultanServiceAncAccessCRUID.IsImport"
                          VisibleNew="GeneralConsultanServiceAncAccessCRUID.IsCreate"
                          VisibleEdit="GeneralConsultanServiceAncAccessCRUID.IsUpdate"
                          VisibleDelete="GeneralConsultanServiceAncAccessCRUID.IsDelete" />
     </ToolbarTemplate>


     <Columns>
         <DxGridSelectionColumn Width="15px" />
         <DxGridDataColumn FieldName="GeneralConsultanServiceAnc.Name" Caption="GeneralConsultanServiceAnc"></DxGridDataColumn>
         <DxGridDataColumn FieldName="TeoriticalQty" Caption="Teoritical Qty" />
         <DxGridDataColumn FieldName="RealQty" Caption="Real Qty" />
         <DxGridDataColumn FieldName="Difference" Caption="Difference" />
         <DxGridDataColumn FieldName="Batch" Caption="Lot Serial Number" />
         <DxGridDataColumn FieldName="ExpiredDate" Caption="Expired Date" SortIndex="0" DisplayFormat="@Helper.DefaultFormatDate" />
         <DxGridDataColumn FieldName="GeneralConsultanServiceAnc.GeneralConsultanServiceAnc.Name" Caption="GeneralConsultanServiceAnc" />
     </Columns>
     <EditFormTemplate Context="EditFormContext">
         @{
             if (EditFormContext.DataItem is null)
             {
                 FormGeneralConsultanServiceAnc = (GeneralConsultanServiceAncDto)EditFormContext.EditModel;
             }
             var IsBatch = GeneralConsultanServiceAncs.FirstOrDefault(x => x.Id == FormGeneralConsultanServiceAnc.GeneralConsultanServiceAncId)?.TraceAbility ?? false;

             ActiveButton = FormGeneralConsultanServiceAnc.GeneralConsultanServiceAncId is null ||
             string.IsNullOrWhiteSpace(FormGeneralConsultanServiceAnc.Batch) && IsBatch ||
             FormGeneralConsultanServiceAnc.ExpiredDate is null ||
             FormGeneralConsultanServiceAnc.GeneralConsultanServiceAncId is null;
         }
         <div class="row w-100">
             <DxFormLayout CssClass="w-100">
                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="GeneralConsultanServiceAnc" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox Data="@GeneralConsultanServiceAncs"
                                     @bind-Value="@FormGeneralConsultanServiceAnc.GeneralConsultanServiceAncId"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     NullText="Select GeneralConsultanServiceAnc..."
                                     TextFieldName="Name"
                                     ReadOnly="@(FormGeneralConsultanServiceAnc.Id != 0)"
                                     ValueFieldName="Id"
                                     SelectedItemChanged="@(async (GeneralConsultanServiceAncDto freq) => await OnSelectGeneralConsultanServiceAnc(freq))"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     ShowValidationIcon="true" />
                         <ValidationMessage For="@(()=> FormGeneralConsultanServiceAnc.GeneralConsultanServiceAncId)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem Caption="Batch" Enabled="FormGeneralConsultanServiceAnc.Id == 0" Visible="IsBatch" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <MyDxComboBox Data="@Batch"
                                       ReadOnly="@(FormGeneralConsultanServiceAnc.Id != 0)"
                                       NullText="Select Batch..."
                                       AllowGeneralConsultanServiceAncInput="true"
                                       @bind-Value="@FormGeneralConsultanServiceAnc.Batch"
                                       @bind-Text="@FormGeneralConsultanServiceAnc.Batch"
                                       SelectedItemChanged="@((string a)=> SelectedBatch(a))" />
                         <ValidationMessage For="@(() => FormGeneralConsultanServiceAnc.Batch)" />

                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Teoritical Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     ReadOnly
                                     MinValue="0"
                                     @bind-Value="@FormGeneralConsultanServiceAnc.TeoriticalQty"
                                     NullText="Teoritical Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormGeneralConsultanServiceAnc.TeoriticalQty)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Real Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     MinValue="0"
                                     @bind-Value="@FormGeneralConsultanServiceAnc.RealQty"
                                     NullText="Real Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormGeneralConsultanServiceAnc.RealQty)"   />
                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Expired Date" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxDateEdit ShowValidationIcon="true"
                                     ReadOnly="@(FormGeneralConsultanServiceAnc.Id != 0)"
                                     DisplayFormat="@Helper.DefaultFormatDate"
                                     @bind-Date="@FormGeneralConsultanServiceAnc.ExpiredDate"
                                     NullText="Expired Date">
                         </DxDateEdit>
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption required-caption" Caption="GeneralConsultanServiceAnc" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox ShowValidationIcon="true" Data="@GeneralConsultanServiceAncs"
                                     NullText="GeneralConsultanServiceAnc"
                                     ReadOnly="@(FormGeneralConsultanServiceAnc.Id != 0)"
                                     TextFieldName="Name"
                                     ValueFieldName="Id"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     @bind-Value="FormGeneralConsultanServiceAnc.GeneralConsultanServiceAncId">
                         </DxComboBox>
                         <ValidationMessage For="@(() => FormGeneralConsultanServiceAnc.GeneralConsultanServiceAncId)" />
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

GeneralConsultanServiceAnc

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="GeneralConsultanServiceAnc" ColSpanMd="12">
    <DxComboBox Data="GeneralConsultanServiceAncs"
                AllowGeneralConsultanServiceAncInput="true"
                NullText="Select GeneralConsultanServiceAnc"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputGeneralConsultanServiceAnc"
                @bind-Value="a.GeneralConsultanServiceAncId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(GeneralConsultanServiceAnc.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(GeneralConsultanServiceAnc.Code)" Caption="Code" />
        </Columns>
    </DxComboBox>
    <ValidationMessage For="@(()=>a.GeneralConsultanServiceAncId)" />
</DxFormLayoutItem>

#region ComboBox GeneralConsultanServiceAnc
 
private CancellationTokenSource? _ctsGeneralConsultanServiceAnc;
private async Task OnInputGeneralConsultanServiceAnc(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;
            
        _ctsGeneralConsultanServiceAnc?.Cancel();
        _ctsGeneralConsultanServiceAnc?.Dispose();
        _ctsGeneralConsultanServiceAnc = new CancellationTokenSource();
            
        await Task.Delay(700, _ctsGeneralConsultanServiceAnc.Token);
            
        await LoadGeneralConsultanServiceAnc(e.Value?.ToString() ?? "");
    } 
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsGeneralConsultanServiceAnc?.Dispose();
        _ctsGeneralConsultanServiceAnc = null;
    } 
}

 private async Task LoadGeneralConsultanServiceAnc(string? e = "", Expression<Func<GeneralConsultanServiceAnc, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         GeneralConsultanServiceAncs = await Mediator.QueryGetComboBox<GeneralConsultanServiceAnc, GeneralConsultanServiceAncDto>(e, predicate);
         PanelVisible = false;
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastService);
     }
     finally { PanelVisible = false; }
 }

#endregion


// Ini buat di EditItemClick
await LoadGeneralConsultanServiceAnc(id:  a.GeneralConsultanServiceAncId);

GeneralConsultanServiceAnc



VIRAL 2025

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="GeneralConsultanServiceAnc" ColSpanMd="12">
    <MyDxComboBox Data="GeneralConsultanServiceAncs"
                NullText="Select GeneralConsultanServiceAnc"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputGeneralConsultanServiceAnc"
                SelectedItemChanged="((GeneralConsultanServiceAncDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.GeneralConsultanServiceAncId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(GeneralConsultanServiceAnc.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(GeneralConsultanServiceAnc.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.GeneralConsultanServiceAncId)" />
</DxFormLayoutItem>


#region ComboBox GeneralConsultanServiceAnc

    private GeneralConsultanServiceAncDto SelectedGeneralConsultanServiceAnc { get; set; } = new();
    async Task SelectedItemChanged(GeneralConsultanServiceAncDto e)
    {
        if (e is null)
        {
            SelectedGeneralConsultanServiceAnc = new();
            await LoadGeneralConsultanServiceAnc(); 
        }
        else
            SelectedGeneralConsultanServiceAnc = e;
    }

    private CancellationTokenSource? _ctsGeneralConsultanServiceAnc;
    private async Task OnInputGeneralConsultanServiceAnc(ChangeEventArgs e)
    {
        try
        { 
            _ctsGeneralConsultanServiceAnc?.Cancel();
            _ctsGeneralConsultanServiceAnc?.Dispose();
            _ctsGeneralConsultanServiceAnc = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsGeneralConsultanServiceAnc.Token);

            await LoadGeneralConsultanServiceAnc(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsGeneralConsultanServiceAnc?.Dispose();
            _ctsGeneralConsultanServiceAnc = null;
        }
    }

    private async Task LoadGeneralConsultanServiceAnc(string? e = "", Expression<Func<GeneralConsultanServiceAnc, bool>>? predicate = null)
    {
        try
        { 
            GeneralConsultanServiceAncs = await Mediator.QueryGetComboBox<GeneralConsultanServiceAnc, GeneralConsultanServiceAncDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastService);
        }
        finally { PanelVisible = false; }
    }

#endregion
NursingDiagnoses

public class NursingDiagnosesCommand
 {
     #region GET

    public class GetSingleNursingDiagnosesQuery : IRequest<NursingDiagnosesDto>
    {
        public List<Expression<Func<NursingDiagnoses, object>>> Includes { get; set; }
        public Expression<Func<NursingDiagnoses, bool>> Predicate { get; set; }
        public Expression<Func<NursingDiagnoses, NursingDiagnoses>> Select { get; set; }

        public List<(Expression<Func<NursingDiagnoses, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetNursingDiagnosesQuery : IRequest<(List<NursingDiagnosesDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<NursingDiagnoses, object>>> Includes { get; set; }
        public Expression<Func<NursingDiagnoses, bool>> Predicate { get; set; }
        public Expression<Func<NursingDiagnoses, NursingDiagnoses>> Select { get; set; }

        public List<(Expression<Func<NursingDiagnoses, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateNursingDiagnoses(Expression<Func<NursingDiagnoses, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<NursingDiagnoses, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<NursingDiagnosesDto>
     {
         public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
     }

     public class BulkValidateNursingDiagnoses(List<NursingDiagnosesDto> NursingDiagnosessToValidate) : IRequest<List<NursingDiagnosesDto>>
     {
         public List<NursingDiagnosesDto> NursingDiagnosessToValidate { get; } = NursingDiagnosessToValidate;
     }

     public class CreateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos) : IRequest<List<NursingDiagnosesDto>>
     {
         public List<NursingDiagnosesDto> NursingDiagnosesDtos { get; set; } = NursingDiagnosesDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<NursingDiagnosesDto>
     {
         public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
     }

     public class UpdateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos) : IRequest<List<NursingDiagnosesDto>>
     {
         public List<NursingDiagnosesDto> NursingDiagnosesDtos { get; set; } = NursingDiagnosesDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteNursingDiagnosesRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateNursingDiagnosesQuery, List<NursingDiagnosesDto>>,
  
IRequestHandler<GetNursingDiagnosesQuery, (List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleNursingDiagnosesQuery, NursingDiagnosesDto>,
public class NursingDiagnosesHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetNursingDiagnosesQuery, (List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleNursingDiagnosesQuery, NursingDiagnosesDto>, 
     IRequestHandler<ValidateNursingDiagnoses, bool>,
     IRequestHandler<CreateNursingDiagnosesRequest, NursingDiagnosesDto>,
     IRequestHandler<BulkValidateNursingDiagnoses, List<NursingDiagnosesDto>>,
     IRequestHandler<CreateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
     IRequestHandler<UpdateNursingDiagnosesRequest, NursingDiagnosesDto>,
     IRequestHandler<UpdateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
     IRequestHandler<DeleteNursingDiagnosesRequest, bool>
{
    #region GET
    public async Task<List<NursingDiagnosesDto>> Handle(BulkValidateNursingDiagnoses request, CancellationToken cancellationToken)
    {
        var NursingDiagnosesDtos = request.NursingDiagnosessToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var NursingDiagnosesNames = NursingDiagnosesDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = NursingDiagnosesDtos.Select(x => x.Code).Distinct().ToList();

        //var existingNursingDiagnosess = await _unitOfWork.Repository<NursingDiagnoses>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => NursingDiagnosesNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingNursingDiagnosess.Adapt<List<NursingDiagnosesDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidateNursingDiagnoses request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<NursingDiagnoses>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetNursingDiagnosesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<NursingDiagnoses>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<NursingDiagnoses>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<NursingDiagnoses>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.NursingDiagnoses.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new NursingDiagnoses
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

                return (pagedItems.Adapt<List<NursingDiagnosesDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<NursingDiagnosesDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<NursingDiagnosesDto> Handle(GetSingleNursingDiagnosesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<NursingDiagnoses>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<NursingDiagnoses>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<NursingDiagnoses>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.NursingDiagnoses.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new NursingDiagnoses
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<NursingDiagnosesDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<NursingDiagnosesDto> Handle(CreateNursingDiagnosesRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDto.Adapt<CreateUpdateNursingDiagnosesDto>().Adapt<NursingDiagnoses>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<NursingDiagnosesDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<NursingDiagnosesDto>> Handle(CreateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDtos.Adapt<List<NursingDiagnoses>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<NursingDiagnosesDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<NursingDiagnosesDto> Handle(UpdateNursingDiagnosesRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDto.Adapt<NursingDiagnosesDto>().Adapt<NursingDiagnoses>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<NursingDiagnosesDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<NursingDiagnosesDto>> Handle(UpdateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDtos.Adapt<List<NursingDiagnoses>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<NursingDiagnosesDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteNursingDiagnosesRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateNursingDiagnosesQuery(List<NursingDiagnosesDto> NursingDiagnosessToValidate) : IRequest<List<NursingDiagnosesDto>>
{
    public List<NursingDiagnosesDto> NursingDiagnosessToValidate { get; } = NursingDiagnosessToValidate;
}a


IRequestHandler<BulkValidateNursingDiagnosesQuery, List<NursingDiagnosesDto>>,
  
IRequestHandler<GetNursingDiagnosesQuery, (List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleNursingDiagnosesQuery, NursingDiagnosesDto>,



 var a = await Mediator.Send(new GetNursingDiagnosessQuery
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

var patienss = (await Mediator.Send(new GetSingleNursingDiagnosesQuery
{
    Predicate = x => x.Id == data.DiagnosisBPJSIntegrationTempId,
    Select = x => new NursingDiagnoses
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
    var result = await Mediator.Send(new GetNursingDiagnosesQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    NursingDiagnosess = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastNursingDiagnoses);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetNursingDiagnosesQuery
 {
     Predicate = x => x.NursingDiagnosesId == NursingDiagnosesId,
     SearchTerm = refNursingDiagnosesComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 NursingDiagnosess = result.Item1;
 totalCountNursingDiagnoses = result.PageCount;

 NursingDiagnosess = (await Mediator.Send(new GetNursingDiagnosesQuery
 {
     Predicate = x => x.Id == NursingDiagnosesForm.IdCardNursingDiagnosesId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleNursingDiagnosessQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.DiagnosisBPJSIntegrationTemp
    ],
    Select = x => new NursingDiagnoses
    {
        Id = x.Id,
        DiagnosisBPJSIntegrationTempId = x.DiagnosisBPJSIntegrationTempId,
        DiagnosisBPJSIntegrationTemp = new NursingDiagnoses
        {
            DateOfBirth = x.DiagnosisBPJSIntegrationTemp.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new NursingDiagnoses
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


#region ComboboxNursingDiagnoses

 private DxComboBox<NursingDiagnosesDto, long?> refNursingDiagnosesComboBox { get; set; }
 private int NursingDiagnosesComboBoxIndex { get; set; } = 0;
 private int totalCountNursingDiagnoses = 0;

 private async Task OnSearchNursingDiagnoses()
 {
     await LoadDataNursingDiagnoses();
 }

 private async Task OnSearchNursingDiagnosesIndexIncrement()
 {
     if (NursingDiagnosesComboBoxIndex < (totalCountNursingDiagnoses - 1))
     {
         NursingDiagnosesComboBoxIndex++;
         await LoadDataNursingDiagnoses(NursingDiagnosesComboBoxIndex, 10);
     }
 }

 private async Task OnSearchNursingDiagnosesIndexDecrement()
 {
     if (NursingDiagnosesComboBoxIndex > 0)
     {
         NursingDiagnosesComboBoxIndex--;
         await LoadDataNursingDiagnoses(NursingDiagnosesComboBoxIndex, 10);
     }
 }

 private async Task OnInputNursingDiagnosesChanged(string e)
 {
     NursingDiagnosesComboBoxIndex = 0;
     await LoadDataNursingDiagnoses();
 }

 
  private async Task LoadDataNursingDiagnoses(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetNursingDiagnosesQuery
          {
              SearchTerm = refNursingDiagnosesComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          NursingDiagnosess = result.Item1;
          totalCountNursingDiagnoses = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastNursingDiagnoses);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxNursingDiagnoses

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="NursingDiagnoses" ColSpanMd="12">
    <MyDxComboBox Data="@NursingDiagnosess"
                  NullText="Select NursingDiagnoses"
                  @ref="refNursingDiagnosesComboBox"
                  @bind-Value="@a.NursingDiagnosesId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputNursingDiagnosesChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchNursingDiagnosesIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchNursingDiagnoses"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchNursingDiagnosesIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(NursingDiagnosesDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="NursingDiagnoses.Name" Caption="NursingDiagnoses" />
            <DxListEditorColumn FieldName="@nameof(NursingDiagnosesDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.NursingDiagnosesId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDto.Adapt<CreateUpdateNursingDiagnosesDto>().Adapt<NursingDiagnoses>());
var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDtos.Adapt<List<CreateUpdateNursingDiagnosesDto>>().Adapt<List<NursingDiagnoses>>()); 

var result = await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDto.Adapt<CreateUpdateNursingDiagnosesDto>().Adapt<NursingDiagnoses>());  
var result = await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDtos.Adapt<List<CreateUpdateNursingDiagnosesDto>>().Adapt<List<NursingDiagnoses>>());

list3 = (await Mediator.Send(new GetNursingDiagnosesQuery
{
    Predicate = x => NursingDiagnosesNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new NursingDiagnoses
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeNursingDiagnoses { get; set; } = 10;
    private int totalCountNursingDiagnoses = 0;
    private int activePageIndexNursingDiagnoses { get; set; } = 0;
    private string searchTermNursingDiagnoses { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedNursingDiagnoses(string searchText)
    {
        searchTermNursingDiagnoses = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeNursingDiagnoses);
    }

    private async Task OnpageSizeNursingDiagnosesIndexChanged(int newpageSizeNursingDiagnoses)
    {
        pageSizeNursingDiagnoses = newpageSizeNursingDiagnoses;
        await LoadDataOnSearchBoxChanged(0, newpageSizeNursingDiagnoses);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeNursingDiagnoses);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeNursingDiagnoses = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetNursingDiagnosesQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeNursingDiagnoses,
            SearchTerm = searchTermNursingDiagnoses,
        });
        NursingDiagnosess = result.Item1;
        totalCountNursingDiagnoses = result.PageCount;
        activePageIndexNursingDiagnoses = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastNursingDiagnoses);
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
          var a = await Mediator.Send(new GetGeneralConsultanNursingDiagnosessQuery
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

          GeneralConsultanNursingDiagnosess = a.Item1;
          totalCount = a.PageCount;
          activePageIndex = pageIndex;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastNursingDiagnoses);
      }
      finally { PanelVisible = false; }
  }

  #endregion Searching


   <MyGridPaginate @ref="GridDetail"
                 Data="NursingDiagnosess"
                 @bind-SelectedDataItems="@SelectedDetailDataItems"
                 EditModelSaving="OnSaveInventoryAdjumentDetail"
                 DataItemDeleting="OnDeleteInventoryAdjumentDetail"
                 EditFormButtonsVisible="false"
                 FocusedRowChanged="GridDetail_FocusedRowChanged"
                 SearchTextChanged="OnSearchBoxChanged"
                 KeyFieldName="Id">


     <ToolbarTemplate>
         <MyDxToolbarBase TItem="NursingDiagnosesDto"
                          Items="@NursingDiagnosess"
                          Grid="GridDetail"
                          SelectedDataItems="@SelectedDetailDataItems"
                          NewItem_Click="@NewItem_Click"
                          EditItem_Click="@EditItem_Click"
                          DeleteItem_Click="@DeleteItem_Click"
                          Refresh_Click="@(async () => await LoadData())"
                          IsImport="NursingDiagnosesAccessCRUID.IsImport"
                          VisibleNew="NursingDiagnosesAccessCRUID.IsCreate"
                          VisibleEdit="NursingDiagnosesAccessCRUID.IsUpdate"
                          VisibleDelete="NursingDiagnosesAccessCRUID.IsDelete" />
     </ToolbarTemplate>


     <Columns>
         <DxGridSelectionColumn Width="15px" />
         <DxGridDataColumn FieldName="NursingDiagnoses.Name" Caption="NursingDiagnoses"></DxGridDataColumn>
         <DxGridDataColumn FieldName="TeoriticalQty" Caption="Teoritical Qty" />
         <DxGridDataColumn FieldName="RealQty" Caption="Real Qty" />
         <DxGridDataColumn FieldName="Difference" Caption="Difference" />
         <DxGridDataColumn FieldName="Batch" Caption="Lot Serial Number" />
         <DxGridDataColumn FieldName="ExpiredDate" Caption="Expired Date" SortIndex="0" DisplayFormat="@Helper.DefaultFormatDate" />
         <DxGridDataColumn FieldName="NursingDiagnoses.NursingDiagnoses.Name" Caption="NursingDiagnoses" />
     </Columns>
     <EditFormTemplate Context="EditFormContext">
         @{
             if (EditFormContext.DataItem is null)
             {
                 FormNursingDiagnoses = (NursingDiagnosesDto)EditFormContext.EditModel;
             }
             var IsBatch = NursingDiagnosess.FirstOrDefault(x => x.Id == FormNursingDiagnoses.NursingDiagnosesId)?.TraceAbility ?? false;

             ActiveButton = FormNursingDiagnoses.NursingDiagnosesId is null ||
             string.IsNullOrWhiteSpace(FormNursingDiagnoses.Batch) && IsBatch ||
             FormNursingDiagnoses.ExpiredDate is null ||
             FormNursingDiagnoses.NursingDiagnosesId is null;
         }
         <div class="row w-100">
             <DxFormLayout CssClass="w-100">
                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="NursingDiagnoses" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox Data="@NursingDiagnosess"
                                     @bind-Value="@FormNursingDiagnoses.NursingDiagnosesId"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     NullText="Select NursingDiagnoses..."
                                     TextFieldName="Name"
                                     ReadOnly="@(FormNursingDiagnoses.Id != 0)"
                                     ValueFieldName="Id"
                                     SelectedItemChanged="@(async (NursingDiagnosesDto freq) => await OnSelectNursingDiagnoses(freq))"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     ShowValidationIcon="true" />
                         <ValidationMessage For="@(()=> FormNursingDiagnoses.NursingDiagnosesId)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem Caption="Batch" Enabled="FormNursingDiagnoses.Id == 0" Visible="IsBatch" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <MyDxComboBox Data="@Batch"
                                       ReadOnly="@(FormNursingDiagnoses.Id != 0)"
                                       NullText="Select Batch..."
                                       AllowNursingDiagnosesInput="true"
                                       @bind-Value="@FormNursingDiagnoses.Batch"
                                       @bind-Text="@FormNursingDiagnoses.Batch"
                                       SelectedItemChanged="@((string a)=> SelectedBatch(a))" />
                         <ValidationMessage For="@(() => FormNursingDiagnoses.Batch)" />

                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Teoritical Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     ReadOnly
                                     MinValue="0"
                                     @bind-Value="@FormNursingDiagnoses.TeoriticalQty"
                                     NullText="Teoritical Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormNursingDiagnoses.TeoriticalQty)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Real Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     MinValue="0"
                                     @bind-Value="@FormNursingDiagnoses.RealQty"
                                     NullText="Real Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormNursingDiagnoses.RealQty)"   />
                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Expired Date" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxDateEdit ShowValidationIcon="true"
                                     ReadOnly="@(FormNursingDiagnoses.Id != 0)"
                                     DisplayFormat="@Helper.DefaultFormatDate"
                                     @bind-Date="@FormNursingDiagnoses.ExpiredDate"
                                     NullText="Expired Date">
                         </DxDateEdit>
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption required-caption" Caption="NursingDiagnoses" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox ShowValidationIcon="true" Data="@NursingDiagnosess"
                                     NullText="NursingDiagnoses"
                                     ReadOnly="@(FormNursingDiagnoses.Id != 0)"
                                     TextFieldName="Name"
                                     ValueFieldName="Id"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     @bind-Value="FormNursingDiagnoses.NursingDiagnosesId">
                         </DxComboBox>
                         <ValidationMessage For="@(() => FormNursingDiagnoses.NursingDiagnosesId)" />
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

NursingDiagnoses

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="NursingDiagnoses" ColSpanMd="12">
    <DxComboBox Data="NursingDiagnosess"
                AllowNursingDiagnosesInput="true"
                NullText="Select NursingDiagnoses"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputNursingDiagnoses"
                @bind-Value="a.NursingDiagnosesId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(NursingDiagnoses.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(NursingDiagnoses.Code)" Caption="Code" />
        </Columns>
    </DxComboBox>
    <ValidationMessage For="@(()=>a.NursingDiagnosesId)" />
</DxFormLayoutItem>

#region ComboBox NursingDiagnoses
 
private CancellationTokenSource? _ctsNursingDiagnoses;
private async Task OnInputNursingDiagnoses(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;
            
        _ctsNursingDiagnoses?.Cancel();
        _ctsNursingDiagnoses?.Dispose();
        _ctsNursingDiagnoses = new CancellationTokenSource();
            
        await Task.Delay(700, _ctsNursingDiagnoses.Token);
            
        await LoadNursingDiagnoses(e.Value?.ToString() ?? "");
    } 
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsNursingDiagnoses?.Dispose();
        _ctsNursingDiagnoses = null;
    } 
}

 private async Task LoadNursingDiagnoses(string? e = "", Expression<Func<NursingDiagnoses, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         NursingDiagnosess = await Mediator.QueryGetComboBox<NursingDiagnoses, NursingDiagnosesDto>(e, predicate);
         PanelVisible = false;
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastNursingDiagnoses);
     }
     finally { PanelVisible = false; }
 }

#endregion


// Ini buat di EditItemClick
await LoadNursingDiagnoses(id:  a.NursingDiagnosesId);

NursingDiagnoses



VIRAL 2025

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="NursingDiagnoses" ColSpanMd="12">
    <MyDxComboBox Data="NursingDiagnosess"
                NullText="Select NursingDiagnoses"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputNursingDiagnoses"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                SelectedItemChanged="((NursingDiagnosesDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.NursingDiagnosesId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(NursingDiagnoses.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(NursingDiagnoses.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.NursingDiagnosesId)" />
</DxFormLayoutItem>


#region ComboBox NursingDiagnoses

    private NursingDiagnosesDto SelectedNursingDiagnoses { get; set; } = new();
    async Task SelectedItemChanged(NursingDiagnosesDto e)
    {
        if (e is null)
        {
            SelectedNursingDiagnoses = new();
            await LoadNursingDiagnoses(); 
        }
        else
            SelectedNursingDiagnoses = e;
    }

    private CancellationTokenSource? _ctsNursingDiagnoses;
    private async Task OnInputNursingDiagnoses(ChangeEventArgs e)
    {
        try
        { 
            _ctsNursingDiagnoses?.Cancel();
            _ctsNursingDiagnoses?.Dispose();
            _ctsNursingDiagnoses = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsNursingDiagnoses.Token);

            await LoadNursingDiagnoses(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsNursingDiagnoses?.Dispose();
            _ctsNursingDiagnoses = null;
        }
    }

    private async Task LoadNursingDiagnoses(string? e = "", Expression<Func<NursingDiagnoses, bool>>? predicate = null)
    {
        try
        { 
            NursingDiagnosess = await Mediator.QueryGetComboBox<NursingDiagnoses, NursingDiagnosesDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastNursingDiagnoses);
        }
        finally { PanelVisible = false; }
    }

#endregion

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="DiagnosisBPJSIntegrationTemp" ColSpanMd="12">
     <MyDxComboBox Data="@DiagnosisBPJSIntegrationTemps"
                   NullText="Select DiagnosisBPJSIntegrationTemp"
                   @ref="refDiagnosisBPJSIntegrationTempComboBox"
                   @bind-Value="@GeneralConsultanService.DiagnosisBPJSIntegrationTempId"
                   TextFieldName="Name"
                   ValueFieldName="Id"
                   ReadOnly="@(!GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned))"
                   SelectedItemChanged="@((UserDto e) => SelectedItemDiagnosisBPJSIntegrationTempChanged(e))"
                   TextChanged="((string e) => OnInputDiagnosisBPJSIntegrationTempChanged(e))">
         <Buttons>
             <DxEditorButton Click="OnSearchDiagnosisBPJSIntegrationTempIndexDecrement"
                             IconCssClass="fa-solid fa-caret-left"
                             Visible="!ReadOnlyForm() && IsStatus(EnumStatusGeneralConsultantService.Planned)"
                             Tooltip="Previous Index" />
             <DxEditorButton Click="OnSearchDiagnosisBPJSIntegrationTemp"
                             IconCssClass="fa-solid fa-magnifying-glass"
                             Visible="!ReadOnlyForm() && IsStatus(EnumStatusGeneralConsultantService.Planned)"
                             Tooltip="Search" />
             <DxEditorButton Click="OnSearchDiagnosisBPJSIntegrationTempIndexIncrement"
                             Visible="!ReadOnlyForm() && IsStatus(EnumStatusGeneralConsultantService.Planned)"
                             IconCssClass="fa-solid fa-caret-right"
                             Tooltip="Next Index" />
         </Buttons>
         <Columns>
             <DxListEditorColumn FieldName="NoRm" Caption="Medical Record" />
             <DxListEditorColumn FieldName="Name" />
             <DxListEditorColumn FieldName="Email" />
             <DxListEditorColumn FieldName="MobilePhone" Caption="Mobile Phone" />
             <DxListEditorColumn FieldName="Gender" />
             <DxListEditorColumn FieldName="DateOfBirth" Caption="Date Of Birth" />
         </Columns>
     </MyDxComboBox>
     <ValidationMessage For="@(()=>GeneralConsultanService.DiagnosisBPJSIntegrationTempId)" />
 </DxFormLayoutItem>

 #region ComboboxDiagnosisBPJSIntegrationTemp

private DxComboBox<UserDto, long?> refDiagnosisBPJSIntegrationTempComboBox { get; set; }
private int DiagnosisBPJSIntegrationTempComboBoxIndex { get; set; } = 0;
private int totalCountDiagnosisBPJSIntegrationTemp = 0;

private async Task OnSearchDiagnosisBPJSIntegrationTemp()
{
    await LoadDataDiagnosisBPJSIntegrationTemp();
}

private async Task OnSearchDiagnosisBPJSIntegrationTempIndexIncrement()
{
    if (DiagnosisBPJSIntegrationTempComboBoxIndex < (totalCountDiagnosisBPJSIntegrationTemp - 1))
    {
        DiagnosisBPJSIntegrationTempComboBoxIndex++;
        await LoadDataDiagnosisBPJSIntegrationTemp(DiagnosisBPJSIntegrationTempComboBoxIndex, 10);
    }
}

private async Task OnSearchDiagnosisBPJSIntegrationTempIndexDecrement()
{
    if (DiagnosisBPJSIntegrationTempComboBoxIndex > 0)
    {
        DiagnosisBPJSIntegrationTempComboBoxIndex--;
        await LoadDataDiagnosisBPJSIntegrationTemp(DiagnosisBPJSIntegrationTempComboBoxIndex, 10);
    }
}

private async Task OnInputDiagnosisBPJSIntegrationTempChanged(string e)
{
    DiagnosisBPJSIntegrationTempComboBoxIndex = 0;
    await LoadDataDiagnosisBPJSIntegrationTemp();
}

private async Task LoadDataDiagnosisBPJSIntegrationTemp(int pageIndex = 0, int pageSize = 10)
{
    try
    {
        PanelVisible = true;
        var result = await Mediator.Send(new GetUserQueryNew
        {
            Predicate = x => x.IsDiagnosisBPJSIntegrationTemp == true,
            SearchTerm = refDiagnosisBPJSIntegrationTempComboBox?.Text ?? "",
            PageIndex = pageIndex,
            PageSize = pageSize,
            Select = x => new User
            {
                Id = x.Id,
                Name = x.Name,
                NoRm = x.NoRm,
                Email = x.Email,
                MobilePhone = x.MobilePhone,
                Gender = x.Gender,
                DateOfBirth = x.DateOfBirth,
                NoId = x.NoId,
                CurrentMobile = x.CurrentMobile,

                IsWeatherDiagnosisBPJSIntegrationTempAllergyIds = x.IsWeatherDiagnosisBPJSIntegrationTempAllergyIds,
                IsFoodDiagnosisBPJSIntegrationTempAllergyIds = x.IsFoodDiagnosisBPJSIntegrationTempAllergyIds,
                IsPharmacologyDiagnosisBPJSIntegrationTempAllergyIds = x.IsPharmacologyDiagnosisBPJSIntegrationTempAllergyIds,
                WeatherDiagnosisBPJSIntegrationTempAllergyIds = x.WeatherDiagnosisBPJSIntegrationTempAllergyIds,
                FoodDiagnosisBPJSIntegrationTempAllergyIds = x.FoodDiagnosisBPJSIntegrationTempAllergyIds,
                PharmacologyDiagnosisBPJSIntegrationTempAllergyIds = x.PharmacologyDiagnosisBPJSIntegrationTempAllergyIds,

                IsFamilyMedicalHistory = x.IsFamilyMedicalHistory,
                FamilyMedicalHistory = x.FamilyMedicalHistory,
                FamilyMedicalHistoryOther = x.FamilyMedicalHistoryOther,

                IsMedicationHistory = x.IsMedicationHistory,
                MedicationHistory = x.MedicationHistory,
                PastMedicalHistory = x.PastMedicalHistory
            }
        });
        DiagnosisBPJSIntegrationTemps = result.Item1;
        totalCountDiagnosisBPJSIntegrationTemp = result.PageCount;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}

private async Task SelectedItemDiagnosisBPJSIntegrationTempChanged(UserDto e)
{
    GeneralConsultanService.InsurancePolicyId = null;
    InsurancePolicies.Clear();
    GeneralConsultanService.DiagnosisBPJSIntegrationTemp = new();

    if (e is null)
        return;

    GeneralConsultanService.DiagnosisBPJSIntegrationTemp = DiagnosisBPJSIntegrationTemps.FirstOrDefault(x => x.Id == e.Id) ?? new();
    GeneralConsultanService.DiagnosisBPJSIntegrationTempId = e.Id;
    UserForm = DiagnosisBPJSIntegrationTemps.FirstOrDefault(x => x.Id == e.Id) ?? new();

    if (!string.IsNullOrWhiteSpace(GeneralConsultanService.Payment))
    {
        InsurancePolicies = (await Mediator.Send(new GetInsurancePolicyQuery
        {
            Predicate = x => x.UserId == GeneralConsultanService.DiagnosisBPJSIntegrationTempId && x.Insurance != null && x.Insurance.IsBPJS == GeneralConsultanService.Payment.Equals("BPJS") && x.Active == true,
            Select = x => new InsurancePolicy
            {
                Id = x.Id,
                Insurance = new Insurance
                {
                    Name = x.Insurance == null ? "" : x.Insurance.Name,
                },
                NoKartu = x.NoKartu,
                KdProviderPstKdProvider = x.KdProviderPstKdProvider,
                PolicyNumber = x.PolicyNumber,
                PstPrb = x.PstPrb,
                PstProl = x.PstProl
            }
        })).Item1;
    }
}

#endregion ComboboxDiagnosisBPJSIntegrationTemp
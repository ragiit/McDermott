PatientFamilyRelation

public class PatientFamilyRelationCommand
 {
     #region GET

    public class GetSinglePatientFamilyRelationQuery : IRequest<PatientFamilyRelationDto>
    {
        public List<Expression<Func<PatientFamilyRelation, object>>> Includes { get; set; }
        public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; set; }
        public Expression<Func<PatientFamilyRelation, PatientFamilyRelation>> Select { get; set; }

        public List<(Expression<Func<PatientFamilyRelation, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetPatientFamilyRelationQuery : IRequest<(List<PatientFamilyRelationDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<PatientFamilyRelation, object>>> Includes { get; set; }
        public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; set; }
        public Expression<Func<PatientFamilyRelation, PatientFamilyRelation>> Select { get; set; }

        public List<(Expression<Func<PatientFamilyRelation, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidatePatientFamilyRelation(Expression<Func<PatientFamilyRelation, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreatePatientFamilyRelationRequest(PatientFamilyRelationDto PatientFamilyRelationDto) : IRequest<PatientFamilyRelationDto>
     {
         public PatientFamilyRelationDto PatientFamilyRelationDto { get; set; } = PatientFamilyRelationDto;
     }

     public class BulkValidatePatientFamilyRelation(List<PatientFamilyRelationDto> PatientFamilyRelationsToValidate) : IRequest<List<PatientFamilyRelationDto>>
     {
         public List<PatientFamilyRelationDto> PatientFamilyRelationsToValidate { get; } = PatientFamilyRelationsToValidate;
     }

     public class CreateListPatientFamilyRelationRequest(List<PatientFamilyRelationDto> PatientFamilyRelationDtos) : IRequest<List<PatientFamilyRelationDto>>
     {
         public List<PatientFamilyRelationDto> PatientFamilyRelationDtos { get; set; } = PatientFamilyRelationDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdatePatientFamilyRelationRequest(PatientFamilyRelationDto PatientFamilyRelationDto) : IRequest<PatientFamilyRelationDto>
     {
         public PatientFamilyRelationDto PatientFamilyRelationDto { get; set; } = PatientFamilyRelationDto;
     }

     public class UpdateListPatientFamilyRelationRequest(List<PatientFamilyRelationDto> PatientFamilyRelationDtos) : IRequest<List<PatientFamilyRelationDto>>
     {
         public List<PatientFamilyRelationDto> PatientFamilyRelationDtos { get; set; } = PatientFamilyRelationDtos;
     }

     #endregion Update

     #region DELETE

     public class DeletePatientFamilyRelationRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidatePatientFamilyRelationQuery, List<PatientFamilyRelationDto>>,
  
IRequestHandler<GetPatientFamilyRelationQuery, (List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSinglePatientFamilyRelationQuery, PatientFamilyRelationDto>,
public class PatientFamilyRelationHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetPatientFamilyRelationQuery, (List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSinglePatientFamilyRelationQuery, PatientFamilyRelationDto>, 
     IRequestHandler<ValidatePatientFamilyRelation, bool>,
     IRequestHandler<CreatePatientFamilyRelationRequest, PatientFamilyRelationDto>,
     IRequestHandler<BulkValidatePatientFamilyRelation, List<PatientFamilyRelationDto>>,
     IRequestHandler<CreateListPatientFamilyRelationRequest, List<PatientFamilyRelationDto>>,
     IRequestHandler<UpdatePatientFamilyRelationRequest, PatientFamilyRelationDto>,
     IRequestHandler<UpdateListPatientFamilyRelationRequest, List<PatientFamilyRelationDto>>,
     IRequestHandler<DeletePatientFamilyRelationRequest, bool>
{
    #region GET
    public async Task<List<PatientFamilyRelationDto>> Handle(BulkValidatePatientFamilyRelation request, CancellationToken cancellationToken)
    {
        var PatientFamilyRelationDtos = request.PatientFamilyRelationsToValidate;

        // Ekstrak semua kombinasi yang akan dicari di database
        //var PatientFamilyRelationNames = PatientFamilyRelationDtos.Select(x => x.Name).Distinct().ToList();
        //var Codes = PatientFamilyRelationDtos.Select(x => x.Code).Distinct().ToList();

        //var existingPatientFamilyRelations = await _unitOfWork.Repository<PatientFamilyRelation>()
        //    .Entities
        //    .AsNoTracking()
        //    .Where(v => PatientFamilyRelationNames.Contains(v.Name) && Codes.Contains(v.Code))
        //    .ToListAsync(cancellationToken);

        //return existingPatientFamilyRelations.Adapt<List<PatientFamilyRelationDto>>();

        return [];
    }
    public async Task<bool> Handle(ValidatePatientFamilyRelation request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<PatientFamilyRelation>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetPatientFamilyRelationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<PatientFamilyRelation>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<PatientFamilyRelation>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<PatientFamilyRelation>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.PatientFamilyRelation.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new PatientFamilyRelation
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

                return (pagedItems.Adapt<List<PatientFamilyRelationDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<PatientFamilyRelationDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<PatientFamilyRelationDto> Handle(GetSinglePatientFamilyRelationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<PatientFamilyRelation>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<PatientFamilyRelation>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<PatientFamilyRelation>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.PatientFamilyRelation.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new PatientFamilyRelation
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<PatientFamilyRelationDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<PatientFamilyRelationDto> Handle(CreatePatientFamilyRelationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<PatientFamilyRelation>().AddAsync(request.PatientFamilyRelationDto.Adapt<CreateUpdatePatientFamilyRelationDto>().Adapt<PatientFamilyRelation>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<PatientFamilyRelationDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<PatientFamilyRelationDto>> Handle(CreateListPatientFamilyRelationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<PatientFamilyRelation>().AddAsync(request.PatientFamilyRelationDtos.Adapt<List<PatientFamilyRelation>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<PatientFamilyRelationDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<PatientFamilyRelationDto> Handle(UpdatePatientFamilyRelationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<PatientFamilyRelation>().UpdateAsync(request.PatientFamilyRelationDto.Adapt<PatientFamilyRelationDto>().Adapt<PatientFamilyRelation>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<PatientFamilyRelationDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<PatientFamilyRelationDto>> Handle(UpdateListPatientFamilyRelationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<PatientFamilyRelation>().UpdateAsync(request.PatientFamilyRelationDtos.Adapt<List<PatientFamilyRelation>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<PatientFamilyRelationDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeletePatientFamilyRelationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<PatientFamilyRelation>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<PatientFamilyRelation>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidatePatientFamilyRelationQuery(List<PatientFamilyRelationDto> PatientFamilyRelationsToValidate) : IRequest<List<PatientFamilyRelationDto>>
{
    public List<PatientFamilyRelationDto> PatientFamilyRelationsToValidate { get; } = PatientFamilyRelationsToValidate;
}a


IRequestHandler<BulkValidatePatientFamilyRelationQuery, List<PatientFamilyRelationDto>>,
  
IRequestHandler<GetPatientFamilyRelationQuery, (List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSinglePatientFamilyRelationQuery, PatientFamilyRelationDto>,



 var a = await Mediator.Send(new GetPatientFamilyRelationsQuery
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

var patienss = (await Mediator.Send(new GetSinglePatientFamilyRelationQuery
{
    Predicate = x => x.Id == data.DiagnosisBPJSIntegrationTempId,
    Select = x => new PatientFamilyRelation
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
    var result = await Mediator.Send(new GetPatientFamilyRelationQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    PatientFamilyRelations = result.Item1;
    totalCount = result.PageCount;
    activePageIndex = pageIndex;
}
catch (Exception ex)
{
    ex.HandleException(ToastPatientFamilyRelation);
}
finally
{ 
    PanelVisible = false;
}

 var result = await Mediator.Send(new GetPatientFamilyRelationQuery
 {
     Predicate = x => x.PatientFamilyRelationId == PatientFamilyRelationId,
     SearchTerm = refPatientFamilyRelationComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 PatientFamilyRelations = result.Item1;
 totalCountPatientFamilyRelation = result.PageCount;

 PatientFamilyRelations = (await Mediator.Send(new GetPatientFamilyRelationQuery
 {
     Predicate = x => x.Id == PatientFamilyRelationForm.IdCardPatientFamilyRelationId,
 })).Item1;

var data = (await Mediator.Send(new GetSinglePatientFamilyRelationsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.DiagnosisBPJSIntegrationTemp
    ],
    Select = x => new PatientFamilyRelation
    {
        Id = x.Id,
        DiagnosisBPJSIntegrationTempId = x.DiagnosisBPJSIntegrationTempId,
        DiagnosisBPJSIntegrationTemp = new PatientFamilyRelation
        {
            DateOfBirth = x.DiagnosisBPJSIntegrationTemp.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new PatientFamilyRelation
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


#region ComboboxPatientFamilyRelation

 private DxComboBox<PatientFamilyRelationDto, long?> refPatientFamilyRelationComboBox { get; set; }
 private int PatientFamilyRelationComboBoxIndex { get; set; } = 0;
 private int totalCountPatientFamilyRelation = 0;

 private async Task OnSearchPatientFamilyRelation()
 {
     await LoadDataPatientFamilyRelation();
 }

 private async Task OnSearchPatientFamilyRelationIndexIncrement()
 {
     if (PatientFamilyRelationComboBoxIndex < (totalCountPatientFamilyRelation - 1))
     {
         PatientFamilyRelationComboBoxIndex++;
         await LoadDataPatientFamilyRelation(PatientFamilyRelationComboBoxIndex, 10);
     }
 }

 private async Task OnSearchPatientFamilyRelationIndexDecrement()
 {
     if (PatientFamilyRelationComboBoxIndex > 0)
     {
         PatientFamilyRelationComboBoxIndex--;
         await LoadDataPatientFamilyRelation(PatientFamilyRelationComboBoxIndex, 10);
     }
 }

 private async Task OnInputPatientFamilyRelationChanged(string e)
 {
     PatientFamilyRelationComboBoxIndex = 0;
     await LoadDataPatientFamilyRelation();
 }

 
  private async Task LoadDataPatientFamilyRelation(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetPatientFamilyRelationQuery
          {
              SearchTerm = refPatientFamilyRelationComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          PatientFamilyRelations = result.Item1;
          totalCountPatientFamilyRelation = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastPatientFamilyRelation);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxPatientFamilyRelation

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="PatientFamilyRelation" ColSpanMd="12">
    <MyDxComboBox Data="@PatientFamilyRelations"
                  NullText="Select PatientFamilyRelation"
                  @ref="refPatientFamilyRelationComboBox"
                  @bind-Value="@a.PatientFamilyRelationId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputPatientFamilyRelationChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchPatientFamilyRelationIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchPatientFamilyRelation"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchPatientFamilyRelationIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(PatientFamilyRelationDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="PatientFamilyRelation.Name" Caption="PatientFamilyRelation" />
            <DxListEditorColumn FieldName="@nameof(PatientFamilyRelationDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.PatientFamilyRelationId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<PatientFamilyRelation>().AddAsync(request.PatientFamilyRelationDto.Adapt<CreateUpdatePatientFamilyRelationDto>().Adapt<PatientFamilyRelation>());
var result = await _unitOfWork.Repository<PatientFamilyRelation>().AddAsync(request.PatientFamilyRelationDtos.Adapt<List<CreateUpdatePatientFamilyRelationDto>>().Adapt<List<PatientFamilyRelation>>()); 

var result = await _unitOfWork.Repository<PatientFamilyRelation>().UpdateAsync(request.PatientFamilyRelationDto.Adapt<CreateUpdatePatientFamilyRelationDto>().Adapt<PatientFamilyRelation>());  
var result = await _unitOfWork.Repository<PatientFamilyRelation>().UpdateAsync(request.PatientFamilyRelationDtos.Adapt<List<CreateUpdatePatientFamilyRelationDto>>().Adapt<List<PatientFamilyRelation>>());

list3 = (await Mediator.Send(new GetPatientFamilyRelationQuery
{
    Predicate = x => PatientFamilyRelationNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new PatientFamilyRelation
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizePatientFamilyRelation { get; set; } = 10;
    private int totalCountPatientFamilyRelation = 0;
    private int activePageIndexPatientFamilyRelation { get; set; } = 0;
    private string searchTermPatientFamilyRelation { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedPatientFamilyRelation(string searchText)
    {
        searchTermPatientFamilyRelation = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizePatientFamilyRelation);
    }

    private async Task OnpageSizePatientFamilyRelationIndexChanged(int newpageSizePatientFamilyRelation)
    {
        pageSizePatientFamilyRelation = newpageSizePatientFamilyRelation;
        await LoadDataOnSearchBoxChanged(0, newpageSizePatientFamilyRelation);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizePatientFamilyRelation);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizePatientFamilyRelation = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetPatientFamilyRelationQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizePatientFamilyRelation,
            SearchTerm = searchTermPatientFamilyRelation,
        });
        PatientFamilyRelations = result.Item1;
        totalCountPatientFamilyRelation = result.PageCount;
        activePageIndexPatientFamilyRelation = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastPatientFamilyRelation);
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
          var a = await Mediator.Send(new GetGeneralConsultanPatientFamilyRelationsQuery
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

          GeneralConsultanPatientFamilyRelations = a.Item1;
          totalCount = a.PageCount;
          activePageIndex = pageIndex;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastPatientFamilyRelation);
      }
      finally { PanelVisible = false; }
  }

  #endregion Searching


   <MyGridPaginate @ref="GridDetail"
                 Data="PatientFamilyRelations"
                 @bind-SelectedDataItems="@SelectedDetailDataItems"
                 EditModelSaving="OnSaveInventoryAdjumentDetail"
                 DataItemDeleting="OnDeleteInventoryAdjumentDetail"
                 EditFormButtonsVisible="false"
                 FocusedRowChanged="GridDetail_FocusedRowChanged"
                 SearchTextChanged="OnSearchBoxChanged"
                 KeyFieldName="Id">


     <ToolbarTemplate>
         <MyDxToolbarBase TItem="PatientFamilyRelationDto"
                          Items="@PatientFamilyRelations"
                          Grid="GridDetail"
                          SelectedDataItems="@SelectedDetailDataItems"
                          NewItem_Click="@NewItem_Click"
                          EditItem_Click="@EditItem_Click"
                          DeleteItem_Click="@DeleteItem_Click"
                          Refresh_Click="@(async () => await LoadData())"
                          IsImport="PatientFamilyRelationAccessCRUID.IsImport"
                          VisibleNew="PatientFamilyRelationAccessCRUID.IsCreate"
                          VisibleEdit="PatientFamilyRelationAccessCRUID.IsUpdate"
                          VisibleDelete="PatientFamilyRelationAccessCRUID.IsDelete" />
     </ToolbarTemplate>


     <Columns>
         <DxGridSelectionColumn Width="15px" />
         <DxGridDataColumn FieldName="PatientFamilyRelation.Name" Caption="PatientFamilyRelation"></DxGridDataColumn>
         <DxGridDataColumn FieldName="TeoriticalQty" Caption="Teoritical Qty" />
         <DxGridDataColumn FieldName="RealQty" Caption="Real Qty" />
         <DxGridDataColumn FieldName="Difference" Caption="Difference" />
         <DxGridDataColumn FieldName="Batch" Caption="Lot Serial Number" />
         <DxGridDataColumn FieldName="ExpiredDate" Caption="Expired Date" SortIndex="0" DisplayFormat="@Helper.DefaultFormatDate" />
         <DxGridDataColumn FieldName="PatientFamilyRelation.PatientFamilyRelation.Name" Caption="PatientFamilyRelation" />
     </Columns>
     <EditFormTemplate Context="EditFormContext">
         @{
             if (EditFormContext.DataItem is null)
             {
                 FormPatientFamilyRelation = (PatientFamilyRelationDto)EditFormContext.EditModel;
             }
             var IsBatch = PatientFamilyRelations.FirstOrDefault(x => x.Id == FormPatientFamilyRelation.PatientFamilyRelationId)?.TraceAbility ?? false;

             ActiveButton = FormPatientFamilyRelation.PatientFamilyRelationId is null ||
             string.IsNullOrWhiteSpace(FormPatientFamilyRelation.Batch) && IsBatch ||
             FormPatientFamilyRelation.ExpiredDate is null ||
             FormPatientFamilyRelation.PatientFamilyRelationId is null;
         }
         <div class="row w-100">
             <DxFormLayout CssClass="w-100">
                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="PatientFamilyRelation" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox Data="@PatientFamilyRelations"
                                     @bind-Value="@FormPatientFamilyRelation.PatientFamilyRelationId"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     NullText="Select PatientFamilyRelation..."
                                     TextFieldName="Name"
                                     ReadOnly="@(FormPatientFamilyRelation.Id != 0)"
                                     ValueFieldName="Id"
                                     SelectedItemChanged="@(async (PatientFamilyRelationDto freq) => await OnSelectPatientFamilyRelation(freq))"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     ShowValidationIcon="true" />
                         <ValidationMessage For="@(()=> FormPatientFamilyRelation.PatientFamilyRelationId)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem Caption="Batch" Enabled="FormPatientFamilyRelation.Id == 0" Visible="IsBatch" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <MyDxComboBox Data="@Batch"
                                       ReadOnly="@(FormPatientFamilyRelation.Id != 0)"
                                       NullText="Select Batch..."
                                       AllowPatientFamilyRelationInput="true"
                                       @bind-Value="@FormPatientFamilyRelation.Batch"
                                       @bind-Text="@FormPatientFamilyRelation.Batch"
                                       SelectedItemChanged="@((string a)=> SelectedBatch(a))" />
                         <ValidationMessage For="@(() => FormPatientFamilyRelation.Batch)" />

                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Teoritical Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     ReadOnly
                                     MinValue="0"
                                     @bind-Value="@FormPatientFamilyRelation.TeoriticalQty"
                                     NullText="Teoritical Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormPatientFamilyRelation.TeoriticalQty)"   />
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption" Caption="Real Qty" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxSpinEdit ShowValidationIcon="true"
                                     MinValue="0"
                                     @bind-Value="@FormPatientFamilyRelation.RealQty"
                                     NullText="Real Qty"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto" />
                         <ValidationMessage For="@(()=> FormPatientFamilyRelation.RealQty)"   />
                     </DxFormLayoutItem>
                 </div>

                 <div class="col-md-4">
                     <DxFormLayoutItem Caption="Expired Date" CaptionCssClass="required-caption normal-caption" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxDateEdit ShowValidationIcon="true"
                                     ReadOnly="@(FormPatientFamilyRelation.Id != 0)"
                                     DisplayFormat="@Helper.DefaultFormatDate"
                                     @bind-Date="@FormPatientFamilyRelation.ExpiredDate"
                                     NullText="Expired Date">
                         </DxDateEdit>
                     </DxFormLayoutItem>

                     <DxFormLayoutItem CaptionCssClass="normal-caption required-caption" Caption="PatientFamilyRelation" ColSpanMd="12" CaptionPosition="CaptionPosition.Vertical">
                         <DxComboBox ShowValidationIcon="true" Data="@PatientFamilyRelations"
                                     NullText="PatientFamilyRelation"
                                     ReadOnly="@(FormPatientFamilyRelation.Id != 0)"
                                     TextFieldName="Name"
                                     ValueFieldName="Id"
                                     ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                                     FilteringMode="@DataGridFilteringMode.Contains"
                                     @bind-Value="FormPatientFamilyRelation.PatientFamilyRelationId">
                         </DxComboBox>
                         <ValidationMessage For="@(() => FormPatientFamilyRelation.PatientFamilyRelationId)" />
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

PatientFamilyRelation

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="PatientFamilyRelation" ColSpanMd="12">
    <DxComboBox Data="PatientFamilyRelations"
                AllowPatientFamilyRelationInput="true"
                NullText="Select PatientFamilyRelation"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputPatientFamilyRelation"
                @bind-Value="a.PatientFamilyRelationId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(PatientFamilyRelation.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(PatientFamilyRelation.Code)" Caption="Code" />
        </Columns>
    </DxComboBox>
    <ValidationMessage For="@(()=>a.PatientFamilyRelationId)" />
</DxFormLayoutItem>

#region ComboBox PatientFamilyRelation
 
private CancellationTokenSource? _ctsPatientFamilyRelation;
private async Task OnInputPatientFamilyRelation(ChangeEventArgs e)
{
    try
    {
        PanelVisible = true;
            
        _ctsPatientFamilyRelation?.Cancel();
        _ctsPatientFamilyRelation?.Dispose();
        _ctsPatientFamilyRelation = new CancellationTokenSource();
            
        await Task.Delay(700, _ctsPatientFamilyRelation.Token);
            
        await LoadPatientFamilyRelation(e.Value?.ToString() ?? "");
    } 
    finally
    {
        PanelVisible = false;

        // Untuk menghindari kebocoran memori (memory leaks).
        _ctsPatientFamilyRelation?.Dispose();
        _ctsPatientFamilyRelation = null;
    } 
}

 private async Task LoadPatientFamilyRelation(string? e = "", Expression<Func<PatientFamilyRelation, bool>>? predicate = null)
 {
     try
     {
         PanelVisible = true;
         PatientFamilyRelations = await Mediator.QueryGetComboBox<PatientFamilyRelation, PatientFamilyRelationDto>(e, predicate);
         PanelVisible = false;
     }
     catch (Exception ex)
     {
         ex.HandleException(ToastPatientFamilyRelation);
     }
     finally { PanelVisible = false; }
 }

#endregion


// Ini buat di EditItemClick
await LoadPatientFamilyRelation(id:  a.PatientFamilyRelationId);

PatientFamilyRelation



VIRAL 2025

<DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="PatientFamilyRelation" ColSpanMd="12">
    <MyDxComboBox Data="PatientFamilyRelations"
                NullText="Select PatientFamilyRelation"
                TextFieldName="Name"
                ValueFieldName="Id"
                @oninput="OnInputPatientFamilyRelation"
                ClearButtonDisplayMode="DataEditorClearButtonDisplayMode.Auto"
                SelectedItemChanged="((PatientFamilyRelationDto e) => SelectedItemChanged(e))"  
                @bind-Value="a.PatientFamilyRelationId">
        <Columns>
            <DxListEditorColumn FieldName="@nameof(PatientFamilyRelation.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="@nameof(PatientFamilyRelation.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.PatientFamilyRelationId)" />
</DxFormLayoutItem>


#region ComboBox PatientFamilyRelation

    private PatientFamilyRelationDto SelectedPatientFamilyRelation { get; set; } = new();
    async Task SelectedItemChanged(PatientFamilyRelationDto e)
    {
        if (e is null)
        {
            SelectedPatientFamilyRelation = new();
            await LoadPatientFamilyRelation(); 
        }
        else
            SelectedPatientFamilyRelation = e;
    }

    private CancellationTokenSource? _ctsPatientFamilyRelation;
    private async Task OnInputPatientFamilyRelation(ChangeEventArgs e)
    {
        try
        { 
            _ctsPatientFamilyRelation?.Cancel();
            _ctsPatientFamilyRelation?.Dispose();
            _ctsPatientFamilyRelation = new CancellationTokenSource();

            await Task.Delay(Helper.CBX_DELAY, _ctsPatientFamilyRelation.Token);

            await LoadPatientFamilyRelation(e.Value?.ToString() ?? "");
        }
        finally
        { 
            _ctsPatientFamilyRelation?.Dispose();
            _ctsPatientFamilyRelation = null;
        }
    }

    private async Task LoadPatientFamilyRelation(string? e = "", Expression<Func<PatientFamilyRelation, bool>>? predicate = null)
    {
        try
        { 
            PatientFamilyRelations = await Mediator.QueryGetComboBox<PatientFamilyRelation, PatientFamilyRelationDto>(e, predicate); 
        }
        catch (Exception ex)
        {
            ex.HandleException(ToastPatientFamilyRelation);
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
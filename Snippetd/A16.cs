 public class GetSingleBpjsClassificationQuery : IRequest<BpjsClassificationDto>
 {
     public List<Expression<Func<BpjsClassification, object>>> Includes { get; set; }
     public Expression<Func<BpjsClassification, bool>> Predicate { get; set; }
     public Expression<Func<BpjsClassification, BpjsClassification>> Select { get; set; }

     public List<(Expression<Func<BpjsClassification, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

     public bool IsDescending { get; set; } = false; // default to ascending
     public int PageIndex { get; set; } = 0;
     public int PageSize { get; set; } = 10;
     public bool IsGetAll { get; set; } = false;
     public string SearchTerm { get; set; }
 }

public class GetBpjsClassificationQuery : IRequest<(List<BpjsClassificationDto>, int PageIndex, int PageSize, int PageCount)>
{
    public List<Expression<Func<BpjsClassification, object>>> Includes { get; set; }
    public Expression<Func<BpjsClassification, bool>> Predicate { get; set; }
    public Expression<Func<BpjsClassification, BpjsClassification>> Select { get; set; }

    public List<(Expression<Func<BpjsClassification, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public bool IsGetAll { get; set; } = false;
    public string SearchTerm { get; set; }
}

public async Task<List<BpjsClassificationDto>> Handle(BulkValidateBpjsClassificationQuery request, CancellationToken cancellationToken)
{
    var BpjsClassificationDtos = request.BpjsClassificationsToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var BpjsClassificationNames = BpjsClassificationDtos.Select(x => x.Name).Distinct().ToList();
    var postalCodes = BpjsClassificationDtos.Select(x => x.PostalCode).Distinct().ToList();
    var provinceIds = BpjsClassificationDtos.Select(x => x.ProvinceId).Distinct().ToList();
    var cityIds = BpjsClassificationDtos.Select(x => x.CityId).Distinct().ToList();
    var BpjsClassificationIds = BpjsClassificationDtos.Select(x => x.BpjsClassificationId).Distinct().ToList();

    var existingBpjsClassifications = await _unitOfWork.Repository<BpjsClassification>()
        .Entities
        .AsNoTracking()
        .Where(v => BpjsClassificationNames.Contains(v.Name)
                    && postalCodes.Contains(v.PostalCode)
                    && provinceIds.Contains(v.ProvinceId)
                    && cityIds.Contains(v.CityId)
                    && BpjsClassificationIds.Contains(v.BpjsClassificationId))
        .ToListAsync(cancellationToken);

    return existingBpjsClassifications.Adapt<List<BpjsClassificationDto>>();
}


        public class BulkValidateBpjsClassificationQuery(List<BpjsClassificationDto> BpjsClassificationsToValidate) : IRequest<List<BpjsClassificationDto>>
        {
            public List<BpjsClassificationDto> BpjsClassificationsToValidate { get; } = BpjsClassificationsToValidate;
        }


IRequestHandler<BulkValidateBpjsClassificationQuery, List<BpjsClassificationDto>>,
  
IRequestHandler<GetBpjsClassificationQuery, (List<BpjsClassificationDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleBpjsClassificationQuery, BpjsClassificationDto>,

public async Task<(List<BpjsClassificationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetBpjsClassificationQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<BpjsClassification>().Entities.AsNoTracking(); 

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
                    ? ((IOrderedQueryable<BpjsClassification>)query).ThenByDescending(additionalOrderBy.OrderBy)
                    : ((IOrderedQueryable<BpjsClassification>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.BpjsClassification.Name, $"%{request.SearchTerm}%")
                    );
        }

        // Apply dynamic select if provided
        if (request.Select is not null)
            query = query.Select(request.Select);
        else
            query = query.Select(x => new BpjsClassification
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

            return (pagedItems.Adapt<List<BpjsClassificationDto>>(), request.PageIndex, request.PageSize, totalPages);
        }
        else
        {
            return ((await query.ToListAsync(cancellationToken)).Adapt<List<BpjsClassificationDto>>(), 0, 1, 1);
        }
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}


  public async Task<BpjsClassificationDto> Handle(GetSingleBpjsClassificationQuery request, CancellationToken cancellationToken)
 {
     try
     {
         var query = _unitOfWork.Repository<BpjsClassification>().Entities.AsNoTracking();

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
                     ? ((IOrderedQueryable<BpjsClassification>)query).ThenByDescending(additionalOrderBy.OrderBy)
                     : ((IOrderedQueryable<BpjsClassification>)query).ThenBy(additionalOrderBy.OrderBy);
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
                EF.Functions.Like(v.BpjsClassification.Name, $"%{request.SearchTerm}%")
                );
         }

         // Apply dynamic select if provided
         if (request.Select is not null)
             query = query.Select(request.Select);
         else
             query = query.Select(x => new BpjsClassification
             {
                 Id = x.Id, 
             });

         return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<BpjsClassificationDto>();
     }
     catch (Exception ex)
     {
         // Consider logging the exception
         throw;
     }
 }

 var a = await Mediator.Send(new GetBpjsClassificationsQuery
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

var patienss = (await Mediator.Send(new GetSingleBpjsClassificationQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new BpjsClassification
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
    var result = await Mediator.Send(new GetBpjsClassificationQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    BpjsClassifications = result.Item1;
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

 var result = await Mediator.Send(new GetBpjsClassificationQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refBpjsClassificationComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 BpjsClassifications = result.Item1;
 totalCountBpjsClassification = result.PageCount;

 BpjsClassifications = (await Mediator.Send(new GetBpjsClassificationQuery
 {
     Predicate = x => x.Id == UserForm.IdCardBpjsClassificationId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleBpjsClassificationsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new BpjsClassification
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new BpjsClassification
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new BpjsClassification
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


#region ComboboxBpjsClassification

 private DxComboBox<BpjsClassificationDto, long?> refBpjsClassificationComboBox { get; set; }
 private int BpjsClassificationComboBoxIndex { get; set; } = 0;
 private int totalCountBpjsClassification = 0;

 private async Task OnSearchBpjsClassification()
 {
     await LoadDataBpjsClassification();
 }

 private async Task OnSearchBpjsClassificationIndexIncrement()
 {
     if (BpjsClassificationComboBoxIndex < (totalCountBpjsClassification - 1))
     {
         BpjsClassificationComboBoxIndex++;
         await LoadDataBpjsClassification(BpjsClassificationComboBoxIndex, 10);
     }
 }

 private async Task OnSearchBpjsClassificationIndexDecrement()
 {
     if (BpjsClassificationComboBoxIndex > 0)
     {
         BpjsClassificationComboBoxIndex--;
         await LoadDataBpjsClassification(BpjsClassificationComboBoxIndex, 10);
     }
 }

 private async Task OnInputBpjsClassificationChanged(string e)
 {
     BpjsClassificationComboBoxIndex = 0;
     await LoadDataBpjsClassification();
 }

 
  private async Task LoadDataBpjsClassification(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetBpjsClassificationQuery
          {
              SearchTerm = refBpjsClassificationComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          BpjsClassifications = result.Item1;
          totalCountBpjsClassification = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxBpjsClassification

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="BpjsClassification" ColSpanMd="12">
    <MyDxComboBox Data="@BpjsClassifications"
                  NullText="Select BpjsClassification"
                  @ref="refBpjsClassificationComboBox"
                  @bind-Value="@a.BpjsClassificationId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputBpjsClassificationChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchBpjsClassificationIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchBpjsClassification"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchBpjsClassificationIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(BpjsClassificationDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="BpjsClassification.Name" Caption="BpjsClassification" />
            <DxListEditorColumn FieldName="@nameof(BpjsClassificationDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.BpjsClassificationId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<BpjsClassification>().AddAsync(request.BpjsClassificationDto.Adapt<CreateUpdateBpjsClassificationDto>().Adapt<BpjsClassification>());
var result = await _unitOfWork.Repository<BpjsClassification>().AddAsync(request.BpjsClassificationDtos.Adapt<List<CreateUpdateBpjsClassificationDto>>().Adapt<List<BpjsClassification>>()); 

var result = await _unitOfWork.Repository<BpjsClassification>().UpdateAsync(request.BpjsClassificationDto.Adapt<CreateUpdateBpjsClassificationDto>().Adapt<BpjsClassification>());  
var result = await _unitOfWork.Repository<BpjsClassification>().UpdateAsync(request.BpjsClassificationDtos.Adapt<List<CreateUpdateBpjsClassificationDto>>().Adapt<List<BpjsClassification>>());

list3 = (await Mediator.Send(new GetBpjsClassificationQuery
{
    Predicate = x => BpjsClassificationNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new BpjsClassification
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
        var result = await Mediator.Send(new GetBpjsClassificationQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchTerm = searchTerm,
        });
        BpjsClassifications = result.Item1;
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
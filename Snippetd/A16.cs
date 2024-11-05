public class MaintenanceCommand
 {
     #region GET

    public class GetSingleMaintenanceQuery : IRequest<MaintenanceDto>
    {
        public List<Expression<Func<Maintenance, object>>> Includes { get; set; }
        public Expression<Func<Maintenance, bool>> Predicate { get; set; }
        public Expression<Func<Maintenance, Maintenance>> Select { get; set; }

        public List<(Expression<Func<Maintenance, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetMaintenanceQuery : IRequest<(List<MaintenanceDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<Maintenance, object>>> Includes { get; set; }
        public Expression<Func<Maintenance, bool>> Predicate { get; set; }
        public Expression<Func<Maintenance, Maintenance>> Select { get; set; }

        public List<(Expression<Func<Maintenance, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateMaintenance(Expression<Func<Maintenance, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<Maintenance, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateMaintenanceRequest(MaintenanceDto MaintenanceDto) : IRequest<MaintenanceDto>
     {
         public MaintenanceDto MaintenanceDto { get; set; } = MaintenanceDto;
     }

     public class BulkValidateMaintenance(List<MaintenanceDto> MaintenancesToValidate) : IRequest<List<MaintenanceDto>>
     {
         public List<MaintenanceDto> MaintenancesToValidate { get; } = MaintenancesToValidate;
     }

     public class CreateListMaintenanceRequest(List<MaintenanceDto> MaintenanceDtos) : IRequest<List<MaintenanceDto>>
     {
         public List<MaintenanceDto> MaintenanceDtos { get; set; } = MaintenanceDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateMaintenanceRequest(MaintenanceDto MaintenanceDto) : IRequest<MaintenanceDto>
     {
         public MaintenanceDto MaintenanceDto { get; set; } = MaintenanceDto;
     }

     public class UpdateListMaintenanceRequest(List<MaintenanceDto> MaintenanceDtos) : IRequest<List<MaintenanceDto>>
     {
         public List<MaintenanceDto> MaintenanceDtos { get; set; } = MaintenanceDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteMaintenanceRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateMaintenanceQuery, List<MaintenanceDto>>,
  
IRequestHandler<GetMaintenanceQuery, (List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleMaintenanceQuery, MaintenanceDto>,
public class MaintenanceHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetMaintenanceQuery, (List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleMaintenanceQuery, MaintenanceDto>, IRequestHandler<ValidateMaintenance, bool>,
     IRequestHandler<CreateMaintenanceRequest, MaintenanceDto>,
     IRequestHandler<BulkValidateMaintenance, List<MaintenanceDto>>,
     IRequestHandler<CreateListMaintenanceRequest, List<MaintenanceDto>>,
     IRequestHandler<UpdateMaintenanceRequest, MaintenanceDto>,
     IRequestHandler<UpdateListMaintenanceRequest, List<MaintenanceDto>>,
     IRequestHandler<DeleteMaintenanceRequest, bool>
{
    #region GET
    public async Task<List<MaintenanceDto>> Handle(BulkValidateMaintenance request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.MaintenancesToValidate;

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
    public async Task<bool> Handle(ValidateMaintenance request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Maintenance>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaintenanceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Maintenance>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<Maintenance>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Maintenance>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Maintenance.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Maintenance
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

                return (pagedItems.Adapt<List<MaintenanceDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<MaintenanceDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<MaintenanceDto> Handle(GetSingleMaintenanceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Maintenance>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<Maintenance>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Maintenance>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.Maintenance.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Maintenance
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<MaintenanceDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<MaintenanceDto> Handle(CreateMaintenanceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Maintenance>().AddAsync(request.MaintenanceDto.Adapt<CreateUpdateMaintenanceDto>().Adapt<Maintenance>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<MaintenanceDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<MaintenanceDto>> Handle(CreateListMaintenanceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Maintenance>().AddAsync(request.MaintenanceDtos.Adapt<List<Maintenance>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<MaintenanceDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<MaintenanceDto> Handle(UpdateMaintenanceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Maintenance>().UpdateAsync(request.MaintenanceDto.Adapt<MaintenanceDto>().Adapt<Maintenance>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<MaintenanceDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<MaintenanceDto>> Handle(UpdateListMaintenanceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Maintenance>().UpdateAsync(request.MaintenanceDtos.Adapt<List<Maintenance>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<MaintenanceDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteMaintenanceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<Maintenance>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<Maintenance>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateMaintenanceQuery(List<MaintenanceDto> MaintenancesToValidate) : IRequest<List<MaintenanceDto>>
{
    public List<MaintenanceDto> MaintenancesToValidate { get; } = MaintenancesToValidate;
}a


IRequestHandler<BulkValidateMaintenanceQuery, List<MaintenanceDto>>,
  
IRequestHandler<GetMaintenanceQuery, (List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleMaintenanceQuery, MaintenanceDto>,



 var a = await Mediator.Send(new GetMaintenancesQuery
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

var patienss = (await Mediator.Send(new GetSingleMaintenanceQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Maintenance
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
    var result = await Mediator.Send(new GetMaintenanceQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    Maintenances = result.Item1;
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

 var result = await Mediator.Send(new GetMaintenanceQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refMaintenanceComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 Maintenances = result.Item1;
 totalCountMaintenance = result.PageCount;

 Maintenances = (await Mediator.Send(new GetMaintenanceQuery
 {
     Predicate = x => x.Id == MaintenanceForm.IdCardMaintenanceId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleMaintenancesQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Maintenance
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Maintenance
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Maintenance
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


#region ComboboxMaintenance

 private DxComboBox<MaintenanceDto, long?> refMaintenanceComboBox { get; set; }
 private int MaintenanceComboBoxIndex { get; set; } = 0;
 private int totalCountMaintenance = 0;

 private async Task OnSearchMaintenance()
 {
     await LoadDataMaintenance();
 }

 private async Task OnSearchMaintenanceIndexIncrement()
 {
     if (MaintenanceComboBoxIndex < (totalCountMaintenance - 1))
     {
         MaintenanceComboBoxIndex++;
         await LoadDataMaintenance(MaintenanceComboBoxIndex, 10);
     }
 }

 private async Task OnSearchMaintenanceIndexDecrement()
 {
     if (MaintenanceComboBoxIndex > 0)
     {
         MaintenanceComboBoxIndex--;
         await LoadDataMaintenance(MaintenanceComboBoxIndex, 10);
     }
 }

 private async Task OnInputMaintenanceChanged(string e)
 {
     MaintenanceComboBoxIndex = 0;
     await LoadDataMaintenance();
 }

 
  private async Task LoadDataMaintenance(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetMaintenanceQuery
          {
              SearchTerm = refMaintenanceComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Maintenances = result.Item1;
          totalCountMaintenance = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxMaintenance

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Maintenance" ColSpanMd="12">
    <MyDxComboBox Data="@Maintenances"
                  NullText="Select Maintenance"
                  @ref="refMaintenanceComboBox"
                  @bind-Value="@a.MaintenanceId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputMaintenanceChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchMaintenanceIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchMaintenance"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchMaintenanceIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(MaintenanceDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Maintenance.Name" Caption="Maintenance" />
            <DxListEditorColumn FieldName="@nameof(MaintenanceDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.MaintenanceId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<Maintenance>().AddAsync(request.MaintenanceDto.Adapt<CreateUpdateMaintenanceDto>().Adapt<Maintenance>());
var result = await _unitOfWork.Repository<Maintenance>().AddAsync(request.MaintenanceDtos.Adapt<List<CreateUpdateMaintenanceDto>>().Adapt<List<Maintenance>>()); 

var result = await _unitOfWork.Repository<Maintenance>().UpdateAsync(request.MaintenanceDto.Adapt<CreateUpdateMaintenanceDto>().Adapt<Maintenance>());  
var result = await _unitOfWork.Repository<Maintenance>().UpdateAsync(request.MaintenanceDtos.Adapt<List<CreateUpdateMaintenanceDto>>().Adapt<List<Maintenance>>());

list3 = (await Mediator.Send(new GetMaintenanceQuery
{
    Predicate = x => MaintenanceNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new Maintenance
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeMaintenanceAttendance { get; set; } = 10;
    private int totalCountMaintenanceAttendance = 0;
    private int activePageIndexMaintenanceAttendance { get; set; } = 0;
    private string searchTermMaintenanceAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedMaintenanceAttendance(string searchText)
    {
        searchTermMaintenanceAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeMaintenanceAttendance);
    }

    private async Task OnpageSizeMaintenanceAttendanceIndexChanged(int newpageSizeMaintenanceAttendance)
    {
        pageSizeMaintenanceAttendance = newpageSizeMaintenanceAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeMaintenanceAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeMaintenanceAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeMaintenanceAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetMaintenanceAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeMaintenanceAttendance,
            SearchTerm = searchTermMaintenanceAttendance,
        });
        MaintenanceAttendances = result.Item1;
        totalCountMaintenanceAttendance = result.PageCount;
        activePageIndexMaintenanceAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
    }
    finally { PanelVisible = false; }
}
    #endregion Searching
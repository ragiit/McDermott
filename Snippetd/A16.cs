public class ServiceCommand
 {
     #region GET

    public class GetSingleServiceQuery : IRequest<ServiceDto>
    {
        public List<Expression<Func<Service, object>>> Includes { get; set; }
        public Expression<Func<Service, bool>> Predicate { get; set; }
        public Expression<Func<Service, Service>> Select { get; set; }

        public List<(Expression<Func<Service, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

    public class GetServiceQuery : IRequest<(List<ServiceDto>, int PageIndex, int PageSize, int PageCount)>
    {
        public List<Expression<Func<Service, object>>> Includes { get; set; }
        public Expression<Func<Service, bool>> Predicate { get; set; }
        public Expression<Func<Service, Service>> Select { get; set; }

        public List<(Expression<Func<Service, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

        public bool IsDescending { get; set; } = false; // default to ascending
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public bool IsGetAll { get; set; } = false;
        public string SearchTerm { get; set; }
    }

     public class ValidateService(Expression<Func<Service, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<Service, bool>> Predicate { get; } = predicate!;
     }

     public class BulkValidateService(List<ServiceDto> ServicesToValidate) : IRequest<List<ServiceDto>>
     {
         public List<ServiceDto> ServicesToValidate { get; } = ServicesToValidate;
     }

     #endregion GET

     #region CREATE

     public class CreateServiceRequest(ServiceDto ServiceDto) : IRequest<ServiceDto>
     {
         public ServiceDto ServiceDto { get; set; } = ServiceDto;
     }

     public class CreateListServiceRequest(List<ServiceDto> ServiceDtos) : IRequest<List<ServiceDto>>
     {
         public List<ServiceDto> ServiceDtos { get; set; } = ServiceDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateServiceRequest(ServiceDto ServiceDto) : IRequest<ServiceDto>
     {
         public ServiceDto ServiceDto { get; set; } = ServiceDto;
     }

     public class UpdateListServiceRequest(List<ServiceDto> ServiceDtos) : IRequest<List<ServiceDto>>
     {
         public List<ServiceDto> ServiceDtos { get; set; } = ServiceDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteServiceRequest : IRequest<bool>
     {
         public long Id { get; set; }  
         public List<long> Ids { get; set; }  
     }

     #endregion DELETE
 }

IRequestHandler<BulkValidateServiceQuery, List<ServiceDto>>,
  
IRequestHandler<GetServiceQuery, (List<ServiceDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleServiceQuery, ServiceDto>,
public class ServiceHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetServiceQuery, (List<ServiceDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleServiceQuery, ServiceDto>, 
     IRequestHandler<ValidateService, bool>,
     IRequestHandler<CreateServiceRequest, ServiceDto>,
     IRequestHandler<BulkValidateService, List<ServiceDto>>,
     IRequestHandler<CreateListServiceRequest, List<ServiceDto>>,
     IRequestHandler<UpdateServiceRequest, ServiceDto>,
     IRequestHandler<UpdateListServiceRequest, List<ServiceDto>>,
     IRequestHandler<DeleteServiceRequest, bool>
{
    #region GET
    public async Task<List<ServiceDto>> Handle(BulkValidateService request, CancellationToken cancellationToken)
    {
        var CountryDtos = request.ServicesToValidate;

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
    public async Task<bool> Handle(ValidateService request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Service>()
            .Entities
            .AsNoTracking()
            .Where(request.Predicate)  // Apply the Predicate for filtering
            .AnyAsync(cancellationToken);  // Check if any record matches the condition
    }

    public async Task<(List<ServiceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetServiceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Service>().Entities.AsNoTracking(); 

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
                        ? ((IOrderedQueryable<Service>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Service>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Service.Name, $"%{request.SearchTerm}%")
                        );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Service
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

                return (pagedItems.Adapt<List<ServiceDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            else
            {
                return ((await query.ToListAsync(cancellationToken)).Adapt<List<ServiceDto>>(), 0, 1, 1);
            }
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    public async Task<ServiceDto> Handle(GetSingleServiceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Repository<Service>().Entities.AsNoTracking();

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
                        ? ((IOrderedQueryable<Service>)query).ThenByDescending(additionalOrderBy.OrderBy)
                        : ((IOrderedQueryable<Service>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.Service.Name, $"%{request.SearchTerm}%")
                    );
            }

            // Apply dynamic select if provided
            if (request.Select is not null)
                query = query.Select(request.Select);
            else
                query = query.Select(x => new Service
                {
                    Id = x.Id, 
                });

            return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ServiceDto>();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            throw;
        }
    }
 
    #endregion GET

     #region CREATE

     public async Task<ServiceDto> Handle(CreateServiceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Service>().AddAsync(request.ServiceDto.Adapt<CreateUpdateServiceDto>().Adapt<Service>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<ServiceDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<ServiceDto>> Handle(CreateListServiceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Service>().AddAsync(request.ServiceDtos.Adapt<List<Service>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<ServiceDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<ServiceDto> Handle(UpdateServiceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Service>().UpdateAsync(request.ServiceDto.Adapt<ServiceDto>().Adapt<Service>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<ServiceDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<ServiceDto>> Handle(UpdateListServiceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<Service>().UpdateAsync(request.ServiceDtos.Adapt<List<Service>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<ServiceDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteServiceRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<Service>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<Service>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
}

 
public class BulkValidateServiceQuery(List<ServiceDto> ServicesToValidate) : IRequest<List<ServiceDto>>
{
    public List<ServiceDto> ServicesToValidate { get; } = ServicesToValidate;
}a


IRequestHandler<BulkValidateServiceQuery, List<ServiceDto>>,
  
IRequestHandler<GetServiceQuery, (List<ServiceDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleServiceQuery, ServiceDto>,



 var a = await Mediator.Send(new GetServicesQuery
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

var patienss = (await Mediator.Send(new GetSingleServiceQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Service
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
    var result = await Mediator.Send(new GetServiceQuery
    {
        SearchTerm = searchTerm,
        PageIndex = pageIndex,
        PageSize = pageSize,
    });
    Services = result.Item1;
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

 var result = await Mediator.Send(new GetServiceQuery
 {
     Predicate = x => x.CityId == cityId,
     SearchTerm = refServiceComboBox?.Text ?? "",
     PageIndex = pageIndex,
     PageSize = pageSize,
 });
 Services = result.Item1;
 totalCountService = result.PageCount;

 Services = (await Mediator.Send(new GetServiceQuery
 {
     Predicate = x => x.Id == ServiceForm.IdCardServiceId,
 })).Item1;

var data = (await Mediator.Send(new GetSingleServicesQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Service
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Service
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Service
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


#region ComboboxService

 private DxComboBox<ServiceDto, long?> refServiceComboBox { get; set; }
 private int ServiceComboBoxIndex { get; set; } = 0;
 private int totalCountService = 0;

 private async Task OnSearchService()
 {
     await LoadDataService();
 }

 private async Task OnSearchServiceIndexIncrement()
 {
     if (ServiceComboBoxIndex < (totalCountService - 1))
     {
         ServiceComboBoxIndex++;
         await LoadDataService(ServiceComboBoxIndex, 10);
     }
 }

 private async Task OnSearchServiceIndexDecrement()
 {
     if (ServiceComboBoxIndex > 0)
     {
         ServiceComboBoxIndex--;
         await LoadDataService(ServiceComboBoxIndex, 10);
     }
 }

 private async Task OnInputServiceChanged(string e)
 {
     ServiceComboBoxIndex = 0;
     await LoadDataService();
 }

 
  private async Task LoadDataService(int pageIndex = 0, int pageSize = 10)
  {
      try
      {
          PanelVisible = true;
          var result = await Mediator.Send(new GetServiceQuery
          {
              SearchTerm = refServiceComboBox?.Text ?? "",
              PageIndex = pageIndex,
              PageSize = pageSize,
          });
          Services = result.Item1;
          totalCountService = result.PageCount;
          PanelVisible = false;
      }
      catch (Exception ex)
      {
          ex.HandleException(ToastService);
      }
      finally { PanelVisible = false; }
  }

 #endregion ComboboxService

 <DxFormLayoutItem CaptionCssClass="required-caption normal-caption" Caption="Service" ColSpanMd="12">
    <MyDxComboBox Data="@Services"
                  NullText="Select Service"
                  @ref="refServiceComboBox"
                  @bind-Value="@a.ServiceId"
                  TextFieldName="Name"
                  ValueFieldName="Id"
                  TextChanged="((string e) => OnInputServiceChanged(e))">
        <Buttons>
            <DxEditorButton Click="OnSearchServiceIndexDecrement"
                            IconCssClass="fa-solid fa-caret-left"
                            Tooltip="Previous Index" />
            <DxEditorButton Click="OnSearchService"
                            IconCssClass="fa-solid fa-magnifying-glass"
                            Tooltip="Search" />
            <DxEditorButton Click="OnSearchServiceIndexIncrement"
                            IconCssClass="fa-solid fa-caret-right"
                            Tooltip="Next Index" />
        </Buttons>
        <Columns>
            <DxListEditorColumn FieldName="@nameof(ServiceDto.Name)" Caption="Name" />
            <DxListEditorColumn FieldName="Service.Name" Caption="Service" />
            <DxListEditorColumn FieldName="@nameof(ServiceDto.Code)" Caption="Code" />
        </Columns>
    </MyDxComboBox>
    <ValidationMessage For="@(()=>a.ServiceId)" />
</DxFormLayoutItem>

var result = await _unitOfWork.Repository<Service>().AddAsync(request.ServiceDto.Adapt<CreateUpdateServiceDto>().Adapt<Service>());
var result = await _unitOfWork.Repository<Service>().AddAsync(request.ServiceDtos.Adapt<List<CreateUpdateServiceDto>>().Adapt<List<Service>>()); 

var result = await _unitOfWork.Repository<Service>().UpdateAsync(request.ServiceDto.Adapt<CreateUpdateServiceDto>().Adapt<Service>());  
var result = await _unitOfWork.Repository<Service>().UpdateAsync(request.ServiceDtos.Adapt<List<CreateUpdateServiceDto>>().Adapt<List<Service>>());

list3 = (await Mediator.Send(new GetServiceQuery
{
    Predicate = x => ServiceNames.Contains(x.Name.ToLower()),
    IsGetAll = true,
    Select = x => new Service
    {
        Id = x.Id,
        Name = x.Name
    }
})).Item1;


#region Searching

    private int pageSizeServiceAttendance { get; set; } = 10;
    private int totalCountServiceAttendance = 0;
    private int activePageIndexServiceAttendance { get; set; } = 0;
    private string searchTermServiceAttendance { get; set; } = string.Empty;

    private async Task OnSearchBoxChangedServiceAttendance(string searchText)
    {
        searchTermServiceAttendance = searchText;
        await LoadDataOnSearchBoxChanged(0, pageSizeServiceAttendance);
    }

    private async Task OnpageSizeServiceAttendanceIndexChanged(int newpageSizeServiceAttendance)
    {
        pageSizeServiceAttendance = newpageSizeServiceAttendance;
        await LoadDataOnSearchBoxChanged(0, newpageSizeServiceAttendance);
    }

    private async Task OnPageIndexChangedOnSearchBoxChanged(int newPageIndex)
    {
        await LoadDataOnSearchBoxChanged(newPageIndex, pageSizeServiceAttendance);
    }
 private async Task LoadDataOnSearchBoxChanged(int pageIndex = 0, int pageSizeServiceAttendance = 10)
{
    try
    {
        PanelVisible = true;
        SelectedDataItems = new ObservableRangeCollection<object>();
        var result = await Mediator.Send(new GetServiceAttendanceQuery
        {
            PageIndex = pageIndex,
            PageSize = pageSizeServiceAttendance,
            SearchTerm = searchTermServiceAttendance,
        });
        ServiceAttendances = result.Item1;
        totalCountServiceAttendance = result.PageCount;
        activePageIndexServiceAttendance = pageIndex;
        PanelVisible = false;
    }
    catch (Exception ex)
    {
        ex.HandleException(ToastService);
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
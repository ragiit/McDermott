 public class GetSingleLocationQuery : IRequest<LocationDto>
 {
     public List<Expression<Func<Location, object>>> Includes { get; set; }
     public Expression<Func<Location, bool>> Predicate { get; set; }
     public Expression<Func<Location, Location>> Select { get; set; }

     public List<(Expression<Func<Location, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

     public bool IsDescending { get; set; } = false; // default to ascending
     public int PageIndex { get; set; } = 0;
     public int PageSize { get; set; } = 10;
     public bool IsGetAll { get; set; } = false;
     public string SearchTerm { get; set; }
 }

public class GetLocationQuery : IRequest<(List<LocationDto>, int PageIndex, int PageSize, int PageCount)>
{
    public List<Expression<Func<Location, object>>> Includes { get; set; }
    public Expression<Func<Location, bool>> Predicate { get; set; }
    public Expression<Func<Location, Location>> Select { get; set; }

    public List<(Expression<Func<Location, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public bool IsGetAll { get; set; } = false;
    public string SearchTerm { get; set; }
}
  
IRequestHandler<GetLocationQuery, (List<LocationDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleLocationQuery, LocationDto>,

public async Task<(List<LocationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLocationQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<Location>().Entities.AsNoTracking(); 

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
                    ? ((IOrderedQueryable<Location>)query).ThenByDescending(additionalOrderBy.OrderBy)
                    : ((IOrderedQueryable<Location>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    EF.Functions.Like(v.Location.Name, $"%{request.SearchTerm}%")
                    );
        }

        // Apply dynamic select if provided
        if (request.Select is not null)
            query = query.Select(request.Select);
        else
            query = query.Select(x => new Location
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

            return (pagedItems.Adapt<List<LocationDto>>(), request.PageIndex, request.PageSize, totalPages);
        }
        else
        {
            return ((await query.ToListAsync(cancellationToken)).Adapt<List<LocationDto>>(), 0, 1, 1);
        }
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}


  public async Task<LocationDto> Handle(GetSingleLocationQuery request, CancellationToken cancellationToken)
 {
     try
     {
         var query = _unitOfWork.Repository<Location>().Entities.AsNoTracking();

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
                     ? ((IOrderedQueryable<Location>)query).ThenByDescending(additionalOrderBy.OrderBy)
                     : ((IOrderedQueryable<Location>)query).ThenBy(additionalOrderBy.OrderBy);
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
                EF.Functions.Like(v.Location.Name, $"%{request.SearchTerm}%")
                );
         }

         // Apply dynamic select if provided
         if (request.Select is not null)
             query = query.Select(request.Select);
         else
             query = query.Select(x => new Location
             {
                 Id = x.Id, 
             });

         return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<LocationDto>();
     }
     catch (Exception ex)
     {
         // Consider logging the exception
         throw;
     }
 }




 var a = await Mediator.Send(new GetLocationsQuery
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

var patienss = (await Mediator.Send(new GetSingleLocationQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new Location
    {
        Id = x.Id,
        IsEmployee = x.IsEmployee,
        Name = x.Name,
        Gender = x.Gender,
        DateOfBirth = x.DateOfBirth
    },
})) ?? new();

 var result = await Mediator.Send(new GetLocationQuery
 { 
     SearchTerm = searchTerm,
     PageIndex = pageIndex,
     PageSize = pageSize,
 }); 
 Locations = result.Item1;
 activePageIndex = pageIndex;
 totalCount = result.PageCount;






var data = (await Mediator.Send(new GetSingleLocationsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new Location
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new Location
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new Location
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
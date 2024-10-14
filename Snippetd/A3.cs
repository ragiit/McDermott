 public class GetLocationsQuery(Expression<Func<Locations, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<LocationsDto>, int pageIndex, int pageSize, int pageCount)>
 {
     public Expression<Func<Locations, bool>> Predicate { get; } = predicate!;
     public bool RemoveCache { get; } = removeCache!;
     public string SearchTerm { get; } = searchTerm!;
     public int PageIndex { get; } = pageIndex;
     public int PageSize { get; set; } = pageSize ?? 10;
 }

public class GetLocationsQuery(Expression<Func<Locations, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Locations, object>>>? includes = null, Expression<Func<Locations, Locations>>? select = null) : IRequest<(List<LocationsDto>, int pageIndex, int pageSize, int pageCount)>
{
    public Expression<Func<Locations, bool>> Predicate { get; } = predicate!;
    public bool RemoveCache { get; } = removeCache!;
    public string SearchTerm { get; } = searchTerm!;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize ?? 10;

    public List<Expression<Func<Locations, object>>> Includes { get; } = includes!;
    public Expression<Func<Locations, Locations>>? Select { get; } = select!;
}

 public class BulkValidateLocationsQuery(List<LocationsDto> LocationssToValidate) : IRequest<List<LocationsDto>>
 {
     public List<LocationsDto> LocationssToValidate { get; } = LocationssToValidate;
 }

 public class ValidateLocationsQuery(Expression<Func<Locations, bool>>? predicate = null) : IRequest<bool>
 {
     public Expression<Func<Locations, bool>> Predicate { get; } = predicate!;
 }

IRequestHandler<GetLocationsQuery, (List<LocationsDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateLocationsQuery, bool>,
IRequestHandler<BulkValidateLocationsQuery, List<LocationsDto>>,


public async Task<List<LocationsDto>> Handle(BulkValidateLocationsQuery request, CancellationToken cancellationToken)
{
    var LocationsDtos = request.LocationssToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var LocationsNames = LocationsDtos.Select(x => x.Name).Distinct().ToList();
    var LocationsIds = LocationsDtos.Select(x => x.LocationsId).Distinct().ToList();

    var existingLocationss = await _unitOfWork.Repository<Locations>()
        .Entities
        .AsNoTracking()
        .Where(v => LocationsNames.Contains(v.Name)
                    && LocationsIds.Contains(v.LocationsId))
        .ToListAsync(cancellationToken);

    return existingLocationss.Adapt<List<LocationsDto>>();
}


public async Task<(List<LocationsDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
{
    try
    { 
        var query = _unitOfWork.Repository<Locations>().Entities.AsNoTracking();

        // Apply dynamic includes
        if (request.Includes is not null)
        {
            foreach (var includeExpression in request.Includes)
            {
                query = query.Include(includeExpression);
            }
        }

        if (request.Predicate is not null)
            query = query.Where(request.Predicate);

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
        } 

        // Apply dynamic select if provided
        if (request.Select is not null)
        {
            query = query.Select(request.Select);
        }

        var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                          query,
                          request.PageSize,
                          request.PageIndex,
                          q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                          cancellationToken);

        return (pagedItems.Adapt<List<LocationsDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

public async Task<bool> Handle(ValidateLocationsQuery request, CancellationToken cancellationToken)
{
    return await _unitOfWork.Repository<Locations>()
        .Entities
        .AsNoTracking()
        .Where(request.Predicate)  // Apply the Predicate for filtering
        .AnyAsync(cancellationToken);  // Check if any record matches the condition
}

public async Task<(List<LocationsDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<Locations>().Entities
            .AsNoTracking()
            .Include(v => v.Locations)
            .AsQueryable();

        if (request.Predicate is not null)
            query = query.Where(request.Predicate);

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                EF.Functions.Like(v.Locations.Name, $"%{request.SearchTerm}%"));
        }

        var pagedResult = query.OrderBy(x => x.Name);

        var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

        return (paged.Adapt<List<LocationsDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}
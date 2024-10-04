 public class GetSampleTypeQuery(Expression<Func<SampleType, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<SampleTypeDto>, int pageIndex, int pageSize, int pageCount)>
 {
     public Expression<Func<SampleType, bool>> Predicate { get; } = predicate!;
     public bool RemoveCache { get; } = removeCache!;
     public string SearchTerm { get; } = searchTerm!;
     public int PageIndex { get; } = pageIndex;
     public int PageSize { get; set; } = pageSize ?? 10;
 }

public class GetSampleTypeQuery(Expression<Func<SampleType, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<SampleType, object>>>? includes = null, Expression<Func<SampleType, SampleType>>? select = null) : IRequest<(List<SampleTypeDto>, int pageIndex, int pageSize, int pageCount)>
{
    public Expression<Func<SampleType, bool>> Predicate { get; } = predicate!;
    public bool RemoveCache { get; } = removeCache!;
    public string SearchTerm { get; } = searchTerm!;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize ?? 10;

    public List<Expression<Func<SampleType, object>>> Includes { get; } = includes!;
    public Expression<Func<SampleType, SampleType>>? Select { get; } = select!;
}

 public class BulkValidateSampleTypeQuery(List<SampleTypeDto> SampleTypesToValidate) : IRequest<List<SampleTypeDto>>
 {
     public List<SampleTypeDto> SampleTypesToValidate { get; } = SampleTypesToValidate;
 }

 public class ValidateSampleTypeQuery(Expression<Func<SampleType, bool>>? predicate = null) : IRequest<bool>
 {
     public Expression<Func<SampleType, bool>> Predicate { get; } = predicate!;
 }

IRequestHandler<GetSampleTypeQuery, (List<SampleTypeDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateSampleTypeQuery, bool>,
IRequestHandler<BulkValidateSampleTypeQuery, List<SampleTypeDto>>,


public async Task<List<SampleTypeDto>> Handle(BulkValidateSampleTypeQuery request, CancellationToken cancellationToken)
{
    var SampleTypeDtos = request.SampleTypesToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var SampleTypeNames = SampleTypeDtos.Select(x => x.Name).Distinct().ToList();
    var SampleTypeIds = SampleTypeDtos.Select(x => x.SampleTypeId).Distinct().ToList();

    var existingSampleTypes = await _unitOfWork.Repository<SampleType>()
        .Entities
        .AsNoTracking()
        .Where(v => SampleTypeNames.Contains(v.Name)
                    && SampleTypeIds.Contains(v.SampleTypeId))
        .ToListAsync(cancellationToken);

    return existingSampleTypes.Adapt<List<SampleTypeDto>>();
}


public async Task<(List<SampleTypeDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetSampleTypeQuery request, CancellationToken cancellationToken)
{
    try
    { 
        var query = _unitOfWork.Repository<SampleType>().Entities.AsNoTracking();

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

        return (pagedItems.Adapt<List<SampleTypeDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

public async Task<bool> Handle(ValidateSampleTypeQuery request, CancellationToken cancellationToken)
{
    return await _unitOfWork.Repository<SampleType>()
        .Entities
        .AsNoTracking()
        .Where(request.Predicate)  // Apply the Predicate for filtering
        .AnyAsync(cancellationToken);  // Check if any record matches the condition
}

public async Task<(List<SampleTypeDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetSampleTypeQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<SampleType>().Entities
            .AsNoTracking()
            .Include(v => v.SampleType)
            .AsQueryable();

        if (request.Predicate is not null)
            query = query.Where(request.Predicate);

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                EF.Functions.Like(v.SampleType.Name, $"%{request.SearchTerm}%"));
        }

        var pagedResult = query.OrderBy(x => x.Name);

        var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

        return (paged.Adapt<List<SampleTypeDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}
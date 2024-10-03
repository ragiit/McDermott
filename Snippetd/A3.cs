 public class GetLabUomQuery(Expression<Func<LabUom, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<LabUomDto>, int pageIndex, int pageSize, int pageCount)>
 {
     public Expression<Func<LabUom, bool>> Predicate { get; } = predicate!;
     public bool RemoveCache { get; } = removeCache!;
     public string SearchTerm { get; } = searchTerm!;
     public int PageIndex { get; } = pageIndex;
     public int PageSize { get; set; } = pageSize ?? 10;
 }

public class GetLabUomQuery(Expression<Func<LabUom, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<LabUom, object>>>? includes = null, Expression<Func<LabUom, LabUom>>? select = null) : IRequest<(List<LabUomDto>, int pageIndex, int pageSize, int pageCount)>
{
    public Expression<Func<LabUom, bool>> Predicate { get; } = predicate!;
    public bool RemoveCache { get; } = removeCache!;
    public string SearchTerm { get; } = searchTerm!;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize ?? 10;

    public List<Expression<Func<LabUom, object>>> Includes { get; } = includes!;
    public Expression<Func<LabUom, LabUom>>? Select { get; } = select!;
}

 public class BulkValidateLabUomQuery(List<LabUomDto> LabUomsToValidate) : IRequest<List<LabUomDto>>
 {
     public List<LabUomDto> LabUomsToValidate { get; } = LabUomsToValidate;
 }

 public class ValidateLabUomQuery(Expression<Func<LabUom, bool>>? predicate = null) : IRequest<bool>
 {
     public Expression<Func<LabUom, bool>> Predicate { get; } = predicate!;
 }

IRequestHandler<GetLabUomQuery, (List<LabUomDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateLabUomQuery, bool>,
IRequestHandler<BulkValidateLabUomQuery, List<LabUomDto>>,


public async Task<List<LabUomDto>> Handle(BulkValidateLabUomQuery request, CancellationToken cancellationToken)
{
    var LabUomDtos = request.LabUomsToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var LabUomNames = LabUomDtos.Select(x => x.Name).Distinct().ToList();
    var provinceIds = LabUomDtos.Select(x => x.ProvinceId).Distinct().ToList();

    var existingLabUoms = await _unitOfWork.Repository<LabUom>()
        .Entities
        .AsNoTracking()
        .Where(v => LabUomNames.Contains(v.Name)
                    && provinceIds.Contains(v.ProvinceId))
        .ToListAsync(cancellationToken);

    return existingLabUoms.Adapt<List<LabUomDto>>();
}


public async Task<(List<LabUomDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLabUomQuery request, CancellationToken cancellationToken)
{
    try
    { 
        var query = _unitOfWork.Repository<LabUom>().Entities.AsNoTracking();

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

        return (pagedItems.Adapt<List<LabUomDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

public async Task<bool> Handle(ValidateLabUomQuery request, CancellationToken cancellationToken)
{
    return await _unitOfWork.Repository<LabUom>()
        .Entities
        .AsNoTracking()
        .Where(request.Predicate)  // Apply the Predicate for filtering
        .AnyAsync(cancellationToken);  // Check if any record matches the condition
}

public async Task<(List<LabUomDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLabUomQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<LabUom>().Entities
            .AsNoTracking()
            .Include(v => v.Province)
            .AsQueryable();

        if (request.Predicate is not null)
            query = query.Where(request.Predicate);

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%"));
        }

        var pagedResult = query.OrderBy(x => x.Name);

        var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

        return (paged.Adapt<List<LabUomDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}
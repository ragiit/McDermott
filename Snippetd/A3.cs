 public class GetFamilyQuery(Expression<Func<Family, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<FamilyDto>, int pageIndex, int pageSize, int pageCount)>
 {
     public Expression<Func<Family, bool>> Predicate { get; } = predicate!;
     public bool RemoveCache { get; } = removeCache!;
     public string SearchTerm { get; } = searchTerm!;
     public int PageIndex { get; } = pageIndex;
     public int PageSize { get; set; } = pageSize ?? 10;
 }

 public class BulkValidateFamilyQuery(List<FamilyDto> FamilysToValidate) : IRequest<List<FamilyDto>>
 {
     public List<FamilyDto> FamilysToValidate { get; } = FamilysToValidate;
 }

 public class ValidateFamilyQuery(Expression<Func<Family, bool>>? predicate = null) : IRequest<bool>
 {
     public Expression<Func<Family, bool>> Predicate { get; } = predicate!;
 }

IRequestHandler<GetFamilyQuery, (List<FamilyDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateFamilyQuery, bool>,
IRequestHandler<BulkValidateFamilyQuery, List<FamilyDto>>,


public async Task<List<FamilyDto>> Handle(BulkValidateFamilyQuery request, CancellationToken cancellationToken)
{
    var FamilyDtos = request.FamilysToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var FamilyNames = FamilyDtos.Select(x => x.Name).Distinct().ToList();
    var provinceIds = FamilyDtos.Select(x => x.ProvinceId).Distinct().ToList();

    var existingFamilys = await _unitOfWork.Repository<Family>()
        .Entities
        .AsNoTracking()
        .Where(v => FamilyNames.Contains(v.Name)
                    && provinceIds.Contains(v.ProvinceId))
        .ToListAsync(cancellationToken);

    return existingFamilys.Adapt<List<FamilyDto>>();
}

public async Task<(List<FamilyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetFamilyQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<Family>().Entities
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

        return (paged.Adapt<List<FamilyDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

public async Task<bool> Handle(ValidateFamilyQuery request, CancellationToken cancellationToken)
{
    return await _unitOfWork.Repository<Family>()
        .Entities
        .AsNoTracking()
        .Where(request.Predicate)  // Apply the Predicate for filtering
        .AnyAsync(cancellationToken);  // Check if any record matches the condition
}

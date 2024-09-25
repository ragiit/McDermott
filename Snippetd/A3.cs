 public class GetInsuranceQuery(Expression<Func<Insurance, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<InsuranceDto>, int pageIndex, int pageSize, int pageCount)>
 {
     public Expression<Func<Insurance, bool>> Predicate { get; } = predicate!;
     public bool RemoveCache { get; } = removeCache!;
     public string SearchTerm { get; } = searchTerm!;
     public int PageIndex { get; } = pageIndex;
     public int PageSize { get; set; } = pageSize ?? 10;
 }

 public class BulkValidateInsuranceQuery(List<InsuranceDto> InsurancesToValidate) : IRequest<List<InsuranceDto>>
 {
     public List<InsuranceDto> InsurancesToValidate { get; } = InsurancesToValidate;
 }

 public class ValidateInsuranceQuery(Expression<Func<Insurance, bool>>? predicate = null) : IRequest<bool>
 {
     public Expression<Func<Insurance, bool>> Predicate { get; } = predicate!;
 }

IRequestHandler<GetInsuranceQuery, (List<InsuranceDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateInsuranceQuery, bool>,
IRequestHandler<BulkValidateInsuranceQuery, List<InsuranceDto>>,


public async Task<List<InsuranceDto>> Handle(BulkValidateInsuranceQuery request, CancellationToken cancellationToken)
{
    var InsuranceDtos = request.InsurancesToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var InsuranceNames = InsuranceDtos.Select(x => x.Name).Distinct().ToList();
    var provinceIds = InsuranceDtos.Select(x => x.ProvinceId).Distinct().ToList();

    var existingInsurances = await _unitOfWork.Repository<Insurance>()
        .Entities
        .AsNoTracking()
        .Where(v => InsuranceNames.Contains(v.Name)
                    && provinceIds.Contains(v.ProvinceId))
        .ToListAsync(cancellationToken);

    return existingInsurances.Adapt<List<InsuranceDto>>();
}

public async Task<(List<InsuranceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetInsuranceQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<Insurance>().Entities
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

        return (paged.Adapt<List<InsuranceDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

public async Task<bool> Handle(ValidateInsuranceQuery request, CancellationToken cancellationToken)
{
    return await _unitOfWork.Repository<Insurance>()
        .Entities
        .AsNoTracking()
        .Where(request.Predicate)  // Apply the Predicate for filtering
        .AnyAsync(cancellationToken);  // Check if any record matches the condition
}

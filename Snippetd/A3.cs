 public class GetCompanyQuery(Expression<Func<Company, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>
 {
     public Expression<Func<Company, bool>> Predicate { get; } = predicate!;
     public bool RemoveCache { get; } = removeCache!;
     public string SearchTerm { get; } = searchTerm!;
     public int PageIndex { get; } = pageIndex;
     public int PageSize { get; set; } = pageSize ?? 10;
 }

 public class BulkValidateCompanyQuery(List<CompanyDto> CompanysToValidate) : IRequest<List<CompanyDto>>
 {
     public List<CompanyDto> CompanysToValidate { get; } = CompanysToValidate;
 }

 public class ValidateCompanyQuery(Expression<Func<Company, bool>>? predicate = null) : IRequest<bool>
 {
     public Expression<Func<Company, bool>> Predicate { get; } = predicate!;
 }

IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateCompanyQuery, bool>,
IRequestHandler<BulkValidateCompanyQuery, List<CompanyDto>>,


public async Task<List<CompanyDto>> Handle(BulkValidateCompanyQuery request, CancellationToken cancellationToken)
{
    var CompanyDtos = request.CompanysToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var CompanyNames = CompanyDtos.Select(x => x.Name).Distinct().ToList();
    var provinceIds = CompanyDtos.Select(x => x.ProvinceId).Distinct().ToList();

    var existingCompanys = await _unitOfWork.Repository<Company>()
        .Entities
        .AsNoTracking()
        .Where(v => CompanyNames.Contains(v.Name)
                    && provinceIds.Contains(v.ProvinceId))
        .ToListAsync(cancellationToken);

    return existingCompanys.Adapt<List<CompanyDto>>();
}

public async Task<(List<CompanyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<Company>().Entities
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

        var pagedResult = query
                    .OrderBy(x => x.Name);

        var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

        return (paged.Adapt<List<CompanyDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

public async Task<bool> Handle(ValidateCompanyQuery request, CancellationToken cancellationToken)
{
    return await _unitOfWork.Repository<Company>()
        .Entities
        .AsNoTracking()
        .Where(request.Predicate)  // Apply the Predicate for filtering
        .AnyAsync(cancellationToken);  // Check if any record matches the condition
}

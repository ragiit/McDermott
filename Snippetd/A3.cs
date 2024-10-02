 public class GetProductCategoryQuery(Expression<Func<ProductCategory, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)>
 {
     public Expression<Func<ProductCategory, bool>> Predicate { get; } = predicate!;
     public bool RemoveCache { get; } = removeCache!;
     public string SearchTerm { get; } = searchTerm!;
     public int PageIndex { get; } = pageIndex;
     public int PageSize { get; set; } = pageSize ?? 10;
 }

 public class BulkValidateProductCategoryQuery(List<ProductCategoryDto> ProductCategorysToValidate) : IRequest<List<ProductCategoryDto>>
 {
     public List<ProductCategoryDto> ProductCategorysToValidate { get; } = ProductCategorysToValidate;
 }

 public class ValidateProductCategoryQuery(Expression<Func<ProductCategory, bool>>? predicate = null) : IRequest<bool>
 {
     public Expression<Func<ProductCategory, bool>> Predicate { get; } = predicate!;
 }

IRequestHandler<GetProductCategoryQuery, (List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateProductCategoryQuery, bool>,
IRequestHandler<BulkValidateProductCategoryQuery, List<ProductCategoryDto>>,


public async Task<List<ProductCategoryDto>> Handle(BulkValidateProductCategoryQuery request, CancellationToken cancellationToken)
{
    var ProductCategoryDtos = request.ProductCategorysToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var ProductCategoryNames = ProductCategoryDtos.Select(x => x.Name).Distinct().ToList();
    var provinceIds = ProductCategoryDtos.Select(x => x.ProvinceId).Distinct().ToList();

    var existingProductCategorys = await _unitOfWork.Repository<ProductCategory>()
        .Entities
        .AsNoTracking()
        .Where(v => ProductCategoryNames.Contains(v.Name)
                    && provinceIds.Contains(v.ProvinceId))
        .ToListAsync(cancellationToken);

    return existingProductCategorys.Adapt<List<ProductCategoryDto>>();
}

public async Task<(List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<ProductCategory>().Entities
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

        return (paged.Adapt<List<ProductCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

public async Task<bool> Handle(ValidateProductCategoryQuery request, CancellationToken cancellationToken)
{
    return await _unitOfWork.Repository<ProductCategory>()
        .Entities
        .AsNoTracking()
        .Where(request.Predicate)  // Apply the Predicate for filtering
        .AnyAsync(cancellationToken);  // Check if any record matches the condition
}

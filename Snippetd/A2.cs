public async Task<List<ProductCategoryDto>> Handle(BulkValidateProductCategoryQuery request, CancellationToken cancellationToken)
{
    var ProductCategoryDtos = request.ProductCategorysToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var ProductCategoryNames = ProductCategoryDtos.Select(x => x.Name).Distinct().ToList();
    var postalCodes = ProductCategoryDtos.Select(x => x.PostalCode).Distinct().ToList();
    var provinceIds = ProductCategoryDtos.Select(x => x.ProvinceId).Distinct().ToList();
    var cityIds = ProductCategoryDtos.Select(x => x.CityId).Distinct().ToList();
    var ProductCategoryIds = ProductCategoryDtos.Select(x => x.ProductCategoryId).Distinct().ToList();

    var existingProductCategorys = await _unitOfWork.Repository<ProductCategory>()
        .Entities
        .AsNoTracking()
        .Where(v => ProductCategoryNames.Contains(v.Name)
                    && postalCodes.Contains(v.PostalCode)
                    && provinceIds.Contains(v.ProvinceId)
                    && cityIds.Contains(v.CityId)
                    && ProductCategoryIds.Contains(v.ProductCategoryId))
        .ToListAsync(cancellationToken);

    return existingProductCategorys.Adapt<List<ProductCategoryDto>>();
}

public class BulkValidateProductCategoryQuery(List<ProductCategoryDto> ProductCategorysToValidate) : IRequest<List<ProductCategoryDto>>
{
    public List<ProductCategoryDto> ProductCategorysToValidate { get; } = ProductCategorysToValidate;
}


IRequestHandler<BulkValidateProductCategoryQuery, List<ProductCategoryDto>>,
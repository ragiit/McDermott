namespace McDermott.Application.Features.Commands.Inventory
{
    public class ProductCategoryCommand
    {
        #region GET

        public class GetAllProductCategoryQuery(Expression<Func<ProductCategory, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProductCategoryDto>>
        {
            public Expression<Func<ProductCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetProductCategoryQuery(Expression<Func<ProductCategory, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<ProductCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateProductCategoryQuery(Expression<Func<ProductCategory, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<ProductCategory, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateProductCategoryRequest(ProductCategoryDto ProductCategoryDto) : IRequest<ProductCategoryDto>
        {
            public ProductCategoryDto ProductCategoryDto { get; set; } = ProductCategoryDto;
        }

        public class CreateListProductCategoryRequest(List<ProductCategoryDto> ProductCategoryDtos) : IRequest<List<ProductCategoryDto>>
        {
            public List<ProductCategoryDto> ProductCategoryDtos { get; set; } = ProductCategoryDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateProductCategoryRequest(ProductCategoryDto ProductCategoryDto) : IRequest<ProductCategoryDto>
        {
            public ProductCategoryDto ProductCategoryDto { get; set; } = ProductCategoryDto;
        }

        public class UpdateListProductCategoryRequest(List<ProductCategoryDto> ProductCategoryDtos) : IRequest<List<ProductCategoryDto>>
        {
            public List<ProductCategoryDto> ProductCategoryDtos { get; set; } = ProductCategoryDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteProductCategoryRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
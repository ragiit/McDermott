namespace McDermott.Application.Features.Commands.Inventory
{
    public class ProductCategoryCommand
    {
        #region GET

        public class GetProductCategoryQuery(Expression<Func<ProductCategory, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProductCategoryDto>>
        {
            public Expression<Func<ProductCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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
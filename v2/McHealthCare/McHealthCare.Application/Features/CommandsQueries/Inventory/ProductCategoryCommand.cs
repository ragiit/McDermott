namespace McHealthCare.Application.Features.Commands.Inventory
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

        public class DeleteProductCategoryRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
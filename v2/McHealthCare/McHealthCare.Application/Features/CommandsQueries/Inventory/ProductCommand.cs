namespace McHealthCare.Application.Features.Commands.Inventory
{
    public class ProductCommand
    {
        #region GET

        public class GetProductQuery(Expression<Func<Product, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProductDto>>
        {
            public Expression<Func<Product, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateProductRequest(ProductDto ProductDto) : IRequest<ProductDto>
        {
            public ProductDto ProductDto { get; set; } = ProductDto;
        }

        public class CreateListProductRequest(List<ProductDto> ProductDtos) : IRequest<List<ProductDto>>
        {
            public List<ProductDto> ProductDtos { get; set; } = ProductDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateProductRequest(ProductDto ProductDto) : IRequest<ProductDto>
        {
            public ProductDto ProductDto { get; set; } = ProductDto;
        }

        public class UpdateListProductRequest(List<ProductDto> ProductDtos) : IRequest<List<ProductDto>>
        {
            public List<ProductDto> ProductDtos { get; set; } = ProductDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteProductRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
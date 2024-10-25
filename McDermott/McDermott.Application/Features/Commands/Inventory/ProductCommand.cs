

namespace McDermott.Application.Features.Commands.Inventory
{
    public class ProductCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)
        public class GetAllProductQuery(Expression<Func<Product, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProductDto>>
        {
            public Expression<Func<Product, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetProductQuery(Expression<Func<Product, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<ProductDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Product, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateProductQuery(Expression<Func<Product, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Product, bool>> Predicate { get; } = predicate!;
        }

        public class GetSingleProductQueryNew : IRequest<ProductDto>
        {
            public List<Expression<Func<Product, object>>> Includes { get; set; }
            public Expression<Func<Product, bool>> Predicate { get; set; }
            public Expression<Func<Product, Product>> Select { get; set; }

            public List<(Expression<Func<Product, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetProductQueryNew : IRequest<(List<ProductDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Product, object>>> Includes { get; set; }
            public Expression<Func<Product, bool>> Predicate { get; set; }
            public Expression<Func<Product, Product>> Select { get; set; }

            public List<(Expression<Func<Product, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateProductRequest(ProductDto ProductDto) : IRequest<ProductDto>
        {
            public ProductDto ProductDto { get; set; } = ProductDto;
        }

        public class CreateListProductRequest(List<ProductDto> GeneralConsultanCPPTDtos) : IRequest<List<ProductDto>>
        {
            public List<ProductDto> ProductDtos { get; set; } = GeneralConsultanCPPTDtos;
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

        public class DeleteProductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

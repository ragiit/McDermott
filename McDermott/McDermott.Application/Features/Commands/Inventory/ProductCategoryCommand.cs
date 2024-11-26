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

        public class GetSingleProductCategoryQuery : IRequest<ProductCategoryDto>
        {
            public List<Expression<Func<ProductCategory, object>>> Includes { get; set; }
            public Expression<Func<ProductCategory, bool>> Predicate { get; set; }
            public Expression<Func<ProductCategory, ProductCategory>> Select { get; set; }

            public List<(Expression<Func<ProductCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetProductCategoryQuery : IRequest<(List<ProductCategoryDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<ProductCategory, object>>> Includes { get; set; }
            public Expression<Func<ProductCategory, bool>> Predicate { get; set; }
            public Expression<Func<ProductCategory, ProductCategory>> Select { get; set; }

            public List<(Expression<Func<ProductCategory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateProductCategoryQuery(List<ProductCategoryDto> ProductCategorysToValidate) : IRequest<List<ProductCategoryDto>>
        {
            public List<ProductCategoryDto> ProductCategorysToValidate { get; } = ProductCategorysToValidate;
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
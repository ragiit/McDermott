using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacy
{
   public class ProductCommand
    {
        #region GET 

        public class GetProductQuery(Expression<Func<Product, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProductDto>>
        {
            public Expression<Func<Product, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

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

        public class DeleteProductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

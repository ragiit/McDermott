using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Inventory.ProductCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class ProductQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProductQuery, List<ProductDto>>,
        IRequestHandler<CreateProductRequest, ProductDto>,
        IRequestHandler<CreateListProductRequest, List<ProductDto>>,
        IRequestHandler<UpdateProductRequest, ProductDto>,
        IRequestHandler<UpdateListProductRequest, List<ProductDto>>,
        IRequestHandler<DeleteProductRequest, bool>
    {
        #region GET

        public async Task<List<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetProductQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Product>? result))
                {
                    result = await _unitOfWork.Repository<Product>().Entities
                      .Include(x=>x.Medicaments)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ProductDto> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Product>().AddAsync(request.ProductDto.Adapt<Product>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductDto>> Handle(CreateListProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Product>().AddAsync(request.ProductDtos.Adapt<List<Product>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProductDto> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Product>().UpdateAsync(request.ProductDto.Adapt<Product>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductDto>> Handle(UpdateListProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Product>().UpdateAsync(request.ProductDtos.Adapt<List<Product>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Product>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Product>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai


                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
    }
}

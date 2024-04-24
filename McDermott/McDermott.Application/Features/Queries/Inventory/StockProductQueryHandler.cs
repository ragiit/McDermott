using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Inventory.StockProductCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class StockProductQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetStockProductQuery, List<StockProductDto>>,
         IRequestHandler<CreateStockProductRequest, StockProductDto>,
         IRequestHandler<CreateListStockProductRequest, List<StockProductDto>>,
         IRequestHandler<UpdateStockProductRequest, StockProductDto>,
         IRequestHandler<UpdateListStockProductRequest, List<StockProductDto>>,
         IRequestHandler<DeleteStockProductRequest, bool>
    {
        #region GET

        public async Task<List<StockProductDto>> Handle(GetStockProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetStockProductQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<StockProduct>? result))
                {
                    result = await _unitOfWork.Repository<StockProduct>().Entities
                        .Include(x=>x.Product)
                        .Include(x=>x.Source)
                        .Include(x=>x.Destinance)
                        .Include(x=>x.Uom)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<StockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<StockProductDto> Handle(CreateStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockProduct>().AddAsync(request.StockProductDto.Adapt<StockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockProductQuery_");

                return result.Adapt<StockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockProductDto>> Handle(CreateListStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockProduct>().AddAsync(request.StockProductDtos.Adapt<List<StockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<StockProductDto> Handle(UpdateStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockProduct>().UpdateAsync(request.StockProductDto.Adapt<StockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockProductDto>> Handle(UpdateListStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockProduct>().UpdateAsync(request.StockProductDtos.Adapt<List<StockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<StockProduct>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<StockProduct>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockProductQuery_"); // Ganti dengan key yang sesuai

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

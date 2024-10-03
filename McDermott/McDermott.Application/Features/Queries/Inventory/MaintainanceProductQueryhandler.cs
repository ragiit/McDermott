using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Inventory.MaintainanceProductCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintainanceProductQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
    IRequestHandler<GetMaintainanceProductQuery, (List<MaintainanceProductDto>, int pageIndex, int pageSize, int pageCount)>,
    IRequestHandler<GetAllMaintainanceProductQuery, List<MaintainanceProductDto>>,
    IRequestHandler<ValidateMaintainanceProductQuery, bool>,
    IRequestHandler<CreateMaintainanceProductRequest, MaintainanceProductDto>,
    IRequestHandler<CreateListMaintainanceProductRequest, List<MaintainanceProductDto>>,
    IRequestHandler<UpdateMaintainanceProductRequest, MaintainanceProductDto>,
    IRequestHandler<UpdateListMaintainanceProductRequest, List<MaintainanceProductDto>>,
    IRequestHandler<DeleteMaintainanceProductRequest, bool>
    {
        #region GET

        public async Task<List<MaintainanceProductDto>> Handle(GetAllMaintainanceProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMaintainanceProductQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<MaintainanceProduct>? result))
                {
                    result = await _unitOfWork.Repository<MaintainanceProduct>().Entities
                    .Include(x => x.Product)
                    .Include(x => x.Maintainance)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaintainanceProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<MaintainanceProductDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaintainanceProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MaintainanceProduct>().Entities
                    .Include(x => x.Maintainance)
                    .Include(x => x.Product)
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.ProductId.ToString(), $"%{request.SearchTerm}%") );
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.CreatedDate);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<MaintainanceProductDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateMaintainanceProductQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<MaintainanceProduct>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<MaintainanceProductDto> Handle(CreateMaintainanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintainanceProduct>().AddAsync(request.MaintainanceProductDto.Adapt<MaintainanceProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintainanceProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintainanceProductDto>> Handle(CreateListMaintainanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintainanceProduct>().AddAsync(request.MaintainanceProductDtos.Adapt<List<MaintainanceProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintainanceProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MaintainanceProductDto> Handle(UpdateMaintainanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintainanceProduct>().UpdateAsync(request.MaintainanceProductDto.Adapt<MaintainanceProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintainanceProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintainanceProductDto>> Handle(UpdateListMaintainanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintainanceProduct>().UpdateAsync(request.MaintainanceProductDtos.Adapt<List<MaintainanceProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintainanceProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMaintainanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<MaintainanceProduct>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<MaintainanceProduct>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintainanceProductQuery_"); // Ganti dengan key yang sesuai

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

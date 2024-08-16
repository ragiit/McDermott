using static McHealthCare.Application.Features.Commands.Inventory.UomCommand;

namespace McHealthCare.Application.Features.Queries.Inventory
{
    public class UomQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetUomQuery, List<UomDto>>,
        IRequestHandler<CreateUomRequest, UomDto>,
        IRequestHandler<CreateListUomRequest, List<UomDto>>,
        IRequestHandler<UpdateUomRequest, UomDto>,
        IRequestHandler<UpdateListUomRequest, List<UomDto>>,
        IRequestHandler<DeleteUomRequest, bool>
    {
        #region GET

        public async Task<List<UomDto>> Handle(GetUomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetUomQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Uom>? result))
                {
                    result = await _unitOfWork.Repository<Uom>().Entities
                        .Include(x => x.UomCategory)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<UomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<UomDto> Handle(CreateUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Uom>().AddAsync(request.UomDto.Adapt<Uom>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<UomDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UomDto>> Handle(CreateListUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Uom>().AddAsync(request.UomDtos.Adapt<List<Uom>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<UomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<UomDto> Handle(UpdateUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Uom>().UpdateAsync(request.UomDto.Adapt<Uom>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<UomDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UomDto>> Handle(UpdateListUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Uom>().UpdateAsync(request.UomDtos.Adapt<List<Uom>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<UomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<Uom>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Uom>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

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
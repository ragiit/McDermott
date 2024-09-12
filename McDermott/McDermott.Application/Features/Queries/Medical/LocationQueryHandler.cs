

namespace McDermott.Application.Features.Queries.Medical
{
    public class LocationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLocationQuery, List<LocationDto>>,
        IRequestHandler<CreateLocationRequest, LocationDto>,
        IRequestHandler<CreateListLocationRequest, List<LocationDto>>,
        IRequestHandler<UpdateLocationRequest, LocationDto>,
        IRequestHandler<UpdateListLocationRequest, List<LocationDto>>,
        IRequestHandler<DeleteLocationRequest, bool>
    {
        #region GET

        public async Task<List<LocationDto>> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetLocationQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Location>? result))
                {
                    result = await _unitOfWork.Repository<Location>().Entities
                       .Include(x => x.ParentLocation)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<LocationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<LocationDto> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().AddAsync(request.LocationDto.Adapt<Location>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LocationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LocationDto>> Handle(CreateListLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().AddAsync(request.LocationDtos.Adapt<List<Location>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LocationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LocationDto> Handle(UpdateLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().UpdateAsync(request.LocationDto.Adapt<Location>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LocationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LocationDto>> Handle(UpdateListLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().UpdateAsync(request.LocationDtos.Adapt<List<Location>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LocationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    var result = await _unitOfWork.Repository<Location>().Entities.Where(x => x.ParentLocationId == request.Id).ToListAsync(cancellationToken);

                    foreach (var item in result)
                    {
                        item.ParentLocationId = null;
                    }

                    await _unitOfWork.Repository<Location>().UpdateAsync(result);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    //await _unitOfWork.Repository<Location>().DeleteAsync(x => result.Select(z => z.Id).Contains(x.Id));

                    await _unitOfWork.Repository<Location>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    foreach (var item in request.Ids)
                    {
                        var result = await _unitOfWork.Repository<Location>().Entities.Where(x => x.ParentLocationId == item).ToListAsync(cancellationToken);

                        foreach (var item2 in result)
                        {
                            item2.ParentLocationId = null;
                        }

                        await _unitOfWork.Repository<Location>().UpdateAsync(result);
                    }
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _unitOfWork.Repository<Location>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

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
using static McDermott.Application.Features.Commands.Config.CityCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class CityQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCityQuery, List<CityDto>>,
        IRequestHandler<CreateCityRequest, CityDto>,
        IRequestHandler<CreateListCityRequest, List<CityDto>>,
        IRequestHandler<UpdateCityRequest, CityDto>,
        IRequestHandler<UpdateListCityRequest, List<CityDto>>,
        IRequestHandler<DeleteCityRequest, bool>
    {
        #region GET

        public async Task<List<CityDto>> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetCityQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<City>? result))
                {
                    result = await _unitOfWork.Repository<City>().Entities
                        .Include(x => x.Province)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<CityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<CityDto> Handle(CreateCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<City>().AddAsync(request.CityDto.Adapt<City>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CityDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CityDto>> Handle(CreateListCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<City>().AddAsync(request.CityDtos.Adapt<List<City>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CityDto> Handle(UpdateCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<City>().UpdateAsync(request.CityDto.Adapt<City>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CityDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CityDto>> Handle(UpdateListCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<City>().UpdateAsync(request.CityDtos.Adapt<List<City>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CityDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<City>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<City>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCityQuery_"); // Ganti dengan key yang sesuai

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
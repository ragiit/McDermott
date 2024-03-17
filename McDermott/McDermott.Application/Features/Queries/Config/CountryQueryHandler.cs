using static McDermott.Application.Features.Commands.Config.CountryCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class CountryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCountryQuery, List<CountryDto>>,
        IRequestHandler<CreateCountryRequest, CountryDto>,
        IRequestHandler<CreateListCountryRequest, List<CountryDto>>,
        IRequestHandler<UpdateCountryRequest, CountryDto>,
        IRequestHandler<UpdateListCountryRequest, List<CountryDto>>,
        IRequestHandler<DeleteCountryRequest, bool>
    {

        #region GET
        public async Task<List<CountryDto>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetCountryQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Country>? result))
                {
                    result = await _unitOfWork.Repository<Country>().GetAsync(
                        null,
                        cancellationToken: cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<CountryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CREATE
        public async Task<CountryDto> Handle(CreateCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDto.Adapt<Country>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<CountryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CountryDto>> Handle(CreateListCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDtos.Adapt<List<Country>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CountryDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UPDATE
        public async Task<CountryDto> Handle(UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDto.Adapt<Country>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CountryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CountryDto>> Handle(UpdateListCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDtos.Adapt<List<Country>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CountryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DELETE
        public async Task<bool> Handle(DeleteCountryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Country>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Country>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCountryQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
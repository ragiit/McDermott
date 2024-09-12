using static McDermott.Application.Features.Commands.Config.ProvinceCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class ProvinceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProvinceQuery, List<ProvinceDto>>,
        IRequestHandler<CreateProvinceRequest, ProvinceDto>,
        IRequestHandler<CreateListProvinceRequest, List<ProvinceDto>>,
        IRequestHandler<UpdateProvinceRequest, ProvinceDto>,
        IRequestHandler<UpdateListProvinceRequest, List<ProvinceDto>>,
        IRequestHandler<DeleteProvinceRequest, bool>
    {
        #region GET

        public async Task<List<ProvinceDto>> Handle(GetProvinceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetProvinceQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Province>? result))
                {
                    result = await _unitOfWork.Repository<Province>().Entities
                        .Include(x => x.Country)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ProvinceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ProvinceDto> Handle(CreateProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().AddAsync(request.ProvinceDto.Adapt<Province>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProvinceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProvinceDto>> Handle(CreateListProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().AddAsync(request.ProvinceDtos.Adapt<List<Province>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProvinceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProvinceDto> Handle(UpdateProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().UpdateAsync(request.ProvinceDto.Adapt<Province>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProvinceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProvinceDto>> Handle(UpdateListProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().UpdateAsync(request.ProvinceDtos.Adapt<List<Province>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProvinceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProvinceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Province>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Province>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProvinceQuery_"); // Ganti dengan key yang sesuai

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
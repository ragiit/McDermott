using static McDermott.Application.Features.Commands.Config.DistrictCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class DistrictQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDistrictQuery, List<DistrictDto>>,
        IRequestHandler<CreateDistrictRequest, DistrictDto>,
        IRequestHandler<CreateListDistrictRequest, List<DistrictDto>>,
        IRequestHandler<UpdateDistrictRequest, DistrictDto>,
        IRequestHandler<UpdateListDistrictRequest, List<DistrictDto>>,
        IRequestHandler<DeleteDistrictRequest, bool>
    {
        #region GET

        public async Task<List<DistrictDto>> Handle(GetDistrictQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDistrictQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<District>? result))
                {
                    result = await _unitOfWork.Repository<District>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.City)
                        .Include(z => z.Province),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DistrictDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<DistrictDto> Handle(CreateDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<District>().AddAsync(request.DistrictDto.Adapt<District>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DistrictDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DistrictDto>> Handle(CreateListDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<District>().AddAsync(request.DistrictDtos.Adapt<List<District>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DistrictDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DistrictDto> Handle(UpdateDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<District>().UpdateAsync(request.DistrictDto.Adapt<District>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DistrictDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DistrictDto>> Handle(UpdateListDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<District>().UpdateAsync(request.DistrictDtos.Adapt<List<District>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DistrictDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDistrictRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<District>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<District>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDistrictQuery_"); // Ganti dengan key yang sesuai

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
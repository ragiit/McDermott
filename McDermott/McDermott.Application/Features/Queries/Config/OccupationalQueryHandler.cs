using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class OccupationalQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetOccupationalQuery, List<OccupationalDto>>,
        IRequestHandler<CreateOccupationalRequest, OccupationalDto>,
        IRequestHandler<CreateListOccupationalRequest, List<OccupationalDto>>,
        IRequestHandler<UpdateOccupationalRequest, OccupationalDto>,
        IRequestHandler<UpdateListOccupationalRequest, List<OccupationalDto>>,
        IRequestHandler<DeleteOccupationalRequest, bool>
    {
        #region GET

        public async Task<List<OccupationalDto>> Handle(GetOccupationalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetOccupationalQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Occupational>? result))
                {
                    result = await _unitOfWork.Repository<Occupational>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<OccupationalDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<OccupationalDto> Handle(CreateOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Occupational>().AddAsync(request.OccupationalDto.Adapt<Occupational>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<OccupationalDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OccupationalDto>> Handle(CreateListOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Occupational>().AddAsync(request.OccupationalDtos.Adapt<List<Occupational>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<OccupationalDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<OccupationalDto> Handle(UpdateOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Occupational>().UpdateAsync(request.OccupationalDto.Adapt<Occupational>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<OccupationalDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OccupationalDto>> Handle(UpdateListOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Occupational>().UpdateAsync(request.OccupationalDtos.Adapt<List<Occupational>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<OccupationalDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Occupational>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Occupational>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

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
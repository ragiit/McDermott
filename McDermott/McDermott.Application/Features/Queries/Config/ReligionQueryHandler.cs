namespace McDermott.Application.Features.Queries.Config
{
    public class ReligionQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetReligionQuery, List<ReligionDto>>,
        IRequestHandler<CreateReligionRequest, ReligionDto>,
        IRequestHandler<CreateListReligionRequest, List<ReligionDto>>,
        IRequestHandler<UpdateReligionRequest, ReligionDto>,
        IRequestHandler<UpdateListReligionRequest, List<ReligionDto>>,
        IRequestHandler<DeleteReligionRequest, bool>
    {
        #region GET

        public async Task<List<ReligionDto>> Handle(GetReligionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetReligionQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Religion>? result))
                {
                    result = await _unitOfWork.Repository<Religion>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ReligionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ReligionDto> Handle(CreateReligionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Religion>().AddAsync(request.ReligionDto.Adapt<Religion>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReligionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ReligionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReligionDto>> Handle(CreateListReligionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Religion>().AddAsync(request.ReligionDtos.Adapt<List<Religion>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReligionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReligionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ReligionDto> Handle(UpdateReligionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Religion>().UpdateAsync(request.ReligionDto.Adapt<Religion>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReligionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ReligionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReligionDto>> Handle(UpdateListReligionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Religion>().UpdateAsync(request.ReligionDtos.Adapt<List<Religion>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReligionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReligionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteReligionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Religion>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Religion>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReligionQuery_"); // Ganti dengan key yang sesuai

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
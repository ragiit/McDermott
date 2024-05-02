namespace McDermott.Application.Features.Queries.Bpjs
{
    public class BPJSIntegrationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetBPJSIntegrationQuery, List<BPJSIntegrationDto>>,
        IRequestHandler<CreateBPJSIntegrationRequest, BPJSIntegrationDto>,
        IRequestHandler<CreateListBPJSIntegrationRequest, List<BPJSIntegrationDto>>,
        IRequestHandler<UpdateBPJSIntegrationRequest, BPJSIntegrationDto>,
        IRequestHandler<UpdateListBPJSIntegrationRequest, List<BPJSIntegrationDto>>,
        IRequestHandler<DeleteBPJSIntegrationRequest, bool>
    {
        #region GET

        public async Task<List<BPJSIntegrationDto>> Handle(GetBPJSIntegrationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetBPJSIntegrationQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<BPJSIntegration>? result))
                {
                    result = await _unitOfWork.Repository<BPJSIntegration>().Entities
                       .Include(x => x.InsurancePolicy)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<BPJSIntegrationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<BPJSIntegrationDto> Handle(CreateBPJSIntegrationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BPJSIntegration>().AddAsync(request.BPJSIntegrationDto.Adapt<BPJSIntegration>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBPJSIntegrationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<BPJSIntegrationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BPJSIntegrationDto>> Handle(CreateListBPJSIntegrationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BPJSIntegration>().AddAsync(request.BPJSIntegrationDtos.Adapt<List<BPJSIntegration>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBPJSIntegrationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<BPJSIntegrationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<BPJSIntegrationDto> Handle(UpdateBPJSIntegrationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BPJSIntegration>().UpdateAsync(request.BPJSIntegrationDto.Adapt<BPJSIntegration>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBPJSIntegrationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<BPJSIntegrationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BPJSIntegrationDto>> Handle(UpdateListBPJSIntegrationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BPJSIntegration>().UpdateAsync(request.BPJSIntegrationDtos.Adapt<List<BPJSIntegration>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBPJSIntegrationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<BPJSIntegrationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteBPJSIntegrationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<BPJSIntegration>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<BPJSIntegration>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBPJSIntegrationQuery_"); // Ganti dengan key yang sesuai

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

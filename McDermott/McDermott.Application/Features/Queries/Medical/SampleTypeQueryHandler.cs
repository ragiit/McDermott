namespace McDermott.Application.Features.Queries.Medical
{
    public class SampleTypeQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetSampleTypeQuery, List<SampleTypeDto>>,
        IRequestHandler<CreateSampleTypeRequest, SampleTypeDto>,
        IRequestHandler<CreateListSampleTypeRequest, List<SampleTypeDto>>,
        IRequestHandler<UpdateSampleTypeRequest, SampleTypeDto>,
        IRequestHandler<UpdateListSampleTypeRequest, List<SampleTypeDto>>,
        IRequestHandler<DeleteSampleTypeRequest, bool>
    {
        #region GET

        public async Task<List<SampleTypeDto>> Handle(GetSampleTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetSampleTypeQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<SampleType>? result))
                {
                    result = await _unitOfWork.Repository<SampleType>().GetAsync(
                        null,
                        null,
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<SampleTypeDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<SampleTypeDto> Handle(CreateSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SampleType>().AddAsync(request.SampleTypeDto.Adapt<SampleType>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SampleTypeDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SampleTypeDto>> Handle(CreateListSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SampleType>().AddAsync(request.SampleTypeDtos.Adapt<List<SampleType>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SampleTypeDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SampleTypeDto> Handle(UpdateSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SampleType>().UpdateAsync(request.SampleTypeDto.Adapt<SampleType>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SampleTypeDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SampleTypeDto>> Handle(UpdateListSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SampleType>().UpdateAsync(request.SampleTypeDtos.Adapt<List<SampleType>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SampleTypeDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<SampleType>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<SampleType>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

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

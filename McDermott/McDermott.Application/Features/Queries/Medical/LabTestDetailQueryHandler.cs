namespace McDermott.Application.Features.Queries.Medical
{
    public class LabTestDetailQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLabTestDetailQuery, List<LabTestDetailDto>>,
        IRequestHandler<CreateLabTestDetailRequest, LabTestDetailDto>,
        IRequestHandler<CreateListLabTestDetailRequest, List<LabTestDetailDto>>,
        IRequestHandler<UpdateLabTestDetailRequest, LabTestDetailDto>,
        IRequestHandler<UpdateListLabTestDetailRequest, List<LabTestDetailDto>>,
        IRequestHandler<DeleteLabTestDetailRequest, bool>
    {
        #region GET

        public async Task<List<LabTestDetailDto>> Handle(GetLabTestDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetLabTestDetailQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<LabTestDetail>? result))
                {
                    result = await _unitOfWork.Repository<LabTestDetail>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.LabTest)
                        .Include(x => x.LabUom),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<LabTestDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<LabTestDetailDto> Handle(CreateLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTestDetail>().AddAsync(request.LabTestDetailDto.Adapt<LabTestDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabTestDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabTestDetailDto>> Handle(CreateListLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTestDetail>().AddAsync(request.LabTestDetailDtos.Adapt<List<LabTestDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabTestDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabTestDetailDto> Handle(UpdateLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTestDetail>().UpdateAsync(request.LabTestDetailDto.Adapt<LabTestDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabTestDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabTestDetailDto>> Handle(UpdateListLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTestDetail>().UpdateAsync(request.LabTestDetailDtos.Adapt<List<LabTestDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabTestDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<LabTestDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<LabTestDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

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

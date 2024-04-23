

using static McDermott.Application.Features.Commands.Transaction.LabResultDetailCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class LabResultDetailQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLabResultDetailQuery, List<LabResultDetailDto>>,
        IRequestHandler<CreateLabResultDetailRequest, LabResultDetailDto>,
        IRequestHandler<CreateListLabResultDetailRequest, List<LabResultDetailDto>>,
        IRequestHandler<UpdateLabResultDetailRequest, LabResultDetailDto>,
        IRequestHandler<UpdateListLabResultDetailRequest, List<LabResultDetailDto>>,
        IRequestHandler<DeleteLabResultDetailRequest, bool>
    {
        #region GET

        public async Task<List<LabResultDetailDto>> Handle(GetLabResultDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetLabResultDetailQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<LabResultDetail>? result))
                {
                    result = await _unitOfWork.Repository<LabResultDetail>().Entities
                      .Include(x => x.LabUom)
                      .Include(x => x.GeneralConsultanMedicalSupport)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<LabResultDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<LabResultDetailDto> Handle(CreateLabResultDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabResultDetail>().AddAsync(request.LabResultDetailDto.Adapt<LabResultDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabResultDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabResultDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabResultDetailDto>> Handle(CreateListLabResultDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabResultDetail>().AddAsync(request.LabResultDetailDtos.Adapt<List<LabResultDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabResultDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabResultDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabResultDetailDto> Handle(UpdateLabResultDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabResultDetail>().UpdateAsync(request.LabResultDetailDto.Adapt<LabResultDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabResultDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabResultDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabResultDetailDto>> Handle(UpdateListLabResultDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabResultDetail>().UpdateAsync(request.LabResultDetailDtos.Adapt<List<LabResultDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabResultDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabResultDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabResultDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<LabResultDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<LabResultDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabResultDetailQuery_"); // Ganti dengan key yang sesuai

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

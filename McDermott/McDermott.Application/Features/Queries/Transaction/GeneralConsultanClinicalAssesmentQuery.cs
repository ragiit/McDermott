using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanClinicalAssesmentCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanClinicalAssesmentQuery(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralConsultantClinicalAssesmentQuery, List<GeneralConsultantClinicalAssesmentDto>>,
        IRequestHandler<CreateGeneralConsultantClinicalAssesmentRequest, GeneralConsultantClinicalAssesmentDto>,
        IRequestHandler<CreateListGeneralConsultantClinicalAssesmentRequest, List<GeneralConsultantClinicalAssesmentDto>>,
        IRequestHandler<UpdateGeneralConsultantClinicalAssesmentRequest, GeneralConsultantClinicalAssesmentDto>,
        IRequestHandler<UpdateListGeneralConsultantClinicalAssesmentRequest, List<GeneralConsultantClinicalAssesmentDto>>,
        IRequestHandler<DeleteGeneralConsultantClinicalAssesmentRequest, bool>
    {
        #region GET

        public async Task<List<GeneralConsultantClinicalAssesmentDto>> Handle(GetGeneralConsultantClinicalAssesmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultanClinicalAssesmentQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultantClinicalAssesment>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().GetAsync(
                        null,
                        null,
                        cancellationToken: cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultantClinicalAssesmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region Create

        public async Task<GeneralConsultantClinicalAssesmentDto> Handle(CreateGeneralConsultantClinicalAssesmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().AddAsync(request.GeneralConsultantClinicalAssesmentDto.Adapt<GeneralConsultantClinicalAssesment>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanClinicalAssesmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultantClinicalAssesmentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultantClinicalAssesmentDto>> Handle(CreateListGeneralConsultantClinicalAssesmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().AddAsync(request.GeneralConsultantClinicalAssesmentDtos.Adapt<List<GeneralConsultantClinicalAssesment>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanClinicalAssesmentQuery_");

                return result.Adapt<List<GeneralConsultantClinicalAssesmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Create

        #region Update

        public async Task<GeneralConsultantClinicalAssesmentDto> Handle(UpdateGeneralConsultantClinicalAssesmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().UpdateAsync(request.GeneralConsultantClinicalAssesmentDto.Adapt<GeneralConsultantClinicalAssesment>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanClinicalAssesmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultantClinicalAssesmentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultantClinicalAssesmentDto>> Handle(UpdateListGeneralConsultantClinicalAssesmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().UpdateAsync(request.GeneralConsultantClinicalAssesmentDtos.Adapt<List<GeneralConsultantClinicalAssesment>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanClinicalAssesmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultantClinicalAssesmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update

        #region Delete

        public async Task<bool> Handle(DeleteGeneralConsultantClinicalAssesmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultantClinicalAssesment>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanClinicalAssesmentQuery_");

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Delete
    }
}
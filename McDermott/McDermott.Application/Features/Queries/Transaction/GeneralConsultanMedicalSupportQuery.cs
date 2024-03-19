namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanMedicalSupportQuery(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralConsultanMedicalSupportQuery, List<GeneralConsultanMedicalSupportDto>>,
        IRequestHandler<CreateGeneralConsultanMedicalSupportRequest, GeneralConsultanMedicalSupportDto>,
        IRequestHandler<CreateListGeneralConsultanMedicalSupportRequest, List<GeneralConsultanMedicalSupportDto>>,
        IRequestHandler<UpdateGeneralConsultanMedicalSupportRequest, GeneralConsultanMedicalSupportDto>,
        IRequestHandler<DeleteGeneralConsultanMedicalSupportRequest, bool>
    {
        #region GET

        public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(GetGeneralConsultanMedicalSupportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultanMedicalSupportQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultanMedicalSupport>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().GetAsync(
                        null,
                        x => x
                            .Include(z => z.GeneralConsultanService)
                            .ThenInclude(z => z.Patient),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultanMedicalSupportDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanMedicalSupportDto> Handle(CreateGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(request.GeneralConsultanMedicalSupportDto.Adapt<GeneralConsultanMedicalSupport>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanMedicalSupportDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(CreateListGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(request.GeneralConsultanMedicalSupportDtos.Adapt<List<GeneralConsultanMedicalSupport>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanMedicalSupportDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GeneralConsultanMedicalSupportDto> Handle(UpdateGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().UpdateAsync(request.GeneralConsultanMedicalSupportDto.Adapt<GeneralConsultanMedicalSupport>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanMedicalSupportDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(UpdateListGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().UpdateAsync(request.GeneralConsultanMedicalSupportDtos.Adapt<List<GeneralConsultanMedicalSupport>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanMedicalSupportDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

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
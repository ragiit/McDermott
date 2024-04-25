



namespace McDermott.Application.Features.Queries.Patient
{
    public class PatientAllergyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetPatientAllergyQuery, List<PatientAllergyDto>>,
        IRequestHandler<CreatePatientAllergyRequest, PatientAllergyDto>,
        IRequestHandler<CreateListPatientAllergyRequest, List<PatientAllergyDto>>,
        IRequestHandler<UpdatePatientAllergyRequest, PatientAllergyDto>,
        IRequestHandler<UpdateListPatientAllergyRequest, List<PatientAllergyDto>>,
        IRequestHandler<DeletePatientAllergyRequest, bool>
    {
        #region GET

        public async Task<List<PatientAllergyDto>> Handle(GetPatientAllergyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetPatientAllergyQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<PatientAllergy>? result))
                {
                    result = await _unitOfWork.Repository<PatientAllergy>().Entities
                       .Include(x => x.User)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<PatientAllergyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<PatientAllergyDto> Handle(CreatePatientAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PatientAllergy>().AddAsync(request.PatientAllergyDto.Adapt<PatientAllergy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientAllergyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PatientAllergyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PatientAllergyDto>> Handle(CreateListPatientAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PatientAllergy>().AddAsync(request.PatientAllergyDtos.Adapt<List<PatientAllergy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientAllergyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PatientAllergyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<PatientAllergyDto> Handle(UpdatePatientAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PatientAllergy>().UpdateAsync(request.PatientAllergyDto.Adapt<PatientAllergy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientAllergyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PatientAllergyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PatientAllergyDto>> Handle(UpdateListPatientAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PatientAllergy>().UpdateAsync(request.PatientAllergyDtos.Adapt<List<PatientAllergy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientAllergyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PatientAllergyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeletePatientAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<PatientAllergy>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<PatientAllergy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientAllergyQuery_"); // Ganti dengan key yang sesuai

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

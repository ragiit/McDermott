namespace McDermott.Application.Features.Queries.Patient
{
    public class PatientFamilyRelationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetPatientFamilyByPatientQuery, List<PatientFamilyRelationDto>>,
        IRequestHandler<CreateListPatientFamilyRelationRequest, List<PatientFamilyRelationDto>>,
        IRequestHandler<DeletePatientFamilyRelationRequest, bool>
    {
        public async Task<List<PatientFamilyRelationDto>> Handle(GetPatientFamilyByPatientQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetPatientFamilyRelationQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique
                if (!_cache.TryGetValue(cacheKey, out List<PatientFamilyRelation>? result))
                {
                    result = await _unitOfWork.Repository<PatientFamilyRelation>().GetAsync(
                            null,
                                x => x
                                .Include(z => z.FamilyMember)
                                .Include(z => z.Family)
                                .Include(z => z.Patient),
                                cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<PatientFamilyRelationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PatientFamilyRelationDto>> Handle(CreateListPatientFamilyRelationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PatientFamilyRelation>().AddAsync(request.PatientFamilyRelationDto.Adapt<List<PatientFamilyRelation>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PatientFamilyRelationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(DeletePatientFamilyRelationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<PatientFamilyRelation>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<PatientFamilyRelation>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Patient
{
    public class PatientFamilyRelationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetPatientFamilyRelationQuery, (List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidatePatientFamilyRelationQuery, bool>,
        IRequestHandler<BulkValidatePatientFamilyRelationQuery, List<PatientFamilyRelationDto>>,
        IRequestHandler<GetPatientFamilyByPatientQuery, List<PatientFamilyRelationDto>>,
        IRequestHandler<CreateListPatientFamilyRelationRequest, List<PatientFamilyRelationDto>>,
        IRequestHandler<UpdatePatientFamilyRelationRequest, PatientFamilyRelationDto>,
        IRequestHandler<UpdateListPatientFamilyRelationRequest, List<PatientFamilyRelationDto>>,
        IRequestHandler<DeletePatientFamilyRelationRequest, bool>
    {
        public async Task<List<PatientFamilyRelationDto>> Handle(BulkValidatePatientFamilyRelationQuery request, CancellationToken cancellationToken)
        {
            var PatientFamilyRelationDtos = request.PatientFamilyRelationsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var PatientFamilyRelationNames = PatientFamilyRelationDtos.Select(x => x.PatientId).Distinct().ToList();
            var a = PatientFamilyRelationDtos.Select(x => x.FamilyMemberId).Distinct().ToList();
            var b = PatientFamilyRelationDtos.Select(x => x.FamilyId).Distinct().ToList();

            var existingPatientFamilyRelations = await _unitOfWork.Repository<PatientFamilyRelation>()
                .Entities
                .AsNoTracking()
                .Where(v => PatientFamilyRelationNames.Contains(v.PatientId)
                            && a.Contains(v.FamilyMemberId)
                            && b.Contains(v.FamilyId)
                            )
                .ToListAsync(cancellationToken);

            return existingPatientFamilyRelations.Adapt<List<PatientFamilyRelationDto>>();
        }

        public async Task<(List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetPatientFamilyRelationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<PatientFamilyRelation>().Entities
                    .AsNoTracking()
                    .Include(v => v.FamilyMember)
                    .Include(v => v.Patient)
                    .Include(v => v.Family)
                    .ThenInclude(x => x.InverseRelation)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Patient.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.FamilyMember.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Family.Name, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query.OrderBy(x => x.Id);

                var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

                return (paged.Adapt<List<PatientFamilyRelationDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidatePatientFamilyRelationQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<PatientFamilyRelation>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

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
                var createUpdateDtos = request.PatientFamilyRelationDto.Adapt<List<CreateUpdatePatientFamilyRelationDto>>();
                var patientFamilyRelations = createUpdateDtos.Adapt<List<PatientFamilyRelation>>();
                var result = await _unitOfWork.Repository<PatientFamilyRelation>().AddAsync(patientFamilyRelations);

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

        #region UPDATE

        public async Task<PatientFamilyRelationDto> Handle(UpdatePatientFamilyRelationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PatientFamilyRelation>().UpdateAsync(request.PatientFamilyRelationDto.Adapt<CreateUpdatePatientFamilyRelationDto>().Adapt<PatientFamilyRelation>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PatientFamilyRelationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PatientFamilyRelationDto>> Handle(UpdateListPatientFamilyRelationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<PatientFamilyRelation>().UpdateAsync(request.PatientFamilyRelationDtos.Adapt<List<PatientFamilyRelation>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PatientFamilyRelationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE
    }
}
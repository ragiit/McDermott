using static McDermott.Application.Features.Commands.Medical.DiagnosisCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DiagnosisQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDiagnosisQuery, List<DiagnosisDto>>,
        IRequestHandler<CreateDiagnosisRequest, DiagnosisDto>,
        IRequestHandler<CreateListDiagnosisRequest, List<DiagnosisDto>>,
        IRequestHandler<UpdateDiagnosisRequest, DiagnosisDto>,
        IRequestHandler<UpdateListDiagnosisRequest, List<DiagnosisDto>>,
        IRequestHandler<DeleteDiagnosisRequest, bool>
    {
        #region GET

        public async Task<List<DiagnosisDto>> Handle(GetDiagnosisQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDiagnosisQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Diagnosis>? result))
                {
                    result = await _unitOfWork.Repository<Diagnosis>().Entities
                        .Include(x => x.DiseaseCategory)
                        .Include(x => x.CronisKategory)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DiagnosisDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<DiagnosisDto> Handle(CreateDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Diagnosis>().AddAsync(request.DiagnosisDto.Adapt<Diagnosis>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DiagnosisDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DiagnosisDto>> Handle(CreateListDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Diagnosis>().AddAsync(request.DiagnosisDtos.Adapt<List<Diagnosis>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DiagnosisDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DiagnosisDto> Handle(UpdateDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Diagnosis>().UpdateAsync(request.DiagnosisDto.Adapt<Diagnosis>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DiagnosisDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DiagnosisDto>> Handle(UpdateListDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Diagnosis>().UpdateAsync(request.DiagnosisDtos.Adapt<List<Diagnosis>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DiagnosisDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Diagnosis>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Diagnosis>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

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
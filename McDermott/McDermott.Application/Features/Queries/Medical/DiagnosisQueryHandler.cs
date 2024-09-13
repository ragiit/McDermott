using static McDermott.Application.Features.Commands.Medical.DiagnosisCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DiagnosisQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDiagnosisQuery, (List<DiagnosisDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateDiagnosisRequest, DiagnosisDto>,
        IRequestHandler<CreateListDiagnosisRequest, List<DiagnosisDto>>,
        IRequestHandler<UpdateDiagnosisRequest, DiagnosisDto>,
        IRequestHandler<UpdateListDiagnosisRequest, List<DiagnosisDto>>,
        IRequestHandler<DeleteDiagnosisRequest, bool>
    {
        #region GET

        public async Task<(List<DiagnosisDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDiagnosisQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Diagnosis>().Entities
                    .Include(x => x.DiseaseCategory)
                    .Include(x => x.CronisCategory)
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<DiagnosisDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDiagnosisQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Diagnosis>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
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
using static McDermott.Application.Features.Commands.Medical.NursingDiagnosesCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class NursingDiagnosesQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetNursingDiagnosesQuery, (List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)>,
    IRequestHandler<CreateNursingDiagnosesRequest, NursingDiagnosesDto>,
    IRequestHandler<CreateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
    IRequestHandler<UpdateNursingDiagnosesRequest, NursingDiagnosesDto>,
    IRequestHandler<UpdateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
        IRequestHandler<DeleteNursingDiagnosesRequest, bool>
    {
        #region GET

        public async Task<(List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetNursingDiagnosesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<NursingDiagnoses>().Entities
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Problem, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var pagedResult = query
                            .OrderBy(x => x.Problem);

                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<NursingDiagnosesDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateNursingDiagnosesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<NursingDiagnoses>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<NursingDiagnosesDto> Handle(CreateNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDto.Adapt<NursingDiagnoses>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<NursingDiagnosesDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<NursingDiagnosesDto>> Handle(CreateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDtos.Adapt<List<NursingDiagnoses>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<NursingDiagnosesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<NursingDiagnosesDto> Handle(UpdateNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDto.Adapt<NursingDiagnoses>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<NursingDiagnosesDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<NursingDiagnosesDto>> Handle(UpdateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDtos.Adapt<List<NursingDiagnoses>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<NursingDiagnosesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

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
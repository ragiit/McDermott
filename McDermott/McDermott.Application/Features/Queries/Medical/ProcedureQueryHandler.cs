using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Medical.ProcedureCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class ProcedureQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProcedureQuery, (List<ProcedureDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateProcedureRequest, ProcedureDto>,
        IRequestHandler<BulkValidateProcedureQuery, List<ProcedureDto>>,
        IRequestHandler<CreateListProcedureRequest, List<ProcedureDto>>,
        IRequestHandler<UpdateProcedureRequest, ProcedureDto>,
        IRequestHandler<UpdateListProcedureRequest, List<ProcedureDto>>,
        IRequestHandler<DeleteProcedureRequest, bool>
    {
        #region GET

        public async Task<List<ProcedureDto>> Handle(BulkValidateProcedureQuery request, CancellationToken cancellationToken)
        {
            var ProcedureDtos = request.ProceduresToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ProcedureNames = ProcedureDtos.Select(x => x.Name).Distinct().ToList();
            var a = ProcedureDtos.Select(x => x.Code_Test).Distinct().ToList();
            var b = ProcedureDtos.Select(x => x.Classification).Distinct().ToList();

            var existingProcedures = await _unitOfWork.Repository<Procedure>()
                .Entities
                .AsNoTracking()
                .Where(v => ProcedureNames.Contains(v.Name)
                            && a.Contains(v.Code_Test)
                            && b.Contains(v.Classification))
                .ToListAsync(cancellationToken);

            return existingProcedures.Adapt<List<ProcedureDto>>();
        }

        public async Task<(List<ProcedureDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProcedureQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Procedure>().Entities
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code_Test, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Classification, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query.OrderBy(x => x.Name);

                var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

                return (paged.Adapt<List<ProcedureDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateProcedureQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Procedure>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<ProcedureDto> Handle(CreateProcedureRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Procedure>().AddAsync(request.ProcedureDto.Adapt<Procedure>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProcedureQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProcedureDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProcedureDto>> Handle(CreateListProcedureRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Procedure>().AddAsync(request.ProcedureDtos.Adapt<List<Procedure>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProcedureQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProcedureDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProcedureDto> Handle(UpdateProcedureRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Procedure>().UpdateAsync(request.ProcedureDto.Adapt<Procedure>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProcedureQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProcedureDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProcedureDto>> Handle(UpdateListProcedureRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Procedure>().UpdateAsync(request.ProcedureDtos.Adapt<List<Procedure>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProcedureQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProcedureDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProcedureRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Procedure>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Procedure>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProcedureQuery_"); // Ganti dengan key yang sesuai

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
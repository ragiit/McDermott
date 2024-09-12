using static McDermott.Application.Features.Commands.Medical.ProcedureCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class ProcedureQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProcedureQuery, List<ProcedureDto>>,
        IRequestHandler<CreateProcedureRequest, ProcedureDto>,
        IRequestHandler<CreateListProcedureRequest, List<ProcedureDto>>,
        IRequestHandler<UpdateProcedureRequest, ProcedureDto>,
        IRequestHandler<UpdateListProcedureRequest, List<ProcedureDto>>,
        IRequestHandler<DeleteProcedureRequest, bool>
    {
        #region GET

        public async Task<List<ProcedureDto>> Handle(GetProcedureQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetProcedureQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Procedure>? result))
                {
                    result = await _unitOfWork.Repository<Procedure>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ProcedureDto>>();
            }
            catch (Exception)
            {
                throw;
            }
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
namespace McDermott.Application.Features.Queries.Medical
{
    public class NursingDiagnosesQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetNursingDiagnosesQuery, List<NursingDiagnosesDto>>,
        IRequestHandler<CreateNursingDiagnosesRequest, NursingDiagnosesDto>,
        IRequestHandler<CreateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
        IRequestHandler<UpdateNursingDiagnosesRequest, NursingDiagnosesDto>,
        IRequestHandler<UpdateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
        IRequestHandler<DeleteNursingDiagnosesRequest, bool>
    {
        #region GET

        public async Task<List<NursingDiagnosesDto>> Handle(GetNursingDiagnosesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetNursingDiagnosesQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<NursingDiagnoses>? result))
                {
                    result = await _unitOfWork.Repository<NursingDiagnoses>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<NursingDiagnosesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
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
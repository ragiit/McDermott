using static McDermott.Application.Features.Commands.Medical.InsuranceCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class InsuranceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInsuranceQuery, List<InsuranceDto>>,
        IRequestHandler<CreateInsuranceRequest, InsuranceDto>,
        IRequestHandler<CreateListInsuranceRequest, List<InsuranceDto>>,
        IRequestHandler<UpdateInsuranceRequest, InsuranceDto>,
        IRequestHandler<UpdateListInsuranceRequest, List<InsuranceDto>>,
        IRequestHandler<DeleteInsuranceRequest, bool>
    {
        #region GET

        public async Task<List<InsuranceDto>> Handle(GetInsuranceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetInsuranceQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Insurance>? result))
                {
                    result = await _unitOfWork.Repository<Insurance>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<InsuranceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<InsuranceDto> Handle(CreateInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().AddAsync(request.InsuranceDto.Adapt<Insurance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsuranceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsuranceDto>> Handle(CreateListInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().AddAsync(request.InsuranceDtos.Adapt<List<Insurance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsuranceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<InsuranceDto> Handle(UpdateInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().UpdateAsync(request.InsuranceDto.Adapt<Insurance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsuranceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsuranceDto>> Handle(UpdateListInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().UpdateAsync(request.InsuranceDtos.Adapt<List<Insurance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsuranceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Insurance>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Insurance>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

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
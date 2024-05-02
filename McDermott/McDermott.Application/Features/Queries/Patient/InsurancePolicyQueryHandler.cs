

namespace McDermott.Application.Features.Queries.Patient
{
    public class InsurancePolicyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInsurancePolicyQuery, List<InsurancePolicyDto>>,
        IRequestHandler<CreateInsurancePolicyRequest, InsurancePolicyDto>,
        IRequestHandler<CreateListInsurancePolicyRequest, List<InsurancePolicyDto>>,
        IRequestHandler<UpdateInsurancePolicyRequest, InsurancePolicyDto>,
        IRequestHandler<UpdateListInsurancePolicyRequest, List<InsurancePolicyDto>>,
        IRequestHandler<DeleteInsurancePolicyRequest, bool>
    {
        #region GET

        public async Task<List<InsurancePolicyDto>> Handle(GetInsurancePolicyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetInsurancePolicyQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<InsurancePolicy>? result))
                {
                    result = await _unitOfWork.Repository<InsurancePolicy>().Entities
                       .Include(x => x.User)
                       .Include(x => x.Insurance)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    //result = await _unitOfWork.Repository<InsurancePolicy>().GetAsync(
                    //    null,
                    //    x => x.Include(z => z.Country),
                    //    cancellationToken);

                    //return await _unitOfWork.Repository<Counter>().Entities
                    //  .Include(x => x.Physician)
                    //  .Include(x => x.Service)
                    //  .AsNoTracking()
                    //  .Select(Counter => Counter.Adapt<CounterDto>())
                    //  .AsNoTracking()
                    //  .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<InsurancePolicyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<InsurancePolicyDto> Handle(CreateInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().AddAsync(request.InsurancePolicyDto.Adapt<InsurancePolicy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsurancePolicyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsurancePolicyDto>> Handle(CreateListInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().AddAsync(request.InsurancePolicyDtos.Adapt<List<InsurancePolicy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsurancePolicyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<InsurancePolicyDto> Handle(UpdateInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().UpdateAsync(request.InsurancePolicyDto.Adapt<InsurancePolicy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsurancePolicyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsurancePolicyDto>> Handle(UpdateListInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().UpdateAsync(request.InsurancePolicyDtos.Adapt<List<InsurancePolicy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsurancePolicyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<InsurancePolicy>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<InsurancePolicy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

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
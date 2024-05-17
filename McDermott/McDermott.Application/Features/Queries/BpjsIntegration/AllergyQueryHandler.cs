
namespace McDermott.Application.Features.Queries.BpjsIntegration
{
    public class AllergyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllergyQuery, List<AllergyDto>>,
        IRequestHandler<CreateAllergyRequest, AllergyDto>,
        IRequestHandler<CreateListAllergyRequest, List<AllergyDto>>,
        IRequestHandler<UpdateAllergyRequest, AllergyDto>,
        IRequestHandler<UpdateListAllergyRequest, List<AllergyDto>>,
        IRequestHandler<UpdateToDbAllergyRequest, List<AllergyDto>>,
        IRequestHandler<DeleteAllergyRequest, bool>
    {
        #region GET

        public async Task<List<AllergyDto>> Handle(GetAllergyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllergyQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Allergy>? result))
                {
                    result = await _unitOfWork.Repository<Allergy>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<AllergyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<AllergyDto> Handle(CreateAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Allergy>().AddAsync(request.AllergyDto.Adapt<Allergy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAllergyQuery_");

                return result.Adapt<AllergyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AllergyDto>> Handle(CreateListAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Allergy>().AddAsync(request.AllergyDtos.Adapt<List<Allergy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAllergyQuery_");

                return result.Adapt<List<AllergyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<AllergyDto> Handle(UpdateAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Allergy>().UpdateAsync(request.AllergyDto.Adapt<Allergy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAllergyQuery_");

                return result.Adapt<AllergyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AllergyDto>> Handle(UpdateListAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Allergy>().UpdateAsync(request.AllergyDtos.Adapt<List<Allergy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAllergyQuery_");

                return result.Adapt<List<AllergyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AllergyDto>> Handle(UpdateToDbAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.Repository<Allergy>().DeleteAsync(true);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var result = await _unitOfWork.Repository<Allergy>().AddAsync(request.AllergyDtos.Adapt<List<Allergy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAllergyQuery_");

                return result.Adapt<List<AllergyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteAllergyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Allergy>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Allergy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAllergyQuery_"); // Ganti dengan key yang sesuai

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

namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class ReorderingRuleQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetReorderingRuleQuery, List<ReorderingRuleDto>>,
        IRequestHandler<CreateReorderingRuleRequest, ReorderingRuleDto>,
        IRequestHandler<CreateListReorderingRuleRequest, List<ReorderingRuleDto>>,
        IRequestHandler<UpdateReorderingRuleRequest, ReorderingRuleDto>,
        IRequestHandler<UpdateListReorderingRuleRequest, List<ReorderingRuleDto>>,
        IRequestHandler<DeleteReorderingRuleRequest, bool>
    {
        #region GET

        public async Task<List<ReorderingRuleDto>> Handle(GetReorderingRuleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetReorderingRuleQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ReorderingRule>? result))
                {
                    result = await _unitOfWork.Repository<ReorderingRule>().Entities
                       .Include(x => x.Location)
                       .Include(x => x.Company)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ReorderingRuleDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ReorderingRuleDto> Handle(CreateReorderingRuleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReorderingRule>().AddAsync(request.ReorderingRuleDto.Adapt<ReorderingRule>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReorderingRuleQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ReorderingRuleDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReorderingRuleDto>> Handle(CreateListReorderingRuleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReorderingRule>().AddAsync(request.ReorderingRuleDtos.Adapt<List<ReorderingRule>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReorderingRuleQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReorderingRuleDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ReorderingRuleDto> Handle(UpdateReorderingRuleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReorderingRule>().UpdateAsync(request.ReorderingRuleDto.Adapt<ReorderingRule>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReorderingRuleQuery_");

                return result.Adapt<ReorderingRuleDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReorderingRuleDto>> Handle(UpdateListReorderingRuleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReorderingRule>().UpdateAsync(request.ReorderingRuleDtos.Adapt<List<ReorderingRule>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReorderingRuleQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReorderingRuleDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteReorderingRuleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ReorderingRule>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ReorderingRule>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReorderingRuleQuery_"); // Ganti dengan key yang sesuai

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

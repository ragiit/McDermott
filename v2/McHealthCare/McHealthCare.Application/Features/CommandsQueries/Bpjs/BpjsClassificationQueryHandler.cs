namespace McHealthCare.Application.Features.Queries.Bpjs
{
    public class BpjsClassificationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetBpjsClassificationQuery, List<BpjsClassificationDto>>,
        IRequestHandler<CreateBpjsClassificationRequest, BpjsClassificationDto>,
        IRequestHandler<CreateListBpjsClassificationRequest, List<BpjsClassificationDto>>,
        IRequestHandler<UpdateBpjsClassificationRequest, BpjsClassificationDto>,
        IRequestHandler<UpdateListBpjsClassificationRequest, List<BpjsClassificationDto>>,
        IRequestHandler<DeleteBpjsClassificationRequest, bool>
    {
        #region GET

        public async Task<List<BpjsClassificationDto>> Handle(GetBpjsClassificationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetBpjsClassificationQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<BpjsClassification>? result))
                {
                    result = await _unitOfWork.Repository<BpjsClassification>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<BpjsClassificationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<BpjsClassificationDto> Handle(CreateBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BpjsClassification>().AddAsync(request.BpjsClassificationDto.Adapt<BpjsClassification>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

                return result.Adapt<BpjsClassificationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BpjsClassificationDto>> Handle(CreateListBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BpjsClassification>().AddAsync(request.BpjsClassificationDtos.Adapt<List<BpjsClassification>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

                return result.Adapt<List<BpjsClassificationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<BpjsClassificationDto> Handle(UpdateBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BpjsClassification>().UpdateAsync(request.BpjsClassificationDto.Adapt<BpjsClassification>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

                return result.Adapt<BpjsClassificationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BpjsClassificationDto>> Handle(UpdateListBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BpjsClassification>().UpdateAsync(request.BpjsClassificationDtos.Adapt<List<BpjsClassification>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

                return result.Adapt<List<BpjsClassificationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<BpjsClassification>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<BpjsClassification>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

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
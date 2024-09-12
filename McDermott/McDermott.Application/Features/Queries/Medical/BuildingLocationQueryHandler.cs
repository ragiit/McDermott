namespace McDermott.Application.Features.Queries.Medical
{
    public class BuildingLocationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetBuildingLocationQuery, List<BuildingLocationDto>>,
        IRequestHandler<CreateBuildingLocationRequest, BuildingLocationDto>,
        IRequestHandler<CreateListBuildingLocationRequest, List<BuildingLocationDto>>,
        IRequestHandler<UpdateBuildingLocationRequest, BuildingLocationDto>,
        IRequestHandler<UpdateListBuildingLocationRequest, List<BuildingLocationDto>>,
        IRequestHandler<DeleteBuildingLocationRequest, bool>
    {
        #region GET

        public async Task<List<BuildingLocationDto>> Handle(GetBuildingLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetBuildingLocationQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<BuildingLocation>? result))
                {
                    result = await _unitOfWork.Repository<BuildingLocation>().Entities
                       .Include(x => x.Building)
                       .Include(x => x.Location)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<BuildingLocationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<BuildingLocationDto> Handle(CreateBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BuildingLocation>().AddAsync(request.BuildingLocationDto.Adapt<BuildingLocation>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingLocationQuery_");

                return result.Adapt<BuildingLocationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BuildingLocationDto>> Handle(CreateListBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BuildingLocation>().AddAsync(request.BuildingLocationDtos.Adapt<List<BuildingLocation>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingLocationQuery_");

                return result.Adapt<List<BuildingLocationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<BuildingLocationDto> Handle(UpdateBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BuildingLocation>().UpdateAsync(request.BuildingLocationDto.Adapt<BuildingLocation>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingLocationQuery_");

                return result.Adapt<BuildingLocationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BuildingLocationDto>> Handle(UpdateListBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BuildingLocation>().UpdateAsync(request.BuildingLocationDtos.Adapt<List<BuildingLocation>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingLocationQuery_");

                return result.Adapt<List<BuildingLocationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<BuildingLocation>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<BuildingLocation>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingLocationQuery_");

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

namespace McDermott.Application.Features.Queries.Medical
{
    public class BuildingQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetBuildingQuery, List<BuildingDto>>,
        IRequestHandler<CreateBuildingRequest, BuildingDto>,
        IRequestHandler<CreateListBuildingRequest, List<BuildingDto>>,
        IRequestHandler<UpdateBuildingRequest, BuildingDto>,
        IRequestHandler<UpdateListBuildingRequest, List<BuildingDto>>,
        IRequestHandler<DeleteBuildingRequest, bool>
    {
        #region GET

        public async Task<List<BuildingDto>> Handle(GetBuildingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetBuildingQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Building>? result))
                {
                    result = await _unitOfWork.Repository<Building>().Entities
                       .Include(x => x.HealthCenter)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<BuildingDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<BuildingDto> Handle(CreateBuildingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Building>().AddAsync(request.BuildingDto.Adapt<Building>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<BuildingDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BuildingDto>> Handle(CreateListBuildingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Building>().AddAsync(request.BuildingDtos.Adapt<List<Building>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<BuildingDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<BuildingDto> Handle(UpdateBuildingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Building>().UpdateAsync(request.BuildingDto.Adapt<Building>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<BuildingDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BuildingDto>> Handle(UpdateListBuildingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Building>().UpdateAsync(request.BuildingDtos.Adapt<List<Building>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<BuildingDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteBuildingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Building>().DeleteAsync(request.Id);

                    var a = await _unitOfWork.Repository<BuildingLocation>().GetAllAsync();

                    foreach (var item in a.Where(x => x.BuildingId == request.Id).ToList())
                    {
                        await _unitOfWork.Repository<BuildingLocation>().DeleteAsync(item.Id);
                    }
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Building>().DeleteAsync(x => request.Ids.Contains(x.Id));

                    foreach (var item in request.Ids)
                    {
                        var a = await _unitOfWork.Repository<BuildingLocation>().GetAllAsync();

                        foreach (var i in a.Where(x => x.BuildingId == item).ToList())
                        {
                            await _unitOfWork.Repository<BuildingLocation>().DeleteAsync(i.Id);
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBuildingQuery_"); // Ganti dengan key yang sesuai

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
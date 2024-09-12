using static McDermott.Application.Features.Commands.Medical.BuildingCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class BuildingQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetBuildingQuery, (List<BuildingDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateBuildingRequest, BuildingDto>,
        IRequestHandler<CreateListBuildingRequest, List<BuildingDto>>,
        IRequestHandler<UpdateBuildingRequest, BuildingDto>,
        IRequestHandler<UpdateListBuildingRequest, List<BuildingDto>>,
        IRequestHandler<DeleteBuildingRequest, bool>
    {
        #region GET

        public async Task<(List<BuildingDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetBuildingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Building>().Entities
                    .Include(x=>x.HealthCenter)
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * request.PageSize;

                var totalCount = await query.CountAsync(cancellationToken);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<BuildingDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateBuildingQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Building>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
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
using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Medical
{
    public class BuildingLocationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetBuildingLocationQuery, (List<BuildingLocationDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateBuildingLocationRequest, BuildingLocationDto>,
        IRequestHandler<CreateListBuildingLocationRequest, List<BuildingLocationDto>>,
        IRequestHandler<UpdateBuildingLocationRequest, BuildingLocationDto>,
        IRequestHandler<UpdateListBuildingLocationRequest, List<BuildingLocationDto>>,
        IRequestHandler<DeleteBuildingLocationRequest, bool>
    {
        #region GET

        public async Task<(List<BuildingLocationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetBuildingLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<BuildingLocation>().Entities.AsNoTracking();

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Building.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Location.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                                  query,
                                  request.PageSize,
                                  request.PageIndex,
                                  q => q.OrderBy(x => x.Id), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<BuildingLocationDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateBuildingLocationQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<BuildingLocation>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
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
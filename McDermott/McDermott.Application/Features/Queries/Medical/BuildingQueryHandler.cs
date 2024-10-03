using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Medical.BuildingCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class BuildingQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetBuildingQuery, (List<BuildingDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateBuildingRequest, BuildingDto>,
        IRequestHandler<ValidateBuildingQuery, bool>,
        IRequestHandler<BulkValidateBuildingQuery, List<BuildingDto>>,
        IRequestHandler<CreateListBuildingRequest, List<BuildingDto>>,
        IRequestHandler<UpdateBuildingRequest, BuildingDto>,
        IRequestHandler<UpdateListBuildingRequest, List<BuildingDto>>,
        IRequestHandler<DeleteBuildingRequest, bool>
    {
        #region GET

        public async Task<List<BuildingDto>> Handle(BulkValidateBuildingQuery request, CancellationToken cancellationToken)
        {
            var BuildingDtos = request.BuildingsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var BuildingNames = BuildingDtos.Select(x => x.Name).Distinct().ToList();
            var a = BuildingDtos.Select(x => x.Code).Distinct().ToList();
            var ab = BuildingDtos.Select(x => x.HealthCenterId).Distinct().ToList();

            var existingBuildings = await _unitOfWork.Repository<Building>()
                .Entities
                .AsNoTracking()
                .Where(v => BuildingNames.Contains(v.Name)
                            && a.Contains(v.Code)
                            && ab.Contains(v.HealthCenterId))
                .ToListAsync(cancellationToken);

            return existingBuildings.Adapt<List<BuildingDto>>();
        }

        public async Task<(List<BuildingDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetBuildingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Building>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.HealthCenter.Name, $"%{request.SearchTerm}%")
                        );
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
                                  q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<BuildingDto>>(), request.PageIndex, request.PageSize, totalPages);
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
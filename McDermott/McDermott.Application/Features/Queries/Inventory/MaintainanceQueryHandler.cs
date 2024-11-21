using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintenanceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMaintenanceQuery, (List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetAllMaintenanceQuery, List<MaintenanceDto>>,
        IRequestHandler<ValidateMaintenanceQuery, bool>,
        IRequestHandler<CreateMaintenanceRequest, MaintenanceDto>,
        IRequestHandler<CreateListMaintenanceRequest, List<MaintenanceDto>>,
        IRequestHandler<UpdateMaintenanceRequest, MaintenanceDto>,
        IRequestHandler<UpdateListMaintenanceRequest, List<MaintenanceDto>>,
        IRequestHandler<DeleteMaintenanceRequest, bool>
    {
        #region GET

        public async Task<List<MaintenanceDto>> Handle(GetAllMaintenanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMaintenanceQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Maintenance>? result))
                {
                    result = await _unitOfWork.Repository<Maintenance>().Entities
                    .Include(x => x.RequestBy)
                    .Include(x => x.Location)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaintenanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaintenanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Maintenance>().Entities
                    .Include(x => x.RequestBy)
                    .Include(x => x.Location)
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Title, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Sequence, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.Title);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<MaintenanceDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateMaintenanceQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Maintenance>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<MaintenanceDto> Handle(CreateMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintenance>().AddAsync(request.MaintenanceDto.Adapt<Maintenance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintenanceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintenanceDto>> Handle(CreateListMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintenance>().AddAsync(request.MaintenanceDtos.Adapt<List<Maintenance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintenanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MaintenanceDto> Handle(UpdateMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintenance>().UpdateAsync(request.MaintenanceDto.Adapt<Maintenance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintenanceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintenanceDto>> Handle(UpdateListMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintenance>().UpdateAsync(request.MaintenanceDtos.Adapt<List<Maintenance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintenanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Maintenance>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Maintenance>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

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
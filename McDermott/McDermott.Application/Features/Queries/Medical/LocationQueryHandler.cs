using static McDermott.Application.Features.Commands.Medical.LocationCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class LocationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLocationQuery, (List<LocationDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateLocationRequest, LocationDto>,
        IRequestHandler<CreateListLocationRequest, List<LocationDto>>,
        IRequestHandler<UpdateLocationRequest, LocationDto>,
        IRequestHandler<UpdateListLocationRequest, List<LocationDto>>,
        IRequestHandler<DeleteLocationRequest, bool>
    {
        #region GET

        public async Task<(List<LocationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Location>().Entities
                    .Include(x => x.ParentLocation)
                    .AsNoTracking()
                    .AsQueryable();
                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.ParentLocation.Name, $"{request.SearchTerm}") ||
                        EF.Functions.Like(v.Type, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<LocationDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateLocationQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Location>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<LocationDto> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().AddAsync(request.LocationDto.Adapt<Location>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LocationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LocationDto>> Handle(CreateListLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().AddAsync(request.LocationDtos.Adapt<List<Location>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LocationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LocationDto> Handle(UpdateLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().UpdateAsync(request.LocationDto.Adapt<Location>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LocationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LocationDto>> Handle(UpdateListLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Location>().UpdateAsync(request.LocationDtos.Adapt<List<Location>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LocationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLocationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    var result = await _unitOfWork.Repository<Location>().Entities.Where(x => x.ParentLocationId == request.Id).ToListAsync(cancellationToken);

                    foreach (var item in result)
                    {
                        item.ParentLocationId = null;
                    }

                    await _unitOfWork.Repository<Location>().UpdateAsync(result);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    //await _unitOfWork.Repository<Location>().DeleteAsync(x => result.Select(z => z.Id).Contains(x.Id));

                    await _unitOfWork.Repository<Location>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    foreach (var item in request.Ids)
                    {
                        var result = await _unitOfWork.Repository<Location>().Entities.Where(x => x.ParentLocationId == item).ToListAsync(cancellationToken);

                        foreach (var item2 in result)
                        {
                            item2.ParentLocationId = null;
                        }

                        await _unitOfWork.Repository<Location>().UpdateAsync(result);
                    }
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _unitOfWork.Repository<Location>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLocationQuery_"); // Ganti dengan key yang sesuai

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
using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Medical
{
    public class LocationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLocationQuery, (List<LocationDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleLocationQuery, LocationDto>,
        IRequestHandler<ValidateLocationQuery, bool>,
        IRequestHandler<BulkValidateLocationsQuery, List<LocationDto>>,
        IRequestHandler<CreateLocationRequest, LocationDto>,
        IRequestHandler<CreateListLocationRequest, List<LocationDto>>,
        IRequestHandler<UpdateLocationRequest, LocationDto>,
        IRequestHandler<UpdateListLocationRequest, List<LocationDto>>,
        IRequestHandler<DeleteLocationRequest, bool>
    {
        #region GET

        public async Task<List<LocationDto>> Handle(BulkValidateLocationsQuery request, CancellationToken cancellationToken)
        {
            var LocationsDtos = request.LocationssToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var LocationsNames = LocationsDtos.Select(x => x.Name).Distinct().ToList();
            var a = LocationsDtos.Select(x => x.ParentLocationId).Distinct().ToList();
            var b = LocationsDtos.Select(x => x.Type).Distinct().ToList();
            var c = LocationsDtos.Select(x => x.CompanyId).Distinct().ToList();

            var existingLocationss = await _unitOfWork.Repository<Locations>()
                .Entities
                .AsNoTracking()
                .Where(v => LocationsNames.Contains(v.Name)
                            && a.Contains(v.ParentLocationId)
                            && c.Contains(v.CompanyId)
                            && b.Contains(v.Type))
                .ToListAsync(cancellationToken);

            return existingLocationss.Adapt<List<LocationDto>>();
        }

        public async Task<(List<LocationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Locations>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<Locations>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Locations>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.ParentLocation.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Type, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Locations
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        ParentLocationId = x.ParentLocationId,
                        CompanyId = x.CompanyId,
                        Company = new Company
                        {
                            Name = x.Company.Name
                        },
                        ParentLocation = new Locations
                        {
                            Name = x.ParentLocation.Name
                        }
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<LocationDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<LocationDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<LocationDto> Handle(GetSingleLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Locations>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<Locations>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Locations>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                         EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                         EF.Functions.Like(v.ParentLocation.Name, $"%{request.SearchTerm}%") ||
                         EF.Functions.Like(v.Type, $"%{request.SearchTerm}%") ||
                         EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%")
                         );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Locations
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        ParentLocationId = x.ParentLocationId,
                        CompanyId = x.CompanyId,
                        Company = new Company
                        {
                            Name = x.Company.Name
                        },
                        ParentLocation = new Locations
                        {
                            Name = x.ParentLocation.Name
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<LocationDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateLocationQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Locations>()
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
                var result = await _unitOfWork.Repository<Locations>().AddAsync(request.LocationDto.Adapt<Locations>());

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
                var result = await _unitOfWork.Repository<Locations>().AddAsync(request.LocationDtos.Adapt<List<Locations>>());

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
                var result = await _unitOfWork.Repository<Locations>().UpdateAsync(request.LocationDto.Adapt<Locations>());

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
                var result = await _unitOfWork.Repository<Locations>().UpdateAsync(request.LocationDtos.Adapt<List<Locations>>());

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
                    var result = await _unitOfWork.Repository<Locations>().Entities.Where(x => x.ParentLocationId == request.Id).ToListAsync(cancellationToken);

                    foreach (var item in result)
                    {
                        item.ParentLocationId = null;
                    }

                    await _unitOfWork.Repository<Locations>().UpdateAsync(result);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    //await _unitOfWork.Repository<Location>().DeleteAsync(x => result.Select(z => z.Id).Contains(x.Id));

                    await _unitOfWork.Repository<Locations>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    foreach (var item in request.Ids)
                    {
                        var result = await _unitOfWork.Repository<Locations>().Entities.Where(x => x.ParentLocationId == item).ToListAsync(cancellationToken);

                        foreach (var item2 in result)
                        {
                            item2.ParentLocationId = null;
                        }

                        await _unitOfWork.Repository<Locations>().UpdateAsync(result);
                    }
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _unitOfWork.Repository<Locations>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
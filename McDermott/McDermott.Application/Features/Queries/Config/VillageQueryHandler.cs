using McDermott.Application.Dtos;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.VillageCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class VillageQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetVillageQueryNew, (List<VillageDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetVillageQuery, (List<VillageDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateVillageQuery, bool>,
        IRequestHandler<BulkValidateVillageQuery, List<VillageDto>>,
        IRequestHandler<GetVillageQuery2, IQueryable<VillageDto>>,
        //IRequestHandler<GetVillageQuerylable, IQueryable<VillageDto>>,
        IRequestHandler<GetPagedDataQuery, (List<VillageDto> Data, int TotalCount)>,
        IRequestHandler<CreateVillageRequest, VillageDto>,
        IRequestHandler<CreateListVillageRequest, List<VillageDto>>,
        IRequestHandler<UpdateVillageRequest, VillageDto>,
        IRequestHandler<UpdateListVillageRequest, List<VillageDto>>,
        IRequestHandler<GetDistrictsQuery, PaginatedList<VillageDto>>,
        IRequestHandler<DeleteVillageRequest, bool>
    {
        #region GET

        public async Task<(List<VillageDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetVillageQueryNew request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Village>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Village>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Village>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.District.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.City.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Village
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PostalCode = x.PostalCode,
                        ProvinceId = x.ProvinceId,
                        CityId = x.CityId,
                        DistrictId = x.DistrictId,
                        Province = new Domain.Entities.Province
                        {
                            Name = x.Province.Name
                        },
                        City = new Domain.Entities.City
                        {
                            Name = x.City.Name
                        },
                        District = new Domain.Entities.District
                        {
                            Name = x.District.Name
                        },
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<VillageDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<VillageDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        //public async Task<(List<VillageDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetVillageQuery request, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var query = _unitOfWork.Repository<Village>().Entities
        //            .AsNoTracking()
        //            .Include(v => v.City)
        //            .Include(v => v.Province)
        //            .Include(v => v.District)
        //            .AsQueryable();

        //        if (!string.IsNullOrEmpty(request.SearchTerm))
        //        {
        //            query = query.Where(v =>
        //                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
        //                EF.Functions.Like(v.City.Name, $"%{request.SearchTerm}%") ||
        //                EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%"));
        //        }

        //        var pagedResult = query
        //                    .OrderBy(x => x.Name);

        //                        var skip = (request.PageIndex) * request.PageSize;

        //        var totalCount = await query.CountAsync(cancellationToken);

        //        var paged = pagedResult
        //                    .Skip(skip)
        //                    .Take(request.PageSize);

        //        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        //        return (paged.Adapt<List<VillageDto>>(), request.PageIndex, request.PageSize, totalPages);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public async Task<bool> Handle(ValidateVillageQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Village>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<VillageDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetVillageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Village>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.PostalCode, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.City.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.District.Name, $"%{request.SearchTerm}%")
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

                return (pagedItems.Adapt<List<VillageDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<VillageDto>> Handle(GetVillageQuery2 request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetVillageQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out IQueryable<Village>? result))
                {
                    result = _unitOfWork.Repository<Village>().Entities
                            .Include(z => z.Province)
                            .Include(z => z.City)
                            .Include(z => z.District)
                            .AsNoTracking();

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                if (request.Predicate is not null)
                    result = result.Where(request.Predicate);

                return result.Adapt<IQueryable<VillageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<VillageDto> Handle(CreateVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Village>().AddAsync(request.VillageDto.Adapt<CreateUpdateVillageDto>().Adapt<Village>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<VillageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VillageDto>> Handle(CreateListVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Village>().AddAsync(request.VillageDtos.Adapt<List<Village>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<VillageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<VillageDto> Handle(UpdateVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Village>().UpdateAsync(request.VillageDto.Adapt<CreateUpdateVillageDto>().Adapt<Village>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<VillageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VillageDto>> Handle(UpdateListVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Village>().UpdateAsync(request.VillageDtos.Adapt<CreateUpdateVillageDto>().Adapt<List<Village>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<VillageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteVillageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Village>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Village>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetVillageQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginatedList<VillageDto>> Handle(GetDistrictsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Village>()
                .Entities
                .Include(d => d.Province)
                .Include(d => d.City)
                .Select(d => d.Adapt<VillageDto>()).AsQueryable();

            return await PaginatedList<VillageDto>.CreateAsync(query, request.PageNumber, request.PageSize);
        }

        #endregion DELETE

        public async Task<(List<VillageDto> Data, int TotalCount)> Handle(GetPagedDataQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;

            var query = _unitOfWork.Repository<Village>().GetAllQuerylable();

            // Apply the predicate if provided
            if (request.Predicate != null)
            {
                query = query
                .Where(request.Predicate);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var data = await query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var aa = data.Adapt<List<VillageDto>>();

            return (aa, totalCount);
        }

        //public async Task<IQueryable<Village>> Handle(GetVillageQuerylable request, CancellationToken cancellationToken)
        //{
        //    return await Task.FromResult(_unitOfWork.Repository<Village>().GetAllQuerylable());
        //}

        //public Task<IQueryable<VillageDto>> Handle(GetVillageQuerylable request, CancellationToken cancellationToken)
        //{
        //    var query = _unitOfWork.Repository<Village>()
        //           .Entities
        //           .Include(x => x.Province)
        //           .Include(x => x.City)
        //           .Include(x => x.District)
        //           .AsNoTracking()
        //           .AsQueryable();

        //    return Task.FromResult(query.Adapt<IQueryable<VillageDto>>());
        //}

        public async Task<List<VillageDto>> Handle(BulkValidateVillageQuery request, CancellationToken cancellationToken)
        {
            var villageDtos = request.VillagesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var villageNames = villageDtos.Select(x => x.Name).Distinct().ToList();
            var postalCodes = villageDtos.Select(x => x.PostalCode).Distinct().ToList();
            var provinceIds = villageDtos.Select(x => x.ProvinceId).Distinct().ToList();
            var cityIds = villageDtos.Select(x => x.CityId).Distinct().ToList();
            var districtIds = villageDtos.Select(x => x.DistrictId).Distinct().ToList();

            var existingVillages = await _unitOfWork.Repository<Village>()
                .Entities
                .AsNoTracking()
                .Where(v => villageNames.Contains(v.Name)
                            && postalCodes.Contains(v.PostalCode)
                            && provinceIds.Contains(v.ProvinceId)
                            && cityIds.Contains(v.CityId)
                            && districtIds.Contains(v.DistrictId))
                .ToListAsync(cancellationToken);

            return existingVillages.Adapt<List<VillageDto>>();
        }
    }
}
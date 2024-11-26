using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionCommand;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class ConcoctionQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllConcoctionQuery, List<ConcoctionDto>>,//Concoction
        IRequestHandler<GetConcoctionQuery, (List<ConcoctionDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleConcoctionQuery, ConcoctionDto>, IRequestHandler<ValidateConcoctionQuery, bool>,
        IRequestHandler<BulkValidateConcoctionQuery, List<ConcoctionDto>>,
        IRequestHandler<CreateConcoctionRequest, ConcoctionDto>,
        IRequestHandler<CreateListConcoctionRequest, List<ConcoctionDto>>,
        IRequestHandler<UpdateConcoctionRequest, ConcoctionDto>,
        IRequestHandler<UpdateListConcoctionRequest, List<ConcoctionDto>>,
        IRequestHandler<DeleteConcoctionRequest, bool>
    {
        #region Concoction

        #region GET Concoction

        public async Task<List<ConcoctionDto>> Handle(GetAllConcoctionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllConcoctionQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Concoction>? result))
                {
                    result = await _unitOfWork.Repository<Concoction>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ConcoctionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionDto>> Handle(BulkValidateConcoctionQuery request, CancellationToken cancellationToken)
        {
            var ConcoctionDtos = request.ConcoctionToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ConcoctionNames = ConcoctionDtos.Select(x => x.ConcoctionQty).Distinct().ToList();

            var existingConcoctions = await _unitOfWork.Repository<Concoction>()
                .Entities
                .AsNoTracking()
                .Where(v => ConcoctionNames.Contains((long)v.ConcoctionQty))
                .ToListAsync(cancellationToken);

            return existingConcoctions.Adapt<List<ConcoctionDto>>();
        }

        public async Task<bool> Handle(ValidateConcoctionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Concoction>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<ConcoctionDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetConcoctionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Concoction>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList?.Count > 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<Concoction>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Concoction>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }
                else
                {
                    query = query.OrderBy(x => x.Id); // Default ordering
                }

                // Apply includes
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
                        EF.Functions.Like(v.MedicamenName, $"%{request.SearchTerm}%"));
                }

                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Concoction
                    {
                        Id = x.Id,
                        PharmacyId = x.PharmacyId,
                        PractitionerId = x.PractitionerId,
                        DrugDosageId = x.DrugDosageId,
                        DrugFormId = x.DrugFormId,
                        DrugRouteId = x.DrugRouteId,
                        MedicamentGroupId = x.MedicamentGroupId,
                        ConcoctionQty = x.ConcoctionQty,
                        MedicamenName = x.MedicamenName,
                        Practitioner = new User
                        {
                            Name = x.Practitioner == null ? string.Empty : x.Practitioner.Name,
                        },
                        DrugDosage = new DrugDosage
                        {
                            Frequency = x.DrugDosage == null ? string.Empty : x.DrugDosage.Frequency,
                        },
                        DrugForm = new DrugForm
                        {
                            Name = x.DrugForm == null ? string.Empty : x.DrugForm.Name,
                        },
                        DrugRoute = new DrugRoute
                        {
                            Route = x.DrugRoute == null ? string.Empty : x.DrugRoute.Route,
                        },
                    });

                // Ensure single query mode or apply pagination
                if (!request.IsGetAll)
                {
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
             query,
             request.PageSize,
             request.PageIndex,
             cancellationToken
         );

                    return (pagedItems.Adapt<List<ConcoctionDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<ConcoctionDto>>(), 0, 1, 1);
                }

            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<ConcoctionDto> Handle(GetSingleConcoctionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Concoction>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Concoction>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Concoction>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.MedicamenName, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Concoction
                    {
                        Id = x.Id,
                        PharmacyId = x.PharmacyId,
                        PractitionerId = x.PractitionerId,
                        DrugDosageId = x.DrugDosageId,
                        DrugFormId = x.DrugFormId,
                        DrugRouteId = x.DrugRouteId,
                        MedicamentGroupId = x.MedicamentGroupId,
                        ConcoctionQty = x.ConcoctionQty,
                        MedicamenName = x.MedicamenName,
                        Practitioner = new User
                        {
                            Name = x.Practitioner == null ? string.Empty : x.Practitioner.Name,
                        },
                        DrugDosage = new DrugDosage
                        {
                            Frequency = x.DrugDosage == null ? string.Empty : x.DrugDosage.Frequency,
                        },
                        DrugForm = new DrugForm
                        {
                            Name = x.DrugForm == null ? string.Empty : x.DrugForm.Name,
                        },
                        DrugRoute = new DrugRoute
                        {
                            Route = x.DrugRoute == null ? string.Empty : x.DrugRoute.Route,
                        },
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ConcoctionDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Concoction

        #region CREATE

        public async Task<ConcoctionDto> Handle(CreateConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Concoction>().AddAsync(request.ConcoctionDto.Adapt<Concoction>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ConcoctionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionDto>> Handle(CreateListConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Concoction>().AddAsync(request.ConcoctionDtos.Adapt<List<Concoction>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ConcoctionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ConcoctionDto> Handle(UpdateConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Concoction>().UpdateAsync(request.ConcoctionDto.Adapt<Concoction>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ConcoctionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionDto>> Handle(UpdateListConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Concoction>().UpdateAsync(request.ConcoctionDtos.Adapt<List<Concoction>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ConcoctionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteConcoctionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Concoction>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Concoction>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion Concoction
    }
}
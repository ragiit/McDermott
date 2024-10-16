using McDermott.Application.Dtos.Queue;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Queue.KioskConfigCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class KioskConfigHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
      IRequestHandler<GetKioskConfigQuery, (List<KioskConfigDto>, int pageIndex, int pageSize, int pageCount)>,
      IRequestHandler<GetSingleKioskConfigQuery, KioskConfigDto>, IRequestHandler<ValidateKioskConfig, bool>,
      IRequestHandler<CreateKioskConfigRequest, KioskConfigDto>,
      IRequestHandler<BulkValidateKioskConfig, List<KioskConfigDto>>,
      IRequestHandler<CreateListKioskConfigRequest, List<KioskConfigDto>>,
      IRequestHandler<UpdateKioskConfigRequest, KioskConfigDto>,
      IRequestHandler<UpdateListKioskConfigRequest, List<KioskConfigDto>>,
      IRequestHandler<DeleteKioskConfigRequest, bool>
    {
        #region GET

        public async Task<List<KioskConfigDto>> Handle(BulkValidateKioskConfig request, CancellationToken cancellationToken)
        {
            var KioskConfigDtos = request.KioskConfigsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var KioskConfigNames = KioskConfigDtos.Select(x => x.Name).Distinct().ToList();
            //var a = KioskConfigDtos.Select(x => x.CountryId).Distinct().ToList();

            //var existingKioskConfigs = await _unitOfWork.Repository<KioskConfig>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => KioskConfigNames.Contains(v.Name)
            //                && a.Contains(v.CountryId))
            //    .ToListAsync(cancellationToken);

            //return existingKioskConfigs.Adapt<List<KioskConfigDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateKioskConfig request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<KioskConfig>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<KioskConfigDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetKioskConfigQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<KioskConfig>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<KioskConfig>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<KioskConfig>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new KioskConfig
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ServiceIds = x.ServiceIds
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<KioskConfigDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<KioskConfigDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<KioskConfigDto> Handle(GetSingleKioskConfigQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<KioskConfig>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<KioskConfig>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<KioskConfig>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new KioskConfig
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ServiceIds = x.ServiceIds
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<KioskConfigDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<KioskConfigDto> Handle(CreateKioskConfigRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<KioskConfig>().AddAsync(request.KioskConfigDto.Adapt<KioskConfig>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetKioskConfig_"); // Ganti dengan key yang sesuai

                return result.Adapt<KioskConfigDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<KioskConfigDto>> Handle(CreateListKioskConfigRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<KioskConfig>().AddAsync(request.KioskConfigDtos.Adapt<List<KioskConfig>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetKioskConfig_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<KioskConfigDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<KioskConfigDto> Handle(UpdateKioskConfigRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<KioskConfig>().UpdateAsync(request.KioskConfigDto.Adapt<KioskConfigDto>().Adapt<KioskConfig>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetKioskConfig_"); // Ganti dengan key yang sesuai

                return result.Adapt<KioskConfigDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<KioskConfigDto>> Handle(UpdateListKioskConfigRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<KioskConfig>().UpdateAsync(request.KioskConfigDtos.Adapt<List<KioskConfig>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetKioskConfig_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<KioskConfigDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteKioskConfigRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<KioskConfig>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<KioskConfig>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetKioskConfig_"); // Ganti dengan key yang sesuai

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
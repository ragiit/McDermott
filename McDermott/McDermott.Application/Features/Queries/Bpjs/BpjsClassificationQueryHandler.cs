using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Bpjs
{
    public class BpjsClassificationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetBpjsClassificationQuery, (List<BpjsClassificationDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<BulkValidateBpjsClassificationQuery, List<BpjsClassificationDto>>,
        IRequestHandler<GetSingleBpjsClassificationQuery, BpjsClassificationDto>, IRequestHandler<CreateBpjsClassificationRequest, BpjsClassificationDto>,
        IRequestHandler<CreateListBpjsClassificationRequest, List<BpjsClassificationDto>>,
        IRequestHandler<UpdateBpjsClassificationRequest, BpjsClassificationDto>,
        IRequestHandler<UpdateListBpjsClassificationRequest, List<BpjsClassificationDto>>,
        IRequestHandler<DeleteBpjsClassificationRequest, bool>
    {
        #region GET

        public async Task<List<BpjsClassificationDto>> Handle(BulkValidateBpjsClassificationQuery request, CancellationToken cancellationToken)
        {
            var BpjsClassificationDtos = request.BpjsClassificationsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var BpjsClassificationNames = BpjsClassificationDtos.Select(x => x.Name).Distinct().ToList();
            var Codes = BpjsClassificationDtos.Select(x => x.Code).Distinct().ToList();

            var existingBpjsClassifications = await _unitOfWork.Repository<BpjsClassification>()
                .Entities
                .AsNoTracking()
                .Where(v => BpjsClassificationNames.Contains(v.Name)
                            && Codes.Contains(v.Code))
                .ToListAsync(cancellationToken);

            return existingBpjsClassifications.Adapt<List<BpjsClassificationDto>>();
        }

        public async Task<(List<BpjsClassificationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetBpjsClassificationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<BpjsClassification>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<BpjsClassification>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<BpjsClassification>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new BpjsClassification
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<BpjsClassificationDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<BpjsClassificationDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<BpjsClassificationDto> Handle(GetSingleBpjsClassificationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<BpjsClassification>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<BpjsClassification>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<BpjsClassification>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new BpjsClassification
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<BpjsClassificationDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<BpjsClassificationDto> Handle(CreateBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BpjsClassification>().AddAsync(request.BpjsClassificationDto.Adapt<BpjsClassification>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

                return result.Adapt<BpjsClassificationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BpjsClassificationDto>> Handle(CreateListBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BpjsClassification>().AddAsync(request.BpjsClassificationDtos.Adapt<List<BpjsClassification>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

                return result.Adapt<List<BpjsClassificationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<BpjsClassificationDto> Handle(UpdateBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BpjsClassification>().UpdateAsync(request.BpjsClassificationDto.Adapt<BpjsClassification>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

                return result.Adapt<BpjsClassificationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BpjsClassificationDto>> Handle(UpdateListBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BpjsClassification>().UpdateAsync(request.BpjsClassificationDtos.Adapt<List<BpjsClassification>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

                return result.Adapt<List<BpjsClassificationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteBpjsClassificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<BpjsClassification>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<BpjsClassification>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBpjsClassificationQuery_");

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
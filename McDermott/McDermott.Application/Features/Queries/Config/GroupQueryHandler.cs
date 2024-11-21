using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Config
{
    public class GroupQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGroupQuery, (List<GroupDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleGroupQuery, GroupDto>,
        IRequestHandler<ValidateGroupQuery, bool>,
        IRequestHandler<BulkValidateGroupQuery, List<GroupDto>>,

        IRequestHandler<CreateGroupRequest, GroupDto>,
        IRequestHandler<CreateListGroupRequest, List<GroupDto>>,
        IRequestHandler<UpdateGroupRequest, GroupDto>,
        IRequestHandler<UpdateListGroupRequest, List<GroupDto>>,
        IRequestHandler<DeleteGroupRequest, bool>
    {
        #region GET

        public async Task<GroupDto> Handle(GetSingleGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Group>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Group>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Group>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new Group
                    {
                        Id = x.Id,
                        Name = x.Name
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GroupDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<List<GroupDto>> Handle(BulkValidateGroupQuery request, CancellationToken cancellationToken)
        {
            var GroupDtos = request.GroupsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var GroupNames = GroupDtos.Select(x => x.Name).Distinct().ToList();

            var existingGroups = await _unitOfWork.Repository<Group>()
                .Entities
                .AsNoTracking()
                .Where(v => GroupNames.Contains(v.Name))
                .ToListAsync(cancellationToken);

            return existingGroups.Adapt<List<GroupDto>>();
        }

        public async Task<(List<GroupDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Group>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Group>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Group>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new Group
                    {
                        Id = x.Id,
                        Name = x.Name
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<GroupDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GroupDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateGroupQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Group>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<GroupDto> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDto.Adapt<Group>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GroupDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupDto>> Handle(CreateListGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Group>().AddAsync(request.GroupDtos.Adapt<List<Group>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GroupDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GroupDto> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDto.Adapt<Group>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GroupDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupDto>> Handle(UpdateListGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Group>().UpdateAsync(request.GroupDtos.Adapt<List<Group>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GroupDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Group>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Group>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupQuery_"); // Ganti dengan key yang sesuai

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
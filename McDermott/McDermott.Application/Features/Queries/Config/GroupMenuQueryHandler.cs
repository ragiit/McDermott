using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.GroupMenuCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class GroupMenuQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGroupMenuQuery, (List<GroupMenuDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleGroupMenuQuery, GroupMenuDto>,
        IRequestHandler<ValidateGroupMenuQuery, bool>,
        IRequestHandler<CreateGroupMenuRequest, GroupMenuDto>,
        IRequestHandler<BulkValidateGroupMenuQuery, List<GroupMenuDto>>,
        IRequestHandler<CreateListGroupMenuRequest, List<GroupMenuDto>>,
        IRequestHandler<UpdateGroupMenuRequest, GroupMenuDto>,
        IRequestHandler<UpdateListGroupMenuRequest, List<GroupMenuDto>>,
        IRequestHandler<DeleteGroupMenuRequest, bool>
    {
        #region GET

        public async Task<List<GroupMenuDto>> Handle(BulkValidateGroupMenuQuery request, CancellationToken cancellationToken)
        {
            var GroupMenuDtos = request.GroupMenusToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var GroupMenuNames = GroupMenuDtos.Select(x => x.GroupId).Distinct().ToList();
            var A = GroupMenuDtos.Select(x => x.MenuId).Distinct().ToList();
            var B = GroupMenuDtos.Select(x => x.IsCreate).Distinct().ToList();
            var C = GroupMenuDtos.Select(x => x.IsRead).Distinct().ToList();
            var D = GroupMenuDtos.Select(x => x.IsUpdate).Distinct().ToList();
            var E = GroupMenuDtos.Select(x => x.IsDelete).Distinct().ToList();
            var F = GroupMenuDtos.Select(x => x.IsImport).Distinct().ToList();

            var existingGroupMenus = await _unitOfWork.Repository<GroupMenu>()
                .Entities
                .AsNoTracking()
                .Where(v => GroupMenuNames.Contains(v.GroupId)
                            && A.Contains(v.MenuId)
                            && B.Contains((bool)(v.IsCreate))
                            && C.Contains((bool)(v.IsRead))
                            && D.Contains((bool)(v.IsUpdate))
                            && E.Contains((bool)(v.IsDelete))
                            && F.Contains((bool)(v.IsImport)))
                .ToListAsync(cancellationToken);

            return existingGroupMenus.Adapt<List<GroupMenuDto>>();
        }

        public async Task<bool> Handle(ValidateGroupMenuQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GroupMenu>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<GroupMenuDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGroupMenuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GroupMenu>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GroupMenu>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GroupMenu>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Menu.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Menu.Parent.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GroupMenu
                    {
                        Id = x.Id,
                        MenuId = x.MenuId,
                        Menu = new Menu
                        {
                            Name = x.Menu.Name,
                            Parent = new Menu
                            {
                                Name = x.Menu.Parent.Name
                            }
                        },

                        IsCreate = x.IsCreate,
                        IsDelete = x.IsDelete,
                        IsDefaultData = x.IsDefaultData,
                        IsImport = x.IsImport,
                        IsRead = x.IsRead,
                        IsUpdate = x.IsUpdate,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<GroupMenuDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GroupMenuDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GroupMenuDto> Handle(GetSingleGroupMenuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GroupMenu>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GroupMenu>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GroupMenu>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.GroupMenu.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GroupMenu
                    {
                        Id = x.Id,
                        MenuId = x.MenuId,
                        Menu = new Menu
                        {
                            Name = x.Menu.Name,
                            Parent = new Menu
                            {
                                Name = x.Menu.Parent.Name
                            }
                        },

                        IsCreate = x.IsCreate,
                        IsDelete = x.IsDelete,
                        IsDefaultData = x.IsDefaultData,
                        IsImport = x.IsImport,
                        IsRead = x.IsRead,
                        IsUpdate = x.IsUpdate,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GroupMenuDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GroupMenuDto> Handle(CreateGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GroupMenu>().AddAsync(request.GroupMenuDto.Adapt<CreateUpdateGroupMenuDto>().Adapt<GroupMenu>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GroupMenuDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupMenuDto>> Handle(CreateListGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GroupMenu>().AddAsync(request.GroupMenuDtos.Adapt<List<GroupMenu>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GroupMenuDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GroupMenuDto> Handle(UpdateGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GroupMenu>().UpdateAsync(request.GroupMenuDto.Adapt<CreateUpdateGroupMenuDto>().Adapt<GroupMenu>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GroupMenuDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupMenuDto>> Handle(UpdateListGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GroupMenu>().UpdateAsync(request.GroupMenuDtos.Adapt<List<GroupMenu>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GroupMenuDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGroupMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GroupMenu>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GroupMenu>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGroupMenuQuery_"); // Ganti dengan key yang sesuai

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
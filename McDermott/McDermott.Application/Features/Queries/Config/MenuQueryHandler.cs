using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.MenuCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class MenuQueryHandler
        (IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMenuQuery, (List<MenuDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleMenuQuery, MenuDto>,
        IRequestHandler<CreateMenuRequest, MenuDto>,
        IRequestHandler<BulkValidateMenuQuery, List<MenuDto>>,
        IRequestHandler<ValidateMenuQuery, bool>,
        IRequestHandler<CreateListMenuRequest, List<MenuDto>>,
        IRequestHandler<UpdateMenuRequest, MenuDto>,
        IRequestHandler<UpdateListMenuRequest, List<MenuDto>>,
        IRequestHandler<DeleteMenuRequest, bool>
    {
        #region GET

        public async Task<List<MenuDto>> Handle(BulkValidateMenuQuery request, CancellationToken cancellationToken)
        {
            var MenuDtos = request.MenusToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var MenuNames = MenuDtos.Select(x => x.Name).Distinct().ToList();
            var A = MenuDtos.Select(x => x.ParentId).Distinct().ToList();
            var B = MenuDtos.Select(x => x.Url).Distinct().ToList();
            var C = MenuDtos.Select(x => x.Sequence).Distinct().ToList();

            var existingMenus = await _unitOfWork.Repository<Menu>()
                .Entities
                .AsNoTracking()
                .Where(v => MenuNames.Contains(v.Name)
                            && A.Contains(v.ParentId)
                            && B.Contains(v.Url)
                            && C.Contains(v.Sequence))
                .ToListAsync(cancellationToken);

            return existingMenus.Adapt<List<MenuDto>>();
        }

        public async Task<bool> Handle(ValidateMenuQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Menu>()
                .Entities
                .AsNoTracking()
                .AnyAsync(request.Predicate, cancellationToken);// Check if any record matches the condition
        }

        public async Task<(List<MenuDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Menu>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Menu>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Menu>)query).ThenBy(additionalOrderBy.OrderBy);
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
                         EF.Functions.Like(v.Parent.Name, $"%{request.SearchTerm}%") ||
                         v.Sequence.Equals(request.SearchTerm) ||
                         EF.Functions.Like(v.Url, $"%{request.SearchTerm}%")
                         );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Menu
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Sequence = x.Sequence,
                        Url = x.Url,
                        ParentId = x.ParentId,
                        Icon = x.Icon,
                        IsDefaultData = x.IsDefaultData,
                        Parent = new Menu
                        {
                            Name = x.Parent.Name
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

                    return (pagedItems.Adapt<List<MenuDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<MenuDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<MenuDto> Handle(GetSingleMenuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Menu>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Menu>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Menu>)query).ThenBy(additionalOrderBy.OrderBy);
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
                         EF.Functions.Like(v.Parent.Name, $"%{request.SearchTerm}%") ||
                         v.Sequence.Equals(request.SearchTerm) ||
                         EF.Functions.Like(v.Url, $"%{request.SearchTerm}%")
                         );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Menu
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Sequence = x.Sequence,
                        Url = x.Url,
                        ParentId = x.ParentId,
                        Icon = x.Icon,
                        IsDefaultData = x.IsDefaultData,
                        Parent = new Menu
                        {
                            Name = x.Parent.Name
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<MenuDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<MenuDto> Handle(CreateMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Menu>().AddAsync(request.MenuDto.Adapt<CreateUpdateMenuDto>().Adapt<Menu>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMenuQuery_");
                _cache.Remove("GetGroupMenuQuery_");

                return result.Adapt<MenuDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MenuDto>> Handle(CreateListMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Menu>().AddAsync(request.MenuDtos.Adapt<List<Menu>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMenuQuery_");
                _cache.Remove("GetGroupMenuQuery_");

                return result.Adapt<List<MenuDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MenuDto> Handle(UpdateMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Menu>().UpdateAsync(request.MenuDto.Adapt<CreateUpdateMenuDto>().Adapt<Menu>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMenuQuery_");
                _cache.Remove("GetGroupMenuQuery_");

                return result.Adapt<MenuDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MenuDto>> Handle(UpdateListMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Menu>().UpdateAsync(request.MenuDtos.Adapt<List<Menu>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMenuQuery_");
                _cache.Remove("GetGroupMenuQuery_");

                return result.Adapt<List<MenuDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Menu>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Menu>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMenuQuery_");
                _cache.Remove("GetGroupMenuQuery_");

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
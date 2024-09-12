using static McDermott.Application.Features.Commands.Config.MenuCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class MenuQueryHandler
        (IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMenuQuery, (List<MenuDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateMenuRequest, MenuDto>,
        IRequestHandler<ValidateMenuQuery, bool>,
        IRequestHandler<CreateListMenuRequest, List<MenuDto>>,
        IRequestHandler<UpdateMenuRequest, MenuDto>,
        IRequestHandler<UpdateListMenuRequest, List<MenuDto>>,
        IRequestHandler<DeleteMenuRequest, bool>
    {
        #region GET

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
                var query = _unitOfWork.Repository<Menu>().Entities
                    .AsNoTracking()
                    .Include(x => x.Parent)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Parent.Name, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * request.PageSize;

                var totalCount = await query.CountAsync(cancellationToken);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<MenuDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<MenuDto> Handle(CreateMenuRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Menu>().AddAsync(request.MenuDto.Adapt<Menu>());

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
                var result = await _unitOfWork.Repository<Menu>().UpdateAsync(request.MenuDto.Adapt<Menu>());

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
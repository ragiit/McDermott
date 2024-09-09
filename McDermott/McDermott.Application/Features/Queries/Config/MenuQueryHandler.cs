using static McDermott.Application.Features.Commands.Config.MenuCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class MenuQueryHandler
        (IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMenuQuery, List<MenuDto>>,
        IRequestHandler<CreateMenuRequest, MenuDto>,
        IRequestHandler<CreateListMenuRequest, List<MenuDto>>,
        IRequestHandler<UpdateMenuRequest, MenuDto>,
        IRequestHandler<UpdateListMenuRequest, List<MenuDto>>,
        IRequestHandler<DeleteMenuRequest, bool>
    {
        #region GET

        public async Task<List<MenuDto>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMenuQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique
                if (!_cache.TryGetValue(cacheKey, out List<Menu>? result))
                {
                    result = await _unitOfWork.Repository<Menu>().Entities
                        .AsNoTracking()
                        .OrderBy(x => x.Name)
                        .Include(x => x.Parent)
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MenuDto>>();
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
using Microsoft.Extensions.DependencyInjection;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.MenuCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class MenuCommand
    {
        public sealed record GetMenuQuery(Expression<Func<Menu, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<MenuDto>>;
        public sealed record CreateMenuRequest(MenuDto MenuDto, bool ReturnNewData = false) : IRequest<MenuDto>;
        public sealed record CreateListMenuRequest(List<MenuDto> MenuDtos, bool ReturnNewData = false) : IRequest<List<MenuDto>>;
        public sealed record UpdateMenuRequest(MenuDto MenuDto, bool ReturnNewData = false) : IRequest<MenuDto>;
        public sealed record UpdateListMenuRequest(List<MenuDto> MenuDtos, bool ReturnNewData = false) : IRequest<List<MenuDto>>;
        public sealed record DeleteMenuRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class MenuQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetMenuQuery, List<MenuDto>>,
        IRequestHandler<CreateMenuRequest, MenuDto>,
        IRequestHandler<CreateListMenuRequest, List<MenuDto>>,
        IRequestHandler<UpdateMenuRequest, MenuDto>,
        IRequestHandler<UpdateListMenuRequest, List<MenuDto>>,
        IRequestHandler<DeleteMenuRequest, bool>
    {
        private string CacheKey = "GetMenuQuery_";

        private async Task<(MenuDto, List<MenuDto>)> Result(Menu? result = null, List<Menu>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<MenuDto>(), []);
                else
                    return ((await unitOfWork.Repository<Menu>().Entities
                        .AsNoTracking()
                        .Include(x => x.Parent)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<MenuDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<MenuDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Menu>().Entities
                        .AsNoTracking()
                        .Include(x => x.Parent)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<MenuDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<MenuDto>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Menu> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                result = await unitOfWork.Repository<Menu>().Entities
                        .AsNoTracking()
                        .Include(x => x.Parent)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<MenuDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<MenuDto> Handle(CreateMenuRequest request, CancellationToken cancellationToken)
        {
            var req = request.MenuDto.Adapt<CreateUpdateMenuDto>();
            var result = await unitOfWork.Repository<Menu>().AddAsync(req.Adapt<Menu>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<MenuDto>> Handle(CreateListMenuRequest request, CancellationToken cancellationToken)
        {
            var req = request.MenuDtos.Adapt<List<CreateUpdateMenuDto>>();
            var result = await unitOfWork.Repository<Menu>().AddAsync(req.Adapt<List<Menu>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MenuDto> Handle(UpdateMenuRequest request, CancellationToken cancellationToken)
        {
            var req = request.MenuDto.Adapt<CreateUpdateMenuDto>();
            var result = await unitOfWork.Repository<Menu>().UpdateAsync(req.Adapt<Menu>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<MenuDto>> Handle(UpdateListMenuRequest request, CancellationToken cancellationToken)
        {
            var req = request.MenuDtos.Adapt<CreateUpdateMenuDto>();
            var result = await unitOfWork.Repository<Menu>().UpdateAsync(req.Adapt<List<Menu>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMenuRequest request, CancellationToken cancellationToken)
        {
            List<Menu> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Menu = await unitOfWork.Repository<Menu>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Menu != null)
                {
                    deletedCountries.Add(Menu);
                    await unitOfWork.Repository<Menu>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Menu>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Menu>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
                {
                    Type = EnumTypeReceiveData.Delete,
                    Data = deletedCountries,
                });
            }

            return true;
        }

        #endregion DELETE
    }
}
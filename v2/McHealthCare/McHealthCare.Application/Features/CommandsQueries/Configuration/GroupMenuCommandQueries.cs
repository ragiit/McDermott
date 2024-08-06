using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Application.Extentions;
using McHealthCare.Application.Interfaces;
using McHealthCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.GroupMenuCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class GroupMenuCommand
    {
        public sealed record GetGroupMenuQuery(Expression<Func<GroupMenu, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<GroupMenuDto>>;
        public sealed record CreateGroupMenuRequest(GroupMenuDto GroupMenuDto, bool ReturnNewData = false) : IRequest<GroupMenuDto>;
        public sealed record CreateListGroupMenuRequest(List<GroupMenuDto> GroupMenuDtos, bool ReturnNewData = false) : IRequest<List<GroupMenuDto>>;
        public sealed record UpdateGroupMenuRequest(GroupMenuDto GroupMenuDto, bool ReturnNewData = false) : IRequest<GroupMenuDto>;
        public sealed record UpdateListGroupMenuRequest(List<GroupMenuDto> GroupMenuDtos, bool ReturnNewData = false) : IRequest<List<GroupMenuDto>>;
        public sealed record DeleteGroupMenuRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class GroupMenuQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService) :
        IRequestHandler<GetGroupMenuQuery, List<GroupMenuDto>>,
        IRequestHandler<CreateGroupMenuRequest, GroupMenuDto>,
        IRequestHandler<CreateListGroupMenuRequest, List<GroupMenuDto>>,
        IRequestHandler<UpdateGroupMenuRequest, GroupMenuDto>,
        IRequestHandler<UpdateListGroupMenuRequest, List<GroupMenuDto>>,
        IRequestHandler<DeleteGroupMenuRequest, bool>
    {
        private string CacheKey = "GetGroupMenuQuery_";

        private async Task<(GroupMenuDto, List<GroupMenuDto>)> Result(GroupMenu? result = null, List<GroupMenu>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<GroupMenuDto>(), []);
                else
                    return ((await unitOfWork.Repository<GroupMenu>().Entities
                        .AsNoTracking()
                        .Include(x => x.Group)
                        .Include(x => x.Menu)
                        .Include(x => x.Menu.Parent)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<GroupMenuDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<GroupMenuDto>>());
                else
                    return (new(), (await unitOfWork.Repository<GroupMenu>().Entities
                        .AsNoTracking()
                        .Include(x => x.Group)
                        .Include(x => x.Menu)
                        .Include(x => x.Menu.Parent)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<GroupMenuDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<GroupMenuDto>> Handle(GetGroupMenuQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<GroupMenu> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<GroupMenu>().Entities
                    .AsNoTracking()
                    .Include(x => x.Group)
                    .Include(x => x.Menu)
                    .Include(x => x.Menu.Parent)
                    .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<GroupMenuDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<GroupMenuDto> Handle(CreateGroupMenuRequest request, CancellationToken cancellationToken)
        {
            var req = request.GroupMenuDto.Adapt<CreateUpdateGroupMenuDto>();
            var result = await unitOfWork.Repository<GroupMenu>().AddAsync(req.Adapt<GroupMenu>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<GroupMenuDto>> Handle(CreateListGroupMenuRequest request, CancellationToken cancellationToken)
        {
            var req = request.GroupMenuDtos.Adapt<List<CreateUpdateGroupMenuDto>>();
            var result = await unitOfWork.Repository<GroupMenu>().AddAsync(req.Adapt<List<GroupMenu>>());
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

        public async Task<GroupMenuDto> Handle(UpdateGroupMenuRequest request, CancellationToken cancellationToken)
        {
            var req = request.GroupMenuDto.Adapt<CreateUpdateGroupMenuDto>();
            var result = await unitOfWork.Repository<GroupMenu>().UpdateAsync(req.Adapt<GroupMenu>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<GroupMenuDto>> Handle(UpdateListGroupMenuRequest request, CancellationToken cancellationToken)
        {
            var req = request.GroupMenuDtos.Adapt<CreateUpdateGroupMenuDto>();
            var result = await unitOfWork.Repository<GroupMenu>().UpdateAsync(req.Adapt<List<GroupMenu>>());
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

        public async Task<bool> Handle(DeleteGroupMenuRequest request, CancellationToken cancellationToken)
        {
            List<GroupMenu> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var GroupMenu = await unitOfWork.Repository<GroupMenu>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (GroupMenu != null)
                {
                    deletedCountries.Add(GroupMenu);
                    await unitOfWork.Repository<GroupMenu>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<GroupMenu>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<GroupMenu>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
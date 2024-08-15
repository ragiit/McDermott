using Microsoft.Extensions.DependencyInjection;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.GroupCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class GroupCommand
    {
        public sealed record GetGroupQuery(Expression<Func<Group, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<GroupDto>>;
        public sealed record CreateGroupRequest(GroupDto GroupDto, bool ReturnNewData = false) : IRequest<GroupDto>;
        public sealed record CreateListGroupRequest(List<GroupDto> GroupDtos, bool ReturnNewData = false) : IRequest<List<GroupDto>>;
        public sealed record UpdateGroupRequest(GroupDto GroupDto, bool ReturnNewData = false) : IRequest<GroupDto>;
        public sealed record UpdateListGroupRequest(List<GroupDto> GroupDtos, bool ReturnNewData = false) : IRequest<List<GroupDto>>;
        public sealed record DeleteGroupRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class GroupQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetGroupQuery, List<GroupDto>>,
        IRequestHandler<CreateGroupRequest, GroupDto>,
        IRequestHandler<CreateListGroupRequest, List<GroupDto>>,
        IRequestHandler<UpdateGroupRequest, GroupDto>,
        IRequestHandler<UpdateListGroupRequest, List<GroupDto>>,
        IRequestHandler<DeleteGroupRequest, bool>
    {
        private string CacheKey = "GetGroupQuery_";

        private async Task<(GroupDto, List<GroupDto>)> Result(Group? result = null, List<Group>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<GroupDto>(), []);
                else
                    return ((await unitOfWork.Repository<Group>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<GroupDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<GroupDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Group>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<GroupDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<GroupDto>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Group> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                result = await unitOfWork.Repository<Group>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<GroupDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<GroupDto> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
        {
            var req = request.GroupDto.Adapt<CreateUpdateGroupDto>();
            var result = await unitOfWork.Repository<Group>().AddAsync(req.Adapt<Group>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<GroupDto>> Handle(CreateListGroupRequest request, CancellationToken cancellationToken)
        {
            var req = request.GroupDtos.Adapt<List<CreateUpdateGroupDto>>();
            var result = await unitOfWork.Repository<Group>().AddAsync(req.Adapt<List<Group>>());
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

        public async Task<GroupDto> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
        {
            var req = request.GroupDto.Adapt<CreateUpdateGroupDto>();
            var result = await unitOfWork.Repository<Group>().UpdateAsync(req.Adapt<Group>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<GroupDto>> Handle(UpdateListGroupRequest request, CancellationToken cancellationToken)
        {
            var req = request.GroupDtos.Adapt<CreateUpdateGroupDto>();
            var result = await unitOfWork.Repository<Group>().UpdateAsync(req.Adapt<List<Group>>());
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

        public async Task<bool> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
        {
            List<Group> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Group = await unitOfWork.Repository<Group>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Group != null)
                {
                    deletedCountries.Add(Group);
                    await unitOfWork.Repository<Group>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Group>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Group>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
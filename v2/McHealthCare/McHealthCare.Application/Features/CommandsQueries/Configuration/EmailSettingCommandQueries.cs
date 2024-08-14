using Microsoft.Extensions.DependencyInjection;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.EmailSettingCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class EmailSettingCommand
    {
        public sealed record GetEmailSettingQuery(Expression<Func<EmailSetting, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<EmailSettingDto>>;
        public sealed record CreateEmailSettingRequest(EmailSettingDto EmailSettingDto, bool ReturnNewData = false) : IRequest<EmailSettingDto>;
        public sealed record CreateListEmailSettingRequest(List<EmailSettingDto> EmailSettingDtos, bool ReturnNewData = false) : IRequest<List<EmailSettingDto>>;
        public sealed record UpdateEmailSettingRequest(EmailSettingDto EmailSettingDto, bool ReturnNewData = false) : IRequest<EmailSettingDto>;
        public sealed record UpdateListEmailSettingRequest(List<EmailSettingDto> EmailSettingDtos, bool ReturnNewData = false) : IRequest<List<EmailSettingDto>>;
        public sealed record DeleteEmailSettingRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class EmailSettingQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetEmailSettingQuery, List<EmailSettingDto>>,
        IRequestHandler<CreateEmailSettingRequest, EmailSettingDto>,
        IRequestHandler<CreateListEmailSettingRequest, List<EmailSettingDto>>,
        IRequestHandler<UpdateEmailSettingRequest, EmailSettingDto>,
        IRequestHandler<UpdateListEmailSettingRequest, List<EmailSettingDto>>,
        IRequestHandler<DeleteEmailSettingRequest, bool>
    {
        private string CacheKey = "GetEmailSettingQuery_";

        private async Task<(EmailSettingDto, List<EmailSettingDto>)> Result(EmailSetting? result = null, List<EmailSetting>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<EmailSettingDto>(), []);
                else
                    return ((await unitOfWork.Repository<EmailSetting>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<EmailSettingDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<EmailSettingDto>>());
                else
                    return (new(), (await unitOfWork.Repository<EmailSetting>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<EmailSettingDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<EmailSettingDto>> Handle(GetEmailSettingQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<EmailSetting> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                result = await unitOfWork.Repository<EmailSetting>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<EmailSettingDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<EmailSettingDto> Handle(CreateEmailSettingRequest request, CancellationToken cancellationToken)
        {
            var req = request.EmailSettingDto.Adapt<CreateUpdateEmailSettingDto>();
            var result = await unitOfWork.Repository<EmailSetting>().AddAsync(req.Adapt<EmailSetting>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<EmailSettingDto>> Handle(CreateListEmailSettingRequest request, CancellationToken cancellationToken)
        {
            var req = request.EmailSettingDtos.Adapt<List<CreateUpdateEmailSettingDto>>();
            var result = await unitOfWork.Repository<EmailSetting>().AddAsync(req.Adapt<List<EmailSetting>>());
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

        public async Task<EmailSettingDto> Handle(UpdateEmailSettingRequest request, CancellationToken cancellationToken)
        {
            var req = request.EmailSettingDto.Adapt<CreateUpdateEmailSettingDto>();
            var result = await unitOfWork.Repository<EmailSetting>().UpdateAsync(req.Adapt<EmailSetting>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<EmailSettingDto>> Handle(UpdateListEmailSettingRequest request, CancellationToken cancellationToken)
        {
            var req = request.EmailSettingDtos.Adapt<CreateUpdateEmailSettingDto>();
            var result = await unitOfWork.Repository<EmailSetting>().UpdateAsync(req.Adapt<List<EmailSetting>>());
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

        public async Task<bool> Handle(DeleteEmailSettingRequest request, CancellationToken cancellationToken)
        {
            List<EmailSetting> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var EmailSetting = await unitOfWork.Repository<EmailSetting>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (EmailSetting != null)
                {
                    deletedCountries.Add(EmailSetting);
                    await unitOfWork.Repository<EmailSetting>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<EmailSetting>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<EmailSetting>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
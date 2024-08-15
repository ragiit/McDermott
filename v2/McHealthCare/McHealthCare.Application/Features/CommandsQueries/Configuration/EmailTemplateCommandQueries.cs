using Microsoft.Extensions.DependencyInjection;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.EmailTemplateCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class EmailTemplateCommand
    {
        public sealed record GetEmailTemplateQuery(Expression<Func<EmailTemplate, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<EmailTemplateDto>>;
        public sealed record CreateEmailTemplateRequest(EmailTemplateDto EmailTemplateDto, bool ReturnNewData = false) : IRequest<EmailTemplateDto>;
        public sealed record CreateListEmailTemplateRequest(List<EmailTemplateDto> EmailTemplateDtos, bool ReturnNewData = false) : IRequest<List<EmailTemplateDto>>;
        public sealed record UpdateEmailTemplateRequest(EmailTemplateDto EmailTemplateDto, bool ReturnNewData = false) : IRequest<EmailTemplateDto>;
        public sealed record UpdateListEmailTemplateRequest(List<EmailTemplateDto> EmailTemplateDtos, bool ReturnNewData = false) : IRequest<List<EmailTemplateDto>>;
        public sealed record DeleteEmailTemplateRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class EmailTemplateQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetEmailTemplateQuery, List<EmailTemplateDto>>,
        IRequestHandler<CreateEmailTemplateRequest, EmailTemplateDto>,
        IRequestHandler<CreateListEmailTemplateRequest, List<EmailTemplateDto>>,
        IRequestHandler<UpdateEmailTemplateRequest, EmailTemplateDto>,
        IRequestHandler<UpdateListEmailTemplateRequest, List<EmailTemplateDto>>,
        IRequestHandler<DeleteEmailTemplateRequest, bool>
    {
        private string CacheKey = "GetEmailTemplateQuery_";

        private async Task<(EmailTemplateDto, List<EmailTemplateDto>)> Result(EmailTemplate? result = null, List<EmailTemplate>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<EmailTemplateDto>(), []);
                else
                    return ((await unitOfWork.Repository<EmailTemplate>().Entities
                        .AsNoTracking()
                        .Include(x => x.By)
                        .Include(x => x.EmailFrom)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<EmailTemplateDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<EmailTemplateDto>>());
                else
                    return (new(), (await unitOfWork.Repository<EmailTemplate>().Entities
                        .AsNoTracking()
                        .Include(x => x.By)
                        .Include(x => x.EmailFrom)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<EmailTemplateDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<EmailTemplateDto>> Handle(GetEmailTemplateQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<EmailTemplate> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                result = await unitOfWork.Repository<EmailTemplate>().Entities
                        .AsNoTracking()
                        .Include(x => x.By)
                        .Include(x => x.EmailFrom)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<EmailTemplateDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<EmailTemplateDto> Handle(CreateEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var req = request.EmailTemplateDto.Adapt<CreateUpdateEmailTemplateDto>();
            var result = await unitOfWork.Repository<EmailTemplate>().AddAsync(req.Adapt<EmailTemplate>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<EmailTemplateDto>> Handle(CreateListEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var req = request.EmailTemplateDtos.Adapt<List<CreateUpdateEmailTemplateDto>>();
            var result = await unitOfWork.Repository<EmailTemplate>().AddAsync(req.Adapt<List<EmailTemplate>>());
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

        public async Task<EmailTemplateDto> Handle(UpdateEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var req = request.EmailTemplateDto.Adapt<CreateUpdateEmailTemplateDto>();
            var result = await unitOfWork.Repository<EmailTemplate>().UpdateAsync(req.Adapt<EmailTemplate>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<EmailTemplateDto>> Handle(UpdateListEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            var req = request.EmailTemplateDtos.Adapt<CreateUpdateEmailTemplateDto>();
            var result = await unitOfWork.Repository<EmailTemplate>().UpdateAsync(req.Adapt<List<EmailTemplate>>());
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

        public async Task<bool> Handle(DeleteEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            List<EmailTemplate> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var EmailTemplate = await unitOfWork.Repository<EmailTemplate>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (EmailTemplate != null)
                {
                    deletedCountries.Add(EmailTemplate);
                    await unitOfWork.Repository<EmailTemplate>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<EmailTemplate>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<EmailTemplate>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
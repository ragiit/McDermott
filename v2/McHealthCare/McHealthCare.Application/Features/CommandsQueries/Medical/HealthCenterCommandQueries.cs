using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.HealthCenterCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class HealthCenterCommand
    {
        public sealed record GetHealthCenterQuery(Expression<Func<HealthCenter, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<HealthCenterDto>>;
        public sealed record CreateHealthCenterRequest(HealthCenterDto HealthCenterDto, bool ReturnNewData = false) : IRequest<HealthCenterDto>;
        public sealed record CreateListHealthCenterRequest(List<HealthCenterDto> HealthCenterDtos, bool ReturnNewData = false) : IRequest<List<HealthCenterDto>>;
        public sealed record UpdateHealthCenterRequest(HealthCenterDto HealthCenterDto, bool ReturnNewData = false) : IRequest<HealthCenterDto>;
        public sealed record UpdateListHealthCenterRequest(List<HealthCenterDto> HealthCenterDtos, bool ReturnNewData = false) : IRequest<List<HealthCenterDto>>;
        public sealed record DeleteHealthCenterRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class HealthCenterQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataHealthCenter) :
        IRequestHandler<GetHealthCenterQuery, List<HealthCenterDto>>,
        IRequestHandler<CreateHealthCenterRequest, HealthCenterDto>,
        IRequestHandler<CreateListHealthCenterRequest, List<HealthCenterDto>>,
        IRequestHandler<UpdateHealthCenterRequest, HealthCenterDto>,
        IRequestHandler<UpdateListHealthCenterRequest, List<HealthCenterDto>>,
        IRequestHandler<DeleteHealthCenterRequest, bool>
    {
        private string CacheKey = "GetHealthCenterQuery_";

        private async Task<(HealthCenterDto, List<HealthCenterDto>)> Result(HealthCenter? result = null, List<HealthCenter>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<HealthCenterDto>(), []);
                else
                    return ((await unitOfWork.Repository<HealthCenter>().Entities
                        .AsNoTracking()
                        .Include(x=>x.City)
                        .Include(x=>x.Province)
                        .Include(x=>x.Country)
                        .Include(x=>x.Buildings)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<HealthCenterDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<HealthCenterDto>>());
                else
                    return (new(), (await unitOfWork.Repository<HealthCenter>().Entities
                        .AsNoTracking()
                        .Include(x => x.City)
                        .Include(x => x.Province)
                        .Include(x => x.Country)
                        .Include(x => x.Buildings)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<HealthCenterDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<HealthCenterDto>> Handle(GetHealthCenterQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<HealthCenter>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<HealthCenter>().Entities
                        .AsNoTracking()
                        .Include(x => x.City)
                        .Include(x => x.Province)
                        .Include(x => x.Country)
                        .Include(x => x.Buildings)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<HealthCenterDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<HealthCenterDto> Handle(CreateHealthCenterRequest request, CancellationToken cancellationToken)
        {
            var req = request.HealthCenterDto.Adapt<CreateUpdateHealthCenterDto>();
            var result = await unitOfWork.Repository<HealthCenter>().AddAsync(req.Adapt<HealthCenter>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataHealthCenter.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<HealthCenterDto>> Handle(CreateListHealthCenterRequest request, CancellationToken cancellationToken)
        {
            var req = request.HealthCenterDtos.Adapt<List<CreateUpdateHealthCenterDto>>();
            var result = await unitOfWork.Repository<HealthCenter>().AddAsync(req.Adapt<List<HealthCenter>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataHealthCenter.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<HealthCenterDto> Handle(UpdateHealthCenterRequest request, CancellationToken cancellationToken)
        {
            var req = request.HealthCenterDto.Adapt<CreateUpdateHealthCenterDto>();
            var result = await unitOfWork.Repository<HealthCenter>().UpdateAsync(req.Adapt<HealthCenter>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataHealthCenter.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<HealthCenterDto>> Handle(UpdateListHealthCenterRequest request, CancellationToken cancellationToken)
        {
            var req = request.HealthCenterDtos.Adapt<CreateUpdateHealthCenterDto>();
            var result = await unitOfWork.Repository<HealthCenter>().UpdateAsync(req.Adapt<List<HealthCenter>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataHealthCenter.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteHealthCenterRequest request, CancellationToken cancellationToken)
        {
            List<HealthCenter> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var HealthCenter = await unitOfWork.Repository<HealthCenter>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (HealthCenter != null)
                {
                    deletedCountries.Add(HealthCenter);
                    await unitOfWork.Repository<HealthCenter>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<HealthCenter>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<HealthCenter>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataHealthCenter.Clients.All.ReceiveNotification(new ReceiveDataDto
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

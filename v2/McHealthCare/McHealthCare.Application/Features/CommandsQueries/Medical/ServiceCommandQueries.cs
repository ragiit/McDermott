using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.ServiceCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class ServiceCommand
    {
        public sealed record GetServiceQuery(Expression<Func<Service, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<ServiceDto>>;
        public sealed record CreateServiceRequest(ServiceDto ServiceDto, bool ReturnNewData = false) : IRequest<ServiceDto>;
        public sealed record CreateListServiceRequest(List<ServiceDto> ServiceDtos, bool ReturnNewData = false) : IRequest<List<ServiceDto>>;
        public sealed record UpdateServiceRequest(ServiceDto ServiceDto, bool ReturnNewData = false) : IRequest<ServiceDto>;
        public sealed record UpdateListServiceRequest(List<ServiceDto> ServiceDtos, bool ReturnNewData = false) : IRequest<List<ServiceDto>>;
        public sealed record DeleteServiceRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class ServiceQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService) :
        IRequestHandler<GetServiceQuery, List<ServiceDto>>,
        IRequestHandler<CreateServiceRequest, ServiceDto>,
        IRequestHandler<CreateListServiceRequest, List<ServiceDto>>,
        IRequestHandler<UpdateServiceRequest, ServiceDto>,
        IRequestHandler<UpdateListServiceRequest, List<ServiceDto>>,
        IRequestHandler<DeleteServiceRequest, bool>
    {
        private string CacheKey = "GetServiceQuery_";

        private async Task<(ServiceDto, List<ServiceDto>)> Result(Service? result = null, List<Service>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<ServiceDto>(), []);
                else
                    return ((await unitOfWork.Repository<Service>().Entities
                        .AsNoTracking()
                        .Include(x=>x.Serviced)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<ServiceDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<ServiceDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Service>().Entities
                        .AsNoTracking()
                        .Include(x => x.Serviced)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<ServiceDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<ServiceDto>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Service>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Service>().Entities
                        .AsNoTracking()
                        .Include(x => x.Serviced)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<ServiceDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<ServiceDto> Handle(CreateServiceRequest request, CancellationToken cancellationToken)
        {
            var req = request.ServiceDto.Adapt<CreateUpdateServiceDto>();
            var result = await unitOfWork.Repository<Service>().AddAsync(req.Adapt<Service>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ServiceDto>> Handle(CreateListServiceRequest request, CancellationToken cancellationToken)
        {
            var req = request.ServiceDtos.Adapt<List<CreateUpdateServiceDto>>();
            var result = await unitOfWork.Repository<Service>().AddAsync(req.Adapt<List<Service>>());
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

        public async Task<ServiceDto> Handle(UpdateServiceRequest request, CancellationToken cancellationToken)
        {
            var req = request.ServiceDto.Adapt<CreateUpdateServiceDto>();
            var result = await unitOfWork.Repository<Service>().UpdateAsync(req.Adapt<Service>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ServiceDto>> Handle(UpdateListServiceRequest request, CancellationToken cancellationToken)
        {
            var req = request.ServiceDtos.Adapt<CreateUpdateServiceDto>();
            var result = await unitOfWork.Repository<Service>().UpdateAsync(req.Adapt<List<Service>>());
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

        public async Task<bool> Handle(DeleteServiceRequest request, CancellationToken cancellationToken)
        {
            List<Service> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Service = await unitOfWork.Repository<Service>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Service != null)
                {
                    deletedCountries.Add(Service);
                    await unitOfWork.Repository<Service>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Service>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Service>().DeleteAsync(x => request.Ids.Contains(x.Id));
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

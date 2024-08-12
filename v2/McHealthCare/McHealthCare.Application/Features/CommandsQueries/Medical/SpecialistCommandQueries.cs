using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.SpecialistCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class SpecialistCommand
    {
        public sealed record GetSpecialistQuery(Expression<Func<Specialist, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<SpecialistDto>>;
        public sealed record CreateSpecialistRequest(SpecialistDto SpecialistDto, bool ReturnNewData = false) : IRequest<SpecialistDto>;
        public sealed record CreateListSpecialistRequest(List<SpecialistDto> SpecialistDtos, bool ReturnNewData = false) : IRequest<List<SpecialistDto>>;
        public sealed record UpdateSpecialistRequest(SpecialistDto SpecialistDto, bool ReturnNewData = false) : IRequest<SpecialistDto>;
        public sealed record UpdateListSpecialistRequest(List<SpecialistDto> SpecialistDtos, bool ReturnNewData = false) : IRequest<List<SpecialistDto>>;
        public sealed record DeleteSpecialistRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class SpecialistQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataSpecialist) :
        IRequestHandler<GetSpecialistQuery, List<SpecialistDto>>,
        IRequestHandler<CreateSpecialistRequest, SpecialistDto>,
        IRequestHandler<CreateListSpecialistRequest, List<SpecialistDto>>,
        IRequestHandler<UpdateSpecialistRequest, SpecialistDto>,
        IRequestHandler<UpdateListSpecialistRequest, List<SpecialistDto>>,
        IRequestHandler<DeleteSpecialistRequest, bool>
    {
        private string CacheKey = "GetSpecialistQuery_";

        private async Task<(SpecialistDto, List<SpecialistDto>)> Result(Specialist? result = null, List<Specialist>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<SpecialistDto>(), []);
                else
                    return ((await unitOfWork.Repository<Specialist>().Entities
                        .AsNoTracking()                        
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<SpecialistDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<SpecialistDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Specialist>().Entities
                        .AsNoTracking()                        
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<SpecialistDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<SpecialistDto>> Handle(GetSpecialistQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Specialist>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Specialist>().Entities
                        .AsNoTracking()                        
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable()?.Where(request.Predicate)?.ToList() ?? [];

            return result?.Adapt<List<SpecialistDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<SpecialistDto> Handle(CreateSpecialistRequest request, CancellationToken cancellationToken)
        {
            var req = request.SpecialistDto.Adapt<CreateUpdateSpecialistDto>();
            var result = await unitOfWork.Repository<Specialist>().AddAsync(req.Adapt<Specialist>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSpecialist.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<SpecialistDto>> Handle(CreateListSpecialistRequest request, CancellationToken cancellationToken)
        {
            var req = request.SpecialistDtos.Adapt<List<CreateUpdateSpecialistDto>>();
            var result = await unitOfWork.Repository<Specialist>().AddAsync(req.Adapt<List<Specialist>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSpecialist.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SpecialistDto> Handle(UpdateSpecialistRequest request, CancellationToken cancellationToken)
        {
            var req = request.SpecialistDto.Adapt<CreateUpdateSpecialistDto>();
            var result = await unitOfWork.Repository<Specialist>().UpdateAsync(req.Adapt<Specialist>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataSpecialist.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<SpecialistDto>> Handle(UpdateListSpecialistRequest request, CancellationToken cancellationToken)
        {
            var req = request.SpecialistDtos.Adapt<CreateUpdateSpecialistDto>();
            var result = await unitOfWork.Repository<Specialist>().UpdateAsync(req.Adapt<List<Specialist>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSpecialist.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSpecialistRequest request, CancellationToken cancellationToken)
        {
            List<Specialist> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Specialist = await unitOfWork.Repository<Specialist>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Specialist != null)
                {
                    deletedCountries.Add(Specialist);
                    await unitOfWork.Repository<Specialist>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Specialist>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Specialist>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataSpecialist.Clients.All.ReceiveNotification(new ReceiveDataDto
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

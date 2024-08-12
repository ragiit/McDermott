using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.SampleTypeCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class SampleTypeCommand
    {
        public sealed record GetSampleTypeQuery(Expression<Func<SampleType, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<SampleTypeDto>>;
        public sealed record CreateSampleTypeRequest(SampleTypeDto SampleTypeDto, bool ReturnNewData = false) : IRequest<SampleTypeDto>;
        public sealed record CreateListSampleTypeRequest(List<SampleTypeDto> SampleTypeDtos, bool ReturnNewData = false) : IRequest<List<SampleTypeDto>>;
        public sealed record UpdateSampleTypeRequest(SampleTypeDto SampleTypeDto, bool ReturnNewData = false) : IRequest<SampleTypeDto>;
        public sealed record UpdateListSampleTypeRequest(List<SampleTypeDto> SampleTypeDtos, bool ReturnNewData = false) : IRequest<List<SampleTypeDto>>;
        public sealed record DeleteSampleTypeRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class SampleTypeQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataSampleType) :
        IRequestHandler<GetSampleTypeQuery, List<SampleTypeDto>>,
        IRequestHandler<CreateSampleTypeRequest, SampleTypeDto>,
        IRequestHandler<CreateListSampleTypeRequest, List<SampleTypeDto>>,
        IRequestHandler<UpdateSampleTypeRequest, SampleTypeDto>,
        IRequestHandler<UpdateListSampleTypeRequest, List<SampleTypeDto>>,
        IRequestHandler<DeleteSampleTypeRequest, bool>
    {
        private string CacheKey = "GetSampleTypeQuery_";

        private async Task<(SampleTypeDto, List<SampleTypeDto>)> Result(SampleType? result = null, List<SampleType>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<SampleTypeDto>(), []);
                else
                    return ((await unitOfWork.Repository<SampleType>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<SampleTypeDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<SampleTypeDto>>());
                else
                    return (new(), (await unitOfWork.Repository<SampleType>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<SampleTypeDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<SampleTypeDto>> Handle(GetSampleTypeQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<SampleType>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<SampleType>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<SampleTypeDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<SampleTypeDto> Handle(CreateSampleTypeRequest request, CancellationToken cancellationToken)
        {
            var req = request.SampleTypeDto.Adapt<CreateUpdateSampleTypeDto>();
            var result = await unitOfWork.Repository<SampleType>().AddAsync(req.Adapt<SampleType>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSampleType.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<SampleTypeDto>> Handle(CreateListSampleTypeRequest request, CancellationToken cancellationToken)
        {
            var req = request.SampleTypeDtos.Adapt<List<CreateUpdateSampleTypeDto>>();
            var result = await unitOfWork.Repository<SampleType>().AddAsync(req.Adapt<List<SampleType>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSampleType.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SampleTypeDto> Handle(UpdateSampleTypeRequest request, CancellationToken cancellationToken)
        {
            var req = request.SampleTypeDto.Adapt<CreateUpdateSampleTypeDto>();
            var result = await unitOfWork.Repository<SampleType>().UpdateAsync(req.Adapt<SampleType>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataSampleType.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<SampleTypeDto>> Handle(UpdateListSampleTypeRequest request, CancellationToken cancellationToken)
        {
            var req = request.SampleTypeDtos.Adapt<CreateUpdateSampleTypeDto>();
            var result = await unitOfWork.Repository<SampleType>().UpdateAsync(req.Adapt<List<SampleType>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSampleType.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSampleTypeRequest request, CancellationToken cancellationToken)
        {
            List<SampleType> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var SampleType = await unitOfWork.Repository<SampleType>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (SampleType != null)
                {
                    deletedCountries.Add(SampleType);
                    await unitOfWork.Repository<SampleType>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<SampleType>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<SampleType>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataSampleType.Clients.All.ReceiveNotification(new ReceiveDataDto
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

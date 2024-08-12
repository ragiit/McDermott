using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.LabUomCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class LabUomCommand
    {
        public sealed record GetLabUomQuery(Expression<Func<LabUom, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<LabUomDto>>;
        public sealed record CreateLabUomRequest(LabUomDto LabUomDto, bool ReturnNewData = false) : IRequest<LabUomDto>;
        public sealed record CreateListLabUomRequest(List<LabUomDto> LabUomDtos, bool ReturnNewData = false) : IRequest<List<LabUomDto>>;
        public sealed record UpdateLabUomRequest(LabUomDto LabUomDto, bool ReturnNewData = false) : IRequest<LabUomDto>;
        public sealed record UpdateListLabUomRequest(List<LabUomDto> LabUomDtos, bool ReturnNewData = false) : IRequest<List<LabUomDto>>;
        public sealed record DeleteLabUomRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class LabUomQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataLabUom) :
        IRequestHandler<GetLabUomQuery, List<LabUomDto>>,
        IRequestHandler<CreateLabUomRequest, LabUomDto>,
        IRequestHandler<CreateListLabUomRequest, List<LabUomDto>>,
        IRequestHandler<UpdateLabUomRequest, LabUomDto>,
        IRequestHandler<UpdateListLabUomRequest, List<LabUomDto>>,
        IRequestHandler<DeleteLabUomRequest, bool>
    {
        private string CacheKey = "GetLabUomQuery_";

        private async Task<(LabUomDto, List<LabUomDto>)> Result(LabUom? result = null, List<LabUom>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<LabUomDto>(), []);
                else
                    return ((await unitOfWork.Repository<LabUom>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<LabUomDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<LabUomDto>>());
                else
                    return (new(), (await unitOfWork.Repository<LabUom>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<LabUomDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<LabUomDto>> Handle(GetLabUomQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<LabUom>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<LabUom>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<LabUomDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<LabUomDto> Handle(CreateLabUomRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabUomDto.Adapt<CreateUpdateLabUomDto>();
            var result = await unitOfWork.Repository<LabUom>().AddAsync(req.Adapt<LabUom>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabUom.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<LabUomDto>> Handle(CreateListLabUomRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabUomDtos.Adapt<List<CreateUpdateLabUomDto>>();
            var result = await unitOfWork.Repository<LabUom>().AddAsync(req.Adapt<List<LabUom>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabUom.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabUomDto> Handle(UpdateLabUomRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabUomDto.Adapt<CreateUpdateLabUomDto>();
            var result = await unitOfWork.Repository<LabUom>().UpdateAsync(req.Adapt<LabUom>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataLabUom.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<LabUomDto>> Handle(UpdateListLabUomRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabUomDtos.Adapt<CreateUpdateLabUomDto>();
            var result = await unitOfWork.Repository<LabUom>().UpdateAsync(req.Adapt<List<LabUom>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabUom.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabUomRequest request, CancellationToken cancellationToken)
        {
            List<LabUom> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var LabUom = await unitOfWork.Repository<LabUom>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (LabUom != null)
                {
                    deletedCountries.Add(LabUom);
                    await unitOfWork.Repository<LabUom>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<LabUom>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<LabUom>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataLabUom.Clients.All.ReceiveNotification(new ReceiveDataDto
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

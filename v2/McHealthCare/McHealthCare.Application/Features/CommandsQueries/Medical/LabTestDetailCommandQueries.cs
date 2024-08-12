using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.LabTestDetailCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class LabTestDetailCommand
    {
        public sealed record GetLabTestDetailQuery(Expression<Func<LabTestDetail, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<LabTestDetailDto>>;
        public sealed record CreateLabTestDetailRequest(LabTestDetailDto LabTestDetailDto, bool ReturnNewData = false) : IRequest<LabTestDetailDto>;
        public sealed record CreateListLabTestDetailRequest(List<LabTestDetailDto> LabTestDetailDtos, bool ReturnNewData = false) : IRequest<List<LabTestDetailDto>>;
        public sealed record UpdateLabTestDetailRequest(LabTestDetailDto LabTestDetailDto, bool ReturnNewData = false) : IRequest<LabTestDetailDto>;
        public sealed record UpdateListLabTestDetailRequest(List<LabTestDetailDto> LabTestDetailDtos, bool ReturnNewData = false) : IRequest<List<LabTestDetailDto>>;
        public sealed record DeleteLabTestDetailRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class LabTestDetailQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataLabTestDetail) :
        IRequestHandler<GetLabTestDetailQuery, List<LabTestDetailDto>>,
        IRequestHandler<CreateLabTestDetailRequest, LabTestDetailDto>,
        IRequestHandler<CreateListLabTestDetailRequest, List<LabTestDetailDto>>,
        IRequestHandler<UpdateLabTestDetailRequest, LabTestDetailDto>,
        IRequestHandler<UpdateListLabTestDetailRequest, List<LabTestDetailDto>>,
        IRequestHandler<DeleteLabTestDetailRequest, bool>
    {
        private string CacheKey = "GetLabTestDetailQuery_";

        private async Task<(LabTestDetailDto, List<LabTestDetailDto>)> Result(LabTestDetail? result = null, List<LabTestDetail>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<LabTestDetailDto>(), []);
                else
                    return ((await unitOfWork.Repository<LabTestDetail>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<LabTestDetailDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<LabTestDetailDto>>());
                else
                    return (new(), (await unitOfWork.Repository<LabTestDetail>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<LabTestDetailDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<LabTestDetailDto>> Handle(GetLabTestDetailQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<LabTestDetail>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<LabTestDetail>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<LabTestDetailDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<LabTestDetailDto> Handle(CreateLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabTestDetailDto.Adapt<CreateUpdateLabTestDetailDto>();
            var result = await unitOfWork.Repository<LabTestDetail>().AddAsync(req.Adapt<LabTestDetail>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabTestDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<LabTestDetailDto>> Handle(CreateListLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabTestDetailDtos.Adapt<List<CreateUpdateLabTestDetailDto>>();
            var result = await unitOfWork.Repository<LabTestDetail>().AddAsync(req.Adapt<List<LabTestDetail>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabTestDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabTestDetailDto> Handle(UpdateLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabTestDetailDto.Adapt<CreateUpdateLabTestDetailDto>();
            var result = await unitOfWork.Repository<LabTestDetail>().UpdateAsync(req.Adapt<LabTestDetail>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataLabTestDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<LabTestDetailDto>> Handle(UpdateListLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabTestDetailDtos.Adapt<CreateUpdateLabTestDetailDto>();
            var result = await unitOfWork.Repository<LabTestDetail>().UpdateAsync(req.Adapt<List<LabTestDetail>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabTestDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            List<LabTestDetail> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var LabTestDetail = await unitOfWork.Repository<LabTestDetail>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (LabTestDetail != null)
                {
                    deletedCountries.Add(LabTestDetail);
                    await unitOfWork.Repository<LabTestDetail>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<LabTestDetail>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<LabTestDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataLabTestDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
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

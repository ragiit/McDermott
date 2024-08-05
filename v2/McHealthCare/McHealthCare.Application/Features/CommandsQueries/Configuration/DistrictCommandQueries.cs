using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Application.Extentions;
using McHealthCare.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class DistrictCommand
    {
        public sealed record GetDistrictQuery(Expression<Func<District, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<DistrictDto>>;
        public sealed record CreateDistrictRequest(DistrictDto DistrictDto, bool ReturnNewData = false) : IRequest<DistrictDto>;
        public sealed record CreateListDistrictRequest(List<DistrictDto> DistrictDtos, bool ReturnNewData = false) : IRequest<List<DistrictDto>>;
        public sealed record UpdateDistrictRequest(DistrictDto DistrictDto, bool ReturnNewData = false) : IRequest<DistrictDto>;
        public sealed record UpdateListDistrictRequest(List<DistrictDto> DistrictDtos, bool ReturnNewData = false) : IRequest<List<DistrictDto>>;
        public sealed record DeleteDistrictRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class DistrictQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService) :
        IRequestHandler<GetDistrictQuery, List<DistrictDto>>,
        IRequestHandler<CreateDistrictRequest, DistrictDto>,
        IRequestHandler<CreateListDistrictRequest, List<DistrictDto>>,
        IRequestHandler<UpdateDistrictRequest, DistrictDto>,
        IRequestHandler<UpdateListDistrictRequest, List<DistrictDto>>,
        IRequestHandler<DeleteDistrictRequest, bool>
    {
        private string CacheKey = "GetDistrictQuery_";

        private async Task<(DistrictDto, List<DistrictDto>)> Result(District? result = null, List<District>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<DistrictDto>(), []);
                else
                    return ((await unitOfWork.Repository<District>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .Include(x => x.City)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<DistrictDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<DistrictDto>>());
                else
                    return (new(), (await unitOfWork.Repository<District>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .Include(x => x.City)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<DistrictDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<DistrictDto>> Handle(GetDistrictQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<District> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<District>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .Include(x => x.City)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<DistrictDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<DistrictDto> Handle(CreateDistrictRequest request, CancellationToken cancellationToken)
        {
            var req = request.DistrictDto.Adapt<CreateUpdateDistrictDto>();
            var result = await unitOfWork.Repository<District>().AddAsync(req.Adapt<District>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DistrictDto>> Handle(CreateListDistrictRequest request, CancellationToken cancellationToken)
        {
            var req = request.DistrictDtos.Adapt<List<CreateUpdateDistrictDto>>();
            var result = await unitOfWork.Repository<District>().AddAsync(req.Adapt<List<District>>());
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

        public async Task<DistrictDto> Handle(UpdateDistrictRequest request, CancellationToken cancellationToken)
        {
            var req = request.DistrictDto.Adapt<CreateUpdateDistrictDto>();
            var result = await unitOfWork.Repository<District>().UpdateAsync(req.Adapt<District>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DistrictDto>> Handle(UpdateListDistrictRequest request, CancellationToken cancellationToken)
        {
            var req = request.DistrictDtos.Adapt<CreateUpdateDistrictDto>();
            var result = await unitOfWork.Repository<District>().UpdateAsync(req.Adapt<List<District>>());
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

        public async Task<bool> Handle(DeleteDistrictRequest request, CancellationToken cancellationToken)
        {
            List<District> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var District = await unitOfWork.Repository<District>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (District != null)
                {
                    deletedCountries.Add(District);
                    await unitOfWork.Repository<District>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<District>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<District>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
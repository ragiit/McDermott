using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.ChronicCategoryCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class ChronicCategoryCommand
    {
        public sealed record GetChronicCategoryQuery(Expression<Func<ChronicCategory, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<ChronicCategoryDto>>;
        public sealed record CreateChronicCategoryRequest(ChronicCategoryDto ChronicCategoryDto, bool ReturnNewData = false) : IRequest<ChronicCategoryDto>;
        public sealed record CreateListChronicCategoryRequest(List<ChronicCategoryDto> ChronicCategoryDtos, bool ReturnNewData = false) : IRequest<List<ChronicCategoryDto>>;
        public sealed record UpdateChronicCategoryRequest(ChronicCategoryDto ChronicCategoryDto, bool ReturnNewData = false) : IRequest<ChronicCategoryDto>;
        public sealed record UpdateListChronicCategoryRequest(List<ChronicCategoryDto> ChronicCategoryDtos, bool ReturnNewData = false) : IRequest<List<ChronicCategoryDto>>;
        public sealed record DeleteChronicCategoryRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class ChronicCategoryQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataChronicCategory) :
        IRequestHandler<GetChronicCategoryQuery, List<ChronicCategoryDto>>,
        IRequestHandler<CreateChronicCategoryRequest, ChronicCategoryDto>,
        IRequestHandler<CreateListChronicCategoryRequest, List<ChronicCategoryDto>>,
        IRequestHandler<UpdateChronicCategoryRequest, ChronicCategoryDto>,
        IRequestHandler<UpdateListChronicCategoryRequest, List<ChronicCategoryDto>>,
        IRequestHandler<DeleteChronicCategoryRequest, bool>
    {
        private string CacheKey = "GetChronicCategoryQuery_";

        private async Task<(ChronicCategoryDto, List<ChronicCategoryDto>)> Result(ChronicCategory? result = null, List<ChronicCategory>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<ChronicCategoryDto>(), []);
                else
                    return ((await unitOfWork.Repository<ChronicCategory>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<ChronicCategoryDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<ChronicCategoryDto>>());
                else
                    return (new(), (await unitOfWork.Repository<ChronicCategory>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<ChronicCategoryDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<ChronicCategoryDto>> Handle(GetChronicCategoryQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<ChronicCategory>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<ChronicCategory>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<ChronicCategoryDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<ChronicCategoryDto> Handle(CreateChronicCategoryRequest request, CancellationToken cancellationToken)
        {
            var req = request.ChronicCategoryDto.Adapt<CreateUpdateChronicCategoryDto>();
            var result = await unitOfWork.Repository<ChronicCategory>().AddAsync(req.Adapt<ChronicCategory>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataChronicCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ChronicCategoryDto>> Handle(CreateListChronicCategoryRequest request, CancellationToken cancellationToken)
        {
            var req = request.ChronicCategoryDtos.Adapt<List<CreateUpdateChronicCategoryDto>>();
            var result = await unitOfWork.Repository<ChronicCategory>().AddAsync(req.Adapt<List<ChronicCategory>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataChronicCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ChronicCategoryDto> Handle(UpdateChronicCategoryRequest request, CancellationToken cancellationToken)
        {
            var req = request.ChronicCategoryDto.Adapt<CreateUpdateChronicCategoryDto>();
            var result = await unitOfWork.Repository<ChronicCategory>().UpdateAsync(req.Adapt<ChronicCategory>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataChronicCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ChronicCategoryDto>> Handle(UpdateListChronicCategoryRequest request, CancellationToken cancellationToken)
        {
            var req = request.ChronicCategoryDtos.Adapt<CreateUpdateChronicCategoryDto>();
            var result = await unitOfWork.Repository<ChronicCategory>().UpdateAsync(req.Adapt<List<ChronicCategory>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataChronicCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteChronicCategoryRequest request, CancellationToken cancellationToken)
        {
            List<ChronicCategory> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var ChronicCategory = await unitOfWork.Repository<ChronicCategory>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (ChronicCategory != null)
                {
                    deletedCountries.Add(ChronicCategory);
                    await unitOfWork.Repository<ChronicCategory>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<ChronicCategory>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<ChronicCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataChronicCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
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

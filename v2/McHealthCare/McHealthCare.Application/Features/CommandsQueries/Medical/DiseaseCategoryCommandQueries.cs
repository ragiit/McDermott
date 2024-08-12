using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.DiseaseCategoryCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class DiseaseCategoryCommand
    {
        public sealed record GetDiseaseCategoryQuery(Expression<Func<DiseaseCategory, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<DiseaseCategoryDto>>;
        public sealed record CreateDiseaseCategoryRequest(DiseaseCategoryDto DiseaseCategoryDto, bool ReturnNewData = false) : IRequest<DiseaseCategoryDto>;
        public sealed record CreateListDiseaseCategoryRequest(List<DiseaseCategoryDto> DiseaseCategoryDtos, bool ReturnNewData = false) : IRequest<List<DiseaseCategoryDto>>;
        public sealed record UpdateDiseaseCategoryRequest(DiseaseCategoryDto DiseaseCategoryDto, bool ReturnNewData = false) : IRequest<DiseaseCategoryDto>;
        public sealed record UpdateListDiseaseCategoryRequest(List<DiseaseCategoryDto> DiseaseCategoryDtos, bool ReturnNewData = false) : IRequest<List<DiseaseCategoryDto>>;
        public sealed record DeleteDiseaseCategoryRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class DiseaseCategoryQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataDiseaseCategory) :
        IRequestHandler<GetDiseaseCategoryQuery, List<DiseaseCategoryDto>>,
        IRequestHandler<CreateDiseaseCategoryRequest, DiseaseCategoryDto>,
        IRequestHandler<CreateListDiseaseCategoryRequest, List<DiseaseCategoryDto>>,
        IRequestHandler<UpdateDiseaseCategoryRequest, DiseaseCategoryDto>,
        IRequestHandler<UpdateListDiseaseCategoryRequest, List<DiseaseCategoryDto>>,
        IRequestHandler<DeleteDiseaseCategoryRequest, bool>
    {
        private string CacheKey = "GetDiseaseCategoryQuery_";

        private async Task<(DiseaseCategoryDto, List<DiseaseCategoryDto>)> Result(DiseaseCategory? result = null, List<DiseaseCategory>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<DiseaseCategoryDto>(), []);
                else
                    return ((await unitOfWork.Repository<DiseaseCategory>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<DiseaseCategoryDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<DiseaseCategoryDto>>());
                else
                    return (new(), (await unitOfWork.Repository<DiseaseCategory>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<DiseaseCategoryDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<DiseaseCategoryDto>> Handle(GetDiseaseCategoryQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<DiseaseCategory>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<DiseaseCategory>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<DiseaseCategoryDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<DiseaseCategoryDto> Handle(CreateDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            var req = request.DiseaseCategoryDto.Adapt<CreateUpdateDiseaseCategoryDto>();
            var result = await unitOfWork.Repository<DiseaseCategory>().AddAsync(req.Adapt<DiseaseCategory>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDiseaseCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DiseaseCategoryDto>> Handle(CreateListDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            var req = request.DiseaseCategoryDtos.Adapt<List<CreateUpdateDiseaseCategoryDto>>();
            var result = await unitOfWork.Repository<DiseaseCategory>().AddAsync(req.Adapt<List<DiseaseCategory>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDiseaseCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DiseaseCategoryDto> Handle(UpdateDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            var req = request.DiseaseCategoryDto.Adapt<CreateUpdateDiseaseCategoryDto>();
            var result = await unitOfWork.Repository<DiseaseCategory>().UpdateAsync(req.Adapt<DiseaseCategory>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataDiseaseCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DiseaseCategoryDto>> Handle(UpdateListDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            var req = request.DiseaseCategoryDtos.Adapt<CreateUpdateDiseaseCategoryDto>();
            var result = await unitOfWork.Repository<DiseaseCategory>().UpdateAsync(req.Adapt<List<DiseaseCategory>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDiseaseCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            List<DiseaseCategory> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var DiseaseCategory = await unitOfWork.Repository<DiseaseCategory>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (DiseaseCategory != null)
                {
                    deletedCountries.Add(DiseaseCategory);
                    await unitOfWork.Repository<DiseaseCategory>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<DiseaseCategory>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<DiseaseCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataDiseaseCategory.Clients.All.ReceiveNotification(new ReceiveDataDto
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

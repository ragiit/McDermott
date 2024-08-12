using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.LabTestCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class LabTestCommand
    {
        public sealed record GetLabTestQuery(Expression<Func<LabTest, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<LabTestDto>>;
        public sealed record CreateLabTestRequest(LabTestDto LabTestDto, bool ReturnNewData = false) : IRequest<LabTestDto>;
        public sealed record CreateListLabTestRequest(List<LabTestDto> LabTestDtos, bool ReturnNewData = false) : IRequest<List<LabTestDto>>;
        public sealed record UpdateLabTestRequest(LabTestDto LabTestDto, bool ReturnNewData = false) : IRequest<LabTestDto>;
        public sealed record UpdateListLabTestRequest(List<LabTestDto> LabTestDtos, bool ReturnNewData = false) : IRequest<List<LabTestDto>>;
        public sealed record DeleteLabTestRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class LabTestQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataLabTest) :
        IRequestHandler<GetLabTestQuery, List<LabTestDto>>,
        IRequestHandler<CreateLabTestRequest, LabTestDto>,
        IRequestHandler<CreateListLabTestRequest, List<LabTestDto>>,
        IRequestHandler<UpdateLabTestRequest, LabTestDto>,
        IRequestHandler<UpdateListLabTestRequest, List<LabTestDto>>,
        IRequestHandler<DeleteLabTestRequest, bool>
    {
        private string CacheKey = "GetLabTestQuery_";

        private async Task<(LabTestDto, List<LabTestDto>)> Result(LabTest? result = null, List<LabTest>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<LabTestDto>(), []);
                else
                    return ((await unitOfWork.Repository<LabTest>().Entities
                        .AsNoTracking()
                        .Include(x=>x.SampleType)
                        .Include(x=>x.LabTestDetails)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<LabTestDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<LabTestDto>>());
                else
                    return (new(), (await unitOfWork.Repository<LabTest>().Entities
                        .AsNoTracking()
                        .Include(x=>x.SampleType)
                        .Include(x=>x.LabTestDetails)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<LabTestDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<LabTestDto>> Handle(GetLabTestQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<LabTest>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<LabTest>().Entities
                        .AsNoTracking()
                        .Include(x=>x.SampleType)
                        .Include(x=>x.LabTestDetails)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<LabTestDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<LabTestDto> Handle(CreateLabTestRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabTestDto.Adapt<CreateUpdateLabTestDto>();
            var result = await unitOfWork.Repository<LabTest>().AddAsync(req.Adapt<LabTest>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabTest.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<LabTestDto>> Handle(CreateListLabTestRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabTestDtos.Adapt<List<CreateUpdateLabTestDto>>();
            var result = await unitOfWork.Repository<LabTest>().AddAsync(req.Adapt<List<LabTest>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabTest.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabTestDto> Handle(UpdateLabTestRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabTestDto.Adapt<CreateUpdateLabTestDto>();
            var result = await unitOfWork.Repository<LabTest>().UpdateAsync(req.Adapt<LabTest>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataLabTest.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<LabTestDto>> Handle(UpdateListLabTestRequest request, CancellationToken cancellationToken)
        {
            var req = request.LabTestDtos.Adapt<CreateUpdateLabTestDto>();
            var result = await unitOfWork.Repository<LabTest>().UpdateAsync(req.Adapt<List<LabTest>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLabTest.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabTestRequest request, CancellationToken cancellationToken)
        {
            List<LabTest> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var LabTest = await unitOfWork.Repository<LabTest>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (LabTest != null)
                {
                    deletedCountries.Add(LabTest);
                    await unitOfWork.Repository<LabTest>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<LabTest>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<LabTest>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataLabTest.Clients.All.ReceiveNotification(new ReceiveDataDto
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

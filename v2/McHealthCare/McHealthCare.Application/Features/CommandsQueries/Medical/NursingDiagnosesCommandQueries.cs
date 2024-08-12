using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.NursingDiagnosesCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class NursingDiagnosesCommand
    {
        public sealed record GetNursingDiagnosesQuery(Expression<Func<NursingDiagnoses, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<NursingDiagnosesDto>>;
        public sealed record CreateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto, bool ReturnNewData = false) : IRequest<NursingDiagnosesDto>;
        public sealed record CreateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos, bool ReturnNewData = false) : IRequest<List<NursingDiagnosesDto>>;
        public sealed record UpdateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto, bool ReturnNewData = false) : IRequest<NursingDiagnosesDto>;
        public sealed record UpdateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos, bool ReturnNewData = false) : IRequest<List<NursingDiagnosesDto>>;
        public sealed record DeleteNursingDiagnosesRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class NursingDiagnosesQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataNursingDiagnoses) :
        IRequestHandler<GetNursingDiagnosesQuery, List<NursingDiagnosesDto>>,
        IRequestHandler<CreateNursingDiagnosesRequest, NursingDiagnosesDto>,
        IRequestHandler<CreateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
        IRequestHandler<UpdateNursingDiagnosesRequest, NursingDiagnosesDto>,
        IRequestHandler<UpdateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
        IRequestHandler<DeleteNursingDiagnosesRequest, bool>
    {
        private string CacheKey = "GetNursingDiagnosesQuery_";

        private async Task<(NursingDiagnosesDto, List<NursingDiagnosesDto>)> Result(NursingDiagnoses? result = null, List<NursingDiagnoses>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<NursingDiagnosesDto>(), []);
                else
                    return ((await unitOfWork.Repository<NursingDiagnoses>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<NursingDiagnosesDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<NursingDiagnosesDto>>());
                else
                    return (new(), (await unitOfWork.Repository<NursingDiagnoses>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<NursingDiagnosesDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<NursingDiagnosesDto>> Handle(GetNursingDiagnosesQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<NursingDiagnoses>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<NursingDiagnoses>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<NursingDiagnosesDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<NursingDiagnosesDto> Handle(CreateNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            var req = request.NursingDiagnosesDto.Adapt<CreateUpdateNursingDiagnosesDto>();
            var result = await unitOfWork.Repository<NursingDiagnoses>().AddAsync(req.Adapt<NursingDiagnoses>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataNursingDiagnoses.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<NursingDiagnosesDto>> Handle(CreateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            var req = request.NursingDiagnosesDtos.Adapt<List<CreateUpdateNursingDiagnosesDto>>();
            var result = await unitOfWork.Repository<NursingDiagnoses>().AddAsync(req.Adapt<List<NursingDiagnoses>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataNursingDiagnoses.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<NursingDiagnosesDto> Handle(UpdateNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            var req = request.NursingDiagnosesDto.Adapt<CreateUpdateNursingDiagnosesDto>();
            var result = await unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(req.Adapt<NursingDiagnoses>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataNursingDiagnoses.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<NursingDiagnosesDto>> Handle(UpdateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            var req = request.NursingDiagnosesDtos.Adapt<CreateUpdateNursingDiagnosesDto>();
            var result = await unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(req.Adapt<List<NursingDiagnoses>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataNursingDiagnoses.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            List<NursingDiagnoses> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var NursingDiagnoses = await unitOfWork.Repository<NursingDiagnoses>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (NursingDiagnoses != null)
                {
                    deletedCountries.Add(NursingDiagnoses);
                    await unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<NursingDiagnoses>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataNursingDiagnoses.Clients.All.ReceiveNotification(new ReceiveDataDto
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

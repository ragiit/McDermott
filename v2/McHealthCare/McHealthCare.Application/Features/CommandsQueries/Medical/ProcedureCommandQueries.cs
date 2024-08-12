using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.ProcedureCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class ProcedureCommand
    {
        public sealed record GetProcedureQuery(Expression<Func<Procedure, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<ProcedureDto>>;
        public sealed record CreateProcedureRequest(ProcedureDto ProcedureDto, bool ReturnNewData = false) : IRequest<ProcedureDto>;
        public sealed record CreateListProcedureRequest(List<ProcedureDto> ProcedureDtos, bool ReturnNewData = false) : IRequest<List<ProcedureDto>>;
        public sealed record UpdateProcedureRequest(ProcedureDto ProcedureDto, bool ReturnNewData = false) : IRequest<ProcedureDto>;
        public sealed record UpdateListProcedureRequest(List<ProcedureDto> ProcedureDtos, bool ReturnNewData = false) : IRequest<List<ProcedureDto>>;
        public sealed record DeleteProcedureRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class ProcedureQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataProcedure) :
        IRequestHandler<GetProcedureQuery, List<ProcedureDto>>,
        IRequestHandler<CreateProcedureRequest, ProcedureDto>,
        IRequestHandler<CreateListProcedureRequest, List<ProcedureDto>>,
        IRequestHandler<UpdateProcedureRequest, ProcedureDto>,
        IRequestHandler<UpdateListProcedureRequest, List<ProcedureDto>>,
        IRequestHandler<DeleteProcedureRequest, bool>
    {
        private string CacheKey = "GetProcedureQuery_";

        private async Task<(ProcedureDto, List<ProcedureDto>)> Result(Procedure? result = null, List<Procedure>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<ProcedureDto>(), []);
                else
                    return ((await unitOfWork.Repository<Procedure>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<ProcedureDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<ProcedureDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Procedure>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<ProcedureDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<ProcedureDto>> Handle(GetProcedureQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Procedure>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Procedure>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<ProcedureDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<ProcedureDto> Handle(CreateProcedureRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProcedureDto.Adapt<CreateUpdateProcedureDto>();
            var result = await unitOfWork.Repository<Procedure>().AddAsync(req.Adapt<Procedure>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataProcedure.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ProcedureDto>> Handle(CreateListProcedureRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProcedureDtos.Adapt<List<CreateUpdateProcedureDto>>();
            var result = await unitOfWork.Repository<Procedure>().AddAsync(req.Adapt<List<Procedure>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataProcedure.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProcedureDto> Handle(UpdateProcedureRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProcedureDto.Adapt<CreateUpdateProcedureDto>();
            var result = await unitOfWork.Repository<Procedure>().UpdateAsync(req.Adapt<Procedure>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataProcedure.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ProcedureDto>> Handle(UpdateListProcedureRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProcedureDtos.Adapt<CreateUpdateProcedureDto>();
            var result = await unitOfWork.Repository<Procedure>().UpdateAsync(req.Adapt<List<Procedure>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataProcedure.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProcedureRequest request, CancellationToken cancellationToken)
        {
            List<Procedure> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Procedure = await unitOfWork.Repository<Procedure>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Procedure != null)
                {
                    deletedCountries.Add(Procedure);
                    await unitOfWork.Repository<Procedure>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Procedure>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Procedure>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataProcedure.Clients.All.ReceiveNotification(new ReceiveDataDto
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

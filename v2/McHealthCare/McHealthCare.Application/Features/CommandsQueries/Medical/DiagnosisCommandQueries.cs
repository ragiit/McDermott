using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.DiagnosisCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class DiagnosisCommand
    {
        public sealed record GetDiagnosisQuery(Expression<Func<Diagnosis, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<DiagnosisDto>>;
        public sealed record CreateDiagnosisRequest(DiagnosisDto DiagnosisDto, bool ReturnNewData = false) : IRequest<DiagnosisDto>;
        public sealed record CreateListDiagnosisRequest(List<DiagnosisDto> DiagnosisDtos, bool ReturnNewData = false) : IRequest<List<DiagnosisDto>>;
        public sealed record UpdateDiagnosisRequest(DiagnosisDto DiagnosisDto, bool ReturnNewData = false) : IRequest<DiagnosisDto>;
        public sealed record UpdateListDiagnosisRequest(List<DiagnosisDto> DiagnosisDtos, bool ReturnNewData = false) : IRequest<List<DiagnosisDto>>;
        public sealed record DeleteDiagnosisRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class DiagnosisQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataDiagnosis) :
        IRequestHandler<GetDiagnosisQuery, List<DiagnosisDto>>,
        IRequestHandler<CreateDiagnosisRequest, DiagnosisDto>,
        IRequestHandler<CreateListDiagnosisRequest, List<DiagnosisDto>>,
        IRequestHandler<UpdateDiagnosisRequest, DiagnosisDto>,
        IRequestHandler<UpdateListDiagnosisRequest, List<DiagnosisDto>>,
        IRequestHandler<DeleteDiagnosisRequest, bool>
    {
        private string CacheKey = "GetDiagnosisQuery_";

        private async Task<(DiagnosisDto, List<DiagnosisDto>)> Result(Diagnosis? result = null, List<Diagnosis>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<DiagnosisDto>(), []);
                else
                    return ((await unitOfWork.Repository<Diagnosis>().Entities
                        .AsNoTracking()
                        .Include(x=>x.ChronicCategory)
                        .Include(x=>x.DiseaseCategory)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<DiagnosisDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<DiagnosisDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Diagnosis>().Entities
                        .AsNoTracking()
                        .Include(x => x.ChronicCategory)
                        .Include(x => x.DiseaseCategory)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<DiagnosisDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<DiagnosisDto>> Handle(GetDiagnosisQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Diagnosis>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Diagnosis>().Entities
                        .AsNoTracking()
                        .Include(x => x.ChronicCategory)
                        .Include(x => x.DiseaseCategory)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<DiagnosisDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<DiagnosisDto> Handle(CreateDiagnosisRequest request, CancellationToken cancellationToken)
        {
            var req = request.DiagnosisDto.Adapt<CreateUpdateDiagnosisDto>();
            var result = await unitOfWork.Repository<Diagnosis>().AddAsync(req.Adapt<Diagnosis>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDiagnosis.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DiagnosisDto>> Handle(CreateListDiagnosisRequest request, CancellationToken cancellationToken)
        {
            var req = request.DiagnosisDtos.Adapt<List<CreateUpdateDiagnosisDto>>();
            var result = await unitOfWork.Repository<Diagnosis>().AddAsync(req.Adapt<List<Diagnosis>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDiagnosis.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DiagnosisDto> Handle(UpdateDiagnosisRequest request, CancellationToken cancellationToken)
        {
            var req = request.DiagnosisDto.Adapt<CreateUpdateDiagnosisDto>();
            var result = await unitOfWork.Repository<Diagnosis>().UpdateAsync(req.Adapt<Diagnosis>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataDiagnosis.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DiagnosisDto>> Handle(UpdateListDiagnosisRequest request, CancellationToken cancellationToken)
        {
            var req = request.DiagnosisDtos.Adapt<CreateUpdateDiagnosisDto>();
            var result = await unitOfWork.Repository<Diagnosis>().UpdateAsync(req.Adapt<List<Diagnosis>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDiagnosis.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDiagnosisRequest request, CancellationToken cancellationToken)
        {
            List<Diagnosis> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Diagnosis = await unitOfWork.Repository<Diagnosis>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Diagnosis != null)
                {
                    deletedCountries.Add(Diagnosis);
                    await unitOfWork.Repository<Diagnosis>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Diagnosis>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Diagnosis>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataDiagnosis.Clients.All.ReceiveNotification(new ReceiveDataDto
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

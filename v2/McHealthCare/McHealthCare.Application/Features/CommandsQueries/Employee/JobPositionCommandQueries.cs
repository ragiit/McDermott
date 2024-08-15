using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Application.Dtos.Employees;
using McHealthCare.Application.Extentions;
using McHealthCare.Application.Interfaces;
using McHealthCare.Domain.Entities.Employees;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.JobPositionCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class JobPositionCommand
    {
        public sealed record GetJobPositionQuery(Expression<Func<JobPosition, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<JobPositionDto>>;
        public sealed record CreateJobPositionRequest(JobPositionDto JobPositionDto, bool ReturnNewData = false) : IRequest<JobPositionDto>;
        public sealed record CreateListJobPositionRequest(List<JobPositionDto> JobPositionDtos, bool ReturnNewData = false) : IRequest<List<JobPositionDto>>;
        public sealed record UpdateJobPositionRequest(JobPositionDto JobPositionDto, bool ReturnNewData = false) : IRequest<JobPositionDto>;
        public sealed record UpdateListJobPositionRequest(List<JobPositionDto> JobPositionDtos, bool ReturnNewData = false) : IRequest<List<JobPositionDto>>;
        public sealed record DeleteJobPositionRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class JobPositionQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetJobPositionQuery, List<JobPositionDto>>,
        IRequestHandler<CreateJobPositionRequest, JobPositionDto>,
        IRequestHandler<CreateListJobPositionRequest, List<JobPositionDto>>,
        IRequestHandler<UpdateJobPositionRequest, JobPositionDto>,
        IRequestHandler<UpdateListJobPositionRequest, List<JobPositionDto>>,
        IRequestHandler<DeleteJobPositionRequest, bool>
    {
        private string CacheKey = "GetJobPositionQuery_";

        private async Task<(JobPositionDto, List<JobPositionDto>)> Result(JobPosition? result = null, List<JobPosition>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<JobPositionDto>(), []);
                else
                    return ((await unitOfWork.Repository<JobPosition>().Entities
                        .AsNoTracking()
                        .Include(x => x.Department)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<JobPositionDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<JobPositionDto>>());
                else
                    return (new(), (await unitOfWork.Repository<JobPosition>().Entities
                        .AsNoTracking()
                        .Include(x => x.Department)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<JobPositionDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<JobPositionDto>> Handle(GetJobPositionQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<JobPosition> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                result = await unitOfWork.Repository<JobPosition>().Entities
                        .AsNoTracking()
                        .Include(x => x.Department)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<JobPositionDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<JobPositionDto> Handle(CreateJobPositionRequest request, CancellationToken cancellationToken)
        {
            var req = request.JobPositionDto.Adapt<CreateUpdateJobPositionDto>();
            var result = await unitOfWork.Repository<JobPosition>().AddAsync(req.Adapt<JobPosition>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<JobPositionDto>> Handle(CreateListJobPositionRequest request, CancellationToken cancellationToken)
        {
            var req = request.JobPositionDtos.Adapt<List<CreateUpdateJobPositionDto>>();
            var result = await unitOfWork.Repository<JobPosition>().AddAsync(req.Adapt<List<JobPosition>>());
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

        public async Task<JobPositionDto> Handle(UpdateJobPositionRequest request, CancellationToken cancellationToken)
        {
            var req = request.JobPositionDto.Adapt<CreateUpdateJobPositionDto>();
            var result = await unitOfWork.Repository<JobPosition>().UpdateAsync(req.Adapt<JobPosition>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<JobPositionDto>> Handle(UpdateListJobPositionRequest request, CancellationToken cancellationToken)
        {
            var req = request.JobPositionDtos.Adapt<CreateUpdateJobPositionDto>();
            var result = await unitOfWork.Repository<JobPosition>().UpdateAsync(req.Adapt<List<JobPosition>>());
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

        public async Task<bool> Handle(DeleteJobPositionRequest request, CancellationToken cancellationToken)
        {
            List<JobPosition> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var JobPosition = await unitOfWork.Repository<JobPosition>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (JobPosition != null)
                {
                    deletedCountries.Add(JobPosition);
                    await unitOfWork.Repository<JobPosition>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<JobPosition>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<JobPosition>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
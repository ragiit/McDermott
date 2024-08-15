using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.ProjectCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public class ProjectCommandQuery
    {
    }
    public sealed class ProjectCommand
    {
        public sealed record GetProjectQuery(Expression<Func<Project, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<ProjectDto>>;
        public sealed record CreateProjectRequest(ProjectDto ProjectDto, bool ReturnNewData = false) : IRequest<ProjectDto>;
        public sealed record CreateListProjectRequest(List<ProjectDto> ProjectDtos, bool ReturnNewData = false) : IRequest<List<ProjectDto>>;
        public sealed record UpdateProjectRequest(ProjectDto ProjectDto, bool ReturnNewData = false) : IRequest<ProjectDto>;
        public sealed record UpdateListProjectRequest(List<ProjectDto> ProjectDtos, bool ReturnNewData = false) : IRequest<List<ProjectDto>>;
        public sealed record DeleteProjectRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class ProjectQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataProject) :
        IRequestHandler<GetProjectQuery, List<ProjectDto>>,
        IRequestHandler<CreateProjectRequest, ProjectDto>,
        IRequestHandler<CreateListProjectRequest, List<ProjectDto>>,
        IRequestHandler<UpdateProjectRequest, ProjectDto>,
        IRequestHandler<UpdateListProjectRequest, List<ProjectDto>>,
        IRequestHandler<DeleteProjectRequest, bool>
    {
        private string CacheKey = "GetProjectQuery_";

        private async Task<(ProjectDto, List<ProjectDto>)> Result(Project? result = null, List<Project>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<ProjectDto>(), []);
                else
                    return ((await unitOfWork.Repository<Project>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<ProjectDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<ProjectDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Project>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<ProjectDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Project>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Project>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<ProjectDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<ProjectDto> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProjectDto.Adapt<CreateUpdateProjectDto>();
            var result = await unitOfWork.Repository<Project>().AddAsync(req.Adapt<Project>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataProject.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ProjectDto>> Handle(CreateListProjectRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProjectDtos.Adapt<List<CreateUpdateProjectDto>>();
            var result = await unitOfWork.Repository<Project>().AddAsync(req.Adapt<List<Project>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataProject.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProjectDto> Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProjectDto.Adapt<CreateUpdateProjectDto>();
            var result = await unitOfWork.Repository<Project>().UpdateAsync(req.Adapt<Project>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataProject.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ProjectDto>> Handle(UpdateListProjectRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProjectDtos.Adapt<CreateUpdateProjectDto>();
            var result = await unitOfWork.Repository<Project>().UpdateAsync(req.Adapt<List<Project>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataProject.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
        {
            List<Project> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Project = await unitOfWork.Repository<Project>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Project != null)
                {
                    deletedCountries.Add(Project);
                    await unitOfWork.Repository<Project>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Project>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Project>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataProject.Clients.All.ReceiveNotification(new ReceiveDataDto
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

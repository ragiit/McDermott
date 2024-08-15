using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Application.Dtos.Employee;
using McHealthCare.Application.Extentions;
using McHealthCare.Application.Interfaces;
using McHealthCare.Domain.Entities.Employee;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using static McHealthCare.Application.Features.CommandsQueries.Employee.DepartmentCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Employee
{
    public sealed class DepartmentCommand
    {
        public sealed record GetDepartmentQuery(Expression<Func<Department, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<DepartmentDto>>;
        public sealed record CreateDepartmentRequest(DepartmentDto DepartmentDto, bool ReturnNewData = false) : IRequest<DepartmentDto>;
        public sealed record CreateListDepartmentRequest(List<DepartmentDto> DepartmentDtos, bool ReturnNewData = false) : IRequest<List<DepartmentDto>>;
        public sealed record UpdateDepartmentRequest(DepartmentDto DepartmentDto, bool ReturnNewData = false) : IRequest<DepartmentDto>;
        public sealed record UpdateListDepartmentRequest(List<DepartmentDto> DepartmentDtos, bool ReturnNewData = false) : IRequest<List<DepartmentDto>>;
        public sealed record DeleteDepartmentRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class DepartmentQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetDepartmentQuery, List<DepartmentDto>>,
        IRequestHandler<CreateDepartmentRequest, DepartmentDto>,
        IRequestHandler<CreateListDepartmentRequest, List<DepartmentDto>>,
        IRequestHandler<UpdateDepartmentRequest, DepartmentDto>,
        IRequestHandler<UpdateListDepartmentRequest, List<DepartmentDto>>,
        IRequestHandler<DeleteDepartmentRequest, bool>
    {
        private string CacheKey = "GetDepartmentQuery_";

        private async Task<(DepartmentDto, List<DepartmentDto>)> Result(Department? result = null, List<Department>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<DepartmentDto>(), []);
                else
                    return ((await unitOfWork.Repository<Department>().Entities
                        .AsNoTracking()
                        .Include(x => x.Manager)
                        .Include(x => x.ParentDepartment)
                        .Include(x => x.Company)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<DepartmentDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<DepartmentDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Department>().Entities
                        .AsNoTracking()
                        .Include(x => x.Manager)
                        .Include(x => x.ParentDepartment)
                        .Include(x => x.Company)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<DepartmentDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<DepartmentDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Department> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                result = await unitOfWork.Repository<Department>().Entities
                        .AsNoTracking()
                        .Include(x => x.Manager)
                        .Include(x => x.ParentDepartment)
                        .Include(x => x.Company)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<DepartmentDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<DepartmentDto> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var req = request.DepartmentDto.Adapt<CreateUpdateDepartmentDto>();
            var result = await unitOfWork.Repository<Department>().AddAsync(req.Adapt<Department>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DepartmentDto>> Handle(CreateListDepartmentRequest request, CancellationToken cancellationToken)
        {
            var req = request.DepartmentDtos.Adapt<List<CreateUpdateDepartmentDto>>();
            var result = await unitOfWork.Repository<Department>().AddAsync(req.Adapt<List<Department>>());
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

        public async Task<DepartmentDto> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var req = request.DepartmentDto.Adapt<CreateUpdateDepartmentDto>();
            var result = await unitOfWork.Repository<Department>().UpdateAsync(req.Adapt<Department>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DepartmentDto>> Handle(UpdateListDepartmentRequest request, CancellationToken cancellationToken)
        {
            var req = request.DepartmentDtos.Adapt<CreateUpdateDepartmentDto>();
            var result = await unitOfWork.Repository<Department>().UpdateAsync(req.Adapt<List<Department>>());
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

        public async Task<bool> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
        {
            List<Department> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Department = await unitOfWork.Repository<Department>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Department != null)
                {
                    deletedCountries.Add(Department);
                    await unitOfWork.Repository<Department>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Department>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Department>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
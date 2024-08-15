using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Application.Extentions;
using McHealthCare.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.CompanyCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class CompanyCommand
    {
        public sealed record GetCompanyQuery(Expression<Func<Company, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<CompanyDto>>;
        public sealed record CreateCompanyRequest(CompanyDto CompanyDto, bool ReturnNewData = false) : IRequest<CompanyDto>;
        public sealed record CreateListCompanyRequest(List<CompanyDto> CompanyDtos, bool ReturnNewData = false) : IRequest<List<CompanyDto>>;
        public sealed record UpdateCompanyRequest(CompanyDto CompanyDto, bool ReturnNewData = false) : IRequest<CompanyDto>;
        public sealed record UpdateListCompanyRequest(List<CompanyDto> CompanyDtos, bool ReturnNewData = false) : IRequest<List<CompanyDto>>;
        public sealed record DeleteCompanyRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class CompanyQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetCompanyQuery, List<CompanyDto>>,
        IRequestHandler<CreateCompanyRequest, CompanyDto>,
        IRequestHandler<CreateListCompanyRequest, List<CompanyDto>>,
        IRequestHandler<UpdateCompanyRequest, CompanyDto>,
        IRequestHandler<UpdateListCompanyRequest, List<CompanyDto>>,
        IRequestHandler<DeleteCompanyRequest, bool>
    {
        private string CacheKey = "GetCompanyQuery_";

        private async Task<(CompanyDto, List<CompanyDto>)> Result(Company? result = null, List<Company>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<CompanyDto>(), []);
                else
                    return ((await unitOfWork.Repository<Company>().Entities
                        .Include(x => x.City)
                        .Include(x => x.Country)    
                        .Include(x => x.Province)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<CompanyDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<CompanyDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Company>().Entities
                        .Include(x => x.City)
                        .Include(x => x.Country)
                        .Include(x => x.Province)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<CompanyDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<CompanyDto>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Company> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                result = await unitOfWork.Repository<Company>().Entities
                        .Include(x => x.City)
                        .Include(x => x.Country)
                        .Include(x => x.Province)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<CompanyDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<CompanyDto> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
        {
            var req = request.CompanyDto.Adapt<CreateUpdateCompanyDto>();
            var result = await unitOfWork.Repository<Company>().AddAsync(req.Adapt<Company>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<CompanyDto>> Handle(CreateListCompanyRequest request, CancellationToken cancellationToken)
        {
            var req = request.CompanyDtos.Adapt<List<CreateUpdateCompanyDto>>();
            var result = await unitOfWork.Repository<Company>().AddAsync(req.Adapt<List<Company>>());
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

        public async Task<CompanyDto> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
        {
            var req = request.CompanyDto.Adapt<CreateUpdateCompanyDto>();
            var result = await unitOfWork.Repository<Company>().UpdateAsync(req.Adapt<Company>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<CompanyDto>> Handle(UpdateListCompanyRequest request, CancellationToken cancellationToken)
        {
            var req = request.CompanyDtos.Adapt<CreateUpdateCompanyDto>();
            var result = await unitOfWork.Repository<Company>().UpdateAsync(req.Adapt<List<Company>>());
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

        public async Task<bool> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken)
        {
            List<Company> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Company = await unitOfWork.Repository<Company>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Company != null)
                {
                    deletedCountries.Add(Company);
                    await unitOfWork.Repository<Company>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Company>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Company>().DeleteAsync(x => request.Ids.Contains(x.Id));
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
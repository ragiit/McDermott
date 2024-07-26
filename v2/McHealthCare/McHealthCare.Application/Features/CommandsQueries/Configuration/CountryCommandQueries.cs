using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Application.Interfaces;
using McHealthCare.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using System.Threading;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.CountryCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class CountryCommand
    {
        public sealed record GetCountryQuery(Expression<Func<Country, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<CountryDto>>;
        public sealed record CreateCountryRequest(CountryDto CountryDto, bool ReturnNewData = false) : IRequest<CountryDto>;
        public sealed record CreateListCountryRequest(List<CountryDto> CountryDtos, bool ReturnNewData = false) : IRequest<List<CountryDto>>;
        public sealed record UpdateCountryRequest(CountryDto CountryDto, bool ReturnNewData = false) : IRequest<CountryDto>;
        public sealed record UpdateListCountryRequest(List<CountryDto> CountryDtos, bool ReturnNewData = false) : IRequest<List<CountryDto>>;
        public sealed record DeleteCountryRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class CountryQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache) :
        IRequestHandler<GetCountryQuery, List<CountryDto>>,
        IRequestHandler<CreateCountryRequest, CountryDto>,
        IRequestHandler<CreateListCountryRequest, List<CountryDto>>,
        IRequestHandler<UpdateCountryRequest, CountryDto>,
        IRequestHandler<UpdateListCountryRequest, List<CountryDto>>,
        IRequestHandler<DeleteCountryRequest, bool>
    {

        private async Task<(CountryDto, List<CountryDto>)> Result(Country? result = null, List<Country>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<CountryDto>(), []);
                else
                    return ((await unitOfWork.Repository<Country>().Entities.FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<CountryDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<CountryDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Country>().Entities.FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<CountryDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<CountryDto>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetCountryQuery_";
            if (request.RemoveCache)
                cache.Remove(cacheKey);

            if (!cache.TryGetValue(cacheKey, out List<Country>? result))
            {
                result = await unitOfWork.Repository<Country>().Entities.ToListAsync(cancellationToken);
                cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<CountryDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<CountryDto> Handle(CreateCountryRequest request, CancellationToken cancellationToken)
        {
            var req = request.CountryDto.Adapt<CreateUpdateCountryDto>();
            var result = await unitOfWork.Repository<Country>().AddAsync(req.Adapt<Country>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove("GetCountryQuery_");

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<CountryDto>> Handle(CreateListCountryRequest request, CancellationToken cancellationToken)
        {
            var req = request.CountryDtos.Adapt<List<CreateUpdateCountryDto>>();
            var result = await unitOfWork.Repository<Country>().AddAsync(req.Adapt<List<Country>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove("GetCountryQuery_");

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CountryDto> Handle(UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            var req = request.CountryDto.Adapt<CreateUpdateCountryDto>();
            var result = await unitOfWork.Repository<Country>().UpdateAsync(req.Adapt<Country>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove("GetCountryQuery_");

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;

        }

        public async Task<List<CountryDto>> Handle(UpdateListCountryRequest request, CancellationToken cancellationToken)
        {
            var req = request.CountryDtos.Adapt<CreateUpdateCountryDto>();
            var result = await unitOfWork.Repository<Country>().UpdateAsync(req.Adapt<List<Country>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove("GetCountryQuery_");

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCountryRequest request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                await unitOfWork.Repository<Country>().DeleteAsync(request.Id.GetValueOrDefault());
            }

            if (request.Ids?.Count > 0)
            {
                await unitOfWork.Repository<Country>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove("GetCountryQuery_");
            return true;
        }

        #endregion DELETE
    }
}
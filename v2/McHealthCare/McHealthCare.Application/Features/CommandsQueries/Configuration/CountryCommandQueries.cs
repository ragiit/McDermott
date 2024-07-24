using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Application.Interfaces;
using McHealthCare.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.CountryCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class CountryCommand
    {
        public sealed record GetCountryQuery(Expression<Func<Country, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<CountryDto>>;
        public sealed record CreateCountryRequest(CountryDto CountryDto) : IRequest<CountryDto>;
        public sealed record CreateListCountryRequest(List<CountryDto> CountryDtos) : IRequest<List<CountryDto>>;
        public sealed record UpdateCountryRequest(CountryDto CountryDto) : IRequest<CountryDto>;
        public sealed record UpdateListCountryRequest(List<CountryDto> CountryDtos) : IRequest<List<CountryDto>>;
        public sealed record DeleteCountryRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>
        {
            public Guid? Id { get; init; } = Id;
            public List<Guid>? Ids { get; init; } = Ids ?? [];
        }
    }

    public sealed class CountryQueryHandler :
        IRequestHandler<GetCountryQuery, List<CountryDto>>,
        IRequestHandler<CreateCountryRequest, CountryDto>,
        IRequestHandler<CreateListCountryRequest, List<CountryDto>>,
        IRequestHandler<UpdateCountryRequest, CountryDto>,
        IRequestHandler<UpdateListCountryRequest, List<CountryDto>>,
        IRequestHandler<DeleteCountryRequest, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public CountryQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        #region GET

        public async Task<List<CountryDto>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetCountryQuery_";
            if (request.RemoveCache)
                _cache.Remove(cacheKey);

            if (!_cache.TryGetValue(cacheKey, out List<Country>? result))
            {
                result = await _unitOfWork.Repository<Country>().GetAsync(null, cancellationToken: cancellationToken);
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<CountryDto>>() ?? new List<CountryDto>();
        }

        #endregion GET

        #region CREATE

        public async Task<CountryDto> Handle(CreateCountryRequest request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDto.Adapt<Country>());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove("GetCountryQuery_");
            return result.Adapt<CountryDto>();
        }

        public async Task<List<CountryDto>> Handle(CreateListCountryRequest request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository<Country>().AddAsync(request.CountryDtos.Adapt<List<Country>>());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove("GetCountryQuery_");
            return result.Adapt<List<CountryDto>>();
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CountryDto> Handle(UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDto.Adapt<Country>());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove("GetCountryQuery_");
            return result.Adapt<CountryDto>();
        }

        public async Task<List<CountryDto>> Handle(UpdateListCountryRequest request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository<Country>().UpdateAsync(request.CountryDtos.Adapt<List<Country>>());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove("GetCountryQuery_");
            return result.Adapt<List<CountryDto>>();
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCountryRequest request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                await _unitOfWork.Repository<Country>().DeleteAsync(request.Id.GetValueOrDefault());
            }

            if (request.Ids?.Count > 0)
            {
                await _unitOfWork.Repository<Country>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove("GetCountryQuery_");
            return true;
        }

        #endregion DELETE
    }
}
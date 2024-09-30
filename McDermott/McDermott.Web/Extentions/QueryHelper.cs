using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using MediatR;
using System.Linq.Expressions;

namespace McDermott.Web.Extentions
{
    public static class QueryHelper
    {
        public static async Task<(List<TDto>, int pageCount)> QueryGetHelper<T, TDto>(
             this IMediator mediator,
             Expression<Func<T, bool>>? predicate = null,
             int pageIndex = 0,
             int pageSize = 10,
             string? searchTerm = "")
        {
            if (typeof(TDto) == typeof(CountryDto))
            {
                var result = await mediator.Send(new GetCountryQuery(
                    predicate as Expression<Func<Country, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: x => new Country
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(ProvinceDto))
            {
                var result = await mediator.Send(new GetProvinceQuery(
                    predicate as Expression<Func<Province, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes:
                    [
                        x => x.Country
                    ],
                    select: x => new Province
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CountryId = x.CountryId,
                        Country = new Domain.Entities.Country
                        {
                            Name = x.Country.Name
                        },
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }

            throw new NotSupportedException($"Query for type {typeof(T)} is not supported.");
        }

        public static async Task<(List<CountryDto>, int pageCount)> QueryGetCountries(this IMediator mediator, Expression<Func<Province, bool>>? predicate = null, int pageIndex = 0, int pageSize = 10, string? searchTerm = "")
        {
            var result = await mediator.Send(new GetCountryQuery(
               pageIndex: pageIndex,
               pageSize: pageSize,
               searchTerm: searchTerm ?? "",
               select: x => new Country
               {
                   Id = x.Id,
                   Name = x.Name,
               }
           ));
            return (result.Item1, result.pageCount);
        }

        public static async Task<(List<ProvinceDto>, int pageCount)> QueryGetProvinces(this IMediator mediator, Expression<Func<Province, bool>>? predicate = null, int pageIndex = 0, int pageSize = 10, string? searchTerm = "")
        {
            var result = await mediator.Send(new GetProvinceQuery(
                predicate,
                pageIndex: pageIndex,
                pageSize: pageSize,
                searchTerm: searchTerm ?? "",
                includes:
                [
                    x => x.Country
                ],
                select: x => new Province
                {
                    Id = x.Id,
                    Name = x.Name,
                    CountryId = x.CountryId,
                    Country = new Domain.Entities.Country
                    {
                        Name = x.Country.Name
                    },
                }
            ));

            return (result.Item1, result.pageCount);
        }
    }
}
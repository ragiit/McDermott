using DocumentFormat.OpenXml.Office2010.Excel;
using MediatR;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Config.EmailEmailTemplateCommand;
using static McDermott.Application.Features.Commands.Config.EmailSettingCommand;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Web.Extentions
{
    public static class QueryComboBoxHelper
    {
        const short PAGE_SIZE = 20;

        /// <summary>
        /// Queries data using the provided mediator and search term.
        /// Supports different DTO types with specific queries.
        /// </summary>
        /// <typeparam name="T">The entity type to query.</typeparam>
        /// <typeparam name="TDto">The DTO type to return.</typeparam>
        /// <param name="mediator">The mediator instance to send queries.</param>
        /// <param name="searchTerm">The optional search term for filtering.</param>
        /// <returns>A list of DTOs matching the search criteria.</returns>
        /// <exception cref="NotSupportedException">Thrown if the type is not supported.</exception>
        public static async Task<List<TDto>> QueryGetComboBox<T, TDto>(this IMediator mediator, string? searchTerm = "", Expression<Func<T, bool>>? predicate = null)
        {
            searchTerm ??= string.Empty;

            #region Configurations

            if (typeof(TDto) == typeof(UserDto))
            {
                var result = (await mediator.Send(new GetUserQueryNew
                {
                    Predicate = predicate as Expression<Func<User, bool>> ?? (x => true),
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return ((List<TDto>)(object)result);
            }

            if (typeof(TDto) == typeof(GroupDto))
            {
                var result = (await mediator.Send(new GetGroupQuery
                {
                    Predicate = predicate as Expression<Func<Group, bool>> ?? (x => true),
                    Select = x => new Group
                    {
                        Id = x.Id,
                        Name = x.Name
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(MenuDto))
            {
                var result = (await mediator.Send(new GetMenuQuery
                {
                    Predicate = predicate as Expression<Func<Menu, bool>> ?? (x => true),
                    Select = x => new Menu
                    {
                        Id = x.Id,
                        Name = x.Name
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(CompanyDto))
            {
                var result = (await mediator.Send(new GetCompanyQuery
                {
                    Predicate = predicate as Expression<Func<Company, bool>> ?? (x => true),
                    Select = x => new Company
                    {
                        Id = x.Id,
                        Name = x.Name
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(CountryDto))
            {
                var result = (await mediator.Send(new GetCountryQuery
                {
                    Predicate = predicate as Expression<Func<Country, bool>> ?? (x => true),
                    Select = x => new Country
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(ProvinceDto))
            {
                var result = (await mediator.Send(new GetProvinceQuery
                {
                    Predicate = predicate as Expression<Func<Province, bool>> ?? (x => true),
                    Select = x => new Province
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Country = new Country
                        {
                            Name = x.Country == null ? "" : x.Country.Name
                        }
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(CityDto))
            {
                var result = (await mediator.Send(new GetCityQuery
                {
                    Predicate = predicate as Expression<Func<City, bool>> ?? (x => true),
                    Select = x => new City
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Province = new Province
                        {
                            Name = x.Province == null ? "" : x.Province.Name
                        }
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(DistrictDto))
            {
                var result = (await mediator.Send(new GetDistrictQuery
                {
                    Predicate = predicate as Expression<Func<District, bool>> ?? (x => true),
                    Select = x => new District
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Province = new Province
                        {
                            Name = x.Province == null ? "" : x.Province.Name
                        },
                        City = new City
                        {
                            Name = x.City == null ? "" : x.City.Name
                        }
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(VillageDto))
            {
                var result = (await mediator.Send(new GetVillageQueryNew
                {
                    Predicate = predicate as Expression<Func<Village, bool>> ?? (x => true),
                    Select = x => new Village
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Province = new Province
                        {
                            Name = x.Province == null ? "" : x.Province.Name
                        },
                        City = new City
                        {
                            Name = x.City == null ? "" : x.City.Name
                        },
                        District = new District
                        {
                            Name = x.District == null ? "" : x.District.Name
                        }
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

            }

            #endregion

            #region Inventories
            if (typeof(TDto) == typeof(ProductDto))
            {
                var result = (await mediator.Send(new GetProductQueryNew
                {
                    Predicate = predicate as Expression<Func<Product, bool>> ?? (x => true),
                    Select = x => new Product
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UomId = x.UomId,
                        TraceAbility = x.TraceAbility,
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }
            #endregion

            throw new NotSupportedException($"Query for type {typeof(T)} is not supported.");
        }
    }
}

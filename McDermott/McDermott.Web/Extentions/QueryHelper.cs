using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using MediatR;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Web.Extentions
{
    public static class QueryHelper
    {
        public static async Task<(List<TDto>, int pageCount)> QueryGetHelper<T, TDto>(
             this IMediator mediator,
             int pageIndex = 0,
             int pageSize = 10,
             string? searchTerm = "",

             Expression<Func<T, bool>>? predicate = null,
             List<Expression<Func<T, object>>>? includes = null,
             Expression<Func<T, T>>? select = null)
        {
            if (typeof(TDto) == typeof(CountryDto))
            {
                var result = await mediator.Send(new GetCountryQuery(
                    predicate as Expression<Func<Country, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new Country
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    } : select as Expression<Func<Country, Country>>
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
                    includes: includes is null ?
                    [
                        x => x.Country
                    ] : includes as List<Expression<Func<Province, object>>>,
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
            else if (typeof(TDto) == typeof(CityDto))
            {
                var result = await mediator.Send(new GetCityQuery(
                    predicate as Expression<Func<City, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Province
                    ] : includes as List<Expression<Func<City, object>>>,
                    select: x => new City
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProvinceId = x.ProvinceId,
                        Province = new Domain.Entities.Province
                        {
                            Name = x.Province.Name
                        },
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(DistrictDto))
            {
                var result = await mediator.Send(new GetDistrictQuery(
                    predicate as Expression<Func<District, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Province,
                        x => x.City
                    ] : includes as List<Expression<Func<District, object>>>,
                    select: x => new District
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProvinceId = x.ProvinceId,
                        CityId = x.CityId,
                        Province = new Domain.Entities.Province
                        {
                            Name = x.Province.Name
                        },
                        City = new Domain.Entities.City
                        {
                            Name = x.City.Name
                        },
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(VillageDto))
            {
                var result = await mediator.Send(new GetVillageQuery(
                    predicate as Expression<Func<Village, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Province,
                        x => x.City,
                        x => x.District
                    ] : includes as List<Expression<Func<Village, object>>>,
                    select: x => new Village
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PostalCode = x.PostalCode,
                        ProvinceId = x.ProvinceId,
                        CityId = x.CityId,
                        DistrictId = x.DistrictId,
                        Province = new Domain.Entities.Province
                        {
                            Name = x.Province.Name
                        },
                        City = new Domain.Entities.City
                        {
                            Name = x.City.Name
                        },
                        District = new Domain.Entities.District
                        {
                            Name = x.District.Name
                        },
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(OccupationalDto))
            {
                var result = await mediator.Send(new GetOccupationalQuery(
                    predicate as Expression<Func<Occupational, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: x => new Occupational
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(SpecialityDto))
            {
                var result = await mediator.Send(new GetSpecialityQuery(
                    predicate as Expression<Func<Speciality, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: x => new Speciality
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(JobPositionDto))
            {
                var result = await mediator.Send(new GetJobPositionQuery(
                    predicate as Expression<Func<JobPosition, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Department,
                    ] : includes as List<Expression<Func<JobPosition, object>>>,
                    select: x => new JobPosition
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DepartmentId = x.DepartmentId,
                        Department = new Department
                        {
                            Name = x.Department.Name
                        }
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(CompanyDto))
            {
                var result = await mediator.Send(new GetCompanyQuery(
                    predicate as Expression<Func<Company, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Country,
                        x => x.Province,
                        x => x.City,
                    ] : includes as List<Expression<Func<Company, object>>>,
                    select: x => new Company
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Phone = x.Phone,
                        Email = x.Email,
                        Website = x.Website,
                        VAT = x.VAT,
                        Street1 = x.Street1,
                        Street2 = x.Street2,
                        Zip = x.Zip,
                        CurrencyId = x.CurrencyId,
                        Logo = x.Logo,
                        CityId = x.CityId,
                        ProvinceId = x.ProvinceId,
                        CountryId = x.CountryId,
                        Country = new Country
                        {
                            Name = x.Country.Name
                        },
                        Province = new Province
                        {
                            Name = x.Province.Name
                        },
                        City = new City
                        {
                            Name = x.City.Name
                        },
                    }
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(DepartmentDto))
            {
                var result = await mediator.Send(new GetDepartmentQuery(
                    predicate as Expression<Func<Department, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes:
                    [
                        x => x.Manager,
                        x => x.ParentDepartment,
                        x => x.Company,
                    ],
                    select: x => new Department
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentDepartmentId = x.ParentDepartmentId,
                        CompanyId = x.CompanyId,
                        ManagerId = x.ManagerId,
                        ParentDepartment = new Domain.Entities.Department
                        {
                            Name = x.ParentDepartment.Name
                        },
                        Company = new Domain.Entities.Company
                        {
                            Name = x.Company.Name
                        },
                        Manager = new Domain.Entities.User
                        {
                            Name = x.Manager.Name
                        },
                        DepartmentCategory = x.DepartmentCategory
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
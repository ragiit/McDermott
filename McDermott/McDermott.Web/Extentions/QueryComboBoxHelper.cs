﻿using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;
using static McDermott.Application.Features.Commands.Pharmacies.DrugFormCommand;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanServiceAncCommand;
using Project = McDermott.Domain.Entities.Project;

namespace McDermott.Web.Extentions
{
    public static class QueryComboBoxHelper
    {
        public static async Task<LoadResult> LoadCustomData<TData>(
             DataSourceLoadOptionsBase options,
             Func<Task<IQueryable<TData>>> queryProvider,
             string defaultSortColumn = "Name", // Default sort column parameter
             bool isSortColumn = true,
             Expression<Func<TData, bool>>? filter = null,
             CancellationToken cancellationToken = default)
        {
            if (isSortColumn)
                options.DefaultSort = defaultSortColumn; // Use the provided default sort column

            var query = await queryProvider();

            // Apply the filter if provided
            if (filter != null)
                query = query.Where(filter);

            return await DataSourceLoader.LoadAsync(query/*.Skip(options.Skip).Take(options.Take)*/, options, cancellationToken);
        }

        private const short PAGE_SIZE = 20;

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
        public static async Task<List<TDto>> QueryGetComboBox<T, TDto>(
            this IMediator mediator, string? searchTerm = "",
            Expression<Func<T, bool>>? predicate = null,
            List<(Expression<Func<T, object>> OrderBy, bool IsDescending)>? orderBy = null,
            Expression<Func<T, T>>? select = null
            )
        {
            orderBy ??= [];
            searchTerm ??= string.Empty;

            #region Configurations

            if (typeof(TDto) == typeof(UserDto))
            {
                var result = (await mediator.Send(new GetUserQueryNew
                {
                    Predicate = predicate as Expression<Func<User, bool>> ?? (x => true),
                    Select = select is null ? x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        IsEmployee = x.IsEmployee,
                    } : select as Expression<Func<User, User>>,
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return ((List<TDto>)(object)result);
            }

            if (typeof(TDto) == typeof(OccupationalDto))
            {
                var result = (await mediator.Send(new GetOccupationalQuery
                {
                    Predicate = predicate as Expression<Func<Occupational, bool>> ?? (x => true),
                    Select = x => new Occupational
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
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
                    OrderByList = orderBy as List<(Expression<Func<Menu, object>> OrderBy, bool IsDescending)>,
                    Select = x => new Menu
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentId = x.ParentId,
                        Parent = new Menu
                        {
                            Name = x.Parent == null ? "" : x.Parent.Name,
                        },
                        Sequence = x.Sequence,
                        Url = x.Url,
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

                return result.Cast<TDto>().ToList();
            }

            #endregion Configurations

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
                        PurchaseUomId = x.PurchaseUomId,
                        TraceAbility = x.TraceAbility,
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(CronisCategoryDto))
            {
                var result = (await mediator.Send(new GetCronisCategoryQuery
                {
                    Predicate = predicate as Expression<Func<CronisCategory, bool>> ?? (x => true),
                    Select = x => new CronisCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(UomDto))
            {
                var result = (await mediator.Send(new GetUomQuery
                {
                    Predicate = predicate as Expression<Func<Uom, bool>> ?? (x => true),
                    Select = x => new Uom
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        UomCategory = new UomCategory
                        {
                            Name = x.UomCategory == null ? "" : x.UomCategory.Name
                        }
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(LocationDto))
            {
                var result = (await mediator.Send(new GetLocationQuery
                {
                    Predicate = predicate as Expression<Func<Locations, bool>> ?? (x => true),
                    Select = select is null ? x => new Locations
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentLocationId = x.ParentLocationId,
                        ParentLocation = new Locations
                        {
                            Name = x.ParentLocation == null ? "" : x.ParentLocation.Name,
                        }
                    } : select as Expression<Func<Locations, Locations>>,
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            #endregion Inventories

            #region Employees

            if (typeof(TDto) == typeof(JobPositionDto))
            {
                var result = (await mediator.Send(new GetJobPositionQuery
                {
                    Predicate = predicate as Expression<Func<JobPosition, bool>> ?? (x => true),
                    Select = x => new JobPosition
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DepartmentId = x.DepartmentId,
                        Department = new Department
                        {
                            Name = x.Department.Name
                        }
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(DepartmentDto))
            {
                var result = (await mediator.Send(new GetDepartmentQuery
                {
                    Predicate = predicate as Expression<Func<Department, bool>> ?? (x => true),
                    Select = x => new Department
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
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            #endregion Employees

            #region Medicals

            if (typeof(TDto) == typeof(ServiceDto))
            {
                var result = (await mediator.Send(new GetServiceQuery
                {
                    Predicate = predicate as Expression<Func<Service, bool>> ?? (x => true),
                    Select = select is null ? x => new Service
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        IsMaternity = x.IsMaternity,
                    } : select as Expression<Func<Service, Service>>,
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return ((List<TDto>)(object)result);
            }

            if (typeof(TDto) == typeof(ProjectDto))
            {
                var result = (await mediator.Send(new GetProjectQuery
                {
                    Predicate = predicate as Expression<Func<Project, bool>> ?? (x => true),
                    Select = select is null ? x => new Project
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    } : select as Expression<Func<Project, Project>>,
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return ((List<TDto>)(object)result);
            }

            if (typeof(TDto) == typeof(NursingDiagnosesDto))
            {
                var result = (await mediator.Send(new GetNursingDiagnosesQuery
                {
                    Predicate = predicate as Expression<Func<NursingDiagnoses, bool>> ?? (x => true),
                    Select = select is null ? x => new NursingDiagnoses
                    {
                        Id = x.Id,
                        Problem = x.Problem,
                        Code = x.Code,
                    } : select as Expression<Func<NursingDiagnoses, NursingDiagnoses>>,
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return ((List<TDto>)(object)result);
            }

            if (typeof(TDto) == typeof(DiagnosisDto))
            {
                var result = (await mediator.Send(new GetDiagnosisQuery
                {
                    Predicate = predicate as Expression<Func<Diagnosis, bool>> ?? (x => true),
                    Select = select is null ? x => new Diagnosis
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        CronisCategory = new CronisCategory
                        {
                            Name = x.CronisCategory == null ? "-" : x.CronisCategory.Name,
                        }
                    } : select as Expression<Func<Diagnosis, Diagnosis>>,
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return ((List<TDto>)(object)result);
            }

            if (typeof(TDto) == typeof(InsurancePolicyDto))
            {
                var result = (await mediator.Send(new GetInsurancePolicyQuery
                {
                    Predicate = predicate as Expression<Func<InsurancePolicy, bool>> ?? (x => true),
                    Select = select is null ? x => new InsurancePolicy
                    {
                        Id = x.Id,
                        PolicyNumber = x.PolicyNumber,
                        Insurance = new Insurance
                        {
                            Name = x.Insurance == null ? "" : x.Insurance.Name,
                        },
                        NoKartu = x.NoKartu,
                        PstPrb = x.PstPrb,
                        PstProl = x.PstProl
                    } : select as Expression<Func<InsurancePolicy, InsurancePolicy>>,
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return ((List<TDto>)(object)result);
            }

            #endregion Medicals

            #region Transactions

            if (typeof(TDto) == typeof(GeneralConsultanServiceAncDto))
            {
                var result = (await mediator.Send(new GetGeneralConsultanServiceAncQuery
                {
                    Predicate = predicate as Expression<Func<GeneralConsultanServiceAnc, bool>> ?? (x => true),
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            #endregion Transactions

            #region Pharmacy

            if (typeof(TDto) == typeof(DrugFormDto))
            {
                var result = (await mediator.Send(new GetDrugFormQuery
                {
                    Predicate = predicate as Expression<Func<DrugForm, bool>> ?? (x => true),
                    Select = x => new DrugForm
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(DrugDosageDto))
            {
                var result = (await mediator.Send(new GetDrugDosageQuery
                {
                    Predicate = predicate as Expression<Func<DrugDosage, bool>> ?? (x => true),
                    Select = x => new DrugDosage
                    {
                        Id = x.Id,
                        Frequency = x.Frequency,
                        DrugRouteId = x.DrugRouteId,
                        DrugRoute = new DrugRoute
                        {
                            Route = x.DrugRoute == null ? string.Empty : x.DrugRoute.Route
                        }
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            if (typeof(TDto) == typeof(DrugRouteDto))
            {
                var result = (await mediator.Send(new GetDrugRouteQuery
                {
                    Predicate = predicate as Expression<Func<DrugRoute, bool>> ?? (x => true),
                    Select = x => new DrugRoute
                    {
                        Id = x.Id,
                        Route = x.Route,
                        Code = x.Code,
                    },
                    SearchTerm = searchTerm,
                    PageSize = PAGE_SIZE,
                })).Item1;

                return result.Cast<TDto>().ToList();
            }

            #endregion Pharmacy

            throw new NotSupportedException($"Query for type {typeof(T)} is not supported.");
        }
    }
}
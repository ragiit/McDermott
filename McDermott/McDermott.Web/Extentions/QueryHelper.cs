using MailKit.Search;
using MediatR;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;
using static McDermott.Application.Features.Commands.Pharmacies.DrugFormCommand;
using static McDermott.Application.Features.Commands.Pharmacies.SignaCommand;

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
            if (typeof(TDto) == typeof(UomCategoryDto))
            {
                var result = await mediator.Send(new GetUomCategoryQuery(
                    predicate as Expression<Func<UomCategory, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new UomCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                    } : select as Expression<Func<UomCategory, UomCategory>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(UomCategoryDto))
            {
                var result = await mediator.Send(new GetDrugRouteQuery(
                    predicate as Expression<Func<DrugRoute, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new DrugRoute
                    {
                        Id = x.Id,
                        Route = x.Route,
                        Code = x.Code,
                    } : select as Expression<Func<DrugRoute, DrugRoute>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(DrugDosageDto))
            {
                var result = await mediator.Send(new GetDrugDosageQuery(
                    predicate as Expression<Func<DrugDosage, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.DrugRoute
                    ] : includes as List<Expression<Func<DrugDosage, object>>>,
                    select: select is null ? x => new DrugDosage
                    {
                        Id = x.Id,
                        Frequency = x.Frequency,
                        TotalQtyPerDay = x.TotalQtyPerDay,
                        Days = x.Days,
                        DrugRouteId = x.DrugRouteId,
                        DrugRoute = new DrugRoute
                        {
                            Route = x.DrugRoute.Route
                        }
                    } : select as Expression<Func<DrugDosage, DrugDosage>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(ProductCategoryDto))
            {
                var result = await mediator.Send(new GetProductCategoryQuery(
                    predicate as Expression<Func<ProductCategory, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new ProductCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    } : select as Expression<Func<ProductCategory, ProductCategory>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(ActiveComponentDto))
            {
                var result = await mediator.Send(new GetActiveComponentQuery(
                    predicate as Expression<Func<ActiveComponent, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Uom
                    ] : includes as List<Expression<Func<ActiveComponent, object>>>,
                    select: select is null ? x => new ActiveComponent
                    {
                        Id = x.Id,
                        Name = x.Name,
                        AmountOfComponent = x.AmountOfComponent,
                        UomId = x.UomId,
                        Uom = new Uom
                        {
                            Name = x.Uom.Name
                        }
                    } : select as Expression<Func<ActiveComponent, ActiveComponent>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(BuildingDto))
            {
                var result = await mediator.Send(new GetBuildingQuery(
                    predicate as Expression<Func<Building, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.HealthCenter
                    ] : includes as List<Expression<Func<Building, object>>>,
                    select: select is null ? x => new Building
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        HealthCenterId = x.HealthCenterId,
                        HealthCenter = new HealthCenter
                        {
                            Name = x.HealthCenter.Name
                        }
                    } : select as Expression<Func<Building, Building>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(BuildingLocationDto))
            {
                var result = await mediator.Send(new GetBuildingLocationQuery(
                    predicate as Expression<Func<BuildingLocation, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Building,
                        x => x.Location,
                    ] : includes as List<Expression<Func<BuildingLocation, object>>>,
                    select: select is null ? x => new BuildingLocation
                    {
                        Id = x.Id,
                        LocationId = x.LocationId,
                        BuildingId = x.BuildingId,
                        Location = new Locations
                        {
                            Name = x.Location.Name
                        }
                    } : select as Expression<Func<BuildingLocation, BuildingLocation>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(HealthCenterDto))
            {
                var result = await mediator.Send(new GetHealthCenterQuery(
                    predicate as Expression<Func<HealthCenter, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Country,
                        x => x.Province,
                        x => x.City,
                    ] : includes as List<Expression<Func<HealthCenter, object>>>,
                    select: select is null ? x => new HealthCenter
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        Phone = x.Phone,
                        Mobile = x.Mobile,
                        Email = x.Email,
                        WebsiteLink = x.WebsiteLink,
                        Street1 = x.Street1,
                        Street2 = x.Street2,
                        CountryId = x.CountryId,
                        ProvinceId = x.ProvinceId,
                        CityId = x.CityId,
                    } : select as Expression<Func<HealthCenter, HealthCenter>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(UomDto))
            {
                var result = await mediator.Send(new GetUomQuery(
                    predicate as Expression<Func<Uom, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.UomCategory
                    ] : includes as List<Expression<Func<Uom, object>>>,
                    select: select is null ? x => new Uom
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UomCategoryId = x.UomCategoryId,
                        UomCategory = new UomCategory
                        {
                            Name = x.UomCategory.Name
                        },
                        BiggerRatio = x.BiggerRatio,
                        Type = x.Type,
                        Active = x.Active,
                        RoundingPrecision = x.RoundingPrecision
                    } : select as Expression<Func<Uom, Uom>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(PatientFamilyRelationDto))
            {
                var result = await mediator.Send(new GetPatientFamilyRelationQuery(
                    predicate as Expression<Func<PatientFamilyRelation, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Family,
                        x => x.Patient,
                        x => x.FamilyMember,
                    ] : includes as List<Expression<Func<PatientFamilyRelation, object>>>,
                    select: select is null ? x => new PatientFamilyRelation
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        FamilyId = x.FamilyId,
                        FamilyMemberId = x.FamilyMemberId,
                    } : select as Expression<Func<PatientFamilyRelation, PatientFamilyRelation>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(FamilyDto))
            {
                var result = await mediator.Send(new GetFamilyQuery(
                    predicate as Expression<Func<Family, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.InverseRelation
                    ] : includes as List<Expression<Func<Family, object>>>,
                    select: select is null ? x => new Family
                    {
                        Id = x.Id,
                        Name = x.Name,
                        InverseRelationId = x.InverseRelationId,
                        InverseRelation = new Family
                        {
                            Name = x.InverseRelation.Name
                        }
                    } : select as Expression<Func<Family, Family>>
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
                    select: select is null ? x => new Village
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
                    } : select as Expression<Func<Village, Village>>
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
                    select: select is null ? x => new Occupational
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    } : select as Expression<Func<Occupational, Occupational>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(DrugFormDto))
            {
                var result = await mediator.Send(new GetDrugFormQuery(
                    predicate as Expression<Func<DrugForm, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new DrugForm
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code
                    } : select as Expression<Func<DrugForm, DrugForm>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(DrugRouteDto))
            {
                var result = await mediator.Send(new GetDrugRouteQuery(
                    predicate as Expression<Func<DrugRoute, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new DrugRoute
                    {
                        Id = x.Id,
                        Route = x.Route,
                        Code = x.Code
                    } : select as Expression<Func<DrugRoute, DrugRoute>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(SignaDto))
            {
                var result = await mediator.Send(new GetSignaQuery(
                    predicate as Expression<Func<Signa, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new Signa
                    {
                        Id = x.Id,
                        Name = x.Name
                    } : select as Expression<Func<Signa, Signa>>
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
                    select: select is null ? x => new Speciality
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    } : select as Expression<Func<Speciality, Speciality>>
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
                    select: select is null ? x => new JobPosition
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DepartmentId = x.DepartmentId,
                        Department = new Department
                        {
                            Name = x.Department.Name
                        }
                    } : select as Expression<Func<JobPosition, JobPosition>>
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
                    select: select is null ? x => new Department
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
                    } : select as Expression<Func<Department, Department>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }

            throw new NotSupportedException($"Query for type {typeof(T)} is not supported.");
        }
    }
}
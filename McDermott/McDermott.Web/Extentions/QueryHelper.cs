using MailKit.Search;
using MediatR;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;
using static McDermott.Application.Features.Commands.Pharmacy.DrugFormCommand;
using static McDermott.Application.Features.Commands.Pharmacy.SignaCommand;

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
            else if (typeof(TDto) == typeof(UomCategoryDto))
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
            else if (typeof(TDto) == typeof(LabTestDto))
            {
                var result = await mediator.Send(new GetLabTestQuery(
                    predicate as Expression<Func<LabTest, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.SampleType
                    ] : includes as List<Expression<Func<LabTest, object>>>,
                    select: select is null ? x => new LabTest
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        ResultType = x.ResultType,
                        SampleTypeId = x.SampleTypeId,
                        SampleType = new SampleType
                        {
                            Name = x.SampleType.Name
                        }
                    } : select as Expression<Func<LabTest, LabTest>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(LabTestDetailDto))
            {
                var result = await mediator.Send(new GetLabTestDetailQuery(
                    predicate as Expression<Func<LabTestDetail, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.LabTest,
                        x => x.LabUom,
                    ] : includes as List<Expression<Func<LabTestDetail, object>>>,
                    select: select is null ? x => new LabTestDetail
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NormalRangeFemale = x.NormalRangeFemale,
                        NormalRangeMale = x.NormalRangeMale,
                        LabTestId = x.LabTestId,
                        LabTest = new LabTest
                        {
                            Name = x.LabTest.Name
                        },
                        LabUomId = x.LabUomId,
                        LabUom = new LabUom
                        {
                            Name = x.LabUom.Name
                        },
                        ResultValueType = x.ResultValueType,
                        Remark = x.Remark,
                        ResultType = x.ResultType
                    } : select as Expression<Func<LabTestDetail, LabTestDetail>>
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
            else if (typeof(TDto) == typeof(LabUomDto))
            {
                var result = await mediator.Send(new GetLabUomQuery(
                    predicate as Expression<Func<LabUom, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new LabUom
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    } : select as Expression<Func<LabUom, LabUom>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(GroupDto))
            {
                var result = await mediator.Send(new GetGroupQuery(
                    predicate as Expression<Func<Group, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    select: select is null ? x => new Group
                    {
                        Id = x.Id,
                        Name = x.Name
                    } : select as Expression<Func<Group, Group>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(GroupMenuDto))
            {
                var result = await mediator.Send(new GetGroupMenuQuery(
                    predicate as Expression<Func<GroupMenu, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Menu,
                        x => x.Menu.Parent
                    ] : includes as List<Expression<Func<GroupMenu, object>>>,
                    select: select is null ? x => new GroupMenu
                    {
                        Id = x.Id,
                        MenuId = x.MenuId,
                        Menu = new Menu
                        {
                            Name = x.Menu.Name,
                        },

                        IsCreate = x.IsCreate,
                        IsDelete = x.IsDelete,
                        IsDefaultData = x.IsDefaultData,
                        IsImport = x.IsImport,
                        IsRead = x.IsRead,
                        IsUpdate = x.IsUpdate,
                    } : select as Expression<Func<GroupMenu, GroupMenu>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(MenuDto))
            {
                var result = await mediator.Send(new GetMenuQuery(
                    predicate as Expression<Func<Menu, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Parent
                    ] : includes as List<Expression<Func<Menu, object>>>,
                    select: select is null ? x => new Menu
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Sequence = x.Sequence,
                        Url = x.Url,
                        ParentId = x.ParentId,
                        Icon = x.Icon,
                        IsDefaultData = x.IsDefaultData,
                        Parent = new Menu
                        {
                            Name = x.Parent.Name
                        }
                    } : select as Expression<Func<Menu, Menu>>
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
            else if (typeof(TDto) == typeof(LocationDto))
            {
                var result = await mediator.Send(new GetLocationQuery(
                    predicate as Expression<Func<Locations, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.ParentLocation,
                        x => x.Company,
                    ] : includes as List<Expression<Func<Locations, object>>>,
                    select: select is null ? x => new Locations
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = x.Type,
                        ParentLocationId = x.ParentLocationId,
                        CompanyId = x.CompanyId,
                        Company = new Company
                        {
                            Name = x.Company.Name
                        },
                        ParentLocation = new Locations
                        {
                            Name = x.ParentLocation.Name
                        }
                    } : select as Expression<Func<Locations, Locations>>
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
                    select: select is null ? x => new Province
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CountryId = x.CountryId,
                        Country = new Domain.Entities.Country
                        {
                            Name = x.Country.Name
                        },
                    } : select as Expression<Func<Province, Province>>
                ));

                return ((List<TDto>)(object)result.Item1, result.pageCount);
            }
            else if (typeof(TDto) == typeof(ProvinceDto))
            {
                var result = await mediator.Send(new GetMenuQuery(
                    predicate as Expression<Func<Menu, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Parent
                    ] : includes as List<Expression<Func<Menu, object>>>,
                    select: select is null ? x => new Menu
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Sequence = x.Sequence,
                        Url = x.Url,
                        ParentId = x.ParentId,
                        Parent = new Domain.Entities.Menu
                        {
                            Name = x.Parent.Name
                        },
                    } : select as Expression<Func<Menu, Menu>>
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
                    select: select is null ? x => new City
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProvinceId = x.ProvinceId,
                        Province = new Domain.Entities.Province
                        {
                            Name = x.Province.Name
                        },
                    } : select as Expression<Func<City, City>>
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
                    select: select is null ? x => new District
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
                    } : select as Expression<Func<District, District>>
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
            else if (typeof(TDto) == typeof(ServiceDto))
            {
                var result = await mediator.Send(new GetServiceQuery(
                    predicate as Expression<Func<Service, bool>>,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    searchTerm: searchTerm ?? "",
                    includes: includes is null ?
                    [
                        x => x.Serviced,
                    ] : includes as List<Expression<Func<Service, object>>>,
                    select: select is null ? x => new Service
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Quota = x.Quota,
                        IsKiosk = x.IsKiosk,
                        IsMcu = x.IsMcu,
                        IsPatient = x.IsPatient,
                        IsTelemedicine = x.IsTelemedicine,
                        IsVaccination = x.IsVaccination,
                        ServicedId = x.ServicedId,
                        Serviced = new Service
                        {
                            Name = x.Serviced.Name
                        }
                    } : select as Expression<Func<Service, Service>>
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
                    select: select is null ? x => new Company
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
                    } : select as Expression<Func<Company, Company>>
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
list1 = (await Mediator.Send(new GetCountryQuery(x => countryNames.Contains(x.Name.ToLower()), 0, 0,
    select: x => new Country
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list1 = (await Mediator.Send(new GetProvinceQuery(x => provinceNames.Contains(x.Name.ToLower()), 0, 0,
    select: x => new Province
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list2 = (await Mediator.Send(new GetCityQuery(x => cityNames.Contains(x.Name.ToLower()), 0, 0,
    select: x => new City
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list3 = (await Mediator.Send(new GetDistrictQuery(x => districtNames.Contains(x.Name), 0, 0,
    select: x => new District
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list1 = (await Mediator.Send(new GetMenuQuery(x => parentNames.Contains(x.Name.ToLower()), 0, 0,
    select: x => new Menu
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

groups = (await Mediator.Send(new GetGroupQuery(x => groupNames.Contains(x.Name.ToLower()), 0, 0,
    select: x => new Group
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list1 = (await Mediator.Send(new GetDiseaseCategoryQuery(x => parentNames.Contains(x.Name.ToLower()), 0, 0,
    select: x => new DiseaseCategory
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list2 = (await Mediator.Send(new GetCronisCategoryQuery(x => b.Contains(x.Name.ToLower()), 0, 0,
    select: x => new
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list1 = (await Mediator.Send(new GetDepartmentQuery(x => aa.Contains(x.Name.ToLower()), 0, 0,
    select: x => new Department
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list2 = (await Mediator.Send(new GetCompanyQuery(x => companyNames.Contains(x.Name.ToLower()), 0, 0,
    select: x => new Company
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list1 = (await Mediator.Send(new GetUomCategoryQuery(x => uomNames.Contains(x.Name.ToLower()), 0, 0,
    select: x => new UomCategory
    {
        Id = x.Id,
        Name = x.Name
    }))).Item1;

list1 = (await Mediator.Send(new GetDrugRouteQuery(x => dr.Contains(x.Route.ToLower()), 0, 0,
    select: x => new DrugRoute
    {
        Id = x.Id,
        Route = x.Route
    }))).Item1;

list1 = (await Mediator.Send(new GetUomQuery(x => aa.Contains(x.Name.ToLower()), 0, 0,
    select: x => new Uom
    {
        Id = x.Id, 
        Name = x.Name,
    }))).Item1;
    
select: x => new 
{
    Id = x.Id,
    Name = x.Name
}
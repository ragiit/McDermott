public class GetHealthCenterQuery(Expression<Func<HealthCenter, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<HealthCenter, object>>>? includes = null, Expression<Func<HealthCenter, HealthCenter>>? select = null) : IRequest<(List<HealthCenterDto>, int pageIndex, int pageSize, int pageCount)>
{
    public Expression<Func<HealthCenter, bool>> Predicate { get; } = predicate!;
    public bool RemoveCache { get; } = removeCache!;
    public string SearchTerm { get; } = searchTerm!;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize ?? 10;

    public List<Expression<Func<HealthCenter, object>>> Includes { get; } = includes!;
    public Expression<Func<HealthCenter, HealthCenter>>? Select { get; } = select!;
}


public async Task<(List<HealthCenterDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetHealthCenterQuery request, CancellationToken cancellationToken)
{
    try
    { 
        var query = _unitOfWork.Repository<HealthCenter>().Entities.AsNoTracking();

        // Apply dynamic includes
        if (request.Includes is not null)
        {
            foreach (var includeExpression in request.Includes)
            {
                query = query.Include(includeExpression);
            }
        }

        if (request.Predicate is not null)
            query = query.Where(request.Predicate);

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
        } 

        // Apply dynamic select if provided
        if (request.Select is not null)
        {
            query = query.Select(request.Select);
        }

        var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                          query,
                          request.PageSize,
                          request.PageIndex,
                          q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                          cancellationToken);

        return (pagedItems.Adapt<List<HealthCenterDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

 private async Task LoadDataHealthCenter(int pageIndex = 0, int pageSize = 10)
 {
     PanelVisible = true;
     var result = await Mediator.Send(new GetHealthCenterQuery(
         pageIndex: pageIndex,
         pageSize: pageSize,
         searchTerm: refHealthCenterComboBox?.Text ?? "",
         includes:
         [
             x => x.Department
         ],
         select: x => new HealthCenter
         {
             Id = x.Id,
             Name = x.Name,
             Department = new Domain.Entities.Department
             {
                 Name = x.Department.Name
             },
         }

     ));
     HealthCenters = result.Item1;
     totalCountHealthCenter = result.pageCount;
     PanelVisible = false;
 }


var result = await Mediator.QueryGetHelper<HealthCenter, HealthCenterDto>(pageIndex, pageSize, searchTerm);

var result = await Mediator.QueryGetHelper<HealthCenter, HealthCenterDto>(pageIndex, pageSize, refHealthCenterComboBox?.Text ?? "");

var result = await Mediator.QueryGetHelper<Country, CountryDto>(pageIndex, pageSize, refCountryComboBox?.Text ?? "");

var result = await Mediator.QueryGetHelper<Village, VillageDto>(pageIndex, pageSize, refVillageResidenceComboBox?.Text ?? "", x => x.DistrictId == DistrictResidenceId);

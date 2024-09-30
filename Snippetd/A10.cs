public class GetServiceQuery(Expression<Func<Service, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Service, object>>>? includes = null, Expression<Func<Service, Service>>? select = null) : IRequest<(List<ServiceDto>, int pageIndex, int pageSize, int pageCount)>
{
    public Expression<Func<Service, bool>> Predicate { get; } = predicate!;
    public bool RemoveCache { get; } = removeCache!;
    public string SearchTerm { get; } = searchTerm!;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize ?? 10;

    public List<Expression<Func<Service, object>>> Includes { get; } = includes!;
    public Expression<Func<Service, Service>>? Select { get; } = select!;
}


public async Task<(List<ServiceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetServiceQuery request, CancellationToken cancellationToken)
{
    try
    { 
        var query = _unitOfWork.Repository<Service>().Entities.AsNoTracking();

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

        return (pagedItems.Adapt<List<ServiceDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

 private async Task LoadDataService(int pageIndex = 0, int pageSize = 10)
 {
     PanelVisible = true;
     var result = await Mediator.Send(new GetServiceQuery(
         pageIndex: pageIndex,
         pageSize: pageSize,
         searchTerm: refServiceComboBox?.Text ?? "",
         includes:
         [
             x => x.Department
         ],
         select: x => new Service
         {
             Id = x.Id,
             Name = x.Name,
             Department = new Domain.Entities.Department
             {
                 Name = x.Department.Name
             },
         }

     ));
     Services = result.Item1;
     totalCountService = result.pageCount;
     PanelVisible = false;
 }


var result = await Mediator.QueryGetHelper<Country, CountryDto>(pageIndex, pageSize, searchTerm);

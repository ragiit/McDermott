public class GetProvinceQuery(Expression<Func<Province, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Province, object>>>? includes = null, Expression<Func<Province, Province>>? select = null) : IRequest<(List<ProvinceDto>, int pageIndex, int pageSize, int pageCount)>
{
    public Expression<Func<Province, bool>> Predicate { get; } = predicate!;
    public bool RemoveCache { get; } = removeCache!;
    public string SearchTerm { get; } = searchTerm!;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize ?? 10;

    public List<Expression<Func<Province, object>>> Includes { get; } = includes!;
    public Expression<Func<Province, Province>>? Select { get; } = select!;
}


public async Task<(List<ProvinceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProvinceQuery request, CancellationToken cancellationToken)
{
    try
    { 
        var query = _unitOfWork.Repository<Province>().Entities.AsNoTracking();

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

        return (pagedItems.Adapt<List<ProvinceDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

 private async Task LoadDataProvince(int pageIndex = 0, int pageSize = 10)
 {
     PanelVisible = true;
     var result = await Mediator.Send(new GetProvinceQuery(
         pageIndex: pageIndex,
         pageSize: pageSize,
         searchTerm: refProvinceComboBox?.Text ?? "",
         includes:
         [
             x => x.Department
         ],
         select: x => new Province
         {
             Id = x.Id,
             Name = x.Name,
             Department = new Domain.Entities.Department
             {
                 Name = x.Department.Name
             },
         }

     ));
     Provinces = result.Item1;
     totalCountProvince = result.pageCount;
     PanelVisible = false;
 }
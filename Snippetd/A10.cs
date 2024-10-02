public class GetProductCategoryQuery(Expression<Func<ProductCategory, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<ProductCategory, object>>>? includes = null, Expression<Func<ProductCategory, ProductCategory>>? select = null) : IRequest<(List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)>
{
    public Expression<Func<ProductCategory, bool>> Predicate { get; } = predicate!;
    public bool RemoveCache { get; } = removeCache!;
    public string SearchTerm { get; } = searchTerm!;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize ?? 10;

    public List<Expression<Func<ProductCategory, object>>> Includes { get; } = includes!;
    public Expression<Func<ProductCategory, ProductCategory>>? Select { get; } = select!;
}


public async Task<(List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
{
    try
    { 
        var query = _unitOfWork.Repository<ProductCategory>().Entities.AsNoTracking();

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

        return (pagedItems.Adapt<List<ProductCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

 private async Task LoadDataProductCategory(int pageIndex = 0, int pageSize = 10)
 {
     PanelVisible = true;
     var result = await Mediator.Send(new GetProductCategoryQuery(
         pageIndex: pageIndex,
         pageSize: pageSize,
         searchTerm: refProductCategoryComboBox?.Text ?? "",
         includes:
         [
             x => x.Department
         ],
         select: x => new ProductCategory
         {
             Id = x.Id,
             Name = x.Name,
             Department = new Domain.Entities.Department
             {
                 Name = x.Department.Name
             },
         }

     ));
     ProductCategorys = result.Item1;
     totalCountProductCategory = result.pageCount;
     PanelVisible = false;
 }


var result = await Mediator.QueryGetHelper<ProductCategory, ProductCategoryDto>(pageIndex, pageSize, searchTerm);

var result = await Mediator.QueryGetHelper<ProductCategory, ProductCategoryDto>(pageIndex, pageSize, refProductCategoryComboBox?.Text ?? "");

var result = await Mediator.QueryGetHelper<Country, CountryDto>(pageIndex, pageSize, refCountryComboBox?.Text ?? "");

var result = await Mediator.QueryGetHelper<Village, VillageDto>(pageIndex, pageSize, refVillageResidenceComboBox?.Text ?? "", x => x.DistrictId == DistrictResidenceId);

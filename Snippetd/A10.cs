public class GetSignaQuery(Expression<Func<Signa, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Signa, object>>>? includes = null, Expression<Func<Signa, Signa>>? select = null) : IRequest<(List<SignaDto>, int pageIndex, int pageSize, int pageCount)>
{
    public Expression<Func<Signa, bool>> Predicate { get; } = predicate!;
    public bool RemoveCache { get; } = removeCache!;
    public string SearchTerm { get; } = searchTerm!;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize ?? 10;

    public List<Expression<Func<Signa, object>>> Includes { get; } = includes!;
    public Expression<Func<Signa, Signa>>? Select { get; } = select!;
}

IRequestHandler<GetSignaQuery, (List<SignaDto>, int pageIndex, int pageSize, int pageCount)>,


public async Task<(List<SignaDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetSignaQuery request, CancellationToken cancellationToken)
{
    try
    { 
        var query = _unitOfWork.Repository<Signa>().Entities.AsNoTracking();

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

        return (pagedItems.Adapt<List<SignaDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

 private async Task LoadDataSigna(int pageIndex = 0, int pageSize = 10)
 {
     PanelVisible = true;
     var result = await Mediator.Send(new GetSignaQuery(
         pageIndex: pageIndex,
         pageSize: pageSize,
         searchTerm: refSignaComboBox?.Text ?? "",
         includes:
         [
             x => x.Department
         ],
         select: x => new Signa
         {
             Id = x.Id,
             Name = x.Name,
             Department = new Domain.Entities.Department
             {
                 Name = x.Department.Name
             },
         }

     ));
     Signas = result.Item1;
     totalCountSigna = result.pageCount;
     PanelVisible = false;
 }


var result = await Mediator.QueryGetHelper<Signa, SignaDto>(pageIndex, pageSize, searchTerm);

var result = await Mediator.QueryGetHelper<Signa, SignaDto>(pageIndex, pageSize, refSignaComboBox?.Text ?? "");

var result = await Mediator.QueryGetHelper<Country, CountryDto>(pageIndex, pageSize, refCountryComboBox?.Text ?? "");
var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as VillageDto ?? new());
Countries = (await Mediator.QueryGetHelper<Country, CountryDto>(predicate: x => x.Id == a.CountryId)).Item1;
var result = await Mediator.QueryGetHelper<Village, VillageDto>(pageIndex, pageSize, refVillageResidenceComboBox?.Text ?? "", x => x.DistrictId == DistrictResidenceId);

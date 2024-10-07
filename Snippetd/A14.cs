var result = await Mediator.Send(new GetMedicamentGroupQuery
{
    SearchTerm = searchTerm,
    PageIndex = pageIndex,
    PageSize = pageSize, 
});

var result1 = await Mediator.Send(new GetMedicamentGroupQuery
{
    SearchTerm = searchTerm,
    PageIndex = pageIndex,
    PageSize = pageSize,
    IsDescending = true,

});

var resul3t = await Mediator.Send(new GetMedicamentGroupQuery
{
    SearchTerm = searchTerm,
    PageIndex = pageIndex,
    PageSize = pageSize,
    IsDescending = false,
    OrderBy = x => x.Code,
    Select = x => new Domain.Entities.MedicamentGroup
    {
        Name = x.Name
    }
});

public class GetMedicamentGroupQuery : IRequest<(List<MedicamentGroupDto>, int PageIndex, int PageSize, int PageCount)>
{
    public List<Expression<Func<MedicamentGroup, object>>> Includes { get; set; }
    public Expression<Func<MedicamentGroup, bool>> Predicate { get; set; }
    public Expression<Func<MedicamentGroup, MedicamentGroup>> Select { get; set; }
    public Expression<Func<MedicamentGroup, object>> OrderBy { get; set; }
    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string SearchTerm { get; set; }
}

public class BulkValidateMedicamentGroupQuery(List<MedicamentGroupDto> MedicamentGroupsToValidate) : IRequest<List<MedicamentGroupDto>>
{
    public List<MedicamentGroupDto> MedicamentGroupsToValidate { get; } = MedicamentGroupsToValidate;
}

public class ValidateMedicamentGroupQuery(Expression<Func<MedicamentGroup, bool>>? predicate = null) : IRequest<bool>
{
    public Expression<Func<MedicamentGroup, bool>> Predicate { get; } = predicate!;
}

IRequestHandler<GetMedicamentGroupQuery, (List<MedicamentGroupDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateMedicamentGroupQuery, bool>,
IRequestHandler<BulkValidateMedicamentGroupQuery, List<MedicamentGroupDto>>,

public async Task<(List<MedicamentGroupDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMedicamentGroupQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<MedicamentGroup>().Entities.AsNoTracking();

        // Apply custom order by if provided
        if (request.OrderBy is not null) 
            query = request.IsDescending ?
                query.OrderByDescending(request.OrderBy) :
                query.OrderBy(request.OrderBy); 
        else
            query = query.OrderBy(x => x.Name);

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
                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                EF.Functions.Like(v.Code, $"%{request.SearchTerm}%") 
                );
        }

        // Apply dynamic select if provided
        if (request.Select is not null) 
            query = query.Select(request.Select);
        else
            query = query.Select(x => new MedicamentGroup
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                // PhycisianId = x.PhycisianId,
                // Phycisian = new User
                // {
                //     Name = x.Phycisian == null ? string.Empty : x.Phycisian.Name,
                // },
            });

        // Paginate and sort
        var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
            query,
            request.PageSize,
            request.PageIndex,
            cancellationToken
        );

        return (pagedItems.Adapt<List<MedicamentGroupDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}
var result = await Mediator.Send(new GetGeneralConsultanServiceQuery
{
    SearchTerm = searchTerm,
    PageIndex = pageIndex,
    PageSize = pageSize, 
});

var result1 = await Mediator.Send(new GetGeneralConsultanServiceQuery
{
    SearchTerm = searchTerm,
    PageIndex = pageIndex,
    PageSize = pageSize,
    IsDescending = true,

});

var resul3t = await Mediator.Send(new GetGeneralConsultanServiceQuery
{
    SearchTerm = searchTerm,
    PageIndex = pageIndex,
    PageSize = pageSize,
    IsDescending = false,
    OrderBy = x => x.Code,
    Select = x => new Domain.Entities.GeneralConsultanService
    {
        Name = x.Name
    }
});

public class GetGeneralConsultanServiceQuery : IRequest<(List<GeneralConsultanServiceDto>, int PageIndex, int PageSize, int PageCount)>
{
    public List<Expression<Func<GeneralConsultanService, object>>> Includes { get; set; }
    public Expression<Func<GeneralConsultanService, bool>> Predicate { get; set; }
    public Expression<Func<GeneralConsultanService, GeneralConsultanService>> Select { get; set; }
    public Expression<Func<GeneralConsultanService, object>> OrderBy { get; set; }
    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string SearchTerm { get; set; }
}

public class BulkValidateGeneralConsultanServiceQuery(List<GeneralConsultanServiceDto> GeneralConsultanServicesToValidate) : IRequest<List<GeneralConsultanServiceDto>>
{
    public List<GeneralConsultanServiceDto> GeneralConsultanServicesToValidate { get; } = GeneralConsultanServicesToValidate;
}

public class ValidateGeneralConsultanServiceQuery(Expression<Func<GeneralConsultanService, bool>>? predicate = null) : IRequest<bool>
{
    public Expression<Func<GeneralConsultanService, bool>> Predicate { get; } = predicate!;
}

IRequestHandler<GetGeneralConsultanServiceQuery, (List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateGeneralConsultanServiceQuery, bool>,
IRequestHandler<BulkValidateGeneralConsultanServiceQuery, List<GeneralConsultanServiceDto>>,

public async Task<(List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanServiceQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<GeneralConsultanService>().Entities.AsNoTracking();

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
            query = query.Select(x => new GeneralConsultanService
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

        return (pagedItems.Adapt<List<GeneralConsultanServiceDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}
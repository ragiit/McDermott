 public class GetPatientFamilyRelationQuery(Expression<Func<PatientFamilyRelation, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)>
 {
     public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; } = predicate!;
     public bool RemoveCache { get; } = removeCache!;
     public string SearchTerm { get; } = searchTerm!;
     public int PageIndex { get; } = pageIndex;
     public int PageSize { get; set; } = pageSize ?? 10;
 }

 public class BulkValidatePatientFamilyRelationQuery(List<PatientFamilyRelationDto> PatientFamilyRelationsToValidate) : IRequest<List<PatientFamilyRelationDto>>
 {
     public List<PatientFamilyRelationDto> PatientFamilyRelationsToValidate { get; } = PatientFamilyRelationsToValidate;
 }

 public class ValidatePatientFamilyRelationQuery(Expression<Func<PatientFamilyRelation, bool>>? predicate = null) : IRequest<bool>
 {
     public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; } = predicate!;
 }

IRequestHandler<GetPatientFamilyRelationQuery, (List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidatePatientFamilyRelationQuery, bool>,
IRequestHandler<BulkValidatePatientFamilyRelationQuery, List<PatientFamilyRelationDto>>,


public async Task<List<PatientFamilyRelationDto>> Handle(BulkValidatePatientFamilyRelationQuery request, CancellationToken cancellationToken)
{
    var PatientFamilyRelationDtos = request.PatientFamilyRelationsToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var PatientFamilyRelationNames = PatientFamilyRelationDtos.Select(x => x.Name).Distinct().ToList();
    var provinceIds = PatientFamilyRelationDtos.Select(x => x.ProvinceId).Distinct().ToList();

    var existingPatientFamilyRelations = await _unitOfWork.Repository<PatientFamilyRelation>()
        .Entities
        .AsNoTracking()
        .Where(v => PatientFamilyRelationNames.Contains(v.Name)
                    && provinceIds.Contains(v.ProvinceId))
        .ToListAsync(cancellationToken);

    return existingPatientFamilyRelations.Adapt<List<PatientFamilyRelationDto>>();
}

public async Task<(List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetPatientFamilyRelationQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<PatientFamilyRelation>().Entities
            .AsNoTracking()
            .Include(v => v.Province)
            .AsQueryable();

        if (request.Predicate is not null)
            query = query.Where(request.Predicate);

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%"));
        }

        var pagedResult = query.OrderBy(x => x.Name);

        var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

        return (paged.Adapt<List<PatientFamilyRelationDto>>(), request.PageIndex, request.PageSize, totalPages);
    }
    catch (Exception)
    {
        throw;
    }
}

public async Task<bool> Handle(ValidatePatientFamilyRelationQuery request, CancellationToken cancellationToken)
{
    return await _unitOfWork.Repository<PatientFamilyRelation>()
        .Entities
        .AsNoTracking()
        .Where(request.Predicate)  // Apply the Predicate for filtering
        .AnyAsync(cancellationToken);  // Check if any record matches the condition
}

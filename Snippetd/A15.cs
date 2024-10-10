public class GetGeneralConsultanCPPTsQuery : IRequest<(List<GeneralConsultanCPPTDto>, int PageIndex, int PageSize, int PageCount)>
{
    public List<Expression<Func<GeneralConsultanCPPT, object>>> Includes { get; set; }
    public Expression<Func<GeneralConsultanCPPT, bool>> Predicate { get; set; }
    public Expression<Func<GeneralConsultanCPPT, GeneralConsultanCPPT>> Select { get; set; }

    public List<(Expression<Func<GeneralConsultanCPPT, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public bool IsGetAll { get; set; } = false;
    public string SearchTerm { get; set; }
}

public class GetSingleGeneralConsultanCPPTsQuery : IRequest<GeneralConsultanCPPTDto>
{
    public List<Expression<Func<GeneralConsultanCPPT, object>>> Includes { get; set; }
    public Expression<Func<GeneralConsultanCPPT, bool>> Predicate { get; set; }
    public Expression<Func<GeneralConsultanCPPT, GeneralConsultanCPPT>> Select { get; set; }

    public List<(Expression<Func<GeneralConsultanCPPT, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public bool IsGetAll { get; set; } = false;
    public string SearchTerm { get; set; }
}


IRequestHandler<GetGeneralConsultanCPPTsQuery, (List<GeneralConsultanCPPTDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleGeneralConsultanCPPTsQuery, GeneralConsultanCPPTDto>,


public async Task<(List<GeneralConsultanCPPTDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanCPPTsQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<GeneralConsultanCPPT>().Entities.AsNoTracking();

        //// Apply custom order by if provided
        //if (request.OrderBy is not null)
        //    query = request.IsDescending ?
        //        query.OrderByDescending(request.OrderBy) :
        //        query.OrderBy(request.OrderBy);
        //else
        //    query = query.OrderBy(x => x.Id);

        // Apply ordering
        if (request.OrderByList.Count != 0)
        {
            var firstOrderBy = request.OrderByList.First();
            query = firstOrderBy.IsDescending
                ? query.OrderByDescending(firstOrderBy.OrderBy)
                : query.OrderBy(firstOrderBy.OrderBy);

            foreach (var additionalOrderBy in request.OrderByList.Skip(1))
            {
                query = additionalOrderBy.IsDescending
                    ? ((IOrderedQueryable<GeneralConsultanCPPT>)query).ThenByDescending(additionalOrderBy.OrderBy)
                    : ((IOrderedQueryable<GeneralConsultanCPPT>)query).ThenBy(additionalOrderBy.OrderBy);
            }
        }

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
            //query = query.Where(v =>
            //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
            //    EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%") ||
            //    EF.Functions.Like(v.UoM.Name, $"%{request.SearchTerm}%") ||
            //    EF.Functions.Like(v.FormDrug.Name, $"%{request.SearchTerm}%")
            //    );
        }

        // Apply dynamic select if provided
        if (request.Select is not null)
            query = query.Select(request.Select);
        else
            query = query.Select(x => new GeneralConsultanCPPT
            {
                Id = x.Id,
                Status = x.Status,
                PatientId = x.PatientId,
                Patient = new User
                {
                    Name = x.Patient == null ? string.Empty : x.Patient.Name,
                },
                PratitionerId = x.PratitionerId,
                Pratitioner = new User
                {
                    Name = x.Pratitioner == null ? string.Empty : x.Pratitioner.Name,
                },
                ServiceId = x.ServiceId,
                Service = new Service
                {
                    Name = x.Service == null ? string.Empty : x.Service.Name,
                },
                Payment = x.Payment,
                AppointmentDate = x.AppointmentDate,
                IsAlertInformationSpecialCase = x.IsAlertInformationSpecialCase,
                RegistrationDate = x.RegistrationDate,
                ClassType = x.ClassType,
                SerialNo = x.SerialNo,
                Reference = x.Reference,
            });

        if (!request.IsGetAll)
        { // Paginate and sort
            var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                query,
                request.PageSize,
                request.PageIndex,
                cancellationToken
            );

            return (pagedItems.Adapt<List<GeneralConsultanCPPTDto>>(), request.PageIndex, request.PageSize, totalPages);
        }
        else
        {
            return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanCPPTDto>>(), 0, 1, 1);
        }
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}

public async Task<GeneralConsultanCPPTDto> Handle(GetSingleGeneralConsultanCPPTsQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<GeneralConsultanCPPT>().Entities.AsNoTracking();

        if (request.Predicate is not null)
            query = query.Where(request.Predicate);

        //// Apply custom order by if provided
        //if (request.OrderBy is not null)
        //    query = request.IsDescending ?
        //        query.OrderByDescending(request.OrderBy) :
        //        query.OrderBy(request.OrderBy);
        //else
        //    query = query.OrderBy(x => x.Id);

        // Apply ordering
        if (request.OrderByList.Count != 0)
        {
            var firstOrderBy = request.OrderByList.First();
            query = firstOrderBy.IsDescending
                ? query.OrderByDescending(firstOrderBy.OrderBy)
                : query.OrderBy(firstOrderBy.OrderBy);

            foreach (var additionalOrderBy in request.OrderByList.Skip(1))
            {
                query = additionalOrderBy.IsDescending
                    ? ((IOrderedQueryable<GeneralConsultanCPPT>)query).ThenByDescending(additionalOrderBy.OrderBy)
                    : ((IOrderedQueryable<GeneralConsultanCPPT>)query).ThenBy(additionalOrderBy.OrderBy);
            }
        }

        // Apply dynamic includes
        if (request.Includes is not null)
        {
            foreach (var includeExpression in request.Includes)
            {
                query = query.Include(includeExpression);
            }
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            //query = query.Where(v =>
            //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
            //    EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%") ||
            //    EF.Functions.Like(v.UoM.Name, $"%{request.SearchTerm}%") ||
            //    EF.Functions.Like(v.FormDrug.Name, $"%{request.SearchTerm}%")
            //    );
        }

        // Apply dynamic select if provided
        if (request.Select is not null)
            query = query.Select(request.Select);
        else
            query = query.Select(x => new GeneralConsultanCPPT
            {
                Id = x.Id,
                Status = x.Status,
                PatientId = x.PatientId,
                Patient = new User
                {
                    Name = x.Patient == null ? string.Empty : x.Patient.Name,
                },
                PratitionerId = x.PratitionerId,
                Pratitioner = new User
                {
                    Name = x.Pratitioner == null ? string.Empty : x.Pratitioner.Name,
                },
                ServiceId = x.ServiceId,
                Service = new Service
                {
                    Name = x.Service == null ? string.Empty : x.Service.Name,
                },
                Payment = x.Payment,
                AppointmentDate = x.AppointmentDate,
                IsAlertInformationSpecialCase = x.IsAlertInformationSpecialCase,
                RegistrationDate = x.RegistrationDate,
                ClassType = x.ClassType,
            });

        return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanCPPTDto>();
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}

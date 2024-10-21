 public class GetSingleLabUomQuery : IRequest<LabUomDto>
 {
     public List<Expression<Func<LabUom, object>>> Includes { get; set; }
     public Expression<Func<LabUom, bool>> Predicate { get; set; }
     public Expression<Func<LabUom, LabUom>> Select { get; set; }

     public List<(Expression<Func<LabUom, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

     public bool IsDescending { get; set; } = false; // default to ascending
     public int PageIndex { get; set; } = 0;
     public int PageSize { get; set; } = 10;
     public bool IsGetAll { get; set; } = false;
     public string SearchTerm { get; set; }
 }

public class GetLabUomQuery : IRequest<(List<LabUomDto>, int PageIndex, int PageSize, int PageCount)>
{
    public List<Expression<Func<LabUom, object>>> Includes { get; set; }
    public Expression<Func<LabUom, bool>> Predicate { get; set; }
    public Expression<Func<LabUom, LabUom>> Select { get; set; }

    public List<(Expression<Func<LabUom, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public bool IsGetAll { get; set; } = false;
    public string SearchTerm { get; set; }
}
  
IRequestHandler<GetLabUomQuery, (List<LabUomDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<GetSingleLabUomQuery, LabUomDto>,

public async Task<(List<LabUomDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLabUomQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<LabUom>().Entities.AsNoTracking(); 

        if (request.Predicate is not null)
            query = query.Where(request.Predicate);

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
                    ? ((IOrderedQueryable<LabUom>)query).ThenByDescending(additionalOrderBy.OrderBy)
                    : ((IOrderedQueryable<LabUom>)query).ThenBy(additionalOrderBy.OrderBy);
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
            query = query.Where(v =>
                    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    EF.Functions.Like(v.LabUom.Name, $"%{request.SearchTerm}%")
                    );
        }

        // Apply dynamic select if provided
        if (request.Select is not null)
            query = query.Select(request.Select);
        else
            query = query.Select(x => new LabUom
            {
                Id = x.Id, 
            });

        if (!request.IsGetAll)
        { // Paginate and sort
            var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                query,
                request.PageSize,
                request.PageIndex,
                cancellationToken
            );

            return (pagedItems.Adapt<List<LabUomDto>>(), request.PageIndex, request.PageSize, totalPages);
        }
        else
        {
            return ((await query.ToListAsync(cancellationToken)).Adapt<List<LabUomDto>>(), 0, 1, 1);
        }
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}


  public async Task<LabUomDto> Handle(GetSingleLabUomQuery request, CancellationToken cancellationToken)
 {
     try
     {
         var query = _unitOfWork.Repository<LabUom>().Entities.AsNoTracking();

         if (request.Predicate is not null)
             query = query.Where(request.Predicate);
           
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
                     ? ((IOrderedQueryable<LabUom>)query).ThenByDescending(additionalOrderBy.OrderBy)
                     : ((IOrderedQueryable<LabUom>)query).ThenBy(additionalOrderBy.OrderBy);
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
            query = query.Where(v =>
                EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                EF.Functions.Like(v.LabUom.Name, $"%{request.SearchTerm}%")
                );
         }

         // Apply dynamic select if provided
         if (request.Select is not null)
             query = query.Select(request.Select);
         else
             query = query.Select(x => new LabUom
             {
                 Id = x.Id, 
             });

         return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<LabUomDto>();
     }
     catch (Exception ex)
     {
         // Consider logging the exception
         throw;
     }
 }




 var a = await Mediator.Send(new GetLabUomsQuery
 {
     OrderByList =
     [
         (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
         (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
         (x => x.ClassType != null, true)               // ThenByDescending ClassType is not null
     ],
     PageIndex = pageIndex,
     PageSize = pageSize,
 });

var patienss = (await Mediator.Send(new GetSingleLabUomQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new LabUom
    {
        Id = x.Id,
        IsEmployee = x.IsEmployee,
        Name = x.Name,
        Gender = x.Gender,
        DateOfBirth = x.DateOfBirth
    },
})) ?? new();






var data = (await Mediator.Send(new GetSingleLabUomsQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new LabUom
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new LabUom
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new LabUom
        {
            Name = x.Pratitioner.Name,
            SipNo = x.Pratitioner.SipNo
        },
        StartMaternityLeave = x.StartMaternityLeave,
        EndMaternityLeave = x.EndMaternityLeave,
        StartDateSickLeave = x.StartDateSickLeave,
        EndDateSickLeave = x.EndDateSickLeave,
    }
})) ?? new();
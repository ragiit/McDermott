public class GetGeneralConsultanServicesQuery : IRequest<(List<GeneralConsultanServiceDto>, int PageIndex, int PageSize, int PageCount)>
{
    public List<Expression<Func<GeneralConsultanService, object>>> Includes { get; set; }
    public Expression<Func<GeneralConsultanService, bool>> Predicate { get; set; }
    public Expression<Func<GeneralConsultanService, GeneralConsultanService>> Select { get; set; }

    public List<(Expression<Func<GeneralConsultanService, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

    public bool IsDescending { get; set; } = false; // default to ascending
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public bool IsGetAll { get; set; } = false;
    public string SearchTerm { get; set; }
}
  
IRequestHandler<GetGeneralConsultanServicesQuery, (List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)>,

public async Task<(List<GeneralConsultanServiceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanServicesQuery request, CancellationToken cancellationToken)
{
    try
    {
        var query = _unitOfWork.Repository<GeneralConsultanService>().Entities.AsNoTracking(); 

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
                    ? ((IOrderedQueryable<GeneralConsultanService>)query).ThenByDescending(additionalOrderBy.OrderBy)
                    : ((IOrderedQueryable<GeneralConsultanService>)query).ThenBy(additionalOrderBy.OrderBy);
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
            query = query.Select(x => new GeneralConsultanService
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

            return (pagedItems.Adapt<List<GeneralConsultanServiceDto>>(), request.PageIndex, request.PageSize, totalPages);
        }
        else
        {
            return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanServiceDto>>(), 0, 1, 1);
        }
    }
    catch (Exception ex)
    {
        // Consider logging the exception
        throw;
    }
}

 var a = await Mediator.Send(new GetGeneralConsultanServicesQuery
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

var patienss = (await Mediator.Send(new GetSingleUserQuery
{
    Predicate = x => x.Id == data.PatientId,
    Select = x => new User
    {
        Id = x.Id,
        IsEmployee = x.IsEmployee,
        Name = x.Name,
        Gender = x.Gender,
        DateOfBirth = x.DateOfBirth
    },
})) ?? new();



 public class GetSingleGeneralConsultanServicesQuery : IRequest<GeneralConsultanServiceDto>
 {
     public List<Expression<Func<GeneralConsultanService, object>>> Includes { get; set; }
     public Expression<Func<GeneralConsultanService, bool>> Predicate { get; set; }
     public Expression<Func<GeneralConsultanService, GeneralConsultanService>> Select { get; set; }

     public List<(Expression<Func<GeneralConsultanService, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

     public bool IsDescending { get; set; } = false; // default to ascending
     public int PageIndex { get; set; } = 0;
     public int PageSize { get; set; } = 10;
     public bool IsGetAll { get; set; } = false;
     public string SearchTerm { get; set; }
 }

  IRequestHandler<GetSingleGeneralConsultanServicesQuery, GeneralConsultanServiceDto>,

  public async Task<GeneralConsultanServiceDto> Handle(GetSingleGeneralConsultanServicesQuery request, CancellationToken cancellationToken)
 {
     try
     {
         var query = _unitOfWork.Repository<GeneralConsultanService>().Entities.AsNoTracking();

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
                     ? ((IOrderedQueryable<GeneralConsultanService>)query).ThenByDescending(additionalOrderBy.OrderBy)
                     : ((IOrderedQueryable<GeneralConsultanService>)query).ThenBy(additionalOrderBy.OrderBy);
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
             query = query.Select(x => new GeneralConsultanService
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

         return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanServiceDto>();
     }
     catch (Exception ex)
     {
         // Consider logging the exception
         throw;
     }
 }



var data = (await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
{
    Predicate = x => x.Id == id,
    Includes =
    [
        x => x.Pratitioner,
        x => x.Patient
    ],
    Select = x => new GeneralConsultanService
    {
        Id = x.Id,
        PatientId = x.PatientId,
        Patient = new User
        {
            DateOfBirth = x.Patient.DateOfBirth
        },
        RegistrationDate = x.RegistrationDate,
        PratitionerId = x.PratitionerId,
        Pratitioner = new User
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
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Transaction.AccidentCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class AccidentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAccidentQuery, (List<AccidentDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleAccidentQuery, AccidentDto>,
        IRequestHandler<CreateAccidentRequest, AccidentDto>,
        IRequestHandler<CreateListAccidentRequest, List<AccidentDto>>,
        IRequestHandler<UpdateAccidentRequest, AccidentDto>,
        IRequestHandler<UpdateListAccidentRequest, List<AccidentDto>>,
        IRequestHandler<DeleteAccidentRequest, bool>,
        IRequestHandler<DeleteAccidentByGcIdRequest, bool>
    {
        #region GET

        public async Task<(List<AccidentDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetAccidentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Accident>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Accident>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Accident>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //        EF.Functions.Like(v.Accident.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Accident
                    {
                        Id = x.Id,
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        SafetyPersonnelId = x.SafetyPersonnelId,
                        EmployeeClass = x.EmployeeClass,
                        Sent = x.Sent,
                        AccidentLocation = x.AccidentLocation,
                        DateOfOccurrence = x.DateOfOccurrence,
                        DateOfFirstTreatment = x.DateOfFirstTreatment,
                        AreaOfYard = x.AreaOfYard,
                        ProjectId = x.ProjectId,
                        Status = x.Status,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<AccidentDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<AccidentDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<AccidentDto> Handle(GetSingleAccidentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Accident>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Accident>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Accident>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //    EF.Functions.Like(v.Accident.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Accident
                    {
                        Id = x.Id,
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        SafetyPersonnelId = x.SafetyPersonnelId,
                        EmployeeClass = x.EmployeeClass,
                        Sent = x.Sent,
                        AccidentLocation = x.AccidentLocation,
                        DateOfOccurrence = x.DateOfOccurrence,
                        DateOfFirstTreatment = x.DateOfFirstTreatment,
                        AreaOfYard = x.AreaOfYard,
                        ProjectId = x.ProjectId,
                        Status = x.Status,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<AccidentDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<AccidentDto> Handle(CreateAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Accident>().AddAsync(request.AccidentDto.Adapt<CreateUpdateAccidentDto>().Adapt<Accident>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<AccidentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AccidentDto>> Handle(CreateListAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Accident>().AddAsync(request.AccidentDtos.Adapt<List<CreateUpdateAccidentDto>>().Adapt<List<Accident>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<AccidentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<AccidentDto> Handle(UpdateAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Accident>().UpdateAsync(request.AccidentDto.Adapt<CreateUpdateAccidentDto>().Adapt<Accident>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<AccidentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AccidentDto>> Handle(UpdateListAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Accident>().UpdateAsync(request.AccidentDtos.Adapt<List<CreateUpdateAccidentDto>>().Adapt<List<Accident>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<AccidentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Accident>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Accident>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(DeleteAccidentByGcIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    var req = await _unitOfWork.Repository<Accident>().Entities
                        .Select(x => new
                        {
                            x.GeneralConsultanServiceId,
                            x.Id,
                        })
                        .FirstOrDefaultAsync(x => x.GeneralConsultanServiceId == request.Id);

                    if (req is null)
                        return false;

                    await _unitOfWork.Repository<Accident>().DeleteAsync(req.Id);
                }

                if (request.Ids.Count > 0)
                {
                    var req = await _unitOfWork.Repository<Accident>().Entities
                        .Select(x => new
                        {
                            x.GeneralConsultanServiceId,
                            x.Id,
                        })
                        .Where(x => request.Ids.Contains(x.GeneralConsultanServiceId))
                        .ToListAsync();

                    if (req is null)
                        return false;

                    await _unitOfWork.Repository<Accident>().DeleteAsync(req.Select(z => z.Id).ToList());
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
    }
}
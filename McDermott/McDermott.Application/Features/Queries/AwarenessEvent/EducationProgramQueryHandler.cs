using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.AwarenessEvent.EducationProgramCommand;

namespace McDermott.Application.Features.Queries.AwarenessEvent
{
    public class EducationProgramQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllEducationProgramQuery, List<EducationProgramDto>>,//EducationProgram
        IRequestHandler<GetEducationProgramQuery, (List<EducationProgramDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleEducationProgramQuery, EducationProgramDto>, IRequestHandler<ValidateEducationProgramQuery, bool>,
        IRequestHandler<BulkValidateEducationProgramQuery, List<EducationProgramDto>>,
        IRequestHandler<CreateEducationProgramRequest, EducationProgramDto>,
        IRequestHandler<CreateListEducationProgramRequest, List<EducationProgramDto>>,
        IRequestHandler<UpdateEducationProgramRequest, EducationProgramDto>,
        IRequestHandler<UpdateListEducationProgramRequest, List<EducationProgramDto>>,
        IRequestHandler<DeleteEducationProgramRequest, bool>
    {
        #region GET Education Program

        public async Task<List<EducationProgramDto>> Handle(GetAllEducationProgramQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllEducationProgramQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<EducationProgram>? result))
                {
                    result = await _unitOfWork.Repository<EducationProgram>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<EducationProgramDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EducationProgramDto>> Handle(BulkValidateEducationProgramQuery request, CancellationToken cancellationToken)
        {
            var EducationProgramDtos = request.EducationProgramToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var EducationProgramNames = EducationProgramDtos.Select(x => x.EventName).Distinct().ToList();

            var existingEducationPrograms = await _unitOfWork.Repository<EducationProgram>()
                .Entities
                .AsNoTracking()
                .Where(v => EducationProgramNames.Contains(v.EventName))
                .ToListAsync(cancellationToken);

            return existingEducationPrograms.Adapt<List<EducationProgramDto>>();
        }

        public async Task<bool> Handle(ValidateEducationProgramQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<EducationProgram>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<EducationProgramDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetEducationProgramQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<EducationProgram>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<EducationProgram>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<EducationProgram>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.EventName, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.EventCategory.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new EducationProgram
                    {
                        Id = x.Id,
                        EventName = x.EventName,
                        EventCategoryId = x.EventCategoryId,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Slug = x.Slug,
                        HTMLContent = x.HTMLContent,
                        HTMLMaterial = x.HTMLMaterial,
                        Description = x.Description,
                        Status = x.Status,
                        Attendance = x.Attendance,
                        EventCategory = new AwarenessEduCategory
                        {
                            Name = x.EventCategory == null ? string.Empty : x.EventCategory.Name,
                        }
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<EducationProgramDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<EducationProgramDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<EducationProgramDto> Handle(GetSingleEducationProgramQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<EducationProgram>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<EducationProgram>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<EducationProgram>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.EventName, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.EventCategory.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new EducationProgram
                    {
                        Id = x.Id,
                        EventName = x.EventName,
                        EventCategoryId = x.EventCategoryId,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Slug = x.Slug,
                        HTMLContent = x.HTMLContent,
                        HTMLMaterial = x.HTMLMaterial,
                        Description = x.Description,
                        Status = x.Status,
                        Attendance = x.Attendance,
                        EventCategory = new AwarenessEduCategory
                        {
                            Name = x.EventCategory == null ? string.Empty : x.EventCategory.Name,
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<EducationProgramDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Education Program

        #region CREATE Education Program

        public async Task<EducationProgramDto> Handle(CreateEducationProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EducationProgram>().AddAsync(request.EducationProgramDto.Adapt<EducationProgram>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEducationProgramQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<EducationProgramDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EducationProgramDto>> Handle(CreateListEducationProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EducationProgram>().AddAsync(request.EducationProgramDtos.Adapt<List<EducationProgram>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEducationProgramQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<EducationProgramDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public async Task<EducationProgramDto> Handle(UpdateEducationProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EducationProgram>().UpdateAsync(request.EducationProgramDto.Adapt<EducationProgram>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEducationProgramQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<EducationProgramDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EducationProgramDto>> Handle(UpdateListEducationProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EducationProgram>().UpdateAsync(request.EducationProgramDtos.Adapt<List<EducationProgram>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEducationProgramQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<EducationProgramDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public async Task<bool> Handle(DeleteEducationProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<EducationProgram>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<EducationProgram>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEducationProgramQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Education Program
    }
}
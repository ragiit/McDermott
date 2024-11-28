using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Employee.DepartmentCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class DepartmentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDepartmentQuery, (List<DepartmentDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleDepartmentQuery, DepartmentDto>,
        IRequestHandler<ValidateDepartmentQuery, bool>,
        IRequestHandler<BulkValidateDepartmentQuery, List<DepartmentDto>>,
         IRequestHandler<CreateDepartmentRequest, DepartmentDto>,
         IRequestHandler<CreateListDepartmentRequest, List<DepartmentDto>>,
         IRequestHandler<UpdateDepartmentRequest, DepartmentDto>,
         IRequestHandler<UpdateListDepartmentRequest, List<DepartmentDto>>,
         IRequestHandler<DeleteDepartmentRequest, bool>
    {
        public async Task<List<DepartmentDto>> Handle(BulkValidateDepartmentQuery request, CancellationToken cancellationToken)
        {
            var DepartmentDtos = request.DepartmentsToValidate;

            var DepartmentNames = DepartmentDtos.Select(x => x.Name).Distinct().ToList();
            var a = DepartmentDtos.Select(x => x.ParentDepartmentId).Distinct().ToList();
            var b = DepartmentDtos.Select(x => x.ManagerId).Distinct().ToList();
            var c = DepartmentDtos.Select(x => x.CompanyId).Distinct().ToList();
            var d = DepartmentDtos.Select(x => x.DepartmentCategory).Distinct().ToList();

            var existingDepartments = await _unitOfWork.Repository<Department>()
                .Entities
                .AsNoTracking()
                .Where(v => DepartmentNames.Contains(v.Name)
                            && a.Contains(v.ParentDepartmentId)
                            && b.Contains(v.ManagerId)
                            && c.Contains(v.CompanyId)
                            && d.Contains(v.DepartmentCategory))
                .ToListAsync(cancellationToken);
            return existingDepartments.Adapt<List<DepartmentDto>>();
        }

        public async Task<(List<DepartmentDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Department>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Department>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Department>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.ParentDepartment.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Department
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentDepartmentId = x.ParentDepartmentId,
                        CompanyId = x.CompanyId,
                        ManagerId = x.ManagerId,
                        ParentDepartment = new Domain.Entities.Department
                        {
                            Name = x.ParentDepartment.Name
                        },
                        Company = new Domain.Entities.Company
                        {
                            Name = x.Company.Name
                        },
                        Manager = new Domain.Entities.User
                        {
                            Name = x.Manager.Name
                        },
                        DepartmentCategory = x.DepartmentCategory
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<DepartmentDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<DepartmentDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<DepartmentDto> Handle(GetSingleDepartmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Department>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Department>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Department>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.ParentDepartment.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Department
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentDepartmentId = x.ParentDepartmentId,
                        CompanyId = x.CompanyId,
                        ManagerId = x.ManagerId,
                        ParentDepartment = new Domain.Entities.Department
                        {
                            Name = x.ParentDepartment.Name
                        },
                        Company = new Domain.Entities.Company
                        {
                            Name = x.Company.Name
                        },
                        Manager = new Domain.Entities.User
                        {
                            Name = x.Manager.Name
                        },
                        DepartmentCategory = x.DepartmentCategory
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<DepartmentDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDepartmentQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Department>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<DepartmentDto> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Department>().AddAsync(request.DepartmentDto.Adapt<Department>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDepartmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DepartmentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DepartmentDto>> Handle(CreateListDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Department>().AddAsync(request.DepartmentDtos.Adapt<List<Department>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDepartmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DepartmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DepartmentDto> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Department>().UpdateAsync(request.DepartmentDto.Adapt<Department>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDepartmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DepartmentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DepartmentDto>> Handle(UpdateListDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Department>().UpdateAsync(request.DepartmentDtos.Adapt<List<Department>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDepartmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DepartmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Department>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Department>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDepartmentQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
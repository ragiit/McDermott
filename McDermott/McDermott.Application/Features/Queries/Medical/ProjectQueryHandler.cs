﻿using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Medical.ProjectCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class ProjectQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProjectQuery, (List<ProjectDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleProjectQuery, ProjectDto>,
        IRequestHandler<ValidateProjectQuery, bool>,
        IRequestHandler<BulkValidateProjectQuery, List<ProjectDto>>,
        IRequestHandler<CreateProjectRequest, ProjectDto>,
        IRequestHandler<CreateListProjectRequest, List<ProjectDto>>,
        IRequestHandler<UpdateProjectRequest, ProjectDto>,
        IRequestHandler<UpdateListProjectRequest, List<ProjectDto>>,
        IRequestHandler<DeleteProjectRequest, bool>
    {
        #region GET

        public async Task<List<ProjectDto>> Handle(BulkValidateProjectQuery request, CancellationToken cancellationToken)
        {
            var ProjectDtos = request.ProjectsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ProjectNames = ProjectDtos.Select(x => x.Name).Distinct().ToList();
            var Codes = ProjectDtos.Select(x => x.Code).Distinct().ToList();

            var existingProjects = await _unitOfWork.Repository<Project>()
                .Entities
                .AsNoTracking()
                .Where(v => ProjectNames.Contains(v.Name) && Codes.Contains(v.Code))
                .ToListAsync(cancellationToken);

            return existingProjects.Adapt<List<ProjectDto>>();
        }

        public async Task<(List<ProjectDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Project>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Project>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Project>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Project
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<ProjectDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<ProjectDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<ProjectDto> Handle(GetSingleProjectQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Project>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Project>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Project>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Project
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ProjectDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateProjectQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Project>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<ProjectDto> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Project>().AddAsync(request.ProjectDto.Adapt<CreateUpdateProjectDto>().Adapt<Project>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProjectQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProjectDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProjectDto>> Handle(CreateListProjectRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Project>().AddAsync(request.ProjectDtos.Adapt<List<Project>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProjectQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProjectDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProjectDto> Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Project>().UpdateAsync(request.ProjectDto.Adapt<CreateUpdateProjectDto>().Adapt<Project>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProjectQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProjectDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProjectDto>> Handle(UpdateListProjectRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Project>().UpdateAsync(request.ProjectDtos.Adapt<List<Project>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProjectQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProjectDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Project>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Project>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProjectQuery_"); // Ganti dengan key yang sesuai

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
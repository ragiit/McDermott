using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Employee.JobPositionCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class JobPositionQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
       IRequestHandler<GetJobPositionQuery, (List<JobPositionDto>, int pageIndex, int pageSize, int pageCount)>,
IRequestHandler<ValidateJobPositionQuery, bool>,
IRequestHandler<BulkValidateJobPositionQuery, List<JobPositionDto>>,
        IRequestHandler<CreateJobPositionRequest, JobPositionDto>,
        IRequestHandler<CreateListJobPositionRequest, List<JobPositionDto>>,
        IRequestHandler<UpdateJobPositionRequest, JobPositionDto>,
        IRequestHandler<UpdateListJobPositionRequest, List<JobPositionDto>>,
        IRequestHandler<DeleteJobPositionRequest, bool>
    {
        #region GET

        public async Task<List<JobPositionDto>> Handle(BulkValidateJobPositionQuery request, CancellationToken cancellationToken)
        {
            var JobPositionDtos = request.JobPositionsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var JobPositionNames = JobPositionDtos.Select(x => x.Name).Distinct().ToList();
            var provinceIds = JobPositionDtos.Select(x => x.DepartmentId).Distinct().ToList();

            var existingJobPositions = await _unitOfWork.Repository<JobPosition>()
                .Entities
                .AsNoTracking()
                .Where(v => JobPositionNames.Contains(v.Name)
                            && provinceIds.Contains(v.DepartmentId))
                .ToListAsync(cancellationToken);

            return existingJobPositions.Adapt<List<JobPositionDto>>();
        }

        public async Task<(List<JobPositionDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetJobPositionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<JobPosition>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.Department.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                                  query,
                                  request.PageSize,
                                  request.PageIndex,
                                  q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<JobPositionDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateJobPositionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<JobPosition>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<JobPositionDto> Handle(CreateJobPositionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<JobPosition>().AddAsync(request.JobPositionDto.Adapt<CreateUpdateJobPositionDto>().Adapt<JobPosition>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetJobPositionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<JobPositionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<JobPositionDto>> Handle(CreateListJobPositionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<JobPosition>().AddAsync(request.JobPositionDtos.Adapt<List<JobPosition>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetJobPositionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<JobPositionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<JobPositionDto> Handle(UpdateJobPositionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<JobPosition>().UpdateAsync(request.JobPositionDto.Adapt<CreateUpdateJobPositionDto>().Adapt<JobPosition>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetJobPositionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<JobPositionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<JobPositionDto>> Handle(UpdateListJobPositionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<JobPosition>().UpdateAsync(request.JobPositionDtos.Adapt<List<JobPosition>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetJobPositionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<JobPositionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteJobPositionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<JobPosition>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<JobPosition>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetJobPositionQuery_"); // Ganti dengan key yang sesuai

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
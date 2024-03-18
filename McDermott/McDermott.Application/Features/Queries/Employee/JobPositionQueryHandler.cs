using static McDermott.Application.Features.Commands.Employee.JobPositionCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class JobPositionQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetJobPositionQuery, List<JobPositionDto>>,
        IRequestHandler<CreateJobPositionRequest, JobPositionDto>,
        IRequestHandler<CreateListJobPositionRequest, List<JobPositionDto>>,
        IRequestHandler<UpdateJobPositionRequest, JobPositionDto>,
        IRequestHandler<UpdateListJobPositionRequest, List<JobPositionDto>>,
        IRequestHandler<DeleteJobPositionRequest, bool>
    {
        #region GET

        public async Task<List<JobPositionDto>> Handle(GetJobPositionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetJobPositionQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<JobPosition>? result))
                {
                    result = await _unitOfWork.Repository<JobPosition>().GetAsync(
                        null,
                        x => x.Include(z => z.Department),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<JobPositionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<JobPositionDto> Handle(CreateJobPositionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<JobPosition>().AddAsync(request.JobPositionDto.Adapt<JobPosition>());

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
                var result = await _unitOfWork.Repository<JobPosition>().UpdateAsync(request.JobPositionDto.Adapt<JobPosition>());

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
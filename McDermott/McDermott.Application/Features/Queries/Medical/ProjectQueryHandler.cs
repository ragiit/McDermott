
using static McDermott.Application.Features.Commands.Medical.ProjectCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class ProjectQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProjectQuery, List<ProjectDto>>,
        IRequestHandler<CreateProjectRequest, ProjectDto>,
        IRequestHandler<CreateListProjectRequest, List<ProjectDto>>,
        IRequestHandler<UpdateProjectRequest, ProjectDto>,
        IRequestHandler<UpdateListProjectRequest, List<ProjectDto>>,
        IRequestHandler<DeleteProjectRequest, bool>
    {
        #region GET

        public async Task<List<ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetProjectQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Project>? result))
                {
                    result = await _unitOfWork.Repository<Project>().Entities.AsNoTracking().ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ProjectDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ProjectDto> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Project>().AddAsync(request.ProjectDto.Adapt<Project>());

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
                var result = await _unitOfWork.Repository<Project>().UpdateAsync(request.ProjectDto.Adapt<Project>());

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
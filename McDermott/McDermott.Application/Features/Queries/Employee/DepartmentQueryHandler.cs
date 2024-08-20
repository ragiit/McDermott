using static McDermott.Application.Features.Commands.Employee.DepartmentCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class DepartmentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetDepartmentQuery, List<DepartmentDto>>,
         IRequestHandler<CreateDepartmentRequest, DepartmentDto>,
         IRequestHandler<CreateListDepartmentRequest, List<DepartmentDto>>,
         IRequestHandler<UpdateDepartmentRequest, DepartmentDto>,
         IRequestHandler<UpdateListDepartmentRequest, List<DepartmentDto>>,
         IRequestHandler<DeleteDepartmentRequest, bool>
    {
        public async Task<List<DepartmentDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDepartmentQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Department>? result))
                {
                    result = await _unitOfWork.Repository<Department>().Entities
                        .Include(z => z.Manager)
                        .Include(z => z.ParentDepartment)
                        .Include(z => z.Company)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DepartmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
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
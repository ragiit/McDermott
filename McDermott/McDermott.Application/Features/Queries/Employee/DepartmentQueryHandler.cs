using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Employee.DepartmentCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class DepartmentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetDepartmentQuery, (List<DepartmentDto>, int pageIndex, int pageSize, int pageCount)>,
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

            // Ekstrak semua kombinasi yang akan dicari di database
            var DepartmentNames = DepartmentDtos.Select(x => x.Name).Distinct().ToList();
            //var provinceIds = DepartmentDtos.Select(x => x.ProvinceId).Distinct().ToList();

            //var existingDepartments = await _unitOfWork.Repository<Department>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => DepartmentNames.Contains(v.Name)
            //                && provinceIds.Contains(v.ProvinceId))
            //    .ToListAsync(cancellationToken);
            return [];
            //return existingDepartments.Adapt<List<DepartmentDto>>();
        }

        public async Task<(List<DepartmentDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Department>().Entities
                    .AsNoTracking()
                    .Include(v => v.Manager)
                    .Include(v => v.ParentDepartment)
                    .Include(v => v.Company)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Manager.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.ParentDepartment.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

                return (paged.Adapt<List<DepartmentDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
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
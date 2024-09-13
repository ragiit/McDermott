using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.CompanyCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class CompanyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateCompanyQuery, bool>,
        IRequestHandler<BulkValidateCompanyQuery, List<CompanyDto>>,
        IRequestHandler<CreateCompanyRequest, CompanyDto>,
        IRequestHandler<CreateListCompanyRequest, List<CompanyDto>>,
        IRequestHandler<UpdateCompanyRequest, CompanyDto>,
        IRequestHandler<UpdateListCompanyRequest, List<CompanyDto>>,
        IRequestHandler<DeleteCompanyRequest, bool>
    {
        #region GET

        public async Task<List<CompanyDto>> Handle(BulkValidateCompanyQuery request, CancellationToken cancellationToken)
        {
            var CompanyDtos = request.CompanysToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var CompanyNames = CompanyDtos.Select(x => x.Name).Distinct().ToList();
            var provinceIds = CompanyDtos.Select(x => x.ProvinceId).Distinct().ToList();

            var existingCompanys = await _unitOfWork.Repository<Company>()
                .Entities
                .AsNoTracking()
                .Where(v => CompanyNames.Contains(v.Name)
                            && provinceIds.Contains(v.ProvinceId))
                .ToListAsync(cancellationToken);

            return existingCompanys.Adapt<List<CompanyDto>>();
        }

        public async Task<(List<CompanyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Company>().Entities
                    .AsNoTracking()
                    .Include(v => v.Province)
                    .Include(v => v.City)
                    .Include(v => v.Country)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.City.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Country.Name, $"%{request.SearchTerm}%"));
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

                return (paged.Adapt<List<CompanyDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateCompanyQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Company>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<CompanyDto> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDto.Adapt<Company>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CompanyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CompanyDto>> Handle(CreateListCompanyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Company>().AddAsync(request.CompanyDtos.Adapt<List<Company>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CompanyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CompanyDto> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDto.Adapt<Company>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<CompanyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CompanyDto>> Handle(UpdateListCompanyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Company>().UpdateAsync(request.CompanyDtos.Adapt<List<Company>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CompanyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Company>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Company>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCompanyQuery_"); // Ganti dengan key yang sesuai

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
using static McDermott.Application.Features.Commands.Config.CompanyCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class CompanyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateCompanyRequest, CompanyDto>,
        IRequestHandler<CreateListCompanyRequest, List<CompanyDto>>,
        IRequestHandler<UpdateCompanyRequest, CompanyDto>,
        IRequestHandler<UpdateListCompanyRequest, List<CompanyDto>>,
        IRequestHandler<DeleteCompanyRequest, bool>
    {
        #region GET

        public async Task<(List<CompanyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Company>().Entities
                    .Include(x => x.City)
                    .Include(x => x.Province)
                    .Include(x => x.Country)
                    .AsNoTracking()
                    .AsQueryable();
                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Email, $"{request.SearchTerm}") ||
                        EF.Functions.Like(v.Phone, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

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
using static McDermott.Application.Features.Commands.Config.CompanyCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class CompanyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCompanyQuery, List<CompanyDto>>,
        IRequestHandler<CreateCompanyRequest, CompanyDto>,
        IRequestHandler<CreateListCompanyRequest, List<CompanyDto>>,
        IRequestHandler<UpdateCompanyRequest, CompanyDto>,
        IRequestHandler<UpdateListCompanyRequest, List<CompanyDto>>,
        IRequestHandler<DeleteCompanyRequest, bool>
    {
        #region GET

        public async Task<List<CompanyDto>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetCompanyQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Company>? result))
                {
                    result = await _unitOfWork.Repository<Company>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.Country)
                        .Include(z => z.City)
                        .Include(z => z.Province),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<CompanyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
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
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.CompanyCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class CompanyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCompanyQuery, (List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleCompanyQuery, CompanyDto>,
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
                var query = _unitOfWork.Repository<Company>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Company>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Company>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //        EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Company
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Phone = x.Phone,
                        Email = x.Email,
                        Website = x.Website,
                        VAT = x.VAT,
                        Street1 = x.Street1,
                        Street2 = x.Street2,
                        Zip = x.Zip,
                        CurrencyId = x.CurrencyId,
                        Logo = x.Logo,
                        CityId = x.CityId,
                        ProvinceId = x.ProvinceId,
                        CountryId = x.CountryId,
                        Country = new Country
                        {
                            Name = x.Country.Name
                        },
                        Province = new Province
                        {
                            Name = x.Province.Name
                        },
                        City = new City
                        {
                            Name = x.City.Name
                        },
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<CompanyDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<CompanyDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<CompanyDto> Handle(GetSingleCompanyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Company>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Company>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Company>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Company.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Company
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Phone = x.Phone,
                        Email = x.Email,
                        Website = x.Website,
                        VAT = x.VAT,
                        Street1 = x.Street1,
                        Street2 = x.Street2,
                        Zip = x.Zip,
                        CurrencyId = x.CurrencyId,
                        Logo = x.Logo,
                        CityId = x.CityId,
                        ProvinceId = x.ProvinceId,
                        CountryId = x.CountryId,
                        Country = new Country
                        {
                            Name = x.Country.Name
                        },
                        Province = new Province
                        {
                            Name = x.Province.Name
                        },
                        City = new City
                        {
                            Name = x.City.Name
                        },
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<CompanyDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
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
                var req = request.CompanyDto.Adapt<CreateUpdateCompanyDto>().Adapt<Company>();

                var result = await _unitOfWork.Repository<Company>().AddAsync(req);

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
                var req = request.CompanyDtos.Adapt<CreateUpdateCompanyDto>().Adapt<Company>();

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
                var req = request.CompanyDto.Adapt<CreateUpdateCompanyDto>().Adapt<Company>();

                var result = await _unitOfWork.Repository<Company>().UpdateAsync(req);

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
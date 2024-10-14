using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Medical.InsuranceCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class InsuranceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInsuranceQuery, (List<InsuranceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleInsuranceQuery, InsuranceDto>,
        IRequestHandler<CreateInsuranceRequest, InsuranceDto>,
        IRequestHandler<BulkValidateInsuranceQuery, List<InsuranceDto>>,
        IRequestHandler<CreateListInsuranceRequest, List<InsuranceDto>>,
        IRequestHandler<UpdateInsuranceRequest, InsuranceDto>,
        IRequestHandler<UpdateListInsuranceRequest, List<InsuranceDto>>,
        IRequestHandler<DeleteInsuranceRequest, bool>
    {
        #region GET

        public async Task<List<InsuranceDto>> Handle(BulkValidateInsuranceQuery request, CancellationToken cancellationToken)
        {
            var InsuranceDtos = request.InsurancesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var InsuranceNames = InsuranceDtos.Select(x => x.Name).Distinct().ToList();
            var a = InsuranceDtos.Select(x => x.Code).Distinct().ToList();
            var b = InsuranceDtos.Select(x => x.Type).Distinct().ToList();
            var c = InsuranceDtos.Select(x => x.AdminFee).Distinct().ToList();
            var d = InsuranceDtos.Select(x => x.Presentase).Distinct().ToList();
            var e = InsuranceDtos.Select(x => x.AdminFeeMax).Distinct().ToList();

            var existingInsurances = await _unitOfWork.Repository<Insurance>()
                .Entities
                .AsNoTracking()
                .Where(v => InsuranceNames.Contains(v.Name)
                            && a.Contains(v.Code)
                            && b.Contains(v.Type)
                            && c.Contains(v.AdminFee)
                            && d.Contains(v.Presentase)
                            && e.Contains(v.AdminFeeMax))
                .ToListAsync(cancellationToken);

            return existingInsurances.Adapt<List<InsuranceDto>>();
        }

        public async Task<InsuranceDto> Handle(GetSingleInsuranceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Insurance>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Insurance>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Insurance>)query).ThenBy(additionalOrderBy.OrderBy);
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
                       EF.Functions.Like(v.Code, $"%{request.SearchTerm}%") ||
                       EF.Functions.Like(v.Type, $"%{request.SearchTerm}%") ||
                       v.AdminFee.Equals(request.SearchTerm) ||
                       v.AdminFeeMax.Equals(request.SearchTerm) |
                       v.Presentase.Equals(request.SearchTerm));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Insurance
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Type = x.Type,
                        IsBPJS = x.IsBPJS,
                        IsBPJSKesehatan = x.IsBPJSKesehatan,
                        IsBPJSTK = x.IsBPJSTK,
                        AdminFee = x.AdminFee,
                        AdminFeeMax = x.AdminFeeMax,
                        Presentase = x.Presentase,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<InsuranceDto>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<InsuranceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetInsuranceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Insurance>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Insurance>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Insurance>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Type, $"%{request.SearchTerm}%") ||
                        v.AdminFee.Equals(request.SearchTerm) ||
                        v.AdminFeeMax.Equals(request.SearchTerm) |
                        v.Presentase.Equals(request.SearchTerm));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Insurance
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Type = x.Type,
                        IsBPJS = x.IsBPJS,
                        IsBPJSKesehatan = x.IsBPJSKesehatan,
                        IsBPJSTK = x.IsBPJSTK,
                        AdminFee = x.AdminFee,
                        AdminFeeMax = x.AdminFeeMax,
                        Presentase = x.Presentase,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<InsuranceDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<InsuranceDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateInsuranceQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Insurance>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<InsuranceDto> Handle(CreateInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().AddAsync(request.InsuranceDto.Adapt<Insurance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsuranceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsuranceDto>> Handle(CreateListInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().AddAsync(request.InsuranceDtos.Adapt<List<Insurance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsuranceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<InsuranceDto> Handle(UpdateInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().UpdateAsync(request.InsuranceDto.Adapt<Insurance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsuranceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsuranceDto>> Handle(UpdateListInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Insurance>().UpdateAsync(request.InsuranceDtos.Adapt<List<Insurance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsuranceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInsuranceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Insurance>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Insurance>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsuranceQuery_"); // Ganti dengan key yang sesuai

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
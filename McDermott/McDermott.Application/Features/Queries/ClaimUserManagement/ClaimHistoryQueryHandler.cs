using McDermott.Application.Dtos.ClaimUserManagement;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.ClaimUserManagement.ClaimHistoryCommand;

namespace McDermott.Application.Features.Queries.ClaimUserManagement
{
    public class ClaimHistoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllClaimHistoryQuery, List<ClaimHistoryDto>>,//ClaimHistory
        IRequestHandler<GetClaimHistoryQuery, (List<ClaimHistoryDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleClaimHistoryQuery, ClaimHistoryDto>, IRequestHandler<ValidateClaimHistoryQuery, bool>,
        IRequestHandler<BulkValidateClaimHistoryQuery, List<ClaimHistoryDto>>,
        IRequestHandler<CreateClaimHistoryRequest, ClaimHistoryDto>,
        IRequestHandler<CreateListClaimHistoryRequest, List<ClaimHistoryDto>>,
        IRequestHandler<UpdateClaimHistoryRequest, ClaimHistoryDto>,
        IRequestHandler<UpdateListClaimHistoryRequest, List<ClaimHistoryDto>>,
        IRequestHandler<DeleteClaimHistoryRequest, bool>
    {
        #region GET Education Program

      

        public async Task<List<ClaimHistoryDto>> Handle(GetAllClaimHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllClaimHistoryQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ClaimHistory>? result))
                {
                    result = await _unitOfWork.Repository<ClaimHistory>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ClaimHistoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ClaimHistoryDto>> Handle(BulkValidateClaimHistoryQuery request, CancellationToken cancellationToken)
        {
            var ClaimHistoryDtos = request.ClaimHistoryToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ClaimHistoryNames = ClaimHistoryDtos.Select(x => x.Patient.Name).Distinct().ToList();

            var existingClaimHistorys = await _unitOfWork.Repository<ClaimHistory>()
                .Entities
                .AsNoTracking()
                .Where(v => ClaimHistoryNames.Contains(v.Patient.Name))
                .ToListAsync(cancellationToken);

            return existingClaimHistorys.Adapt<List<ClaimHistoryDto>>();
        }

        public async Task<bool> Handle(ValidateClaimHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<ClaimHistory>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<ClaimHistoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetClaimHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ClaimHistory>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<ClaimHistory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<ClaimHistory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Patient.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Benefit.BenefitName, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new ClaimHistory
                    {
                        Id = x.Id,
                        ClaimDate = x.ClaimDate,
                        ClaimedValue = x.ClaimedValue,
                        PatientId = x.PatientId,
                        BenefitId = x.BenefitId,
                        PhycisianId = x.PhycisianId,
                        Patient = new User
                        {
                            Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        },
                        Phycisian = new User
                        {
                            Name = x.Phycisian == null ? string.Empty : x.Phycisian.Name,
                        },
                        Benefit = new BenefitConfiguration
                        {
                            BenefitName = x.Benefit == null ? string.Empty : x.Benefit.BenefitName
                        }
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<ClaimHistoryDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<ClaimHistoryDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<ClaimHistoryDto> Handle(GetSingleClaimHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ClaimHistory>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<ClaimHistory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<ClaimHistory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Patient.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Benefit.BenefitName, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new ClaimHistory
                    {
                        Id = x.Id,
                        ClaimDate = x.ClaimDate,
                        ClaimedValue = x.ClaimedValue,
                        PatientId = x.PatientId,
                        BenefitId = x.BenefitId,
                        PhycisianId = x.PhycisianId,
                        Patient = new User
                        {
                            Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        },
                        Phycisian = new User
                        {
                            Name = x.Phycisian == null ? string.Empty : x.Phycisian.Name,
                        },
                        Benefit = new BenefitConfiguration
                        {
                            BenefitName = x.Benefit == null ? string.Empty : x.Benefit.BenefitName
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ClaimHistoryDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Education Program

        #region CREATE Education Program

        public async Task<ClaimHistoryDto> Handle(CreateClaimHistoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClaimHistory>().AddAsync(request.ClaimHistoryDto.Adapt<ClaimHistory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimHistoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ClaimHistoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ClaimHistoryDto>> Handle(CreateListClaimHistoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClaimHistory>().AddAsync(request.ClaimHistoryDtos.Adapt<List<ClaimHistory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimHistoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ClaimHistoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public async Task<ClaimHistoryDto> Handle(UpdateClaimHistoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClaimHistory>().UpdateAsync(request.ClaimHistoryDto.Adapt<ClaimHistory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimHistoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ClaimHistoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ClaimHistoryDto>> Handle(UpdateListClaimHistoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClaimHistory>().UpdateAsync(request.ClaimHistoryDtos.Adapt<List<ClaimHistory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimHistoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ClaimHistoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public async Task<bool> Handle(DeleteClaimHistoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ClaimHistory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ClaimHistory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimHistoryQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Education Program
    }
}
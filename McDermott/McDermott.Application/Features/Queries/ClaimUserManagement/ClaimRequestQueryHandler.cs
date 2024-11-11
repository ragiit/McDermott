using McDermott.Application.Dtos.ClaimUserManagement;
using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.ClaimUserManagement.ClaimRequestCommand;

namespace McDermott.Application.Features.Queries.ClaimUserManagement
{
    public class ClaimRequestQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetAllClaimRequestQuery, List<ClaimRequestDto>>,//ClaimRequest
     IRequestHandler<GetClaimRequestQuery, (List<ClaimRequestDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleClaimRequestQuery, ClaimRequestDto>, IRequestHandler<ValidateClaimRequestQuery, bool>,
     IRequestHandler<BulkValidateClaimRequestQuery, List<ClaimRequestDto>>,
     IRequestHandler<CreateClaimRequestRequest, ClaimRequestDto>,
     IRequestHandler<CreateListClaimRequestRequest, List<ClaimRequestDto>>,
     IRequestHandler<UpdateClaimRequestRequest, ClaimRequestDto>,
     IRequestHandler<UpdateListClaimRequestRequest, List<ClaimRequestDto>>,
     IRequestHandler<DeleteClaimRequestRequest, bool>
    {
        #region GET Education Program

        public async Task<List<ClaimRequestDto>> Handle(GetAllClaimRequestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllClaimRequestQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ClaimRequest>? result))
                {
                    result = await _unitOfWork.Repository<ClaimRequest>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ClaimRequestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ClaimRequestDto>> Handle(BulkValidateClaimRequestQuery request, CancellationToken cancellationToken)
        {
            var ClaimRequestDtos = request.ClaimRequestToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ClaimRequestNames = ClaimRequestDtos.Select(x => x.Patient.Name).Distinct().ToList();

            var existingClaimRequests = await _unitOfWork.Repository<ClaimRequest>()
                .Entities
                .AsNoTracking()
                .Where(v => ClaimRequestNames.Contains(v.Patient.Name))
                .ToListAsync(cancellationToken);

            return existingClaimRequests.Adapt<List<ClaimRequestDto>>();
        }

        public async Task<bool> Handle(ValidateClaimRequestQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<ClaimRequest>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<ClaimRequestDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetClaimRequestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ClaimRequest>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<ClaimRequest>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<ClaimRequest>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Benefit.BenefitName, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new ClaimRequest
                    {
                        Id = x.Id,
                        ClaimDate = x.ClaimDate,
                        Remark = x.Remark,
                        Status = x.Status,
                        PatientId = x.PatientId,
                        PhycisianId = x.PhycisianId,
                        BenefitId = x.BenefitId,
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
                            BenefitName = x.Benefit == null ? string.Empty : x.Benefit.BenefitName,
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

                    return (pagedItems.Adapt<List<ClaimRequestDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<ClaimRequestDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<ClaimRequestDto> Handle(GetSingleClaimRequestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ClaimRequest>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<ClaimRequest>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<ClaimRequest>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Benefit.BenefitName, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new ClaimRequest
                    {
                        Id = x.Id,
                        ClaimDate = x.ClaimDate,
                        Remark = x.Remark,
                        Status = x.Status,
                        PatientId = x.PatientId,
                        PhycisianId = x.PhycisianId,
                        BenefitId = x.BenefitId,
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
                            BenefitName = x.Benefit == null ? string.Empty : x.Benefit.BenefitName,
                        }

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ClaimRequestDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }



        #endregion GET Education Program

        #region CREATE Education Program

        public async Task<ClaimRequestDto> Handle(CreateClaimRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClaimRequest>().AddAsync(request.ClaimRequestDto.Adapt<ClaimRequest>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimRequestQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<ClaimRequestDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ClaimRequestDto>> Handle(CreateListClaimRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClaimRequest>().AddAsync(request.ClaimRequestDtos.Adapt<List<ClaimRequest>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimRequestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ClaimRequestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public async Task<ClaimRequestDto> Handle(UpdateClaimRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClaimRequest>().UpdateAsync(request.ClaimRequestDto.Adapt<ClaimRequest>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);


                _cache.Remove("GetClaimRequestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ClaimRequestDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ClaimRequestDto>> Handle(UpdateListClaimRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ClaimRequest>().UpdateAsync(request.ClaimRequestDtos.Adapt<List<ClaimRequest>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimRequestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ClaimRequestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public async Task<bool> Handle(DeleteClaimRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ClaimRequest>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ClaimRequest>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetClaimRequestQuery_"); // Ganti dengan key yang sesuai

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

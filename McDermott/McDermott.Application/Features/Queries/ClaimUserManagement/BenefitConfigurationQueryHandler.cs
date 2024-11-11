using McDermott.Application.Dtos.ClaimUserManagement;
using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.ClaimUserManagement.BenefitConfigurationCommand;

namespace McDermott.Application.Features.Queries.ClaimUserManagement
{
    public class BenefitConfigurationQueryhandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllBenefitConfigurationQuery, List<BenefitConfigurationDto>>,//BenefitConfiguration
        IRequestHandler<GetBenefitConfigurationQuery, (List<BenefitConfigurationDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleBenefitConfigurationQuery, BenefitConfigurationDto>, IRequestHandler<ValidateBenefitConfigurationQuery, bool>,
        IRequestHandler<BulkValidateBenefitConfigurationQuery, List<BenefitConfigurationDto>>,
        IRequestHandler<CreateBenefitConfigurationRequest, BenefitConfigurationDto>,
        IRequestHandler<CreateListBenefitConfigurationRequest, List<BenefitConfigurationDto>>,
        IRequestHandler<UpdateBenefitConfigurationRequest, BenefitConfigurationDto>,
        IRequestHandler<UpdateListBenefitConfigurationRequest, List<BenefitConfigurationDto>>,
        IRequestHandler<DeleteBenefitConfigurationRequest, bool>
    {
        #region GET Education Program

        public async Task<List<BenefitConfigurationDto>> Handle(GetAllBenefitConfigurationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllBenefitConfigurationQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<BenefitConfiguration>? result))
                {
                    result = await _unitOfWork.Repository<BenefitConfiguration>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<BenefitConfigurationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BenefitConfigurationDto>> Handle(BulkValidateBenefitConfigurationQuery request, CancellationToken cancellationToken)
        {
            var BenefitConfigurationDtos = request.BenefitConfigurationToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var BenefitConfigurationNames = BenefitConfigurationDtos.Select(x => x.BenefitName).Distinct().ToList();

            var existingBenefitConfigurations = await _unitOfWork.Repository<BenefitConfiguration>()
                .Entities
                .AsNoTracking()
                .Where(v => BenefitConfigurationNames.Contains(v.BenefitName))
                .ToListAsync(cancellationToken);

            return existingBenefitConfigurations.Adapt<List<BenefitConfigurationDto>>();
        }

        public async Task<bool> Handle(ValidateBenefitConfigurationQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<BenefitConfiguration>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<BenefitConfigurationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetBenefitConfigurationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<BenefitConfiguration>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<BenefitConfiguration>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<BenefitConfiguration>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.BenefitName, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new BenefitConfiguration
                    {
                        Id = x.Id,
                        BenefitName = x.BenefitName,
                        DurationOfBenefit = x.DurationOfBenefit,
                        Status = x.Status,
                        TypeOfBenefit = x.TypeOfBenefit,
                        IsEmployee = x.IsEmployee,
                        BenefitValue = x.BenefitValue,
                        BenefitDuration = x.BenefitDuration,

                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<BenefitConfigurationDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<BenefitConfigurationDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }
        public async Task<BenefitConfigurationDto> Handle(GetSingleBenefitConfigurationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<BenefitConfiguration>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<BenefitConfiguration>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<BenefitConfiguration>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.BenefitName, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new BenefitConfiguration
                    {
                        Id = x.Id,
                        BenefitName = x.BenefitName,
                        DurationOfBenefit = x.DurationOfBenefit,
                        Status = x.Status,
                        TypeOfBenefit = x.TypeOfBenefit,
                        IsEmployee = x.IsEmployee,
                        BenefitValue = x.BenefitValue,
                        BenefitDuration = x.BenefitDuration,

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<BenefitConfigurationDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Education Program

        #region CREATE Education Program

        public async Task<BenefitConfigurationDto> Handle(CreateBenefitConfigurationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BenefitConfiguration>().AddAsync(request.BenefitConfigurationDto.Adapt<BenefitConfiguration>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBenefitConfigurationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<BenefitConfigurationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BenefitConfigurationDto>> Handle(CreateListBenefitConfigurationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BenefitConfiguration>().AddAsync(request.BenefitConfigurationDtos.Adapt<List<BenefitConfiguration>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBenefitConfigurationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<BenefitConfigurationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public async Task<BenefitConfigurationDto> Handle(UpdateBenefitConfigurationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BenefitConfiguration>().UpdateAsync(request.BenefitConfigurationDto.Adapt<BenefitConfiguration>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBenefitConfigurationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<BenefitConfigurationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BenefitConfigurationDto>> Handle(UpdateListBenefitConfigurationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<BenefitConfiguration>().UpdateAsync(request.BenefitConfigurationDtos.Adapt<List<BenefitConfiguration>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBenefitConfigurationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<BenefitConfigurationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public async Task<bool> Handle(DeleteBenefitConfigurationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<BenefitConfiguration>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<BenefitConfiguration>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetBenefitConfigurationQuery_"); // Ganti dengan key yang sesuai

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

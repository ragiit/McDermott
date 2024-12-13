using McDermott.Application.Dtos.Telemedicines;
using McDermott.Application.Features.Commands;
using McDermott.Application.Features.Services;
using McDermott.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.GetDataCommand;
using static McDermott.Application.Features.Commands.Telemedicines.OTPRequestCommand;

namespace McDermott.Application.Features.Queries.Telemedicines
{
    public class OTPRequestHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetOTPRequestQuery, (List<OTPRequestDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleOTPRequestQuery, OTPRequestDto>,
     IRequestHandler<GetOTPRequestQuerylable, IQueryable<OTPRequest>>,
     IRequestHandler<ValidateOTPRequest, bool>,
     IRequestHandler<CreateOTPRequestRequest, OTPRequestDto>,
     IRequestHandler<BulkValidateOTPRequest, List<OTPRequestDto>>,
     IRequestHandler<CreateListOTPRequestRequest, List<OTPRequestDto>>,
     IRequestHandler<UpdateOTPRequestRequest, OTPRequestDto>,
     IRequestHandler<UpdateListOTPRequestRequest, List<OTPRequestDto>>,
     IRequestHandler<DeleteOTPRequestRequest, bool>
    {
        private Task<IQueryable<TEntity>> HandleQuery<TEntity>(BaseQuery<TEntity> request, CancellationToken cancellationToken, Expression<Func<TEntity, TEntity>>? select = null)
         where TEntity : BaseAuditableEntity // Add the constraint here
        {
            try
            {
                var query = _unitOfWork.Repository<TEntity>().Entities.AsNoTracking();

                // Apply Predicate (filtering)
                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply Ordering
                if (request.OrderByList.Any())
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<TEntity>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TEntity>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply Includes (eager loading)
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                // Apply Search Term
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = ApplySearchTerm(query, request.SearchTerm);
                }

                // Apply Select if provided, else return the entity as it is
                if (select is not null)
                    query = query.Select(select);

                return Task.FromResult(query.Adapt<IQueryable<TEntity>>());
            }
            catch (Exception)
            {
                // Return empty IQueryable<TEntity> if there's an exception
                return Task.FromResult(Enumerable.Empty<TEntity>().AsQueryable());
            }
        }

        private IQueryable<TEntity> ApplySearchTerm<TEntity>(IQueryable<TEntity> query, string searchTerm) where TEntity : class
        {
            // This method applies the search term based on the entity type
            if (typeof(TEntity) == typeof(Village))
            {
                var villageQuery = query as IQueryable<Village>;
                return (IQueryable<TEntity>)villageQuery.Where(v =>
                    EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.District.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.City.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Province.Name, $"%{searchTerm}%"));
            }
            else if (typeof(TEntity) == typeof(District))
            {
                var districtQuery = query as IQueryable<District>;
                return (IQueryable<TEntity>)districtQuery.Where(d => EF.Functions.Like(d.Name, $"%{searchTerm}%"));
            }
            else if (typeof(TEntity) == typeof(District))
            {
                var districtQuery = query as IQueryable<Occupational>;
                return (IQueryable<TEntity>)districtQuery.Where(v =>
                            EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                            EF.Functions.Like(v.Description, $"%{searchTerm}%"));
            }
            else if (typeof(TEntity) == typeof(District))
            {
                var districtQuery = query as IQueryable<Diagnosis>;
                return (IQueryable<TEntity>)districtQuery.Where(v =>
                           EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                           EF.Functions.Like(v.NameInd, $"%{searchTerm}%") ||
                           EF.Functions.Like(v.CronisCategory.Name, $"%{searchTerm}%") ||
                           EF.Functions.Like(v.Code, $"%{searchTerm}%"));
            }

            return query; // No filtering if the type doesn't match
        }

        #region GET

        public Task<IQueryable<OTPRequest>> Handle(GetOTPRequestQuerylable request, CancellationToken cancellationToken)
        {
            //return HandleQuery<GeneralConsultanService>(request, cancellationToken, request.Select is null ? x => new GeneralConsultanService
            return HandleQuery<OTPRequest>(request, cancellationToken, request.Select is null ? x => new OTPRequest
            {
                Id = x.Id,
            } : request.Select);
        }

        public async Task<List<OTPRequestDto>> Handle(BulkValidateOTPRequest request, CancellationToken cancellationToken)
        {
            var OTPRequestDtos = request.OTPRequestsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var OTPRequestNames = OTPRequestDtos.Select(x => x.Name).Distinct().ToList();
            //var Codes = OTPRequestDtos.Select(x => x.Code).Distinct().ToList();

            //var existingOTPRequests = await _unitOfWork.Repository<OTPRequest>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => OTPRequestNames.Contains(v.Name) && Codes.Contains(v.Code))
            //    .ToListAsync(cancellationToken);

            //return existingOTPRequests.Adapt<List<OTPRequestDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateOTPRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<OTPRequest>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<OTPRequestDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetOTPRequestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<OTPRequest>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<OTPRequest>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<OTPRequest>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //        EF.Functions.Like(v.OTPRequest.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new OTPRequest
                    {
                        Id = x.Id,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<OTPRequestDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<OTPRequestDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<OTPRequestDto> Handle(GetSingleOTPRequestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<OTPRequest>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<OTPRequest>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<OTPRequest>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //    EF.Functions.Like(v.OTPRequest.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new OTPRequest
                    {
                        Id = x.Id,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<OTPRequestDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<OTPRequestDto> Handle(CreateOTPRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<OTPRequest>().AddAsync(request.OTPRequestDto.Adapt<CreateUpdateOTPRequestDto>().Adapt<OTPRequest>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<OTPRequestDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OTPRequestDto>> Handle(CreateListOTPRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<OTPRequest>().AddAsync(request.OTPRequestDtos.Adapt<List<OTPRequest>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<List<OTPRequestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<OTPRequestDto> Handle(UpdateOTPRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<OTPRequest>().UpdateAsync(request.OTPRequestDto.Adapt<OTPRequestDto>().Adapt<OTPRequest>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<OTPRequestDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OTPRequestDto>> Handle(UpdateListOTPRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<OTPRequest>().UpdateAsync(request.OTPRequestDtos.Adapt<List<OTPRequest>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<List<OTPRequestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteOTPRequestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<OTPRequest>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<OTPRequest>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

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
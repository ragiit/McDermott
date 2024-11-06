using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramDetailCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class WellnessProgramDetailHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetWellnessProgramDetailQuery, (List<WellnessProgramDetailDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleWellnessProgramDetailQuery, WellnessProgramDetailDto>, IRequestHandler<ValidateWellnessProgramDetail, bool>,
     IRequestHandler<CreateWellnessProgramDetailRequest, WellnessProgramDetailDto>,
     IRequestHandler<BulkValidateWellnessProgramDetail, List<WellnessProgramDetailDto>>,
     IRequestHandler<CreateListWellnessProgramDetailRequest, List<WellnessProgramDetailDto>>,
     IRequestHandler<UpdateWellnessProgramDetailRequest, WellnessProgramDetailDto>,
     IRequestHandler<UpdateListWellnessProgramDetailRequest, List<WellnessProgramDetailDto>>,
     IRequestHandler<DeleteWellnessProgramDetailRequest, bool>
    {
        #region GET

        public async Task<List<WellnessProgramDetailDto>> Handle(BulkValidateWellnessProgramDetail request, CancellationToken cancellationToken)
        {
            var CountryDtos = request.WellnessProgramDetailsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var CountryNames = CountryDtos.Select(x => x.Name).Distinct().ToList();
            //var Codes = CountryDtos.Select(x => x.Code).Distinct().ToList();

            //var existingCountrys = await _unitOfWork.Repository<Country>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => CountryNames.Contains(v.Name) && Codes.Contains(v.Code))
            //    .ToListAsync(cancellationToken);

            //return existingCountrys.Adapt<List<CountryDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateWellnessProgramDetail request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<WellnessProgramDetail>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<WellnessProgramDetailDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetWellnessProgramDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgramDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgramDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgramDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //        EF.Functions.Like(v.WellnessProgramDetail.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgramDetail
                    {
                        Id = x.Id,
                        WellnessProgramId = x.WellnessProgramId,
                        Name = x.Name,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Notes = x.Notes,
                        Slug = x.Slug
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<WellnessProgramDetailDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<WellnessProgramDetailDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<WellnessProgramDetailDto> Handle(GetSingleWellnessProgramDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgramDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgramDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgramDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //    EF.Functions.Like(v.WellnessProgramDetail.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgramDetail
                    {
                        Id = x.Id,
                        WellnessProgramId = x.WellnessProgramId,
                        Name = x.Name,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Notes = x.Notes,
                        Slug = x.Slug
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<WellnessProgramDetailDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<WellnessProgramDetailDto> Handle(CreateWellnessProgramDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramDetail>().AddAsync(request.WellnessProgramDetailDto.Adapt<CreateUpdateWellnessProgramDetailDto>().Adapt<WellnessProgramDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramDetailDto>> Handle(CreateListWellnessProgramDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramDetail>().AddAsync(request.WellnessProgramDetailDtos.Adapt<List<WellnessProgramDetail>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<WellnessProgramDetailDto> Handle(UpdateWellnessProgramDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramDetail>().UpdateAsync(request.WellnessProgramDetailDto.Adapt<WellnessProgramDetailDto>().Adapt<WellnessProgramDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramDetailDto>> Handle(UpdateListWellnessProgramDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramDetail>().UpdateAsync(request.WellnessProgramDetailDtos.Adapt<List<WellnessProgramDetail>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteWellnessProgramDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<WellnessProgramDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<WellnessProgramDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramDetailQuery_"); // Ganti dengan key yang sesuai

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
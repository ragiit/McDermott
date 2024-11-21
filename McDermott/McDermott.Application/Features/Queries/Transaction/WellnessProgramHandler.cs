using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class WellnessProgramHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetWellnessProgramQuery, (List<WellnessProgramDto>, int pageIndex, int pageSize, int pageCount)>,
         IRequestHandler<GetSingleWellnessProgramQuery, WellnessProgramDto>,
         IRequestHandler<ValidateWellnessProgram, bool>,
         IRequestHandler<CreateWellnessProgramRequest, WellnessProgramDto>,
         IRequestHandler<BulkValidateWellnessProgram, List<WellnessProgramDto>>,
         IRequestHandler<CreateListWellnessProgramRequest, List<WellnessProgramDto>>,
         IRequestHandler<UpdateWellnessProgramRequest, WellnessProgramDto>,
         IRequestHandler<CancelWellnessProgramRequest, WellnessProgramDto>,
         IRequestHandler<UpdateListWellnessProgramRequest, List<WellnessProgramDto>>,
         IRequestHandler<DeleteWellnessProgramRequest, bool>
    {
        #region GET

        public async Task<List<WellnessProgramDto>> Handle(BulkValidateWellnessProgram request, CancellationToken cancellationToken)
        {
            var CountryDtos = request.WellnessProgramsToValidate;

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

        public async Task<bool> Handle(ValidateWellnessProgram request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<WellnessProgram>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<WellnessProgramDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetWellnessProgramQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgram>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgram>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgram>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //        EF.Functions.Like(v.WellnessProgram.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgram
                    {
                        Id = x.Id,
                        Name = x.Name,
                        AwarenessEduCategoryId = x.AwarenessEduCategoryId,
                        AwarenessEduCategory = new AwarenessEduCategory
                        {
                            Name = x.AwarenessEduCategory == null ? "" : x.AwarenessEduCategory.Name
                        },
                        Content = x.Content,
                        Status = x.Status,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
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

                    return (pagedItems.Adapt<List<WellnessProgramDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<WellnessProgramDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<WellnessProgramDto> Handle(GetSingleWellnessProgramQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgram>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgram>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgram>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //    EF.Functions.Like(v.WellnessProgram.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgram
                    {
                        Id = x.Id,
                        Name = x.Name,
                        AwarenessEduCategoryId = x.AwarenessEduCategoryId,
                        Content = x.Content,
                        Status = x.Status,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Slug = x.Slug
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<WellnessProgramDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<WellnessProgramDto> Handle(CreateWellnessProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgram>().AddAsync(request.WellnessProgramDto.Adapt<CreateUpdateWellnessProgramDto>().Adapt<WellnessProgram>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramDto>> Handle(CreateListWellnessProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgram>().AddAsync(request.WellnessProgramDtos.Adapt<List<WellnessProgram>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<WellnessProgramDto> Handle(CancelWellnessProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the entity from the database, or you can attach it directly if the ID is known
                var entity = new WellnessProgram { Id = request.WellnessProgramDto.Id };

                // Attach the entity to the context
                _unitOfWork.Repository<WellnessProgram>().Attach(entity);

                // Only update the specific fields you need
                entity.Status = request.WellnessProgramDto.Status;

                // Mark specific properties as modified
                _unitOfWork.Repository<WellnessProgram>().SetPropertyModified(entity, nameof(entity.Status));

                // Save changes
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Clear cache if needed
                _cache.Remove("GetWellnessProgramQuery_");

                return entity.Adapt<WellnessProgramDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WellnessProgramDto> Handle(UpdateWellnessProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgram>().UpdateAsync(request.WellnessProgramDto.Adapt<WellnessProgramDto>().Adapt<WellnessProgram>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramDto>> Handle(UpdateListWellnessProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgram>().UpdateAsync(request.WellnessProgramDtos.Adapt<List<WellnessProgram>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteWellnessProgramRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<WellnessProgram>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<WellnessProgram>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramQuery_"); // Ganti dengan key yang sesuai

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
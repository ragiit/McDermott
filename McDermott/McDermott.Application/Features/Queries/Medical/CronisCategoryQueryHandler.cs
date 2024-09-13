using static McDermott.Application.Features.Commands.Medical.CronisCategoryCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class CronisCategoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCronisCategoryQuery, (List<CronisCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateCronisCategoryRequest, CronisCategoryDto>,
        IRequestHandler<CreateListCronisCategoryRequest, List<CronisCategoryDto>>,
        IRequestHandler<UpdateCronisCategoryRequest, CronisCategoryDto>,
        IRequestHandler<UpdateListCronisCategoryRequest, List<CronisCategoryDto>>,
        IRequestHandler<DeleteCronisCategoryRequest, bool>
    {
        #region GET

        public async Task<(List<CronisCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCronisCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<CronisCategory>().Entities
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Description, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<CronisCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateCronisCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<CronisCategory>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<CronisCategoryDto> Handle(CreateCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().AddAsync(request.CronisCategoryDto.Adapt<CronisCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<CronisCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CronisCategoryDto>> Handle(CreateListCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().AddAsync(request.CronisCategoryDtos.Adapt<List<CronisCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<List<CronisCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CronisCategoryDto> Handle(UpdateCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().UpdateAsync(request.CronisCategoryDto.Adapt<CronisCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<CronisCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CronisCategoryDto>> Handle(UpdateListCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().UpdateAsync(request.CronisCategoryDtos.Adapt<List<CronisCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<List<CronisCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<CronisCategory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<CronisCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

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
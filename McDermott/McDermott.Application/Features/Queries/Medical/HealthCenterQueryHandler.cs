using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Medical
{
    public class HealthCenterQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetHealthCenterQuery, (List<HealthCenterDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateHealthCenterRequest, HealthCenterDto>,
        IRequestHandler<CreateListHealthCenterRequest, List<HealthCenterDto>>,
        IRequestHandler<UpdateHealthCenterRequest, HealthCenterDto>,
        IRequestHandler<UpdateListHealthCenterRequest, List<HealthCenterDto>>,
        IRequestHandler<DeleteHealthCenterRequest, bool>
    {
        #region GET

        public async Task<(List<HealthCenterDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetHealthCenterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<HealthCenter>().Entities.AsNoTracking();

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Type, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Phone, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Mobile, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Email, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.WebsiteLink, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Street1, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Street2, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                                  query,
                                  request.PageSize,
                                  request.PageIndex,
                                  q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<HealthCenterDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateHealthCenterQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<HealthCenter>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<HealthCenterDto> Handle(CreateHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<HealthCenter>().AddAsync(request.HealthCenterDto.Adapt<HealthCenter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

                return result.Adapt<HealthCenterDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<HealthCenterDto>> Handle(CreateListHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<HealthCenter>().AddAsync(request.HealthCenterDtos.Adapt<List<HealthCenter>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

                return result.Adapt<List<HealthCenterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<HealthCenterDto> Handle(UpdateHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<HealthCenter>().UpdateAsync(request.HealthCenterDto.Adapt<HealthCenter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

                return result.Adapt<HealthCenterDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<HealthCenterDto>> Handle(UpdateListHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<HealthCenter>().UpdateAsync(request.HealthCenterDtos.Adapt<List<HealthCenter>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

                return result.Adapt<List<HealthCenterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<HealthCenter>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<HealthCenter>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

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
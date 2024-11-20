using McDermott.Application.Dtos.Pharmacies;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Pharmacies.ActiveComponentCommand;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class ActiveComponentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetActiveComponentQuery, (List<ActiveComponentDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateActiveComponentQuery, bool>,
        IRequestHandler<BulkValidateActiveComponentQuery, List<ActiveComponentDto>>,
        IRequestHandler<CreateActiveComponentRequest, ActiveComponentDto>,
        IRequestHandler<CreateListActiveComponentRequest, List<ActiveComponentDto>>,
        IRequestHandler<UpdateActiveComponentRequest, ActiveComponentDto>,
        IRequestHandler<UpdateListActiveComponentRequest, List<ActiveComponentDto>>,
        IRequestHandler<DeleteActiveComponentRequest, bool>
    {
        #region GET

        public async Task<List<ActiveComponentDto>> Handle(BulkValidateActiveComponentQuery request, CancellationToken cancellationToken)
        {
            var ActiveComponentDtos = request.ActiveComponentsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ActiveComponentNames = ActiveComponentDtos.Select(x => x.Name).Distinct().ToList();
            var a = ActiveComponentDtos.Select(x => x.AmountOfComponent).Distinct().ToList();
            var b = ActiveComponentDtos.Select(x => x.UomId).Distinct().ToList();

            var existingActiveComponents = await _unitOfWork.Repository<ActiveComponent>()
                .Entities
                .AsNoTracking()
                .Where(v => ActiveComponentNames.Contains(v.Name)
                            && a.Contains(v.AmountOfComponent)
                            && b.Contains(v.UomId)
                            )
                .ToListAsync(cancellationToken);

            return existingActiveComponents.Adapt<List<ActiveComponentDto>>();
        }

        public async Task<(List<ActiveComponentDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetActiveComponentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ActiveComponent>().Entities.AsNoTracking();

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
                        v.AmountOfComponent.Equals(request.SearchTerm) ||
                        EF.Functions.Like(v.Uom.Name, $"%{request.SearchTerm}%")
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

                return (pagedItems.Adapt<List<ActiveComponentDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateActiveComponentQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<ActiveComponent>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<ActiveComponentDto> Handle(CreateActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ActiveComponent>().AddAsync(request.ActiveComponentDto.Adapt<ActiveComponent>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ActiveComponentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ActiveComponentDto>> Handle(CreateListActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ActiveComponent>().AddAsync(request.ActiveComponentDtos.Adapt<List<ActiveComponent>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ActiveComponentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ActiveComponentDto> Handle(UpdateActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ActiveComponent>().UpdateAsync(request.ActiveComponentDto.Adapt<ActiveComponent>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ActiveComponentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ActiveComponentDto>> Handle(UpdateListActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ActiveComponent>().UpdateAsync(request.ActiveComponentDtos.Adapt<List<ActiveComponent>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ActiveComponentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ActiveComponent>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ActiveComponent>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetUomQuery_");

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
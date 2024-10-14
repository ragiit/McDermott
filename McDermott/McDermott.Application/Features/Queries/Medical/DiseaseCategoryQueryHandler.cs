using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.CountryCommand;
using static McDermott.Application.Features.Commands.Medical.DiseaseCategoryCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DiseaseCategoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDiseaseCategoryQuery, (List<DiseaseCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateDiseaseCategoryRequest, DiseaseCategoryDto>,
        IRequestHandler<BulkValidateDiseaseCategoryQuery, List<DiseaseCategoryDto>>,
        IRequestHandler<CreateListDiseaseCategoryRequest, List<DiseaseCategoryDto>>,
        IRequestHandler<UpdateDiseaseCategoryRequest, DiseaseCategoryDto>,
        IRequestHandler<UpdateListDiseaseCategoryRequest, List<DiseaseCategoryDto>>,
        IRequestHandler<DeleteDiseaseCategoryRequest, bool>
    {
        #region GET

        public async Task<List<DiseaseCategoryDto>> Handle(BulkValidateDiseaseCategoryQuery request, CancellationToken cancellationToken)
        {
            var DiseaseCategoryDtos = request.DiseaseCategorysToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DiseaseCategoryNames = DiseaseCategoryDtos.Select(x => x.Name).Distinct().ToList();
            var a = DiseaseCategoryDtos.Select(x => x.ParentDiseaseCategoryId).Distinct().ToList();

            var existingDiseaseCategorys = await _unitOfWork.Repository<DiseaseCategory>()
                .Entities
                .AsNoTracking()
                .Where(v => DiseaseCategoryNames.Contains(v.Name)
                            && a.Contains(v.ParentDiseaseCategoryId))
                .ToListAsync(cancellationToken);

            return existingDiseaseCategorys.Adapt<List<DiseaseCategoryDto>>();
        }

        public async Task<(List<DiseaseCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDiseaseCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DiseaseCategory>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.ParentDiseaseCategory.Name, $"%{request.SearchTerm}%")
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

                return (pagedItems.Adapt<List<DiseaseCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDiseaseCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DiseaseCategory>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<DiseaseCategoryDto> Handle(CreateDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().AddAsync(request.DiseaseCategoryDto.Adapt<CreateUpdateDiseaseCategoryDto>().Adapt<DiseaseCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DiseaseCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DiseaseCategoryDto>> Handle(CreateListDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().AddAsync(request.DiseaseCategoryDtos.Adapt<List<DiseaseCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DiseaseCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DiseaseCategoryDto> Handle(UpdateDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().UpdateAsync(request.DiseaseCategoryDto.Adapt<DiseaseCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DiseaseCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DiseaseCategoryDto>> Handle(UpdateListDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().UpdateAsync(request.DiseaseCategoryDtos.Adapt<List<DiseaseCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DiseaseCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DiseaseCategory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DiseaseCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

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
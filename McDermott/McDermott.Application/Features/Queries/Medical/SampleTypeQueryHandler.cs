using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Medical
{
    public class SampleTypeQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetSampleTypeQuery, (List<SampleTypeDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateSampleTypeRequest, SampleTypeDto>,
        IRequestHandler<BulkValidateSampleTypeQuery, List<SampleTypeDto>>,
        IRequestHandler<CreateListSampleTypeRequest, List<SampleTypeDto>>,
        IRequestHandler<UpdateSampleTypeRequest, SampleTypeDto>,
        IRequestHandler<UpdateListSampleTypeRequest, List<SampleTypeDto>>,
        IRequestHandler<DeleteSampleTypeRequest, bool>
    {
        #region GET

        public async Task<List<SampleTypeDto>> Handle(BulkValidateSampleTypeQuery request, CancellationToken cancellationToken)
        {
            var SampleTypeDtos = request.SampleTypesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var SampleTypeNames = SampleTypeDtos.Select(x => x.Name).Distinct().ToList();

            var existingSampleTypes = await _unitOfWork.Repository<SampleType>()
                .Entities
                .AsNoTracking()
                .Where(v => SampleTypeNames.Contains(v.Name))
                .ToListAsync(cancellationToken);

            return existingSampleTypes.Adapt<List<SampleTypeDto>>();
        }

        public async Task<(List<SampleTypeDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetSampleTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<SampleType>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.Description, $"%{request.SearchTerm}%")
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

                return (pagedItems.Adapt<List<SampleTypeDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateSampleTypeQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<SampleType>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<SampleTypeDto> Handle(CreateSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SampleType>().AddAsync(request.SampleTypeDto.Adapt<SampleType>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SampleTypeDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SampleTypeDto>> Handle(CreateListSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SampleType>().AddAsync(request.SampleTypeDtos.Adapt<List<SampleType>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SampleTypeDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SampleTypeDto> Handle(UpdateSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SampleType>().UpdateAsync(request.SampleTypeDto.Adapt<SampleType>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SampleTypeDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SampleTypeDto>> Handle(UpdateListSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<SampleType>().UpdateAsync(request.SampleTypeDtos.Adapt<List<SampleType>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SampleTypeDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSampleTypeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<SampleType>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<SampleType>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSampleTypeQuery_"); // Ganti dengan key yang sesuai

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
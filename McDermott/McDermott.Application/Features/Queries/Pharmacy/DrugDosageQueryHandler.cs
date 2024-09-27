namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class DrugDosageQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDrugDosageQuery, (List<DrugDosageDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetAllDrugDosageQuery, List<DrugDosageDto>>,
        IRequestHandler<BulkValidateDrugDosageQuery, List<DrugDosageDto>>,
        IRequestHandler<ValidateDrugDosageQuery, bool>,
        IRequestHandler<CreateDrugDosageRequest, DrugDosageDto>,
        IRequestHandler<CreateListDrugDosageRequest, List<DrugDosageDto>>,
        IRequestHandler<UpdateDrugDosageRequest, DrugDosageDto>,
        IRequestHandler<UpdateListDrugDosageRequest, List<DrugDosageDto>>,
        IRequestHandler<DeleteDrugDosageRequest, bool>
    {
        #region GET

        public async Task<List<DrugDosageDto>> Handle(GetAllDrugDosageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDrugDosageQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<DrugDosage>? result))
                {
                    result = await _unitOfWork.Repository<DrugDosage>().Entities
                        .Include(x => x.DrugRoute)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DrugDosageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<DrugDosageDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDrugDosageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DrugDosage>().Entities
                    .Include(x => x.DrugRoute)
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Frequency, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.Frequency);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<DrugDosageDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugDosageDto>> Handle(BulkValidateDrugDosageQuery request, CancellationToken cancellationToken)
        {
            var DrugDosages = request.DrugDosageToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var A = DrugDosages.Select(x => x.Frequency).Distinct().ToList();
            var B = DrugDosages.Select(x => x.TotalQtyPerDay).Distinct().ToList();
            var C = DrugDosages.Select(x => x.DrugRouteId).Distinct().ToList();
            var D = DrugDosages.Select(x => x.Days).Distinct().ToList();

            var existingLabTests = await _unitOfWork.Repository<DrugDosage>()
                .Entities
                .AsNoTracking()
                .Where(v => A.Contains(v.Frequency)
                            && B.Contains(v.TotalQtyPerDay)                            
                            && C.Contains(v.DrugRouteId)
                            && D.Contains(v.Days)
                            )
                .ToListAsync(cancellationToken);

            return existingLabTests.Adapt<List<DrugDosageDto>>();
        }
        public async Task<bool> Handle(ValidateDrugDosageQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DrugDosage>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<DrugDosageDto> Handle(CreateDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().AddAsync(request.DrugDosageDto.Adapt<DrugDosage>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

                return result.Adapt<DrugDosageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugDosageDto>> Handle(CreateListDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().AddAsync(request.DrugDosageDtos.Adapt<List<DrugDosage>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DrugDosageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DrugDosageDto> Handle(UpdateDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().UpdateAsync(request.DrugDosageDto.Adapt<DrugDosage>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DrugDosageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugDosageDto>> Handle(UpdateListDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().UpdateAsync(request.DrugDosageDtos.Adapt<List<DrugDosage>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

                return result.Adapt<List<DrugDosageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DrugDosage>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DrugDosage>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

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

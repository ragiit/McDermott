using static McDermott.Application.Features.Commands.Medical.LabTestDetailCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class LabTestDetailQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllLabTestDetailQuery, List<LabTestDetailDto>>,
        IRequestHandler<GetLabTestDetailQuery, (List<LabTestDetailDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateLabTestDetailRequest, LabTestDetailDto>,
        IRequestHandler<CreateListLabTestDetailRequest, List<LabTestDetailDto>>,
        IRequestHandler<UpdateLabTestDetailRequest, LabTestDetailDto>,
        IRequestHandler<UpdateListLabTestDetailRequest, List<LabTestDetailDto>>,
        IRequestHandler<DeleteLabTestDetailRequest, bool>
    {
        #region GET

        public async Task<List<LabTestDetailDto>> Handle(GetAllLabTestDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllLabTestDetailQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<LabTestDetail>? result))
                {
                    result = await _unitOfWork.Repository<LabTestDetail>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.LabTest)
                        .Include(x => x.LabUom),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<LabTestDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<LabTestDetailDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLabTestDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<LabTestDetail>().Entities
                    .Include(x=>x.LabUom)
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.ResultType, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<LabTestDetailDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateLabTestDetailQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<LabTestDetail>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<LabTestDetailDto> Handle(CreateLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTestDetail>().AddAsync(request.LabTestDetailDto.Adapt<LabTestDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabTestDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabTestDetailDto>> Handle(CreateListLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTestDetail>().AddAsync(request.LabTestDetailDtos.Adapt<List<LabTestDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabTestDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabTestDetailDto> Handle(UpdateLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTestDetail>().UpdateAsync(request.LabTestDetailDto.Adapt<LabTestDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabTestDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabTestDetailDto>> Handle(UpdateListLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTestDetail>().UpdateAsync(request.LabTestDetailDtos.Adapt<List<LabTestDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabTestDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabTestDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<LabTestDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<LabTestDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestDetailQuery_"); // Ganti dengan key yang sesuai

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
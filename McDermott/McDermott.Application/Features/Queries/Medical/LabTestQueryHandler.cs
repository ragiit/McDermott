namespace McDermott.Application.Features.Queries.Medical
{
    public class LabTestQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLabTestQuery, List<LabTestDto>>,
        IRequestHandler<CreateLabTestRequest, LabTestDto>,
        IRequestHandler<CreateListLabTestRequest, List<LabTestDto>>,
        IRequestHandler<UpdateLabTestRequest, LabTestDto>,
        IRequestHandler<UpdateListLabTestRequest, List<LabTestDto>>,
        IRequestHandler<DeleteLabTestRequest, bool>
    {
        #region GET

        public async Task<List<LabTestDto>> Handle(GetLabTestQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetLabTestQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<LabTest>? result))
                {
                    result = await _unitOfWork.Repository<LabTest>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.SampleType)
                        .Include(x => x.LabUom),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<LabTestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<LabTestDto> Handle(CreateLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTest>().AddAsync(request.LabTestDto.Adapt<LabTest>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabTestDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabTestDto>> Handle(CreateListLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTest>().AddAsync(request.LabTestDtos.Adapt<List<LabTest>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabTestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabTestDto> Handle(UpdateLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTest>().UpdateAsync(request.LabTestDto.Adapt<LabTest>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabTestDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabTestDto>> Handle(UpdateListLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabTest>().UpdateAsync(request.LabTestDtos.Adapt<List<LabTest>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabTestDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabTestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<LabTest>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<LabTest>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabTestQuery_"); // Ganti dengan key yang sesuai

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

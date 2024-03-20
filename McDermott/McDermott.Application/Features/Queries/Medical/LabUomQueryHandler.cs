namespace McDermott.Application.Features.Queries.Medical
{
    public class LabUomQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLabUomQuery, List<LabUomDto>>,
        IRequestHandler<CreateLabUomRequest, LabUomDto>,
        IRequestHandler<CreateListLabUomRequest, List<LabUomDto>>,
        IRequestHandler<UpdateLabUomRequest, LabUomDto>,
        IRequestHandler<UpdateListLabUomRequest, List<LabUomDto>>,
        IRequestHandler<DeleteLabUomRequest, bool>
    {
        #region GET

        public async Task<List<LabUomDto>> Handle(GetLabUomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetLabUomQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<LabUom>? result))
                {
                    result = await _unitOfWork.Repository<LabUom>().GetAsync(
                        null,
                        null,
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<LabUomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<LabUomDto> Handle(CreateLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabUom>().AddAsync(request.LabUomDto.Adapt<LabUom>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabUomDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabUomDto>> Handle(CreateListLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabUom>().AddAsync(request.LabUomDtos.Adapt<List<LabUom>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabUomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabUomDto> Handle(UpdateLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabUom>().UpdateAsync(request.LabUomDto.Adapt<LabUom>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabUomDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabUomDto>> Handle(UpdateListLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabUom>().UpdateAsync(request.LabUomDtos.Adapt<List<LabUom>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabUomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<LabUom>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<LabUom>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

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

namespace McDermott.Application.Features.Queries.Config
{
    public class GenderQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGenderQuery, List<GenderDto>>,
        IRequestHandler<CreateGenderRequest, GenderDto>,
        IRequestHandler<CreateListGenderRequest, List<GenderDto>>,
        IRequestHandler<UpdateGenderRequest, GenderDto>,
        IRequestHandler<UpdateListGenderRequest, List<GenderDto>>,
        IRequestHandler<DeleteGenderRequest, bool>
    {
        #region GET

        public async Task<List<GenderDto>> Handle(GetGenderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGenderQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Gender>? result))
                {
                    result = await _unitOfWork.Repository<Gender>().GetAsync(
                        null,
                        null,
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromHours(24)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GenderDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GenderDto> Handle(CreateGenderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Gender>().AddAsync(request.GenderDto.Adapt<Gender>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGenderQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GenderDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GenderDto>> Handle(CreateListGenderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Gender>().AddAsync(request.GenderDtos.Adapt<List<Gender>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGenderQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GenderDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GenderDto> Handle(UpdateGenderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Gender>().UpdateAsync(request.GenderDto.Adapt<Gender>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGenderQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GenderDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GenderDto>> Handle(UpdateListGenderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Gender>().UpdateAsync(request.GenderDtos.Adapt<List<Gender>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGenderQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GenderDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGenderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Gender>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Gender>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGenderQuery_"); // Ganti dengan key yang sesuai

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
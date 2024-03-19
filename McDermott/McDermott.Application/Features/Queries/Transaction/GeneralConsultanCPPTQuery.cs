namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanCPPTQuery(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralConsultanCPPTQuery, List<GeneralConsultanCPPTDto>>,
        IRequestHandler<CreateGeneralConsultanCPPTRequest, GeneralConsultanCPPTDto>,
        IRequestHandler<CreateListGeneralConsultanCPPTRequest, List<GeneralConsultanCPPTDto>>,
        IRequestHandler<UpdateGeneralConsultanCPPTRequest, GeneralConsultanCPPTDto>,
        IRequestHandler<UpdateListGeneralConsultanCPPTRequest, List<GeneralConsultanCPPTDto>>,
        IRequestHandler<DeleteGeneralConsultanCPPTRequest, bool>
    {

        #region GET
        public async Task<List<GeneralConsultanCPPTDto>> Handle(GetGeneralConsultanCPPTQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultanCPPTQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultanCPPT>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultanCPPT>().GetAsync(
                        null,
                        x => x.Include(z => z.GeneralConsultanService),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultanCPPTDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CREATE
        public async Task<GeneralConsultanCPPTDto> Handle(CreateGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().AddAsync(request.GeneralConsultanCPPTDto.Adapt<GeneralConsultanCPPT>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<GeneralConsultanCPPTDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanCPPTDto>> Handle(CreateListGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().AddAsync(request.GeneralConsultanCPPTDtos.Adapt<List<GeneralConsultanCPPT>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanCPPTDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UPDATE
        public async Task<GeneralConsultanCPPTDto> Handle(UpdateGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().UpdateAsync(request.GeneralConsultanCPPTDto.Adapt<GeneralConsultanCPPT>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanCPPTDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanCPPTDto>> Handle(UpdateListGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanCPPT>().UpdateAsync(request.GeneralConsultanCPPTDtos.Adapt<List<GeneralConsultanCPPT>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanCPPTDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DELETE
        public async Task<bool> Handle(DeleteGeneralConsultanCPPTRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanCPPT>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanCPPT>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanCPPTQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
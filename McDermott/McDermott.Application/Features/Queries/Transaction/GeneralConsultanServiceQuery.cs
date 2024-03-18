namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanServiceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralConsultanServiceQuery, List<GeneralConsultanServiceDto>>,
        IRequestHandler<CreateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<CreateListGeneralConsultanServiceRequest, List<GeneralConsultanServiceDto>>,
        IRequestHandler<UpdateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<DeleteGeneralConsultanServiceRequest, bool>
    {
        #region GET

        public async Task<List<GeneralConsultanServiceDto>> Handle(GetGeneralConsultanServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultanServiceQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultanService>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultanService>().GetAsync(
                        null,
                        x => x.Include(z => z.Service)
                             .Include(z => z.Insurance)
                             .Include(z => z.Patient)
                             .Include(z => z.Pratitioner)
                             .Include(z => z.ClassType),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultanServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanServiceDto> Handle(CreateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(request.GeneralConsultanServiceDto.Adapt<GeneralConsultanService>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanServiceDto>> Handle(CreateListGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(request.GeneralConsultanServiceDtos.Adapt<List<GeneralConsultanService>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return result.Adapt<List<GeneralConsultanServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region Update

        public async Task<GeneralConsultanServiceDto> Handle(UpdateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanService>().UpdateAsync(request.GeneralConsultanServiceDto.Adapt<GeneralConsultanService>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update

        #region Delete

        public async Task<bool> Handle(DeleteGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Delete
    }
}
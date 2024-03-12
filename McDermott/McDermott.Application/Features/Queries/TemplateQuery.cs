using static McDermott.Application.Features.Commands.TemplateCommand;

namespace McDermott.Application.Features.Queries
{
    public class TemplateQuery(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetTemplateQuery, List<ProvinceDto>>,
        IRequestHandler<CreateTemplateRequest, ProvinceDto>,
        IRequestHandler<CreateListTemplateRequest, List<ProvinceDto>>,
        IRequestHandler<UpdateTemplateRequest, ProvinceDto>,
        IRequestHandler<UpdateListTemplateRequest, List<ProvinceDto>>,
        IRequestHandler<DeleteTemplateRequest, bool>
    {

        #region GET
        public async Task<List<ProvinceDto>> Handle(GetTemplateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetTemplateQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Province>? result))
                {
                    result = await _unitOfWork.Repository<Province>().GetAsync(
                        request.Predicate,
                        x => x.Include(z => z.Country),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ProvinceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CREATE
        public async Task<ProvinceDto> Handle(CreateTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().AddAsync(request.TemplateDto.Adapt<Province>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTemplateQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<ProvinceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProvinceDto>> Handle(CreateListTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().AddAsync(request.TemplateDtos.Adapt<List<Province>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTemplateQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProvinceDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UPDATE
        public async Task<ProvinceDto> Handle(UpdateTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().UpdateAsync(request.TemplateDto.Adapt<Province>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTemplateQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProvinceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProvinceDto>> Handle(UpdateListTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Province>().UpdateAsync(request.TemplateDtos.Adapt<List<Province>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTemplateQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProvinceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DELETE
        public async Task<bool> Handle(DeleteTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Province>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Province>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTemplateQuery_"); // Ganti dengan key yang sesuai

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

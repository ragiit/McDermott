using static McDermott.Application.Features.Commands.Medical.ServiceCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class ServiceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetServiceQuery, List<ServiceDto>>,
        IRequestHandler<CreateServiceRequest, ServiceDto>,
        IRequestHandler<CreateListServiceRequest, List<ServiceDto>>,
        IRequestHandler<UpdateServiceRequest, ServiceDto>,
        IRequestHandler<UpdateListServiceRequest, List<ServiceDto>>,
        IRequestHandler<DeleteServiceRequest, bool>
    {
        #region GET

        public async Task<List<ServiceDto>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetServiceQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Service>? result))
                {
                    result = await _unitOfWork.Repository<Service>().Entities
                        .Include(x => x.Serviced)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ServiceDto> Handle(CreateServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Service>().AddAsync(request.ServiceDto.Adapt<Service>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceDto>> Handle(CreateListServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Service>().AddAsync(request.ServiceDtos.Adapt<List<Service>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ServiceDto> Handle(UpdateServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Service>().UpdateAsync(request.ServiceDto.Adapt<Service>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceDto>> Handle(UpdateListServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Service>().UpdateAsync(request.ServiceDtos.Adapt<List<Service>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Service>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Service>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetServiceQuery_"); // Ganti dengan key yang sesuai

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
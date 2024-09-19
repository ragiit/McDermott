using static McDermott.Application.Features.Commands.Config.CountryCommand;
using static McDermott.Application.Features.Commands.Medical.ServiceCommand;

using static McDermott.Application.Features.Commands.Medical.ServiceCommand;
using static McDermott.Application.Features.Commands.Medical.ServiceCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class ServiceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetServiceQuery, (List<ServiceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateServiceRequest, ServiceDto>,
IRequestHandler<BulkValidateServiceQuery, List<ServiceDto>>,
        IRequestHandler<CreateListServiceRequest, List<ServiceDto>>,
        IRequestHandler<UpdateServiceRequest, ServiceDto>,
        IRequestHandler<UpdateListServiceRequest, List<ServiceDto>>,
        IRequestHandler<DeleteServiceRequest, bool>
    {
        #region GET

        public async Task<List<ServiceDto>> Handle(BulkValidateServiceQuery request, CancellationToken cancellationToken)
        {
            var ServiceDtos = request.ServicesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ServiceNames = ServiceDtos.Select(x => x.Name).Distinct().ToList();
            var B = ServiceDtos.Select(x => x.Code).Distinct().ToList();
            var C = ServiceDtos.Select(x => x.Quota).Distinct().ToList();

            var existingServices = await _unitOfWork.Repository<Service>()
                .Entities
                .AsNoTracking()
                .Where(v => ServiceNames.Contains(v.Name)
                            && B.Contains(v.Code)
                            && C.Contains(v.Quota))
                .ToListAsync(cancellationToken);

            return existingServices.Adapt<List<ServiceDto>>();
        }

        public async Task<(List<ServiceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Service>().Entities
                    .Include(x => x.Serviced)
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<ServiceDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateServiceQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Service>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
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
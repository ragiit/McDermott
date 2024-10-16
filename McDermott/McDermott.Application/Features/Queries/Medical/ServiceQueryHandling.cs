using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Medical.ServiceCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class ServiceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetServiceQuery, (List<ServiceDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleServiceQuery, ServiceDto>,
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
                var query = _unitOfWork.Repository<Service>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<Service>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Service>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                            EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Service
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Quota = x.Quota,
                        IsKiosk = x.IsKiosk,
                        IsMcu = x.IsMcu,
                        IsPatient = x.IsPatient,
                        IsTelemedicine = x.IsTelemedicine,
                        IsVaccination = x.IsVaccination,
                        ServicedId = x.ServicedId,
                        Serviced = new Service
                        {
                            Name = x.Serviced.Name
                        }
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<ServiceDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<ServiceDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<ServiceDto> Handle(GetSingleServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Service>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<Service>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Service>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Service
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Quota = x.Quota,
                        IsKiosk = x.IsKiosk,
                        IsMcu = x.IsMcu,
                        IsPatient = x.IsPatient,
                        IsTelemedicine = x.IsTelemedicine,
                        IsVaccination = x.IsVaccination,
                        ServicedId = x.ServicedId,
                        Serviced = new Service
                        {
                            Name = x.Serviced.Name
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ServiceDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
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
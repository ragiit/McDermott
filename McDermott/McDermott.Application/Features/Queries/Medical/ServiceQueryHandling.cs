using McDermott.Application.Dtos.Medical;
using static McDermott.Application.Features.Commands.Medical.ServiceCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class ServiceQueryHandler
    {
        internal class GetAllServiceQueryHandler : IRequestHandler<GetServiceQuery, List<ServiceDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllServiceQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<ServiceDto>> Handle(GetServiceQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Service>().Entities
                        .Include(x => x.Serviced)
                        .Select(Service => Service.Adapt<ServiceDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetServiceByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Service>().GetByIdAsync(request.Id);

                return result.Adapt<ServiceDto>();
            }
        }

        internal class CreateServiceHandler : IRequestHandler<CreateServiceRequest, ServiceDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateServiceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ServiceDto> Handle(CreateServiceRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Service>().AddAsync(request.ServiceDto.Adapt<Service>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<ServiceDto>();
            }
        }

        internal class UpdateServiceHandler : IRequestHandler<UpdateServiceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateServiceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateServiceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Service>().UpdateAsync(request.ServiceDto.Adapt<Service>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteServiceHandler : IRequestHandler<DeleteServiceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteServiceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteServiceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Service>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListServiceHandler : IRequestHandler<DeleteListServiceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListServiceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListServiceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Service>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
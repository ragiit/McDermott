using static McDermott.Application.Features.Commands.HealthCenterCommand;

namespace McDermott.Application.Features.Queries
{
    public class HealthCenterQueryHandler
    {
        internal class GetAllHealthCenterQueryHandler : IRequestHandler<GetHealthCenterQuery, List<HealthCenterDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllHealthCenterQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<HealthCenterDto>> Handle(GetHealthCenterQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<HealthCenter>().Entities
                        .Select(HealthCenter => HealthCenter.Adapt<HealthCenterDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetHealthCenterByIdQueryHandler : IRequestHandler<GetHealthCenterByIdQuery, HealthCenterDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetHealthCenterByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<HealthCenterDto> Handle(GetHealthCenterByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<HealthCenter>().GetByIdAsync(request.Id);

                return result.Adapt<HealthCenterDto>();
            }
        }

        internal class CreateHealthCenterHandler : IRequestHandler<CreateHealthCenterRequest, HealthCenterDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateHealthCenterHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<HealthCenterDto> Handle(CreateHealthCenterRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<HealthCenter>().AddAsync(request.HealthCenterDto.Adapt<HealthCenter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<HealthCenterDto>();
            }
        }

        internal class UpdateHealthCenterHandler : IRequestHandler<UpdateHealthCenterRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateHealthCenterHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateHealthCenterRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<HealthCenter>().UpdateAsync(request.HealthCenterDto.Adapt<HealthCenter>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteHealthCenterHandler : IRequestHandler<DeleteHealthCenterRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteHealthCenterHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteHealthCenterRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<HealthCenter>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListHealthCenterHandler : IRequestHandler<DeleteListHealthCenterRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListHealthCenterHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListHealthCenterRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<HealthCenter>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
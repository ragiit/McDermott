using static McDermott.Application.Features.Commands.InsuranceCommand;

namespace McDermott.Application.Features.Queries
{
    public class InsuranceQueryHandler
    {
        internal class GetAllInsuranceQueryHandler : IRequestHandler<GetInsuranceQuery, List<InsuranceDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllInsuranceQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<InsuranceDto>> Handle(GetInsuranceQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Insurance>().Entities
                        .Select(Insurance => Insurance.Adapt<InsuranceDto>())
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetInsuranceByIdQueryHandler : IRequestHandler<GetInsuranceByIdQuery, InsuranceDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetInsuranceByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<InsuranceDto> Handle(GetInsuranceByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Insurance>().GetByIdAsync(request.Id);

                return result.Adapt<InsuranceDto>();
            }
        }

        internal class CreateInsuranceHandler : IRequestHandler<CreateInsuranceRequest, InsuranceDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateInsuranceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<InsuranceDto> Handle(CreateInsuranceRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Insurance>().AddAsync(request.InsuranceDto.Adapt<Insurance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<InsuranceDto>();
            }
        }

        internal class UpdateInsuranceHandler : IRequestHandler<UpdateInsuranceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateInsuranceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateInsuranceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Insurance>().UpdateAsync(request.InsuranceDto.Adapt<Insurance>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteInsuranceHandler : IRequestHandler<DeleteInsuranceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteInsuranceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteInsuranceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Insurance>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListInsuranceHandler : IRequestHandler<DeleteListInsuranceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListInsuranceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListInsuranceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Insurance>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
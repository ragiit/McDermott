using static McDermott.Application.Features.Commands.BuildingCommand;

namespace McDermott.Application.Features.Queries
{
    public class BuildingQueryHandler
    {
        internal class GetAllBuildingQueryHandler : IRequestHandler<GetBuildingQuery, List<BuildingDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllBuildingQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<BuildingDto>> Handle(GetBuildingQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Building>().Entities
                    .Include(x => x.HealthCenter)
                        .Select(Building => Building.Adapt<BuildingDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetBuildingByIdQueryHandler : IRequestHandler<GetBuildingByIdQuery, BuildingDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetBuildingByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<BuildingDto> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Building>().GetByIdAsync(request.Id);

                return result.Adapt<BuildingDto>();
            }
        }

        internal class CreateBuildingHandler : IRequestHandler<CreateBuildingRequest, BuildingDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateBuildingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<BuildingDto> Handle(CreateBuildingRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Building>().AddAsync(request.BuildingDto.Adapt<Building>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<BuildingDto>();
            }
        }

        internal class UpdateBuildingHandler : IRequestHandler<UpdateBuildingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateBuildingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateBuildingRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Building>().UpdateAsync(request.BuildingDto.Adapt<Building>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteBuildingHandler : IRequestHandler<DeleteBuildingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteBuildingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteBuildingRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Building>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListBuildingHandler : IRequestHandler<DeleteListBuildingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListBuildingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListBuildingRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Building>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
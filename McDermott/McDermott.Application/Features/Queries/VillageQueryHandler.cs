using static McDermott.Application.Features.Commands.VillageCommand;

namespace McDermott.Application.Features.Queries
{
    public class VillageQueryHandler
    {
        internal class GetAllVillageQueryHandler : IRequestHandler<GetVillageQuery, List<VillageDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllVillageQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<VillageDto>> Handle(GetVillageQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Village>().Entities
                        .Include(x => x.City)
                        .Include(x => x.Province)
                        .Include(x => x.District)
                        .Select(Village => Village.Adapt<VillageDto>())
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetVillageByIdQueryHandler : IRequestHandler<GetVillageByIdQuery, VillageDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetVillageByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<VillageDto> Handle(GetVillageByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Village>().GetByIdAsync(request.Id);

                return result.Adapt<VillageDto>();
            }
        }

        internal class CreateVillageHandler : IRequestHandler<CreateVillageRequest, VillageDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateVillageHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<VillageDto> Handle(CreateVillageRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Village>().AddAsync(request.VillageDto.Adapt<Village>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<VillageDto>();
            }
        }

        internal class UpdateVillageHandler : IRequestHandler<UpdateVillageRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateVillageHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateVillageRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Village>().UpdateAsync(request.VillageDto.Adapt<Village>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteVillageHandler : IRequestHandler<DeleteVillageRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteVillageHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteVillageRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Village>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
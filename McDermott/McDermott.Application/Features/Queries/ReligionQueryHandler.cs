using static McDermott.Application.Features.Commands.ReligionCommand;
using static McDermott.Application.Features.Commands.SpecialityCommand;

namespace McDermott.Application.Features.Queries
{
    public class ReligionQueryHandler
    {
        internal class GetAllReligionQueryHandler : IRequestHandler<GetReligionQuery, List<ReligionDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllReligionQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<ReligionDto>> Handle(GetReligionQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Religion>().Entities
                        .Select(Religion => Religion.Adapt<ReligionDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetReligionByIdQueryHandler : IRequestHandler<GetReligionByIdQuery, ReligionDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetReligionByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ReligionDto> Handle(GetReligionByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Religion>().GetByIdAsync(request.Id);

                return result.Adapt<ReligionDto>();
            }
        }

        internal class CreateReligionHandler : IRequestHandler<CreateReligionRequest, ReligionDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateReligionHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ReligionDto> Handle(CreateReligionRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Religion>().AddAsync(request.ReligionDto.Adapt<Religion>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<ReligionDto>();
            }
        }

        internal class UpdateReligionHandler : IRequestHandler<UpdateReligionRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateReligionHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateReligionRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Religion>().UpdateAsync(request.ReligionDto.Adapt<Religion>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteReligionHandler : IRequestHandler<DeleteReligionRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteReligionHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteReligionRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Religion>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        internal class DeleteListReligionHandler : IRequestHandler<DeleteListReligionRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListReligionHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListReligionRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Religion>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
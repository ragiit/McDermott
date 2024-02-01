using static McDermott.Application.Features.Commands.OccupationalCommand;

namespace McDermott.Application.Features.Queries
{
    public class OccupationalQueryHandler
    {
        internal class GetAllOccupationalQueryHandler : IRequestHandler<GetOccupationalQuery, List<OccupationalDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllOccupationalQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<OccupationalDto>> Handle(GetOccupationalQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Occupational>().Entities
                        .Select(Occupational => Occupational.Adapt<OccupationalDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetOccupationalByIdQueryHandler : IRequestHandler<GetOccupationalByIdQuery, OccupationalDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetOccupationalByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<OccupationalDto> Handle(GetOccupationalByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Occupational>().GetByIdAsync(request.Id);

                return result.Adapt<OccupationalDto>();
            }
        }

        internal class CreateOccupationalHandler : IRequestHandler<CreateOccupationalRequest, OccupationalDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateOccupationalHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<OccupationalDto> Handle(CreateOccupationalRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Occupational>().AddAsync(request.OccupationalDto.Adapt<Occupational>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<OccupationalDto>();
            }
        }

        internal class UpdateOccupationalHandler : IRequestHandler<UpdateOccupationalRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateOccupationalHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateOccupationalRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Occupational>().UpdateAsync(request.OccupationalDto.Adapt<Occupational>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteOccupationalHandler : IRequestHandler<DeleteOccupationalRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteOccupationalHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteOccupationalRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Occupational>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListOccupationalHandler : IRequestHandler<DeleteListOccupationalRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListOccupationalHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListOccupationalRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Occupational>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
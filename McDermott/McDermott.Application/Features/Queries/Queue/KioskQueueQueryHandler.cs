using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class KioskQueueQueueQueryHandler
    {
        internal class GetAllKioskQueueQueryHandler : IRequestHandler<GetKioskQueueQuery, List<KioskQueueDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllKioskQueueQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<KioskQueueDto>> Handle(GetKioskQueueQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<KioskQueue>().Entities
                        .Include(x => x.Kiosk)
                        .Include(x => x.Kiosk.Patient)
                        .Include(x => x.Kiosk.Service)
                        .Include(x => x.Kiosk.Physician)
                        .Include(x => x.Service)
                        .Include(x => x.ServiceK)
                        .AsNoTracking()
                        .Select(KioskQueue => KioskQueue.Adapt<KioskQueueDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetKioskQueueByIdQueryHandler : IRequestHandler<GetKioskQueueByIdQuery, KioskQueueDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetKioskQueueByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<KioskQueueDto> Handle(GetKioskQueueByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<KioskQueue>().GetByIdAsync(request.Id);

                return result.Adapt<KioskQueueDto>();
            }
        }

        internal class CreateKioskQueueHandler : IRequestHandler<CreateKioskQueueRequest, KioskQueueDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateKioskQueueHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<KioskQueueDto> Handle(CreateKioskQueueRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<KioskQueue>().AddAsync(request.KioskQueueDto.Adapt<KioskQueue>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<KioskQueueDto>();
            }
        }

        internal class UpdateKioskQueueHandler : IRequestHandler<UpdateKioskQueueRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateKioskQueueHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateKioskQueueRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskQueue>().UpdateAsync(request.KioskQueueDto.Adapt<KioskQueue>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteKioskQueueHandler : IRequestHandler<DeleteKioskQueueRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteKioskQueueHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteKioskQueueRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskQueue>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListKioskQueueHandler : IRequestHandler<DeleteListKioskQueueRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListKioskQueueHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListKioskQueueRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskQueue>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
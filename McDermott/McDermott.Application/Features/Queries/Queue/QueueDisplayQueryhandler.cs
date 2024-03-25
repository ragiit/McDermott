using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.QueueDisplayCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class QueueDisplayQueryhandler
    {
        internal class GetAllQueueDisplayQueryHandler : IRequestHandler<GetQueueDisplayQuery, List<QueueDisplayDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllQueueDisplayQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<QueueDisplayDto>> Handle(GetQueueDisplayQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<QueueDisplay>().Entities
                        .AsNoTracking()
                        .Select(QueueDisplay => QueueDisplay.Adapt<QueueDisplayDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetQueueDisplayByIdQueryHandler : IRequestHandler<GetQueueDisplayByIdQuery, QueueDisplayDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetQueueDisplayByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<QueueDisplayDto> Handle(GetQueueDisplayByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<QueueDisplay>().GetByIdAsync(request.Id);

                return result.Adapt<QueueDisplayDto>();
            }
        }

        internal class CreateQueueDisplayHandler : IRequestHandler<CreateQueueDisplayRequest, QueueDisplayDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateQueueDisplayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<QueueDisplayDto> Handle(CreateQueueDisplayRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<QueueDisplay>().AddAsync(request.QueueDisplayDto.Adapt<QueueDisplay>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<QueueDisplayDto>();
            }
        }

        internal class UpdateQueueDisplayHandler : IRequestHandler<UpdateQueueDisplayRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateQueueDisplayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateQueueDisplayRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<QueueDisplay>().UpdateAsync(request.QueueDisplayDto.Adapt<QueueDisplay>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteQueueDisplayHandler : IRequestHandler<DeleteQueueDisplayRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteQueueDisplayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteQueueDisplayRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<QueueDisplay>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListQueueDisplayHandler : IRequestHandler<DeleteListQueueDisplayRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListQueueDisplayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListQueueDisplayRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<QueueDisplay>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
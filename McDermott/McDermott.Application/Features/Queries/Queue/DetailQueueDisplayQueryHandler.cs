using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.DetailQueueDisplayCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class DetailQueueDisplayQueryHandler
    {
        internal class GetAllDetailQueueDisplayQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetQueueDisplay, List<DetailQueueDisplayDto>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<List<DetailQueueDisplayDto>> Handle(GetQueueDisplay query, CancellationToken cancellationToken)
            {
                try
                {
                    return await _unitOfWork.Repository<DetailQueueDisplay>().Entities
                        .Select(DetailQueueDisplay => DetailQueueDisplay.Adapt<DetailQueueDisplayDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        internal class GetDetailQueueDisplayByIdQueryHandler : IRequestHandler<GetDetailQueueDisplayByIdQuery, DetailQueueDisplayDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDetailQueueDisplayByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DetailQueueDisplayDto> Handle(GetDetailQueueDisplayByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<DetailQueueDisplay>().GetByIdAsync(request.Id);

                return result.Adapt<DetailQueueDisplayDto>();
            }
        }

        internal class CreateDetailQueueDisplayHandler : IRequestHandler<CreateDetailQueueDisplayRequest, DetailQueueDisplayDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateDetailQueueDisplayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DetailQueueDisplayDto> Handle(CreateDetailQueueDisplayRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<DetailQueueDisplay>().AddAsync(request.DetailQueueDisplayDto.Adapt<DetailQueueDisplay>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<DetailQueueDisplayDto>();
            }
        }

        internal class UpdateDetailQueueDisplayHandler : IRequestHandler<UpdateDetailQueueDisplayRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateDetailQueueDisplayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateDetailQueueDisplayRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DetailQueueDisplay>().UpdateAsync(request.DetailQueueDisplayDto.Adapt<DetailQueueDisplay>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteDetailQueueDisplayHandler : IRequestHandler<DeleteDetailQueueDisplayRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDetailQueueDisplayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDetailQueueDisplayRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DetailQueueDisplay>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListDetailQueueDisplayHandler : IRequestHandler<DeleteListDetailQueueDisplayRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListDetailQueueDisplayHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListDetailQueueDisplayRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DetailQueueDisplay>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
using McDermott.Application.Dtos.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Queue.DetailQueueDisplayCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class DetailQueueDisplayQueryHandler
    {
        internal class GetAllDetailQueueDisplayQueryHandler : IRequestHandler<GetDetailQueueDisplayQuery, List<DetailQueueDisplayDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllDetailQueueDisplayQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DetailQueueDisplayDto>> Handle(GetDetailQueueDisplayQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<DetailQueueDisplay>().Entities
                        .Include(x => x.QueueDisplay)
                        .Include(x => x.Counter)
                        .AsNoTracking()
                        .Select(DetailQueueDisplay => DetailQueueDisplay.Adapt<DetailQueueDisplayDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
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

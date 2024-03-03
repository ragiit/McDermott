using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.KioskDepartementCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class KioskDepartementQueryHandler
    {
        internal class GetAllKioskDepartementQueryHandler : IRequestHandler<GetKioskDepartementQuery, List<KioskDepartementDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllKioskDepartementQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<KioskDepartementDto>> Handle(GetKioskDepartementQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<KioskDepartement>().Entities
                        .Include(x => x.ServiceK)
                        .Include(x => x.ServiceP)
                        .Select(KioskDepartement => KioskDepartement.Adapt<KioskDepartementDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetKioskDepartementByIdQueryHandler : IRequestHandler<GetKioskDepartementByIdQuery, KioskDepartementDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetKioskDepartementByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<KioskDepartementDto> Handle(GetKioskDepartementByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<KioskDepartement>().GetByIdAsync(request.Id);

                return result.Adapt<KioskDepartementDto>();
            }
        }

        internal class CreateKioskDepartementHandler : IRequestHandler<CreateKioskDepartementRequest, KioskDepartementDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateKioskDepartementHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<KioskDepartementDto> Handle(CreateKioskDepartementRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<KioskDepartement>().AddAsync(request.KioskDepartementDto.Adapt<KioskDepartement>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<KioskDepartementDto>();
            }
        }

        internal class UpdateKioskDepartementHandler : IRequestHandler<UpdateKioskDepartementRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateKioskDepartementHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateKioskDepartementRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskDepartement>().UpdateAsync(request.KioskDepartementDto.Adapt<KioskDepartement>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteKioskDepartementHandler : IRequestHandler<DeleteKioskDepartementRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteKioskDepartementHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteKioskDepartementRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskDepartement>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListKioskDepartementHandler : IRequestHandler<DeleteListKioskDepartementRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListKioskDepartementHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListKioskDepartementRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskDepartement>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
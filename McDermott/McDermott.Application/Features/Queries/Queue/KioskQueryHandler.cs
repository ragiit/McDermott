using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McDermott.Application.Dtos.Transaction;
using static McDermott.Application.Features.Commands.Queue.KioskCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class KioskQueryHandler
    {
        internal class GetAllKioskQueryHandler : IRequestHandler<GetKioskQuery, List<KioskDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllKioskQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<KioskDto>> Handle(GetKioskQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Kiosk>().Entities
                        .Include(x => x.Patient)
                        .Include(x => x.Service)
                        .Include(x => x.Physician)
                        .AsNoTracking()
                        .Select(Kiosk => Kiosk.Adapt<KioskDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetKioskByIdQueryHandler : IRequestHandler<GetKioskByIdQuery, KioskDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetKioskByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<KioskDto> Handle(GetKioskByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Kiosk>().GetByIdAsync(request.Id);

                return result.Adapt<KioskDto>();
            }
        }

        internal class CreateKioskHandler : IRequestHandler<CreateKioskRequest, KioskDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateKioskHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<KioskDto> Handle(CreateKioskRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Kiosk>().AddAsync(request.KioskDto.Adapt<Kiosk>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<KioskDto>();
            }
        }

        internal class UpdateKioskHandler : IRequestHandler<UpdateKioskRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateKioskHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateKioskRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Kiosk>().UpdateAsync(request.KioskDto.Adapt<Kiosk>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteKioskHandler : IRequestHandler<DeleteKioskRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteKioskHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteKioskRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Kiosk>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListKioskHandler : IRequestHandler<DeleteListKioskRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListKioskHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListKioskRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Kiosk>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}


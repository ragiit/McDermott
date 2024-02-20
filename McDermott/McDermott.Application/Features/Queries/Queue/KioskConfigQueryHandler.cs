using McDermott.Application.Dtos.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Queue.KioskConfigCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class KioskConfigQueryHandler
    {
        internal class GetAllKioskConfigQueryHandler : IRequestHandler<GetKioskConfigQuery, List<KioskConfigDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllKioskConfigQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<KioskConfigDto>> Handle(GetKioskConfigQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<KioskConfig>().Entities
                        .Select(KioskConfig => KioskConfig.Adapt<KioskConfigDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetKioskConfigByIdQueryHandler : IRequestHandler<GetKioskConfigByIdQuery, KioskConfigDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetKioskConfigByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<KioskConfigDto> Handle(GetKioskConfigByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<KioskConfig>().GetByIdAsync(request.Id);

                return result.Adapt<KioskConfigDto>();
            }
        }

        internal class CreateKioskConfigHandler : IRequestHandler<CreateKioskConfigRequest, KioskConfigDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateKioskConfigHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<KioskConfigDto> Handle(CreateKioskConfigRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<KioskConfig>().AddAsync(request.KioskConfigDto.Adapt<KioskConfig>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<KioskConfigDto>();
            }
        }

        internal class UpdateKioskConfigHandler : IRequestHandler<UpdateKioskConfigRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateKioskConfigHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateKioskConfigRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskConfig>().UpdateAsync(request.KioskConfigDto.Adapt<KioskConfig>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteKioskConfigHandler : IRequestHandler<DeleteKioskConfigRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteKioskConfigHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteKioskConfigRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskConfig>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListKioskConfigHandler : IRequestHandler<DeleteListKioskConfigRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListKioskConfigHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListKioskConfigRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<KioskConfig>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}

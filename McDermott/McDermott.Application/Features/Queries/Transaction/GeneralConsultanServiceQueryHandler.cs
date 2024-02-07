using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McDermott.Application.Dtos.Transaction;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanServiceCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanServiceQueryHandler
    {
        internal class GetAllGeneralConsultanServiceQueryHandler : IRequestHandler<GetGeneralConsultanServiceQuery, List<GeneralConsultanServiceDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllGeneralConsultanServiceQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<GeneralConsultanServiceDto>> Handle(GetGeneralConsultanServiceQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    return await _unitOfWork.Repository<GeneralConsultanService>().Entities
                            .Include(x => x.Service)
                            .Include(x => x.Insurance)
                            .Include(x => x.Patient)
                            .Include(t => t.Pratitioner)
                            .AsNoTracking()
                            .Select(GeneralConsultanService => GeneralConsultanService.Adapt<GeneralConsultanServiceDto>())
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        internal class GetGeneralConsultanServiceByIdQueryHandler : IRequestHandler<GetGeneralConsultanServiceByIdQuery, GeneralConsultanServiceDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetGeneralConsultanServiceByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<GeneralConsultanServiceDto> Handle(GetGeneralConsultanServiceByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<GeneralConsultanService>().GetByIdAsync(request.Id);

                return result.Adapt<GeneralConsultanServiceDto>();
            }
        }

        internal class CreateGeneralConsultanServiceHandler : IRequestHandler<CreateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateGeneralConsultanServiceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<GeneralConsultanServiceDto> Handle(CreateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(request.GeneralConsultanServiceDto.Adapt<GeneralConsultanService>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<GeneralConsultanServiceDto>();
            }
        }

        internal class UpdateGeneralConsultanServiceHandler : IRequestHandler<UpdateGeneralConsultanServiceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateGeneralConsultanServiceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<GeneralConsultanService>().UpdateAsync(request.GeneralConsultanServiceDto.Adapt<GeneralConsultanService>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteGeneralConsultanServiceHandler : IRequestHandler<DeleteGeneralConsultanServiceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteGeneralConsultanServiceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListGeneralConsultanServiceHandler : IRequestHandler<DeleteListGeneralConsultanServiceRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListGeneralConsultanServiceHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}

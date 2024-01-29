using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.ProcedureCommand;

namespace McDermott.Application.Features.Queries
{
    public class ProcedureQueryHandler
    {
        internal class GetAllProcedureQueryHandler : IRequestHandler<GetProcedureQuery, List<ProcedureDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllProcedureQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<ProcedureDto>> Handle(GetProcedureQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Procedure>().Entities
                        .Select(Procedure => Procedure.Adapt<ProcedureDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetProcedureByIdQueryHandler : IRequestHandler<GetProcedureByIdQuery, ProcedureDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetProcedureByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ProcedureDto> Handle(GetProcedureByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Procedure>().GetByIdAsync(request.Id);

                return result.Adapt<ProcedureDto>();
            }
        }

        internal class CreateProcedureHandler : IRequestHandler<CreateProcedureRequest, ProcedureDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateProcedureHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ProcedureDto> Handle(CreateProcedureRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Procedure>().AddAsync(request.ProcedureDto.Adapt<Procedure>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<ProcedureDto>();
            }
        }

        internal class UpdateProcedureHandler : IRequestHandler<UpdateProcedureRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateProcedureHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateProcedureRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Procedure>().UpdateAsync(request.ProcedureDto.Adapt<Procedure>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteProcedureHandler : IRequestHandler<DeleteProcedureRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteProcedureHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteProcedureRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Procedure>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListProcedureHandler : IRequestHandler<DeleteListProcedureRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListProcedureHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListProcedureRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Procedure>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}

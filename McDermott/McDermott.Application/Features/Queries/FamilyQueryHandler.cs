using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.FamilyCommand;

namespace McDermott.Application.Features.Queries
{
    public class FamilyQueryHandler
    {
        internal class GetAllFamilyQueryHandler : IRequestHandler<GetFamilyQuery, List<FamilyDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllFamilyQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<FamilyDto>> Handle(GetFamilyQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Family>().Entities
                        .Select(Family => Family.Adapt<FamilyDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetFamilyByIdQueryHandler : IRequestHandler<GetFamilyByIdQuery, FamilyDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetFamilyByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<FamilyDto> Handle(GetFamilyByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Family>().GetByIdAsync(request.Id);

                return result.Adapt<FamilyDto>();
            }
        }

        internal class CreateFamilyHandler : IRequestHandler<CreateFamilyRequest, FamilyDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateFamilyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<FamilyDto> Handle(CreateFamilyRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Family>().AddAsync(request.FamilyDto.Adapt<Family>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<FamilyDto>();
            }
        }

        internal class UpdateFamilyHandler : IRequestHandler<UpdateFamilyRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateFamilyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateFamilyRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Family>().UpdateAsync(request.FamilyDto.Adapt<Family>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteFamilyHandler : IRequestHandler<DeleteFamilyRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteFamilyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteFamilyRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Family>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListFamilyHandler : IRequestHandler<DeleteListFamilyRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListFamilyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListFamilyRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Family>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
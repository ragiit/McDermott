using static McDermott.Application.Features.Commands.SpecialityCommand;

namespace McDermott.Application.Features.Queries
{
    public class SpecialityQueryHandler
    {
        internal class GetAllSpecialityQueryHandler : IRequestHandler<GetSpecialityQuery, List<SpecialityDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllSpecialityQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<SpecialityDto>> Handle(GetSpecialityQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Speciality>().Entities
                        .Select(Speciality => Speciality.Adapt<SpecialityDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetSpecialityByIdQueryHandler : IRequestHandler<GetSpecialityByIdQuery, SpecialityDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetSpecialityByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<SpecialityDto> Handle(GetSpecialityByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Speciality>().GetByIdAsync(request.Id);

                return result.Adapt<SpecialityDto>();
            }
        }

        internal class CreateSpecialityHandler : IRequestHandler<CreateSpecialityRequest, SpecialityDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateSpecialityHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<SpecialityDto> Handle(CreateSpecialityRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Speciality>().AddAsync(request.SpecialityDto.Adapt<Speciality>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<SpecialityDto>();
            }
        }

        internal class UpdateSpecialityHandler : IRequestHandler<UpdateSpecialityRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateSpecialityHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateSpecialityRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Speciality>().UpdateAsync(request.SpecialityDto.Adapt<Speciality>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteSpecialityHandler : IRequestHandler<DeleteSpecialityRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteSpecialityHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteSpecialityRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Speciality>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListSpecialityHandler : IRequestHandler<DeleteListSpecialityRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListSpecialityHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListSpecialityRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Speciality>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
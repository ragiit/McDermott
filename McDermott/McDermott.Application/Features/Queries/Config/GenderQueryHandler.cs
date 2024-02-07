

namespace McDermott.Application.Features.Queries.Config
{
    public class GenderQueryHandler
    {
        internal class GetAllGenderQueryHandler : IRequestHandler<GetGenderQuery, List<GenderDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllGenderQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<GenderDto>> Handle(GetGenderQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Gender>().Entities
                        .Select(Gender => Gender.Adapt<GenderDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetGenderByIdQueryHandler : IRequestHandler<GetGenderByIdQuery, GenderDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetGenderByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<GenderDto> Handle(GetGenderByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Gender>().GetByIdAsync(request.Id);

                return result.Adapt<GenderDto>();
            }
        }

        internal class CreateGenderHandler : IRequestHandler<CreateGenderRequest, GenderDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateGenderHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<GenderDto> Handle(CreateGenderRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Gender>().AddAsync(request.GenderDto.Adapt<Gender>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<GenderDto>();
            }
        }

        internal class UpdateGenderHandler : IRequestHandler<UpdateGenderRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateGenderHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateGenderRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Gender>().UpdateAsync(request.GenderDto.Adapt<Gender>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteGenderHandler : IRequestHandler<DeleteGenderRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteGenderHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteGenderRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Gender>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
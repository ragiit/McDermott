using static McDermott.Application.Features.Commands.Medical.DiseaseCategoryCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DiseaseCategoryQueryHandler
    {
        internal class GetAllDiseaseCategoryQueryHandler : IRequestHandler<GetDiseaseCategoryQuery, List<DiseaseCategoryDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllDiseaseCategoryQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DiseaseCategoryDto>> Handle(GetDiseaseCategoryQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<DiseaseCategory>().Entities
                        .Select(DiseaseCategory => DiseaseCategory.Adapt<DiseaseCategoryDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetDiseaseCategoryByIdQueryHandler : IRequestHandler<GetDiseaseCategoryByIdQuery, DiseaseCategoryDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDiseaseCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DiseaseCategoryDto> Handle(GetDiseaseCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().GetByIdAsync(request.Id);

                return result.Adapt<DiseaseCategoryDto>();
            }
        }

        internal class CreateDiseaseCategoryHandler : IRequestHandler<CreateDiseaseCategoryRequest, DiseaseCategoryDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateDiseaseCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DiseaseCategoryDto> Handle(CreateDiseaseCategoryRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().AddAsync(request.DiseaseCategoryDto.Adapt<DiseaseCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<DiseaseCategoryDto>();
            }
        }

        internal class UpdateDiseaseCategoryHandler : IRequestHandler<UpdateDiseaseCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateDiseaseCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateDiseaseCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DiseaseCategory>().UpdateAsync(request.DiseaseCategoryDto.Adapt<DiseaseCategory>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteDiseaseCategoryHandler : IRequestHandler<DeleteDiseaseCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDiseaseCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDiseaseCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DiseaseCategory>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListDiseaseCategoryHandler : IRequestHandler<DeleteListDiseaseCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListDiseaseCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListDiseaseCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DiseaseCategory>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
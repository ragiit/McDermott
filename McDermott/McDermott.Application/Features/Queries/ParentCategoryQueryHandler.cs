using static McDermott.Application.Features.Commands.ParentCategoryCommand;

namespace McDermott.Application.Features.Queries
{
    public class ParentCategoryQueryHandler
    {
        internal class GetAllParentCategoryQueryHandler : IRequestHandler<GetParentCategoryQuery, List<ParentCategoryDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllParentCategoryQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<ParentCategoryDto>> Handle(GetParentCategoryQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<ParentCategory>().Entities
                        .Select(ParentCategory => ParentCategory.Adapt<ParentCategoryDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetParentCategoryByIdQueryHandler : IRequestHandler<GetParentCategoryByIdQuery, ParentCategoryDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetParentCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ParentCategoryDto> Handle(GetParentCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<ParentCategory>().GetByIdAsync(request.Id);

                return result.Adapt<ParentCategoryDto>();
            }
        }

        internal class CreateParentCategoryHandler : IRequestHandler<CreateParentCategoryRequest, ParentCategoryDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateParentCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ParentCategoryDto> Handle(CreateParentCategoryRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<ParentCategory>().AddAsync(request.ParentCategoryDto.Adapt<ParentCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<ParentCategoryDto>();
            }
        }

        internal class UpdateParentCategoryHandler : IRequestHandler<UpdateParentCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateParentCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateParentCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<ParentCategory>().UpdateAsync(request.ParentCategoryDto.Adapt<ParentCategory>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteParentCategoryHandler : IRequestHandler<DeleteParentCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteParentCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteParentCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<ParentCategory>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListParentCategoryHandler : IRequestHandler<DeleteListParentCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListParentCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListParentCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<ParentCategory>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.CronisCategoryCommand;

namespace McDermott.Application.Features.Queries
{
    public class CronisCategoryQueryHandler
    {
        internal class GetAllCronisCategoryQueryHandler : IRequestHandler<GetCronisCategoryQuery, List<CronisCategoryDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllCronisCategoryQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<CronisCategoryDto>> Handle(GetCronisCategoryQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<CronisCategory>().Entities
                        .Select(CronisCategory => CronisCategory.Adapt<CronisCategoryDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetCronisCategoryByIdQueryHandler : IRequestHandler<GetCronisCategoryByIdQuery, CronisCategoryDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetCronisCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CronisCategoryDto> Handle(GetCronisCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<CronisCategory>().GetByIdAsync(request.Id);

                return result.Adapt<CronisCategoryDto>();
            }
        }

        internal class CreateCronisCategoryHandler : IRequestHandler<CreateCronisCategoryRequest, CronisCategoryDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateCronisCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<CronisCategoryDto> Handle(CreateCronisCategoryRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<CronisCategory>().AddAsync(request.CronisCategoryDto.Adapt<CronisCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<CronisCategoryDto>();
            }
        }

        internal class UpdateCronisCategoryHandler : IRequestHandler<UpdateCronisCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateCronisCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateCronisCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<CronisCategory>().UpdateAsync(request.CronisCategoryDto.Adapt<CronisCategory>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteCronisCategoryHandler : IRequestHandler<DeleteCronisCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteCronisCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteCronisCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<CronisCategory>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListCronisCategoryHandler : IRequestHandler<DeleteListCronisCategoryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListCronisCategoryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListCronisCategoryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<CronisCategory>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
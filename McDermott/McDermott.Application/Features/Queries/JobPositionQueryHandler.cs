using static McDermott.Application.Features.Commands.JobPositionCommand;

namespace McDermott.Application.Features.Queries
{
    public class JobPositionQueryHandler
    {
        internal class GetAllJobPositionQueryHandler : IRequestHandler<GetJobPositionQuery, List<JobPositionDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllJobPositionQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<JobPositionDto>> Handle(GetJobPositionQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<JobPosition>().Entities
                    .Include(x => x.Department)
                        .Select(JobPosition => JobPosition.Adapt<JobPositionDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetJobPositionByIdQueryHandler : IRequestHandler<GetJobPositionByIdQuery, JobPositionDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetJobPositionByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<JobPositionDto> Handle(GetJobPositionByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<JobPosition>().GetByIdAsync(request.Id);

                return result.Adapt<JobPositionDto>();
            }
        }

        internal class CreateJobPositionHandler : IRequestHandler<CreateJobPositionRequest, JobPositionDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateJobPositionHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<JobPositionDto> Handle(CreateJobPositionRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<JobPosition>().AddAsync(request.JobPositionDto.Adapt<JobPosition>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<JobPositionDto>();
            }
        }

        internal class UpdateJobPositionHandler : IRequestHandler<UpdateJobPositionRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateJobPositionHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateJobPositionRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<JobPosition>().UpdateAsync(request.JobPositionDto.Adapt<JobPosition>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteJobPositionHandler : IRequestHandler<DeleteJobPositionRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteJobPositionHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteJobPositionRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<JobPosition>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListJobPositionHandler : IRequestHandler<DeleteListJobPositionRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListJobPositionHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListJobPositionRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<JobPosition>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
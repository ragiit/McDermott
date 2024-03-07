namespace McDermott.Application.Features.Commands.Employee
{
    public class JobPositionCommand
    {
        public class GetJobPositionQuery : IRequest<List<JobPositionDto>>;

        public class GetJobPositionByIdQuery : IRequest<JobPositionDto>
        {
             public long Id { get; set; }

            public GetJobPositionByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateJobPositionRequest : IRequest<JobPositionDto>
        {
            public JobPositionDto JobPositionDto { get; set; }

            public CreateJobPositionRequest(JobPositionDto JobPositionDto)
            {
                this.JobPositionDto = JobPositionDto;
            }
        }

        public class UpdateJobPositionRequest : IRequest<bool>
        {
            public JobPositionDto JobPositionDto { get; set; }

            public UpdateJobPositionRequest(JobPositionDto JobPositionDto)
            {
                this.JobPositionDto = JobPositionDto;
            }
        }

        public class DeleteJobPositionRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteJobPositionRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListJobPositionRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListJobPositionRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}
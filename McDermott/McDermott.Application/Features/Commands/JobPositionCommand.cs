namespace McDermott.Application.Features.Commands
{
    public class JobPositionCommand
    {
        public class GetJobPositionQuery : IRequest<List<JobPositionDto>>;

        public class GetJobPositionByIdQuery : IRequest<JobPositionDto>
        {
            public int Id { get; set; }

            public GetJobPositionByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteJobPositionRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListJobPositionRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListJobPositionRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}
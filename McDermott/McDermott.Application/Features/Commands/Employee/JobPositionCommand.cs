namespace McDermott.Application.Features.Commands.Employee
{
    public class JobPositionCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetJobPositionQuery(Expression<Func<JobPosition, bool>>? predicate = null, bool removeCache = false) : IRequest<List<JobPositionDto>>
        {
            public Expression<Func<JobPosition, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateJobPositionRequest(JobPositionDto JobPositionDto) : IRequest<JobPositionDto>
        {
            public JobPositionDto JobPositionDto { get; set; } = JobPositionDto;
        }

        public class CreateListJobPositionRequest(List<JobPositionDto> GeneralConsultanCPPTDtos) : IRequest<List<JobPositionDto>>
        {
            public List<JobPositionDto> JobPositionDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateJobPositionRequest(JobPositionDto JobPositionDto) : IRequest<JobPositionDto>
        {
            public JobPositionDto JobPositionDto { get; set; } = JobPositionDto;
        }

        public class UpdateListJobPositionRequest(List<JobPositionDto> JobPositionDtos) : IRequest<List<JobPositionDto>>
        {
            public List<JobPositionDto> JobPositionDtos { get; set; } = JobPositionDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteJobPositionRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
namespace McDermott.Application.Features.Commands.Employee
{
    public class JobPositionCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleJobPositionQuery : IRequest<JobPositionDto>
        {
            public List<Expression<Func<JobPosition, object>>> Includes { get; set; }
            public Expression<Func<JobPosition, bool>> Predicate { get; set; }
            public Expression<Func<JobPosition, JobPosition>> Select { get; set; }

            public List<(Expression<Func<JobPosition, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetJobPositionQuery : IRequest<(List<JobPositionDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<JobPosition, object>>> Includes { get; set; }
            public Expression<Func<JobPosition, bool>> Predicate { get; set; }
            public Expression<Func<JobPosition, JobPosition>> Select { get; set; }

            public List<(Expression<Func<JobPosition, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateJobPositionQuery(List<JobPositionDto> JobPositionsToValidate) : IRequest<List<JobPositionDto>>
        {
            public List<JobPositionDto> JobPositionsToValidate { get; } = JobPositionsToValidate;
        }

        public class ValidateJobPositionQuery(Expression<Func<JobPosition, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<JobPosition, bool>> Predicate { get; } = predicate!;
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
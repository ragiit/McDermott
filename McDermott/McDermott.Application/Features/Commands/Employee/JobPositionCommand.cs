namespace McDermott.Application.Features.Commands.Employee
{
    public class JobPositionCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetJobPositionQuery(Expression<Func<JobPosition, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<JobPositionDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<JobPosition, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; set; } = pageSize ?? 10;
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
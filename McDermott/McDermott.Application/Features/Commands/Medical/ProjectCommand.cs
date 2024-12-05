namespace McDermott.Application.Features.Commands.Medical
{
    public class ProjectCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleProjectQuery : IRequest<ProjectDto>
        {
            public List<Expression<Func<Project, object>>> Includes { get; set; }
            public Expression<Func<Project, bool>> Predicate { get; set; }
            public Expression<Func<Project, Project>> Select { get; set; }

            public List<(Expression<Func<Project, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetProjectQuery : IRequest<(List<ProjectDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Project, object>>> Includes { get; set; }
            public Expression<Func<Project, bool>> Predicate { get; set; }
            public Expression<Func<Project, Project>>? Select { get; set; }

            public List<(Expression<Func<Project, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateProjectQuery(List<ProjectDto> ProjectsToValidate) : IRequest<List<ProjectDto>>
        {
            public List<ProjectDto> ProjectsToValidate { get; } = ProjectsToValidate;
        }

        public class ValidateProjectQuery(Expression<Func<Project, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Project, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateProjectRequest(ProjectDto ProjectDto) : IRequest<ProjectDto>
        {
            public ProjectDto ProjectDto { get; set; } = ProjectDto;
        }

        public class CreateListProjectRequest(List<ProjectDto> GeneralConsultanCPPTDtos) : IRequest<List<ProjectDto>>
        {
            public List<ProjectDto> ProjectDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateProjectRequest(ProjectDto ProjectDto) : IRequest<ProjectDto>
        {
            public ProjectDto ProjectDto { get; set; } = ProjectDto;
        }

        public class UpdateListProjectRequest(List<ProjectDto> ProjectDtos) : IRequest<List<ProjectDto>>
        {
            public List<ProjectDto> ProjectDtos { get; set; } = ProjectDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteProjectRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
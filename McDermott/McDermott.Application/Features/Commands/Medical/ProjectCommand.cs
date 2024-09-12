namespace McDermott.Application.Features.Commands.Medical
{
    public class ProjectCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetProjectQuery(Expression<Func<Project, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProjectDto>>
        {
            public Expression<Func<Project, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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
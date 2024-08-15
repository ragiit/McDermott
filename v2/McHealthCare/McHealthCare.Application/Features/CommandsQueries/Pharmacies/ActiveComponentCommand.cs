namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class ActiveComponentCommand
    {
        #region GET

        public class GetActiveComponentQuery(Expression<Func<ActiveComponent, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ActiveComponentDto>>
        {
            public Expression<Func<ActiveComponent, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateActiveComponentRequest(ActiveComponentDto ActiveComponentDto) : IRequest<ActiveComponentDto>
        {
            public ActiveComponentDto ActiveComponentDto { get; set; } = ActiveComponentDto;
        }

        public class CreateListActiveComponentRequest(List<ActiveComponentDto> ActiveComponentDtos) : IRequest<List<ActiveComponentDto>>
        {
            public List<ActiveComponentDto> ActiveComponentDtos { get; set; } = ActiveComponentDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateActiveComponentRequest(ActiveComponentDto ActiveComponentDto) : IRequest<ActiveComponentDto>
        {
            public ActiveComponentDto ActiveComponentDto { get; set; } = ActiveComponentDto;
        }

        public class UpdateListActiveComponentRequest(List<ActiveComponentDto> ActiveComponentDtos) : IRequest<List<ActiveComponentDto>>
        {
            public List<ActiveComponentDto> ActiveComponentDtos { get; set; } = ActiveComponentDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteActiveComponentRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class ActiveComponentCommand
    {
        #region GET 

        public class GetActiveComponentQuery(Expression<Func<ActiveComponent, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ActiveComponentDto>>
        {
            public Expression<Func<ActiveComponent, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

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

        public class DeleteActiveComponentRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

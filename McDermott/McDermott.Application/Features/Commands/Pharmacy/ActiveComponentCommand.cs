namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class ActiveComponentCommand
    {
        #region GET

        public class GetActiveComponentQuery(Expression<Func<ActiveComponent, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<ActiveComponent, object>>>? includes = null, Expression<Func<ActiveComponent, ActiveComponent>>? select = null) : IRequest<(List<ActiveComponentDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<ActiveComponent, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<ActiveComponent, object>>> Includes { get; } = includes!;
            public Expression<Func<ActiveComponent, ActiveComponent>>? Select { get; } = select!;
        }

        public class BulkValidateActiveComponentQuery(List<ActiveComponentDto> ActiveComponentsToValidate) : IRequest<List<ActiveComponentDto>>
        {
            public List<ActiveComponentDto> ActiveComponentsToValidate { get; } = ActiveComponentsToValidate;
        }

        public class ValidateActiveComponentQuery(Expression<Func<ActiveComponent, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<ActiveComponent, bool>> Predicate { get; } = predicate!;
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

        public class DeleteActiveComponentRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
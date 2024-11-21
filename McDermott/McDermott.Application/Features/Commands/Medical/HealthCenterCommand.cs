namespace McDermott.Application.Features.Commands.Medical
{
    public class HealthCenterCommand
    {
        #region GET

        public class GetHealthCenterQuery(Expression<Func<HealthCenter, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<HealthCenter, object>>>? includes = null, Expression<Func<HealthCenter, HealthCenter>>? select = null) : IRequest<(List<HealthCenterDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<HealthCenter, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<HealthCenter, object>>> Includes { get; } = includes!;
            public Expression<Func<HealthCenter, HealthCenter>>? Select { get; } = select!;
        }

        public class ValidateHealthCenterQuery(Expression<Func<HealthCenter, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<HealthCenter, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateHealthCenterRequest(HealthCenterDto HealthCenterDto) : IRequest<HealthCenterDto>
        {
            public HealthCenterDto HealthCenterDto { get; set; } = HealthCenterDto;
        }

        public class CreateListHealthCenterRequest(List<HealthCenterDto> HealthCenterDtos) : IRequest<List<HealthCenterDto>>
        {
            public List<HealthCenterDto> HealthCenterDtos { get; set; } = HealthCenterDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateHealthCenterRequest(HealthCenterDto HealthCenterDto) : IRequest<HealthCenterDto>
        {
            public HealthCenterDto HealthCenterDto { get; set; } = HealthCenterDto;
        }

        public class UpdateListHealthCenterRequest(List<HealthCenterDto> HealthCenterDtos) : IRequest<List<HealthCenterDto>>
        {
            public List<HealthCenterDto> HealthCenterDtos { get; set; } = HealthCenterDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteHealthCenterRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
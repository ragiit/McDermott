namespace McDermott.Application.Features.Commands.Medical
{
    public class HealthCenterCommand
    {
        #region GET 

        public class GetHealthCenterQuery(Expression<Func<HealthCenter, bool>>? predicate = null, bool removeCache = false) : IRequest<List<HealthCenterDto>>
        {
            public Expression<Func<HealthCenter, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

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
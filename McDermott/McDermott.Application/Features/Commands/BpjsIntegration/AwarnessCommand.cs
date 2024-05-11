namespace McDermott.Application.Features.Commands.BpjsIntegration
{
    public class AwarenessCommand
    {
        #region GET

        public class GetAwarenessQuery(Expression<Func<Awareness, bool>>? predicate = null, bool removeCache = false) : IRequest<List<AwarenessDto>>
        {
            public Expression<Func<Awareness, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateAwarenessRequest(AwarenessDto AwarenessDto) : IRequest<AwarenessDto>
        {
            public AwarenessDto AwarenessDto { get; set; } = AwarenessDto;
        }

        public class CreateListAwarenessRequest(List<AwarenessDto> AwarenessDtos) : IRequest<List<AwarenessDto>>
        {
            public List<AwarenessDto> AwarenessDtos { get; set; } = AwarenessDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateAwarenessRequest(AwarenessDto AwarenessDto) : IRequest<AwarenessDto>
        {
            public AwarenessDto AwarenessDto { get; set; } = AwarenessDto;
        }

        public class UpdateListAwarenessRequest(List<AwarenessDto> AwarenessDtos) : IRequest<List<AwarenessDto>>
        {
            public List<AwarenessDto> AwarenessDtos { get; set; } = AwarenessDtos;
        }

        public class UpdateToDbAwarenessRequest(List<AwarenessDto> AwarenessDtos) : IRequest<List<AwarenessDto>>
        {
            public List<AwarenessDto> AwarenessDtos { get; set; } = AwarenessDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteAwarenessRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        public class DeleteAllAwareness() : IRequest<bool>
        {
        }

        #endregion DELETE
    }
}
namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class ConcoctionCommand
    {
        #region GET

        public class GetConcoctionQuery(Expression<Func<Concoction, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ConcoctionDto>>
        {
            public Expression<Func<Concoction, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateConcoctionRequest(ConcoctionDto ConcoctionDto) : IRequest<ConcoctionDto>
        {
            public ConcoctionDto ConcoctionDto { get; set; } = ConcoctionDto;
        }

        public class CreateListConcoctionRequest(List<ConcoctionDto> ConcoctionDtos) : IRequest<List<ConcoctionDto>>
        {
            public List<ConcoctionDto> ConcoctionDtos { get; set; } = ConcoctionDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateConcoctionRequest(ConcoctionDto ConcoctionDto) : IRequest<ConcoctionDto>
        {
            public ConcoctionDto ConcoctionDto { get; set; } = ConcoctionDto;
        }

        public class UpdateListConcoctionRequest(List<ConcoctionDto> ConcoctionDtos) : IRequest<List<ConcoctionDto>>
        {
            public List<ConcoctionDto> ConcoctionDtos { get; set; } = ConcoctionDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteConcoctionRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
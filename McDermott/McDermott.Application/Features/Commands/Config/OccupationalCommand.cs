namespace McDermott.Application.Features.Commands.Config
{
    public class OccupationalCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetOccupationalQuery(Expression<Func<Occupational, bool>>? predicate = null, bool removeCache = false) : IRequest<List<OccupationalDto>>
        {
            public Expression<Func<Occupational, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateOccupationalRequest(OccupationalDto OccupationalDto) : IRequest<OccupationalDto>
        {
            public OccupationalDto OccupationalDto { get; set; } = OccupationalDto;
        }

        public class CreateListOccupationalRequest(List<OccupationalDto> GeneralConsultanCPPTDtos) : IRequest<List<OccupationalDto>>
        {
            public List<OccupationalDto> OccupationalDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateOccupationalRequest(OccupationalDto OccupationalDto) : IRequest<OccupationalDto>
        {
            public OccupationalDto OccupationalDto { get; set; } = OccupationalDto;
        }

        public class UpdateListOccupationalRequest(List<OccupationalDto> OccupationalDtos) : IRequest<List<OccupationalDto>>
        {
            public List<OccupationalDto> OccupationalDtos { get; set; } = OccupationalDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteOccupationalRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
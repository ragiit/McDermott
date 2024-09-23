namespace McDermott.Application.Features.Commands.Config
{
    public class ReligionCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetReligionQuery(Expression<Func<Religion, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ReligionDto>>
        {
            public Expression<Func<Religion, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class BulkValidateReligionQuery(List<ReligionDto> ReligionsToValidate) : IRequest<List<ReligionDto>>
        {
            public List<ReligionDto> ReligionsToValidate { get; } = ReligionsToValidate;
        }

        public class ValidateReligionQuery(Expression<Func<Religion, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Religion, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateReligionRequest(ReligionDto ReligionDto) : IRequest<ReligionDto>
        {
            public ReligionDto ReligionDto { get; set; } = ReligionDto;
        }

        public class CreateListReligionRequest(List<ReligionDto> GeneralConsultanCPPTDtos) : IRequest<List<ReligionDto>>
        {
            public List<ReligionDto> ReligionDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateReligionRequest(ReligionDto ReligionDto) : IRequest<ReligionDto>
        {
            public ReligionDto ReligionDto { get; set; } = ReligionDto;
        }

        public class UpdateListReligionRequest(List<ReligionDto> ReligionDtos) : IRequest<List<ReligionDto>>
        {
            public List<ReligionDto> ReligionDtos { get; set; } = ReligionDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteReligionRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
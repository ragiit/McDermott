namespace McDermott.Application.Features.Commands.Transaction
{
    public class WellnessProgramParticipantCommand
    {
        #region GET

        public class GetSingleWellnessProgramParticipantQuery : IRequest<WellnessProgramParticipantDto>
        {
            public List<Expression<Func<WellnessProgramParticipant, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgramParticipant, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgramParticipant, WellnessProgramParticipant>> Select { get; set; }

            public List<(Expression<Func<WellnessProgramParticipant, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetWellnessProgramParticipantQuery : IRequest<(List<WellnessProgramParticipantDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<WellnessProgramParticipant, object>>> Includes { get; set; }
            public Expression<Func<WellnessProgramParticipant, bool>> Predicate { get; set; }
            public Expression<Func<WellnessProgramParticipant, WellnessProgramParticipant>> Select { get; set; }

            public List<(Expression<Func<WellnessProgramParticipant, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateWellnessProgramParticipant(Expression<Func<WellnessProgramParticipant, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<WellnessProgramParticipant, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateWellnessProgramParticipantRequest(WellnessProgramParticipantDto WellnessProgramParticipantDto) : IRequest<WellnessProgramParticipantDto>
        {
            public WellnessProgramParticipantDto WellnessProgramParticipantDto { get; set; } = WellnessProgramParticipantDto;
        }

        public class BulkValidateWellnessProgramParticipant(List<WellnessProgramParticipantDto> WellnessProgramParticipantsToValidate) : IRequest<List<WellnessProgramParticipantDto>>
        {
            public List<WellnessProgramParticipantDto> WellnessProgramParticipantsToValidate { get; } = WellnessProgramParticipantsToValidate;
        }

        public class CreateListWellnessProgramParticipantRequest(List<WellnessProgramParticipantDto> WellnessProgramParticipantDtos) : IRequest<List<WellnessProgramParticipantDto>>
        {
            public List<WellnessProgramParticipantDto> WellnessProgramParticipantDtos { get; set; } = WellnessProgramParticipantDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateWellnessProgramParticipantRequest(WellnessProgramParticipantDto WellnessProgramParticipantDto) : IRequest<WellnessProgramParticipantDto>
        {
            public WellnessProgramParticipantDto WellnessProgramParticipantDto { get; set; } = WellnessProgramParticipantDto;
        }

        public class UpdateListWellnessProgramParticipantRequest(List<WellnessProgramParticipantDto> WellnessProgramParticipantDtos) : IRequest<List<WellnessProgramParticipantDto>>
        {
            public List<WellnessProgramParticipantDto> WellnessProgramParticipantDtos { get; set; } = WellnessProgramParticipantDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteWellnessProgramParticipantRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}
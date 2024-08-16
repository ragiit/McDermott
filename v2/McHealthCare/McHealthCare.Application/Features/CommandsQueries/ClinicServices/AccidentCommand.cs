namespace McHealthCare.Application.Features.Commands.ClinicServices
{
    public class AccidentCommand
    {
        #region GET

        public class GetAccidentQuery(Expression<Func<Accident, bool>>? predicate = null, bool removeCache = false) : IRequest<List<AccidentDto>>
        {
            public Expression<Func<Accident, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET



        #region CREATE

        public class CreateAccidentRequest(AccidentDto AccidentDto) : IRequest<AccidentDto>
        {
            public AccidentDto AccidentDto { get; set; } = AccidentDto;
        }

        public class CreateListAccidentRequest(List<AccidentDto> AccidentDtos) : IRequest<List<AccidentDto>>
        {
            public List<AccidentDto> AccidentDtos { get; set; } = AccidentDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateAccidentRequest(AccidentDto AccidentDto) : IRequest<AccidentDto>
        {
            public AccidentDto AccidentDto { get; set; } = AccidentDto;
        }

        public class UpdateListAccidentRequest(List<AccidentDto> AccidentDtos) : IRequest<List<AccidentDto>>
        {
            public List<AccidentDto> AccidentDtos { get; set; } = AccidentDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteAccidentRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
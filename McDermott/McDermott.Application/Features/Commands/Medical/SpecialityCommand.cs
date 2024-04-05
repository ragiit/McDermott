namespace McDermott.Application.Features.Commands.Medical
{
    public class SpecialityCommand
    {
        #region GET 

        public class GetSpecialityQuery(Expression<Func<Speciality, bool>>? predicate = null, bool removeCache = false) : IRequest<List<SpecialityDto>>
        {
            public Expression<Func<Speciality, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion

        #region CREATE

        public class CreateSpecialityRequest(SpecialityDto SpecialityDto) : IRequest<SpecialityDto>
        {
            public SpecialityDto SpecialityDto { get; set; } = SpecialityDto;
        }

        public class CreateListSpecialityRequest(List<SpecialityDto> SpecialityDtos) : IRequest<List<SpecialityDto>>
        {
            public List<SpecialityDto> SpecialityDtos { get; set; } = SpecialityDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateSpecialityRequest(SpecialityDto SpecialityDto) : IRequest<SpecialityDto>
        {
            public SpecialityDto SpecialityDto { get; set; } = SpecialityDto;
        }

        public class UpdateListSpecialityRequest(List<SpecialityDto> SpecialityDtos) : IRequest<List<SpecialityDto>>
        {
            public List<SpecialityDto> SpecialityDtos { get; set; } = SpecialityDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteSpecialityRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
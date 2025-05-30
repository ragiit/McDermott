namespace McDermott.Application.Features.Commands.Medical
{
    public class SpecialityCommand
    {
        #region GET

        public class GetSpecialityQuery(Expression<Func<Speciality, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Speciality, object>>>? includes = null, Expression<Func<Speciality, Speciality>>? select = null) : IRequest<(List<SpecialityDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Speciality, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<Speciality, object>>> Includes { get; } = includes!;
            public Expression<Func<Speciality, Speciality>>? Select { get; } = select!;
        }

        public class ValidateSpecialityQuery(Expression<Func<Speciality, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Speciality, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateSpecialityRequest(SpecialityDto SpecialityDto) : IRequest<SpecialityDto>
        {
            public SpecialityDto SpecialityDto { get; set; } = SpecialityDto;
        }

        public class BulkValidateSpecialityQuery(List<SpecialityDto> SpecialitysToValidate) : IRequest<List<SpecialityDto>>
        {
            public List<SpecialityDto> SpecialitysToValidate { get; } = SpecialitysToValidate;
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
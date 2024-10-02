namespace McDermott.Application.Features.Commands.Patient
{
    public class FamilyRelationCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetFamilyQuery(Expression<Func<Family, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Family, object>>>? includes = null, Expression<Func<Family, Family>>? select = null) : IRequest<(List<FamilyDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Family, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<Family, object>>> Includes { get; } = includes!;
            public Expression<Func<Family, Family>>? Select { get; } = select!;
        }

        public class BulkValidateFamilyQuery(List<FamilyDto> FamilysToValidate) : IRequest<List<FamilyDto>>
        {
            public List<FamilyDto> FamilysToValidate { get; } = FamilysToValidate;
        }

        public class ValidateFamilyQuery(Expression<Func<Family, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Family, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateFamilyRequest(FamilyDto FamilyDto) : IRequest<FamilyDto>
        {
            public FamilyDto FamilyDto { get; set; } = FamilyDto;
        }

        public class CreateListFamilyRequest(List<FamilyDto> GeneralConsultanCPPTDtos) : IRequest<List<FamilyDto>>
        {
            public List<FamilyDto> FamilyDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateFamilyRequest(FamilyDto FamilyDto) : IRequest<FamilyDto>
        {
            public FamilyDto FamilyDto { get; set; } = FamilyDto;
        }

        public class UpdateListFamilyRequest(List<FamilyDto> FamilyDtos) : IRequest<List<FamilyDto>>
        {
            public List<FamilyDto> FamilyDtos { get; set; } = FamilyDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteFamilyRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
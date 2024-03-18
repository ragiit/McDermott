namespace McDermott.Application.Features.Commands.Patient
{
    public class FamilyRelationCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetFamilyQuery(Expression<Func<Family, bool>>? predicate = null) : IRequest<List<FamilyDto>>
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
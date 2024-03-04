namespace McDermott.Application.Features.Commands.Patient
{
    public class FamilyRelationCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)
        public class GetFamilyQuery(Expression<Func<Family, bool>>? predicate = null) : IRequest<List<FamilyDto>>
        {
            public Expression<Func<Family, bool>> Predicate { get; } = predicate!;
        }
        #endregion

        #region CREATE
        public class CreateFamilyRequest(FamilyDto FamilyDto) : IRequest<FamilyDto>
        {
            public FamilyDto FamilyDto { get; set; } = FamilyDto;
        }

        public class CreateListFamilyRequest(List<FamilyDto> GeneralConsultanCPPTDtos) : IRequest<List<FamilyDto>>
        {
            public List<FamilyDto> FamilyDtos { get; set; } = GeneralConsultanCPPTDtos;
        }
        #endregion

        #region Update
        public class UpdateFamilyRequest(FamilyDto FamilyDto) : IRequest<FamilyDto>
        {
            public FamilyDto FamilyDto { get; set; } = FamilyDto;
        }

        public class UpdateListFamilyRequest(List<FamilyDto> FamilyDtos) : IRequest<List<FamilyDto>>
        {
            public List<FamilyDto> FamilyDtos { get; set; } = FamilyDtos;
        }

        #endregion

        #region DELETE 
        public class DeleteFamilyRequest(int id = 0, List<int>? ids = null) : IRequest<bool>
        {
            public int Id { get; set; } = id;
            public List<int> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}
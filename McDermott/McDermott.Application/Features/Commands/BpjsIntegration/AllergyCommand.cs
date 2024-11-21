namespace McDermott.Application.Features.Commands.BpjsIntegration
{
    public class AllergyCommand
    {
        #region GET

        public class GetAllergyQuery(Expression<Func<Allergy, bool>>? predicate = null, bool removeCache = false) : IRequest<List<AllergyDto>>
        {
            public Expression<Func<Allergy, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateAllergyRequest(AllergyDto AllergyDto) : IRequest<AllergyDto>
        {
            public AllergyDto AllergyDto { get; set; } = AllergyDto;
        }

        public class CreateListAllergyRequest(List<AllergyDto> AllergyDtos) : IRequest<List<AllergyDto>>
        {
            public List<AllergyDto> AllergyDtos { get; set; } = AllergyDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateAllergyRequest(AllergyDto AllergyDto) : IRequest<AllergyDto>
        {
            public AllergyDto AllergyDto { get; set; } = AllergyDto;
        }

        public class UpdateListAllergyRequest(List<AllergyDto> AllergyDtos) : IRequest<List<AllergyDto>>
        {
            public List<AllergyDto> AllergyDtos { get; set; } = AllergyDtos;
        }

        public class UpdateToDbAllergyRequest(List<AllergyDto> AllergyDtos) : IRequest<List<AllergyDto>>
        {
            public List<AllergyDto> AllergyDtos { get; set; } = AllergyDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteAllergyRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        public class DeleteAllAllergy() : IRequest<bool>
        {
        }

        #endregion DELETE
    }
}
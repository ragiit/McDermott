namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class PharmacyCommand
    {
        #region GET 

        public class GetPharmacyQuery(Expression<Func<Domain.Entities.Pharmacy, bool>>? predicate = null, bool removeCache = false) : IRequest<List<PharmacyDto>>
        {
            public Expression<Func<Domain.Entities.Pharmacy, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreatePharmacyRequest(PharmacyDto PharmacyDto) : IRequest<PharmacyDto>
        {
            public PharmacyDto PharmacyDto { get; set; } = PharmacyDto;
        }

        public class CreateListPharmacyRequest(List<PharmacyDto> PharmacyDtos) : IRequest<List<PharmacyDto>>
        {
            public List<PharmacyDto> PharmacyDtos { get; set; } = PharmacyDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdatePharmacyRequest(PharmacyDto PharmacyDto) : IRequest<PharmacyDto>
        {
            public PharmacyDto PharmacyDto { get; set; } = PharmacyDto;
        }

        public class UpdateListPharmacyRequest(List<PharmacyDto> PharmacyDtos) : IRequest<List<PharmacyDto>>
        {
            public List<PharmacyDto> PharmacyDtos { get; set; } = PharmacyDtos;
        }

        #endregion Update

        #region DELETE

        public class DeletePharmacyRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class PharmacyCommand
    {
        #region Pharmacy
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
        #endregion

        #region PharmacyLog
        #region Get
        public class GetPharmacyLogQuery(Expression<Func<PharmacyLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<PharmacyLogDto>>
        {
            public Expression<Func<PharmacyLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        #endregion

        #region Create
        public class CreatePharmacyLogRequest(PharmacyLogDto PharmacyLogDto) : IRequest<PharmacyLogDto>
        {
            public PharmacyLogDto PharmacyLogDto { get; set; } = PharmacyLogDto;
        }

        public class CreateListPharmacyLogRequest(List<PharmacyLogDto> PharmacyLogDtos) : IRequest<List<PharmacyLogDto>>
        {
            public List<PharmacyLogDto> PharmacyLogDtos { get; set; } = PharmacyLogDtos;
        }

        #endregion

        #region Update
        public class UpdatePharmacyLogRequest(PharmacyLogDto PharmacyLogDto) : IRequest<PharmacyLogDto>
        {
            public PharmacyLogDto PharmacyLogDto { get; set; } = PharmacyLogDto;
        }

        public class UpdateListPharmacyLogRequest(List<PharmacyLogDto> PharmacyLogDtos) : IRequest<List<PharmacyLogDto>>
        {
            public List<PharmacyLogDto> PharmacyLogDtos { get; set; } = PharmacyLogDtos;
        }
        #endregion

        #region Delete
        public class DeletePharmacyLogRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion

        #endregion
    }
}

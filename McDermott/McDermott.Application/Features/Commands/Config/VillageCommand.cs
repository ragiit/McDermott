using McDermott.Application.Dtos;

namespace McDermott.Application.Features.Commands.Config
{
    public class VillageCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetVillageQuery(Expression<Func<Village, bool>>? predicate = null, bool removeCache = false) : IRequest<List<VillageDto>>
        {
            public Expression<Func<Village, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetVillageQuery2(Expression<Func<Village, bool>>? predicate = null, bool removeCache = false) : IRequest<IQueryable<VillageDto>>
        {
            public Expression<Func<Village, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetPagedDataQuery : IRequest<(List<VillageDto> Data, int TotalCount)>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public Expression<Func<Village, bool>>? Predicate { get; set; }
        }

        public class GetVillageQuerylable : IRequest<IQueryable<Village>>
        {
        }

        public class GetDistrictsQuery : IRequest<PaginatedList<VillageDto>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateVillageRequest(VillageDto VillageDto) : IRequest<VillageDto>
        {
            public VillageDto VillageDto { get; set; } = VillageDto;
        }

        public class CreateListVillageRequest(List<VillageDto> GeneralConsultanCPPTDtos) : IRequest<List<VillageDto>>
        {
            public List<VillageDto> VillageDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateVillageRequest(VillageDto VillageDto) : IRequest<VillageDto>
        {
            public VillageDto VillageDto { get; set; } = VillageDto;
        }

        public class UpdateListVillageRequest(List<VillageDto> VillageDtos) : IRequest<List<VillageDto>>
        {
            public List<VillageDto> VillageDtos { get; set; } = VillageDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteVillageRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
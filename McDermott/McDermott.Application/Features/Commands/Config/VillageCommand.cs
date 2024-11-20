using McDermott.Application.Dtos;

namespace McDermott.Application.Features.Commands.Config
{
    public class VillageCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetVillageQuery(Expression<Func<Village, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Village, object>>>? includes = null, Expression<Func<Village, Village>>? select = null) : IRequest<(List<VillageDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Village, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<Village, object>>> Includes { get; } = includes!;
            public Expression<Func<Village, Village>>? Select { get; } = select!;
        }

        public class GetVillageQueryNew : IRequest<(List<VillageDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Village, object>>> Includes { get; set; }
            public Expression<Func<Village, bool>> Predicate { get; set; }
            public Expression<Func<Village, Village>> Select { get; set; }

            public List<(Expression<Func<Village, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateVillageQuery(Expression<Func<Village, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Village, bool>> Predicate { get; } = predicate!;
        }

        public class BulkValidateVillageQuery(List<VillageDto> villagesToValidate) : IRequest<List<VillageDto>>
        {
            public List<VillageDto> VillagesToValidate { get; } = villagesToValidate;
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
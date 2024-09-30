namespace McDermott.Application.Features.Commands.Config
{
    public class DistrictCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetDistrictQuery(Expression<Func<District, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<District, object>>>? includes = null, Expression<Func<District, District>>? select = null) : IRequest<(List<DistrictDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<District, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<District, object>>> Includes { get; } = includes!;
            public Expression<Func<District, District>>? Select { get; } = select!;
        }

        public class BulkValidateDistrictQuery(List<DistrictDto> DistrictsToValidate) : IRequest<List<DistrictDto>>
        {
            public List<DistrictDto> DistrictsToValidate { get; } = DistrictsToValidate;
        }

        public class ValidateDistrictQuery(Expression<Func<District, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<District, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateDistrictRequest(DistrictDto DistrictDto) : IRequest<DistrictDto>
        {
            public DistrictDto DistrictDto { get; set; } = DistrictDto;
        }

        public class CreateListDistrictRequest(List<DistrictDto> GeneralConsultanCPPTDtos) : IRequest<List<DistrictDto>>
        {
            public List<DistrictDto> DistrictDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDistrictRequest(DistrictDto DistrictDto) : IRequest<DistrictDto>
        {
            public DistrictDto DistrictDto { get; set; } = DistrictDto;
        }

        public class UpdateListDistrictRequest(List<DistrictDto> DistrictDtos) : IRequest<List<DistrictDto>>
        {
            public List<DistrictDto> DistrictDtos { get; set; } = DistrictDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDistrictRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
namespace McDermott.Application.Features.Commands.Config
{
    public class DistrictCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleDistrictQuery : IRequest<DistrictDto>
        {
            public List<Expression<Func<District, object>>> Includes { get; set; }
            public Expression<Func<District, bool>> Predicate { get; set; }
            public Expression<Func<District, District>> Select { get; set; }

            public List<(Expression<Func<District, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetDistrictQuery : IRequest<(List<DistrictDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<District, object>>> Includes { get; set; }
            public Expression<Func<District, bool>> Predicate { get; set; }
            public Expression<Func<District, District>> Select { get; set; }

            public List<(Expression<Func<District, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
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
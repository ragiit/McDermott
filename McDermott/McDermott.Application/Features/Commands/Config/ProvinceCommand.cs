namespace McDermott.Application.Features.Commands.Config
{
    public class ProvinceCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleProvinceQuery : IRequest<ProvinceDto>
        {
            public List<Expression<Func<Province, object>>> Includes { get; set; }
            public Expression<Func<Province, bool>> Predicate { get; set; }
            public Expression<Func<Province, Province>> Select { get; set; }

            public List<(Expression<Func<Province, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetProvinceQuery : IRequest<(List<ProvinceDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Province, object>>> Includes { get; set; }
            public Expression<Func<Province, bool>> Predicate { get; set; }
            public Expression<Func<Province, Province>> Select { get; set; }

            public List<(Expression<Func<Province, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateProvinceQuery(List<ProvinceDto> ProvincesToValidate) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> ProvincesToValidate { get; } = ProvincesToValidate;
        }

        public class ValidateProvinceQuery(Expression<Func<Province, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Province, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateProvinceRequest(ProvinceDto ProvinceDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto ProvinceDto { get; set; } = ProvinceDto;
        }

        public class CreateListProvinceRequest(List<ProvinceDto> GeneralConsultanCPPTDtos) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> ProvinceDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateProvinceRequest(ProvinceDto ProvinceDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto ProvinceDto { get; set; } = ProvinceDto;
        }

        public class UpdateListProvinceRequest(List<ProvinceDto> ProvinceDtos) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> ProvinceDtos { get; set; } = ProvinceDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteProvinceRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
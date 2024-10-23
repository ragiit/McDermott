namespace McDermott.Application.Features.Commands.Medical
{
    public class LabTestDetailCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetAllLabTestDetailQuery(Expression<Func<LabTestDetail, bool>>? predicate = null, bool removeCache = false) : IRequest<List<LabTestDetailDto>>
        {
            public Expression<Func<LabTestDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetSingleLabTestDetailQuery : IRequest<LabTestDetailDto>
        {
            public List<Expression<Func<LabTestDetail, object>>> Includes { get; set; }
            public Expression<Func<LabTestDetail, bool>> Predicate { get; set; }
            public Expression<Func<LabTestDetail, LabTestDetail>> Select { get; set; }

            public List<(Expression<Func<LabTestDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetLabTestDetailQuery : IRequest<(List<LabTestDetailDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<LabTestDetail, object>>> Includes { get; set; }
            public Expression<Func<LabTestDetail, bool>> Predicate { get; set; }
            public Expression<Func<LabTestDetail, LabTestDetail>> Select { get; set; }

            public List<(Expression<Func<LabTestDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateLabTestDetailQuery(Expression<Func<LabTestDetail, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<LabTestDetail, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateLabTestDetailRequest(LabTestDetailDto LabTestDetailDto) : IRequest<LabTestDetailDto>
        {
            public LabTestDetailDto LabTestDetailDto { get; set; } = LabTestDetailDto;
        }

        public class CreateListLabTestDetailRequest(List<LabTestDetailDto> GeneralConsultanCPPTDtos) : IRequest<List<LabTestDetailDto>>
        {
            public List<LabTestDetailDto> LabTestDetailDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateLabTestDetailRequest(LabTestDetailDto LabTestDetailDto) : IRequest<LabTestDetailDto>
        {
            public LabTestDetailDto LabTestDetailDto { get; set; } = LabTestDetailDto;
        }

        public class UpdateListLabTestDetailRequest(List<LabTestDetailDto> LabTestDetailDtos) : IRequest<List<LabTestDetailDto>>
        {
            public List<LabTestDetailDto> LabTestDetailDtos { get; set; } = LabTestDetailDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteLabTestDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
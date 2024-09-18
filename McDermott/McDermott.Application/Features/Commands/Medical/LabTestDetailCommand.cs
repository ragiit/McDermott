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
        public class GetLabTestDetailQuery(Expression<Func<LabTestDetail, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<LabTestDetailDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<LabTestDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
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

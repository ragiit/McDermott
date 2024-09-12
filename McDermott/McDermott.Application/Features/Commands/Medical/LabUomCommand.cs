namespace McDermott.Application.Features.Commands.Medical
{
    public class LabUomCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetLabUomQuery(Expression<Func<LabUom, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<LabUomDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<LabUom, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateLabUomQuery(Expression<Func<LabUom, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<LabUom, bool>> Predicate { get; } = predicate!;
        }


        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateLabUomRequest(LabUomDto LabUomDto) : IRequest<LabUomDto>
        {
            public LabUomDto LabUomDto { get; set; } = LabUomDto;
        }

        public class CreateListLabUomRequest(List<LabUomDto> GeneralConsultanCPPTDtos) : IRequest<List<LabUomDto>>
        {
            public List<LabUomDto> LabUomDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateLabUomRequest(LabUomDto LabUomDto) : IRequest<LabUomDto>
        {
            public LabUomDto LabUomDto { get; set; } = LabUomDto;
        }

        public class UpdateListLabUomRequest(List<LabUomDto> LabUomDtos) : IRequest<List<LabUomDto>>
        {
            public List<LabUomDto> LabUomDtos { get; set; } = LabUomDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteLabUomRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

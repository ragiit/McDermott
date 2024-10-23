namespace McDermott.Application.Features.Commands.Medical
{
    public class LabUomCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleLabUomQuery : IRequest<LabUomDto>
        {
            public List<Expression<Func<LabUom, object>>> Includes { get; set; }
            public Expression<Func<LabUom, bool>> Predicate { get; set; }
            public Expression<Func<LabUom, LabUom>> Select { get; set; }

            public List<(Expression<Func<LabUom, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetLabUomQuery : IRequest<(List<LabUomDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<LabUom, object>>> Includes { get; set; }
            public Expression<Func<LabUom, bool>> Predicate { get; set; }
            public Expression<Func<LabUom, LabUom>> Select { get; set; }

            public List<(Expression<Func<LabUom, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateLabUomQuery(List<LabUomDto> LabUomsToValidate) : IRequest<List<LabUomDto>>
        {
            public List<LabUomDto> LabUomsToValidate { get; } = LabUomsToValidate;
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
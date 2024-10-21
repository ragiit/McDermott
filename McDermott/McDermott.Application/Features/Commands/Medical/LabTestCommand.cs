namespace McDermott.Application.Features.Commands.Medical
{
    public class LabTestCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleLabTestQuery : IRequest<LabTestDto>
        {
            public List<Expression<Func<LabTest, object>>> Includes { get; set; }
            public Expression<Func<LabTest, bool>> Predicate { get; set; }
            public Expression<Func<LabTest, LabTest>> Select { get; set; }

            public List<(Expression<Func<LabTest, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetLabTestQuery : IRequest<(List<LabTestDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<LabTest, object>>> Includes { get; set; }
            public Expression<Func<LabTest, bool>> Predicate { get; set; }
            public Expression<Func<LabTest, LabTest>> Select { get; set; }

            public List<(Expression<Func<LabTest, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateLabTestQuery(List<LabTestDto> LabTestsToValidate) : IRequest<List<LabTestDto>>
        {
            public List<LabTestDto> LabTestsToValidate { get; } = LabTestsToValidate;
        }

        public class ValidateLabTestQuery(Expression<Func<LabTest, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<LabTest, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateLabTestRequest(LabTestDto LabTestDto) : IRequest<LabTestDto>
        {
            public LabTestDto LabTestDto { get; set; } = LabTestDto;
        }

        public class CreateListLabTestRequest(List<LabTestDto> GeneralConsultanCPPTDtos) : IRequest<List<LabTestDto>>
        {
            public List<LabTestDto> LabTestDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateLabTestRequest(LabTestDto LabTestDto) : IRequest<LabTestDto>
        {
            public LabTestDto LabTestDto { get; set; } = LabTestDto;
        }

        public class UpdateListLabTestRequest(List<LabTestDto> LabTestDtos) : IRequest<List<LabTestDto>>
        {
            public List<LabTestDto> LabTestDtos { get; set; } = LabTestDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteLabTestRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
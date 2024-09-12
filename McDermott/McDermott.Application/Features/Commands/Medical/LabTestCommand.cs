
namespace McDermott.Application.Features.Commands.Medical
{
    public class LabTestCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetLabTestQuery(Expression<Func<LabTest, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<LabTestDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<LabTest, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
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

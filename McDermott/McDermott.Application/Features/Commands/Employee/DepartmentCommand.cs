namespace McDermott.Application.Features.Commands.Employee
{
    public class DepartmentCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetDepartmentQuery(Expression<Func<Department, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DepartmentDto>>
        {
            public Expression<Func<Department, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateDepartmentRequest(DepartmentDto DepartmentDto) : IRequest<DepartmentDto>
        {
            public DepartmentDto DepartmentDto { get; set; } = DepartmentDto;
        }

        public class CreateListDepartmentRequest(List<DepartmentDto> GeneralConsultanCPPTDtos) : IRequest<List<DepartmentDto>>
        {
            public List<DepartmentDto> DepartmentDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDepartmentRequest(DepartmentDto DepartmentDto) : IRequest<DepartmentDto>
        {
            public DepartmentDto DepartmentDto { get; set; } = DepartmentDto;
        }

        public class UpdateListDepartmentRequest(List<DepartmentDto> DepartmentDtos) : IRequest<List<DepartmentDto>>
        {
            public List<DepartmentDto> DepartmentDtos { get; set; } = DepartmentDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDepartmentRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
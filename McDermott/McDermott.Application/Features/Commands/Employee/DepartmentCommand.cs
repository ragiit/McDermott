namespace McDermott.Application.Features.Commands.Employee
{
    public class DepartmentCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetDepartmentQuery(Expression<Func<Department, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Department, object>>>? includes = null, Expression<Func<Department, Department>>? select = null) : IRequest<(List<DepartmentDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Department, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<Department, object>>> Includes { get; } = includes!;
            public Expression<Func<Department, Department>>? Select { get; } = select!;
        }

        public class BulkValidateDepartmentQuery(List<DepartmentDto> DepartmentsToValidate) : IRequest<List<DepartmentDto>>
        {
            public List<DepartmentDto> DepartmentsToValidate { get; } = DepartmentsToValidate;
        }

        public class ValidateDepartmentQuery(Expression<Func<Department, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Department, bool>> Predicate { get; } = predicate!;
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
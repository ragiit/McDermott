namespace McDermott.Application.Features.Commands.Employee
{
    public class DepartmentCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleDepartmentQuery : IRequest<DepartmentDto>
        {
            public List<Expression<Func<Department, object>>> Includes { get; set; }
            public Expression<Func<Department, bool>> Predicate { get; set; }
            public Expression<Func<Department, Department>> Select { get; set; }

            public List<(Expression<Func<Department, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetDepartmentQuery : IRequest<(List<DepartmentDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Department, object>>> Includes { get; set; }
            public Expression<Func<Department, bool>> Predicate { get; set; }
            public Expression<Func<Department, Department>> Select { get; set; }

            public List<(Expression<Func<Department, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
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
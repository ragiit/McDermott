using McDermott.Application.Dtos.Employee;

namespace McDermott.Application.Features.Commands.Employee
{
    public class DepartmentCommand
    {
        public class GetDepartmentQuery : IRequest<List<DepartmentDto>>;

        public class GetDepartmentByIdQuery : IRequest<DepartmentDto>
        {
            public int Id { get; set; }

            public GetDepartmentByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateDepartmentRequest : IRequest<DepartmentDto>
        {
            public DepartmentDto DepartmentDto { get; set; }

            public CreateDepartmentRequest(DepartmentDto DepartmentDto)
            {
                this.DepartmentDto = DepartmentDto;
            }
        }

        public class UpdateDepartmentRequest : IRequest<bool>
        {
            public DepartmentDto DepartmentDto { get; set; }

            public UpdateDepartmentRequest(DepartmentDto DepartmentDto)
            {
                this.DepartmentDto = DepartmentDto;
            }
        }

        public class DeleteDepartmentRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteDepartmentRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListDepartmentRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListDepartmentRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
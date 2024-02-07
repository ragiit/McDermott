using McDermott.Application.Dtos.Employee;
using static McDermott.Application.Features.Commands.Employee.DepartmentCommand;

namespace McDermott.Application.Features.Queries.Employee
{
    public class DepartmentQueryHandler
    {
        internal class GetAllDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, List<DepartmentDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllDepartmentQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DepartmentDto>> Handle(GetDepartmentQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Department>().Entities
                    .Include(x => x.Company)
                        .Select(Department => Department.Adapt<DepartmentDto>())
                        .AsNoTracking()
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDepartmentByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Department>().GetByIdAsync(request.Id);

                return result.Adapt<DepartmentDto>();
            }
        }

        internal class CreateDepartmentHandler : IRequestHandler<CreateDepartmentRequest, DepartmentDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateDepartmentHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DepartmentDto> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Department>().AddAsync(request.DepartmentDto.Adapt<Department>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<DepartmentDto>();
            }
        }

        internal class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateDepartmentHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Department>().UpdateAsync(request.DepartmentDto.Adapt<Department>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDepartmentHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Department>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListDepartmentHandler : IRequestHandler<DeleteListDepartmentRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListDepartmentHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListDepartmentRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Department>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
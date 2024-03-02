namespace McDermott.Application.Features.Queries.Employee
{
    public class EmployeeQueryHandler
    {
        #region Get

        internal class GetAllUserEmployeeQueryHandler : IRequestHandler<GetUserEmployeeQuery, List<UserDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllUserEmployeeQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<UserDto>> Handle(GetUserEmployeeQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<User>().Entities
                        .Include(x => x.JobPosition)
                        .Include(x => x.Department)
                        .Where(x => x.IsEmployee == true)
                        .Select(User => User.Adapt<UserDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        #endregion Get
    }
}
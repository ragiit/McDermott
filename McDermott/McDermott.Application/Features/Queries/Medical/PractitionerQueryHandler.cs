namespace McDermott.Application.Features.Queries.Medical
{
    public class PractitionerQueryHandler
    {
        #region Get
        internal class GetAllUserEmployeeQueryHandler : IRequestHandler<GetUserPractitionerQuery, List<UserDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllUserEmployeeQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<UserDto>> Handle(GetUserPractitionerQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<User>().Entities
                        .Include(x => x.JobPosition)
                        .Include(x => x.Department)
                        .Where(x => x.IsDoctor == true)
                        .Select(User => User.Adapt<UserDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }
        #endregion

    }
}



namespace McDermott.Application.Features.Queries.Patient
{
    public class PatientQueryHandler
    {
        #region Get
        internal class GetAllUserEmployeeQueryHandler : IRequestHandler<GetUserPatientQuery, List<UserDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllUserEmployeeQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<UserDto>> Handle(GetUserPatientQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<User>().Entities
                        .Include(x => x.JobPosition)
                        .Include(x => x.Department)
                        .Where(x => x.IsPatient == true)
                        .Select(User => User.Adapt<UserDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }
        #endregion

    }
}

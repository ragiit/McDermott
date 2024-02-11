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
                        .Include(x => x.Department)
                        .Where(x => x.IsPatient == true)
                        .Select(User => User.Adapt<UserDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }
        #endregion

        #region Create
        internal class CreatePatientAllergyHandler : IRequestHandler<CreatePatientAllergyRequest, PatientAllergyDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreatePatientAllergyHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<PatientAllergyDto> Handle(CreatePatientAllergyRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<PatientAllergy>().AddAsync(request.PatientAllergyDto.Adapt<PatientAllergy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<PatientAllergyDto>();
            }
        }

        #endregion
    }
}

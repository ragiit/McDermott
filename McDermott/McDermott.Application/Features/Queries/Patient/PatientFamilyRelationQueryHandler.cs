

namespace McDermott.Application.Features.Queries.Patient
{
    public class PatientFamilyRelationQueryHandler
    {
        internal class GetPatientFamilyByPatientIdQueryHandler : IRequestHandler<GetPatientFamilyByPatientQuery, List<PatientFamilyRelationDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetPatientFamilyByPatientIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<PatientFamilyRelationDto>> Handle(GetPatientFamilyByPatientQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<PatientFamilyRelation>().GetAsync(
                    request.Predicate, 
                        x => x
                        .Include(z => z.FamilyMember)
                        .Include(z => z.Family)
                        .Include(z => z.Patient), 
                    cancellationToken
                    );

                return result.Adapt<List<PatientFamilyRelationDto>>();
            }
        }
    }
}

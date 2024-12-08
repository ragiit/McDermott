using static McDermott.Application.Features.Commands.AllQueries.CountModelCommand;

namespace McDermott.Application.Features.Queries.AllQueries
{
    public class CountModelQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInsurancePolicyCountQuery, int>,
        IRequestHandler<GetPrescriptionCountQuery, int>,
        IRequestHandler<GetWellnessAttendanceCountQuery, int>,
        IRequestHandler<GetClaimHistoryCountQuery, int>,
        IRequestHandler<GetGeneralConsultationCountQuery, int>
    {
        public async Task<int> Handle(GetWellnessAttendanceCountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = await _unitOfWork.Repository<WellnessProgramAttendance>().Entities
                   .AsNoTracking()
                   .Where(request.Predicate)
                   .CountAsync(cancellationToken);

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Handle(GetInsurancePolicyCountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = await _unitOfWork.Repository<InsurancePolicy>().Entities
                   .AsNoTracking()
                   .Where(request.Predicate)
                   .CountAsync(cancellationToken);

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Handle(GetClaimHistoryCountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = await _unitOfWork.Repository<ClaimHistory>().Entities
                   .AsNoTracking()
                   .Where(request.Predicate)
                   .CountAsync(cancellationToken);

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Handle(GetPrescriptionCountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = await _unitOfWork.Repository<Domain.Entities.Pharmacy>().Entities
                   .AsNoTracking()
                   .Where(request.Predicate)
                   .CountAsync(cancellationToken);

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Handle(GetGeneralConsultationCountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = await _unitOfWork.Repository<GeneralConsultanService>().Entities
                   .AsNoTracking()
                   .Where(request.Predicate)
                   .CountAsync(cancellationToken);

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
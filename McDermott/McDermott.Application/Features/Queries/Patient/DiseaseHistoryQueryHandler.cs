using static McDermott.Application.Features.Commands.Patient.DiseaseHistoryCommand;

namespace McDermott.Application.Features.Queries.Patient
{
    internal class DiseaseHistoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
            IRequestHandler<GetDiseaseHistoryQuery, (List<DiseaseHistoryTemp>, int pageIndex, int pageSize, int pageCount)>
    {
        public async Task<(List<DiseaseHistoryTemp>, int pageIndex, int pageSize, int pageCount)> Handle(GetDiseaseHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanCPPT>()
                    .Entities
                    .AsNoTracking()
                    .Where(x =>
                        x.DiagnosisId != null &&
                        x.Diagnosis.CronisCategoryId != null &&
                        (
                        EF.Functions.Like(x.GeneralConsultanService.Patient.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(x.Diagnosis.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(x.Diagnosis.Code, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(x.GeneralConsultanService.Pratitioner.Name, $"%{request.SearchTerm}%"))
                    )
                    .OrderByDescending(x => x.DateTime)
                    .Select(x => new DiseaseHistoryTemp
                    {
                        Id = x.Id,
                        Patient = x.GeneralConsultanService.Patient.Name,
                        Disease = $"{x.Diagnosis.Code} - {x.Diagnosis.Name}",
                        Physician = x.GeneralConsultanService.Pratitioner.Name,
                        DiseaseDate = x.DateTime,
                        CronisCategory = x.Diagnosis.CronisCategory.Name,
                        Reference = x.GeneralConsultanService.Reference
                    });

                var totalCount = await query.CountAsync(cancellationToken);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
                var pagedItems = await query
                        .Skip((request.PageIndex) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync(cancellationToken);

                return (pagedItems, request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception ex)
            {
                return ([], 0, 1, 1);
            }
        }
    }
}
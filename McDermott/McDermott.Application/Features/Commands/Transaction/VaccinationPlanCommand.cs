namespace McDermott.Application.Features.Commands.Transaction
{
    public class VaccinationPlanCommand
    {
        #region GET

        public class GetVaccinationPlanQuery(Expression<Func<VaccinationPlan, bool>>? predicate = null, bool removeCache = false) : IRequest<List<VaccinationPlanDto>>
        {
            public Expression<Func<VaccinationPlan, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateVaccinationPlanRequest(VaccinationPlanDto VaccinationPlanDto) : IRequest<VaccinationPlanDto>
        {
            public VaccinationPlanDto VaccinationPlanDto { get; set; } = VaccinationPlanDto;
        }

        public class CreateListVaccinationPlanRequest(List<VaccinationPlanDto> GeneralConsultanCPPTDtos) : IRequest<List<VaccinationPlanDto>>
        {
            public List<VaccinationPlanDto> VaccinationPlanDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateVaccinationPlanRequest(VaccinationPlanDto VaccinationPlanDto) : IRequest<VaccinationPlanDto>
        {
            public VaccinationPlanDto VaccinationPlanDto { get; set; } = VaccinationPlanDto;
        }

        public class UpdateListVaccinationPlanRequest(List<VaccinationPlanDto> VaccinationPlanDtos) : IRequest<List<VaccinationPlanDto>>
        {
            public List<VaccinationPlanDto> VaccinationPlanDtos { get; set; } = VaccinationPlanDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteVaccinationPlanRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
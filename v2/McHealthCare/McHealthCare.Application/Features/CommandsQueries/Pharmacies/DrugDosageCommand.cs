namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class DrugDosageCommand
    {
        #region GET

        public class GetDrugDosageQuery(Expression<Func<DrugDosage, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DrugDosageDto>>
        {
            public Expression<Func<DrugDosage, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateDrugDosageRequest(DrugDosageDto DrugDosageDto) : IRequest<DrugDosageDto>
        {
            public DrugDosageDto DrugDosageDto { get; set; } = DrugDosageDto;
        }

        public class CreateListDrugDosageRequest(List<DrugDosageDto> DrugDosageDtos) : IRequest<List<DrugDosageDto>>
        {
            public List<DrugDosageDto> DrugDosageDtos { get; set; } = DrugDosageDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDrugDosageRequest(DrugDosageDto DrugDosageDto) : IRequest<DrugDosageDto>
        {
            public DrugDosageDto DrugDosageDto { get; set; } = DrugDosageDto;
        }

        public class UpdateListDrugDosageRequest(List<DrugDosageDto> DrugDosageDtos) : IRequest<List<DrugDosageDto>>
        {
            public List<DrugDosageDto> DrugDosageDtos { get; set; } = DrugDosageDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDrugDosageRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
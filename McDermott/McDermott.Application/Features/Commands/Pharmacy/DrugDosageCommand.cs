
namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class DrugDosageCommand
    {
        #region GET  

        public class GetDrugDosageQuery(Expression<Func<DrugDosage, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DrugDosageDto>>
        {
            public Expression<Func<DrugDosage, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

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

        public class DeleteDrugDosageRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

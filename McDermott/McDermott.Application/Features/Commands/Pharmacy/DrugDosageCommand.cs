namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class DrugDosageCommand
    {
        #region GET

        public class GetAllDrugDosageQuery(Expression<Func<DrugDosage, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DrugDosageDto>>
        {
            public Expression<Func<DrugDosage, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetDrugDosageQuery(Expression<Func<DrugDosage, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<DrugDosage, object>>>? includes = null, Expression<Func<DrugDosage, DrugDosage>>? select = null) : IRequest<(List<DrugDosageDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<DrugDosage, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<DrugDosage, object>>> Includes { get; } = includes!;
            public Expression<Func<DrugDosage, DrugDosage>>? Select { get; } = select!;
        }

        public class BulkValidateDrugDosageQuery(List<DrugDosageDto> DrugDosageToValidate) : IRequest<List<DrugDosageDto>>
        {
            public List<DrugDosageDto> DrugDosageToValidate { get; } = DrugDosageToValidate;
        }

        public class ValidateDrugDosageQuery(Expression<Func<DrugDosage, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DrugDosage, bool>> Predicate { get; } = predicate!;
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

        public class DeleteDrugDosageRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
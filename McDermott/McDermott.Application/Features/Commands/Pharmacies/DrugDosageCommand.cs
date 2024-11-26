namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class DrugDosageCommand
    {
        #region GET

        public class GetSingleDrugDosageQuery : IRequest<DrugDosageDto>
        {
            public List<Expression<Func<DrugDosage, object>>> Includes { get; set; }
            public Expression<Func<DrugDosage, bool>> Predicate { get; set; }
            public Expression<Func<DrugDosage, DrugDosage>> Select { get; set; }

            public List<(Expression<Func<DrugDosage, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetDrugDosageQuery : IRequest<(List<DrugDosageDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<DrugDosage, object>>> Includes { get; set; }
            public Expression<Func<DrugDosage, bool>> Predicate { get; set; }
            public Expression<Func<DrugDosage, DrugDosage>> Select { get; set; }

            public List<(Expression<Func<DrugDosage, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateDrugDosageQuery(Expression<Func<DrugDosage, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DrugDosage, bool>> Predicate { get; } = predicate!;
        }

        public class BulkValidateDrugDosageQuery(List<DrugDosageDto> DrugDosagesToValidate) : IRequest<List<DrugDosageDto>>
        {
            public List<DrugDosageDto> DrugDosagesToValidate { get; } = DrugDosagesToValidate;
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
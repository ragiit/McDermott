namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class DrugFormCommand
    {
        #region GET

        public class GetSingleDrugFormQuery : IRequest<DrugFormDto>
        {
            public List<Expression<Func<DrugForm, object>>> Includes { get; set; }
            public Expression<Func<DrugForm, bool>> Predicate { get; set; }
            public Expression<Func<DrugForm, DrugForm>> Select { get; set; }

            public List<(Expression<Func<DrugForm, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetDrugFormQuery : IRequest<(List<DrugFormDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<DrugForm, object>>> Includes { get; set; }
            public Expression<Func<DrugForm, bool>> Predicate { get; set; }
            public Expression<Func<DrugForm, DrugForm>> Select { get; set; }

            public List<(Expression<Func<DrugForm, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateDrugFormQuery(Expression<Func<DrugForm, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DrugForm, bool>> Predicate { get; } = predicate!;
        }

        public class BulkValidateDrugFormQuery(List<DrugFormDto> DrugFormsToValidate) : IRequest<List<DrugFormDto>>
        {
            public List<DrugFormDto> DrugFormsToValidate { get; } = DrugFormsToValidate;
        }

        #endregion GET

        #region CREATE

        public class CreateDrugFormRequest(DrugFormDto DrugFormDto) : IRequest<DrugFormDto>
        {
            public DrugFormDto DrugFormDto { get; set; } = DrugFormDto;
        }

        public class CreateListDrugFormRequest(List<DrugFormDto> GeneralConsultanCPPTDtos) : IRequest<List<DrugFormDto>>
        {
            public List<DrugFormDto> DrugFormDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDrugFormRequest(DrugFormDto DrugFormDto) : IRequest<DrugFormDto>
        {
            public DrugFormDto DrugFormDto { get; set; } = DrugFormDto;
        }

        public class UpdateListDrugFormRequest(List<DrugFormDto> DrugFormDtos) : IRequest<List<DrugFormDto>>
        {
            public List<DrugFormDto> DrugFormDtos { get; set; } = DrugFormDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDrugFormRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class FormDrugCommand
    {
        #region GET

        public class GetFormDrugQuery(Expression<Func<DrugForm, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DrugFormDto>>
        {
            public Expression<Func<DrugForm, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateFormDrugRequest(DrugFormDto FormDrugDto) : IRequest<DrugFormDto>
        {
            public DrugFormDto FormDrugDto { get; set; } = FormDrugDto;
        }

        public class CreateListFormDrugRequest(List<DrugFormDto> GeneralConsultanCPPTDtos) : IRequest<List<DrugFormDto>>
        {
            public List<DrugFormDto> FormDrugDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateFormDrugRequest(DrugFormDto FormDrugDto) : IRequest<DrugFormDto>
        {
            public DrugFormDto FormDrugDto { get; set; } = FormDrugDto;
        }

        public class UpdateListFormDrugRequest(List<DrugFormDto> FormDrugDtos) : IRequest<List<DrugFormDto>>
        {
            public List<DrugFormDto> FormDrugDtos { get; set; } = FormDrugDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteFormDrugRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
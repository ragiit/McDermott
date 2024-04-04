using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacy
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

        public class DeleteFormDrugRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

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

        public class GetFormDrugQuery(Expression<Func<FormDrug, bool>>? predicate = null, bool removeCache = false) : IRequest<List<FormDrugDto>>
        {
            public Expression<Func<FormDrug, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateFormDrugRequest(FormDrugDto FormDrugDto) : IRequest<FormDrugDto>
        {
            public FormDrugDto FormDrugDto { get; set; } = FormDrugDto;
        }

        public class CreateListFormDrugRequest(List<FormDrugDto> GeneralConsultanCPPTDtos) : IRequest<List<FormDrugDto>>
        {
            public List<FormDrugDto> FormDrugDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateFormDrugRequest(FormDrugDto FormDrugDto) : IRequest<FormDrugDto>
        {
            public FormDrugDto FormDrugDto { get; set; } = FormDrugDto;
        }

        public class UpdateListFormDrugRequest(List<FormDrugDto> FormDrugDtos) : IRequest<List<FormDrugDto>>
        {
            public List<FormDrugDto> FormDrugDtos { get; set; } = FormDrugDtos;
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

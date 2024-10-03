using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class DrugFormCommand
    {
        #region GET

        public class GetAllDrugFormQuery(Expression<Func<DrugForm, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DrugFormDto>>
        {
            public Expression<Func<DrugForm, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class BulkValidateDrugFormQuery(List<DrugFormDto> DrugFormsToValidate) : IRequest<List<DrugFormDto>>
        {
            public List<DrugFormDto> DrugFormsToValidate { get; } = DrugFormsToValidate;
        }

        public class ValidateDrugFormQuery(Expression<Func<DrugForm, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<DrugForm, bool>> Predicate { get; } = predicate!;
        }

        public class GetDrugFormQuery(Expression<Func<DrugForm, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<DrugForm, object>>>? includes = null, Expression<Func<DrugForm, DrugForm>>? select = null) : IRequest<(List<DrugFormDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<DrugForm, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<DrugForm, object>>> Includes { get; } = includes!;
            public Expression<Func<DrugForm, DrugForm>>? Select { get; } = select!;
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
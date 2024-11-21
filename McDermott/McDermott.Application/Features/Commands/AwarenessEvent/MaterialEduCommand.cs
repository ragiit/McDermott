using McDermott.Application.Dtos.AwarenessEvent;

namespace McDermott.Application.Features.Commands.AwarenessEvent
{
    public class MaterialEduCommand
    {
        #region GET Education Program Detail

        public class GetAllMaterialEduQuery(Expression<Func<MaterialEdu, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MaterialEduDto>>
        {
            public Expression<Func<MaterialEdu, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetSingleMaterialEduQuery : IRequest<MaterialEduDto>
        {
            public List<Expression<Func<MaterialEdu, object>>> Includes { get; set; }
            public Expression<Func<MaterialEdu, bool>> Predicate { get; set; }
            public Expression<Func<MaterialEdu, MaterialEdu>> Select { get; set; }

            public List<(Expression<Func<MaterialEdu, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetMaterialEduQuery : IRequest<(List<MaterialEduDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<MaterialEdu, object>>> Includes { get; set; }
            public Expression<Func<MaterialEdu, bool>> Predicate { get; set; }
            public Expression<Func<MaterialEdu, MaterialEdu>> Select { get; set; }

            public List<(Expression<Func<MaterialEdu, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateMaterialEduQuery(List<MaterialEduDto> MaterialEduToValidate) : IRequest<List<MaterialEduDto>>
        {
            public List<MaterialEduDto> MaterialEduToValidate { get; } = MaterialEduToValidate;
        }

        public class ValidateMaterialEduQuery(Expression<Func<MaterialEdu, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<MaterialEdu, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET Education Program Detail

        #region CREATE Education Program

        public class CreateMaterialEduRequest(MaterialEduDto MaterialEduDto) : IRequest<MaterialEduDto>
        {
            public MaterialEduDto MaterialEduDto { get; set; } = MaterialEduDto;
        }

        public class CreateListMaterialEduRequest(List<MaterialEduDto> MaterialEduDtos) : IRequest<List<MaterialEduDto>>
        {
            public List<MaterialEduDto> MaterialEduDtos { get; set; } = MaterialEduDtos;
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public class UpdateMaterialEduRequest(MaterialEduDto MaterialEduDto) : IRequest<MaterialEduDto>
        {
            public MaterialEduDto MaterialEduDto { get; set; } = MaterialEduDto;
        }

        public class UpdateListMaterialEduRequest(List<MaterialEduDto> MaterialEduDtos) : IRequest<List<MaterialEduDto>>
        {
            public List<MaterialEduDto> MaterialEduDtos { get; set; } = MaterialEduDtos;
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public class DeleteMaterialEduRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Education Program
    }
}
namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class MedicamentGroupCommand
    {
        #region GET

        public class GetMedicamentGroupQuery : IRequest<(List<MedicamentGroupDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<MedicamentGroup, object>>> Includes { get; set; }
            public Expression<Func<MedicamentGroup, bool>> Predicate { get; set; }
            public Expression<Func<MedicamentGroup, MedicamentGroup>> Select { get; set; }
            public Expression<Func<MedicamentGroup, object>> OrderBy { get; set; }
            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateMedicamentGroupQuery(List<MedicamentGroupDto> MedicamentGroupsToValidate) : IRequest<List<MedicamentGroupDto>>
        {
            public List<MedicamentGroupDto> MedicamentGroupsToValidate { get; } = MedicamentGroupsToValidate;
        }

        public class ValidateMedicamentGroupQuery(Expression<Func<MedicamentGroup, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<MedicamentGroup, bool>> Predicate { get; } = predicate!;
        }

        public class GetMedicamentGroupDetailQuery(Expression<Func<MedicamentGroupDetail, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MedicamentGroupDetailDto>>
        {
            public Expression<Func<MedicamentGroupDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateMedicamentGroupRequest(MedicamentGroupDto MedicamentGroupDto) : IRequest<MedicamentGroupDto>
        {
            public MedicamentGroupDto MedicamentGroupDto { get; set; } = MedicamentGroupDto;
        }

        public class CreateListMedicamentGroupRequest(List<MedicamentGroupDto> MedicamentGroupDtos) : IRequest<List<MedicamentGroupDto>>
        {
            public List<MedicamentGroupDto> MedicamentGroupDtos { get; set; } = MedicamentGroupDtos;
        }

        public class CreateMedicamentGroupDetailRequest(MedicamentGroupDetailDto MedicamentGroupDetailDto) : IRequest<MedicamentGroupDetailDto>
        {
            public MedicamentGroupDetailDto MedicamentGroupDetailDto { get; set; } = MedicamentGroupDetailDto;
        }

        public class CreateListMedicamentGroupDetailRequest(List<MedicamentGroupDetailDto> MedicamentGroupDetailDtos) : IRequest<List<MedicamentGroupDetailDto>>
        {
            public List<MedicamentGroupDetailDto> MedicamentGroupDetailDtos { get; set; } = MedicamentGroupDetailDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMedicamentGroupRequest(MedicamentGroupDto MedicamentGroupDto) : IRequest<MedicamentGroupDto>
        {
            public MedicamentGroupDto MedicamentGroupDto { get; set; } = MedicamentGroupDto;
        }

        public class UpdateListMedicamentGroupRequest(List<MedicamentGroupDto> MedicamentGroupDtos) : IRequest<List<MedicamentGroupDto>>
        {
            public List<MedicamentGroupDto> MedicamentGroupDtos { get; set; } = MedicamentGroupDtos;
        }

        public class UpdateMedicamentGroupDetailRequest(MedicamentGroupDetailDto MedicamentGroupDetailDto) : IRequest<MedicamentGroupDetailDto>
        {
            public MedicamentGroupDetailDto MedicamentGroupDetailDto { get; set; } = MedicamentGroupDetailDto;
        }

        public class UpdateListMedicamentGroupDetailRequest(List<MedicamentGroupDetailDto> MedicamentGroupDetailDtos) : IRequest<List<MedicamentGroupDetailDto>>
        {
            public List<MedicamentGroupDetailDto> MedicamentGroupDetailDtos { get; set; } = MedicamentGroupDetailDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMedicamentGroupRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        public class DeleteMedicamentGroupDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
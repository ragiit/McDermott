namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class MedicamentGroupCommand
    {
        #region GET

        public class GetMedicamentGroupQuery(Expression<Func<MedicamentGroup, bool>>? predicate = null, bool removeCache = false) : IRequest<List<MedicamentGroupDto>>
        {
            public Expression<Func<MedicamentGroup, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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

        public class DeleteMedicamentGroupRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        public class DeleteMedicamentGroupDetailRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
namespace McDermott.Application.Features.Commands.Patient
{
    public class ClassTypeCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)
        public class GetClassTypeQuery(Expression<Func<ClassType, bool>>? predicate = null) : IRequest<List<ClassTypeDto>>
        {
            public Expression<Func<ClassType, bool>> Predicate { get; } = predicate!;
        }
        #endregion

        #region CREATE
        public class CreateClassTypeRequest(ClassTypeDto ClassTypeDto) : IRequest<ClassTypeDto>
        {
            public ClassTypeDto ClassTypeDto { get; set; } = ClassTypeDto;
        }

        public class CreateListClassTypeRequest(List<ClassTypeDto> GeneralConsultanCPPTDtos) : IRequest<List<ClassTypeDto>>
        {
            public List<ClassTypeDto> ClassTypeDtos { get; set; } = GeneralConsultanCPPTDtos;
        }
        #endregion

        #region Update
        public class UpdateClassTypeRequest(ClassTypeDto ClassTypeDto) : IRequest<ClassTypeDto>
        {
            public ClassTypeDto ClassTypeDto { get; set; } = ClassTypeDto;
        }

        public class UpdateListClassTypeRequest(List<ClassTypeDto> ClassTypeDtos) : IRequest<List<ClassTypeDto>>
        {
            public List<ClassTypeDto> ClassTypeDtos { get; set; } = ClassTypeDtos;
        }

        #endregion

        #region DELETE 
        public class DeleteClassTypeRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}

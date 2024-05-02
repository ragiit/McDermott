
namespace McDermott.Application.Features.Commands.Bpjs
{
    public class SystemParameterCommand
    {
        #region GET 

        public class GetSystemParameterQuery(Expression<Func<SystemParameter, bool>>? predicate = null, bool removeCache = false) : IRequest<List<SystemParameterDto>>
        {
            public Expression<Func<SystemParameter, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreateSystemParameterRequest(SystemParameterDto SystemParameterDto) : IRequest<SystemParameterDto>
        {
            public SystemParameterDto SystemParameterDto { get; set; } = SystemParameterDto;
        }

        public class CreateListSystemParameterRequest(List<SystemParameterDto> SystemParameterDtos) : IRequest<List<SystemParameterDto>>
        {
            public List<SystemParameterDto> SystemParameterDtos { get; set; } = SystemParameterDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateSystemParameterRequest(SystemParameterDto SystemParameterDto) : IRequest<SystemParameterDto>
        {
            public SystemParameterDto SystemParameterDto { get; set; } = SystemParameterDto;
        }

        public class UpdateListSystemParameterRequest(List<SystemParameterDto> SystemParameterDtos) : IRequest<List<SystemParameterDto>>
        {
            public List<SystemParameterDto> SystemParameterDtos { get; set; } = SystemParameterDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteSystemParameterRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}

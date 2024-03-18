namespace McDermott.Application.Features.Commands.Transaction
{
    public class GeneralConsultanServiceCommand
    {
        #region GET

        public class GetGeneralConsultanServiceQuery(Expression<Func<GeneralConsultanService, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GeneralConsultanServiceDto>>
        {
            public Expression<Func<GeneralConsultanService, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region Create

        public class CreateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto) : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; } = GeneralConsultanServiceDto;
        }

        public class CreateListGeneralConsultanServiceRequest(List<GeneralConsultanServiceDto> GeneralConsultanServiceDtos) : IRequest<List<GeneralConsultanServiceDto>>
        {
            public List<GeneralConsultanServiceDto> GeneralConsultanServiceDtos { get; set; } = GeneralConsultanServiceDtos;
        }

        #endregion Create

        #region Update

        public class UpdateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto) : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; } = GeneralConsultanServiceDto;
        }

        public class UpdateListGeneralConsultanServiceRequest(List<GeneralConsultanServiceDto> GeneralConsultanServiceDtos) : IRequest<List<GeneralConsultanServiceDto>>
        {
            public List<GeneralConsultanServiceDto> GeneralConsultanServiceDtos { get; set; } = GeneralConsultanServiceDtos;
        }

        #endregion Update

        #region Delete

        public class DeleteGeneralConsultanServiceRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}
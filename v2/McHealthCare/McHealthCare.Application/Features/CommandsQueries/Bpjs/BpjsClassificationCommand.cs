namespace McHealthCare.Application.Features.Commands.Bpjs
{
    public class BpjsClassificationCommand
    {
        #region GET

        public class GetBpjsClassificationQuery(Expression<Func<BpjsClassification, bool>>? predicate = null, bool removeCache = false) : IRequest<List<BpjsClassificationDto>>
        {
            public Expression<Func<BpjsClassification, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET



        #region CREATE

        public class CreateBpjsClassificationRequest(BpjsClassificationDto BpjsClassificationDto) : IRequest<BpjsClassificationDto>
        {
            public BpjsClassificationDto BpjsClassificationDto { get; set; } = BpjsClassificationDto;
        }

        public class CreateListBpjsClassificationRequest(List<BpjsClassificationDto> BpjsClassificationDtos) : IRequest<List<BpjsClassificationDto>>
        {
            public List<BpjsClassificationDto> BpjsClassificationDtos { get; set; } = BpjsClassificationDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateBpjsClassificationRequest(BpjsClassificationDto BpjsClassificationDto) : IRequest<BpjsClassificationDto>
        {
            public BpjsClassificationDto BpjsClassificationDto { get; set; } = BpjsClassificationDto;
        }

        public class UpdateListBpjsClassificationRequest(List<BpjsClassificationDto> BpjsClassificationDtos) : IRequest<List<BpjsClassificationDto>>
        {
            public List<BpjsClassificationDto> BpjsClassificationDtos { get; set; } = BpjsClassificationDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteBpjsClassificationRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
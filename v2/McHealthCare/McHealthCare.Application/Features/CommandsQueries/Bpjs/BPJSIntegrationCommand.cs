namespace McHealthCare.Application.Features.Commands.Bpjs
{
    public class BPJSIntegrationCommand
    {
        #region GET

        public class GetBPJSIntegrationQuery(Expression<Func<BPJSIntegration, bool>>? predicate = null, bool removeCache = false) : IRequest<List<BPJSIntegrationDto>>
        {
            public Expression<Func<BPJSIntegration, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET



        #region CREATE

        public class CreateBPJSIntegrationRequest(BPJSIntegrationDto BPJSIntegrationDto) : IRequest<BPJSIntegrationDto>
        {
            public BPJSIntegrationDto BPJSIntegrationDto { get; set; } = BPJSIntegrationDto;
        }

        public class CreateListBPJSIntegrationRequest(List<BPJSIntegrationDto> BPJSIntegrationDtos) : IRequest<List<BPJSIntegrationDto>>
        {
            public List<BPJSIntegrationDto> BPJSIntegrationDtos { get; set; } = BPJSIntegrationDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateBPJSIntegrationRequest(BPJSIntegrationDto BPJSIntegrationDto) : IRequest<BPJSIntegrationDto>
        {
            public BPJSIntegrationDto BPJSIntegrationDto { get; set; } = BPJSIntegrationDto;
        }

        public class UpdateListBPJSIntegrationRequest(List<BPJSIntegrationDto> BPJSIntegrationDtos) : IRequest<List<BPJSIntegrationDto>>
        {
            public List<BPJSIntegrationDto> BPJSIntegrationDtos { get; set; } = BPJSIntegrationDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteBPJSIntegrationRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
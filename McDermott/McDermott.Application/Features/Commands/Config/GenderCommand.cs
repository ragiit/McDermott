namespace McDermott.Application.Features.Commands.Config
{
    public class GenderCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetGenderQuery(Expression<Func<Gender, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GenderDto>>
        {
            public Expression<Func<Gender, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateGenderRequest(GenderDto GenderDto) : IRequest<GenderDto>
        {
            public GenderDto GenderDto { get; set; } = GenderDto;
        }

        public class CreateListGenderRequest(List<GenderDto> GeneralConsultanCPPTDtos) : IRequest<List<GenderDto>>
        {
            public List<GenderDto> GenderDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateGenderRequest(GenderDto GenderDto) : IRequest<GenderDto>
        {
            public GenderDto GenderDto { get; set; } = GenderDto;
        }

        public class UpdateListGenderRequest(List<GenderDto> GenderDtos) : IRequest<List<GenderDto>>
        {
            public List<GenderDto> GenderDtos { get; set; } = GenderDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteGenderRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
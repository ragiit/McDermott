namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class SignaCommand
    {
        #region GET

        public class GetSignaQuery(Expression<Func<Signa, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Signa, object>>>? includes = null, Expression<Func<Signa, Signa>>? select = null) : IRequest<(List<SignaDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Signa, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<Signa, object>>> Includes { get; } = includes!;
            public Expression<Func<Signa, Signa>>? Select { get; } = select!;
        }

        public class BulkValidateSignaQuery(List<SignaDto> SignasToValidate) : IRequest<List<SignaDto>>
        {
            public List<SignaDto> SignasToValidate { get; } = SignasToValidate;
        }

        public class ValidateSignaQuery(Expression<Func<Signa, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Signa, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateSignaRequest(SignaDto SignaDto) : IRequest<SignaDto>
        {
            public SignaDto SignaDto { get; set; } = SignaDto;
        }

        public class CreateListSignaRequest(List<SignaDto> Signas) : IRequest<List<SignaDto>>
        {
            public List<SignaDto> SignaDtos { get; set; } = Signas;
        }

        #endregion CREATE

        #region Update

        public class UpdateSignaRequest(SignaDto SignaDto) : IRequest<SignaDto>
        {
            public SignaDto SignaDto { get; set; } = SignaDto;
        }

        public class UpdateListSignaRequest(List<SignaDto> SignaDtos) : IRequest<List<SignaDto>>
        {
            public List<SignaDto> SignaDtos { get; set; } = SignaDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteSignaRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
using McDermott.Application.Dtos.ClaimUserManagement;

namespace McDermott.Application.Features.Commands.ClaimUserManagement
{
    public class ClaimHistoryCommand
    {
        #region GET Claim History

        public class GetAllClaimHistoryQuery(Expression<Func<ClaimHistory, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ClaimHistoryDto>>
        {
            public Expression<Func<ClaimHistory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetSingleClaimHistoryQuery : IRequest<ClaimHistoryDto>
        {
            public List<Expression<Func<ClaimHistory, object>>> Includes { get; set; }
            public Expression<Func<ClaimHistory, bool>> Predicate { get; set; }
            public Expression<Func<ClaimHistory, ClaimHistory>> Select { get; set; }

            public List<(Expression<Func<ClaimHistory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetClaimHistoryQuery : IRequest<(List<ClaimHistoryDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<ClaimHistory, object>>> Includes { get; set; }
            public Expression<Func<ClaimHistory, bool>> Predicate { get; set; }
            public Expression<Func<ClaimHistory, ClaimHistory>> Select { get; set; }

            public List<(Expression<Func<ClaimHistory, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateClaimHistoryQuery(List<ClaimHistoryDto> ClaimHistoryToValidate) : IRequest<List<ClaimHistoryDto>>
        {
            public List<ClaimHistoryDto> ClaimHistoryToValidate { get; } = ClaimHistoryToValidate;
        }

        public class ValidateClaimHistoryQuery(Expression<Func<ClaimHistory, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<ClaimHistory, bool>> Predicate { get; } = predicate!;
        }

        public class GetClaimHistoryCountQuery(Expression<Func<ClaimHistory, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<ClaimHistory, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET Claim History

        #region CREATE Claim History

        public class CreateClaimHistoryRequest(ClaimHistoryDto ClaimHistoryDto) : IRequest<ClaimHistoryDto>
        {
            public ClaimHistoryDto ClaimHistoryDto { get; set; } = ClaimHistoryDto;
        }

        public class CreateListClaimHistoryRequest(List<ClaimHistoryDto> ClaimHistoryDtos) : IRequest<List<ClaimHistoryDto>>
        {
            public List<ClaimHistoryDto> ClaimHistoryDtos { get; set; } = ClaimHistoryDtos;
        }

        #endregion CREATE Claim History

        #region UPDATE Claim History

        public class UpdateClaimHistoryRequest(ClaimHistoryDto ClaimHistoryDto) : IRequest<ClaimHistoryDto>
        {
            public ClaimHistoryDto ClaimHistoryDto { get; set; } = ClaimHistoryDto;
        }

        public class UpdateListClaimHistoryRequest(List<ClaimHistoryDto> ClaimHistoryDtos) : IRequest<List<ClaimHistoryDto>>
        {
            public List<ClaimHistoryDto> ClaimHistoryDtos { get; set; } = ClaimHistoryDtos;
        }

        #endregion UPDATE Claim History

        #region DELETE Claim History

        public class DeleteClaimHistoryRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Claim History
    }
}
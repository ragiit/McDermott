using McDermott.Application.Dtos.ClaimUserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.ClaimUserManagement
{
    public class ClaimRequestCommand
    {
        #region GET Claim Request 

        public class GetAllClaimRequestQuery(Expression<Func<ClaimRequest, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ClaimRequestDto>>
        {
            public Expression<Func<ClaimRequest, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleClaimRequestQuery : IRequest<ClaimRequestDto>
        {
            public List<Expression<Func<ClaimRequest, object>>> Includes { get; set; }
            public Expression<Func<ClaimRequest, bool>> Predicate { get; set; }
            public Expression<Func<ClaimRequest, ClaimRequest>> Select { get; set; }

            public List<(Expression<Func<ClaimRequest, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetClaimRequestQuery : IRequest<(List<ClaimRequestDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<ClaimRequest, object>>> Includes { get; set; }
            public Expression<Func<ClaimRequest, bool>> Predicate { get; set; }
            public Expression<Func<ClaimRequest, ClaimRequest>> Select { get; set; }

            public List<(Expression<Func<ClaimRequest, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateClaimRequestQuery(List<ClaimRequestDto> ClaimRequestToValidate) : IRequest<List<ClaimRequestDto>>
        {
            public List<ClaimRequestDto> ClaimRequestToValidate { get; } = ClaimRequestToValidate;
        }

        public class ValidateClaimRequestQuery(Expression<Func<ClaimRequest, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<ClaimRequest, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Claim Request Detail

        #region CREATE Claim Request

        public class CreateClaimRequestRequest(ClaimRequestDto ClaimRequestDto) : IRequest<ClaimRequestDto>
        {
            public ClaimRequestDto ClaimRequestDto { get; set; } = ClaimRequestDto;
        }

        public class CreateListClaimRequestRequest(List<ClaimRequestDto> ClaimRequestDtos) : IRequest<List<ClaimRequestDto>>
        {
            public List<ClaimRequestDto> ClaimRequestDtos { get; set; } = ClaimRequestDtos;
        }

        #endregion CREATE Claim Request

        #region UPDATE Claim Request

        public class UpdateClaimRequestRequest(ClaimRequestDto ClaimRequestDto) : IRequest<ClaimRequestDto>
        {
            public ClaimRequestDto ClaimRequestDto { get; set; } = ClaimRequestDto;
        }

        public class UpdateListClaimRequestRequest(List<ClaimRequestDto> ClaimRequestDtos) : IRequest<List<ClaimRequestDto>>
        {
            public List<ClaimRequestDto> ClaimRequestDtos { get; set; } = ClaimRequestDtos;
        }

        #endregion Update Claim Request

        #region DELETE Claim Request

        public class DeleteClaimRequestRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Claim Request

    }
}

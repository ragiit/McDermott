using McDermott.Application.Dtos.Telemedicines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Telemedicines
{
    public class OTPRequestCommand
    {
        #region GET

        public class GetSingleOTPRequestQuery : IRequest<OTPRequestDto>
        {
            public List<Expression<Func<OTPRequest, object>>> Includes { get; set; }
            public Expression<Func<OTPRequest, bool>> Predicate { get; set; }
            public Expression<Func<OTPRequest, OTPRequest>> Select { get; set; }

            public List<(Expression<Func<OTPRequest, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetOTPRequestQuerylable : BaseQuery<OTPRequest>
        { }

        public class GetOTPRequestQuery : IRequest<(List<OTPRequestDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<OTPRequest, object>>> Includes { get; set; }
            public Expression<Func<OTPRequest, bool>> Predicate { get; set; }
            public Expression<Func<OTPRequest, OTPRequest>> Select { get; set; }

            public List<(Expression<Func<OTPRequest, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateOTPRequest(Expression<Func<OTPRequest, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<OTPRequest, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateOTPRequestRequest(OTPRequestDto OTPRequestDto) : IRequest<OTPRequestDto>
        {
            public OTPRequestDto OTPRequestDto { get; set; } = OTPRequestDto;
        }

        public class BulkValidateOTPRequest(List<OTPRequestDto> OTPRequestsToValidate) : IRequest<List<OTPRequestDto>>
        {
            public List<OTPRequestDto> OTPRequestsToValidate { get; } = OTPRequestsToValidate;
        }

        public class CreateListOTPRequestRequest(List<OTPRequestDto> OTPRequestDtos) : IRequest<List<OTPRequestDto>>
        {
            public List<OTPRequestDto> OTPRequestDtos { get; set; } = OTPRequestDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateOTPRequestRequest(OTPRequestDto OTPRequestDto) : IRequest<OTPRequestDto>
        {
            public OTPRequestDto OTPRequestDto { get; set; } = OTPRequestDto;
        }

        public class UpdateListOTPRequestRequest(List<OTPRequestDto> OTPRequestDtos) : IRequest<List<OTPRequestDto>>
        {
            public List<OTPRequestDto> OTPRequestDtos { get; set; } = OTPRequestDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteOTPRequestRequest : IRequest<bool>
        {
            public long Id { get; set; }
            public List<long> Ids { get; set; }
        }

        #endregion DELETE
    }
}
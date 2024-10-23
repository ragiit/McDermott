using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Features.Commands.Queue
{
    public class KioskConfigCommand
    {
        #region GET

        public class GetKioskConfigQuery : IRequest<(List<KioskConfigDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<KioskConfig, object>>> Includes { get; set; }
            public Expression<Func<KioskConfig, bool>> Predicate { get; set; }
            public Expression<Func<KioskConfig, KioskConfig>> Select { get; set; }

            public List<(Expression<Func<KioskConfig, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleKioskConfigQuery : IRequest<KioskConfigDto>
        {
            public List<Expression<Func<KioskConfig, object>>> Includes { get; set; }
            public Expression<Func<KioskConfig, bool>> Predicate { get; set; }
            public Expression<Func<KioskConfig, KioskConfig>> Select { get; set; }

            public List<(Expression<Func<KioskConfig, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateKioskConfig(Expression<Func<KioskConfig, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<KioskConfig, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateKioskConfigRequest(KioskConfigDto KioskConfigDto) : IRequest<KioskConfigDto>
        {
            public KioskConfigDto KioskConfigDto { get; set; } = KioskConfigDto;
        }

        public class BulkValidateKioskConfig(List<KioskConfigDto> KioskConfigsToValidate) : IRequest<List<KioskConfigDto>>
        {
            public List<KioskConfigDto> KioskConfigsToValidate { get; } = KioskConfigsToValidate;
        }

        public class CreateListKioskConfigRequest(List<KioskConfigDto> KioskConfigDtos) : IRequest<List<KioskConfigDto>>
        {
            public List<KioskConfigDto> KioskConfigDtos { get; set; } = KioskConfigDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateKioskConfigRequest(KioskConfigDto KioskConfigDto) : IRequest<KioskConfigDto>
        {
            public KioskConfigDto KioskConfigDto { get; set; } = KioskConfigDto;
        }

        public class UpdateListKioskConfigRequest(List<KioskConfigDto> KioskConfigDtos) : IRequest<List<KioskConfigDto>>
        {
            public List<KioskConfigDto> KioskConfigDtos { get; set; } = KioskConfigDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteKioskConfigRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
namespace McDermott.Application.Features.Commands.Bpjs
{
    public class BpjsClassificationCommand
    {
        #region GET

        public class GetSingleBpjsClassificationQuery : IRequest<BpjsClassificationDto>
        {
            public List<Expression<Func<BpjsClassification, object>>> Includes { get; set; }
            public Expression<Func<BpjsClassification, bool>> Predicate { get; set; }
            public Expression<Func<BpjsClassification, BpjsClassification>> Select { get; set; }

            public List<(Expression<Func<BpjsClassification, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetBpjsClassificationQuery : IRequest<(List<BpjsClassificationDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<BpjsClassification, object>>> Includes { get; set; }
            public Expression<Func<BpjsClassification, bool>> Predicate { get; set; }
            public Expression<Func<BpjsClassification, BpjsClassification>> Select { get; set; }

            public List<(Expression<Func<BpjsClassification, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateBpjsClassificationQuery(List<BpjsClassificationDto> BpjsClassificationsToValidate) : IRequest<List<BpjsClassificationDto>>
        {
            public List<BpjsClassificationDto> BpjsClassificationsToValidate { get; } = BpjsClassificationsToValidate;
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

        public class DeleteBpjsClassificationRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
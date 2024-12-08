namespace McDermott.Application.Features.Commands.Transaction
{
    public class GCReferToInternalCommand
    {
        #region GET

        #region GET GC Refer To Internal

        public class GetSingleGCReferToInternalQuery : IRequest<GCReferToInternalDto>
        {
            public List<Expression<Func<GCReferToInternal, object>>> Includes { get; set; }
            public Expression<Func<GCReferToInternal, bool>> Predicate { get; set; }
            public Expression<Func<GCReferToInternal, GCReferToInternal>> Select { get; set; }

            public List<(Expression<Func<GCReferToInternal, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetGCReferToInternalQuery : IRequest<(List<GCReferToInternalDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GCReferToInternal, object>>> Includes { get; set; }
            public Expression<Func<GCReferToInternal, bool>> Predicate { get; set; }
            public Expression<Func<GCReferToInternal, GCReferToInternal>> Select { get; set; }

            public List<(Expression<Func<GCReferToInternal, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateGCReferToInternalQuery(List<GCReferToInternalDto> GCReferToInternalToValidate) : IRequest<List<GCReferToInternalDto>>
        {
            public List<GCReferToInternalDto> GCReferToInternalToValidate { get; } = GCReferToInternalToValidate;
        }

        public class ValidateGCReferToInternalQuery(Expression<Func<GCReferToInternal, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GCReferToInternal, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET GC Refer To Internal

        #endregion GET

        #region CREATE

        #region CREATE GC Refer To Internal

        public class CreateGCReferToInternalRequest(GCReferToInternalDto GCReferToInternalDtos) : IRequest<GCReferToInternalDto>
        {
            public GCReferToInternalDto GCReferToInternalDto { get; set; } = GCReferToInternalDtos;
        }

        public class CreateListGCReferToInternalRequest(List<GCReferToInternalDto> GCReferToInternalDtos) : IRequest<List<GCReferToInternalDto>>
        {
            public List<GCReferToInternalDto> GCReferToInternalDtos { get; set; } = GCReferToInternalDtos;
        }

        #endregion CREATE GC Refer To Internal

        #endregion CREATE

        #region UPDATE

        #region UPDATE GC Refer To Internal

        public class UpdateGCReferToInternalRequest(GCReferToInternalDto GCReferToInternalDto) : IRequest<GCReferToInternalDto>
        {
            public GCReferToInternalDto GCReferToInternalDto { get; set; } = GCReferToInternalDto;
        }

        public class UpdateListGCReferToInternalRequest(List<GCReferToInternalDto> GCReferToInternalDtos) : IRequest<List<GCReferToInternalDto>>
        {
            public List<GCReferToInternalDto> GCReferToInternalDtos { get; set; } = GCReferToInternalDtos;
        }

        #endregion UPDATE GC Refer To Internal

        #endregion UPDATE

        #region DELETE

        #region DELETE GC Refer To Internal

        public class DeleteGCReferToInternalRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE GC Refer To Internal

        #region DELETE GC Refer To Internal Log

        public class DeleteGCReferToInternalLogRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE GC Refer To Internal Log

        #endregion DELETE
    }
}
using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Features.Commands.Queue
{
    public class QueueDisplayCommand
    {
        #region GET

        public class GetQueueDisplayQuery : IRequest<(List<QueueDisplayDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<QueueDisplay, object>>> Includes { get; set; }
            public Expression<Func<QueueDisplay, bool>> Predicate { get; set; }
            public Expression<Func<QueueDisplay, QueueDisplay>> Select { get; set; }

            public List<(Expression<Func<QueueDisplay, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleQueueDisplayQuery : IRequest<QueueDisplayDto>
        {
            public List<Expression<Func<QueueDisplay, object>>> Includes { get; set; }
            public Expression<Func<QueueDisplay, bool>> Predicate { get; set; }
            public Expression<Func<QueueDisplay, QueueDisplay>> Select { get; set; }

            public List<(Expression<Func<QueueDisplay, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateQueueDisplayQuery(Expression<Func<QueueDisplay, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<QueueDisplay, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateQueueDisplayRequest(QueueDisplayDto QueueDisplayDto) : IRequest<QueueDisplayDto>
        {
            public QueueDisplayDto QueueDisplayDto { get; set; } = QueueDisplayDto;
        }

        public class BulkValidateQueueDisplayQuery(List<QueueDisplayDto> QueueDisplaysToValidate) : IRequest<List<QueueDisplayDto>>
        {
            public List<QueueDisplayDto> QueueDisplaysToValidate { get; } = QueueDisplaysToValidate;
        }

        public class CreateListQueueDisplayRequest(List<QueueDisplayDto> QueueDisplayDtos) : IRequest<List<QueueDisplayDto>>
        {
            public List<QueueDisplayDto> QueueDisplayDtos { get; set; } = QueueDisplayDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateQueueDisplayRequest(QueueDisplayDto QueueDisplayDto) : IRequest<QueueDisplayDto>
        {
            public QueueDisplayDto QueueDisplayDto { get; set; } = QueueDisplayDto;
        }

        public class UpdateListQueueDisplayRequest(List<QueueDisplayDto> QueueDisplayDtos) : IRequest<List<QueueDisplayDto>>
        {
            public List<QueueDisplayDto> QueueDisplayDtos { get; set; } = QueueDisplayDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteQueueDisplayRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
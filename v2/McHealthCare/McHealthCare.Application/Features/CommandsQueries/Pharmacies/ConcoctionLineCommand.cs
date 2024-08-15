namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class ConcoctionLineCommand
    {
        #region ConcoctionLine

        #region GET

        public class GetConcoctionLineQuery(Expression<Func<ConcoctionLine, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ConcoctionLineDto>>
        {
            public Expression<Func<ConcoctionLine, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateConcoctionLineRequest(ConcoctionLineDto ConcoctionLineDto) : IRequest<ConcoctionLineDto>
        {
            public ConcoctionLineDto ConcoctionLineDto { get; set; } = ConcoctionLineDto;
        }

        public class CreateListConcoctionLineRequest(List<ConcoctionLineDto> ConcoctionLineDtos) : IRequest<List<ConcoctionLineDto>>
        {
            public List<ConcoctionLineDto> ConcoctionLineDtos { get; set; } = ConcoctionLineDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateConcoctionLineRequest(ConcoctionLineDto ConcoctionLineDto) : IRequest<ConcoctionLineDto>
        {
            public ConcoctionLineDto ConcoctionLineDto { get; set; } = ConcoctionLineDto;
        }

        public class UpdateListConcoctionLineRequest(List<ConcoctionLineDto> ConcoctionLineDtos) : IRequest<List<ConcoctionLineDto>>
        {
            public List<ConcoctionLineDto> ConcoctionLineDtos { get; set; } = ConcoctionLineDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteConcoctionLineRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE

        #endregion ConcoctionLine

        #region Cut Stock ConcoctionLine

        #region GET

        public class GetStockOutLineQuery(Expression<Func<StockOutLines, bool>>? predicate = null, bool removeCache = false) : IRequest<List<StockOutLinesDto>>
        {
            public Expression<Func<StockOutLines, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET



        #region CREATE

        public class CreateStockOutLinesRequest(StockOutLinesDto StockOutLinesDto) : IRequest<StockOutLinesDto>
        {
            public StockOutLinesDto StockOutLinesDto { get; set; } = StockOutLinesDto;
        }

        public class CreateListStockOutLinesRequest(List<StockOutLinesDto> StockOutLinesDtos) : IRequest<List<StockOutLinesDto>>
        {
            public List<StockOutLinesDto> StockOutLinesDtos { get; set; } = StockOutLinesDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateStockOutLinesRequest(StockOutLinesDto StockOutLinesDto) : IRequest<StockOutLinesDto>
        {
            public StockOutLinesDto StockOutLinesDto { get; set; } = StockOutLinesDto;
        }

        public class UpdateListStockOutLinesRequest(List<StockOutLinesDto> StockOutLinesDto) : IRequest<List<StockOutLinesDto>>
        {
            public List<StockOutLinesDto> StockOutLinesDto { get; set; } = StockOutLinesDto;
        }

        #endregion Update

        #region DELETE

        public class DeleteStockOutLinesRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE

        #endregion Cut Stock ConcoctionLine
    }
}
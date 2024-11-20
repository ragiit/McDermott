using McDermott.Application.Dtos.Pharmacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class ConcoctionLineCommand
    {
        #region ConcoctionLine
        #region GET ConcoctionLine

        public class GetAllConcoctionLineQuery(Expression<Func<ConcoctionLine, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ConcoctionLineDto>>
        {
            public Expression<Func<ConcoctionLine, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleConcoctionLineQuery : IRequest<ConcoctionLineDto>
        {
            public List<Expression<Func<ConcoctionLine, object>>> Includes { get; set; }
            public Expression<Func<ConcoctionLine, bool>> Predicate { get; set; }
            public Expression<Func<ConcoctionLine, ConcoctionLine>> Select { get; set; }

            public List<(Expression<Func<ConcoctionLine, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetConcoctionLineQuery : IRequest<(List<ConcoctionLineDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<ConcoctionLine, object>>> Includes { get; set; }
            public Expression<Func<ConcoctionLine, bool>> Predicate { get; set; }
            public Expression<Func<ConcoctionLine, ConcoctionLine>> Select { get; set; }

            public List<(Expression<Func<ConcoctionLine, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateConcoctionLineQuery(List<ConcoctionLineDto> ConcoctionLineToValidate) : IRequest<List<ConcoctionLineDto>>
        {
            public List<ConcoctionLineDto> ConcoctionLineToValidate { get; } = ConcoctionLineToValidate;
        }

        public class ValidateConcoctionLineQuery(Expression<Func<ConcoctionLine, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<ConcoctionLine, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Education Program Detail

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

        public class DeleteConcoctionLineRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
        #endregion

        #region Cut Stock ConcoctionLine
        #region GET StockOutLines

        public class GetAllStockOutLinesQuery(Expression<Func<StockOutLines, bool>>? predicate = null, bool removeCache = false) : IRequest<List<StockOutLinesDto>>
        {
            public Expression<Func<StockOutLines, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleStockOutLinesQuery : IRequest<StockOutLinesDto>
        {
            public List<Expression<Func<StockOutLines, object>>> Includes { get; set; }
            public Expression<Func<StockOutLines, bool>> Predicate { get; set; }
            public Expression<Func<StockOutLines, StockOutLines>> Select { get; set; }

            public List<(Expression<Func<StockOutLines, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetStockOutLinesQuery : IRequest<(List<StockOutLinesDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<StockOutLines, object>>> Includes { get; set; }
            public Expression<Func<StockOutLines, bool>> Predicate { get; set; }
            public Expression<Func<StockOutLines, StockOutLines>> Select { get; set; }

            public List<(Expression<Func<StockOutLines, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateStockOutLinesQuery(List<StockOutLinesDto> StockOutLinesToValidate) : IRequest<List<StockOutLinesDto>>
        {
            public List<StockOutLinesDto> StockOutLinesToValidate { get; } = StockOutLinesToValidate;
        }

        public class ValidateStockOutLinesQuery(Expression<Func<StockOutLines, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<StockOutLines, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Education Program Detail

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

        public class DeleteStockOutLinesRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class GoodsReceiptCommand
    {
        #region GET
        #region GET Goods Receipt Detail

        public class GetAllGoodsReceiptDetailQuery(Expression<Func<GoodsReceiptDetail, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GoodsReceiptDetailDto>>
        {
            public Expression<Func<GoodsReceiptDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleGoodsReceiptDetailQuery : IRequest<GoodsReceiptDetailDto>
        {
            public List<Expression<Func<GoodsReceiptDetail, object>>> Includes { get; set; }
            public Expression<Func<GoodsReceiptDetail, bool>> Predicate { get; set; }
            public Expression<Func<GoodsReceiptDetail, GoodsReceiptDetail>> Select { get; set; }

            public List<(Expression<Func<GoodsReceiptDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetGoodsReceiptDetailQuery : IRequest<(List<GoodsReceiptDetailDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GoodsReceiptDetail, object>>> Includes { get; set; }
            public Expression<Func<GoodsReceiptDetail, bool>> Predicate { get; set; }
            public Expression<Func<GoodsReceiptDetail, GoodsReceiptDetail>> Select { get; set; }

            public List<(Expression<Func<GoodsReceiptDetail, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateGoodsReceiptDetailQuery(List<GoodsReceiptDetailDto> GoodsReceiptDetailToValidate) : IRequest<List<GoodsReceiptDetailDto>>
        {
            public List<GoodsReceiptDetailDto> GoodsReceiptDetailToValidate { get; } = GoodsReceiptDetailToValidate;
        }

        public class ValidateGoodsReceiptDetailQuery(Expression<Func<GoodsReceiptDetail, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GoodsReceiptDetail, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Goods Receipt Detail

        #region GET Goods Receipt

        public class GetSingleGoodsReceiptQuery : IRequest<GoodsReceiptDto>
        {
            public List<Expression<Func<GoodsReceipt, object>>> Includes { get; set; }
            public Expression<Func<GoodsReceipt, bool>> Predicate { get; set; }
            public Expression<Func<GoodsReceipt, GoodsReceipt>> Select { get; set; }

            public List<(Expression<Func<GoodsReceipt, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetGoodsReceiptQuery : IRequest<(List<GoodsReceiptDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GoodsReceipt, object>>> Includes { get; set; }
            public Expression<Func<GoodsReceipt, bool>> Predicate { get; set; }
            public Expression<Func<GoodsReceipt, GoodsReceipt>> Select { get; set; }

            public List<(Expression<Func<GoodsReceipt, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = []; 

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }
    
        public class BulkValidateGoodsReceiptQuery(List<GoodsReceiptDto> GoodsReceiptToValidate) : IRequest<List<GoodsReceiptDto>>
        {
            public List<GoodsReceiptDto> GoodsReceiptToValidate { get; } = GoodsReceiptToValidate;
        }

        public class ValidateGoodsReceiptQuery(Expression<Func<GoodsReceipt, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GoodsReceipt, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Goods Receipt

        #region GET Goods Receipt Log

        public class GetAllGoodsReceiptLogQuery(Expression<Func<GoodsReceiptLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GoodsReceiptLogDto>>
        {
            public Expression<Func<GoodsReceiptLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleGoodsReceiptLogQuery : IRequest<GoodsReceiptLogDto>
        {
            public List<Expression<Func<GoodsReceiptLog, object>>> Includes { get; set; }
            public Expression<Func<GoodsReceiptLog, bool>> Predicate { get; set; }
            public Expression<Func<GoodsReceiptLog, GoodsReceiptLog>> Select { get; set; }

            public List<(Expression<Func<GoodsReceiptLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetGoodsReceiptLogQuery : IRequest<(List<GoodsReceiptLogDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GoodsReceiptLog, object>>> Includes { get; set; }
            public Expression<Func<GoodsReceiptLog, bool>> Predicate { get; set; }
            public Expression<Func<GoodsReceiptLog, GoodsReceiptLog>> Select { get; set; }

            public List<(Expression<Func<GoodsReceiptLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateGoodsReceiptLogQuery(List<GoodsReceiptLogDto> GoodsReceiptLogToValidate) : IRequest<List<GoodsReceiptLogDto>>
        {
            public List<GoodsReceiptLogDto> GoodsReceiptLogToValidate { get; } = GoodsReceiptLogToValidate;
        }

        public class ValidateGoodsReceiptLogQuery(Expression<Func<GoodsReceiptLog, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GoodsReceiptLog, bool>> Predicate { get; } = predicate!;
        }
        #endregion

        #endregion

        #region CREATE
        #region CREATE Goods Receipt Detail

        public class CreateGoodsReceiptDetailRequest(GoodsReceiptDetailDto GoodsReceiptDetailDto) : IRequest<GoodsReceiptDetailDto>
        {
            public GoodsReceiptDetailDto GoodsReceiptDetailDto { get; set; } = GoodsReceiptDetailDto;
        }

        public class CreateListGoodsReceiptDetailRequest(List<GoodsReceiptDetailDto> GoodsReceiptDetailDtos) : IRequest<List<GoodsReceiptDetailDto>>
        {
            public List<GoodsReceiptDetailDto> GoodsReceiptDetailDtos { get; set; } = GoodsReceiptDetailDtos;
        }

        #endregion CREATE Goods Receipt Detail

        #region CREATE Goods Receipt

        public class CreateGoodsReceiptRequest(GoodsReceiptDto GoodsReceiptDtos) : IRequest<GoodsReceiptDto>
        {
            public GoodsReceiptDto GoodsReceiptDto { get; set; } = GoodsReceiptDtos;
        }

        public class CreateListGoodsReceiptRequest(List<GoodsReceiptDto> GoodsReceiptDtos) : IRequest<List<GoodsReceiptDto>>
        {
            public List<GoodsReceiptDto> GoodsReceiptDtos { get; set; } = GoodsReceiptDtos;
        }

        #endregion CREATE Receiving Stock

        #region CREATE GoodsReceipt Log
        public class CreateGoodsReceiptLogRequest(GoodsReceiptLogDto GoodsReceiptLogDtos) : IRequest<GoodsReceiptLogDto>
        {
            public GoodsReceiptLogDto GoodsReceiptLogDto { get; set; } = GoodsReceiptLogDtos;
        }

        public class CreateListGoodsReceiptLogRequest(List<GoodsReceiptLogDto> GoodsReceiptLogDtos) : IRequest<List<GoodsReceiptLogDto>>
        {
            public List<GoodsReceiptLogDto> GoodsReceiptLogDtos { get; set; } = GoodsReceiptLogDtos;
        }
        #endregion
        #endregion

        #region UPDATE
        #region UPDATE Goods Receipt Detail

        public class UpdateGoodsReceiptDetailRequest(GoodsReceiptDetailDto GoodsReceiptDetailDto) : IRequest<GoodsReceiptDetailDto>
        {
            public GoodsReceiptDetailDto GoodsReceiptDetailDto { get; set; } = GoodsReceiptDetailDto;
        }

        public class UpdateListGoodsReceiptDetailRequest(List<GoodsReceiptDetailDto> GoodsReceiptDetailDtos) : IRequest<List<GoodsReceiptDetailDto>>
        {
            public List<GoodsReceiptDetailDto> GoodsReceiptDetailDtos { get; set; } = GoodsReceiptDetailDtos;
        }

        #endregion Update Goods Receipt Detail

        #region UPDATE Goods Receipt

        public class UpdateGoodsReceiptRequest(GoodsReceiptDto GoodsReceiptDto) : IRequest<GoodsReceiptDto>
        {
            public GoodsReceiptDto GoodsReceiptDto { get; set; } = GoodsReceiptDto;
        }

        public class UpdateListGoodsReceiptRequest(List<GoodsReceiptDto> GoodsReceiptDtos) : IRequest<List<GoodsReceiptDto>>
        {
            public List<GoodsReceiptDto> GoodsReceiptDtos { get; set; } = GoodsReceiptDtos;
        }

        #endregion Update Goods Receipt 

        #region UPDATE Goods Receipt Log
        public class UpdateGoodsReceiptLogRequest(GoodsReceiptLogDto GoodsReceiptLogDto) : IRequest<GoodsReceiptLogDto>
        {
            public GoodsReceiptLogDto GoodsReceiptLogDto { get; set; } = GoodsReceiptLogDto;
        }

        public class UpdateListGoodsReceiptLogRequest(List<GoodsReceiptLogDto> GoodsReceiptLogDtos) : IRequest<List<GoodsReceiptLogDto>>
        {
            public List<GoodsReceiptLogDto> GoodsReceiptLogDtos { get; set; } = GoodsReceiptLogDtos;
        }
        #endregion
        #endregion

        #region DELETE
        #region DELETE Goods Receipt Detail

        public class DeleteGoodsReceiptDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Goods Receipt Detail

        #region DELETE Goods Receipt

        public class DeleteGoodsReceiptRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Goods Receipt

        #region DELETE Goods Receipt Log
        public class DeleteGoodsReceiptLogRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion
        #endregion
    }
}

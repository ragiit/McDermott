using McDermott.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class TransferStockCommand
    {
        #region GET
        #region GET Transfer Stock Detail

        public class GetAllTransferStockProductQuery(Expression<Func<TransferStockProduct, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransferStockProductDto>>
        {
            public Expression<Func<TransferStockProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleTransferStockProductQuery : IRequest<TransferStockProductDto>
        {
            public List<Expression<Func<TransferStockProduct, object>>> Includes { get; set; }
            public Expression<Func<TransferStockProduct, bool>> Predicate { get; set; }
            public Expression<Func<TransferStockProduct, TransferStockProduct>> Select { get; set; }

            public List<(Expression<Func<TransferStockProduct, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetTransferStockProductQuery : IRequest<(List<TransferStockProductDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<TransferStockProduct, object>>> Includes { get; set; }
            public Expression<Func<TransferStockProduct, bool>> Predicate { get; set; }
            public Expression<Func<TransferStockProduct, TransferStockProduct>> Select { get; set; }

            public List<(Expression<Func<TransferStockProduct, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateTransferStockProductQuery(List<TransferStockProductDto> TransferStockProductToValidate) : IRequest<List<TransferStockProductDto>>
        {
            public List<TransferStockProductDto> TransferStockProductToValidate { get; } = TransferStockProductToValidate;
        }

        public class ValidateTransferStockProductQuery(Expression<Func<TransferStockProduct, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<TransferStockProduct, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Transfer Stock Detail

        #region GET Transfer Stock

        public class GetSingleTransferStockQuery : IRequest<TransferStockDto>
        {
            public List<Expression<Func<TransferStock, object>>> Includes { get; set; }
            public Expression<Func<TransferStock, bool>> Predicate { get; set; }
            public Expression<Func<TransferStock, TransferStock>> Select { get; set; }

            public List<(Expression<Func<TransferStock, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetTransferStockQuery : IRequest<(List<TransferStockDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<TransferStock, object>>> Includes { get; set; }
            public Expression<Func<TransferStock, bool>> Predicate { get; set; }
            public Expression<Func<TransferStock, TransferStock>> Select { get; set; }

            public List<(Expression<Func<TransferStock, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }


        public class BulkValidateTransferStockQuery(List<TransferStockDto> TransferStockToValidate) : IRequest<List<TransferStockDto>>
        {
            public List<TransferStockDto> TransferStockToValidate { get; } = TransferStockToValidate;
        }

        public class ValidateTransferStockQuery(Expression<Func<TransferStock, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<TransferStock, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Transfer Stock

        #region GET Transfer Stock Log

        public class GetAllTransferStockLogQuery(Expression<Func<TransferStockLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransferStockLogDto>>
        {
            public Expression<Func<TransferStockLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleTransferStockLogQuery : IRequest<TransferStockLogDto>
        {
            public List<Expression<Func<TransferStockLog, object>>> Includes { get; set; }
            public Expression<Func<TransferStockLog, bool>> Predicate { get; set; }
            public Expression<Func<TransferStockLog, TransferStockLog>> Select { get; set; }

            public List<(Expression<Func<TransferStockLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetTransferStockLogQuery : IRequest<(List<TransferStockLogDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<TransferStockLog, object>>> Includes { get; set; }
            public Expression<Func<TransferStockLog, bool>> Predicate { get; set; }
            public Expression<Func<TransferStockLog, TransferStockLog>> Select { get; set; }

            public List<(Expression<Func<TransferStockLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateTransferStockLogQuery(List<TransferStockLogDto> TransferStockLogToValidate) : IRequest<List<TransferStockLogDto>>
        {
            public List<TransferStockLogDto> TransferStockLogToValidate { get; } = TransferStockLogToValidate;
        }

        public class ValidateTransferStockLogQuery(Expression<Func<TransferStockLog, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<TransferStockLog, bool>> Predicate { get; } = predicate!;
        }
        #endregion
        #endregion
        #region CREATE

        public class CreateTransferStockRequest(TransferStockDto TransferStockDto) : IRequest<TransferStockDto>
        {
            public TransferStockDto TransferStockDto { get; set; } = TransferStockDto;
        }

        public class CreateListTransferStockRequest(List<TransferStockDto> TransferStockDtos) : IRequest<List<TransferStockDto>>
        {
            public List<TransferStockDto> TransferStockDtos { get; set; } = TransferStockDtos;
        }

        #endregion CREATE

        #region CREATE Product

        public class CreateTransferStockProductRequest(TransferStockProductDto TransferStockProductDto) : IRequest<TransferStockProductDto>
        {
            public TransferStockProductDto TransferStockProductDto { get; set; } = TransferStockProductDto;
        }

        public class CreateListTransferStockProductRequest(List<TransferStockProductDto> TransferStockProductDtos) : IRequest<List<TransferStockProductDto>>
        {
            public List<TransferStockProductDto> TransferStockProductDtos { get; set; } = TransferStockProductDtos;
        }

        #endregion CREATE Product

        #region CREATE Detail

        public class CreateTransferStockLogRequest(TransferStockLogDto TransferStockLogDto) : IRequest<TransferStockLogDto>
        {
            public TransferStockLogDto TransferStockLogDto { get; set; } = TransferStockLogDto;
        }

        public class CreateListTransferStockLogRequest(List<TransferStockLogDto> TransferStockLogDtos) : IRequest<List<TransferStockLogDto>>
        {
            public List<TransferStockLogDto> TransferStockLogDtos { get; set; } = TransferStockLogDtos;
        }

        #endregion CREATE Detail

        #region Update

        public class UpdateTransferStockRequest(TransferStockDto TransferStockDto) : IRequest<TransferStockDto>
        {
            public TransferStockDto TransferStockDto { get; set; } = TransferStockDto;
        }

        public class UpdateListTransferStockRequest(List<TransferStockDto> TransferStockDtos) : IRequest<List<TransferStockDto>>
        {
            public List<TransferStockDto> TransferStockDtos { get; set; } = TransferStockDtos;
        }

        #endregion Update        

        #region Update Product

        public class UpdateTransferStockProductRequest(TransferStockProductDto TransferStockProductDto) : IRequest<TransferStockProductDto>
        {
            public TransferStockProductDto TransferStockProductDto { get; set; } = TransferStockProductDto;
        }

        public class UpdateListTransferStockProductRequest(List<TransferStockProductDto> TransferStockProductDtos) : IRequest<List<TransferStockProductDto>>
        {
            public List<TransferStockProductDto> TransferStockProductDtos { get; set; } = TransferStockProductDtos;
        }

        #endregion Update Product

        #region Update Detail

        public class UpdateTransferStockLogRequest(TransferStockLogDto TransferStockLogDto) : IRequest<TransferStockLogDto>
        {
            public TransferStockLogDto TransferStockLogDto { get; set; } = TransferStockLogDto;
        }

        public class UpdateListTransferStockLogRequest(List<TransferStockLogDto> TransferStockLogDtos) : IRequest<List<TransferStockLogDto>>
        {
            public List<TransferStockLogDto> TransferStockLogDtos { get; set; } = TransferStockLogDtos;
        }

        #endregion Update Detail

        #region DELETE

        public class DeleteTransferStockRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE        

        #region DELETE Product

        public class DeleteTransferStockProductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Product

        #region DELETE Detail

        public class DeleteTransferStockLogRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Detail
    }
}
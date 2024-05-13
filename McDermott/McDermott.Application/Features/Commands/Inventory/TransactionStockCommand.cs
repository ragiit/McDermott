using McDermott.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class TransactionStockCommand
    {
        #region GET

        public class GetTransactionStockQuery(Expression<Func<TransactionStock, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransactionStockDto>>
        {
            public Expression<Func<TransactionStock, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region GET Receiving Stock Detail

        public class GetReceivingStockDetailQuery(Expression<Func<ReceivingStockDetail, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ReceivingStockDetailDto>>
        {
            public Expression<Func<ReceivingStockDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Receiving Stock Detail

        #region GET Product

        public class GetTransactionStockProductQuery(Expression<Func<TransactionStockProduct, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransactionStockProductDto>>
        {
            public Expression<Func<TransactionStockProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Product

        #region GET Detail

        public class GetTransactionStockDetailQuery(Expression<Func<TransactionStockDetail, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransactionStockDetailDto>>
        {
            public Expression<Func<TransactionStockDetail, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Detail

        #region CREATE

        public class CreateTransactionStockRequest(TransactionStockDto TransactionStockDto) : IRequest<TransactionStockDto>
        {
            public TransactionStockDto TransactionStockDto { get; set; } = TransactionStockDto;
        }

        public class CreateListTransactionStockRequest(List<TransactionStockDto> TransactionStockDtos) : IRequest<List<TransactionStockDto>>
        {
            public List<TransactionStockDto> TransactionStockDtos { get; set; } = TransactionStockDtos;
        }

        #endregion CREATE

        #region CREATE Receiving Stock Detail

        public class CreateReceivingStockDetailRequest(ReceivingStockDetailDto ReceivingStockDetailDto) : IRequest<ReceivingStockDetailDto>
        {
            public ReceivingStockDetailDto ReceivingStockDetailDto { get; set; } = ReceivingStockDetailDto;
        }

        public class CreateListReceivingStockDetailRequest(List<ReceivingStockDetailDto> ReceivingStockDetailDtos) : IRequest<List<ReceivingStockDetailDto>>
        {
            public List<ReceivingStockDetailDto> ReceivingStockDetailDtos { get; set; } = ReceivingStockDetailDtos;
        }

        #endregion CREATE Receiving Stock Detail

        #region CREATE Product

        public class CreateTransactionStockProductRequest(TransactionStockProductDto TransactionStockProductDto) : IRequest<TransactionStockProductDto>
        {
            public TransactionStockProductDto TransactionStockProductDto { get; set; } = TransactionStockProductDto;
        }

        public class CreateListTransactionStockProductRequest(List<TransactionStockProductDto> TransactionStockProductDtos) : IRequest<List<TransactionStockProductDto>>
        {
            public List<TransactionStockProductDto> TransactionStockProductDtos { get; set; } = TransactionStockProductDtos;
        }

        #endregion CREATE Product

        #region CREATE Detail

        public class CreateTransactionStockDetailRequest(TransactionStockDetailDto TransactionStockDetailDto) : IRequest<TransactionStockDetailDto>
        {
            public TransactionStockDetailDto TransactionStockDetailDto { get; set; } = TransactionStockDetailDto;
        }

        public class CreateListTransactionStockDetailRequest(List<TransactionStockDetailDto> TransactionStockDetailDtos) : IRequest<List<TransactionStockDetailDto>>
        {
            public List<TransactionStockDetailDto> TransactionStockDetailDtos { get; set; } = TransactionStockDetailDtos;
        }

        #endregion CREATE Detail

        #region Update

        public class UpdateTransactionStockRequest(TransactionStockDto TransactionStockDto) : IRequest<TransactionStockDto>
        {
            public TransactionStockDto TransactionStockDto { get; set; } = TransactionStockDto;
        }

        public class UpdateListTransactionStockRequest(List<TransactionStockDto> TransactionStockDtos) : IRequest<List<TransactionStockDto>>
        {
            public List<TransactionStockDto> TransactionStockDtos { get; set; } = TransactionStockDtos;
        }

        #endregion Update

        #region Update Receiving Stock Detail

        public class UpdateReceivingStockDetailRequest(ReceivingStockDetailDto ReceivingStockDetailDto) : IRequest<ReceivingStockDetailDto>
        {
            public ReceivingStockDetailDto ReceivingStockDetailDto { get; set; } = ReceivingStockDetailDto;
        }

        public class UpdateListReceivingStockDetailRequest(List<ReceivingStockDetailDto> ReceivingStockDetailDtos) : IRequest<List<ReceivingStockDetailDto>>
        {
            public List<ReceivingStockDetailDto> ReceivingStockDetailDtos { get; set; } = ReceivingStockDetailDtos;
        }

        #endregion Update Receiving Stock Detail

        #region Update Product

        public class UpdateTransactionStockProductRequest(TransactionStockProductDto TransactionStockProductDto) : IRequest<TransactionStockProductDto>
        {
            public TransactionStockProductDto TransactionStockProductDto { get; set; } = TransactionStockProductDto;
        }

        public class UpdateListTransactionStockProductRequest(List<TransactionStockProductDto> TransactionStockProductDtos) : IRequest<List<TransactionStockProductDto>>
        {
            public List<TransactionStockProductDto> TransactionStockProductDtos { get; set; } = TransactionStockProductDtos;
        }

        #endregion Update Product

        #region Update Detail

        public class UpdateTransactionStockDetailRequest(TransactionStockDetailDto TransactionStockDetailDto) : IRequest<TransactionStockDetailDto>
        {
            public TransactionStockDetailDto TransactionStockDetailDto { get; set; } = TransactionStockDetailDto;
        }

        public class UpdateListTransactionStockDetailRequest(List<TransactionStockDetailDto> TransactionStockDetailDtos) : IRequest<List<TransactionStockDetailDto>>
        {
            public List<TransactionStockDetailDto> TransactionStockDetailDtos { get; set; } = TransactionStockDetailDtos;
        }

        #endregion Update Detail

        #region DELETE

        public class DeleteTransactionStockRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE

        #region DELETE Receiving Stock Detail

        public class DeleteReceivingStockDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Receiving Stock Detail

        #region DELETE Product

        public class DeleteTransactionStockProductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Product

        #region DELETE Detail

        public class DeleteTransactionStockDetailRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Detail
    }
}